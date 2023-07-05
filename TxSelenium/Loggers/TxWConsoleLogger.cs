using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxSelenium.Loggers
{
    public class TxWConsoleLogger : ILogger
    {
        private LogLevel _minimumLevel;

        public TxWConsoleLogger(LogLevel minimumLevel)
        {
            _minimumLevel = minimumLevel;
        }

        public LogLevel GetMinLevel()
        {
            return _minimumLevel;
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
