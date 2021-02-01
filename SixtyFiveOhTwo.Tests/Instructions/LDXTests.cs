using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.LDX;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
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
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXImmediateInstruction().Write(value))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_Immediate, TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_ZeroPage_WithAPositiveValue_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXZeroPageInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_ZeroPage, TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_ZeroPageYOffset_WithAPositiveValue_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXZeroPageYOffsetInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7E, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDY_Immediate, 
                    TCnt.LDX_ZeroPageY, 
                    TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_Absolute_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXAbsoluteInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Absolute,
                    TCnt.GracefulExit));
        }

        [Theory]
        [InlineData(0x7A, false, false)]
        [InlineData(0xFA, true, false)]
        [InlineData(0x00, false, true)]
        public void LDX_AbsoluteY_WithAPositiveValue_SetsXAndStatusFlags(byte value, bool negativeFlag, bool zeroFlag)
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXAbsoluteYOffsetInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7E, value)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x01);
            State.IndexRegisterX.Should().Be(value);
            State.Status.HasFlag(ProcessorStatus.ZeroFlag).Should().Be(zeroFlag);
            State.Status.HasFlag(ProcessorStatus.NegativeFlag).Should().Be(negativeFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Immediate,
                    TCnt.LDX_AbsoluteY,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_AbsoluteY_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXAbsoluteYOffsetInstruction().Write(0x77FF))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77FF, 0x7A, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x01);
            State.IndexRegisterX.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Immediate,
                    TCnt.LDX_AbsoluteY,
                    TCnt.PageBoundaryPenalty,
                    TCnt.GracefulExit));
        }
    }
}
