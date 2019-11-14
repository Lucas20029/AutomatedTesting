using CC.AutomatedTesting.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.TextAnalyzer
{
    public class Escaper 
    {
        public static Dictionary<string, string> EscapeTable = new Dictionary<string, string>()
        {
            { "&cma","," }
        };
        public static string Process(string source)
        {
            var result = source.Clone().ToString();
            foreach(var kv in EscapeTable)
            {
                result = result.Replace(kv.Key, kv.Value);
            }
            return result;
        }
        
    }
}
