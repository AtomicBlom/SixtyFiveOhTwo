using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAZeroPageXOffsetInstruction : ZeroPageXOffsetInstructionBase
    {
	    public LoadAZeroPageXOffsetInstruction() : base(0xB5, "LDA") { }

        private new class Microcode : ZeroPageXOffsetInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte zeroPageOffset)
            {
                Yield();
                var address = ZeroPageAddress(zeroPageOffset, CPUState.IndexRegisterX);
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
