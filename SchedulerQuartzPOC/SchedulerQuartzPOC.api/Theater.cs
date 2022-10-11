using Quartz;

namespace SchedulerQuartzPOC.api
{
    public class Theater
    {
        public int TheaterId { get; set; }

        public int ChainId { get; set; }
        public int PosId { get; set; }



        // creating test theater data

        private IEnumerable<Theater> BuildTheaterList()
        {
            var theaters = new List<Theater>();

            for (int i = 1; i < 31; i++)
            {
                for (int j = 1; j < 101; j++)
                {
                    var t = new Theater()
                    {
                        TheaterId = j*i,
                        ChainId = SetChainId(j),
                        PosId = i
                    };

                    theaters.Add(t);
                }
                
            }

            return theaters;
        }

        public List<Theater> GetTheaters()
        {
            var theaters = this.BuildTheaterList().ToList();


            return theaters;
        }

        public List<Theater> GetTheatersForChain(int chainId)
        {
            var theaters = this.GetTheaters().Where(t => t.ChainId == chainId).ToList();
            return theaters;
        }

        public List<Theater> GetTheatersForPos(int posId)
        {
            var theaters = this.GetTheaters().Where(t => t.PosId == posId).ToList();
            return theaters;
        }


        private int SetChainId(int j)
        {
            if (j < 20)
                return j;
            return j % 20;
        }



        public IEnumerable<Theater> GetTheater(int theaterId)
        {
            var theaters = new List<Theater>();
            var theater = this.GetTheaters().FirstOrDefault(t => t.TheaterId == theaterId) ?? throw new ArgumentNullException($"Theater {theaterId} is not found");

                theaters.Add(theater);


            return theaters;
        }

        public IEnumerable<Theater> ScheduleShowtimeLoaderJob()
        {
            var theaters = this.GetTheaters();

            return theaters;

        }

        public void CancelAllJobs()
        {
            throw new NotImplementedException();
        }


        public void CancelRecurringJob()
        {
            throw new NotImplementedException();
        }

        public void StopRunningJob(JobKey jobKey)
        {
            throw new NotImplementedException();
        }
    }


}