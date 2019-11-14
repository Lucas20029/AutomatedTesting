using CC.AutomatedTesting.Infrastructure.ActionFactory;
using CC.AutomatedTesting.Infrastructure.Exceptions;
using CC.AutomatedTesting.Infrastructure.FunctionProcessor;
using CC.UPaaSCore.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    #region 根据用户输入的Command，来创建TestCase
    /// <summary>
    /// 创建TestCase的工厂：根据用户命令，调用ActionFactory，来实现创建Action
    /// </summary>
    public class TestCaseFactory
    {
        #region Signleton
        private static readonly TestCaseFactory instance = new TestCaseFactory();
        private TestCaseFactory()
        {
        }
        public static TestCaseFactory Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        public TestCase CreateTestCase(string[] userCommands, string caseName, ConstructingContext context)
        {
            var testCase = new TestCase();
            testCase.Name = caseName;
            Version version = context.DefaultFrontendVersion;
            testCase.Actions = ActionsFactory.Instance.CreateActions(userCommands, context);
            return testCase;
        }


    }
    #endregion

    public class FunctionFactory
    {
        #region Signleton
        private static readonly FunctionFactory instance = new FunctionFactory();
        private FunctionFactory()
        {
        }
        public static FunctionFactory Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public FunctionAction CreateFunction(string command, ConstructingContext context)
        {
            //解析Command，获得函数名、实参列表
            var functionInfo = GetFunctionActualInfo(command);
            //如果之前解析的上下文中，没有该函数定义，则异常
            if (!context.UserFunctions.ContainsKey(functionInfo.Item1))
                throw new FunctionNotFoundException(functionInfo.Item1);
            //找到函数定义，并根据实参列表，进行形参->实参转换 
            var functionActual = context.UserFunctions[functionInfo.Item1].ToRuntime(functionInfo.Item2); //获得实参下的函数

            FunctionAction funcAction = new FunctionAction() { FunctionActual = functionActual, CommandText = command };
            funcAction.BodyActions = ActionsFactory.Instance.CreateActions(functionActual.ActualBody.ToArray(), context);
            return funcAction;
        }
        private Tuple<string, List<string>> GetFunctionActualInfo(string userCommand)
        {
            var commandSlices = userCommand.Trim().Split('：');
            if (commandSlices.Length < 3)
                throw new FunctionCallException(userCommand);
            var functionName = commandSlices[1].Trim();
            var actualParameters = commandSlices[2].Trim().Split('，').Select(p => p.Trim()).ToList();
            return new Tuple<string, List<string>>(functionName, actualParameters);
        }
    }

    public class ActionsFactory
    {
        #region Signleton
        private static readonly ActionsFactory instance = new ActionsFactory();
        private ActionsFactory()
        {
        }
        public static ActionsFactory Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        public ObservableCollection<Action> CreateActions(string[] commands, ConstructingContext context)
        {
            ObservableCollection<Action> actions = new ObservableCollection<Action>();
            Version version = context.DefaultFrontendVersion;
            foreach (var command in commands)
            {
                if (IsFrontEndSwitch(command))
                {
                    var versionCodeStr = command.Trim().Replace(Constants.Configs.FrontEndSwitchPrefix, "").Replace("：", "").Trim();
                    version = Version.FromString(versionCodeStr);
                    continue;
                }
                else if (IsFunctionCall(command))
                {
                    //FunctionFactory和ActionFactory存在循环依赖的可能。这取决于用户定义函数的时候，不能递归调用自身
                    actions.Add(FunctionFactory.Instance.CreateFunction(command, context));
                }
                else if (IsAssert(command))
                {
                    actions.Add(AssertActionFactory.Instance.CreateAction(command.Trim(), version, context));
                    //testCase.AssertActions.Add(AssertActionFactory.Instance.CreateAction(command.Trim(), version,context));
                }
                else if (IsUserFunc(command))
                {
                    actions.Add(FuncActionFactory.Instance.CreateAction(command.Trim(), version, context));
                }
                else
                {
                    actions.Add(PerformActionFactory.Instance.CreateAction(command.Trim(), version, context));
                    //testCase.OrdinaryActions.Add(PerformActionFactory.Instance.CreateAction(command.Trim(),version,context));
                }
            }
            return actions;
        }

        private bool IsAssert(string userCommand)
        {
            return userCommand.StartsWith(Constants.Configs.AssertPrefix);
        }

        private bool IsFrontEndSwitch(string userCommand)
        {
            return userCommand.StartsWith(Constants.Configs.FrontEndSwitchPrefix);
        }

        private bool IsFunctionCall(string userCommand)
        {
            return userCommand.StartsWith(Constants.Configs.FunctionCallPrefix);
        }

        private bool IsUserFunc(string userCommand)
        {
            return userCommand.StartsWith(Constants.Configs.FuncPrefix);
        }
    }
}
