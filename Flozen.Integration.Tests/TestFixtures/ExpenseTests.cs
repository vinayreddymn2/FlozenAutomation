namespace Flozen.Integration.Tests.TestFixtures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using FlozenAutomation.Common;
    using Flozen.Integration.Tests.Pages;

    [TestFixture]
    public class ExpenseTests : BaseTest
    {
        [Test, Order(3)]
        [Category("Integration")]
        public void CreateNewExpenseReport()
        {
            TestData Data = DataManager.GetTestData("ExpenseData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Employee Central").OpenMenu("Expenses");

            Expense expense= new Expense(this.DriverManager);
            expense.AddNewExpense(Data);
        }

        [Test, Order(4)]
        [Category("Integration")]
        public void EditExpenseReport()
        {
            TestData Data = DataManager.GetTestData("ExpenseData", DataRandomToken, TestRunNum);

            Login login = new Login(this.DriverManager);
            login.OpenApp().LogIn(Data.Get("Login"));

            Dashboard dashboard = new Dashboard(this.DriverManager);
            dashboard.OpenSideMenu("Employee Central").OpenMenu("Expenses");

            Expense expense = new Expense(this.DriverManager);
            expense.EditExpense(Data);
        }
    }
}
