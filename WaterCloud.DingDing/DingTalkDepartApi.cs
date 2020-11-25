using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 对钉钉部门的业务操作
    /// 宜宾机电一体化研究所：吴远德
    /// 相关API开发文档见：https://open-doc.dingtalk.com/microapp/serverapi2/dubakq
    /// 需要结合钉钉API接口.Net版本动态库文件TopSdk，引用 DingTalk.Api
    /// 创建时间：2018-12-15
    /// </summary>
    public class DingTalkDepartApi
    {
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="parentId">父级部门ID</param>
        /// <returns></returns>
        public static List<DepartmentInfo> GetDepartmentList(string accessToken, string parentId = "1")
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
                OapiDepartmentListRequest request = new OapiDepartmentListRequest();
                request.Id = parentId;
                request.SetHttpMethod("GET");

                OapiDepartmentListResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        List<DepartmentInfo> departmentinfolist = new List<DepartmentInfo>();
                        foreach (OapiDepartmentListResponse.DepartmentDomain info in response.Department)
                        {
                            DepartmentInfo depart = new DepartmentInfo();
                            depart.Id = info.Id.ToString();
                            depart.Name = info.Name;
                            depart.Parentid = info.Parentid.ToString();
                            depart.CreateDeptGroup = info.CreateDeptGroup;
                            depart.AutoAddUser = info.AutoAddUser;
                            departmentinfolist.Add(depart);
                        }
                        return departmentinfolist;
                    }
                    else
                    {
                        throw new Exception(response.ErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        /// <summary>
        ///  获取部门ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="parentId">父级部门ID</param>
        /// <returns></returns>
        public static List<long> GetDepartmentId(string accessToken, string parentId = "1")
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list_ids");
                OapiDepartmentListIdsRequest request = new OapiDepartmentListIdsRequest();
                request.Id = parentId;
                request.SetHttpMethod("GET");

                OapiDepartmentListIdsResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        List<DepartmentInfo> departmentinfolist = new List<DepartmentInfo>();
                        return response.SubDeptIdList;
                    }
                    else
                    {
                        throw new Exception(response.ErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="idStr">部门ID</param>
        /// <returns></returns>
        public static DepartmentInfo GetDepartmentInfo(string accessToken, string departmentId)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/get");
                OapiDepartmentGetRequest request = new OapiDepartmentGetRequest();
                request.Id = departmentId;
                request.SetHttpMethod("GET");

                OapiDepartmentGetResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        DepartmentInfo depart = new DepartmentInfo();
                        depart.Id = response.Id.ToString();
                        depart.Name = response.Name;
                        depart.Parentid = response.Parentid.ToString();
                        depart.CreateDeptGroup = response.CreateDeptGroup;
                        depart.AutoAddUser = response.AutoAddUser;
                        return depart;
                    }
                    else
                    {
                        throw new Exception(response.ErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="department">部门信息</param>
        /// <returns></returns>
        public static bool CreateDepartment(string accessToken, DepartmentInfo department)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/create");
                OapiDepartmentCreateRequest request = new OapiDepartmentCreateRequest();
                request.Parentid= department.Parentid;
                request.CreateDeptGroup= department.CreateDeptGroup;
                request.Order= department.Order;
                request.Name= department.Name;
                request.OuterDept = department.OuterDept;
                request.OuterPermitDepts = department.OuterPermitDepts;
                request.OuterPermitUsers = department.OuterPermitUsers;
                OapiDepartmentCreateResponse response = client.Execute(request, accessToken);

                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(response.ErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="department">部门信息</param>
        /// <returns></returns>
        public static bool UpdateDepartment(string accessToken, DepartmentInfo department)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/update");
                OapiDepartmentUpdateRequest request = new OapiDepartmentUpdateRequest();
                request.Parentid = department.Parentid;
                request.CreateDeptGroup = department.CreateDeptGroup;
                request.Order = department.Order;
                request.Name = department.Name;
                request.OuterDept = department.OuterDept;
                request.OuterPermitDepts = department.OuterPermitDepts;
                request.OuterPermitUsers = department.OuterPermitUsers;
                OapiDepartmentUpdateResponse response = client.Execute(request, accessToken);

                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(response.ErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        public static bool DeleteDepartment(string accessToken, string departmentId)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/delete");
                OapiDepartmentDeleteRequest request = new OapiDepartmentDeleteRequest();
                request.Id= departmentId;
                OapiDepartmentDeleteResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(response.ErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

    }
}
