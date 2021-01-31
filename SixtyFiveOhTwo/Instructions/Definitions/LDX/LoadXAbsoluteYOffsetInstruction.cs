using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using SixtyFiveOhTwo.Util;
using static SixtyFiveOhTwo.Util.UShortExtensions;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDX
{
    public sealed class LoadXAbsoluteYOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xBE;
        public string Mnemonic => "LDX";

        // read across boundary may incur an extra read
        //https://retrocomputing.stackexchange.com/questions/145/why-does-6502-indexed-lda-take-an-extra-cycle-at-page-boundaries

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var zeroPageOffset = cpu.ReadProgramCounterWord();
            var address = zeroPageOffset.Offset(cpuState.IndexRegisterY);
            var firstFetchAddress = MakeUShort(zeroPageOffset.HighOrderByte(), address.LowOrderByte());

            var value = cpu.Bus.ReadByte(firstFetchAddress);

            if (zeroPageOffset.HighOrderByte() != address.HighOrderByte())
            {
                value = cpu.Bus.ReadByte(address);
            }

            cpuState.IndexRegisterX = value;
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.IndexRegisterX);
        }

        public IInstructionEncoder Write(ushort absoluteAddress)
        {
            return new AbsoluteYOffsetAddressInstructionEncoder(this, absoluteAddress);
        }
    }
}