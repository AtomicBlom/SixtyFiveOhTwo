using System;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Definitions.JSR;
using SixtyFiveOhTwo.Instructions.Definitions.LDX;
using SixtyFiveOhTwo.Instructions.Definitions.LDY;

namespace SixtyFiveOhTwo.Emit
{
    public class ProgramBuilder
    {
        private readonly ILogger _logger;
        private readonly byte[] _memory = new byte[0xFFFF];
        private ushort _currentLocation;

        private ProgramBuilder(ILogger logger)
        {
            _logger = logger;
            _currentLocation = CPU.ResetVectorAddressLow;
        }

        public ProgramBuilder MoveCursor(ushort address)
        {
            _currentLocation = address;
            return this;
        }

        public void Write(byte[] memory)
        {
            if (memory.Length != _memory.Length)
            {
                throw new ArgumentException("memory array had an invalid length", nameof(memory));
            }

            for (var i = 0; i < _memory.Length; i++)
            {
                memory[i] = _memory[i];
            }

            _logger.WriteLine("--- End Program Builder ---");
            _logger.WriteLine(string.Empty);
        }

        private ProgramBuilder JSR(ushort address, bool cursorFollow = false)
        {
            AddInstruction(new JumpToSubroutineInstruction().Write(address));

            if (cursorFollow)
            {
                _currentLocation = address;
            }

            return this;
        }

        public ProgramBuilder JMP(ushort address, bool cursorFollow = false)
        {
            AddInstruction(new JumpAbsoluteInstruction().Write(address));

            if (cursorFollow)
            {
                _currentLocation = address;
            }

            return this;
        }

        public static ProgramBuilder Start(ILogger logger)
        {
            logger.WriteLine("--- Start Program Builder ---");
            return new(logger);
        }

        public ProgramBuilder AddInstruction(IInstructionEncoder opCodeWriter)
        {
            _logger.WriteLine($"{_currentLocation:X4}: {opCodeWriter.ToStringMnemonic()}");
            opCodeWriter.Write(ref _currentLocation, _memory);
            return this;
        }

        public ProgramBuilder SetData(ushort dataStart, params byte[] data)
        {
            for (var i = 0; i < data.Length; ++i)
            {
                var address = (dataStart + i) & 0xFFFF;
                _memory[address] = data[i];
            }
            return this;
        }

        public ProgramBuilder ScrambleData(ushort dataStart = 0x0000, ushort length = 0xFFFF, int? seed = null)
        {
            var random = seed == null ? new Random() : new Random(seed.Value);
            
            var end = Math.Min(dataStart + length, 0xFFFF);
            random.NextBytes(_memory.AsSpan(dataStart, end - dataStart));
            return this;
        }

        public ProgramBuilder SetXRegister(byte value)
        {
            AddInstruction(new LoadXImmediateInstruction().Write(value));
            return this;
        }

        public ProgramBuilder SetYRegister(byte value)
        {
            AddInstruction(new LoadYImmediateInstruction().Write(value));
            return this;
        }
    }
}