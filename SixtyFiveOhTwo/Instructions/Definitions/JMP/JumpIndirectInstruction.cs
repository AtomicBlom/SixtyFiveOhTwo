using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.JMP
{
    public sealed class JumpIndirectInstruction : IInstruction
    {
        public byte OpCode => 0x6C;
        public string Mnemonic => "JMP";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;
            var address = cpu.ReadProgramCounterWord();
            cpuState.ProgramCounter = cpu.Bus.ReadWord(address);
        }

        public IInstructionEncoder Write(byte value)
        {
            return new IndirectAddressInstructionEncoder(this, value);
        }
    }
}