using FluentAssertions;
using SixtyFiveOhTwo.Components;
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

        [Theory]
        [InlineData(0x42, 0x42, false, false)]
        public void AND_(byte value, byte expectedResult, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetARegister(0xFF)
                .AddInstruction<AndImmediateInstruction>(value)
                .AddInstruction<GracefulExitInstruction>()
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);
            State.Accumulator.Should().Be(expectedResult);

            AssertProgramStats();
        }
    }
}