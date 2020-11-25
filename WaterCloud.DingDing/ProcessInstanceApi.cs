using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterCloud.Code;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using WaterCloud.DingTalk.Entities;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 钉钉审批业务操作
    /// 宜宾机电一体化研究所：吴远德
    /// 相关API开发文档见：https://open-doc.dingtalk.com/microapp/serverapi2/ca8r99
    /// 需要结合钉钉API接口.Net版本动态库文件TopSdk，引用 DingTalk.Api
    /// 创建时间：2019-4-15
    /// </summary>
    public class ProcessInstanceApi
    {
        /// <summary>
        /// 发起审批实例
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="processinstance">审批示例表单</param>
        /// <param name="form"></param>
        /// <returns></returns>
        public static string Create(string accessToken, Processinstance processinstance)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/processinstance/create");
                OapiProcessinstanceCreateRequest request = new OapiProcessinstanceCreateRequest();
                //request.AgentId = 41605932L;                              //企业应用标识(ISV调用必须设置)
                request.ProcessCode = processinstance.ProcessCode;          //审批流的唯一码，process_code就在审批流编辑的页面URL中:"PROC-BY6LI83V-4R8T1CHNUX58O0Z6C55M3-DTQJJGEJ-1"
                List<OapiProcessinstanceCreateRequest.FormComponentValueVoDomain> formComponentValues = new List<OapiProcessinstanceCreateRequest.FormComponentValueVoDomain>();
                OapiProcessinstanceCreateRequest.FormComponentValueVoDomain vo = new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain();
               
                foreach (FormComponentValue item  in processinstance.FormComponentValues)
                {
                    switch (item.FieldType)
                    {
                        case FormField.singletext:
                        case FormField.multipletext:
                            vo.Name = item.Name;
                            vo.Value = item.Value.ToString();
                            formComponentValues.Add(vo);
                            break;
                        case FormField.image:
                            vo.Name = item.Name;
                            vo.Value = item.Value.ToString();
                            formComponentValues.Add(vo);
                            break;
                        case FormField.attach:
                            vo.Name = item.Name;
                            vo.Value = item.Value.ToString();
                            formComponentValues.Add(vo);
                            break;
                        case FormField.detail:
                            vo.Name = item.Name;
                            var data = item.Value.ToString();
                            formComponentValues.Add(vo);
                            break;
                    }
                }
               
                request.FormComponentValues_ = formComponentValues;
                request.Approvers = processinstance.Approvers;                      //审批人userid列表，最大列表长度：20。多个审批人用逗号分隔，按传入的顺序依次审批
                request.OriginatorUserId = processinstance.OriginatorUserId;        //审批实例发起人的userid
                request.DeptId = processinstance.OriginatorDeptId;                  //发起人所在的部门，如果发起人属于根部门，传-1
                request.CcList = Utils.List2String(processinstance.CcUserids);                            //(可选)抄送人userid列表，最大列表长度：20。多个抄送人用逗号分隔
                request.CcPosition = processinstance.CcPosition;                    //(可选)抄送时间，分为（START, FINISH, START_FINISH）
                OapiProcessinstanceCreateResponse response = client.Execute(request, accessToken);
                if (response.Errcode == 0)
                {
                    return response.ProcessInstanceId;
                }
                else
                {
                    throw new Exception(response.Errcode.ToString());
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 批量获取审批实例id
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="processCode">审批流的唯一码</param>
        /// <param name="useridlist">发起人用户id列表</param>
        /// <param name="starttime">查询开始时间</param>
        /// <returns></returns>
        public static List<string> GetListIds(string accessToken, string processCode,string useridlist,DateTime starttime)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/processinstance/listids");
                OapiProcessinstanceListidsRequest request = new OapiProcessinstanceListidsRequest();
                request.ProcessCode = processCode;
                request.StartTime= DingTalkUtils.DateTimeToTimeStamp(starttime);            //开始时间。Unix时间戳
                request.EndTime=DingTalkUtils.DateTimeToTimeStamp(DateTime.Now);            //结束时间，默认取当前时间。Unix时间戳
                request.Size=10L;                                                           //分页参数，每页大小，最多传10，默认值：10
                request.Cursor=0L;                                                          //分页查询的游标，最开始传0，后续传返回参数中的next_cursor值，默认值：0
                request.UseridList= useridlist;                                             //发起人用户id列表，用逗号分隔，最大列表长度：10
                OapiProcessinstanceListidsResponse response = client.Execute(request, accessToken);
                if (response.Errcode == 0)
                {
                    return response.Result.List;
                }
                else
                {
                    throw new Exception(response.Errcode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取单个审批实例
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="process_instance_id">审批实例id</param>
        /// <returns></returns>
        public static Processinstance GetListIds(string accessToken, string process_instance_id)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/processinstance/get");
                OapiProcessinstanceGetRequest request = new OapiProcessinstanceGetRequest();
                request.ProcessInstanceId= process_instance_id;
                OapiProcessinstanceGetResponse response = client.Execute(request, accessToken);
                if (response.Errcode == 0)
                {
                    Processinstance processinstance = new Processinstance();
                    processinstance.Title = response.ProcessInstance.Title;
                    processinstance.CreateTime = response.ProcessInstance.CreateTime;
                    processinstance.FinishTime = response.ProcessInstance.FinishTime;
                    processinstance.OriginatorUserId = response.ProcessInstance.OriginatorUserid;
                    processinstance.OriginatorDeptId = Convert.ToInt32(response.ProcessInstance.OriginatorDeptId);
                    processinstance.Status = response.ProcessInstance.Status;
                    processinstance.CcUserids = response.ProcessInstance.CcUserids;
                    processinstance.Result = response.ProcessInstance.Result;
                    processinstance.BizAction = response.ProcessInstance.BizAction;
                    processinstance.BusinessId = response.ProcessInstance.BusinessId;
                    processinstance.OriginatorDeptName = response.ProcessInstance.OriginatorDeptName;
                    processinstance.FormComponentValues = new List<FormComponentValue>();
                    foreach (OapiProcessinstanceGetResponse.FormComponentValueVoDomain item in response.ProcessInstance.FormComponentValues)
                    {
                        FormComponentValue form = new FormComponentValue();
                        form.Name = item.Name;
                        form.Value = item.Value;
                        form.ExtValue = item.ExtValue;
                        processinstance.FormComponentValues.Add(form);
                    }

                    foreach (OapiProcessinstanceGetResponse.OperationRecordsVoDomain item in response.ProcessInstance.OperationRecords)
                    {
                        OperationRecord model = new OperationRecord();
                        model.UserId = item.Userid;
                        model.Date = Convert.ToDateTime(item.Date); ;
                        model.OperationType = item.OperationType;
                        model.OperationResult = item.OperationResult;
                        model.Remark = item.Remark;
                        processinstance.OperationRecords.Add(model);
                    }
                    foreach (OapiProcessinstanceGetResponse.TaskTopVoDomain item in response.ProcessInstance.Tasks)
                    {
                        Task model = new Task();
                        model.UserId = item.Userid;
                        model.TaskStatus = item.TaskStatus;
                        model.TaskResult = item.TaskResult;
                        model.CreateTime = Convert.ToDateTime(item.CreateTime);
                        model.FinishTime = Convert.ToDateTime(item.FinishTime);
                        processinstance.Tasks.Add(model);
                    }
                    return processinstance;
                }
                else
                {
                    throw new Exception(response.Errcode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取用户待审批数量 
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="userid">用户Id</param>
        /// <returns></returns>
        public static long GetTodoNum(string accessToken,string userid)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/process/gettodonum");
                OapiProcessGettodonumRequest req = new OapiProcessGettodonumRequest();
                req.Userid= userid;
                OapiProcessGettodonumResponse response = client.Execute(req, accessToken);
                if (response.Errcode == 0)
                {
                    return response.Count;
                }
                else
                {
                    throw new Exception(response.Errcode.ToString());
                }
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取用户可见的审批模板
        /// </summary>
        /// <param name="accessToken">accessToken</param>
        /// <param name="userid">用户ID,不传表示获取企业的所有模板</param>
        /// <returns></returns>
        public static List<ProcessTemplate> GetProcessListByUserid(string accessToken, string userid="")
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/process/listbyuserid");
                OapiProcessListbyuseridRequest request = new OapiProcessListbyuseridRequest();
                if (!string.IsNullOrEmpty(userid))
                {
                    request.Userid = userid;
                }
                request.Offset=0L;
                request.Size=100L;
                OapiProcessListbyuseridResponse response = client.Execute(request, accessToken);
                if (response.Errcode == 0)
                {
                    List<ProcessTemplate> list = new List<ProcessTemplate>();
                    foreach (OapiProcessListbyuseridResponse.ProcessTopVoDomain item in response.Result.ProcessList)
                    {
                        ProcessTemplate form = new ProcessTemplate();
                        form.Name = item.Name;
                        form.IconUrl = item.IconUrl;
                        form.ProcessCode = item.ProcessCode;
                        form.Url = item.Url;
                        list.Add(form);
                    }
                    return list;
                }
                else
                {
                    throw new Exception(response.Errcode.ToString());
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
