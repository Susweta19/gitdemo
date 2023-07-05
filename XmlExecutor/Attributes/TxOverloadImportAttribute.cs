using System;
using System.Composition;
using XmlExecutor.Interfaces;

namespace XmlExecutor.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    class TxOverloadImportAttribute: ImportManyAttribute
    {
        public TxOverloadImportAttribute(): base(typeof(IOverload).FullName) {}
    }
}
