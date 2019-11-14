using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.AutomatedTesting.Infrastructure.TextAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CC.AutomatedTesting.Infrastructure.TextAnalyzer.Tests
{
    [TestClass()]
    public class AnalyzerTests
    {
        
        [TestMethod()]
        public void Test_RegexVariableName()
        {
            string regex = @":\s*[^=\s]+\s*=";
            string source = "定义 :变量4=1000=[@时间戳";
            var result = Regex.Match(source, regex).Value;
            Assert.IsTrue(result== ":变量4=");
        }

        [TestMethod()]
        public void Test_RegexVariableDefines()
        {
            string regex = @"^\s*定义\s*:\s*[^=\s]+\s*=\s*\S+\s*$";
            string source = " 定义 :变量2 = 1000时间戳 ";
            var result = Regex.IsMatch(source,regex);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Test_EatVariableDefines()
        {
            List<string> text = new List<string>()
            {
                "//123123",
                "",
                "定义 : 变量1 = 变量2  ",
                " 定义:变量2=1000@时间戳",
                "//",
                "   ",
                "测试123123",
                "123123"
            };
            var expected = new List<string>()
            {
                "//123123",
                "",
                "//",
                "   ",
                "测试123123",
                "123123"
            };
            var expectedVariable = new List<string>()
            {
                "定义 : 变量1 = 变量2  ",
                " 定义:变量2=1000@时间戳",
            };
            List<string> definedLines = new List<string>();
            var result = TextPreProcesser.EatVariableDefines(text, out definedLines);
            Assert.IsTrue(IsListEquals(result, expected));
            Assert.IsTrue(IsListEquals(definedLines, expectedVariable));
        }
        [TestMethod()]
        public void Test_Explain()
        {
            var defineVariable = new List<string>()
            {
                "定义 : 变量1 = 变量2  ",
                " 定义:变量2=1000@时间戳",
                " 定义:变量3=随机数@随机数109",
                " 定义 :变量4=1000=[@时间戳",
            };
            var resultDict = TextPreProcesser.Explain(defineVariable);
            Assert.IsTrue(resultDict.Count == 4);
            Assert.IsTrue(resultDict["变量1"]=="变量2");
            Assert.IsTrue(resultDict["变量2"].StartsWith("1000"));
            Assert.IsTrue(resultDict["变量3"].StartsWith("随机数"));
            Assert.IsTrue(resultDict["变量3"].EndsWith("109"));

        }


        private bool IsListEquals(List<string> source, List<string> target)
        {
            if (source.Count != target.Count)
                return false;
            for(int i = 0; i < source.Count; i++)
            {
                if (source[i] != target[i])
                    return false;
            }
            return true;
        }
    }
}