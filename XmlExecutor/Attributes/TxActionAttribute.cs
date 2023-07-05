using System;

namespace XmlExecutor.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class TxActionAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Comment { get; private set; }

        public TxActionAttribute(string name, string comment) 
        {
            this.Name = name;
            this.Comment = comment;
        }
    }
}
