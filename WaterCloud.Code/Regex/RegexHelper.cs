using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WaterCloud.Code
{
    public class RegexHelper
    {
        /// <summary>
        /// 判断字符串是否是a-zA-Z0-9_范围内（3-50位范围内）
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string strIn)
        {
            return Regex.IsMatch(strIn, @"^[A-Za-z0-9_]{3,50}$");
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string strIn)
        {
            return Regex.IsMatch(strIn, @"^[1]+[3,5,8]+\d{9}$");
        }
       /// <summary>
       /// 验证手机号
       /// </summary>
        /// <param name="strIn"></param>
       /// <returns></returns>
        public static bool IsValidTelPhone(string strIn)
        {
            return Regex.IsMatch(strIn, @"^(\d{3,4}-)?\d{6,8}$");

        }
    }
}
