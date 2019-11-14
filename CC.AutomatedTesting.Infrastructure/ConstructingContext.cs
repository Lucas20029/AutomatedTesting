using CC.AutomatedTesting.Infrastructure.FunctionProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class ConstructingContext
    {
        public ConstructingContext()
        {
            Bindings = new Dictionary<string, string>();
            FunctionText = new List<string>();
        }
        public ConstructingContext(Dictionary<string, string> binding, Version defaultVersion, Dictionary<string, UserFunctionDefinition> userFunctions=null)
        {
            Bindings = binding;
            DefaultFrontendVersion = defaultVersion;
            UserFunctions = userFunctions;
        }

        public Dictionary<string,string> Bindings { get; set; }

        public Version DefaultFrontendVersion { get; set; }

        public Dictionary<string,UserFunctionDefinition> UserFunctions { get; set; }
        public List<string> FunctionText { get; set; }
    }


}
