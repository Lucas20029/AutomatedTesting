using CC.AutomatedTesting.Infrastructure.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Extensions
{
    public static class WebDriverWaitEx
    {
        public static void UntilTrue(this WebDriverWait wait, Func<bool> checkFunction, int maxSeconds = 10)
        {
            DateTime startTime = DateTime.Now;
            while (true)
            {
                var funcResult = false;
                try
                {
                    funcResult = checkFunction();
                }
                catch
                {
                }
                if (funcResult)
                    break;
                if ((DateTime.Now - startTime).TotalSeconds > maxSeconds)
                    throw new TimeoutException("等待超时");
                Thread.Sleep(500);
            }
        }

        public static bool UntilDisappear(this WebDriverWait wait, By by, IWebDriver webDriver, IWebElement parentContainer = null, int maxSecond = 60)
        {
            DateTime startTime = DateTime.Now;
            if (maxSecond <= 0)
                maxSecond = 10;
            while (true)
            {
                var elements = webDriver.FindElements(by);
                //如果找不到该元素
                if (elements.Count == 0)
                    return true;
                //或者只有一个元素，且此元素的display属性还是none
                else if(elements.Count==1)
                {
                    try
                    {
                        var displayValue = elements.FirstOrDefault().GetCssValue("display");
                        if (displayValue != null && displayValue == "none")
                            return true;
                    }
                    catch (StaleElementReferenceException ex)
                    {//如果出现异常（很有可能StaleElementReferenceException），就跳过本次循环，再重新找一次即可。
                    }
                }

                if ((DateTime.Now - startTime).TotalSeconds > maxSecond)
                    throw new TimeoutException("等待超时");
                Thread.Sleep(500);
            }
        }

        public static IWebElement UntilFindElement(this WebDriverWait wait, By by, ISearchContext searchContext, int maxSecond = 60)
        {
            if (maxSecond <= 0)
                maxSecond = 10;
            DateTime startTime = DateTime.Now;
            while (true)
            {
                var elements = searchContext.FindElements(by);
                if (elements.Count > 0)
                    return elements.FirstOrDefault();
                if ((DateTime.Now - startTime).TotalSeconds > maxSecond)
                    throw new WaitingElementTimeOutException(by);
                Thread.Sleep(500);
            }
        }
    }
}
