using System;
using System.Composition;
using XmlExecutor.Interfaces;

namespace XmlExecutor.Attributes
{    
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class TxOverloadAttribute : ExportAttribute, IOverloadMetadata
    {
        public Type BaseType { get; private set; }
        public Type NewType { get; private set; }

        public TxOverloadAttribute(Type baseType, Type newType) : base(typeof(IOverload))
        {
            BaseType = baseType;
            NewType = newType;
        }
    }
}
