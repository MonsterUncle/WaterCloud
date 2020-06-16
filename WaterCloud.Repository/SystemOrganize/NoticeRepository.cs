//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using Chloe;
using System.Threading.Tasks;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemOrganize;

namespace WaterCloud.Repository.SystemOrganize
{
    public class NoticeRepository : RepositoryBase<NoticeEntity>, INoticeRepository
    {
        private string ConnectStr;
        private string providerName;
        private DbContext dbcontext;
        public NoticeRepository()
        {
            dbcontext = GetDbContext();
        }
        public NoticeRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
            dbcontext = GetDbContext();
        }
        public async Task DeleteForm(string keyValue)
        {
            using (var db =new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                await db.Delete<NoticeEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
    }
}