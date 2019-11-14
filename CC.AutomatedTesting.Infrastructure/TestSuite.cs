using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class TestSuite
    {
        public TestSuite()
        {
            TestCases = new ObservableCollection<TestCase>();
        }
        public TestSuite(string suitename)
        {
            SuiteName = suitename;
            TestCases = new ObservableCollection<TestCase>();
        }
        public ObservableCollection<TestCase> TestCases { get; set; }
        public string SuiteName { get; set; }
        public Version DefaultFrontendVersion { get; set; }
    }
}
