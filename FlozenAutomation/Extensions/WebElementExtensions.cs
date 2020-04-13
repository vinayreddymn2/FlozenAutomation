namespace FlozenAutomation.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FlozenAutomation.Common;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Remote;
    using Serilog;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Internal;
    using System.Data;

    public static class WebElementExtensions
    {
        public static void EnterText(this IWebElement webElement, string textToEnter)
        {
            if (textToEnter != null && textToEnter.Length > 0)
            {
                try
                {
                    Log.Information("WebElementExtension.EnterText..Text: {textToEnter}", textToEnter);
                    webElement.Clear();
                    webElement.SendKeys(textToEnter);
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Log.Error("WebElementExtension.EnterText()...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }

        public static void EnterText(this IWebElement webElement, string textToEnter, string key)
        {
            if (textToEnter != null && textToEnter.Length > 0)
            {
                try
                {
                    Log.Information("WebElementExtension.EnterText..Text: {textToEnter}", textToEnter);
                    webElement.Clear();
                    webElement.SendKeys(textToEnter);
                    webElement.SendKeys(key);
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Log.Error("WebElementExtension.EnterText()...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }

        public static void ClickIt(this IWebElement webElement)
        {
            try
            {
                Log.Information("WebElementExtension.ClickIt..");
                if (webElement.Displayed && webElement.Enabled)
                {
                    webElement.Click();
                } else
                {
                    throw new Exception("Element not displayed or not enabled");
                }
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.ClickIt()...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IWebElement GetElement(this IWebElement webElement, BaseElement baseElement)
        {
            try
            {
                Log.Information("GetElement()...{element}", baseElement);
                IWebElement element = webElement.FindElement(baseElement.ToBy());
                return element;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetElement...{baseElement} {message} {stacktrace}", baseElement, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IList<IWebElement> GetElements(this IWebElement webElement, BaseElement baseElement)
        {
            try
            {
                Log.Information("GetElements()...{element}", baseElement);
                IList<IWebElement> elements = webElement.FindElements(baseElement.ToBy());
                return elements;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetElements...{baseElement} {message} {stacktrace}", baseElement, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void Select(this IWebElement webElement, string selectMode, string selectValue)
        {
            try
            {
                Log.Information("WebElementExtension.Select..{textToSelect}", selectValue);
                SelectElement select = new SelectElement(webElement);
                if (string.Equals(selectMode, "Text", StringComparison.CurrentCultureIgnoreCase))
                {
                    select.SelectByText(selectValue);
                }
                else if (string.Equals(selectMode, "Value", StringComparison.CurrentCultureIgnoreCase))
                {
                    select.SelectByValue(selectValue);
                }
                else if (string.Equals(selectMode, "Index", StringComparison.CurrentCultureIgnoreCase))
                {
                    select.SelectByIndex(Convert.ToInt32(selectValue));
                } else
                {
                    throw new Exception("Invalid Select Option");
                }
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.Select...{textToselect}, {message} {stacktrace}", selectValue, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IList<string> GetSelectValues(this IWebElement webElement)
        {
            try
            {
                Log.Information("WebElementExtension.GetSelectValues..");
                IList<string> values = new List<string>();
                SelectElement select = new SelectElement(webElement);
                foreach(var option in select.Options)
                {
                    values.Add(option.Text);
                }
                return values;
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.GetSelectValues...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void SelectComboValue(this IWebElement webElement, string textToselect)
        {
            try
            {
                Log.Information("WebElementExtension.SelectCombo..{textToSelect}", textToselect);
                if (!String.IsNullOrEmpty(textToselect) && textToselect.Length > 0)
                {
                    webElement.Click();
                    var found = false;
                    var optionsList = webElement.FindElements(By.XPath("//div[@class='chosen-drop']/ul/li"));
                    foreach (var option in optionsList)
                    {
                        if (option.Text.ToLower() == textToselect.ToLower())
                        {
                            option.Click();
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Log.Error("Text {0} not found in drop down list", textToselect);
                        webElement.SendKeys(Keys.Escape);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.SelectCombo...{textToselect} {message}  {stacktrace}", textToselect, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IList<string> GetSelectComboValues(this IWebElement webElement)
        {
            try
            {
                Log.Information("WebElementExtension.GetSelectComboValues...");
                IList<string> comboValues = new List<string>();
                var optionsList = webElement.FindElements(By.XPath("//div[@class='chosen-drop']/ul/li"));
                foreach (var option in optionsList)
                {
                    comboValues.Add(option.Text);
                }
                return comboValues;
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.GetSelectComboValues...{message}  {stacktrace}",  ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void MultiSelectComboValues(this IWebElement webElement, string[] values)
        {
            Log.Information("In MultiSelectCombo...{values}", values);
            foreach (string value in values)
            {
                webElement.SelectComboValue(value.Trim());
            }
        }

        public static IList<string> GetMultiSelectComboSelectedValues(this IWebElement webElement)
        {
            try
            {
                Log.Information("In GetMultiSelectComboSelectedValues...");
                IList<string> comboValues = new List<string>();
                var optionsList = webElement.FindElements(By.XPath(".//ul[@class='chosen-choices']/li[@class='search-choice']/span"));
                foreach (var option in optionsList)
                {
                    comboValues.Add(option.Text);
                }
                return comboValues;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetMultiSelectComboSelectedValues {message} {stack}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static int ClearMultiSelectCombo(this IWebElement webElement)
        {
            try
            {
                Log.Information("In ClearMultiSelectCombo...");
                int count = 0;

                var optionsList = webElement.FindElements(By.XPath(".//ul[@class='chosen-choices']/li[@class='search-choice']"));
                foreach (var option in optionsList)
                {
                    var deleteLink = option.FindElement(By.XPath(".//a[@class='search-choice-close']"));
                    deleteLink.Click();
                    count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ClearMultiSelectCombo {message} {stack}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static string GetText(this IWebElement webElement)
        {
            try
            {                
                var text = webElement.Text;

                Log.Information("WebElementExtension.GetText..Text: {text}", text);
                return text;
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.EnterText()...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static bool VerifyElementText(this IWebElement webElement, string expected, bool ignoreCase = false)
        {
            Log.Information("In VerifyElementText()...");
            try
            {
                var result = false;
                if (ignoreCase)
                {
                    result = string.Equals(webElement.GetText(), expected, StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    result = webElement.GetText().Equals(expected);
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Error occurred in VerifyElementText...{message} {stack}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IWebElement GetTableCell(this IWebElement table, string targetColName, string searchCriteria)
        {
            try
            {
                Log.Information("GetTableCell()...{colname} {criteria}", targetColName, searchCriteria);

                IWebElement targetElement = null;
                int targetColIndex=1, index=0, searchColIndex=-1;

                var searchKeyVals = searchCriteria.Split(';');
                var searchColName = searchKeyVals[0].Split('=')[0];
                var searchColVal = searchKeyVals[0].Split('=')[1];

                var headerColumns = table.FindElements(By.CssSelector("tr > th"));
                if (headerColumns.Count == 0)
                {
                    headerColumns = table.FindElements(By.CssSelector("thead > tr > td"));
                }
                foreach(var headerColumn in headerColumns)
                {
                    if (String.Equals(headerColumn.Text, targetColName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetColIndex = index;
                    }

                    if (String.Equals(headerColumn.Text, searchColName, StringComparison.OrdinalIgnoreCase))
                    {
                        searchColIndex = index;
                    }
                    index++;
                }

                if (targetColIndex == -1 || searchColIndex == -1)
                {
                    Log.Error("GetTableCellValue..Either Target Column {TargetColumn} or Source Column {SourceColumn} not found in table", targetColName, searchColName);
                    throw new Exception();
                }

                IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody > tr"));
                foreach (IWebElement row in rows)
                {
                    var cells = row.FindElements(By.CssSelector("td"));
                    if (String.Equals(cells[searchColIndex].GetText(), searchColVal, StringComparison.OrdinalIgnoreCase))
                    {
                        targetElement = cells[targetColIndex];
                        break;
                    }

                }
                if (targetElement == null)
                {
                    Log.Error("WebElementExtension.GetTableCell...Target TableCell not found. Please check your search criteria");
                } else
                {
                    Log.Information("WebElementExtension.GetTableCell..TableCell found");
                }
                
                return targetElement;
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.GetTableCellValue()...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static IList<IWebElement> GetTableRows(this IWebElement table, bool includeHeaderRow = false)
        {
            try
            {
                Log.Information("GetTableRows()...{includeHeaderRow}", includeHeaderRow);
                var rowSelector = "tbody > tr";
                if (includeHeaderRow)
                {
                    rowSelector = "tr";
                }
                IList<IWebElement> rows = table.FindElements(By.CssSelector(rowSelector));
                return rows;
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtension.GetTableRows()...{message}  {stacktrace}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static DataTable GetTableData(this IWebElement table)
        {
            Log.Information("In GetTableData()...");
            DataTable dt = new DataTable();
            dt.Clear();

            var headerColumns = table.FindElements(By.CssSelector("tr > th"));
            if (headerColumns.Count == 0)
            {
                headerColumns = table.FindElements(By.CssSelector("thead > tr > td"));
            }

            foreach(IWebElement header in headerColumns)
            {
                dt.Columns.Add(header.GetText());
            }

            IList<IWebElement> rows = table.GetTableRows(false);
            foreach (IWebElement row in rows)
            {
                IList<IWebElement> cells = row.FindElements(By.CssSelector("td"));
                int colInd = 0;
                DataRow dr = dt.NewRow();
                foreach (IWebElement cell in cells)
                {
                    dr[dt.Columns[colInd]] = cell.GetText();
                    colInd++;
                }
                dt.Rows.Add(dr);
            }
            Log.Information("In GetTableData()...end");

            return dt;

        }

        public static IWebElement OpenTreeNode(this IWebElement element, string folderName)
        {
            try
            {
                Log.Information("WebElementExtensions.OpenTreeNode()...{folderName}", folderName);
                BaseElement treeFolder = new BaseElement("Xpath",
                    String.Format("..//a[text()='{0}' and contains(@class,'jstree-anchor')]", folderName));
                IWebElement node = GetElement(element, treeFolder);
                var driver = ((IWrapsDriver)element).WrappedDriver;
                new Actions(driver).DoubleClick(node).Build().Perform();
                Thread.Sleep(5000);
                return node;
            }
            catch (Exception ex)
            {
                Log.Error("WebElementExtensions.Error in OpenTreeNode...{folderName} {message} {stacktrace}", folderName, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

    }
}