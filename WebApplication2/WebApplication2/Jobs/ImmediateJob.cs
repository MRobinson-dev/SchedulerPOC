using Quartz;

namespace WebApplication2.Jobs
{
    public class ImmediateJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("This Job runs only once");
            return Task.CompletedTask;
        }
    }
}
