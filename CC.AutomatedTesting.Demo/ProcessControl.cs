using CC.AutomatedTesting.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Demo
{
    class ProcessControl
    {
        const string WorkSpace = "../../脚本/";
        //控制执行流程
        //1. 读取指定目录下的所有子目录
        //2. 屏幕打印所有子目录
        //3. 等待输入子目录编号
        //4. 执行子目录下的所有文件
        private static void ExecuteFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath, Encoding.Default);
            var suite = TestSuiteFactory.Instance.CreateTestSuiteFrom(lines, filePath, null);
            TestSuiteRunner runner = new TestSuiteRunner();
            runner.Run(suite);
        }
        public static void ExecuteAllFilesInDir(string dirPath)
        {
            var files = Directory.GetFiles(dirPath).OrderBy(p=>p);
            var dirName = Path.GetFileName(dirPath);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                Console.WriteLine("");
                Console.WriteLine("文件：" + Path.GetFileName(file));
                ExecuteFile(file);
                Console.WriteLine("结束");
                Console.WriteLine("");
            }
            Console.WriteLine("");
            Console.WriteLine("--Finished--");
        }
        public static string GetDirToExecute()
        {
            var subDirs = Directory.GetDirectories(WorkSpace);
            int i = 1;
            var dict = subDirs.ToDictionary(kv => (i++).ToString(), kv => kv);
            
            var fileNameList = dict.Select(p =>  p.Key + "\t" + Path.GetFileName(p.Value)).ToList();
            fileNameList.ForEach(p => { Console.WriteLine(p); });
            
            while (true)
            {
                Console.Write("请输入要执行的目录序号：");
                var num = Console.ReadLine();
                if (dict.ContainsKey(num))
                {
                    return dict[num];
                }
            }
        }

    }
}
