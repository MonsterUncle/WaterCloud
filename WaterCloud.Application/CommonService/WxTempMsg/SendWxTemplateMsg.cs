using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterCloud.Code;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace WaterCloud.Application.CommonService
{
    /// <summary>
    /// 发送微信公众号模板消息
    /// </summary>
    public  class SendWxTemplateMsg
    {
        public static bool SendBindDeviceTemplateMsg(string openid,string serialNo,string companyname)
        {
            try
            {
                if (!string.IsNullOrEmpty(openid))
                {
                    
                    var template_id = "KyUHKW-WE-QfrqKWarijRQ9pa64gF3pdo68cpQWtTxQ";
                    //{ { first.DATA} }
                    //设备编号：{ { keyword1.DATA} }
                    //售后服务电话：{ { keyword2.DATA} }
                    //{ { remark.DATA} }
                    string data_first = "尊敬的用户，您已成功绑定设备，公司名称："+ companyname;
                    string data_remark = "感谢您的使用，如有疑问，请与客服人员联系。";
                    var data = new
                    {
                        first = new TemplateDataItem(data_first, "#3990eb"),
                        keyword1 = new TemplateDataItem(serialNo, "#feb91a"),
                        keyword2 = new TemplateDataItem("15378272341"),
                        remark = new TemplateDataItem(data_remark)
                    };
                    WxTemplateMessageServer.SendTemplateMessage(openid, template_id, data);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "88")//排除白名单问题
                {
                    throw new Exception(ex.Message);
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 售后服务结束通知
        /// </summary>
        /// <param name="mEntity"></param>
        /// <returns></returns>
        public static bool SendFinishWorkOrderMsg(string uuid,string procduct,string mode)
        {
            try
            {
                List<string> users = new List<string>();
                users.Add("dd");// openId
                if (users.Count > 0)
                {
                    string url = HttpHelper.GetWebSite() + "/WeixinApp/WorkOrder/Detail?keyValue=" + uuid;
                    var template_id = "JOMBZ4cqjRgHEC-v8YBugMyGoBN0KxVtSqPqqy951eo";
                    //{ { first.DATA} }
                    //产品型号：{ { keyword1.DATA} }
                    //产品序列号：{ { keyword2.DATA} }
                    //联系人：{ { keyword3.DATA} }
                    //电话：{ { keyword4.DATA} }
                    //寄回地址：{ { keyword5.DATA} }
                    //{ { remark.DATA} }
                    string data_first = "您好，您提交的问题单号为‘PD20011’已处理完成";
                    string data_remark = "点击【详情】查看处理情况，如有问题请致电：15378272341";
                    var data = new
                    {
                        first = new TemplateDataItem(data_first, "#3990eb"),
                        keyword1 = new TemplateDataItem(procduct, "#feb91a"),
                        keyword2 = new TemplateDataItem(mode),
                        keyword3 = new TemplateDataItem("李四" ),
                        keyword4 = new TemplateDataItem("1822222222"),
                        keyword5 = new TemplateDataItem("无需寄回"),
                        remark = new TemplateDataItem(data_remark)
                    };
                    foreach(string openid in users)
                    {
                        if(!string.IsNullOrEmpty(openid)){
                            WxTemplateMessageServer.SendTemplateMessage(openid, template_id, data, url);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("模板消息："+ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="openids"></param>
        /// <param name="productname"></param>
        /// <param name="serno"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool SendExpireDeviceRemindMsg(List<string> openids, string productname,string serialNo, string date)
        {
            try
            {
                if (openids.Count > 0)
                {
                    //string url = HttpHelper.GetWebSite() + "/WeixinApp/WorkOrder/Detail?keyValue=" ;
                    var template_id = "WuxpV2O-LLzUdVw3CPt9ypizV2IBzq2VJCGy4cATo80";
                    //{{ first.DATA}}
                    //设备编号：{{ keyword1.DATA}}
                    //到期时间：{{ keyword2.DATA}}
                    //{{ remark.DATA}}
                    string data_first = "您好，您购买的：" + productname + "系统授权即将到期";
                    string data_remark = "请尽快联系售后专员处理，联系电话：15378272341";
                    var data = new
                    {
                        first = new TemplateDataItem(data_first, "#3990eb"),
                        keyword1 = new TemplateDataItem(serialNo, "#feb91a"),
                        keyword2 = new TemplateDataItem(date),
                        remark = new TemplateDataItem(data_remark)
                    };
                    foreach (string openid in openids)
                    {
                        if (!string.IsNullOrEmpty(openid))
                        {
                            WxTemplateMessageServer.SendTemplateMessage(openid, template_id, data);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("模板消息：" + ex.Message);
            }
        }
    }
}
