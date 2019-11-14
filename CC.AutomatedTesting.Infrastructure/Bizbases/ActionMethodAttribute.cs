using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class ActionMethodAttribute : Attribute
    {
        //示例：
        //[ActionMethod("弹窗部门选择", "在部门选择框中的部门树上，按指定的层级的名称依次查找指定部门，并选择最终的部门","选择框控件标题。。部门路径，以-分隔")]
        public ActionMethodAttribute(string title,string description="", string paramDescStr="")
        {
            Title = title;
            Description = description;
            ParamDesc = new List<string>();
            if (!string.IsNullOrEmpty(paramDescStr.Trim()))
            {
                string[] paramArray = Regex.Split(paramDescStr, "。。", RegexOptions.IgnoreCase);
                if (paramArray != null)
                    ParamDesc= paramArray.ToList();
            }
        }
        public string Title { get; private set; }
        private string Description { get; set; }
        private List<string> ParamDesc { get; set; } 
    }
}
