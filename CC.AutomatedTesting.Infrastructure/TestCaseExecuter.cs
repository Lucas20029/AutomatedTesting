using CC.AutomatedTesting.Infrastructure.ActionFactory;
using CC.AutomatedTesting.Infrastructure.Exceptions;
using CC.AutomatedTesting.Infrastructure.Extensions;
using CC.AutomatedTesting.Reporting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    /*TestCase结果规则：
     *  1. 如果某个Action（包括Perform和Assert）出现异常，则整个Case结果为Exception，并中断执行，返回结果
     *  2. 如果所有Action都没异常，会执行所有的PerformAction，之后执行所有的AssertAction，并记录所有AssertAction的结果
     *  3.     如果所有AssertAction结果都是OK，则整个Case结果为Pass；任一AssertAction不是OK，整个结果为Fail
     */
    public class TestCaseExecuter
    {
        public TestCaseExecuter(IWebDriver _webDriver, ManualResetEvent _executerResetEvent)
        {
            WebDriver = _webDriver;
            ExecuterAutoResetEvent=_executerResetEvent;
        }
        public IWebDriver WebDriver { get; private set; }
        private ManualResetEvent ExecuterAutoResetEvent;

        public TestCaseResult RunPerformAction(PerformAction action)
        {
            try
            {
                action.WebDriver = WebDriver;
                action.SwitchToActionRequiredIframe();
                action.Excuting();
                action.PreConditionCheck();
                action.Perform();
                action.PostConditionCheck();
                action.Success();
                return new TestCaseResult()
                {
                    Status = TestCaseResultStatus.Pass
                };
            }
            catch (RuntimeException ex)
            {
                //action.Fail(ex);
                ex.ActionName = action.ActionName;
                //LogHelper.Error("RuntimeException Exception Happens:" + ex.ToString());
                var screenShotAddress = WebDriver.ScreenShot();
                //Case执行中，一旦发生异常，就退出，不再继续执行
                return new TestCaseResult()
                {
                    Status = TestCaseResultStatus.Exception,
                    ExceptionInfo = new Tuple<Action, Exception, string>(action, ex, screenShotAddress)
                };
            }
            catch (Exception ex)
            {
                //action.Fail(ex);
                //LogHelper.Error("OrdinaryActions Exception Happens:" + ex.ToString());
                var screenShotAddress = WebDriver.ScreenShot();
                //Case执行中，一旦发生异常，就退出，不再继续执行
                return new TestCaseResult()
                {
                    Status = TestCaseResultStatus.Exception,
                    ExceptionInfo = new Tuple<Action, Exception, string>(action, ex, screenShotAddress)
                };
            }
        }

        public AssertionResult RunAssertAction(AssertAction assertion)
        {
            try
            {
                assertion.WebDriver = WebDriver;
                assertion.SwitchToActionRequiredIframe();
                assertion.Excuting();
                assertion.PreConditionCheck();
                AssertionResult currentResult = assertion.Assert();//当前这个断言的结果
                assertion.PostConditionCheck();
                assertion.Success();
                return currentResult;
            }
            catch (Exception ex)//用户自定义的异常
            {
                assertion.Fail(ex);
                Console.WriteLine("AssertActions Exception Happens:" + ex.ToString());
                return null;
                //var screenShotAddress = webDriver.ScreenShot();
                ////调用selennium进行截图。
                //return new TestCaseResult()
                //{
                //    Status = TestCaseResultStatus.Exception,
                //    ExceptionInfo = new Tuple<Action, Exception, string>(assertion, ex, screenShotAddress),
                //    ResultDetails = resultDetails
                //};
            }
        }

        public TestCaseResult Run(TestCase testcase)
        {
            var tempCurrentIframePath = new List<string>();
            Dictionary<AssertAction, AssertionResult> resultDetails = new Dictionary<AssertAction, AssertionResult>();
            Stopwatch stopWatch = new Stopwatch();
            foreach (var action in testcase.Actions)
            {
                stopWatch.Start();
                //ExecuterAutoResetEvent.WaitOne();
                if (action is FunctionAction)
                {
                    var funcAction = action as FunctionAction;
                    Console.WriteLine("执行函数调用：" + funcAction.CommandText);
                    foreach (var bodyaction in funcAction.BodyActions)
                    {
                        if (!(bodyaction is PerformAction))
                            continue;
                        Console.WriteLine("  --执行:" + bodyaction.AnalyziedCommandText);
                        var bodyPerformAction = bodyaction as PerformAction;
                        RunPerformAction(bodyPerformAction);
                    }
                }
                else if (action is PerformAction)
                {
                    Console.WriteLine("执行:" + action.AnalyziedCommandText);
                    var performAction = action as PerformAction;
                    RunPerformAction(performAction);
                }
                else if (action is AssertAction)
                {
                    Console.WriteLine("验证:" + action.AnalyziedCommandText);
                    var assertAction = action as AssertAction;
                    var assertResult = RunAssertAction(assertAction);
                    if (assertResult != null) //断言正常结束：断言成功、失败
                    {
                        resultDetails.Add(assertAction, assertResult);
                        if(!assertResult.IsOk)
                        {
                            action.ExecuteState = Action.ActionExecuteState.AssertionFail;
                            Console.WriteLine($"断言验证失败。期望结果：{assertResult.ExpectedResult}，实际结果{assertResult.ActualResult}");
                            //Console.WriteLine("点击任何按键继续");
                            //Console.ReadKey();
                        }
                        else
                        {
                            action.ExecuteState = Action.ActionExecuteState.AssertionPass;
                            Console.WriteLine("******断言验证通过******");
                        }
                    }
                }
                if (action.ExecuteState == Action.ActionExecuteState.Failed)
                {
                    Console.WriteLine($"命令执行失败：{action.Message}、\r\n堆栈信息\r\n：{action.InnerException.ToString()}");
                    Console.WriteLine("点击任何按键继续");
                    Console.ReadKey();
                    //暂停执行，卡住
                }
                stopWatch.Stop();
                Console.WriteLine($"{action}命令执行时间为：{stopWatch.Elapsed}");
            }

            return new TestCaseResult()
            {
                //如果验证结果有一个的IsOK为false，则整个Case结果为Fail
                Status = resultDetails.Values.Any(p => p.IsOk == false) ? TestCaseResultStatus.Fail : TestCaseResultStatus.Pass,
                ResultDetails = resultDetails
            };
        }
    }

    public class TestCaseResult
    {
        //总体状态
        public TestCaseResultStatus Status { get; set; }
        //如果走到了断言，记录每个断言的结果
        public Dictionary<AssertAction, AssertionResult> ResultDetails { get; set; }
        //如果有异常，记录异常发生的Action、异常信息、截图地址
        public Tuple<Action, Exception, string> ExceptionInfo { get; set; }

        public string GenerateResultMessage()
        {
            if (Status == TestCaseResultStatus.Pass)
            {
                return "";
            }
            else if (Status == TestCaseResultStatus.Fail)
            {
                return AssertionResult;
            }
            else if (Status == TestCaseResultStatus.Exception)
            {
                return ExceptionResult /*+ "\r\n验证执行结果：\r\n" + AssertionResult*/;
            }
            else if (Status == TestCaseResultStatus.Unexecuted)
            {
                return "未执行";
            }
            return "未知";
        }

        private string AssertionResult
        {
            get
            {
                if (ResultDetails != null)
                {
                    return string.Join("\r\n", ResultDetails.Select(p => p.Key.CommandText + "：" + p.Value.ToString()).ToArray());
                }
                return "";
            }
        }
        private string ExceptionResult
        {
            get
            {
                if (ExceptionInfo != null)
                {
                    var exceptionMessage = ExceptionInfo.Item2.Message;
                    if (ExceptionInfo.Item2 is RuntimeException)
                    {
                        var ex = ExceptionInfo.Item2 as RuntimeException;
                        return "执行 " + ExceptionInfo.Item1.CommandText + " 时，出现异常： " + ex.ToUserInfo() + " 。\r\n截图地址：" + ExceptionInfo.Item3;
                    }
                    return "执行 " + ExceptionInfo.Item1.CommandText + " 时，出现异常： " + ExceptionInfo.Item2.Message + "。\r\n截图地址：" + ExceptionInfo.Item3;
                }
                return "";
            }
        }
    }
}
