using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYImmediateInstruction : ImmediateInstructionBase
    {
	    public LoadYImmediateInstruction(): base(0xA0, "LDY", 2) { }

        private new class Microcode : ImmediateInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }
            
            protected override void RunMicrocode(byte value)
            {
                CPUState.IndexRegisterY = value;
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.IndexRegisterY);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
