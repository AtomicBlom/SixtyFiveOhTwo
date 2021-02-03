using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.LDX;
using SixtyFiveOhTwo.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
    public class LDXTests : InstructionTest
    {
        private const int ScrambleSeed = 0x5F3759DF;

        public LDXTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_Immediate_WithAPositiveValue_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction<LoadXImmediateInstruction>(value)
                .AddInstruction<GracefulExitInstruction>()
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_ZeroPage_WithAPositiveValue_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction<LoadXZeroPageInstruction>(0x77)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(CPU.ZeroPageStart + 0x77, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_ZeroPageYOffset_WithAPositiveValue_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction<LoadXZeroPageYOffsetInstruction>(0x77)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(CPU.ZeroPageStart + 0x77, 0x7E, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_Absolute_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction<LoadXAbsoluteInstruction>(0x77AA)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(0x77AA, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_AbsoluteY_WithAPositiveValue_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction<LoadXAbsoluteYOffsetInstruction>(0x77AA)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(0x77AA, 0x7E, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x01);
            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            AssertProgramStats();
        }

        [Fact]
        public void LDX_AbsoluteY_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ProgramBuilder.Start(Cpu.InstructionSet, Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction<LoadXAbsoluteYOffsetInstruction>(0x77FF)
                .AddInstruction<GracefulExitInstruction>()
                .SetData(0x77FF, 0x7A, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x01);
            State.IndexRegisterX.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            AssertProgramStats(1);
        }
    }
}
