using CC.AutomatedTesting.Infrastructure.Bizbases;
using CC.AutomatedTesting.Infrastructure.Extensions;
using CC.AutomatedTesting.Infrastructure.FunctionProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.ActionFactory
{
    public class FuncActionFactory : IActionFactory<FunctionAction>
    {
        #region Signleton
        private static readonly FuncActionFactory instance = new FuncActionFactory();
        private FuncActionFactory()
        {
        }
        public static FuncActionFactory Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        public FunctionAction CreateAction(string userCommand, Version version, ConstructingContext context)
        {
            FunctionAction funcAction = new FunctionAction();
            var spliterIndex = userCommand.IndexOf(Constants.CommandParameterSpliter);
            var parameterText = userCommand.Substring(spliterIndex + 1, userCommand.Length - spliterIndex - 1).Trim();
            string funcName = parameterText.Substring(0, parameterText.IndexOf("（"));
            List<string> actualParamList = parameterText.Split(new[] { "，" }, StringSplitOptions.RemoveEmptyEntries)?.ToList();

            var orderSetDic = FuncProcessor.Instance.Process(actualParamList, context.FunctionText);
            List<string> paramList = orderSetDic.ContainsKey(funcName) ? orderSetDic[funcName] : new List<string>();
            var instance = PerformActionFactory.Instance;
            funcAction.ActionName = funcName;
            funcAction.ActionName = parameterText;
            funcAction.CommandText = parameterText;
            foreach (var param in paramList)
            {
                funcAction.BodyActions.Add(instance.CreateAction(param.Trim(), version, context));
            }

            return funcAction;

        }
    }
}
