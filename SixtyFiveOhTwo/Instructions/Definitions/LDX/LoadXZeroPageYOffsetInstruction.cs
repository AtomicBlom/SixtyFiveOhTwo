using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    //Logic:
    //A = M
    //P.N = A.7
    //P.Z = (A == 0) ? 1 : 0
    public sealed class LoadXZeroPageYOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xB6;
        public string Mnemonic => "LDX";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            cpu.Bus.Clock.Wait(); //One cycle for the adder to calculate
            var address = (ushort)((zeroPageOffset + cpuState.IndexRegisterY) & 0xFF);
            cpuState.IndexRegisterX = cpu.Bus.ReadByte(address);

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterX);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageYOffsetAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
