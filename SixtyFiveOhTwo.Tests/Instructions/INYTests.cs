using FluentAssertions;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.INY;
using SixtyFiveOhTwo.Instructions.Definitions.JSR;
using SixtyFiveOhTwo.Tests.Util;
using SixtyFiveOhTwo.Util;
using Xunit;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Instructions
{
	public class INYTests : InstructionTest
	{
		private const int ScrambleSeed = 0x5F3759DF;

		public INYTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
		{
		}

		[Fact]
		public void INY_()
		{
			ProgramBuilder.Start(Cpu.InstructionSet, Logger)
				.ScrambleData(seed: ScrambleSeed)
				.JMP(ProgramStartAddress, true)
				.SetYRegister(0x41)
				.AddInstruction<IncrementYInstruction>()
				.AddInstruction<GracefulExitInstruction>()
				.Write(MemoryBytes);

			Cpu.Run();


			Cpu.State.IndexRegisterY.Should().Be(0x42);
			State.ProgramCounter.Should().Be(ProgramStartAddress.Offset(2 + 1 + 1));
			Clock.Ticks.Should().Be(
				Timings.For(
					TCnt.JMP_Absolute,
					TCnt.LDY_Immediate,
					TCnt.INY,
					TCnt.GracefulExit));
		}
	}
}