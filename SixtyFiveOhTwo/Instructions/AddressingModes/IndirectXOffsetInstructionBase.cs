using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;
using SixtyFiveOhTwo.Instructions.Encoding;
using static SixtyFiveOhTwo.Util.AddressUtilities;

namespace SixtyFiveOhTwo.Instructions.AddressingModes
{
	public abstract class IndirectXOffsetInstructionBase : InstructionBase, IParameterInstruction<byte>
	{
        protected IndirectXOffsetInstructionBase(byte opCode, string mnemonic, byte tCnt) : base(opCode, mnemonic, tCnt, 2) { }

        protected new abstract class Microcode : InstructionBase.Microcode
        {

            protected Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
            {
            }

            public sealed override DebugExecutionResult Execute()
            {


                var initialXRegister = CPUState.IndexRegisterX;
                var zeroPageOffset = ReadNextOpCodeByte();
                var lsb = ReadByteFromBus(ZeroPageAddress(zeroPageOffset, initialXRegister));
                var msb = ReadByteFromBus(ZeroPageAddress(zeroPageOffset, initialXRegister, 1));
                var address = MakeUShort(msb, lsb);

                RunMicrocode(address);
                return new()
                {
                    OpCode = Instruction.OpCode,
                    AddressingMode = AddressingMode.IndexedIndirect,
                    CPUState = CPUState,
                    ZeroPageOffset = zeroPageOffset,
                    X = initialXRegister
                };
            }

            protected abstract void RunMicrocode(ushort address);
        }
        
        public IInstructionEncoder GetEncoder(byte zeroPageOffset)
		{
			return new IndirectXAddressInstructionEncoder(this, zeroPageOffset);
		}
	}
}