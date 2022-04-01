/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Service;
using System;
using System.Threading.Tasks;

namespace WaterCloud.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ModuleController : ControllerBase
    {

        public ModuleService _service { get; set; }
        public ModuleButtonService _moduleButtonService { get; set; }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data =await _service.GetList();
            data = data.Where(a => a.Target == "expand"&&a.IsExpand==true).ToList();
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.FullName;
                treeModel.parentId = item.ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword)
        {
            var data =await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.FullName.Contains(keyword));
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetMaxSortCodeText(string ParentId)
        {
            var data = await _service.GetMaxSortCode(ParentId);

            return Content(data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSelectJson()
        {
            var data = await _service.GetList();
            data = data.Where(a => a.Target == "expand" && a.IsExpand == true).ToList();
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.EnCode;
                treeModel.parentId = item.ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSelectMunuJson(string keyword)
        {
            var data = (await _service.GetList()).Where(a => a.Target=="iframe").ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.FullName.Contains(keyword)).ToList();
            }
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.Id, text = item.FullName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSelectMunuBesidesJson(string keyword)
        {
            var data = await _service.GetBesidesList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(a => a.FullName.Contains(keyword)).ToList();
            }
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.Id, text = item.FullName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _service.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAdmin(false)]
        public async Task<ActionResult> SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.DeleteMark = false;
                moduleEntity.AllowEdit = false;
                moduleEntity.AllowDelete = false;
                moduleEntity.IsPublic = false;
            }
            else
            {
                if (keyValue==moduleEntity.ParentId)
                {
                    throw new Exception("父级不能是自身");
                }
                //前端传值为null，更新的时候null不更新
                if (moduleEntity.Icon==null)
                {
                    moduleEntity.Icon = "";
                }
            }
            try
            {
                if (moduleEntity.ParentId == "0")
                {
                    moduleEntity.Layers = 1;
                }
                else
                {
                    moduleEntity.Layers =(await _service.GetForm(moduleEntity.ParentId)).Layers + 1;
                }
                if (!string.IsNullOrEmpty(moduleEntity.UrlAddress))
                {
                    var templist = await _service.GetList();
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        templist = templist.Where(a => a.Id != keyValue).ToList();
                    }
                    if(templist.Find(a=>a.UrlAddress==moduleEntity.UrlAddress)!=null)
                    throw new Exception("菜单地址不能重复！");
                }
                else
                {
                    moduleEntity.UrlAddress = null;
                }
                await _service.SubmitForm(moduleEntity, keyValue);
                return await Success("操作成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitUpdateForm(string Id, int SortCode)
        {
            try
            {
                await _service.SubmitUpdateForm(Id, SortCode);
                return await Success("操作成功。", "", Id);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", Id);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [HandlerAdmin(false)]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            try
            {
                await _service.DeleteForm(keyValue);
                return await Success("操作成功。", "", keyValue, DbLogType.Delete);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue, DbLogType.Delete);
            }
        }
    }
}
