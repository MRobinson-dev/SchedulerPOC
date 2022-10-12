using Quartz;
using SchedulerQuartzPOC.api.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQuartz(q =>
{
    q.SchedulerName = "Showtime Loaders Scheduler";

    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobkey = new JobKey("ShowtimeLoadersScheduledJob");

    q.AddJob<ShowtimeLoadersScheduledJob>(opts => opts
        .WithIdentity(jobkey)
        .UsingJobData("posId", "all")
        .UsingJobData("chainId", "all")
        .UsingJobData("theaterId", "all"));
    q.AddTrigger(opts => opts
        .WithIdentity("ShowtimeLoadersScheduledJob-trigger")
        .ForJob(jobkey)
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromSeconds(5))
            .RepeatForever()));

    var posJobKey = new JobKey("ShowtimeLoaderPosJob");
    q.AddJob<ShowtimeLoaderPosJob>(opts => opts
        .WithIdentity(posJobKey)
        .StoreDurably(true));

    var chainJobKey = new JobKey("ShowtimeLoaderChainJob");
    q.AddJob<ShowtimeLoaderChainJob>(opts => opts
        .WithIdentity(chainJobKey)
        .StoreDurably(true));

    var theaterJobKey = new JobKey("ShowtimeLoaderTheaterJob");
    q.AddJob<ShowtimeLoaderTheaterJob>(opts => opts
        .WithIdentity(theaterJobKey)
        .StoreDurably(true));

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
//builder.Services.AddQuartzHostedService();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();