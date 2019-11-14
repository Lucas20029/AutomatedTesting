using CC.AutomatedTesting.Infrastructure.Bizbases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.ActionFactory
{
    /*
    测试命令的来源可以分为多种渠道：
    1. .NET程序集
    2. JS脚本执行器
    3. HTML元素编排器
    这三种渠道均可以输出测试命令

    测试命令的分类：
    1. 平台测试命令：以“命令名”的格式
    2. 产品专属测试命令：以“产品名_命令名”格式

    提供一个类，输入命令的名字、当前平台前端版本号，这个类可以输出一个固定的Object
    示例：
    输入：登录系统    输出： CC.Testing.Cloud.LoginAction的实例
    输入：薪酬-修改表格   输出： CC.Testing.Compensation.ModifyTableListCellAction的实例
    输入：填充文本框（如果是JS脚本命令）      输出： CC.Testing.Common.JSExecutorAction 的实例，把JS脚本属性给赋上值
    输入：选择下拉菜单（如果是HTML编排）      输出： CC.Testing.Common.HtmlMakeUp的实例，把HtmlScript属性给赋上值
    输出逻辑为：
    查找顺序为: CSharpDll  -->  HTML编排器  -->  JS脚本执行器
    如果有符合条件的，就地结束，输出。如果没有符合条件的，继续向下查找
    */
    /*
    在编写测试后台时，在创建产品线测试命令的时候，需要在保存时，自动给加上产品线的名称，以后不管是呈现、使用，都以产品线_命令名为标准名称
    */
    public class ActionResolver
    {
        public static PerformAction ResolvePerform(string actionName, Version requiredVersion)
        {
            return TypeContainer.Resolve<PerformAction>(actionName, requiredVersion);
            //1. 先调用DllTypeContainer，看看是否能实例化对应的Action
            //2. 如果为空，则调用JSOrHTMLActionContainer,看看是否能实例化对应的Action
            //3.如果都不能，则抛出异常
        }
        public static AssertAction ResolveAssert(string actionName, Version requiredVersion)
        {
            return TypeContainer.Resolve<AssertAction>(actionName, requiredVersion);
        }
        public static Action ResolveFunc(string actionName, Version requiredVersion)
        {
            return TypeContainer.Resolve<Action>(actionName, requiredVersion);
        }
        public static List<string> GetAllActions()
        {
            var dllActions = TypeContainer.Lookups;
            return dllActions.Select(p => p.Key).ToList();
        }
    }

    public class JsOrHtmlActionContainer
    {
        public PerformAction ResolvePerform(string actionName, Version requiredVersion)
        {
            //1. 查询多租赁指定的actionName，是否有名字匹配的结果
            //2. 如果有，则查看版本是否匹配
            //3. 如果存在版本匹配的，就看是JS类型的，就输出JSPerformAction
            //                           是HTML类型的，就输出HTMLPerformAction
            //没有匹配的，就输出Null
            return null;
        } 
        public PerformAction ResolveAssert(string actionName, Version requiredVersion)
        {
            //1. 查询多租赁指定的actionName，是否有名字匹配的结果
            //2. 如果有，则查看版本是否匹配
            //3. 如果存在版本匹配的，就看是JS类型的，就输出JSPerformAction
            //                           是HTML类型的，就输出HTMLPerformAction
            //没有匹配的，就输出Null
            return null;
        }
    }
}
