using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Code;
using WaterCloud.Domain.FileManage;
using WaterCloud.Service;
using WaterCloud.Service.FileManage;
using WaterCloud.Service.SystemOrganize;
using System.Net.Http.Headers;
using System.IO;

namespace WaterCloud.Web.Areas.FileManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-22 12:04
    /// 描 述：文件管理控制器类
    /// </summary>
    [Area("FileManage")]
    public class UploadfileController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public UploadfileService _service {get;set;}
        public SystemSetService _setService { get; set; }

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
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _service.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [ServiceFilter(typeof(HandlerLoginAttribute))]
        public async Task<ActionResult> Upload(string fileby,int filetype =0)
        {
            try
            {
                string stemp = "local";
                if (_service.currentuser.CompanyId != GlobalContext.SystemConfig.SysemMasterProject)
                {
                    var temp = await _setService.GetForm(_service.currentuser.CompanyId);
                    if (temp != null)
                    {
                        stemp = temp.F_CompanyName;
                    }
                    else
                    {
                        throw new Exception("租户不存在");
                    }
                }
                var files = HttpContext.Request.Form.Files;
                long size = files.Sum(f => f.Length);
                if (size > 104857600)
                {
                    throw new Exception("大小必须小于100M");
                }
                var file = files.FirstOrDefault();
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var ispic = FileHelper.IsPicture(fileName);
                if (filetype==1&&!ispic)
                {
                    throw new Exception("请上传图片");
                }
                var isexcle = FileHelper.IsExcel(fileName);
                if (filetype == 2 && !isexcle)
                {
                    throw new Exception("请上传Excel");
                }
                if (ispic)
                {
                    filetype = 1;
                }
                if (isexcle)
                {
                    filetype = 2;
                }
                string fileValue = "";
                if (fileby=="公司logo")
                {
                    fileValue = "icon";
                }
                else
                {
                    fileValue = "file";
                }
                string filePath = "";
                fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + fileName.Substring(fileName.LastIndexOf("."));
                UploadfileEntity entity = new UploadfileEntity();
                if (!string.IsNullOrEmpty(stemp))
                {
                    entity.F_FilePath = stemp + $@"/" + DateTime.Now.ToString("yyyyMMdd") + $@"/" + fileName;
                    filePath = GlobalContext.HostingEnvironment.WebRootPath + $@"/" + fileValue + $@"/" + stemp + $@"/" + DateTime.Now.ToString("yyyyMMdd") + $@"/";
                }
                string fileFullName = filePath + fileName;
                entity.Create();
                entity.F_EnabledMark = true;
                entity.F_FileBy = fileby;
                entity.F_FileType = filetype;
                entity.F_CreatorUserName = _service.currentuser.UserName;
                entity.F_FileSize = size.ToIntOrNull();

                entity.F_FileName = fileName;
                entity.F_OrganizeId = _service.currentuser.DepartmentId.Split(',')[0];
                if (fileName.LastIndexOf(".") >= 0)
                {
                    entity.F_FileExtension = fileName.Substring(fileName.LastIndexOf("."));
                }
                if (!await SubmitForm(entity, ""))
                {
                    throw new Exception("数据库操作失败");
                }       
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                await _logService.WriteLog("操作成功。",className,"",DbLogType.Visit);
                return Content(new { code = 0, msg = "操作成功", data = new { src = entity.F_FilePath, title = fileName } }.ToJson());
            }
            catch (Exception ex)
            {
                await _logService.WriteLog(ex.Message, className, "", DbLogType.Visit,true);
                return Content(new { code = 400, msg = "操作失败," + ex.Message }.ToJson());
            }
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerLoginAttribute))]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public async Task<FileResult> Download(string keyValue)
        {
            var data = await _service.GetForm(keyValue);
            string fileValue = "";
            if (data.F_FileBy == "公司logo")
            {
                fileValue = "icon";
            }
            else
            {
                fileValue = "file";
            }
            string filePath = GlobalContext.HostingEnvironment.WebRootPath + $@"/" + fileValue + $@"/" + data.F_FilePath;
            if (!FileHelper.IsExistFile(filePath))
            {
                throw new Exception("文件不存在");
            }
            ///定义并实例化一个内存流，以存放图片的字节数组。
            MemoryStream ms = new MemoryStream();
            ///图片读入FileStream
            FileStream f = new FileStream(filePath, FileMode.Open);
            ///把FileStream写入MemoryStream
            ms.SetLength(f.Length);
            f.Read(ms.GetBuffer(), 0, (int)f.Length);
            ms.Flush();
            f.Close();
            string filename = DateTime.Now.ToString("yyyyMMdd_HHmmss") + data.F_FileExtension;
            var contentType = MimeMapping.GetMimeMapping(filename);
            return File(ms, contentType, filename);
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<bool> SubmitForm(UploadfileEntity entity, string keyValue)
        {
            try
            {
                await _service.DeleteForm(keyValue);
                await Success("操作成功。", className, keyValue);
                return true;
            }
            catch (Exception ex)
            {
                await Error(ex.Message, className, keyValue);
                return false;
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
