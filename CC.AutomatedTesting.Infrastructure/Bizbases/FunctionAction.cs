using CC.AutomatedTesting.Infrastructure.FunctionProcessor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public class FunctionAction :PerformAction
    {
        public ObservableCollection<Action> BodyActions { get; set; }
        public UserFunctionActual FunctionActual { get; set; } 

        public override void PostConditionCheck()
        {
            
        }
        public override void PreConditionCheck()
        {
            
        }
    }
}
