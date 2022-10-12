using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;

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
            var jd = context.JobDetail.JobDataMap.Values;

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
        private readonly ISchedulerFactory _factory;

        private readonly ILogger<ShowTimeSchedulerController> _logger;


        public ShowTimeSchedulerController(ILogger<ShowTimeSchedulerController> logger, ISchedulerFactory factory)
        {
            _factory = factory;
            _logger = logger;
        }

        [HttpPost]
        [ActionName("RunShowtimeJobForPos")]
        public async Task<OkObjectResult> RunShowtimeJobForPos(int posId)
        {
            var jobKeyName = "ShowtimeLoaderPosJob";
            var jobKey = new JobKey(jobKeyName);
            IScheduler scheduler = await _factory.GetScheduler();
            scheduler.Context.Clear();
            scheduler.Context.Add("posId", posId);
            await scheduler.TriggerJob(jobKey);

            return Ok("OK");
        }

        [HttpPost]
        [ActionName("RunShowtimeJobForChain")]
        public async Task<OkObjectResult> RunShowtimeJobForChain(int chainId)
        {
            var jobKeyName = "ShowtimeLoaderChainJob";
            var jobKey = new JobKey(jobKeyName);
            
            IScheduler scheduler = await _factory.GetScheduler();
            scheduler.Context.Clear();
            scheduler.Context.Add("chainId", chainId);
            await scheduler.TriggerJob(jobKey);

            return Ok("OK");

        }

        [HttpPost]
        [ActionName("RunShowtimeJobForTheater")]
        public async Task<OkObjectResult> RunShowtimeJobForTheater(int theaterId)
        {

            var jobKeyName = "ShowtimeLoaderTheaterJob";
            var jobKey = new JobKey(jobKeyName);

            IScheduler scheduler = await _factory.GetScheduler();
            scheduler.Context.Clear();
            scheduler.Context.Add("theaterId", theaterId);
            await scheduler.TriggerJob(jobKey);

            return Ok("OK");
        }

        [HttpPost]
        [ActionName("ScheduleShowtimeJobForAllTheaters")]
        public async Task<OkObjectResult> ScheduleShowtimeJobForAllTheaters()
        {
            IScheduler scheduler = await _factory.GetScheduler();
            await scheduler.TriggerJob(new JobKey("ShowtimeLoadersScheduledJob"));

            return Ok("OK");

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