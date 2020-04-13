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
    using NUnit.Framework.Interfaces;
    using Serilog;

    [TestFixture]
    public class ProjectTests : BaseTest
    {
        [Test, Order(2)]
        [Category("Integration")]
        public void CreateNewProject()
        {
            TestData Data = DataManager.GetTestData("ProjectData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Employee Central").OpenMenu("Projects");

            Project project = new Project(this.DriverManager);
            project.OpenNewProject();
            project.CreateNewProject(Data.Get("Project"));

            bool searchResult = project.SearchProject(Data.Get("Project"));
            Assert.IsTrue(searchResult);
        }
    }
}
