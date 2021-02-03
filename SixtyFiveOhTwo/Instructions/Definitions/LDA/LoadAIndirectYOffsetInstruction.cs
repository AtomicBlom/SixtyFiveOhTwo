using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using SixtyFiveOhTwo.Util;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAIndirectYOffsetInstruction : IndirectYOffsetInstructionBase
    {
        public LoadAIndirectYOffsetInstruction() : base(0xB1, "LDA", 5) { }

        private new class Microcode : IndirectYOffsetInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(byte zeroPageOffset)
            {
                var lsb = ReadByteFromBus(ZeroPageAddress(zeroPageOffset));
                var msb = ReadByteFromBus(ZeroPageAddress(zeroPageOffset, 1));

                var baseAddress = MakeUShort(msb, lsb);
                var address = baseAddress.Offset(CPUState.IndexRegisterY);

                var firstFetchAddress = MakeUShort(baseAddress.HighOrderByte(), address.LowOrderByte());
                var value = ReadByteFromBus(firstFetchAddress);

                if (baseAddress.HighOrderByte() != address.HighOrderByte())
                {
                    value = ReadByteFromBus(address);
                }

                CPUState.Accumulator = value;
                CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.Accumulator);
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}