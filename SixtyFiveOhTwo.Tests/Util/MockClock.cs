namespace SixtyFiveOhTwo.Tests.Util
{
    public class MockClock : IClock
    {
        public long Ticks { get; set; } = -1; // Take ignore first tick because CPU will do a sync tick at the start.
        public void Wait()
        {
            Ticks++;
        }

        public void WaitFalling()
        {
        }
    }
}