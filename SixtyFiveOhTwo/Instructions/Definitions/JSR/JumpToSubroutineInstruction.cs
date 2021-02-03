using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;
using SixtyFiveOhTwo.Util;

namespace SixtyFiveOhTwo.Instructions.Definitions.JSR
{
    public sealed class JumpToSubroutineInstruction : AbsoluteInstructionBase
    {
        public JumpToSubroutineInstruction() : base(0x20, "JSR", 6) { }

        private new class Microcode : AbsoluteInstructionBase.Microcode
        {
            public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            protected override void RunMicrocode(ushort address)
            {
                var t = CPUState.ProgramCounter.Offset(-3);

                PushStack(t.HighOrderByte());
                PushStack(t.LowOrderByte());

                CPUState.ProgramCounter = address;
                Yield();
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu);
        }
    }
}
