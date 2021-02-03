using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.JMP
{
    public sealed class JumpIndirectInstruction : IndirectInstructionBase
    {
        public JumpIndirectInstruction() : base(0x6C, "JMP", 5) { }

        private new class Microcode : IndirectInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(ushort addressOfAddress)
            {
                CPUState.ProgramCounter = ReadWordFromBus(addressOfAddress);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}