using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.ActionFactory
{
    public interface IActionFactory<TAction> where TAction :Action
    {
        TAction CreateAction(string userCommand, Version version, ConstructingContext context);
    }

    

}
