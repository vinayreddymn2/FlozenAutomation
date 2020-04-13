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
    public class ReportTests : BaseTest
    {
        [Test, Order(14)]
        [Category("Integration")]
        public void CustomReport()
        {
            TestData data = DataManager.GetTestData("CustomReportData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(data.Get("Login"), 0);

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Reports").OpenMenu("Time Activities");

            Report report = new Report(this.DriverManager);
            report.CustomizeClientReport(data);
            
            string mailContent = MailManager.GetNadaMailMessageBySubjectStartWith("flotest@getnada.com", "IsMyEmailWorking").text;
        }
    }
}
