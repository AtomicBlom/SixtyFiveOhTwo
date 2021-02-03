using FluentAssertions;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.AND;
using SixtyFiveOhTwo.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
    public class ANDTests : InstructionTest
    {
        private const int ScrambleSeed = 0x5F3759DF;

        public ANDTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void AND_()
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetARegister(0xAE)
                .AddInstruction<AndImmediateInstruction>(0x07)
                .AddInstruction<GracefulExitInstruction>()
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(6);

            AssertProgramStats();
        }
    }
}