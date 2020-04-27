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
            bool isAsc = pagination.order.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sort.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Query<TEntity>().AsEnumerable().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        public List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
        {
            bool isAsc = pagination.order.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sort.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Query<TEntity>().Where(predicate).AsEnumerable().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        public List<T> OrderList<T>(IQuery<T> query, Pagination pagination)
        {
            bool isAsc = pagination.order.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sort.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = query.AsEnumerable().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<T>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<T>(pagination.rows * (pagination.page - 1)).Take<T>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        public List<TEntity> CheckCacheList(string cacheKey, long old = 0)
        {
            var cachedata = RedisHelper.Get<List<TEntity>>(cacheKey);
            if (cachedata == null || cachedata.Count() == 0)
            {
                using (var db = new RepositoryBase().BeginTrans())
                {
                    cachedata = db.IQueryable<TEntity>().ToList();
                    RedisHelper.Set(cacheKey, cachedata);
                }
            }
            return cachedata;
        }

        public TEntity CheckCache(string cacheKey, string keyValue, long old = 0)
        {
            var cachedata = RedisHelper.Get<TEntity>(cacheKey + keyValue);
            if (cachedata == null)
            {
                using (var db = new RepositoryBase().BeginTrans())
                {
                    cachedata = db.FindEntity<TEntity>(keyValue);
                    RedisHelper.Set(cacheKey + keyValue, cachedata);
                }
            }
            return cachedata;
        }
    }
}
