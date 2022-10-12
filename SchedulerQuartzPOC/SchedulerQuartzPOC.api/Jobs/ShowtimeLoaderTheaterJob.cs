using Quartz;

namespace SchedulerQuartzPOC.api.Jobs
{
    public class ShowtimeLoaderTheaterJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            object theaterId;
            var x = context.Scheduler.Context.TryGetValue("theaterId", out theaterId);
            Console.WriteLine($"Job for Theater {theaterId.ToString()} ran");
            return Task.CompletedTask;
        }
    }
}
