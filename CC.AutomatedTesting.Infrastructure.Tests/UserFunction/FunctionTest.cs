using CC.AutomatedTesting.Infrastructure.FunctionProcessor;
using CC.AutomatedTesting.Infrastructure.TextAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Tests.UserFunction
{
    [TestClass()]
    public class FunctionTest
    {
        [TestMethod()]
        public void Test_ConvertFunc()
        {
            try
            {
                List<string> actualParam = new List<string>
                {
                    "测试发薪方案","薪酬1","月"
                };
                var scriptContent = LoadFromFile("../../脚本/函数1.txt");
                var tupleObj = TextPreProcesser.PreprocessV2(scriptContent.ToList());

                FuncProcessor.Instance.Process(actualParam, tupleObj.Item3);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public static string[] LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath + " Not Exist");
            return File.ReadAllLines(filePath, Encoding.Default);
        }
    }
}
