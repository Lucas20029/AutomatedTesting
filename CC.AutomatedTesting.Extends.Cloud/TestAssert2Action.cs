using CC.Meeting.DataConvert.CommonConvert;
using CC.AutomatedTesting.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.CCBiz
{
    [ActionMethod("测试断言2")]
    public class TestAssert1Action : AssertAction
    {

        public override AssertionResult Assert()
        {
            if (Parameters[0].To<int>() == 0) //验证成功
            {
                return AssertionResult.Success("第一列列名是" + Parameters[1]);
            }
            else if (Parameters[0].To<int>() == 1) //验证未通过
            {
                return AssertionResult.Fail($"第一列是XXOO", $"第一列列名应该是{Parameters[1]}");
            }
            else //执行过程中，抛出异常
            {
                throw new Exception("测试断言2出现异常");
            }
        }

        public override void PostConditionCheck()
        {
        }

        public override void PreConditionCheck()
        { 
        }
    }
}
