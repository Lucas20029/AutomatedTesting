using CC.AutomatedTesting.Infrastructure.ActionFactory;
using CC.AutomatedTesting.Infrastructure.FunctionProcessor;
using CC.AutomatedTesting.Infrastructure.ParallelRun;
using CC.AutomatedTesting.Infrastructure.TextAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class TestSuiteFactory
    {
        #region Signleton
        private static readonly TestSuiteFactory instance = new TestSuiteFactory();
        private TestSuiteFactory()
        {
        }
        public static TestSuiteFactory Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public TestSuite CreateTestSuiteFrom(string[] userTestLines, string suiteName, Dictionary<string,UserFunctionDefinition> userFunctions)
        {
            TestSuite suite = new TestSuite(suiteName);
            var result = TextPreProcesser.Preprocess(userTestLines.ToList());
            var defaultVersion = ExtractFrontEndVersion(result.Item1);
            ConstructingContext context = new ConstructingContext(result.Item2, defaultVersion.Item2,userFunctions); //保存构建上下文
            var splitedCaseLines = SplitTestCases(result.Item1);
            foreach (var caseTerm in splitedCaseLines)
            {
                if (caseTerm.Count == 0)
                    continue;
                var title = GetCaseTitle(caseTerm[0]);
                caseTerm.RemoveAt(0);
                suite.TestCases.Add(TestCaseFactory.Instance.CreateTestCase(caseTerm.ToArray(), title, context));
            }
            return suite;
        }

        public TestSuite CreateTestSuiteFrom(string[] userTestLines, string suiteName)
        {
            TestSuite suite = new TestSuite(suiteName);
            var result = TextPreProcesser.Preprocess(userTestLines.ToList());
            var defaultVersion = ExtractFrontEndVersion(result.Item1);
            ConstructingContext context = new ConstructingContext(result.Item2, defaultVersion.Item2); //保存构建上下文
            var splitedCaseLines = SplitTestCases(defaultVersion.Item1);
            foreach (var caseTerm in splitedCaseLines)
            {
                if (caseTerm.Count == 0)
                    continue;
                var title = GetCaseTitle(caseTerm[0]);
                caseTerm.RemoveAt(0);
                suite.TestCases.Add(TestCaseFactory.Instance.CreateTestCase(caseTerm.ToArray(), title, context));
            }
            return suite;
        }
        private Tuple<List<string>, Version> ExtractFrontEndVersion(List<string> lines)
        {
            List<string> refinedLines = lines.Select(p => p).ToList();
            var pattern = @"^\s*前端版本\s*：\s*\S+";
            var line = refinedLines.FirstOrDefault(p => Regex.IsMatch(p, pattern));
            refinedLines.Remove(line);
            var versionCodeStr = line.Trim().Replace("前端版本", "").Replace("：", "").Trim();
            Version version = Version.FromString(versionCodeStr);
            return new Tuple<List<string>, Version>(refinedLines, version);
        }

        public List<List<string>> SplitTestCases(List<string> lines)
        {
            List<List<string>> caseLines = new List<List<string>>();
            List<string> tempCase = new List<string>();


            var refinedLines = lines.Where(p => !IsLineToSkip(p)) // Skip useless lines
                                    .Select(p => p.Trim())  //Trim All Lines
                                    .ToList();

            int index = 1;
            foreach (var line in refinedLines)
            {
                // Test case name line
                if (refinedLines.Count == index || line.StartsWith(Constants.Configs.TestCasePrefix))
                {
                    if (refinedLines.Count == index)
                    {
                        tempCase.Add(line);
                    }
                    //When calling ToList, system will deep copy the selected element to a new List
                    caseLines.Add(tempCase.ToList());
                    tempCase.Clear();
                }
                tempCase.Add(line);
                index++;
            }
            return caseLines;
        }

        public string GetCaseTitle(string caseTitle)
        {
            if (caseTitle.Contains(Constants.CommandParameterSpliter))
            {
                var spliterIndex = caseTitle.IndexOf(Constants.CommandParameterSpliter);
                return caseTitle.Substring(spliterIndex + 1, caseTitle.Length - spliterIndex - 1);
            }
            return caseTitle;
        }
        public bool IsLineToSkip(string line)
        {
            //Skip empty lines and notations
            return string.IsNullOrWhiteSpace(line) || line.StartsWith(Constants.Configs.NotaionPrefix);
        }
    }
}
