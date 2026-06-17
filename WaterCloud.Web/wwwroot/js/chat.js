"use strict";

// 用原生WebSocket替代SignalR
var chatSocket = null;
var chatReconnectTimer = null;

function connectChat() {
    var protocol = location.protocol === 'https:' ? 'wss:' : 'ws:';
    var wsUrl = protocol + '//' + location.host + '/ws/chat';

    if ('WebSocket' in window) {
        chatSocket = new WebSocket(wsUrl);
    } else {
        console.error('浏览器不支持WebSocket，无法启动聊天功能');
        return;
    }

    chatSocket.onopen = function () {
        console.log("聊天WebSocket连接成功");
        sendChatMessage({ type: "Login", token: "" });
        if (chatReconnectTimer) {
            clearTimeout(chatReconnectTimer);
            chatReconnectTimer = null;
        }
    };

    chatSocket.onmessage = function (e) {
        if (e.data === "pong" || e.data === "Close!") return;
        try {
            var data = JSON.parse(e.data);
            handleReceiveMessage(data);
        } catch (err) {
            console.error("聊天消息解析失败：" + err);
        }
    };

    chatSocket.onerror = function () {
        console.log("聊天WebSocket连接失败");
    };

    chatSocket.onclose = function () {
        console.log("聊天WebSocket断开，3秒后重连...");
        chatReconnectTimer = setTimeout(connectChat, 3000);
    };
}

function sendChatMessage(msg) {
    try {
        if (chatSocket && chatSocket.readyState === WebSocket.OPEN) {
            chatSocket.send(JSON.stringify(msg));
        }
    } catch (e) {
        console.error("发送聊天消息失败:", e);
    }
}

// 消息接收处理（保持和原SignalR版一致的UI逻辑）
function handleReceiveMessage(data) {
    layui.use(['notice', 'common'], function () {
        var notice = layui.notice;
        var common = layui.common;
        if (data.F_MessageType == 0) {
            notice.options = {
                positionClass: "toast-bottom-right",
                onclick: function () {
                    common.ajax({
                        url: "/InfoManage/Message/ReadMsgForm",
                        data: { keyValue: data.F_Id },
                        type: 'POST',
                        success: function () {
                            var title = '通知---' + data.F_CreatorUserName,
                                noticeTime = data.F_CreatorTime,
                                content = data.F_MessageInfo;
                            var html = '<div style="padding:15px 20px; text-align:justify; line-height: 22px;border-bottom:1px solid #e2e2e2;background-color: #2f4056;color: #ffffff">\n' +
                                '<div style="text-align: center;margin-bottom: 20px;font-weight: bold;border-bottom:1px solid #718fb5;padding-bottom: 5px"><h4 class="text-danger">' + title + '</h4></div>\n' +
                                '<div style="font-size: 12px">' + content + '</div>\n' +
                                '</div>\n';
                            parent.layer.open({
                                type: 1,
                                title: '通知' + '<span style="float: right;right: 1px;font-size: 12px;color: #b1b3b9;margin-top: 1px">' + noticeTime + '</span>',
                                area: '150px;',
                                shade: 0.8,
                                id: 'layuimini-notice',
                                btn: ['确定'],
                                btnAlign: 'c',
                                moveType: 1,
                                content: html
                            });
                        }
                    });
                }
            };
            notice.success(data.F_MessageInfo);
        }
        else if (data.F_MessageType == 1) {
            notice.options = {
                positionClass: "toast-bottom-right",
                onclick: function () {
                    common.ajax({
                        url: "/InfoManage/Message/ReadMsgForm",
                        data: { keyValue: data.F_Id },
                        type: 'POST',
                        success: function () {
                            var title = '私信---' + data.F_CreatorUserName,
                                noticeTime = data.F_CreatorTime,
                                content = data.F_MessageInfo;
                            var html = '<div style="padding:15px 20px; text-align:justify; line-height: 22px;border-bottom:1px solid #e2e2e2;background-color: #2f4056;color: #ffffff">\n' +
                                '<div style="text-align: center;margin-bottom: 20px;font-weight: bold;border-bottom:1px solid #718fb5;padding-bottom: 5px"><h4 class="text-danger">' + title + '</h4></div>\n' +
                                '<div style="font-size: 12px">' + content + '</div>\n' +
                                '</div>\n';
                            parent.layer.open({
                                type: 1,
                                title: '私信' + '<span style="float: right;right: 1px;font-size: 12px;color: #b1b3b9;margin-top: 1px">' + noticeTime + '</span>',
                                area: '150px;',
                                shade: 0.8,
                                id: 'layuimini-notice',
                                btn: ['确定'],
                                btnAlign: 'c',
                                moveType: 1,
                                content: html
                            });
                        }
                    });
                },
            };
            notice.warning(data.F_MessageInfo);
        }
        else {
            notice.options = {
                positionClass: "toast-bottom-right",
                onclick: function () {
                    common.ajax({
                        url: "/InfoManage/Message/ReadMsgForm",
                        data: { keyValue: data.F_Id },
                        type: 'POST',
                        success: function () {
                            $("[layuimini-href='" + data.F_Href + "']", ".layuimini-menu-left").click();
                        }
                    });
                }
            };
            notice.error(data.F_MessageInfo);
        }
        $("#noticeMarker").html("<span class='layui-badge-dot'></span>");
    });
}

// 启动连接
connectChat();
