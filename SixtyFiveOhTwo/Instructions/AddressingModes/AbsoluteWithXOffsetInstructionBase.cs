using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
    public abstract class AbsoluteWithXOffsetInstructionBase : InstructionBase, IParameterInstruction<ushort>
    {
        protected AbsoluteWithXOffsetInstructionBase(byte opCode, string mnemonic) : base(opCode, mnemonic) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var initialXRegister = CPUState.IndexRegisterX;
                var address = ReadNextOpCodeWord();
                RunMicrocode(address);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.AbsoluteIndexed,
                    CPUState = CPUState,
                    Address = address,
                    X = initialXRegister
                };
            }

            protected abstract void RunMicrocode(ushort address);
        }

        public IInstructionEncoder GetEncoder(ushort address)
		{
			return new AbsoluteXOffsetAddressInstructionEncoder(this, address);
		}
	}
}