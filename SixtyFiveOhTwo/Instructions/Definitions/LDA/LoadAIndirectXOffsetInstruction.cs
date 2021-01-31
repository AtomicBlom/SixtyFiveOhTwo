using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using static SixtyFiveOhTwo.Util.UShortExtensions;

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
            var lsb = cpu.Bus.ReadByte(ZeroPageAddress(zeroPageOffset, cpuState.IndexRegisterX));
            var msb = cpu.Bus.ReadByte(ZeroPageAddress(zeroPageOffset, cpuState.IndexRegisterX, 1));
            var address = MakeUShort(msb, lsb);

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