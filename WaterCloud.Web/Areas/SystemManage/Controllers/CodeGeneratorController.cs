using System.Collections.Generic;
using System.Web;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Domain;
using WaterCloud.Code;
using WaterCloud.CodeGenerator;
using WaterCloud.Service.CommonService;
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Service.SystemManage;
using WaterCloud.Domain.SystemSecurity;
using System.Linq;
using WaterCloud.Service;
using System.Threading.Tasks;
using WaterCloud.Code.Extend;
using Serenity;
using Chloe;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class CodeGeneratorController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        private readonly IDatabaseTableService _service;
        private readonly LogService _logService;
        private readonly IDbContext _context;
        public CodeGeneratorController(LogService logService, IDbContext context)
        {
            string dbType = GlobalContext.SystemConfig.DBProvider;
            _context = context;
            switch (dbType)
            {
                case "System.Data.SqlClient":
                    _service = new DatabaseTableSqlServerService(context);
                    break;
                case "MySql.Data.MySqlClient":
                    _service = new DatabaseTableMySqlService(context);
                    break;
                default:
                    _service = new DatabaseTableMySqlService(context);
                    break;
            }
            _logService = logService;
        }
        #region 视图功能

        #endregion

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTablePageListJson(Pagination pagination,string keyword)
        {
            List<TableInfo> data =await _service.GetTablePageList(keyword, pagination);
            return Success(pagination.records, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableJson(string keyword)
        {
            List<TableInfo> data =await _service.GetTableList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableSelectJson(string keyword)
        {
            List<TableInfo> data =await _service.GetTableList(keyword);
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { name = item.TableName, value = item.TableName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableFieldJson(string keyValue,string keyword)
        {
            List<TableFieldInfo> data =await _service.GetTableFieldList(keyValue);
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.TableColumn.Contains(keyword)).ToList();
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTableFieldSelectJson(string keyValue)
        {
            List<TableFieldInfo> data =await _service.GetTableFieldList(keyValue);
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { text = item.TableColumn, id = item.TableColumn });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetBaseConfigJson(string keyValue)
        {
            BaseConfigModel data = new BaseConfigModel();

            string tableDescription = string.Empty;
            List<TableFieldInfo> tDataTableField =await _service.GetTableFieldList(keyValue);

            var columnList = tDataTableField.Where(p => !BaseField.BaseFieldList.Contains(p.TableColumn)&&p.TableIdentity!="Y").Select(p =>new { p.TableColumn,p.Remark }).ToList();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in columnList)
            {
                dic.Add(item.TableColumn, string.IsNullOrEmpty(item.Remark) ? item.TableColumn : item.Remark);
            }
            string serverPath = GlobalContext.HostingEnvironment.ContentRootPath;
            data = new SingleTableTemplate(_context).GetBaseConfig(serverPath, OperatorProvider.Provider.GetCurrent().UserName, keyValue, tableDescription, dic);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> CodePreviewJson(BaseConfigModel baseConfig)
        {
            try
            {
                List<TableFieldInfo> list =await _service.GetTableFieldList(baseConfig.TableName);
                SingleTableTemplate template = new SingleTableTemplate(_context);
                DataTable dt = DataTableHelper.ListToDataTable(list);  // 用DataTable类型，避免依赖
                string idcolumn = string.Empty;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                baseConfig.PageIndex.ButtonList=ExtList.removeNull(baseConfig.PageIndex.ButtonList);
                baseConfig.PageIndex.ColumnList.Remove("");
                baseConfig.PageForm.FieldList.Remove("");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["TableIdentity"].ToString() == "Y")
                    {
                        idcolumn = dr["TableColumn"].ToString();
                    }
                }
                string codeEntity = template.BuildEntity(baseConfig, dt, idcolumn);
                string codeIRepository = template.BuildIRepository(baseConfig);
                string codeRepository = template.BuildRepository(baseConfig);
                string codeService = template.BuildService(baseConfig,dt, idcolumn);
                string codeController = template.BuildController(baseConfig, idcolumn);
                string codeIndex = template.BuildIndex(baseConfig, idcolumn);
                string codeForm = template.BuildForm(baseConfig);
                string codeDetails = template.BuildDetails(baseConfig);
                string codeMenu = template.BuildMenu(baseConfig);
                var json = new
                {
                    CodeEntity = HttpUtility.HtmlEncode(codeEntity),
                    CodeIRepository = HttpUtility.HtmlEncode(codeIRepository),
                    CodeRepository = HttpUtility.HtmlEncode(codeRepository),
                    CodeService = HttpUtility.HtmlEncode(codeService),
                    CodeController = HttpUtility.HtmlEncode(codeController),
                    CodeIndex = HttpUtility.HtmlEncode(codeIndex),
                    CodeForm = HttpUtility.HtmlEncode(codeForm),
                    CodeDetails = HttpUtility.HtmlEncode(codeDetails),
                    CodeMenu = HttpUtility.HtmlEncode(codeMenu)
                };
               return Success("操作成功",json);
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
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CodeGenerateJson(BaseConfigModel baseConfig, string Code)
        {
            LogEntity logEntity = await _logService.CreateLog(className, DbLogType.Create.ToString());
            logEntity.F_Description += DbLogType.Create.ToDescription();
            try
            {
                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
                if (!GlobalContext.SystemConfig.Debug)
                {
                    throw new System.Exception("请在本地开发模式时使用代码生成");
                }
                else
                {
                    SingleTableTemplate template = new SingleTableTemplate(_context);
                    await template.CreateCode(baseConfig, HttpUtility.UrlDecode(Code));
                }
                logEntity.F_Description += "操作成功";
                await _logService.WriteDbLog(logEntity);
                return Success("操作成功。");
            }
            catch (System.Exception ex)
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