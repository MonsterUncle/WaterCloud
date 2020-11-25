using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterCloud.Code;

namespace WaterCloud.Application.CommonService
{
    /// <summary>
    /// 对公司产品授权码的生成处理
    /// </summary>
    public class LicenseSecret
    {
        /// <summary>
        /// 对公司MDK系列产品授权码的生成处理
        /// </summary>
        /// <param name="orgain">待授权的组织机构名称</param>
        /// <param name="dateStr">授权日期：yyyy-MM-dd</param>
        /// <param name="SecretKey">返回值：注册码</param>
        /// <param name="SaltKey">返回值：验证码</param>
        /// <param name="SysRegiKey">返回值：授权码</param>
        /// <returns></returns>
        public static bool GetMdkLicenseSecret(string orgain, string dateStr, out string SecretKey, out string SaltKey, out string SysRegiKey)
        {
            SecretKey = string.Empty;
            SaltKey = string.Empty;
            SysRegiKey = string.Empty;
            string encryptStr = string.Empty;
            if (string.IsNullOrEmpty(orgain))
            {
                return false;
            }
            try
            {
                SaltKey = Utils.GetLetterOrNumberRandom(6);

                encryptStr = "Metec:" + orgain.Trim();
                SysRegiKey = DESEncrypt.Encrypt2(encryptStr, SaltKey);
                encryptStr = "Metec:" + dateStr.Trim();
                SecretKey = DESEncrypt.Encrypt2(encryptStr, SaltKey);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        ///  验证产品许可认证
        /// </summary>
        /// <param name="key"></param>
        /// <returns>-1 授权码破坏，0 授权码不正确，1 正确，2试用到期</returns>
        private static int VerifyLicense(string secretKey, string saltKey, string key, string regiOrganName, out int daynum)
        {
            daynum = 0;
            //加密授权码都不为空，表示已注册，验证授权码
            if (string.IsNullOrEmpty(secretKey) == false && string.IsNullOrEmpty(saltKey) == false)
            {
                string name = DESEncrypt.Decrypt2(secretKey, saltKey);
                string[] strs = name.Split(':');
                if (strs[0].ToLower() == "Metec".ToLower() && strs[1].Trim() == regiOrganName.Trim())
                {
                    return 1;
                }
                return 0;
            }
            //加密授权码都为空，表示未注册授权，判断试用期
            else if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(secretKey))
            {
                if (string.IsNullOrEmpty(key))
                {
                    string date = DateTime.Now.AddDays(30).ToString("yyyyMMdd");
                    string newkey = DESEncrypt.Encrypt2(date);
                    //DbHelperSQL.ExecuteSql("update SysConfig set SysTrialRegi='" + newkey + "'");
                    return 1;
                }
                else
                {
                    string nowkey = DESEncrypt.Decrypt2(key);
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    if (nowkey != "")
                    {
                        date = nowkey.Substring(0, 4) + "-" + nowkey.Substring(4, 2) + "-" + nowkey.Substring(6, 2);
                    }
                    DateTime nowdate = Utils.StrToDateTime(date, DateTime.Now);
                    if (nowdate > DateTime.Now.Date)
                    {
                        daynum = (int)nowdate.Subtract(DateTime.Now.Date).TotalDays;
                        return 1;
                    }
                    return 2;
                }
            }
            // 加密授权码已破坏
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// 获取系统授权验证信息
        /// </summary>
        /// <param name="daynum"></param>
        /// <returns></returns>
        public static int GetLicenseMsg(string SecretKey, string SaltKey, string SysTrialRegi, string RegiOrganName, out int daynum)
        {
            daynum = 0;
            //加密授权码都不为空，表示已注册，验证授权码
            if (string.IsNullOrEmpty(SecretKey) == false && string.IsNullOrEmpty(SaltKey) == false)
            {
                string name = DESEncrypt.Decrypt2(SecretKey, SaltKey);
                string[] strs = name.Split(':');
                if (strs[0].ToLower() == "Metec".ToLower() && strs[1].Trim() == RegiOrganName.Trim())
                {
                    return 1;
                }
                return 0;
            }
            //加密授权码都为空，表示未注册授权，判断试用期
            else if (string.IsNullOrEmpty(SecretKey) && string.IsNullOrEmpty(SecretKey))
            {
                if (string.IsNullOrEmpty(SysTrialRegi))
                {
                    return -1;
                }
                else
                {
                    string nowkey = DESEncrypt.Decrypt2(SysTrialRegi);
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    if (nowkey != "")
                    {
                        date = nowkey.Substring(0, 4) + "-" + nowkey.Substring(4, 2) + "-" + nowkey.Substring(6, 2);
                    }
                    DateTime nowdate = Utils.StrToDateTime(date, DateTime.Now);
                    if (nowdate > DateTime.Now.Date)
                    {
                        daynum = (int)nowdate.Subtract(DateTime.Now.Date).TotalDays;
                        return 1;
                    }
                    return 2;
                }
            }
            // 加密授权码已破坏
            else
            {
                return -1;
            }
        }
    }
}
