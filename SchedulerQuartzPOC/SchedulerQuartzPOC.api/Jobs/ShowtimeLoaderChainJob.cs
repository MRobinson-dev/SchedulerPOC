using Quartz;

namespace SchedulerQuartzPOC.api.Jobs
{
    public class ShowtimeLoaderChainJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            object chainId;
            var x = context.Scheduler.Context.TryGetValue("chainId", out chainId);
            Console.WriteLine($"Job for Chain {chainId.ToString()} ran");
            return Task.CompletedTask;
        }
    }
}
