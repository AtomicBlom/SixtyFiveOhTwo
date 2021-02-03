using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
	public abstract class IndirectYOffsetInstructionBase : InstructionBase, IParameterInstruction<byte>
	{
        protected IndirectYOffsetInstructionBase(byte opCode, string mnemonic, byte tCnt) : base(opCode, mnemonic, tCnt, 2) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {
                var initialYRegister = CPUState.IndexRegisterY;
                var zeroPageOffset = ReadNextOpCodeByte();
                RunMicrocode(zeroPageOffset);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.IndirectIndexed,
                    CPUState = CPUState,
                    ZeroPageOffset = zeroPageOffset,
                    Y = initialYRegister
                };
            }

            protected abstract void RunMicrocode(byte zeroPageOffset);
        }

        public IInstructionEncoder GetEncoder(byte zeroPageOffset)
		{
			return new IndirectYAddressInstructionEncoder(this, zeroPageOffset);
		}
	}
}