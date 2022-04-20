using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.DataBase;
using WaterCloud.Domain.InfoManage;

namespace WaterCloud.Service.BackgroundService
{
	public class MessageService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private string cacheHubKey = GlobalContext.SystemConfig.ProjectPrefix + "_hubuserinfo_";
        private readonly RabbitMqHelper _rabbitMqHelper;
        private readonly IHubContext<MessageHub> _messageHub;

        public MessageService(RabbitMqHelper rabbitMqHelper, IHubContext<MessageHub> messageHub)
		{
			_rabbitMqHelper = rabbitMqHelper;
			_messageHub = messageHub;
		}
        /// <summary>
        /// 开始执行任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            LogHelper.WriteWithTime("开始执行...");
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// 停止执行任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            //释放RabbitMq
            _rabbitMqHelper.Dispose();

            LogHelper.WriteWithTime("停止执行...");

            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// 处理任务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            async Task handler(string msg, int retrycount, Exception ex)
            {
                LogHelper.WriteWithTime($"重试次数 : {retrycount}，异常 : {ex?.ToString()}");
                await Task.CompletedTask;
            }
            //订阅信息推送
            _rabbitMqHelper.Subscribe<MessageEntity>(
                async (input, y) =>
                {
                    if (!string.IsNullOrEmpty(input.companyId) && input.ToUserId.Length == 0)
                    {
                        await _messageHub.Clients.Group(input.companyId).SendAsync("ReceiveMessage", input.ToJson());
                    }
                    else
                    {
                        var users = input.ToUserId.Split(',');
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
                    return true;
                },
                handler);
            await Task.CompletedTask;
        }
    }
}
