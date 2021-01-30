namespace SixtyFiveOhTwo.Util
{
    public static class UShortExtensions
    {
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
