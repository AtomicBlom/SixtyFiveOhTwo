using System.Text;
using Xunit.Abstractions;

namespace SixtyFiveOhTwo.Tests.Util
{
    public class TestOutputLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private StringBuilder _lineBuilder;

        public TestOutputLogger(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
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
            _testOutputHelper.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            if (_lineBuilder is not null)
            {
                _lineBuilder.Append(string.Format(format, args));
                var message = _lineBuilder.ToString();
                _lineBuilder = null;
                _testOutputHelper.WriteLine(message);
            }
            else
            {
                _testOutputHelper.WriteLine(format, args);
            }
        }
    }
}