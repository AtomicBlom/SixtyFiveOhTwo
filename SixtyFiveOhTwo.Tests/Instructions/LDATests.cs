using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.LDA;
using SixtyFiveOhTwo.Tests.Util;
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
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction<LoadAZeroPageInstruction>(dataStart)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_Immediate_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction<LoadAImmediateInstruction>(value)
                .AddInstruction<GracefulExitInstruction>()
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_ZeroPageXOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            byte dataStart = 0x05;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction<LoadAZeroPageXOffsetInstruction>(dataStart)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, 0x55, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_Absolute_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction<LoadAAbsoluteInstruction>(dataStart)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_AbsoluteXOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction<LoadAAbsoluteXOffsetInstruction>(dataStart)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, 0x55, value)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Fact]
        public void LDA_AbsoluteXOffset_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction<LoadAAbsoluteXOffsetInstruction>(dataStart)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, 0x55, 0x66)
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(0x66);

            AssertProgramStats(1);
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_AbsoluteYOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort dataStart = 0xAAA5;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction<LoadAAbsoluteYOffsetInstruction>(dataStart)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, 0x55, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(value);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Fact]
        public void LDA_AbsoluteYOffset_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction<LoadAAbsoluteYOffsetInstruction>(dataStart)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, 0x55, 0x66)
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(0x66);

            AssertProgramStats(1);
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_IndexedIndirectXOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction<LoadAIndirectXOffsetInstruction>(0x00)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, value)
                .SetData(zeroPageStart, 0x88, 0xFF, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.IndexRegisterX.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDA_IndexedIndirectYOffset_ShouldSetAccumulatorAndStatus(byte value, bool negativeFlag, bool zeroFlag)
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAF0;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction<LoadAIndirectYOffsetInstruction>(0x01)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, 0x55, value)
                .SetData(zeroPageStart, 0x88, 0xF0, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();
            
            State.Accumulator.Should().Be(value);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Fact]
        public void LDA_IndexedIndirectYOffset_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ushort zeroPageStart = 0x0000;
            ushort dataStart = 0xAAFF;
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction<LoadAIndirectYOffsetInstruction>(0x01)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(dataStart, 0x55, 0x00)
                .SetData(zeroPageStart, 0x88, 0xFF, 0xAA)
                .Write(MemoryBytes);

            Cpu.Run();

            State.Accumulator.Should().Be(0x00);
            State.IndexRegisterY.Should().Be(0x01);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);

            AssertProgramStats(1);
        }
    }
}
