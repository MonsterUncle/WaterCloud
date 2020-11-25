using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace WaterCloud.Code
{
    /// <summary>
    /// 配置文件辅助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        ///     更新配置信息，将配置信息对象序列化至相应的配置文件中，文件格式为带签名的UTF-8
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <param name="config">配置信息</param>
        public static void UpdateConfig<T>(T config)
        {
            Type configClassType = typeof(T);
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件  
            try
            {
                var xmlSerializer = new XmlSerializer(configClassType);
                using (var xmlTextWriter = new XmlTextWriter(configFilePath, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    var xmlNamespace = new XmlSerializerNamespaces();
                    xmlNamespace.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(xmlTextWriter, config, xmlNamespace);
                }
            }
            catch (SecurityException ex)
            {
                throw new SecurityException(ex.Message, ex.DenySetInstance, ex.PermitOnlySetInstance, ex.Method,
                                            ex.Demanded, ex.FirstPermissionThatFailed);
            }
        }


        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置信息</returns>
        public static T GetConfig<T>() where T : class, new()
        {
            var configObject = new object();
            Type configClassType = typeof(T);
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件  
            if (File.Exists(configFilePath))
            {
                using (var xmlTextReader = new XmlTextReader(configFilePath))
                {
                    var xmlSerializer = new XmlSerializer(configClassType);
                    configObject = xmlSerializer.Deserialize(xmlTextReader);
                }
            }
            var config = configObject as T;
            if (config == null)
            {
                return new T();
            }
            return config;
        }

        /// <summary>
        /// 获取配置文件的服务器物理文件路径
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置文件路径</returns>
        public static string GetConfigPath<T>()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (path == AppDomain.CurrentDomain.BaseDirectory)
            {
                path = path + typeof(T).Name + ".config";
            }
            return path;
        }


        public static string GetDirPath(string dirName)
        {
            string dirPath = dirName;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }

        public static bool GetConfigBool(string key)
        {
            bool flag1 = false;
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return flag1;
            }
            try
            {
                return bool.Parse(text1);
            }
            catch (FormatException)
            {
                return flag1;
            }
        }
        public static decimal GetConfigDecimal(string key)
        {
            decimal num1 = new decimal(0);
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return num1;
            }
            try
            {
                return decimal.Parse(text1);
            }
            catch (FormatException)
            {
                return num1;
            }
        }
        public static int GetConfigInt(string key)
        {
            int num1 = 0;
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return num1;
            }
            try
            {
                return int.Parse(text1);
            }
            catch (FormatException)
            {
                return num1;
            }
        }
        public static string GetConfigString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string Md5(string input)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(data).Replace("-", string.Empty).ToLower(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        ///     读取配置文件
        /// </summary>
        /// <returns></returns>
        public static string ReadConfig<T>()
        {
            string configContent = string.Empty;
            string filePath = GetConfigPath<T>();
            if (File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath, Encoding.Default))
                {
                    configContent = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return configContent;
        }

        /// <summary>
        ///     写入配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static void WriteConfig<T>(string config)
        {
            string fileName = GetConfigPath<T>();
            using (StreamWriter w = File.AppendText(fileName))
            {
                w.WriteLine(config);
                w.Close();
            }
        }

        /// <summary>
        ///  读取appStrings配置节， 返回＊.exe.config文件中appSettings配置节的value项
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string GetAppConfig(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }


        /// <summary>
        /// 更新appStrings配置节,在＊.exe.config文件中appSettings配置节增加一对键、值对
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="newValue"></param>
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == newKey)
                {
                    isModified = true;
                }
            }

            // Open App.Config of executable
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // You need to remove the old settings object before you can replace it
            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            // Add an Application Setting.
            config.AppSettings.Settings.Add(newKey, newValue);
            // Save the changes in App.config file.
            config.Save(ConfigurationSaveMode.Modified);
            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
        }



        /// <summary>
        /// 读取connectionStrings配置节,依据连接串名字connectionName返回数据连接字符串
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static string GetConnectionStringsConfig(string connectionName)
        {
            string connectionString =
                    ConfigurationManager.ConnectionStrings[connectionName].ConnectionString.ToString();
            Console.WriteLine(connectionString);
            return connectionString;
        }


        /// <summary>
        /// 更新connectionStrings配置节, 更新连接字符串
        /// </summary>
        /// <param name="newName"> 连接字符串名称 </param>
        /// <param name="newConString"> 连接字符串内容 </param>
        /// <param name="newProviderName"> 数据提供程序名称 </param>
        public static void UpdateConnectionStringsConfig(string newName,
            string newConString,
            string newProviderName)
        {
            bool isModified = false;    // 记录该连接串是否已经存在
            // 如果要更改的连接串已经存在
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            // 新建一个连接字符串实例
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);
            // 打开可执行的配置文件*.exe.config
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果连接串已存在，首先删除它
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中.
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }
    }
}