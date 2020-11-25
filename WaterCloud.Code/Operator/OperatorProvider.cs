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
        private ICache redisCache = CacheFactory.CaChe();
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

        public OperatorModel GetCurrent()
        {
            OperatorModel operatorModel = new OperatorModel();
            try
            {
                string token = WebHelper.GetCookie(LoginUserToken).ToString();
                string loginMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
                operatorModel = redisCache.Read<OperatorModel>(cacheKeyOperator + loginMark, CacheId.loginInfo);
            }
            catch
            {
                operatorModel = null;
            }
            return operatorModel;
        }
        public void RemoveCurrent()
        {

        }
        /// <summary>
        /// 获取浏览器设配号
        /// </summary>
        /// <returns></returns>
        public string GetMark()
        {
            string cookieMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
            if (string.IsNullOrEmpty(cookieMark))
            {
                cookieMark = Guid.NewGuid().ToString();
                WebHelper.WriteCookie(LoginUserMarkKey, cookieMark);
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
                    string cookieMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
                    if (string.IsNullOrEmpty(cookieMark))
                    {
                        operatorModel.loginMark = Guid.NewGuid().ToString();
                        WebHelper.WriteCookie(LoginUserMarkKey, operatorModel.loginMark);
                    }
                    else
                    {
                        operatorModel.loginMark = cookieMark;
                    }
                    WebHelper.WriteCookie(LoginUserToken, token);
                }
                else
                {
                    operatorModel.loginMark = loginMark;
                }
                //redis 登录token列表更新
                Dictionary<string, string> tokenMarkList = redisCache.Read<Dictionary<string, string>>(cacheKeyToken + operatorModel.UserId, CacheId.loginInfo);
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

                redisCache.Write<Dictionary<string, string>>(cacheKeyToken + operatorModel.UserId, tokenMarkList, CacheId.loginInfo);
                redisCache.Write<OperatorModel>(cacheKeyOperator + operatorModel.loginMark, operatorModel, CacheId.loginInfo);
                redisCache.Remove(cacheKeyOperator + facilityMark + operatorModel.UserId, CacheId.loginInfo);
                redisCache.Write<string>(cacheKeyOperator + facilityMark + operatorModel.UserId, token, CacheId.loginInfo);
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
                string token = WebHelper.GetCookie(LoginUserToken).ToString();
                string loginMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
                EmptyCurrent(token, loginMark);
                WebHelper.RemoveCookie(LoginUserMarkKey.Trim());
                WebHelper.RemoveCookie(LoginUserToken.Trim());
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
                OperatorModel operatorInfo = redisCache.Read<OperatorModel>(cacheKeyOperator + loginMark, CacheId.loginInfo);
                if (operatorInfo != null && operatorInfo.LoginToken == token)
                {
                    Dictionary<string, string> tokenMarkList = redisCache.Read<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId, CacheId.loginInfo);
                    tokenMarkList.Remove(loginMark);
                    redisCache.Remove(cacheKeyOperator + loginMark, CacheId.loginInfo);
                    redisCache.Write<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId, tokenMarkList, CacheId.loginInfo);
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
                string token = WebHelper.GetCookie(LoginUserToken).ToString();
                string loginMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
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
                OperatorModel operatorInfo = redisCache.Read<OperatorModel>(cacheKeyOperator + loginMark, CacheId.loginInfo);
                if (operatorInfo != null && operatorInfo.LoginToken == token)
                {
                    TimeSpan span = (TimeSpan)(DateTime.Now - operatorInfo.LoginTime);
                    //超时
                    if (span.TotalHours >= 12)// 登录操作过12小时移除
                    {
                        operatorResult.stateCode = 0;
                        Dictionary<string, string> tokenMarkList = redisCache.Read<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId, CacheId.loginInfo);
                        tokenMarkList.Remove(loginMark);
                        redisCache.Write<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId, tokenMarkList, CacheId.loginInfo);
                        redisCache.Remove(cacheKeyOperator + loginMark, CacheId.loginInfo);
                    }
                    //账号被顶(排除admin)
                    else if (!operatorInfo.IsSystem&&token != redisCache.Read<string>(cacheKeyOperator + facilityMark + operatorInfo.UserId, CacheId.loginInfo))
                    {
                        operatorResult.stateCode = -2;
                        Dictionary<string, string> tokenMarkList = redisCache.Read<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId, CacheId.loginInfo);
                        tokenMarkList.Remove(loginMark);
                        redisCache.Write<Dictionary<string, string>>(cacheKeyToken + operatorInfo.UserId, tokenMarkList, CacheId.loginInfo);
                        redisCache.Remove(cacheKeyOperator + loginMark, CacheId.loginInfo);
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
                string cookieMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
                if (string.IsNullOrEmpty(cookieMark))
                {
                    cookieMark = Guid.NewGuid().ToString();
                    WebHelper.WriteCookie(LoginUserMarkKey, cookieMark);
                }
                string num = redisCache.Read<string>(cacheKeyError + cookieMark, CacheId.loginInfo);
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
                string cookieMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
                if (string.IsNullOrEmpty(cookieMark))
                {
                    cookieMark = Guid.NewGuid().ToString();
                    WebHelper.WriteCookie(LoginUserMarkKey, cookieMark);
                }
                string num = redisCache.Read<string>(cacheKeyError + cookieMark, CacheId.loginInfo);
                if (!string.IsNullOrEmpty(num))
                {
                    res = Convert.ToInt32(num);
                }
                res++;
                num = res + "";
                redisCache.Write<string>(cacheKeyError + cookieMark, num, CacheId.loginInfo);
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
                string cookieMark = WebHelper.GetCookie(LoginUserMarkKey).ToString();
                if (string.IsNullOrEmpty(cookieMark))
                {
                    cookieMark = Guid.NewGuid().ToString();
                    WebHelper.WriteCookie(LoginUserMarkKey, cookieMark);
                }
                redisCache.Remove(cacheKeyError + cookieMark, CacheId.loginInfo);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
