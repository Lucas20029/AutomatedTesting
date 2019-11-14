using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Extends.CCCloud
{
    public class RandomResult
    {
        public static int Next(int resultCount)
        {
            Random ran = new Random();
            return ran.Next() % resultCount;
        }
    }
}
