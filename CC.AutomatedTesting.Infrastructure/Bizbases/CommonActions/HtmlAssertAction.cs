using CC.AutomatedTesting.Infrastructure.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Bizbases.CommonActions
{
    public class HtmlAssertAction : AssertAction
    {

        public string Version { get; private set; }
        public List<HtmlScript> HtmlScripts { get; private set; }
        public HtmlAssertAction(string actionName, List<HtmlScript> htmlScripts)
        { 
            ActionName = actionName;
            HtmlScripts = htmlScripts;
        }
        public override AssertionResult Assert()
        {
            foreach (var script in HtmlScripts)
            {
                //TODO：执行Script。参考雪姣南京实现和RobotFramework
            }
            return base.Assert();
        }

        public override void PreConditionCheck()
        {
            //throw new NotImplementedException();
        }

        public override void PostConditionCheck()
        {
            //throw new NotImplementedException();
        }
    }
    
}
