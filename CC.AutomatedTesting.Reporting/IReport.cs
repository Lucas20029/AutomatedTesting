using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
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
    public enum TestCaseResultStatus
    {
        Unexecuted = 0,
        Pass =1,
        Fail=2,
        Exception =3
    }
    public interface ITestReport:IDisposable
    {
        void RecordCase(string testCaseName, TestCaseResultStatus status, string message=""); 
        
        ITestReport SetEnvironment(string environment); 

    }
}
