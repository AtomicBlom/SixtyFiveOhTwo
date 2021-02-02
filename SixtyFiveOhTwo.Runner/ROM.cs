using System.Collections.Generic;
using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions;
using SixtyFiveOhTwo.Instructions.Definitions.LDA;

namespace SixtyFiveOhTwo.Runner
{
    public class ROM
    {
        private readonly IEnumerable<InstructionBase> _instructionSet;
        private readonly ILogger _logger;

        public ROM(IEnumerable<InstructionBase> instructionSet, ILogger logger)
        {
            _instructionSet = instructionSet;
            _logger = logger;
        }

        public byte[] Bytes
        {
            get
            {
                const ushort programStartLabel = 0x0200;

                var bytes = new byte[0xFFFF];
                ProgramBuilder.Start(_instructionSet, _logger)
                    .JMP(programStartLabel, true)
                    .AddInstruction<LoadAZeroPageInstruction>(0x01)
                    .SetData(0x0001, 0xFF)
                    .Write(bytes);

                return bytes;
            }
        }
    }
}
