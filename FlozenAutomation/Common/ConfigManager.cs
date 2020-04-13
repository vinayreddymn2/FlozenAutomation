namespace FlozenAutomation.Common
{
    using System;
    using System.Configuration;
    using System.IO;

    public static class ConfigManager
    {
        public static string AppUrl
        {
            get { return ConfigurationManager.AppSettings["AppUrl"]; }
        }

        public static string AppName
        {
            get { return ConfigurationManager.AppSettings["AppName"]; }
        }

        public static BrowserType Browser
        {
            get {
               
                if (Enum.TryParse(ConfigurationManager.AppSettings["Browser"], out BrowserType browserType))
                {
                    return browserType;
                } else
                {
                    return BrowserType.None;
                }
           }
        }

        public static string DataFolder
        {
            get
            {
                var dataFolder = ConfigurationManager.AppSettings["DataFolder"];

                if (dataFolder.StartsWith("\\") ||  dataFolder.StartsWith(".."))
                {
                    return AppDomain.CurrentDomain.BaseDirectory + dataFolder;
                }

                return dataFolder;
            }
        }

        public static string DataRandomToken
        {
            get { return ConfigurationManager.AppSettings["DataRandomToken"]; }
        }

        public static DirectoryInfo CreateFolder(string FolderName)
        {
            if (FolderName.StartsWith("\\") || FolderName.StartsWith(".."))
            {
                FolderName = AppDomain.CurrentDomain.BaseDirectory + FolderName;
            }

            DirectoryInfo di = null;
            if (!Directory.Exists(FolderName))
            {
                di = Directory.CreateDirectory(FolderName);
            }

            return di;
        }

        public static bool EnableLogging
        {
            get
            {
                return ConfigurationManager.AppSettings["EnableLogging"].ToLower().Equals("yes") ? true : false;
            }
        }

        public static string LogsFolder
        {
            get
            {
                var logsFolder = ConfigurationManager.AppSettings["LogsFolder"];
                //GetConfigFolder(logsFolder);
                /*var dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
                logsFolder = dir + logsFolder;
                DirectoryInfo di = Directory.CreateDirectory(logsFolder);*/
                if (logsFolder.StartsWith("\\") || logsFolder.StartsWith(".."))
                {
                    logsFolder = AppDomain.CurrentDomain.BaseDirectory + logsFolder;
                }

                return logsFolder;
            }
        }

        public static string LogFileName
        {
            get { return ConfigurationManager.AppSettings["LogFileName"]; }
        }

        public static string ReportsFolder
        {
            get
            {
                var reportsFolder = ConfigurationManager.AppSettings["ReportsFolder"];

                if (reportsFolder.StartsWith("\\") || reportsFolder.StartsWith(".."))
                {
                    reportsFolder = AppDomain.CurrentDomain.BaseDirectory + reportsFolder;
                }

                return reportsFolder;
            }
        }

        public static string ReportTitle
        {
            get { return ConfigurationManager.AppSettings["ReportTitle"]; }
        }

        public static string BuildNum
        {
            get { return ConfigurationManager.AppSettings["BuildNum"]; }
        }

        public static string ScreenshotsFolder
        {
            get
            {
                var screenshotsFolder = ConfigurationManager.AppSettings["ScreenshotsFolder"];
                if (screenshotsFolder.StartsWith("\\") || screenshotsFolder.StartsWith(".."))
                {
                    screenshotsFolder = AppDomain.CurrentDomain.BaseDirectory + screenshotsFolder;
                }
                
                if (!Directory.Exists(screenshotsFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(screenshotsFolder);
                }

                return screenshotsFolder;
            }
        }

        public static string ChromeDriverPath
        {
            get {
                var chromePath = ConfigurationManager.AppSettings["ChromeDriverPath"];

                if (chromePath.StartsWith("\\") || chromePath.StartsWith(".."))
                {
                    return AppDomain.CurrentDomain.BaseDirectory + chromePath;
                }

                return chromePath;               
            }
        }
        
        public static string ChromeDriverArchivePath
        {
            get
            {
                var chromePath = ConfigurationManager.AppSettings["ChromeDriverArchivePath"];

                if (chromePath.StartsWith("\\") || chromePath.StartsWith(".."))
                {
                    return AppDomain.CurrentDomain.BaseDirectory + chromePath;
                }

                return chromePath;
            }
        }

        public static string FirefoxDriverPath
        {
            get { return ConfigurationManager.AppSettings["FirefoxDriverPath"]; }
        }

        public static string UserName
        {
            get { return ConfigurationManager.AppSettings["Username"]; }
        }

        public static string Password
        {
            get { return ConfigurationManager.AppSettings["Password"]; }
        }

        public static double MaxTimeout
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["MaxTimeout"]); }
        }

        public static double PageLoadTimeOut
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["PageLoadTimeout"]); }
        }

        public static double ImplicitWaitTime
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["ImplicitWaitTime"]); }
        }

        public static double QuickWaitTime
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["QuickWaitTime"]); }
        }

        public static bool SendEmailReport
        {
            get {
                return ConfigurationManager.AppSettings["SendEmailReport"].ToLower().Equals("yes") ? true : false;
            }
        }

        public static string SmtpServer
        {
            get { return ConfigurationManager.AppSettings["SmtpServer"]; }
        }

        public static int SmtpPort
        {
            get { return Convert.ToInt16(ConfigurationManager.AppSettings["SmtpPort"]); }
        }

        public static string SenderMail
        {
            get { return ConfigurationManager.AppSettings["SenderMail"]; }
        }

        public static string SenderMailUser
        {
            get { return ConfigurationManager.AppSettings["SenderMailUser"]; }
        }

        public static string SenderPassword
        {
            get { return ConfigurationManager.AppSettings["SenderPassword"]; }
        }

        public static string MailTo
        {
            get { return ConfigurationManager.AppSettings["MailTo"]; }
        }

        public static string MailCc
        {
            get { return ConfigurationManager.AppSettings["MailCc"]; }
        }

        public static bool AttachExecLogsWithEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["AttachExecLogsWithEmail"].ToLower().Equals("yes") ? true : false;
            }
        }
    }
}
