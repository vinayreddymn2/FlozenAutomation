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

    public class Project : BasePage
    {
        private readonly BaseElement
                AddProjectLink = new BaseElement("Xpath", "//a[@class='addproject']"),
                ProjectName = new BaseElement("Xpath", "//input[@id='projectname']"),
                ProjectShortName = new BaseElement("Xpath", "//input[@id='projectshortname']"),
                ProjectDescription = new BaseElement("Xpath", "//textarea[@id='projectdescription']"),
                Client = new BaseElement("Xpath", "//div[@id='clientidsproject_chosen']"),
                StartDate = new BaseElement("Xpath", "//input[@id='projectstartdate']"),
                DurationType = new BaseElement("Xpath", "//div[@id='durationtype_chosen']"),
                Duration = new BaseElement("Xpath", "//input[@id='duration']"),
                //Next = new BaseElement("Xpath", "//a[@id='btnNext']"),
                Next = new BaseElement("LinkText", "Next"),
                PurchaseOrder = new BaseElement("Xpath", "//input[@type='file' and @id='purchaseorder']"),
                Rate = new BaseElement("Xpath", "//input[@id='rate']"),
                Weekends = new BaseElement("Xpath", "//div[@id='selweekenddaysproject_chosen']"),
                HomeAddress1 = new BaseElement("Xpath", "//input[@id='address1']"),
                HomeAddress2 = new BaseElement("Xpath", "//input[@id='address2']"),
                HomeCountry = new BaseElement("Xpath", "//div[@id='country_chosen']"),
                HomeCity = new BaseElement("Xpath", "//input[@id='city']"),
                HomeZip = new BaseElement("Xpath", "//input[@id='zip']"),
                //Submit = new BaseElement("Xpath", "//a[@id='btnFinish']"),
                Submit = new BaseElement("LinkText", "Submit"), 
                ProjectStatus = new BaseElement("Xpath", "//div[@id='projectstatus_chosen']"),
                ProjectSearch = new BaseElement("Xpath", "//input[@id='projectsearch']"),
                ProjectSearchResults = new BaseElement("Xpath", "//ul[@id='projectmenulist']/li"),
                SearchResultProjectCard = new BaseElement("Xpath", ".//div[@class='employeListSection']/ul/li");

        public Project(DriverManager driverManager) : base(driverManager) { }

        public void OpenNewProject()
        {
            WaitForProgressToComplete(5);
            HoverElement(AddProjectLink);
            GetElement(AddProjectLink).ClickIt();
        }

        public void CreateNewProject(TestDataSheet data)
        {
            Log.Information("In CreateProject...");

            WaitForProgressToComplete(5);
            GetElement(ProjectName).EnterText(data.Value(0, "ProjectName"));
            GetElement(ProjectShortName).EnterText(data.Value(0, "ProjectShortName"));
            GetElement(Client).SelectComboValue(data.Value(0, "Client"));
            GetElement(StartDate).EnterText(data.Value(0, "StartDate"));
            Thread.Sleep(3000); // Workaround
            GetElement(DurationType).SelectComboValue(data.Value(0, "DurationType"));
            GetElement(Duration).EnterText(data.Value(0, "Duration"));
            GetElement(ProjectDescription).SendKeys(Keys.Tab);

            GetElement(Next).ClickIt();

            var purhcaseOrderFile = Path.Combine(ConfigManager.DataFolder, data.Value(0, "PurchaseOrderFile"));
            SelectFile(PurchaseOrder, purhcaseOrderFile);
            GetElement(Rate).EnterText(data.Value(0, "Rate"));
            GetElement(Weekends).MultiSelectComboValues(data.Value(0, "Weekends").Split(','));
            GetElement(Next).ClickIt();

            GetElement(HomeAddress1).EnterText(data.Value(0, "HomeAddress1"));
            GetElement(HomeAddress2).EnterText(data.Value(0, "HomeAddress2"));
            GetElement(HomeCountry).SelectComboValue(data.Value(0, "HomeCountry"));
            GetElement(HomeCity).EnterText(data.Value(0, "HomeCity"));
            GetElement(HomeZip).EnterText(data.Value(0, "HomeZip"));
            GetElement(Next).ClickIt();

            GetElement(Submit).ClickIt();

            WaitForProgressToComplete(10);
            Log.Information("End of CreateProject");
        }

        public bool SearchProject(TestDataSheet data, string projectStatus="All")
        {
            Log.Information("Start of SearchProject()...");
            WaitForProgressToComplete();
            HoverElement(AddProjectLink); 
            GetElement(ProjectStatus).SelectComboValue(projectStatus);
            GetElement(ProjectSearch).EnterText(data.Value(0, "ProjectName"));
            WaitForProgressToComplete();
            if (IsElementPresent(ProjectSearchResults))
            {
                if (IsElementPresent(SearchResultProjectCard))
                {
                    var searchResultProjectName = new BaseElement("Xpath", ".//span[@class='empName']");
                    var searchResultCard = GetElement(SearchResultProjectCard);
                    var actualProjectName = searchResultCard.GetElement(searchResultProjectName).Text;
                    Log.Information("Found {actualProjectName} in Search Results", actualProjectName);
                    return String.Equals(actualProjectName, data.Value(0, "ProjectName"));
                } else
                {
                    return false;
                }
            } 
            else
            {
                return false;
            }
        }
    }
}
