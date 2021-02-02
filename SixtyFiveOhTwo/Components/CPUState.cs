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

        public override string ToString()
        {
            return $"PC: {ProgramCounter:X4}, SP: {StackPointer:X2}, A: {Accumulator:X2}, X: {IndexRegisterX:X2}, Y: {IndexRegisterY:X2}, P: {Status}";
        }
    }
}