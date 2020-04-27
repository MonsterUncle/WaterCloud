/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using System;
using System.Security.Cryptography;
using System.Text;

namespace WaterCloud.Code
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class Md5
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string md5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                var md5 = System.Security.Cryptography.MD5.Create();
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder builder = new StringBuilder();
                // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("X2"));
                }
                strEncrypt = builder.ToString().Substring(8, 16);
            }

            if (code == 32)
            {
                var md5 = System.Security.Cryptography.MD5.Create();
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder builder = new StringBuilder();
                // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("X2"));
                }
                strEncrypt = builder.ToString();
            }

            return strEncrypt;
        }
        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(str);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');

            }
            return str;
        }
        public static string MD5Lower16(string str)
        {
            return MD5(str).ToLower().Substring(8, 16);
        }

        public static string MD5Lower32(string str)
        {
            return MD5(str).ToLower(); ;
        }

        /// <summary> 
        /// 32位小写 
        /// </summary> 
        /// <returns></returns> 
        public static string SHA1(string s)
        {
            SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
            string shaStr = BitConverter.ToString(hash);
            shaStr = shaStr.Replace("-", "");
            shaStr = shaStr.ToLower();
            return s.ToLower();
        }
    }
}
