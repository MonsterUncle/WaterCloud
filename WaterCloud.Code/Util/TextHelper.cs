using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Code
{
    public class TextHelper
    {
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetCustomValue(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubString(string value, int length, bool ellipsis = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (value.Length > length)
            {
                value = value.Substring(0, length);
                if (ellipsis)
                {
                    value += "...";
                }
            }
            return value;
        }

        /// <summary>
        /// 字符串转指定类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(string value, char split)
        {
            T[] arr = value.Split(new string[] { split.ToString() }, StringSplitOptions.RemoveEmptyEntries).CastSuper<T>().ToArray();
            return arr;
        }
        /// <summary>
        /// 判断是否有交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool IsArrayIntersection<T>(List<T> list1, List<T> list2)
        {
            List<T> t = list1.Distinct().ToList();

            var exceptArr = t.Except(list2).ToList();

            if (exceptArr.Count < t.Count)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
