using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Service.SystemOrganize;
using WaterCloud.Service.SystemSecurity;

namespace WaterCloud.Web.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Default")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //自动注入服务
        public FilterIPService _filterIPService { get; set; }
        public UserService _userService { get; set; }
        public LogService _logService { get; set; }
        public SystemSetService _setService { get; set; }
        public IHttpContextAccessor _httpContextAccessor { get; set; }

        #region 提交数据
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AlwaysResult> Login([FromBody] LoginRequest request)
        {
            var apitoken = Utils.GuId();
            if (!string.IsNullOrEmpty(OperatorProvider.Provider.GetToken()))
            {
                apitoken = OperatorProvider.Provider.GetToken();
            }

            LogEntity logEntity = new LogEntity();
            logEntity.ModuleName = "用户Api";
            logEntity.Type = DbLogType.Login.ToString();
            try
            {
                if (!await CheckIP())
                {
                    throw new Exception("IP受限");
                }
                UserEntity userEntity = await _userService.CheckLogin(request.userName, Md5.md5(request.password, 32), request.localurl);
                OperatorModel operatorModel = new OperatorModel();
                operatorModel.UserId = userEntity.Id;
                operatorModel.UserCode = userEntity.Account;
                operatorModel.UserName = userEntity.RealName;
                operatorModel.CompanyId = userEntity.OrganizeId;
                operatorModel.DepartmentId = userEntity.DepartmentId;
                operatorModel.RoleId = userEntity.RoleId;
                operatorModel.LoginIPAddress = WebHelper.Ip;
                if (GlobalContext.SystemConfig.LocalLAN != false)
                {
                    operatorModel.LoginIPAddressName = "本地局域网";
                }
                else
                {
                    operatorModel.LoginIPAddressName = WebHelper.GetIpLocation(operatorModel.LoginIPAddress);
                }
                operatorModel.LoginTime = DateTime.Now;
                operatorModel.DdUserId = userEntity.DingTalkUserId;
                operatorModel.WxOpenId = userEntity.WxOpenId;
                operatorModel.IsAdmin = userEntity.IsAdmin.Value;
                operatorModel.IsBoss = userEntity.IsBoss.Value;
                operatorModel.IsLeaderInDepts = userEntity.IsLeaderInDepts.Value;
                operatorModel.IsSenior = userEntity.IsSenior.Value;
                SystemSetEntity setEntity = await _setService.GetForm(userEntity.OrganizeId);
                operatorModel.DbNumber = setEntity.DbNumber;
                if (operatorModel.IsAdmin && operatorModel.DbNumber == GlobalContext.SystemConfig.MainDbNumber)
                {
                    operatorModel.IsSuperAdmin = true;
                }
                else
                {
                    operatorModel.IsSuperAdmin = false;
                }
                await OperatorProvider.Provider.AddLoginUser(operatorModel, apitoken, "api_");
                logEntity.Account = userEntity.Account;
                logEntity.NickName = userEntity.RealName;
                logEntity.Result = true;
                logEntity.Description = "登录成功";
                await _logService.WriteDbLog(logEntity);
                // 设置刷新Token令牌
                _httpContextAccessor.HttpContext.Response.Headers[GlobalContext.SystemConfig.TokenName] = apitoken;
                return new AlwaysResult<string> { state = ResultType.success.ToString(), message = "登录成功。", data = apitoken };
            }
            catch (Exception ex)
            {
                logEntity.Account = request.userName;
                logEntity.NickName = request.userName;
                logEntity.Result = false;
                logEntity.Description = "登录失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return new AlwaysResult<string> { state = ResultType.error.ToString(), message = ex.Message, data = apitoken };
            }
        }
        private async Task<bool> CheckIP()
        {
            string ip = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
            return await _filterIPService.CheckIP(ip);
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [LoginFilter]
        public async Task<AlwaysResult> LoginOff()
        {
            await _logService.WriteDbLog(new LogEntity
            {
                ModuleName = "用户Api",
                Type = DbLogType.Exit.ToString(),
                Account = _userService.currentuser.UserCode,
                NickName = _userService.currentuser.UserName,
                Result = true,
                Description = "安全退出系统",
            });
            await OperatorProvider.Provider.EmptyCurrent("api_");
            return new AlwaysResult { state = ResultType.success.ToString() };
        }
        #endregion

        #region 请求对象
        /// <summary>
        /// 登录请求对象
        /// </summary>
        public class LoginRequest
        {
            /// <summary>
            /// 用户名
            /// </summary>
            [Required(ErrorMessage = "用户名不能为空")]
            public string userName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            [Required(ErrorMessage = "密码不能为空")]
            public string password { get; set; }
            /// <summary>
            /// 域名
            /// </summary>
            public string localurl { get; set; }

        }
        #endregion
    }
}
