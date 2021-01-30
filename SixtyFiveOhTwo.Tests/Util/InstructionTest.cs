using System.Threading;
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

        protected readonly GracefulExitInstruction GracefulExitInstruction;
        protected TestOutputLogger Logger { get; }

        protected InstructionTest(ITestOutputHelper testOutputHelper)
        {
            Logger = new TestOutputLogger(testOutputHelper);
            Clock = new MockClock();

            var cpuExecutionCompletedTokenSource = new CancellationTokenSource();

            var instructionSet = new InstructionSet();
            var instructions = instructionSet.Instructions;
            //Add a fake instruction for unit tests to have an exit point.
            GracefulExitInstruction = new GracefulExitInstruction(cpuExecutionCompletedTokenSource);
            instructions[0xFF] = GracefulExitInstruction;

            MemoryBytes = new byte[0xFFFF];
            MemoryBytes[CPU.ResetVectorAddressLow] = new JumpToSubroutineInstruction().OpCode;
            MemoryBytes[CPU.ResetVectorAddressLow + 1] = ProgramStartAddress.LowOrderByte();
            MemoryBytes[CPU.ResetVectorAddressLow + 2] = ProgramStartAddress.HighOrderByte();

            var bus = new MockBus(Clock, MemoryBytes);
            Cpu = new CPU(instructions, bus, cpuExecutionCompletedTokenSource, Logger);
        }
    }
}