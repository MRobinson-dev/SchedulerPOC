using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace SchedulerQuartzPOC.api.Controllers
{
    //class DownloadTheaterJob : IJob
    //{

    //}

    //class DownloadTheaterChainJob : IJob
    //{

    //}

    public class DownloadAllTheatersForPosSystemJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
             var posId =  context.JobDetail.JobDataMap["posId"];
            Console.WriteLine(posId);
            
        }
    }

    //class DownloadAllTheatersForAllPosSystemsJob : IJob
    //{

    //}
    


    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShowTimeSchedulerController : ControllerBase
    {
        private readonly IScheduler _scheduler;

        private readonly ILogger<ShowTimeSchedulerController> _logger;


        public ShowTimeSchedulerController(ILogger<ShowTimeSchedulerController> logger, IScheduler scheduler)
        {
            _logger = logger;
            _scheduler = scheduler;
        }

        [HttpPost]
        [ActionName("RunShowtimeJobForPos")]
        public async Task RunShowtimeJobForPos(int posId)
        {
            /**
             * 202
             */
            var job = JobBuilder.Create<DownloadAllTheatersForPosSystemJob>()
                .WithIdentity("pos1")
                .UsingJobData("posId", posId)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("...")
                .StartNow()
                .Build();
            await _scheduler.ScheduleJob(job, trigger);
        }

        [HttpPost]
        [ActionName("RunShowtimeJobForChain")]
        public void RunShowtimeJobForChain(int chainId)
        {
            //var theaters = _theater.GetTheatersForChain(chainId);

            //return theaters;

        }

        [HttpPost]
        [ActionName("RunShowtimeJobForTheater")]
        public void RunShowtimeJobForTheater(int theaterId)
        {
            //var theaters = _theater.GetTheater(theaterId);

            //return theaters;

        }

        [HttpPost]
        [ActionName("ScheduleShowtimeJobForAllTheaters")]
        public void ScheduleShowtimeJobForAllTheaters()
        {
            //var theaters = _theater.ScheduleShowtimeLoaderJob();

            //return theaters;

        }

        [HttpPost]
        [ActionName("CancelAllJobs")]
        public void CancelAllJobs()
        {
//_theater.CancelAllJobs();

        }

        [HttpPost]
        [ActionName("CancelRecurringJob")]
        public void CancelRecurringJob()
        {
          //  _theater.CancelRecurringJob();

        }

        [HttpDelete]
        [ActionName("StopRunningJob")]
        public void StopRunningJob(JobKey jobKey)
        {
            // Start job execution
            // POST /jobs/:name/executions
            // { args: "..." }

            // Stop running job
            // DELETE /jobs/:name/executions/:executionId

            // Start or stop a scheduled job
            // PUT /jobs/:name/schedule/status
            // { "status": "started|stopped" }

            // Stop all jobs
            // DELETE /jobs/executions

           // _theater.StopRunningJob(jobKey);

        }

    }
}