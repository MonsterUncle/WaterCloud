/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using WaterCloud.DataBase;
using WaterCloud.Service.SystemSecurity;
using WaterCloud.Domain.SystemSecurity;

namespace WaterCloud.Service.SystemOrganize
{
    public class UserService : BaseService<UserEntity>, IDenpendency
    {
        public SystemSetService syssetApp { get; set; }
        public FilterIPService ipApp { get; set; }
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKeyOperator = GlobalContext.SystemConfig.ProjectPrefix + "_operator_";// +登录者token
                                                                                                  //获取类名

        public UserService(ISqlSugarClient context) : base(context)
		{
        }

        public async Task<List<UserExtend>> GetLookList(SoulPage<UserExtend> pagination, string keyword)
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("F_EnabledMark", enabledTemp);
            Dictionary<string, string> sexTemp = new Dictionary<string, string>();
            sexTemp.Add("1", "男");
            sexTemp.Add("0", "女");
            dic.Add("F_Gender", sexTemp);
            pagination = ChangeSoulData(dic, pagination);
            //获取数据权限
            var query = GetQuery().Where(a => a.F_IsAdmin == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_Account.Contains(keyword) || a.F_RealName.Contains(keyword)||a.F_MobilePhone.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            var data = await query.ToPageListAsync(pagination);
            var roles = repository.Db.Queryable<RoleEntity>().ToList();
            var orgs = repository.Db.Queryable<OrganizeEntity>().ToList();
            foreach (var item in data)
            {
                string[] roleIds = item.F_RoleId.Split(',');
                string[] departmentIds = item.F_DepartmentId.Split(',');
                item.F_DepartmentName = string.Join(',', orgs.Where(a => departmentIds.Contains(a.F_Id)).Select(a => a.F_FullName).ToList());
                item.F_RoleName = string.Join(',', roles.Where(a => roleIds.Contains(a.F_Id)).Select(a => a.F_FullName).ToList());
            }
            return data;
        }
        public async Task<List<UserExtend>> GetList(string keyword)
        {
            var cachedata = GetQuery().Where(a => a.F_IsAdmin == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(a => a.F_Account.Contains(keyword) || a.F_RealName.Contains(keyword) || a.F_MobilePhone.Contains(keyword));
            }
            var data = await cachedata.OrderBy(a => a.F_Account).ToListAsync();
            var roles = await repository.Db.Queryable<RoleEntity>().ToListAsync();
            var orgs = await repository.Db.Queryable<OrganizeEntity>().ToListAsync();
            foreach (var item in data)
            {
                string[] roleIds = item.F_RoleId.Split(',');
                string[] departmentIds = item.F_DepartmentId.Split(',');
                item.F_DepartmentName = string.Join(',', orgs.Where(a => departmentIds.Contains(a.F_Id)).Select(a => a.F_FullName).ToList());
                item.F_RoleName = string.Join(',', roles.Where(a => roleIds.Contains(a.F_Id)).Select(a => a.F_FullName).ToList());
            }
            return data;
        }
        private ISugarQueryable<UserExtend> GetQuery()
        {
            var query = repository.Db.Queryable<UserEntity, RoleEntity, SystemSetEntity>((a,b,c)=>new JoinQueryInfos(
                JoinType.Left, a.F_DutyId == b.F_Id,
                JoinType.Left, a.F_OrganizeId == c.F_Id
                )).Where(a => a.F_DeleteMark == false)
                .Select((a, b,c) => new UserExtend
                {
                    F_Id = a.F_Id,
                    F_IsSenior=a.F_IsSenior,
                    F_SecurityLevel=a.F_SecurityLevel,
                    F_Account=a.F_Account,
                    F_DingTalkAvatar=a.F_DingTalkAvatar,
                    F_IsAdmin=a.F_IsAdmin,
                    F_Birthday=a.F_Birthday,
                    F_CompanyName=c.F_CompanyName,
                    F_CreatorTime=a.F_CreatorTime,
                    F_CreatorUserId=a.F_CreatorUserId,  
                    F_DepartmentId=a.F_DepartmentId,
                    F_Description=a.F_Description,
                    F_DingTalkUserId=a.F_DingTalkUserId,
                    F_DingTalkUserName=a.F_DingTalkUserName,
                    F_DutyId=a.F_DutyId,
                    F_DutyName=b.F_FullName,
                    F_Email=a.F_Email,
                    F_EnabledMark=a.F_EnabledMark,
                    F_Gender=a.F_Gender,
                    F_HeadIcon=a.F_HeadIcon,
                    F_HeadImgUrl=a.F_HeadImgUrl,
                    F_IsBoss=a.F_IsBoss,
                    F_IsLeaderInDepts=a.F_IsLeaderInDepts,
                    F_ManagerId=a.F_ManagerId,
                    F_MobilePhone=a.F_MobilePhone,
                    F_NickName=a.F_NickName,
                    F_OrganizeId=a.F_OrganizeId,
                    F_RealName=a.F_RealName,
                    F_Remark=a.F_RealName,
                    F_RoleId=a.F_RoleId,
                    F_Signature=a.F_Signature,
                    F_SortCode=a.F_SortCode,
                    F_WeChat=a.F_WeChat,
                    F_WxNickName=a.F_WxNickName,
                    F_WxOpenId=a.F_WxOpenId,
                    F_DepartmentName="",
                    F_RoleName=""
                }).MergeTable();
            return query;
        }
        public async Task SubmitUserForm(UserEntity userEntity)
        {
            await repository.Update(userEntity);
        }

        public async Task<List<UserEntity>> GetUserList(string keyword)
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_Account.Contains(keyword) || a.F_RealName.Contains(keyword) || a.F_MobilePhone.Contains(keyword));
            }
            return await query.Where(a => a.F_EnabledMark ==true && a.F_DeleteMark == false).OrderBy(a => a.F_Account).ToListAsync();
        }

        public async Task<UserEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task<UserEntity> GetFormExtend(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            string[] temp;
            if (data.F_RoleId != null)
            {
                temp = data.F_RoleId.Split(',');
                data.F_RoleName = string.Join(",", repository.Db.Queryable<RoleEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }
            if (data.F_DepartmentId != null)
            {
                temp = data.F_DepartmentId.Split(',');
                data.F_DepartmentName = string.Join(",", repository.Db.Queryable<OrganizeEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }
            return data;
        }
        public async Task<UserEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            string[] temp;
            if (data.F_RoleId != null)
            {
                temp = data.F_RoleId.Split(',');
                data.F_RoleName = string.Join(",", repository.Db.Queryable<RoleEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }
            if (data.F_DepartmentId != null)
            {
                temp = data.F_DepartmentId.Split(',');
                data.F_DepartmentName = string.Join(",", repository.Db.Queryable<OrganizeEntity>().Where(a => temp.Contains(a.F_Id)).Select(a => a.F_FullName).ToList().ToArray());
            }
            return GetFieldsFilterData(data);
        }
        public async Task DeleteForm(string keyValue)
        {
			repository.Db.Ado.BeginTran();
			await repository.Delete(a => a.F_Id == keyValue);
            await repository.Db.Deleteable<UserLogOnEntity>(a => a.F_UserId == keyValue).ExecuteCommandAsync();
			repository.Db.Ado.CommitTran();
		}
        public async Task SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
            }
            else
            {
                userEntity.Create();
                userLogOnEntity.F_Id= Utils.GuId();
                userLogOnEntity.F_UserId = userEntity.F_Id;
                 userLogOnEntity.F_ErrorNum = 0;
                userLogOnEntity.F_UserOnLine = false;
                userLogOnEntity.F_LogOnCount = 0;
            }
			repository.Db.Ado.BeginTran();
			if (!string.IsNullOrEmpty(keyValue))
            {
                await repository.Update(userEntity);
            }
            else
            {
                userLogOnEntity.F_Id = userEntity.F_Id;
                userLogOnEntity.F_UserId = userEntity.F_Id;
                userLogOnEntity.F_UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userLogOnEntity.F_UserPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                await repository.Insert(userEntity);
                await repository.Db.Insertable(userLogOnEntity).ExecuteCommandAsync();
            }
			repository.Db.Ado.CommitTran();
		}
        public async Task UpdateForm(UserEntity userEntity)
        {
            await repository.Update(userEntity);
        }
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserEntity> CheckLogin(string username, string password, string localurl)
        {
            //根据登录公司查找公司
            if (GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT)
            {
                repository.ChangeEntityDb(GlobalContext.SystemConfig.MainDbNumber);
                var setTemp=(await syssetApp.GetList()).Where(a=> localurl.Contains(a.F_HostUrl)).FirstOrDefault();
                if (setTemp!=null)
                {
					if (setTemp.F_EndTime<DateTime.Now.Date)
					{
                        throw new Exception("租户已到期，请联系供应商");
                    }
					if (!_context.AsTenant().IsAnyConnection(setTemp.F_DbNumber))
					{
						var dblist = DBInitialize.GetConnectionConfigs(true);
						_context.AsTenant().AddConnection(dblist.FirstOrDefault(a => a.ConfigId == setTemp.F_DbNumber));
						repository.ChangeEntityDb(setTemp.F_DbNumber);
						(repository.Db as SqlSugarProvider).DefaultConfig();
                    }
                    else
                    {
						repository.ChangeEntityDb(setTemp.F_DbNumber);
					}
				}
            }
            if (!(await CheckIP()))
            {
                throw new Exception("IP受限");
			}
            UserEntity userEntity =await repository.IQueryable().SingleAsync(a => a.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    //缓存用户账户信息
                    var userLogOnEntity=await CacheHelper.GetAsync<OperatorUserInfo>(cacheKeyOperator + "info_" + userEntity.F_Id);
                    if (userLogOnEntity==null)
                    {
                        userLogOnEntity = new OperatorUserInfo();
                        UserLogOnEntity entity =await repository.Db.Queryable<UserLogOnEntity>().InSingleAsync(userEntity.F_Id);
                        userLogOnEntity.F_UserPassword = entity.F_UserPassword;
                        userLogOnEntity.F_UserSecretkey = entity.F_UserSecretkey;
                        userLogOnEntity.F_AllowEndTime = entity.F_AllowEndTime;
                        userLogOnEntity.F_AllowStartTime = entity.F_AllowStartTime;
                        userLogOnEntity.F_AnswerQuestion = entity.F_AnswerQuestion;
                        userLogOnEntity.F_ChangePasswordDate = entity.F_ChangePasswordDate;
                        userLogOnEntity.F_FirstVisitTime = entity.F_FirstVisitTime;
                        userLogOnEntity.F_LastVisitTime = entity.F_LastVisitTime;
                        userLogOnEntity.F_LockEndDate = entity.F_LockEndDate;
                        userLogOnEntity.F_LockStartDate = entity.F_LockStartDate;
                        userLogOnEntity.F_LogOnCount = entity.F_LogOnCount;
                        userLogOnEntity.F_PreviousVisitTime = entity.F_PreviousVisitTime;
                        userLogOnEntity.F_Question = entity.F_Question;
                        userLogOnEntity.F_Theme = entity.F_Theme;
                        await CacheHelper.SetAsync(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
                    }
                    if (userLogOnEntity == null)
                    {
                        throw new Exception("账户未初始化设置密码,请联系管理员");
                    }
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        if (userEntity.F_IsAdmin != true)
                        {
                            var list = userEntity.F_RoleId.Split(',');
                            var rolelist = repository.Db.Queryable<RoleEntity>().Where(a=>list.Contains(a.F_Id)&&a.F_EnabledMark==true).ToList();
                            if (!rolelist.Any())
                            {
                                throw new Exception("账户未设置权限,请联系管理员");
                            }
                        }
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.F_LastVisitTime != null)
                        {
                            userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
                        }
                        userLogOnEntity.F_LastVisitTime = lastVisitTime;
                        userLogOnEntity.F_LogOnCount = LogOnCount;
                        userLogOnEntity.F_UserOnLine = true;
                        await CacheHelper.RemoveAsync(cacheKeyOperator + "info_" + userEntity.F_Id);
                        await CacheHelper.SetAsync(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
                        await OperatorProvider.Provider.ClearCurrentErrorNum();
                        return userEntity;
                    }
                    else
                    {
                        //登录错误不超过指定次数
                        int num = await OperatorProvider.Provider.AddCurrentErrorNum();
                        int errorcount = GlobalContext.SystemConfig.LoginErrorCount ?? 4;
                        string erornum = (errorcount - num).ToString();
                        if (num == errorcount)
                        {
                            FilterIPEntity ipentity = new FilterIPEntity();
                            ipentity.F_Id = Utils.GuId();
                            ipentity.F_StartIP = WebHelper.Ip;
                            ipentity.F_CreatorTime = DateTime.Now;
                            ipentity.F_DeleteMark = false;
                            ipentity.F_EnabledMark = true;
                            ipentity.F_Type = false;
                            //默认封禁12小时
                            ipentity.F_EndTime = DateTime.Now.AddHours(12);
                            await ipApp.SubmitForm(ipentity,null);
                            await OperatorProvider.Provider.ClearCurrentErrorNum();
                            throw new Exception("密码不正确，IP被锁定");
                        }
                        else
                        {
                            throw new Exception("密码不正确，请重新输入，还有" + erornum + "次机会");
                        }
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }

        private async Task<bool> CheckIP()
        {
            string ip = WebHelper.Ip;
            return await ipApp.CheckIP(ip);
        }
    }
}
