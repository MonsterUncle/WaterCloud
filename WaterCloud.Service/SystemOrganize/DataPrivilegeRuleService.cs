using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Domain.SystemManage;
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-01 09:44
    /// 描 述：数据权限服务类
    /// </summary>
    public class DataPrivilegeRuleService : DataFilterService<DataPrivilegeRuleEntity>,IDenpendency
    {
        public DataPrivilegeRuleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        //获取类名
        
        #region 获取数据
        public async Task<List<DataPrivilegeRuleEntity>> GetList(string keyword = "")
        {
            var list =  repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(a => a.ModuleCode.Contains(keyword) || a.Description.Contains(keyword));
            }
            return await list.Where(a => a.DeleteMark == false).OrderBy(a => a.Id,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<DataPrivilegeRuleEntity>> GetLookList(SoulPage<DataPrivilegeRuleEntity> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("EnabledMark", enabledTemp);
            pagination = ChangeSoulData(dic, pagination);
            var query = repository.IQueryable().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.ModuleCode.Contains(keyword) || a.Description.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await repository.OrderList(query, pagination);
        }

        public async Task<DataPrivilegeRuleEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<DataPrivilegeRuleEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        #region 提交数据
        public async Task SubmitForm(DataPrivilegeRuleEntity entity, string keyValue)
        {
            entity.ModuleCode = repository.Db.Queryable<ModuleEntity>().InSingle(entity.ModuleId).EnCode;
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
            }
            else
            {
                entity.Modify(keyValue); 
                await repository.Update(entity);
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            await repository.Delete(a => a.Id == keyValue);
        }
        #endregion

    }
}
