namespace TxWebTests.HeaderUtils
{
    public class TeexmaData
    {
        public string CustomerResPath { get; private set; }
        public string TeexmaVersion { get; private set; }
        public string DatabasePath { get; private set; }

        public TeexmaData(string customerResPath, string teexmaVersion, string databasePath)
        {
            CustomerResPath = customerResPath;
            TeexmaVersion = teexmaVersion;
            DatabasePath = databasePath;
        }
    }
}
