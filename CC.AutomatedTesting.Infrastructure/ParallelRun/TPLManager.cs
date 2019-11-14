using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CC.AutomatedTesting.Infrastructure.ParallelRun
{
    public class TPLManager
    {
        #region Signleton
        private static readonly TPLManager instance = new TPLManager();
        private TPLManager()
        {
        }
        public static TPLManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        
        public ActionBlock<TestSuite> abAsync = new ActionBlock<TestSuite>((suite) =>
        {
            Console.WriteLine(suite + " ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
            TestSuiteRunner runner = new TestSuiteRunner();//调用Runner，执行Suite
            runner.Run(suite);
        }
        , new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 5 });

        public void QueueTestSuite(TestSuite testSuite)
        {
            abAsync.Post(testSuite);
        }
    }
}
