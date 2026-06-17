using Jaina;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Code.Model;
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
            // NLogDbLogEnabled=false 时仅写NLog文件，不写数据库
            if (!GlobalContext.SystemConfig.NLogDbLogEnabled)
            {
                await Task.CompletedTask;
                return;
            }

            var todo = (BaseEventSource)context.Source;
            var input = (LogEntity)todo.Payload;
            var user = todo.User;
            using var dbContext = new SqlSugarClient(DBInitialize.GetConnectionConfigs(true),
            db =>
            {
                foreach (var item in DBInitialize.GetConnectionConfigs(false))
                {
                    db.GetConnection(item.ConfigId).DefaultConfig();
                }
            });
            await new LogService(dbContext).WriteDbLog(input, user);
            await Task.CompletedTask;
        }
    }
}
