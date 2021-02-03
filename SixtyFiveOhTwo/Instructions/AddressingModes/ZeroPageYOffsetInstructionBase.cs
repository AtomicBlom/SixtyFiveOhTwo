using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
	public abstract class ZeroPageYOffsetInstructionBase : InstructionBase, IParameterInstruction<byte>
	{
        protected ZeroPageYOffsetInstructionBase(byte opCode, string mnemonic, byte tCnt) : base(opCode, mnemonic, tCnt, 2) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var initialXRegister = CPUState.IndexRegisterX;
                var zeroPageOffset = ReadNextOpCodeByte();
                RunMicrocode(zeroPageOffset);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.ZeroPageIndexed,
                    CPUState = CPUState,
                    ZeroPageOffset = zeroPageOffset,
                    Y = initialXRegister
                };
            }

            protected abstract void RunMicrocode(byte zeroPageOffset);
        }

        public IInstructionEncoder GetEncoder(byte address)
		{
			return new ZeroPageYOffsetAddressInstructionEncoder(this, address);
		}
	}
}