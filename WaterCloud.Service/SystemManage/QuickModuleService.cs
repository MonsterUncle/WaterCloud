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
    public class QuickModuleService:DataFilterService<QuickModuleEntity> ,IDenpendency
    {
        public QuickModuleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<object> GetTransferList(string userId)
        {
            var quicklist = repository.IQueryable(a => a.CreatorUserId == userId && a.EnabledMark == true).ToList();
            List<ModuleEntity> quicks = new List<ModuleEntity>();
            var user = await repository.Db.Queryable<UserEntity>().InSingleAsync(userId);
            var roleId = user.RoleId;
            if (user.IsAdmin == true)
            {
                roleId = "admin";
            }
            var rolelist = roleId.Split(',');
            var modulelist = repository.Db.Queryable<RoleAuthorizeEntity,ModuleEntity>((a,b)=>new JoinQueryInfos(
                JoinType.Inner, a.ItemId == b.Id && b.IsMenu == true

                )).Where(a => roleId.Contains(a.ObjectId) && a.ItemType == 1).Select(a => a.ItemId).ToList();
            if (roleId == "admin")
            {
                modulelist = repository.Db.Queryable<ModuleEntity>().Where(a => a.EnabledMark == true && a.IsMenu == true && a.DeleteMark == false).Select(a => a.Id).ToList();
            }
            modulelist = modulelist.Distinct().ToList();
            quicks = repository.Db.Queryable<ModuleEntity>().Where(a => (modulelist.Contains(a.Id) || a.IsPublic == true) && a.IsMenu == true && a.EnabledMark == true && a.UrlAddress != null)
                .Select(a => new ModuleEntity
                {
                    Id = a.Id,
                    EnabledMark = false,
                    FullName = a.FullName
                }).ToList();
            foreach (var item in quicklist)
            {
                var temp = quicks.Find(a => a.Id == item.ModuleId);
                if (temp != null)
                {
                    temp.EnabledMark = true;
                }
            }
            return quicks;
        }

        public async Task<List<QuickModuleExtend>> GetQuickModuleList(string userId)
        {
            var quicklist = repository.IQueryable(a => a.CreatorUserId == userId && a.EnabledMark == true);
            List<QuickModuleExtend> list = new List<QuickModuleExtend>();
            List<QuickModuleEntity> quicks = new List<QuickModuleEntity>();
            unitofwork.CurrentBeginTrans();
            if (!await quicklist.AnyAsync())
            {
                var user = await repository.Db.Queryable<UserEntity>().InSingleAsync(userId);
                var roleId = user.RoleId;
                if (user.IsAdmin == true)
                {
                    roleId = "admin";
                }
                var rolelist = roleId.Split(',');
                var modulelist = repository.Db.Queryable<RoleAuthorizeEntity, ModuleEntity>((a, b) => new JoinQueryInfos(
                 JoinType.Inner, a.ItemId == b.Id && b.IsMenu == true

                 )).Where(a => roleId.Contains(a.ObjectId) && a.ItemType == 1).Select(a => a.ItemId).ToList();
                if (roleId == "admin")
                {
                    modulelist = repository.Db.Queryable<ModuleEntity>().Where(a => a.EnabledMark == true && a.IsMenu == true && a.DeleteMark == false).Select(a => a.Id).ToList();
                }
                var temp = repository.Db.Queryable<ModuleEntity>().Where(a => a.IsPublic == true && a.IsMenu == true && a.EnabledMark == true && a.DeleteMark == false).Select(a => a.Id).ToList();
                modulelist.AddRange(temp);
                modulelist = modulelist.Distinct().ToList();
                foreach (var item in modulelist)
                {
                    var module = await repository.Db.Queryable<ModuleEntity>().Where(a => a.Id == item && a.EnabledMark == true).FirstAsync();
                    if (module != null && module.UrlAddress != null && list.Count < 8)
                    {
                        list.Add(new QuickModuleExtend
                        {
                            id = module.Id,
                            title = module.FullName,
                            href = module.UrlAddress,
                            icon = module.Icon
                        });
                        QuickModuleEntity quick = new QuickModuleEntity();
                        quick.Create();
                        quick.DeleteMark = false;
                        quick.EnabledMark = true;
                        quick.ModuleId = module.Id;
                        quicks.Add(quick);
                    }
                }
            }
            else
            {
                foreach (var item in quicklist.ToList())
                {
                    var module = await repository.Db.Queryable<ModuleEntity>().Where(a => a.Id == item.ModuleId && a.EnabledMark == true).FirstAsync();
                    if (module != null)
                    {
                        list.Add(new QuickModuleExtend
                        {
                            id = module.Id,
                            title = module.FullName,
                            href = module.UrlAddress,
                            icon = module.Icon
                        });
                    }
					else
					{
                        await repository.Delete(a => a.Id == item.Id);
                    }
                }
            }
            if (quicks.Count > 0)
            {
                await repository.Db.Insertable(quicks).ExecuteCommandAsync();
            }
            unitofwork.CurrentCommit();
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
                    entity.ModuleId = itemId;
                    entity.EnabledMark = true;
                    entity.DeleteMark = false;
                    list.Add(entity);
                }
            }
            unitofwork.CurrentBeginTrans();
            await repository.Delete(a => a.CreatorUserId == currentuser.UserId);
            await repository.Insert(list);
            unitofwork.CurrentCommit();
        }

    }
}