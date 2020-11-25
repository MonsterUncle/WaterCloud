using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace WaterCloud.Code
{
    /// <summary>
    ///  对象处理类
    /// </summary>
    public class ObjectHelper
    {
        public class ForeachClass
        {
            /// <summary>
            /// C#反射遍历对象属性
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="model">对象</param>
            public static void ForeachClassProperties<T>(T model)
            {
                Type t = model.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    object value = item.GetValue(model, null);
                }
            }

            public static object GetValue<T>(T model, string PropertyName)
            {
                Type t = model.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    if (PropertyName == name)
                    {
                        object value = item.GetValue(model, null);
                        return value;
                    }
                }
                return null;
            }

        }
    }
}
