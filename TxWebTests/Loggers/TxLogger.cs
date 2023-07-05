using System;
using System.Collections.Generic;
using System.IO;
using TxWebTests.Interfaces;
using XmlExecutor.DataTypes;

namespace TxWebTests.Loggers
{
    public class TxLogger : IWebTestsLogger, IDisposable
    {
        private int _nextId = 1;
        private readonly Stack<int> _ids = new Stack<int>();
        private readonly StreamWriter _fileWriter;

        public TxLogger(string filePath)
        {
            _fileWriter = new StreamWriter(filePath);
            _fileWriter.WriteLine("ID	Date	Deepness	Action	Text	Duration*	Errors*	Warnings*");
        }

        public void LogBefore(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            string name = signature.Name;
            int depth = callingTree.Depth + 1;
            _ids.Push(_nextId);
            DateTime now = DateTime.Now;
            _fileWriter.WriteLine(_nextId + "\t" + now.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + depth + "\t" + "Start" + "\t" + name);
            _nextId++;
        }

        public void LogAfter(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            string name = signature.Name;
            int depth = callingTree.Depth + 1;
            int id = _ids.Pop();
            DateTime now = DateTime.Now;
            _fileWriter.WriteLine(id + "\t" + now.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + depth + "\t" + "End" + "\t\t0\t0\t0");
        }

        public void LogError(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception e)
        {
            string error = e.ToString();
            Console.WriteLine(error);
            int depth = callingTree.Depth + 1;

            foreach (var messageLine in e.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string now = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                _fileWriter.WriteLine(_nextId + "\t" + now + "\t" + depth + "\t" + "Start" + "\t" + messageLine);
                _fileWriter.WriteLine(_nextId + "\t" + now + "\t" + depth + "\t" + "End" + "\t\t0\t0\t0");
                _nextId++;
            }
        }

        public void Dispose()
        {
            _fileWriter.Dispose();
        }
    }

}
