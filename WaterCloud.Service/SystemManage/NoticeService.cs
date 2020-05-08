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
namespace WaterCloud.Service.SystemManage
{
    public class NoticeService: IDenpendency
    {
		private INoticeRepository service = new NoticeRepository();
        /// <summary>
        /// »º´æ²Ù×÷Àà
        /// </summary>

        private string cacheKey = "watercloud_noticedata_";

        public List<NoticeEntity> GetList(string keyword)
        {
            var cachedata = service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Title .Contains( keyword)||t.F_Content.Contains(keyword)).ToList();
            }
            return cachedata.ToList();
        }
        public List<NoticeEntity> GetList(Pagination pagination, string keyword = "")
        {
            var expression = ExtLinq.True<NoticeEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Title.Contains(keyword));
                expression = expression.Or(t => t.F_Content.Contains(keyword));
            }
            expression = expression.And(t => t.F_EnabledMark == true);
            return service.FindList(expression, pagination);
        }
        public NoticeEntity GetForm(string keyValue)
        {
            var cachedata = service.CheckCache(cacheKey, keyValue);
            return cachedata;
        }

		public void SubmitForm(NoticeEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
                RedisHelper.Del(cacheKey + keyValue);
                RedisHelper.Del(cacheKey + "list");
            }
            else
            {
                entity.Create();
                service.Insert(entity);
                RedisHelper.Del(cacheKey + "list");
            }
        }

		public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
            RedisHelper.Del(cacheKey + keyValue);
            RedisHelper.Del(cacheKey + "list");
        }

    }
}