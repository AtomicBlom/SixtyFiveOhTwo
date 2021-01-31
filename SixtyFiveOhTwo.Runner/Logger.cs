using System;
using System.Text;

namespace SixtyFiveOhTwo
{
    public class Logger : ILogger
    {
        private StringBuilder _lineBuilder;

        public void Write(string message)
        {
            _lineBuilder ??= new StringBuilder();
            _lineBuilder.Append(message);
        }

        public void WriteLine(string message)
        {
            if (_lineBuilder is not null)
            {
                _lineBuilder.Append(message);
                message = _lineBuilder.ToString();
                _lineBuilder = null;
            }
            Console.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            if (_lineBuilder is not null)
            {
                _lineBuilder.Append(string.Format(format, args));
                var message = _lineBuilder.ToString();
                _lineBuilder = null;
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(format, args);
            }
        }
    }
}
