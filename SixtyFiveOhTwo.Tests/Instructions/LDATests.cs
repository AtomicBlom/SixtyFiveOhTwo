using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.LDA;
using SixtyFiveOhTwo.Instructions.Definitions.LDX;
using SixtyFiveOhTwo.Instructions.Definitions.LDY;
using SixtyFiveOhTwo.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
    public class LDATests : InstructionTest
    {
        private const int ScrambleSeed = 0x5F3759DF;

        public LDATests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void LDA_ZeroPage_WithPositiveValue_ShouldSetAccumulatorAndStatus()
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAZeroPageInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55)
                .Write(MemoryBytes);

            Cpu.Run();
            //6 to JMP, 3 to LDA(ZP), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 3 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 1));
            State.Accumulator.Should().Be(0x55);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_ZeroPage_WithNegativeValue_ShouldSetAccumulatorAndStatus()
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAZeroPageInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0xF5)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 3 to LDA(ZP), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 3 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 1));
            State.Accumulator.Should().Be(0xF5);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_ZeroPage_WithZeroValue_ShouldSetAccumulatorAndStatus()
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAZeroPageInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 3 to LDA(ZP), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 3 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 1));
            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_Immediate_WithPositiveValue_ShouldSetAccumulatorAndStatus()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAImmediateInstruction().Write(0x55))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDA(Immediate), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 1));
            State.Accumulator.Should().Be(0x55);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_Immediate_WithNegativeValue_ShouldSetAccumulatorAndStatus()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAImmediateInstruction().Write(0xF5))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDA(Immediate), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 1));
            State.Accumulator.Should().Be(0xF5);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_Immediate_WithZeroValue_ShouldSetAccumulatorAndStatus()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAImmediateInstruction().Write(0x00))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDA(Immediate), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 1));
            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_ZeroPageXOffset_WithPositiveValue_ShouldSetAccumulatorAndStatus()
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAZeroPageXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x66)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 2 to LDA(ZP,X), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0x66);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_ZeroPageXOffset_WithNegativeValue_ShouldSetAccumulatorAndStatus()
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAZeroPageXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0xF6)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(ZP,X), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0xF6);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_ZeroPageXOffset_WithZeroValue_ShouldSetAccumulatorAndStatus()
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAZeroPageXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(ZP,X), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_Absolute_WithPositiveValue_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAAbsoluteInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 3 + 1));
            State.Accumulator.Should().Be(0x55);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_Absolute_WithNegativeValue_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAAbsoluteInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0xF5)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 3 + 1));
            State.Accumulator.Should().Be(0xF5);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_Absolute_WithZeroValue_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAAbsoluteInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 3 + 1));
            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_AbsoluteXOffset_WithPositiveValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x66)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0x66);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_AbsoluteXOffset_WithNegativeValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0xF6)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0xF6);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_AbsoluteXOffset_WithZeroValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_AbsoluteXOffset_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x66)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4+1 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 5 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0x66);
        }

        [Fact]
        public void LDA_AbsoluteYOffset_WithPositiveValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteYOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x66)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0x66);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_AbsoluteYOffset_WithNegativeValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteYOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0xF6)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0xF6);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_AbsoluteYOffset_WithZeroValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteYOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 4 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_AbsoluteYOffset_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteYOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x66)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 5 to LDA(Abs) with cross boundary access, 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 5 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 3 + 1));
            State.Accumulator.Should().Be(0x66);
        }

        [Fact]
        public void LDA_IndexedIndirectXOffset_WithPositiveValue_ShouldSetAccumulatorAndStatus()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectXOffsetInstruction().Write(0x00))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55)
                .SetData(zeroPageStart, 0x88, 0xFF, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm),  4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 6 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0x55);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_IndexedIndirectXOffset_WithNegativeValue_ShouldSetAccumulatorAndStatus()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectXOffsetInstruction().Write(0x00))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0xF5)
                .SetData(zeroPageStart, 0x88, 0xFF, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm),  4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 6 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0xF5);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_IndexedIndirectXOffset_WithZeroValue_ShouldSetAccumulatorAndStatus()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectXOffsetInstruction().Write(0x00))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x00)
                .SetData(zeroPageStart, 0x88, 0xFF, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm),  4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 6 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_IndexedIndirectYOffset_WithPositiveValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAF0;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectYOffsetInstruction().Write(0x01))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x66)
                .SetData(zeroPageStart, 0x88, 0xF0, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm),  4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 5 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0x66);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_IndexedIndirectYOffset_WithNegativeValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAF0;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectYOffsetInstruction().Write(0x01))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0xF6)
                .SetData(zeroPageStart, 0x88, 0xF0, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm),  4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 5 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0xF6);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_IndexedIndirectYOffset_WithZeroValueWithinPageBoundary_ShouldSetAccumulatorAndStatus()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAF0;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectYOffsetInstruction().Write(0x01))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x00)
                .SetData(zeroPageStart, 0x88, 0xF0, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm),  4 to LDA(Abs), 1 to Graceful Exit.
            Clock.Ticks.Should().Be(3 + 2 + 5 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }

        [Fact]
        public void LDA_IndexedIndirectYOffset_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectYOffsetInstruction().Write(0x01))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, 0x00)
                .SetData(zeroPageStart, 0x88, 0xFF, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            //3 to JMP, 2 to LDX(imm), 5 to LDA(IndirectY), 1 to Graceful Exit, +1 page boundary penalty.
            Clock.Ticks.Should().Be(3 + 2 + 5 + 1 + 1);
            State.ProgramCounter.Should().Be((ushort)(ProgramStartAddress + 2 + 2 + 1));
            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
        }
    }
}
