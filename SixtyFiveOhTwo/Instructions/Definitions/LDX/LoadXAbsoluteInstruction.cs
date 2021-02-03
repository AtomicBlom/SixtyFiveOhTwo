using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXAbsoluteInstruction : AbsoluteInstructionBase
    {
        public LoadXAbsoluteInstruction() : base(0xAE, "LDX", 4) { }
        
        private new class Microcode : AbsoluteInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(ushort address)
            {
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