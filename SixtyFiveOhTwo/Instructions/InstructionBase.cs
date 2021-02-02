using SixtyFiveOhTwo.Components;
using SixtyFiveOhTwo.Instructions.Debugger;

namespace SixtyFiveOhTwo.Instructions
{
    public abstract class InstructionBase
    {
        public InstructionBase(byte opCode, string mnemonic)
        {
            OpCode = opCode;
            Mnemonic = mnemonic;
        }

        public byte OpCode { get; }
        public string Mnemonic { get; }

        public abstract Microcode GetExecutableMicrocode(CPU cpu);

        public abstract class Microcode
        {
            public InstructionBase Instruction { get; }

            private readonly CPU _processor;

            protected ref CPUState CPUState => ref _processor.State;

            public Microcode(InstructionBase instruction, CPU processor)
            {
                Instruction = instruction;
                _processor = processor;
            }

            protected byte ReadNextOpCodeByte()
            {
                return _processor.ReadProgramCounterByte();
            }

            protected ushort ReadNextOpCodeWord()
            {
                return _processor.ReadProgramCounterWord();
            }

            protected byte ReadByteFromBus(ushort address)
            {
                return _processor.Bus.ReadByte(address);
            }

            protected ushort ReadWordFromBus(ushort address)
            {
                return _processor.Bus.ReadWord(address);
            }

            protected void PushStack(byte value)
            {
                _processor.PushStack(value);
            }

            //Indicates that there's a stall in an opcode that isn't purely bus access
            protected void Yield()
            {
                _processor.Bus.Clock.Wait(); //Needs an extra wait in here somewhere
            }

            public abstract DebugExecutionResult Execute();
        }
    }
}
