using CC.AutomatedTesting.Infrastructure.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Bizbases.CommonActions
{
    public enum ByType
    {
        ClassName,
        CssSelector,
        Id,
        LinkText,
        Name,
        PartialLinkText,
        TagName,
        XPath
    }
    public class HtmlScript
    {
        /// <summary>
        /// 根据 Id、Name、ClassName、CssSelector、XPath查找
        /// </summary>
        public ByType ByType { get; set; }
        public string ByContent { get; set; }

    }
    public class HtmlPerfromAction : PerformAction
    {

        public string Version { get; private set; }
        public List<HtmlScript> HtmlScripts { get; private set; }
        public HtmlPerfromAction(string actionName, List<HtmlScript> htmlScripts)
        { 
            ActionName = actionName;
            HtmlScripts = htmlScripts;
        }
        public override void Perform()
        {
            try
            {
                foreach(var script in HtmlScripts)
                {
                    //TODO：执行Script
                }
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
