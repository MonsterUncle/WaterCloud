using Jaina.EventBus;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Service.Event
{
	// 实现 IEventSubscriber 接口
	public class LogEventSubscriber : IEventSubscriber
	{

		public LogEventSubscriber()
		{
		}

		[EventSubscribe("Log:create")]
		public async Task SendMessages(EventHandlerExecutingContext context)
		{
			var todo = (BaseEventSource)context.Source;
			var input = (LogEntity)todo.Payload;
			var user = todo.User;
			var serviceProvider= GlobalContext.RootServices.CreateScope();
			var logService= serviceProvider.ServiceProvider.GetService<LogService>();
			await logService.WriteDbLog(input,user);
			await Task.CompletedTask;
		}
	}
}
