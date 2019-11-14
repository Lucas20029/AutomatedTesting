using CC.Meeting.DataConvert.CommonConvert;
using CC.AutomatedTesting.Infrastructure;
using CC.AutomatedTesting.Infrastructure.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.CCBiz
{
    [ActionMethod("暂停")]
    public class ThreadSleepAction : PerformAction
    {
        public override void Perform()
        {
            var milSecondStr = Parameters[0];
            int milSecondToWait = 1000;
            if(string.IsNullOrEmpty(milSecondStr) || int.TryParse(milSecondStr,out milSecondToWait))
            {
                Thread.Sleep(milSecondToWait);
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
