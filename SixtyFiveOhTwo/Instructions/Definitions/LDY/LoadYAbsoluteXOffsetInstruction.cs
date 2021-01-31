using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using SixtyFiveOhTwo.Util;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDY
{
    public sealed class LoadYAbsoluteXOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xBC;
        public string Mnemonic => "LDY";

        // read across boundary may incur an extra read
        //https://retrocomputing.stackexchange.com/questions/145/why-does-6502-indexed-lda-take-an-extra-cycle-at-page-boundaries

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterWord();
            var address = (ushort)((zeroPageOffset + cpuState.IndexRegisterX) & 0xFFFF);

            var firstFetchAddress = (ushort)(zeroPageOffset.HighOrderByte() << 8 | address.LowOrderByte());
            var value = cpu.Bus.ReadByte(firstFetchAddress);

            if (zeroPageOffset.HighOrderByte() != address.HighOrderByte())
            {
                value = cpu.Bus.ReadByte(address);
            }

            cpuState.IndexRegisterY = value;
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterY);
        }

        public IInstructionEncoder Write(ushort absoluteAddress)
        {
            return new AbsoluteXOffsetAddressInstructionEncoder(this, absoluteAddress);
        }
    }
}