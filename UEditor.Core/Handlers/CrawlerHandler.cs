using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace UEditor.Core.Handlers
{
    public class CrawlerHandler : Handler
    {
        private string[] Sources;
        private Crawler[] Crawlers;
        public CrawlerHandler(HttpContext context) : base(context) { }

        public override void Process()
        {
            Sources = Request.Form["source[]"];
            if (Sources == null || Sources.Length == 0)
            {
                WriteJson(new
                {
                    state = "参数错误：没有指定抓取源"
                });
                return;
            }
            Crawlers = Sources.Select(x => new Crawler(x).Fetch()).ToArray();
            WriteJson(new
            {
                state = "SUCCESS",
                list = Crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = x.ServerUrl
                })
            });
        }
    }

    public class Crawler
    {
        public string SourceUrl { get; set; }
        public string ServerUrl { get; set; }
        public string State { get; set; }

        // private HttpServerUtility Server { get; set; }


        public Crawler(string sourceUrl)
        {
            this.SourceUrl = sourceUrl;
            // this.Server = server;
        }

        public Crawler Fetch()
        {
            if (!IsExternalIPAddress(this.SourceUrl))
            {
                State = "INVALID_URL";
                return this;
            }
            var request = HttpWebRequest.Create(this.SourceUrl) as HttpWebRequest;
            using (var response = request.GetResponseAsync().Result as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    State = "Url returns " + response.StatusCode + ", " + response.StatusDescription;
                    return this;
                }
                if (response.ContentType.IndexOf("image") == -1)
                {
                    State = "Url is not an image";
                    return this;
                }
                ServerUrl = PathFormatter.Format(Path.GetFileName(this.SourceUrl), Config.GetString("catcherPathFormat"));
                //var savePath = Path.Combine(Config.WebRootPath, ServerUrl);
                var savePath = Path.Combine(Config.GetString("catcherSaveAbsolutePath"), ServerUrl);
                //

                try
                {
                    var stream = response.GetResponseStream();
                    //
                    if (Config.GetValue<bool>("catcherFtpUpload"))
                    {
                        var fileExt = ServerUrl.Substring(savePath.LastIndexOf('.')).TrimStart('.');
                        var key = Config.GetString("catcherPathFormat") + "." + fileExt;
                        //FtpUpload.UploadFile(stream, savePath, Consts.ImgFtpServer.ip,
                        //    Consts.ImgFtpServer.account, Consts.ImgFtpServer.pwd);
                        AliyunOssUpload.UploadFile(Consts.AliyunOssServer.AccessEndpoint,
                            Consts.AliyunOssServer.AccessKeyId, Consts.AliyunOssServer.AccessKeySecret,
                            Consts.AliyunOssServer.BucketName, key, fileExt, stream);
                    }
                    else
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                        }

                        var reader = new BinaryReader(stream);
                        byte[] bytes;
                        using (var ms = new MemoryStream())
                        {
                            byte[] buffer = new byte[4096];
                            int count;
                            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, count);
                            }

                            bytes = ms.ToArray();
                        }

                        File.WriteAllBytes(savePath, bytes);
                    }

                    State = "SUCCESS";
                }
                catch (Exception e)
                {
                    State = "抓取错误：" + e.Message;
                }
                return this;
            }
        }

        private bool IsExternalIPAddress(string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntryAsync(uri.DnsSafeHost).Result;
                    foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                    {
                        byte[] ipBytes = ipAddress.GetAddressBytes();
                        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (!IsPrivateIP(ipAddress))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case UriHostNameType.IPv4:
                    return !IsPrivateIP(IPAddress.Parse(uri.DnsSafeHost));
            }
            return false;
        }

        private bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }
            return false;
        }
    }
}