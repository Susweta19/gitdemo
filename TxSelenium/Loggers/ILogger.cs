using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxSelenium.Loggers
{
    public enum LogLevel {TRACE, INFO, DEBUG, WARNING, ERROR};
    public interface ILogger
    {
        void Log(string message);

        LogLevel GetMinLevel();
    }
}
