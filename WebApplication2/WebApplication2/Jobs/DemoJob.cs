using Quartz;
namespace WebApplication2.Jobs
{
    public class DemoJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            string message = "This job is: " +
                             context.JobDetail.Key.ToString();
            Console.WriteLine(message);
            
            return Task.CompletedTask;
        }
    }
}
