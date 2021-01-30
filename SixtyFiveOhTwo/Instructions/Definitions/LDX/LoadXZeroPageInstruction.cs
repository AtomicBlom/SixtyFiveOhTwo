using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    //Logic:
    //X = M
    //P.N = X.7
    //P.Z = (X == 0) ? 1 : 0
    public sealed class LoadXZeroPageInstruction : IInstruction
    {
        public byte OpCode => 0xA6;
        public string Mnemonic => "LDX";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var address = (ushort)(0x0000 | (cpu.ReadProgramCounterByte() & 0xFF));
            cpuState.IndexRegisterX = cpu.Bus.ReadByte(address);

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterX);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
