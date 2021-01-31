using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using SixtyFiveOhTwo.Util;
using static SixtyFiveOhTwo.Util.UShortExtensions;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAIndirectYOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xB1;
        public string Mnemonic => "LDA";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            var lsb = cpu.Bus.ReadByte(ZeroPageAddress(zeroPageOffset));
            var msb = cpu.Bus.ReadByte(ZeroPageAddress(zeroPageOffset, 1));

            var baseAddress = MakeUShort(msb, lsb);
            var address = baseAddress.Offset(cpuState.IndexRegisterY);

            var firstFetchAddress = MakeUShort(baseAddress.HighOrderByte(), address.LowOrderByte());
            var value = cpu.Bus.ReadByte(firstFetchAddress);

            if (baseAddress.HighOrderByte() != address.HighOrderByte())
            {
                value = cpu.Bus.ReadByte(address);
            }

            cpuState.Accumulator = value;
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.Accumulator);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new IndirectYAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}