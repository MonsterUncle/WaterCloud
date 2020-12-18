using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using Chloe;
using WaterCloud.Domain.InfoManage;
using Microsoft.AspNetCore.SignalR;
using WaterCloud.Domain.SystemOrganize;
using WaterCloud.Service.SystemManage;

namespace WaterCloud.Service.InfoManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-07-29 16:41
    /// 描 述：通知管理服务类
    /// </summary>
    public class MessageService : DataFilterService<MessageEntity>, IDenpendency
    {
        private string cacheKey = "watercloud_messagedata_";
        private string cacheHubKey = "watercloud_hubuserinfo_";
        
        private readonly IHubContext<MessageHub> _messageHub;
        private ItemsDataService itemsApp;
        public MessageService(IDbContext context, IHubContext<MessageHub> messageHub) : base(context)
        {
            itemsApp = new ItemsDataService(context);
            _messageHub = messageHub;
        }
        #region 获取数据
        public async Task<List<MessageEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_MessageInfo.Contains(keyword) || t.F_CreatorUserName.Contains(keyword)).ToList();
            }
            return cachedata.Where(a => a.F_EnabledMark == true).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<MessageEntity>> GetLookList(string keyword = "")
        {
            var list = new List<MessageEntity>();
            if (!CheckDataPrivilege())
            {
                list = await repository.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u");
                list = forms.ToList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(t => t.F_MessageInfo.Contains(keyword) || t.F_CreatorUserName.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(a => a.F_EnabledMark == true).OrderByDescending(t => t.F_CreatorTime).ToList());
        }

        public async Task<List<MessageEntity>> GetUnReadListJson()
        {
            var hisquery = uniwork.IQueryable<MessageHistoryEntity>(a => a.F_CreatorUserId == currentuser.UserId).Select(a => a.F_MessageId).ToList();
            var tempList= repository.IQueryable(a => a.F_MessageType == 2).InnerJoin<MessageHistoryEntity>((a, b) => a.F_Id == b.F_MessageId).Select((a, b) => a.F_Id).ToList();
            hisquery.AddRange(tempList);
            var query = repository.IQueryable(a => (a.F_ToUserId.Contains(currentuser.UserId) || a.F_ToUserId == "") && a.F_EnabledMark == true && !hisquery.Contains(a.F_Id));
            return GetFieldsFilterData(query.OrderByDesc(t => t.F_CreatorTime).ToList());
        }

        public async Task<List<MessageEntity>> GetLookList(SoulPage<MessageEntity> pagination, string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            var setList = await itemsApp.GetItemList("MessageType");
            Dictionary<string, string> messageTypeTemp = new Dictionary<string, string>();
            foreach (var item in setList)
            {
                messageTypeTemp.Add(item.F_ItemName, item.F_ItemCode);
            }
            dic.Add("F_MessageType", messageTypeTemp);
            pagination = ChangeSoulData(dic, pagination);
            //获取数据权限
            var list = GetDataPrivilege("u");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(t => t.F_MessageInfo.Contains(keyword) || t.F_CreatorUserName.Contains(keyword));
            }
            return GetFieldsFilterData(await repository.OrderList(list.Where(a => a.F_EnabledMark == true), pagination));
        }

        public async Task<MessageEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<MessageEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata);
        }

        #region 提交数据
        public async Task SubmitForm(MessageEntity entity)
        {
            entity.Create();
            if (entity.F_MessageType==0||string.IsNullOrEmpty(entity.F_ToUserId))
            {
                string msg = entity.ToJson();
                entity.F_ToUserName = "所有人";
                entity.F_ToUserId = "";
                await _messageHub.Clients.Group(currentuser.CompanyId).SendAsync("ReceiveMessage",msg);
            }
            else
            {
                var str = entity.F_ToUserId.Split(",");
                entity.F_ToUserName = string.Join(",", uniwork.IQueryable<UserEntity>(a => str.Contains(a.F_Id)).Select(a => a.F_RealName).ToList());
                foreach (var item in str)
                {
                    //存在就私信
                    var connectionID = await CacheHelper.Get<string>(cacheHubKey + item);
                    if (connectionID == null)
                    {
                        continue;
                    }
                    string msg = entity.ToJson();
                    await _messageHub.Clients.Client(connectionID).SendAsync("ReceiveMessage", msg);
                }
            }
            await repository.Insert(entity);
            await CacheHelper.Remove(cacheKey + "list");
        }

        public async Task ReadAllMsgForm(int type)
        {
            var unList=await GetUnReadListJson();
            var strList = unList.Where(a => a.F_MessageType == type&&a.F_ClickRead==true).Select(a=>a.F_Id).ToList();
            uniwork.BeginTrans();
            foreach (var item in strList)
            {
               await ReadMsgForm(item);
            }
            uniwork.Commit();
        }

        public async Task ReadMsgForm(string keyValue)
        {            
            MessageHistoryEntity msghis = new MessageHistoryEntity();
            msghis.Create();
            msghis.F_CreatorUserName = currentuser.UserName;
            msghis.F_MessageId = keyValue;
            await uniwork.Insert(msghis);
        }

        public async Task<bool> CheckMsg(string keyValue)
        {
            var msg = await repository.FindEntity(keyValue);
            if (msg==null)
            {
                return true;
            }
            if (msg.F_ClickRead==false)
            {
                return true;
            }
            if (uniwork.IQueryable<MessageHistoryEntity>(a => a.F_MessageId == keyValue && a.F_CreatorUserId == currentuser.UserId).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Update(t => ids.Contains(t.F_Id), t=>new MessageEntity { 
                F_EnabledMark=false         
            });
            foreach (var item in ids)
            {
            await CacheHelper.Remove(cacheKey + item);
            }
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
