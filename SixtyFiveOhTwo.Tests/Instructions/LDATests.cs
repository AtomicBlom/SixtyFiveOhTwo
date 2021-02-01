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

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_ZeroPage_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAZeroPageInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_ZeroPage, 
                    TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_Immediate_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAImmediateInstruction().Write(value))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Immediate, 
                    TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_ZeroPageXOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAZeroPageXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_ZeroPageX, 
                    TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_Absolute_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadAAbsoluteInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDA_Absolute, 
                    TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_AbsoluteXOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteXOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

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

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_AbsoluteYOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAAbsoluteYOffsetInstruction().Write(dataStart))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(value);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

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

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_IndexedIndirectXOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectXOffsetInstruction().Write(0x00))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, value)
                .SetData(zeroPageStart, 0x88, 0xFF, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDA_IndirectX, 
                    TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_IndexedIndirectYOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAF0;
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x01))
                .AddInstruction(new LoadAIndirectYOffsetInstruction().Write(0x01))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(dataStart, 0x55, value)
                .SetData(zeroPageStart, 0x88, 0xF0, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

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
