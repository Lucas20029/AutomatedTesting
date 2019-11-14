using CC.AutomatedTesting.Infrastructure.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public abstract class Action : IPreConditionCheck,IPostConditionCheck, INotifyPropertyChanged
    {
        #region Properties
        public string CommandText { get;  set; }        

        public string AnalyziedCommandText
        {
            get
            {
                return $"{ActionName}：{string.Join("，",Parameters)}";
            }
        }

        //Action的中文名称
        public string ActionName { get;  set; }
        //用于保存用户参数
        public List<string> Parameters { get; internal set; }

        private ActionExecuteState _ExecuteState = ActionExecuteState.UnExecuted;
        public ActionExecuteState ExecuteState
        {
            get
            {
                return _ExecuteState;
            }
            set
            {
                _ExecuteState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExecuteState"));//对Name进行监听  
            }
        }

        public Exception InnerException { get; set; }
        public string Message {
            get
            {
                if (InnerException == null)
                    return string.Empty;
                var ex = (InnerException as BizException);
                if (ex == null)
                    return InnerException.Message;
                else
                    return ex.ToUserInfo();
            }
        }
        public bool IsFunction { get; set; } = false;
        #endregion

        /// <summary>
        /// Action运行前，要求的Iframe路径。支持正则表达式匹配IframeID路径
        /// </summary>
        protected List<string> ActionRequiredIframeIDPath = new List<string>() { }; 
        /// <summary>
        /// 保存当前IframeID路径
        /// </summary>
        internal List<string> CurrentIframeIDPath = new List<string>() { };

        private string CurrentIframeId { get; set; }

        const int WaitSeconds = 20;
        private IWebDriver webDriver;

        public event PropertyChangedEventHandler PropertyChanged;

        public IWebDriver WebDriver
        {
            get
            {
                return webDriver;
            }
            internal set
            {
                webDriver = value;
                Waiter = new WebDriverWait(webDriver, TimeSpan.FromSeconds(WaitSeconds));
            }
        } 
        public WebDriverWait Waiter { get; internal set; }

        /// <summary>
        /// 前置检查条件：本Action执行的前提是什么？。
        /// 注意：该方法在切换所需Iframe之后执行。
        /// </summary>
        public abstract void PreConditionCheck();
        /// <summary>
        /// 后置检查条件:本Action完成的标准是什么？
        /// </summary>
        public abstract void PostConditionCheck();
        public virtual void SwitchToActionRequiredIframe()
        {
            //如果所在iframe跟要求的不匹配，则从根向下逐级切换
            SwitchToIframe(ActionRequiredIframeIDPath);
        }

        #region Helper
        private bool IsInTheIframe(List<string> iframeIdPath)
        {
            if(iframeIdPath.Count()==CurrentIframeIDPath.Count())
            {
                for(int i=0;i< CurrentIframeIDPath.Count();i++)
                { //当前IframeID是否跟要求的IframeID匹配
                    if (!Regex.IsMatch(CurrentIframeIDPath[i], iframeIdPath[i]))
                        return false;
                }
                //全部匹配通过，则认为相等
                return true;
            }
            return false;
        }
        public virtual void SwitchToIframe(List<string> iframeIdPath)  
        {
            //如果所在iframe跟要求的不匹配，则从根向下逐级切换
            //if (!IsInTheIframe(iframeIdPath))
            //{
                CurrentIframeIDPath.Clear();
                webDriver.SwitchTo().DefaultContent();
                foreach (var frameId in iframeIdPath)
                {
                    var targetFrame = webDriver
                        .FindElements(By.TagName("iframe"))
                        .LastOrDefault(p => Regex.IsMatch(p.GetAttribute("id"), frameId));
                    if (targetFrame == null)
                        throw new IFrameNotFoundException(ActionName, frameId);
                    var iframeId = targetFrame.GetAttribute("id").Clone().ToString();
                    webDriver.SwitchTo().Frame(targetFrame);
                    CurrentIframeIDPath.Add(iframeId);
                }
            //}
        }

        public void Excuting()
        {
            ExecuteState = ActionExecuteState.Executing;
        }
        public void Success()
        {
            ExecuteState = ActionExecuteState.Succeed;
        }
        public void Fail(Exception ex=null)
        {
            ExecuteState = ActionExecuteState.Failed;
            InnerException = ex;
        }
        #endregion

        public enum ActionExecuteState
        {
            UnExecuted,
            Executing,
            Succeed,
            Failed,
            AssertionPass,
            AssertionFail
        }
    }
}

