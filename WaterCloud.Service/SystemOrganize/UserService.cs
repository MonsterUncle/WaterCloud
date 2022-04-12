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
    public class UserService : DataFilterService<UserEntity>, IDenpendency
    {
        public SystemSetService syssetApp { get; set; }
        public FilterIPService ipApp { get; set; }
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKeyOperator = GlobalContext.SystemConfig.ProjectPrefix + "_operator_";// +登录者token
        //获取类名
        
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<UserExtend>> GetLookList(SoulPage<UserExtend> pagination, string keyword)
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("EnabledMark", enabledTemp);
            Dictionary<string, string> sexTemp = new Dictionary<string, string>();
            sexTemp.Add("1", "男");
            sexTemp.Add("0", "女");
            dic.Add("Gender", sexTemp);
            pagination = ChangeSoulData(dic, pagination);
            //获取数据权限
            var query = GetQuery().Where(a => a.IsAdmin == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.Account.Contains(keyword) || a.RealName.Contains(keyword)||a.MobilePhone.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            var data = await query.ToPageListAsync(pagination);
            var roles = repository.Db.Queryable<RoleEntity>().ToList();
            var orgs = repository.Db.Queryable<OrganizeEntity>().ToList();
            foreach (var item in data)
            {
                string[] roleIds = item.RoleId.Split(',');
                string[] departmentIds = item.DepartmentId.Split(',');
                item.DepartmentName = string.Join(',', orgs.Where(a => departmentIds.Contains(a.Id)).Select(a => a.FullName).ToList());
                item.RoleName = string.Join(',', roles.Where(a => roleIds.Contains(a.Id)).Select(a => a.FullName).ToList());
            }
            return data;
        }
        public async Task<List<UserExtend>> GetList(string keyword)
        {
            var cachedata = GetQuery().Where(a => a.IsAdmin == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(a => a.Account.Contains(keyword) || a.RealName.Contains(keyword) || a.MobilePhone.Contains(keyword));
            }
            var data = await cachedata.OrderBy(a => a.Account).ToListAsync();
            var roles = await repository.Db.Queryable<RoleEntity>().ToListAsync();
            var orgs = await repository.Db.Queryable<OrganizeEntity>().ToListAsync();
            foreach (var item in data)
            {
                string[] roleIds = item.RoleId.Split(',');
                string[] departmentIds = item.DepartmentId.Split(',');
                item.DepartmentName = string.Join(',', orgs.Where(a => departmentIds.Contains(a.Id)).Select(a => a.FullName).ToList());
                item.RoleName = string.Join(',', roles.Where(a => roleIds.Contains(a.Id)).Select(a => a.FullName).ToList());
            }
            return data;
        }
        private ISugarQueryable<UserExtend> GetQuery()
        {
            var query = repository.Db.Queryable<UserEntity, RoleEntity, SystemSetEntity>((a,b,c)=>new JoinQueryInfos(
                JoinType.Left, a.DutyId == b.Id,
                JoinType.Left, a.OrganizeId == c.Id
                )).Where(a => a.DeleteMark == false)
                .Select((a, b,c) => new UserExtend
                {
                    Id = a.Id,
                    IsSenior=a.IsSenior,
                    SecurityLevel=a.SecurityLevel,
                    Account=a.Account,
                    DingTalkAvatar=a.DingTalkAvatar,
                    IsAdmin=a.IsAdmin,
                    Birthday=a.Birthday,
                    CompanyName=c.CompanyName,
                    CreatorTime=a.CreatorTime,
                    CreatorUserId=a.CreatorUserId,  
                    DepartmentId=a.DepartmentId,
                    Description=a.Description,
                    DingTalkUserId=a.DingTalkUserId,
                    DingTalkUserName=a.DingTalkUserName,
                    DutyId=a.DutyId,
                    DutyName=b.FullName,
                    Email=a.Email,
                    EnabledMark=a.EnabledMark,
                    Gender=a.Gender,
                    HeadIcon=a.HeadIcon,
                    HeadImgUrl=a.HeadImgUrl,
                    IsBoss=a.IsBoss,
                    IsLeaderInDepts=a.IsLeaderInDepts,
                    ManagerId=a.ManagerId,
                    MobilePhone=a.MobilePhone,
                    NickName=a.NickName,
                    OrganizeId=a.OrganizeId,
                    RealName=a.RealName,
                    Remark=a.RealName,
                    RoleId=a.RoleId,
                    Signature=a.Signature,
                    SortCode=a.SortCode,
                    WeChat=a.WeChat,
                    WxNickName=a.WxNickName,
                    WxOpenId=a.WxOpenId,
                    DepartmentName="",
                    RoleName=""
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
                query = query.Where(a => a.Account.Contains(keyword) || a.RealName.Contains(keyword) || a.MobilePhone.Contains(keyword));
            }
            return await query.Where(a => a.EnabledMark ==true && a.DeleteMark == false).OrderBy(a => a.Account).ToListAsync();
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
            if (data.RoleId != null)
            {
                temp = data.RoleId.Split(',');
                data.RoleName = string.Join(",", repository.Db.Queryable<RoleEntity>().Where(a => temp.Contains(a.Id)).Select(a => a.FullName).ToList().ToArray());
            }
            if (data.DepartmentId != null)
            {
                temp = data.DepartmentId.Split(',');
                data.DepartmentName = string.Join(",", repository.Db.Queryable<OrganizeEntity>().Where(a => temp.Contains(a.Id)).Select(a => a.FullName).ToList().ToArray());
            }
            return data;
        }
        public async Task<UserEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            string[] temp;
            if (data.RoleId != null)
            {
                temp = data.RoleId.Split(',');
                data.RoleName = string.Join(",", repository.Db.Queryable<RoleEntity>().Where(a => temp.Contains(a.Id)).Select(a => a.FullName).ToList().ToArray());
            }
            if (data.DepartmentId != null)
            {
                temp = data.DepartmentId.Split(',');
                data.DepartmentName = string.Join(",", repository.Db.Queryable<OrganizeEntity>().Where(a => temp.Contains(a.Id)).Select(a => a.FullName).ToList().ToArray());
            }
            return GetFieldsFilterData(data);
        }
        public async Task DeleteForm(string keyValue)
        {
            unitofwork.CurrentBeginTrans();
            await repository.Delete(a => a.Id == keyValue);
            await repository.Db.Deleteable<UserLogOnEntity>(a => a.UserId == keyValue).ExecuteCommandAsync();
            unitofwork.CurrentCommit();
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
                userLogOnEntity.Id= Utils.GuId();
                userLogOnEntity.UserId = userEntity.Id;
                 userLogOnEntity.ErrorNum = 0;
                userLogOnEntity.UserOnLine = false;
                userLogOnEntity.LogOnCount = 0;
            }
            unitofwork.CurrentBeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                await repository.Update(userEntity);
            }
            else
            {
                userLogOnEntity.Id = userEntity.Id;
                userLogOnEntity.UserId = userEntity.Id;
                userLogOnEntity.UserSecretkey = Md5.md5(Utils.CreateNo(), 16).ToLower();
                userLogOnEntity.UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userLogOnEntity.UserPassword, 32).ToLower(), userLogOnEntity.UserSecretkey).ToLower(), 32).ToLower();
                await repository.Insert(userEntity);
                await repository.Db.Insertable(userLogOnEntity).ExecuteCommandAsync();
            }
            unitofwork.CurrentCommit();
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
        public async Task<UserEntity> CheckLogin(string username, string password,string localurl)
        {
            //根据登录公司查找公司
            if (GlobalContext.SystemConfig.SqlMode == Define.SQL_TENANT)
            {
                unitofwork.GetDbClient().ChangeDatabase(GlobalContext.SystemConfig.MainDbNumber);
                var setTemp=(await syssetApp.GetList()).Where(a=> localurl.Contains(a.HostUrl)).FirstOrDefault();
                if (setTemp!=null)
                {
					if (setTemp.EndTime<DateTime.Now.Date)
					{
                        throw new Exception("租户已到期，请联系供应商");
                    }
                    unitofwork.GetDbClient().ChangeDatabase(setTemp.DbNumber);
                    repository = new RepositoryBase<UserEntity>(unitofwork);
                }
            }
			if (!(await CheckIP()))
			{
                throw new Exception("IP受限");
			}
            UserEntity userEntity =await repository.FindEntity(a => a.Account == username);
            if (userEntity != null)
            {
                if (userEntity.EnabledMark == true)
                {
                    //缓存用户账户信息
                    var userLogOnEntity=await CacheHelper.GetAsync<OperatorUserInfo>(cacheKeyOperator + "info_" + userEntity.Id);
                    if (userLogOnEntity==null)
                    {
                        userLogOnEntity = new OperatorUserInfo();
                        UserLogOnEntity entity =await repository.Db.Queryable<UserLogOnEntity>().InSingleAsync(userEntity.Id);
                        userLogOnEntity.UserPassword = entity.UserPassword;
                        userLogOnEntity.UserSecretkey = entity.UserSecretkey;
                        userLogOnEntity.AllowEndTime = entity.AllowEndTime;
                        userLogOnEntity.AllowStartTime = entity.AllowStartTime;
                        userLogOnEntity.AnswerQuestion = entity.AnswerQuestion;
                        userLogOnEntity.ChangePasswordDate = entity.ChangePasswordDate;
                        userLogOnEntity.FirstVisitTime = entity.FirstVisitTime;
                        userLogOnEntity.LastVisitTime = entity.LastVisitTime;
                        userLogOnEntity.LockEndDate = entity.LockEndDate;
                        userLogOnEntity.LockStartDate = entity.LockStartDate;
                        userLogOnEntity.LogOnCount = entity.LogOnCount;
                        userLogOnEntity.PreviousVisitTime = entity.PreviousVisitTime;
                        userLogOnEntity.Question = entity.Question;
                        userLogOnEntity.Theme = entity.Theme;
                        await CacheHelper.SetAsync(cacheKeyOperator + "info_" + userEntity.Id, userLogOnEntity);
                    }
                    if (userLogOnEntity == null)
                    {
                        throw new Exception("账户未初始化设置密码,请联系管理员");
                    }
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.UserPassword)
                    {
                        if (userEntity.IsAdmin != true)
                        {
                            var list = userEntity.RoleId.Split(',');
                            var rolelist =repository.Db.Queryable<RoleEntity>().Where(a=>list.Contains(a.Id)&&a.EnabledMark==true).ToList();
                            if (!rolelist.Any())
                            {
                                throw new Exception("账户未设置权限,请联系管理员");
                            }
                        }
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.LastVisitTime != null)
                        {
                            userLogOnEntity.PreviousVisitTime = userLogOnEntity.LastVisitTime.ToDate();
                        }
                        userLogOnEntity.LastVisitTime = lastVisitTime;
                        userLogOnEntity.LogOnCount = LogOnCount;
                        userLogOnEntity.UserOnLine = true;
                        await CacheHelper.RemoveAsync(cacheKeyOperator + "info_" + userEntity.Id);
                        await CacheHelper.SetAsync(cacheKeyOperator + "info_" + userEntity.Id, userLogOnEntity);
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
                            ipentity.Id = Utils.GuId();
                            ipentity.StartIP = WebHelper.Ip;
                            ipentity.CreatorTime = DateTime.Now;
                            ipentity.DeleteMark = false;
                            ipentity.EnabledMark = true;
                            ipentity.Type = false;
                            //默认封禁12小时
                            ipentity.EndTime = DateTime.Now.AddHours(12);
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
