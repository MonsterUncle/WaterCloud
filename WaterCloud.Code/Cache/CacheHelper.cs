using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    public class CacheHelper
    {
        private static string cacheProvider = GlobalContext.SystemConfig.CacheProvider;
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长h</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public static async Task<bool> Set(string key, object value, int expiresIn=-1)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (value==null)
                throw new ArgumentNullException(nameof(value));
            switch (cacheProvider)
            {
                case Define.CACHEPROVIDER_REDIS:
                    if (expiresIn > 0)
                    {
                      await RedisHelper.SetAsync(key, value, expiresIn*3600);
                    }
                    else
                    {
                        await RedisHelper.SetAsync(key, value);
                    }
                    return await Exists(key);
                case Define.CACHEPROVIDER_MEMORY:
                    if (expiresIn>0)
                    {
                        MemoryCacheHelper.Set(key, value, TimeSpan.FromHours(expiresIn));
                    }
                    else
                    {
                        MemoryCacheHelper.Set(key, value);
                    }
                    return await Exists(key);
                default:
                    if (expiresIn > 0)
                    {
                        MemoryCacheHelper.Set(key, value, TimeSpan.FromHours(expiresIn));
                    }
                    else
                    {
                        MemoryCacheHelper.Set(key, value);
                    }
                    return await Exists(key);
            }

        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static async Task<T> Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            switch (cacheProvider)
            {
                case Define.CACHEPROVIDER_REDIS:
                    return await RedisHelper.GetAsync<T>(key);
                case Define.CACHEPROVIDER_MEMORY:
                    return MemoryCacheHelper.Get<T>(key);
                default:
                    return MemoryCacheHelper.Get<T>(key);
            }

        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static async Task Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            switch (cacheProvider)
            {
                case Define.CACHEPROVIDER_REDIS:
                    await RedisHelper.DelAsync(key);
                    break;
                case Define.CACHEPROVIDER_MEMORY:
                    MemoryCacheHelper.Remove(key);
                    break;
            }

        }
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static async Task<bool> Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            switch (cacheProvider)
            {
                case Define.CACHEPROVIDER_REDIS:
                    return  await RedisHelper.ExistsAsync(key);
                case Define.CACHEPROVIDER_MEMORY:
                    return MemoryCacheHelper.Exists(key);
                default:
                    return MemoryCacheHelper.Exists(key);
            }
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static async Task FlushAll()
        {
            switch (cacheProvider)
            {
                case Define.CACHEPROVIDER_REDIS:
                    await RedisHelper.NodesServerManager.FlushAllAsync();
                    break;
                case Define.CACHEPROVIDER_MEMORY:
                    MemoryCacheHelper.RemoveCacheAll();
                    break;
                default:
                    MemoryCacheHelper.RemoveCacheAll();
                    break;
            }
        }
    }
}
