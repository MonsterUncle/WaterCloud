using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using WaterCloud.DingTalk.Entities;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 钉钉待办事项业务操作
    /// 宜宾机电一体化研究所：吴远德
    /// 相关API开发文档见：https://open-doc.dingtalk.com/microapp/serverapi2/pgoxpy
    /// 需要结合钉钉API接口.Net版本动态库文件TopSdk，引用 DingTalk.Api
    /// 创建时间：2018-12-15
    /// </summary>
    public class WorkRecordApi
    {

        /// <summary>
        /// 发起一条待办事项
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId">待办事项对应的用户ID</param>
        /// <param name="title">待办事项的标题</param>
        /// <param name="url">待办事项的跳转链接</param>
        /// <param name="form">表单内容（标题+内容）</param>
        /// <returns></returns>
        public static SendWorkRecordResult AddWorkRecord(string accessToken,string userId, string title, string url, Dictionary<string,object> form)
        {
            SendWorkRecordResult result = new SendWorkRecordResult();
            try
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                long time = Convert.ToInt64(ts.TotalSeconds);
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/workrecord/add");
                OapiWorkrecordAddRequest req = new OapiWorkrecordAddRequest();
                req.Userid = userId;
                req.CreateTime = time;//待办时间。Unix时间戳，毫秒级
                req.Title = title;
                req.Url = url;
                List<OapiWorkrecordAddRequest.FormItemVoDomain> list2 = new List<OapiWorkrecordAddRequest.FormItemVoDomain>();
                OapiWorkrecordAddRequest.FormItemVoDomain obj3 = new OapiWorkrecordAddRequest.FormItemVoDomain();
                foreach(KeyValuePair<string, object> item in form)
                {
                    obj3.Title = item.Key;
                    obj3.Content = item.Value.ToString();
                    list2.Add(obj3);
                }
                req.FormItemList_ = list2;
                OapiWorkrecordAddResponse response = client.Execute(req, accessToken);
               
                if (response.Errcode == 0)
                {
                   return  result = SendWorkRecordResult.CreateSuccessResult(
                    response.Errcode,
                    response.ErrMsg,
                    response.RecordId
                    );
                }
                else
                {
                    return result = SendWorkRecordResult.CreateFailResult(
                    response.Errcode,
                    response.SubErrMsg
                    ); 
                }
            }
            catch
            {
                throw new Exception("发送出错");
            }
        }

        /// <summary>
        /// 更新一条待办事项,该待办事项在该用户的“待办事项”列表页面中消失
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <param name="record_id"></param>
        /// <returns></returns>
        public static SendWorkRecordResult UpdateWorkRecord(string accessToken, string userId, string record_id)
        {
            SendWorkRecordResult result = new SendWorkRecordResult();
            try
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                long time = Convert.ToInt64(ts.TotalSeconds);
                IDingTalkClient client = new  DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/workrecord/update");
                OapiWorkrecordUpdateRequest req = new OapiWorkrecordUpdateRequest();
                req.Userid = userId;
                req.RecordId = record_id;//待办时间。Unix时间戳，毫秒级
                OapiWorkrecordUpdateResponse response = client.Execute(req, accessToken);
                if (response.Errcode == 0)
                {
                    return result = SendWorkRecordResult.CreateSuccessResult(
                     response.Errcode,
                     response.ErrMsg,
                     response.Result
                     );
                }
                else
                {
                    return result = SendWorkRecordResult.CreateFailResult(
                    response.Errcode,
                    response.SubErrMsg
                    );
                }
            }
            catch
            {
                throw new Exception("发送出错");
            }
        }

        /// <summary>
        /// 获取用户待办事项 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<Workrecord> getWorkRecordbyUserid(string accessToken, string userId)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/workrecord/getbyuserid");
                OapiWorkrecordGetbyuseridRequest req = new OapiWorkrecordGetbyuseridRequest();
                req.Userid = userId;
                req.Offset = 0L;//分页游标，从0开始
                req.Limit = 0L;//分页大小，最多50
                req.Status = 0L;//待办事项状态，0表示未完成，1表示完成
                OapiWorkrecordGetbyuseridResponse response = client.Execute(req, accessToken);
                if (response.Errcode == 0)
                {
                    List<Workrecord> list = new List<Workrecord>();
                    OapiWorkrecordGetbyuseridResponse.PageResultDomain result = new OapiWorkrecordGetbyuseridResponse.PageResultDomain();
                    result = response.Records;
                    if (result.List.Count > 0)
                    {
                        foreach(OapiWorkrecordGetbyuseridResponse.WorkRecordVoDomain record in result.List)
                        {
                            Workrecord model = new Workrecord();
                            model.RecordId = record.RecordId;
                            model.CreateTime = DingTalkUtils.TimeStampToDateTime(record.CreateTime);
                            model.forms[0] = new Workrecord.FormsModel();
                            model.forms[0].Title = record.Forms[0].Title;
                            model.forms[0].Content = record.Forms[0].Content;
                            list.Add(model);
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new Exception("发送出错");
            }
        }
    }

    /// <summary>
    /// 工作任务返回状态
    /// </summary>
    public class SendWorkRecordResult : DingTalkJsonResult
    {
        public SendWorkRecordResult() { }
        public string RecordId { get; set; }
        public new bool Result { get; set; }
        
        public static SendWorkRecordResult CreateSuccessResult(long errorCode, string errMsg, string record_id)
        {
            return new SendWorkRecordResult()
            {
                IsSuccess = true,
                Errcode = errorCode,
                Errmsg = errMsg,
                RecordId = record_id
            };
        }
        public static SendWorkRecordResult CreateSuccessResult(long errorCode, string errMsg, bool result)
        {
            return new SendWorkRecordResult()
            {
                IsSuccess = true,
                Errcode = errorCode,
                Errmsg = errMsg,
                Result = result
            };
        }
        public static new SendWorkRecordResult CreateFailResult(string errMsg)
        {
            return new SendWorkRecordResult()
            {
                IsSuccess = false,
                Errcode = -1,
                Errmsg = errMsg
            };
        }
        public static new SendWorkRecordResult CreateFailResult(long errorCode, string errMsg)
        {
            return new SendWorkRecordResult()
            {
                IsSuccess = false,
                Errcode = errorCode,
                Errmsg = errMsg
            };
        }
    }
}
