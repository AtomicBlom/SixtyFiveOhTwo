using System.Threading;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.AddressingModes;

namespace SixtyFiveOhTwo.Tests.Util
{
    public class GracefulExitInstruction : ImpliedInstructionBase
    {
        private readonly CancellationTokenSource _cpuExecutionCompletedTokenSource;

        public GracefulExitInstruction(CancellationTokenSource cpuExecutionCompletedTokenSource) : base(0xFF, "**TEST END")
        {
            _cpuExecutionCompletedTokenSource = cpuExecutionCompletedTokenSource;
        }

        private new class Microcode : ImpliedInstructionBase.Microcode
        {
            private readonly CancellationTokenSource _cpuExecutionCompletedTokenSource;

            public Microcode(InstructionBase instruction, CPU processor,
                CancellationTokenSource cpuExecutionCompletedTokenSource) : base(instruction, processor)
            {
                _cpuExecutionCompletedTokenSource = cpuExecutionCompletedTokenSource;
            }

            protected override void RunMicrocode()
            {
                _cpuExecutionCompletedTokenSource.Cancel();
            }
        }

        public override InstructionBase.Microcode GetExecutableMicrocode(CPU cpu)
        {
            return new Microcode(this, cpu, _cpuExecutionCompletedTokenSource);
        }
    }
}