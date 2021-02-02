using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
    public abstract class IndirectInstructionBase : InstructionBase, IParameterInstruction<ushort>
    {
        protected IndirectInstructionBase(byte opCode, string mnemonic) : base(opCode, mnemonic) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var addressOfAddress = ReadNextOpCodeWord();
                RunMicrocode(addressOfAddress);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.Indirect,
                    CPUState = CPUState,
                    PointerToAddress = addressOfAddress
                };
            }

            protected abstract void RunMicrocode(ushort addressOfAddress);
        }

        public IInstructionEncoder GetEncoder(ushort addressOfAddress)
        {
            return new IndirectAddressInstructionEncoder(this, addressOfAddress);
        }
    }
}