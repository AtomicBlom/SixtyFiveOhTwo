using System.Threading;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Tests.Util
{
    public class GracefulExitInstruction : IInstruction
    {
        private readonly CancellationTokenSource _cpuExecutionCompletedTokenSource;

        public GracefulExitInstruction(CancellationTokenSource cpuExecutionCompletedTokenSource)
        {
            _cpuExecutionCompletedTokenSource = cpuExecutionCompletedTokenSource;
        }

        public byte OpCode => 0xFF;
        public string Mnemonic => "**TEST END";
        public void Execute(CPU cpu)
        {
            _cpuExecutionCompletedTokenSource.Cancel();
        }

        public IInstructionEncoder Write()
        {
            return new ImpliedAddressInstructionEncoder(this);
        }
    }
}