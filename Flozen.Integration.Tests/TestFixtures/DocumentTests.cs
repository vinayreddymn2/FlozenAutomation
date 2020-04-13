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
    public class DocumentTests : BaseTest
    {
        [Test]
        public void CreateClientFolders()
        {
            TestData Data = DataManager.GetTestData("ClientData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Company").OpenMenu("Documents");

            Document document = new Document(this.DriverManager);
            document.UploadDocument(Data.Get("Client"), "Clients");

        }
    }
}
