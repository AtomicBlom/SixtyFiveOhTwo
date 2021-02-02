using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAIndirectXOffsetInstruction : IndirectXOffsetInstructionBase
    {
	    public LoadAIndirectXOffsetInstruction() : base(0xA1, "LDA") { }

        private new class Microcode : IndirectXOffsetInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(ushort address)
            {
                Yield();
                CPUState.Accumulator = ReadByteFromBus(address);
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.Accumulator);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}