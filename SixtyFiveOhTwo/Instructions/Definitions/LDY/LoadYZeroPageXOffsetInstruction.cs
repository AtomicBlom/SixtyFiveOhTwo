using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using static SixtyFiveOhTwo.Util.UShortExtensions;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYZeroPageXOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xB4;
        public string Mnemonic => "LDY";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterByte();
            cpu.Bus.Clock.Wait(); //One cycle for the adder to calculate
            var address = ZeroPageAddress(zeroPageOffset, cpuState.IndexRegisterX);
            cpuState.IndexRegisterY = cpu.Bus.ReadByte(address);

            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterY);
        }

        public IInstructionEncoder Write(byte zeroPageAddress)
        {
            return new ZeroPageXOffsetAddressInstructionEncoder(this, zeroPageAddress);
        }
    }
}
