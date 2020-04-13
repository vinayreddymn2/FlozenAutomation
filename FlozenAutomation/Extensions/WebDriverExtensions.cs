namespace FlozenAutomation.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Serilog;
    using FlozenAutomation.Common;
    using System.IO;
    using OpenQA.Selenium.Interactions;

    public static class WebDriverExtensions
    {
        public static IWebElement GetElement(this IWebDriver driver, BaseElement baseElement)
        {
            try
            {
                Log.Information("GetElement()...{element}", baseElement);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.MaxTimeout));
                wait.Until(c => c.FindElement(baseElement.ToBy()));
                IWebElement element = driver.FindElement(baseElement.ToBy());

                return element;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetElement...{baseElement} {message} {stacktrace}", baseElement, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IList<IWebElement> GetElements(this IWebDriver driver, BaseElement baseElement)
        {
            try
            {
                Log.Information("GetElements()...{element}", baseElement);

                IList<IWebElement> elements = new List<IWebElement>();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.MaxTimeout));
                wait.Until(c => elements = c.FindElements(baseElement.ToBy()));

                return elements;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetElements...{baseElement} {message}  {stacktrace}", baseElement, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static bool IsElementPresent(this IWebDriver webDriver, BaseElement baseElement)
        {
            try
            {
                webDriver.GetElement(baseElement);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void WaitForElementToDisapper(this IWebDriver webDriver, BaseElement baseElement)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(ConfigManager.QuickWaitTime));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(d => 
                webDriver.FindElement(baseElement.ToBy())
                         .GetAttribute("style")
                         .Replace(" ", string.Empty)
                         .ToLower()
                         .Contains("display:none"));
        }

        public static void HoverElement(this IWebDriver driver, BaseElement baseElement)
        {
            try
            {
                Log.Information("WebElementExtension.Hover..{webElement}", baseElement.ToString());                
                var action = new OpenQA.Selenium.Interactions.Actions(driver);
                action.MoveToElement(driver.GetElement(baseElement)).Perform();
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.Hover...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                throw ex;
            }

        }

        public static IWebElement GetInlineSwitchElement(this IWebDriver driver, string label, string value)
        {
            try
            {
                Log.Information("GetInlineSwitchElement()...{label}, {value}", label, value);

                var xpath = string.Format("//*[contains(text(),'{0}')]/div/div/span[text()='{1}']", label, value);
                Log.Information("GetInlineSwitchElement(). {xpath}", xpath);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.MaxTimeout));
                wait.Until(c => c.FindElement(By.XPath(xpath)));
                IWebElement element = driver.FindElement(By.XPath(xpath));

                return element;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetInlineSwitchElement...{label} {value} {message} {stacktrace}", label, value, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IWebElement GetSwitchElement(this IWebDriver driver, string label, string value)
        {
            try
            {
                Log.Information("GetSwitchElement()...{label}, {value}", label, value);

                var xpath = string.Format("//label/strong[text()='{0}']/../../div/div/span[text()='{1}']", label, value);
                Log.Information("GetSwitchElement(). {xpath}", xpath);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.MaxTimeout));
                wait.Until(c => c.FindElement(By.XPath(xpath)));
                IWebElement element = driver.FindElement(By.XPath(xpath));

                return element;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSwitchElement...{label} {value} {message} {stacktrace}", label, value, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void SelectFile(this IWebDriver driver, BaseElement baseElement, string fileFullPath)
        {
            try
            {
                fileFullPath = Path.GetFullPath((new Uri(fileFullPath)).LocalPath);
                Log.Information("SelectFile()...{element}, {file}", baseElement, fileFullPath);
                
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.MaxTimeout));
                wait.Until(c => c.FindElement(baseElement.ToBy()));
                IWebElement element = driver.FindElement(baseElement.ToBy());
                element.Click();
                Thread.Sleep(7000);
                System.Windows.Forms.SendKeys.SendWait(fileFullPath);
                Thread.Sleep(3000);
                System.Windows.Forms.SendKeys.SendWait(@"{Enter}");
            }
            catch (Exception ex)
            {
                Log.Error("SelectFile()...{element}, {file}, {message}, {trace}", 
                    baseElement, fileFullPath, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void SelectCountryCode(this IWebDriver driver, BaseElement phoneLabel, string codeToChoose)
        {
            try
            {
                Log.Information("SelectCountryCode()...{element}, {code}", phoneLabel, codeToChoose);

                IWebElement labelElement = driver.FindElement(phoneLabel.ToBy());
                IWebElement phoneDiv = labelElement.FindElement(By.XPath(".//following-sibling::div/div"));
                phoneDiv.Click();
                IList<IWebElement> countries = phoneDiv.FindElements(By.XPath(".//ul/li"));
                foreach (IWebElement ele in countries)
                {
                    if (ele.GetAttribute("data-dial-code") != null 
                                && ele.GetAttribute("data-dial-code").Equals(codeToChoose))
                    {
                        ele.Click();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("SelectCountryCode()...{message}, {trace}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IWebElement ScrollToElement(this IWebDriver driver, BaseElement baseElement)
        {
            try
            {
                Log.Information("ScrollToElement()...{element}", baseElement);

                IWebElement element = GetElement(driver, baseElement);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView();", element);

                return element;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ScrollToElement...{baseElement} {message} {stacktrace}", baseElement, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IWebElement ScrollToElement(this IWebDriver driver, IWebElement element)
        {
            try
            {
                Log.Information("ScrollToElement()...{element}", element);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView();", element);

                return element;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ScrollToElement...{baseElement} {message} {stacktrace}", element, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void ScrollWindow(this IWebDriver driver, int x, int y)
        {
            try
            {
                Log.Information("ScrollToWindow()...{x}, {y}", x, y);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollBy(" + x + "," + y + ")");
            }
            catch (Exception ex)
            {
                Log.Error("Error in ScrollToWindow()...{x}, {y}: {message} {stacktrace}", x, y, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IWebElement JsClick(this IWebDriver driver, BaseElement baseElement)
        {
            try
            {
                Log.Information("JsClick()...{element}", baseElement);
                IWebElement element = GetElement(driver, baseElement);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click()", element);

                return element;
            }
            catch (Exception ex)
            {
                Log.Error("Error in JsClick...{baseElement} {message} {stacktrace}", baseElement, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IWebElement OpenTreeNode(this IWebDriver driver, string folderName)
        {
            try
            {
                Log.Information("OpenTreeNode()...{folderName}", folderName);
                BaseElement treeFolder = new BaseElement("Xpath",
                    String.Format("//a[text()='{0}' and contains(@class,'jstree-anchor')]", folderName));
                IWebElement node = GetElement(driver, treeFolder);
                new Actions(driver).DoubleClick(node).Build().Perform();
                return node;
            }
            catch (Exception ex)
            {
                Log.Error("Error in OpenTreeNode...{folderName} {message} {stacktrace}", folderName, ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}
