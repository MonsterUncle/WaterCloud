using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Code.Model;
using System.Globalization;
using NetTaste;

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
            var ids = keyValue.Split(',');
            await repository.Delete(a => ids.Contains(a.F_Id.ToString()));
        }
		/// <summary>
		/// 计算编码规则生成流水号<para>注意：此编码规则只支持单个流水号</para>
		/// </summary>
		/// <param name="ruleName">规则名称</param>
		/// <param name="count">生成数量</param>
		/// <param name="preview">是否预览,预览不新增条码不操作Redis zincr</param>
		/// <param name="replaceList">通配符替换列表</param>
		/// <returns></returns>
		public async Task<List<string>> GetBillNumber(
			string ruleName,
			int count,
			bool preview = false,
			List<string> replaceList = null)
		{
			var now = DateTime.Now;
			var rule = await repository.Db.Queryable<CoderuleEntity>().FirstAsync(a => a.F_RuleName == ruleName);
			var resetRule = "NoReset";
			switch (rule.F_Reset)
			{
				//By 年 月 日
				case "yyyy":
				case "yyyy-MM":
				case "yyyy-MM-dd":
					resetRule = DateTime.Now.ToString(rule.F_Reset); break;
				//By 周
				case "yyyy-MM-WW":
					var gc = new GregorianCalendar();
					var thisWeek = gc.GetWeekOfYear(now, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
					resetRule = DateTime.Now.ToString("yyyy") + "W" + thisWeek; break;
				default:
					break;
			}
			var codeRuleFormatList = rule.F_RuleJson.ToObject<List<CodeRuleFormatEntity>>();
			//格式化
			var format = string.Empty;
			var reset = string.Empty;
			//流水号长度
			int flowNumberLength = 5;
			//流水号初始值
			int initVal = 0;
			//流水号最大值
			int? maxVal = null;
			//默认10进制
			int toBase = 10;
			//步长
			decimal increment = 1;

			string diyCode = string.Empty;

			char replaceChar = '*';

			List<string> list = new List<string>();

			codeRuleFormatList.ForEach(item =>
			{
				//编码前缀类型 1 - 固定参数 2 - 日期 3 - 年 4 - 月 5 - 日 6 - 周别 7 - 周几 8 - 小时 9 - 上午下午 10 - 班别 11 - 流水号 12 - 自定义
				switch (item.FormatType)
				{
					//固定参数
					case 1:
						format += item.FormatString;
						break;
					//日期
					case 2:
						format += now.ToString(item.FormatString);
						break;
					//年
					case 3:
						var ye = now.ToString("yyyy");
						//判断是否有自定义
						if (!string.IsNullOrEmpty(item.FormatString))
						{
							var yl = item.FormatString.Length;//yyyy 还是yy 还是y 
							if (yl <= 4)
								ye = ye.Substring(4 - yl, yl);
						}
						format += ye;
						break;
					//月
					case 4:
						var m = item.DiyDate[now.Month - 1];
						format += m;
						break;
					//日
					case 5:
						var d = item.DiyDate[now.Day - 1];
						format += d;
						break;
					//周别 （本年的第几周）
					case 6:
						var gc = new GregorianCalendar();
						var thisWeek = gc.GetWeekOfYear(now, CalendarWeekRule.FirstDay,DayOfWeek.Sunday);
						var y = item.DiyDate[thisWeek - 1];
						format += y;
						break;
					//周几  (按照周一是第一天算)
					case 7:
						var wk = now.DayOfWeek.ToInt();
						if (wk == 0)
							wk = 6;
						else
							wk -= 1;
						var w = item.DiyDate[wk];
						format += w;
						break;
					case 8:
						var h = now.ToString(item.FormatString);
						format += h;
						break;
					case 9:
						var ch = now.Hour;
						format += ch < 12? item.DiyDate[0] : item.DiyDate[1];
						break;
					//班别
					case 10:
						string csstr = "";
						foreach (var cs in item.DiyDate)
						{
							var strs = cs.Split(".");
							var times = strs[1].Split("-");
							if (int.Parse(times[0].Split(":")[0]) < int.Parse(times[1].Split(":")[0]))
							{
								//开始时间
								var startTime = DateTime.Parse($"{now:yyyy/MM/dd} {times[0]}");
								//结束时间
								var endTime = DateTime.Parse($"{now:yyyy/MM/dd} {times[1]}");
								if (DateTime.Now>=startTime && DateTime.Now < endTime)
								{
									csstr = strs[0];
									break;
								}
							}
							else
							{
								//开始时间
								var startTime = DateTime.Parse($"{now:yyyy/MM/dd} {times[0]}");
								//结束时间
								var endTime = DateTime.Parse($"{now:yyyy/MM/dd} {times[1]}").AddDays(1);
								if (DateTime.Now >= startTime && DateTime.Now < endTime)
								{
									csstr = strs[0];
									break;
								}
							}
						}
						format += csstr;
						break;
					//流水
					case 11:
						initVal = item.InitValue.Value;
						maxVal = item.MaxValue;
						flowNumberLength = item.FormatString.Length;
						toBase = item.ToBase;
						format += "^_^";
						increment = item.Increment.Value;
						break;
					//自定义方法
					case 12:
						//反射执行方法获取参数
						var udf =((Task<string>)this.GetType().GetMethod(item.FormatString, new Type[] { }).Invoke(this, new object[] { ruleName })).GetAwaiter().GetResult();
						format += udf;
						break;
					//通配符
					case 13:
						//如果作为检查流水号的长度的条件，则最后一个通配符是00,2位，那么流水号是4位，就会有问题  2 != 4 
						replaceChar = item.FormatString.FirstOrDefault();
						var tempstr = "".PadLeft(item.FormatString.Length, replaceChar);
						format += tempstr;
						break;
					default:
						break;
				}
			});

			//判断编码格式是否有效
			if (!string.IsNullOrEmpty(format))
			{
				for (int i = 0; i < count; i++)
				{
					var score = 0M;
					var scoreString = string.Empty;
					//预览
					if (!preview)
					{
						if (GlobalContext.SystemConfig.CacheProvider == Define.CACHEPROVIDER_REDIS)
						{
							score = await BaseHelper.ZIncrByAsync(GlobalContext.SystemConfig.ProjectPrefix + $"_BillNumber:{ruleName}", resetRule, increment);
						}
						else
						{
							var rulelog = await repository.Db.Queryable<CoderulelogEntity>().FirstAsync(a => a.F_Key == GlobalContext.SystemConfig.ProjectPrefix + $"_BillNumber:{ruleName}" && a.F_Value == resetRule);
							if (rulelog == null)
							{
								rulelog = new CoderulelogEntity();
								rulelog.F_Id = Utils.GetGuid();
								rulelog.F_Key = GlobalContext.SystemConfig.ProjectPrefix + $"_BillNumber:{ruleName}";
								rulelog.F_Value = resetRule;
								rulelog.F_Score = (int)increment;
								rulelog.F_RuleId = rule.F_Id;
								await repository.Db.Insertable(rulelog).ExecuteCommandAsync();
							}
							else
							{
								await repository.Db.Updateable<CoderulelogEntity>(a => new CoderulelogEntity
								{
									F_Score = a.F_Score + (int)increment
								}).Where(a => a.F_Id == rulelog.F_Id).ExecuteCommandAsync();
							}
						}
					}
					else
					{
						if (GlobalContext.SystemConfig.CacheProvider == Define.CACHEPROVIDER_REDIS)
						{
							score = await BaseHelper.ZScoreAsync(GlobalContext.SystemConfig.ProjectPrefix + $"_BillNumber:{ruleName}", resetRule)??0;
							score += increment;
						}
						else
						{
							var rulelog = await repository.Db.Queryable<CoderulelogEntity>().FirstAsync(a => a.F_Key == GlobalContext.SystemConfig.ProjectPrefix + $"_BillNumber:{ruleName}" && a.F_Value == resetRule);
							if (rulelog == null)
							{
								score += increment;
							}
							else
							{
								score = (rulelog.F_Score??0) + increment;
							}
						}
					}
					//10进制流水号
					var flowNumber = score + initVal;

					//判断流水号是否超过流水号设定的最大值
					if (maxVal > 0 && flowNumber > maxVal)
						throw new Exception($"Morethan the flownumber settinged max value:{maxVal} and now value:{flowNumber}");

					//默认10进制
					scoreString = flowNumber.ToString();

					if (toBase != 10)
						scoreString = scoreString.ToLong().ToBase(toBase);

					//^_^是代表可以任意位置流水号
					var flow = scoreString.PadLeft(flowNumberLength, '0');
					if (flow.Length != flowNumberLength)
					{
						throw new Exception($"bill encode ({ruleName})settinged error,FlowLength,current:[{flow.Length}],setting:[{flowNumberLength}]");
					}
					var v = format.Replace("^_^", flow);
					if (replaceList!=null&&replaceList.Count>0)
						foreach (var item in replaceList)
						{
							var temp = "".PadLeft(item.Length, replaceChar);
							v = v.ReplaceFrist(temp, item);
						}
					list.Add(v);
				}
			}

			return list;
		}
		public async Task<Dictionary<string,string>> GetPrintJson(string code,string templateId)
		{
			var template = await repository.Db.Queryable<TemplateEntity>().FirstAsync(a => a.F_Id == templateId);
			List<SugarParameter> list = new List<SugarParameter>();
			list.Add(new SugarParameter("rulecode", code));
			if (!string.IsNullOrEmpty(template.F_TemplateSqlParm))
			{
				var dic = template.F_TemplateSqlParm.ToObject<Dictionary<string, object>>();
				foreach (var item in dic)
				{
					list.Add(new SugarParameter(item.Key, item.Value));
				}
			}
			var printResult = string.IsNullOrEmpty(template.F_TemplateSql) ? null : await repository.Db.Ado.SqlQueryAsync<dynamic>(template.F_TemplateSql, list);
			if (printResult!=null && printResult.Count>0)
			{
				var printData = (printResult[0] as IDictionary<string, object>)?.ToDictionary(k => k.Key.ToLower(), v => v.Value?.ToString());
				printData.Add("rulecode", code);
				return printData;
			}
			else
			{
				var printData = new Dictionary<string, string>();
				printData.Add("rulecode", code);
				return printData;
			}
		}
		public async Task<List<PrintEntity>> CreateForm(string keyValue, int count = 1, bool needPrint = false)
		{
            var list = new List<PrintEntity>();
			var rule = await repository.Db.Queryable<CoderuleEntity>().FirstAsync(a => a.F_Id == keyValue);
			var template = await repository.Db.Queryable<TemplateEntity>().FirstAsync(a => a.F_Id == rule.F_TemplateId);
			var logs = new List<CodegeneratelogEntity>();
			var codes = await GetBillNumber(rule.F_RuleName, count);
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
					var log = new CodegeneratelogEntity();
					log.Create();
					log.F_Code = codes[i];
					log.F_PrintCount = needPrint ? 1 : 0;
					log.F_RuleId = rule.F_Id;
					log.F_RuleName = rule.F_RuleName;
					log.F_EnabledMark = true;
					log.F_DeleteMark = false;
					var printJson = await GetPrintJson(log.F_Code, template.F_Id);
					log.F_PrintJson = printJson.ToJson();
					logs.Add(log);
					listJson.Add(printJson);
				}
				entity.data.data = listJson;
				list.Add(entity);
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					var log = new CodegeneratelogEntity();
					log.Create();
					log.F_Code = codes[i];
					log.F_PrintCount = needPrint ? 1 : 0;
					log.F_RuleId = rule.F_Id;
					log.F_RuleName = rule.F_RuleName;
					log.F_EnabledMark = true;
					log.F_DeleteMark = false;
					var printJson = await GetPrintJson(log.F_Code, template.F_Id);
					log.F_PrintJson = printJson.ToJson();
					logs.Add(log);
					PrintEntity entity = new PrintEntity();
					entity.data = new PrintDetail();
					entity.data.printIniInfo = new PrintInitInfo();
					entity.data.printIniInfo.printType = template.F_PrintType;
					entity.data.printIniInfo.isBatch = template.F_Batch;
					entity.data.printIniInfo.realName = template.F_TemplateName;
					entity.data.printIniInfo.filePath = (GlobalContext.HttpContext.Request.IsHttps ? "https://" : "http://") + GlobalContext.HttpContext.Request.Host + template.F_TemplateFile;
					entity.requestId = Utils.GetGuid();
					entity.data.data = printJson;
					list.Add(entity);
				}
			}
			//新增log
			await repository.Db.Insertable(logs).ExecuteCommandAsync();
			return list;
		}
		#endregion

	}
}
