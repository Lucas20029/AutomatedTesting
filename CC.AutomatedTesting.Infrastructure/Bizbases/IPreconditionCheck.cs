using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure
{
    public interface IPreConditionCheck
    {
        void PreConditionCheck();//如果检查失败，则抛出异常
    }
    public interface IPostConditionCheck
    {
        void PostConditionCheck();//如果检查失败，则抛出异常
    }
}
