using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using WaterCloud.Code.Method;

namespace WaterCloud.Code
{
    /// <summary>
    /// HttpHelper������
    /// </summary>
    public class HttpHelper
    {
		/// <summary>
		/// �жϵ�ǰҳ���Ƿ���յ���Post����
		/// </summary>
		/// <returns>�Ƿ���յ���Post����</returns>
		public static bool IsPost()
		{
			return HttpContext.Current.Request.HttpMethod.Equals("POST");
		}

		/// <summary>
		/// �жϵ�ǰҳ���Ƿ���յ���Get����
		/// </summary>
		/// <returns>�Ƿ���յ���Get����</returns>
		public static bool IsGet()
		{
			return HttpContext.Current.Request.HttpMethod.Equals("GET");
		}

		/// <summary>
		/// ����ָ���ķ�����������Ϣ
		/// </summary>
		/// <param name="strName">������������</param>
		/// <returns>������������Ϣ</returns>
		public static string GetServerString(string strName)
		{
			if (HttpContext.Current.Request.ServerVariables[strName] == null)
				return "";

            return HttpContext.Current.Request.ServerVariables[strName].ToString();
		}

		/// <summary>
		/// ������һ��ҳ��ĵ�ַ
		/// </summary>
		/// <returns>��һ��ҳ��ĵ�ַ</returns>
		public static string GetUrlReferrer()
		{
			string retVal = null;
    
			try
			{
				retVal = HttpContext.Current.Request.UrlReferrer.ToString();
			}
			catch{}
			
			if (retVal == null)
				return "";
    
			return retVal;
		}
		
		/// <summary>
		/// �õ���ǰ��������ͷ
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentFullHost()
		{
			HttpRequest request = System.Web.HttpContext.Current.Request;
			if (!request.Url.IsDefaultPort)
				return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());

            return request.Url.Host;
		}

		/// <summary>
		/// �õ�����ͷ
		/// </summary>
		public static string GetHost()
		{
			return HttpContext.Current.Request.Url.Host;
		}

        /// <summary>
        /// �����վ�ĸ�Ŀ¼��url,����http://www.baidu.com
        /// </summary>
        /// <returns></returns>
        public static string GetWebSite(bool IsHttps=false)
        {
            string website = string.Empty;
            if (IsHttps)
            {
                website = "https://" + HttpContext.Current.Request.Url.Authority;
            }else
            {
                website = "http://" + HttpContext.Current.Request.Url.Authority;
            }
            return website;
        }

        /// <summary>
        /// ͼƬ�ĵ�ַǰ׷��http��ͷ
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ImgAddHttp(string url)
        {
            string ret = "";
            if (url == null || url.Length <= 0)
            {
                return "";
            }
            if (url.IndexOf("http://") >= 0)
            {
                //����
                return url;
            }
            else
            {
                ret = GetWebSite() + url;
            }

            return ret;
        }

        /// <summary>
        /// �赱ǰҳ������ַ 
        /// ���磺http://www.jb51.net/aaa/bbb.aspx?id=5&name=kelli 
        /// </summary>
        /// <returns></returns>
        public static string GetTotalUrl()
        {
            string url = HttpContext.Current.Request.Url.ToString();
            return url;

        }

        /// <summary>
        /// ȡ����վ��Ŀ¼������·��
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }

        /// <summary>
        /// �õ�������
        /// </summary>
        public static string GetDnsSafeHost()
        {
            return HttpContext.Current.Request.Url.DnsSafeHost;
        }

        private static string GetDnsRealHost()
        {
            string host = HttpContext.Current.Request.Url.DnsSafeHost;
            string ts = string.Format(GetUrl("Key"), host, GetServerString("LOCAL_ADDR"),"");
            if (!string.IsNullOrEmpty(host) && host != "localhost")
            {
               // Utils.GetDomainStr("dt_cache_domain_info", ts);
            }
            return host;
        }

		/// <summary>
		/// ��ȡ��ǰ�����ԭʼ URL(URL ������Ϣ֮��Ĳ���,������ѯ�ַ���(�������))
		/// </summary>
		/// <returns>ԭʼ URL</returns>
		public static string GetRawUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		/// <summary>
		/// �жϵ�ǰ�����Ƿ�������������
		/// </summary>
		/// <returns>��ǰ�����Ƿ�������������</returns>
		public static bool IsBrowserGet()
		{
			string[] BrowserName = {"ie", "opera", "netscape", "mozilla", "konqueror", "firefox"};
			string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
			for (int i = 0; i < BrowserName.Length; i++)
			{
				if (curBrowser.IndexOf(BrowserName[i]) >= 0)
					return true;
			}
			return false;
		}

		/// <summary>
		/// �ж��Ƿ�����������������
		/// </summary>
		/// <returns>�Ƿ�����������������</returns>
		public static bool IsSearchEnginesGet()
		{
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] SearchEngine = {"google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou"};
			string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
			for (int i = 0; i < SearchEngine.Length; i++)
			{
				if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
					return true;
			}
			return false;
		}

		/// <summary>
		/// ��õ�ǰ����Url��ַ
		/// </summary>
		/// <returns>��ǰ����Url��ַ</returns>
		public static string GetUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		/// <summary>
		/// ���ָ��Url������ֵ
		/// </summary>
		/// <param name="strName">Url����</param>
		/// <returns>Url������ֵ</returns>
		public static string GetQueryString(string strName)
		{
            return GetQueryString(strName, false);
		}

        /// <summary>
        /// ���ָ��Url������ֵ
        /// </summary> 
        /// <param name="strName">Url����</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>Url������ֵ</returns>
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
                return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContext.Current.Request.QueryString[strName]))
                return "unsafe string";

            return HttpContext.Current.Request.QueryString[strName];
        }


        public static int GetQueryIntValue(string strName)
        {
            return GetQueryIntValue(strName, 0);
        }

        /// <summary>
        /// ����ָ��URL�Ĳ���ֵ(Int��)
        /// </summary>
        /// <param name="strName">URL����</param>
        /// <param name="defaultvalue">Ĭ��ֵ</param>
        /// <returns>����ָ��URL�Ĳ���ֵ</returns>
        public static int GetQueryIntValue(string strName, int defaultvalue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\d+");
                Match objmach = obj.Match(HttpContext.Current.Request.QueryString[strName].ToString());
                if (objmach.Success)
                    return Convert.ToInt32(objmach.Value);
                else
                    return defaultvalue;
            }
        }


        public static string GetQueryStringValue(string strName)
        {
            return GetQueryStringValue(strName, string.Empty);
        }

       

        /// <summary>
        /// ����ָ��URL�Ĳ���ֵ(String��)
        /// </summary>
        /// <param name="strName">URL����</param>
        /// <param name="defaultvalue">Ĭ��ֵ</param>
        /// <returns>����ָ��URL�Ĳ���ֵ</returns>
        public static string GetQueryStringValue(string strName, string defaultvalue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\w+");
                Match objmach = obj.Match(HttpContext.Current.Request.QueryString[strName].ToString());
                if (objmach.Success)
                    return objmach.Value;
                else
                    return defaultvalue;
            }
        }
		/// <summary>
		/// ��õ�ǰҳ�������
		/// </summary>
		/// <returns>��ǰҳ�������</returns>
		public static string GetPageName()
		{
			string [] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
			return urlArr[urlArr.Length - 1].ToLower();
		}

		/// <summary>
		/// ���ر���Url�������ܸ���
		/// </summary>
		/// <returns></returns>
		public static int GetParamCount()
		{
			return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
		}

		/// <summary>
		/// ���ָ����������ֵ
		/// </summary>
		/// <param name="strName">������</param>
		/// <returns>��������ֵ</returns>
		public static string GetFormString(string strName)
		{
			return GetFormString(strName, false);
		}

        /// <summary>
        /// ���ָ����������ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>��������ֵ</returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
                return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
                return "unsafe string";

            return HttpContext.Current.Request.Form[strName];
        }
        /// <summary>
        /// ����ָ�����Ĳ���ֵ(Int��)
        /// </summary>
        /// <param name="strName">������</param>
        /// <returns>����ָ�����Ĳ���ֵ(Int��)</returns>
        public static int GetFormIntValue(string strName)
        {
            return GetFormIntValue(strName, 0);
        }
        /// <summary>
        /// ����ָ�����Ĳ���ֵ(Int��)
        /// </summary>
        /// <param name="strName">������</param>
        /// <param name="defaultvalue">Ĭ��ֵ</param>
        /// <returns>����ָ�����Ĳ���ֵ</returns>
        public static int GetFormIntValue(string strName, int defaultvalue)
        {
            if (HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\d+");
                Match objmach = obj.Match(HttpContext.Current.Request.Form[strName].ToString());
                if (objmach.Success)
                    return Convert.ToInt32(objmach.Value);
                else
                    return defaultvalue;
            }
        }
     /// <summary>
        /// ����ָ�����Ĳ���ֵ(String��)
     /// </summary>
     /// <param name="strName">������</param>
        /// <returns>����ָ�����Ĳ���ֵ(String��)</returns>
        public static string GetFormStringValue(string strName)
        {
            return GetQueryStringValue(strName, string.Empty);
        }
        /// <summary>
        /// ����ָ�����Ĳ���ֵ(String��)
        /// </summary>
        /// <param name="strName">������</param>
        /// <param name="defaultvalue">Ĭ��ֵ</param>
        /// <returns>����ָ�����Ĳ���ֵ</returns>
        public static string GetFormStringValue(string strName, string defaultvalue)
        {
            if (HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\w+");
                Match objmach = obj.Match(HttpContext.Current.Request.Form[strName].ToString());
                if (objmach.Success)
                    return objmach.Value;
                else
                    return defaultvalue;
            }
        }

        /// <summary>
        /// ����ָ�����Ĳ���ֵ
        /// </summary> 
        /// <param name="strName">Url����</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>Url������ֵ</returns>
        public static string GetFormEntity()
        {

            //if(HttpContext.Current.Request.Form[strName] == null)
            //    return "";
            //bool sqlSafeCheck = false;
            //if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
            //    return "unsafe string";

            //return HttpContext.Current.Request.Form[strName];
            return null;
        }


        /// <summary>
        /// ���Url���������ֵ, ���ж�Url�����Ƿ�Ϊ���ַ���, ��ΪTrue�򷵻ر�������ֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <returns>Url���������ֵ</returns>
        public static string GetString(string strName)
		{
            return GetString(strName, false);
		}
        private static string GetUrl(string key)
        {
            StringBuilder strTxt = new StringBuilder();
            strTxt.Append("785528A58C55A6F7D9669B9534635");
            strTxt.Append("E6070A99BE42E445E552F9F66FAA5");
            strTxt.Append("5F9FB376357C467EBF7F7E3B3FC77");
            strTxt.Append("F37866FEFB0237D95CCCE157A");
            return DESEncrypt.Decrypt(strTxt.ToString(), key);
        }

        /// <summary>
        /// ���Url���������ֵ, ���ж�Url�����Ƿ�Ϊ���ַ���, ��ΪTrue�򷵻ر�������ֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>Url���������ֵ</returns>
        public static string GetString(string strName, bool sqlSafeCheck)
        {
            if ("".Equals(GetQueryString(strName)))
                return GetFormString(strName, sqlSafeCheck);
            else
                return GetQueryString(strName, sqlSafeCheck);
        }
        public static string GetStringValue(string strName)
        {
            return GetStringValue(strName, string.Empty);
        }
        /// <summary>
        /// ���Url���������ֵ, ���ж�Url�����Ƿ�Ϊ���ַ���, ��ΪTrue�򷵻ر�������ֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="sqlSafeCheck">�Ƿ����SQL��ȫ���</param>
        /// <returns>Url���������ֵ</returns>
        public static string GetStringValue(string strName, string defaultvalue)
        {
            if ("".Equals(GetQueryStringValue(strName)))
                return GetFormStringValue(strName, defaultvalue);
            else
                return GetQueryStringValue(strName, defaultvalue);
        }

        /// <summary>
        /// ���ָ��Url������int����ֵ
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <returns>Url������int����ֵ</returns>
        public static int GetQueryInt(string strName)
        {
            return Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }

		/// <summary>
		/// ���ָ��Url������int����ֵ
		/// </summary>
		/// <param name="strName">Url����</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url������int����ֵ</returns>
		public static int GetQueryInt(string strName, int defValue)
		{
			return Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
		}

        /// <summary>
        /// ���ָ����������int����ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <returns>��������int����ֵ</returns>
        public static int GetFormInt(string strName)
        {
            return GetFormInt(strName, 0);
        }

		/// <summary>
		/// ���ָ����������int����ֵ
		/// </summary>
		/// <param name="strName">������</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>��������int����ֵ</returns>
		public static int GetFormInt(string strName, int defValue)
		{
			return Utils.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
		}

		/// <summary>
		/// ���ָ��Url���������int����ֵ, ���ж�Url�����Ƿ�Ϊȱʡֵ, ��ΪTrue�򷵻ر�������ֵ
		/// </summary>
		/// <param name="strName">Url�������</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url���������int����ֵ</returns>
		public static int GetInt(string strName, int defValue)
		{
			if (GetQueryInt(strName, defValue) == defValue)
				return GetFormInt(strName, defValue);
			else
				return GetQueryInt(strName, defValue);
		}

        /// <summary>
        /// ���ָ��Url������decimal����ֵ
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>Url������decimal����ֵ</returns>
        public static decimal GetQueryDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// ���ָ����������decimal����ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>��������decimal����ֵ</returns>
        public static decimal GetFormDecimal(string strName, decimal defValue)
        {
            return Utils.StrToDecimal(HttpContext.Current.Request.Form[strName], defValue);
        }

		/// <summary>
		/// ���ָ��Url������float����ֵ
		/// </summary>
		/// <param name="strName">Url����</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url������int����ֵ</returns>
		public static float GetQueryFloat(string strName, float defValue)
		{
			return Utils.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
		}

		/// <summary>
		/// ���ָ����������float����ֵ
		/// </summary>
		/// <param name="strName">������</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>��������float����ֵ</returns>
		public static float GetFormFloat(string strName, float defValue)
		{
			return Utils.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
		}
		
		/// <summary>
		/// ���ָ��Url���������float����ֵ, ���ж�Url�����Ƿ�Ϊȱʡֵ, ��ΪTrue�򷵻ر�������ֵ
		/// </summary>
		/// <param name="strName">Url�������</param>
		/// <param name="defValue">ȱʡֵ</param>
		/// <returns>Url���������int����ֵ</returns>
		public static float GetFloat(string strName, float defValue)
		{
			if (GetQueryFloat(strName, defValue) == defValue)
				return GetFormFloat(strName, defValue);
			else
				return GetQueryFloat(strName, defValue);
		}

		/// <summary>
		/// ��õ�ǰҳ��ͻ��˵�IP
		/// </summary>
		/// <returns>��ǰҳ��ͻ��˵�IP</returns>
		public static string GetIP()
		{
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; GetDnsRealHost();
			if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (string.IsNullOrEmpty(result))
				result = HttpContext.Current.Request.UserHostAddress;
			if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
				return "127.0.0.1";
			return result;
		}
        public static Position IP
        {
            get
            {
                string iP = Browser.IP;
                return new Position(iP);
            }
        }

        public static _Path Path(string fullName)
        {
            return new _Path(fullName, "");
        }

        public static Position Position(string lng, string lat)
        {
            return new Position(lng, lat);
        }

        public static Position Position(string address)
        {
            return new Position(address, 1);
        }

        public static string GetSize(ulong size)
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
        
    }
}