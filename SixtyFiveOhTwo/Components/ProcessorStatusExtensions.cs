﻿namespace SixtyFiveOhTwo.Components
{
    public static class ProcessorStatusExtensions
    {
        public static ProcessorStatus SetNumericFlags(this ProcessorStatus status, byte register)
        {
            var result = status;
            result = result.SetFlag(ProcessorStatus.NegativeFlag, (register & 1 << 7) != 0);
            result = result.SetFlag(ProcessorStatus.ZeroFlag, (register == 0));
            return result;
        }

        public static ProcessorStatus SetFlag(this ProcessorStatus a, ProcessorStatus b)
        {
            return a | b;
        }

        public static ProcessorStatus SetFlag(this ProcessorStatus a, ProcessorStatus b, bool isSet)
        {
            return isSet 
                ? (a | b) 
                : (a & (~b));
        }

        public static ProcessorStatus UnsetFlag(this ProcessorStatus a, ProcessorStatus b)
        {
            return a & (~b);
        }

        public static ProcessorStatus ToogleFlag(this ProcessorStatus a, ProcessorStatus b)
        {
            return a ^ b;
        }


    }
}