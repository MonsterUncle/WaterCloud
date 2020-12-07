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
using System.Data;
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
    public class RepositoryBase : IRepositoryBase, IDisposable
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
        public IRepositoryBase BeginTrans()
        {
            if (dbcontext.Session.CurrentTransaction == null)
            {
                dbcontext.Session.BeginTransaction();
            }
            return this;
        }
        public void Commit()
        {
            try
            {
                if (dbcontext.Session.CurrentTransaction != null)
                {
                    dbcontext.Session.CommitTransaction();
                }
            }
            catch (Exception)
            {
                if (dbcontext.Session.CurrentTransaction != null)
                {
                    dbcontext.Session.RollbackTransaction();
                }
                throw;
            }
        }
        public void Dispose()
        {
            if (dbcontext.Session.CurrentTransaction != null)
            {
                dbcontext.Session.Dispose();
            }
        }
        public void Rollback()
        {
            if (dbcontext.Session.CurrentTransaction != null)
            {
                dbcontext.Session.RollbackTransaction();
            }
        }
        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            dbcontext.Insert(entity);
        }
        public void Insert<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            if (dbcontext.DatabaseProvider.DatabaseType=="SqlServer")
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
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {

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
        public int Update<TEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content) where TEntity : class
        {
            int id = dbcontext.Update(predicate, content);
            return id;
        }
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            int id = dbcontext.Delete(entity);
            return id;
        }
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            int id = dbcontext.Delete(predicate);
            return id;
        }
        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class
        {
            return dbcontext.QueryByKey<TEntity>(keyValue);
        }
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Query<TEntity>().FirstOrDefault(predicate);
        }
        public IQuery<TEntity> IQueryable<TEntity>() where TEntity : class
        {
            return dbcontext.Query<TEntity>();
        }
        public IQuery<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Query<TEntity>().Where(predicate);
        }
        public List<TEntity> FindList<TEntity>(string strSql) where TEntity : class
        {
            return dbcontext.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(string strSql, DbParam[] dbParameter) where TEntity : class
        {
            return dbcontext.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(Pagination pagination) where TEntity : class, new()
        {
            var tempData = dbcontext.Query<TEntity>();
            pagination.records = tempData.Count();
            tempData = tempData.OrderBy(pagination.sort);
            tempData = tempData.TakePage(pagination.page, pagination.rows);
            return tempData.ToList();
        }
        public List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class, new()
        {
            var tempData = dbcontext.Query<TEntity>().Where(predicate);
            pagination.records = tempData.Count();
            tempData = tempData.OrderBy(pagination.sort);
            tempData = tempData.TakePage(pagination.page, pagination.rows);
            return tempData.ToList();
        }
        public List<T> OrderList<T>(IQuery<T> query, Pagination pagination)
        {
            var tempData = query;
            pagination.records = tempData.Count();
            tempData = tempData.OrderBy(pagination.sort);
            tempData = tempData.TakePage(pagination.page, pagination.rows);
            return tempData.ToList();
        }
        public List<TEntity> CheckCacheList<TEntity>(string cacheKey, long old = 0) where TEntity : class
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

        public TEntity CheckCache<TEntity>(string cacheKey, string keyValue, long old = 0) where TEntity : class
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
