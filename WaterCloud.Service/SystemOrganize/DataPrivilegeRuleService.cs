using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Service.SystemOrganize
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-06-01 09:44
	/// 描 述：数据权限服务类
	/// </summary>
	public class DataPrivilegeRuleService : BaseService<DataPrivilegeRuleEntity>, IDenpendency
	{
		public DataPrivilegeRuleService(ISqlSugarClient context) : base(context)
		{
		}

		//获取类名

		#region 获取数据

		public async Task<List<DataPrivilegeRuleEntity>> GetList(string keyword = "")
		{
			var list = repository.IQueryable();
			if (!string.IsNullOrEmpty(keyword))
			{
				list = list.Where(a => a.F_ModuleCode.Contains(keyword) || a.F_Description.Contains(keyword));
			}
			return await list.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
		}

		public async Task<List<DataPrivilegeRuleEntity>> GetLookList(SoulPage<DataPrivilegeRuleEntity> pagination, string keyword = "")
		{
			//反格式化显示只能用"等于"，其他不支持
			Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
			Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
			enabledTemp.Add("1", "有效");
			enabledTemp.Add("0", "无效");
			dic.Add("F_EnabledMark", enabledTemp);
			pagination = ChangeSoulData(dic, pagination);
			var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(a => a.F_ModuleCode.Contains(keyword) || a.F_Description.Contains(keyword));
			}
			query = GetDataPrivilege("a", "", query);
			return await query.ToPageListAsync(pagination);
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

		#endregion 获取数据

		#region 提交数据

		public async Task SubmitForm(DataPrivilegeRuleEntity entity, string keyValue)
		{
			entity.F_ModuleCode = repository.Db.Queryable<ModuleEntity>().InSingle(entity.F_ModuleId).F_EnCode;
			if (string.IsNullOrEmpty(keyValue))
			{
				entity.F_DeleteMark = false;
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
			await repository.Delete(a => a.F_Id == keyValue);
		}

		#endregion 提交数据
	}
}