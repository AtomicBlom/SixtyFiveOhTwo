using SixtyFiveOhTwo.Components;

namespace SixtyFiveOhTwo
{
    public interface IBus
    {
        BusAccessMode AccessMode { get; set; }
        IClock Clock { get; }
        ushort Address { get; set; }
        byte Data { get; set; }
    }
}