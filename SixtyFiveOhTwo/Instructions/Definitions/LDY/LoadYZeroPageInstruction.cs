using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    //Logic:
    //Y = M
    //P.N = Y.7
    //P.Z = (Y == 0) ? 1 : 0
    public sealed class LoadYZeroPageInstruction : IInstruction
    {
        public byte OpCode => 0xA4;
        public string Mnemonic => "LDY";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var address = (ushort)(0x0000 | (cpu.ReadProgramCounterByte() & 0xFF));
            cpuState.IndexRegisterY = cpu.Bus.ReadByte(address);

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterY);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
