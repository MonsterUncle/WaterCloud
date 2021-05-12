/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WaterCloud.Code;

namespace WaterCloud.DataBase
{
	public interface IRepositoryBase<TEntity> where TEntity : class, new()
	{
		/// <summary>
		/// SqlsugarClient实体
		/// </summary>
		ISqlSugarClient Db { get; }
		Task<TEntity> Insert(TEntity entity);
		Task<int> Insert(List<TEntity> entitys);
        Task<int> Update(TEntity entity);
        Task<int> Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content);
        Task<int> Delete(TEntity entity);
        Task<int> Delete(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindEntity(object keyValue);
        Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> predicate);
        ISugarQueryable<TEntity> IQueryable();
        ISugarQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate);
        ISugarQueryable<TEntity> IQueryable(string strSql);
        Task<List<T>> OrderList<T>(ISugarQueryable<T> query, Pagination pagination);
        Task<List<T>> OrderList<T>(ISugarQueryable<T> query, SoulPage<T> pagination);
        Task<List<TEntity>> CheckCacheList(string cacheKey);
        Task<TEntity> CheckCache(string cacheKey, object keyValue);
    }
}