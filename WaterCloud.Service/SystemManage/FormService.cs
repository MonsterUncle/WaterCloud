using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-08 14:33
    /// 描 述：表单设计服务类
    /// </summary>
    public class FormService : DataFilterService<FormEntity>, IDenpendency
    {
        public FormService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<FormEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Name.Contains(keyword) || a.Description.Contains(keyword));
            }
            return await query.Where(a => a.DeleteMark == false).OrderBy(a => a.Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<List<FormEntity>> GetLookList(string ItemId="", string keyword = "")
        {
            var query = GetQuery().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(ItemId))
            {
                query = query.Where(a => a.OrganizeId == ItemId || a.OrganizeId == null || a.OrganizeId == "");
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Name.Contains(keyword) || a.Description.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await query.Where(a => a.DeleteMark == false).OrderBy(a => a.Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<List<FormEntity>> GetLookList(Pagination pagination,string keyword = "")
        {
            //获取数据权限
            var query = GetQuery().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Name.Contains(keyword) || a.Description.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await repository.OrderList(query, pagination);
        }
        private ISugarQueryable<FormEntity> GetQuery()
        {
            var query = repository.Db.Queryable<FormEntity, OrganizeEntity>((a,b)=>new JoinQueryInfos(
                    JoinType.Left,a.OrganizeId==b.Id            
                ))
                .Select((a, b) => new FormEntity
                {
                    Id = a.Id.SelectAll(),
                    OrganizeName = b.FullName,
                }).MergeTable();
            return query;
        }
        public async Task<FormEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task<FormEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(FormEntity entity, string keyValue)
        {
            if (entity.FrmType!=1)
            {
                var temp = FormUtil.SetValue(entity.Content);
                entity.ContentData =string.Join(',', temp.ToArray()) ;
                entity.Fields = temp.Count();
            }
            else
            {
                var temp = FormUtil.SetValueByWeb(entity.WebId);
                entity.ContentData = string.Join(',', temp.ToArray());
                entity.Fields = temp.Count();
            }
            if (string.IsNullOrEmpty(keyValue))
            {
                //此处需修改
                entity.DeleteMark = false;
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
            await repository.Delete(a => ids.Contains(a.Id));
        }
        #endregion

    }
}
