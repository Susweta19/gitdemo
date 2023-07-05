using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using TxSelenium.Utils;
using System.Threading;
using TxWebTests.Spreadsheet;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using System.Globalization;
using TxWebTests.Loggers;
using System.Text;
using System.IO;

namespace TxWebTests.Config
{
    public class Configuration
    {
        public string TestsFolderPath { get; private set; }
        public string ServerURL { get; private set; }
        public string Language { get; private set; }
        public IDictionary<string, string> StringsToReplace { get; private set; }
        public string FileTimePath { get; private set; }
        public string FileLogPath { get; private set; }
        public string FileFailedPath { get; private set; }
        public string FileParseErrorPath { get; private set; }
        public string FileSuccessPath { get; private set; }
        public string ReleaseNumber { get; private set; }
        public string TagName { get; private set; }
        public bool IsAdminTest { get; private set; }
        public bool IsTagVersion { get; private set; }
        public bool ScreenshotOnError { get; private set; }
        public SpreadsheetManager Spreadsheet { get; private set; }
        public bool DetailsMode { get; private set; }
        public bool VerboseMode { get; private set; }
        public XmlLogger XmlLogger { get; private set; }
        public GoogleDriveManager GoogleDrive { get; private set; }

        private readonly string _browserType;
        private readonly bool _userData;
        private readonly string _downloadDirectory;
        private string hubUrl;
        public string browserName;
        public string browserVersion;
        public string lastDatabaseName;
        public string webDriverVersion;

        public Configuration(string testsFolderPath, string serverURL, string browserType, string language = null,
                             bool screenshotOnError = false, ICollection<KeyValuePair<string, string>> stringsToReplace = null,
                            SpreadsheetConfig spreadSheet = null, bool detailsMode = false, bool verboseMode = false, bool userData = false,
                            string downloadDirectory = null, string hubUrl = null, string releaseNumber = null,
                            bool isAdminTest=true, bool IsTagVersion = false,string TagName="", string lastDatabaseName= "WriteForm")
        {
            this.TestsFolderPath = testsFolderPath;
            this.ServerURL = serverURL;
            this.Language = language;
            this._browserType = browserType;
            this.ScreenshotOnError = screenshotOnError;
            this.DetailsMode = detailsMode;
            this._userData = userData;
            this._downloadDirectory = downloadDirectory;
            this.FileSuccessPath = "Success.txt";
            this.FileFailedPath = "Failed.txt";
            this.FileTimePath = "Time.txt";
            this.FileParseErrorPath = "ParseError.txt";
            this.ReleaseNumber = releaseNumber;
            IsAdminTest = isAdminTest;
            this.hubUrl = hubUrl;
            this.lastDatabaseName = lastDatabaseName;
            this.VerboseMode = verboseMode;
            this.IsTagVersion = IsTagVersion;
            this.TagName = TagName;

            this.StringsToReplace = GenerateTagsToReplace();
            foreach (KeyValuePair<string, string> pair in stringsToReplace)
                this.StringsToReplace[pair.Key] = pair.Value;
            ReplaceTagPattern();

            foreach (KeyValuePair<string, string> pair in this.StringsToReplace)
                Console.WriteLine(pair.Key + " : " + pair.Value);
            XmlLogger = new XmlLogger(Path.GetFileName(testsFolderPath));
        }

        /// <summary>
        /// Replace tags present in the scripts 
        /// by special value
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GenerateTagsToReplace()
        {
            Dictionary<string, string> tagsToReplace = new Dictionary<string, string>
            {
                {"#CurrentDate#", DateTime.Today.ToShortDateString()},
                {"#YesterdayDate#", DateTime.Today.AddDays(-1).ToShortDateString()},
                {"#CurrentDateAndTime#", DateTime.Today.ToString()},
                {"#CurrentMonth#", DateTime.Today.Month.ToString()},
                {"#YYYY#", DateTime.Today.Year.ToString()},
                {"#HH#", DateTime.Now.Hour.ToString()},
                {"#YY#", DateTime.Today.Year.ToString().Substring(2)}

            };

            tagsToReplace.Add("#NoSpace#", "");
            tagsToReplace.Add("#Space#", " ");

            tagsToReplace.Add("#D#", DateTime.Today.Day.ToString());

            if (DateTime.Today.Day.ToString().Count() == 1)
                tagsToReplace.Add("#DD#", "0" + DateTime.Today.Day.ToString());
            else
                tagsToReplace.Add("#DD#", DateTime.Today.Day.ToString());

            if (DateTime.Today.Month.ToString().Count() == 1)
                tagsToReplace.Add("#MM#", "0" + DateTime.Today.Month.ToString());
            else
                tagsToReplace.Add("#MM#", DateTime.Today.Month.ToString());

            tagsToReplace.Add("#M#", DateTime.Today.Month.ToString());

            tagsToReplace.Add("#Month#", DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture));

            if (this._downloadDirectory != null)
                tagsToReplace.Add("#DownloadDirectory#", _downloadDirectory);
            return tagsToReplace;
        }

        /// <summary>
        /// adjust stringsToReplace if there is pattern inside
        /// </summary>
        private void ReplaceTagPattern()
        {
            List<KeyValuePair<string, string>> list = StringsToReplace.ToList();
            list.Sort((firstPair, nextPair) => { return firstPair.Value.Length.CompareTo(nextPair.Value.Length); });
            //stringsToReplace is sorted according value.length
            StringsToReplace = list.ToDictionary(pair => pair.Key, pair => pair.Value);
            //list is sorted according key.length
            list.Sort((firstPair, nextPair) => { return firstPair.Key.Length.CompareTo(nextPair.Key.Length); });
            //we start by the longest value.length in stringsToReplace
            for (int i = StringsToReplace.Count - 1; i >= 0; i--)
            {
                //we start by the shortest pair.key.length in list
                foreach (var pair in list.Where(pair => pair.Key.Length < StringsToReplace.ElementAt(i).Value.Length))
                {
                    if (StringsToReplace.ElementAt(i).Value.Contains(pair.Key))
                        this.StringsToReplace[StringsToReplace.ElementAt(i).Key] = StringsToReplace.ElementAt(i).Value.Replace(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Starts the browser to a default page.
        /// </summary>
        /// <returns>The webdriver piloting the browser</returns>
        public IWebDriver StartBrowser(int i = 1)
        {
            try
            {
                IWebDriver webDriver = GetDriver();
                try
                {
                    webDriver.Manage().Window.Size = new Size(1366, 768);
                }
                catch (Exception e)
                {               
                    Console.WriteLine(e.ToString());
                }
               
                Console.WriteLine("Webdriver Resolution is: " + webDriver.Manage().Window.Size.Width.ToString() + "x" + webDriver.Manage().Window.Size.Height.ToString());
                return webDriver;
            }
            catch (Exception e)
            {
                Console.WriteLine("StartBrowserFailed  - " + i + " Time(s)");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                if (i < 4)
                {
                    Thread.Sleep(i * 10000);
                    i++;
                    return StartBrowser(i);
                }
                else
                {
                    throw new Exception("falied after three reloading");
                }
            }
        }

        private IWebDriver GetDriver()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IWebDriver webDriver;
            if (_browserType == "InternetExplorer")
            {
                //browser = new ChromeDriver();
                InternetExplorerOptions options = new InternetExplorerOptions();
                options.Proxy = null;
                options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                options.RequireWindowFocus = true;

                if (hubUrl != null && hubUrl != "")
                {
                    DesiredCapabilities capa = (DesiredCapabilities)options.ToCapabilities();
                    webDriver = new RemoteWebDriver(new Uri(hubUrl), capa);
                    ((RemoteWebDriver)webDriver).FileDetector = new LocalFileDetector();
                }
                else
                {
                    webDriver = new InternetExplorerDriver(options);
                }
                browserName = "IE";
            }
            else if (_browserType == "Firefox")
            {
                if (!LinuxUtils.IsLinux)
                {
                    FirefoxOptions option = new FirefoxOptions() { BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe" };
                    webDriver = new FirefoxDriver(option);
                }
                else
                    webDriver = new FirefoxDriver();
                browserName = "FF";
            }
            else if (_browserType == "Edge")
            {
                webDriver = new EdgeDriver();
                browserName = "ED";
            }
            else if (_browserType == "Chrome")
            {
                ChromeOptions options = new ChromeOptions { Proxy = null };
                options.AddArgument("enable-automation");
                options.AddArgument("--disable-infobars");
                options.AddArgument("--verbose");
                //options.AddArgument("--start-maximized");
                options.SetLoggingPreference(LogType.Browser, LogLevel.Severe);
                options.SetLoggingPreference(LogType.Client, LogLevel.Off);
                options.SetLoggingPreference(LogType.Driver, LogLevel.Off);
                options.SetLoggingPreference(LogType.Profiler, LogLevel.Off);
                options.SetLoggingPreference(LogType.Server, LogLevel.Off);
                options.AcceptInsecureCertificates = true;
                if (LinuxUtils.IsLinux)
                {
                    //change download directory
                    options.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                    options.AddUserProfilePreference("download.prompt_for_download", "false");
                    options.AddUserProfilePreference("download.directory_upgrade", "true");
                    options.AddUserProfilePreference("download.default_directory", "/home/seluser/Downloads/");
                }
                if (_userData)
                    CreateAppDataChrome(ref options);

                if (hubUrl != null && hubUrl != "")
                {
                    //options.AddAdditionalCapability("timeZone", "Asia/Kolkata");
                    //DesiredCapabilities capa = (DesiredCapabilities)options.ToCapabilities();
                    //capa.SetCapability("timeZone", "Asia/Kolkata");
                    webDriver = new RemoteWebDriver(new Uri(hubUrl), options.ToCapabilities(), TimeSpan.FromSeconds(60));
                    var capabilities = ((RemoteWebDriver)webDriver).Capabilities;
                    browserVersion = (capabilities.GetCapability("chrome") as Dictionary<string, object>)["chromedriverVersion"].ToString();
                }
                else
                {
                    webDriver = new ChromeDriver(options);
                    var capabilities = ((ChromeDriver)webDriver).Capabilities;
                    var version = capabilities.GetCapability("version") ?? capabilities.GetCapability("browserVersion");
                    browserVersion = version.ToString();
                }
                browserName = "GC";
                Dictionary<string, Object> chromeDetails = (Dictionary<string, Object>)((RemoteWebDriver)webDriver).Capabilities.GetCapability("chrome");
                webDriverVersion = chromeDetails.First().Value.ToString().Split('(')[0].Trim();
                Thread.Sleep(500);
            }
            else
            {
                throw new Exception("This driver type is not supported : " + _browserType);
            }
            webDriver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 180);
            return webDriver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        private void CreateAppDataChrome(ref ChromeOptions options)
        {
            if (LinuxUtils.IsLinux)
            {
                options.AddArgument("user-data-dir=" + Environment.CurrentDirectory + "/Chrome/UserData");
                options.AddArgument("no-sandbox");
                Console.WriteLine("set user data dir");
            }
            else
                options.AddArgument("user-data-dir=" + Environment.CurrentDirectory + "\\Chrome\\UserData");
        }
    }
}