namespace Flozen.Integration.Tests.TestFixtures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using Flozen.Integration.Tests.Pages;
    using FlozenAutomation.Extensions;
    using FlozenAutomation.Common;
    using System.Threading;
    using System.Data;
    using Serilog;

    [TestFixture]
    public class EmployeeTests : BaseTest
    {
        [Test, Order(1)]
        [Category("Integration")]
        public void CreateNewEmp()
        {
            TestData Data = DataManager.GetTestData("EmpData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Employee Central").OpenMenu("Employees");
            
            RegisterEmployee registerEmployee = new RegisterEmployee(this.DriverManager);
            string actualMessage = registerEmployee.Register(Data.Get("Employee"));
            string expectedMessage = registerEmployee.GetExpectedMessage(Data);
            Assert.AreEqual(actualMessage, expectedMessage);

            dashboard.OpenSideMenu("Company").OpenMenu("Settings").OpenTab("Others");
            ActivateEmployee activation = new ActivateEmployee(this.DriverManager);
            activation.ActivateEmployeeViaUrl(Data.Get("Employee"));
            Assert.IsTrue(activation.IsPersonalTabPresent());

            registerEmployee.AddBasicDetails(Data.Get("Employee"));
        }

        /*[Test, Order(1)]
        [Category("Selenium")]
        public void LoginLogoutTest()
        {
            TestData Data = DataManager.GetTestData("EmpData", DataRandomToken, TestRunNum);
         
            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Supplier Central").OpenMenu("Suppliers");
            DriverManager.CaptureScreenshot("ssone", TestRunNum);
            // File Browse/Upload
            this.DriverManager.Driver
                .SelectFile(new BaseElement("Xpath", "//input[@type='file' and @id='1vendormsa']"),
                    ConfigManager.DataFolder + "\\sample.pdf");
            Thread.Sleep(10000);
            BaseElement phone = new BaseElement("XPath", "//label[@for='1phone1']");
            this.DriverManager.Driver.SelectCountryCode(phone, "44");
            DriverManager.CaptureScreenshot("sstwo");
            login.Logout(); 
        }
        */
    }
}
