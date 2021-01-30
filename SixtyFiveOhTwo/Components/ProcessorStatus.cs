using System;

namespace SixtyFiveOhTwo.Components
{
    [Flags]
    public enum ProcessorStatus
    {
        CarryFlag = 1 << 0,
        ZeroFlag = 1 << 1,
        InterruptDisable = 1 << 2,
        DecimalMode = 1 << 3,
        BreakCommand = 1 << 4,
        OverflowFlag = 1 << 6,
        NegativeFlag = 1 << 7
    }
}