using System;
using System.Collections.Generic;
using System.Diagnostics;
using TxWebTests.Interfaces;
using TxWebTests.Spreadsheet;
using XmlExecutor.DataTypes;

namespace TxWebTests.Loggers
{
    public class ConsoleLogger : IWebTestsLogger
    {

        public void LogAfter(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            Console.WriteLine(DateTime.Now.ToString() + " After : " + signature.Name);
        }

        public void LogBefore(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            Console.WriteLine(DateTime.Now.ToString() + " Before : " + signature.Name);
        }

        public void LogError(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception e)
        {
            Console.WriteLine(DateTime.Now.ToString() + " Error on : " + signature.Name);
            Console.WriteLine(e);
        }
    }
}
