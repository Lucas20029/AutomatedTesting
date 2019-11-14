using CC.AutomatedTesting.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.TextAnalyzer
{
    public class TextPreProcesser
    {
        /// <summary>
        /// 预处理：删掉空白、注释行。解析变量定义
        /// </summary>
        /// <param name="text">原脚本</param>
        /// <returns>Item1：预处理后剩下的脚本；Item2：上下文</returns>
        public static Tuple<List<string>,Dictionary<string,string>> Preprocess(List<string> text)
        {
            var refinedText = new List<string>();
            refinedText = EatComment(text);
            refinedText = EatEmptyLines(refinedText);
            return ExtractVariables(refinedText);

        }
        public static Tuple<List<string>, Dictionary<string, string>, List<string>> PreprocessV2(List<string> text)
        {
            var refinedText = new List<string>();
            refinedText = EatComment(text);
            refinedText = EatEmptyLines(refinedText);
            return ExtractVariablesV2(refinedText);

        }


        public static List<string> EatComment(List<string> text)
        {
            string commentPrefix = "\\\\";
            return text.Where(p => !p.StartsWith(commentPrefix)).ToList();
        }
        public static List<string> EatEmptyLines(List<string> text)
        {
            List<string> textList = new List<string>(); 
            foreach (var textItem in text)
            {
                if (!string.IsNullOrEmpty(textItem.Trim()))
                {
                    textList.Add(textItem.Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", ""));
                }  
            }
            return textList;
        }

        public static Tuple<List<string>, Dictionary<string, string>> ExtractVariables(List<string> text)
        {
            List<string> variableLines = new List<string>();
            var refinedText = EatVariableDefines(text, out variableLines);
            Dictionary<string, string> variableMapping = Explain(variableLines);
            return new Tuple<List<string>, Dictionary<string, string>>(refinedText,variableMapping);
        }
        public static Tuple<List<string>, Dictionary<string, string>, List<string>> ExtractVariablesV2(List<string> text)
        {
            List<string> variableLines = new List<string>();
            var refinedText = EatVariableDefines(text, out variableLines);
            Dictionary<string, string> variableMapping = Explain(variableLines);
            List<string> funcOrderSet = GetFuncTextSection(refinedText).ToList();
            var removeFuncText = refinedText.Except(funcOrderSet)?.ToList();
            return new Tuple<List<string>, Dictionary<string, string>, List<string>>(removeFuncText, variableMapping, funcOrderSet);
        }

        private static List<string> GetFuncTextSection(List<string> text)
        {
            List<string> funcTextList = new List<string>();
            List<int> startIndexList = new List<int>();
            List<int> endIndexList = new List<int>();
            for (int index = 0; index < text.Count; index++)
            {
                if (text[index].StartsWith("定义函数："))
                {
                    startIndexList.Add(index);
                }
                if (text[index].Equals("}"))
                {
                    endIndexList.Add(index);
                }
            }
            if(startIndexList.Count != endIndexList.Count)
            {
                throw new Exception("函数格式不正确，注意函数体的闭合！");
            }
            for (int index = 0; index < startIndexList.Count; index++)
            {
                funcTextList.AddRange(text.GetRange(startIndexList[index], endIndexList[index] - startIndexList[index] + 1));
            }
            return funcTextList;
        }

        public static List<string> EatVariableDefines(List<string> text, out List<string> defineLines)
        {
            string regex = @"^\s*定义\s*：\s*[^=\s]+\s*=\s*\S+\s*$";
            defineLines= text.Where(p => Regex.IsMatch(p, regex)).ToList();
            return text.Where(p => !Regex.IsMatch(p, regex)).ToList();
        }

        /// <summary>
        /// 解析参数定义。 输入： 员工姓名=墨竹@随机数； 输入：  Key:员工姓名，Value：墨竹09192
        /// </summary>
        /// <param name="defineLines"></param>
        /// <returns></returns>
        public static Dictionary<string,string> Explain(List<string> defineLines)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var randomSeed = DateTime.Now.Subtract(new DateTime(2010, 1, 1)).Seconds;
            Random ran = new Random(randomSeed);
            var randomNumber = ran.Next(99999).ToString();
            string regexVariableName = @"：\s*[^=\s]+\s*=";
            string regexValue = @"=\s*\S+\s*$";
            foreach(var line in defineLines)
            {
                var namePart = Regex.Match(line, regexVariableName).Value;
                namePart = namePart.Substring(1, namePart.Length - 2).Trim();
                var valuePart = Regex.Match(line, regexValue).Value;
                valuePart = valuePart.Substring(1, valuePart.Length - 1).Trim();
                valuePart = valuePart.Replace("@时间戳", timeStamp);
                valuePart = valuePart.Replace("@随机数", randomNumber);
                if (result.ContainsKey(namePart))
                {
                    throw new RedunantVariableDefinedException(namePart);
                }
                result.Add(namePart, valuePart);
            }
            return result;
        }
    }
}
