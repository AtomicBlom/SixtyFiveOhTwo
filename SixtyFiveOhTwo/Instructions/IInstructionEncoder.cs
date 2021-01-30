namespace SixtyFiveOhTwo.Instructions
{
    public interface IInstructionEncoder
    {
        void Write(ref ushort address, byte[] memory);
        string ToStringMnemonic();
    }
}