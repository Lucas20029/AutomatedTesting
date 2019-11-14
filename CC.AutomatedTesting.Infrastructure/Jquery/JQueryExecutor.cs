using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Jquery
{
    public class JQueryExecutor
    {
        private IWebDriver WebDriver { get; set; }

        public JQueryExecutor(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }
        public object ExecuteJquery(string jqueryScript)
        {
            InjectJqueryIfNeeded();
            return ((IJavaScriptExecutor)WebDriver).ExecuteScript(jqueryScript);
        }


        //加载jquery
        private void InjectJqueryIfNeeded()
        {
            if (!JqueryIsLoaded())
            {
                injectJQuery();
            }
        }

        /// <summary>
        /// 判断是否加载jquery,返回true表示已加载jquery
        /// </summary>
        /// <returns></returns>
        private Boolean JqueryIsLoaded()
        {
            try
            {
                return (Boolean)((IJavaScriptExecutor)WebDriver).ExecuteScript("return jQuery()!=null");
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 注入jquery
        /// </summary>
        private void injectJQuery()
        {
            //在head中拼出加载jquery的html,固定写法
            String jquery = "var headID=document.getElementsByTagName(\"head\")[0];" +
                            "var newScript = document.createElement('script');" +
                            "newScript.type='text/javascript';" +
                            "newScript.src='https://cdn.rawgit.com/anshooarora/extentreports/jquery.js';" +
                            "headID.appendChild(newScript);";
            //执行js
            ((IJavaScriptExecutor)WebDriver).ExecuteScript(jquery);
        }

    }
}
