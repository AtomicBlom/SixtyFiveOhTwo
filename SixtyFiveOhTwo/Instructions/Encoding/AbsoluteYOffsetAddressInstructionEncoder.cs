using SixtyFiveOhTwo.Util;

namespace SixtyFiveOhTwo.Instructions.Encoding
{
    public class AbsoluteYOffsetAddressInstructionEncoder : IInstructionEncoder
    {
        private readonly IInstruction _instruction;
        private readonly ushort _absoluteAddress;

        public AbsoluteYOffsetAddressInstructionEncoder(IInstruction instruction, ushort absoluteAddress)
        {
            _instruction = instruction;
            _absoluteAddress = absoluteAddress;
        }

        public void Write(ref ushort address, byte[] memory)
        {
            memory[address++] = _instruction.OpCode;
            memory[address++] = _absoluteAddress.LowOrderByte();
            memory[address++] = _absoluteAddress.HighOrderByte();
        }

        public string ToStringMnemonic()
        {
            return $"{_instruction.Mnemonic} ${_absoluteAddress:X4},Y";
        }
    }
}