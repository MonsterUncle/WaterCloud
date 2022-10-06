using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-10-06 11:25
    /// 描 述：条码规则服务类
    /// </summary>
    public class CoderuleService : BaseService<CoderuleEntity>, IDenpendency
    {
        public CoderuleService(ISqlSugarClient context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<CoderuleEntity>> GetList(string keyword = "")
        {
            var data = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.F_RuleName.Contains(keyword));
            }
            return await data.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_Id , OrderByType.Desc).ToListAsync();
        }

        public async Task<List<CoderuleEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_RuleName.Contains(keyword));
            }
             //权限过滤
             query = GetDataPrivilege("a", "", query);
             return await query.OrderBy(a => a.F_Id , OrderByType.Desc).ToListAsync();
        }

        public async Task<List<CoderuleEntity>> GetLookList(SoulPage<CoderuleEntity> pagination,string keyword = "",string id="")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_RuleName.Contains(keyword));
            }
            if(!string.IsNullOrEmpty(id))
            {
                query= query.Where(a=>a.F_Id==id);
            }
            //权限过滤
            query = GetDataPrivilege("a","",query);
            return  await query.ToPageListAsync(pagination);
        }

        public async Task<CoderuleEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        public async Task<CoderuleEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }

        #region 提交数据
        public async Task SubmitForm(CoderuleEntity entity, string keyValue)
        {
            if(string.IsNullOrEmpty(keyValue))
            {
                    //初始值添加
                entity.F_DeleteMark = false;
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
            await repository.Delete(a => ids.Contains(a.F_Id.ToString()));
        }
        #endregion

    }
}
