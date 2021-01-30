namespace SixtyFiveOhTwo.Components
{
    public class Bus : IBus
    {
        public BusAccessMode AccessMode { get; set; }
        public IClock Clock { get; }
        public ushort Address { get; set; }
        public byte Data { get; set; }

        public Bus(IClock clock)
        {
            Clock = clock;
        }
    }
}