using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.JSR;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
    public class JSRTests : InstructionTest
    {
        private const int ScrambleSeed = 0x5F3759DF;

        public JSRTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void JMP_Absolute_SetsProgramCounter()
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction<JumpToSubroutineInstruction>(ProgramStartAddress.Offset(0x100))
                .MoveCursor(ProgramStartAddress.Offset(0x100))
                .AddInstruction<GracefulExitInstruction>()
                .Write(MemoryBytes);

            Cpu.Run();

            MemoryBytes[CPU.StackEnd].Should().Be(CPU.ResetVectorAddressLow.HighOrderByte());
            MemoryBytes[CPU.StackEnd.Offset(-1)].Should().Be(CPU.ResetVectorAddressLow.LowOrderByte());

            AssertProgramStats();
        }
    }
}
