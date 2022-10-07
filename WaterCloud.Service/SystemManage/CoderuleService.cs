using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemOrganize;

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
            var data = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.F_RuleName.Contains(keyword));
            }
            return await data.OrderBy(a => a.F_Id , OrderByType.Desc).ToListAsync();
        }

        public async Task<List<CoderuleEntity>> GetLookList(string keyword = "")
        {
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_RuleName.Contains(keyword));
            }
            //权限过滤
            query = GetDataPrivilege("a", query: query);
             return await query.OrderBy(a => a.F_Id , OrderByType.Desc).ToListAsync();
        }

        public async Task<List<CoderuleEntity>> GetLookList(SoulPage<CoderuleEntity> pagination,string keyword = "",string id="")
        {
			//反格式化显示只能用"等于"，其他不支持
			Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
			Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
			enabledTemp.Add("1", "有效");
			enabledTemp.Add("0", "无效");
			dic.Add("F_EnabledMark", enabledTemp);
			var setList = await GlobalContext.GetService<ItemsDataService>().GetItemList("RuleReset");
			Dictionary<string, string> resetTemp = new Dictionary<string, string>();
			foreach (var item in setList)
			{
				resetTemp.Add(item.F_ItemCode, item.F_ItemName);
			}
			dic.Add("F_Reset", resetTemp);
			var printList = await GlobalContext.GetService<ItemsDataService>().GetItemList("PrintType");
			Dictionary<string, string> printTemp = new Dictionary<string, string>();
			foreach (var item in printList)
			{
				printTemp.Add(item.F_ItemCode, item.F_ItemName);
			}
			dic.Add("F_PrintType", printTemp);
			pagination = ChangeSoulData(dic, pagination);
			var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_RuleName.Contains(keyword));
            }
            if(!string.IsNullOrEmpty(id))
            {
                query = query.Where(a => a.F_Id == id);
            }
            //权限过滤
            query = GetDataPrivilege("a", query: query);
            return  await query.ToPageListAsync(pagination);
        }

        public async Task<CoderuleEntity> GetForm(string keyValue)
        {
			var data = await GetQuery().FirstAsync(a => a.F_Id == keyValue);
			return data;
        }
		private ISugarQueryable<CoderuleEntity> GetQuery()
		{
			var query = repository.IQueryable()
                .InnerJoin<TemplateEntity>((a,b)=>a.F_TemplateId == b.F_Id)
				.Select((a, b) => new CoderuleEntity
				{
					F_Id=a.F_Id.SelectAll(),
                    F_TemplateName = b.F_TemplateName,
                    F_PrintType = b.F_PrintType,
                    F_Batch=b.F_Batch
				}).MergeTable().Where(a => a.F_DeleteMark == false);
			return query;
		}
		public async Task<CoderuleEntity> GetLookForm(string keyValue)
		{
            var data = await GetQuery().FirstAsync(a => a.F_Id == keyValue);
			return GetFieldsFilterData(data);
		}
		#endregion


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
