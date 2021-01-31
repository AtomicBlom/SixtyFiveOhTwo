using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYAbsoluteInstruction : IInstruction
    {
        public byte OpCode => 0xAC;
        public string Mnemonic => "LDY";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var absoluteAddress = cpu.ReadProgramCounterWord();
            cpuState.IndexRegisterY = cpu.Bus.ReadByte(absoluteAddress);
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterY);
        }

        public IInstructionEncoder Write(ushort absoluteAddress)
        {
            return new AbsoluteAddressInstructionEncoder(this, absoluteAddress);
        }
    }
}