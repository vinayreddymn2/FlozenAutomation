namespace FlozenAutomation.Common
{
    using ExcelDataReader;
    using Fare;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using Serilog;

    public static class DataManager
    {
        private static DataTableCollection ReadTestDataColl(String DataFileName)
        {

            if (!DataFileName.ToLower().EndsWith(".xlsx")) {
                DataFileName += ".xlsx";
            }
            var dataFileFullPath = ConfigManager.DataFolder + "\\" + DataFileName;
            Console.WriteLine(dataFileFullPath);
            if (!File.Exists(dataFileFullPath))
            {
                Log.Error("Data File {datafile} doesn't exist. Please check the path and file", dataFileFullPath);
                throw new Exception("Incorrect Data File " + dataFileFullPath);
            }

            FileStream fs = File.Open(dataFileFullPath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            DataTableCollection dTableColl = ds.Tables;

            return dTableColl;
        }

        public static TestData GetTestData(string dataFileName, string dataRandomToken, long TestRunNum)
        {
            DataTableCollection tblColl = ReadTestDataColl(dataFileName);

            Dictionary<String, List<Dictionary<String, String>>> dataSheets =
                new Dictionary<String, List<Dictionary<String, String>>>();

            foreach (DataTable dt in tblColl)
            {
                var sheetName = dt.TableName;
                List<Dictionary<String, String>> lstRows = new List<Dictionary<String, String>>();
                foreach (DataRow row in dt.Rows)
                {
                    Dictionary<String, String> dictRow = new Dictionary<string, string>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        var cName = col.ColumnName;
                        var cVal = row[col].ToString().Replace("{{DRT}}", dataRandomToken);
                        cVal = DataManager.ReplaceRandomTokens(cVal);
                        dictRow.Add(cName, cVal);
                    }

                    lstRows.Add(dictRow);
                }

                dataSheets[sheetName] = lstRows;
            }

            try
            {
                var jsonFolder = ConfigManager.LogsFolder + "\\" + TestRunNum + "\\Json\\";
                ConfigManager.CreateFolder(jsonFolder);
                var jsonFileFullPath = jsonFolder + dataFileName + ".json";
                Log.Information("Runtime Data File Contents (JSON) {jsonfile}", jsonFileFullPath);
                using (StreamWriter file = File.CreateText(jsonFileFullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, dataSheets);
                }
            } 
            catch (Exception ex) {
                Log.Error("Error when converting datasheet to json {message}", ex.Message);
            }

            TestData tData = new TestData(dataSheets);

            return tData;
        }

        public static String RandomEmail()
        {
            return Guid.NewGuid().ToString() + "@mailnator.com"; ;
        }

        public static String RandomPAN()
        {
            var xeger=new Xeger(@"[A-Z]{5}\d{4}[A-Z]{1}");
            var randomPAN = xeger.Generate();
            
            return randomPAN;
        }

        public static Int64 RandomInt(int length=4)
        {
            var xeger=new Xeger(@"\d{" + length + "}");
            return Convert.ToInt64(xeger.Generate());
        }

        public static String ReplaceRandomTokens(string str)
        {
            string finalToken = str;

            // Random PAN
            finalToken = finalToken.Replace("{{RAND-PAN}}", DataManager.RandomPAN());

            // Random Email
            finalToken = finalToken.Replace("{{RAND-EMAIL}}", DataManager.RandomEmail());

            // Random Number
            try
            {
                MatchCollection mcoll = Regex.Matches(str, @"{{RAND-NUM\d*}}");
                foreach (Match mch in mcoll)
                {
                    var token = mch.Value.ToString();
                    Regex re = new Regex(@"^\d$");
                    var numToken = Regex.Match(token, @"\d+").Value;
                    var randLength = Convert.ToInt32((numToken != null && numToken.Length > 0) ? numToken : "5");
                    finalToken = finalToken.Replace(mch.Value, Convert.ToString(DataManager.RandomInt(randLength)));
                }
            }
            catch (Exception ex )
            {
                Log.Error("DataManager.ReplaceRandomTokens...Error RandNum block {message} {stack}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return finalToken;
        }
    }
}
