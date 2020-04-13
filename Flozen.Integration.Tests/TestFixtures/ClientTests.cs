
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
    public class ClientTests : BaseTest
    {
        [Test, Order(5)]
        [Category("Integration")]
        public void CreateClient()
        {
            TestData Data = DataManager.GetTestData("ClientData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Client Central").OpenMenu("Clients");

            Client client = new Client(this.DriverManager);
            client.OpenNewClientWindow();
            client.AddClient(Data);

            if (!client.SearchClient(Data.Get("Client")))
            {
                Assert.Fail("Client not found in search.");
            }
        }

        [Test, Order(6)]
        [Category("Integration")]
        public void AddClientContacts()
        {
            TestData Data = DataManager.GetTestData("ClientData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Client Central").OpenMenu("Clients");

            Client client = new Client(this.DriverManager);
            if (client.SearchClient(Data.Get("Client")))
            {
                client.AddClientContacts(Data.Get("Contact"));
            }
            else
            {
                Assert.Fail("Client not found in search.");
            }

            login.Logout();
        }

        [Test, Order(7)]
        [Category("Selenium")]
        public void AddClientProjects()
        {
            TestData Data = DataManager.GetTestData("ClientData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Client Central").OpenMenu("Clients");

            Client client = new Client(this.DriverManager);
            if (client.SearchClient(Data.Get("Client")))
            {
                client.AddClientProjects(Data.Get("Project"));
            }
            else
            {
                Assert.Fail("Client not found in search.");
            }

            login.Logout();
        }
    }
}
