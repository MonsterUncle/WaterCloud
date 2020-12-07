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
using Chloe;

namespace WaterCloud.Service.SystemOrganize
{
    public class NoticeService: DataFilterService<NoticeEntity>,IDenpendency
    {
        //获取类名
        
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_noticedata_";
        public NoticeService(IDbContext context) : base(context)
        {
        }
        public async Task<List<NoticeEntity>> GetList(string keyword)
        {
            List<NoticeEntity> list = new List<NoticeEntity>();
            list = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.F_Title.Contains(keyword) || t.F_Content.Contains(keyword)).ToList();
            }
            //公告这里不控制
            //数据权限和缓存获取冲突
            //if (!CheckDataPrivilege("u"))
            //{
            //    list = await service.CheckCacheList(cacheKey + "list");
            //}
            //else
            //{
            //    var forms = GetDataPrivilege();
            //    list = list.ToList();
            //}
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    list = list.Where(t => t.F_Title.Contains(keyword) || t.F_Content.Contains(keyword)).ToList();
            //}
            return list.Where(a => a.F_DeleteMark == false).ToList();
        }
        public async Task<List<NoticeEntity>> GetLookList(Pagination pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u");
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_Title.Contains(keyword) || u.F_Content.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination));
        }
        public async Task<NoticeEntity> GetLookForm(string keyValue)
        {
            var cachedata =await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata);
        }
        public async Task<NoticeEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task SubmitForm(NoticeEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                await repository.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                entity.F_CreatorUserName = currentuser.UserName;
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }

		public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Delete(t => ids.Contains(t.F_Id));
            foreach (var item in ids)
            {
                await CacheHelper.Remove(cacheKey + item);
            }
            await CacheHelper.Remove(cacheKey + "list");
        }

    }
}