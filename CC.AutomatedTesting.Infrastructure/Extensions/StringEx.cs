using CC.AutomatedTesting.Infrastructure.TextAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Extensions
{
    public static class StringEx
    {
        public static string[] SplitAndTrim(this string source, char splitter)
        {
            var result = source.Split(splitter);
            return result.Select(p => p.Trim()).ToArray();
        }
        public static List<string> Translate(this List<string> source, Dictionary<string,string> bindings)
        {
            List<string> result = new List<string>();
            for (int i=0;i<source.Count;i++)
            {
                var line = source[i];
                if (line.StartsWith("@"))
                {
                    var realVariable = line.Substring(1, line.Length - 1);
                    if (bindings.ContainsKey(realVariable))
                    {
                        result.Add(bindings[realVariable]);
                        continue;
                    }
                }
                result.Add(line);
            }
            return result;
        }

        public static List<string> Escape(this List<string> sources)
        {
            return sources.Select(p => Escaper.Process(p)).ToList();
        }
    }
}
