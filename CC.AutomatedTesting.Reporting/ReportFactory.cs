using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Reporting
{
    public class ReportFactory
    {
        public static ITestReport CreateInstance(string reportTitle)
        {
            //本Dll对外返回的类
            return new ExtentReport(reportTitle);
        }
    }
}
