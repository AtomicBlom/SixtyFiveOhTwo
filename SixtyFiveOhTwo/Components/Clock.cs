using System;
using System.Threading;

namespace SixtyFiveOhTwo.Components
{
    public class Clock : IClock
    {

        private static ManualResetEvent _risingEdge = new(false);
        private static ManualResetEvent _fallingEdge = new(false);
        
        public TimeSpan cpuTime = TimeSpan.FromMilliseconds(100);
        private bool _running = true;

        private long _ticks = 0;
        public long Ticks => Interlocked.Read(ref _ticks);

        public void Wait()
        {
            _risingEdge.WaitOne();
        }

        public void WaitFalling()
        {
            _fallingEdge.WaitOne();
        }

        public void Run()
        {
            Thread.Sleep(cpuTime);
            while (_running)
            {
                var delay = cpuTime / 2;
                Thread.Sleep(delay);
                Interlocked.Increment(ref _ticks);
                _risingEdge.Set();
                _risingEdge.Reset();
                Thread.Sleep(delay);
                _fallingEdge.Set();
                _fallingEdge.Reset();
            }

        }

        public void Stop()
        {
            _running = false;
        }

 
    }
}