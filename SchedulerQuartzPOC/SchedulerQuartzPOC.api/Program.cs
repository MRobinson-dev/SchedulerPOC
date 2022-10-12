using Quartz;
using SchedulerQuartzPOC.api.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQuartz(q =>
{
    q.SchedulerName = "Example Quartz Scheduler";
// Use a Scoped container for creating IJobs
q.AddJob<DownloadAllTheatersForPosSystemJob>(opts => opts.WithIdentity("jobKey"));
q.AddTrigger(opts => opts
    .WithIdentity("...")
    .ForJob("jobKey")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithInterval(TimeSpan.FromSeconds(30))
        .RepeatForever())
);
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
