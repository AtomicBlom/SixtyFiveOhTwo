﻿using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.JSR;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
    public class JMPTests : InstructionTest
    {
        private const int ScrambleSeed = 0x5F3759DF;

        public JMPTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void JMP_Absolute_SetsProgramCounter()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new JumpAbsoluteInstruction().Write(ProgramStartAddress))
                .MoveCursor(ProgramStartAddress)
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.GracefulExit));
        }

        [Fact]
        public void JMP_Indirect_SetsProgramCounter()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .SetData(CPU.ZeroPageStart, 00, ProgramStartAddress.LowOrderByte(), ProgramStartAddress.HighOrderByte())
                .AddInstruction(new JumpIndirectInstruction().Write(0x0001))
                .MoveCursor(ProgramStartAddress)
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Indirect, 
                    TCnt.GracefulExit));
        }
    }
}
