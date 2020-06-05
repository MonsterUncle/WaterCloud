using WaterCloud.Service.SystemSecurity;
using WaterCloud.Code;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Web;
using WaterCloud.Service;
using Senparc.CO2NET.Extensions;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
    /// <summary>
    /// 定时任务
    /// </summary>
    [Area("SystemSecurity")]
    public class OpenJobsController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly OpenJobService _jobService;
        private readonly LogService _logService;

        public OpenJobsController(OpenJobService jobService, LogService logService)
        {
            _jobService = jobService;
            _logService = logService;
        }
        //获取详情
        [HttpGet]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _jobService.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(OpenJobEntity entity, string keyValue)
        {
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_EnabledMark = false;
                entity.F_DeleteMark = false;
                logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
            }
            else
            {
                logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
                logEntity.F_Description += DbLogType.Update.ToDescription();
                logEntity.F_KeyValue = keyValue;
            }
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _jobService.SubmitForm(entity, keyValue);
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _jobService.DeleteForm(keyValue);
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取本地可执行的任务列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> QueryLocalHandlers()
        {
            var data = _jobService.QueryLocalHandlers();
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "desc";
            pagination.sort = "F_EnabledMark";
            var data = await _jobService.GetLookList(pagination, keyword);
            return Success(pagination.records, data);
        }
        /// <summary>
        /// 改变任务状态，启动/停止
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(string keyValue, int status)
        {
            LogEntity logEntity;
            logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            logEntity.F_KeyValue = keyValue;
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _jobService.ChangeJobStatus(keyValue, status);
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
    }
}
