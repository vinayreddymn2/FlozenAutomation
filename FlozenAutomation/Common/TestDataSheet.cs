namespace FlozenAutomation.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Serilog;

    public class TestDataSheet
    {
        //public List<Dictionary<String, String>> Rows { get; set; }
        public List<TestDataRow> Rows { get; set; }

        /*public TestDataSheet(List<Dictionary<String, String>> rows)
        {
            this.Rows = rows;
        }*/

        public TestDataSheet(List<TestDataRow> rows)
        {
            this.Rows = rows;
        }

        public string Value(int row, string column)
        {
            if (this.Rows.Count == 0 || this.Rows.Count <= row)
            {
                Log.Error("Incorrect Data Sheet Row Number {0}, Total Rows={1}", row, this.Rows.Count);
                throw new Exception("Incorrect Data Sheet Row Number " + row);
            }

            if (!this.Rows[row].dataMap.ContainsKey(column))
            {
                Log.Error("Incorrect Data Sheet Column Name {0}", column);
                throw new Exception("Incorrect Data Sheet Column Name " + column);
            }

            return this.Rows[row].dataMap[column];
        }

        public List<TestDataRow> FilterRows(string searchCriteria = "")
        {
            List<TestDataRow> testDataRows = new List<TestDataRow>();
            foreach(TestDataRow row in this.Rows)
            {
                if (searchCriteria.Length > 0 )
                {
                    var searchColumn = searchCriteria.Split('=')[0].Trim();
                    var searchValue = searchCriteria.Split('=')[1].Trim();
                    if (row.dataMap.ContainsKey(searchColumn) && String.Equals(row.dataMap[searchColumn], searchValue))
                    {
                        testDataRows.Add(row);
                    }
                }
                else
                {
                    testDataRows.Add(row);
                }
            }
            return testDataRows;
        }
    }
}
