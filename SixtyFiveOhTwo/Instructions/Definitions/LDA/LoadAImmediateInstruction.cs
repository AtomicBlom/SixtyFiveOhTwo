using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAImmediateInstruction : ImmediateInstructionBase
    {
        public LoadAImmediateInstruction() : base(0xA9, "LDA") { }

        private new class Microcode : ImmediateInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte value)
            {
                CPUState.Accumulator = value;
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.Accumulator);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
