using CC.AutomatedTesting.Infrastructure;
using CC.AutomatedTesting.Infrastructure.Exceptions;
using CC.AutomatedTesting.Infrastructure.FunctionProcessor;
using CC.AutomatedTesting.Infrastructure.ParallelRun;
using CC.AutomatedTesting.Infrastructure.TextAnalyzer;
using CC.AutomatedTesting.Reporting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace CC.AutomatedTesting.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var funcDefinitions = LoadFunctionsFromFiles();
            var dirPath = ProcessControl.GetDirToExecute();
            ProcessControl.ExecuteAllFilesInDir(dirPath);
        }
        //读函数
        private static Dictionary<string,UserFunctionDefinition> LoadFunctionsFromFiles()
        {
            var funFiles = GetFunctionDefineFileNames();
            var funScripts = funFiles.SelectMany(p => LoadFromFile(p)).ToList();
            var preprocessResult = TextPreProcesser.PreprocessV2(funScripts.ToList());
            var funs = preprocessResult.Item3;
            var funcDefinitions = FuncProcessor.Instance.GetFunctionDefines(funs);
            return funcDefinitions;
        }

        private static TestSuite LoadTestSuiteFromFile(Dictionary<string, UserFunctionDefinition> funcDefinitions)
        {
            while(true)
            {
                try
                {
                    var file = GetFileName();
                    var scriptContent = LoadFromFile(file);
                    var suite = TestSuiteFactory.Instance.CreateTestSuiteFrom(scriptContent, file, funcDefinitions);
                    return suite;
                }
                catch (BizException ex)
                {
                    Console.WriteLine("脚本错误："+ex.ToUserInfo());
                    Console.WriteLine("");
                }
                catch(Exception ex)
                {
                    Console.WriteLine("脚本异常："+ex.Message+"\r\n"+ex.StackTrace);
                    Console.WriteLine("");
                }
            }
        }

        const string WorkSpace = "../../脚本/";
        const string FunctionDirectory = WorkSpace + "函数/";
        static string[] GetFunctionDefineFileNames()
        {
            return Directory.GetFiles(FunctionDirectory);
        }

        static string GetFileName()
        {
            Console.WriteLine("请输入要执行脚本的序号，并按Enter：");
            var files = Directory.GetFiles(WorkSpace);
            Dictionary<string, string> fileDict = new Dictionary<string, string>();
            var index = 1;
            foreach (var file in files)
            {
                fileDict.Add(index.ToString(), file);
                Console.WriteLine($"{index}\t{file}");
                index++;
            }
            while(true)
            {
                var key = Console.ReadLine();
                if (fileDict.Keys.Contains(key))
                {
                    return fileDict[key];
                }
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
