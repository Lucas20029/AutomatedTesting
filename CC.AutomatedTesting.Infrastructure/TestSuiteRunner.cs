using CC.AutomatedTesting.Infrastructure.ActionFactory;
using CC.AutomatedTesting.Infrastructure.ParallelRun;
using CC.AutomatedTesting.Reporting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class TestSuiteRunner
    {
        public ITestReport ReportEngine { get; set; }
        public bool IsPaused { get; private set; } = false;

        private readonly  static TaskFactory RunnerTaskFactory = new TaskFactory();
        private ManualResetEvent RunnerResetEvent;
        private TestCaseExecuter Executer;
        //应用内只有一个WebDriver
        private static IWebDriver webDriver;
        private static IWebDriver WebDriver
        {
            get
            {
                if(webDriver==null)
                    webDriver = WebDriverFactory.GetInstance(WebDriverType.LocalChrome);
                return webDriver;
            }
        }
        /// <summary>
        /// 执行TestSuite。返回Report的结果
        /// </summary>
        /// <param name="suite"></param>
        /// <returns></returns>
        public void Run(TestSuite testSuite)
        {
            //RunnerTaskFactory.StartNew(() =>
            //{
            //    RunnerResetEvent = new ManualResetEvent(true);
                using (ReportEngine = ReportFactory.CreateInstance(testSuite.SuiteName))
                {
                    Executer = new TestCaseExecuter(WebDriver, RunnerResetEvent);
                    bool isProcessBreak = false;
                    foreach (var testCase in testSuite.TestCases)
                    {
                        if (!isProcessBreak)
                        {
                            //可能结果：Pass、Fail、Fatal 或者抛出异常
                            var caseResult = Executer.Run(testCase);
                            //需要扩展方法：把caseResult的Exception、Details直接TOString（）
                            ReportEngine.RecordCase(testCase.Name, caseResult.Status, caseResult.GenerateResultMessage());
                            //当断言验证失败时，还继续执行，不中断
                            if (caseResult.Status == TestCaseResultStatus.Fail || caseResult.Status == TestCaseResultStatus.Exception)
                            {
                                isProcessBreak = true;
                            }
                        }
                        else
                        {
                            ReportEngine.RecordCase(testCase.Name, TestCaseResultStatus.Unexecuted, "");
                        }
                        //先判断isProcessBreak==true？。如果是，则执行Case。不是，直接打Unexecuted结果
                        //TODO:对于Pass的Case，调用ReportEngine，记录一个Pass结果
                        //     对于Fail的Case，调用ReportEngine，记录一个Fail结果。并终止所有case的执行，isProcessBreak=true，把未执行的标记为Unexecuted。
                        //     对于Exception的Case，调用ReportEngine，记录一个Fatal结果。并终止所有case的执行，isProcessBreak=true，把未执行的标记为Unexecuted。
                        //     对于isProcessBreak=true，调用ReportEngine，把所有的标记为Unexecuted
                    }
                }
            //});
        }
        
        public void Pause()
        {
            IsPaused = true;
            RunnerResetEvent?.Reset();
        }

        public void Continue()
        {
            IsPaused = false;
            RunnerResetEvent?.Set();
        }
    }
}
