using Jaina;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.InfoManage;
using WaterCloud.Service.WebSocket;

namespace WaterCloud.Service.Event
{
    // 实现 IEventSubscriber 接口
    public class MessageEventSubscriber : IEventSubscriber
    {
        private readonly WebSocketConnectionManager _wsManager;
        private string cacheHubKey = GlobalContext.SystemConfig.ProjectPrefix + "_hubuserinfo_";

        public MessageEventSubscriber(WebSocketConnectionManager wsManager)
        {
            _wsManager = wsManager;
        }

        [EventSubscribe("Message:send")]
        public async Task SendMessage(EventHandlerExecutingContext context)
        {
            var todo = (BaseEventSource)context.Source;
            var input = (MessageEntity)todo.Payload;
            if (!string.IsNullOrEmpty(input.companyId) && input.F_ToUserId.Length == 0)
            {
                await _wsManager.SendToCompanyAsync(input.companyId, input.ToJson());
            }
            else
            {
                var users = input.F_ToUserId.Split(',');
                foreach (var item in users)
                {
                    // 存在就私信
                    var connectionIDs = await CacheHelper.GetAsync<List<string>>(cacheHubKey + item);
                    if (connectionIDs == null)
                    {
                        continue;
                    }
                    foreach (var connectionID in connectionIDs)
                    {
                        try
                        {
                            await _wsManager.SendToConnectionAsync(connectionID, input.ToJson());
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
            await Task.CompletedTask;
        }
    }
}
