using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Definitions.JSR;
using SixtyFiveOhTwo.Util;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Util
{
    public class InstructionTest
    {
        protected readonly ushort ProgramStartAddress = 0x0200;

        protected IClock Clock { get; }
        protected byte[] MemoryBytes { get; }
        protected CPU Cpu { get; }
        protected CPUState State => Cpu.State;
        private Dictionary<byte, InstructionBase> InstructionSetLookup { get; }

        protected TestOutputLogger Logger { get; }

        protected InstructionTest(ITestOutputHelper testOutputHelper)
        {
            Logger = new TestOutputLogger(testOutputHelper);
            Clock = new MockClock();

            var cpuExecutionCompletedTokenSource = new CancellationTokenSource();
            
            MemoryBytes = new byte[0xFFFF];
            
            var bus = new MockBus(Clock, MemoryBytes);
            Cpu = new CPU(bus, cpuExecutionCompletedTokenSource, Logger);

            //Add a fake instruction for unit tests to have an exit point.
            var gracefulExitInstruction = new GracefulExitInstruction(cpuExecutionCompletedTokenSource);
            Cpu.AddNonStandardInstruction(gracefulExitInstruction);

            MemoryBytes[CPU.ResetVectorAddressLow] = Cpu.GetInstruction<JumpToSubroutineInstruction>().OpCode;
            MemoryBytes[CPU.ResetVectorAddressLow + 1] = ProgramStartAddress.LowOrderByte();
            MemoryBytes[CPU.ResetVectorAddressLow + 2] = ProgramStartAddress.HighOrderByte();

            InstructionSetLookup = Cpu.InstructionSet.ToDictionary(i => i.OpCode);
        }

        //Normally I absolutely HATE to calculate what results should be, but calculating the PC and Ticks
        // is tedious af.
        protected void AssertProgramStats(int pageBoundaryPenalties = 0)
        {
            var (tCnt, pc) = Cpu.GetExecutionResults(int.MaxValue)
                .ExecutionStats(InstructionSetLookup, pageBoundaryPenalties);

            Clock.Ticks.Should().Be(tCnt);
            State.ProgramCounter.Should().Be(pc);
        }
    }
}