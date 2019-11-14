using CC.AutomatedTesting.Infrastructure.Bizbases.CommonActions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Exceptions
{
    public class BizException:Exception
    {
        public BizException()
        {

        }
        public BizException(string message):base(message)
        {

        }
        public virtual string ToUserInfo()
        {
            return "出现业务异常:"+Message;
        }
    }

    public class ConstructingException: BizException
    {
    }

    public class RuntimeException: BizException
    {
        public RuntimeException():base()
        {

        }
        public RuntimeException(string actionName):base()
        {
            ActionName = actionName;
        }
        public RuntimeException(string actionName, string message) : base(message)
        {
            ActionName = actionName;
        }
        public string ActionName { get; set; }
    }
    public class InvalidCommandFormatException : ConstructingException
    {
        public string VariableName { get; private set; }
        public InvalidCommandFormatException(string variableName)
        { 
            VariableName = variableName;
        }
        public override string ToUserInfo()
        {
            return "命令格式错误：" + VariableName;
        }
    }
    public class RedunantVariableDefinedException : ConstructingException
    {
        public string VariableName { get; private set; }
        public RedunantVariableDefinedException(string variableName)
        {
            VariableName = variableName;
        }
        public override string ToUserInfo()
        {
            return "重复定义的变量："+VariableName;
        }
    }
    public class UnSpecifiedVersionException : ConstructingException
    {
        public UnSpecifiedVersionException()
        {
        }
        public override string ToUserInfo()
        {
            return "该测试用例未指定版本号。需指定全局版本号或用例版本号";
        }
    }

    public class InvalidVersionException : ConstructingException
    {
        public string VersionStr { get; private set; }
        public InvalidVersionException(string versionStr)
        {
            VersionStr = versionStr;
        }
        public override string ToUserInfo()
        {
            return "不合法的版本号：" + VersionStr;
        }
    }

    public class CommandHandlerNotFound : ConstructingException
    {
        public CommandHandlerNotFound(string command)
        {
            CommandTitle = command;
        }
        public string CommandTitle { get; private set; }
        public override string ToUserInfo()
        {
            return $"未知命令：{CommandTitle} ";
        }
    }

    public class JavascriptRuntimeException:RuntimeException
    {
        public JavascriptRuntimeException(string actionName, string message):base(actionName)
        {
        }
        public override string ToUserInfo()
        {
            return $"JavaScript运行时异常："+ Message;
        }
    }
    public class TimeOutRuntimeException : RuntimeException
    {
        public TimeOutRuntimeException(string actionName, string message) : base(actionName)
        { 
        }

        public override string ToUserInfo()
        {
            return $"运行时等待超时异常：" + Message;
        }
    }

    public class WebElementNotFoundException : RuntimeException
    {
        public WebElementNotFoundException(string actionName, string elementDescription) : base(actionName)
        {
            ElementDescription = elementDescription;
        }

        public string ElementDescription { get; set; }
        public override string ToUserInfo()
        {
            return $"未找到指定元素：" + ElementDescription;
        }
    }

    public class WaitingElementTimeOutException : RuntimeException
    {
        public WaitingElementTimeOutException(By by) : base()
        {
            ByType = by;
        }
        public By ByType { get; set; }
        
        public override string ToUserInfo()
        {
            return $"等待查找元素超时："+ ByType.ToString();
        }
    }

    public class CouldNotFindCheckboxColumnInTableList : RuntimeException
    {
        public CouldNotFindCheckboxColumnInTableList() : base()
        {
        }
        public override string ToUserInfo()
        {
            return $"未能找到Checkbox列";
    }
    }
    public class CouldNotFindCellValueInTableList : RuntimeException
    {
        public string ColumnTitle { get; set; }
        public string ColumnValue { get; set; }
        public CouldNotFindCellValueInTableList(string columnTitle, string columnValue) : base()
        {
            ColumnTitle = columnTitle;
            ColumnValue = columnValue;
        }
        public override string ToUserInfo()
        {
            return $"未能找到 {ColumnTitle} 列内的 {ColumnValue} ";
        }
    }

    public class ElementNotFoundException : RuntimeException
    {
        public ElementNotFoundException(string actionName, string elementTitle) : base(actionName)
        {
            ElementTitle = elementTitle;
        }
        public string ElementTitle { get; private set; }
        public override string ToUserInfo()
        {
            return $"未能找到{ElementTitle}";
        }
    }
    public class IFrameNotFoundException : RuntimeException
    {
        public IFrameNotFoundException(string actionName ,string iframeId):base(actionName)
        {
            IframeId = iframeId;
        }
        public string IframeId { get; private set; }
        public override string ToUserInfo()
        {
            return $"未能找到id与{IframeId}匹配的iframe元素";
        }
    }

    public class FunctionStructureException : RuntimeException
    {
        public FunctionStructureException(string message)
        {
        }

        public override string ToUserInfo()
        {
            return $"解析用户函数运行时异常：" + Message;
        }
    }
    public class FunctionParameterException : RuntimeException
    {
        public FunctionParameterException(string message)
        {
        }

        public override string ToUserInfo()
        {
            return $"用户函数参数异常：" + Message;
        }
    }

    public class FunctionNotFoundException:RuntimeException
    {
        public FunctionNotFoundException(string message)
        {
        }

        public override string ToUserInfo()
        {
            return $"未找到函数：" + Message;
        }
    }

    public class FunctionCallException : RuntimeException
    {
        public FunctionCallException(string message)
        {
        }

        public override string ToUserInfo()
        {
            return $"函数调用异常：" + Message;
        }
    }
}
