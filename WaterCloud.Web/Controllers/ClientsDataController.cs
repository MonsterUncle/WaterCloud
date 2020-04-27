/*******************************************************************************
 * Copyright © 2020 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/
using WaterCloud.Service.SystemManage;
using WaterCloud.Code;
using WaterCloud.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using WaterCloud.Entity;
using WaterCloud.Service.SystemSecurity;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WaterCloud.Web.Controllers
{
    [HandlerLogin]
    public class ClientsDataController : Controller
    {
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private string cacheKey = "watercloud_quickmoduledata_";
        private string initcacheKey = "watercloud_init_";
        private string cacheKeyOperator = "watercloud_operator_";// +登录者token
        private readonly QuickModuleService _quickModuleService;
        private readonly NoticeService _noticeService;
        private readonly UserService _userService;
        private readonly ModuleService _moduleService;
        private readonly LogService _logService;
        private readonly RoleAuthorizeService _roleAuthorizeService;
        private readonly ItemsDetailService _itemsDetailService;
        private readonly ItemsService _itemsService;
        private readonly OrganizeService _organizeService;
        private readonly RoleService _roleService;
        private readonly DutyService _dutyService;

        public ClientsDataController(QuickModuleService quickModuleService, NoticeService noticeService, UserService userService, ModuleService moduleService, LogService logService, RoleAuthorizeService roleAuthorizeService, ItemsDetailService itemsDetailService, ItemsService itemsService, OrganizeService organizeService, RoleService roleService, DutyService dutyService)
        {
            _quickModuleService = quickModuleService;
            _noticeService = noticeService;
            _userService = userService;
            _moduleService = moduleService;
            _logService = logService;
            _roleAuthorizeService = roleAuthorizeService;
            _itemsDetailService = itemsDetailService;
            _itemsService = itemsService;
            _organizeService = organizeService;
            _roleService = roleService;
            _dutyService = dutyService;
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetClientsDataJson()
        {
            var data = new
            {
                dataItems = this.GetDataItemList(),
                organize = this.GetOrganizeList(),
                role = this.GetRoleList(),
                duty = this.GetDutyList(),
                user = this.GetUserList(),
                authorizeButton = this.GetMenuButtonListNew(),
            };
            return Content(data.ToJson());
        }

        private object GetQuickModuleList()
        {
            if (OperatorProvider.Provider.GetCurrent()==null)
            {
                return null;
            }
            var userId = OperatorProvider.Provider.GetCurrent().UserId;
            var data = RedisHelper.Get<Dictionary<string,List<QuickModuleExtend>>>(cacheKey + "list");
            if (data==null)
            {
                data = new Dictionary<string, List<QuickModuleExtend>>();
                data.Add(userId, _quickModuleService.GetQuickModuleList(userId));
            }
            else
            {
                if (data.ContainsKey(userId))
                {
                    data[userId] = _quickModuleService.GetQuickModuleList(userId);
                }
                else
                {
                    data.Add(userId, _quickModuleService.GetQuickModuleList(userId));
                }
            }
            RedisHelper.Del(cacheKey + "list");
            RedisHelper.Set(cacheKey + "list", data);
            return data[userId];
        }

        private object GetNoticeList()
        {
            var data = _noticeService.GetList("").Where(a=>a.F_EnabledMark==true).OrderByDescending(a=>a.F_CreatorTime).Take(6).ToList();
            return data;
        }

        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
            if (roleId==null&& OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                roleId = "admin";
            }
            Dictionary<string, string > data = RedisHelper.Get<Dictionary<string, string>>(initcacheKey + "list");
            if (data == null)
            {
                data =new Dictionary <string, string>();
                data .Add(roleId, this.GetMenuListNew());

            }
            else
            {
                if (data.ContainsKey(roleId))
                {
                    data[roleId] = this.GetMenuListNew();
                }
                else
                {
                    data.Add(roleId, this.GetMenuListNew());
                }
            }
            RedisHelper.Del(initcacheKey + "list");
            RedisHelper.Set(initcacheKey + "list",data);
            return Content(data[roleId]);
        }
        [HttpGet]
        public ActionResult GetNoticeInfo()
        {
            var data = this.GetNoticeList();
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUserCode()
        {
            var data = new UserService().GetForm(OperatorProvider.Provider.GetCurrent().UserId);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetQuickModule()
        {
            try
            {
                var data = this.GetQuickModuleList();
                return Content(data.ToJson());
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult GetCoutData()
        {
            int usercout = _userService.GetUserList("").Count();
            int logincout = RedisHelper.Get<OperatorUserInfo>(cacheKeyOperator + "info_" + OperatorProvider.Provider.GetCurrent().UserId).F_LogOnCount??0;
            int modulecout = _moduleService.GetList().Where(a => a.F_EnabledMark == true && a.F_UrlAddress != null).Count();
            int logcout = _logService.GetList().Count();
            var data= new { usercout = usercout, logincout = logincout, modulecout = modulecout, logcout = logcout };
            return Content(data.ToJson());
        }
        private string GetMenuListNew()
        {
            var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
            StringBuilder sbJson = new StringBuilder();
            InitEntity init = new InitEntity();
            init.homeInfo = new HomeInfoEntity();
            init.logoInfo = new LogoInfoEntity();
            init.menuInfo = new List<MenuInfoEntity>();
            MenuInfoEntity munu = new MenuInfoEntity();
            init.menuInfo.Add(munu);
            munu.title = "常规管理";
            munu.icon = "fa fa-address-book";
            munu.href = "";
            munu.target = "_self";
            munu.child = new List<MenuInfoEntity>();
            munu.child = ToMenuJsonNew(_roleAuthorizeService.GetMenuList(roleId), "0");
            CreateMunu(init.menuInfo);


            sbJson.Append(init.ToJson());
            return sbJson.ToString() ;
        }
        /// <summary>
        /// 组件管理
        /// </summary>
        /// <param name="menuInfo"></param>
        private void CreateMunu(List<MenuInfoEntity> menuInfo)
        {

            MenuInfoEntity modelmunu = new MenuInfoEntity();
            modelmunu.title = "组件管理";
            modelmunu.icon = "fa fa-lemon-o";
            modelmunu.href = "";
            modelmunu.target = "_self";
            modelmunu.child = new List<MenuInfoEntity>();
            MenuInfoEntity child1 = new MenuInfoEntity();
            child1.title = "图标列表";
            child1.href = "../page/icon.html";
            child1.icon = "fa fa-dot-circle-o";
            child1.target = "_self";
            modelmunu.child.Add(child1);
            MenuInfoEntity child2 = new MenuInfoEntity();
            child2.title = "图标选择";
            child2.href = "../page/icon-picker.html";
            child2.icon = "fa fa-adn";
            child2.target = "_self";
            modelmunu.child.Add(child2);
            MenuInfoEntity child3 = new MenuInfoEntity();
            child3.title = "颜色选择";
            child3.href = "../page/color-select.html";
            child3.icon = "fa fa-dashboard";
            child3.target = "_self";
            modelmunu.child.Add(child3);
            MenuInfoEntity child4 = new MenuInfoEntity();
            child4.title = "下拉选择";
            child4.href = "../page/table-select.html";
            child4.icon = "fa fa-angle-double-down";
            child4.target = "_self";
            modelmunu.child.Add(child4);
            MenuInfoEntity child5 = new MenuInfoEntity();
            child5.title = "文件上传";
            child5.href = "../page/upload.html";
            child5.icon = "fa fa-arrow-up";
            child5.target = "_self";
            modelmunu.child.Add(child5);
            MenuInfoEntity child6 = new MenuInfoEntity();
            child6.title = "富文本编辑器";
            child6.href = "../page/editor.html";
            child6.icon = "fa fa-edit";
            child6.target = "_self";
            modelmunu.child.Add(child6);
            MenuInfoEntity child7 = new MenuInfoEntity();
            child7.title = "省市县区选择器";
            child7.href = "../page/area.html";
            child7.icon = "fa fa-rocket";
            child7.target = "_self";
            modelmunu.child.Add(child7);
            menuInfo.Add(modelmunu);
        }

        private List<MenuInfoEntity> ToMenuJsonNew(List<ModuleEntity> data, string parentId)
        {
            List<MenuInfoEntity> list = new List<MenuInfoEntity>();
            List<ModuleEntity> entitys = data.FindAll(t => t.F_ParentId == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    MenuInfoEntity munu = new MenuInfoEntity();
                    munu.title = item.F_FullName;
                    munu.icon = item.F_Icon;
                    munu.href = item.F_UrlAddress;
                    munu.target = "_self";
                    if (data.FindAll(t => t.F_ParentId == item.F_Id).Count>0)
                    {
                        munu.child = new List<MenuInfoEntity>();
                        munu.child = ToMenuJsonNew(data, item.F_Id);
                    }
                    if (item.F_Layers == 1)
                    {
                        list.Add(munu);
                    }
                    if (item.F_Layers>1&& item.F_IsMenu ==true)
                    {
                        list.Add(munu);
                    }

                };
            }
            return list;
        }

        private object GetDataItemList()
        {
            var itemdata = _itemsDetailService.GetList();
            Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
            foreach (var item in _itemsService.GetList())
            {
                var dataItemList = itemdata.FindAll(t => t.F_ItemId.Equals(item.F_Id));
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (var itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.F_ItemCode, itemList.F_ItemName);
                }
                dictionaryItem.Add(item.F_EnCode, dictionaryItemList);
            }

            return dictionaryItem;
        }
        private object GetOrganizeList()
        {
            var data = _organizeService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (OrganizeEntity item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetRoleList()
        {
            var data = _roleService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleEntity item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetDutyList()
        {
            var data = _dutyService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleEntity item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetUserList()
        {
            var data = _userService.GetUserList("");
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (UserEntity item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_Account,
                    fullname = item.F_RealName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetMenuButtonListNew()
        {
            var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
            var data = _roleAuthorizeService.GetButtonList(roleId);
            if (roleId==null&& OperatorProvider.Provider.GetCurrent().IsSystem)
            {
                roleId = "admin";
            }
            var dataModuleId = data.Distinct(new ExtList<ModuleButtonEntity>("F_ModuleId"));
            Dictionary<string, Dictionary<string, List<ModuleButtonEntity>>> dictionary = RedisHelper.Get<Dictionary<string, Dictionary<string, List<ModuleButtonEntity>>>>(initcacheKey+ "modulebutton_list");
            var dictionarytemp = new Dictionary<string, List<ModuleButtonEntity>>();
            foreach (ModuleButtonEntity item in dataModuleId)
            {
                var buttonList = data.Where(t => t.F_ModuleId.Equals(item.F_ModuleId)).ToList();
                dictionarytemp.Add(item.F_ModuleId, buttonList);
            }
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, Dictionary<string, List<ModuleButtonEntity>>>();
                dictionary.Add(roleId, dictionarytemp);
            }
            else
            {
                if (dictionary.ContainsKey(roleId))
                {
                    dictionary[roleId] = dictionarytemp;
                }
                else
                {
                    dictionary.Add(roleId, dictionarytemp);
                }
            }
            RedisHelper.Del(initcacheKey + "modulebutton_list");
            RedisHelper.Set(initcacheKey + "modulebutton_list", dictionary);
            return dictionary[roleId];
        }
    }
}
