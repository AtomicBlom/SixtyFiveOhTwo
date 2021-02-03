using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
    public abstract class ImpliedInstructionBase : InstructionBase, INoParameterInstruction
    {
        protected ImpliedInstructionBase(byte opCode, string mnemonic, byte tCnt) : base(opCode, mnemonic, tCnt, 1) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor) { }

            public sealed override DebugExecutionResult Execute()
            {
                RunMicrocode();
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.Implied,
                    CPUState = CPUState
                };
            }

            protected abstract void RunMicrocode();
        }

        public IInstructionEncoder GetEncoder()
        {
            return new ImpliedAddressInstructionEncoder(this);
        }
    }
}