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
    public class NoticeService: DataFilterService<NoticeEntity>,IDenpendency
    {
		private INoticeRepository service = new NoticeRepository();
        //获取类名
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        /// <summary>
        /// 缓存操作类
        /// </summary>

        private string cacheKey = "watercloud_noticedata_";

        public async Task<List<NoticeEntity>> GetList(string keyword)
        {
            List<NoticeEntity> list = new List<NoticeEntity>();
            list = await service.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(t => t.F_Title.Contains(keyword) || t.F_Content.Contains(keyword)).ToList();
            }
            //公告这里不控制
            //数据权限和缓存获取冲突
            //if (!CheckDataPrivilege("u", className.Substring(0, className.Length - 7)))
            //{
            //    list = await service.CheckCacheList(cacheKey + "list");
            //}
            //else
            //{
            //    var forms = GetDataPrivilege(className.Substring(0, className.Length - 7));
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
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(u => u.F_Title.Contains(keyword) || u.F_Content.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await service.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }
        public async Task<NoticeEntity> GetForm(string keyValue)
        {
            var cachedata =await service.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
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