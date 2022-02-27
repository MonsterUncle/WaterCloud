using System.Collections.Generic;
using System.Web;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Domain;
using WaterCloud.Code;
using WaterCloud.CodeGenerator;
using WaterCloud.Service.CommonService;
using System.Linq;
using WaterCloud.Service;
using System.Threading.Tasks;
using WaterCloud.Code.Extend;
using WaterCloud.DataBase;
using SqlSugar;
using System;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
	[Area("SystemManage")]
    public class CodeGeneratorController : ControllerBase
    {

        public DatabaseTableService _service { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public CodeGeneratorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region 视图功能

        #endregion

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTablePageListJson(Pagination pagination, string keyword)
        {
            //导出全部页使用
            if (pagination.rows == 0 && pagination.page == 0)
            {
                pagination.rows = 99999999;
                pagination.page = 1;
            }
            List<DbTableInfo> data = _service.GetTablePageList(keyword, pagination);
            return Success(pagination.records, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableJson(string keyword)
        {
            List<DbTableInfo> data = _service.GetTableList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableSelectJson(string keyword)
        {
            List<DbTableInfo> data = _service.GetTableList(keyword);
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { name = item.Name, value = item.Name });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableFieldJson(string keyValue, string keyword)
        {
            List<DbColumnInfo> data = _service.GetTableFieldList(keyValue);
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.DbColumnName.Contains(keyword)).ToList();
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableFieldSelectJson(string keyValue)
        {
            List<DbColumnInfo> data = _service.GetTableFieldList(keyValue);
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { text = item.DbColumnName, id = item.DbColumnName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetBaseConfigJson(string keyValue)
        {
            BaseConfigModel data = new BaseConfigModel();

            string tableDescription = string.Empty;
            List<DbColumnInfo> tDataTableField = _service.GetTableFieldList(keyValue);

            var columnList = tDataTableField.Where(p => !BaseField.BaseFieldList.Contains(p.DbColumnName) && !p.IsPrimarykey).Select(p => new { p.DbColumnName, p.ColumnDescription }).ToList();
            List<ColumnField> dic = new List<ColumnField>();
            foreach (var item in columnList)
            {
                ColumnField field = new ColumnField();
                field.field = item.DbColumnName;
                field.title = string.IsNullOrEmpty(item.ColumnDescription) ? item.DbColumnName : item.ColumnDescription;
                field.isFilter = true;
                field.isAotuWidth = false;
                field.isSorted = true;
                field.isShow = true;
                field.width = 100;
                dic.Add(field);
            }
            string serverPath = GlobalContext.HostingEnvironment.ContentRootPath;
            data = new SingleTableTemplate(_unitOfWork.GetDbClient()).GetBaseConfig(serverPath, _logService.currentuser.UserName, keyValue, tableDescription, dic);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> CodePreviewJson(BaseConfigModel baseConfig)
        {
            try
            {
                List<DbColumnInfo> list = _service.GetTableFieldList(baseConfig.TableName);
                SingleTableTemplate template = new SingleTableTemplate(_unitOfWork.GetDbClient());
                string idcolumn = list.FirstOrDefault(a=>a.IsPrimarykey==true)?.DbColumnName;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                baseConfig.PageIndex.ButtonList = ExtList.removeNull(baseConfig.PageIndex.ButtonList);
                baseConfig.PageIndex.ColumnList = baseConfig.PageIndex.ColumnList.Where(a => a.field != "").ToList();
                baseConfig.PageForm.FieldList.Remove("");
                string idType = "string";
                //构造虚拟参数
				foreach (var item in baseConfig.PageIndex.ColumnList)
				{
					if (!list.Where(a=>a.DbColumnName == item.field).Any())
					{
                        DbColumnInfo temp =new DbColumnInfo();
                        temp.IsPrimarykey = false;
                        temp.DbColumnName = item.field;
                        temp.ColumnDescription = item.title;
                        temp.IsIdentity = false;
                        temp.IsNullable = true;
                        list.Add(temp);
                    }
				}
                DataTable dt = DataTableHelper.ListToDataTable(list);  // 用DataTable类型，避免依赖
                var tableinfo = _unitOfWork.GetDbClient().DbMaintenance.GetColumnInfosByTableName(baseConfig.TableName, false);
                string codeEntity = template.BuildEntity(baseConfig, dt, idcolumn);
                string codeService = template.BuildService(baseConfig, dt, idcolumn, idType);
                string codeController = template.BuildController(baseConfig, idcolumn, idType);
                string codeIndex = template.BuildIndex(baseConfig, idcolumn);
                string codeForm = template.BuildForm(baseConfig);
                string codeDetails = template.BuildDetails(baseConfig);
                string codeMenu = template.BuildMenu(baseConfig, idcolumn);
                var json = new
                {
                    CodeEntity = HttpUtility.HtmlEncode(codeEntity),
                    CodeService = HttpUtility.HtmlEncode(codeService),
                    CodeController = HttpUtility.HtmlEncode(codeController),
                    CodeIndex = HttpUtility.HtmlEncode(codeIndex),
                    CodeForm = HttpUtility.HtmlEncode(codeForm),
                    CodeDetails = HttpUtility.HtmlEncode(codeDetails),
                    CodeMenu = HttpUtility.HtmlEncode(codeMenu)
                };
                return Success("操作成功", json);
            }
            catch (System.Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpGet]
        public virtual ActionResult AddForm()
        {
            return View();
        }
        [HttpGet]
        public virtual ActionResult RuleForm()
        {
            return View();
        }
        [HttpGet]
        public virtual ActionResult EntityCode()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> CodeGenerateJson(BaseConfigModel baseConfig, string Code)
        {
            try
            {
                if (!GlobalContext.SystemConfig.Debug)
                {
                    throw new System.Exception("请在本地开发模式时使用代码生成");
                }
                else
                {
                    SingleTableTemplate template = new SingleTableTemplate(_unitOfWork.GetDbClient());
                    await template.CreateCode(baseConfig, HttpUtility.UrlDecode(Code));
                }
                return await Success("操作成功。", "", "");
            }
            catch (System.Exception ex)
            {
                return await Error(ex.Message, "", "");
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> EntityCodeGenerateJson(BaseConfigModel baseConfig, string keyValue)
        {
            try
            {
                if (!GlobalContext.SystemConfig.Debug)
                {
                    throw new System.Exception("请在本地开发模式时使用代码生成");
                }
                else
                {
                    List<DbColumnInfo> list = _service.GetTableFieldList(baseConfig.TableName);
                    SingleTableTemplate template = new SingleTableTemplate(_unitOfWork.GetDbClient());
                    DataTable dt = DataTableHelper.ListToDataTable(list);  // 用DataTable类型，避免依赖
                    string idcolumn = list.FirstOrDefault(a => a.IsPrimarykey == true)?.DbColumnName;
                    string codeEntity = template.BuildEntity(baseConfig, dt, idcolumn,true);
                    await template.EntityCreateCode(baseConfig, codeEntity);
                }
                return await Success("操作成功。", "", "");
            }
            catch (System.Exception ex)
            {
                return await Error(ex.Message, "", "");
            }
        }
        #endregion
    }
}