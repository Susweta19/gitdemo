using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxSelenium.Loggers
{
    public class LogsService
    { 
        private static IList<ILogger> _loggers = new List<ILogger>();

        public static void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public static void Log(string message, LogLevel level)
        {
            foreach(ILogger logger in _loggers)
            {
                if (logger.GetMinLevel().CompareTo(level) >= 0)
                {
                    logger.Log(message);
                }
            }
        }

    }
}
