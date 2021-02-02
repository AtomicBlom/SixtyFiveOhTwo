using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAAbsoluteInstruction : AbsoluteInstructionBase
    {
        public LoadAAbsoluteInstruction() : base(0xAD, "LDA") { }

        private new class Microcode : AbsoluteInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(ushort address)
            {
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