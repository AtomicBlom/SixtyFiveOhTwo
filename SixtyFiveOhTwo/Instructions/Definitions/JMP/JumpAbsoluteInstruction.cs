using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.JMP
{
    public sealed class JumpAbsoluteInstruction : IInstruction
    {
        public byte OpCode => 0x4C;
        public string Mnemonic => "JMP";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;
            cpuState.ProgramCounter = cpu.ReadProgramCounterWord();
        }

        public IInstructionEncoder Write(ushort value)
        {
            return new AbsoluteAddressInstructionEncoder(this, value);
        }
    }
}
