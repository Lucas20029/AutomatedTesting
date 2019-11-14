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
    [ActionMethod("搜索")]
    [VersionControl(Range = "1..")]
    public class SearchAction : PerformAction
    {
        public SearchAction()
        {
        }
        public override void Perform()
        {
            var searchBox = WebDriver.SafeFindElement(By.CssSelector(".j_search_form .search_ipt"));
            searchBox.SendKeys(Parameters[0]);
        }

        public override void PostConditionCheck()
        {
        }

        public override void PreConditionCheck()
        {
        }
    }
}
