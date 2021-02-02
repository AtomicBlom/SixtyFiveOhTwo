using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
    public abstract class AbsoluteInstructionBase : InstructionBase, IParameterInstruction<ushort>
    {
        protected AbsoluteInstructionBase(byte opCode, string mnemonic) : base(opCode, mnemonic) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var address = ReadNextOpCodeWord();
                RunMicrocode(address);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.Absolute,
                    CPUState = CPUState,
                    Address = address
                };
            }

            protected abstract void RunMicrocode(ushort address);
        }

        public IInstructionEncoder GetEncoder(ushort address)
        {
            return new AbsoluteAddressInstructionEncoder(this, address);
        }
    }
}