using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.InfoManage;
using WaterCloud.Service.InfoManage;
using WaterCloud.Service.SystemOrganize;

namespace WaterCloud.Service
{
    public class MessageHub : Hub
    {
        private string cacheKey = "watercloud_hubuserinfo_";
        private readonly UserService _service;
        private readonly MessageService _msgService;
        public MessageHub(UserService service,MessageService msgService)
        {
            _service = service;
            _msgService = msgService;
        }
        public override async Task OnConnectedAsync()
        {
            var user = _service.currentuser;
            //一个公司一个分组
            await Groups.AddToGroupAsync(Context.ConnectionId, user.CompanyId);
            //将用户信息存进缓存
            await CacheHelper.Set(cacheKey + user.UserId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="reUserId">收消息的人员Id</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public async Task SendMessage(string reUserId, string message)
        {
            if (string.IsNullOrEmpty(reUserId))
            {
                return;
            }
            else
            {
                MessageEntity msg = new MessageEntity();
                msg.F_EnabledMark = true;
                msg.F_MessageType = 1;
                msg.F_CreatorUserName = _service.currentuser.UserName;
                msg.F_MessageInfo = message;
                msg.F_ToUserId = reUserId;
                msg.F_ClickRead = true;
                await _msgService.SubmitForm(msg);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
            }
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //删除缓存连接
            var user = _service.currentuser;
            if (user!=null)
            {
                await CacheHelper.Remove(cacheKey + user.UserId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
