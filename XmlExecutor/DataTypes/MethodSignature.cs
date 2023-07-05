using System.Reflection;

namespace XmlExecutor.DataTypes
{
    public class MethodSignature
    {
        public string Name { get; private set; }

        public string Comment { get; private set; }

        public MethodInfo Info { get; private set; }

        public MethodSignature(string name, string comment, MethodInfo methodInfo)
        {
            Info = methodInfo;
            Name = name;
            Comment = comment;
        }

    }
}
