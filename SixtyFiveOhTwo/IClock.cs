namespace SixtyFiveOhTwo
{
    public interface IClock
    {
        long Ticks { get; }
        void Wait();
        void WaitFalling();
    }
}