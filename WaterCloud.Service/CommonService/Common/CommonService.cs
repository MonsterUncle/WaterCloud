using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterCloud.Code;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Service.SystemManage;
namespace WaterCloud.Service.CommonService
{
    public class CommonService
    {

        /// <summary>
        /// 根据用户角色获取用户
        /// </summary>
        /// <param name="rolescode"></param>
        /// <returns></returns>
        public static List<UserEntity> GetUsers(string rolescode)
        {
            if (string.IsNullOrEmpty(rolescode))
            {
                List<string> list = new List<string>(rolescode.Split(','));
                return (new UserService()).GetUserList(list, "Roles");
            }
            return null;
        }

      
        /// <summary>
        /// 根据UserId获取用户姓名
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public  static string GetUserName(string usersid)
        {
            StringBuilder username = new StringBuilder();
            if (!string.IsNullOrEmpty(usersid))
            {
                List<string> users = new List<string>();
                users = new List<System.String>(usersid.Split(','));

                List<UserEntity> userlist = (new UserService()).GetUserList("");
                foreach (string uid in users)
                {
                    if (!string.IsNullOrEmpty(uid))
                    {
                        UserEntity user = userlist.Find(p => p.F_Id == uid);
                        if (user != null)
                        {
                            username.Append(user.F_RealName + ",");
                        }
                    }
                }
                return username.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据UserId获取用户姓名
        /// </summary>
        /// <param name="userlist"></param>
        /// <param name="usersid"></param>
        /// <returns></returns>
        public static string GetUserName(List<UserEntity> userlist,string usersid)
        {
            StringBuilder username = new StringBuilder();
            if (!string.IsNullOrEmpty(usersid))
            {
                List<string> users = new List<string>();
                users = new List<System.String>(usersid.Split(','));
                foreach (string uid in users)
                {
                    if (!string.IsNullOrEmpty(uid))
                    {
                        UserEntity user = userlist.Find(p => p.F_Id == uid);
                        if (user != null)
                        {
                            username.Append(user.F_RealName + ",");
                        }
                    }
                }
                return username.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据机构ID或其部门ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetDepartName(string departmentid)
        {
            StringBuilder departname = new StringBuilder();
            if (!string.IsNullOrEmpty(departmentid))
            {
                List<string> liststr = new List<string>();
                liststr = new List<System.String>(departmentid.Split(','));

                List<OrganizeEntity> list = (new OrganizeService()).GetList();
                foreach (string departid in liststr)
                {
                    if (!string.IsNullOrEmpty(departid))
                    {
                        OrganizeEntity user = list.Find(p => p.F_Id == departid);
                        if (user != null)
                        {
                            departname.Append(user.F_FullName + ",");
                        }
                    }
                }
                return departname.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据UserId获取用户姓名
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetDingTalkUserId(string userid)
        {

            if (!string.IsNullOrEmpty(userid))
            {
                UserEntity user = (new UserService()).GetForm(userid);
                if (user != null)
                {
                    return user.F_DingTalkUserId;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据UserId获取用户姓名
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<string> GetDingTalkUserIdList(string usersid)
        {
            List<string> dUseridList = new List<string>();
            if (!string.IsNullOrEmpty(usersid))
            {
                List<string> users = new List<string>();
                users = new List<System.String>(usersid.Split(','));

                List <UserEntity> userlist = (new UserService()).GetUserList("");
                foreach(string uid in users)
                {
                    if (!string.IsNullOrEmpty(uid))
                    {
                        UserEntity user = userlist.Find(p => p.F_Id == uid);
                        if (user!=null && !string.IsNullOrEmpty(user.F_DingTalkUserId))
                        {
                            dUseridList.Add(user.F_DingTalkUserId);
                        }
                    }
                }
                return dUseridList.Distinct().ToList(); ;
            }
            return dUseridList;
        }

        /// <summary>
        /// 根据UserId获取用户钉钉ID
        /// </summary>
        /// <param name="usersids"></param>
        /// <returns></returns>
        public static List<string> GetDingTalkUserIdList(List<string> usersids)
        {
            List<string> dUseridList = new List<string>();
            if (usersids.Count > 0)
            {
                List<string> users = new List<string>();
                List<UserEntity> userlist = (new UserService()).GetUserList("");
                foreach (string uid in usersids)
                {
                    if (!string.IsNullOrEmpty(uid))
                    {
                        List<string> strlist = new List<string>();
                        strlist = new List<System.String>(uid.Split(','));
                        foreach (string child in strlist)
                        {
                            UserEntity user = userlist.Find(p => p.F_Id == child);
                            if (user != null && !string.IsNullOrEmpty(user.F_DingTalkUserId))
                            {
                                dUseridList.Add(user.F_DingTalkUserId);
                            }
                        }
                       
                    }
                }
                
                return dUseridList.Distinct().ToList();
            }
            return dUseridList;
        }

        /// <summary>
        /// 根据用户钉钉ID获取UserId
        /// </summary>
        /// <param name="usersids"></param>
        /// <returns></returns>
        public static List<string> GetUserIdListByDingtalkId(List<string> usersids)
        {
            List<string> dUseridList = new List<string>();
            if (usersids.Count > 0)
            {
                List<string> users = new List<string>();
                List<UserEntity> userlist = (new UserService()).GetUserList("");
                foreach (string uid in usersids)
                {
                    if (!string.IsNullOrEmpty(uid))
                    {
                        UserEntity user = userlist.Find(p => p.F_DingTalkUserId == uid);
                        if (user != null && !string.IsNullOrEmpty(user.F_Id))
                        {
                            dUseridList.Add(user.F_Id);
                        }
                    }
                }
                return dUseridList.Distinct().ToList(); ;
            }
            return dUseridList;
        }
        /// <summary>
        /// 根据用户名获取用户ID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string GetUserIdByUserName(string username)
        {
            List<string> useridList = new List<string>();
            if (!string.IsNullOrEmpty(username))
            {
                List<UserEntity> userlist = (new UserService()).GetUserList("");
                if (userlist.Count > 0)
                {
                    UserEntity user = userlist.Find(p => p.F_RealName.Contains(username) || p.F_NickName.Contains(username));
                    if (user != null)
                    {
                        return user.F_Id;
                    }
                }
            }
            return string.Empty;
        }

       
        /// <summary>
        ///  判断是否拥有权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckAuthority(OperatorModel user)
        {
            if (user != null)
            {
                if (user.IsSystem || user.IsBoss || user.IsSenior || user.IsAdmin)
                {
                    return true;
                }else
                {
                    return false;
                }
            }else
            {
                return false;
            }
        }

        /// <summary>
        ///  判断是否拥有客户数据权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckIsCustomerAuthority(OperatorModel user)
        {
            if (user != null)
            {
                if (user.IsSystem || user.IsBoss || user.IsSenior || user.IsAdmin || user.IsSaleman)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
