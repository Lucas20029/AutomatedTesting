using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.AutomatedTesting.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Tests
{
    [TestClass()]
    public class VersionHelperTests
    {
        [TestMethod()]
        public void Test_AnalyzeVersionCode()
        {
            var result = Version.FromString("1.16.9-01-alpha");
            Assert.IsTrue(result.Codes.Count() == 5);
            Assert.IsTrue(result.Codes[0] == 1);
            Assert.IsTrue(result.Codes[1] == 16);
            Assert.IsTrue(result.Codes[2] == 9);
            Assert.IsTrue(result.Codes[3] == 1);
            Assert.IsTrue(result.Codes[4] == 1);
        }

        [TestMethod()]
        public void Test_AnalyzeVersion()
        {
            var result = VersionSeries.FromString("1.16.9-01-alpha..1.16.9-20,1.16.11");
            Assert.IsTrue(result.Ranges.Count() == 1);
            Assert.IsTrue(result.Ranges[0].Item2.Codes[3] == 20);
            Assert.IsTrue(result.Singles[0].Codes[2] == 11);
        }
        [TestMethod()]
        public void Test_AnalyzeVersion_Empty()
        {
            var result = VersionSeries.FromString("*");
            Assert.IsTrue(result.Ranges[0].Item1.Codes[0] == int.MinValue);
            Assert.IsTrue(result.Ranges[0].Item2.Codes[0] == int.MaxValue);
        }

        [TestMethod()]
        public void Test_CompareTo_Similar()
        {
            Version v1 = Version.FromString("1.16.9-01");
            Version v2 = Version.FromString("1.16.9-05");
            Assert.IsTrue(v1 <= v2);
        }

        [TestMethod()]
        public void Test_CompareTo_ShortVersion()
        {
            Version v1 = Version.FromString("2");
            Version v2 = Version.FromString("1.16.9-05");
            Assert.IsTrue(v1 >= v2);
        }

        [TestMethod()]
        public void Test_CompareTo_WithBeta()
        {
            Version v1 = Version.FromString("1.16.9-01");
            Version v2 = Version.FromString("1.16.9-01-beta");
            Assert.IsTrue(v1 <= v2);
        }

        [TestMethod()]
        public void Test_IsBelongsToMe()
        {
            var versions = VersionSeries.FromString("0..1.16.9,1.19.1,2.8.5-alpha");
            var version = Version.FromString("1.12.0");
            var version1 = Version.FromString("2.8.5");
            Assert.IsTrue(versions.IsBelongsToMe(version));
            Assert.IsFalse(versions.IsBelongsToMe(version1));
        }

        [TestMethod()]
        public void Test_IsAny()
        {
            var result = VersionSeries.AnySeries.IsAny();
            Assert.IsTrue(result);
        }
    }
}