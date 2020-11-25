using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Entities.Menu;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.Exceptions;
using Senparc.CO2NET.HttpUtility;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP.CommonAPIs;

namespace WaterCloud.WeixinMP
{
    public class MenuManage
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public WxJsonResult CreateMenu(string accessToken, ButtonGroup buttonData)
        {
            var result = CommonApi.CreateMenu(accessToken, buttonData);
             return result;
        }

        /// <summary>
        /// 获取当前菜单，如果菜单不存在，将返回null
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public GetMenuResult GetMenu(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", accessToken);

            var jsonString = Senparc.CO2NET.HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            //var finalResult = GetMenuFromJson(jsonString);

            GetMenuResult finalResult;
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                var jsonResult = js.Deserialize<GetMenuResultFull>(jsonString);
                if (jsonResult.menu == null || jsonResult.menu.button.Count == 0)
                {
                    throw new WeixinException(jsonResult.errmsg);
                }

                finalResult = GetMenuFromJsonResult(jsonResult);
            }
            catch (WeixinException ex)
            {
                finalResult = null;
                throw new Exception("未获得到菜单：" + ex.Message);
            }

            return finalResult;
        }


        /// <summary>
        /// 根据微信返回的Json数据得到可用的GetMenuResult结果
        /// </summary>
        /// <param name="resultFull"></param>
        /// <returns></returns>
        public GetMenuResult GetMenuFromJsonResult(GetMenuResultFull resultFull)
        {
            GetMenuResult result = null;
            try
            {
                //重新整理按钮信息
                ButtonGroup bg = new ButtonGroup();
                foreach (var rootButton in resultFull.menu.button)
                {
                    if (rootButton.name == null)
                    {
                        continue;//没有设置一级菜单
                    }
                    var availableSubButton = rootButton.sub_button.Count(z => !string.IsNullOrEmpty(z.name));//可用二级菜单按钮数量
                    if (availableSubButton == 0)
                    {
                        //底部单击按钮
                        if (rootButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                            && string.IsNullOrEmpty(rootButton.key))
                        {
                            throw new WeixinMenuException("单击按钮的key不能为空！");
                        }

                        if (rootButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                        {
                            //点击
                            bg.button.Add(new SingleClickButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else
                        {
                            //URL
                            bg.button.Add(new SingleViewButton()
                            {
                                name = rootButton.name,
                                url = rootButton.url,
                                type = rootButton.type
                            });
                        }

                    }
                    //else if (availableSubButton < 1)
                    //{
                    //    throw new WeixinMenuException("子菜单至少需要填写1个！");
                    //}
                    else
                    {
                        //底部二级菜单
                        var subButton = new SubButton(rootButton.name);
                        bg.button.Add(subButton);

                        foreach (var subSubButton in rootButton.sub_button)
                        {
                            if (subSubButton.name == null)
                            {
                                continue; //没有设置菜单
                            }

                            if (subSubButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                                && string.IsNullOrEmpty(subSubButton.key))
                            {
                                throw new WeixinMenuException("单击按钮的key不能为空！");
                            }


                            if (subSubButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                            {
                                //点击
                                subButton.sub_button.Add(new SingleClickButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else
                            {
                                //URL
                                subButton.sub_button.Add(new SingleViewButton()
                                {
                                    name = subSubButton.name,
                                    url = subSubButton.url,
                                    type = subSubButton.type
                                });
                            }
                        }
                    }
                }

                if (bg.button.Count < 1)
                {
                    throw new WeixinMenuException("一级菜单按钮至少为1个！");
                }

                result = new GetMenuResult(bg)
                {
                    menu = bg
                };
            }
            catch (Exception ex)
            {
                throw new WeixinMenuException(ex.Message, ex);
            }
            return result;
        }



        /// <summary>
        /// 获取当前菜单 json
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public string GetMenu2(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", accessToken);
            var jsonString = RequestUtility.HttpGet(url, Encoding.UTF8);
            return jsonString;
        }


        // 从一个对象信息生成Json串
        public string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

    }
}
