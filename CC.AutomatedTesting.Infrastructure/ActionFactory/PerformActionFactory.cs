using CC.AutomatedTesting.Infrastructure.Exceptions;
using CC.AutomatedTesting.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.ActionFactory
{
    public class PerformActionFactory : IActionFactory<PerformAction>
    {
        #region Signleton
        private static readonly PerformActionFactory instance = new PerformActionFactory();
        private PerformActionFactory()
        {
        }
        public static PerformActionFactory Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        public PerformAction CreateAction(string userCommand, Version version, ConstructingContext context)
        {
            if (version == null)
                throw new UnSpecifiedVersionException();
            //命令不带参数的情况
            if (!userCommand.Contains(Constants.CommandParameterSpliter))
            {
                var title = userCommand.Trim();
                var action = ActionResolver.ResolvePerform(title, version);
                //var action = TypeContainer.Resolve<PerformAction>(title,version);
                action.CommandText = userCommand;
                action.ActionName = title;
                action.Parameters = new List<string>();
                return action;
            }
            else
            {
                var spliterIndex = userCommand.IndexOf(Constants.CommandParameterSpliter);
                var title = userCommand.Substring(0, spliterIndex).Trim();
                var parameterText = userCommand.Substring(spliterIndex + 1, userCommand.Length - spliterIndex - 1).Trim();
                var action = ActionResolver.ResolvePerform(title, version);
                //var action = TypeContainer.Resolve<PerformAction>(title,version);
                action.CommandText = userCommand;
                action.ActionName = title;
                action.Parameters = parameterText.SplitAndTrim(Constants.ParameterSpliter).ToList().Translate(context.Bindings).Escape();
                return action;
            }
            

            //使用这种方法是不行的。
            //var actionFullName = TypeContainer.Mapping[title].FullName;
            //var action = Assembly.GetExecutingAssembly().CreateInstance(actionFullName) as PerformAction;
            
        }
    }

}
