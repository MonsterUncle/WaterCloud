/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Repository.SystemSecurity;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WaterCloud.Service.SystemSecurity
{
    public class FilterIPService: IDenpendency
    {
        private IFilterIPRepository service = new FilterIPRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_filterip_";// 区域

        public List<FilterIPEntity> GetList(string keyword)
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_StartIP.Contains(keyword)).ToList();

            }
            return cachedata.OrderBy(t => t.F_CreatorTime).ToList();
        }
        public FilterIPEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
            RedisHelper.Del(cacheKey + keyValue);
            RedisHelper.Del(cacheKey + "list");
        }
        public bool CheckIP(string ip)
        {
            var list = GetList("");
            foreach (var item in list)
            {
                if (item.F_Type == false)
                {
                    long start = IP2Long(item.F_StartIP);
                    long end = IP2Long(item.F_EndIP);
                    long ipAddress = IP2Long(ip);
                    bool inRange = (ipAddress >= start && ipAddress <= end);
                    if (inRange)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static long IP2Long(string ip)
        {
            string[] ipBytes;
            double num = 0;
            if (!string.IsNullOrEmpty(ip))
            {
                ipBytes = ip.Split('.');
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(ipBytes[i]) % 256) * Math.Pow(256, (3 - i)));
                }
            }
            return (long)num;
        }
        public void SubmitForm(FilterIPEntity filterIPEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                filterIPEntity.Modify(keyValue);
                service.Update(filterIPEntity);
                RedisHelper.Del(cacheKey + keyValue);
                RedisHelper.Del(cacheKey + "list");
            }
            else
            {
                filterIPEntity.Create();
                service.Insert(filterIPEntity);
                RedisHelper.Del(cacheKey + "list");
            }
        }
    }
}
