using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomHost;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;
using System.CodeDom.Compiler;
using System.Windows.Forms;
using System.Configuration;


namespace EntityInfo
{
    class CreateCode
    {
        public static string CreatT4Class(EntityClassInfo classInfo,string path,string strModuleDic)
        {
            string templatePath = string.Empty;
            try
            {
                templatePath = System.Configuration.ConfigurationManager.AppSettings["TemplateData"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取配置文件错误！TemplateEntity" + ex.Message);
                return null;
            }
            var list = templatePath.Split(',');
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (!File.Exists(item))
                    {
                        MessageBox.Show("未找到" + item + "，请修改配置文件！");
                        return null;
                    }
                    CustomTextTemplatingEngineHost host = new CustomTextTemplatingEngineHost();
                    host.TemplateFileValue = item;
                    string input = File.ReadAllText(item);
                    host.Session = new TextTemplatingSession();
                    host.Session.Add("entity", classInfo);
                    string output = new Engine().ProcessTemplate(input, host);
                    StringBuilder errorWarn = new StringBuilder();
                    foreach (CompilerError error in host.Errors)
                    {
                        errorWarn.Append(error.Line).Append(":").AppendLine(error.ErrorText);
                    }
                    if (!File.Exists("Error.log"))
                    {
                        File.Create("Error.log");
                    }
                    File.WriteAllText("Error.log", errorWarn.ToString());
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (item.IndexOf("WaterCloudApp") > 0)
                        {
                            File.WriteAllText(path + classInfo.ClassName.Replace("_", "") + "App" + ".cs",
                                output);
                        }
                        if (item.IndexOf("WaterCloudEntity") > 0)
                        {
                            File.WriteAllText(path + classInfo.ClassName.Replace("_", "") + "Entity" + ".cs",
                                output);
                        }
                        if (item.IndexOf("WaterCloudIRepository") > 0)
                        {
                            File.WriteAllText(path + "I" + classInfo.ClassName.Replace("_", "") + "Repository" + ".cs",
                                output);
                        }
                        if (item.IndexOf("WaterCloudMap") > 0)
                        {
                            File.WriteAllText(path + classInfo.ClassName.Replace("_", "") + "Map" + ".cs",
                                output);
                        }
                        if (item.IndexOf("WaterCloudRepository") > 0)
                        {
                            File.WriteAllText(path + classInfo.ClassName.Replace("_", "") + "Repository" + ".cs",
                                output);
                        }
                    }
                    //return output;
                }

                MessageBox.Show("生成成功！");
            }
            return null;
        }
        public static string CreateEntityClass(EntityClassInfo classInfo)
        {
            string templatePath = string.Empty;
            try
            {
                templatePath = System.Configuration.ConfigurationManager.AppSettings["TemplateEntity"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取配置文件错误！TemplateEntity" + ex.Message);
                return null;
            }
            var ss= Directory.GetCurrentDirectory();
            if (!File.Exists(templatePath))
            {
                MessageBox.Show("未找到Entity.tt，请修改配置文件！");
                return null;
            }
            CustomTextTemplatingEngineHost host = new CustomTextTemplatingEngineHost();
            host.TemplateFileValue = templatePath;
            string input = File.ReadAllText(templatePath);
            host.Session = new TextTemplatingSession();
            host.Session.Add("entity", classInfo);
            
            string output = new Engine().ProcessTemplate(input, host);

            StringBuilder errorWarn = new StringBuilder();
            foreach (CompilerError error in host.Errors)
            {
                errorWarn.Append(error.Line).Append(":").AppendLine(error.ErrorText);
            }
            if (!File.Exists("Error.log"))
            {
                File.Create("Error.log");
            }
            File.WriteAllText("Error.log", errorWarn.ToString());
            return output;
        }

        public static string CreateDataAccessClass(EntityClassInfo classInfo)
        {
            string templatePath = string.Empty;
            try
            {
                templatePath = System.Configuration.ConfigurationManager.AppSettings["TemplateDataAccess"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取配置文件错误！TemplateDataAccess" + ex.Message);
                return null;
            }
            if (!File.Exists(templatePath))
            {
                MessageBox.Show("未找到DataAccess.tt，请修改配置文件！");
                return null;
            }
            CustomTextTemplatingEngineHost host = new CustomTextTemplatingEngineHost();
            host.TemplateFileValue = templatePath;
            string input = File.ReadAllText(templatePath);
            host.Session = new TextTemplatingSession();
            host.Session.Add("entity", classInfo);

            string output = new Engine().ProcessTemplate(input, host);

            StringBuilder errorWarn = new StringBuilder();
            foreach (CompilerError error in host.Errors)
            {
                errorWarn.Append(error.Line).Append(":").AppendLine(error.ErrorText);
            }
            if (!File.Exists("Error.log"))
            {
                File.Create("Error.log");
            }
            File.WriteAllText("Error.log", errorWarn.ToString());
            
            return output;
        }
    }
}
