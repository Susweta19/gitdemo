using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TxSelenium.NativeTests.WebElements;
using XmlExecutor.Attributes;

namespace TxSelenium.GenericTests.TxVisualDesign
{
    public class GTSettings : WERefreshed
    {
        public GTSettings(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null, By frameBy = null) 
            : base(driver, elemIdentifier, parent, frameBy)
        {
        }

        [TxAction("KeepThePositions", "")]
        public void KeepThePositions()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id=\"idVDKeepLocation\"]"), this);
        }


        [TxAction("DirectionOfAutomaticPositioning", "")]
        public void DirectionOfAutomaticPositioning(string value)
        {
            GetDriver().ClickAt(By.Id("idVDLayoutAngle"), this);
            GetDriver().ClickAt(By.XPath(".//option[text()=\"" + value + "\"]"), this);
            //return new WEGDHtmlxComboBox(GetDriver(), By.Id("idVDLayoutAngle"), this);
        }

        [TxAction("DisplayFreeShapes", "")]
        public void DisplayFreeShapes()
        {
            GetDriver().ClickAt(By.XPath(".//input[@id=\"idVDDisplayFreeShape\"]"), this);
        }

        [TxAction("Figure", "")]
        public void Figure(string RowNumber, string Value)
        {
            new WEGInput(GetDriver(), By.XPath(".//*[@id=\"txVDInspector\"]//tr[" + RowNumber + "]//input"), this).Fill(Value);
        }

        [TxAction("AllowEditionFieldsInDesignMode", "")]
        public void AllowEditionFieldsInDesignMode()
        {
            GetDriver().ClickAt(By.XPath(".//*[starts-with(@id,\"idChEditableField\")]"), this);
        }

        [TxAction("Form", "RowNumber starts from 1")]
        public WEGDHtmlxComboBox Form(string RowNumber)
        {
            GetDriver().ClickAt(By.XPath(".//table/tbody/tr[" + RowNumber + "]//select"), this);
            return new WEGDHtmlxComboBox(GetDriver(), By.Name("shapefigure"), this);
        }

        [TxAction("DepthOfPositioning", "RowNumber starts from 1")]
        public WEGDHtmlxComboBox DepthOfPositioning(string RowNumber)
        {
            GetDriver().ClickAt(By.XPath(".//table/tbody/tr[" + RowNumber + "]//select"), this);
            return new WEGDHtmlxComboBox(GetDriver(), By.Name("nodeLayerName"), this);
        }
    }
}
