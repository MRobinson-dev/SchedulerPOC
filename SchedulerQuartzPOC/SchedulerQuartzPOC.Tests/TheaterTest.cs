namespace SchedulerQuartzPOC.Tests
{
    public class TheaterTests
    {
        private Theater _theater = new Theater();



        [Fact]
        public void GetTheatersReturns2970Theaters()
        {
            var allTheaters = _theater.GetTheaters();

            Assert.Equal(3000, allTheaters.Count);
        }

        [Fact]
        public void GetTheatersForChain()
        {
            var chainId = 1;
            var theatersInChain = _theater.GetTheatersForChain(chainId);

            Assert.Equal(150, theatersInChain.Count);

        }

        [Fact]
        public void GetTheatersForPOS()
        {
            var posId = 3;
            var theaters = _theater.GetTheatersForPos(posId);

            Assert.Equal(100, theaters.Count);

        }

        [Fact]
        public void GetTheater()
        {
            var theaterId = 3;
            var theaters = _theater.GetTheater(theaterId);

            Assert.Equal(1, theaters.Count());

        }
    }
}