using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlozenAutomation.Common
{
    public class TestDataRow
    {
        public Dictionary<String, String> dataMap { get; set; }

        public TestDataRow(Dictionary<String, String> row)
        {
            this.dataMap = row;
        }

        public string Value(string key)
        {
            if (dataMap.ContainsKey(key))
            {
                return dataMap[key];
            } else
            {
                throw new Exception("Invalid Column Name " + key);
            }
        }
    }
}
