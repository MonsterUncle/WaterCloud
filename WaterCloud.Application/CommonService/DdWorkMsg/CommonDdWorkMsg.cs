using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterCloud.Code;
using WaterCloud.DingTalk;
using WaterCloud.DingTalk.Entities;
using WaterCloud.Application.DingTalkManage;
namespace WaterCloud.Application.CommonService
{
    /// <summary>
    /// 钉钉消息推送
    /// </summary>
    public class CommonDdWorkMsg
    {
        /// <summary>
        /// 产品注册授权申请通知推送
        /// </summary>
        /// <param name="usersid"></param>
        /// <param name="mEntity"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool SendLicenseApplyWorkMsg(string uuid)
        {
            try
            {
                DingTalkCorpConfig config = (new DingTalkApp()).GetAccessToken();
                List<string> users = new List<string>();
                users.Add("172557595021881487");// 钉钉UserId
                if (users.Count > 0)
                {
                    string title = "产品注册授权审核通知";
                    string msg = string.Format("# {0}向您申请授权MDK产品注册码，请审批  \n  客户名称：{1}  \n  产品名称：{2}  \n  申请到日：{3}  \n  # 备注说明：{4}");
                    string url = HttpHelper.GetWebSite() + "/ProductManage/ProductLicence/LicenceAudit?keyValue=" + uuid;
                    var result = WorkMessageApi.SendMessagebyActionCard(
                        config.AccessToken, 211537030, EnumClass.RecvType.UseridList, users, title, msg, "查看详情", url);
                    (new DingTalkWorkMsgApp()).AddWorkMsgLog(config.AccessToken, 211537030, title, users.ToString(), result.Errmsg);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message!="88")//排除白名单问题
                {
                    throw new Exception(ex.Message);
                }
                else
                {
                    return true;
                }
            }
        }

       

    
    }
}
