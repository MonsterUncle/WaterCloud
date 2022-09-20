using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.InfoManage;
using WaterCloud.Service.InfoManage;

namespace WaterCloud.Web.Areas.InfoManage.Controllers
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-07-29 16:41
	/// 描 述：通知管理控制器类
	/// </summary>
	[Area("InfoManage")]
	public class MessageController : BaseController
	{
		public MessageService _service { get; set; }

		[HttpGet]
		[HandlerAuthorize]
		public ActionResult OwnerMessage()
		{
			return View();
		}

		#region 获取数据

		[HandlerAjaxOnly]
		[IgnoreAntiforgeryToken]
		public async Task<ActionResult> GetGridJson(SoulPage<MessageEntity> pagination, string keyword)
		{
			if (string.IsNullOrEmpty(pagination.field))
			{
				pagination.field = "F_Id";
				pagination.order = "desc";
			}
			var data = await _service.GetLookList(pagination, keyword);
			return Content(pagination.setData(data).ToJson());
		}

		[HttpGet]
		[HandlerAjaxOnly]
		public async Task<ActionResult> GetListJson(string keyword)
		{
			var data = await _service.GetList(keyword);
			return Content(data.ToJson());
		}

		[HttpGet]
		[HandlerAjaxOnly]
		public async Task<ActionResult> GetUnReadListJson()
		{
			var data = await _service.GetUnReadListJson();
			return Content(data.ToJson());
		}

		[HttpGet]
		[HandlerAjaxOnly]
		public async Task<ActionResult> GetFormJson(string keyValue)
		{
			var data = await _service.GetLookForm(keyValue);
			return Content(data.ToJson());
		}

		#endregion 获取数据

		#region 提交数据

		[HttpPost]
		[HandlerAjaxOnly]
		public async Task<ActionResult> SubmitForm(MessageEntity entity)
		{
			try
			{
				entity.F_EnabledMark = true;
				entity.F_ClickRead = true;
				entity.F_CreatorUserName = _service.currentuser.UserName;
				await _service.SubmitForm(entity);
				return await Success("操作成功。", "", "");
			}
			catch (Exception ex)
			{
				return await Error(ex.Message, "", "");
			}
		}

		[HttpPost]
		[HandlerAjaxOnly]
		[IgnoreAntiforgeryToken]
		public async Task<ActionResult> ReadMsgForm(string keyValue)
		{
			if (await _service.CheckMsg(keyValue))
			{
				return Success("信息已读");
			}
			try
			{
				await _service.ReadMsgForm(keyValue);
				return await Success("操作成功。", "", keyValue);
			}
			catch (Exception ex)
			{
				return await Error(ex.Message, "", keyValue);
			}
		}

		[HttpPost]
		[HandlerAjaxOnly]
		[IgnoreAntiforgeryToken]
		public async Task<ActionResult> ReadAllMsgForm(int type = 0)
		{
			try
			{
				await _service.ReadAllMsgForm(type);
				return await Success("操作成功。", "", "");
			}
			catch (Exception ex)
			{
				return await Error(ex.Message, "", "");
			}
		}

		[HttpPost]
		[HandlerAjaxOnly]
		[HandlerAuthorize]
		public async Task<ActionResult> DeleteForm(string keyValue)
		{
			try
			{
				await _service.DeleteForm(keyValue);
				return await Success("操作成功。", "", keyValue, DbLogType.Delete);
			}
			catch (Exception ex)
			{
				return await Error(ex.Message, "", keyValue, DbLogType.Delete);
			}
		}

		#endregion 提交数据
	}
}