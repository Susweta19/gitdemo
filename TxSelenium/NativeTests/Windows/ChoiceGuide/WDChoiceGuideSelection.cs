using OpenQA.Selenium;
using XmlExecutor.Attributes;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Windows
{
    public class WDChoiceGuideSelection : WERefreshed
    {
        public WDChoiceGuideSelection(TxWebDriver driver, By elemIdentifier, WERefreshed parent = null)
            : base(driver, elemIdentifier, parent)
        {
        }

        [TxAction("ChoiceGuideFromTemplate", "Open athe choice guide and selcet a template.")]
        public WDFramedWindow<WDChoiceGuide> Template(string template = null)
        {

            new WEGSelect(GetDriver(), By.ClassName("comboChoixGDC"), this).SelectByText(template);
            GetDriver().ClickAt(By.Id("btn_open_gdc"), this);

            WDChoiceGuide choiceGuide = new WDChoiceGuide(GetDriver(), By.TagName("body"));
            return new WDFramedWindow<WDChoiceGuide>(GetDriver(), choiceGuide, null, WDFramedWindow<WERefreshed>.frameWindowBy);
        }
        [TxAction("ChoiceGuideFrom", "")]
        public WDChoiceGuide ChoiceGuideFrom()
        {
            return new WDChoiceGuide(GetDriver(), By.ClassName("dhxwin_active"),this);
        }

        [TxAction("ChoiceGuideFromXML", "Click on Browse button")]

        public WDFramedWindow<WDChoiceGuide> Browse(string pathFile)
        {
            WERefreshed button = GetDriver().WaitForElement(By.XPath(".//input[@id=\"idBtnUploadChoiceGuide\"]"), this);
            GetDriver().UploadFile(button, pathFile);
            WDChoiceGuide choiceGuide = new WDChoiceGuide(GetDriver(), By.TagName("body"));
            return new WDFramedWindow<WDChoiceGuide>(GetDriver(), choiceGuide, null, WDFramedWindow<WERefreshed>.frameWindowBy);

        }
        [TxAction("ReadTitle", "Click on Browse button")]
        public string ReadTitle()
        {
            WERefreshed elem = new WERefreshed(GetDriver(), By.Id("titre"),null, WDFramedWindow<WERefreshed>.frameWindowBy);
            return elem.GetElement().Text;
           
        }
    }
}