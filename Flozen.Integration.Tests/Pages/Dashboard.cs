namespace Flozen.Integration.Tests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FlozenAutomation.Common;
    using FlozenAutomation.Extensions;
    using Serilog;

    public class Dashboard : BasePage
    {
        private readonly BaseElement 
            RegisterEmployeeLink = new BaseElement("Xpath", "//a[text()='Register Employee']"),
            MainTab = new BaseElement("XPath", "//ul[@role='tablist']//a[text()='{0}']");

        public Dashboard(DriverManager driverManager) : base(driverManager) { }

        public Dashboard OpenSideMenu(String MenuName)
        {
            Log.Information("In OpenSideMenu()...{MenuName}", MenuName);
            WaitForProgressToComplete(15);
            
            var sideMenu = new BaseElement("LinkText", MenuName);
            this.Driver.HoverElement(sideMenu);

            return this;
        }

        public Dashboard OpenMenu(String MenuName)
        {
            Log.Information("In Dashboard > OpenMenu...{MenuName}", MenuName);
            var menu = new BaseElement("Xpath", String.Format("//a[contains(text(),'{0}')]", MenuName));
            this.Driver.GetElement(menu).ClickIt();
            WaitForProgressToComplete();
            return this;
        }

        public Dashboard OpenTab(String TabName)
        {
            Log.Information("In Dashboard > OpenTab...{TabName}", TabName);
            MainTab.ReplaceToken("{0}", TabName);
            this.Driver.HoverElement(MainTab); // Workaround
            this.Driver.GetElement(MainTab).ClickIt();

            return this;
        }

    }
}
