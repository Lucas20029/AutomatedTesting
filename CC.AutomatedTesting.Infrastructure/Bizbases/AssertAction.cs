using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public abstract class AssertAction : Action, IAssert
    {
        //业务端实现的时候，需要根据DOM元素的比较结果，返回不同的AssertionResult  Success 或 Fail(是否有截图，业务端可以自己决定)
        public virtual AssertionResult Assert()
        {
            return AssertionResult.Success("该断言未实现");
        }
    }

    public class AssertionResult
    {
        public bool IsOk { get; set; }
        public string ActualResult { get; set; }
        public string ExpectedResult { get; set; }
        public string ScreenShot { get; set; }

        public static AssertionResult Success(string actualResult="")
        {
            return new AssertionResult()
            {
                IsOk = true,
                ActualResult = actualResult,
                ExpectedResult = actualResult
            };
        }
        public static AssertionResult Fail(string actualResult, string expectedResult)
        {
            return new AssertionResult()
            {
                IsOk = false,
                ActualResult = actualResult,
                ExpectedResult = expectedResult
            };
        }
        public static AssertionResult Fail(string actualResult, string expectedResult, string screenShot)
        {
            return new AssertionResult()
            {
                IsOk = false,
                ActualResult = actualResult,
                ExpectedResult = expectedResult,
                ScreenShot=screenShot
            };
        }
        public override string ToString()
        {
            string resultInfo = IsOk ?"成功":"失败";
            return $"验证{resultInfo}：期望结果：{ExpectedResult}，实际结果：{ActualResult}，屏幕截图：{ScreenShot}";
        }

        public static AssertionResult Fail(object p, string screenShotAddress)
        {
            throw new NotImplementedException();
        }
    }
}
