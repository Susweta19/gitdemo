using XmlExecutor;
using XmlExecutor.Attributes;
using XmlExecutor.Factories;

namespace TxWebTests.Config
{
    public class ConfigManager
    {

        private static ConfigManager _configManager = null;

        public static Configuration GetConfig(AttributeUtils utils, string configPath)
        {
            if (_configManager == null)
            {
                _configManager = new ConfigManager();
                ActionsParser parser = new ActionsParser(utils, typeof(ConfigManager));
                ActionsExecutor executor = parser.ParseFile(configPath);
                executor.Execute(_configManager);
            }
            return _configManager.Config;
        }

        public Configuration Config { get; private set; }

        private ConfigManager() { }

        [TxAction("Config", "Fills the configuration parameters")]
        public void GenerateConfig(Configuration config)
        {
            Config = config;
        }
    }
}
