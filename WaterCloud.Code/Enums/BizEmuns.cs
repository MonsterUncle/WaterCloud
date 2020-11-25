using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    public class BizEmuns
    {
        
        public enum EnumMemberType
        {
            普通会员 = 1,
            信息会员 = 2,
            成交会员 = 3,
            收益会员 = 4
        }

        /// <summary>
        /// 奖励类型设置
        /// </summary>
        public enum AwardWay
        {
            积分奖励= 0,
            佣金奖励=1
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        public enum OrderType
        {
            出险报案订单 = 1,
            拖车救援订单 = 2,
            美容洗车订单 = 3,
            维修保养订单 = 4,
            全部类型订单 = 0
        }

        /// <summary>
        ///  服务商状态
        /// </summary>
        public enum PartnerStatus
        {
            未通过 = -1,
            待补充 = 0,
            待审核 = 1,
            已通过 = 2,
            已注销 = 3
        }
        /// <summary>
        /// 会员卡状态
        /// </summary>
        public enum CardStatus
        {
            未通过 = -1,
            待审核 = 0,
            已通过 = 1,
            已注销 = 3
        }
        /// <summary>
        ///  会员用户状态
        /// </summary>
        public enum UserStatus
        {
            未通过 = -1,
            待审核 = 0,
            已审核 = 1,
            正常 = 2,
            已注销 = 3
        }
        /// <summary>
        ///  订单状态
        /// </summary>
        public enum OrderStatus
        {
            已取消 = -2,
            未通过 = -1,
            待审核 = 0,
            已审核 = 1,
            已派单 = 2,
            已接单 = 3,
            已完成 = 4
        }
        /// <summary>
        /// 服务状态
        /// </summary>
        public enum ServerStatus
        {
            未接单 = 0,
            已接单 = 1,
            已服务 = 2,
            已拒单 = 3
        }
        /// <summary>
        /// 支付状态
        /// </summary>
        public enum PaymentStatus
        {
            未支付 = 0,
            已支付 = 1
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public enum SerType
        {
            美容洗车 = 1,
            维修保养 = 2,
            其他服务 = 3
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public enum PaymentType
        {
            在线支付,
            线下支付
        }

        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogType
        {
            订单奖励积分 = 0,
            分佣奖励积分 = 1,
            推荐奖励积分 = 2,
            商城奖励积分 = 3,
            签到奖励积分 = 4,
            积分使用 = 5,
            积分兑换 = 6,
            在线充值 = 7,
            金额消费 = 8
        }
        /// <summary>
        /// 
        /// </summary>
        public enum IndustryCode
        {

        }
    }
}
