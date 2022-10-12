using Quartz;

namespace SchedulerQuartzPOC.api.Jobs
{
    public class ShowtimeLoadersScheduledJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var jobName = context.JobDetail.Key.Name;
            var x = context.Trigger.GetNextFireTimeUtc();
            Console.WriteLine($"Running job {jobName} Next Run: {x}");

             await Task.CompletedTask;
        }
    }
}
