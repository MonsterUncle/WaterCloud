using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace WaterCloud.Code
{
    public class ReflectionHelper
    {
        private static ConcurrentDictionary<string, object> dictCache = new ConcurrentDictionary<string, object>();
        private static List<string> exceptionList=new List<string> { "DataFilterService", "ControllerBase" };

        #region 得到类里面的属性集合
        /// <summary>
        /// 得到类里面的属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(Type type, string[] columns = null)
        {
            PropertyInfo[] properties = null;
            if (dictCache.ContainsKey(type.FullName))
            {
                properties = dictCache[type.FullName] as PropertyInfo[];

            }
            else
            {
                properties = type.GetProperties();
                dictCache.TryAdd(type.FullName, properties);
            }

            if (columns != null && columns.Length > 0)
            {
                //  按columns顺序返回属性
                var columnPropertyList = new List<PropertyInfo>();
                foreach (var column in columns)
                {
                    var columnProperty = properties.Where(p => p.Name == column).FirstOrDefault();
                    if (columnProperty != null)
                    {
                        columnPropertyList.Add(columnProperty);
                    }
                }
                return columnPropertyList.ToArray();
            }
            else
            {
                return properties;
            }
        }

        public static object GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;
            return o;
        }
        #endregion

        /// <summary>
        /// StackTrace获取名称
        /// </summary>
        /// <param name="count">搜索层级</param>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public static string GetModuleName(int count = 5,bool isReplace = true, string prefix="Service")
        {
            try
            {
                string moduleName = "";

                for (int i = 0; i < count; i++)
				{
                    string className = new StackFrame(i, true).GetMethod().DeclaringType.FullName;
                    className = className.Split('+')[0];
                    className = className.Split('.').LastOrDefault();
                    bool skip = false;
					foreach (var item in exceptionList)
					{
						if (className.Contains(item))
						{
                            skip = true;
                            break;
						}
					}
					if (skip)
					{
                        continue;
                    }
					if (className.IndexOf(prefix)>-1)
					{
                        moduleName = className;
                        if (isReplace)
						{
                            moduleName = moduleName.Replace(prefix, "");
                        }
                    }
                }
                return moduleName;
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
                return "";
            }
        }
    }
}
