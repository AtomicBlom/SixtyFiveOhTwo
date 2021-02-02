using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests
{
    public class CPUTests
    {
        const ushort ProgramStartLabel = 0x0000;

        private readonly IClock _clock;
        private readonly CancellationTokenSource _cpuExecutionCompletedTokenSource;
        private readonly ILogger _logger;
        private readonly byte[] _bytes;
        private readonly MockBus _bus;
        private CPU _cpu;

        public CPUTests(ITestOutputHelper testOutputHelper)
        {
            _logger = new TestOutputLogger(testOutputHelper);
            _clock = Substitute.For<IClock>();

            _cpuExecutionCompletedTokenSource = new CancellationTokenSource();
            
            _bytes = new byte[0xFFFF];
            _bytes[CPU.ResetVectorAddressLow] = 0x4C; //JMP
            _bytes[CPU.ResetVectorAddressLow + 1] = ProgramStartLabel.LowOrderByte();
            _bytes[CPU.ResetVectorAddressLow + 2] = ProgramStartLabel.HighOrderByte();

            _bus = new MockBus(_clock, _bytes);
            _cpu = new CPU(_bus, _cpuExecutionCompletedTokenSource, _logger);
            //Add a fake instruction for unit tests to have an exit point.
            _cpu.AddNonStandardInstruction(new GracefulExitInstruction(_cpuExecutionCompletedTokenSource));
        }

        [Fact]
        public void CPU_Run_CanExitGracefully()
        {
            _bytes[CPU.ResetVectorAddressLow] = 0xFF;


            _cpu.Run();
            // 1 cycle to wait, 1 to Graceful Exit.
            _clock.Received(2).Wait();
            _cpu.State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 1);
        }

        [Fact]
        public void CPU_Reset_InitializesCPUCorrectly()
        {
            _bytes[CPU.ResetVectorAddressLow] = 0xFF;

            _cpu.Reset();
            _cpu.State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow);
            _cpu.State.Status.Should().HaveFlag(ProcessorStatus.InterruptDisable);
        }

        [Fact]
        public void CPU_NoInstructionsHaveCollidingOpCodes()
        {
            var seenInstructions = new InstructionBase[byte.MaxValue + 1];
            foreach (var instruction in _cpu.InstructionSet.Where(i => i is not null))
            {
                if (seenInstructions[instruction.OpCode] != null)
                {
                    throw new Exception($"Opcode {instruction.OpCode} collides between {instruction.GetType().Name} and {seenInstructions[instruction.OpCode].GetType().Name}");
                }
                seenInstructions[instruction.OpCode] = instruction;
            }
        }


#if COMPLETION_REPORT
        [Fact]
        public void CPU_ImplementsAll6502Instructions()
        {
            var implementedInstructions = _cpu.InstructionSet.Where(i => i is not null).GroupBy(i => i.Mnemonic);
            implementedInstructions.Should().HaveCount(56);
        }

        [Fact]
        public void CPU_ImplementsAll6502OpCodes()
        {
            var implementedOpCodes = _cpu.InstructionSet.Where(i => i is not null).ToList();
            implementedOpCodes.Should().HaveCount(151);
        }
#endif
    }
}
