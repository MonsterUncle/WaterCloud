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
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemSecurity
{
    public class FilterIPService: IDenpendency
    {
        private IFilterIPRepository service = new FilterIPRepository();
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_filterip_";// IP过滤

        public async Task<List<FilterIPEntity>> GetList(string keyword)
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_StartIP.Contains(keyword)).ToList();

            }
            return cachedata.OrderBy(t => t.F_CreatorTime).ToList();
        }
        public async Task<FilterIPEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task DeleteForm(string keyValue)
        {
            await service.Delete(t => t.F_Id == keyValue);
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
        }
        public async Task<bool> CheckIP(string ip)
        {
            var list =await GetList("");
            list = list.Where(a => a.F_EnabledMark == true).ToList();
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
        public async Task SubmitForm(FilterIPEntity filterIPEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                filterIPEntity.Modify(keyValue);
                await service.Update(filterIPEntity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
            }
            else
            {
                filterIPEntity.Create();
                await service.Insert(filterIPEntity);
            }
            await RedisHelper.DelAsync(cacheKey + "list");
        }
    }
}
