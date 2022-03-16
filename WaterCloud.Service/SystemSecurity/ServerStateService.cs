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
using System;
using System.Collections.Generic;
using WaterCloud.Code;
using System.Threading.Tasks;
using WaterCloud.DataBase;
using SqlSugar;

namespace WaterCloud.Service.SystemSecurity
{
    public class ServerStateService : DataFilterService<ServerStateEntity>, IDenpendency
    {
        public ServerStateService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
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
            expression = expression.AndAlso(a => a.Date >= startTime && a.Date <= endTime);
            return await repository.IQueryable(expression).ToListAsync();
        }

		public async Task SubmitForm(ServerStateEntity entity)
        {
            var old = repository.IQueryable().First(a => a.WebSite == entity.WebSite && a.Date == DateTime.Now.Date);
            if (old != null)
            {
                entity.Id = old.Id;
                entity.Date = old.Date;
                entity.Cout = old.Cout + 1;
                entity.ARM = Math.Round(((old.ARM).ToDouble() * old.Cout + entity.ARM.ToDouble()) / entity.Cout, 2).ToString();
                entity.CPU = Math.Round(((old.CPU).ToDouble() * old.Cout + entity.CPU.ToDouble()) / entity.Cout, 2).ToString();
                entity.IIS = Math.Round(((old.IIS).ToDouble() * old.Cout + entity.IIS.ToDouble()) / entity.Cout, 0).ToString();
                await repository.Update(entity);
            }
            else
            {
                entity.Id = Utils.GuId();
                entity.Cout = 1;
                entity.Date = DateTime.Now.Date;
                await repository.Insert(entity);
            }
        }
    }
}