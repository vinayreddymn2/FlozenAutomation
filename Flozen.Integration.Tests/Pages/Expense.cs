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

    public class Expense : BasePage
    {
        private readonly BaseElement
        AddExpense = new BaseElement("Xpath", "//a[@id='expenseentry']"),
        EmployeeName = new BaseElement("Xpath", "//div[@id='1employeeid_chosen']"),
        ProjectName = new BaseElement("Xpath", "//select[@id='1projectid']"),
        AddReceipt = new BaseElement("Xpath", "//a[text()='Add Receipt']"),
        StartDate = new BaseElement("Xpath", "//input[@id='1expensestartdate']"),
        EndDate = new BaseElement("Xpath", "//input[@id='1expenseenddate']"),
        ApproverName = new BaseElement("Xpath", "//input[@id='1approvedclientname']"),
        ReceiptAmount = new BaseElement("Xpath", "//input[@id='10expenseamount']"),
        ReceiptType = new BaseElement("Xpath", "//div[@id='10expensetypeid_chosen']"),
        SaveReceipt = new BaseElement("Xpath", "//button[contains(text(),'Save')]"),
        SaveExpense = new BaseElement("Xpath", "//input[@id='expsubmit']"),
        SearchProject = new BaseElement("Xpath", "//div[@id='projectid_chosen']"),
        SearchEmployee = new BaseElement("Xpath", "//div[@id='employeeid_chosen']"),
        SearchYear = new BaseElement("Xpath", "//div[@id='tyear_chosen']"),
        EditExpenseIcon = new BaseElement("Xpath", "//a[@title='Edit']"),
        ExpenseTable = new BaseElement("XPath", "//table[@id='employeeexpensessummary']");

        public Expense(DriverManager driverManager) : base(driverManager) { }

        public void AddNewExpense(TestData data)
        {
            Log.Information("In AddNewExpense...");
            try
            {
                TestDataSheet expenseRows = data.Get("Expense");
                WaitForProgressToComplete();
                HoverElement(AddExpense);
                GetElement(AddExpense).ClickIt();

                WaitForProgressToComplete(10);
                var empName = expenseRows.Value(0, "EmployeeName");
                GetElement(EmployeeName).SelectComboValue(empName);
                WaitForProgressToComplete(30); // Workaround - no progress bar
                GetElement(ProjectName).Select("Text", expenseRows.Value(0, "ProjectName"));
                GetInlineSwitchElement("Approved By Client", expenseRows.Value(0, "ApprovedByClient")).ClickIt();
                GetElement(StartDate).EnterText(expenseRows.Value(0, "StartDate"));
                GetElement(EndDate).EnterText(expenseRows.Value(0, "EndDate"));
                GetElement(ApproverName).EnterText(expenseRows.Value(0, "ApproverName"));

                List<TestDataRow> receiptLines = data.Get("Receipts")
                                                    .FilterRows("EmployeeName=" + empName);
                foreach (TestDataRow receipt in receiptLines)
                {
                    GetElement(AddReceipt).ClickIt();
                    GetElement(ReceiptAmount).EnterText(receipt.Value("ReceiptAmount"));
                    GetElement(ReceiptType).SelectComboValue(receipt.Value("ReceiptType"));
                    GetElement(SaveReceipt).ClickIt();
                }

                GetElement(SaveExpense).ClickIt();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Log.Information("End of AddNewExpense");
        }

        public int SearchExpense(TestData data)
        {
            var resultRows = -1;
            Log.Information("In SearchExpense...");
            try
            {
                WaitForProgressToComplete();
                Thread.Sleep(20000); // Workaround - No Progress Bar
                TestDataSheet expenseRows = data.Get("Expense");
                GetElement(SearchProject).SelectComboValue(expenseRows.Value(0, "ProjectName"));
                WaitForProgressToComplete();
                GetElement(SearchEmployee).SelectComboValue(expenseRows.Value(0, "EmployeeName"));
                WaitForProgressToComplete();
                GetElement(SearchYear).SelectComboValue(expenseRows.Value(0, "SearchYear"));

                WaitForProgressToComplete(3);
                if (IsElementPresent(ExpenseTable))
                {
                    resultRows = GetElement(ExpenseTable).GetTableRows().Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Log.Information("End of SearchExpense...");

            return resultRows;
        }

        public void EditExpense(TestData data)
        {
            Log.Information("In EditExpense...");
            try
            {
                TestDataSheet expenseRows = data.Get("Expense");

                int searchRows = SearchExpense(data);
                
                if (searchRows > 0)
                {
                    var searchCriteria = String.Format("Consultant={0}", expenseRows.Value(0, "EmployeeName"));
                    GetElement(ExpenseTable)
                        .GetTableCell("Actions", searchCriteria)
                        .GetElement(EditExpenseIcon)
                        .ClickIt();
                } else
                {
                    throw new Exception("No results found in Expense Summary Table");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Log.Information("End of EditExpense");
        }
    }
}
