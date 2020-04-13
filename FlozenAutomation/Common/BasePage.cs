namespace FlozenAutomation.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using FlozenAutomation.Extensions;
    using Serilog;

    public class BasePage
    {
        protected DriverManager DriverManager;
        protected IWebDriver Driver;

        protected BaseElement 
            LoadingDialog = new BaseElement("Xpath", "//div[@id='loading-overlay']"),
            ProgressBar = new BaseElement("Xpath", "//div[@id='loadingbar']");
        
        public BasePage(DriverManager driverManager)
        {
            this.DriverManager = driverManager;
            this.Driver = driverManager.Driver;
        }

        protected IWebElement GetElement(BaseElement baseElement)
        {
            return this.Driver.GetElement(baseElement);
        }

        protected IWebElement ScrollToElement(BaseElement baseElement)
        {
            return this.Driver.ScrollToElement(baseElement);
        }

        protected IWebElement ScrollToElement(IWebElement element)
        {
            return this.Driver.ScrollToElement(element);
        }

        protected void WaitForElementToDisapper(BaseElement baseElement)
        {
            this.Driver.WaitForElementToDisapper(baseElement);
        }

        protected void HoverElement(BaseElement baseElement)
        {
            this.Driver.HoverElement(baseElement);
        }

        protected IWebElement GetSwitchElement(string label, string value)
        {
            return this.Driver.GetSwitchElement(label, value);
        }

        protected IWebElement GetInlineSwitchElement(string label, string value)
        {
            return this.Driver.GetInlineSwitchElement(label, value);
        }

        protected bool IsElementPresent(BaseElement baseElement)
        {
            return this.Driver.IsElementPresent(baseElement);
        }

        protected void WaitForLoadingToComplete()
        {
            WaitForElementToDisapper(LoadingDialog);
        }

        protected void WaitForProgressToComplete(int additionalWaitTimeInSeconds=0)
        {
            try
            {
                Thread.Sleep(additionalWaitTimeInSeconds * 1000);
                WaitForElementToDisapper(ProgressBar);
                WaitForLoadingToComplete();
            }
            catch (Exception)
            {
                Log.Debug("Error in WaitForProgressToComplete");            
            }
        }

        protected void SelectFile(BaseElement baseElement, string filePath)
        {
            this.Driver.SelectFile(baseElement, filePath);
        }

        protected IWebElement OpenTreeNode(string nodeName)
        {
            return this.Driver.OpenTreeNode(nodeName);
        }
    }
}
