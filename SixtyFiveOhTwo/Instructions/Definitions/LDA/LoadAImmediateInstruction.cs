using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAImmediateInstruction : IInstruction
    {
        public byte OpCode => 0xA9;
        public string Mnemonic => "LDA";
        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var value = cpu.ReadProgramCounterByte();
            cpuState.Accumulator = value;
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.Accumulator);
        }

        public IInstructionEncoder Write(byte value)
        {
            return new ImmediateAddressInstructionEncoder(this, value);
        }
    }
}
