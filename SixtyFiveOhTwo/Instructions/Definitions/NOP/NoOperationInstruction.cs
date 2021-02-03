using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.NOP
{
    public class NoOperationInstruction : ImpliedInstructionBase
    {
        public NoOperationInstruction() : base(0xEA, "NOP", 2) { }

        private new class Microcode : ImpliedInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode()
            {
                Yield();
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
