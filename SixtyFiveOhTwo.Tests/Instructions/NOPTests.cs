using FluentAssertions;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.NOP;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
    public class NOPTests : InstructionTest
    {
        private const int ScrambleSeed = 0x5F3759DF;

        public NOPTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void JMP_Absolute_SetsProgramCounter()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new NoOperationInstruction().Write())
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(1 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.NOP_Implied,
                    TCnt.GracefulExit));
        }
    }
}
