using CC.AutomatedTesting.Infrastructure.Exceptions;
using CC.AutomatedTesting.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.ActionFactory
{
    public class AssertActionFactory : IActionFactory<AssertAction>
    {
        #region Signleton
        private static readonly AssertActionFactory instance = new AssertActionFactory();
        private AssertActionFactory()
        {
        }
        public static AssertActionFactory Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        public AssertAction CreateAction(string userCommand, Version version, ConstructingContext context)
        {
            if (version == null)
                throw new UnSpecifiedVersionException();

            if (!userCommand.StartsWith(Constants.Configs.AssertPrefix))
            {
                throw new Exception("invalid user assert command. Should be started with " + Constants.Configs.AssertPrefix);
            }
            var firstSpliterIndex = userCommand.IndexOf(Constants.CommandParameterSpliter);
            if (firstSpliterIndex < 0)
                throw new InvalidCommandFormatException(userCommand);
            userCommand = userCommand.Remove(0, firstSpliterIndex+1).Trim();//删掉前缀（验证）和冒号

            var spliterIndex = userCommand.IndexOf(Constants.CommandParameterSpliter);
            if (spliterIndex < 0)
                throw new InvalidCommandFormatException(userCommand);
            var title = userCommand.Substring(0, spliterIndex).Trim(); // 获取命令前缀  “表格列存在”
            var parameterText = userCommand.Substring(spliterIndex+1, userCommand.Length - spliterIndex -1).Trim(); // 参数

            var action = ActionResolver.ResolveAssert(title, version);
            //var action = TypeContainer.Resolve<AssertAction>(title, version); //如果实例化有问题，内部会抛出异常。不会返回null
            action.Parameters = parameterText.SplitAndTrim(Constants.ParameterSpliter).ToList().Translate(context.Bindings).Escape();
            action.ActionName = title;
            action.CommandText = userCommand;
            return action;
        }        
    }

}
