using System.Runtime.CompilerServices;
using Quartz;
using SchedulerQuartzPOC.api.Controllers;

namespace SchedulerQuartzPOC.api.Jobs
{
    [DisallowConcurrentExecution]
    
    public class ShowtimeLoaderChainJob : IJob
    {
        private Theater _theater = new Theater();
        
        public Task Execute(IJobExecutionContext context)
        {

            object obj;
            var hasChainId = context.Scheduler.Context.TryGetValue("chainId", out obj);
            if (!hasChainId)
            {
                throw new InvalidOperationException("Chain Id is null");

            }

            int chainId;
            try
            {
                chainId = (int)obj;

            }
            catch
            {
                throw new ArgumentException($"ChainId {obj.ToString()} is not valid");
            }
            var theaters = _theater.GetTheatersForChain(chainId);
            if (theaters != null && theaters.Count > 0)
            {
                Console.WriteLine($"___________________________________________________________________________________________");
                Console.WriteLine($"Job: {context.JobDetail.Key.Name} for theaters in Chain {chainId.ToString()} started. Processing {theaters.Count.ToString()} theaters.  Start: {DateTime.Now.ToLongDateString()}");
                Console.WriteLine("");

                foreach (var t in theaters)
                {
                    Console.WriteLine($"Theater: {t.TheaterId.ToString()}, Chain: {chainId.ToString()}, POS: {t.PosId.ToString()}");
                    Thread.Sleep(20000);
                }
                Console.WriteLine($"___________________________________________________________________________________________");
                Console.WriteLine("");
            }
            else
            {
                throw new InvalidOperationException($"No theaters found for ChainId");
            }

            return Task.CompletedTask;

            


        }
    }
}
