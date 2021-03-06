﻿using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.NOP;
using SixtyFiveOhTwo.Tests.Util;
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
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction<NoOperationInstruction>()
                .AddInstruction<GracefulExitInstruction>()
                .Write(MemoryBytes);

            Cpu.Run();

            AssertProgramStats();
        }
    }
}
