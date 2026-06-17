using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.InfoManage;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Service.InfoManage;
using WaterCloud.Service.SystemOrganize;
using WaterCloud.Service.WebSocket;

namespace WaterCloud.Web.Middleware
{
    public class WebSocketChatMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketChatMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws/chat")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await HandleWebSocketAsync(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleWebSocketAsync(HttpContext context, System.Net.WebSockets.WebSocket webSocket)
        {
            var manager = context.RequestServices.GetService(typeof(WebSocketConnectionManager)) as WebSocketConnectionManager;
            var userService = context.RequestServices.GetService(typeof(UserService)) as UserService;
            var msgService = context.RequestServices.GetService(typeof(MessageService)) as MessageService;

            string connectionId = Guid.NewGuid().ToString();
            string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_hubuserinfo_";
            string cacheKeyOperator = GlobalContext.SystemConfig.ProjectPrefix + "_operator_";

            manager.AddConnection(connectionId, webSocket);

            try
            {
                var buffer = new byte[1024 * 4];
                var messageBuffer = new StringBuilder();

                while (webSocket.State == WebSocketState.Open)
                {
                    messageBuffer.Clear();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                            break;
                        }
                        messageBuffer.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                    }
                    while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    var message = messageBuffer.ToString();
                    if (message == "ping")
                    {
                        var pongBytes = Encoding.UTF8.GetBytes("pong");
                        await webSocket.SendAsync(new ArraySegment<byte>(pongBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                        continue;
                    }

                    await ProcessMessage(message, webSocket, manager, userService, msgService,
                        connectionId, cacheKey, cacheKeyOperator);
                }
            }
            finally
            {
                await manager.RemoveConnection(connectionId);
                try { webSocket.Dispose(); } catch { }
            }
        }

        private async Task ProcessMessage(string message, System.Net.WebSockets.WebSocket webSocket,
            WebSocketConnectionManager manager, UserService userService, MessageService msgService,
            string connectionId, string cacheKey, string cacheKeyOperator)
        {
            try
            {
                var json = JObject.Parse(message);
                var type = json["type"]?.ToString();

                switch (type)
                {
                    case "Login":
                        await HandleLogin(json, manager, userService, connectionId, cacheKey, cacheKeyOperator);
                        break;

                    case "SendMessage":
                        await HandleSendMessage(json, userService, msgService);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
            }
        }

        private async Task HandleLogin(JObject json, WebSocketConnectionManager manager,
            UserService userService, string connectionId, string cacheKey, string cacheKeyOperator)
        {
            var token = json["token"]?.ToString() ?? "";
            var user = userService.currentuser;

            if (user == null || user.UserId == null)
            {
                user = await CacheHelper.GetAsync<OperatorModel>(cacheKeyOperator + token);
            }

            if (user != null && user.CompanyId != null)
            {
                await manager.AddToGroup(connectionId, user.UserId, user.CompanyId);
            }
        }

        private async Task HandleSendMessage(JObject json, UserService userService, MessageService msgService)
        {
            var reUserId = json["toUserId"]?.ToString();
            var messageText = json["message"]?.ToString();

            if (string.IsNullOrEmpty(reUserId))
                return;

            var msg = new MessageEntity
            {
                F_EnabledMark = true,
                F_MessageType = 1,
                F_CreatorUserName = userService.currentuser?.UserName,
                F_MessageInfo = messageText,
                F_ToUserId = reUserId,
                F_ClickRead = true
            };

            await msgService.SubmitForm(msg);
        }
    }
}
