/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemOrganize;
using WaterCloud.Code;
using WaterCloud.Domain.SystemOrganize;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WaterCloud.Service;
using System;
using System.Threading.Tasks;

namespace WaterCloud.Web.Areas.SystemOrganize.Controllers
{
    [Area("SystemOrganize")]
    public class OrganizeController : BaseController
    {

        public OrganizeService _service { get; set; }

        [HttpGet]
        public virtual ActionResult AddForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data =await _service.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (OrganizeEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.FullName;
                treeModel.parentId = item.ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSelectJson()
        {
            var data = await _service.GetList();
            data = data.Where(a => a.EnabledMark == true).ToList();
            var treeList = new List<TreeSelectModel>();
            foreach (OrganizeEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.FullName;
                treeModel.parentId = item.ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeJson()
        {
            var data =await _service.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (OrganizeEntity item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword,string ids)
        {
            var data =await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.FullName.Contains(keyword));
            }
			if (!string.IsNullOrEmpty(ids))
			{
                var str = ids.Split(',');
				foreach (var item in str)
				{
                    if (data.Where(a => a.Id == item).Any())
                    {
                        var temp = data.Find(a => a.Id == item);
                        temp.LAY_CHECKED = true;
                    }
				}
			}
            return Success(data.Count, data);
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
        public async Task<ActionResult> SubmitForm(OrganizeEntity organizeEntity, string keyValue)
        {
            try
            {
                if (organizeEntity.ParentId=="0")
                {
                    organizeEntity.Layers = 1;
                }
                else
                {
                    organizeEntity.Layers =(await _service.GetForm(organizeEntity.ParentId)).Layers + 1;
                }
                await _service.SubmitForm(organizeEntity, keyValue);
                return await Success("操作成功。", "", keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, "", keyValue);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
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
