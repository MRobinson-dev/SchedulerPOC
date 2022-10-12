using Microsoft.Extensions.Logging;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzHostedService
{
    public record Theater(string PosName, string Id);

    public class TheaterClient
    {
        public Task<List<Theater>> LoadTheaters()
        {
            return Task.Run(() =>
            {
                var list = new List<Theater>();
                for (var i = 0; i < 350; i++)
                {
                    list.Add(new Theater("amc", i.ToString()));
                }
                for (var i = 0; i < 50; i++)
                {
                    list.Add(new Theater("clarity", i.ToString()));
                }
                for (var i = 0; i < 10; i++)
                {
                    list.Add(new Theater("crainAtlanta", i.ToString()));
                }
                for (var i = 0; i < 200; i++)
                {
                    list.Add(new Theater("fandangoApi", i.ToString()));
                }
                for (var i = 0; i < 175; i++)
                {
                    list.Add(new Theater("hybris", i.ToString()));
                }
                return list;
            });
        }
    }

    public class TheaterDownloadClient
    {
        private readonly ILogger<TheaterDownloadClient> _logger;

        public TheaterDownloadClient(ILogger<TheaterDownloadClient> logger)
        {
            _logger = logger;
        }

        public Task DownloadSchedule(Theater theater)
        {
            return Task.Run(async () =>
            {
                _logger.LogInformation($"Downloading POS schedule for theater {theater.PosName}/{theater.Id}.");
                await Task.Delay(5000);
                _logger.LogInformation($"POS schedule downloaded for theater {theater.PosName}/{theater.Id}.");
            });
        }
    }

    public class ConcurrencyLimits
    {
        public static IDictionary<string, SemaphoreSlim> Semaphores = new Dictionary<string, SemaphoreSlim>
        {
            ["amc"] = new SemaphoreSlim(20),
            ["clarity"] = new SemaphoreSlim(10),
            ["crainAtlanta"] = new SemaphoreSlim(1024),
            ["fandangoApi"] = new SemaphoreSlim(50),
            ["hybris"] = new SemaphoreSlim(25)
        };
    }

    [DisallowConcurrentExecution]
    public class PosDownloaderJob : IJob
    {
        private readonly ILogger<PosDownloaderJob> _logger;
        private readonly TheaterClient _theaterClient;
        private readonly TheaterDownloadClient _theaterDownloadClient;

        public PosDownloaderJob(ILogger<PosDownloaderJob> logger, TheaterClient theaterClient, TheaterDownloadClient theaterDownloadClient)
        {
            _logger = logger;
            _theaterClient = theaterClient;
            _theaterDownloadClient = theaterDownloadClient;
        }

        // Kick off one-time job for a theater from API
        // Kick off one-time job for all theaters in a chain from the API
        // Kick off one-time job for all theaters on a POS system from the API
        // Stop each of the one-time jobs from the API
        // Stop the recurring job from the API
        // When stopping a job try to stop the job midway.
        // Investigate if SemaphoreSlim is actually a good idea.
        public async Task Execute(IJobExecutionContext context)
        {
            var theaterGroups =
                from theater in await _theaterClient.LoadTheaters()
                group theater by theater.PosName
                into theaterGroup
                select theaterGroup;

            _logger.LogInformation("Starting POS downloader job.");

            var theaterGroupTasks = theaterGroups.Select(theaterGroup =>
            {
                return Task.Run(async () =>
                {
                    var theaters = theaterGroup.ToList();
                    var theaterDownloadTasks = new List<Task>();
                    foreach (var theater in theaters)
                    {
                        await ConcurrencyLimits.Semaphores[theater.PosName].WaitAsync();
                        var task = Task.Run(async () =>
                        {
                            try
                            {
                                await _theaterDownloadClient.DownloadSchedule(theater);
                            }
                            finally
                            {
                                ConcurrencyLimits.Semaphores[theater.PosName].Release();
                            }
                        });
                        theaterDownloadTasks.Add(task);
                    }
                    await Task.WhenAll(theaterDownloadTasks);
                });
            });

            await Task.WhenAll(theaterGroupTasks);

            _logger.LogInformation("Finished downloading POS schedules.");
        }
    }

}
