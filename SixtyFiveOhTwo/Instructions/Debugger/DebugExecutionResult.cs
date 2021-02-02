using System.Runtime.InteropServices;
using SixtyFiveOhTwo.Components;

namespace SixtyFiveOhTwo.Instructions.Debugger
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DebugExecutionResult
    {
        private readonly ushort _address;
        private readonly ushort _pointerToAddress;
        private readonly byte _zeroPageOffset;
        private readonly byte _x;
        private readonly byte _y;
        private readonly byte _value;
        public byte OpCode { get; init; }
        public AddressingMode AddressingMode { get; init; }
        public CPUState CPUState { get; init; }
        public UsedFields UsedFields { get; private init; }

        public ushort Address
        {
            get => _address;
            init
            {
                _address = value;
                UsedFields |= UsedFields.Address;
            }
        }

        public ushort PointerToAddress
        {
            get => _pointerToAddress;
            init
            {
                _pointerToAddress = value;
                UsedFields |= UsedFields.PointerToAddress;
            }
        }

        public byte ZeroPageOffset
        {
            get => _zeroPageOffset;
            init
            {
                _zeroPageOffset = value;
                UsedFields |= UsedFields.ZeroPageOffset;
            }
        }

        public byte X
        {
            get => _x;
            init
            {
                _x = value;
                UsedFields |= UsedFields.X;
            }
        }

        public byte Y
        {
            get => _y;
            init
            {
                _y = value;
                UsedFields |= UsedFields.Y;
            }
        }

        public byte Value
        {
            get => _value;
            init
            {
                _value = value;
                UsedFields |= UsedFields.Value;
            }
        }

        public override string ToString()
        {
            var address = UsedFields.HasFlag(UsedFields.Address) ? $"{nameof(Address)}: {Address:X4}, " : string.Empty;
            var pointerToAddress = UsedFields.HasFlag(UsedFields.PointerToAddress) ? $"{nameof(PointerToAddress)}: {PointerToAddress:X4}, " : string.Empty;
            var zeroPageOffset = UsedFields.HasFlag(UsedFields.ZeroPageOffset) ? $"{nameof(ZeroPageOffset)}: {ZeroPageOffset:X2}, " : string.Empty;
            var x = UsedFields.HasFlag(UsedFields.X) ? $"{nameof(X)}: {X:X2}, " : string.Empty;
            var y = UsedFields.HasFlag(UsedFields.Y) ? $"{nameof(Y)}: {Y:X2}, " : string.Empty;
            var value = UsedFields.HasFlag(UsedFields.Value) ? $"{nameof(Value)}: {Value:X2}, " : string.Empty;



            return $"{nameof(OpCode)}: {OpCode:X2}, Mode: {AddressingMode}, {nameof(CPUState)}: {{{CPUState}}}, {address}{pointerToAddress}{zeroPageOffset}{x}{y}{value}";
        }
    }
}