using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using WaterCloud.Code;

namespace WaterCloud.Code
{
    public class FormUtil {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>System.String.</returns>
        public static List<string> SetValue(string content)
        {
            List<FormValue> list = JsonHelper.ToObject<List<FormValue>>(content);
            List<string> temp = new List<string>();
            SetFormValue(list, temp);
            return temp;
        }

        private static List<string> SetFormValue(List<FormValue> list, List<string> temp)
        {
            foreach (var item in list)
            {
                if (item.tag == "grid")
                {
                    foreach (var column in item.columns)
                    {
                        SetFormValue(column.list, temp);
                    }
                }
                else
                {
                    temp.Add(item.id);
                }
            }
            return temp;
        }

        public static List<string> SetValueByWeb(string webForm)
        {
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var t = referencedAssemblies
                .SelectMany(a => a.GetTypes().Where(t => t.FullName.Contains("WaterCloud.Domain.FlowManage." + webForm + "Entity"))).FirstOrDefault();
            List<string> temp = new List<string>();
            PropertyInfo[] pArray = t.GetProperties();
            Array.ForEach<PropertyInfo>(pArray, p =>
            {
                temp.Add(p.Name);
            });
            return temp;
        }
    }
}
