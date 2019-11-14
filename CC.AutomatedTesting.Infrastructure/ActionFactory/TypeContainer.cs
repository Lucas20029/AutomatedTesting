using CC.AutomatedTesting.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CC.AutomatedTesting.Infrastructure.ActionFactory
{
    /// <summary>
    /// 反射加载程序集、标签和类型
    /// </summary>
    public class TypeContainer
    {
        /// <summary>
        /// string:Action的命令，VersionSeries：支持的版本范围，Type：具体的类型
        /// </summary>
        private static Dictionary<string, List<Tuple<VersionSeries, Type>>> lookups;
        public static Dictionary<string, List<Tuple<VersionSeries, Type>>> Lookups
        {
            get
            {
                if (lookups == null || lookups.Count == 0)
                {
                    LoadBizTestingDlls();
                    LoadLookup();
                }
                return lookups;
            }
        }
        private static Dictionary<string, Type> mapping;
        public static Dictionary<string, Type> Mapping
        {
            get
            {
                if (mapping == null || mapping.Count == 0)
                {
                    LoadBizTestingDlls();
                    LoadMappings();
                }
                return mapping;
            }
        }
        public static T Resolve<T>(string command) where T : class
        {
            if (!Mapping.ContainsKey(command))
            {
                throw new CommandHandlerNotFound(command);
            }
            var result = Activator.CreateInstance(Mapping[command]) as T;
            if (result == null)
            {
                throw new CommandHandlerNotFound(command);
            }
            return result;
        }
        public static T Resolve<T>(string command, Version version) where T : class
        {
            var type = GetTypeByCommandVersion(command, version);
            if (type == null)
            {
                throw new CommandHandlerNotFound(command);
            }
            var result = Activator.CreateInstance(type) as T;
            if (result == null)
            {
                throw new CommandHandlerNotFound(command);
            }
            return result;
        }
        /// <summary>
        /// 从指定目录把程序集加载到内存
        /// </summary>
        public static void LoadBizTestingDlls()
        {
            var applicationDirectory = Environment.CurrentDirectory;
            //var dllDir = applicationDirectory + "\\bizdlls";
            //if (!Directory.Exists(dllDir))
            //{
            //    Directory.CreateDirectory(dllDir);
            //}   
            var dllNames = Directory.GetFiles(applicationDirectory).Where(p => p.EndsWith(".dll") && p.Contains("CC.AutomatedTesting"));
            foreach (var dllName in dllNames)
            {
                Assembly.LoadFrom(dllName);
            }
        }
        /// <summary>
        /// 从加载的程序集中，获取测试步骤类，放在mapping中
        /// </summary>
        private static void LoadMappings()
        {
            mapping = new Dictionary<string, Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => p.FullName.Contains("CC.AutomatedTesting"));
            var publicTypes = assemblies.SelectMany(p => p.ExportedTypes);
            foreach (var type in publicTypes)
            {
                var actionAttribute = type.GetCustomAttributes(typeof(ActionMethodAttribute), false);
                if (actionAttribute != null && actionAttribute.Length > 0)
                {
                    var title = (actionAttribute.First() as ActionMethodAttribute).Title;
                    mapping.Add(title, type);
                }
            }
        }
        /// <summary>
        /// 从加载的程序集中，获取测试步骤类，放在mapping中
        /// </summary>
        private static void LoadLookup()
        {
            lookups = new Dictionary<string, List<Tuple<VersionSeries, Type>>>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => p.FullName.Contains("CC.AutomatedTesting"));
            var publicTypes = assemblies.SelectMany(p => p.ExportedTypes);
            foreach (var type in publicTypes)
            {
                var actionAttribute = type.GetCustomAttributes(typeof(ActionMethodAttribute), false);
                if (actionAttribute != null && actionAttribute.Length > 0)
                {
                    var title = (actionAttribute.First() as ActionMethodAttribute).Title;
                    if (string.IsNullOrEmpty(title)) continue;
                    if (!lookups.ContainsKey(title)) lookups.Add(title, new List<Tuple<VersionSeries, Type>>());
                    var versionAttribute = type.GetCustomAttribute<VersionControlAttribute>(false);
                    var versions = VersionSeries.AnySeries;
                    if (versionAttribute != null && !string.IsNullOrEmpty(versionAttribute.Range))
                    {
                        versions = VersionSeries.FromString(versionAttribute.Range);
                    }
                    lookups[title].Add(new Tuple<VersionSeries, Type>(versions, type));
                }
            }
        }
        private static Type GetTypeByCommandVersion(string command, Version version)
        {
            if (!Lookups.ContainsKey(command) || Lookups[command] == null)
                return null;
            Type tempDefaultType = null;
            Type formerType = null;
            foreach (var kv in Lookups[command])
            {
                if (kv.Item1.IsAny())
                {
                    tempDefaultType = kv.Item2;
                    continue;//假如脚本有2个按钮，优先匹配带版本的按钮，因此Continue;
                }
                if (kv.Item1.IsBelongsToMe(version))
                {
                    formerType = kv.Item2;
                    break;
                }
            }
            if (formerType == null && tempDefaultType != null)
            {
                formerType = tempDefaultType;
            }
            return formerType;
        }
    }
}
