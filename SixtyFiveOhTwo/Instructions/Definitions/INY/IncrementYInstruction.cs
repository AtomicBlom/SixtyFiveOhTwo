using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Instructions.Definitions.INY
{
	public sealed class IncrementYInstruction : ImpliedInstructionBase
	{
		public IncrementYInstruction() : base(0xC8, "INY", 2)
		{
		}

		private new class Microcode : ImpliedInstructionBase.Microcode
		{
			public Microcode(InstructionBase instruction, CPU processor) : base(instruction, processor)
			{
			}

			protected override void RunMicrocode()
			{
				CPUState.IndexRegisterY++;
				CPUState.Status = CPUState.Status.SetNumericFlags(CPUState.IndexRegisterY);
				Yield();
			}
		}

		public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
		{
			return new Microcode(this, cpu);
		}
	}
}
