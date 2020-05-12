//-----------------------------------------------------------------------
// <copyright file=" ServerState.cs" company="JR">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: ServerState.cs
// * history : Created by T4 04/13/2020 11:54:48 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.Domain.SystemSecurity;
using WaterCloud.Repository.SystemSecurity;
using System;
using System.Collections.Generic;
using WaterCloud.Code;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemSecurity
{
    public class ServerStateService:IDenpendency
    {
		private IServerStateRepository service = new ServerStateRepository();

		public async Task<List<ServerStateEntity>> GetList(int timetype)
        {
            var expression = ExtLinq.True<ServerStateEntity>();
            DateTime startTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
            DateTime endTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate().AddDays(1);
            switch (timetype)
            {
                case 1:
                    break;
                case 2:
                    startTime = startTime.AddDays(-7);
                    break;
                case 3:
                    startTime = startTime.AddMonths(-1);
                    break;
                case 4:
                    startTime = startTime.AddMonths(-3);
                    break;
                default:
                    break;
            }
            expression = expression.And(t => t.F_Date >= startTime && t.F_Date <= endTime);
            return service.IQueryable(expression).ToList();
        }

	    public async Task<ServerStateEntity> GetForm(string keyValue)
        {
            return await service.FindEntity(keyValue);
        }

		public async Task SubmitForm(ServerStateEntity entity)
        {
            var old = service.IQueryable(a => a.F_WebSite == entity.F_WebSite && a.F_Date == DateTime.Now.Date).FirstOrDefault();
            if (old != null)
            {
                entity.F_Id = old.F_Id;
                entity.F_Date = old.F_Date;
                entity.F_Cout = old.F_Cout + 1;
                entity.F_ARM = Math.Round(((old.F_ARM).ToDouble() * old.F_Cout + entity.F_ARM.ToDouble()) / entity.F_Cout, 2).ToString();
                entity.F_CPU = Math.Round(((old.F_CPU).ToDouble() * old.F_Cout + entity.F_CPU.ToDouble()) / entity.F_Cout, 2).ToString();
                entity.F_IIS = Math.Round(((old.F_IIS).ToDouble() * old.F_Cout + entity.F_IIS.ToDouble()) / entity.F_Cout, 0).ToString();
                await service.Update(entity);
            }
            else
            {
                entity.F_Id = Utils.GuId();
                entity.F_Cout = 1;
                entity.F_Date = DateTime.Now.Date;
                await service.Insert(entity);
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            await service.DeleteForm(keyValue);
        }

    }
}