namespace XmlExecutor.DataTypes
{
    public class MethodCall
    {
        public string Name { get; private set; }
        public ParametersValues CallValues { get; private set; }
        public string Message { get; private set; }

        public MethodCall(string name, ParametersValues callValues, string message = null)
        {
            Name = name;
            CallValues = callValues;
            Message = message;
        }
    }
}
