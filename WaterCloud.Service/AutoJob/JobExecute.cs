using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.AutoJob
{
	public class JobExecute : IJob
	{
		private IScheduler _scheduler;
		private ISchedulerFactory _schedulerFactory;
		private IJobFactory _iocJobfactory;
		private readonly IHttpClientFactory _httpClient;

		public JobExecute(ISchedulerFactory schedulerFactory, IJobFactory iocJobfactory, IHttpClientFactory httpClient)
		{
			_scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
			_scheduler.JobFactory = iocJobfactory;
			_schedulerFactory = schedulerFactory;
			_iocJobfactory = iocJobfactory;
			_httpClient = httpClient;
		}

		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(async () =>
			{
				string jobId = "";
				JobDataMap jobData = null;
				OpenJobEntity dbJobEntity = null;
				DateTime now = DateTime.Now;
				var dbContext = GlobalContext.RootServices.GetService<ISqlSugarClient>();
				var repository = new RepositoryBase<LogEntity>(dbContext);
				try
				{
					jobData = context.JobDetail.JobDataMap;
					jobId = jobData.GetString("F_Id");
					OpenJobsService autoJobService = new OpenJobsService(dbContext, _schedulerFactory, _iocJobfactory, _httpClient);
					// 获取数据库中的任务
					dbJobEntity = await autoJobService.GetForm(jobId);
					if (dbJobEntity != null)
					{
						if (dbJobEntity.F_EnabledMark == true)
						{
							CronTriggerImpl trigger = context.Trigger as CronTriggerImpl;
							if (trigger != null)
							{
								if (trigger.CronExpressionString != dbJobEntity.F_CronExpress)
								{
									// 更新任务周期
									trigger.CronExpressionString = dbJobEntity.F_CronExpress;
									await _scheduler.RescheduleJob(trigger.Key, trigger);
									return;
								}

								#region 执行任务

								OpenJobLogEntity log = new OpenJobLogEntity();
								log.F_Id = Utils.GuId();
								log.F_JobId = jobId;
								log.F_JobName = dbJobEntity.F_JobName;
								log.F_CreatorTime = now;
								AlwaysResult result = new AlwaysResult();
								if (dbJobEntity.F_JobType == 0)
								{
									//反射执行就行
									var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
									//反射取指定前后缀的dll
									var referencedAssemblies = Directory.GetFiles(path, "WaterCloud.*.dll").Select(Assembly.LoadFrom).ToArray();
									var types = referencedAssemblies
										.SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
										.Contains(typeof(IJobTask)))).ToArray();
									string filename = dbJobEntity.F_FileName;
									var implementType = types.Where(x => x.IsClass && x.FullName == filename).FirstOrDefault();
									repository.ChangeEntityDb(dbJobEntity.F_DbNumber);
									var obj = System.Activator.CreateInstance(implementType, dbContext);       // 创建实例(带参数)
									MethodInfo method = implementType.GetMethod("Start", new Type[] { });      // 获取方法信息
									object[] parameters = null;
									result = ((Task<AlwaysResult>)method.Invoke(obj, parameters)).GetAwaiter().GetResult();     // 调用方法，参数为空
									if (result.state.ToString() == ResultType.success.ToString())
									{
										log.F_EnabledMark = true;
										log.F_Description = "执行成功，" + result.message.ToString();
									}
									else
									{
										log.F_EnabledMark = false;
										log.F_Description = "执行失败，" + result.message.ToString();
									}
								}
								else if (dbJobEntity.F_JobType == 5)
								{
									try
									{
										repository.ChangeEntityDb(dbJobEntity.F_JobDBProvider);
										repository.Db.Ado.BeginTran();
										if (!string.IsNullOrEmpty(dbJobEntity.F_JobSqlParm))
										{
											var dic = dbJobEntity.F_JobSqlParm.ToObject<Dictionary<string, object>>();
											List<SugarParameter> list = new List<SugarParameter>();
											foreach (var item in dic)
											{
												list.Add(new SugarParameter(item.Key, item.Value));
											}
											var dbResult = await repository.Db.Ado.SqlQueryAsync<dynamic>(dbJobEntity.F_JobSql, list);
											log.F_EnabledMark = true;
											log.F_Description = "执行成功，" + dbResult.ToJson();
										}
										else
										{
											var dbResult = await repository.Db.Ado.SqlQueryAsync<dynamic>(dbJobEntity.F_JobSql);
											log.F_EnabledMark = true;
											log.F_Description = "执行成功，" + dbResult.ToJson();
										}
										repository.Db.Ado.CommitTran();
									}
									catch (Exception ex)
									{
										log.F_EnabledMark = false;
										log.F_Description = "执行失败，" + ex.Message;
										repository.Db.Ado.RollbackTran();
									}
								}
								else
								{
									HttpMethod method = HttpMethod.Get;
									switch (dbJobEntity.F_JobType)
									{
										case 1:
											method = HttpMethod.Get;
											break;

										case 2:
											method = HttpMethod.Post;
											break;

										case 3:
											method = HttpMethod.Put;
											break;

										case 4:
											method = HttpMethod.Delete;
											break;
									}
									var dic = dbJobEntity.F_RequestHeaders.ToObject<Dictionary<string, string>>();
									if (dic == null)
									{
										dic = new Dictionary<string, string>();
									}
									//请求头添加租户号
									dic.Add("dbNumber", dbJobEntity.F_DbNumber);
									try
									{
										var temp = await new HttpWebClient(_httpClient).ExecuteAsync(dbJobEntity.F_RequestUrl, method, dbJobEntity.F_RequestString, dic);
										log.F_EnabledMark = true;
										log.F_Description = $"执行成功。{temp}";
									}
									catch (Exception ex)
									{
										log.F_EnabledMark = false;
										log.F_Description = "执行失败，" + ex.Message.ToString();
									}
								}

								#endregion 执行任务

								repository.ChangeEntityDb(GlobalContext.SystemConfig.MainDbNumber);
								repository.Db.Ado.BeginTran();
								if (log.F_EnabledMark == true)
								{
									await repository.Db.Updateable<OpenJobEntity>(a => new OpenJobEntity
									{
										F_LastRunMark = true,
										F_LastRunTime = now,
									}).Where(a => a.F_Id == jobId).ExecuteCommandAsync();
								}
								else
								{
									await repository.Db.Updateable<OpenJobEntity>(a => new OpenJobEntity
									{
										F_LastRunMark = false,
										F_LastRunTime = now,
										F_LastRunErrTime = now,
										F_LastRunErrMsg = log.F_Description
									}).Where(a => a.F_Id == jobId).ExecuteCommandAsync();
								}
								if (dbJobEntity.F_IsLog == "是")
								{
									repository.Db.Insertable(log).ExecuteCommand();
								}
								repository.Db.Ado.CommitTran();
							}
						}
					}
				}
				catch (Exception ex)
				{
					repository.Db.Ado.RollbackTran();
					LogHelper.WriteWithTime(ex);
				}
			});
		}
	}
}