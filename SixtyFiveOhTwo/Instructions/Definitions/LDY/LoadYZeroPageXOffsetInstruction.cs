using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYZeroPageXOffsetInstruction : ZeroPageXOffsetInstructionBase
    {
	    public LoadYZeroPageXOffsetInstruction() : base(0xB4, "LDY") { }

        private new class Microcode : ZeroPageXOffsetInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte zeroPageOffset)
            {
                Yield();
                var address = ZeroPageAddress(zeroPageOffset, CPUState.IndexRegisterX);

                CPUState.IndexRegisterY = ReadByteFromBus(address);
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.IndexRegisterY);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
