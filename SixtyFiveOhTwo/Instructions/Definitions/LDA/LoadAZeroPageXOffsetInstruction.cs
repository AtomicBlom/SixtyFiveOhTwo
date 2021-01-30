using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    //Logic:
    //A = M
    //P.N = A.7
    //P.Z = (A == 0) ? 1 : 0
    public sealed class LoadAZeroPageXOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xB5;
        public string Mnemonic => "LDA";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            cpu.Bus.Clock.Wait(); //One cycle for the adder to calculate
            var address = (ushort)((zeroPageOffset + cpuState.IndexRegisterX) & 0xFF);
            cpuState.Accumulator = cpu.Bus.ReadByte(address);

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.Accumulator);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageXOffsetAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
