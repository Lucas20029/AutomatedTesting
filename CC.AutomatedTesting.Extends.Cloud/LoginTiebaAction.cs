using CC.Meeting.DataConvert.CommonConvert;
using CC.AutomatedTesting.Infrastructure;
using CC.AutomatedTesting.Infrastructure.Exceptions;
using CC.AutomatedTesting.Infrastructure.Extensions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CC.AutomatedTesting.CCBiz
{
    [ActionMethod("登录贴吧")]
    [VersionControl(Range = "1..")]
    public class LoginTiebaAction : PerformAction
    {
        public LoginTiebaAction()
        {
        }
        public override void Perform()
        {
            WebDriver.Navigate().GoToUrl("https://tieba.baidu.com/index.html");

            /*
            //var userBar = WebDriver.SafeFindElement(By.Id("com_userbar"));
            //var loginBtn = userBar.SafeFindElement(By.CssSelector(".u_login a"));
            var loginBtn = WebDriver.SafeFindElement(By.CssSelector("#com_userbar .u_login .u_menu_item a"));

            loginBtn.Click();
            var userPwdLogin = WebDriver.SafeFindElement(By.ClassName("tang-pass-footerBarULogin"));
            userPwdLogin.Click();
            var userName = WebDriver.SafeFindElement(By.Id("TANGRAM__PSP_10__userName"));
            var pwd = WebDriver.SafeFindElement(By.Id("TANGRAM__PSP_10__password"));
            userName.SendKeys(Parameters[0]);
            pwd.SendKeys(Parameters[1]);
            var submitBtn = WebDriver.SafeFindElement(By.Id("TANGRAM__PSP_10__submit"));
            submitBtn.Click();
            //var safeForceWin = WebDriver.SafeFindElement(By.ClassName("pass-forceverify-wrapper"));
            //if(safeForceWin!=null)//如果弹出来了【安全验证】的窗口，则直接关闭掉
            //{
            //    var closeBtn = safeForceWin.SafeFindElement(By.ClassName("forceverify-header-a"));
            //    closeBtn.Click();
            //}
            Waiter.UntilDisappear(By.ClassName("passport-login-pop"),WebDriver);
            */

        }

        public override void PostConditionCheck()
        {
        }

        public override void PreConditionCheck()
        {
        }
    }
}
