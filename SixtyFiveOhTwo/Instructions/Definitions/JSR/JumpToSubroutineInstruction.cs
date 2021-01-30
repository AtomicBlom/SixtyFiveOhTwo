﻿using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Encoding;
using SixtyFiveOhTwo.Util;

namespace SixtyFiveOhTwo.Instructions.Definitions.JSR
{
    //Logic:
    //t = PC - 1
    //bPoke(SP, t.h)
    //SP = SP - 1
    //bPoke(SP, t.l)
    //SP = SP - 1
    //PC = $A5B6
    public sealed class JumpToSubroutineInstruction : IInstruction
    {
        public byte OpCode => 0x20;
        public string Mnemonic => "JSR";

        public void Execute(CPU cpu)
        {
            ref var cpuState = ref cpu.State;
            var t = (ushort)(cpuState.ProgramCounter - 1);
            
            cpu.PushStack(t.HighOrderByte());
            cpu.PushStack(t.LowOrderByte());

            cpuState.ProgramCounter = cpu.ReadProgramCounterWord();
        }

        public IInstructionEncoder Write(ushort value)
        {
            return new AbsoluteAddressInstructionEncoder(this, value);
        }
    }
}