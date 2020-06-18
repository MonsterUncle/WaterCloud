/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe;
using Chloe.SqlServer;
using WaterCloud.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WaterCloud.DataBase
{
    /// <summary>
    /// 仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        private DbContext dbcontext;
        public RepositoryBase()
        {
            dbcontext = DBContexHelper.Contex();
        }
        public DbContext GetDbContext()
        {
            return dbcontext;
        }

        public RepositoryBase(string ConnectStr, string providerName)
        {
            dbcontext = DBContexHelper.Contex(ConnectStr, providerName);
        }
        public async Task<TEntity> Insert(TEntity entity)
        {
           return await dbcontext.InsertAsync(entity);
        }
        public async Task<int> Insert(List<TEntity> entitys)
        {
            int i = 1;
            await dbcontext.InsertRangeAsync(entitys);
            return i;
        }
        public async Task<int> Update(TEntity entity)
        {
            //反射对比更新对象变更
            TEntity newentity = dbcontext.QueryByKey<TEntity>(entity);
            dbcontext.TrackEntity(newentity);
            PropertyInfo[] newprops = newentity.GetType().GetProperties();
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    PropertyInfo item = newprops.Where(a => a.Name == prop.Name).FirstOrDefault();
                    if (item != null)
                    {
                        item.SetValue(newentity, prop.GetValue(entity, null), null);
                        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                            item.SetValue(newentity, null, null);
                    }
                }
            }
            return await dbcontext.UpdateAsync(newentity);
        }
        public async Task<int> Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content)
        {
            return await dbcontext.UpdateAsync(predicate, content);
        }
        public async Task<int> Delete(TEntity entity)
        {
            return await dbcontext.DeleteAsync(entity);
        }
        public async Task<int> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbcontext.DeleteAsync(predicate);
        }
        public async Task<TEntity> FindEntity(object keyValue)
        {
            return await dbcontext.QueryByKeyAsync<TEntity>(keyValue);
        }
        public async Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Query<TEntity>().FirstOrDefault(predicate);
        }
        public IQuery<TEntity> IQueryable()
        {
            return dbcontext.Query<TEntity>();
        }
        public IQuery<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Query<TEntity>().Where(predicate);
        }
        public async Task<List<TEntity>> FindList(string strSql)
        {
            return await dbcontext.SqlQueryAsync<TEntity>(strSql);
        }
        public async Task<List<TEntity>> FindList(string strSql, DbParam[] dbParameter)
        {
            return await dbcontext.SqlQueryAsync<TEntity>(strSql, dbParameter);
        }
        public async Task<List<TEntity>> FindList(Pagination pagination)
        {
            var tempData = dbcontext.Query<TEntity>();
            tempData = tempData.OrderBy(pagination.sort);
            pagination.records = tempData.Count();
            tempData = tempData.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows);
            return tempData.ToList();
        }
        public async Task<List<TEntity>> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
        {
            var tempData = dbcontext.Query<TEntity>().Where(predicate);
            tempData = tempData.OrderBy(pagination.sort);
            pagination.records = tempData.Count();
            tempData = tempData.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows);
            return tempData.ToList();
        }
        public async Task<List<T>> OrderList<T>(IQuery<T> query, Pagination pagination)
        {
            var tempData = query;
            tempData = tempData.OrderBy(pagination.sort);
            pagination.records = tempData.Count();
            tempData = tempData.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows);
            return tempData.ToList();
        }
        public async Task<List<TEntity>> CheckCacheList(string cacheKey, long old = 0)
        {
            var cachedata =await CacheHelper.Get<List<TEntity>>(cacheKey);
            if (cachedata == null || cachedata.Count() == 0)
            {
                cachedata = dbcontext.Query<TEntity>().ToList();
                await CacheHelper.Set(cacheKey, cachedata);
            }
            return cachedata;
        }

        public async Task<TEntity> CheckCache(string cacheKey, string keyValue, long old = 0)
        {
            var cachedata = await CacheHelper.Get<TEntity>(cacheKey + keyValue);
            if (cachedata == null)
            {
                cachedata = await dbcontext.QueryByKeyAsync<TEntity>(keyValue);
                await CacheHelper.Set(cacheKey + keyValue, cachedata);
            }
            return cachedata;
        }
    }
}
