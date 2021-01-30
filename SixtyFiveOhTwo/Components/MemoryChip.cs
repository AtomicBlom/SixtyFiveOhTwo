using System.Collections.Generic;
using System.Threading;

namespace SixtyFiveOhTwo.Components
{
    public class MemoryChip
    {
        private readonly int MAX_MEMORY = 0xFFFF;
        private readonly Bus _bus;
        private readonly CancellationToken _finishToken;
        private readonly byte[] _memory;


        public MemoryChip(Bus bus, CancellationToken finishToken, IReadOnlyList<byte> initialBytes = null)
        {
            _bus = bus;
            _finishToken = finishToken;
            _memory = new byte[MAX_MEMORY];
            if (initialBytes is not null)
            {
                for (var i = 0; i < initialBytes.Count; i++)
                {
                    _memory[i] = initialBytes[i];
                }
            }
        }

        public void Run()
        {
            while (!_finishToken.IsCancellationRequested)
            {
                _bus.Clock.WaitFalling();
                switch (_bus.AccessMode)
                {
                    case BusAccessMode.Read:
                        _bus.Data = _memory[_bus.Address];
                        break;
                    case BusAccessMode.Write:
                        _memory[_bus.Address] = _bus.Data;
                        break;
                }
            }
        }
    }
}