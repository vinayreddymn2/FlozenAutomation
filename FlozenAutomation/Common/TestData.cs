namespace FlozenAutomation.Common
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Data;
    using System.Collections.Generic;
    using Serilog;

    public class TestData
    {
        public Dictionary<String, List<Dictionary<String, String>>> Data { get; set; }

        public string DataRandomToken { get; set; }

        public TestData(Dictionary<String, List<Dictionary<String, String>>> data)
        {
            this.Data = data;
        }

        public TestDataSheet Get(String sheetName)
        {
            //List<Dictionary<String, String>> lstRows = new List<Dictionary<String, String>>();
            List<TestDataRow> lstRows = new List<TestDataRow>();
            try { 
                if (!Data.ContainsKey(sheetName))
                {
                    Log.Error("Incorrect sheet name {sheetName}", sheetName);
                    throw new Exception("Incorrect sheet name " + sheetName);
                }
                List<Dictionary<string, string>> temp = Data[sheetName];
                foreach (Dictionary<string, string> rowDict in Data[sheetName])
                {
                    lstRows.Add(new TestDataRow(rowDict));
                }
                //lstRows = Data[sheetName];
            } catch (Exception ex)
            {
                throw ex;
            }
            /*
            DataTable dt = Data[SheetName];
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<String, String> dictRow = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    var cName = col.ColumnName;
                    var cVal = row[col].ToString().Replace("{{DRT}}", this.DataRandomToken);
                    cVal = DataManager.ReplaceRandomTokens(cVal);                                
                    dictRow.Add(cName, cVal);
                }
                lstRows.Add(dictRow);
            }
            */
            TestDataSheet rows = new TestDataSheet(lstRows);
            return rows;
        }

        public string Value(string sheet, int row, string column)
        {
            if (this.Get(sheet).Rows.Count == 0 || this.Get(sheet).Rows.Count <= row)
            {
                Log.Error("Incorrect Data Sheet Row Number {0}, Total Rows={1}", row, this.Get(sheet).Rows);
                throw new Exception("Incorrect Data Sheet Row Number " + row);
            }

            if (!this.Get(sheet).Rows[row].dataMap.ContainsKey(column))
            {
                Log.Error("Incorrect Data Sheet Column Name {0}", column);
                throw new Exception("Incorrect Data Sheet Column Name " + column);
            }

            return this.Get(sheet).Rows[row].dataMap[column];
        }
    }
}
