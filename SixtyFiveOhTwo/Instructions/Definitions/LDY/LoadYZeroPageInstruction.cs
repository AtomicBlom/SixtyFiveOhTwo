using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYZeroPageInstruction : ZeroPageInstructionBase
    {
        public LoadYZeroPageInstruction() : base(0xA4, "LDY") { }

        private new class Microcode : ZeroPageInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte zeroPageOffset)
            {
                CPUState.IndexRegisterY = ReadByteFromBus(ZeroPageAddress(zeroPageOffset));
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.IndexRegisterY);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
