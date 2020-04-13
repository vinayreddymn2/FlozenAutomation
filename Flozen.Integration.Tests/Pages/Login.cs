namespace Flozen.Integration.Tests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FlozenAutomation.Common;
    using FlozenAutomation.Extensions;
    using Serilog;

    public class Login : BasePage
    {
        private readonly BaseElement
            UserName = new BaseElement("Xpath", "//input[@id='username']"),
            Password = new BaseElement("Xpath", "//input[@id='password']"),
            LoginBtn = new BaseElement("Id", "login_btn"),
            ProfileMenu = new BaseElement("XPath", "//a[@id='ddLabel']"),
            LogoutMenu = new BaseElement("XPath", "//a[@id='alogoutuser']");


        public Login(DriverManager driverManager) : base (driverManager) { }

        public Login OpenApp()
        {
            this.DriverManager.OpenApp();
            return this;
        }

        public Login LogIn(TestDataSheet data, int rowNum=0)
        {
            GetElement(UserName).EnterText(data.Value(rowNum, "UserName"));
            GetElement(Password).EnterText(data.Value(rowNum, "Password"));
            GetElement(LoginBtn).ClickIt();

            Log.Information("Logging with User Name {UserName}, Password {Password}", data.Value(0,"UserName"), data.Value(0,"Password"));

            return this;
        }

        public void Logout()
        {
            GetElement(ProfileMenu).ClickIt();
            GetElement(LogoutMenu).ClickIt();

            Log.Information("Logged out of application");
        }
    }
}
