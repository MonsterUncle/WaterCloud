using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterCloud.DingTalk.Entities
{
    public class DingTalkJsonResult
    {
        public DingTalkJsonResult() { }
        public bool IsSuccess { get;  set; }
        public string errcode { get; set; }
        public long Errcode { get; set; }
        public string Errmsg { get; set; }
        public object Result { get; }
       

        public static  DingTalkJsonResult CreateSuccessResult(long errorCode,string errMsg)
        {
            return new DingTalkJsonResult()
            {
                IsSuccess = true,
                Errcode = errorCode,
                Errmsg = errMsg
            };
        }
        public static DingTalkJsonResult CreateFailResult(string errMsg)
        {
            return new DingTalkJsonResult()
            {
                IsSuccess = false,
                Errcode = -1,
                Errmsg = errMsg
            };
        }
        public static DingTalkJsonResult CreateFailResult(long errorCode, string errMsg)
        {
            return new DingTalkJsonResult()
            {
                IsSuccess = false,
                Errcode = errorCode,
                Errmsg = errMsg
            };
        }
    }
    public class DingTalkJsonResult<T> : DingTalkJsonResult
    { 
    
    }
}
