using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Code.Model;

namespace WaterCloud.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-10-06 14:18
    /// 描 述：条码生成记录服务类
    /// </summary>
    public class CodegeneratelogService : BaseService<CodegeneratelogEntity>, IDenpendency
    {
        public CodegeneratelogService(ISqlSugarClient context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<CodegeneratelogEntity>> GetList(string keyword = "")
        {
            var data = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.F_Code.Contains(keyword)
                || a.F_RuleName.Contains(keyword));
            }
            return await data.Where(a => a.F_DeleteMark == false).OrderBy(a => a.F_Id , OrderByType.Desc).ToListAsync();
        }

        public async Task<List<CodegeneratelogEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_Code.Contains(keyword)
				|| a.F_RuleName.Contains(keyword));
            }
             //权限过滤
             query = GetDataPrivilege("a", "", query);
             return await query.OrderBy(a => a.F_Id , OrderByType.Desc).ToListAsync();
        }

        public async Task<List<CodegeneratelogEntity>> GetLookList(SoulPage<CodegeneratelogEntity> pagination,string keyword = "",string id="")
        {
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_Code.Contains(keyword)
				|| a.F_RuleName.Contains(keyword));
            }
            if(!string.IsNullOrEmpty(id))
            {
                query= query.Where(a=>a.F_Id==id);
            }
            //权限过滤
            query = GetDataPrivilege("a","",query);
            return  await query.ToPageListAsync(pagination);
        }

        public async Task<CodegeneratelogEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        public async Task<CodegeneratelogEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }

        #region 提交数据
		public async Task<List<PrintEntity>> Reprint(string keyValue, int count = 1)
		{
            var list = new List<PrintEntity>();
			var data = await repository.FindEntity(keyValue);
			var rule = await repository.Db.Queryable<CoderuleEntity>().FirstAsync(a => a.F_Id == data.F_RuleId);
			var template = await repository.Db.Queryable<TemplateEntity>().FirstAsync(a => a.F_Id == rule.F_TemplateId);
            if (template.F_Batch == true)
            {
				PrintEntity entity = new PrintEntity();
				entity.data = new PrintDetail();
				entity.data.printIniInfo = new PrintInitInfo();
				entity.data.printIniInfo.printType = template.F_PrintType;
				entity.data.printIniInfo.isBatch = template.F_Batch;
				entity.data.printIniInfo.realName = template.F_TemplateName;
				entity.data.printIniInfo.filePath = (GlobalContext.HttpContext.Request.IsHttps ? "https://" : "http://") + GlobalContext.HttpContext.Request.Host + template.F_TemplateFile;
				entity.requestId = Utils.GetGuid();
                var listJson = new List<Dictionary<string, string>>();
                for (int i = 0; i < count; i++)
                {
					listJson.Add(data.F_PrintJson.ToObject<Dictionary<string, string>>());
				}
                entity.data.data = listJson;
				list.Add(entity);
			}
            else
            {
                for (int i = 0; i < count; i++)
                {
					PrintEntity entity = new PrintEntity();
					entity.data = new PrintDetail();
					entity.data.printIniInfo = new PrintInitInfo();
					entity.data.printIniInfo.printType = template.F_PrintType;
					entity.data.printIniInfo.isBatch = template.F_Batch;
					entity.data.printIniInfo.realName = template.F_TemplateName;
					entity.data.printIniInfo.filePath = (GlobalContext.HttpContext.Request.IsHttps ? "https://" : "http://") + GlobalContext.HttpContext.Request.Host + template.F_TemplateFile;
					entity.requestId = Utils.GetGuid();
					entity.data.data = data.F_PrintJson.ToObject<Dictionary<string, string>>();
					list.Add(entity);
				}
			}
            //更新打印次数
            await repository.Update(a => a.F_Id == keyValue, a => new CodegeneratelogEntity
            {
                F_PrintCount = a.F_PrintCount + count
            });
			return list;
		}
		#endregion

	}
}
