using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    public class OperatorUserInfo
    {
        //密码
        public string UserPassword { get; set; }
        //
        public string UserSecretkey { get; set; }
        //登录时间设置
        public DateTime? AllowStartTime { get; set; }
        public DateTime? AllowEndTime { get; set; }
        //锁定时间设置
        public DateTime? LockStartDate { get; set; }
        public DateTime? LockEndDate { get; set; }
        //第一次登录
        public DateTime? FirstVisitTime { get; set; }
        //上一次登录时间
        public DateTime? PreviousVisitTime { get; set; }
        //最后一次登录时间
        public DateTime? LastVisitTime { get; set; }
        //修改密码时间
        public DateTime? ChangePasswordDate { get; set; }
        //登录次数
        public int? LogOnCount { get; set; }
        //在线标记
        public bool? UserOnLine { get; set; }
        //安全问题
        public string Question { get; set; }
        //问题答案
        public string AnswerQuestion { get; set; }
        //默认主题
        public string Theme { get; set; }
    }
}
