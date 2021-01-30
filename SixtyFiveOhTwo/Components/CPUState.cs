namespace SixtyFiveOhTwo.Components
{
    public struct CPUState
    {
        public ushort ProgramCounter;
        public byte StackPointer;

        public byte Accumulator;
        public byte IndexRegisterX;
        public byte IndexRegisterY;

        public ProcessorStatus Status;
    }
}