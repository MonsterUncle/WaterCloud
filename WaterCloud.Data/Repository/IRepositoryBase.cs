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
        /// <summary>
        /// 切换上下文，不传参切换到实体租户
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        ISqlSugarClient ChangeEntityDb(object configId = null);
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
    }
}