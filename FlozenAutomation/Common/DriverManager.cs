namespace FlozenAutomation.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using Serilog;

    public class DriverManager
    {
        private IWebDriver driver;

        public String CurrentWorkingFolder { get; set; }

        public IWebDriver Driver
        {
            get { return this.driver; }
        }

        public void WindowMaximize()
        {
            this.driver.Manage().Window.Maximize();
        }

        public Screenshot TakeScreenshot()
        {
            try
            {
                return ((ITakesScreenshot)this.driver).GetScreenshot();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while taking screenshot");
            }

            return null;
        }

        public string CaptureScreenshot(string screenShotName, long testRunNum=0)
        {
            var ssPath = "";
            try
            {
                var ssFolder = ConfigManager.ScreenshotsFolder; 
                if (testRunNum > 0)
                    ssFolder += "\\" + testRunNum;
                ConfigManager.CreateFolder(ssFolder);

                Screenshot screenshot = TakeScreenshot();
                ssPath = new Uri(ssFolder + "\\" + screenShotName + ".png").LocalPath;
                screenshot.SaveAsFile(ssPath);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ssPath;
        }

        public void Init()
        {
            switch (ConfigManager.Browser)
            {
                case BrowserType.Chrome:
                    try
                    {
                        KillDriverProcessess();
                        string chromeVersion = UtilManager.ChromeVersion();
                        if (chromeVersion.StartsWith("68") || chromeVersion.StartsWith("69"))
                        {
                            this.driver = new ChromeDriver(ConfigManager.ChromeDriverArchivePath);
                        }
                        else 
                        {
                            this.driver = new ChromeDriver(ConfigManager.ChromeDriverPath);

                        }
                    } 
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.StackTrace);
                        throw ex;
                    }
                    break;
                case BrowserType.Firefox:
                    this.driver = new FirefoxDriver();
                    break;
            }

            //this.driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ConfigManager.PageLoadTimeOut);
            //this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigManager.ImplicitWaitTime);
            this.WindowMaximize();
        }

        public void OpenApp()
        {
            try
            {
                this.driver.Url = ConfigManager.AppUrl;
                //Log.Information("Launching URL " + ConfigManager.AppUrl);
            }
            catch (Exception ex)
            {
                //Log.Error("An error occurred while launching application {AppURL}", ConfigManager.AppUrl);
                throw ex;
            }
        }

        public void OpenUrl(String Url)
        {
            this.driver.Navigate().GoToUrl(Url);
        }

        public void Close()
        {
            if (this.driver != null)
            {
                this.driver.Quit();
            }
        }

        public void StartLogging(Int64 testRunNum, string testName)
        {
            if (ConfigManager.EnableLogging)
            {
                var folder = ConfigManager.LogsFolder + "\\" + testRunNum;
                ConfigManager.CreateFolder(folder);
                var logFile = folder + "\\" + testName + ".txt"; //ConfigManager.LogFileName;
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File(logFile)
                    .CreateLogger();
            }
        }

        public void StopLogging()
        {
            if (ConfigManager.EnableLogging)
            {
                Log.CloseAndFlush();
            }
        }

        private void KillDriverProcessess()
        {
            try
            {
                var processes = Process.GetProcesses().
                     Where(pr => pr.ProcessName == "chromedriver");

                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
