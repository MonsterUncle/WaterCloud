var websocket = '';
let setIntervalWesocketPush = null;
var websocketUrl = '';
var lockReconnect = false; 

/**
 * 建立websocket连接
 * @param {string} url ws地址
 */
function createSocket(url) {
    if (!!url) {
        websocketUrl = url;
    }
    else {
        url = websocketUrl;
    }
    if ('WebSocket' in window) {
        websocket = new WebSocket(url);
    } else if ('MozWebSocket' in window) {
        websocket = new MozWebSocket(url);
    } else {
        _alert("当前浏览器不支持websocket协议,建议使用现代浏览器", 3000)
    }
    websocket.onopen = onopenWS;
    websocket.onmessage = onmessageWS;
    websocket.onerror = onerrorWS;
    websocket.onclose = oncloseWS;
}
// 重连
function reconnect() {
    if (lockReconnect) return;
    lockReconnect = true;
    setTimeout(function () {     //没连接上会一直重连，设置延迟避免请求过多
        createSocket();
        lockReconnect = false;
    }, 3000);
}
/**打开WS之后发送心跳 */
function onopenWS() {
    console.log('websocket连接成功');
    sendPing();
}
/**连接失败重连 */
function onerrorWS()
{
    console.log('websocket连接失败');
    reconnect();
}

/**WS数据接收统一处理 */
function onmessageWS(e)
{
    window.dispatchEvent(new CustomEvent('onmessageWS', {
        detail: {
            data: e.data
        }
    }))
}

/**
 * 发送数据但连接未建立时进行处理等待重发
 * @param {any} message 需要发送的数据
 */
function connecting(message)
{
    setTimeout(() => {
        if (websocket.readyState === 0) {
            connecting(message);
        } else {
            websocket.send(JSON.stringify(message));
        }
    }, 1000)
}

/**
 * 发送数据
 * @param {any} message 需要发送的数据
 */
function sendWSPush(message)
{
    if (top.websocket !== null && top.websocket.readyState === 3) {
        top.websocket.close();
        createSocket();
    } else if (top.websocket.readyState === 1) {
        top.websocket.send(JSON.stringify(message));
    } else if (top.websocket.readyState === 0) {
        connecting(message);
    }
}
var websockettimecount = 0;
/**断开重连 */
function oncloseWS() {
    reconnect();
}
/**发送心跳
 * @param {number} time 心跳间隔毫秒 默认5000
 * @param {string} ping 心跳名称 默认字符串ping
 */
function sendPing(time = 5000, ping = 'ping')
{
    if (!lockReconnect) {
        setTimeout(() => {
            websocket.send(ping);
            sendPing();
        }, time)
    }

}
