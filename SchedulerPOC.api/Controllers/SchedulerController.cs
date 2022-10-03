using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("ImmediateJob")]
        public String ImmediateJob()
        {
            //Fire - and - Forget Job - this job is executed only once
            var now = new TimeOnly();
            now = TimeOnly.FromDateTime(DateTime.Now.ToUniversalTime());
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("This job is executed immediately"));

            return $"Job ID: {jobId}. This Immediate Job was submitted at {now.ToLongTimeString()} Universal Time";
        }

        [HttpGet]
        [Route("timedjob")]
        public String TimedJob(int seconds = 30)
        {
            //Delayed Job - this job executed only once but not immedietly after some time.
            var now = new TimeOnly();
            now = TimeOnly.FromDateTime(DateTime.Now.ToUniversalTime());
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine($"You scheduled a job to run in {seconds} seconds!"), TimeSpan.FromSeconds(seconds));

            return $"Job ID: {jobId}. This Timed Job was submitted at {now.ToLongTimeString()} Universal Time to run in {seconds} seconds";
        }

        [HttpGet]
        [Route("parentchildjob")]
        public String ParentChildJob()
        {
            //Fire and Forget Job - this job is executed only once
            var parentjobId = BackgroundJob.Enqueue(() => Console.WriteLine("This is the Parent Job"));

            //Continuations Job - this job executed when its parent job is executed.
            var jobId = BackgroundJob.ContinueJobWith(parentjobId, () => Console.WriteLine("This is the Child Job"));

            return $"The Parent Job {parentjobId} ran then the Child Job {jobId} ran!";
        }

        [HttpGet]
        [Route("dailyjob")]
        public String DailyJob()
        {
            //Recurring Job - this job is executed many times on the specified cron schedule
            RecurringJob.AddOrUpdate(() => Console.WriteLine("This job will run daily"), Cron.Daily);

            return "daily job scheduled";
        }
    }
}
