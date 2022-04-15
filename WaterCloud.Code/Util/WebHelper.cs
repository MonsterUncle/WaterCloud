﻿/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Net.Sockets;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace WaterCloud.Code
{
    public class WebHelper
    {
        #region ResolveUrl(解析相对Url)
        /// <summary>
        /// 解析相对Url
        /// </summary>
        /// <param name="relativeUrl">相对Url</param>
        public static string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return string.Empty;
            relativeUrl = relativeUrl.Replace("\\", "/");
            if (relativeUrl.StartsWith("/"))
                return relativeUrl;
            if (relativeUrl.Contains("://"))
                return relativeUrl;
            return VirtualPathUtility.ToAbsolute(relativeUrl);
        }

        #endregion

        #region HtmlEncode(对html字符串进行编码)
        /// <summary>
        /// 对html字符串进行编码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// 对html字符串进行解码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        #endregion

        #region UrlEncode(对Url进行编码)

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// 获取大写编码字符串
        /// </summary>
        private static string GetUpperEncode(string encode)
        {
            var result = new StringBuilder();
            int index = int.MinValue;
            for (int i = 0; i < encode.Length; i++)
            {
                string character = encode[i].ToString();
                if (character == "%")
                    index = i;
                if (i - index == 1 || i - index == 2)
                    character = character.ToUpper();
                result.Append(character);
            }
            return result.ToString();
        }

        #endregion

        #region UrlDecode(对Url进行解码)

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion

        #region Session操作

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession(string key, string value)
        {
            if (key.IsEmpty())
                return;
            GlobalContext.HttpContext?.Session.SetString(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public static string GetSession(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            return GlobalContext.HttpContext?.Session.GetString(key) ?? "";
        }
        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static void RemoveSession(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            GlobalContext.HttpContext?.Session.Remove(key);
        }

        #endregion

        #region Cookie操作
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue, CookieOptions option = null)
        {
            if (option == null)
            {
                option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(30);
            }
            GlobalContext.HttpContext?.Response.Cookies.Append(strName, strValue, option);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expires);
            GlobalContext.HttpContext?.Response.Cookies.Append(strName, strValue, option);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            return GlobalContext.HttpContext?.Request.Cookies[strName]??"";
        }
        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="CookiesName">Cookie对象名称</param>
        public static void RemoveCookie(string CookiesName)
        {
            GlobalContext.HttpContext?.Response.Cookies.Delete(CookiesName);
        }
        #endregion

        #region 去除HTML标记
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="NoHTML">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHtml(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&hellip;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&mdash;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            
            Htmlstring = HtmlEncoder.Default.Encode(Htmlstring).Trim();
            return Htmlstring;

        }
        #endregion

        #region 格式化文本（防止SQL注入）
        /// <summary>
        /// 格式化文本（防止SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Formatstr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex10 = new System.Text.RegularExpressions.Regex(@"select", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex11 = new System.Text.RegularExpressions.Regex(@"update", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex12 = new System.Text.RegularExpressions.Regex(@"delete", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex10.Replace(html, "s_elect");
            html = regex11.Replace(html, "u_pudate");
            html = regex12.Replace(html, "d_elete");
            html = html.Replace("'", "’");
            html = html.Replace("&nbsp;", " ");
            return html;
        }
        #endregion

        #region 当前连接
        public static HttpContext HttpContext
        {
            get { return GlobalContext.HttpContext; }
        }
        #endregion

        #region 网络信息
        public static string Ip
        {
            get
            {
                string result = string.Empty;
                try
                {
                    if (HttpContext != null)
                    {
                        result = GetWebClientIp();
                    }
                    if (string.IsNullOrEmpty(result))
                    {
                        result = GetLanIp();
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
                return result;
            }
        }

        private static string GetWebClientIp()
        {
            try
            {
                string ip = GetWebRemoteIp();
                foreach (var hostAddress in Dns.GetHostAddresses(ip))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                    else if(hostAddress.AddressFamily == AddressFamily.InterNetworkV6)
					{
                        return hostAddress.MapToIPv4().ToString();
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return string.Empty;
        }

        public static string GetLanIp()
        {
            try
            {
                foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                    else if (hostAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        return hostAddress.MapToIPv4().ToString();
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return string.Empty;
        }

        public static string GetWanIp()
        {
            string ip = string.Empty;
            try
            {
                string url = "http://www.net.cn/static/customercare/yourip.asp";
                string html = "";
                using (var client=new HttpClient())
				{
                   var reponse= client.GetAsync(url).GetAwaiter().GetResult();
                    reponse.EnsureSuccessStatusCode();
                   html = reponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                if (!string.IsNullOrEmpty(html))
                {
                    ip = WebHelper.Resove(html, "<h2>", "</h2>");
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return ip;
        }

        private static string GetWebRemoteIp()
        {
            try
            {
                string ip = HttpContext?.Connection?.RemoteIpAddress.ParseToString();
                if (HttpContext != null && HttpContext.Request != null)
                {
                    if (HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
                    {
                        ip = HttpContext.Request.Headers["X-Real-IP"].ToString();
                    }

                    if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        ip = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
                    }
                }
                return ip;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string UserAgent
        {
            get
            {
                string userAgent = string.Empty;
                try
                {
                    userAgent = HttpContext?.Request?.Headers["User-Agent"];
                }
                catch (Exception ex)
                {
                    LogHelper.WriteWithTime(ex);
                }
                return userAgent;
            }
        }

        public static string GetOSVersion()
        {
            var osVersion = string.Empty;
            try
            {
                var userAgent = UserAgent;
                if (userAgent.Contains("NT 10"))
                {
                    osVersion = "Windows 10";
                }
                else if (userAgent.Contains("NT 6.3"))
                {
                    osVersion = "Windows 8";
                }
                else if (userAgent.Contains("NT 6.1"))
                {
                    osVersion = "Windows 7";
                }
                else if (userAgent.Contains("NT 6.0"))
                {
                    osVersion = "Windows Vista/Server 2008";
                }
                else if (userAgent.Contains("NT 5.2"))
                {
                    osVersion = "Windows Server 2003";
                }
                else if (userAgent.Contains("NT 5.1"))
                {
                    osVersion = "Windows XP";
                }
                else if (userAgent.Contains("NT 5"))
                {
                    osVersion = "Windows 2000";
                }
                else if (userAgent.Contains("NT 4"))
                {
                    osVersion = "Windows NT4";
                }
                else if (userAgent.Contains("Android"))
                {
                    osVersion = "Android";
                }
                else if (userAgent.Contains("Me"))
                {
                    osVersion = "Windows Me";
                }
                else if (userAgent.Contains("98"))
                {
                    osVersion = "Windows 98";
                }
                else if (userAgent.Contains("95"))
                {
                    osVersion = "Windows 95";
                }
                else if (userAgent.Contains("Mac"))
                {
                    osVersion = "Mac";
                }
                else if (userAgent.Contains("Unix"))
                {
                    osVersion = "UNIX";
                }
                else if (userAgent.Contains("Linux"))
                {
                    osVersion = "Linux";
                }
                else if (userAgent.Contains("SunOS"))
                {
                    osVersion = "SunOS";
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return osVersion;
        }
        #endregion

        #region IP位置查询
        public static string GetIpLocation(string ipAddress)
        {
            string ipLocation = "未知";
            try
            {
                if (!IsInnerIP(ipAddress))
                {
                    ipLocation = GetIpLocationFromPCOnline(ipAddress);
                }
				else
				{
                    ipLocation = "本地局域网";
                }
            }
            catch (Exception)
            {
                return ipLocation;
            }
            return ipLocation;
        }

        private static string GetIpLocationFromPCOnline(string ipAddress)
        {
            string ipLocation = "未知";
            try
            {
                var res = "";
                using (var client=new HttpClient())
				{
                    var URL = "http://whois.pconline.com.cn/ip.jsp?ip=" + ipAddress;
                    var response = client.GetAsync(URL).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    res = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                if (!string.IsNullOrEmpty(res))
                {
                    ipLocation = res.Trim();
                }
            }
            catch
			{
                ipLocation = "未知";
            }
            return ipLocation;
        }
        #endregion

        #region 判断是否是外网IP
        public static bool IsInnerIP(string ipAddress)
        {
            bool isInnerIp = false;
            long ipNum = GetIpNum(ipAddress);
            /**
                私有IP：A类 10.0.0.0-10.255.255.255
                            B类 172.16.0.0-172.31.255.255
                            C类 192.168.0.0-192.168.255.255
                当然，还有127这个网段是环回地址 
           **/
            long aBegin = GetIpNum("10.0.0.0");
            long aEnd = GetIpNum("10.255.255.255");
            long bBegin = GetIpNum("172.16.0.0");
            long bEnd = GetIpNum("172.31.255.255");
            long cBegin = GetIpNum("192.168.0.0");
            long cEnd = GetIpNum("192.168.255.255");
            isInnerIp = IsInner(ipNum, aBegin, aEnd) || IsInner(ipNum, bBegin, bEnd) || IsInner(ipNum, cBegin, cEnd) || ipAddress.Equals("127.0.0.1");
            return isInnerIp;
        }

        /// <summary>
        /// 把IP地址转换为Long型数字
        /// </summary>
        /// <param name="ipAddress">IP地址字符串</param>
        /// <returns></returns>
        private static long GetIpNum(string ipAddress)
        {
            string[] ip = ipAddress.Split('.');
            long a = int.Parse(ip[0]);
            long b = int.Parse(ip[1]);
            long c = int.Parse(ip[2]);
            long d = int.Parse(ip[3]);

            long ipNum = a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
            return ipNum;
        }

        private static bool IsInner(long userIp, long begin, long end)
        {
            return (userIp >= begin) && (userIp <= end);
        }
        #endregion

        #region html处理
        /// <summary>
        /// Get part Content from HTML by apply prefix part and subfix part
        /// </summary>
        /// <param name="html">souce html</param>
        /// <param name="prefix">prefix</param>
        /// <param name="subfix">subfix</param>
        /// <returns>part content</returns>
        public static string Resove(string html, string prefix, string subfix)
        {
            int inl = html.IndexOf(prefix);
            if (inl == -1)
            {
                return null;
            }
            inl += prefix.Length;
            int inl2 = html.IndexOf(subfix, inl);
            string s = html.Substring(inl, inl2 - inl);
            return s;
        }
        public static string ResoveReverse(string html, string subfix, string prefix)
        {
            int inl = html.IndexOf(subfix);
            if (inl == -1)
            {
                return null;
            }
            string subString = html.Substring(0, inl);
            int inl2 = subString.LastIndexOf(prefix);
            if (inl2 == -1)
            {
                return null;
            }
            string s = subString.Substring(inl2 + prefix.Length, subString.Length - inl2 - prefix.Length);
            return s;
        }
        public static List<string> ResoveList(string html, string prefix, string subfix)
        {
            List<string> list = new List<string>();
            int index = prefix.Length * -1;
            do
            {
                index = html.IndexOf(prefix, index + prefix.Length);
                if (index == -1)
                {
                    break;
                }
                index += prefix.Length;
                int index4 = html.IndexOf(subfix, index);
                string s78 = html.Substring(index, index4 - index);
                list.Add(s78);
            }
            while (index > -1);
            return list;
        }
        #endregion
    }
}
