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
    using System.Threading;

    public class RegisterEmployee : BasePage
    {
        private readonly BaseElement 
                RegisterEmployeeLink = new BaseElement("Xpath", "//a[@class='regemployee']"),
                EmployeeType = new BaseElement("Xpath", "//div[@id='employeetype_chosen']"),
                Salutation = new BaseElement("Xpath", "//div[@id='salutation_chosen']"),
                FirstName = new BaseElement("Xpath", "//input[@id='firstname']"),
                LastName = new BaseElement("Xpath", "//input[@id='lastname']"),
                DateOfBirth = new BaseElement("Xpath", "//input[@id='dateofbirth']"),
                SocialSecurityNumber = new BaseElement("Xpath", "//input[@id='socialsecuritynumber']"),
                Gender = new BaseElement("Xpath", "//div[@id='gender_chosen']"),
                Email = new BaseElement("Xpath", "//input[@id='homeemail']"), 
                Phone = new BaseElement("Xpath", "//input[@id='telmobilephone']"),
                RegistrationSubmit = new BaseElement("Xpath", "//button[@id='regbtn']"),
                AlertMessage = new BaseElement("Xpath", "//p[@id='alertmsg']"),
                AlertOkBtn = new BaseElement("Xpath", "//button[@id='alertmsgok-button']"),
                HomePhone = new BaseElement("Xpath", "//input[@id='tel1homephone']"),
                HomeAddress1 = new BaseElement("Xpath", "//input[@id='1address1']"),
                HomeAddress2 = new BaseElement("Xpath", "//input[@id='1address2']"),
                HomeCountry = new BaseElement("Xpath", "//div[@id='1country_chosen']"),
                HomeCity = new BaseElement("Xpath", "//input[@id='1city']"),
                HomeZip = new BaseElement("Xpath", "//input[@id='1zip']"),
                NextBtn = new BaseElement("Xpath", "//a[@id='btnNext']"),
                PositionAppliedFor = new BaseElement("Xpath", "//input[@id='1positionsappliedfor']"),
                AddSkillBtn = new BaseElement("Xpath", "//a[@class='open-add-skill-dialog']"),
                SkillName = new BaseElement("Xpath", "//input[@id='4skillname']"),
                SkillLevel = new BaseElement("Xpath", "//div[@id='4skilllevel_chosen']"),
                SkillSaveBtn = new BaseElement("Xpath", "//div[@id='dialog_add_employeeskills']//button[contains(text(),'Save')]"),
                AcknowlegeCheck = new BaseElement("Xpath", "//input[@id='ackemp']/../span[@class='checkbox']"),
                SubmitBtn = new BaseElement("Xpath", "//a[text()='Submit']");

        public RegisterEmployee(DriverManager driverManager) : base(driverManager) { }
        
        public string Register(TestDataSheet data)
        {
            try
            {
                Log.Information("RegisterEmployee.Register...Start");
                WaitForProgressToComplete();
                HoverElement(RegisterEmployeeLink); 
                GetElement(RegisterEmployeeLink).ClickIt();
                GetElement(EmployeeType).SelectComboValue(data.Value(0,"EmployeeType"));
                GetElement(Salutation).SelectComboValue(data.Value(0,"Salutation"));
                GetElement(FirstName).EnterText(data.Value(0,"FirstName"));
                GetElement(LastName).EnterText(data.Value(0,"LastName"));
                GetElement(DateOfBirth).EnterText(data.Value(0,"DateOfBirth"));
                GetElement(SocialSecurityNumber).EnterText(data.Value(0,"SocialSecurityNumber"));
                GetElement(Gender).SelectComboValue(data.Value(0,"Gender"));
                GetElement(Email).EnterText(data.Value(0,"Email"));
                GetElement(Phone).EnterText(data.Value(0,"Phone"));
                GetElement(RegistrationSubmit).ClickIt();                

                var actualMessage = this.Driver.GetElement(AlertMessage).GetText();
                GetElement(AlertOkBtn).ClickIt();

                if (IsElementPresent(AlertOkBtn)) // Bug
                    GetElement(AlertOkBtn).ClickIt();

                Log.Information("RegisterEmployee.Register...End");

                return actualMessage;
            }
            catch (Exception ex)
            {
                Log.Error("Error in RegisterEmployee.Register {message} {stack}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public string EmployeeRegistrationExpectedMessage(TestDataSheet data)
        {
            return String.Format("Employee {0} {1} added.", data.Value(0,"FirstName"), data.Value(0,"LastName"));
        }

        public bool AddBasicDetails(TestDataSheet data)
        {
            //try
            //{
                Log.Information("RegisterEmployee.AddBasicDetails...Start");

                WaitForLoadingToComplete();
                GetElement(HomePhone).EnterText(data.Value(0,"HomePhone"));
                GetElement(HomeAddress1).EnterText(data.Value(0,"HomeAddress1"));
                GetElement(HomeAddress2).EnterText(data.Value(0,"HomeAddress2"));
                GetElement(HomeCountry).SelectComboValue(data.Value(0,"HomeCountry"));
                GetElement(HomeCity).EnterText(data.Value(0,"HomeCity"));
                GetElement(HomeZip).EnterText(data.Value(0,"HomeZip"));
                WaitForLoadingToComplete();
                GetElement(NextBtn).ClickIt();

                // Additional Info
                GetElement(PositionAppliedFor).EnterText(data.Value(0,"PositionAppliedFor"));
                GetElement(NextBtn).ClickIt();

                // Edu & Exp
                GetElement(NextBtn).ClickIt();
                
                // Skills & Cert
                GetElement(AddSkillBtn).ClickIt();
                GetElement(SkillName).EnterText(data.Value(0,"SkillName"));
                GetElement(SkillLevel).SelectComboValue(data.Value(0,"SkillLevel"));
                if (data.Value(0,"IsCertified").ToLower().Equals("yes"))
                    GetSwitchElement("Certified", "Yes").ClickIt();
                GetElement(SkillSaveBtn).ClickIt();
                GetElement(NextBtn).ClickIt();

                // Dependents
                WaitForLoadingToComplete();
                GetElement(AlertOkBtn).ClickIt(); // Application Issue
                GetElement(NextBtn).ClickIt();

                // Emergency Contacts
                WaitForLoadingToComplete();
                GetElement(AlertOkBtn).ClickIt(); // Application Issue
                GetElement(NextBtn).ClickIt();

                // Medical CC
                WaitForLoadingToComplete();
                GetElement(AlertOkBtn).ClickIt(); // Application Issue
                GetElement(NextBtn).ClickIt();

                // Reference
                WaitForLoadingToComplete();
                GetElement(AlertOkBtn).ClickIt(); // Application Issue
                GetElement(NextBtn).ClickIt();

                // Summary
                WaitForLoadingToComplete();
                GetElement(AcknowlegeCheck).ClickIt();
                GetElement(SubmitBtn).ClickIt();

                Log.Information("RegisterEmployee.AddBasicDetails...End");

                return true;
            /*}
            catch (Exception ex)
            {
                Log.Error("Error in AddBasicDetails {message} {stack}", ex.Message, ex.StackTrace);
                throw ex;
            }*/
        }

        public string GetExpectedMessage(TestData data)
        {
            return String.Format("Employee {0} {1} added.",
                                    data.Value("Employee", 0, "FirstName"),
                                    data.Value("Employee", 0, "LastName"));
        }
    }
}
