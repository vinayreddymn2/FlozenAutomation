namespace FlozenAutomation.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Serilog;

    public static class UtilManager
    {
        public static string RandomAlphaNumString(int length=4)
        {
            var allowedChars = Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length);
            var alphaNumString = new string(allowedChars.SelectMany(str => str).OrderBy(c => Guid.NewGuid()).Take(length).ToArray());

            return alphaNumString;
        }

        public static string ChromeVersion()
        {
            try
            {
                string chromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                string chromeVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(chromePath).ProductVersion;
                Console.WriteLine("Chrome Version:" + chromeVersion);

                return chromeVersion;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred in GetChromeVersion:{0}", ex.Message);
                return "";
            }
        }

        public static int CopyFiles(string sourceFolder, string destFolder)
        {
            int count = 0;
            try
            {
                if (Directory.Exists(sourceFolder) && Directory.Exists(destFolder))
                {
                    string[] files = System.IO.Directory.GetFiles(sourceFolder);
                    foreach (string sourceFile in files)
                    {
                        var destFile = System.IO.Path.Combine(destFolder, System.IO.Path.GetFileName(sourceFile));
                        System.IO.File.Copy(sourceFile, destFile, true);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in CopyFiles()...{message}, {stack}", ex.Message, ex.StackTrace);
            }
            
            return count;
        }

        public static string BundleResultFiles(long testRunNum)
        {
            String bundleFile = String.Empty;
            try
            {
                string logsFolder = Path.Combine(ConfigManager.LogsFolder, Convert.ToString(testRunNum));
                string dataFolder = Path.Combine(ConfigManager.LogsFolder, Convert.ToString(testRunNum), "Json");
                string ssFolder = Path.Combine(ConfigManager.ScreenshotsFolder, Convert.ToString(testRunNum));
                string reportsFolder = Path.Combine(ConfigManager.ReportsFolder, Convert.ToString(testRunNum));

                string execFolder = Path.Combine(reportsFolder, "ExecLogs");
                ConfigManager.CreateFolder(execFolder);

                CopyFiles(reportsFolder, execFolder);
                CopyFiles(ssFolder, execFolder);
                CopyFiles(logsFolder, execFolder);
                CopyFiles(dataFolder, execFolder);

                bundleFile = Path.Combine(reportsFolder, "ExecLogs-" + testRunNum + ".zip");
                Log.Information("Execution Logs Bunde {zipFile}", bundleFile);
                ZipFile.CreateFromDirectory(execFolder, bundleFile);

                Directory.Delete(execFolder, true);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BundleResultFiles() method...{message} {stack}", ex.Message, ex.StackTrace);
            }
            return bundleFile;
        }
    }
}
