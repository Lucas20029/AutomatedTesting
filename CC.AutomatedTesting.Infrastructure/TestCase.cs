using CC.AutomatedTesting.Infrastructure.Bizbases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class TestCase
    {
        public TestCase()
        {
            OrdinaryActions = new List<PerformAction>();
            AssertActions = new List<AssertAction>();
            Actions = new ObservableCollection<Action>();
        }
        public string TestSetID { get; set; }//一组测试用例的ID标识。用于输出时，把这些用例归到一起生成report
        public string Name { get; set; }
        public List<PerformAction> OrdinaryActions { get; set; }
        public List<AssertAction> AssertActions { get; set; }
        public ObservableCollection<Action> Actions { get; set; }
    }
}
