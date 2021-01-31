using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Definitions.JSR;
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
        private readonly IInstruction[] _instructions;
        private readonly byte[] _bytes;
        private readonly MockBus _bus;

        public CPUTests(ITestOutputHelper testOutputHelper)
        {
            _logger = new TestOutputLogger(testOutputHelper);
            _clock = Substitute.For<IClock>();

            _cpuExecutionCompletedTokenSource = new CancellationTokenSource();

            var instructionSet = new InstructionSet();
            _instructions = instructionSet.Instructions;
            //Add a fake instruction for unit tests to have an exit point.
            _instructions[0xFF] = new GracefulExitInstruction(_cpuExecutionCompletedTokenSource);

            _bytes = new byte[0xFFFF];
            _bytes[CPU.ResetVectorAddressLow] = new JumpToSubroutineInstruction().OpCode;
            _bytes[CPU.ResetVectorAddressLow + 1] = ProgramStartLabel.LowOrderByte();
            _bytes[CPU.ResetVectorAddressLow + 2] = ProgramStartLabel.HighOrderByte();

            _bus = new MockBus(_clock, _bytes);
        }

        [Fact]
        public void CPU_Run_CanExitGracefully()
        {
            _bytes[CPU.ResetVectorAddressLow] = 0xFF;

            var cpu = new CPU(_instructions, _bus, _cpuExecutionCompletedTokenSource, _logger);
            cpu.Run();
            // 1 cycle to wait, 1 to Graceful Exit.
            _clock.Received(2).Wait();
            cpu.State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 1);
        }

        [Fact]
        public void CPU_Reset_InitializesCPUCorrectly()
        {
            _bytes[CPU.ResetVectorAddressLow] = 0xFF;

            var cpu = new CPU(_instructions, _bus, _cpuExecutionCompletedTokenSource, _logger);
            cpu.Reset();
            cpu.State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow);
            cpu.State.Status.Should().HaveFlag(ProcessorStatus.InterruptDisable);
        }

        [Fact]
        public void CPU_NoInstructionsHaveCollidingOpCodes()
        {
            var instructions =
                typeof(InstructionSet)
                    .Assembly
                    .DefinedTypes
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => t.IsAssignableTo(typeof(IInstruction)))
                    .Select(Activator.CreateInstance)
                    .Cast<IInstruction>();

            var seenInstructions = new IInstruction[byte.MaxValue + 1];
            foreach (var instruction in instructions)
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
            var instructions = new InstructionSet().Instructions;

            var implementedInstructions = instructions.Where(i => i is not null).GroupBy(i => i.Mnemonic);
            implementedInstructions.Should().HaveCount(56);
        }

        [Fact]
        public void CPU_ImplementsAll6502OpCodes()
        {
            var instructions = new InstructionSet().Instructions;

            var implementedOpCodes = instructions.Where(i => i is not null).ToList();
            implementedOpCodes.Should().HaveCount(151);
        }
#endif
    }
}
