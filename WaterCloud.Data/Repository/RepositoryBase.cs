﻿/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;

namespace WaterCloud.DataBase
{
	/// <summary>
	/// 泛型仓储实现
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        private SqlSugarClient _dbBase;
        // 用于其他表操作
        private readonly IUnitOfWork _unitOfWork;
        private ISqlSugarClient _db
        {
            get
            {
                if (typeof(TEntity).GetTypeInfo().GetCustomAttributes(typeof(SugarTable), true).FirstOrDefault((x => x.GetType() == typeof(SugarTable))) is SugarTable sugarTable && !string.IsNullOrEmpty(sugarTable.TableDescription))
                {
                    _dbBase.ChangeDatabase(sugarTable.TableDescription.ToLower());
                }
                return _dbBase;
            }
        }
        public ISqlSugarClient Db
        {
            get { return _db; }
        }
        public IUnitOfWork unitOfWork
        {
            get { return _unitOfWork; }
        }
        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbBase = unitOfWork.GetDbClient();
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
           return await _db.Insertable(entity).ExecuteReturnEntityAsync();
        }
        public async Task<int> Insert(List<TEntity> entitys)
        {
            return await _db.Insertable(entitys).ExecuteCommandAsync();
        }
        public async Task<int> Update(TEntity entity)
        {
            return await _db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }
        public async Task<int> Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> content)
        {
            return await _db.Updateable(content).Where(predicate).ExecuteCommandAsync();
        }
        public async Task<int> Delete(TEntity entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandAsync();
        }
        public async Task<int> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Deleteable(predicate).ExecuteCommandAsync();
        }
        public async Task<TEntity> FindEntity(object keyValue)
        {
            return await _db.Queryable<TEntity>().InSingleAsync(keyValue);
        }
        public async Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Queryable<TEntity>().FirstAsync(predicate);
        }
        public ISugarQueryable<TEntity> IQueryable()
        {
            return _db.Queryable<TEntity>();
        }
        public ISugarQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Queryable<TEntity>().Where(predicate);
        }
        public ISugarQueryable<TEntity> IQueryable(string strSql)
        {
            return _db.SqlQueryable<TEntity>(strSql);
        }
        public async Task<List<T>> OrderList<T>(ISugarQueryable<T> query, Pagination pagination)
        {
            var tempData = query;
            int totalCount = 0;
            if (pagination.order == "desc")
            {
                tempData = tempData.OrderBy(pagination.sort + " " + pagination.order);
            }
            else
            {
                tempData = tempData.OrderBy(pagination.sort);
            }
            var data = tempData.ToPageList(pagination.page, pagination.rows, ref totalCount);
            pagination.records = totalCount;
            return data;
        }
        public async Task<List<T>> OrderList<T>(ISugarQueryable<T> query, SoulPage<T> pagination)
        {
            var tempData = query;
            List<FilterSo> filterSos = pagination.getFilterSos();
            if (filterSos!=null && filterSos.Count>0)
            {
                tempData = tempData.GenerateFilter("a", filterSos);
            }
            if (pagination.order == "desc")
            {
                tempData = tempData.OrderBy(pagination.field + " " + pagination.order);
            }
            else
            {
                tempData = tempData.OrderBy(pagination.field);
            }
            int totalCount = 0;
            var data = tempData.ToPageList(pagination.page, pagination.rows, ref totalCount);
            pagination.count = totalCount;
            return data;
        }
        public async Task<List<TEntity>> CheckCacheList(string cacheKey)
        {
            var cachedata =await CacheHelper.Get<List<TEntity>>(cacheKey);
            if (cachedata == null || cachedata.Count() == 0)
            {
                cachedata = _db.Queryable<TEntity>().ToList();
                await CacheHelper.Set(cacheKey, cachedata);
            }
            return cachedata;
        }
        public async Task<TEntity> CheckCache(string cacheKey, object keyValue)
        {
            var cachedata = await CacheHelper.Get<TEntity>(cacheKey + keyValue);
            if (cachedata == null)
            {
                cachedata = await _db.Queryable<TEntity>().InSingleAsync(keyValue);
                if (cachedata != null)
                {
                    await CacheHelper.Set(cacheKey + keyValue, cachedata);
                }
            }
            return cachedata;
        }
    }
}
