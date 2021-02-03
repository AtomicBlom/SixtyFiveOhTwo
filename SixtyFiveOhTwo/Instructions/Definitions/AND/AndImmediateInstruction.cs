using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.AND
{
    public sealed class AndImmediateInstruction : ImmediateInstructionBase
    {
        public AndImmediateInstruction() : base(0x29, "AND", 2)
        {
        }

        private new class Microcode : ImmediateInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            protected override void RunMicrocode(byte value)
            {
                CPUState.Accumulator &= value;
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.Accumulator);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
