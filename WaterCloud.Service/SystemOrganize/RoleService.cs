/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using WaterCloud.Service.SystemManage;
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    public class RoleService : DataFilterService<RoleEntity>, IDenpendency
    {
        public ModuleService moduleApp { get; set; }
        public ModuleButtonService moduleButtonApp { get; set; }
        public ModuleFieldsService moduleFieldsApp { get; set; }
        public ItemsDataService itemsApp { get; set; }
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string authorizecacheKey = GlobalContext.SystemConfig.ProjectPrefix + "_authorizeurldata_";// +权限
        //获取类名
        
        public RoleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<RoleExtend>> GetList( string keyword = "")
        {
            var query = GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_FullName.Contains(keyword) || a.F_EnCode.Contains(keyword));
            }
            return await query.ToListAsync();
        }
        public async Task<List<RoleExtend>> GetLookList(SoulPage<RoleExtend> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("1", "有效");
            enabledTemp.Add("0", "无效");
            dic.Add("F_EnabledMark", enabledTemp);
            var setList = await itemsApp.GetItemList("RoleType");
            Dictionary<string, string> messageTypeTemp = new Dictionary<string, string>();
            foreach (var item in setList)
            {
                messageTypeTemp.Add(item.F_ItemCode,item.F_ItemName);
            }
            dic.Add("F_Type", messageTypeTemp);
            pagination = ChangeSoulData(dic, pagination);
            //获取数据权限
            var query =  GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_FullName.Contains(keyword) || a.F_EnCode.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await repository.OrderList(query, pagination);
        }
        public async Task<RoleEntity> GetForm(string keyValue)
        {
            var data =await repository.FindEntity(keyValue);
            return data;
        }
        public async Task<RoleEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }
        private ISugarQueryable<RoleExtend> GetQuery()
        {
            var query = repository.Db.Queryable<RoleEntity, SystemSetEntity>((a,b)=>new JoinQueryInfos(
                JoinType.Left, a.F_OrganizeId == b.F_Id
                )).Where(a => a.F_DeleteMark == false && a.F_Category == 1)
                .Select((a, b) => new RoleExtend
                {
                    F_Id = a.F_Id,
                    F_AllowDelete = a.F_AllowDelete,
                    F_AllowEdit = a.F_AllowEdit,
                    F_Category = a.F_Category,
                    F_CreatorTime = a.F_CreatorTime,
                    F_CreatorUserId = a.F_CreatorUserId,
                    F_Description = a.F_Description,
                    F_DeleteMark = a.F_DeleteMark,
                    F_EnabledMark = a.F_EnabledMark,
                    F_EnCode = a.F_EnCode,
                    F_FullName = a.F_FullName,
                    F_OrganizeId=a.F_OrganizeId,
                    F_SortCode=a.F_SortCode,
                    F_Type=a.F_Type,
                    F_CompanyName = b.F_CompanyName,
                }).MergeTable();
            return query;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (repository.Db.Queryable<UserEntity>().Where(a => a.F_RoleId.Contains(keyValue)).Count() > 0 )
            {
                throw new Exception("角色使用中，无法删除");
            }
            unitofwork.CurrentBeginTrans();
            await repository.Delete(a => a.F_Id == keyValue);
            await repository.Db.Deleteable<RoleAuthorizeEntity>(a => a.F_ObjectId == keyValue).ExecuteCommandAsync();
            unitofwork.CurrentCommit();
            await CacheHelper.Remove(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }
        public async Task SubmitForm(RoleEntity roleEntity, string[] permissionIds,string[] permissionfieldsIds, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.F_Id = keyValue;

            }
            else
            {
                roleEntity.F_DeleteMark = false;
                roleEntity.F_AllowEdit = false;
                roleEntity.F_AllowDelete = false;
                roleEntity.Create();

            }
            var moduledata =await moduleApp.GetList();
            var buttondata =await moduleButtonApp.GetList();
            var fieldsdata = await moduleFieldsApp.GetList();
            List<RoleAuthorizeEntity> roleAuthorizeEntitys = new List<RoleAuthorizeEntity>();
            foreach (var itemId in permissionIds)
            {
                RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                roleAuthorizeEntity.F_Id = Utils.GuId();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (moduledata.Find(a => a.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 1;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
                if (buttondata.Find(a => a.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 2;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
            }
            foreach (var itemId in permissionfieldsIds)
            {
                RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                roleAuthorizeEntity.F_Id = Utils.GuId();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (fieldsdata.Find(a => a.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 3;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
            }
            unitofwork.CurrentBeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                await repository.Update(roleEntity);
            }
            else
            {
                roleEntity.F_Category = 1;
                await repository.Insert(roleEntity);
            }
            await repository.Db.Deleteable<RoleAuthorizeEntity>(a => a.F_ObjectId == roleEntity.F_Id).ExecuteCommandAsync();
            await repository.Db.Insertable(roleAuthorizeEntitys).ExecuteCommandAsync();
            unitofwork.CurrentCommit();
            await CacheHelper.Remove(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }
    }
}
