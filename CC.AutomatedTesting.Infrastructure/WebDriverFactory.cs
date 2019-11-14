using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class WebDriverFactory
    {
        private static readonly string remoteHubUrl = "http://10.189.1.206:4444/wd/hub";
        public static IWebDriver GetInstance(WebDriverType type)
        {
            switch (type)
            {
                case WebDriverType.RemoteChrome:
                    return new RemoteWebDriver(new Uri(remoteHubUrl), new ChromeOptions());
                case WebDriverType.LocalChrome:
                    ChromeOptions options = new ChromeOptions();
                    ////忽略Https证书不存在的错误，有助于加快Https页面加载速度
                    options.AddArgument("--ignore-certificate-errors");
                    options.AddArgument("--start-maximized");
                    var webDriver = new ChromeDriver(options);
                    //webDriver.Manage().Window.Maximize();
                    return webDriver;
                default:
                    return null;
            }
        }
    }
    public enum WebDriverType
    {
        LocalChrome=1,
        RemoteChrome=2,
        LocalFirefox=3,
        RemoteFirefox=4,
        LocalIE=5,
        RemoteIE=6
    }
}
