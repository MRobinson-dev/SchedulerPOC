using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using QuartzHostedService.Data;
using System;

namespace QuartzHostedService;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var connectionString = hostContext.Configuration.GetConnectionString("SqlLiteConnection");
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(connectionString)
                );

                services.AddTransient<TheaterClient>();
                services.AddTransient<TheaterDownloadClient>();

                services.AddQuartz(q =>
                {
                    // Normally would take this from appsettings.json, just show it's possible
                    q.SchedulerName = "Example Quartz Scheduler";

                    // Use a Scoped container for creating IJobs
                    q.UseMicrosoftDependencyInjectionScopedJobFactory();

                    // add the job
                    var jobKey = new JobKey("Update exchange rates");
                    q.AddJob<PosDownloaderJob>(opts => opts.WithIdentity(jobKey));
                    q.AddTrigger(opts => opts
                        .ForJob(jobKey)
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithInterval(TimeSpan.FromSeconds(30))
                            .RepeatForever())
                    );
                });
                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            });
}