//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.DataBase;
using WaterCloud.Entity.SystemManage;
using WaterCloud.Domain.IRepository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Repository.SystemManage
{
    public class NoticeRepository : RepositoryBase<NoticeEntity>, INoticeRepository
    {
        private string ConnectStr;
        private string providerName;
        public NoticeRepository()
        {
        }
        public NoticeRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }
        public void DeleteForm(string keyValue)
        {
            using (var db =new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                db.Delete<NoticeEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
    }
}