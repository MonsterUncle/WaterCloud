/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Code
{
    public static partial class Extensions
    {
        /// <summary>
        /// 获取表里某页的数据
        /// </summary>
        /// <param name="data">表数据</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="allPage">返回总页数</param>
        /// <returns>返回当页表数据</returns>
        public static List<T> GetPage<T>(this List<T> data, int pageIndex, int pageSize, out int allPage)
        {
            allPage = 1;
            return null;
        }
        /// <summary>
        /// IList转成List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> IListToList<T>(IList list)
        {
            T[] array = new T[list.Count];
            list.CopyTo(array, 0);
            return new List<T>(array);
        }
        /// <summary>
        /// 去除空元素
        /// </summary>
        public static List<string> removeNull(List<string> oldList)
        {
            // 临时集合
            List<string> listTemp = new List<string>();
            foreach (var item in oldList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    listTemp.Add(item);
                }

            }
            return listTemp;
        }
    }
    public class ExtList<T> : IEqualityComparer<T> where T : class, new()
    {
        private string[] comparintFiledName = { };

        public ExtList() { }
        public ExtList(params string[] comparintFiledName)
        {
            this.comparintFiledName = comparintFiledName;
        }
        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            if (x == null && y == null)
            {
                return false;
            }
            if (comparintFiledName.Length == 0)
            {
                return x.Equals(y);
            }
            bool result = true;
            var typeX = x.GetType();//获取类型
            var typeY = y.GetType();
            foreach (var filedName in comparintFiledName)
            {
                var xPropertyInfo = (from p in typeX.GetProperties() where p.Name.Equals(filedName) select p).FirstOrDefault();
                var yPropertyInfo = (from p in typeY.GetProperties() where p.Name.Equals(filedName) select p).FirstOrDefault();

                result = result
                    && xPropertyInfo != null && yPropertyInfo != null
                    && xPropertyInfo.GetValue(x, null).ToString().Equals(yPropertyInfo.GetValue(y, null));
            }
            return result;
        }
        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
