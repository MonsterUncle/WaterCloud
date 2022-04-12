using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaterCloud.Code;
using SqlSugar;
using WaterCloud.Domain.FlowManage;
using WaterCloud.Domain.SystemManage;
using WaterCloud.Service.CommonService;
using WaterCloud.Domain.SystemOrganize;
using System.Net.Http;
using System.IO;
using System.Reflection;
using WaterCloud.Domain.InfoManage;
using WaterCloud.Service.InfoManage;
using WaterCloud.DataBase;
using Microsoft.Extensions.DependencyInjection;

namespace WaterCloud.Service.FlowManage
{
	/// <summary>
	/// 创 建：超级管理员
	/// 日 期：2020-07-14 09:18
	/// 描 述：我的流程服务类
	/// </summary>
	public class FlowinstanceService : DataFilterService<FlowinstanceEntity>, IDenpendency
    {
        public IHttpClientFactory _httpClientFactory { get; set; }
        public MessageService messageApp { get; set; }
        private string flowCreator;
        private string className { get; set; }  
        public FlowinstanceService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            className= System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3].Substring(0, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3].Length - 7);
        }
        #region 获取数据
        public async Task<List<FlowinstanceEntity>> GetList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_Code.Contains(keyword) || a.F_CustomName.Contains(keyword));
            }
            return await query.Where(a => a.F_EnabledMark == true).OrderBy(a => a.F_Id,OrderByType.Desc).ToListAsync();
        }

        public async Task<List<FlowInstanceOperationHistory>> QueryHistories(string keyValue)
        {
            return await repository.Db.Queryable<FlowInstanceOperationHistory>().Where(u => u.F_InstanceId == keyValue).OrderBy(u => u.F_CreatorTime).ToListAsync();
        }

        public async Task<List<FlowinstanceEntity>> GetLookList(string keyword = "")
        {
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_Code.Contains(keyword) || a.F_CustomName.Contains(keyword));
            }
            query = GetDataPrivilege("a", "", query);
            return await query.Where(a => a.F_EnabledMark == true).OrderBy(a => a.F_Id, OrderByType.Desc).ToListAsync();
        }

        public async Task<List<FlowinstanceEntity>> GetLookList(SoulPage<FlowinstanceEntity> pagination, string type = "", string keyword = "")
        {
            //反格式化显示只能用"等于"，其他不支持
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> enabledTemp = new Dictionary<string, string>();
            enabledTemp.Add("0", "正在运行");
            enabledTemp.Add("1", "审批通过");
            enabledTemp.Add("2", "被撤回");
            enabledTemp.Add("3", "不同意");
            enabledTemp.Add("4", "被驳回");
            dic.Add("F_IsFinish", enabledTemp);
            pagination = ChangeSoulData(dic, pagination);
            var query = repository.IQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                query = query.Where(a => a.F_Code.Contains(keyword) || a.F_CustomName.Contains(keyword));
            }
            var user = currentuser;
            if (type == "todo")   //待办事项
            {
                query = query.Where(a => ((a.F_MakerList == "1" || a.F_MakerList.Contains(user.UserId))) && (a.F_IsFinish == 0 || a.F_IsFinish == 4) && a.F_ActivityType < 3);
            }
            else if (type == "done")  //已办事项（即我参与过的流程）
            {
                var instances = repository.Db.Queryable<FlowInstanceOperationHistory>().Where(a => a.F_CreatorUserId == user.UserId)
                    .Select(a => a.F_InstanceId).Distinct().ToList();
                query = query.Where(a => instances.Contains(a.F_Id));
            }
            else  //我的流程
            {
                query = query.Where(a => a.F_CreatorUserId == user.UserId);
            }
            //权限过滤
            query = GetDataPrivilege("a","",query);
            return await query.Where(a => a.F_EnabledMark == true).ToPageListAsync(pagination);
        }

        public async Task<FlowinstanceEntity> GetForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return data;
        }
        #endregion

        public async Task<FlowinstanceEntity> GetLookForm(string keyValue)
        {
            var data = await repository.FindEntity(keyValue);
            return GetFieldsFilterData(data);
        }

        #region 获取各种节点的流程审核者
        /// <summary>
        /// 驳回
        /// 如果NodeRejectStep不为空，优先使用；否则按照NodeRejectType驳回
        /// </summary>
        /// <returns></returns>
        public async Task<bool> NodeReject(VerificationExtend reqest)
        {
            var user = currentuser;

            FlowinstanceEntity flowInstance = await GetForm(reqest.F_FlowInstanceId);
            flowCreator = flowInstance.F_CreatorUserId;

            FlowRuntime wfruntime = new FlowRuntime(flowInstance);

            string resnode = "";
            resnode = string.IsNullOrEmpty(reqest.NodeRejectStep) ? wfruntime.RejectNode(reqest.NodeRejectType) : reqest.NodeRejectStep;

            var tag = new Tag
            {
                Description = reqest.F_VerificationOpinion,
                Taged = (int)TagState.Reject,
                UserId = user.UserId,
                UserName = user.UserName
            };

            wfruntime.MakeTagNode(wfruntime.currentNodeId, tag);
            flowInstance.F_IsFinish = 4;//4表示驳回（需要申请者重新提交表单）
            unitofwork.CurrentBeginTrans();
            if (resnode != "")
            {
                wfruntime.RemoveNode(resnode);
                flowInstance.F_SchemeContent = wfruntime.ToSchemeObj().ToJson();
                flowInstance.F_ActivityId = resnode;
                var prruntime = new FlowRuntime(flowInstance);
                prruntime.MakeTagNode(prruntime.currentNodeId, tag);
                flowInstance.F_PreviousId = prruntime.previousId;
                flowInstance.F_ActivityType = prruntime.GetNodeType(resnode);
                flowInstance.F_ActivityName = prruntime.Nodes[resnode].name;
                if (resnode == wfruntime.startNodeId)
                {
                    flowInstance.F_MakerList = flowInstance.F_CreatorUserId;
                }
                else
                {
                    flowInstance.F_MakerList = await repository.Db.Queryable<FlowInstanceTransitionHistory>().Where(a => a.F_FromNodeId == resnode && a.F_ToNodeId == prruntime.nextNodeId).OrderBy(a => a.F_CreatorTime,OrderByType.Desc).Select(a => a.F_CreatorUserId).FirstAsync();//当前节点可执行的人信息
                }
                await AddRejectTransHistory(wfruntime, prruntime);
                await repository.Update(flowInstance);
            }
            await repository.Db.Insertable(new FlowInstanceOperationHistory
            {
                F_Id = Utils.GuId(),
                F_InstanceId = reqest.F_FlowInstanceId
                ,
                F_CreatorUserId = user.UserId
                ,
                F_CreatorUserName = user.UserName
                ,
                F_CreatorTime = DateTime.Now
                ,
                F_Content = "【"
                          + wfruntime.currentNode.name
                          + "】【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "】驳回,备注："
                          + reqest.F_VerificationOpinion
            }).ExecuteCommandAsync();
            MessageEntity msg = new MessageEntity();
            if (resnode == wfruntime.startNodeId)
            {
                msg.F_MessageInfo = flowInstance.F_CustomName + "--流程驳回";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress;
                msg.F_HrefTarget = module.F_Target;
                msg.F_ToUserId = flowInstance.F_CreatorUserId;
                msg.F_ToUserName = flowInstance.F_CreatorUserName;
                msg.F_ClickRead = true;
                msg.F_KeyValue = flowInstance.F_Id;
            }
            else
            {
                msg.F_MessageInfo = flowInstance.F_CustomName + "--流程待处理";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress.Remove(module.F_UrlAddress.Length - 5, 5) + "ToDoFlow";
                msg.F_HrefTarget = module.F_Target;
                msg.F_ToUserId = flowInstance.F_MakerList == "1" ? "" : flowInstance.F_MakerList;
                msg.F_ClickRead = false;
                msg.F_KeyValue = flowInstance.F_Id;
            }
            msg.F_CreatorUserName = currentuser.UserName;
            msg.F_EnabledMark = true;
            msg.F_MessageType = 2;
            var lastmsg = repository.Db.Queryable<MessageEntity>().Where(a => a.F_ClickRead == false && a.F_KeyValue == flowInstance.F_Id).OrderBy(a => a.F_CreatorTime,OrderByType.Desc).First();
            if (lastmsg != null && !await repository.Db.Queryable<MessageHistoryEntity>().Where(a => a.F_MessageId == lastmsg.F_Id).AnyAsync())
            {
                await messageApp.ReadMsgForm(lastmsg.F_Id);
            }
            await messageApp.SubmitForm(msg);
            unitofwork.CurrentCommit();

            wfruntime.NotifyThirdParty(_httpClientFactory.CreateClient(), tag);

            return true;
        }
        /// <summary>
        /// 节点审核
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<bool> NodeVerification(VerificationExtend request)
        {
            var user = currentuser;
            var instanceId = request.F_FlowInstanceId;

            var tag = new Tag
            {
                UserName = user.UserName,
                UserId = user.UserId,
                Description = request.F_VerificationOpinion,
                Taged = Int32.Parse(request.F_VerificationFinally)
            };
            FlowinstanceEntity flowInstance = await GetForm(instanceId);
            flowCreator = flowInstance.F_CreatorUserId;
            FlowInstanceOperationHistory flowInstanceOperationHistory = new FlowInstanceOperationHistory
            {
                F_Id = Utils.GuId(),
                F_InstanceId = instanceId,
                F_CreatorUserId = tag.UserId,
                F_CreatorUserName = tag.UserName,
                F_CreatorTime = DateTime.Now
            };//操作记录
            FlowRuntime wfruntime = new FlowRuntime(flowInstance);
            unitofwork.CurrentBeginTrans();
            #region 会签
            if (flowInstance.F_ActivityType == 0)//当前节点是会签节点
            {
                //会签时的【当前节点】一直是会签开始节点
                //TODO: 标记会签节点的状态，这个地方感觉怪怪的
                wfruntime.MakeTagNode(wfruntime.currentNodeId, tag);

                string canCheckId = ""; //寻找当前登录用户可审核的节点Id
                foreach (string fromForkStartNodeId in wfruntime.FromNodeLines[wfruntime.currentNodeId].Select(u => u.to))
                {
                    var fromForkStartNode = wfruntime.Nodes[fromForkStartNodeId];  //与会前开始节点直接连接的节点
                    canCheckId = GetOneForkLineCanCheckNodeId(fromForkStartNode, wfruntime, tag);
                    if (!string.IsNullOrEmpty(canCheckId)) break;
                }

                if (canCheckId == "")
                {
                    throw (new Exception("审核异常,找不到审核节点"));
                }

                flowInstanceOperationHistory.F_Content = "【" + wfruntime.Nodes[canCheckId].name
                                                           + "】【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                                                           + "】" + (tag.Taged == 1 ? "同意" : "不同意") + ",备注："
                                                           + tag.Description;

                wfruntime.MakeTagNode(canCheckId, tag); //标记审核节点状态
                string res = wfruntime.NodeConfluence(canCheckId, tag);
                if (res == TagState.No.ToString("D"))
                {
                    flowInstance.F_IsFinish = 3;
                }
                else if (!string.IsNullOrEmpty(res))
                {
                    flowInstance.F_PreviousId = flowInstance.F_ActivityId;
                    flowInstance.F_ActivityId = wfruntime.nextNodeId;
                    flowInstance.F_ActivityType = wfruntime.nextNodeType;
                    flowInstance.F_ActivityName = wfruntime.nextNode.name;
                    flowInstance.F_IsFinish = (wfruntime.nextNodeType == 4 ? 1 : 0);
                    flowInstance.F_MakerList = wfruntime.nextNodeType == 4 ? "" : GetNextMakers(wfruntime, request);
                    await AddTransHistory(wfruntime);
                }
                else
                {
                    //会签过程中，需要更新用户
                    flowInstance.F_MakerList = GetForkNodeMakers(wfruntime, wfruntime.currentNodeId);
                    await AddTransHistory(wfruntime);
                }

            }
            #endregion 会签

            #region 一般审核
            else
            {
                wfruntime.MakeTagNode(wfruntime.currentNodeId, tag);
                if (tag.Taged == (int)TagState.Ok)
                {
                    flowInstance.F_PreviousId = flowInstance.F_ActivityId;
                    flowInstance.F_ActivityId = wfruntime.nextNodeId;
                    flowInstance.F_ActivityType = wfruntime.nextNodeType;
                    flowInstance.F_ActivityName = wfruntime.nextNode.name;
                    flowInstance.F_MakerList = (wfruntime.GetNextNodeType() != 4 ? GetNextMakers(wfruntime, request) : "");
                    flowInstance.F_IsFinish = (wfruntime.nextNodeType == 4 ? 1 : 0);
                    await AddTransHistory(wfruntime);
                }
                else
                {
                    flowInstance.F_IsFinish = 3; //表示该节点不同意
                }
                flowInstanceOperationHistory.F_Content = "【" + wfruntime.currentNode.name
                                                           + "】【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                                                           + "】" + (tag.Taged == 1 ? "同意" : "不同意") + ",备注："
                                                           + tag.Description;
            }
            #endregion 一般审核

            wfruntime.RemoveNode(wfruntime.nextNodeId);
            flowInstance.F_SchemeContent = wfruntime.ToSchemeObj().ToJson();
            await repository.Db.Updateable(flowInstance).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            await repository.Db.Insertable(flowInstanceOperationHistory).ExecuteCommandAsync();
            MessageEntity msg = new MessageEntity();
            msg.F_CreatorUserName = currentuser.UserName;
            msg.F_EnabledMark = true;
            if (flowInstance.F_IsFinish == 1)
            {
                msg.F_MessageInfo = flowInstance.F_CustomName + "--流程已完成";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress;
                msg.F_HrefTarget = module.F_Target;
                msg.F_ToUserId = flowInstance.F_CreatorUserId;
                msg.F_ClickRead = true;
                msg.F_KeyValue = flowInstance.F_Id;
            }
            else if (flowInstance.F_IsFinish == 3)
            {
                msg.F_MessageInfo = flowInstance.F_CustomName + "--流程已终止";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress;
                msg.F_HrefTarget = module.F_Target;
                var makerList = repository.Db.Queryable<FlowInstanceOperationHistory>().Where(a => a.F_InstanceId == flowInstance.F_Id && a.F_CreatorUserId != currentuser.UserId).Select(a => a.F_CreatorUserId).Distinct().ToList();
                msg.F_ToUserId = flowInstance.F_CreatorUserId;
                msg.F_ClickRead = true;
                msg.F_KeyValue = flowInstance.F_Id;
            }
            else
            {
                msg.F_MessageInfo = flowInstance.F_CustomName + "--流程待处理";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress.Remove(module.F_UrlAddress.Length - 5, 5) + "ToDoFlow";
                msg.F_HrefTarget = module.F_Target;
                msg.F_ToUserId = flowInstance.F_MakerList == "1" ? "" : flowInstance.F_MakerList;
                msg.F_ClickRead = false;
                msg.F_KeyValue = flowInstance.F_Id;
            }
            msg.F_MessageType = 2;
            var lastmsg = repository.Db.Queryable<MessageEntity>().Where(a => a.F_ClickRead == false && a.F_KeyValue == flowInstance.F_Id).OrderBy(a => a.F_CreatorTime,OrderByType.Desc).First();
            if (lastmsg != null && !await repository.Db.Queryable<MessageHistoryEntity>().Where(a => a.F_MessageId == lastmsg.F_Id).AnyAsync())
            {
                await messageApp.ReadMsgForm(lastmsg.F_Id);
            }
            await messageApp.SubmitForm(msg);
            unitofwork.CurrentCommit();

            wfruntime.NotifyThirdParty(_httpClientFactory.CreateClient(), tag);
            return true;
        }
        //会签时，获取一条会签分支上面是否有用户可审核的节点
        private string GetOneForkLineCanCheckNodeId(FlowNode fromForkStartNode, FlowRuntime wfruntime, Tag tag)
        {
            string canCheckId = "";
            var node = fromForkStartNode;
            do  //沿一条分支线路执行，直到遇到会签结束节点
            {
                var makerList = GetNodeMarkers(node);

                if (node.setInfo.Taged == null && !string.IsNullOrEmpty(makerList) && makerList.Split(',').Any(one => tag.UserId == one))
                {
                    canCheckId = node.id;
                    break;
                }

                node = wfruntime.GetNextNode(node.id);
            } while (node.type != FlowNode.JOIN);

            return canCheckId;
        }
        /// <summary>
        /// 寻找下一步的执行人
        /// 一般用于本节点审核完成后，修改流程实例的当前执行人，可以做到通知等功能
        /// </summary>
        /// <returns></returns>
        private string GetNextMakers(FlowRuntime wfruntime, NodeDesignateEntity request = null)
        {
            string makerList = "";
            if (wfruntime.nextNodeId == "-1")
            {
                throw (new Exception("无法寻找到下一个节点"));
            }
            if (wfruntime.nextNodeType == 0)//如果是会签节点
            {
                makerList = GetForkNodeMakers(wfruntime, wfruntime.nextNodeId);
            }
            else if (wfruntime.nextNode.setInfo.NodeDesignate == Setinfo.RUNTIME_SPECIAL_ROLE)
            { //如果是运行时指定角色
                if (wfruntime.nextNode.setInfo.NodeDesignate != request.NodeDesignateType)
                {
                    throw new Exception("前端提交的节点权限类型异常，请检查流程");
                }
                var users = new List<string>();
				foreach (var item in request.NodeDesignates)
				{
                    var temps = repository.Db.Queryable<UserEntity>().Where(a => a.F_RoleId.Contains(item) && a.F_EnabledMark == true && a.F_DeleteMark == false).Select(a => a.F_Id).ToList();
					if (temps!=null&&temps.Count>0)
					{
                        users.AddRange(temps);
                    }
                }
                makerList = JsonHelper.ArrayToString(users.Distinct().ToList(), makerList);
            }
            else if (wfruntime.nextNode.setInfo.NodeDesignate == Setinfo.RUNTIME_SPECIAL_USER)
            {  //如果是运行时指定用户
                if (wfruntime.nextNode.setInfo.NodeDesignate != request.NodeDesignateType)
                {
                    throw new Exception("前端提交的节点权限类型异常，请检查流程");
                }
                makerList = JsonHelper.ArrayToString(request.NodeDesignates, makerList);
            }
            else
            {
                makerList = GetNodeMarkers(wfruntime.nextNode);
                if (string.IsNullOrEmpty(makerList))
                {
                    throw (new Exception("无法寻找到节点的审核者,请查看流程设计是否有问题!"));
                }
            }

            return makerList;
        }

        /// <summary>
        /// 获取会签开始节点的所有可执行者
        /// </summary>
        /// <param name="forkNodeId">会签开始节点</param>
        /// <returns></returns>
        private string GetForkNodeMakers(FlowRuntime wfruntime, string forkNodeId)
        {
            string makerList = "";
            foreach (string fromForkStartNodeId in wfruntime.FromNodeLines[forkNodeId].Select(u => u.to))
            {
                var fromForkStartNode = wfruntime.Nodes[fromForkStartNodeId]; //与会前开始节点直接连接的节点
                if (makerList != "")
                {
                    makerList += ",";
                }

                makerList += GetOneForkLineMakers(fromForkStartNode, wfruntime);
            }

            return makerList;
        }

        //获取会签一条线上的审核者,该审核者应该是已审核过的节点的下一个人
        private string GetOneForkLineMakers(FlowNode fromForkStartNode, FlowRuntime wfruntime)
        {
            string markers = "";
            var node = fromForkStartNode;
            do  //沿一条分支线路执行，直到遇到第一个没有审核的节点
            {
                if (node.setInfo != null && node.setInfo.Taged != null)
                {
                    if (node.type != FlowNode.FORK && node.setInfo.Taged != (int)TagState.Ok)  //如果节点是不同意或驳回，则不用再找了
                    {
                        break;
                    }
                    node = wfruntime.GetNextNode(node.id);  //下一个节点
                    continue;
                }
                var marker = GetNodeMarkers(node);
                if (marker == "")
                {
                    throw (new Exception($"节点{node.name}没有审核者,请检查!"));
                }
                if (marker == "1")
                {
                    throw (new Exception($"节点{node.name}是会签节点，不能用所有人,请检查!"));
                }

                if (markers != "")
                {
                    markers += ",";
                }
                markers += marker;
                break;
            } while (node.type != FlowNode.JOIN);

            return markers;
        }

        /// <summary>
        /// 寻找该节点执行人
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetNodeMarkers(FlowNode node)
        {
            string makerList = "";

            if (node.setInfo != null)
            {
                if (node.setInfo.NodeDesignate == Setinfo.ALL_USER)//所有成员
                {
                    makerList = "1";
                }
                else if (node.setInfo.NodeDesignate == Setinfo.SPECIAL_USER)//指定成员
                {
                    makerList = JsonHelper.ArrayToString(node.setInfo.NodeDesignateData.users, makerList);
                }
                else if (node.setInfo.NodeDesignate == Setinfo.SPECIAL_ROLE)  //指定角色
                {
                    List<UserEntity> list = new List<UserEntity>();
                    List<string> users = new List<string>();
                    foreach (var item in node.setInfo.NodeDesignateData.roles)
                    {
                        var temp = repository.Db.Queryable<UserEntity>().Where(a => a.F_RoleId.Contains(item)).ToList();
                        var tempList = new List<UserEntity>();
                        if (node.setInfo.NodeDesignateData.currentDepart)
                        {
                            var currentDepartment = repository.Db.Queryable<UserEntity>().InSingle(flowCreator).F_DepartmentId.Split(',').ToList();
                            foreach (var user in temp)
                            {
                                var nextCurrentDepartment = user.F_DepartmentId.Split(',').ToList();
                                if (TextHelper.IsArrayIntersection(currentDepartment, nextCurrentDepartment))
                                {
                                    tempList.Add(user);
                                }
                            }
                        }
                        else
                        {
                            tempList = temp;
                        }
                        var tempFinal = tempList.Select(a => a.F_Id).ToList();
                        users.AddRange(tempFinal);
                    }
                    users = users.Distinct().ToList();
                    makerList = JsonHelper.ArrayToString(users, makerList);
                }
                else if (node.setInfo.NodeDesignate == Setinfo.RUNTIME_SPECIAL_ROLE || node.setInfo.NodeDesignate == Setinfo.RUNTIME_SPECIAL_USER)
                {
                    //如果是运行时选定的用户，则暂不处理。由上个节点审批时选定
                }
            }
            else  //如果没有设置节点信息，默认所有人都可以审核
            {
                makerList = "1";
            }
            return makerList;
        }

        /// <summary>
        /// 判定节点需要选择执行人或执行角色
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        private void CheckNodeDesignate(NodeDesignateEntity request)
        {
            if ((request.NodeDesignateType == Setinfo.RUNTIME_SPECIAL_ROLE
                 || request.NodeDesignateType == Setinfo.RUNTIME_SPECIAL_USER) && request.NodeDesignates.Length == 0)
            {
                throw new Exception("下个节点需要选择执行人或执行角色");
            }
        }
        /// <summary>
        /// 返回用于处理流程节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FlowinstanceEntity> GetForVerification(string id)
        {
            var flowinstance = await GetForm(id);
            var runtime = new FlowRuntime(flowinstance);
            if (runtime.nextNodeType != -1 && runtime.nextNode != null && runtime.nextNode.setInfo != null && runtime.nextNodeType != 4)
            {
                flowinstance.NextNodeDesignateType = runtime.nextNode.setInfo.NodeDesignate;
				if (flowinstance.NextNodeDesignateType==Setinfo.SPECIAL_USER)
				{
                    flowinstance.NextNodeDesignates = runtime.nextNode.setInfo.NodeDesignateData.users;
                    flowinstance.NextMakerName = string.Join(',', repository.Db.Queryable<UserEntity>().Where(a => flowinstance.NextNodeDesignates.Contains(a.F_Id)).Select(a => a.F_RealName).ToList());
                }
                else if (flowinstance.NextNodeDesignateType == Setinfo.SPECIAL_ROLE)
                {
                    flowinstance.NextNodeDesignates = runtime.nextNode.setInfo.NodeDesignateData.roles;
                    List<UserEntity> list = new List<UserEntity>();
                    List<string> users = new List<string>();
                    foreach (var item in flowinstance.NextNodeDesignates)
                    {
                        var temp = repository.Db.Queryable<UserEntity>().Where(a => a.F_RoleId.Contains(item)).ToList();
                        var tempList = new List<UserEntity>();
                        if (runtime.nextNode.setInfo.NodeDesignateData.currentDepart)
                        {
                            var currentDepartment = repository.Db.Queryable<UserEntity>().InSingle(flowCreator).F_DepartmentId.Split(',').ToList();
                            foreach (var user in temp)
                            {
                                var nextCurrentDepartment = user.F_DepartmentId.Split(',').ToList();
                                if (TextHelper.IsArrayIntersection(currentDepartment, nextCurrentDepartment))
                                {
                                    tempList.Add(user);
                                }
                            }
                        }
                        else
                        {
                            tempList = temp;
                        }
                        var tempFinal = tempList.Select(a => a.F_Id).ToList();
                        users.AddRange(tempFinal);
                    }
                    users = users.Distinct().ToList();
                    flowinstance.NextMakerName = string.Join(',', repository.Db.Queryable<UserEntity>().Where(a => users.Contains(a.F_Id)).Select(a => a.F_RealName).ToList());
                }
            }
            if (runtime.currentNode != null && runtime.currentNode.setInfo != null && runtime.currentNodeType != 4)
            {
                flowinstance.CurrentNodeDesignateType = runtime.currentNode.setInfo.NodeDesignate;
				if (flowinstance.F_MakerList!="1" && !string.IsNullOrEmpty(flowinstance.F_MakerList))
				{
                    var temps = flowinstance.F_MakerList.Split(',');
                    flowinstance.CurrentMakerName = string.Join(',', repository.Db.Queryable<UserEntity>().Where(a => temps.Contains(a.F_Id)).Select(a => a.F_RealName).ToList());
                }
				else
				{
                    flowinstance.CurrentMakerName = "所有人";
                }
            }
            return flowinstance;
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 添加扭转记录
        /// </summary>
        private async Task AddTransHistory(FlowRuntime wfruntime)
        {
            var tag = currentuser;
            await repository.Db.Insertable(new FlowInstanceTransitionHistory
            {
                F_Id = Utils.GuId(),
                F_InstanceId = wfruntime.flowInstanceId,
                F_CreatorUserId = tag.UserId,
                F_CreatorTime = DateTime.Now,
                F_CreatorUserName = tag.UserName,
                F_FromNodeId = wfruntime.currentNodeId,
                F_FromNodeName = wfruntime.currentNode.name,
                F_FromNodeType = wfruntime.currentNodeType,
                F_ToNodeId = wfruntime.nextNodeId,
                F_ToNodeName = wfruntime.nextNode.name,
                F_ToNodeType = wfruntime.nextNodeType,
                F_IsFinish = wfruntime.nextNodeType == 4 ? true : false,
                F_TransitionSate = false
            }).ExecuteCommandAsync();
        }
        /// <summary>
        /// 添加扭转记录
        /// </summary>
        private async Task AddRejectTransHistory(FlowRuntime wfruntime, FlowRuntime prruntime)
        {
            var tag = currentuser;
            await repository.Db.Insertable(new FlowInstanceTransitionHistory
            {
                F_Id = Utils.GuId(),
                F_InstanceId = wfruntime.flowInstanceId,
                F_CreatorUserId = tag.UserId,
                F_CreatorTime = DateTime.Now,
                F_CreatorUserName = tag.UserName,
                F_FromNodeId = wfruntime.currentNodeId,
                F_FromNodeName = wfruntime.currentNode.name,
                F_FromNodeType = wfruntime.currentNodeType,
                F_ToNodeId = prruntime.currentNodeId,
                F_ToNodeName = prruntime.currentNode.name,
                F_ToNodeType = prruntime.currentNodeType,
                F_IsFinish = false,
                F_TransitionSate = false
            }).ExecuteCommandAsync();
        }
        public async Task Verification(VerificationExtend entity)
        {
            bool isReject = TagState.Reject.Equals((TagState)Int32.Parse(entity.F_VerificationFinally));
            if (isReject)  //驳回
            {
                await NodeReject(entity);
            }
            else
            {
                await NodeVerification(entity);
            }
        }
        public async Task CreateInstance(FlowinstanceEntity entity)
        {
            var nodeDesignate = new NodeDesignateEntity();
            nodeDesignate.NodeDesignates = entity.NextNodeDesignates;
            nodeDesignate.NodeDesignateType = entity.NextNodeDesignateType;
            CheckNodeDesignate(nodeDesignate);
            entity.F_EnabledMark = true;
            FlowschemeEntity scheme = null;
            if (!string.IsNullOrEmpty(entity.F_SchemeId))
            {
                scheme = await repository.Db.Queryable<FlowschemeEntity>().InSingleAsync(entity.F_SchemeId);
            }
            if (scheme == null)
            {
                throw new Exception("该流程模板已不存在，请重新设计流程");
            }
            entity.F_SchemeContent = scheme.F_SchemeContent;
            var form = await repository.Db.Queryable<FormEntity>().InSingleAsync(scheme.F_FrmId);
            if (form == null)
            {
                throw new Exception("该流程模板对应的表单已不存在，请重新设计流程");
            }

            entity.F_FrmContentData = form.F_ContentData;
            entity.F_FrmContent = form.F_Content;
            entity.F_FrmContentParse = form.F_ContentParse;
            entity.F_FrmType = form.F_FrmType;
            entity.F_FrmId = form.F_Id;
            Dictionary<string, string> dic = JsonHelper.ToObject<Dictionary<string, string>>(entity.F_FrmData);
            if (!dic.ContainsKey("申请人"))
            {
                dic.Add("申请人", currentuser.UserId);

            }
            if (!dic.ContainsKey("所属部门"))
            {
                dic.Add("所属部门", currentuser.DepartmentId);
            }
            entity.F_FrmData = dic.ToJson();
            entity.F_InstanceSchemeId = "";
            entity.F_DbName = form.F_WebId;
            entity.F_FlowLevel = 0;
            entity.Create();
            flowCreator = currentuser.UserId;
            //创建运行实例
            var wfruntime = new FlowRuntime(entity);
            var user = currentuser;

            #region 根据运行实例改变当前节点状态
            entity.F_ActivityId = wfruntime.nextNodeId;
            entity.F_ActivityType = wfruntime.GetNextNodeType();
            entity.F_ActivityName = wfruntime.nextNode.name;
            entity.F_PreviousId = wfruntime.currentNodeId;
            entity.F_CreatorUserName = user.UserName;
            entity.F_MakerList = (wfruntime.GetNextNodeType() != 4 ? GetNextMakers(wfruntime, nodeDesignate) : "");
            entity.F_IsFinish = (wfruntime.GetNextNodeType() == 4 ? 1 : 0);
            unitofwork.CurrentBeginTrans();
            await repository.Db.Insertable(entity).ExecuteCommandAsync();

            wfruntime.flowInstanceId = entity.F_Id;
            //复杂表单提交
            if (entity.F_FrmType == 1)
            {
                var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
                var t = referencedAssemblies
                    .SelectMany(a => a.GetTypes().Where(t => t.FullName.Contains("WaterCloud.Service.") && t.FullName.Contains("." + entity.F_DbName + "Service"))).First();
                ICustomerForm icf = (ICustomerForm) GlobalContext.ScopeServiceProvider.GetRequiredService(t);
                await icf.Add(entity.F_Id, entity.F_FrmData);
            }

            #endregion

            #region 流程操作记录

            FlowInstanceOperationHistory processOperationHistoryEntity = new FlowInstanceOperationHistory
            {
                F_Id = Utils.GuId(),
                F_InstanceId = entity.F_Id,
                F_CreatorUserId = entity.F_CreatorUserId,
                F_CreatorUserName = entity.F_CreatorUserName,
                F_CreatorTime = entity.F_CreatorTime,
                F_Content = "【创建】"
                          + entity.F_CreatorUserName
                          + "创建了一个流程【"
                          + entity.F_Code + "/"
                          + entity.F_CustomName + "】"
            };
            await repository.Db.Insertable(processOperationHistoryEntity).ExecuteCommandAsync();

            #endregion 流程操作记录

            await AddTransHistory(wfruntime);

            MessageEntity msg = new MessageEntity();
            msg.F_CreatorUserName = currentuser.UserName;
            msg.F_EnabledMark = true;
            if (entity.F_IsFinish == 1)
            {
                msg.F_MessageInfo = entity.F_CustomName + "--流程已完成";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress;
                msg.F_HrefTarget = module.F_Target;
                msg.F_ClickRead = true;
                msg.F_KeyValue = entity.F_Id;
            }
            else if (entity.F_IsFinish == 3)
            {
                msg.F_MessageInfo = entity.F_CustomName + "--流程已终止";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress;
                msg.F_HrefTarget = module.F_Target;
                var makerList = repository.Db.Queryable<FlowInstanceOperationHistory>().Where(a => a.F_InstanceId == entity.F_Id && a.F_CreatorUserId != currentuser.UserId).Select(a => a.F_CreatorUserId).Distinct().ToList();
                msg.F_ToUserId = entity.F_CreatorUserId;
                msg.F_ClickRead = true;
                msg.F_KeyValue = entity.F_Id;
            }
            else
            {
                msg.F_MessageInfo = entity.F_CustomName + "--流程待处理";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress.Remove(module.F_UrlAddress.Length - 5, 5) + "ToDoFlow";
                msg.F_HrefTarget = module.F_Target;
                msg.F_ClickRead = false;
                msg.F_KeyValue = entity.F_Id;
            }
            msg.F_MessageType = 2;
            msg.F_ToUserId = entity.F_MakerList == "1" ? "" : entity.F_MakerList;
            var lastmsg = repository.Db.Queryable<MessageEntity>().Where(a => a.F_ClickRead == false && a.F_KeyValue == entity.F_Id).OrderBy(a => a.F_CreatorTime,OrderByType.Desc).First();
            if (lastmsg != null && !await repository.Db.Queryable<MessageHistoryEntity>().Where(a => a.F_MessageId == lastmsg.F_Id).AnyAsync())
            {
                await messageApp.ReadMsgForm(lastmsg.F_Id);
            }
            await messageApp.SubmitForm(msg);
            unitofwork.CurrentCommit();
        }
        public async Task UpdateInstance(FlowinstanceEntity entity)
        {
            var nodeDesignate = new NodeDesignateEntity();
            nodeDesignate.NodeDesignates = entity.NextNodeDesignates;
            nodeDesignate.NodeDesignateType = entity.NextNodeDesignateType;
            CheckNodeDesignate(nodeDesignate);
            FlowschemeEntity scheme = null;
            if (!string.IsNullOrEmpty(entity.F_SchemeId))
            {
                scheme = await repository.Db.Queryable<FlowschemeEntity>().InSingleAsync(entity.F_SchemeId);
            }
            if (scheme == null)
            {
                throw new Exception("该流程模板已不存在，请重新设计流程");
            }
            entity.F_SchemeContent = scheme.F_SchemeContent;
            var form = await repository.Db.Queryable<FormEntity>().InSingleAsync(scheme.F_FrmId);
            if (form == null)
            {
                throw new Exception("该流程模板对应的表单已不存在，请重新设计流程");
            }
            Dictionary<string, string> dic = JsonHelper.ToObject<Dictionary<string, string>>(entity.F_FrmData);
            if (!dic.ContainsKey("申请人"))
            {
                dic.Add("申请人", currentuser.UserId);

            }
            if (!dic.ContainsKey("所属部门"))
            {
                dic.Add("所属部门", currentuser.DepartmentId);
            }
            entity.F_FrmData = dic.ToJson();
            var wfruntime = new FlowRuntime(await repository.FindEntity(entity.F_Id));
            entity.F_FrmContentData = form.F_ContentData;
            entity.F_FrmContentParse = form.F_ContentParse;
            entity.F_FrmType = form.F_FrmType;
            entity.F_FrmId = form.F_Id;
            entity.F_InstanceSchemeId = "";
            entity.F_DbName = form.F_WebId;
            entity.F_FlowLevel = 0;
            flowCreator = currentuser.UserId;
            //创建运行实例
            wfruntime = new FlowRuntime(entity);

            var user = currentuser;

            #region 根据运行实例改变当前节点状态
            entity.F_ActivityId = wfruntime.nextNodeId;
            entity.F_ActivityType = wfruntime.GetNextNodeType();
            entity.F_ActivityName = wfruntime.nextNode.name;
            entity.F_PreviousId = wfruntime.currentNodeId;
            entity.F_CreatorUserName = user.UserName;
            entity.F_MakerList = (wfruntime.GetNextNodeType() != 4 ? GetNextMakers(wfruntime, nodeDesignate) : "");
            entity.F_IsFinish = (wfruntime.GetNextNodeType() == 4 ? 1 : 0);
            unitofwork.CurrentBeginTrans();
            await repository.Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            wfruntime.flowInstanceId = entity.F_Id;
            //复杂表单提交
            if (entity.F_FrmType == 1)
            {
                var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
                var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
                var t = referencedAssemblies
                    .SelectMany(a => a.GetTypes().Where(t => t.FullName.Contains("WaterCloud.Service.") && t.FullName.Contains("." + entity.F_DbName + "Service"))).First();
                ICustomerForm icf = (ICustomerForm)GlobalContext.ScopeServiceProvider.GetRequiredService(t);
                await icf.Edit(entity.F_Id, entity.F_FrmData);
            }
            #endregion

            #region 流程操作记录
            FlowInstanceOperationHistory processOperationHistoryEntity = new FlowInstanceOperationHistory
            {
                F_Id = Utils.GuId(),
                F_InstanceId = entity.F_Id,
                F_CreatorUserId = user.UserId,
                F_CreatorUserName = entity.F_CreatorUserName,
                F_CreatorTime = DateTime.Now,
                F_Content = "【修改】"
                          + entity.F_CreatorUserName
                          + "修改了一个流程【"
                          + entity.F_Code + "/"
                          + entity.F_CustomName + "】"
            };
            await repository.Db.Insertable(processOperationHistoryEntity).ExecuteCommandAsync();
            #endregion

            await AddTransHistory(wfruntime);

            MessageEntity msg = new MessageEntity();
            msg.F_CreatorUserName = currentuser.UserName;
            msg.F_EnabledMark = true;
            if (entity.F_IsFinish == 1)
            {
                msg.F_MessageInfo = entity.F_CustomName + "--流程已完成";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress;
                msg.F_HrefTarget = module.F_Target;
                msg.F_ClickRead = true;
                msg.F_KeyValue = entity.F_Id;
            }
            else if (entity.F_IsFinish == 3)
            {
                msg.F_MessageInfo = entity.F_CustomName + "--流程已终止";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress;
                msg.F_HrefTarget = module.F_Target;
                var makerList = repository.Db.Queryable<FlowInstanceOperationHistory>().Where(a => a.F_InstanceId == entity.F_Id && a.F_CreatorUserId != currentuser.UserId).Select(a => a.F_CreatorUserId).Distinct().ToList();
                msg.F_ToUserId = entity.F_CreatorUserId;
                msg.F_ClickRead = true;
                msg.F_KeyValue = entity.F_Id;
            }
            else
            {
                msg.F_MessageInfo = entity.F_CustomName + "--流程待处理";
                var module = repository.Db.Queryable<ModuleEntity>().First(a => a.F_EnCode == className);
                msg.F_Href = module.F_UrlAddress.Remove(module.F_UrlAddress.Length - 5, 5) + "ToDoFlow";
                msg.F_HrefTarget = module.F_Target;
                msg.F_ClickRead = false;
                msg.F_KeyValue = entity.F_Id;
            }
            msg.F_MessageType = 2;
            msg.F_ToUserId = entity.F_MakerList == "1" ? "" : entity.F_MakerList;
            var lastmsg = repository.Db.Queryable<MessageEntity>().Where(a => a.F_ClickRead == false && a.F_KeyValue == entity.F_Id).OrderBy(a => a.F_CreatorTime,OrderByType.Desc).First();
            if (lastmsg != null && !await repository.Db.Queryable<MessageHistoryEntity>().Where(a => a.F_MessageId == lastmsg.F_Id).AnyAsync())
            {
                await messageApp.ReadMsgForm(lastmsg.F_Id);
            }
            await messageApp.SubmitForm(msg);
            unitofwork.CurrentCommit();
            msg.F_ClickRead = false;
            msg.F_KeyValue = entity.F_Id;
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Update(a => ids.Contains(a.F_Id), a => new FlowinstanceEntity
            {
                F_EnabledMark = false
            });
        }
        public async Task CancleForm(string keyValue)
        {
            var user = currentuser;

            FlowinstanceEntity flowInstance = await GetForm(keyValue);
            flowCreator = flowInstance.F_CreatorUserId;

            FlowRuntime wfruntime = new FlowRuntime(flowInstance);

            string resnode = "";
            resnode =  wfruntime.RejectNode("1");

            var tag = new Tag
            {
                Description = "流程撤回",
                Taged = (int)TagState.Reject,
                UserId = user.UserId,
                UserName = user.UserName
            };

            wfruntime.MakeTagNode(wfruntime.currentNodeId, tag);
            flowInstance.F_IsFinish = 2;//2表示撤回（需要申请者重新提交表单）
            unitofwork.CurrentBeginTrans();
            if (resnode != "")
            {
                wfruntime.RemoveNode(resnode);
                flowInstance.F_SchemeContent = wfruntime.ToSchemeObj().ToJson();
                flowInstance.F_ActivityId = resnode;
                var prruntime = new FlowRuntime(flowInstance);
                prruntime.MakeTagNode(prruntime.currentNodeId, tag);
                flowInstance.F_PreviousId = prruntime.previousId;
                flowInstance.F_ActivityType = prruntime.GetNodeType(resnode);
                flowInstance.F_ActivityName = prruntime.Nodes[resnode].name;
                if (resnode == wfruntime.startNodeId)
                {
                    flowInstance.F_MakerList = flowInstance.F_CreatorUserId;
                }
                else
                {
                    flowInstance.F_MakerList = await repository.Db.Queryable<FlowInstanceTransitionHistory>().Where(a => a.F_FromNodeId == resnode && a.F_ToNodeId == prruntime.nextNodeId).OrderBy(a => a.F_CreatorTime, OrderByType.Desc).Select(a => a.F_CreatorUserId).FirstAsync();//当前节点可执行的人信息
                }
                await AddRejectTransHistory(wfruntime, prruntime);
                await repository.Update(flowInstance);
            }
            await repository.Db.Insertable(new FlowInstanceOperationHistory
            {
                F_Id = Utils.GuId(),
                F_InstanceId = keyValue
                ,
                F_CreatorUserId = user.UserId
                ,
                F_CreatorUserName = user.UserName
                ,
                F_CreatorTime = DateTime.Now
                ,
                F_Content = "【"
                          + wfruntime.currentNode.name
                          + "】【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "】撤回,备注：流程撤回"
            }).ExecuteCommandAsync();
            unitofwork.CurrentCommit();
            wfruntime.NotifyThirdParty(_httpClientFactory.CreateClient(), tag);
        }
        #endregion

    }
}
