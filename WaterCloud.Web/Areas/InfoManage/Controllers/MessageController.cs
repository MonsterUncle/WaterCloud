using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Code;
using WaterCloud.Domain.InfoManage;
using WaterCloud.Service;
using WaterCloud.Service.InfoManage;

namespace WaterCloud.Web.Areas.InfoManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-29 16:41
    /// 描 述：通知管理控制器类
    /// </summary>
    [Area("InfoManage")]
    public class MessageController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public MessageService _service {get;set;}
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public ActionResult OwnerMessage()
        {
            return View();
        }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            //此处需修改
            pagination.order = "desc";
            pagination.sort = "F_CreatorTime desc";
            //导出全部页使用
            if (pagination.rows == 0 && pagination.page == 0)
            {
                pagination.rows = 99999999;
                pagination.page = 1;
            }
            var data = await _service.GetLookList(pagination,keyword);
            return Success(pagination.records, data);
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
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(MessageEntity entity)
        {
            try
            {
                entity.F_EnabledMark = true;
                entity.F_ClickRead = true;
                entity.F_CreatorUserName = _service.currentuser.UserName;
                await _service.SubmitForm(entity);
                return await Success("操作成功。", className,"");
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, "");
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> ReadMsgForm(string keyValue)
        {
            if (await _service.CheckMsg(keyValue))
            {
                return Success("信息已读");
            }
            try
            {
                await _service.ReadMsgForm(keyValue);
                return await Success("操作成功。", className, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, keyValue);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> ReadAllMsgForm(int type=0)
        {
            try
            {
                await _service.ReadAllMsgForm(type);
                return await Success("操作成功。", className, "");
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, "");
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            try
            {
                await _service.DeleteForm(keyValue);
                return await Success("操作成功。", className, keyValue, DbLogType.Delete);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, keyValue, DbLogType.Delete);
            }
        }
        #endregion
    }
}
