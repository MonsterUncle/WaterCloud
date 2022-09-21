using Jaina.EventBus;
using System.Threading;
using System;
using WaterCloud.Code;

namespace WaterCloud.Service.Event
{
	public class BaseEventSource : IEventSource
	{
		public BaseEventSource()
		{
		}
		public BaseEventSource(string eventId, object payload, OperatorModel user = null)
		{
			EventId = eventId;
			Payload = payload;
			User = user;
		}
		// 自定义属性
		public string ToDoName { get; set; }
		/// <summary>
		/// 事件 Id
		/// </summary>
		public string EventId { get; set; }
		/// <summary>
		/// 事件承载（携带）数据
		/// </summary>
		public object Payload { get; set; }
		/// <summary>
		/// 事件创建时间
		/// </summary>
		public DateTime CreatedTime { get; set; } = DateTime.Now;
		/// <summary>
		/// 当前用户信息
		/// </summary>
		public OperatorModel User { get; set; }
		/// <summary>
		/// 取消任务 Token
		/// </summary>
		/// <remarks>用于取消本次消息处理</remarks>
		[Newtonsoft.Json.JsonIgnore]
		[System.Text.Json.Serialization.JsonIgnore]
		public CancellationToken CancellationToken { get; set; }
	}
}
