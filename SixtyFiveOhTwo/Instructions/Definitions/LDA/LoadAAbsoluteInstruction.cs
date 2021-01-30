using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAAbsoluteInstruction : IInstruction
    {
        public byte OpCode => 0xAD;
        public string Mnemonic => "LDA";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var absoluteAddress = cpu.ReadProgramCounterWord();
            cpuState.Accumulator = cpu.Bus.ReadByte(absoluteAddress);
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.Accumulator);
        }

        public IInstructionEncoder Write(ushort absoluteAddress)
        {
            return new AbsoluteAddressInstructionEncoder(this, absoluteAddress);
        }
    }
}