//-----------------------------------------------------------------------
// <copyright file=" Notice.cs" company="JR">
// * Copyright (C) WaterCloud.Framework  All Rights Reserved
// * version : 1.0
// * author  : WaterCloud.Framework
// * FileName: Notice.cs
// * history : Created by T4 04/13/2020 16:51:21 
// </copyright>
//-----------------------------------------------------------------------
using WaterCloud.Entity.SystemManage;
using WaterCloud.Domain.IRepository.SystemManage;
using WaterCloud.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterCloud.Code;
namespace WaterCloud.Application.SystemManage
{
    public class NoticeApp
    {
		private INoticeRepository service = new NoticeRepository();
        /// <summary>
        /// »º´æ²Ù×÷Àà
        /// </summary>
        private ICache redisCache = CacheFactory.CaChe();
        private string cacheKey = "watercloud_noticedata_";

        public List<NoticeEntity> GetList(string keyword)
        {
            var cachedata = service.CheckCacheList(cacheKey + "list", CacheId.notice);
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
            var cachedata = service.CheckCache(cacheKey, keyValue, CacheId.notice);
            return cachedata;
        }

		public void SubmitForm(NoticeEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
                redisCache.Remove(cacheKey + keyValue, CacheId.notice);
                redisCache.Remove(cacheKey + "list", CacheId.notice);
            }
            else
            {
                entity.Create();
                service.Insert(entity);
                redisCache.Remove(cacheKey + "list", CacheId.notice);
            }
        }

		public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
            redisCache.Remove(cacheKey + keyValue, CacheId.notice);
            redisCache.Remove(cacheKey + "list", CacheId.notice);
        }

    }
}