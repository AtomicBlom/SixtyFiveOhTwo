using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Debugger;

namespace SixtyFiveOhTwo.Components
{
    public class CPU
    {
        public const ushort ResetVectorAddressLow = 0xFFFC;
        public const ushort ResetVectorAddressHigh = 0xFFFD;
        public const ushort StackStart = 0x0100;
        public const ushort StackEnd = 0x01FF;
        public const ushort ZeroPageStart = 0x0000;

        private readonly Dictionary<byte, InstructionBase> _instructions;
        private readonly IBus _bus;
        private readonly CancellationTokenSource _finishToken;
        private readonly ILogger _logger;
        private CPUState _state;

        private bool _isRunning = false;
        
        public CPU(IBus bus, CancellationTokenSource finishToken, ILogger logger)
        {
            _instructions = InstructionSetBuilder.CreateInstructionSet().ToDictionary(k => k.OpCode);
            _bus = bus;
            _finishToken = finishToken;
            _logger = logger;
        }

        public ref CPUState State => ref _state;
        public IBus Bus => _bus;
        public IEnumerable<InstructionBase> InstructionSet => _instructions.Values;

        private readonly DebugExecutionResult[] _executionResults = new DebugExecutionResult[0xFFF];
        private uint _executionWritePosition = 0;
        private uint _writtenResults = 0;

        public void Run()
        {
            lock (this)
            {
                if (_isRunning) throw new ExecutionEngineException("Engine is already running");
                _isRunning = true;
            }

            var microcodeLookup = InstructionSetBuilder.BuildMicrocodeArray(this, _instructions.Values);

            Reset();
            _bus.Clock.Wait();
            while (!_finishToken.IsCancellationRequested)
            {
                var address = _state.ProgramCounter;
                _logger.Write($"{address:X4}: ");
                var opCode = ReadProgramCounterByte();
                var microcode = microcodeLookup[opCode];
                if (microcode is not null)
                {
                    _executionResults[_executionWritePosition] = microcode.Execute();
                    _executionWritePosition++;
                    _executionWritePosition &= 0xFFF;
                    if (_executionWritePosition > _writtenResults)
                    {
                        _writtenResults = _executionWritePosition;
                    }

                    _logger.WriteLine($" ({Bus.Clock.Ticks:00000000} {microcode.Instruction.GetType().Name})");
                }
                else
                {
                    _logger.WriteLine($" ({Bus.Clock.Ticks:00000000} !!!)");
                    _logger.WriteLine(string.Empty);
                    _logger.WriteLine("-------------------------------------------------");
                    _logger.WriteLine($"Encountered Invalid OpCode 0x{opCode:X2} at address 0x{address:X4}");
                    _finishToken.Cancel();
                }
            }

            _logger.WriteLine(string.Empty);
            _logger.WriteLine("Last known processor state:");
            _logger.WriteLine($"    PC:     {_state.ProgramCounter:X4}");
            _logger.WriteLine($"    SP:     {_state.StackPointer:X2}");
            _logger.WriteLine($"    A:      {_state.Accumulator:X2} ({_state.Accumulator})");
            _logger.WriteLine($"    X:      {_state.IndexRegisterX:X2} ({_state.IndexRegisterX})");
            _logger.WriteLine($"    Y:      {_state.IndexRegisterY:X2} ({_state.IndexRegisterY})");
            _logger.WriteLine($"    Status: {_state.Status:F}");

            var lastExecutionResults = GetExecutionResults(10);

            foreach (var debugExecutionResult in lastExecutionResults)
            {
                _logger.WriteLine($"{debugExecutionResult}");
            }
        }

        public DebugExecutionResult[] GetExecutionResults(int maxCount)
        {
            var count = Math.Min(maxCount, _writtenResults);
            var results = new DebugExecutionResult[count];
            var cursor = _executionWritePosition - 1;
            var i = 0;
            while (count > 0)
            {
                results[i] = _executionResults[cursor];

                --cursor;
                cursor &= 0xFFF;
                count--;
                i++;
            }

            return results;

        }

        public byte ReadProgramCounterByte()
        {
            var result = _bus.ReadByte(_state.ProgramCounter);
            _logger.Write($"{result:X2} ");
            _state.ProgramCounter++;
            return result;
        }

        public ushort ReadProgramCounterWord()
        {
            var result = _bus.ReadWord(_state.ProgramCounter);
            _logger.Write($"{result:X4} ");
            _state.ProgramCounter+=2;
            return result;
        }

        public void Reset()
        {
            _state.ProgramCounter = ResetVectorAddressLow;
            _state.StackPointer = 0xFF;
            _state.Status = ProcessorStatus.InterruptDisable;
        }

        public void PushStack(byte value)
        {
            _bus.WriteValue((ushort)(0x0100 | State.StackPointer), value);
            unchecked
            {
                State.StackPointer--;
            }
            
        }

        public byte PopStack()
        {
            unchecked
            {
                State.StackPointer++;
            }
            var value = _bus.ReadByte((ushort)(0x0100 | State.StackPointer));
            return value;
        }

        public T GetInstruction<T>() where T: InstructionBase
        {
            return _instructions.Values.OfType<T>().FirstOrDefault();
        }

        public void AddNonStandardInstruction(InstructionBase instruction)
        {
            if (_instructions.TryGetValue(instruction.OpCode, out var existingInstruction))
                throw new ArgumentException(
                    $"Opcode {instruction.OpCode} was already in use by {existingInstruction.GetType().Name}",
                    nameof(instruction));

            _instructions.Add(instruction.OpCode, instruction);
        }
    }
}