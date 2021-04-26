using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WaterCloud.Code.SendEmail;


namespace WaterCloud.WebApi.Controllers
{
    /// <summary>
    /// 邮件发送 Create by BanNian
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailsController : ControllerBase
    {
        // GET: api/<SendEmailsController>

        int row = -1;

        #region 邮件发送
        /// <summary>
        /// 工艺点检异常邮件推送
        /// </summary>
        /// <param name="AssetID">资产编号</param>
        /// <param name="AssetName">资产名称</param>
        /// <param name="TrueName">操作人</param>
        /// <param name="UserDep">部门</param>
        /// <param name="sj">发送人</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendGYDJ")]
        public int SendGYDJ(string AssetID, string AssetName, string TrueName, string UserDep, string sj)
        {
            // msg = @"资产设备[WH2034261]@资产名称[620机器人]@备注信息 [设备异常，请保全员及时现场确认]@点检时间[2020-01-18 16:01:08]@操作人[朱邦灏]";
            /*
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dtSend = new DataTable();

                string sql = "";

                string to = "";//发送人
                string cc = "";//抄送人

                sql = " SELECT * FROM EquipmentCheck.dbo.SendEmail   WHERE TYPES='6'  AND DepName='" + sj + "'";
                dtSend = db.GetDataTableSQL(sql, EquipmentCheck);
                if (dtSend.Rows.Count > 0)
                {
                    to = dtSend.Rows[0]["Emails1"].ToString();
                    cc = dtSend.Rows[0]["Emails2"].ToString();
                }
                sql = @" SELECT * FROM  dbo.GYDJ_History WHERE AssetID='" + AssetID + "' AND TheDate=CONVERT(NVARCHAR(100),GETDATE(),23) AND CheckPartivel IN(  SELECT CheckPartivel FROM dbo.history_Check WHERE  AssetID='" + AssetID + "' ) ORDER BY  id DESC ";
                dt1 = db.GetDataTableSQL(sql, EquipmentCheck);
                if (dt1.Rows.Count > 0)
                {
                    string text = @"<style type='text/css'> th,td{border:1px solid #bbb;} tr{line-height:25px;height:30px;} th,td{white-space:nowrap;text-align:center;padding:2px 5px;} .c{background:#dce6f1;}    .r{color:#f00;}    div{font-size:20px;line-height:150%;} </style>
                            <div><b>此邮件由【武汉智能分析平台】系统自动发送，请勿直接回复!!!<br/> 资产设备[" + AssetID + "] <br/> 资产名称[" + AssetName
                                + "] <br/> 使用部门[" + UserDep + "] <br/> 备注信息 [设备异常，请保全员及时现场确认] <br/> 点检时间[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                + "] <br/> 操作人[" + TrueName + "] <br/>  </div><div>&nbsp;</div>";
                    string title = "【" + AssetID + "】-工艺点检设备异常通报";
                    text += @"<style type='text/css'>    th,td{border:1px solid #bbb;}    tr{line-height:25px;height:30px;}    th,td{white-space:nowrap;text-align:center;padding:2px 5px;}    .c{background:#dce6f1;}
                            .r{color:#f00;}    <table style='border-collapse:collapse;'><tr><td colspan='10' style='color:#fff;background:#538dd5;font-weight:bold;'></td></tr>";
                    text += "<tr><th>序号</th><th>资产编号</th><th>部位</th><th>点检结论</th><th>保全结论</th><th>日期</th><th>操作人</th><th>操作时间</th><th>备注</th></tr>";
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        text += "<tr><th>" + (i + 1) + "</th><th>" + dt1.Rows[i]["AssetID"] + "</th><th>" + dt1.Rows[i]["CheckPartivel"]
                         + "</th><th>" + dt1.Rows[i]["Result"] + "</th><th>" + dt1.Rows[i]["Result2"] + "</th><th>" + dt1.Rows[i]["TheDate"]
                         + "</th><th>" + dt1.Rows[i]["AddName"] + "</th><th>" + dt1.Rows[i]["AddTime"] + "</th><th>" + dt1.Rows[i]["Remarks"] + "</th></tr>";
                    }
                    text += "</table>";

                    var message = EmailHelper.CreateMessage(to, cc, "");
                    message.Subject = title;
                    message.Body = text;
                    EmailHelper.SendMail(message);
                    row = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                row = -1;
            }
            */
            return row;
        } 
        #endregion


    }
}
