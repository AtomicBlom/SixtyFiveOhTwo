using System.Diagnostics;
using System.Runtime.CompilerServices;
using SixtyFiveOhTwo.Components;

namespace SixtyFiveOhTwo.Util
{
    public static class AddressUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static ushort MakeUShort(byte msb, byte lsb)
        {
            return (ushort)((msb << 8) | lsb);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static ushort Offset(this ushort value, int offset)
        {
            return (ushort) ((value + offset) & 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static ushort Offset(this ushort value, int offset, int secondOffset)
        {
            return (ushort)((value + offset + secondOffset) & 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static byte HighOrderByte(this ushort value)
        {
            return (byte)(value >> 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static byte LowOrderByte(this ushort value)
        {
            return (byte)(value & 0xFF);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public static ushort ZeroPageAddress(byte value, byte offset = 0, byte secondOffset = 0)
        {
            return (ushort)(CPU.ZeroPageStart | ((value + offset + secondOffset) & 0xFF));
        }
    }
}
