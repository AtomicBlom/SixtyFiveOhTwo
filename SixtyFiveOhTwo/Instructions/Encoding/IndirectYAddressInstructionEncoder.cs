﻿namespace SixtyFiveOhTwo.Instructions.Encoding
{
    public class IndirectYAddressInstructionEncoder : IInstructionEncoder
    {
        private readonly InstructionBase _instruction;
        private readonly byte _zeroPageOffset;

        public IndirectYAddressInstructionEncoder(InstructionBase instruction, byte zeroPageOffset)
        {
            _instruction = instruction;
            _zeroPageOffset = zeroPageOffset;
        }

        public void Write(ref ushort address, byte[] memory)
        {
            memory[address++] = _instruction.OpCode;
            memory[address++] = _zeroPageOffset;
        }

        public string ToStringMnemonic()
        {
            return $"{_instruction.Mnemonic} (${_zeroPageOffset:X4}),Y";
        }
    }
}