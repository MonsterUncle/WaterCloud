using System.Linq;
using Newtonsoft.Json.Linq;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;

namespace WaterCloud.Service.CommonService
{
    public class FormUtil {
	
        public static string GetHtml(string contentData, string contentParse,string frmData, string action)
        {
            if (string.IsNullOrEmpty(contentData))
            {
                return string.Empty;
            }
            JObject tableData = null;//表单数据
            if (!string.IsNullOrEmpty(frmData))
            {
               tableData = JsonHelper.ToObject<JObject>(frmData);
            }

            string html = contentParse;
            foreach (var json in contentData.ToList<JObject>())
            {
                string name = "";
                string leipiplugins = json.GetValue("leipiplugins").ToString();
                if ("checkboxs" == leipiplugins)
                    name = json.GetValue("parse_name").ToString();
                else
                    name = json.GetValue("name").ToString();

                string tempHtml = "";
                switch (leipiplugins)
                {
                    case "text":
                        tempHtml = GetTextBox(json, tableData, action);
                        break;
                    case "textarea":
                        tempHtml = GetTextArea(json, tableData, action);
                        break;
                    case "radios":
                        tempHtml = GetRadios(json, tableData, action);
                        break;
                    case "select":
                        tempHtml = GetSelect(json, tableData, action);
                        break;
                    case "checkboxs":
                        tempHtml = GetCheckboxs(json, tableData, action);
                        break;

                    case "qrcode"://二维码
                        tempHtml = GetQrcode(json, tableData, action);
                        break;

                    case "progressbar"://进度条 (未做处理)
                        /*temp_html = GetProgressbar(json, tableData, action);*/
                        break;
                    default:
                        tempHtml = json.GetValue("content").ToString();
                        break;
                }

                html = html.Replace("{" + name + "}", tempHtml);
            }


            return html;
        }

        /// <summary>
        /// 只显示编辑框
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>System.String.</returns>
        public static string GetHtml(FormEntity form){
            if (form.F_FrmType != 0)  //只有开原版动态表单才需要转换
            {
                return string.Empty;
            }
		
            return GetHtml(form.F_ContentData, form.F_ContentParse, null,  "");

        }

        /// <summary>
        /// 显示编辑框和里面的用户数据
        /// </summary>
        /// <param name="contentdata">The contentdata.</param>
        /// <param name="contentParse">The content parse.</param>
        /// <param name="frmData">The FRM data.</param>
        /// <returns>System.String.</returns>
        //public static string Preview(FlowInstance flowInstance)
        //{
        //    if (flowInstance.FrmType != 0)  //只有开原版动态表单才需要转换
        //    {
        //        return string.Empty;
        //    }
            
        //    return GetHtml(flowInstance.FrmContentData, flowInstance.FrmContentParse, 
        //        flowInstance.FrmData, "view");
        //}

        //text
        private static string GetTextBox(JObject item, JObject formData,string action)
        {
            string temp = "<input type=\"text\" value=\"{0}\"  name=\"{1}\"  style=\"{2}\"/>";
            string name = item.GetValue("name").ToString();

            string value =null;
            JToken data;
            if (formData != null && (data = formData.GetValue(name)) != null)
            {
                value = data.ToString();
            }

            if (value == null)
                value = item.GetValue("value") == null ? "" : item.GetValue("value").ToString();
            string style =item.GetValue("style") == null ? "" : item.GetValue("style").ToString();
            string tempHtml =  string.Format(temp, value, name, style);
            if("view"==action)
                return string.Format("<label style=\"{0}\">{1}</label>",style,value);
            return tempHtml;
        }
	
        //TextArea
        private static string GetTextArea(JObject item, JObject formData,string action)
        {
            string script = "";
            if (item.GetValue("orgrich") != null && "1"==item.GetValue("orgrich").ToString())
                script = "orgrich=\"true\" ";
            string name = item.GetValue("name").ToString();

            string value = null;
            JToken data;
            if (formData != null && (data = formData.GetValue(name)) != null)
            {
                value = data.ToString();
            }

            if (value == null)
                value = item.GetValue("value")== null ? "" : item.GetValue("value").ToString();
            string style = item.GetValue("style") == null ? "" : item.GetValue("style").ToString();


            string temp = "<textarea  name=\"{0}\" id=\"{1}\"  style=\"{2}\" {3}>{4}</textarea>";
        
            string temp_html = string.Format(temp, name, name, style, script, value);
        
            if("view"==action)
                return string.Format("<label style=\"{0}\">{1}</label>", style, value);
            return temp_html;
        }
	
        //Radios
        private static string GetRadios(JObject item, JObject formData,string action)
        {
            var radiosOptions = JArray.Parse(item.GetValue("options").ToString());
            //JArray radiosOptions = item["options"] as JArray;
            string temp = "<input type=\"radio\" name=\"{0}\" value=\"{1}\"  {2}>{3}&nbsp;";
            string temp_html = "";
            string name = item.GetValue("name").ToString();

            string value = null;
            JToken data;
            if (formData != null && (data = formData.GetValue(name)) != null)
            {
                value = data.ToString();
            }
            
            foreach (var json in radiosOptions)
            {
                string cvalue = json["value"].ToString();
                string Ischecked = "";

                if (value == null)
                {
                    string check = json["checked"] != null ? json["checked"].ToString() : "";
                    if ("checked" == check || "true" == check)
                    {
                        Ischecked = " checked=\"checked\" ";
                        value = json["value"].ToString();
                    }
                }

                temp_html += string.Format(temp, name, cvalue, Ischecked, cvalue);
            }
		
            return "view"==action ? string.Format("<label style=\"{0}\">{1}</label>", "", value) : temp_html;
        }
	
        //Checkboxs
        private static string GetCheckboxs(JObject item, JObject formData,string action){
            string temp_html = "";
            string temp = "<input type=\"checkbox\" name=\"{0}\" value=\"{1}\" {2}>{3}&nbsp;";
		
            string view_value="";//view 查看值
		
            var checkOptions = JArray.Parse(item.GetValue("options").ToString());
            foreach (var json in checkOptions)
            {
                string name = json["name"].ToString();

                string value = null;
                JToken data;
                if (formData != null && (data = formData.GetValue(name)) != null)
                {
                    value = data.ToString();
                }

                string cvalue = json["value"].ToString();
                string Ischecked = "";
                if (value == null)
                {
                    string check = json["checked"] != null ? json["checked"].ToString() : "";
                    if (check == "checked" || check == "true")
                    {
                        Ischecked = " checked=\"checked\" ";
                        view_value += cvalue + "&nbsp";//view 查看值
                    }
                }
                else if (value != null && value == cvalue)
                {
                    Ischecked = " checked=\"checked\" ";
                    view_value += cvalue + "&nbsp";//view 查看值
                }

                temp_html += string.Format(temp, name, cvalue, Ischecked, cvalue);

            }

            return "view" == action ? string.Format("<label style=\"{0}\">{1}</label>", "", view_value) : temp_html;
        }
	
        //Select(比较特殊)
        private static string GetSelect(JObject item, JObject formData, string action)
        {
            string name = item.GetValue("name").ToString();  //控件的名称
            string value = null;
            JToken data;

            if (formData != null && (data = formData.GetValue(name)) != null)
            {
                value = data.ToString();
            }

            string content =item.GetValue("content").ToString();
            content = content.Replace("leipiNewField", name);
            if (value != null)//用户设置过值
            {
                content = content.Replace("selected=\"selected\"", "");  //先去掉模板中的选中项
                var option = "value=\"" + value + "\"";        //组成选项
                string selected = option + " selected=\"selected\"";  //组成选中项
                content = content.Replace(option, selected);      //把选项替换成选中项
            }

            return "view" == action ? string.Format("<label style=\"{0}\">{1}</label>", "", value) : content;
        }
	
	
        //Qrcode 二维码
        private static string GetQrcode(JObject item, JObject formData, string action)
        {
            string name = item.GetValue("name").ToString();

            string value = null;
            JToken data;
            if (formData != null && (data = formData.GetValue(name)) != null)
            {
                value = data.ToString();
            }

            string temp_html = "";
            string temp = "";
            string orgType = item.GetValue("orgtype").ToString();
            string style = item.GetValue("style").ToString();
            if ("text"==orgType)
            {
                orgType = "文本";
            }
            else if ("url"==orgType)
            {
                orgType = "超链接";
            }
            else if ("tel"==orgType)
            {
                orgType = "电话";
            }
            string qrcode_value = "";
            if (item.GetValue("value")!= null)
                qrcode_value = item.GetValue("value").ToString();
            //print_R($qrcode_value);exit;  //array(value,qrcode_url)
            if ( "edit"==action)
            {
                temp = orgType + "二维码 <input type=\"text\" name=\"{0}\" value=\"{1}\"/>";
                temp_html =  string.Format(temp, name, value);
            }
            else if ("view"==action)
            {
                //可以采用  http://qrcode.leipi.org/ 

                style = "";
                if (item.GetValue("orgwidth") != null)
                {
                    style = "width:" + item.GetValue("orgwidth") + "px;";
                }
                if (item.GetValue("orgheight") != null)
                {
                    style += "height:" + item.GetValue("orgheight") + "px;";
                }
                temp = "<img src=\"{0}\" title=\"{1}\" style=\"{2}\"/>";
                temp_html = string.Format(temp_html, name, value, style);


            }
            else if ( "preview"==action)
            {
                style = "";
                if (item.GetValue("orgwidth")!= null)
                {
                    style = "width:" + item.GetValue("orgwidth") + "px;";
                }
                if (item.GetValue("orgheight")!= null)
                {
                    style += "height:" + item.GetValue("orgheight") + "px;";
                }
                temp = "<img src=\"{0}\" title=\"{1}\" style=\"{2}\"/>";
                temp_html = string.Format(temp_html, name, value, style);
            }

            return temp_html;
        }
    }
}
