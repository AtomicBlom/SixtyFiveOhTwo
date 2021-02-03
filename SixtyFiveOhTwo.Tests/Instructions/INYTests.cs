using FluentAssertions;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.INY;
using SixtyFiveOhTwo.Tests.Util;
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
            AssertProgramStats();
		}
	}
}