using CC.AutomatedTesting.Infrastructure.Exceptions;
using CC.UPaaSCore.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.FunctionProcessor
{
    public class UserFunctionDefinition
    {
        public string Name { get; internal set; }
        public List<string> FormalParameters { get;internal set; }
        public List<string> FormalBody { get; internal set; }
        /// <summary>
        /// 把函数定义，根据实参，替换形参，转成运行时状态
        /// </summary>
        /// <param name="actualParameters"></param>
        /// <returns></returns>
        public UserFunctionActual ToRuntime(List<string> actualParameters)
        {
            if (actualParameters.Count != FormalParameters.Count)
                throw new FunctionParameterException("形参实参个数不一致");
            UserFunctionActual ufRuntime = new UserFunctionActual(Name,actualParameters);
            foreach(var bodyStr in FormalBody)
            {
                var actualStr = bodyStr.Clone() as string;
                for(int i=0;i<FormalParameters.Count;i++)
                {
                    actualStr = actualStr.Replace(FormalParameters[i], actualParameters[i]);
                }
                ufRuntime.ActualBody.Add(actualStr);
            }
            return ufRuntime;
        }
    }
    public class UserFunctionActual
    {
        public UserFunctionActual(string name, List<string> actualParameters)
        {
            Name = name;
            ActualParameters = actualParameters;
            ActualBody = new List<string>();
        }
        public string Name { get; }
        public List<string> ActualParameters { get; }
        public List<string> ActualBody { get; internal set; }
    }

    public class FuncProcessor
    {
        #region Signleton
        private static readonly FuncProcessor instance = new FuncProcessor();
        private FuncProcessor()
        {
        }
        public static FuncProcessor Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualParam">调用的实参</param>
        /// <param name="scriptStr">用户函数文本处理后的字符串</param>
        /// <returns></returns>
        public Dictionary<string, List<string>> Process( List<string> actualParam, List<string> scriptStr)
        {
            //将函数转换成指令集
            return ConvertFuncToOrderSet(actualParam, scriptStr);
        }
        //拆分函数定义
        public Dictionary<string,UserFunctionDefinition> GetFunctionDefines(List<string> scriptStr)
        {
            Dictionary<string, UserFunctionDefinition> result = new Dictionary<string, UserFunctionDefinition>();
            var processedStr = scriptStr.Select(s => s.Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim('{', '}').Replace(" ", ""));
            string joinStr = string.Join(" ", processedStr.ToArray());
            //先拆分函数
            var funcList = joinStr.Split(new[] { "定义函数：" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var func in funcList)
            {
                string[] funcArrary = func.Split(new[] { "(", ")" }, StringSplitOptions.None);
                if (funcArrary.Length >= 3)
                {
                    var head = funcArrary[0] + "(" + funcArrary[1] + ")";
                    string formBody = func.Replace(head, "");
                    formBody = formBody.Substring(2, formBody.Length - 2);
                    UserFunctionDefinition ufDefine = new UserFunctionDefinition()
                    {
                        Name = funcArrary[0].TrimEnd(':'),
                        FormalParameters = funcArrary[1].Split(',').ToList(),
                        FormalBody = formBody.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    };
                    result.AddOrUpdate(ufDefine.Name, ufDefine);
                }
                else
                {
                    throw new FunctionStructureException(string.Format("函数名称：{0}结构错误！请检查", funcArrary[0]));
                }
            }
            return result;
        }

        private Dictionary<string, List<string>> ConvertFuncToOrderSet(List<string> actualParam, List<string> scriptStr)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            if (!actualParam.Any()) throw new Exception("用户函数调用时，实参为空！");
            var processedStr = scriptStr.Select(s => s.Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim('{', '}').Replace(" ", ""));
            string joinStr = string.Join(" ", processedStr.ToArray());
            //先拆分函数
            var funcList = joinStr.Split(new[] { "定义函数：" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var func in funcList)
            {
                string[] funcArrary = func.Split(new[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                if (funcArrary.Length == 3)
                {
                    var funcBodyStr = funcArrary[2];
                    string funcName = funcArrary[0].TrimEnd(':');
                    if (string.IsNullOrEmpty(funcArrary[1]))
                    {
                        throw new FunctionParameterException(string.Format("函数名称：{0}参数为空！请检查", funcArrary[0]));
                    }
                    Dictionary<string, string> funcParam = new Dictionary<string, string>();
                    string[] parameterArray = funcArrary[1].Split(',');
                    if (actualParam.Count != parameterArray.Length) throw new Exception("用户函数调用时，实参与形参个数不一致！");
                    for (int index = 0; index < parameterArray.Length; index++)
                    {
                        funcParam.Add(parameterArray[index], actualParam[index]);
                        //替换str中的形参
                        funcBodyStr = funcBodyStr.Replace(parameterArray[index], actualParam[index]);
                    }
                    var commandList = funcBodyStr.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    result.Add(funcName, commandList);
                }
                else
                {
                    continue;
                    throw new FunctionStructureException(string.Format("函数名称：{0}结构错误！请检查", funcArrary[0]));
                }
            }
            return result;
        }
    }
}
