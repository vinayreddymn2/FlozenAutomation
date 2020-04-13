using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace FlozenAutomation
{
    public class FlozenDriver
    {
        private IWebDriver driver;

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
            catch (NullReferenceException)
            {
                Logger.Error("Test failed but was unable to get webdriver screenshot.");
            }
            catch (UnhandledAlertException)
            {
                Logger.Error("Test failed but was unable to get webdriver screenshot.");
            }

            return null;
        }

        public void Launch()
        {
            switch (FlozenConfig.Browser)
        }
    }
}
