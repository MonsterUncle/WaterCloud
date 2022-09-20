using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace WaterCloud.Service.AutoJob
{
	/// <summary>
	/// 依赖注入必须，代替原本的SimpleJobFactory
	/// </summary>
	public class IOCJobFactory : IJobFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public IOCJobFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
		{
			var serviceScope = _serviceProvider.CreateScope(); // 获得一个ioc对象，指定创建scope级别的实例（在job里面需要依赖注入ef，但是startup里面配置的ef是scope级别的，必须指定为scope，不然报错）。不写的话，默认是单例。
			return serviceScope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob; // 依赖注入一个 job 然后返回
		}

		public void ReturnJob(IJob job)
		{
			var disposable = job as IDisposable;
			disposable?.Dispose();
		}
	}
}