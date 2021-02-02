using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
    public abstract class ImmediateInstructionBase : InstructionBase, IParameterInstruction<byte>
    {
        protected ImmediateInstructionBase(byte opCode, string mnemonic) : base(opCode, mnemonic) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var value = ReadNextOpCodeByte();
                RunMicrocode(value);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.Immediate,
                    CPUState = CPUState,
                    Value = value
                };
            }

            protected abstract void RunMicrocode(byte value);
        }
        
        public IInstructionEncoder GetEncoder(byte value)
        {
            return new ImmediateAddressInstructionEncoder(this, value);
        }
    }
}