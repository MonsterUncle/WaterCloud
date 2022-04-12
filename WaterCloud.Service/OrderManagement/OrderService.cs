using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.DataBase;
using WaterCloud.Domain.OrderManagement;

namespace WaterCloud.Service.OrderManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-12 20:41
    /// 描 述：订单管理服务类
    /// </summary>
    public class OrderService : DataFilterService<OrderEntity>, IDenpendency
    {
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #region 获取数据
        public async Task<List<OrderEntity>> GetList(string keyword = "")
        {
            var data = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.OrderCode.Contains(keyword)
                || a.Description.Contains(keyword));
            }
            return await data.Where(a => a.DeleteMark == false).OrderBy(a => a.NeedTime).ToListAsync();
        }

        public async Task<List<OrderEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.OrderCode.Contains(keyword)
                || a.Description.Contains(keyword));
            }
            //权限过滤
            query = GetDataPrivilege("a", "", query);
            return await query.OrderBy(a => a.NeedTime).ToListAsync();
        }

        public async Task<List<OrderEntity>> GetLookList(SoulPage<OrderEntity> pagination, string keyword = "", string id = "")
        {
            var query = IQuery().Where(a => a.DeleteMark == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a => a.OrderCode.Contains(keyword)
                || a.Description.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(a => a.Id == id);
            }
            //权限过滤
            query = GetDataPrivilege("a", "", query);
            return await query.ToPageListAsync(pagination);
        }

        private ISugarQueryable<OrderEntity> IQuery()
        {
            var details = repository.Db.Queryable<OrderDetailEntity>().GroupBy(a => a.OrderId).Select<Object>(a => new {
                a.OrderId,
                NeedNum = SqlFunc.AggregateSum(a.NeedNum),
                ActualNum = SqlFunc.AggregateSum(a.ActualNum),
            });
            var order = repository.Db.Queryable<OrderEntity>();
            var query = repository.Db.Queryable(order, details, JoinType.Inner, (a, b) => a.Id == SqlFunc.MappingColumn(default(string), "b.OrderId")).Select((a, b) => new OrderEntity {
                Id = a.Id,
                ActualTime = a.ActualTime,
                CreatorTime = a.CreatorTime,
                CreatorUserId = a.CreatorUserId,
                CreatorUserName = a.CreatorUserName,
                DeleteMark = a.DeleteMark,
                DeleteTime = a.DeleteTime,
                DeleteUserId = a.DeleteUserId,
                Description = a.Description,
                EnabledMark = a.EnabledMark,
                LastModifyTime = a.LastModifyTime,
                LastModifyUserId = a.LastModifyUserId,
                NeedTime = a.NeedTime,
                OrderCode = a.OrderCode,
                OrderState = a.OrderState,
                NeedNum = SqlFunc.MappingColumn(default(int), "b.NeedNum"),
                ActualNum = SqlFunc.MappingColumn(default(int), "b.ActualNum"),
            }).MergeTable();
            return query;
        }

        public async Task<OrderEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            data.list = await repository.Db.Queryable<OrderDetailEntity>().Where(a => a.OrderId == keyValue).ToListAsync();
            return data;
        }
        #endregion

        public async Task<OrderEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            data.list = await repository.Db.Queryable<OrderDetailEntity>().Where(a => a.OrderId == keyValue).ToListAsync();
            return GetFieldsFilterData(data);
        }

        #region 提交数据
        public async Task SubmitForm(OrderEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.Create();
                entity.DeleteMark = false;
                entity.CreatorUserName = currentuser.UserName;
            }
            else
            {
                entity.Modify(keyValue);
            }
            bool isdone = true;
            var dataList = new List<OrderDetailEntity>();
            if (entity.list == null || entity.list.Count == 0)
            {
                throw new Exception("产品明细不能为空!");
            }
            foreach (var item in entity.list)
            {
                item.Create();
                item.CreatorUserName = currentuser.UserName;
                item.CreatorTime = entity.CreatorTime;
                item.NeedTime = entity.NeedTime;
                item.OrderId = entity.Id;
                item.OrderState = item.OrderState == null ? 0 : item.OrderState;
                if (item.OrderState == 0)
                {
                    isdone = false;
                }
                else if (item.ActualTime == null)
                {
                    item.ActualTime = entity.NeedTime;
                }
                if (string.IsNullOrEmpty(item.ProductName))
                {
                    throw new Exception("请输入明细的产品名称");
                }
                dataList.Add(item);
            }
            entity.OrderState = isdone ? 1 : 0;
            entity.ActualTime = isdone ?dataList.Max(a=>a.ActualTime):null;
            unitOfWork.CurrentBeginTrans();
            if (string.IsNullOrEmpty(keyValue))
            {
                await repository.Insert(entity);
            }
            else
            {
                await repository.Update(entity);
            }
            await repository.Db.Deleteable<OrderDetailEntity>().Where(a => a.OrderId == entity.Id).ExecuteCommandAsync();
            await repository.Db.Insertable(dataList).ExecuteCommandAsync();
            unitOfWork.CurrentCommit();
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Update(a => ids.Contains(a.Id),a=>new OrderEntity { 
                DeleteMark=true,
                DeleteTime=DateTime.Now,
                DeleteUserId=currentuser.UserId
            });
        }
        #endregion

    }
}
