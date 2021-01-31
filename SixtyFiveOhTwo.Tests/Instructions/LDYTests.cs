using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.LDY;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
    public class LDYTests : InstructionTest
    {
        private const int ScrambleSeed = 0x5F3759DF;

        public LDYTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void LDY_Immediate_WithAPositiveValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x7A))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x7A);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDY_Immediate, TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_Immediate_WithANegativeValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadYImmediateInstruction().Write(0xFA))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0xFA);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDY_Immediate, TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_Immediate_WithAZeroValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadYImmediateInstruction().Write(0x00))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDY_Immediate, TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_ZeroPage_WithAPositiveValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadYZeroPageInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x7A);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDY_ZeroPage, TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_ZeroPage_WithANegativeValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadYZeroPageInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0xFA)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0xFA);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDY_ZeroPage, TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_ZeroPage_WithAZeroValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadYZeroPageInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDY_ZeroPage, TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_ZeroPageXOffset_WithAPositiveValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction(new LoadYZeroPageXOffsetInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDX_Immediate, 
                    TCnt.LDY_ZeroPageX, 
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_ZeroPageXOffset_WithANegativeValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction(new LoadYZeroPageXOffsetInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A, 0xFE)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0xFE);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Immediate,
                    TCnt.LDY_ZeroPageX,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_ZeroPageXOffset_WithAZeroValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction(new LoadYZeroPageXOffsetInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Immediate,
                    TCnt.LDY_ZeroPageX,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_Absolute_WithAPositiveValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYAbsoluteInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Absolute,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_Absolute_WithANegativeValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYAbsoluteInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0xFE)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0xFE);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Absolute,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_Absolute_WithAZeroValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadYAbsoluteInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Absolute,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_AbsoluteX_WithAPositiveValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction(new LoadYAbsoluteXOffsetInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7A, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x01);
            State.IndexRegisterY.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Immediate,
                    TCnt.LDY_AbsoluteX,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_AbsoluteX_WithANegativeValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction(new LoadYAbsoluteXOffsetInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7A, 0xFE)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x01);
            State.IndexRegisterY.Should().Be(0xFE);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Immediate,
                    TCnt.LDY_AbsoluteX,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_AbsoluteX_WithAZeroValue_SetsYAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction(new LoadYAbsoluteXOffsetInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7A, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x01);
            State.IndexRegisterY.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Immediate,
                    TCnt.LDY_AbsoluteX,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDY_AbsoluteX_AcrossPageBoundary_UsesAnExtraClockCycle()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetXRegister(0x01)
                .AddInstruction(new LoadYAbsoluteXOffsetInstruction().Write(0x77FF))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77FF, 0x7A, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x01);
            State.IndexRegisterY.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Immediate,
                    TCnt.LDY_AbsoluteX,
                    TCnt.PageBoundaryPenalty,
                    TCnt.GracefulExit));
        }
    }
}
