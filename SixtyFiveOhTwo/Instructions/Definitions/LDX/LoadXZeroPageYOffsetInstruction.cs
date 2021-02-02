using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXZeroPageYOffsetInstruction : ZeroPageYOffsetInstructionBase
    {
	    public LoadXZeroPageYOffsetInstruction() : base(0xB6, "LDX") { }

        private new class Microcode : ZeroPageYOffsetInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte zeroPageOffset)
            {
                Yield();
                var address = ZeroPageAddress(zeroPageOffset, CPUState.IndexRegisterY);
                CPUState.IndexRegisterX = ReadByteFromBus(address);

                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.IndexRegisterX);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
