using System.Collections.Generic;
using System.Linq;
using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Debugger;

namespace SixtyFiveOhTwo.Tests.Util
{
    public static class ExecutionResultExtensions
    {
        public static (int TickCount, ushort ProgramCounter) ExecutionStats(this DebugExecutionResult[] results, Dictionary<byte, InstructionBase> instructionSetLookup, int expectedPageBoundaryPenalties = 0)
        {
            var syncTicks = 1;

            var tCount = results.Aggregate(syncTicks + expectedPageBoundaryPenalties, (i, result) => i + instructionSetLookup[result.OpCode].TCnt);
            ushort pc = 0;
            var index = 0;
            while (index < results.Length)
            {
                if (IsJumpOrGoSub(results[index]))
                {
                    pc += results[index].CPUState.ProgramCounter;
                    break;
                }

                pc += instructionSetLookup[results[index].OpCode].ILen;
                index++;

                if (index == results.Length)
                {
                    //We reached the start of the debug loop
                    pc += CPU.ResetVectorAddressLow;
                    break;
                }
            }

            return (tCount, pc);
        }

        private static bool IsJumpOrGoSub(DebugExecutionResult der)
        {
            return der.OpCode == 0x4C || der.OpCode == 0x6C || der.OpCode == 0x20;
        }
    }
}