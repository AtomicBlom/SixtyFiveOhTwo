using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using SixtyFiveOhTwo.Util;

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
            var lsb = cpu.Bus.ReadByte((ushort) (CPU.ZeroPageStart | zeroPageOffset));
            var msb = cpu.Bus.ReadByte((ushort) ((CPU.ZeroPageStart | zeroPageOffset + 1) & 0xFF));

            var baseAddress = (ushort)((msb << 8) | lsb);
            var address = (ushort)((baseAddress + cpuState.IndexRegisterY) & 0xFFFF);

            var firstFetchAddress = (ushort)(baseAddress.HighOrderByte() << 8 | address.LowOrderByte());
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