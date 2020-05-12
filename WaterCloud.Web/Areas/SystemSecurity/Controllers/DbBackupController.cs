/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Code;
using WaterCloud.Domain.SystemSecurity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WaterCloud.Service;
using System;
using WaterCloud.Service.SystemManage;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
    [Area("SystemSecurity")]
    public class DbBackupController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly DbBackupService _dbBackupService;
        private readonly LogService _logService;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        public DbBackupController(LogService logService, DbBackupService dbBackupService, IHostingEnvironment hostingEnvironment)
        {
            _logService = logService;
            _dbBackupService = dbBackupService;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(string keyword)
        {
            var data =await _dbBackupService.GetList(keyword);
            return Success(data.Count,data);
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(DbBackupEntity dbBackupEntity)
        {
            var currentuser = OperatorProvider.Provider.GetCurrent();
            LogEntity logEntity = await _logService.CreateLog(moduleName, className, DbLogType.Create.ToString());
            logEntity.F_Description += DbLogType.Create.ToDescription();
            try
            {
                logEntity.F_Account = currentuser.UserCode;
                logEntity.F_NickName = currentuser.UserName;
                string webRootPath = _hostingEnvironment.WebRootPath;
                dbBackupEntity.F_FilePath = webRootPath+"/Resource/DbBackup/" + dbBackupEntity.F_FileName + ".bak";
                if (!Directory.Exists(webRootPath+"/Resource/DbBackup/"))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(webRootPath + "/Resource/DbBackup/");
                    directoryInfo.Create();
                }
                dbBackupEntity.F_FileName = dbBackupEntity.F_FileName + ".bak";
                await _dbBackupService.SubmitForm(dbBackupEntity);
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
            var currentuser = OperatorProvider.Provider.GetCurrent();
            LogEntity logEntity =await _logService.CreateLog(moduleName, className, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = currentuser.UserCode;
                logEntity.F_NickName = currentuser.UserName;
                await _dbBackupService.DeleteForm(keyValue);
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
        [HandlerAuthorize]
        public async Task DownloadBackup(string keyValue)
        {
            //需要重新定义
        }
    }
}
