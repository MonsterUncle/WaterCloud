using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.ContentManage;
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻类别服务类
    /// </summary>
    public class ArticleCategoryService : DataFilterService<ArticleCategoryEntity>, IDenpendency
    {
        public ArticleCategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }   
        #region 获取数据
        public async Task<List<ArticleCategoryEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_FullName.Contains(keyword) || a.F_Description.Contains(keyword));
            }
            return await query.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_Id,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<ArticleCategoryEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a =>a.F_FullName.Contains(keyword) || a.F_Description.Contains(keyword));
            }
            query = GetDataPrivilege("a","", query);
            return await query.OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<List<ArticleCategoryEntity>> GetLookList(Pagination pagination,string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_FullName.Contains(keyword) || a.F_Description.Contains(keyword));
            }
            //权限过滤
            query = GetDataPrivilege("a","", query);
            return await repository.OrderList(query, pagination);
        }

        public async Task<ArticleCategoryEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task<ArticleCategoryEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(ArticleCategoryEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_DeleteMark = false;
                //此处需修改
                entity.Create();
                await repository.Insert(entity);
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue); 
                await repository.Update(entity);
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            if (await repository.Db.Queryable<ArticleNewsEntity>().Where(a=> ids.Contains(a.F_CategoryId)).AnyAsync())
            {
                throw new Exception("新闻类别使用中，无法删除");
            }
            await repository.Delete(t => ids.Contains(t.F_Id));
        }
        #endregion

    }
}
