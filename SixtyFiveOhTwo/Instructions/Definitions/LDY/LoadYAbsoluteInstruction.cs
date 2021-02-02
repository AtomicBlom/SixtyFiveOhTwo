using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYAbsoluteInstruction : AbsoluteInstructionBase
    {
        public LoadYAbsoluteInstruction() : base(0xAC, "LDY") { }

        private new class Microcode : AbsoluteInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(ushort address)
            {
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