using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxSelenium.NativeTests.Readable;
using TxSelenium.NativeTests.WebElements;
using TxSelenium.NativeTests.Windows;
using XmlExecutor.Attributes;

namespace Lafarge
{
    public class AHRForm: RForm
    {
        public AHRForm(RForm baseRForm)
            : base(baseRForm.GetDriver(), baseRForm.ElemIdentifier, baseRForm.Parent) { }

        [TxOverload(typeof(RForm), typeof(AHRForm))]
        public static object Overload(object baseInstance)
        {
            return new AHRForm(baseInstance as RForm);
        }

        [TxAction("IsLinkExist", "")]
        public bool IsLinkExist(int attrId,string linkName)
        {
            //WERefreshed elem= GetDriver().WaitForElement(By.XPath(".//*[starts-with(@id,'idDivLinksdiv" + attrId + "')]/a[text()='" + linkName + "']"),this);
            return GetDriver().IsElementPresent(By.XPath(".//*[starts-with(@id,'idDivLinksdiv"+attrId+"')]/a[text()='"+linkName+"']"));
        }

        [TxAction("StatusInitial", "")]
        public string StatusInitial(string objectName)
        {
            return objectName.Substring(0,objectName.LastIndexOf(']')+1);
        }
    }
}
