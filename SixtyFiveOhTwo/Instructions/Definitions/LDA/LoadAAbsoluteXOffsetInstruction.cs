using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using SixtyFiveOhTwo.Util;
using static SixtyFiveOhTwo.Util.UShortExtensions;

namespace SixtyFiveOhTwo.Instructions.Definitions.LDA
{
    public sealed class LoadAAbsoluteXOffsetInstruction : IInstruction
    {
        public byte OpCode => 0xBD;
        public string Mnemonic => "LDA";

        // read across boundary may incur an extra read
        //https://retrocomputing.stackexchange.com/questions/145/why-does-6502-indexed-lda-take-an-extra-cycle-at-page-boundaries

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;

            var absoluteAddress = cpu.ReadProgramCounterWord();
            var address = absoluteAddress.Offset(cpuState.IndexRegisterX);
            var firstFetchAddress = MakeUShort(absoluteAddress.HighOrderByte(), address.LowOrderByte());
            var value = cpu.Bus.ReadByte(firstFetchAddress);

            if (absoluteAddress.HighOrderByte() != address.HighOrderByte())
            {
                value = cpu.Bus.ReadByte(address);
            }
            
            cpuState.Accumulator = value;
            cpuState.Status = cpuState.Status.SetFromRegister(cpuState.Accumulator);
        }

        public IInstructionEncoder Write(ushort absoluteAddress)
        {
            return new AbsoluteXOffsetAddressInstructionEncoder(this, absoluteAddress);
        }
    }
}