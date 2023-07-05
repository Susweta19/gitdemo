using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxMCS
{
    public class GTAdvanced : WERefreshed
    {
        By agregation = By.XPath(".//select[@name=\"sAggregation\"]");
        By absenceOfData = By.XPath(".//select[@name=\"sEmptyDataTreatment\"]");
        By presenceOfData = By.XPath(".//select[@name=\"sDataTreatment\"]");
        public GTAdvanced(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null)
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("Agregation", "")]
        public WEGSelect Agregation() {
            return new WEGSelect(GetDriver(),agregation,this);
        }

        [TxAction("AbsenceOfData", "")]
        public WEGSelect AbsenceOfData()
        {
            return new WEGSelect(GetDriver(), absenceOfData, this);
        }

        [TxAction("PresenceOfData", "")]
        public WEGSelect PresenceOfData()
        {
            return new WEGSelect(GetDriver(), presenceOfData, this);
        }
    }
}
