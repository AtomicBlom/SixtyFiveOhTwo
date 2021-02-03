using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.JMP
{
    public sealed class JumpAbsoluteInstruction : AbsoluteInstructionBase
    {
        public JumpAbsoluteInstruction() : base(0x4C, "JMP", 3) { }

        private new class Microcode : AbsoluteInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(ushort address)
            {
                CPUState.ProgramCounter = address;
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
