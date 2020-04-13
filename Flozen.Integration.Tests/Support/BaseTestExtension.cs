using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FlozenAutomation.Common;

[SetUpFixture]
public class BaseTestExtension
{
    public BaseTestExtension()
    {

    }

    [OneTimeTearDown]
    public void AfterAllNUnitTets()
    {
        FlozenAutomation.Common.BaseTest.Report.Flush();

        if (ConfigManager.AttachExecLogsWithEmail)
            UtilManager.BundleResultFiles(FlozenAutomation.Common.BaseTest.TestRunNum);

        MailManager.SendLatestTestReport(FlozenAutomation.Common.BaseTest.TestRunNum);
    }
}

