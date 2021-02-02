using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using SixtyFiveOhTwo.Util;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYAbsoluteXOffsetInstruction : AbsoluteWithXOffsetInstructionBase
    {
	    public LoadYAbsoluteXOffsetInstruction() : base(0xBC, "LDY") { }

        private new class Microcode : AbsoluteWithXOffsetInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            // read across boundary may incur an extra read
            //https://retrocomputing.stackexchange.com/questions/145/why-does-6502-indexed-lda-take-an-extra-cycle-at-page-boundaries
            protected override void RunMicrocode(ushort absoluteAddress)
            {
                var address = absoluteAddress.Offset(CPUState.IndexRegisterX);

                var firstFetchAddress = MakeUShort(absoluteAddress.HighOrderByte(), address.LowOrderByte());
                var value = ReadByteFromBus(firstFetchAddress);

                if (absoluteAddress.HighOrderByte() != address.HighOrderByte())
                {
                    value = ReadByteFromBus(address);
                }

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