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

namespace WaterCloud.Service.SystemOrganize
{
    public class UserService : DataFilterService<UserEntity>, IDenpendency
    {
        public SystemSetService syssetApp { get; set; }
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        //获取类名
        
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<UserExtend>> GetLookList(SoulPage<UserExtend> pagination, string keyword)
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("有效", "1");
            enabledTemp.Add("无效", "0");
            dic.Add("F_EnabledMark", enabledTemp);
            Dictionary<string, string> sexTemp = new Dictionary<string, string>();
            sexTemp.Add("男", "1");
            sexTemp.Add("女", "0");
            dic.Add("F_Gender", sexTemp);
            pagination = ChangeSoulData(dic, pagination);
            //获取数据权限
            var query = GetQuery().Where(a => a.F_IsAdmin == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_Account.Contains(keyword) || a.F_RealName.Contains(keyword)||a.F_MobilePhone.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            var data = await repository.OrderList(query, pagination);
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
            var data = cachedata.OrderBy(a => a.F_Account).ToList();
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
                query = query.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword));
            }
            return await query.Where(t => t.F_EnabledMark ==true && t.F_DeleteMark == false).OrderBy(t => t.F_Account).ToListAsync();
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
            unitofwork.CurrentBeginTrans();
            await repository.Delete(t => t.F_Id == keyValue);
            await repository.Db.Deleteable<UserLogOnEntity>(t => t.F_UserId == keyValue).ExecuteCommandAsync();
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
                userLogOnEntity.F_Id= Utils.GuId();
                userLogOnEntity.F_UserId = userEntity.F_Id;
                 userLogOnEntity.F_ErrorNum = 0;
                userLogOnEntity.F_UserOnLine = false;
                userLogOnEntity.F_LogOnCount = 0;
            }
            unitofwork.CurrentBeginTrans();
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
                unitofwork.GetDbClient().ChangeDatabase("0");
                var setTemp=(await syssetApp.GetList()).Where(a=> localurl.Contains(a.F_HostUrl)).FirstOrDefault();
                if (setTemp!=null)
                {
                    unitofwork.GetDbClient().ChangeDatabase(setTemp.F_DbNumber);
                    repository = new RepositoryBase<UserEntity>(unitofwork);
                }
            }
            UserEntity userEntity =await repository.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    //缓存用户账户信息
                    var userLogOnEntity=await CacheHelper.Get<OperatorUserInfo>(cacheKeyOperator + "info_" + userEntity.F_Id);
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
                        await CacheHelper.Set(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
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
                            var rolelist =repository.Db.Queryable<RoleEntity>().Where(a=>list.Contains(a.F_Id)&&a.F_EnabledMark==true).ToList();
                            if (rolelist.Count() == 0)
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
                        await CacheHelper.Remove(cacheKeyOperator + "info_" + userEntity.F_Id);
                        await CacheHelper.Set(cacheKeyOperator + "info_" + userEntity.F_Id, userLogOnEntity);
                        await OperatorProvider.Provider.ClearCurrentErrorNum();
                        return userEntity;
                    }
                    else
                    {
                        if (userEntity.F_Account != GlobalContext.SystemConfig.SysemUserCode)
                        {
                            int num =await OperatorProvider.Provider.AddCurrentErrorNum();
                            string erornum = (5 - num).ToString();
                            if (num == 5)
                            {
                                userEntity.F_EnabledMark = false;
                                await repository.Update(userEntity);
                                await OperatorProvider.Provider.ClearCurrentErrorNum();
                                throw new Exception("密码不正确，账户被系统锁定");
                            }
                            else
                            {
                                throw new Exception("密码不正确，请重新输入，还有" + erornum + "次机会");
                            }
                        }
                        else
                        {
                            throw new Exception("密码不正确，请重新输入");
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
    }
}
