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
                query = query.Where(a => a.FullName.Contains(keyword) || a.EnCode.Contains(keyword));
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
            dic.Add("EnabledMark", enabledTemp);
            var setList = await itemsApp.GetItemList("RoleType");
            Dictionary<string, string> messageTypeTemp = new Dictionary<string, string>();
            foreach (var item in setList)
            {
                messageTypeTemp.Add(item.ItemCode,item.ItemName);
            }
            dic.Add("Type", messageTypeTemp);
            pagination = ChangeSoulData(dic, pagination);
            //获取数据权限
            var query =  GetQuery();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.FullName.Contains(keyword) || a.EnCode.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await query.ToPageListAsync(pagination);
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
                JoinType.Left, a.OrganizeId == b.Id
                )).Where(a => a.DeleteMark == false && a.Category == 1)
                .Select((a, b) => new RoleExtend
                {
                    Id = a.Id,
                    AllowDelete = a.AllowDelete,
                    AllowEdit = a.AllowEdit,
                    Category = a.Category,
                    CreatorTime = a.CreatorTime,
                    CreatorUserId = a.CreatorUserId,
                    Description = a.Description,
                    DeleteMark = a.DeleteMark,
                    EnabledMark = a.EnabledMark,
                    EnCode = a.EnCode,
                    FullName = a.FullName,
                    OrganizeId=a.OrganizeId,
                    SortCode=a.SortCode,
                    Type=a.Type,
                    CompanyName = b.CompanyName,
                }).MergeTable();
            return query;
        }
        public async Task DeleteForm(string keyValue)
        {
            if (await repository.Db.Queryable<UserEntity>().Where(a => a.RoleId.Contains(keyValue)).AnyAsync())
            {
                throw new Exception("角色使用中，无法删除");
            }
            unitofwork.CurrentBeginTrans();
            await repository.Delete(a => a.Id == keyValue);
            await repository.Db.Deleteable<RoleAuthorizeEntity>(a => a.ObjectId == keyValue).ExecuteCommandAsync();
            unitofwork.CurrentCommit();
            await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }
        public async Task SubmitForm(RoleEntity roleEntity, string[] permissionIds,string[] permissionfieldsIds, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Id = keyValue;

            }
            else
            {
                roleEntity.DeleteMark = false;
                roleEntity.AllowEdit = false;
                roleEntity.AllowDelete = false;
                roleEntity.Create();

            }
            var moduledata =await moduleApp.GetList();
            var buttondata =await moduleButtonApp.GetList();
            var fieldsdata = await moduleFieldsApp.GetList();
            List<RoleAuthorizeEntity> roleAuthorizeEntitys = new List<RoleAuthorizeEntity>();
            foreach (var itemId in permissionIds)
            {
                RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                roleAuthorizeEntity.Id = Utils.GuId();
                roleAuthorizeEntity.ObjectType = 1;
                roleAuthorizeEntity.ObjectId = roleEntity.Id;
                roleAuthorizeEntity.ItemId = itemId;
                if (moduledata.Find(a => a.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 1;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
                if (buttondata.Find(a => a.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 2;
                    roleAuthorizeEntitys.Add(roleAuthorizeEntity);
                }
            }
            foreach (var itemId in permissionfieldsIds)
            {
                RoleAuthorizeEntity roleAuthorizeEntity = new RoleAuthorizeEntity();
                roleAuthorizeEntity.Id = Utils.GuId();
                roleAuthorizeEntity.ObjectType = 1;
                roleAuthorizeEntity.ObjectId = roleEntity.Id;
                roleAuthorizeEntity.ItemId = itemId;
                if (fieldsdata.Find(a => a.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 3;
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
                roleEntity.Category = 1;
                await repository.Insert(roleEntity);
            }
            await repository.Db.Deleteable<RoleAuthorizeEntity>(a => a.ObjectId == roleEntity.Id).ExecuteCommandAsync();
            await repository.Db.Insertable(roleAuthorizeEntitys).ExecuteCommandAsync();
            unitofwork.CurrentCommit();
            await CacheHelper.RemoveAsync(authorizecacheKey + repository.Db.CurrentConnectionConfig.ConfigId + "_list");
        }
    }
}
