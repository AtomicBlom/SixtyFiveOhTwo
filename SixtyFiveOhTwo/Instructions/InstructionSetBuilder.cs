using System;
using System.Collections.Generic;
using System.Linq;
using SixtyFiveOhTwo.Components;

namespace SixtyFiveOhTwo.Instructions
{
    public static class InstructionSetBuilder
    {
        public static List<InstructionBase> CreateInstructionSet()
        {
            var instructions =
                typeof(InstructionSetBuilder)
                    .Assembly
                    .DefinedTypes
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => t.IsAssignableTo(typeof(InstructionBase)))
                    .Select(Activator.CreateInstance)
                    .Cast<InstructionBase>()
                    .ToList();

            return instructions;
        }

        public static InstructionBase.Microcode[] BuildMicrocodeArray(CPU cpu, IEnumerable<InstructionBase> instructions)
        {
            var microcode = new InstructionBase.Microcode[byte.MaxValue + 1];
            foreach (var instruction in instructions)
            {
                microcode[instruction.OpCode] = instruction.GetExecutableMicrocode(cpu);
            }

            return microcode;
        }
    }
}
