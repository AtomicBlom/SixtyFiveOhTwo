using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXImmediateInstruction : IInstruction
    {
        public byte OpCode => 0xA2;
        public string Mnemonic => "LDX";
        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var value = cpu.ReadProgramCounterByte();
            cpuState.IndexRegisterX = value;
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterX);
        }

        public IInstructionEncoder Write(byte value)
        {
            return new ImmediateAddressInstructionEncoder(this, value);
        }
    }
}
