using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using System.Web;
using Newtonsoft.Json.Linq;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using System.Threading.Tasks;
using Chloe;
using WaterCloud.DataBase;

namespace WaterCloud.CodeGenerator
{
    public class SingleTableTemplate
    {
        private string buttoncacheKey = "watercloud_modulebuttondata_";
        private string fieldscacheKey = "watercloud_modulefieldsdata_";
        private string cacheKey = "watercloud_moduleldata_";
        private string quickcacheKey = "watercloud_quickmoduledata_";
        private string initcacheKey = "watercloud_init_";
        private string authorizecacheKey = "watercloud_authorizeurldata_";// +权限
        private IRepositoryBase uniwork;
        public SingleTableTemplate(IDbContext context)
        {
            uniwork = new RepositoryBase(context);
        }
        #region GetBaseConfig
        public BaseConfigModel GetBaseConfig(string path,string username, string tableName, string tableDescription, Dictionary<string,string> tableFieldList)
        {
            path = GetProjectRootPath(path);

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
            baseConfigModel.OutputConfig.OutputService = Path.Combine(path, "WaterCloud.Service");
            baseConfigModel.OutputConfig.OutputWeb = Path.Combine(path, "WaterCloud.Web");
            #endregion

            #region PageIndexModel
            baseConfigModel.PageIndex = new PageIndexModel();
            baseConfigModel.PageIndex.IsMunu = 1;
            baseConfigModel.PageIndex.IsTree = 0;
            baseConfigModel.PageIndex.IsSearch = 1;
            baseConfigModel.PageIndex.IsFields = 1;
            baseConfigModel.PageIndex.IsPagination = 1;
            baseConfigModel.PageIndex.IsFields = 0;
            baseConfigModel.PageIndex.IsPublic = 0;
            baseConfigModel.PageIndex.ButtonList = new List<string>();
            baseConfigModel.PageIndex.ColumnList = tableFieldList;
            #endregion

            #region PageFormModel
            baseConfigModel.PageForm = new PageFormModel();
            baseConfigModel.PageForm.ShowMode = 1;
            baseConfigModel.PageForm.FieldList=new Dictionary<string, string>();
            #endregion

            return baseConfigModel;
        }
        #endregion

        #region BuildEntity
        public string BuildEntity(BaseConfigModel baseConfigModel, DataTable dt,string idColumn = "F_Id")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            //sb.AppendLine("using Newtonsoft.Json;");
            //sb.AppendLine("using WaterCloud.Code;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using Chloe.Annotations;");
            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("实体类", baseConfigModel, sb);
            baseConfigModel.TableNameUpper=TableMappingHelper.ConvertTo_Uppercase(baseConfigModel.TableName);
            sb.AppendLine("    [TableAttribute(\"" + baseConfigModel.TableName + "\")]");
            var baseEntity= GetBaseEntity(baseConfigModel.FileConfig.EntityName, dt, idColumn);
            if (string.IsNullOrEmpty(baseEntity))
            {
                sb.AppendLine("    public class " + baseConfigModel.FileConfig.EntityName);
            }
            else
            {
                sb.AppendLine("    public class " + baseConfigModel.FileConfig.EntityName + " : " + baseEntity);
            }
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
                if (idColumn == column)
                {

                    if (datatype == "int?")
                    {
                        sb.AppendLine("        [AutoIncrement]");
                        sb.AppendLine("        [Column(\"" + column + "\", IsPrimaryKey = true)]");
                        sb.AppendLine("        public int " + column + " { get; set; }");
                    }
                    else if (datatype == "long?")
                    {
                        sb.AppendLine("        [Column(\"" + column + "\", IsPrimaryKey = true)]");
                        sb.AppendLine("        public long " + column + " { get; set; }");
                    }
                    else
                    {
                        sb.AppendLine("        [Column(\"" + column + "\", IsPrimaryKey = true)]");
                        sb.AppendLine("        public " + datatype + " " + column + " { get; set; }");
                    }
                }
                else
                {
                    sb.AppendLine("        public " + datatype + " " + column + " { get; set; }");
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
                
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region BuildService
        public string BuildService(BaseConfigModel baseConfigModel, DataTable dt, string idColumn = "F_Id",string idType="int",string deleteMarkField="IsDelete",string createTimeField="AddTime")
        {
            var baseEntity = GetBaseEntity(baseConfigModel.FileConfig.EntityName, dt, idColumn);
            StringBuilder sb = new StringBuilder();
            string method = string.Empty;
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Threading.Tasks;");          
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using WaterCloud.Code;");
            sb.AppendLine("using Chloe;");
            sb.AppendLine("using WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Service." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("服务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ServiceName + " : DataFilterService<"+ baseConfigModel.FileConfig.EntityName + ">, IDenpendency");
            sb.AppendLine("    {");                    
            sb.AppendLine("        private string cacheKey = \"watercloud_" + baseConfigModel.FileConfig.ClassPrefix.ToLower() + "data_\";");
            sb.AppendLine("        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];");

            sb.AppendLine("        public "+ baseConfigModel.FileConfig.ServiceName + "(IDbContext context) : base(context)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");

            sb.AppendLine("        #region 获取数据");
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> GetList(string keyword = \"\")");
            sb.AppendLine("        {");
            sb.AppendLine("            var cachedata = await repository.CheckCacheList(cacheKey + \"list\");");
            sb.AppendLine("            if (!string.IsNullOrEmpty(keyword))");
            sb.AppendLine("            {");
            sb.AppendLine("                //此处需修改");
            sb.AppendLine("                cachedata = cachedata.Where(t => t.F_FullName.Contains(keyword) || t.F_EnCode.Contains(keyword)).ToList();");
            sb.AppendLine("            }");
            sb.AppendLine($"            return cachedata.Where(t => t.{deleteMarkField} == false).OrderByDescending(t => t.{createTimeField}).ToList();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> GetLookList(string keyword = \"\")");
            sb.AppendLine("        {");
            sb.AppendLine("            var list =new List<" + baseConfigModel.FileConfig.EntityName + ">();");
            sb.AppendLine("            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))");
            sb.AppendLine("            {");
            sb.AppendLine("                list = await repository.CheckCacheList(cacheKey + \"list\");");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                var forms = GetDataPrivilege(\"u\", className.Substring(0, className.Length - 7));");
            sb.AppendLine("                list = forms.ToList();");
            sb.AppendLine("            }");
            sb.AppendLine("            if (!string.IsNullOrEmpty(keyword))");
            sb.AppendLine("            {");
            sb.AppendLine("                //此处需修改");
            sb.AppendLine("                list = list.Where(u => u.F_FullName.Contains(keyword) || u.F_EnCode.Contains(keyword)).ToList();");
            sb.AppendLine("            }");
            sb.AppendLine($"            return GetFieldsFilterData(list.Where(t => t.{deleteMarkField} == false).OrderByDescending(t => t.{createTimeField}).ToList(),className.Substring(0, className.Length - 7));");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> GetLookList(SoulPage<"+ baseConfigModel.FileConfig.EntityName + "> pagination,string keyword = \"\","+idType+" id=\"\")");
            sb.AppendLine("        {");
            sb.AppendLine("            //获取数据权限");
            sb.AppendLine("            var list = GetDataPrivilege(\"u\", className.Substring(0, className.Length - 7));");
            sb.AppendLine("            if (!string.IsNullOrEmpty(keyword))");
            sb.AppendLine("            {");
            sb.AppendLine("                //此处需修改");
            sb.AppendLine("                list = list.Where(u => u.F_FullName.Contains(keyword) || u.F_EnCode.Contains(keyword));");
            sb.AppendLine("            }");
            sb.AppendLine($"            list = list.Where(u => u.{deleteMarkField}==false);");
            if (idType == "int" || idType == "long")
            {
                sb.AppendLine("            if(id==0)");
            }
            else
            {
                sb.AppendLine("            if(!string.IsNullOrEmpty(id))");
            }
            sb.AppendLine("            {");
            sb.AppendLine("                list= list.Where(u=>u." + idColumn + "==id);");
            sb.AppendLine("            }");
            sb.AppendLine("            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<" + baseConfigModel.FileConfig.EntityName + "> GetForm(string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var cachedata = await repository.CheckCache(cacheKey, keyValue);");
            sb.AppendLine("            return cachedata;");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        public async Task<" + baseConfigModel.FileConfig.EntityName + "> GetLookForm(object keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var cachedata = await repository.CheckCache(cacheKey, keyValue);");
            sb.AppendLine("            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        public async Task SubmitForm(" + baseConfigModel.FileConfig.EntityName + " entity, string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (string.IsNullOrEmpty(keyValue))");
            sb.AppendLine("            {");
            sb.AppendLine("                    //此处需修改");
            sb.AppendLine($"                    entity.{deleteMarkField} = false;");
            sb.AppendLine($"                    entity.{createTimeField} = DateTime.Now;");
            foreach (DataRow dr in dt.Rows)
            {
                string column = dr["TableColumn"].ToString();
                if (column != idColumn)
                {
                    if (!baseConfigModel.PageForm.FieldList.Keys.Contains(column))
                    {
                        if (column != deleteMarkField && column != createTimeField)
                        {
                            sb.AppendLine($"                entity.{column} = ;//添写初始化参数;");
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(baseEntity))
            {
                if (idType == "int")
                {
                    sb.AppendLine("                entity." + idColumn + "=0;");
                }
                else if (idType == "long")
                {
                    sb.AppendLine("                entity." + idColumn + "=0;");
                }
                else
                {
                    sb.AppendLine("                entity." + idColumn + "=Utils.GuId();");
                }
            }
            else
            {
                sb.AppendLine("                entity.Create();");
            }
            sb.AppendLine("                await repository.Insert(entity);");
            sb.AppendLine("                await CacheHelper.Remove(cacheKey + \"list\");");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                    //此处需修改");
            if (string.IsNullOrEmpty(baseEntity))
            {
                if (idType == "int")
                {
                    sb.AppendLine("                entity." + idColumn + "=Convert.ToInt32(keyValue);");
                }
                else if (idType == "long")
                {
                    sb.AppendLine("                entity." + idColumn + "=Convert.ToInt64(keyValue);");
                }
                else
                {
                    sb.AppendLine("                entity." + idColumn + "=keyValue;");
                }
            }
            else
            {
                sb.AppendLine("                entity.Modify(keyValue); ");
            }
            sb.AppendLine("                await repository.Update(entity);");
            sb.AppendLine("                await CacheHelper.Remove(cacheKey + keyValue);");
            sb.AppendLine("                await CacheHelper.Remove(cacheKey + \"list\");");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task DeleteForm(string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var ids = keyValue.Split(',');");
            sb.AppendLine("            await repository.Delete(t => ids.Contains(t." + idColumn + ".ToString()));");
            sb.AppendLine("            foreach (var item in ids)");
            sb.AppendLine("            {");
            sb.AppendLine("            await CacheHelper.Remove(cacheKey + item);");
            sb.AppendLine("            }");
            sb.AppendLine("            await CacheHelper.Remove(cacheKey + \"list\");");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion

        #region BuildController
        public string BuildController(BaseConfigModel baseConfigModel, string idColumn="F_Id",string idType="string",string createTimeField="AddTime")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using System.Collections.Generic;");          
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using WaterCloud.Code;");
            sb.AppendLine("using WaterCloud.Domain." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using WaterCloud.Service;");
            sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
            sb.AppendLine("using WaterCloud.Service."+ baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine();

            sb.AppendLine("namespace WaterCloud.Web.Areas." + baseConfigModel.OutputConfig.OutputModule + ".Controllers");
            sb.AppendLine("{");

            SetClassDescription("控制器类", baseConfigModel, sb);

            sb.AppendLine("    [Area(\"" + baseConfigModel.OutputConfig.OutputModule + "\")]");
            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ControllerName + " :  ControllerBase");
            sb.AppendLine("    {");
            sb.AppendLine("        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];");
            sb.AppendLine("        public " + baseConfigModel.FileConfig.ServiceName + " _service {get;set;}");
            sb.AppendLine();
            sb.AppendLine("        #region 获取数据");
            if (baseConfigModel.PageIndex.IsTree>0)
            {
                sb.AppendLine("        [HttpGet]");
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public async Task<ActionResult> GetTreeGridJson(string keyword)");
                sb.AppendLine("        {");
                sb.AppendLine("            var data = await _service.GetLookList(keyword);");
                sb.AppendLine("            if (!string.IsNullOrEmpty(keyword))");
                sb.AppendLine("            {");
                sb.AppendLine("                 //此处需修改");
                sb.AppendLine("                 data = data.TreeWhere(t => t.F_FullName.Contains(keyword));");
                sb.AppendLine("            }");
                sb.AppendLine("            return Success(data.Count, data);");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        [HttpGet]");
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public async Task<ActionResult> GetTreeSelectJson()");
                sb.AppendLine("        {");
                sb.AppendLine("            var data = await _service.GetList();");
                sb.AppendLine("            var treeList = new List<TreeSelectModel>();");
                sb.AppendLine("            foreach (var item in data)");
                sb.AppendLine("            {");
                sb.AppendLine("                //此处需修改");
                sb.AppendLine("                TreeSelectModel treeModel = new TreeSelectModel();");
                sb.AppendLine("                treeModel.id = item."+ idColumn + ";");
                sb.AppendLine("                treeModel.text = item.F_FullName;");
                sb.AppendLine("                treeModel.parentId = item.F_ParentId;");
                sb.AppendLine("                treeList.Add(treeModel);");
                sb.AppendLine("            }");
                sb.AppendLine("            return Content(treeList.TreeSelectJson());");
                sb.AppendLine("        }");
            }
            else
            {
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public async Task<ActionResult> GetGridJson(SoulPage<"+ baseConfigModel.FileConfig .EntityName+ "> pagination, string keyword)");
                sb.AppendLine("        {");
                sb.AppendLine("            if (string.IsNullOrEmpty(pagination.field))");
                sb.AppendLine("            {");
                sb.AppendLine($"                pagination.field = \"{createTimeField}\";");
                sb.AppendLine("                pagination.order = \"desc\";");
                sb.AppendLine("            }");
                sb.AppendLine("            var data = await _service.GetLookList(pagination,keyword);");
                sb.AppendLine("            return Content(pagination.setData(data).ToJson());");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        [HttpGet]");
                sb.AppendLine("        [HandlerAjaxOnly]");
                sb.AppendLine("        public async Task<ActionResult> GetListJson(string keyword)");
                sb.AppendLine("        {");
                sb.AppendLine("            var data = await _service.GetList(keyword);");
                sb.AppendLine("            return Content(data.ToJson());");
                sb.AppendLine("        }");
            }
            sb.AppendLine();
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [HandlerAjaxOnly]");
            sb.AppendLine($"        public async Task<ActionResult> GetFormJson({idType} keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            var data = await _service.GetLookForm(keyValue);");
            sb.AppendLine("            return Content(data.ToJson());");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [HandlerAjaxOnly]");
            sb.AppendLine("        public async Task<ActionResult> SubmitForm(" + baseConfigModel.FileConfig.EntityName + " entity, string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                await _service.SubmitForm(entity, keyValue);");
            sb.AppendLine("                return await Success(\"操作成功。\", className, keyValue);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                return await Error(ex.Message, className, keyValue);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [HandlerAjaxOnly]");
            sb.AppendLine("        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]");
            sb.AppendLine("        public async Task<ActionResult> DeleteForm(string keyValue)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                await _service.DeleteForm(keyValue);");
            sb.AppendLine("                return await Success(\"操作成功。\", className, keyValue, DbLogType.Delete);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                return await Error(ex.Message, className, keyValue, DbLogType.Delete);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion

        #region BuildIndex
        public string BuildIndex(BaseConfigModel baseConfigModel, string idColumn = "F_Id")
        {
            #region 初始化集合
            if (baseConfigModel.PageIndex.ButtonList == null)
            {
                baseConfigModel.PageIndex.ButtonList = new List<string>();
            }
            if (baseConfigModel.PageIndex.ColumnList == null)
            {
                baseConfigModel.PageIndex.ColumnList = new Dictionary<string, string>();
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
                sb.AppendLine("         <fieldset class=\"table-search-fieldset layui-hide\" id=\"searchField\">");
                sb.AppendLine("             <div>");
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
                sb.AppendLine("                 <button id=\""+ button.Value + "\" authorize class=\"layui-btn layui-btn-sm data-add-btn layui-hide\" lay-event=\"" + button.Key + "\"><i class=\"layui-icon\">&#xe654;</i>" + button.Description + "</button>");
            }
            if (baseConfigModel.PageIndex.ButtonList.Contains("edit"))
            {
                KeyValue button = list.Where(p => p.Key == "edit").FirstOrDefault();
                sb.AppendLine("                 <button id=\"" + button.Value + "\" authorize class=\"layui-btn layui-btn-sm layui-btn-warm data-edit-btn layui-hide\" lay-event=\"" + button.Key + "\"><i class=\"layui-icon\">&#xe642;</i>" + button.Description + "</button>");
            }
            if (baseConfigModel.PageIndex.ButtonList.Contains("delete"))
            {
                KeyValue button = list.Where(p => p.Key == "delete").FirstOrDefault();
                sb.AppendLine("                 <button id=\"" + button.Value + "\" authorize class=\"layui-btn layui-btn-sm layui-btn-danger data-delete-btn layui-hide\" lay-event=\"" + button.Key + "\"> <i class=\"layui-icon\">&#xe640;</i>" + button.Description + "</button>");
            }
            if (baseConfigModel.PageIndex.ButtonList.Contains("details"))
            {
                KeyValue button = list.Where(p => p.Key == "details").FirstOrDefault();
                sb.AppendLine("                 <button id=\"" + button.Value + "\" authorize class=\"layui-btn layui-btn-sm layui-btn-normal data-info-btn layui-hide\" lay-event=\"" + button.Key + "\"> <i class=\"layui-icon\">&#xe60b;</i>" + button.Description + "</button>");
            }
            sb.AppendLine("             </div>");
            sb.AppendLine("         </script>");
            #endregion

            sb.AppendLine("         <table class=\"layui-hide\" id=\"currentTableId\" lay-filter=\"currentTableFilter\"></table>");
            sb.AppendLine("     </div>");
            sb.AppendLine(" </div>");

            #region js layui方法
            sb.AppendLine(" <script>");
            sb.AppendLine("     layui.use(['jquery', 'form',"+ (baseConfigModel.PageIndex.IsTree == 1 ? "'treeTable'" : "'table','commonTable'") + ", 'common','optimizeSelectOption'], function () {");
            sb.AppendLine("         var $ = layui.jquery,");
            sb.AppendLine("             form = layui.form,");
            sb.AppendLine("             " + (baseConfigModel.PageIndex.IsTree == 1 ? "treeTable = layui.treeTable," : "table = layui.table,commonTable = layui.commonTable"));
            sb.AppendLine("             common = layui.common;");
            sb.AppendLine("         var entity;");
            sb.AppendLine("         //权限控制(js是值传递)");
            sb.AppendLine("         toolbarDemo.innerHTML = common.authorizeButtonNew(toolbarDemo.innerHTML);");
            if (baseConfigModel.PageIndex.IsTree==1)
            {
                sb.AppendLine("         var queryJson;");
                sb.AppendLine("         var rendertree = common.rendertreetable({");
                sb.AppendLine("             elem: '#currentTableId',");
                sb.AppendLine("             treeIdName: '" + idColumn + "',");
                sb.AppendLine("             //此处需修改 父Id修改");
                if (baseConfigModel.PageIndex.IsSearch != 1)
                {
                    sb.AppendLine("             search:false,");
                }
                sb.AppendLine("             url: '/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetTreeGridJson'+(!queryJson ? '' : '?keyword=' + queryJson),"); sb.AppendLine("             sqlkey: '"+ idColumn + "',//数据库主键");
                sb.AppendLine("             cols: [[");
                sb.AppendLine("                 //此处需修改");
                int cout = 1;
                foreach (var item in baseConfigModel.PageIndex.ColumnList)
                {
                    if (cout==1)
                    {
                        sb.AppendLine("                 { field: '" + item.Key + "', title: '" + item.Value + "', width: 200 },");
                    }
                    else if (1 < cout && cout < baseConfigModel.PageIndex.ColumnList.Count)
                    {
                        sb.AppendLine("                 { field: '" + item.Key + "', title: '" + item.Value + "', width: 120 },");
                    }
                    else
                    {
                        sb.AppendLine("                 { field: '" + item.Key + "', title: '" + item.Value + "', minWidth: 120 },");
                    }
                    cout++;

                }
                sb.AppendLine("             ]]");
                sb.AppendLine("         });");
                sb.AppendLine("         // 监听搜索操作");
                sb.AppendLine("         form.on('submit(data-search-btn)', function (data) {");
                sb.AppendLine("             var queryJson = data.field.txt_keyword;");
                sb.AppendLine("             //执行搜索重载");
                sb.AppendLine("             common.reloadtreetable(rendertree, {");
                sb.AppendLine("                 where: { keyword: queryJson },");
                sb.AppendLine("             }); ");
                sb.AppendLine("             entity = null;");
                sb.AppendLine("             return false;");
                sb.AppendLine("         });");
            }
            else
            {
                sb.AppendLine("         commonTable.rendertable({");
                sb.AppendLine("             elem: '#currentTableId',");
                sb.AppendLine("             id: 'currentTableId',");
                sb.AppendLine("             url: '/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetGridJson',");
                if (baseConfigModel.PageIndex.IsPagination != 1)
                {
                    sb.AppendLine("             page: false,");
                }
                if (baseConfigModel.PageIndex.IsSearch != 1)
                {
                    sb.AppendLine("             search:false,");
                }
                sb.AppendLine("             cols: [[");
                sb.AppendLine("                 //此处需修改");
                int cout = 1;
                foreach (var item in baseConfigModel.PageIndex.ColumnList)
                {
                    if (cout != baseConfigModel.PageIndex.ColumnList.Count)
                    {
                        sb.AppendLine("                 { field: '" + item.Key + "', title: '" + item.Value + "', width: 120, sort: true },");
                    }
                    else
                    {
                        sb.AppendLine("                 { field: '" + item.Key + "', title: '" + item.Value + "', minWidth: 120, sort: true },");
                    }
                    cout++;
                }
                sb.AppendLine("             ]]");
                sb.AppendLine("         });");
                sb.AppendLine("         // 监听搜索操作");
                sb.AppendLine("         form.on('submit(data-search-btn)', function (data) {");
                sb.AppendLine("             //执行搜索重载");
                sb.AppendLine("             commonTable.reloadtable({");
                if (baseConfigModel.PageIndex.IsPagination != 1)
                {
                    sb.AppendLine("                 page: false,");
                }
                sb.AppendLine("                 elem: 'currentTableId',");
                sb.AppendLine("                 curr: 1,");
                sb.AppendLine("                 where: { keyword: data.field.txt_keyword}");
                sb.AppendLine("             });");
                sb.AppendLine("             entity = null;");
                sb.AppendLine("             return false;");
                sb.AppendLine("         });");
            }
            sb.AppendLine("         wcLoading.close();");
            if (baseConfigModel.PageIndex.IsTree == 1)
            {
                sb.AppendLine("         treeTable.on('row(currentTableId)', function (obj) {");
            }
            else
            {
                sb.AppendLine("         table.on('row(currentTableFilter)', function (obj) {");
            }
            sb.AppendLine("             obj.tr.addClass(\"layui-table-click\").siblings().removeClass(\"layui-table-click\");");
            sb.AppendLine("             entity = obj;");
            sb.AppendLine("         })");
            sb.AppendLine("         //toolbar监听事件");
            if (baseConfigModel.PageIndex.IsTree == 1)
            {
                sb.AppendLine("         treeTable.on('toolbar(currentTableId)', function (obj) { ");
            }
            else
            {
                sb.AppendLine("         table.on('toolbar(currentTableFilter)', function (obj) { ");
            }
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
            sb.AppendLine("                     param: { keyValue: entity.data." + idColumn + " },");
            sb.AppendLine("                     success: function () {");
            sb.AppendLine("                         common.reload('data-search-btn');");
            sb.AppendLine("                         entity = null;");
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
            sb.AppendLine("                   url: \"/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/Form?keyValue=\" + entity.data." + idColumn + ",");
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
            sb.AppendLine("                   url: \"/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/Details?keyValue=\" + entity.data." + idColumn + ",");
            sb.AppendLine("                   width: \"500px\",");
            sb.AppendLine("                   height: \"500px\",");
            sb.AppendLine( "                  btn: []");
            sb.AppendLine( "               });");
            sb.AppendLine( "           }");
            if (baseConfigModel.PageIndex.IsSearch == 1)
            {
                sb.AppendLine("           else if (obj.event === 'TABLE_SEARCH') {");
                sb.AppendLine("                var _that = $(\"#searchField\");");
                sb.AppendLine("                if (_that.hasClass(\"layui-hide\")) {");
                sb.AppendLine("                   _that.removeClass('layui-hide');");
                sb.AppendLine("                } else {");
                sb.AppendLine("                   _that.addClass('layui-hide');");
                sb.AppendLine("                }");
                sb.AppendLine("           }");
            }
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
                baseConfigModel.PageForm.FieldList = new Dictionary<string, string>();
            }
            #endregion
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Form\"; ");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Form.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("<script>");
            sb.AppendLine("    layui.use(['jquery', 'form', 'laydate', 'common','optimizeSelectOption'], function () {");
            sb.AppendLine("        var form = layui.form,");
            sb.AppendLine("            $ = layui.$,");
            sb.AppendLine("            common = layui.common,");
            sb.AppendLine("            laydate = layui.laydate;");
            sb.AppendLine("        var keyValue = $.request(\"keyValue\");");
            sb.AppendLine("        //权限字段");
            sb.AppendLine("        common.authorizeFields('adminform');");
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
            sb.AppendLine("                 common.ajax({");
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
            sb.AppendLine("       wcLoading.close();");
            sb.AppendLine("");
            sb.AppendLine("       function initControl() {");
            sb.AppendLine("           //此处需修改");
            sb.AppendLine("           //绑定数据源");
            sb.AppendLine("           //类型为下拉框时");
            sb.AppendLine("       }");
            sb.AppendLine("");
            sb.AppendLine("       //监听提交");
            sb.AppendLine("       form.on('submit(saveBtn)', function (data) {");
            sb.AppendLine("           // 单击之后提交按钮不可选,防止重复提交");
            sb.AppendLine("           $('.site-demo-active').addClass('layui-btn-disabled');");
            sb.AppendLine("           $('.site-demo-active').attr('disabled', 'disabled');");                                       
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
            sb.AppendLine("    <div class=\"layuimini-container\">");
            sb.AppendLine("        <div class=\"layuimini-main\">");
            sb.AppendLine("            <div class=\"layui-form layuimini-form\" lay-filter=\"adminform\">");
            #region 表单控件
            if (baseConfigModel.PageForm.FieldList.Count > 0)
            {
                switch (baseConfigModel.PageForm.ShowMode)
                {
                    case 1:
                        foreach (var item in baseConfigModel.PageForm.FieldList)
                        {
                            sb.AppendLine("                <div class=\"layui-form-item layui-hide\">");
                            sb.AppendLine( "                   <label class=\"layui-form-label required\">"+ item.Value + "</label>");
                            sb.AppendLine( "                   <div class=\"layui-input-block\">");
                            sb.AppendLine("                        <input type=\"text\" id=\"" + item.Key + "\" name=\"" + item.Key + "\" autocomplete=\"off\" lay-verify=\"required\" placeholder=\"请输入\" class=\"layui-input\">");
                            sb.AppendLine( "                   </div>");
                            sb.AppendLine( "               </div>");
                        }
                        break;

                    case 2:
                        int i = 1;
                        foreach (var item in baseConfigModel.PageForm.FieldList)
                        {
                            if (i % 2 != 0)
                            {
                                sb.AppendLine("                       <div class=\"layui-form-item\">");
                            }
                            i++;
                            sb.AppendLine("                    <div class=\"layui-inline layui-hide\">");
                            sb.AppendLine("                        <label class=\"layui-form-label required\">" + item.Value + "</label>");
                            sb.AppendLine("                        <div class=\"layui-input-inline\">");
                            sb.AppendLine("                            <input type=\"text\" id=\"" + item.Key + "\" name=\"" + item.Key + "\" autocomplete=\"off\" lay-verify=\"required\" placeholder=\"请输入\" class=\"layui-input\">");
                            sb.AppendLine("                        </div>");
                            sb.AppendLine("                    </div>");
                            if (i % 2 == 1)
                            {
                                sb.AppendLine("                       </div>");
                            }
                            else if (i == baseConfigModel.PageForm.FieldList.Count)
                            {
                                sb.AppendLine("                       </div>");
                            }
                        }
                        break;
                }
            }
            #endregion
            sb.AppendLine("                <div class=\"layui-form-item layui-hide\">");
            sb.AppendLine("                    <button class=\"layui-btn site-demo-active\" lay-submit id=\"submit\" lay-filter=\"saveBtn\">确认保存</button>");
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
                baseConfigModel.PageForm.FieldList =new Dictionary<string, string>();
            }
            #endregion
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewBag.Title = \"Details\"; ");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Form.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("<script>");
            sb.AppendLine("    layui.use(['jquery', 'form', 'laydate', 'common'], function () {");
            sb.AppendLine("        var form = layui.form,");
            sb.AppendLine("            $ = layui.$,");
            sb.AppendLine("            common = layui.common,");
            sb.AppendLine("            laydate = layui.laydate;");
            sb.AppendLine("        var keyValue = $.request(\"keyValue\");");
            sb.AppendLine("        //权限字段");
            sb.AppendLine("        common.authorizeFields('adminform');");
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
            sb.AppendLine("            common.ajax({");
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
            sb.AppendLine("       wcLoading.close();");
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
            sb.AppendLine("    <div class=\"layuimini-container\">");
            sb.AppendLine("        <div class=\"layuimini-main\">");
            sb.AppendLine("            <div class=\"layui-form layuimini-form\" lay-filter=\"adminform\">");
            #region 表单控件
            if (baseConfigModel.PageForm.FieldList.Count > 0)
            {
                switch (baseConfigModel.PageForm.ShowMode)
                {
                    case 1:
                        foreach (var item in baseConfigModel.PageForm.FieldList)
                        {
                            sb.AppendLine("                <div class=\"layui-form-item layui-hide\">");
                            sb.AppendLine("                   <label class=\"layui-form-label required\">" + item.Value + "</label>");
                            sb.AppendLine("                   <div class=\"layui-input-block\">");
                            sb.AppendLine("                        <input type=\"text\" id=\"" + item.Key + "\" name=\"" + item.Key + "\" lay-verify=\"required\" class=\"layui-input\">");
                            sb.AppendLine("                   </div>");
                            sb.AppendLine("               </div>");
                        }
                        break;

                    case 2:
                        int i = 1;
                        foreach (var item in baseConfigModel.PageForm.FieldList)
                        {
                            if (i % 2 != 0)
                            {
                                sb.AppendLine("                       <div class=\"layui-form-item\">");
                            }
                            i++;
                            sb.AppendLine("                    <div class=\"layui-inline layui-hide\">");
                            sb.AppendLine("                        <label class=\"layui-form-label required\">" + item.Value + "</label>");
                            sb.AppendLine("                        <div class=\"layui-input-inline\">");
                            sb.AppendLine("                            <input type=\"text\" id=\"" + item.Key + "\" name=\"" + item.Key + "\" lay-verify=\"required\" class=\"layui-input\">");
                            sb.AppendLine("                        </div>");
                            sb.AppendLine("                    </div>");
                            if (i % 2 == 1)
                            {
                                sb.AppendLine("                       </div>");
                            }
                            else if (i == baseConfigModel.PageForm.FieldList.Count)
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
        public string BuildMenu(BaseConfigModel baseConfigModel, string idColumn = "F_Id")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("  菜单路径:/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageIndexName);
            sb.AppendLine();
            sb.AppendLine("  主键："+ idColumn);
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
        public async Task<List<KeyValue>> CreateCode(BaseConfigModel baseConfigModel, string code)
        {
            List<KeyValue> result = new List<KeyValue>();
            JObject param = code.ToJObject();

            #region 集中判断
            string codeIndex = "";
            string indexPath = "";
            if (!param["CodeIndex"].IsEmpty())
            {
                codeIndex = HttpUtility.HtmlDecode(param["CodeIndex"].ToString());
                indexPath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageIndexName + ".cshtml");
                if (File.Exists(indexPath))
                {
                    throw new Exception("列表页已存在，列表页生成失败！");
                }
            }
            string codeForm = "";
            string formPath = "";
            if (!param["CodeForm"].IsEmpty())
            {
                codeForm = HttpUtility.HtmlDecode(param["CodeForm"].ToString());
                formPath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageFormName + ".cshtml");
                if (File.Exists(formPath))
                {
                    throw new Exception("表单页存在，表单页生成失败！");
                }
            }
            string codeDetails = "";
            string detailsPath = "";
            if (!param["CodeDetails"].IsEmpty())
            {
                codeDetails = HttpUtility.HtmlDecode(param["CodeDetails"].ToString());
                detailsPath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageDetailsName + ".cshtml");
                if (File.Exists(detailsPath))
                {
                    throw new Exception("详情页存在，详情页生成失败！");
                }
            }
            string codeEntity = "";
            string entityPath = "";
            if (!string.IsNullOrEmpty(param["CodeEntity"].ParseToString()))
            {
                codeEntity = HttpUtility.HtmlDecode(param["CodeEntity"].ToString());
                entityPath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityName + ".cs");
                if (File.Exists(entityPath))
                {
                    throw new Exception("实体类已存在，实体类生成失败！");

                }
            }
            string codeService = "";
            string servicePath = "";
            if (!param["CodeService"].IsEmpty())
            {
                codeService = HttpUtility.HtmlDecode(param["CodeService"].ToString());
                servicePath = Path.Combine(baseConfigModel.OutputConfig.OutputService, baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.ServiceName + ".cs");
                if (File.Exists(servicePath))
                {
                    throw new Exception("服务类已存在，服务类生成失败！");
                }
            }
            string codeController = "";
            string controllerPath = "";
            if (!param["CodeController"].IsEmpty())
            {
                codeController = HttpUtility.HtmlDecode(param["CodeController"].ToString());
                controllerPath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Controllers", baseConfigModel.FileConfig.ControllerName + ".cs");
                if (File.Exists(controllerPath))
                {
                    throw new Exception("控制器已存在，控制器生成失败！");
                }
            }
            #endregion

            #region 列表页
            if (!param["CodeIndex"].IsEmpty())
            {
                // 生成菜单，按钮
                List<KeyValue> buttonAuthorizeList = GetButtonAuthorizeList();
                string menuUrl = "/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageIndexName;
                ModuleEntity moduleEntity = new ModuleEntity();
                moduleEntity.Create();
                moduleEntity.F_Layers = (await uniwork.FindEntity<ModuleEntity>(a => a.F_EnCode == baseConfigModel.OutputConfig.OutputModule)).F_Layers + 1; ;
                moduleEntity.F_FullName = baseConfigModel.FileConfig.ClassDescription;
                moduleEntity.F_UrlAddress = menuUrl;
                moduleEntity.F_EnCode = baseConfigModel.FileConfig.ClassPrefix;
                moduleEntity.F_IsExpand = false;
                moduleEntity.F_IsMenu = baseConfigModel.PageIndex.IsMunu == 1 ? true : false;
                moduleEntity.F_IsFields = baseConfigModel.PageIndex.IsFields == 1 ? true : false;
                moduleEntity.F_IsPublic = baseConfigModel.PageIndex.IsPublic == 1 ? true : false;
                moduleEntity.F_Target = "iframe";
                moduleEntity.F_AllowEdit = false;
                moduleEntity.F_AllowDelete = false;
                moduleEntity.F_EnabledMark = true;
                moduleEntity.F_DeleteMark = false;
                moduleEntity.F_ParentId = (await uniwork.FindEntity<ModuleEntity>(a => a.F_EnCode == baseConfigModel.OutputConfig.OutputModule)).F_Id;
                var parentModule = await uniwork.FindEntity<ModuleEntity>(a => a.F_EnCode == baseConfigModel.OutputConfig.OutputModule);
                moduleEntity.F_SortCode = (uniwork.IQueryable<ModuleEntity>(a => a.F_ParentId == parentModule.F_Id).Max(a => a.F_SortCode) ?? 0) + 1;
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
                    modulebutton.F_Location = button.Key == "add" ? 1 : 2;
                    modulebutton.F_SortCode = sort;
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
                List<ModuleFieldsEntity> moduleFieldsList = new List<ModuleFieldsEntity>();
                foreach (var item in baseConfigModel.PageIndex.ColumnList)
                {
                    ModuleFieldsEntity moduleFields = new ModuleFieldsEntity();
                    moduleFields.Create();
                    moduleFields.F_ModuleId = moduleEntity.F_Id;
                    moduleFields.F_EnCode = item.Key;
                    moduleFields.F_FullName = item.Value;
                    moduleFields.F_IsPublic = true;
                    moduleFields.F_EnabledMark = true;
                    moduleFields.F_DeleteMark = false;
                    moduleFieldsList.Add(moduleFields);
                }
                uniwork.BeginTrans();
                await uniwork.Insert(moduleEntity);
                await uniwork.Insert(moduleButtonList);
                if (moduleFieldsList.Count > 0)
                {
                    await uniwork.Insert(moduleFieldsList);
                }
                uniwork.Commit();
                await CacheHelper.Remove(fieldscacheKey + "list");
                await CacheHelper.Remove(buttoncacheKey + "list");
                await CacheHelper.Remove(cacheKey + "list");
                await CacheHelper.Remove(quickcacheKey + "list");
                await CacheHelper.Remove(initcacheKey + "list");
                await CacheHelper.Remove(initcacheKey + "modulebutton_list");
                await CacheHelper.Remove(initcacheKey + "modulefields_list");
                await CacheHelper.Remove(authorizecacheKey + "list");
                FileHelper.CreateFile(indexPath, codeIndex);
                result.Add(new KeyValue { Key = "列表页", Value = indexPath, Description = "生成成功！" });
            }
            #endregion

            #region 表单页
            if (!param["CodeForm"].IsEmpty())
            {
                FileHelper.CreateFile(formPath, codeForm);
                result.Add(new KeyValue { Key = "表单页", Value = formPath, Description = "生成成功！" });
            }
            #endregion

            #region 查看页
            if (!param["CodeDetails"].IsEmpty())
            {
                FileHelper.CreateFile(detailsPath, codeDetails);
                result.Add(new KeyValue { Key = "详情页", Value = detailsPath, Description = "生成成功！" });
            }
            #endregion

            #region 实体类
            if (!string.IsNullOrEmpty(param["CodeEntity"].ParseToString()))
            {
                FileHelper.CreateFile(entityPath, codeEntity);
                result.Add(new KeyValue { Key = "实体类", Value = entityPath, Description = "生成成功！" });
            }
            #endregion

            #region 服务类
            if (!param["CodeService"].IsEmpty())
            {
                FileHelper.CreateFile(servicePath, codeService);
                result.Add(new KeyValue { Key = "服务类", Value = servicePath, Description = "生成成功！" });
            }
            #endregion

            #region 控制器
            if (!param["CodeController"].IsEmpty())
            {
                FileHelper.CreateFile(controllerPath, codeController);
                result.Add(new KeyValue { Key = "控制器", Value = controllerPath, Description = "生成成功！" });
            }
            #endregion

            return result;
        }
        #endregion
        #region EntityCreateCode
        public async Task EntityCreateCode(BaseConfigModel baseConfigModel, string code)
        {
            string codeEntity = "";
            string entityPath = "";
            if (!string.IsNullOrEmpty(code))
            {
                codeEntity = code;
                entityPath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityName + ".cs");
                if (File.Exists(entityPath))
                {
                    throw new Exception("实体类已存在，实体类生成失败！");

                }
                FileHelper.CreateFile(entityPath, codeEntity);
            }
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

        private string GetBaseEntity(string EntityName, DataTable dt,string idColumn="F_Id",string deleteMarkField="IsDelete",string AddTime="AddTime")
        {
            string entity = string.Empty;
            var columnList = dt.AsEnumerable().Select(p => p["TableColumn"].ParseToString()).ToList();

            bool id = columnList.Where(p => p == idColumn).Any();
            bool baseIsDelete = columnList.Where(p => p == "F_DeleteUserId").Any()&& columnList.Where(p => p == "F_DeleteTime").Any()&& columnList.Where(p => p == "F_DeleteMark").Any();
            bool baseIsCreate = columnList.Where(p => p == "F_Id").Any() && columnList.Where(p => p == "F_CreatorUserId").Any() && columnList.Where(p => p == "F_CreatorTime").Any();
            bool baseIsModifie = columnList.Where(p => p == "F_Id").Any() && columnList.Where(p => p == "F_LastModifyUserId").Any() && columnList.Where(p => p == "F_LastModifyTime").Any();
            if (!id)
            {
                throw new Exception("数据库表必须有主键id字段");
            }
            if (idColumn!= "F_Id")
            {
                return null;
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
