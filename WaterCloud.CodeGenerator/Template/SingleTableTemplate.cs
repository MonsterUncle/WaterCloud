using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using System.Web;
using Newtonsoft.Json.Linq;
using WaterCloud.Code;
using WaterCloud.Code.Extension;
using WaterCloud.Code.Model;
using WaterCloud.Repository.SystemManage;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.CodeGenerator
{
    public class SingleTableTemplate
    {
        private string buttoncacheKey = "watercloud_modulebuttondata_";
        private string cacheKey = "watercloud_moduleldata_";
        private string quickcacheKey = "watercloud_quickmoduledata_";
        private string initcacheKey = "watercloud_init_";
        #region GetBaseConfig
        public BaseConfigModel GetBaseConfig(string path,string username, string tableName, string tableDescription, List<string> tableFieldList)
        {
            path = GetProjectRootPath(path);

            int defaultField = 2; // 默认显示2个字段

            BaseConfigModel baseConfigModel = new BaseConfigModel();
            baseConfigModel.TableName = tableName;
            baseConfigModel.TableNameUpper = TableMappingHelper.ConvertTo_Uppercase(tableName);

            #region FileConfigModel
            baseConfigModel.FileConfig = new FileConfigModel();
            baseConfigModel.FileConfig.ClassPrefix = TableMappingHelper.GetClassNamePrefix(tableName);
            baseConfigModel.FileConfig.ClassDescription = tableDescription;
            baseConfigModel.FileConfig.CreateUserName = username;
            baseConfigModel.FileConfig.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            baseConfigModel.FileConfig.EntityName = string.Format("{0}Entity", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.IRepositoryName = string.Format("I{0}Repository", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.RepositoryName = string.Format("{0}Repository", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ServiceName = string.Format("{0}Service", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ControllerName = string.Format("{0}Controller", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.PageIndexName = "Index";
            baseConfigModel.FileConfig.PageFormName = "Form";
            baseConfigModel.FileConfig.PageDetailsName ="Details";
            #endregion

            #region OutputConfigModel          
            baseConfigModel.OutputConfig = new OutputConfigModel();
            baseConfigModel.OutputConfig.OutputModule = string.Empty;
            baseConfigModel.OutputConfig.OutputEntity = Path.Combine(path, "WaterCloud.Domain\\Entity");
            baseConfigModel.OutputConfig.OutputIRepository = Path.Combine(path, "WaterCloud.Domain\\IRepository");
            baseConfigModel.OutputConfig.OutputRepository = Path.Combine(path, "WaterCloud.Repository");
            baseConfigModel.OutputConfig.OutputService = Path.Combine(path, "WaterCloud.Service");
            baseConfigModel.OutputConfig.OutputWeb = Path.Combine(path, "WaterCloud.Web");
            #endregion

            #region PageIndexModel
            baseConfigModel.PageIndex = new PageIndexModel();
            baseConfigModel.PageIndex.IsMunu = 1;
            baseConfigModel.PageIndex.IsTree = 0;
            baseConfigModel.PageIndex.IsSearch = 1;
            baseConfigModel.PageIndex.IsPagination = 1;
            baseConfigModel.PageIndex.ButtonList = new List<string>();
            baseConfigModel.PageIndex.ColumnList = new List<string>();
            baseConfigModel.PageIndex.ColumnList.AddRange(tableFieldList.Take(defaultField));
            #endregion

            #region PageFormModel
            baseConfigModel.PageForm = new PageFormModel();
            baseConfigModel.PageForm.ShowMode = 1;
            baseConfigModel.PageForm.FieldList = new List<string>();
            baseConfigModel.PageForm.FieldList.AddRange(tableFieldList.Take(defaultField));
            #endregion

            return baseConfigModel;
        }
        #endregion

        #region BuildEntity
        public string BuildEntity(BaseConfigModel baseConfigModel, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            //sb.AppendLine("using Newtonsoft.Json;");
            //sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            //sb.AppendLine("using WaterCloud.Code;");
            sb.AppendLine("using Chloe.Annotations;");
            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("实体类", baseConfigModel, sb);

            sb.AppendLine("    [TableAttribute(\"" + baseConfigModel.TableNameUpper + "\")]");
            sb.AppendLine("    public class " + baseConfigModel.FileConfig.EntityName + " : " + GetBaseEntity(baseConfigModel.FileConfig.EntityName,dt));
            sb.AppendLine("    {");

            string column = string.Empty;
            string remark = string.Empty;
            string datatype = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                column = dr["TableColumn"].ToString();
                //基础字段一样生成
                //if (BaseField.BaseFieldList.Where(p => p == column.ToLower()).Any())
                //{
                //    // 基础字段不需要生成，继承合适的BaseEntity即可。
                //    continue;
                //}
                remark = dr["Remark"].ToString();
                datatype = dr["Datatype"].ToString();
                //column = TableMappingHelper.ConvertToUppercase(column);
                datatype = TableMappingHelper.GetPropertyDatatype(datatype);

                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// " + remark);
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        /// <returns></returns>");
                if (column== "F_Id")
                {
                    sb.AppendLine("        [ColumnAttribute(\"F_Id\", IsPrimaryKey = true)]");
                }
                //switch (datatype)
                //{
                //    case "long?":
                //        sb.AppendLine("        [JsonConverter(typeof(StringJsonConverter))]");
                //        break;

                //    case "DateTime?":
                //        sb.AppendLine("        [JsonConverter(typeof(DateTimeJsonConverter))]");
                //        break;
                //}
                sb.AppendLine("        public " + datatype + " " + column + " { get; set; }");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region BuildIRepository
        public string BuildIRepository(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using WaterCloud.DataBase;");
            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("数据映射接口", baseConfigModel, sb);

            sb.AppendLine("    public interface " + baseConfigModel.FileConfig.IRepositoryName + " : IRepositoryBase<"+ baseConfigModel.FileConfig.EntityName + ">");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region BuildRepository
        public string BuildRepository(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using WaterCloud.DataBase;");
            sb.AppendLine("using WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Repository." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("数据实现类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.RepositoryName + " : RepositoryBase<" + baseConfigModel.FileConfig.EntityName + ">,"+ baseConfigModel.FileConfig.IRepositoryName);
            sb.AppendLine("    {");
            sb.AppendLine("        private string ConnectStr;");
            sb.AppendLine("        private string providerName;");
            sb.AppendLine("        public "+ baseConfigModel.FileConfig.RepositoryName + "()");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine("        public " + baseConfigModel.FileConfig.RepositoryName + "(string ConnectStr, string providerName)");
            sb.AppendLine("             : base(ConnectStr, providerName)");
            sb.AppendLine("        {");
            sb.AppendLine("             this.ConnectStr = ConnectStr;");
            sb.AppendLine("             this.providerName = providerName;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region BuildService
        public string BuildService(BaseConfigModel baseConfigModel, DataTable dt)
        {
            string baseEntity = GetBaseEntity(baseConfigModel.FileConfig.EntityName, dt);

            StringBuilder sb = new StringBuilder();
            string method = string.Empty;
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using WaterCloud.Code;");
            sb.AppendLine("using WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using WaterCloud.Repository." + baseConfigModel.OutputConfig.OutputModule + ";");

            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Service." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("服务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ServiceName + " :  IDenpendency");
            sb.AppendLine("    {");                    
            sb.AppendLine("        private " + baseConfigModel.FileConfig.IRepositoryName + " service = new "+ baseConfigModel.FileConfig.RepositoryName + "();");
            sb.AppendLine("        private string cacheKey = \"watercloud_ " + baseConfigModel.FileConfig.ClassPrefix.ToLower() + "data_\";");
            sb.AppendLine("        #region 获取数据");
            sb.AppendLine("        public List<" + baseConfigModel.FileConfig.EntityName + "> GetList(string keyword = \"\")");
            sb.AppendLine("        {");
            sb.AppendLine("            var cachedata = service.CheckCacheList(cacheKey + \"list\");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(keyword))");
            sb.AppendLine("            {");
            sb.AppendLine("                //此处需修改");
            sb.AppendLine("                cachedata = cachedata.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword)).ToList();");
            sb.AppendLine("            }");
            sb.AppendLine("            return cachedata.OrderByDescending(t => t.F_CreatorTime).ToList();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public List<" + baseConfigModel.FileConfig.EntityName + "> GetList(Pagination pagination,string keyword = \"\")");
            sb.AppendLine("        {");
            sb.AppendLine("            var expression = ExtLinq.True<"+ baseConfigModel.FileConfig.EntityName + ">();");
            sb.AppendLine("            if (!string.IsNullOrEmpty(keyword))");
            sb.AppendLine("            {");
            sb.AppendLine("                //此处需修改");
            sb.AppendLine("                expression = expression.And(t => t.F_FullName.Contains(keyword));");
            sb.AppendLine("                expression = expression.Or(t => t.F_EnCode.Contains(keyword));");
            sb.AppendLine("            }");
            sb.AppendLine("            return service.FindList(expression, pagination);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public " + baseConfigModel.FileConfig.EntityName + " GetForm(string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var cachedata = service.CheckCache(cacheKey, keyValue);");
            sb.AppendLine("            return cachedata;");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        public void SubmitForm(" + baseConfigModel.FileConfig.EntityName + " entity, string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (!string.IsNullOrEmpty(keyValue))");
            sb.AppendLine("            {");
            sb.AppendLine("                entity.Create();");
            sb.AppendLine("                service.Insert(entity);");
            sb.AppendLine("                RedisHelper.Del(cacheKey + \"list\");");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                entity.Modify(keyValue); ");
            sb.AppendLine("                service.Update(entity);");
            sb.AppendLine("                RedisHelper.Del(cacheKey + keyValue);");
            sb.AppendLine("                RedisHelper.Del(cacheKey + \"list\");");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public void DeleteForm(string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            service.Delete(t => t.F_Id == keyValue);");
            sb.AppendLine("            RedisHelper.Del(cacheKey + keyValue);");
            sb.AppendLine("            RedisHelper.Del(cacheKey + \"list\");");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion

        #region BuildController
        public string BuildController(BaseConfigModel baseConfigModel,bool istree=false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections.Generic;");          
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using Senparc.CO2NET.Extensions;");
            sb.AppendLine("using WaterCloud.Code;");
            sb.AppendLine("using WaterCloud.Domain.SystemSecurity;");
            sb.AppendLine("using WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using WaterCloud.Service;");
            sb.AppendLine("using WaterCloud.Service.SystemSecurity;");
            sb.AppendLine("using WaterCloud.Service."+ baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Web.Areas." + baseConfigModel.OutputConfig.OutputModule + ".Controllers");
            sb.AppendLine("{");

            SetClassDescription("控制器类", baseConfigModel, sb);

            sb.AppendLine("    [Area(\"" + baseConfigModel.OutputConfig.OutputModule + "\")]");
            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ControllerName + " :  ControllerBase");
            sb.AppendLine("    {");
            sb.AppendLine("        private string moduleName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[3];");
            sb.AppendLine("        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];");
            sb.AppendLine("        private readonly LogService _logService;");
            sb.AppendLine("        private readonly ModuleService _moduleService;");
            sb.AppendLine("        private readonly "+ baseConfigModel.FileConfig.ServiceName+ " _service;");
            sb.AppendLine("        public "+ baseConfigModel.FileConfig.ControllerName + "("+baseConfigModel.FileConfig.ServiceName+" service, LogService logService, ModuleService moduleService)");
            sb.AppendLine("        {");
            sb.AppendLine("            _logService = logService;");
            sb.AppendLine("            _moduleService = moduleService;");
            sb.AppendLine("            _service = service;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        #region 获取数据");
            if (istree)
            {
                sb.AppendLine("        [HttpGet]");
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public ActionResult GetTreeGridJson(string keyword)");
                sb.AppendLine("        {");
                sb.AppendLine("            var data = _service.GetList(pagination,keyword);");
                sb.AppendLine("            if (!string.IsNullOrEmpty(keyword))");
                sb.AppendLine("            {");
                sb.AppendLine("                 //此处需修改");
                sb.AppendLine("                 data = data.TreeWhere(t => t.F_FullName.Contains(keyword));");
                sb.AppendLine("            }");
                sb.AppendLine("            return ResultLayUiTable(data.Count, data);");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        [HttpGet]");
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public ActionResult GetTreeSelectJson()");
                sb.AppendLine("        {");
                sb.AppendLine("            var data = _service.GetList();");
                sb.AppendLine("            var treeList = new List<TreeSelectModel>();");
                sb.AppendLine("            foreach (AreaEntity item in data)");
                sb.AppendLine("            {");
                sb.AppendLine("                //此处需修改");
                sb.AppendLine("                TreeSelectModel treeModel = new TreeSelectModel();");
                sb.AppendLine("                treeModel.id = item.F_Id;");
                sb.AppendLine("                treeModel.text = item.F_FullName;");
                sb.AppendLine("                treeModel.parentId = item.F_ParentId;");
                sb.AppendLine("                treeList.Add(treeModel);");
                sb.AppendLine("            }");
                sb.AppendLine("            return Content(treeList.TreeSelectJson());");
                sb.AppendLine("        }");
            }
            else
            {
                sb.AppendLine("        [HttpGet]");
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public ActionResult GetGridJson(Pagination pagination, string keyword)");
                sb.AppendLine("        {");
                sb.AppendLine("            //此处需修改");
                sb.AppendLine("            pagination.order = \"desc\";");
                sb.AppendLine("            pagination.sort = \"F_CreatorTime\";");
                sb.AppendLine("            var data = _service.GetList(pagination,keyword);");
                sb.AppendLine("            return ResultLayUiTable(pagination.records, data);");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        [HttpGet]");
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public ActionResult GetListJson(string keyword)");
                sb.AppendLine("        {");
                sb.AppendLine("            var data = _service.GetList(keyword);");
                sb.AppendLine("            return Content(data.ToJson());");
                sb.AppendLine("        }");
            }
            sb.AppendLine();
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [HandlerAjaxOnly]");
            sb.AppendLine("        public ActionResult GetFormJson(string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var data = _service.GetForm(keyValue);");
            sb.AppendLine("            return Content(data.ToJson());");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [HandlerAjaxOnly]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine("        public ActionResult SubmitForm("+ baseConfigModel.FileConfig.EntityName + " entity, string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();");
            sb.AppendLine("            var moduleitem = _moduleService.GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();");
            sb.AppendLine("            LogEntity logEntity;");
            sb.AppendLine("            if (string.IsNullOrEmpty(keyValue))");
            sb.AppendLine("            {");
            sb.AppendLine("                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Create.ToString());");
            sb.AppendLine("                logEntity.F_Description += DbLogType.Create.ToDescription();");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Update.ToString());");
            sb.AppendLine("                logEntity.F_Description += DbLogType.Update.ToDescription();");
            sb.AppendLine("                logEntity.F_KeyValue = keyValue;");
            sb.AppendLine("            }");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;");
            sb.AppendLine("                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;");
            sb.AppendLine("                _service.SubmitForm(entity, keyValue);");
            sb.AppendLine("                logEntity.F_Description += \"操作成功\";");
            sb.AppendLine("                _logService.WriteDbLog(logEntity);");
            sb.AppendLine("                return Success(\"操作成功。\");");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                logEntity.F_Result = false;");
            sb.AppendLine("                logEntity.F_Description += \"操作失败，\" + ex.Message;");
            sb.AppendLine("                _logService.WriteDbLog(logEntity);");
            sb.AppendLine("                return Error(ex.Message);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [HandlerAjaxOnly]");
            sb.AppendLine("        [HandlerAuthorize]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine("        public ActionResult DeleteForm(string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var module = _moduleService.GetList().Where(a => a.F_Layers == 1 && a.F_EnCode == moduleName).FirstOrDefault();");
            sb.AppendLine("            var moduleitem = _moduleService.GetList().Where(a => a.F_Layers > 1 && a.F_EnCode == className.Substring(0, className.Length - 10)).FirstOrDefault();");
            sb.AppendLine("            LogEntity logEntity = new LogEntity(module.F_FullName, moduleitem.F_FullName, DbLogType.Delete.ToString());");
            sb.AppendLine("            logEntity.F_Description += DbLogType.Delete.ToDescription();");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;");
            sb.AppendLine("                logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;");
            sb.AppendLine("                _service.DeleteForm(keyValue);");
            sb.AppendLine("                logEntity.F_Description += \"操作成功\";");
            sb.AppendLine("                _logService.WriteDbLog(logEntity);");
            sb.AppendLine("                return Success(\"操作成功。\");");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                logEntity.F_Result = false;");
            sb.AppendLine("                logEntity.F_Description += \"操作失败，\" + ex.Message;");
            sb.AppendLine("                _logService.WriteDbLog(logEntity);");
            sb.AppendLine("                return Error(ex.Message);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion

        #region BuildIndex
        public string BuildIndex(BaseConfigModel baseConfigModel)
        {
            #region 初始化集合
            if (baseConfigModel.PageIndex.ButtonList == null)
            {
                baseConfigModel.PageIndex.ButtonList = new List<string>();
            }
            if (baseConfigModel.PageIndex.ColumnList == null)
            {
                baseConfigModel.PageIndex.ColumnList = new List<string>();
            }
            #endregion
            List<KeyValue> list = GetButtonAuthorizeList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Index\";");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Index.cshtml\";");
            sb.AppendLine(" }");
            sb.AppendLine(" <div class=\"layuimini-container\">");
            sb.AppendLine("     <div class=\"layuimini-main\">");

            #region 搜索栏
            if (baseConfigModel.PageIndex.IsSearch == 1)
            {
                sb.AppendLine("         <fieldset class=\"table-search-fieldset\">");
                sb.AppendLine("             <div style=\"margin: 10px 10px 10px 10px\">");
                sb.AppendLine("                 <form class=\"layui-form layui-form-pane\" >");
                sb.AppendLine("                     <div class=\"layui-form-item\">");
                sb.AppendLine("                         <div class=\"layui-inline\">");
                sb.AppendLine("                             <label class=\"layui-form-label\">关键字:</label>");
                sb.AppendLine("                             <div class=\"layui-input-inline\">");
                sb.AppendLine("                                 <input type=\"text\" id=\"txt_keyword\" name=\"txt_keyword\" autocomplete=\"off\" class=\"layui-input\" >");
                sb.AppendLine("                             </div>");
                sb.AppendLine("                         </div>");
                sb.AppendLine("                        <div class=\"layui-inline\"> ");
                sb.AppendLine("                             <button type=\"submit\" class=\"layui-btn layui-btn-primary\" lay-submit lay-filter=\"data-search-btn\"><i class=\"layui-icon\">&#xe615;</i> 搜 索</button>");
                sb.AppendLine("                         </div>");
                sb.AppendLine("                     </div>");
                sb.AppendLine("                 </form>");
                sb.AppendLine("             </div>");
                sb.AppendLine("         </fieldset>");
            }
            #endregion

            #region 工具栏
            sb.AppendLine("         <script type=\"text/html\" id=\"toolbarDemo\">");
            sb.AppendLine("             <div class=\"layui-btn-container\" id=\"toolbar\">");
            if (baseConfigModel.PageIndex.ButtonList.Contains("add"))
            {
                KeyValue button = list.Where(p => p.Key == "add").FirstOrDefault();
                sb.AppendLine("                 <button id=\""+ button.Value + "\" authorize=\"yes\" class=\"layui-btn layui-btn-sm data-add-btn\" lay-event=\"" + button.Key + "\"><i class=\"layui-icon\">&#xe654;</i>" + button.Description + "</button>");
            }
            if (baseConfigModel.PageIndex.ButtonList.Contains("edit"))
            {
                KeyValue button = list.Where(p => p.Key == "edit").FirstOrDefault();
                sb.AppendLine("                 <button id=\"" + button.Value + "\" authorize=\"yes\" class=\"layui-btn layui-btn-sm layui-btn-warm data-edit-btn\" lay-event=\"" + button.Key + "\"><i class=\"layui-icon\">&#xe642;</i>" + button.Description + "</button>");
            }
            if (baseConfigModel.PageIndex.ButtonList.Contains("delete"))
            {
                KeyValue button = list.Where(p => p.Key == "delete").FirstOrDefault();
                sb.AppendLine("                 <button id=\"" + button.Value + "\" authorize=\"yes\" class=\"layui-btn layui-btn-sm layui-btn-danger data-delete-btn\" lay-event=\"" + button.Key + "\"> <i class=\"layui-icon\">&#xe640;</i>" + button.Description + "</button>");
            }
            if (baseConfigModel.PageIndex.ButtonList.Contains("details"))
            {
                KeyValue button = list.Where(p => p.Key == "details").FirstOrDefault();
                sb.AppendLine("                 <button id=\"" + button.Value + "\" authorize=\"yes\" class=\"layui-btn layui-btn-sm layui-btn-normal data-info-btn\" lay-event=\"" + button.Key + "\"> <i class=\"layui-icon\">&#xe60b;</i>" + button.Description + "</button>");
            }
            sb.AppendLine("             </div>");
            sb.AppendLine("         </script>");
            #endregion

            sb.AppendLine("         <table class=\"layui-hide\" id=\"currentTableId\" lay-filter=\"currentTableFilter\"></table>");
            sb.AppendLine("     </div>");
            sb.AppendLine(" </div>");

            #region js layui方法
            sb.AppendLine(" <script>");
            sb.AppendLine("     layui.use(['jquery', 'form', 'table', 'common', 'tablePlug', 'treetable'], function () {");
            sb.AppendLine("         var $ = layui.jquery,");
            sb.AppendLine("             form = layui.form,");
            sb.AppendLine("             table = layui.table,");
            sb.AppendLine("             treetable = layui.treetable,");
            sb.AppendLine("             common = layui.common;");
            if (baseConfigModel.PageIndex.IsTree==1)
            {
                sb.AppendLine("     var rendertree = function (queryJson) {");
                sb.AppendLine("         common.rendertreetable({");
                sb.AppendLine("             elem: '#currentTableId',");
                sb.AppendLine("             url: !queryJson ?'/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetTreeGridJson: '/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetTreeGridJson?keyword=' + queryJson',");
                sb.AppendLine("             cols: [[");
                sb.AppendLine("                 { field: 'F_Id', title: 'ID', sort: true, hide: true, hideAlways: true },");
                sb.AppendLine("                 //此处需修改");
                foreach (var item in baseConfigModel.PageIndex.ColumnList)
                {
                    sb.AppendLine("                 { field: '" + item + "', title: '" + item + "', width: 120, sort: true },");
                }
                sb.AppendLine("             ]],");
                sb.AppendLine("             done: function () {");
                sb.AppendLine("                 //权限控制");
                sb.AppendLine("                 common.authorizeButton(\"toolbar\");");
                sb.AppendLine("             }");
                sb.AppendLine("         });");
                sb.AppendLine("         rendertree();");
            }
            else
            {
                sb.AppendLine("         common.rendertable({");
                sb.AppendLine("             elem: '#currentTableId',");
                sb.AppendLine("             url: '/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetGridJson',");
                if (baseConfigModel.PageIndex.IsPagination != 1)
                {
                    sb.AppendLine("             page: false,");
                }                
                sb.AppendLine("             cols: [[");
                sb.AppendLine("                 { field: 'F_Id', title: 'ID', sort: true, hide: true, hideAlways: true },");
                sb.AppendLine("                 //此处需修改");
                foreach (var item in baseConfigModel.PageIndex.ColumnList)
                {
                    sb.AppendLine("                 { field: '" + item + "', title: '" + item + "', width: 120, sort: true },");
                }
                sb.AppendLine("             ]],");
                sb.AppendLine("             done: function () {");
                sb.AppendLine("                 //权限控制");
                sb.AppendLine("                 common.authorizeButton(\"toolbar\");");
                sb.AppendLine("             }");
                sb.AppendLine("         });");
            }
            sb.AppendLine("         // 监听搜索操作");
            sb.AppendLine("         form.on('submit(data-search-btn)', function (data) {");
            sb.AppendLine("             //执行搜索重载");
            sb.AppendLine("             common.reloadtable({");
            sb.AppendLine("                 elem: 'currentTableId',");
            if (baseConfigModel.PageIndex.IsTree == 1|| baseConfigModel.PageIndex.IsPagination != 1)
            {
                sb.AppendLine("                 page: false,");
            }
            sb.AppendLine("                 curr: 1,");
            sb.AppendLine("                 where: { keyword: data.field.txt_keyword}");
            sb.AppendLine("             });");
            sb.AppendLine("             return false;");
            sb.AppendLine("         });");
            sb.AppendLine("         var entity;");
            sb.AppendLine("         table.on('row(currentTableFilter)', function (obj) {");
            sb.AppendLine("             obj.tr.addClass(\"layui-table-click\").siblings().removeClass(\"layui-table-click\");");
            sb.AppendLine("             entity = obj;");
            sb.AppendLine("         })");
            sb.AppendLine("         //toolbar监听事件");
            sb.AppendLine("         table.on('toolbar(currentTableFilter)', function (obj) { ");
            sb.AppendLine("             if (obj.event === 'add') {  // 监听添加操作");
            sb.AppendLine("                 common.modalOpen({");
            sb.AppendLine("                     title: \"添加界面\",");
            sb.AppendLine("                     url: \"/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/Form\",");
            sb.AppendLine("                     width: \"500px\",");
            sb.AppendLine("                     height: \"500px\",");
            sb.AppendLine("                 });");
            sb.AppendLine("             } ");
            sb.AppendLine("             else if (obj.event === 'delete') {");
            sb.AppendLine("                 if (entity == null) {");
            sb.AppendLine("                     common.modalMsg(\"未选中数据\", \"warning\");");
            sb.AppendLine("                     return false;");
            sb.AppendLine("                 }");
            sb.AppendLine("                 common.deleteForm({");
            sb.AppendLine("                     url: \"/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/DeleteForm\",");
            sb.AppendLine("                     param: { keyValue: entity.data.F_Id },");
            sb.AppendLine("                     success: function () {");
            sb.AppendLine("                         common.reload('data-search-btn');");
            sb.AppendLine("                   }");
            sb.AppendLine("               });");
            sb.AppendLine( "           }");
            sb.AppendLine( "           else if (obj.event === 'edit') {");
            sb.AppendLine("               if (entity == null) {");
            sb.AppendLine( "                   common.modalMsg(\"未选中数据\", \"warning\");");
            sb.AppendLine( "                   return false;");
            sb.AppendLine( "               }");
            sb.AppendLine( "               common.modalOpen({");
            sb.AppendLine( "                  title: \"编辑界面\",");
            sb.AppendLine("                   url: \"/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/Form?keyValue=\" + entity.data.F_Id,");
            sb.AppendLine("                   width: \"500px\",");
            sb.AppendLine("                   height: \"500px\",");
            sb.AppendLine( "               });");
            sb.AppendLine( "           }");
            sb.AppendLine( "           else if (obj.event === 'details') {");
            sb.AppendLine("               if (entity == null) {");
            sb.AppendLine( "                   common.modalMsg(\"未选中数据\", \"warning\");");
            sb.AppendLine( "                   return false;");
            sb.AppendLine( "               }");
            sb.AppendLine( "               common.modalOpen({");
            sb.AppendLine( "                  title: \"查看界面\",");
            sb.AppendLine("                   url: \"/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/Details?keyValue=\" + entity.data.F_Id,");
            sb.AppendLine("                   width: \"500px\",");
            sb.AppendLine("                   height: \"500px\",");
            sb.AppendLine( "                  btn: []");
            sb.AppendLine( "               });");
            sb.AppendLine( "           }");
            sb.AppendLine( "           return false;");
            sb.AppendLine( "       });");
            sb.AppendLine( "   });");
            sb.AppendLine("</script>");
            #endregion

            return sb.ToString();
        }
        #endregion

        #region BuildForm
        public string BuildForm(BaseConfigModel baseConfigModel)
        {
            #region 初始化集合
            if (baseConfigModel.PageForm.FieldList == null)
            {
                baseConfigModel.PageForm.FieldList = new List<string>();
            }
            #endregion
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Form\"; ");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Form.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("<script>");
            sb.AppendLine("    layui.use(['jquery', 'form', 'laydate', 'tablePlug', 'common'], function () {");
            sb.AppendLine("        var form = layui.form,");
            sb.AppendLine("            $ = layui.$,");
            sb.AppendLine("            common = layui.common,");
            sb.AppendLine("            laydate = layui.laydate;");
            sb.AppendLine("        var keyValue = $.request(\"keyValue\");");
            sb.AppendLine("        //此处需修改");
            sb.AppendLine("        //类型为时间时");
            sb.AppendLine("        //laydate.render({");
            sb.AppendLine("            //elem: '#F_Birthday'");
            sb.AppendLine("            //, btns: ['clear', 'now']");
            sb.AppendLine("            //, trigger: 'click',");
            sb.AppendLine("            //format: 'yyyy-MM-dd',");
            sb.AppendLine("        //});");
            sb.AppendLine("");
            sb.AppendLine("        $(function () {");
            sb.AppendLine("            initControl();");
            sb.AppendLine("            if (!!keyValue) {");
            sb.AppendLine("                 $.ajax({");
            sb.AppendLine("                   url: '/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetFormJson',");
            sb.AppendLine("                   dataType: 'json',");
            sb.AppendLine("                   data: { keyValue: keyValue },");
            sb.AppendLine("                   async: false,");
            sb.AppendLine("                   success: function (data) {");
            sb.AppendLine("                       common.val('adminform', data);");
            sb.AppendLine("                    }");
            sb.AppendLine("               });");
            sb.AppendLine("           }");
            sb.AppendLine("           form.render();");
            sb.AppendLine("       });");
            sb.AppendLine("");
            sb.AppendLine("       function initControl() {");
            sb.AppendLine("           //此处需修改");
            sb.AppendLine("           //绑定数据源");
            sb.AppendLine("           //类型为下拉框时");
            sb.AppendLine("       }");
            sb.AppendLine("");
            sb.AppendLine("       //监听提交");
            sb.AppendLine("       form.on('submit(saveBtn)', function (data) {");
            sb.AppendLine("           var postData = data.field;");
            //if (baseConfigModel.PageForm.FieldList.Contains("F_EnabledMark"))
            //{
            //    sb.AppendLine("           if (!postData['F_EnabledMark']) postData['F_EnabledMark'] = false;");
            //}
            sb.AppendLine("           common.submitForm({");
            sb.AppendLine("               url: '/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/SubmitForm?keyValue=' + keyValue,");
            sb.AppendLine("               param: postData,");
            sb.AppendLine("               success: function () {");
            sb.AppendLine("                   common.parentreload('data-search-btn');");
            sb.AppendLine("               }");
            sb.AppendLine("           })");
            sb.AppendLine("           return false;");
            sb.AppendLine("       });");
            sb.AppendLine("   });");
            sb.AppendLine("</script>");
            sb.AppendLine("");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"layuimini - container\">");
            sb.AppendLine("        <div class=\"layuimini-main\">");
            sb.AppendLine("            <div class=\"layui-form layuimini-form\" lay-filter=\"adminform\">");
            #region 表单控件
            if (baseConfigModel.PageForm.FieldList.Count > 0)
            {
                string field = string.Empty;
                string fieldLower = string.Empty;
                switch (baseConfigModel.PageForm.ShowMode)
                {
                    case 1:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);
                            sb.AppendLine( "                <div class=\"layui-form-item\">");
                            sb.AppendLine( "                   <label class=\"layui-form-label required\">"+ field + "</label>");
                            sb.AppendLine( "                   <div class=\"layui-input-block\">");
                            sb.AppendLine("                        <input type=\"text\" id=\""+ field + "\" name=\"" + field + "\" autocomplete=\"off\" lay-verify=\"required\" class=\"layui-input\">");
                            sb.AppendLine( "                   </div>");
                            sb.AppendLine( "               </div>");
                        }
                        break;

                    case 2:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);

                            if (i % 2 == 0)
                            {
                                sb.AppendLine("                       <div class=\"layui-form-item\">");
                            }

                            sb.AppendLine("                    <div class=\"layui-inline\">");
                            sb.AppendLine("                        <label class=\"layui-form-label required\">" + field + "</label>");
                            sb.AppendLine("                        <div class=\"layui-input-inline\">");
                            sb.AppendLine("                            <input type=\"text\" id=\"" + field + "\" name=\"" + field + "\" autocomplete=\"off\" lay-verify=\"required\" class=\"layui-input\">");
                            sb.AppendLine("                        </div>");
                            sb.AppendLine("                    </div>");
                            if (i % 2 == 1)
                            {
                                sb.AppendLine("                       </div>");
                            }
                        }
                        break;
                }
            }
            #endregion
            sb.AppendLine("                <div class=\"layui-form-item layui-hide\">");
            sb.AppendLine("                    <button class=\"layui-btn\" lay-submit id=\"submit\" lay-filter=\"saveBtn\">确认保存</button>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("");
            return sb.ToString();
        }
        #endregion

        #region BuildDetails
        public string BuildDetails(BaseConfigModel baseConfigModel)
        {
            #region 初始化集合
            if (baseConfigModel.PageForm.FieldList == null)
            {
                baseConfigModel.PageForm.FieldList = new List<string>();
            }
            #endregion
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Details\"; ");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Form.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("<script>");
            sb.AppendLine("    layui.use(['jquery', 'form', 'laydate', 'tablePlug', 'common'], function () {");
            sb.AppendLine("        var form = layui.form,");
            sb.AppendLine("            $ = layui.$,");
            sb.AppendLine("            common = layui.common,");
            sb.AppendLine("            laydate = layui.laydate;");
            sb.AppendLine("        var keyValue = $.request(\"keyValue\");");
            sb.AppendLine("        //此处需修改");
            sb.AppendLine("        //类型为时间时");
            sb.AppendLine("        //laydate.render({");
            sb.AppendLine("            //elem: '#F_Birthday'");
            sb.AppendLine("            //, btns: ['clear', 'now']");
            sb.AppendLine("            //, trigger: 'click',");
            sb.AppendLine("            //format: 'yyyy-MM-dd',");
            sb.AppendLine("        //});");
            sb.AppendLine("");
            sb.AppendLine("        $(function () {");
            sb.AppendLine("            initControl();");
            sb.AppendLine("            $.ajax({");
            sb.AppendLine("                url: '/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetFormJson',");
            sb.AppendLine("                dataType: 'json',");
            sb.AppendLine("                data: { keyValue: keyValue },");
            sb.AppendLine("                async: false,");
            sb.AppendLine("                success: function (data) {");
            sb.AppendLine("                    common.val('adminform', data);");
            sb.AppendLine("                    common.setReadOnly('adminform');");
            sb.AppendLine("                    form.render();");
            sb.AppendLine("                 }");
            sb.AppendLine("            });");
            sb.AppendLine("       });");
            sb.AppendLine("");
            sb.AppendLine("       function initControl() {");
            sb.AppendLine("           //此处需修改");
            sb.AppendLine("           //绑定数据源");
            sb.AppendLine("           //类型为下拉框时");
            sb.AppendLine("       }");
            sb.AppendLine("   });");
            sb.AppendLine("</script>");
            sb.AppendLine("");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"layuimini - container\">");
            sb.AppendLine("        <div class=\"layuimini-main\">");
            sb.AppendLine("            <div class=\"layui-form layuimini-form\" lay-filter=\"adminform\">");
            #region 表单控件
            if (baseConfigModel.PageForm.FieldList.Count > 0)
            {
                string field = string.Empty;
                string fieldLower = string.Empty;
                switch (baseConfigModel.PageForm.ShowMode)
                {
                    case 1:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);
                            sb.AppendLine("                <div class=\"layui-form-item\">");
                            sb.AppendLine("                   <label class=\"layui-form-label required\">" + field + "</label>");
                            sb.AppendLine("                   <div class=\"layui-input-block\">");
                            sb.AppendLine("                        <input type=\"text\" id=\"" + field + "\" name=\"" + field + "\" lay-verify=\"required\" class=\"layui-input\">");
                            sb.AppendLine("                   </div>");
                            sb.AppendLine("               </div>");
                        }
                        break;

                    case 2:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);

                            if (i % 2 == 0)
                            {
                                sb.AppendLine("                       <div class=\"layui-form-item\">");
                            }

                            sb.AppendLine("                    <div class=\"layui-inline\">");
                            sb.AppendLine("                        <label class=\"layui-form-label required\">" + field + "</label>");
                            sb.AppendLine("                        <div class=\"layui-input-inline\">");
                            sb.AppendLine("                            <input type=\"text\" id=\"" + field + "\" name=\"" + field + "\" lay-verify=\"required\" class=\"layui-input\">");
                            sb.AppendLine("                        </div>");
                            sb.AppendLine("                    </div>");
                            if (i % 2 == 1)
                            {
                                sb.AppendLine("                       </div>");
                            }
                        }
                        break;
                }
            }
            #endregion
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("");
            return sb.ToString();
        }
        #endregion

        #region BuildMenu
        public string BuildMenu(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("  菜单路径:/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageIndexName);
            sb.AppendLine();
            List<KeyValue> list = GetButtonAuthorizeList();
            foreach (string btn in baseConfigModel.PageIndex.ButtonList)
            {
                KeyValue button = list.Where(p => p.Key == btn).FirstOrDefault();
                string form = "";
                switch (btn)
                {
                    case "delete":
                        form = "DeleteForm";
                        break;
                    case "details":
                        form = "Details";
                        break;
                    default:
                        form = "Form";
                        break;
                }
                string url = "/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + form;
                sb.AppendLine("  按钮名称：" + button.Description + ";编号：" + button.Value + ";事件：" + button.Key + ";连接：" + url);
            }
            sb.AppendLine();
            return sb.ToString();
        }
        #endregion

        #region CreateCode
        public List<KeyValue> CreateCode(BaseConfigModel baseConfigModel, string code)
        {
            List<KeyValue> result = new List<KeyValue>();
            JObject param = code.ToJObject();

            #region 实体类
            if (!string.IsNullOrEmpty(param["CodeEntity"].ParseToString()))
            {
                string codeEntity = HttpUtility.HtmlDecode(param["CodeEntity"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeEntity);
                    result.Add(new KeyValue { Key = "实体类", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("实体类已存在，实体类生成失败！");

                }
            }
            #endregion

            #region 业务接口
            if (!string.IsNullOrEmpty(param["CodeIRepository"].ParseToString()))
            {
                string codeEntity = HttpUtility.HtmlDecode(param["CodeIRepository"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputIRepository, baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.IRepositoryName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeEntity);
                    result.Add(new KeyValue { Key = "业务接口", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("业务接口已存在，业务接口生成失败！");
                }
            }
            #endregion

            #region 服务类
            if (!param["CodeService"].IsEmpty())
            {
                string codeService = HttpUtility.HtmlDecode(param["CodeService"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputService, baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.ServiceName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeService);
                    result.Add(new KeyValue { Key = "服务类", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("服务类已存在，服务类生成失败！");
                }
            }
            #endregion

            #region 业务类
            if (!param["CodeRepository"].IsEmpty())
            {
                string codeBusiness = HttpUtility.HtmlDecode(param["CodeRepository"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputRepository, baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.RepositoryName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeBusiness);
                    result.Add(new KeyValue { Key = "业务类", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("业务类已存在，业务类生成失败！");
                }
            }
            #endregion

            #region 控制器
            if (!param["CodeController"].IsEmpty())
            {
                string codeController = HttpUtility.HtmlDecode(param["CodeController"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Controllers", baseConfigModel.FileConfig.ControllerName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeController);
                    result.Add(new KeyValue { Key = "控制器", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("控制器已存在，控制器生成失败！");
                }
            }
            #endregion

            #region 列表页
            if (!param["CodeIndex"].IsEmpty())
            {
                string codeIndex = HttpUtility.HtmlDecode(param["CodeIndex"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageIndexName+ ".cshtml");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeIndex);
                    result.Add(new KeyValue { Key = "列表页", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("列表页已存在，列表页生成失败！");
                }
                ModuleRepository moduleRepository = new ModuleRepository();
                // 生成菜单，按钮
                List<KeyValue> buttonAuthorizeList = GetButtonAuthorizeList();
                string menuUrl ="/"+ baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageIndexName;
                ModuleEntity moduleEntity = new ModuleEntity();
                moduleEntity.Create();
                moduleEntity.F_Layers = moduleRepository.FindEntity(a => a.F_EnCode == baseConfigModel.OutputConfig.OutputModule).F_Layers+1; ;
                moduleEntity.F_FullName = baseConfigModel.FileConfig.ClassDescription;
                moduleEntity.F_UrlAddress = menuUrl;
                moduleEntity.F_EnCode = baseConfigModel.FileConfig.ClassPrefix;
                moduleEntity.F_IsPublic =false;
                moduleEntity.F_IsExpand = false;
                moduleEntity.F_IsMenu = baseConfigModel.PageIndex.IsMunu==1?true:false;
                moduleEntity.F_Target = "iframe";
                moduleEntity.F_AllowEdit = false;
                moduleEntity.F_AllowDelete = false;
                moduleEntity.F_EnabledMark = true;
                moduleEntity.F_DeleteMark = false;
                moduleEntity.F_ParentId = moduleRepository.FindEntity(a => a.F_EnCode == baseConfigModel.OutputConfig.OutputModule).F_Id;
                var parentModule = moduleRepository.FindEntity(a => a.F_EnCode == baseConfigModel.OutputConfig.OutputModule);
                moduleEntity.F_SortCode = (moduleRepository.IQueryable(a => a.F_ParentId == parentModule.F_Id).Max(a=>a.F_SortCode)??0)+1;
                List<ModuleButtonEntity> moduleButtonList = new List<ModuleButtonEntity>();
                int sort = 0;
                foreach (var item in baseConfigModel.PageIndex.ButtonList)
                {
                    KeyValue button = buttonAuthorizeList.Where(p => p.Key == item).FirstOrDefault();
                    string form = "";
                    switch (item)
                    {
                        case "delete":
                            form = "DeleteForm";
                            break;
                        case "details":
                            form = "Details";
                            break;
                        default:
                            form = "Form";
                            break;
                    }
                    string url = "/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + form;
                    ModuleButtonEntity modulebutton = new ModuleButtonEntity();
                    modulebutton.Create();
                    modulebutton.F_ModuleId = moduleEntity.F_Id;
                    modulebutton.F_ParentId = "0";
                    modulebutton.F_Layers = 1;
                    modulebutton.F_EnCode = button.Value;
                    modulebutton.F_JsEvent = button.Key;
                    modulebutton.F_FullName = button.Description;
                    modulebutton.F_Location = button.Key=="add"?1:2;
                    modulebutton.F_SortCode= sort;
                    sort++;
                    modulebutton.F_EnabledMark = true;
                    modulebutton.F_DeleteMark = false;
                    modulebutton.F_Split = false;
                    modulebutton.F_AllowDelete = false;
                    modulebutton.F_AllowEdit = false;
                    modulebutton.F_IsPublic = false;
                    modulebutton.F_UrlAddress = url;
                    moduleButtonList.Add(modulebutton);
                }
                moduleRepository.CreateModuleCode(moduleEntity,moduleButtonList);
                RedisHelper.Del(buttoncacheKey + "list");
                RedisHelper.Del(cacheKey + "list");
                RedisHelper.Del(quickcacheKey + "list");
                RedisHelper.Del(initcacheKey + "list");
                RedisHelper.Del(initcacheKey + "modulebutton_list");
            }
            #endregion

            #region 表单页
            if (!param["CodeForm"].IsEmpty())
            {
                string codeSave = HttpUtility.HtmlDecode(param["CodeForm"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageFormName + ".cshtml");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeSave);
                    result.Add(new KeyValue { Key = "表单页", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("表单页存在，表单页生成失败！");
                }
            }
            #endregion

            #region 查看页
            if (!param["CodeDetails"].IsEmpty())
            {
                string codeSave = HttpUtility.HtmlDecode(param["CodeDetails"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageDetailsName + ".cshtml");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeSave);
                    result.Add(new KeyValue { Key = "详情页", Value = codePath, Description = "生成成功！" });
                }
                else
                {
                    throw new Exception("详情页存在，详情页生成失败！");
                }
            }
            #endregion

            return result;
        }
        #endregion

        #region 私有方法
        #region GetProjectRootPath
        private string GetProjectRootPath(string path)
        {
            path = path.ParseToString();
            path = path.Trim('\\');
            if (GlobalContext.SystemConfig.Debug)
            {
                // 向上找一级
                path = Directory.GetParent(path).FullName;
                //path = Directory.GetParent(path).FullName;
            }
            return path;
        }
        #endregion

        #region SetClassDescription
        private void SetClassDescription(string type, BaseConfigModel baseConfigModel, StringBuilder sb)
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 创 建：" + baseConfigModel.FileConfig.CreateUserName);
            sb.AppendLine("    /// 日 期：" + baseConfigModel.FileConfig.CreateDate);
            sb.AppendLine("    /// 描 述：" + baseConfigModel.FileConfig.ClassDescription + type);
            sb.AppendLine("    /// </summary>");
        }
        #endregion

        #region GetButtonAuthorizeList

        private List<KeyValue> GetButtonAuthorizeList()
        {
            var list = new List<KeyValue>();
            list.Add(new KeyValue { Key = "add", Value = "NF-add", Description = "新增" });
            list.Add(new KeyValue { Key = "edit", Value = "NF-edit", Description = "修改" });
            list.Add(new KeyValue { Key = "delete", Value = "NF-delete", Description = "删除" });
            list.Add(new KeyValue { Key = "details", Value = "NF-details", Description = "查看" });
            return list;
        }
        #endregion 

        private string GetBaseEntity(string EntityName, DataTable dt)
        {
            string entity = string.Empty;
            var columnList = dt.AsEnumerable().Select(p => p["TableColumn"].ParseToString()).ToList();

            bool id = columnList.Where(p => p == "F_Id").Any();
            bool baseIsDelete = columnList.Where(p => p == "F_DeleteUserId").Any()&& columnList.Where(p => p == "F_DeleteTime").Any()&& columnList.Where(p => p == "F_DeleteMark").Any();
            bool baseIsCreate = columnList.Where(p => p == "F_Id").Any() && columnList.Where(p => p == "F_CreatorUserId").Any() && columnList.Where(p => p == "F_CreatorTime").Any();
            bool baseIsModifie = columnList.Where(p => p == "F_Id").Any() && columnList.Where(p => p == "F_LastModifyUserId").Any() && columnList.Where(p => p == "F_LastModifyTime").Any();


            if (!id)
            {
                throw new Exception("数据库表必须有主键id字段");
            }
            entity = "IEntity<"+ EntityName + ">";
            if (baseIsCreate)
            {
                entity += ",ICreationAudited";
            }
            if (baseIsModifie)
            {
                entity += ",IModificationAudited";
            }
            if (baseIsDelete)
            {
                entity += ",IDeleteAudited";
            }
            return entity;
        }
        #endregion
    }
}
