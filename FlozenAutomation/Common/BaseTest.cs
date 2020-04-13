namespace FlozenAutomation.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AventStack.ExtentReports;
    using AventStack.ExtentReports.Reporter;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Serilog;
    using Serilog.Context;
    using FlozenAutomation.Common;

    [SetUpFixture]
    public abstract class BaseTest
    {
        private readonly DriverManager driverManager = new DriverManager();

        protected DriverManager DriverManager
        {
            get
            {
                return this.driverManager;
            }
        }

        public static ExtentReports Report;
        public static ExtentHtmlReporter htmlReport;
        public static ExtentTest Test;

        public static string DataRandomToken { get; set; }
        public static long TestRunNum { get; set; }

        [SetUp]
        public void BeforeAnyTest()
        {
            DriverManager.StartLogging(TestRunNum, TestContext.CurrentContext.Test.Name);

            DriverManager.Init();
            Test = Report.CreateTest(TestContext.CurrentContext.Test.Name);
            Log.Information("### Start of Test {TestName} ", TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void AfterAnyTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                                       ? "" : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            var errorMessage = TestContext.CurrentContext.Result.Message;
            switch (status)
            {
                case TestStatus.Failed:
                    Test.Log(Status.Fail, "Test ended with " + Status.Fail + " – " + errorMessage + " " + stacktrace);
                    string screenshotPath = DriverManager.CaptureScreenshot(TestContext.CurrentContext.Test.Name, TestRunNum);
                    Test.AddScreenCaptureFromPath(screenshotPath);
                    break;
                case TestStatus.Skipped:
                    Test.Log(Status.Skip, "Test ended with " + Status.Skip);
                    break;
                default:
                    Test.Log(Status.Pass, "Test ended with " + Status.Pass);
                    break;
            }

            Log.Information("### End of Test {TestName} ", TestContext.CurrentContext.Test.Name + ", Result: " + status);

            Report.Flush();
            DriverManager.Close();
            DriverManager.StopLogging();
        }

        [OneTimeSetUp]
        protected void BeforeAllTests()
        {
            if (Report == null)
            {
                TestRunNum = DataManager.RandomInt(8);
                var reportsFolder = ConfigManager.ReportsFolder + "\\" + TestRunNum;
                ConfigManager.CreateFolder(reportsFolder);
                var fileName = this.GetType().ToString() + ".html";
                var htmlReporter = new ExtentHtmlReporter(reportsFolder + "\\" + fileName);
                htmlReporter.Config.DocumentTitle = ConfigManager.ReportTitle;
                htmlReporter.Config.ReportName = ConfigManager.BuildNum;
                Report = new ExtentReports();
                Report.AttachReporter(htmlReporter);

                //DriverManager.StartLogging(TestRunNum);
                Log.Information("Start of Test Execution...");

                DataRandomToken = TestContext.Parameters.Get("DataRandomToken",
                                            (ConfigManager.DataRandomToken.Length > 0)
                                                ? ConfigManager.DataRandomToken
                                                : UtilManager.RandomAlphaNumString(4));

                Log.Information("Data Random Token={DataRandomToken}", DataRandomToken);
            }
        }

        /*[OneTimeTearDown]
        protected void AfterAllTests()
        {
            Report.Flush();
            
            if (ConfigManager.AttachExecLogsWithEmail)
                UtilManager.BundleResultFiles(TestRunNum);

            MailManager.SendLatestTestReport(TestRunNum);

            Log.Information("***All Tests Completed***");
            DriverManager.StopLogging();
        }*/
    }
}