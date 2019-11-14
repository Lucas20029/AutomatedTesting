using CC.AutomatedTesting.Infrastructure;
using CC.AutomatedTesting.Infrastructure.Extensions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.CCBiz
{
    [ActionMethod("表格中存在链接")]
    [VersionControl(Range = "0..2")]
    public class AssertTableExistLinkTextV1 : AssertAction
    {
        public AssertTableExistLinkTextV1()
        {
            this.ActionRequiredIframeIDPath = new List<string>() { "iTalFrame" };
        }
        public override AssertionResult Assert()
        {
            var cellText = Parameters[0];
            try
            {
                Waiter.Until(p => p.FindElement(By.LinkText(cellText))); //等待该元素出现
                //在表格中查找
                var targetLink = WebDriver.SafeFindElement(By.ClassName("dataGrid"))?.FindElement(By.LinkText(cellText));
                if (targetLink == null)
                    throw new Exception();
                return AssertionResult.Success("验证通过，存在" + cellText);

            }
            catch (Exception ex)
            { 
                var screenShotAddress = WebDriver.ScreenShot();
                return AssertionResult.Fail("未找到该元素", "表格中存在链接" + cellText, screenShotAddress);
            }

        }

        public override void PostConditionCheck()
        {
            //throw new NotImplementedException();
        }

        public override void PreConditionCheck()
        {
            //throw new NotImplementedException();
        }
    }

    [ActionMethod("表格中存在链接")]
    [VersionControl(Range = "2..")]
    public class AssertTableExistLinkTextV2 : AssertAction 
    {
        public AssertTableExistLinkTextV2()  
        {
            this.ActionRequiredIframeIDPath = new List<string>() { "iTalFrame" };
        }
        public override AssertionResult Assert()
        {
            var cellText = Parameters[0];
            try
            {
                Waiter.Until(p => p.FindElement(By.LinkText(cellText))); //等待该元素出现
                //在表格中查找
                var targetLink = WebDriver.FindElement(By.ClassName("el-table__body"))?.FindElement(By.LinkText(cellText));
                if (targetLink == null)
                    throw new Exception();
                return AssertionResult.Success("验证通过，存在"+cellText);

            }
            catch (Exception ex)
            {
                var screenShotAddress = WebDriver.ScreenShot();
                return AssertionResult.Fail("未找到该元素", "表格中存在" + cellText, screenShotAddress);
            }
            
        }

        public override void PostConditionCheck()
        {
            //throw new NotImplementedException();
        }

        public override void PreConditionCheck()
        {
            //throw new NotImplementedException();
        }
    }
}
