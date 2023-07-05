using OpenQA.Selenium;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDFrameSMC : WERefreshed
    {
        public WDFrameSMC(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupName"></param>
        [TxAction("Name", "Fill input with the groupName")]
        public void Name(string groupName)
        {
            new WEGInput(GetDriver(), By.Id("input_name_gc"), this).Fill(groupName);//input_name_gc
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        [TxAction("AggregationFunction", "Select a aggregation function")]
        public void AggrFunc(string functionName)
        {
            new WEGSelect(GetDriver(), By.Id("select_type_gc"), this).SelectByText(functionName);
        }
    }
}
