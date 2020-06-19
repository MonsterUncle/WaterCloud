//-----------------------------------------------------------------------
// <copyright file=" QuickModule.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: QuickModule.cs
// * history : Created by T4 04/13/2020 16:51:14 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemManage;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Chloe;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemManage
{
    public class QuickModuleRepository : RepositoryBase<QuickModuleEntity>, IQuickModuleRepository
    {
        private IDbContext dbcontext;
        public QuickModuleRepository(IDbContext context) : base(context)
        {
            dbcontext = context;
        }
        public QuickModuleRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            dbcontext = GetDbContext();
        }
        public async Task<List<QuickModuleExtend>> GetQuickModuleList(string userId)
        {
            using (var db =new RepositoryBase(dbcontext).BeginTrans())
            {
                var quicklist= db.IQueryable<QuickModuleEntity>(t => t.F_CreatorUserId == userId&&t.F_EnabledMark==true);
                List<QuickModuleExtend> list = new List<QuickModuleExtend>();
                List<QuickModuleEntity> quicks = new List<QuickModuleEntity>();
                if (quicklist.Count()==0)
                {
                    var user = await db.FindEntity<UserEntity>(userId);
                    var roleId = user.F_RoleId;
                    if (user.F_Account == "admin" || user.F_IsAdmin == true)
                    {
                        roleId = "admin";
                    }
                    var rolelist =roleId.Split(',');
                    var modulelist= db.IQueryable<RoleAuthorizeEntity>(a => rolelist.Contains(a.F_ObjectId) && a.F_ItemType == 1).Select(a=>a.F_ItemId).ToList();
                    if (roleId == "admin")
                    {
                        modulelist= db.IQueryable<ModuleEntity>(a => a.F_EnabledMark==true).Select(a => a.F_Id).ToList();
                    }
                    var temp= db.IQueryable<ModuleEntity>(a => a.F_IsPublic == true && a.F_IsMenu == true && a.F_EnabledMark == true && a.F_DeleteMark == false).Select(a => a.F_Id).ToList();
                    modulelist.AddRange(temp);
                    modulelist = modulelist.Distinct().ToList();
                    foreach (var item in modulelist)
                    {
                        var module =await db.FindEntity<ModuleEntity>(a => a.F_Id == item&& a.F_EnabledMark == true);
                        if (module!=null&&module.F_UrlAddress!=null&&list.Count<8)
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
                        var module =await  db.FindEntity<ModuleEntity>(a => a.F_Id==item.F_ModuleId&&a.F_EnabledMark==true);
                        if (module!=null)
                        {
                            list.Add(new QuickModuleExtend
                            {
                                id = module.F_Id,
                                title = module.F_FullName,
                                href = module.F_UrlAddress,
                                icon = module.F_Icon
                            });
                        }

                    }                   
                }
                if (quicks.Count>0)
                {
                    await db.Insert(quicks);
                    db.Commit();
                }
                return list;

            }
        }

        public async Task<List<ModuleEntity>> GetTransferList(string userId)
        {
            using (var db =new RepositoryBase(dbcontext).BeginTrans())
            {

                var quicklist = db.IQueryable<QuickModuleEntity>(t => t.F_CreatorUserId == userId && t.F_EnabledMark == true).ToList();
                List<ModuleEntity> quicks = new List<ModuleEntity>();
                var user = await db.FindEntity<UserEntity>(userId);
                var roleId = user.F_RoleId;
                if (user.F_Account == "admin"|| user.F_IsAdmin==true)
                {
                    roleId = "admin";
                }
                var rolelist = roleId.Split(',');
                var modulelist = db.IQueryable<RoleAuthorizeEntity>(a => roleId.Contains(a.F_ObjectId) && a.F_ItemType == 1).Select(a => a.F_ItemId).ToList();
                if (roleId == "admin")
                {
                    modulelist = db.IQueryable<ModuleEntity>(a => a.F_EnabledMark == true).Select(a => a.F_Id).ToList();
                }
                modulelist= modulelist.Distinct().ToList();
                quicks = db.IQueryable<ModuleEntity>(a => (modulelist.Contains(a.F_Id)||a.F_IsPublic==true)&&a.F_IsMenu==true && a.F_EnabledMark==true && a.F_UrlAddress != null)
                    .Select(a=>new ModuleEntity { 
                    F_Id=a.F_Id,
                    F_EnabledMark=false,
                    F_FullName=a.F_FullName                                      
                    }).ToList();
                foreach (var item in quicklist)
                {
                   var temp =quicks.Find(a => a.F_Id == item.F_ModuleId);
                    if (temp!=null)
                    {
                        temp.F_EnabledMark = true;
                    }
                }
                return quicks;

            }
        }

        public async Task SubmitForm(List<QuickModuleEntity> list)
        {
            using (var db =new RepositoryBase(dbcontext).BeginTrans())
            {
                await db.Delete<QuickModuleEntity>(t => t.F_CreatorUserId == list[0].F_CreatorUserId);
                await db.Insert(list);
                db.Commit();
            }
        }
    }
}