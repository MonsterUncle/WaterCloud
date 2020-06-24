﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using UEditor.Core.Handlers;

namespace UEditor.Core
{
    public class UEditorActionCollection : Dictionary<string, Action<HttpContext>>
    {
        public UEditorActionCollection()
        {
            Add("config", ConfigAction);
            Add("uploadimage", UploadImageAction);
            Add("uploadscrawl", UploadScrawlAction);
            Add("uploadvideo", UploadVideoAction);
            Add("uploadfile", UploadFileAction);
            Add("listimage", ListImageAction);
            Add("listfile", ListFileAction);
            Add("catchimage", CatchImageAction);
        }

        public new UEditorActionCollection Add(string action, Action<HttpContext> handler)
        {
            if (ContainsKey(action))
                this[action] = handler;
            else
                base.Add(action, handler);

            return this;
        }

        public new UEditorActionCollection Remove(string action)
        {
            base.Remove(action);
            return this;
        }

        private void ConfigAction(HttpContext context)
        {
            new ConfigHandler(context).Process();
        }

        private void UploadImageAction(HttpContext context)
        {
            new UploadHandler(context, new UploadConfig()
            {
                AllowExtensions = Config.GetStringList("imageAllowFiles"),
                PathFormat = Config.GetString("imagePathFormat"),
                SaveAbsolutePath = Config.GetString("imageSaveAbsolutePath"),
                FtpUpload = Config.GetValue<bool>("imageFtpUpload"),
                FtpAccount = Consts.ImgFtpServer.account,
                FtpPwd = Consts.ImgFtpServer.pwd,
                FtpIp = Consts.ImgFtpServer.ip,
                SizeLimit = Config.GetInt("imageMaxSize"),
                UploadFieldName = Config.GetString("imageFieldName")
            }).Process();
        }

        private void UploadScrawlAction(HttpContext context)
        {
            new UploadHandler(context, new UploadConfig()
            {
                AllowExtensions = new string[] { ".png" },
                PathFormat = Config.GetString("scrawlPathFormat"),
                SizeLimit = Config.GetInt("scrawlMaxSize"),
                UploadFieldName = Config.GetString("scrawlFieldName"),
                FtpUpload = Config.GetValue<bool>("scrawlFtpUpload"),
                FtpAccount = Consts.ImgFtpServer.account,
                FtpPwd = Consts.ImgFtpServer.pwd,
                FtpIp = Consts.ImgFtpServer.ip,
                Base64 = true,
                Base64Filename = "scrawl.png"
            }).Process();
        }

        private void UploadVideoAction(HttpContext context)
        {
            new UploadHandler(context, new UploadConfig()
            {
                AllowExtensions = Config.GetStringList("videoAllowFiles"),
                PathFormat = Config.GetString("videoPathFormat"),
                SizeLimit = Config.GetInt("videoMaxSize"),
                FtpUpload = Config.GetValue<bool>("videoFtpUpload"),
                FtpAccount = Consts.ImgFtpServer.account,
                FtpPwd = Consts.ImgFtpServer.pwd,
                FtpIp = Consts.ImgFtpServer.ip,
                UploadFieldName = Config.GetString("videoFieldName")
            }).Process();
        }

        private void UploadFileAction(HttpContext context)
        {
            new UploadHandler(context, new UploadConfig()
            {
                AllowExtensions = Config.GetStringList("fileAllowFiles"),
                PathFormat = Config.GetString("filePathFormat"),
                SizeLimit = Config.GetInt("fileMaxSize"),
                FtpUpload = Config.GetValue<bool>("fileFtpUpload"),
                FtpAccount = Consts.ImgFtpServer.account,
                FtpPwd = Consts.ImgFtpServer.pwd,
                FtpIp = Consts.ImgFtpServer.ip,
                UploadFieldName = Config.GetString("fileFieldName")
            }).Process();
        }

        private void ListImageAction(HttpContext context)
        {
            new ListFileManager(
                    context,
                    Config.GetString("imageManagerListPath"),
                    Config.GetStringList("imageManagerAllowFiles"))
                .Process();
        }

        private void ListFileAction(HttpContext context)
        {
            new ListFileManager(
                    context,
                    Config.GetString("fileManagerListPath"),
                    Config.GetStringList("fileManagerAllowFiles"))
                .Process();
        }

        private void CatchImageAction(HttpContext context)
        {
            new CrawlerHandler(context).Process();
        }
    }
}
