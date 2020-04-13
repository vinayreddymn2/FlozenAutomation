namespace Flozen.Integration.Tests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FlozenAutomation.Common;
    using FlozenAutomation.Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class Document : BasePage
    {
        private readonly BaseElement
            PageHeader = new BaseElement("Xpath", "//h1[text()='Documents']"),
            AddFolder = new BaseElement("Xpath", "//input[@id='addnewfolder']"),
            UploadFile = new BaseElement("Xpath", "//input[@id='addotherdocument']"),
            FolderName = new BaseElement("Xpath", "//input[@id='15folderpath']"),
            Submit = new BaseElement("Xpath", "//div[@id='dialog_add_otherdocuments']//button[@class='submit' and contains(text(),'Save')]"),
            BrowseFile = new BaseElement("Xpath", "//input[@id='15ffilename']"),
            DeleteFolder = new BaseElement("Xpath", "//input[@id='delfolder']"),
            AlertYes = new BaseElement("Xpath", "//div[contains(@class,'ui-dialog') and contains(@style,'display: block')]/.//button[text()='Yes']");
            

        public Document(DriverManager driverManager) : base(driverManager) { }



        public void UploadDocument(TestDataSheet data, string parentNode)
        {
            WaitForProgressToComplete(10);
            HoverElement(PageHeader); 
            IWebElement rootNode = OpenTreeNode(parentNode);
            rootNode.OpenTreeNode("Robert-72Hh")
                        .OpenTreeNode("Projects")
                        .OpenTreeNode("Housing-RJvk")
                        .OpenTreeNode("Others");
            GetElement(AddFolder).ClickIt();
            var folderName = UtilManager.RandomAlphaNumString(4);
            GetElement(FolderName).EnterText(folderName);
            GetElement(Submit).ClickIt();

            WaitForProgressToComplete(15);
            GetElement(UploadFile).ClickIt();
            var purhcaseOrderFile = Path.Combine(ConfigManager.DataFolder, "Sample.pdf");
            SelectFile(BrowseFile, purhcaseOrderFile);
            WaitForProgressToComplete(10);
            GetElement(Submit).ClickIt();
        }
    }
}
