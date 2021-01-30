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
            //2 to LDX, 1 to Graceful Exit.
            Clock.Ticks.Should().Be(2 + 1);
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
            //2 to LDX, 1 to Graceful Exit.
            Clock.Ticks.Should().Be(2 + 1);
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
            //2 to LDX, 1 to Graceful Exit.
            Clock.Ticks.Should().Be(2 + 1);
        }
    }
}
