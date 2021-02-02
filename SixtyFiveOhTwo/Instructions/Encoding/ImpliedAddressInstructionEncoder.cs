namespace SixtyFiveOhTwo.Instructions.Encoding
{
    public class ImpliedAddressInstructionEncoder : IInstructionEncoder
    {
        private readonly InstructionBase _instruction;

        public ImpliedAddressInstructionEncoder(InstructionBase instruction)
        {
            _instruction = instruction;
        }

        public void Write(ref ushort address, byte[] memory)
        {
            memory[address++] = _instruction.OpCode;
        }
        public string ToStringMnemonic()
        {
            return $"{_instruction.Mnemonic}";
        }
    }
}