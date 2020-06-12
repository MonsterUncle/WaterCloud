//-----------------------------------------------------------------------
// <copyright file=" ServerState.cs" company="WaterCloud">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: ServerState.cs
// * history : Created by T4 04/13/2020 11:54:49 
// </copyright>
//-----------------------------------------------------------------------
using Chloe;
using System.Threading.Tasks;
using WaterCloud.DataBase;
using WaterCloud.Domain.SystemSecurity;

namespace WaterCloud.Repository.SystemSecurity
{
    public class ServerStateRepository : RepositoryBase<ServerStateEntity>, IServerStateRepository
    {
        private string ConnectStr;
        private string providerName;
        private DbContext dbcontext;
        public ServerStateRepository()
        {
            dbcontext = GetDbContext();
        }
        public ServerStateRepository(string ConnectStr, string providerName)
            : base(ConnectStr, providerName)
        {
            this.ConnectStr = ConnectStr;
            this.providerName = providerName;
            dbcontext = GetDbContext();
        }
        public async Task DeleteForm(string keyValue)
        {
            await this.Delete(t => t.F_Id == keyValue);
        }
    }
}