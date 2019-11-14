using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Extensions
{
    public static class WebElementEx
    {
        /// <summary>
        /// level=0:返回本身，level=1：父元素（向上1个层级），level=2：父元素的父元素（向上2个层级）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static IWebElement Parent(this IWebElement element, int level=1)
        {
            if (level <= 0)
                return element;
            var xpath = string.Empty;
            for(int i=0;i<level;i++)
            {
                xpath += "../";
            }
            xpath = xpath.Substring(0, xpath.Length - 1);
            return element.FindElement(By.XPath(xpath));
        }

        public static IWebElement FirstChild(this IWebElement element)
        {
            return element.FindElement(By.CssSelector("*:first-child"));
        }

        public static IEnumerable<IWebElement> DirectChild(this IWebElement element, string tagName)
        {
            return element.FindElements(By.XPath(tagName));
        }

        public static string GetInnerText(this IWebElement element, IWebDriver webDriver)
        {
            var s= ((IJavaScriptExecutor)webDriver).ExecuteScript("return arguments[0].innerText", element);
            return s.ToString().Replace("\r\n","");
        }
        public static void ClickMayBeHide(this IWebElement element, IWebDriver webDriver)
        {
            ((IJavaScriptExecutor)webDriver).ExecuteScript("arguments[0].click()", element);
        }
        public static void SetHiddenElementVisible(this IWebElement element, IWebDriver webDriver) 
        {
            ((IJavaScriptExecutor)webDriver).ExecuteScript("arguments[0].style.display='block'", element);
        }

        public static IWebElement FindElementOrNull(this IWebElement element,By by)
        {
            try
            {
                return element.FindElement(by);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static IWebElement FindElementByMultipleBys(this IWebElement element, List<By> bys)
        {
            foreach(var by in bys)
            {
                try
                {
                    return element.FindElement(by);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return null;
        }
        
    }
}
