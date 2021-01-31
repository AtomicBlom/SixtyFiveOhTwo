using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.NOP
{
    public class NoOperationInstruction : IInstruction
    {
        public byte OpCode => 0xEA;
        public string Mnemonic => "NOP";

        public void Execute(CPU cpu)
        {
            cpu.Bus.Clock.Wait();
        }

        public IInstructionEncoder Write()
        {
            return new ImpliedAddressInstructionEncoder(this);
        }
    }
}
