using Jaina.EventBus;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.InfoManage;

namespace WaterCloud.Service.Event
{
	// 实现 IEventSubscriber 接口
	public class MessageEventSubscriber : IEventSubscriber
	{
		private readonly IHubContext<MessageHub> _messageHub;
		private string cacheHubKey = GlobalContext.SystemConfig.ProjectPrefix + "_hubuserinfo_";

		public MessageEventSubscriber(IHubContext<MessageHub> messageHub)
		{
			_messageHub = messageHub;
		}

		[EventSubscribe("Message:send")] // 支持多个
		public async Task SendMessage(EventHandlerExecutingContext context)
		{
			var todo = (BaseEventSource)context.Source;
			var input = (MessageEntity)todo.Payload;
			if (!string.IsNullOrEmpty(input.companyId) && input.F_ToUserId.Length == 0)
			{
				await _messageHub.Clients.Group(input.companyId).SendAsync("ReceiveMessage", input.ToJson());
			}
			else
			{
				var users = input.F_ToUserId.Split(',');
				foreach (var item in users)
				{
					//存在就私信
					var connectionIDs = await CacheHelper.GetAsync<List<string>>(cacheHubKey + item);
					if (connectionIDs == null)
					{
						continue;
					}
					foreach (var connectionID in connectionIDs)
					{
						try
						{
							await _messageHub.Clients.Client(connectionID).SendAsync("ReceiveMessage", input.ToJson());
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
