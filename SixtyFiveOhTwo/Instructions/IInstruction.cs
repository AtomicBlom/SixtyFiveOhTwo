using SixtyFiveOhTwo.Components;

namespace SixtyFiveOhTwo.Instructions
{
    public interface IInstruction
    {
        public byte OpCode { get; }
        public string Mnemonic { get; }
        void Execute(CPU cpu);
    }
}