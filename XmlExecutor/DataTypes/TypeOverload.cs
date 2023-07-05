using System;

namespace XmlExecutor.DataTypes
{
    internal class TypeOverload
    {
        public Type OriginType { get; private set; }
        public Type NewType { get; private set; }
        public Func<object, object> Overloader { get; private set; }

        public TypeOverload(Type originType, Type newType, Func<object, object> overloader)
        {
            OriginType = originType;
            NewType = newType;
            Overloader = overloader;
        }
    }
}
