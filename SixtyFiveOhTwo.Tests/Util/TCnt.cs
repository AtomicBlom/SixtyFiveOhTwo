namespace SixtyFiveOhTwo.Tests.Util
{
    /// <summary>
    /// These values come from the per-opcode t-cnt documented at
    /// https://www.csh.rit.edu/~moffitt/docs/6502.html
    /// </summary>
    public enum TCnt
    {
        GracefulExit = 1,
        PageBoundaryPenalty = 1,

        INY = 2,
        
        JMP_Absolute = 3,
        JMP_Indirect = 5,

        JSR_Absolute = 6,

        NOP_Implied = 2,

        LDA_Immediate = 2,
        LDA_ZeroPage  = 3,
        LDA_ZeroPageX = 4,
        LDA_Absolute  = 4,
        LDA_AbsoluteX = 4, //WithPenalty
        LDA_AbsoluteY = 4, //WithPenalty
        LDA_IndirectX = 6,
        LDA_IndirectY = 5, //WithPenalty

        LDX_Immediate = 2,
        LDX_ZeroPage  = 3,
        LDX_ZeroPageY = 4,
        LDX_Absolute  = 4,
        LDX_AbsoluteY = 4, //WithPenalty

        LDY_Immediate = 2,
        LDY_ZeroPage = 3,
        LDY_ZeroPageX = 4,
        LDY_Absolute = 4,
        LDY_AbsoluteX = 4, //WithPenalty
    }
}