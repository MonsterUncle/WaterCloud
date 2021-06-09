using SqlSugar;
using System;
using System.Collections.Generic;
using WaterCloud.Code;

namespace WaterCloud.DataBase
{
    public class SqlSugarRedisCache : ICacheService
    {
        public void Add<TV>(string key, TV value)
        {
            BaseHelper.Set(key, value);
        }

        public void Add<TV>(string key, TV value, int cacheDurationInSeconds)
        {
            BaseHelper.Set(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<TV>(string key)
        {
            return BaseHelper.Exists(key);
        }

        public TV Get<TV>(string key)
        {
            return BaseHelper.Get<TV>(key);
        }

        public IEnumerable<string> GetAllKey<TV>()
        {

            return BaseHelper.Keys("SqlSugarDataCache.*");
        }

        public TV GetOrCreate<TV>(string cacheKey, Func<TV> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (this.ContainsKey<TV>(cacheKey))
            {
                return this.Get<TV>(cacheKey);
            }
            else
            {
                var result = create();
                this.Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<TV>(string key)
        {
            BaseHelper.Del(key);
        }
    }

}
