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
    public class InvoiceTests : BaseTest
    {
        [Test, Order(8)]
        [Category("Integration")]
        public void CreateInvoice()
        {
            TestData Data = DataManager.GetTestData("InvoiceData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Client Central").OpenMenu("Invoices");

            Invoice invoice = new Invoice(this.DriverManager);
            invoice.OpenNewInvoiceWindow();
            invoice.AddInvoice(Data, "111");
        }

        [Test, Order(9)]
        [Category("Integration")]
        public void EditInvoice()
        {
            TestData Data = DataManager.GetTestData("InvoiceData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Client Central").OpenMenu("Invoices");

            Invoice invoice = new Invoice(this.DriverManager);
            invoice.EditInvoice(Data, "111");
        }

        [Test, Order(10)]
        [Category("Integration")]
        public void DeleteInvoice()
        {
            TestData Data = DataManager.GetTestData("InvoiceData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Client Central").OpenMenu("Invoices");

            Invoice invoice = new Invoice(this.DriverManager);
            invoice.DeleteInvoice(Data, "111");

        }

    }
}
