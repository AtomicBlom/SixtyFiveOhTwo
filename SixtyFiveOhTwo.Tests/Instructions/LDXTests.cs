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

        [Fact]
        public void LDX_Immediate_WithAPositiveValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x7A))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x7A);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_Immediate, TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_Immediate_WithANegativeValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXImmediateInstruction().Write(0xFA))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0xFA);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_Immediate, TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_Immediate_WithAZeroValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXImmediateInstruction().Write(0x00))
                .AddInstruction(GracefulExitInstruction.Write())
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_Immediate, TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_ZeroPage_WithAPositiveValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXZeroPageInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x7A);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_ZeroPage, TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_ZeroPage_WithANegativeValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXZeroPageInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0xFA)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0xFA);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_ZeroPage, TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_ZeroPage_WithAZeroValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .AddInstruction(new LoadXZeroPageInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(CPU.ResetVectorAddressLow + 2 + 1);
            Clock.Ticks.Should()
                .Be(Timings.For(TCnt.LDX_ZeroPage, TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_ZeroPageYOffset_WithAPositiveValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXZeroPageYOffsetInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute, 
                    TCnt.LDY_Immediate, 
                    TCnt.LDX_ZeroPageY, 
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_ZeroPageYOffset_WithANegativeValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXZeroPageYOffsetInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A, 0xFE)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0xFE);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Immediate,
                    TCnt.LDX_ZeroPageY,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_ZeroPageYOffset_WithAZeroValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXZeroPageYOffsetInstruction().Write(0x77))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(CPU.ZeroPageStart + 0x77, 0x7A, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 2 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Immediate,
                    TCnt.LDX_ZeroPageY,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_Absolute_WithAPositiveValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXAbsoluteInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7E)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x7E);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Absolute,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_Absolute_WithANegativeValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXAbsoluteInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0xFE)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0xFE);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Absolute,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_Absolute_WithAZeroValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .AddInstruction(new LoadXAbsoluteInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterX.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDX_Absolute,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_AbsoluteY_WithAPositiveValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXAbsoluteYOffsetInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7A, 0x7E)
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
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_AbsoluteY_WithANegativeValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXAbsoluteYOffsetInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7A, 0xFE)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x01);
            State.IndexRegisterX.Should().Be(0xFE);
            State.Status.Should().HaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().NotHaveFlag(ProcessorStatus.ZeroFlag);

            State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 3 + 1));
            Clock.Ticks.Should().Be(
                Timings.For(
                    TCnt.JMP_Absolute,
                    TCnt.LDY_Immediate,
                    TCnt.LDX_AbsoluteY,
                    TCnt.GracefulExit));
        }

        [Fact]
        public void LDX_AbsoluteY_WithAZeroValue_SetsXAndStatusFlags()
        {
            ProgramBuilder.Start(Logger)
                .ScrambleData(seed: ScrambleSeed)
                .JMP(ProgramStartAddress, true)
                .SetYRegister(0x01)
                .AddInstruction(new LoadXAbsoluteYOffsetInstruction().Write(0x77AA))
                .AddInstruction(GracefulExitInstruction.Write())
                .SetData(0x77AA, 0x7A, 0x00)
                .Write(MemoryBytes);

            Cpu.Run();

            State.IndexRegisterY.Should().Be(0x01);
            State.IndexRegisterX.Should().Be(0x00);
            State.Status.Should().NotHaveFlag(ProcessorStatus.NegativeFlag);
            State.Status.Should().HaveFlag(ProcessorStatus.ZeroFlag);

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
