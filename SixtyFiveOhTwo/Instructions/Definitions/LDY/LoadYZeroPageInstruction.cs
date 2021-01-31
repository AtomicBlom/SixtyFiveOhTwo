using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using static SixtyFiveOhTwo.Util.UShortExtensions;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYZeroPageInstruction : IInstruction
    {
        public byte OpCode => 0xA4;
        public string Mnemonic => "LDY";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            cpuState.IndexRegisterY = cpu.Bus.ReadByte(ZeroPageAddress(zeroPageOffset));

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterY);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
