using System;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
	public class AsyncTaskHelper
	{
		/// <summary>
		/// 开始异步任务
		/// </summary>
		/// <param name="action"></param>
		public static void StartTask(Action action)
		{
			try
			{
				Action newAction = () =>
				{ };
				newAction += action;
				Task task = new Task(newAction);
				task.Start();
			}
			catch (Exception ex)
			{
				LogHelper.WriteWithTime(ex);
			}
		}
	}
}