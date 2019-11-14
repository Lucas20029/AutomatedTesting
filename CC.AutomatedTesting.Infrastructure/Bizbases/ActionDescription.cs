using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Bizbases
{
    public class ActionDescription
    {
        public string ActionName { get; set; }
        public string ActionNotation { get; set; }
        public List<string> Parameters { get; set; }
    }
}
