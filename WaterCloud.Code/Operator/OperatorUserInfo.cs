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
        public string F_UserPassword { get; set; }
        //
        public string F_UserSecretkey { get; set; }
        //登录时间设置
        public DateTime? F_AllowStartTime { get; set; }
        public DateTime? F_AllowEndTime { get; set; }
        //锁定时间设置
        public DateTime? F_LockStartDate { get; set; }
        public DateTime? F_LockEndDate { get; set; }
        //第一次登录
        public DateTime? F_FirstVisitTime { get; set; }
        //上一次登录时间
        public DateTime? F_PreviousVisitTime { get; set; }
        //最后一次登录时间
        public DateTime? F_LastVisitTime { get; set; }
        //修改密码时间
        public DateTime? F_ChangePasswordDate { get; set; }
        //登录次数
        public int? F_LogOnCount { get; set; }
        //在线标记
        public bool? F_UserOnLine { get; set; }
        //安全问题
        public string F_Question { get; set; }
        //问题答案
        public string F_AnswerQuestion { get; set; }
        //默认主题
        public string F_Theme { get; set; }
    }
}
