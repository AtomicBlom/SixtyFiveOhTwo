using System;

namespace SixtyFiveOhTwo.Instructions.Debugger
{
    [Flags]
    public enum UsedFields
    {
        Address = 1 << 0,
        PointerToAddress = 1 << 1,
        ZeroPageOffset = 1 << 2,
        X = 1 << 3,
        Y = 1 << 4,
        Value = 1 << 5
    }
}