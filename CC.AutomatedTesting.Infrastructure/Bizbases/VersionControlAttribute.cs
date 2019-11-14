using CC.Meeting.DataConvert.CommonConvert;
using CC.AutomatedTesting.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    /*
    版本比较规则：从左到右，逐位比较。例如： 2.8.6.1  >(晚于) 2.8.2.5
        2.5.1.. 表示2.5.1到以后任意版本
        0..2.5.1 表示0到2.5.1
        1.2.5-9..1.4.6-08 表示1.2.5-9版本到1.4.6-08版本
        1.2.5-01-alpha .. 2.3.9-5-beta 从 1.2.5-01-alpha 到 2.3.9-5-beta 
        1.2.5-01-alpha .. 2.3.9-5-beta ,1.16.11,2.1.20.. 表示1.2.5-01-alpha 到 2.3.9-5-beta，和1.16.11以及2.1.20以后的版本
    */
    public class VersionControlAttribute : Attribute
    {
        /// <summary>
        /// 从a到b，写a..b；a和b，写a,b；什么都不写或没有这个标签，表示支持所有版本
        /// </summary>
        public string Range{ get; set; }
    } 

    public class Version
    {
        public string OriginCode { get; set; }
        public List<int> Codes { get; set; }
        public static Version FromString(string source)
        {
            return AnalyzeVersionCode(source);
        }
        private static Version AnalyzeVersionCode(string source)
        {
            try
            {
                Version version = new Version() { OriginCode = source };
                Dictionary<string, int> alphabetas = new Dictionary<string, int>()
                {
                    { "alpha", 1 },
                    { "beta", 2 }
                };
                var codeStrs = source.Split(new[] { '.', '-' });
                List<int> codes = new List<int>();
                foreach (var code in codeStrs)
                {
                    int tempCode = 0;
                    if (int.TryParse(code.Trim(), out tempCode))
                    {
                        codes.Add(tempCode);
                    }
                    else
                    {
                        var character = code.Trim().ToLower();
                        if (alphabetas.ContainsKey(character))
                        {
                            codes.Add(alphabetas[character]);
                        }
                    }
                }
                version.Codes = codes;
                return version;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        /// <summary>
        /// 如果本身的版本号大，则1，如果小，则-1。相等0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Compare(object obj)
        {
            if (obj == null)
                return int.MinValue;
            if(obj is Version)
            {
                var version = obj as Version;
                int minV = Math.Min(version.Codes.Count, this.Codes.Count);
                for(int i=0;i<minV;i++)
                {
                    if (version.Codes[i] < Codes[i])
                        return 1;
                    else if (version.Codes[i] > Codes[i])
                        return -1;
                    else
                        continue;
                }
                //如果还相等，则长的是大的
                if (Codes.Count > version.Codes.Count)
                    return 1;
                else if (Codes.Count == version.Codes.Count)
                    return 0;
                else
                    return -1;
            }
            throw new InvalidVersionException("");
        }
        public static bool operator <=(Version v1, Version v2)
        {
            var res = v1.Compare(v2);
            if (res == -1 || res == 0)
                return true;
            return false;
        }
        public static bool operator >=(Version v1, Version v2)
        {
            var res = v1.Compare(v2);
            if (res == 1 || res == 0)
                return true;
            return false;
        }
        //public static bool operator ==(Version v1, Version v2)
        //{
        //    if (v1 == null && v2 == null)
        //        return true;
        //    if (v1 == null && v2 != null)
        //        return false;
        //    var res = v1.Compare(v2);
        //    if (res == 0)
        //        return true;
        //    return false;
        //}
        //public static bool operator !=(Version v1, Version v2)
        //{
        //    if (v1 == null && v2 == null)
        //        return false;
        //    if (v1 == null && v2 != null)
        //        return true;
        //    var res = v1.Compare(v2);
        //    if (res != 0)
        //        return true;
        //    return false;
        //}

        public static Version MaxValue 
        {
            get
            {
                return new Version() { Codes=new List<int>() { int.MaxValue } };
            }
        }
        public static Version MinValue
        {
            get
            {
                return new Version { Codes = new List<int>() { int.MinValue } };
            }
        }

    }

    public class VersionSeries
    {
        public VersionSeries()
        {
            Ranges = new List<Tuple<Version, Version>>();
            Singles = new List<Version>();
        }
        public static VersionSeries FromString(string source)
        {
            return AnalyzeVersion(source);
        }
        public bool IsBelongsToMe(Version version)
        {
            foreach(var range in Ranges)
            {
                if(range.Item1<=version && range.Item2>=version)
                {
                    return true;
                }
            }
            return Singles.Exists(p => p == version);
        }
        public List<Tuple<Version,Version>> Ranges { get; set; }
        public List<Version> Singles { get; set; }
        public static VersionSeries AnySeries
        {
            get
            {
                VersionSeries series = new VersionSeries();
                series.Ranges = new List<Tuple<Version, Version>>()
                {
                    new Tuple<Version, Version>(Version.MinValue,Version.MaxValue)
                };
                return series;
            }
        }
        public bool IsAny()
        {
            return Ranges.Count == 1 
                && Ranges[0].Item1.Compare(Version.MinValue) == 0 
                && Ranges[0].Item2.Compare(Version.MaxValue) == 0;
        }

        private static VersionSeries AnalyzeVersion(string source)
        {
            if (string.IsNullOrEmpty(source) || source.Trim().Equals("*"))
                return VersionSeries.AnySeries;
            VersionSeries result = new VersionSeries();
            var versionSegments = source.Split(',');
            foreach (var seg in versionSegments)
            {
                if (seg.Contains(".."))
                {
                    Version lower, upper;
                    var versions = seg.Split(new[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
                    if (versions.Count() >= 1)
                    {
                        lower = Version.FromString(versions[0]);
                        if (versions.Count() >= 2)
                            upper = Version.FromString(versions[1]);
                        else
                            upper = Version.MaxValue;
                        result.Ranges.Add(new Tuple<Version, Version>(lower, upper));
                    }
                }
                else
                {
                    var version = Version.FromString(seg);
                    if (version != null)
                        result.Singles.Add(version);
                }
            }
            return result;
        }

    }

}
