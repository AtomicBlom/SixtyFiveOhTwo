using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using static SixtyFiveOhTwo.Util.UShortExtensions;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXZeroPageYOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xB6;
        public string Mnemonic => "LDX";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            cpu.Bus.Clock.Wait(); //One cycle for the adder to calculate
            var address = ZeroPageAddress(zeroPageOffset, cpuState.IndexRegisterY);
            cpuState.IndexRegisterX = cpu.Bus.ReadByte(address);

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterX);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageYOffsetAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
