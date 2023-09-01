using Jaina;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
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
            using var dbContext = new SqlSugarClient(DBInitialize.GetConnectionConfigs(true),
			//全局上下文生效
			db =>
			{
				foreach (var item in DBInitialize.GetConnectionConfigs(false))
				{
					string temp = item.ConfigId;
					db.GetConnection(temp).DefaultConfig();
				}
			});
            await new LogService(dbContext).WriteDbLog(input, user);
            await Task.CompletedTask;
		}
	}
}
