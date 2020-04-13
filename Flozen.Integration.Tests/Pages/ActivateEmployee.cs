namespace Flozen.Integration.Tests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using FlozenAutomation.Common;
    using FlozenAutomation.Extensions;
    using Serilog;

    public class ActivateEmployee : BasePage
    {
        private readonly BaseElement
            ActiveLinksAccordion = new BaseElement("XPath", "//a[contains(text(),'Active Links')]"),
            ActiveLinksTable = new BaseElement("XPath", "//table[@id='applinkexpiry']"),
            Email = new BaseElement("Xpath", "//input[@id='email']"),
            LastName = new BaseElement("Xpath", "//input[@id='lastname']"),
            ConfirmBtn = new BaseElement("Xpath", "//input[@name='login_btn']"),
            PersonalTab = new BaseElement("Xpath", "//a[@id='personaltab']");

        public ActivateEmployee(DriverManager driverManager) : base(driverManager) { }

        public string GetActivationUrl(string email)
        {
            try
            {
                WaitForProgressToComplete();
                GetElement(ActiveLinksAccordion).ClickIt();
                WaitForProgressToComplete(30);

                if (IsElementPresent(ActiveLinksTable))
                {
                    var activeLink = GetElement(ActiveLinksTable)
                                        .GetTableCell("Link", String.Format("Email={0}", email))
                                        .GetText();

                    Log.Information("ActiveLinks.GetActivationLink...{activeLink}", activeLink);
                    return activeLink;
                } else
                {
                    throw new Exception("Activation Links Table not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error("ActiveLinks.GetActivationUrl() {message} {trace}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void ActivateEmployeeViaUrl(TestDataSheet data)
        {
            try
            {
                var employeeEmail = data.Value(0,"Email");
                var employeeLastName = data.Value(0,"LastName");

                var activationLink = this.GetActivationUrl(employeeEmail);
                this.DriverManager.OpenUrl(activationLink);
                
                GetElement(LastName).EnterText(employeeLastName);
                GetElement(Email).EnterText(employeeEmail);
                WaitForProgressToComplete();
                GetElement(ConfirmBtn).ClickIt();
            }
            catch (Exception ex)
            {
                Log.Error("Error in ActivateEmployeeViaUrl {message} {stack}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public bool IsPersonalTabPresent()
        {
            bool isPersonalTabPresent = IsElementPresent(PersonalTab);
            return isPersonalTabPresent;
        }
    }
}
