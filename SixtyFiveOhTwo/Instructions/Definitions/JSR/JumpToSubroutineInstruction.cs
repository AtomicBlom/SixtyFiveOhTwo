using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Encoding;
using SixtyFiveOhTwo.Util;

namespace SixtyFiveOhTwo.Instructions.Definitions.JSR
{
    public sealed class JumpToSubroutineInstruction : IInstruction
    {
        public byte OpCode => 0x20;
        public string Mnemonic => "JSR";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;
            var t = cpuState.ProgramCounter.Offset(-1);
            
            cpu.PushStack(t.HighOrderByte());
            cpu.PushStack(t.LowOrderByte());

            cpuState.ProgramCounter = cpu.ReadProgramCounterWord();
            cpu.Bus.Clock.Wait(); //Needs an extra wait in here somewhere
        }

        public IInstructionEncoder Write(ushort value)
        {
            return new AbsoluteAddressInstructionEncoder(this, value);
        }
    }
}
