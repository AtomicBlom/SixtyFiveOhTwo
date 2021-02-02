using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAZeroPageInstruction : ZeroPageInstructionBase
    {
        public LoadAZeroPageInstruction() : base(0xA5, "LDA") { }

        private new class Microcode : ZeroPageInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }
            
            protected override void RunMicrocode(byte zeroPageOffset)
            {
                CPUState.Accumulator = ReadByteFromBus(ZeroPageAddress(zeroPageOffset));
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.Accumulator);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
