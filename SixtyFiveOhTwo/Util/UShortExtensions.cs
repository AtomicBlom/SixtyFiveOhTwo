namespace SixtyFiveOhTwo.Util
{
    public static class UShortExtensions
    {
        public static ushort Offset(this ushort value, int offset)
        {
            return (ushort) ((value + offset) & 0xFFFF);
        }

        public static ushort Offset(this ushort value, int offset, int secondOffset)
        {
            return (ushort)((value + offset + secondOffset) & 0xFFFF);
        }

        public static byte HighOrderByte(this ushort value)
        {
            return (byte)(value >> 8);
        }

        public static byte LowOrderByte(this ushort value)
        {
            return (byte)(value & 0xFF);
        }
    }
}
