namespace FlozenAutomation
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Fare;
    using FlozenAutomation.Common;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System.Text.RegularExpressions;
    using Microsoft.Win32;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            //IWebDriver driver = new ChromeDriver(@"D:\Kiran\Projects\drivers");
            //driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/");
            //driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");
            //Thread.Sleep(5000);
            //System.Diagnostics.Debug.WriteLine(driver.Title);
            // driver.FindElement(By.LinkText("Privacy Policy")).Click();
            // driver.SwitchTo().DefaultContent();
            // driver.FindElement(By.Id("login_btn")).Click();
            // driver.FindElement(By.Id("btnsignup")).Click();
            //driver.FindElement(By.XPath("//input[@id='txtUsername']")).SendKeys("Kal");

            /*var length = 5;
            var xeger = new Xeger(@"\d{" + length + "}");            
            var generatedString = xeger.Generate();
            //Console.WriteLine(generatedString);

            string str = "code.justdo{{RAND-NUM10}}@gmail{{RAND-NUM}}.com{{RAND-NUM3}}";
            MatchCollection mcoll = Regex.Matches(str, @"{{RAND-NUM\d*}}");
            var finalToken = str;
            foreach (Match mch in mcoll)
            {
                var token = mch.Value.ToString();
                Regex re = new Regex(@"^\d$");
                var numToken = Regex.Match(token, @"\d+").Value;
                var randLength = Convert.ToInt32((numToken != null && numToken.Length > 0) ? numToken : "5");
                xeger = new Xeger(@"\d{" + randLength + "}");
                finalToken = finalToken.Replace(mch.Value, Convert.ToString(Convert.ToInt64(xeger.Generate())));
            }
            
            Console.WriteLine(finalToken);*/
            UtilManager.BundleResultFiles(16229696);
        }
    }
}
