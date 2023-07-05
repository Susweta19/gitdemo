using TxSelenium;
using TxWebTests.HeaderUtils;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.Windows;

namespace TxWebTests
{
    public class TestManager
    {
        public const string headerExtractName = "TxWebTests.TestManager.InitHeader";
        private readonly TxWebDriver _driver;
        private WDInterface _weInterface;
        private Header _header;

        public TestManager(TxWebDriver driver)
        {
            this._driver = driver;
        }

        [TxAction("TestHeader", "The header for this test.")]
        [TxExtractOutput(headerExtractName)]
        public Header InitHeader(TestData testData, TeexmaData teexmaData)
        {
            return new Header(testData, teexmaData);
        }

        [TxAction("TxInterface", "The interface for teexma.")]
        public WDInterface GetInterface()
        {
            if (_weInterface == null)
            {
                _weInterface = new WDInterface(_driver);

            }
            return _weInterface;
        }
    }
}
