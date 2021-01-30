using System;
using System.Linq;

namespace SixtyFiveOhTwo.Instructions
{
    public class InstructionSet
    {
        public readonly IInstruction[] Instructions = new IInstruction[byte.MaxValue + 1];

        public InstructionSet()
        {
            var instructions =
                typeof(InstructionSet)
                    .Assembly
                    .DefinedTypes
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => t.IsAssignableTo(typeof(IInstruction)))
                    .Select(Activator.CreateInstance)
                    .Cast<IInstruction>();

            foreach (var instruction in instructions)
            {
                if (Instructions[instruction.OpCode] != null)
                {
                    throw new Exception($"Opcode {instruction.OpCode} collides between {instruction.GetType().Name} and {Instructions[instruction.OpCode].GetType().Name}");
                }
                Instructions[instruction.OpCode] = instruction;
            }
        }
    }
}
