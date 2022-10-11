using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace SchedulerQuartzPOC.api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShowTimeSchedulerController : ControllerBase
    {
        public readonly Theater _theater = new Theater();

        private readonly ILogger<ShowTimeSchedulerController> _logger;

        public ShowTimeSchedulerController(ILogger<ShowTimeSchedulerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ActionName("RunShowtimeJobForPos")]
        public IEnumerable<Theater> RunShowtimeJobForPos(int posId)
        {
            var theaters =  _theater.GetTheatersForPos(posId);

            return theaters;

        }

        [HttpGet]
        [ActionName("RunShowtimeJobForChain")]
        public IEnumerable<Theater> RunShowtimeJobForChain(int chainId)
        {
            var theaters = _theater.GetTheatersForChain(chainId);

            return theaters;

        }

        [HttpGet]
        [ActionName("RunShowtimeJobForTheater")]
        public IEnumerable<Theater> RunShowtimeJobForTheater(int theaterId)
        {
            var theaters = _theater.GetTheater(theaterId);

            return theaters;

        }

        [HttpGet]
        [ActionName("ScheduleShowtimeJobForAllTheaters")]
        public IEnumerable<Theater> ScheduleShowtimeJobForAllTheaters()
        {
            var theaters = _theater.ScheduleShowtimeLoaderJob();

            return theaters;

        }

        [HttpGet]
        [ActionName("CancelAllJobs")]
        public void CancelAllJobs()
        {
             _theater.CancelAllJobs();

        }

        [HttpGet]
        [ActionName("CancelRecurringJob")]
        public void CancelRecurringJob()
        {
            _theater.CancelRecurringJob();

        }

        [HttpGet]
        [ActionName("StopRunningJob")]
        public void StopRunningJob(JobKey jobKey)
        {
            _theater.StopRunningJob(jobKey);

        }

    }
}