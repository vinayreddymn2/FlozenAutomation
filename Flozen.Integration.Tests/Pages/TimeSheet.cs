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

    public class TimeSheet : BasePage
    {

        private readonly BaseElement
            TimeSheetMenu = new BaseElement("Xpath", "//a[@id='cliseltimesheetsummary']"),
            Client = new BaseElement("Xpath", "//div[@id='clientid1_chosen']"),
            Project = new BaseElement("Xpath", "//div[@id='projectid_chosen']"),
            Employee = new BaseElement("Xpath", "//div[@id='employeeid_chosen']"),
            EditInlineIcon = new BaseElement("Xpath", "//a[@title='Edit']"),
            TimeSheetsTable = new BaseElement("XPath", "//table[@id='employeetimesheetsummary']"),
            AddNewTask = new BaseElement("XPath", "//input[@id='addnewtsk']"),
            TaskName = new BaseElement("Xpath", "//select[@id='task_row_0']"),
            RowOneHours = new BaseElement("Xpath", "//input[@class='form-control inputFld2 hideinpboder txt20']"),
            AddFile = new BaseElement("Xpath", "//a[@id='addfile']"),
            ChooseFile = new BaseElement("Xpath", "//input[@type='file' and @id='editfile0']"),
            SubmitFile = new BaseElement("Xpath", "(//button[@class='submit' and contains(text(), 'Save')])[3]"),
            Save = new BaseElement("Xpath", "//input[@id='btnSave']"),
            AlertOkBtn = new BaseElement("Xpath", "//button[@id='alertmsgok-button']"),
            OkButton = new BaseElement("Xpath", "//*[text()='Message']/../..//button[text()='OK']");

        public TimeSheet(DriverManager driverManager) : base(driverManager) { }

        public void EditTimeSheet(TestData data)
        {
            Log.Information("In EditTimeSheet()...start");
            TestDataRow timeSheet = data.Get("TimeSheet").Rows[0];

            GetElement(TimeSheetMenu).ClickIt();
            WaitForProgressToComplete(5); 
            HoverElement(Client); // Workaround
            GetElement(Client).SelectComboValue(timeSheet.Value("Client"));
            WaitForProgressToComplete(5);
            GetElement(Project).SelectComboValue(timeSheet.Value("Project"));
            WaitForProgressToComplete(5);
            GetElement(Employee).SelectComboValue("BESTA");
            WaitForProgressToComplete(10);

            var searchCriteria = String.Format("Duration={0}", timeSheet.Value("Duration"));
            GetElement(TimeSheetsTable).GetTableCell("Actions", searchCriteria).GetElement(EditInlineIcon).ClickIt();
            WaitForProgressToComplete(10);

            if (IsElementPresent(AlertOkBtn)) // workaround
                GetElement(AlertOkBtn).ClickIt();


            //GetElement(AddNewTask).ClickIt();
            GetElement(TaskName).Select("Text", timeSheet.Value("TaskName"));
            GetElement(RowOneHours).EnterText(timeSheet.Value("Hours"));

            GetElement(AddFile).ClickIt();
            var tsFile = Path.Combine(ConfigManager.DataFolder, timeSheet.Value("TSFile"));
            SelectFile(ChooseFile, tsFile);
            GetElement(SubmitFile).ClickIt();
            WaitForProgressToComplete(10);
            GetElement(Save).ClickIt();
            GetElement(OkButton).ClickIt();
            Log.Information("In EditTimeSheet()...end");
        }

    }
}
