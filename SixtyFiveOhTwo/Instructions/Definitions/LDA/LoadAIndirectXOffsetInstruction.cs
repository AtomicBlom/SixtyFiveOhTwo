using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAIndirectXOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xA1;
        public string Mnemonic => "LDA";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            var lsb = cpu.Bus.ReadByte((ushort) ((zeroPageOffset + cpuState.IndexRegisterX) & 0xFF));
            var msb = cpu.Bus.ReadByte((ushort) ((zeroPageOffset + cpuState.IndexRegisterX + 1) & 0xFF)) << 8;
            var address = (ushort)(msb | lsb);
            cpu.Bus.Clock.Wait(); //FIXME: Not sure where the extra wait comes from?
            cpuState.Accumulator = cpu.Bus.ReadByte(address);

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.Accumulator);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new IndirectXAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}