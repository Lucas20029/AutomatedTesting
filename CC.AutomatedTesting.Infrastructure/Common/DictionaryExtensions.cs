using System.Collections.Generic;

namespace CC.UPaaSCore.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 不存在则新增，存在则更新
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            lock (dic)
            {
                if (dic.ContainsKey(key))
                    dic[key] = value;
                else
                    dic.Add(key, value);
                return dic;
            }
        }

        /// <summary>
        /// 存在，则不写入。不存在才写入
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, value);
            return dic;
        }

        /// <summary>
        /// 尝试获取，如果不存在，则返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue TryGetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value = default(TValue))
        {
            if (!dic.ContainsKey(key))
                return value;
            return dic[key];
        }

    }
}