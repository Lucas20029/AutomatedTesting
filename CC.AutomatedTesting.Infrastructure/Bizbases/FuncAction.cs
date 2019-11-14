using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.Bizbases
{
    public class FuncAction : PerformAction
    {
        public ObservableCollection<Action> FucntionActions { get; set; } = new ObservableCollection<Action>();
        public string FuncName { get; set; }
        public override void PostConditionCheck()
        {
            throw new NotImplementedException();
        }

        public override void PreConditionCheck()
        {
            throw new NotImplementedException();
        }
        public FuncAction() => IsFunction = true;
    }
}
