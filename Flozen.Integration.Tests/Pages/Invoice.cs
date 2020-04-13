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

    public class Invoice : BasePage
    {
        private readonly BaseElement
            AddInvoiceBtn = new BaseElement("Xpath", "//span[text()='Create New Invoice']"),
            InvoiceNum = new BaseElement("XPath", "//input[@id='invoicenumber']"),
            Client = new BaseElement("Xpath", "//div[@id='clientid_chosen']"),
            Project = new BaseElement("Xpath", "//div[@id='projectid_chosen']"),
            Employee = new BaseElement("Xpath", "//div[@id='employeeid_chosen']"),
            PaymentTerms = new BaseElement("Xpath", "//div[@id='paymentterms_paymenttermid_chosen']"),
            InvoiceDate = new BaseElement("XPath", "//input[@id='invoicedate']"),
            InvoiceDueDate = new BaseElement("XPath", "//input[@id='invoiceduedate']"),
            StartDate = new BaseElement("XPath", "//input[@id='invoicestartdate']"),
            EndDate = new BaseElement("XPath", "//input[@id='invoiceenddate']"),
            OtherChargesAdd = new BaseElement("Xpath", "//input[@type='button' and @class='small anewits' and @value='Add']"),
            OtherChargesService = new BaseElement("XPath", "//input[@id='aprojid{0}']"),
            OtherChargesDescription = new BaseElement("XPath", "//input[@id='aparticaulars{0}']"),
            OtherChargesQuantity = new BaseElement("XPath", "//input[@id='ada{0}']"),
            OtherChargesRate = new BaseElement("XPath", "//input[@id='add{0}']"),
            OtherChargesTotal = new BaseElement("XPath", "//span[@id='asum{0}']"),
            OtherChargesGrandTotal = new BaseElement("XPath", "//span[@id='atotal']"),
            DeductionsAdd = new BaseElement("Xpath", "//input[@type='button' and @class='dnewits small' and @value='Add']"),
            DeductionsService = new BaseElement("XPath", "//input[@id='dprojid{0}']"),
            DeductionsDescription = new BaseElement("XPath", "//input[@id='dparticaulars{0}']"),
            DeductionsQuantity = new BaseElement("XPath", "//input[@id='dda{0}']"),
            DeductionsRate = new BaseElement("XPath", "//input[@id='ddd{0}']"),
            DeductionsTotal = new BaseElement("XPath", "//span[@id='dsum{0}']"),
            DeductionsGrandTotal = new BaseElement("XPath", "//span[@id='dtotal']"),
            DiscountValue = new BaseElement("XPath", "//input[@id='discounta']"),            
            Tax1 = new BaseElement("XPath", "//input[@id='sscadd0']"),
            Tax2 = new BaseElement("XPath", "//input[@id='sscadd1']"),
            AlertOkBtn = new BaseElement("Xpath", "//button[@id='alertmsgok-button']"),
            Total = new BaseElement("XPath", "//span[@id='spnsubgrandtotal']"),
            DiscountApplied = new BaseElement("XPath", "//span[@class='spndiscount']"),
            GrandTotal = new BaseElement("XPath", "//span[@id='spngrandtotal']"),
            Save = new BaseElement("XPath", "//input[@value='Save' and @type='button']"),
            DeleteInv = new BaseElement("Xpath", "//a[@title='Delete']"),
            EditInv = new BaseElement("Xpath", "//a[@title='Edit']"),
            OkButton = new BaseElement("Xpath", "//*[text()='Confirm Delete']/../..//button[text()='OK']"),
            InvoicesAccordion = new BaseElement("XPath", "//div[@id='invoiceaccordion']"),
            InvoicesTable = new BaseElement("XPath", "//table[@id='invoice']");

        public Invoice(DriverManager driverManager) : base(driverManager) { }

        public void OpenNewInvoiceWindow()
        {
            WaitForProgressToComplete(10);
            HoverElement(AddInvoiceBtn); // Workaround
            GetElement(AddInvoiceBtn).ClickIt();
        }

        public void AddOtherCharges(List<TestDataRow> rows)
        {
            Log.Information("In AddOtherCharges...start");
            int elmInd = 0;
            foreach (TestDataRow row in rows)
            {
                String elmPos = Convert.ToString(elmInd);
                GetElement(OtherChargesAdd).Click();
                GetElement(OtherChargesService.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Service"));
                GetElement(OtherChargesService.ReplaceToken("{0}", elmPos)).SendKeys(Keys.Tab);  // workaround
                WaitForProgressToComplete();
                GetElement(OtherChargesDescription.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Description"));
                GetElement(OtherChargesQuantity.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Quantity"));
                GetElement(OtherChargesRate.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Rate"));
                GetElement(OtherChargesRate.ReplaceToken("{0}", elmPos)).SendKeys(Keys.Tab);
                var total = GetElement(OtherChargesTotal.ReplaceToken("{0}", elmPos)).GetText();
                Log.Information("Invoice Other Charges Total=", total);
                total.ShouldBe(row.Value("Total"));
                elmInd++;
            }
            Log.Information("In AddOtherCharges...end");
        }

        public void AddDeductions(List<TestDataRow> rows)
        {
            Log.Information("In AddDeductions...start");
            int elmInd = 0;
            foreach (TestDataRow row in rows)
            {
                String elmPos = Convert.ToString(elmInd);
                GetElement(DeductionsAdd).Click();
                GetElement(DeductionsService.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Service"));
                GetElement(DeductionsService.ReplaceToken("{0}", elmPos)).SendKeys(Keys.Tab); // workaround
                WaitForProgressToComplete();
                GetElement(DeductionsDescription.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Description"));
                GetElement(DeductionsQuantity.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Quantity"));
                GetElement(DeductionsRate.ReplaceToken("{0}", elmPos)).EnterText(row.Value("Rate"));
                GetElement(DeductionsRate.ReplaceToken("{0}", elmPos)).SendKeys(Keys.Tab);
                var total = GetElement(DeductionsTotal.ReplaceToken("{0}", elmPos)).GetText();
                Log.Information("Invoice Deduction Total=", total);
                total.ShouldBe(row.Value("Total"));
                elmInd++;
            }
            Log.Information("In AddDeductions...end");
        }

        public void AddInvoice(TestData data, String invoiceId)
        {
            Log.Information("In AddInvoice()...start");
            TestDataSheet invoiceSheet = data.Get("Invoice");
            List<TestDataRow> invoiceDataRows = invoiceSheet.FilterRows("InvoiceId=" + invoiceId);
            TestDataRow invoiceData = invoiceDataRows[0];

            WaitForProgressToComplete(5);
            GetElement(InvoiceNum).EnterText(invoiceData.Value("InvoiceNum"));
            GetElement(Client).SelectComboValue(invoiceData.Value("Client"));
            GetElement(Project).SelectComboValue(invoiceData.Value("Project"));
            GetElement(Employee).SelectComboValue(invoiceData.Value("Employee"));
            //GetElement(PaymentTerms).SelectComboValue(invoiceData.Value("PaymentTerms"));
            WaitForProgressToComplete(5);
            //GetElement(InvoiceDate).EnterText(invoiceData.Value("InvoiceDate"));
            //GetElement(InvoiceDueDate).EnterText(invoiceData.Value("InvoiceDueDate"));
            GetElement(StartDate).EnterText(invoiceData.Value("StartDate"));
            GetElement(EndDate).EnterText(invoiceData.Value("EndDate"));
            GetElement(EndDate).SendKeys(Keys.Tab);

            WaitForProgressToComplete(5);

            /*if (IsElementPresent(AlertOkBtn)) // workaround
                GetElement(AlertOkBtn).ClickIt();*/

            GetInlineSwitchElement("Apply Discount", invoiceData.Value("ApplyDiscount")).ClickIt();

            List<TestDataRow> othRows = data.Get("OtherCharges").FilterRows("InvoiceId=" + invoiceId);
            AddOtherCharges(othRows);

            List<TestDataRow> dedRows = data.Get("Deductions").FilterRows("InvoiceId=" + invoiceId);
            AddDeductions(dedRows);

            GetElement(DiscountValue).EnterText(invoiceData.Value("DiscountValue"), Keys.Tab);

            //GetElement(Tax1).EnterText(invoiceData.Value("Tax1"));
            //GetElement(Tax2).EnterText(invoiceData.Value("Tax2"));
            //GetElement(Tax2).SendKeys(Keys.Tab);

            var otherChargesGrandTotal = GetElement(OtherChargesGrandTotal).GetText();
            otherChargesGrandTotal.ShouldBe(invoiceData.Value("OtherChargesGrandTotal"));

            var deductionsGrandTotal = GetElement(DeductionsGrandTotal).GetText();
            deductionsGrandTotal.ShouldBe(invoiceData.Value("DeductionsGrandTotal"));

            //var totalValue = GetElement(Total).GetText();
            //totalValue.ShouldBe(invoiceData.Value("Total"));

            //var discountApplied = GetElement(DiscountApplied).GetText();
            //discountApplied.ShouldBe(invoiceData.Value("DiscountApplied"));

            //var grandTotal = GetElement(GrandTotal).GetText();            
            //grandTotal.ShouldBe(invoiceData.Value("GrandTotal"));

            GetElement(Save).ClickIt();

            //if (IsElementPresent(AlertOkBtn)) 
            //    GetElement(AlertOkBtn).ClickIt();

            Log.Information("In AddInvoice()...end");
        }


        public void EditInvoice(TestData data, string invoiceId)
        {
            Log.Information("In EditInvoice...");

            WaitForProgressToComplete(10);
            HoverElement(AddInvoiceBtn); // Workaround

            TestDataSheet invoiceSheet = data.Get("Invoice");
            List<TestDataRow> invoiceDataRows = invoiceSheet.FilterRows("InvoiceId=" + invoiceId);
            TestDataRow invoiceData = invoiceDataRows[0];

            GetElement(InvoicesAccordion).ClickIt();
            var searchCriteria = String.Format("Inv Num={0}", invoiceData.Value("InvoiceNum"));
            GetElement(InvoicesTable).GetTableCell("Actions", searchCriteria).GetElement(EditInv).ClickIt();
            //GetElement(OkButton).ClickIt();
            GetElement(DiscountValue).EnterText("15", Keys.Tab);
            GetElement(Save).ClickIt();

            Log.Information("End of EditInvoice");
        }

        public void DeleteInvoice(TestData data, string invoiceId)
        {
            Log.Information("In DeleteInvoice...");

            WaitForProgressToComplete(10);
            HoverElement(AddInvoiceBtn); // Workaround

            TestDataSheet invoiceSheet = data.Get("Invoice");
            List<TestDataRow> invoiceDataRows = invoiceSheet.FilterRows("InvoiceId=" + invoiceId);
            TestDataRow invoiceData = invoiceDataRows[0];

            // To get data from table
            DataTable dt = GetElement(InvoicesTable).GetTableData();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    Log.Information(dr[dc].ToString());
                }
            }

            /* To delete an invoice
            var searchCriteria = String.Format("Inv Num={0}", invoiceData.Value("InvoiceNum"));
            GetElement(InvoicesTable).GetTableCell("Actions", searchCriteria).GetElement(DeleteInv).ClickIt();
            GetElement(OkButton).ClickIt();*/
            Log.Information("End of DeleteInvoice");
        }
    }
}
