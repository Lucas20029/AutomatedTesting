using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public abstract class PerformAction : Action, IPerform
    {
        public virtual void Perform()
        {
        }
    }
}
