using Hangfire;

namespace HangfireDemo.Controllers;

public static class ShowtimeLoaderJobs
{

    public static void QueueShowtimeLoaderJobs()
    {

        var theaters = GetTheaterList();
        var theaterCount = theaters.Count();

        for (var n = 1; n < theaterCount; n++)
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine($"load showtimes for theater {n.ToString()}"));
        }

    }

    private static List<int> GetTheaterList()
    {
        List<int> theaters = new List<int>();

        for (int i = 0; i < 3000; i++)
        {
            theaters.Add(i);
        }

        return theaters;
    }
}