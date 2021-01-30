using SixtyFiveOhTwo.Components;

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
    }
}
