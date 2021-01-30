namespace SixtyFiveOhTwo.Components
{
    public static class BusExtensions
    {
        public static void WriteValue(this IBus bus, ushort address, byte value)
        {
            bus.AccessMode = BusAccessMode.Write;
            bus.Address = address;
            bus.Data = value;
            bus.Clock.Wait();
        }

        public static byte ReadByte(this IBus bus, ushort address)
        {
            bus.AccessMode = BusAccessMode.Read;
            bus.Address = address;
            bus.Clock.Wait();
            return bus.Data;
        }

        public static ushort ReadWord(this IBus bus, ushort address)
        {
            var lsb = bus.ReadByte(address);
            var msb = bus.ReadByte((ushort)((address + 1) & 0xFFFF));

            return (ushort)((msb << 8) | lsb);
        }
    }
}