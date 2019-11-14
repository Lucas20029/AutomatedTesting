using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public static class Constants
    {
        public static class Configs
        {
            public readonly static string AssertPrefix = "验证";
            public readonly static string TestCasePrefix = "测试用例";
            public readonly static string FrontEndSwitchPrefix = "前端版本";
            public readonly static string FunctionCallPrefix = "调用函数";

            public readonly static string NotaionPrefix = "//";

            public readonly static string FuncPrefix = "函数";

        }
        public readonly static char CommandParameterSpliter = '：';
        public readonly static char ParameterSpliter = '，';
    }


}
