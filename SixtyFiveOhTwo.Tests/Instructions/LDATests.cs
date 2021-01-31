using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.LDA;
using SixtyFiveOhTwo.Instructions.Definitions.LDX;
using SixtyFiveOhTwo.Instructions.Definitions.LDY;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
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
            
            State.Accumulator.Should().Be(0x55);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_ZeroPage, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0xF5);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            
            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_ZeroPage, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_ZeroPage, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x55);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Immediate, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0xF5);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            
            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Immediate, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Immediate, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x66);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_ZeroPageX, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0xF6);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));

            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_ZeroPageX, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_ZeroPageX, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x55);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Absolute, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0xF5);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Absolute, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x66);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_AbsoluteX, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0xF6);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Immediate,
                    TCnt.LDA_AbsoluteX,
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x00);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Absolute, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_AbsoluteX, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x66);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_AbsoluteX, 
                    TCnt.PageBoundaryPenalty, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x66);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_AbsoluteY, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0xF6);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_AbsoluteY, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_AbsoluteY, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x66);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_AbsoluteY, 
                    TCnt.PageBoundaryPenalty, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x55);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectX, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0xF5);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectX, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectX, 
                    TCnt.GracefulExit));
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
            
            State.Accumulator.Should().Be(0x66);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectY, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0xF6);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectY, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectY, 
                    TCnt.GracefulExit));
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

            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectY, 
                    TCnt.PageBoundaryPenalty, 
                    TCnt.GracefulExit));
        }
    }
}
