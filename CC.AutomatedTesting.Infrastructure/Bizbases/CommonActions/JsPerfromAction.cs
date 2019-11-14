using CC.AutomatedTesting.Infrastructure.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Bizbases.CommonActions
{
    public class JsPerfromAction:PerformAction
    {
        public string Version { get; private set; }
        public string JsScript { get; private set; }
        public JsPerfromAction(string actionName,string jsScript)
        {
            ActionName = actionName;
            JsScript = jsScript;
        }
        public override void Perform()
        {
            try
            {
                ((IJavaScriptExecutor)WebDriver).ExecuteScript(JsScript);
            }
            catch (Exception ex)
            {
                throw new JavascriptRuntimeException(ActionName, ex.Message);
            }
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
