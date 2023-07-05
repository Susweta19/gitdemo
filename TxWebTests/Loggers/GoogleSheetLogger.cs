using System;
using System.Collections.Generic;
using System.Diagnostics;
using TxWebTests.Interfaces;
using TxWebTests.Spreadsheet;
using XmlExecutor.DataTypes;

namespace TxWebTests.Loggers
{
    public class GoogleSheetLogger : IWebTestsLogger
    {
        private readonly SpreadsheetManager _sheetManager;
        private Stopwatch _watch;

        public GoogleSheetLogger(SpreadsheetManager sheetManager)
        {
            this._sheetManager = sheetManager;
        }

        public void LogAfter(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            _watch.Stop();
            double seconds = TimeSpan.FromMilliseconds(_watch.ElapsedMilliseconds).TotalSeconds;

            if (_sheetManager.FuncTimer.ContainsKey(signature.Name))
            {
                _sheetManager.FuncTimer[signature.Name].Add(seconds);
            }
            else
            {
                _sheetManager.FuncTimer[signature.Name] = new List<double>() { seconds };
            }
        }

        public void LogBefore(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            _watch = Stopwatch.StartNew();
            _sheetManager.ReportScenarioAction(callingTree, signature);
        }

        public void LogError(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception e)
        {
            //_sheetManager.ReportException(e);
        }
    }
}
