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

namespace WaterCloud.DataBase
{
    /// <summary>
    /// 仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        private ICache cache = CacheFactory.CaChe();
        private DbContext dbcontext;
        public RepositoryBase()
        {
            dbcontext = DBContexHelper.Contex();
        }
        public RepositoryBase(string ConnectStr, string providerName)
        {
            dbcontext = DBContexHelper.Contex(ConnectStr, providerName);
        }
        public void Insert(TEntity entity)
        {
            dbcontext.Insert(entity);
        }
        public void Insert(List<TEntity> entitys)
        {
            if (dbcontext.DatabaseProvider.DatabaseType== "SqlServer")
            {
                dbcontext.BulkInsert(entitys);
            }
            else
            {
                foreach (var item in entitys)
                {
                    dbcontext.Insert(item);
                }
            }
        }
        public int Update(TEntity entity)
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
            int id = dbcontext.Update(newentity);
            return id;
        }
        public int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content)
        {
            int id = dbcontext.Update(predicate, content);
            return id;
        }
        public int Delete(TEntity entity)
        {
            int id = dbcontext.Delete(entity);
            return id;
        }
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            int id = dbcontext.Delete(predicate);
            return id;
        }
        public TEntity FindEntity(object keyValue)
        {
            return dbcontext.QueryByKey<TEntity>(keyValue);
        }
        public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
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
        public List<TEntity> FindList(string strSql)
        {
            return dbcontext.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList(string strSql, DbParam[] dbParameter)
        {
            return dbcontext.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        public List<TEntity> FindList(Pagination pagination)
        {
            var tempData = dbcontext.Query<TEntity>();
            tempData = tempData.OrderBy(pagination.sort);
            pagination.records = tempData.Count();
            tempData = tempData.TakePage(pagination.page, pagination.rows);
            return tempData.ToList();
        }
        public List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
        {
            var tempData = dbcontext.Query<TEntity>().Where(predicate);
            tempData = tempData.OrderBy(pagination.sort);
            pagination.records = tempData.Count();
            tempData = tempData.TakePage(pagination.page, pagination.rows);
            return tempData.ToList();
        }
        public List<T> OrderList<T>(IQuery<T> query, Pagination pagination)
        {
            var tempData = query;
            tempData = tempData.OrderBy(pagination.sort);
            pagination.records = tempData.Count();
            tempData = tempData.TakePage(pagination.page, pagination.rows);
            return tempData.ToList();
        }
        public List<TEntity> CheckCacheList(string cacheKey, long old = 0)
        {
            var cachedata = cache.Read<List<TEntity>>(cacheKey, old);
            if (cachedata == null || cachedata.Count() == 0)
            {
                using (var db = new RepositoryBase().BeginTrans())
                {
                    cachedata = db.IQueryable<TEntity>().ToList();
                    cache.Write(cacheKey, cachedata, old);
                }
            }
            return cachedata;
        }

        public TEntity CheckCache(string cacheKey, string keyValue, long old = 0)
        {
            var cachedata = cache.Read<TEntity>(cacheKey + keyValue, old);
            if (cachedata == null)
            {
                using (var db = new RepositoryBase().BeginTrans())
                {
                    cachedata = db.FindEntity<TEntity>(keyValue);
                    cache.Write(cacheKey + keyValue, cachedata, old);
                }
            }
            return cachedata;
        }
    }
}
