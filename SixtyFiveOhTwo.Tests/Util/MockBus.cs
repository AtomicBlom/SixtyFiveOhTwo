using SixtyFiveOhTwo.Components;

namespace SixtyFiveOhTwo.Tests.Util
{
    public class MockBus : IBus
    {
        private readonly byte[] _ram;

        public MockBus(IClock clock, byte[] ram)
        {
            _ram = ram;
            Clock = clock;
        }

        public BusAccessMode AccessMode { get; set; }
        public IClock Clock { get; }
        public ushort Address { get; set; }

        public byte Data
        {
            get => _ram[Address];
            set => _ram[Address] = value;
        }
    }
}