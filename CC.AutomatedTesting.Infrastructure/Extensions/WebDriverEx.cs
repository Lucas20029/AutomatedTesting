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
    public static class WebDriverEx
    {
        public const string defaultPath = @"C:\CC.logfiles\自动化测试报表\截图\";
        public static string ScreenShot(this IWebDriver webDriver, string fullPath=null)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss")+".jpg";
            if(string.IsNullOrEmpty(fullPath))
            {
                fullPath = defaultPath;
            }
            if(!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            var fileFullName = defaultPath + fileName;
            Screenshot scrFile = ((ITakesScreenshot)webDriver).GetScreenshot();
            scrFile.SaveAsFile(fileFullName, ScreenshotImageFormat.Jpeg);
            return fileFullName;
        }
        
        public static IList<IWebElement> SafeFindElements(this ISearchContext element, By by, int maxSecondToWait = 10)
        {
            try
            {
                if (element == null || by == null)
                    return null;
                if (maxSecondToWait <= 0)
                    maxSecondToWait = 10;
                DateTime startTime = DateTime.Now;
                while (true)
                {
                    var results = element.FindElements(by);
                    if (results != null && results.Count > 0)
                        return results;

                    if ((DateTime.Now - startTime).TotalSeconds > maxSecondToWait)
                        return new List<IWebElement>();
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static IWebElement SafeFindElement(this ISearchContext element, By by, int maxSecondToWait = 10)
        {
            try
            {
                if (element == null || by == null)
                    return null;
                if (maxSecondToWait <= 0)
                    maxSecondToWait = 10;
                DateTime startTime = DateTime.Now;
                while (true)
                {
                    var results = element.FindElements(by);
                    if (results != null && results.Count > 0)
                        return results.FirstOrDefault();

                    if ((DateTime.Now - startTime).TotalSeconds > maxSecondToWait)
                        return null;
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
