using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXImmediateInstruction : ImmediateInstructionBase
    {
	    public LoadXImmediateInstruction() : base(0xA2, "LDX") { }

        private new class Microcode : ImmediateInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte value)
            {
                CPUState.IndexRegisterX = value;
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.IndexRegisterX);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
