using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using static SixtyFiveOhTwo.Util.UShortExtensions;

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

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            cpuState.IndexRegisterX = cpu.Bus.ReadByte(ZeroPageAddress(zeroPageOffset));

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterX);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
