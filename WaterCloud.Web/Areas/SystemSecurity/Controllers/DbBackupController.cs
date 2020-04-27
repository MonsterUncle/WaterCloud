/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Code;
using WaterCloud.Entity.SystemSecurity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WaterCloud.Service;
using System;
using WaterCloud.Service.SystemManage;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace WaterCloud.Web.Areas.SystemSecurity.Controllers
{
    [Area("SystemSecurity")]
    public class DbBackupController : ControllerBase
    {
        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly DbBackupService _dbBackupService;
        private readonly ModuleService _moduleService;
        private readonly LogService _logService;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        public DbBackupController(ModuleService moduleService, LogService logService, DbBackupService dbBackupService, IHostingEnvironment hostingEnvironment)
        {
            _moduleService = moduleService;
            _logService = logService;
            _dbBackupService = dbBackupService;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _dbBackupService.GetList(keyword);
            return ResultLayUiTable(data.Count,data);
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DbBackupEntity dbBackupEntity)
        {
            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem = _moduleService.GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Create.ToString());
            logEntity.F_Description += DbLogType.Create.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                string webRootPath = _hostingEnvironment.WebRootPath;
                dbBackupEntity.F_FilePath = webRootPath+"/Resource/DbBackup/" + dbBackupEntity.F_FileName + ".bak";
                if (!Directory.Exists(webRootPath+"/Resource/DbBackup/"))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(webRootPath + "/Resource/DbBackup/");
                    directoryInfo.Create();
                }
                dbBackupEntity.F_FileName = dbBackupEntity.F_FileName + ".bak";
                _dbBackupService.SubmitForm(dbBackupEntity);
                logEntity.F_Description += "操作成功";
                _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }

        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();
            var moduleitem= _moduleService.GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();
            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Delete.ToString());
            logEntity.F_Description += DbLogType.Delete.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                _dbBackupService.DeleteForm(keyValue);
                logEntity.F_Description += "操作成功";
                _logService.WriteDbLog(logEntity);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description += "操作失败，" + ex.Message;
                _logService.WriteDbLog(logEntity);
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            //需要重新定义
        }
    }
}
