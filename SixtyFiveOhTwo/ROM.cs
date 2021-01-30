using SixtyFiveOhTwo.Emit;
using SixtyFiveOhTwo.Instructions.Definitions.LDA;

namespace SixtyFiveOhTwo
{
    public class ROM
    {
        private readonly ILogger _logger;

        public ROM(ILogger logger)
        {
            _logger = logger;
        }

        public byte[] Bytes
        {
            get
            {
                const ushort programStartLabel = 0x0200;

                var bytes = new byte[0xFFFF];
                ProgramBuilder.Start(_logger)
                    .JMP(programStartLabel, true)
                    .AddInstruction(new LoadAZeroPageInstruction().Write(0x01))
                    .SetData(0x0001, 0xFF)
                    .Write(bytes);

                return bytes;
            }
        }
    }
}
