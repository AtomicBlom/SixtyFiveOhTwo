using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXZeroPageInstruction : ZeroPageInstructionBase
    {
        public LoadXZeroPageInstruction() : base(0xA6, "LDX") { }

        private new class Microcode : ZeroPageInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte zeroPageOffset)
            {
                CPUState.IndexRegisterX = ReadByteFromBus(ZeroPageAddress(zeroPageOffset));
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.IndexRegisterX);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
