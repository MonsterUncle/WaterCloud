//-----------------------------------------------------------------------
// <copyright file=" QuickModule.cs" company="JR">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: QuickModule.cs
// * history : Created by T4 04/13/2020 16:51:13 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.Domain.SystemManage;
using System.Collections.Generic;
using WaterCloud.Code;
using System.Threading.Tasks;
using System.Linq;
using SqlSugar;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemManage
{
    public class QuickModuleService:BaseService<QuickModuleEntity> ,IDenpendency
    {
        public QuickModuleService(ISqlSugarClient context) : base(context)
		{
        }
        public async Task<object> GetTransferList(string userId)
        {
            var quicklist = repository.IQueryable(a => a.F_CreatorUserId == userId && a.F_EnabledMark == true).ToList();
            List<ModuleEntity> quicks = new List<ModuleEntity>();
            var user = await repository.Db.Queryable<UserEntity>().InSingleAsync(userId);
            var roleId = user.F_RoleId;
            if (user.F_IsAdmin == true)
            {
                roleId = "admin";
            }
            var rolelist = roleId.Split(',');
            var modulelist = repository.Db.Queryable<RoleAuthorizeEntity,ModuleEntity>((a,b)=>new JoinQueryInfos(
                JoinType.Inner, a.F_ItemId == b.F_Id && b.F_IsMenu == true

                )).Where(a => roleId.Contains(a.F_ObjectId) && a.F_ItemType == 1).Select(a => a.F_ItemId).ToList();
            if (roleId == "admin")
            {
                modulelist = repository.Db.Queryable<ModuleEntity>().Where(a => a.F_EnabledMark == true && a.F_IsMenu == true && a.F_DeleteMark == false).Select(a => a.F_Id).ToList();
            }
            modulelist = modulelist.Distinct().ToList();
            quicks = repository.Db.Queryable<ModuleEntity>().Where(a => (modulelist.Contains(a.F_Id) || a.F_IsPublic == true) && a.F_IsMenu == true && a.F_EnabledMark == true && a.F_UrlAddress != null)
                .Select(a => new ModuleEntity
                {
                    F_Id = a.F_Id,
                    F_EnabledMark = false,
                    F_FullName = a.F_FullName
                }).ToList();
            foreach (var item in quicklist)
            {
                var temp = quicks.Find(a => a.F_Id == item.F_ModuleId);
                if (temp != null)
                {
                    temp.F_EnabledMark = true;
                }
            }
            return quicks;
        }

        public async Task<List<QuickModuleExtend>> GetQuickModuleList(string userId)
        {
            var quicklist = repository.IQueryable(a => a.F_CreatorUserId == userId && a.F_EnabledMark == true);
            List<QuickModuleExtend> list = new List<QuickModuleExtend>();
            List<QuickModuleEntity> quicks = new List<QuickModuleEntity>();
			repository.Db.Ado.BeginTran();
			if (!await quicklist.AnyAsync())
            {
                var user = await repository.Db.Queryable<UserEntity>().InSingleAsync(userId);
                var roleId = user.F_RoleId;
                if (user.F_IsAdmin == true)
                {
                    roleId = "admin";
                }
                var rolelist = roleId.Split(',');
                var modulelist = repository.Db.Queryable<RoleAuthorizeEntity, ModuleEntity>((a, b) => new JoinQueryInfos(
                 JoinType.Inner, a.F_ItemId == b.F_Id && b.F_IsMenu == true

                 )).Where(a => roleId.Contains(a.F_ObjectId) && a.F_ItemType == 1).Select(a => a.F_ItemId).ToList();
                if (roleId == "admin")
                {
                    modulelist = repository.Db.Queryable<ModuleEntity>().Where(a => a.F_EnabledMark == true && a.F_IsMenu == true && a.F_DeleteMark == false).Select(a => a.F_Id).ToList();
                }
                var temp = repository.Db.Queryable<ModuleEntity>().Where(a => a.F_IsPublic == true && a.F_IsMenu == true && a.F_EnabledMark == true && a.F_DeleteMark == false).Select(a => a.F_Id).ToList();
                modulelist.AddRange(temp);
                modulelist = modulelist.Distinct().ToList();
                foreach (var item in modulelist)
                {
                    var module = await repository.Db.Queryable<ModuleEntity>().Where(a => a.F_Id == item && a.F_EnabledMark == true).FirstAsync();
                    if (module != null && module.F_UrlAddress != null && list.Count < 8)
                    {
                        list.Add(new QuickModuleExtend
                        {
                            id = module.F_Id,
                            title = module.F_FullName,
                            href = module.F_UrlAddress,
                            icon = module.F_Icon
                        });
                        QuickModuleEntity quick = new QuickModuleEntity();
                        quick.Create();
                        quick.F_DeleteMark = false;
                        quick.F_EnabledMark = true;
                        quick.F_ModuleId = module.F_Id;
                        quicks.Add(quick);
                    }
                }
            }
            else
            {
                foreach (var item in quicklist.ToList())
                {
                    var module = await repository.Db.Queryable<ModuleEntity>().Where(a => a.F_Id == item.F_ModuleId && a.F_EnabledMark == true).FirstAsync();
                    if (module != null)
                    {
                        list.Add(new QuickModuleExtend
                        {
                            id = module.F_Id,
                            title = module.F_FullName,
                            href = module.F_UrlAddress,
                            icon = module.F_Icon
                        });
                    }
					else
					{
                        await repository.Delete(a => a.F_Id == item.F_Id);
                    }
                }
            }
            if (quicks.Count > 0)
            {
                await repository.Db.Insertable(quicks).ExecuteCommandAsync();
            }
			repository.Db.Ado.CommitTran();
			return list;
        }

        public async Task SubmitForm(string[] permissionIds)
        {
            List<QuickModuleEntity> list = new List<QuickModuleEntity>();
            if (permissionIds != null)
            {
                foreach (var itemId in permissionIds)
                {
                    QuickModuleEntity entity = new QuickModuleEntity();
                    entity.Create();
                    entity.F_ModuleId = itemId;
                    entity.F_EnabledMark = true;
                    entity.F_DeleteMark = false;
                    list.Add(entity);
                }
            }
			repository.Db.Ado.BeginTran();
			await repository.Delete(a => a.F_CreatorUserId == currentuser.UserId);
            await repository.Insert(list);
			repository.Db.Ado.CommitTran();
		}

    }
}