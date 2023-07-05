using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;
using XmlExecutor.Interfaces;

namespace Example
{
    public class OverloadExample: WDMainWindow
    {
        public OverloadExample(WDMainWindow baseWindow)
            : base(baseWindow.GetDriver(), baseWindow.ElemIdentifier, baseWindow.Parent) {}

        [TxOverload(typeof(WDMainWindow), typeof(OverloadExample))]
        public static object Overload(object baseInstance)
        {
            return new OverloadExample(baseInstance as WDMainWindow);
        }

        [TxAction("DoNothing", "This does nothing")]
        public void DoNothing()
        {

        }

        [TxAction("ReturnHello", "This returns the string hello")]
        public String ReturnHello()
        {
            return "hello";
        }

        [TxAction("Repeater", "This returns the string passed as a parameter")]
        public String Repeater(String input)
        {
            return input;
        }

    }
}
