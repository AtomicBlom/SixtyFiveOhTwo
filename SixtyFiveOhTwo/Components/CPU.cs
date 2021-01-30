using System.Threading;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Definitions;

namespace SixtyFiveOhTwo.Components
{
    public class CPU
    {
        public const ushort ResetVectorAddressLow = 0xFFFC;
        public const ushort ResetVectorAddressHigh = 0xFFFD;
        public const ushort StackStart = 0x0100;
        public const ushort ZeroPageStart = 0x0000;

        private readonly IInstruction[] _instructions;
        private readonly IBus _bus;
        private readonly CancellationTokenSource _finishToken;
        private readonly ILogger _logger;
        private CPUState _state;
        
        public CPU(IInstruction[] instructions, IBus bus, CancellationTokenSource finishToken, ILogger logger)
        {
            _instructions = instructions;
            _bus = bus;
            _finishToken = finishToken;
            _logger = logger;
        }

        public ref CPUState State => ref _state;
        public IBus Bus => _bus;

        public void Run()
        {
            Reset();
            _bus.Clock.Wait();
            while (!_finishToken.IsCancellationRequested)
            {
                var address = _state.ProgramCounter;
                _logger.Write($"{address:X4}: ");
                var opCode = ReadProgramCounterByte();
                var instruction = _instructions[opCode];
                if (instruction is not null)
                {
                    instruction.Execute(this);
                    _logger.WriteLine($" ({Bus.Clock.Ticks:00000000} {instruction.GetType().Name})");
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
            _logger.WriteLine($"    SP:     {_state.StackPointer:X4}");
            _logger.WriteLine($"    A:      {_state.Accumulator:X2} ({_state.Accumulator})");
            _logger.WriteLine($"    X:      {_state.IndexRegisterX:X2} ({_state.IndexRegisterX})");
            _logger.WriteLine($"    Y:      {_state.IndexRegisterY:X2} ({_state.IndexRegisterY})");
            _logger.WriteLine($"    Status: {_state.Status:F}");
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
            _bus.WriteValue((ushort)(0x0100 & State.StackPointer), value);
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
            var value = _bus.ReadByte((ushort)(0x0100 & State.StackPointer));
            return value;
        }
    }
}