using System.Linq;

namespace SixtyFiveOhTwo.Tests.Util
{
    public static class Timings
    {
        public static int For(params TCnt[] expectedOpCodes)
        {
            //FIXME: Remove hack
            const int initialCPUWait = 1;

            return expectedOpCodes.Aggregate(initialCPUWait, (t, instruction) => t + (int) instruction);
        }
    }
}
