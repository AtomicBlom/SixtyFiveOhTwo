using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
    public abstract class AbsoluteWithYOffsetInstructionBase : InstructionBase, IParameterInstruction<ushort>
    {
        protected AbsoluteWithYOffsetInstructionBase(byte opCode, string mnemonic, byte tCnt) : base(opCode, mnemonic, tCnt, 3) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var initialYRegister = CPUState.IndexRegisterY;
                var address = ReadNextOpCodeWord();
                RunMicrocode(address);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    CPUState = CPUState,
                    Address = address,
                    Y = initialYRegister
                };
            }

            protected abstract void RunMicrocode(ushort address);
        }

        public IInstructionEncoder GetEncoder(ushort address)
		{
			return new AbsoluteYOffsetAddressInstructionEncoder(this, address);
		}
	}
}