using WaterCloud.Service.SystemSecurity;
using WaterCloud.Code;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service;
using System.Linq;
using Quartz;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
    /// <summary>
    /// 定时任务
    /// </summary>
    [Area("SystemSecurity")]
    public class OpenJobsController : ControllerBase
    {
        public OpenJobsService _service { get; set; }

        //获取详情
        [HttpGet]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _service.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitForm(OpenJobEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.EnabledMark = false;
                entity.DeleteMark = false;
            }
            else
            {
                entity.EnabledMark = null;
            }
            try
            {
                await _service.SubmitForm(entity, keyValue);
                return await Success("操作成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue);
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

        /// <summary>
        /// 获取本地可执行的任务列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> QueryLocalHandlers()
        {
            return await Task.Run(() => {
                var data = _service.QueryLocalHandlers();
                return Content(data.ToJson());
            });
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "desc";
            pagination.field = "EnabledMark";
            //导出全部页使用
            if (pagination.rows == 0 && pagination.page == 0)
            {
                pagination.rows = 99999999;
                pagination.page = 1;
            }
            var data = await _service.GetLookList(pagination, keyword);
            foreach (var item in data)
            {
                if (item.EnabledMark == true)
                {
                    CronExpression cronExpression = new CronExpression(item.CronExpress);
                    item.NextValidTimeAfter = cronExpression.GetNextValidTimeAfter(DateTime.Now).Value.ToLocalTime().DateTime;
                }
                else
                {
                    item.NextValidTimeAfter = null;
                }
            }
            return Success(pagination.records, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetLogJson(string keyValue, string keyword)
        {
            var data = await _service.GetLogList(keyValue);
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.Description.Contains(keyword)).ToList();
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetDBListJson()
        {
            var data = DBInitialize.GetConnectionConfigs();
            return Content(data.Select(a => a.ConfigId).ToJson());
        }
        /// <summary>
        /// 改变任务状态，启动/停止
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(string keyValue, int status)
        {
            try
            {
                await _service.ChangeJobStatus(keyValue, status);
                return await Success("操作成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue);
            }
        }
        /// <summary>
        /// 立即执行任务
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> DoNow(string keyValue)
        {
            try
            {
                await _service.DoNow(keyValue);
                var data = await _service.GetForm(keyValue);
                return await Success("操作成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue);
            }
        }
        /// <summary>
        /// 清除任务计划
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ClearScheduleJob()
        {
            try
            {
                await _service.ClearScheduleJob();
                return await Success("操作成功。", "", "");
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", "");
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> DeleteLogForm(string keyValue)
        {
            try
            {
                await _service.DeleteLogForm(keyValue);
                return await Success("操作成功。", "", keyValue, DbLogType.Delete);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue, DbLogType.Delete);
            }
        }
    }
}
