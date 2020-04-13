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
    using System.IO;
    using OpenQA.Selenium;
    using Shouldly;
    using System.Data;

    public class Report : BasePage
    {
        private readonly BaseElement
            ClientTimeActivitesLink = new BaseElement("Xpath", "//span[text()='Client Time Activities']"),
            CustomizeBtn = new BaseElement("Xpath", "//button[contains(text(),'Customize')]"),
            ReportName = new BaseElement("Xpath", "//input[@id='creportName']"),
            ReportFieldsAccordion = new BaseElement("Xpath", "//h3[@id='ui-accordion-2-header-0']"),
            Amount = new BaseElement("Xpath", "//input[@id='rptchk9']"),
            RunReport = new BaseElement("Xpath", "//input[@name='runcustomreport']"),
            AlertOkBtn = new BaseElement("Xpath", "//button[@id='alertmsgok-button']"),
            EmailReport = new BaseElement("Xpath", "//input[@type='button' and @name='email']"),
            EmailTo = new BaseElement("Xpath", "//input[@type='text' and @id='toaddress']"),
            EmailSubject = new BaseElement("Xpath", "//input[@type='text' and @id='esubject']"),
            SubmitEmail = new BaseElement("Xpath", "//button[@class='submit' and contains(text(), 'Send')]"),
            ReportsTable = new BaseElement("XPath", "//table[@id='reportsTable']");
        

        public Report(DriverManager driverManager) : base(driverManager) { }

        public void CustomizeClientReport(TestData data)
        {
            Log.Information("In CustomizeClientReport()...start");
            TestDataRow cReport = data.Get("ClientTime").Rows[0];

            WaitForProgressToComplete(5);
            HoverElement(ClientTimeActivitesLink); 
            GetElement(ClientTimeActivitesLink).ClickIt();
            WaitForProgressToComplete();
            GetElement(CustomizeBtn).ClickIt();
            WaitForProgressToComplete(5);
            GetElement(ReportName).EnterText("TEST", Keys.Tab);
            GetElement(ReportFieldsAccordion).ClickIt();
            GetElement(Amount).ClickIt();
            GetElement(RunReport).ClickIt();
            WaitForProgressToComplete();
            GetElement(AlertOkBtn).ClickIt();
            
            GetElement(EmailReport).ClickIt();
            GetElement(EmailTo).EnterText(cReport.Value("EmailTo"));
            GetElement(EmailSubject).EnterText(cReport.Value("EmailSubject"));
            GetElement(SubmitEmail).ClickIt();
            WaitForProgressToComplete(10);
            GetElement(AlertOkBtn).ClickIt();
            Log.Information("In CustomizeClientReport()...end");
        }
    }
}
