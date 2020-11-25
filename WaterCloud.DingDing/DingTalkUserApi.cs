using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterCloud.Code;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace WaterCloud.DingTalk
{
    /// <summary>
    /// 对钉钉用户（员工）的业务操作
    /// 宜宾机电一体化研究所：吴远德
    /// 相关API开发文档见：https://open-doc.dingtalk.com/microapp/serverapi2/ege851
    /// 需要结合钉钉API接口.Net版本动态库文件TopSdk，引用 DingTalk.Api
    /// 创建时间：2018-12-15
    /// </summary>
    public class DingTalkUserApi
    {
        /// <summary>
        /// 获取部门用户ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<string> GetUserId(string accessToken, string deptId)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getDeptMember");
                OapiUserGetDeptMemberRequest req = new OapiUserGetDeptMemberRequest();

                req.DeptId = deptId;
                req.SetHttpMethod("GET");
                OapiUserGetDeptMemberResponse rsp = client.Execute(req, accessToken);

                if (rsp != null && rsp.Errcode == 0)
                {
                    if (rsp.UserIds[0].Length > 0)
                    {
                        return rsp.UserIds;
                    }
                    else
                    {
                        throw new Exception(rsp.Errmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// 根据unionid获取userid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string getUseridByUnionid(string accessToken, string unionid)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getUseridByUnionid");
                OapiUserGetUseridByUnionidRequest req = new OapiUserGetUseridByUnionidRequest();

                req.Unionid = unionid;
                req.SetHttpMethod("GET");
                OapiUserGetUseridByUnionidResponse rsp = client.Execute(req, accessToken);

                if (rsp != null && rsp.Errcode == 0)
                {
                    return rsp.Userid;
                }
                else
                {
                    throw new Exception(rsp.Errmsg);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过CODE换取用户身份UserId
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public static string getUseridByAuthCode(string accessToken, string authCode)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getuserinfo");
                OapiUserGetuserinfoRequest request = new OapiUserGetuserinfoRequest();
                request.Code=authCode;
                request.SetHttpMethod("GET");
                OapiUserGetuserinfoResponse response = client.Execute(request, accessToken);
                
                if (response != null && response.Errcode == 0)
                {
                    return response.Userid;
                }
                else
                {
                    throw new Exception(response.Errmsg);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据UserId获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserDetailInfo GetUserDetail(string accessToken, string userId)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/get");
                OapiUserGetRequest request = new OapiUserGetRequest();
                request.Userid = userId;
                request.SetHttpMethod("GET");
                OapiUserGetResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    UserDetailInfo userinfo = new UserDetailInfo();
                    if (response.Errcode == 0)
                    {
                        userinfo = new UserDetailInfo();
                        userinfo.UserId = response.Userid;
                        userinfo.OpenId = response.OpenId == null?"": response.OpenId;
                        userinfo.Unionid = response.Unionid == null ? "" : response.Unionid;
                        userinfo.UserName = response.Name == null ? "" : response.Name;
                        userinfo.Mobile = response.Mobile == null ? "" : response.Mobile;
                        userinfo.Email = response.Email == null ? "" : response.Email;
                        userinfo.Active = response.Active;
                        userinfo.OrderInDepts = response.OrderInDepts == null ? "" : response.OrderInDepts;
                        userinfo.IsAdmin = response.IsAdmin;
                        userinfo.IsBoss = response.IsBoss;
                        userinfo.IsLeaderInDepts = response.IsLeaderInDepts == null ? "" : response.IsLeaderInDepts;
                        userinfo.IsHide = response.IsHide;
                        userinfo.Department = response.Department;
                        userinfo.Position = response.Position == null ? "" : response.Position;
                        userinfo.Avatar = response.Avatar == null ? "" : response.Avatar;
                        userinfo.HiredDate = response.HiredDate == null ? "" : response.HiredDate;
                        userinfo.JobNumber = response.Jobnumber == null ? "" : response.Jobnumber;
                        userinfo.IsSenior = response.IsSenior;
                        //userinfo.Roles = response.Roles;
                        userinfo.WorkPlace = response.WorkPlace == null ? "" : response.WorkPlace;
                        return userinfo;
                    }
                    else
                    {
                        throw new Exception(response.Errmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        /// <summary>
        /// 通过CODE换取用户详细信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public static UserDetailInfo GetUserDetailByAuthCode(string accessToken, string authCode)
        {
            try
            {
                string userId = getUseridByAuthCode(accessToken, authCode);
                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/get");
                OapiUserGetRequest request = new OapiUserGetRequest();
                request.Userid = userId;
                request.SetHttpMethod("GET");
                OapiUserGetResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    UserDetailInfo userinfo = new UserDetailInfo();
                    if (response.Errcode == 0)
                    {
                        userinfo = new UserDetailInfo();
                        userinfo.UserId = response.Userid;
                        userinfo.OpenId = response.OpenId == null ? "" : response.OpenId;
                        userinfo.Unionid = response.Unionid == null ? "" : response.Unionid;
                        userinfo.UserName = response.Name == null ? "" : response.Name;
                        userinfo.Mobile = response.Mobile == null ? "" : response.Mobile;
                        userinfo.Email = response.Email == null ? "" : response.Email;
                        userinfo.Active = response.Active;
                        userinfo.OrderInDepts = response.OrderInDepts == null ? "" : response.OrderInDepts;
                        userinfo.IsAdmin = response.IsAdmin;
                        userinfo.IsBoss = response.IsBoss;
                        userinfo.IsLeaderInDepts = response.IsLeaderInDepts == null ? "" : response.IsLeaderInDepts;
                        userinfo.IsHide = response.IsHide;
                        userinfo.Department = response.Department;
                        userinfo.Position = response.Position == null ? "" : response.Position;
                        userinfo.Avatar = response.Avatar == null ? "" : response.Avatar;
                        userinfo.HiredDate = response.HiredDate == null ? "" : response.HiredDate;
                        userinfo.JobNumber = response.Jobnumber == null ? "" : response.Jobnumber;
                        userinfo.IsSenior = response.IsSenior;
                        //userinfo.Roles = response.Roles;
                        userinfo.WorkPlace = response.WorkPlace == null ? "" : response.WorkPlace;
                        return userinfo;
                    }
                    else
                    {
                        throw new Exception(response.Errmsg);
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
        /// 获取部门用户userid列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<string> GetDepartmentUserId(string accessToken, string departmentId)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getDeptMember");
                OapiUserGetDeptMemberRequest req = new OapiUserGetDeptMemberRequest();
                req.DeptId= departmentId;
                req.SetHttpMethod("GET");
                OapiUserGetDeptMemberResponse response = client.Execute(req, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        List<string> userlist = new List<string>();
                        if (response.UserIds != null && response.UserIds.Count > 0)
                        {
                            foreach (string userid in response.UserIds)
                            {
                                userlist.Add(userid);
                            }
                        }
                        return userlist;
                    }
                    else
                    {
                        throw new Exception(response.Errmsg);
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
        /// 获取部门用户列表,仅返回UserId和UserName
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<UserDetailInfo> GetDepartmentUserList(string accessToken, int departmentId = 1)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/simplelist");
                OapiUserSimplelistRequest request = new OapiUserSimplelistRequest();
                request.DepartmentId = departmentId;
                request.Offset = 0L;
                request.Size = 100L;
                request.SetHttpMethod("GET");

                OapiUserSimplelistResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        List<UserDetailInfo> userlist = new List<UserDetailInfo>();
                        if (response.Userlist!=null && response.Userlist.Count>0)
                        {
                            foreach (OapiUserSimplelistResponse.UserlistDomain info in response.Userlist)
                            {
                                UserDetailInfo user = new UserDetailInfo();
                                user.UserId = info.Userid;
                                user.UserName = info.Name;
                                userlist.Add(user);
                            }
                        }
                        return userlist;
                    }
                    else
                    {
                        throw new Exception(response.Errmsg);
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
        /// 获取某部门下的用户详情
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="departmentId">获取的部门id，1表示根部门</param>
        /// <param name="size">此参数代表分页大小，最大100</param>
        /// <param name="offset">此参数代表偏移量</param>
        /// <returns></returns>
        public static List<UserDetailInfo> GetUserInfoByPage(string accessToken, long departmentId = 1,long size = 100, long offset = 0)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/listbypage");
                OapiUserListbypageRequest request = new OapiUserListbypageRequest();
                request.DepartmentId = departmentId;
                request.Offset = offset;
                request.Size = size;
                request.Order = "entry_desc";
                request.SetHttpMethod("GET");

                OapiUserListbypageResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        List<UserDetailInfo> userlist = new List<UserDetailInfo>();
                        if(response.Userlist != null && response.Userlist.Count > 0)
                        {
                            foreach (OapiUserListbypageResponse.UserlistDomain info in response.Userlist)
                            {
                                UserDetailInfo user = new UserDetailInfo();
                                user.UserId = info.Userid;
                                user.UserName = info.Name;
                                user.Unionid = info.Unionid;
                                user.IsAdmin = info.IsAdmin;
                                user.IsBoss = info.IsBoss;
                                user.Active = info.Active;
                                //user.Department = info.Department;
                                user.Email = info.Email;
                                user.HiredDate = info.HiredDate;
                                user.IsHide = info.IsHide;
                                user.IsLeader = info.IsLeader;
                                user.JobNumber = info.Jobnumber;
                                user.Mobile = info.Mobile;
                                user.Order = info.Order;
                                userlist.Add(user);
                            }
                        }
                        return userlist;
                    }
                    else
                    {
                        throw new Exception(response.Errmsg);
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
        /// 获取管理员列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="parentId"></param>
        /// <returns>《userid,level=1主管，=2子管》</userid></returns>
        public static Dictionary<string, long> GetAdminList(string accessToken, string parentId = "1")
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/get_admin");
                OapiUserGetAdminRequest request = new OapiUserGetAdminRequest();
                request.SetHttpMethod("GET");

                OapiUserGetAdminResponse response = client.Execute(request, accessToken);
                if (response != null)
                {
                    if (response.Errcode == 0)
                    {
                        List<DepartmentInfo> departmentinfolist = new List<DepartmentInfo>();
                        Dictionary<string, long> list = new Dictionary<string, long>();
                        foreach (OapiUserGetAdminResponse.AdminListDomain info in response.AdminList)
                        {
                            list.Add(info.Userid, info.SysLevel);
                        }
                        return list;
                    }
                    else
                    {
                        throw new Exception(response.Errmsg);
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
        /// 根据授权Code及appId获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserDetailInfo GetUserDetail(string code, string appId,string appSecret)
        {
            try
            {

                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/sns/getuserinfo_bycode");
                OapiSnsGetuserinfoBycodeRequest req = new OapiSnsGetuserinfoBycodeRequest();
                req.TmpAuthCode= code;
                OapiSnsGetuserinfoBycodeResponse response = client.Execute(req, appId, appSecret);
                if (response != null)
                {
                    UserDetailInfo userinfo = new UserDetailInfo();
                    if (response.Errcode == 0)
                    {
                        userinfo = new UserDetailInfo();
                        userinfo.UserId = response.UserInfo.Nick;
                        userinfo.OpenId = response.UserInfo.Openid;
                        userinfo.Unionid = response.UserInfo.Unionid;
                        return userinfo;
                    }
                    else
                    {
                        throw new Exception(response.Errmsg);
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
        /// 创建用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool CreateUser(string accessToken, UserDetailInfo user)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/create");
                OapiUserCreateRequest request = new OapiUserCreateRequest();
                request.Userid = user.UserId;
                request.Mobile = user.Mobile;
                request.Name = user.UserName;
                request.Mobile = user.Mobile == null ? "" : user.Mobile;
                request.Email = user.Email == null ? "" : user.Email;
                request.OrderInDepts = user.OrderInDepts == null ? "" : user.OrderInDepts;
                request.IsHide = user.IsHide;
                request.Position = user.Position == null ? "" : user.Position;
                request.HiredDate = Convert.ToInt64(user.HiredDate);
                request.Jobnumber = user.JobNumber == null ? "" : user.JobNumber;
                request.IsSenior = user.IsSenior;
                request.WorkPlace = user.WorkPlace == null ? "" : user.WorkPlace;

                // 需要用字符串， "[100,200]" 这种格式
                request.Department = (user.Department.ToJson());

                OapiUserCreateResponse response = client.Execute(request, accessToken);
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
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UpdateUser(string accessToken, UserDetailInfo user)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/update");
                OapiUserUpdateRequest request = new OapiUserUpdateRequest();
                request.Userid = user.UserId;
                request.Mobile = user.Mobile;
                request.Name = user.UserName;
                request.Mobile = user.Mobile == null ? "" : user.Mobile;
                request.Email = user.Email == null ? "" : user.Email;
                request.OrderInDepts = user.OrderInDepts == null ? "" : user.OrderInDepts;
                request.IsHide = user.IsHide;
                request.Position = user.Position == null ? "" : user.Position;
                request.HiredDate = Convert.ToInt64(user.HiredDate);
                request.Jobnumber = user.JobNumber == null ? "" : user.JobNumber;
                request.IsSenior = user.IsSenior;
                request.WorkPlace = user.WorkPlace == null ? "" : user.WorkPlace;
                request.Department = user.Department;

                OapiUserUpdateResponse response = client.Execute(request, accessToken);
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
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        public static bool DeleteUser(string accessToken, string userId)
        {
            try
            {
                IDingTalkClient client = new  DefaultDingTalkClient("https://oapi.dingtalk.com/user/delete");
                OapiUserDeleteRequest request = new OapiUserDeleteRequest();
                request.Userid = userId;
                OapiUserDeleteResponse response = client.Execute(request, accessToken);
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
