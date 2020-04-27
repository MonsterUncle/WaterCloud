//-----------------------------------------------------------------------
// <copyright file=" ServerState.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: ServerState.cs
// * history : Created by T4 04/13/2020 11:54:49 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.DataBase;
using WaterCloud.Entity.SystemSecurity;
using WaterCloud.Domain.IRepository.SystemSecurity;
using WaterCloud.Entity.SystemManage;

namespace WaterCloud.Repository.SystemSecurity
{
    public class ServerStateRepository : RepositoryBase<ServerStateEntity>, IServerStateRepository
    {
        private string ConnectStr;
        private string providerName;
        public ServerStateRepository()
        {
        }
        public ServerStateRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
        }
        public void DeleteForm(string keyValue)
        {
            using (var db =new RepositoryBase(ConnectStr, providerName).BeginTrans())
            {
                db.Delete<ServerStateEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
    }
}