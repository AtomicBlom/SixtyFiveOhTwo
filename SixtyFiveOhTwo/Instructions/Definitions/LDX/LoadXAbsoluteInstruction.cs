using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXAbsoluteInstruction : IInstruction
    {
        public byte OpCode => 0xAE;
        public string Mnemonic => "LDX";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var absoluteAddress = cpu.ReadProgramCounterWord();
            cpuState.IndexRegisterX = cpu.Bus.ReadByte(absoluteAddress);
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterX);
        }

        public IInstructionEncoder Write(ushort absoluteAddress)
        {
            return new AbsoluteAddressInstructionEncoder(this, absoluteAddress);
        }
    }
}