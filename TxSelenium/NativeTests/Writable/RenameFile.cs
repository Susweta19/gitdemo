using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.NativeTests.Writable
{
    public class RenameFile : WERefreshed
    {
        public RenameFile(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("FinalName", "")]
        public WEGInput FinalName(int index = 1)
        {
            return new WEGInput(GetDriver(), By.XPath(".//input[contains(@id,\"idTextBaseName\")]"),this);
        }
    }
}
