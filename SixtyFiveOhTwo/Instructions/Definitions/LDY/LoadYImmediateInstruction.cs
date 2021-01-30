using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYImmediateInstruction : IInstruction
    {
        public byte OpCode => 0xA0;
        public string Mnemonic => "LDY";
        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var value = cpu.ReadProgramCounterByte();
            cpuState.IndexRegisterY = value;
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterY);
        }

        public IInstructionEncoder Write(byte value)
        {
            return new ImmediateAddressInstructionEncoder(this, value);
        }
    }
}
