/*******************************************************************************
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
using System.Reflection;

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
                var entityType = typeof(TEntity);
                if (entityType.IsDefined(typeof(TenantAttribute), false))
                {
                    var tenantAttribute = entityType.GetCustomAttribute<TenantAttribute>(false)!;
                    _dbBase.ChangeDatabase(tenantAttribute.configId);
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
        /// <summary>
        /// 切换上下文，不传参切换到实体租户
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public ISqlSugarClient ChangeEntityDb(object configId = null)
		{
			if (!configId.IsEmpty())
			{
                _dbBase.ChangeDatabase(configId);
            }
			else
			{
                var entityType = typeof(TEntity);
                if (entityType.IsDefined(typeof(TenantAttribute), false))
                {
                    var tenantAttribute = entityType.GetCustomAttribute<TenantAttribute>(false)!;
                    configId = tenantAttribute.configId;
                    _dbBase.ChangeDatabase(configId);
                }
            }
            return _dbBase;
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
            return await IQueryable().InSingleAsync(keyValue);
        }
        public async Task<TEntity> FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return await IQueryable().FirstAsync(predicate);
        }
        public ISugarQueryable<TEntity> IQueryable()
        {
            return _db.Queryable<TEntity>();
        }
        public ISugarQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return IQueryable().Where(predicate);
        }
        public ISugarQueryable<TEntity> IQueryable(string strSql)
        {
            return _db.SqlQueryable<TEntity>(strSql);
        }
    }
}
