using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Meeting.DataConvert.CommonConvert
{
    public static class ValueConverter
    {
        //todo Chenchen:加上null的处理；对于int、double、datetime等类型的default改为异常值，如int.min
        //todo Chenchen:加上object的扩展方法，使调用时用to<int>即可
        public static T SafeConvert<T>(object sourceText)
        {
            return (T) TypeDescriptor.GetConverter(typeof (T)).ConvertFromString(sourceText.ToString());
        }

        #region
        //public static T ConvertTo<T>(object data)
        //{
        //    var dataStr = Convert.ToString(data);
        //    if (typeof(T) == typeof(int))
        //    {
        //        int temp;
        //        return int.TryParse(dataStr, out temp) ? (T)(object)temp : Default<T>();
        //    }
        //    else if (typeof(T) == typeof(double))
        //    {
        //        double temp;
        //        return double.TryParse(dataStr, out temp) ? (T)(object)temp : Default<T>();
        //    }
        //    else if (typeof(T) == typeof(DateTime))
        //    {
        //        DateTime temp = DateTime.MinValue;
        //        return DateTime.TryParse(dataStr, out temp) ? (T)(object)temp : Default<T>();
        //    }
        //    else if (typeof(T) == typeof(long))
        //    {
        //        long temp;
        //        return long.TryParse(dataStr, out temp) ? (T)(object)temp : Default<T>();
        //    }
        //    else if (typeof(T) == typeof(bool))
        //    {
        //        bool temp;
        //        return bool.TryParse(dataStr, out temp) ? (T)(object)temp : Default<T>();
        //    }
        //    else if (typeof (T) == typeof (string))
        //    {
        //        return (T)(object)dataStr;
        //    }
        //    return Default<T>();
        //}
        #endregion
        public static T To<T>(this object source)
        {
            try
            {
                return SafeConvert<T>(source);
            }
            catch
            {
                return Default<T>();
            }
        }
        public static T UnsafeTo<T>(this object source)
        {
            try
            {
                return SafeConvert<T>(source);
            }
            catch
            {
                throw new DataConvertException(typeof(T).FullName, source);
            }
            
        }


        private static Dictionary<Type, object> DefaultValueMapping = new Dictionary<Type, object>()
        {
            { typeof(int),default(int) },
            { typeof(double),default(double) },
            { typeof(long),  default(long) },
            { typeof(float),default(float)  },
            { typeof(DateTime),default(DateTime)  },
            { typeof(short),default(short)  },
            { typeof(ushort),default(ushort)  },
            { typeof(ulong),default(ulong)  },
            { typeof(uint),default(uint)  },
            { typeof(decimal),default(decimal) },
            { typeof(bool),default(bool) }
        };

        public static T Default<T>()
        {
            if (DefaultValueMapping.ContainsKey(typeof(T)))
                return (T)DefaultValueMapping[typeof (T)];
            return default(T);
        }
        

        public static bool Is<T>(this string source) where T : struct
        {
            try
            {
                SafeConvert<T>(source);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }

    public class DataConvertException : Exception
    {
        public DataConvertException(string expectedType, object sourceValue)
        {
            ExpectedType = expectedType;
            SourceValue = Convert.ToString(sourceValue);
        }
        public string SourceValue { get; set; }
        public string ExpectedType { get; set; }
        public string ToUserInfo()
        {
            return "类型转换异常。期望类型：" + ExpectedType + "，原值：" + Convert.ToString(SourceValue);
        }
        public override string ToString()
        {
            return ToUserInfo() + "。 " + base.ToString();
        }
    }



}
