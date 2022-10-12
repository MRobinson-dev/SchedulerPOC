using Quartz;

namespace SchedulerQuartzPOC.api.Jobs
{
    [DisallowConcurrentExecution]
    public class ShowtimeLoaderPosJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            object theaterId;
            var x = context.Scheduler.Context.TryGetValue("posId", out theaterId);
            Console.WriteLine($"Job for Theater {theaterId.ToString()} ran");
            return Task.CompletedTask;
        }
    }
}
