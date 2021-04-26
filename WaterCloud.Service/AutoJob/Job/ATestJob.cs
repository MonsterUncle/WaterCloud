using Chloe;
using Chloe.SqlServer;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using WaterCloud.Code; 
using WaterCloud.Service.SystemSecurity;
using Dapper;
using System.Data.SqlClient;
using WaterCloud.Code.SendEmail;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml.Style;
using System.Net;
using Newtonsoft.Json;

namespace WaterCloud.Service.AutoJob.Job
{
    public class ATestJob : IJobTask
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ServerStateService _server;
        public ATestJob(IDbContext context)
        {
            _hostingEnvironment = GlobalContext.HostingEnvironment;
            _server = new ServerStateService(context);
        }
        public async Task<AlwaysResult> Start()
        {
            AlwaysResult obj = new AlwaysResult();
            try
            {
                List<SCB_KQ> lt = new List<SCB_KQ>();
                using (SqlConnection con = new SqlConnection("data source=172.74.1.175;initial catalog=SmartPlatformDB;user id=sa;password=whgree#175;"))
                {                    
                    con.Execute(" INSERT INTO SmartPlatformDB.dbo.AA (UserName,a, b, c) VALUES('测试1','测试1','测试1','测试1') ");
                    lt = con.Query<SCB_KQ>("SELECT TOP 100 * FROM KingDeeDB.dbo.SCB_KQ ").ToList();
                }
                // SendMail(lt, "朱邦灏(武汉生产部) <742353@gree.com.cn>", "", "", "");
                #region 调用WebApi 发送邮件

                string url = string.Format("http://172.74.1.175:8126/SendGYDJ?AssetID={0}&AssetName={1}&TrueName={2}&UserDep={3}&sj={4}",
                   "WH2084001", "全自动轴承装配机", "全自动轴承装配机", "测试", "测试");
                string json = JsonConvert.SerializeObject(new
                {
                    AssetID = "WH2084001",
                    AssetName = "全自动轴承装配机",
                    TrueName = "全自动轴承装配机",
                    UserDep = "测试",
                    sj = "测试"
                });
                var request = WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "Post";
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(json);
                }
                var response = request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var str11 = sr.ReadToEnd();
                } 
                #endregion


                obj.state = ResultType.success.ToString();
                obj.message = "测试成功！";
            }
            catch (Exception ex)
            {
                obj.state = ResultType.error.ToString();
                obj.message = "测试失败！" + ex.Message;
            }
            return obj;
        }


        #region SendMail 操作Excel
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workDay">时间</param>
        /// <param name="dt"></param>
        /// <param name="to">邮件收件人</param>
        /// <param name="cc">邮件抄送人</param>
        /// <param name="bc">邮件密送人</param>
        /// <param name="pc">邮件收件人</param>
        /// <returns></returns>
        private bool SendMail(List<SCB_KQ> lt, string to, string cc, string bc, string pc)
        {
            int rowIndex = 1;
            int col = 9;//列数
            List<SCB_KQ> list = new List<SCB_KQ>();
            ExcelPackage pck = new ExcelPackage();

            #region Sheet1
            var ws = pck.Workbook.Worksheets.Add("Sheet1");
            Lookup<string, SCB_KQ> lookup = (Lookup<string, SCB_KQ>)list.ToLookup(a => a.DepName);
            foreach (var item in lookup)
            {
                List<SCB_KQ> list1 = list.Where(a => a.DepName == item.Key).ToList();
                rowIndex = CreateExcel(list1, ws, item.Key, rowIndex, bc);
                ws.Cells[rowIndex, 1, rowIndex, col].Merge = true;
                rowIndex++;
            }
            ws.Cells.AutoFitColumns();
            for (int i = 1; i <= col; i++)
            {
                ws.Column(i).Width += 3;
            }
            for (int i = 1; i <= rowIndex; i++)
            {
                ws.Row(i).Height = 23;
            }
            #endregion

            #region Sheet2
            rowIndex = 1;
            ws = pck.Workbook.Worksheets.Add("Sheet2");
            list = lt.Where(a => a.Remark == "OK").ToList();
            lookup = (Lookup<string, SCB_KQ>)list.ToLookup(a => a.DepName);
            foreach (var item in lookup)
            {
                List<SCB_KQ> list1 = list.Where(a => a.DepName == item.Key).ToList();
                rowIndex = CreateExcel(list1, ws, item.Key, rowIndex, bc);
                ws.Cells[rowIndex, 1, rowIndex, col].Merge = true;
                rowIndex++;
            }
            ws.Cells.AutoFitColumns();
            for (int i = 1; i <= col; i++)
            {
                ws.Column(i).Width += 3;
            }
            for (int i = 1; i <= rowIndex; i++)
            {
                ws.Row(i).Height = 23;
            }

            #endregion

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,(string.Format("D:/742353/TaskManage/Tmp/{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), Guid.NewGuid().ToString("n").Replace("-", ""))));
       //     filePath = filePath.Replace("C:\\Tmp", "D:\\742317\\TaskManage\\Tmp");
            File.WriteAllBytes(filePath, pck.GetAsByteArray());
            var dateStr = DateTime.Now.ToString("M月d日");
            var message = EmailHelper.CreateMessage(to, cc, bc);
            message.Subject = string.Format("异常信息（{0}）", dateStr);
            //message.Body = "此邮件由系统自动发送，请勿直接回复。<br />";
            message.Body = GetBody(dateStr, lt);
            message.Attachments.AddFileAttachment("异常信息.xlsx", filePath);
            EmailHelper.SendMail(message);
            File.Delete(filePath);
            return true;
        }
        private int CreateExcel(List<SCB_KQ> ax1, ExcelWorksheet ws, string title, int rowIndex, string bc)
        {
            int col = 9;//列数
            ExcelStyle style1 = ws.Cells[rowIndex, 1].Style;
            style1.Font.Bold = true;
            style1.Font.Color.SetColor(Color.White);
            style1.Fill.PatternType = ExcelFillStyle.Solid;
            style1.Fill.BackgroundColor.SetColor(Color.FromArgb(83, 141, 213));
            ws.Cells[rowIndex, 1].Value = string.Format("{0}生产部【{1}{2}】出勤异常信息", ax1.Count > 0 ? ax1[0].Month : DateTime.Now.ToString("yyyy-MM-dd"), title == "" ? "领导" : title, bc);
            ws.Cells[rowIndex, 1, rowIndex, col].Merge = true;
            AddBorder(ws.Cells[rowIndex, 1, rowIndex, 7]);
            AlignmentCenter(ws.Cells[rowIndex, 1, rowIndex, 7]);
            ws.Cells[rowIndex, 1, rowIndex, col].Style.Font.Size = 14;

            ws.Row(rowIndex + 1).Style.Font.Bold = true;
            ws.Cells[rowIndex + 1, 1].Value = "序号";
            ws.Cells[rowIndex + 1, 2].Value = "工号";
            ws.Cells[rowIndex + 1, 3].Value = "姓名";
            ws.Cells[rowIndex + 1, 4].Value = "部门";
            ws.Cells[rowIndex + 1, 5].Value = "日期";
            ws.Cells[rowIndex + 1, 6].Value = "上班时间";
            ws.Cells[rowIndex + 1, 7].Value = "下班时间";
            ws.Cells[rowIndex + 1, 8].Value = "状态";
            ws.Cells[rowIndex + 1, 9].Value = "备注";
            for (int i = 0; i < col; i++)
            {
                AddBorder(ws.Cells[rowIndex + 1, 1 + i]);
                AlignmentCenter(ws.Cells[rowIndex + 1, 1 + i]);
            }
            for (int i = 0; i < ax1.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    ws.Cells[rowIndex + 2 + i, 1, rowIndex + 2 + i, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIndex + 2 + i, 1, rowIndex + 2 + i, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(222, 231, 247));
                }
                var item = ax1.ElementAt(i);
                ws.Cells[rowIndex + 2 + i, 1].Value = i + 1;
                ws.Cells[rowIndex + 2 + i, 2].Value = item.EmpNo;
                ws.Cells[rowIndex + 2 + i, 3].Value = item.EmpName;
                ws.Cells[rowIndex + 2 + i, 4].Value = item.DepName == "" ? "领导" : item.DepName;
                ws.Cells[rowIndex + 2 + i, 5].Value = item.Month;
                ws.Cells[rowIndex + 2 + i, 6].Value = item.SignTimeAM;
                ws.Cells[rowIndex + 2 + i, 7].Value = item.SignTimePM;
                ws.Cells[rowIndex + 2 + i, 8].Value = item.Remark;
                ws.Cells[rowIndex + 2 + i, 9].Value = item.Explain;
                // 边框、居中
                for (int j = 0; j < col; j++)
                {
                    AddBorder(ws.Cells[rowIndex + 2 + i, 1 + j]);
                    AlignmentCenter(ws.Cells[rowIndex + 2 + i, 1 + j]);
                }
            }
            // 添加合计行
            ws.Cells[rowIndex + 2 + ax1.Count(), 1, rowIndex + 2 + ax1.Count(), 2].Merge = true;
            ws.Cells[rowIndex + 2 + ax1.Count(), 1, rowIndex + 2 + ax1.Count(), 2].Value = "总计";
            AddBorder(ws.Cells[rowIndex + 2 + ax1.Count(), 1, rowIndex + 2 + ax1.Count(), 2]);
            AlignmentCenter(ws.Cells[rowIndex + 2 + ax1.Count(), 1, rowIndex + 2 + ax1.Count(), 2]);
            //ws.Cells[rowIndex + 2 + ax1.Count(), 3].Formula = string.Format("SUM({0}{1}:{0}{2})", ToColumn(3), rowIndex + 2, rowIndex + 1 + ax1.Count());
            ws.Cells[rowIndex + 2 + ax1.Count(), 3].Value = ax1.Count();
            for (int i = 2; i < col; i++)
            {
                AddBorder(ws.Cells[rowIndex + 2 + ax1.Count(), 1 + i]);
                AlignmentCenter(ws.Cells[rowIndex + 2 + ax1.Count(), 1 + i]);
            }
            var cfRule = ws.ConditionalFormatting.AddLessThan(new ExcelAddress(rowIndex + 2, col, rowIndex + 2 + ax1.Count(), col));
            cfRule.Formula = "0";
            cfRule.Style.Font.Color.Color = Color.Red;
            if (ax1.Count() % 2 == 0)
            {
                ws.Cells[rowIndex + 2 + ax1.Count(), 1, rowIndex + 2 + ax1.Count(), col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowIndex + 2 + ax1.Count(), 1, rowIndex + 2 + ax1.Count(), col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(222, 231, 247));
            }
            return rowIndex + 3 + ax1.Count();
        }

        private void AlignmentCenter(ExcelRange range)
        {
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        private void AddBorder(ExcelRange range)
        {
            range.Style.Font.Name = "宋体";
            range.Style.Font.Size = 12;
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
        }

        private string ToColumn(int p)
        {
            if (p > 26)
            {
                char A = (char)('A' + (p - 1) / 26 - 1);
                char B = (char)('A' + (p - 1) % 26);
                return new string(new char[] { A, B });
            }
            else
                return ((char)('A' + (p - 1))).ToString();
        }

        private string GetBody(string workDay, IEnumerable<SCB_KQ> lists)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<style type='text/css'>
    th,td{border:1px solid #bbb;}
    tr{line-height:25px;height:30px;}
    th,td{white-space:nowrap;text-align:center;padding:2px 5px;}
    .c{background:#dce6f1;}
    .r{color:#f00;}
    div{font-size:20px;line-height:150%;}
</style>
<div><b>此邮件由系统自动统计分析及发送，请勿直接回复。</b></div>
<table style='border-collapse:collapse;width:700px;'>");
            // 机型条码重复 漏扫描 未收卡 已锁定   合格

            Lookup<string, SCB_KQ> lookup = (Lookup<string, SCB_KQ>)lists.ToLookup(a => a.DepName);
            foreach (var item in lookup)
            {
                List<SCB_KQ> lt = lists.Where(a => a.DepName == item.Key).ToList();
                AppendText(sb, lt[0].Month, lt, item.Key);
            }
            //sb.Remove(sb.Length - 31, 31);
            sb.Append("</table><div>&nbsp;</div>");
            return sb.ToString();
        }
        private void AppendText(StringBuilder sb, string dt, IEnumerable<SCB_KQ> ax1, string title)
        {
            if (ax1.Count() > 0)
            {
                sb.Append("<tr><td colspan='9' style='color:#fff;background:#538dd5;font-weight:bold;'>");
                sb.AppendFormat(@"{0}</td></tr><tr><th>序号</th><th>工号</th><th>姓名</th><th>部门</th><th>日期</th><th>上班时间</th><th>下班时间</th><th>状态</th><th>备注</th></tr>", string.Format("{0}生产部出勤异常信息", dt, title));
                for (int i = 0; i < ax1.Count(); i++)
                {
                    var item = ax1.ElementAt(i);
                    sb.AppendFormat("<tr {0}>", i % 2 == 1 ? "" : "class='c'");
                    sb.AppendFormat(@"<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>", i + 1, item.EmpNo, item.EmpName, item.DepName == "" ? "领导" : item.DepName, item.Month, item.SignTimeAM, item.SignTimePM, item.Remark == "NG" ? "未打卡" : item.Remark, item.Explain);
                }
                sb.AppendFormat("<tr {0}>", ax1.Count() % 2 == 1 ? "" : "class='c'");
                sb.AppendFormat("<td colspan='4'>总计</td><td colspan='5'>{0}条记录</td></tr>", ax1.Count().ToString(""));
                sb.Append("<tr><td colspan='9'></td></tr>");
            }
            else
            {
                sb.Append("暂无数据 ");
            }
        }

        #endregion


    }
    class AAModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string a { get; set; }
        public string b { get; set; }
    }

    class SCB_KQ
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EmpNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DepName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SignTimeAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SignTimePM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Types { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Str { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Str1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Str2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Str3 { get; set; }
    }

}
