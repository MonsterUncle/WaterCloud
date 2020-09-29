using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Code;
using WaterCloud.Domain.ContentManage;
using WaterCloud.Service;
using WaterCloud.Service.ContentManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WaterCloud.Web.Areas.ContentManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻管理控制器类
    /// </summary>
    [Area("ContentManage")]
    [AllowAnonymous]
    public class ArticleNewsController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        //属性注入示例
        public ArticleNewsService _service { get; set; }
        [HttpGet]
        public override ActionResult Form()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue=HttpContext.Request.Query["keyValue"].ToString();
            ViewBag.UserName = _service.currentuser.UserName;
            ViewBag.Content = _service.GetForm(keyValue).Result.ToJson();
            return View();
        }
        [HttpGet]
        public override ActionResult Details()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            ViewBag.Content = _service.GetForm(keyValue).Result.ToJson();
            return View();
        }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword,string CategoryId)
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
            var data = await _service.GetLookList(pagination,keyword,CategoryId);
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
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _service.GetForm(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ArticleNewsEntity entity, string keyValue)
        {
            try
            {
                await _service.SubmitForm(entity, keyValue);
                return await Success("操作成功。", className, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, keyValue);
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
