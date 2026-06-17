using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WaterCloud.Code;

namespace WaterCloud.Service.WebSocket
{
    public class WebSocketConnectionManager : IDisposable
    {
        private string cacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_hubuserinfo_";

        private readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _connections =
            new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();

        public void AddConnection(string connectionId, System.Net.WebSockets.WebSocket socket)
        {
            _connections.TryAdd(connectionId, socket);
        }

        public async Task RemoveConnection(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);

            var userId = await CacheHelper.GetAsync<string>(cacheKey + connectionId);
            if (!string.IsNullOrEmpty(userId))
            {
                var userConnections = await CacheHelper.GetAsync<List<string>>(cacheKey + userId);
                if (userConnections != null)
                {
                    userConnections.Remove(connectionId);
                    if (userConnections.Count == 0)
                        await CacheHelper.RemoveAsync(cacheKey + userId);
                    else
                        await CacheHelper.SetAsync(cacheKey + userId, userConnections);
                }
                await CacheHelper.RemoveAsync(cacheKey + connectionId);
            }
        }

        public async Task AddToGroup(string connectionId, string userId, string companyId)
        {
            var userConnections = await CacheHelper.GetAsync<List<string>>(cacheKey + userId) ?? new List<string>();
            var onlineList = await CacheHelper.GetAsync<List<string>>(cacheKey + "list_" + companyId) ?? new List<string>();

            userConnections.Add(connectionId);
            onlineList.Add(connectionId);

            await CacheHelper.SetAsync(cacheKey + connectionId, userId);
            await CacheHelper.SetAsync(cacheKey + userId, userConnections);
            await CacheHelper.SetAsync(cacheKey + "list_" + companyId, onlineList);
        }

        public async Task SendToUserAsync(string userId, string message)
        {
            var connectionIds = await CacheHelper.GetAsync<List<string>>(cacheKey + userId);
            if (connectionIds == null) return;

            var bytes = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(bytes);

            foreach (var id in connectionIds)
            {
                if (_connections.TryGetValue(id, out var socket) && socket.State == WebSocketState.Open)
                {
                    try
                    {
                        await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch { }
                }
            }
        }

        public async Task SendToCompanyAsync(string companyId, string message)
        {
            var onlineList = await CacheHelper.GetAsync<List<string>>(cacheKey + "list_" + companyId);
            if (onlineList == null) return;

            var bytes = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(bytes);

            foreach (var id in onlineList)
            {
                if (_connections.TryGetValue(id, out var socket) && socket.State == WebSocketState.Open)
                {
                    try
                    {
                        await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch { }
                }
            }
        }

        public async Task SendToConnectionAsync(string connectionId, string message)
        {
            if (_connections.TryGetValue(connectionId, out var socket) && socket.State == WebSocketState.Open)
            {
                try
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch { }
            }
        }

        public void Dispose()
        {
            foreach (var kv in _connections)
            {
                try { kv.Value.Dispose(); } catch { }
            }
            _connections.Clear();
        }
    }
}
