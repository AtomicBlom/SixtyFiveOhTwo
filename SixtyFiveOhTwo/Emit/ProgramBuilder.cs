using System;
using System.Collections.Generic;
using System.Linq;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Definitions.JMP;
using SixtyFiveOhTwo.Instructions.Definitions.LDA;
using SixtyFiveOhTwo.Instructions.Definitions.LDX;
using SixtyFiveOhTwo.Instructions.Definitions.LDY;

namespace SixtyFiveOhTwo.Emit
{
    public class ProgramBuilder
    {
        private readonly ILogger _logger;
        private readonly byte[] _memory = new byte[0xFFFF];
        private readonly Dictionary<Type, InstructionBase> _instructionLookup;

        private ushort _currentLocation;

        private ProgramBuilder(IEnumerable<InstructionBase> instructionSet, ILogger logger)
        {
            _instructionLookup = instructionSet.Where(t => t is not null).ToDictionary(t => t.GetType());
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

        public ProgramBuilder JMP(ushort address, bool cursorFollow = false)
        {
            AddInstruction<JumpAbsoluteInstruction>(address);

            if (cursorFollow)
            {
                _currentLocation = address;
            }

            return this;
        }

        public static ProgramBuilder Start(IEnumerable<InstructionBase> instructionSet, ILogger logger)
        {
            logger.WriteLine("--- Start Program Builder ---");
            return new(instructionSet, logger);
        }

        public ProgramBuilder AddInstruction<T>(ushort parameter) where T : InstructionBase, IParameterInstruction<ushort>
        {
            if (!_instructionLookup.TryGetValue(typeof(T), out var instruction))
                throw new ArgumentException($"Could not find instruction of type {typeof(T)} in instruction set");

            var encoder = ((T)instruction).GetEncoder(parameter);

            _logger.WriteLine($"{_currentLocation:X4}: {encoder.ToStringMnemonic()}");
            encoder.Write(ref _currentLocation, _memory);
            return this;
        }

        public ProgramBuilder AddInstruction<T>(byte parameter) where T : InstructionBase, IParameterInstruction<byte>
        {
            if (!_instructionLookup.TryGetValue(typeof(T), out var instruction))
                throw new ArgumentException($"Could not find instruction of type {typeof(T)} in instruction set");

            var encoder = ((T)instruction).GetEncoder(parameter);

            _logger.WriteLine($"{_currentLocation:X4}: {encoder.ToStringMnemonic()}");
            encoder.Write(ref _currentLocation, _memory);
            return this;
        }

        public ProgramBuilder AddInstruction<T>() where T : InstructionBase, INoParameterInstruction
        {
            if (!_instructionLookup.TryGetValue(typeof(T), out var instruction))
                throw new ArgumentException($"Could not find instruction of type {typeof(T)} in instruction set");

            var encoder = ((T)instruction).GetEncoder();

            _logger.WriteLine($"{_currentLocation:X4}: {encoder.ToStringMnemonic()}");
            encoder.Write(ref _currentLocation, _memory);
            return this;
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
            AddInstruction<LoadXImmediateInstruction>(value);
            return this;
        }

        public ProgramBuilder SetYRegister(byte value)
        {
            AddInstruction<LoadYImmediateInstruction>(value);
            return this;
        }

        public ProgramBuilder SetARegister(byte value)
        {
            AddInstruction<LoadAImmediateInstruction>(value);
            return this;
        }
    }
}