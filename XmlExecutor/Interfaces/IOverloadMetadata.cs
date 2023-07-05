using System;

namespace XmlExecutor.Interfaces
{
    public interface IOverloadMetadata
    {
        Type BaseType { get; }
        Type NewType { get; }
    }
}
