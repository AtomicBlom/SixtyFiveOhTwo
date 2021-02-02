﻿namespace SixtyFiveOhTwo.Instructions.Encoding
{
    public class ZeroPageYOffsetAddressInstructionEncoder : IInstructionEncoder
    {
        private readonly InstructionBase _instruction;
        private readonly byte _zeroPageAddress;

        public ZeroPageYOffsetAddressInstructionEncoder(InstructionBase instruction, byte zeroPageAddress)
        {
            _instruction = instruction;
            _zeroPageAddress = zeroPageAddress;
        }

        public void Write(ref ushort address, byte[] memory)
        {
            memory[address++] = _instruction.OpCode;
            memory[address++] = _zeroPageAddress;
        }

        public string ToStringMnemonic()
        {
            return $"{_instruction.Mnemonic} ${_zeroPageAddress:X2},Y";
        }
    }
}