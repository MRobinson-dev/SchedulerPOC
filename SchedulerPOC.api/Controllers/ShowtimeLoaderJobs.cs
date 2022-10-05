using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;

namespace HangfireDemo.Controllers;

public static class ShowtimeLoaderJobs
{

    public static void QueueShowtimeLoaderJobs()
    {

        var theaters = GetTheaterList();
        var theaterCount = theaters.Count();

        for (var n = 1; n < theaterCount; n++)
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine($"load showtimes for theater {n.ToString()}"));
        }

    }

    private static List<int> GetTheaterList()
    {
        List<int> theaters = new List<int>();

        for (int i = 0; i < 3000; i++)
        {
            theaters.Add(i);
        }

        return theaters;
    }


    public static void PurgeJobs()
    {
        var monitor = JobStorage.Current.GetMonitoringApi();
        foreach (QueueWithTopEnqueuedJobsDto queue in monitor.Queues())
        {
            PurgeQueue(queue.Name);
        }
    }
    public static void PurgeQueue(string queueName)
    {
        var toDelete = new List<string>();
        var monitor = GetQueue(queueName, out var queue);

        for (var i = 0; i < Math.Ceiling(queue.Length / 1000d); i++)
        {
            monitor.EnqueuedJobs(queue.Name, 1000 * i, 1000)
                .ForEach(x => toDelete.Add(x.Key));
        }
        foreach (var jobId in toDelete)
        {
            BackgroundJob.Delete(jobId);
        }
    }

    private static IMonitoringApi GetQueue(string queueName, out QueueWithTopEnqueuedJobsDto queue)
    {
        var monitor = JobStorage.Current.GetMonitoringApi();
        queue = monitor.Queues().First(x => x.Name == queueName);
        return monitor;
    }
}