//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="JR">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.Domain.SystemManage;
using WaterCloud.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
using System.Threading.Tasks;

namespace WaterCloud.Service.SystemManage
{
    public class NoticeService: IDenpendency
    {
		private INoticeRepository service = new NoticeRepository();
        /// <summary>
        /// »º´æ²Ù×÷Àà
        /// </summary>

        private string cacheKey = "watercloud_noticedata_";

        public async Task<List<NoticeEntity>> GetList(string keyword)
        {
            var cachedata =await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Title .Contains( keyword)||t.F_Content.Contains(keyword)).ToList();
            }
            return cachedata.ToList();
        }
        public async Task<List<NoticeEntity>> GetList(Pagination pagination, string keyword = "")
        {
            var expression = ExtLinq.True<NoticeEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Title.Contains(keyword));
                expression = expression.Or(t => t.F_Content.Contains(keyword));
            }
            expression = expression.And(t => t.F_EnabledMark == true);
            return await service.FindList(expression, pagination);
        }
        public async Task<NoticeEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }

		public async Task SubmitForm(NoticeEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                await service.Update(entity);
                await RedisHelper.DelAsync(cacheKey + keyValue);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
            else
            {
                entity.Create();
                await service.Insert(entity);
                await RedisHelper.DelAsync(cacheKey + "list");
            }
        }

		public async Task DeleteForm(string keyValue)
        {
            await service.DeleteForm(keyValue);
            await RedisHelper.DelAsync(cacheKey + keyValue);
            await RedisHelper.DelAsync(cacheKey + "list");
        }

    }
}