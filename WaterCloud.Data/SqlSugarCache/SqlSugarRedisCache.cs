using SqlSugar;
using System;
using System.Collections.Generic;
using WaterCloud.Code;

namespace WaterCloud.DataBase
{
    public class SqlSugarCache : ICacheService
    {
        public void Add<TV>(string key, TV value)
        {
           CacheHelper.SetBySecond(key, value);
        }

        public void Add<TV>(string key, TV value, int cacheDurationInSeconds)
        {
           CacheHelper.SetBySecond(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<TV>(string key)
        {
            return CacheHelper.Exists(key);
        }

        public TV Get<TV>(string key)
        {
            return CacheHelper.Get<TV>(key);
        }

        public IEnumerable<string> GetAllKey<TV>()
        {

            return CacheHelper.GetAllKey<TV>();
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
            CacheHelper.Remove(key);
        }
    }

}
