//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="JR">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
using System.Threading.Tasks;
using SqlSugar;
using WaterCloud.DataBase;

namespace WaterCloud.Service.SystemOrganize
{
    public class NoticeService: DataFilterService<NoticeEntity>,IDenpendency
    {
        public NoticeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<NoticeEntity>> GetList(string keyword)
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_Title.Contains(keyword) || a.F_Content.Contains(keyword));
            }
            return await query.Where(a => a.F_DeleteMark == false).ToListAsync();
        }
        public async Task<List<NoticeEntity>> GetLookList(SoulPage<NoticeEntity> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledDic = new Dictionary<string, string>();
            enabledDic.Add("有效", "1");
            enabledDic.Add("无效", "0");
            dic.Add("F_EnabledMark", enabledDic);
            pagination = ChangeSoulData(dic, pagination);
            var query = repository.IQueryable().Where(a => a.F_DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.F_Title.Contains(keyword) || a.F_Content.Contains(keyword));
            }
            //权限过滤（保证分页参数存在）
            query = GetDataPrivilege("a", "", query);
            return await repository.OrderList(query, pagination);
        }
        public async Task<NoticeEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity( keyValue);
            return GetFieldsFilterData(data);
        }
        public async Task<NoticeEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        public async Task SubmitForm(NoticeEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                await repository.Update(entity);
            }
            else
            {
                entity.F_CreatorUserName = currentuser.UserName;
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
            }
        }

		public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Delete(a => ids.Contains(a.F_Id));
        }

    }
}