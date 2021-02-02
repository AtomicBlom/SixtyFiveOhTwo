namespace SixtyFiveOhTwo.Instructions
{
    public interface IParameterInstruction<in T>
    {
        public IInstructionEncoder GetEncoder(T value);
    }
}