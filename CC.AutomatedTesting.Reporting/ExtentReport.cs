using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Reporting
{
    //业务需求：
    // 1. 提供一个方法，记录Case是失败还是成功。失败的话，可以写原因。
    //    （规定：Fatal级别是异常导致的，Fail是验证失败导致的）
    // 2. 周围设置：可以改报告存放地址、环境、启动用户名字、
    // 其他操作，用户不需感知。
  
    class ExtentReport : ITestReport
    {
        private string reportDirectory = "C:\\CC.logfiles\\自动化测试报表\\";
        public ExtentReports extent;
        public ExtentTest test;
        public string ReportPath { get; private set; }
        public string ReportName { get; private set; }
        public string ReportFullName => ReportPath + ReportName;
        #region 构造方法
        public ExtentReport(string reportName)
        {
            //ReportPath = GetDefaultReportPath();
            ReportPath = reportDirectory;
            ReportName = DateTime.Now.ToString("yyyyMMddHHmmss")+".html";
            if (!Directory.Exists(ReportPath))
            {
                Directory.CreateDirectory(ReportPath);
            }
            extent = new ExtentReports(ReportPath+ReportName, true);
        }
        public ExtentReport(string fileName, string filePath, bool replaceExisting=true)
        {
            if (!filePath.EndsWith("\\"))
                filePath = filePath + "\\";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (!fileName.EndsWith(".html",StringComparison.OrdinalIgnoreCase))
                fileName += ".html";
            ReportName = fileName;
            ReportPath = filePath;
            extent = new ExtentReports(ReportPath + ReportName, true);

        }
        /// <summary>
        /// 返回当前应用程序的所在目录。通常是在Bin文件下面
        /// </summary>
        /// <returns></returns>
        private string GetDefaultReportPath()
        {
            string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string projectPath = new Uri(path).LocalPath;
            return projectPath.Substring(0,projectPath.LastIndexOf('\\')) + "Reports\\";
        }
        #endregion

        #region 接口内容
        public void Dispose()
        {
            if (extent != null)
            {
                if (test != null)
                {
                    extent.EndTest(test);
                }
                extent.Flush();
                extent.Close();
            }
        }
        public ITestReport SetEnvironment(string environment)
        {
            throw new NotImplementedException();
        }
        
        public void RecordCase(string testCaseName, TestCaseResultStatus status, string message = "")
        {
            test = extent.StartTest(testCaseName);
            test.Log(status.ToExtentStatus(), message);
            extent.EndTest(test);
        }
        #endregion
    }

    public static class ExtentReportEx
    {
        public static LogStatus ToExtentStatus(this TestCaseResultStatus status)
        {
            switch (status)
            {
                case TestCaseResultStatus.Pass:
                    return LogStatus.Pass;
                case TestCaseResultStatus.Exception:
                    return LogStatus.Fatal;
                case TestCaseResultStatus.Fail:
                    return LogStatus.Fail;
                case TestCaseResultStatus.Unexecuted:
                    return LogStatus.Skip;
            }
            return LogStatus.Unknown;
        }
    }

    //ExtentReport使用DEMO
    //public class ReportHelper
    //{
    //    public ExtentReports extent;
    //    public ExtentTest test;



    //    public void StartReport()
    //    {
    //        string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
    //        string actualPath = path.Substring(0, path.LastIndexOf("bin"));
    //        string projectPath = new Uri(actualPath).LocalPath;
    //        string reportPath = projectPath + "Reports\\MyOwnReport.html";

    //        extent = new ExtentReports(reportPath, true);
    //        extent
    //        .AddSystemInfo("Host Name", "Snow Host")
    //        .AddSystemInfo("Environment", "Testing")
    //        .AddSystemInfo("User Name", "Snow Forever");
    //        extent.LoadConfig(projectPath + "extent-config.xml");
    //    }

    //    public void DoTest()
    //    {
    //        test = extent.StartTest("DemoReportPass");
    //        test.Log(LogStatus.Pass, "Assert Pass as condition is True");
    //        test = extent.StartTest("DemoReportFail");
    //        test.Log(LogStatus.Fail, "Assert Pass as condition is Fail");
    //        //Error、Fail、Fatal、Info、Pass、Skip、Unknown、Warning
    //        extent.EndTest(test);
    //        extent.Flush();
    //        extent.Close();
    //    }
    //}
}
