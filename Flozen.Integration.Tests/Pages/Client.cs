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
    using OpenQA.Selenium;
    using Serilog;

    public class Client : BasePage
    {
        private readonly BaseElement
            AddClientBtn = new BaseElement("Xpath", "//span[text()='Add Client']"),
            ClientName = new BaseElement("XPath", "//input[@id='clientname']"),
            RelationshipManager = new BaseElement("Xpath", "//div[@id='clientrelationshipmanager_chosen']"),
            Email = new BaseElement("XPath", "//input[@id='email']"),
            Phone = new BaseElement("XPath", "//input[@id='telphone1']"),
            Next = new BaseElement("XPath", "//a[@id='btnNext']"),
            HomeAddress1 = new BaseElement("Xpath", "//input[@id='caddress1']"),
            HomeAddress2 = new BaseElement("Xpath", "//input[@id='caddress2']"),
            HomeCountry = new BaseElement("Xpath", "//div[@id='ccountry_chosen']"),
            HomeState = new BaseElement("Xpath", "//div[@id='cstate_chosen']"),
            HomeCity = new BaseElement("Xpath", "//input[@id='ccity']"),
            HomeZip = new BaseElement("Xpath", "//input[@id='czip']"),
            Weekends = new BaseElement("Xpath", "//div[@id='selweekenddaysclient_chosen']"),
            Submit = new BaseElement("Xpath", "//a[@id='btnFinish']"),
            ClientStatus = new BaseElement("Xpath", "//div[@id='clientdropdownstatus_chosen']"),
            ClientSearch = new BaseElement("Xpath", "//input[@id='clientsearch']"),
            ClientSearchResults = new BaseElement("Xpath", "//ul[@id='clientmenulist']/li"),
            SearchResultClientCard = new BaseElement("Xpath", "//div[@class='empListMain']/ul/li[contains(@class,'active')]/div/ul/li"),
            SearchResultClientName = new BaseElement("Xpath", ".//span[@class='empName']"),
            OthersTab = new BaseElement("XPath", "//ul[@role='tablist']//a[text()='Others']"),
            ContactsAccordion = new BaseElement("XPath", "//a[text()='Contacts']"),
            AddContactLink = new BaseElement("XPath", "//a[@class='open-add-contact-dialog']"),
            ContactType = new BaseElement("Xpath", "//input[@id='ccontacttype']"),
            ContactEmail = new BaseElement("Xpath", "//input[@id='contactemailid']"),
            ContactFirstName = new BaseElement("Xpath", "//input[@id='contactfirstname']"),
            ContactLastName = new BaseElement("Xpath", "//input[@id='contactlastname']"),
            ContactSave = new BaseElement("Xpath", "//button[contains(text(),'Save')]"),
            ProjectsAccordion = new BaseElement("XPath", "//h3[contains(@class,'i-accordion-header')]/./a[text()='Projects']"),
            AddProjectLink = new BaseElement("XPath", "//a[@class='open-add-projects-dialog']");

        public Client(DriverManager driverManager) : base(driverManager) { }

        public void OpenNewClientWindow()
        {
            WaitForProgressToComplete(10);
            HoverElement(AddClientBtn); // Workaround
            GetElement(AddClientBtn).ClickIt();
        }

        public void AddClient(TestData data)
        {
            Log.Information("In AddClient()...start");
            TestDataSheet clientData = data.Get("Client");

            WaitForProgressToComplete(2);
            GetElement(ClientName).EnterText(clientData.Value(0, "ClientName"));
            GetElement(RelationshipManager).SelectComboValue(clientData.Value(0, "RelationshipManager"));
            GetElement(Email).EnterText(clientData.Value(0, "Email"));
            GetElement(Phone).EnterText(clientData.Value(0, "Phone"));
            GetElement(Next).ClickIt();

            WaitForLoadingToComplete();
            GetElement(HomeAddress1).EnterText(clientData.Value(0, "HomeAddress1"));
            GetElement(HomeAddress2).EnterText(clientData.Value(0, "HomeAddress2"));
            GetElement(HomeCountry).SelectComboValue(clientData.Value(0, "HomeCountry"));
            GetElement(HomeState).SelectComboValue(clientData.Value(0, "HomeState"));
            GetElement(HomeCity).EnterText(clientData.Value(0, "HomeCity"));
            GetElement(HomeZip).EnterText(clientData.Value(0, "HomeZip"));
            GetInlineSwitchElement("Same as Address", clientData.Value(0, "SameAsAddress")).ClickIt();
            GetElement(Next).ClickIt();

            WaitForLoadingToComplete();
            GetElement(Weekends).MultiSelectComboValues(clientData.Value(0, "Weekends").Split(','));
            GetElement(Next).ClickIt();

            GetElement(Submit).ClickIt();

            Log.Information("In AddClient()...End");
        }

        public bool SearchClient(TestDataSheet data, string clientStatus = "All")
        {
            Log.Information("Start of SearchClient()...");
            WaitForProgressToComplete(5);
            HoverElement(AddClientBtn);
            WaitForProgressToComplete(25);
            GetElement(ClientStatus).SelectComboValue(clientStatus);
            GetElement(ClientSearch).EnterText(data.Value(0, "ClientName"));
            WaitForProgressToComplete(5);
            if (IsElementPresent(ClientSearchResults))
            {
                if (IsElementPresent(SearchResultClientCard))
                {
                    var actualClientName = GetElement(SearchResultClientCard)
                                            .GetElement(SearchResultClientName).Text;
                    Log.Information("Found {actualClientName} in Search Results", actualClientName);
                    return String.Equals(actualClientName, data.Value(0, "ClientName"));
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void AddClientContacts(TestDataSheet clientData)
        {
            Log.Information("AddClientContact...Start");
            WaitForProgressToComplete();
            GetElement(OthersTab).ClickIt();
            GetElement(ContactsAccordion).ClickIt();
            foreach (TestDataRow dataRow in clientData.FilterRows())
            {
                GetElement(AddContactLink).ClickIt();
                WaitForProgressToComplete(10);
                GetElement(ContactType).EnterText(dataRow.Value("ContactType"));
                WaitForProgressToComplete();
                GetElement(ContactEmail).EnterText(dataRow.Value("ContactEmail"));
                GetElement(ContactType).EnterText(dataRow.Value("ContactType")); // Bug
                WaitForProgressToComplete();
                GetElement(ContactFirstName).ClickIt();
                WaitForProgressToComplete(5);
                GetElement(ContactFirstName).EnterText(dataRow.Value("ContactFirstName"));
                WaitForProgressToComplete();
                GetElement(ContactLastName).EnterText(dataRow.Value("ContactLastName"));
                GetElement(ContactSave).ClickIt();
            }
            Log.Information("AddClientContact...End");
        }

        public void AddClientProjects(TestDataSheet clientData)
        {
            Log.Information("AddClientProjects...Start");
            WaitForProgressToComplete();
            GetElement(OthersTab).ClickIt();
            GetElement(ProjectsAccordion).ClickIt();
            WaitForProgressToComplete(10);
            foreach (TestDataRow dataRow in clientData.FilterRows())
            {
                GetElement(AddProjectLink).ClickIt();
                Project project = new Project(this.DriverManager);
                project.CreateNewProject(clientData);
            }
            Log.Information("AddClientProjects...End");
        }
    }
}
