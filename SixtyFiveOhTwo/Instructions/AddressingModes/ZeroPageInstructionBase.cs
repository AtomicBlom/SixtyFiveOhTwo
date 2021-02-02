using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
    public abstract class ZeroPageInstructionBase : InstructionBase, IParameterInstruction<byte>
    {
        protected ZeroPageInstructionBase(byte opCode, string mnemonic) : base(opCode, mnemonic) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var zeroPageOffset = ReadNextOpCodeByte();
                RunMicrocode(zeroPageOffset);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.ZeroPage,
                    CPUState = CPUState,
                    ZeroPageOffset = zeroPageOffset
                };
            }

            protected abstract void RunMicrocode(byte zeroPageOffset);
        }

        public IInstructionEncoder GetEncoder(byte address)
        {
            return new ZeroPageAddressInstructionEncoder(this, address);
        }
    }
}