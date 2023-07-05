using System;

namespace XmlExecutor.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class TxGenericAttribute : Attribute
    {
        public bool AllowInterfaces { get; private set; }

        public TxGenericAttribute(bool allowInterfaces)
        {
            AllowInterfaces = allowInterfaces;
        }
    }
}
