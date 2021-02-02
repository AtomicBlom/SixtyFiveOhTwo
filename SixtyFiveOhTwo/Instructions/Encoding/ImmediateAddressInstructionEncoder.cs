namespace SixtyFiveOhTwo.Instructions.Encoding
{
    public class ImmediateAddressInstructionEncoder : IInstructionEncoder
    {
        private readonly InstructionBase _instruction;
        private readonly byte _value;

        public ImmediateAddressInstructionEncoder(InstructionBase instruction, byte value)
        {
            _instruction = instruction;
            _value = value;
        }

        public void Write(ref ushort address, byte[] memory)
        {
            memory[address++] = _instruction.OpCode;
            memory[address++] = _value;
        }

        public string ToStringMnemonic()
        {
            return $"{_instruction.Mnemonic} #${_value:X2}";
        }
    }
}