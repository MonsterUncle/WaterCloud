/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Web;
namespace WaterCloud.Code
{
    public class OperatorProvider
    {
        //登录信息保存方式
        private string LoginProvider = GlobalContext.SystemConfig.LoginProvider;
        //是否允许一个账户在多处登录
        private bool LoginMultiple = GlobalContext.SystemConfig.LoginMultiple;
        public static OperatorProvider Provider
        {
            get { return new OperatorProvider(); }
        }
        //watercloud_operator_pc_ PC端登录
        //watercloud_operator_info_ 登录次数
        //
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        private string cacheKeyToken = "watercloud_token_";// +登录者token
        private string cacheKeyError = "watercloud_error_";// + Mark
        /// <summary>
        /// 秘钥
        /// </summary>
        private string LoginUserToken = "watercloud_Token";
        /// <summary>
        /// 标记登录的浏览器
        /// </summary>
        private string LoginUserMarkKey = "watercloud_Mark";
        public string GetProvider(string key)
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    return WebHelper.GetCookie(key).ToString();
                case "Session":
                    return WebHelper.GetSession(key).ToString();
                default:
                    return WebHelper.GetCookie(key).ToString();
            }
        }
        public void SetProvider(string key,string value)
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    WebHelper.WriteCookie(key, value);
                    break;
                case "Session":
                    WebHelper.WriteSession(key, value);
                    break;
                default:
                    WebHelper.WriteCookie(key, value);
                    break;
            }
        }
        public void RemoveProvider(string key)
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    WebHelper.RemoveCookie(key);
                    break;
                case "Session":
                    WebHelper.RemoveSession(key);
                    break;
                default:
                    WebHelper.RemoveCookie(key);
                    break;
            }
        }
        public OperatorModel GetCurrent()
        {
            OperatorModel operatorModel = new OperatorModel();
            try
            {
                string token = GetProvider(LoginUserToken);
                string loginMark = GetProvider(LoginUserMarkKey);
                operatorModel = RedisHelper.Get<OperatorModel>(cacheKeyOperator + loginMark);
            }
            catch
            {
                operatorModel = null;
            }
            return operatorModel;
        }
        /// <summary>
        /// 获取浏览器设配号
        /// </summary>
        /// <returns></returns>
        public string GetMark()
        {
            string cookieMark = GetProvider(LoginUserMarkKey);
            if (string.IsNullOrEmpty(cookieMark))
            {
                cookieMark = Guid.NewGuid().ToString();
                SetProvider(LoginUserMarkKey, cookieMark);
            }
            return cookieMark;
        }
        /// <summary>
        /// 登录者信息添加到缓存中
        /// </summary>
        /// <param name="userEntity">用户</param>
        /// <param name="loginMark">设备标识uid</param>
        /// <param name="facilityMark">设备类型</param>
        /// <param name="cookie">是否保存cookie，默认是</param>
        /// <returns></returns>
        public string AddLoginUser(OperatorModel operatorModel, string loginMark, string facilityMark, bool cookie = true)
        {
            string token = Guid.NewGuid().ToString();
            try
            {
                // 填写登录信息
                operatorModel.LoginToken = token;
                //cookid登录信息更新
                if (cookie)
                {
                    string cookieMark = GetProvider(LoginUserMarkKey);
                    if (string.IsNullOrEmpty(cookieMark))
                    {
                        operatorModel.loginMark = Guid.NewGuid().ToString();
                        SetProvider(LoginUserMarkKey, operatorModel.loginMark);
                    }
                    else
                    {
                        operatorModel.loginMark = cookieMark;
                    }
                    SetProvider(LoginUserToken, token);
                }
                else
                {
                    operatorModel.loginMark = loginMark;
                }
                //redis 登录token列表更新
                Dictionary<string, string> tokenMarkList = RedisHelper.Get<Dictionary<string, string>>(cacheKeyToken + operatorModel.UserId);
                if (tokenMarkList == null)// 此账号第一次登录
                {
                    tokenMarkList = new Dictionary<string, string>();
                    tokenMarkList.Add(operatorModel.loginMark, token);
                }
                else
                {
                    if (tokenMarkList.ContainsKey(operatorModel.loginMark))
                    {
                        tokenMarkList[operatorModel.loginMark] = token;
                    }
                    else
                    {
                        tokenMarkList.Add(operatorModel.loginMark, token);
                    }
                }

                RedisHelper.Set(cacheKeyToken + operatorModel.UserId, tokenMarkList);
                RedisHelper.Set(cacheKeyOperator + operatorModel.loginMark, operatorModel);
                RedisHelper.Del(cacheKeyOperator + facilityMark + operatorModel.UserId);
                RedisHelper.Set(cacheKeyOperator + facilityMark + operatorModel.UserId, token);
                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 清空当前登录信息
        /// </summary>
        public void EmptyCurrent()
        {
            try
            {
                string token = GetProvider(LoginUserToken);
                string loginMark = GetProvider(LoginUserMarkKey);
                EmptyCurrent(token, loginMark);
                RemoveProvider(LoginUserMarkKey.Trim());
                RemoveProvider(LoginUserToken.Trim());
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 清空当前登录信息
        /// </summary>
        /// <param name="token">登录票据</param>
        /// <param name="facilityMark">登录设备</param>
        /// <param name="loginMark">登录设备标识</param>
        public void EmptyCurrent(string token, string loginMark)
        {
            try
            {
                OperatorModel operatorInfo = RedisHelper.Get<OperatorModel>(cacheKeyOperator + loginMark);
                if (operatorInfo != null && operatorInfo.LoginToken == token)
                {
                    Dictionary<string, string> tokenMarkList = RedisHelper.Get<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId);
                    tokenMarkList.Remove(loginMark);
                    RedisHelper.Del(cacheKeyOperator + loginMark);
                    RedisHelper.Set(cacheKeyToken + operatorInfo.UserId, tokenMarkList);
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 判断登录状态
        /// </summary>
        /// <returns>-1未登录,1登录成功,0登录过期,-2账号被顶</returns>
        public OperatorResult IsOnLine(string facilityMark)
        {
            try
            {
                string token = GetProvider(LoginUserToken);
                string loginMark = GetProvider(LoginUserMarkKey);
                return IsOnLine(token, facilityMark, loginMark);
            }
            catch (Exception)
            {
                return new OperatorResult { stateCode = -1 };
            }
        }
        /// <summary>
        /// 判断登录状态
        /// </summary>
        /// <param name="token">登录票据</param>
        /// <param name="loginMark">登录设备标识</param>
        /// <returns>-1未登录,1登录成功,0登录过期,-2账号被顶</returns>
        public OperatorResult IsOnLine(string token, string facilityMark, string loginMark)
        {
            OperatorResult operatorResult = new OperatorResult();
            operatorResult.stateCode = -1; // -1未登录,1登录成功,0登录过期
            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(loginMark))
                {
                    return operatorResult;
                }
                OperatorModel operatorInfo = RedisHelper.Get<OperatorModel>(cacheKeyOperator + loginMark);
                if (operatorInfo != null && operatorInfo.LoginToken == token)
                {
                    TimeSpan span = (TimeSpan)(DateTime.Now - operatorInfo.LoginTime);
                    //超时
                    if (span.TotalHours >= 12)// 登录操作过12小时移除
                    {
                        operatorResult.stateCode = 0;
                        Dictionary<string, string> tokenMarkList = RedisHelper.Get<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId);
                        tokenMarkList.Remove(loginMark);
                        RedisHelper.Set(cacheKeyToken + operatorInfo.UserId, tokenMarkList);
                        RedisHelper.Del(cacheKeyOperator + loginMark);
                    }
                    //账号被顶(排除admin)
                    else if (!LoginMultiple&&!operatorInfo.IsSystem&&token != RedisHelper.Get<string>(cacheKeyOperator + facilityMark + operatorInfo.UserId))
                    {
                        operatorResult.stateCode = -2;
                        Dictionary<string, string> tokenMarkList = RedisHelper.Get<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId);
                        tokenMarkList.Remove(loginMark);
                        RedisHelper.Set(cacheKeyToken + operatorInfo.UserId, tokenMarkList);
                        RedisHelper.Del(cacheKeyOperator + loginMark);
                    }
                    else
                    {
                        operatorResult.userInfo = operatorInfo;
                        operatorResult.stateCode = 1;
                    }

                }
                return operatorResult;
            }
            catch (Exception)
            {
                return operatorResult;
            }
        }

        #region 登录错误次数记录
        /// <summary>
        /// 获取当前登录错误次数
        /// </summary>
        /// <returns></returns>
        public int GetCurrentErrorNum()
        {
            int res = 0;
            try
            {
                string cookieMark = GetProvider(LoginUserMarkKey);
                if (string.IsNullOrEmpty(cookieMark))
                {
                    cookieMark = Guid.NewGuid().ToString();
                    SetProvider(LoginUserMarkKey, cookieMark);
                }
                string num = RedisHelper.Get<string>(cacheKeyError + cookieMark);
                if (!string.IsNullOrEmpty(num))
                {
                    res = Convert.ToInt32(num);
                }
            }
            catch (Exception)
            {
            }
            return res;
        }
        /// <summary>
        /// 增加错误次数
        /// </summary>
        /// <returns></returns>
        public int AddCurrentErrorNum()
        {
            int res = 0;
            try
            {
                string cookieMark = GetProvider(LoginUserMarkKey);
                if (string.IsNullOrEmpty(cookieMark))
                {
                    cookieMark = Guid.NewGuid().ToString();
                    SetProvider(LoginUserMarkKey, cookieMark);
                }
                string num = RedisHelper.Get<string>(cacheKeyError + cookieMark);
                if (!string.IsNullOrEmpty(num))
                {
                    res = Convert.ToInt32(num);
                }
                res++;
                num = res + "";
                RedisHelper.Set(cacheKeyError + cookieMark, num);
            }
            catch (Exception)
            {
            }
            return res;
        }
        /// <summary>
        /// 清除当前登录错误次数
        /// </summary>
        public void ClearCurrentErrorNum()
        {
            try
            {
                string cookieMark = GetProvider(LoginUserMarkKey);
                if (string.IsNullOrEmpty(cookieMark))
                {
                    cookieMark = Guid.NewGuid().ToString();
                    SetProvider(LoginUserMarkKey, cookieMark);
                }
                RedisHelper.Del(cacheKeyError + cookieMark);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
