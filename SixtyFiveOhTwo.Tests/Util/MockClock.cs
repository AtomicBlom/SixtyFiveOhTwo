namespace SixtyFiveOhTwo.Tests.Util
{
    public class MockClock : IClock
    {
        public long Ticks { get; set; }
        public void Wait()
        {
            Ticks++;
        }

        public void WaitFalling()
        {
        }
    }
}