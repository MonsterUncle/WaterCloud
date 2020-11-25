using Microsoft.Win32;
using System;
using System.Configuration;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Globalization;
using System.Web.Security;

namespace WaterCloud.Code
{
    /// <summary>
    /// 服务端处理
    /// </summary>
    public class Server
    {

        /// <summary>
        /// 获取服务器域名地址
        /// </summary>
        public static string DoMain
        {
            get
            {
                string result;
                if (HttpContext.Current != null)
                {
                    result = HttpContext.Current.Request.Url.Host.ToString();
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        /// <summary>
        /// 获取当前.NET运行版本
        /// </summary>
        public static string DotNetVersion
        {
            get
            {
                string text = Environment.Version.ToString();
                string result;
                if (text.IndexOf('.') < 0)
                {
                    result = text;
                }
                else
                {
                    text = text.Substring(0, 3);
                    result = text;
                }
                return result;
            }
        }

        /// <summary>
        /// 获取硬盘ID
        /// </summary>
        public static string HardDiskID
        {
            get
            {
                string result;
                try
                {
                    ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                    string text = null;
                    using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            ManagementObject managementObject = (ManagementObject)enumerator.Current;
                            text = managementObject["SerialNumber"].ToString().Trim();
                        }
                    }
                    result = text;
                }
                catch
                {
                    result = "";
                }
                return result;
            }
        }

        /// <summary>
        /// 获取主机名称
        /// </summary>
        public static string MainName
        {
            get
            {
                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
                return connectionStringSettings.Name;
            }
        }
        
        /// <summary>
        /// 获取操作系统版本
        /// </summary>
        public static string OS
        {
            get
            {
                OperatingSystem oSVersion = Environment.OSVersion;
                string result;
                switch (oSVersion.Platform)
                {
                    case PlatformID.Win32Windows:
                        {
                            int minor = oSVersion.Version.Minor;
                            if (minor == 0)
                            {
                                result = "Win95 ";
                                return result;
                            }
                            if (minor != 10)
                            {
                                if (minor == 90)
                                {
                                    result = "WinMe ";
                                    return result;
                                }
                            }
                            else
                            {
                                if (oSVersion.Version.Revision.ToString() == "2222A ")
                                {
                                    result = "Win98r2";
                                    return result;
                                }
                                result = "Win98 ";
                                return result;
                            }
                            break;
                        }
                    case PlatformID.Win32NT:
                        switch (oSVersion.Version.Major)
                        {
                            case 3:
                                result = "WindNT 3.51 ";
                                return result;
                            case 4:
                                result = "WinNT 4.0 ";
                                return result;
                            case 5:
                                switch (oSVersion.Version.Minor)
                                {
                                    case 0:
                                        result = "Win2000";
                                        return result;
                                    case 1:
                                        result = "WinXP ";
                                        return result;
                                    case 2:
                                        result = "Win2003 ";
                                        return result;
                                }
                                break;
                            case 6:
                                switch (oSVersion.Version.Minor)
                                {
                                    case 0:
                                        result = "WinVista ";
                                        return result;
                                    case 1:
                                        result = "Win7 ";
                                        return result;
                                }
                                break;
                        }
                        break;
                }
                result = oSVersion.VersionString;
                return result;
            }
        }


        /// <summary>
        /// 获取服务器IP地址
        /// </summary>
        public static string IP
        {
            get
            {
                string result;
                if (Server.DoMain.Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = "127.0.0.1";
                }
                else
                {
                    HttpContext current = HttpContext.Current;
                    if (current != null)
                    {
                        result = HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
                    }
                    else
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                        if (hostEntry == null)
                        {
                            result = "";
                        }
                        else if (hostEntry.AddressList.Length < 1)
                        {
                            result = "";
                        }
                        else
                        {
                            try
                            {
                                IPAddress iPAddress = hostEntry.AddressList[1];
                                result = iPAddress.ToString().Trim();
                            }
                            catch
                            {
                                result = "";
                            }
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 获取服务器端口
        /// </summary>
        public static string Port
        {
            get
            {
                string result;
                if (HttpContext.Current != null)
                {
                    result = HttpContext.Current.Request.Url.Port.ToString();
                }
                else
                {
                    result = "80";
                }
                return result;
            }
        }

        /// <summary>
        /// 获取主路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            string result;
            if (string.IsNullOrWhiteSpace(path))
            {
                result = path;
            }
            else
            {
                string pattern = "^[a-zA-Z]:\\\\{1,2}.+(\\\\?)$";
                if (Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace))
                {
                    path = path.Replace("/", "\\");
                }
                else
                {
                    path = HostingEnvironment.MapPath(path);
                }
                if (path.IndexOf("\\") > -1)
                {
                    if (path.Substring(path.LastIndexOf("\\")).IndexOf(".") < 0 && path.Substring(path.Length - 1) != "\\")
                    {
                        path += "\\";
                    }
                }
                path = Regex.Replace(path, "\\\\+", "\\");
                result = path;
            }
            return result;
        }

        /// <summary>
        /// 参数路径
        /// </summary>
        public static string ProgramPath
        {
            get
            {
                return HostingEnvironment.ApplicationPhysicalPath;
            }
        }

        /// <summary>
        /// 物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string VirtualPath(string path)
        {
            path = path.Replace("\\", "/");
            path = path.Replace("~/", HostingEnvironment.ApplicationVirtualPath);
            if (path.IndexOf("/") > -1)
            {
                if (path.Substring(path.LastIndexOf("/")).IndexOf(".") < 0 && path.Substring(path.Length - 1) != "/")
                {
                    path += "/";
                }
            }
            path = Regex.Replace(path, "\\/+", "/");
            return path;
        }
    }

    /// <summary>
    /// 路径处理方法
    /// </summary>
    public class _Path
    {
        private string _path = "";

        private string _virtualPath = "";

        private string _value = "";

        public string Physics
        {
            get
            {
                return Server.MapPath(this._virtualPath);
            }
        }

        public string Virtual
        {
            get
            {
                return Server.VirtualPath(this._virtualPath);
            }
        }

        public string String
        {
            get
            {
                return this._value;
            }
        }

        public ulong Length
        {
            get
            {
                return this.getLength(this._path);
            }
        }

        public _Path(string root, string path)
        {
            path = (string.IsNullOrWhiteSpace(path) ? "" : path);
            this._value = path;
            if (root == null)
            {
                root = "/";
            }
            if (root.Length < 1)
            {
                root = "/";
            }
            string a = root.Substring(root.Length - 1, 1);
            if (a != "\\" && a != "/")
            {
                root += "/";
            }
            this._path = root + path;
            this._virtualPath = root + path;
            if (!Directory.Exists(this.Physics))
            {
                Directory.CreateDirectory(this.Physics);
            }
        }

        private ulong getLength(string path)
        {
            ulong result;
            if (!Directory.Exists(path))
            {
                result = 0uL;
            }
            else
            {
                ulong num = 0uL;
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FileInfo[] files = directoryInfo.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fileInfo = files[i];
                    num += (ulong)fileInfo.Length;
                }
                DirectoryInfo[] directories = directoryInfo.GetDirectories();
                for (int i = 0; i < directories.Length; i++)
                {
                    DirectoryInfo directoryInfo2 = directories[i];
                    num += this.getLength(directoryInfo2.FullName);
                }
                result = num;
            }
            return result;
        }

        public string GetSize(ulong size)
        {
            string result = "";
            if (size < 1024uL)
            {
                result = size.ToString() + " Bit";
            }
            else
            {
                double num = size / 1024uL;
                num = Math.Round(num * 100.0) / 100.0;
                if (num < 1024.0)
                {
                    result = num.ToString() + " Kb";
                }
                else
                {
                    double num2 = num / 1024.0;
                    num2 = Math.Round(num2 * 100.0) / 100.0;
                    if (num2 < 1024.0)
                    {
                        result = num2.ToString() + " Mb";
                    }
                }
            }
            return result;
        }

        public int DeleteFile(string filename)
        {
            string text = this.Physics + filename;
            string path = text;
            if (text.IndexOf(".") > -1)
            {
                string str = text.Substring(text.LastIndexOf("."));
                path = text.Substring(0, text.LastIndexOf(".")) + "_small" + str;
            }
            try
            {
                File.Delete(text);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 1;
        }
    }


    public class ConvertToAnyValue
    {
        private object _paravlue;

        private string _parakey = "";

        private string _unit = "";

        public string ParaValue
        {
            get
            {
                return this._paravlue.ToString();
            }
            set
            {
                this._paravlue = value;
            }
        }

        public string ParaKey
        {
            get
            {
                return this._parakey;
            }
            set
            {
                this._parakey = value;
            }
        }

        public string Unit
        {
            get
            {
                return this._unit;
            }
            set
            {
                this._unit = value;
            }
        }

        public int? Int16
        {
            get
            {
                int? result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    try
                    {
                        result = new int?((int)Convert.ToInt16(this._paravlue));
                    }
                    catch
                    {
                        result = null;
                    }
                }
                return result;
            }
        }

        public int? Int32
        {
            get
            {
                int? result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    try
                    {
                        result = new int?(Convert.ToInt32(this._paravlue));
                    }
                    catch
                    {
                        result = null;
                    }
                }
                return result;
            }
        }

        public long? Int64
        {
            get
            {
                long? result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    try
                    {
                        result = new long?(Convert.ToInt64(this._paravlue));
                    }
                    catch
                    {
                        result = null;
                    }
                }
                return result;
            }
        }

        public double? Double
        {
            get
            {
                double? result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    try
                    {
                        result = new double?(Convert.ToDouble(this._paravlue));
                    }
                    catch
                    {
                        result = null;
                    }
                }
                return result;
            }
        }

        public string String
        {
            get
            {
                return (this._paravlue == null) ? "" : this._paravlue.ToString().Trim();
            }
        }

        public bool? Boolean
        {
            get
            {
                bool? result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    try
                    {
                        result = new bool?(Convert.ToBoolean(this._paravlue));
                    }
                    catch
                    {
                        result = null;
                    }
                }
                return result;
            }
        }

        public DateTime? DateTime
        {
            get
            {
                DateTime? result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    try
                    {
                        result = new DateTime?(Convert.ToDateTime(this._paravlue));
                    }
                    catch
                    {
                        string text = this._paravlue.ToString();
                        string text2 = text;
                        for (int i = 0; i < text2.Length; i++)
                        {
                            char c = text2[i];
                            text += ((c >= '0' && c <= '9') ? c : '-');
                        }
                        string text3 = text;
                        Regex regex = new Regex("\\-{1,}", RegexOptions.IgnorePatternWhitespace);
                        text3 = regex.Replace(text3, "-");
                        regex = new Regex("^\\-{1,}(\\w)", RegexOptions.IgnorePatternWhitespace);
                        text3 = regex.Replace(text3, "$1");
                        regex = new Regex("(\\w)\\-{1,}$", RegexOptions.IgnorePatternWhitespace);
                        text3 = regex.Replace(text3, "$1");
                        if (text3.IndexOf('-') > -1)
                        {
                            string text4 = text3.Substring(0, text3.IndexOf('-'));
                            text4 = ((text4.Length == 1) ? ("0" + text4) : text4);
                            text4 = ((text4.Length == 2) ? ("19" + text4) : text4);
                            text4 = ((text4.Length > 4) ? text4.Substring(0, 4) : text4);
                            text3 = text4 + "-" + text3.Substring(text3.IndexOf('-') + 1);
                        }
                        else
                        {
                            text3 += "-1-1";
                        }
                        try
                        {
                            result = new DateTime?(Convert.ToDateTime(text3));
                        }
                        catch
                        {
                            result = null;
                        }
                    }
                }
                return result;
            }
        }

        public string MD5
        {
            get
            {
                string result;
                if (this._paravlue == null)
                {
                    result = "";
                }
                else if (string.IsNullOrWhiteSpace(this._paravlue.ToString()))
                {
                    result = "";
                }
                else
                {
                    result = FormsAuthentication.HashPasswordForStoringInConfigFile(this._paravlue.ToString(), "MD5").ToLower();
                }
                return result;
            }
        }

        public string SHA1
        {
            get
            {
                string result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    result = FormsAuthentication.HashPasswordForStoringInConfigFile(this._paravlue.ToString(), "SHA1");
                }
                return result;
            }
        }

        public string UrlDecode
        {
            get
            {
                string result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    result = HttpUtility.UrlDecode(this._paravlue.ToString().Trim());
                }
                return result;
            }
        }

        public string UrlEncode
        {
            get
            {
                string result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    result = HttpUtility.UrlEncode(this._paravlue.ToString().Trim());
                }
                return result;
            }
        }

        public string HtmlDecode
        {
            get
            {
                string result;
                if (this._paravlue == null)
                {
                    result = null;
                }
                else
                {
                    HttpContext current = HttpContext.Current;
                    result = current.Server.HtmlDecode(this._paravlue.ToString());
                }
                return result;
            }
        }

        public string MapPath
        {
            get
            {
                return Server.MapPath(this._paravlue.ToString());
            }
        }

        public string VirtualPath
        {
            get
            {
                return Server.VirtualPath(this._paravlue.ToString());
            }
        }

        public string JavascriptTime
        {
            get
            {
                string result;
                if (this._paravlue == null)
                {
                    result = "";
                }
                else
                {
                    try
                    {
                        DateTime dateTime = System.DateTime.Now;
                        dateTime = Convert.ToDateTime(this._paravlue);
                        string format = "ddd MMM d HH:mm:ss 'UTC'zz'00' yyyy";
                        CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");
                        string text = dateTime.ToString(format, provider);
                        result = text;
                    }
                    catch
                    {
                        result = "";
                    }
                }
                return result;
            }
        }

        public ConvertToAnyValue()
        {
        }

        public ConvertToAnyValue(object para)
        {
            this._paravlue = ((para == null) ? "" : para);
        }

        public ConvertToAnyValue(string para)
        {
            this._paravlue = (string.IsNullOrWhiteSpace(para) ? "" : para);
        }

        public ConvertToAnyValue(string para, string unit)
        {
            this._paravlue = para;
        }

        public string[] Split(char split)
        {
            return this._paravlue.ToString().Split(new char[]
            {
                split
            });
        }
    }
}
