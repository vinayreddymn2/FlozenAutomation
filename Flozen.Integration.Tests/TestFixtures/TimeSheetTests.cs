namespace Flozen.Integration.Tests.TestFixtures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FlozenAutomation.Common;
    using Flozen.Integration.Tests.Pages;
    using NUnit.Framework;

    [TestFixture]
    public class TimeSheetTests : BaseTest
    {
        [Test, Order(11)]
        [Category("Integration")]
        public void EditTimeEntry()
        {
            TestData Data = DataManager.GetTestData("TimeSheetData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"), 1);

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Client Central");//.OpenMenu("TimeSheets");

            TimeSheet ts = new TimeSheet(this.DriverManager);
            ts.EditTimeSheet(Data);

        }

    }
}
