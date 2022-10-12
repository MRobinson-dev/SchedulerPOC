using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using WebApplication2.Jobs;
using static Quartz.MisfireInstruction;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImmediateController : ControllerBase
    {
        private readonly ISchedulerFactory factory;
        private readonly ILogger<ImmediateController> _logger;

        public ImmediateController(ISchedulerFactory factory, ILogger<ImmediateController> logger)
        {
            this.factory = factory;
            _logger = logger;
        }

        [HttpGet("Run")]
        public async Task<OkObjectResult> Run()
        {
            IScheduler scheduler = await factory.GetScheduler();


            await scheduler.TriggerJob(new JobKey("ImmediateJob"));

            return Ok("OK");
        }
        [HttpPost("Run2")]
        public async Task<OkObjectResult> Run2()
        {
            IScheduler scheduler = await factory.GetScheduler();


            await scheduler.TriggerJob(new JobKey("DemoJob"));

            return Ok("OK");
        }
    }
}