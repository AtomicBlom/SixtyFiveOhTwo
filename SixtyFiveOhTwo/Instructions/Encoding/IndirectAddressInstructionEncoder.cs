using SixtyFiveOhTwo.Util;

namespace SixtyFiveOhTwo.Instructions.Encoding
{
    public class IndirectAddressInstructionEncoder : IInstructionEncoder
    {
        private readonly InstructionBase _instruction;
        private readonly ushort _addressWithAddress;

        public IndirectAddressInstructionEncoder(InstructionBase instruction, ushort addressWithAddress)
        {
            _instruction = instruction;
            _addressWithAddress = addressWithAddress;
        }

        public void Write(ref ushort address, byte[] memory)
        {
            memory[address++] = _instruction.OpCode;
            memory[address++] = _addressWithAddress.LowOrderByte();
            memory[address++] = _addressWithAddress.HighOrderByte();
        }

        public string ToStringMnemonic()
        {
            return $"{_instruction.Mnemonic} (${_addressWithAddress:X4})";
        }
    }
}