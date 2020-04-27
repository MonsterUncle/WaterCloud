/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Chloe;
using WaterCloud.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace WaterCloud.DataBase
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        void Insert(TEntity entity);
        void Insert(List<TEntity> entitys);
        int Update(TEntity entity);
        int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content);
        int Delete(TEntity entity);
        int Delete(Expression<Func<TEntity, bool>> predicate);
        TEntity FindEntity(object keyValue);
        TEntity FindEntity(Expression<Func<TEntity, bool>> predicate);
        IQuery<TEntity> IQueryable();
        IQuery<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> FindList(string strSql);
        List<TEntity> FindList(string strSql, DbParam[] dbParameter);
        List<TEntity> FindList(Pagination pagination);
        List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination);
        List<T> OrderList<T>(IQuery<T> query, Pagination pagination);
        List<TEntity> CheckCacheList(string cacheKey, long old = 0);
        TEntity CheckCache(string cacheKey, string keyValue, long old = 0);
    }
}
