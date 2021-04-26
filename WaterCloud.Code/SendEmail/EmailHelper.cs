using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code.SendEmail
{
    public class EmailHelper
    {
        private static EmailMessage CreateMessage(bool admin = false)
        {
            string credentialUserName;
            string credentialUserPwd;
            if (admin)
            {
                credentialUserName = "740003";
                credentialUserPwd = "qwe!23";
            }
            else
            {
                credentialUserName = "740006";
                credentialUserPwd = "qwe!23";
            }
            string domain = "it2004.gree.com.cn";

            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            service.Credentials = new NetworkCredential(credentialUserName, credentialUserPwd, domain);
            service.Url = new Uri("https://whmail03.it2004.gree.com.cn/ews/Exchange.asmx");
            return new EmailMessage(service);
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            //为了通过证书验证，总是返回true
            return true;
        }

        public  static EmailMessage CreateMessage(string to, string cc, string bc, bool admin = false)
        {
            var message = CreateMessage(admin);
            var tos = EmailHelper.SplitAddress(to);
            if (tos != null)
                message.ToRecipients.AddRange(tos);
            var ccs = EmailHelper.SplitAddress(cc);
            if (ccs != null)
                message.CcRecipients.AddRange(ccs);
            var bcs = EmailHelper.SplitAddress(bc);
            if (bcs != null)
                message.BccRecipients.AddRange(bcs);
            return message;
        }

        public static bool SendMail(EmailMessage msg)
        {
            // 第一步：连接邮件服务器
            try
            {
                if (msg.ToRecipients.Count + msg.CcRecipients.Count > 0)
                {
                    msg.Send();
                }
                return true;
            }
            catch (Exception ex)
            {
                // 错误时，重新发送

                try
                {
                    msg.Send();

                    return true;
                }
                catch (Exception ex2)
                {

                    msg.Body = string.Format("<div><font color='red'>{0}</font></div><div>第一次：{1}</div><div>StackTrace：{2}</div><div>第二次：{3}</div><div>StackTrace：{4}</div><div>{5}</div>",
                        msg.Subject, ex.Message, ex.StackTrace, ex2.Message, ex2.StackTrace, msg.Body.Text);
                    msg.Subject = "发送邮件失败（2）"; // 邮件主题
                    msg.ToRecipients.Add("742317@gree.com.cn");
                    msg.Importance = Importance.High;
                    msg.Send();

                    return false;
                }
            }
        }

        internal static IEnumerable<string> SplitAddress(string str)
        {
            if (str == null) return null;
            List<string> result = new List<string>();
            string ADname = "LDAP://it2004.gree.com.cn"; // +DomainName;
            string Loginname = "740006";
            string Loginpwd = "qwe!23";
            System.DirectoryServices.DirectoryEntry objDE = new System.DirectoryServices.DirectoryEntry(ADname, Loginname, Loginpwd);

            var tos = str.Split(';', '；');
            foreach (var item in tos)
            {
                string tmp = item.Trim();
                if (!string.IsNullOrEmpty(tmp))
                {
                    int l1 = tmp.IndexOf('<');
                    int l2 = tmp.IndexOf('>');
                    int l3 = tmp.IndexOf('@');
                    string strFilter = "(&(&(objectCategory=person)(objectClass=user))(displayname=" + tmp + "))";
                    if (l1 >= 0 && l2 > l3 && l3 > l1)
                    {
                        strFilter = "(&(&(objectCategory=person)(objectClass=user))(mail=" + tmp.Substring(l1 + 1, l2 - l1 - 1) + "))";
                    }
                    else
                    {
                        l1 = tmp.LastIndexOf('(');
                        l2 = tmp.LastIndexOf(')');
                        if (l1 >= 0 && l2 > l3 && l3 > l1)
                        {
                            strFilter = "(&(&(objectCategory=person)(objectClass=user))(mail=" + tmp.Substring(l1 + 1, l2 - l1 - 1) + "))";
                        }
                    }
                    DirectorySearcher objSearcher = new DirectorySearcher(objDE, strFilter);
                    SearchResult src = objSearcher.FindOne();
                    if (src != null)
                    {
                        //System.Diagnostics.Debug.WriteLine(src.Path);
                        foreach (DictionaryEntry itemA in src.Properties)
                        {
                            if (itemA.Key.Equals("mail"))
                            {
                                foreach (var itemB in itemA.Value as ResultPropertyValueCollection)
                                {
                                    string address = itemB.ToString();
                                    if (address.Contains('@'))
                                        result.Add(address);
                                }
                            }
                        }
                    }
                }
            }
            return result;

        }
    }
}
