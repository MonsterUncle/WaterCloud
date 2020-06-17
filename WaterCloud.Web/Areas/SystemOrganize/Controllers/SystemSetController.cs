using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Serenity;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Service;
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Service.SystemOrganize;
using System.IO;
using System.Net.Http.Headers;

namespace WaterCloud.Web.Areas.SystemOrganize.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-12 13:50
    /// 描 述：系统设置控制器类
    /// </summary>
    [Area("SystemOrganize")]
    public class SystemSetController :  ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly LogService _logService;
        private readonly SystemSetService _service;
        public SystemSetController(SystemSetService service, LogService logService)
        {
            _logService = logService;
            _service = service;
        }

        #region 获取数据
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult SetForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            //此处需修改
            pagination.order = "desc";
            pagination.sort = "F_CreatorTime";
            var data = await _service.GetLookList(pagination,keyword);
            return Success(pagination.records, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyword)
        {
            var data = await _service.GetList(keyword);
            var currentuser = OperatorProvider.Provider.GetCurrent();
            if (currentuser == null)
            {
                return null;
            }
            else
            {
                return Content(data.Where(a => a.F_Id == OperatorProvider.Provider.GetCurrent().CompanyId).ToJson());
            }
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _service.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSetFormJson()
        {
            var data = await _service.GetForm(OperatorProvider.Provider.GetCurrent().CompanyId);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(SystemSetEntity entity, string keyValue)
        {
            LogEntity logEntity;
            if (string.IsNullOrEmpty(keyValue))
            {
                logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Create.ToString());
                logEntity.F_Description += DbLogType.Create.ToDescription();
                entity.F_DeleteMark = false;
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
                await _service.SubmitForm(entity, keyValue);
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
        public async Task<ActionResult> PutPic( )
        {
            LogEntity logEntity;
            logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Visit.ToString());
            logEntity.F_Description += DbLogType.Create.ToDescription();
            try
            {
                var currentuser = OperatorProvider.Provider.GetCurrent();
                if (currentuser == null)
                {
                    throw new Exception("登录过期");
                }
                string stemp = "";
                if (currentuser.CompanyId != Define.SYSTEM_MASTERPROJECT)
                {
                    stemp = (await _service.GetForm(currentuser.CompanyId)).F_CompanyName;
                }
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                var files = HttpContext.Request.Form.Files;
                long size = files.Sum(f => f.Length);
                if (size > 104857600)
                {
                    throw new Exception("图片必须小于100M");
                }
                string filePathResult = "";
                var file = files.FirstOrDefault();
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                if (!FileHelper.IsPicture(fileName))
                {
                    throw new Exception("请上传图片");
                }
                string filePath = GlobalContext.HostingEnvironment.WebRootPath + $@"\icon\"+stemp+ $@"\";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + fileName.Split('.')[1];
                string fileFullName = filePath + fileName;
                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                if (string.IsNullOrEmpty(stemp))
                {
                    return Content(new { code = 0, msg = "操作成功", data = fileName }.ToJson());
                }
                else
                {
                    return Content(new { code = 0, msg = "操作成功", data = $@"\" + stemp + $@"\" + fileName }.ToJson());
                }

            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Content(new{ code = 400, msg = "操作失败," + ex.Message }.ToJson());
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetSubmitForm(SystemSetEntity entity)
        {
            LogEntity logEntity;
            var keyValue = OperatorProvider.Provider.GetCurrent().CompanyId;
            logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Update.ToString());
            logEntity.F_Description += DbLogType.Update.ToDescription();
            entity.F_DeleteMark = false;
            try
            {
                entity.F_EnabledMark = null;
                entity.F_EndTime = null;
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                await _service.SubmitForm(entity, keyValue);
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
                await _service.DeleteForm(keyValue);
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
        #endregion
    }
}
