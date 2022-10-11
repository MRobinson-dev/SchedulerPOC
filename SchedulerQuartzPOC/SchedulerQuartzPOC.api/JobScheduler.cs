using Quartz;

namespace SchedulerQuartzPOC.api
{
    public class JobScheduler :IJob
    {
        public Task Execute(IJobExecutionContext context)
        {

            return Task.FromResult(true);
        }
    }
}
