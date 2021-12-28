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
    /// DES加密、解密帮助类
    /// </summary>
    public class DESEncrypt
    {
        private static string DESKey = "WaterCloud_desencrypt_2019";

        #region ========加密========
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, DESKey);
        }
        /// <summary> 
        /// 加密数据，用Web.Security的Hash方式加密
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DES des = DES.Create();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(DecryptMd5(sKey));
            des.IV = ASCIIEncoding.ASCII.GetBytes(DecryptMd5(sKey));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        ///  加密数据, 用Security.MD5而非Web.Security的Hash方式加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt2(string Text)
        {
            return Encrypt2(Text, DESKey);
        }

        /// <summary>
        /// 加密数据, 用Security.MD5而非Web.Security的Hash方式加密
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt2(string Text, string sKey)
        {
            DES des = DES.Create();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            sKey = MD5(sKey).Substring(0, 8);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return Decrypt(Text, DESKey);
            }
            else
            {
                return "";
            }
        }

        /// <summary> 
        /// 解密数据，用Web.Security的Hash方式加密 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DES des = DES.Create();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(DecryptMd5(sKey));
            des.IV = ASCIIEncoding.ASCII.GetBytes(DecryptMd5(sKey));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
        public static string DecryptMd5(string str)
        {
            string strEncrypt = string.Empty;
            var md5 = System.Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder builder = new StringBuilder();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("X2"));
            }
            strEncrypt = builder.ToString().Substring(0,8);
            return strEncrypt;
        }
        /// <summary>
        /// 解密数据,用Security.MD5而非Web.Security的Hash方式加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt2(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return Decrypt2(Text, DESKey);
            }
            else
            {
                return "";
            }
        }

        /// <summary> 
        /// 解密数据,用Security.MD5而非Web.Security的Hash方式加密
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt2(string Text, string sKey)
        {
            DES des = DES.Create();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            sKey = MD5(sKey).Substring(0, 8);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        public static string MD5(string pwd)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = System.Text.Encoding.Default.GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');

            }
            return str;
        }

        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public static string Sha1(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
