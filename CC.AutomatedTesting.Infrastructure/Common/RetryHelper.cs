using System;
using System.Collections.Generic;
using System.Threading;

namespace CC.UPaaSCore.Common.Extensions
{
    public static class RetryHelper
    {
        /// <summary>
        /// 必须保证里面的方法幂等性
        /// </summary>
        /// <param name="func"></param>
        /// <param name="retryTimes"></param>
        /// <param name="intervalSeconds"></param>
        /// <returns></returns>
        public static List<Exception> Execute(Action func, int retryTimes=1, int intervalSeconds=1)
        {
            if (retryTimes <= 0)
                retryTimes = 1;
            if (intervalSeconds <= 0 || intervalSeconds >= 1000)
                intervalSeconds = 1;
            List<Exception> exList = new List<Exception>();
            for(int i = 0; i < retryTimes; i++)
            {
                try
                {
                    func();
                    return null;
                }
                catch (Exception ex)
                {
                    exList.Add(ex);
                    Console.WriteLine($"重试{i}...");
                    Thread.Sleep(intervalSeconds);
                }
            }
            return exList;
        }
    }
}