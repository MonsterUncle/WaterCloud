using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serenity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
namespace WaterCloud.Code
{
    /// <summary>
    /// RabbitMq工具类
    /// </summary>
    public class RabbitMqHelper : IDisposable
    {
        #region 私有静态字段
        /// <summary>
        /// RabbitMQ建议客户端线程之间不要共用Model，至少要保证共用Model的线程发送消息必须是串行的，但是建议尽量共用Connection。
        /// </summary>
        private static readonly ConcurrentDictionary<string, IModel> _channelDic = new();

        /// <summary>
        /// QueueHelper缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, QueueHelper<QueueMessage>> _queueHelperDic = new();

        /// <summary>
        /// 用于缓存路由键数据
        /// </summary>
        private static readonly ConcurrentDictionary<string, (string exchange, string queue, string routeKey)> _routeDic = new();

        /// <summary>
        /// RabbitMq连接
        /// </summary>
        private static IConnection _connection;

        /// <summary>
        /// 线程对象，线程锁使用
        /// </summary>
        private static readonly object _locker = new();
        #endregion

        #region 公有属性
        /// <summary>
        /// 默认内置死信交换机，默认值：deadletter.default.router
        /// </summary>
        public string DefaultDeadLetterExchange { get; set; } = "deadletter.default.router";
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">RabbitMq配置</param>
        public RabbitMqHelper(MqConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (_connection == null)
            {
                lock (_locker)
                {
                    if (_connection == null)
                        _connection = new ConnectionFactory
                        {
                            //设置主机名
                            HostName = config.HostName,

                            //虚拟主机
                            VirtualHost = config.VirtualHost,

                            //设置心跳时间
                            RequestedHeartbeat = config.RequestedHeartbeat,

                            //设置自动重连
                            AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,

                            //重连时间
                            NetworkRecoveryInterval = config.NetworkRecoveryInterval,

                            //用户名
                            UserName = config.UserName,

                            //密码
                            Password = config.Password
                        }.CreateConnection();
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="factory">RabbitMq连接工厂</param>
        public RabbitMqHelper(ConnectionFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (_connection == null)
            {
                lock (_locker)
                {
                    if (_connection == null)
                        _connection = factory.CreateConnection();
                }
            }
        }
        #endregion

        #region 管道
        /// <summary>
        /// 获取管道
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <returns></returns>
        public IModel GetChannel(string queue = "default")
        {
            IModel channel = null;

            if (!string.IsNullOrEmpty(queue) && _channelDic.ContainsKey(queue))
                channel = _channelDic[queue];

            if (!string.IsNullOrEmpty(queue) && channel == null)
            {
                lock (_locker)
                {
                    channel = _channelDic.GetOrAdd(queue, queue =>
                    {
                        var model = _connection.CreateModel();
                        _channelDic[queue] = model;

                        return model;
                    });
                }
            }

            channel ??= _connection.CreateModel();

            return EnsureOpened(channel);
        }

        /// <summary>
        /// 确保管道是已打开状态
        /// </summary>
        /// <param name="channel">管道</param>
        /// <returns></returns>
        public IModel EnsureOpened(IModel channel)
        {
            channel ??= GetChannel();

            if (channel.IsClosed)
            {
                var data = _channelDic.Where(x => x.Value == channel)?.FirstOrDefault();
                if (data != null && data.Value.Key != null)
                {
                    //移除已关闭的管道
                    _channelDic.TryRemove(data.Value.Key, out var model);

                    //重新获取管道
                    channel = GetChannel(data.Value.Key);
                }
                else
                {
                    channel = GetChannel();
                }
            }

            return channel;
        }

        /// <summary>
        /// 如果交换机或者队列不存在则移除缓存中异常关闭的管道
        /// </summary>
        /// <param name="channel"></param>
        public void RemoveIfNotExist(IModel channel)
        {
            var data = _channelDic.Where(x => x.Value == channel)?.FirstOrDefault();
            if (data != null && data.Value.Key != null)
            {
                //移除已关闭的管道
                _channelDic.TryRemove(data.Value.Key, out var model);
                channel = null;
            }
        }

        /// <summary>
        /// 获取QueueHelper
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public QueueHelper<QueueMessage> GetQueueHelper(string queue)
        {
            if (!string.IsNullOrEmpty(queue))
                queue = "default";

            if (_queueHelperDic.ContainsKey(queue))
                return _queueHelperDic[queue];

            lock (_locker)
            {
                return _queueHelperDic.GetOrAdd(queue, queue =>
                {
                    var queueHelper = new QueueHelper<QueueMessage>(x =>
                        PublishRabbitMqMessage(
                            x.Exchange,
                            x.Queue,
                            x.RoutingKey,
                            x.Body,
                            x.ExchangeType,
                            x.Durable,
                            x.Confirm,
                            x.Expiration,
                            x.Priority,
                            x.QueueArguments,
                            x.ExchangeArguments,
                            x.Headers));

                    _queueHelperDic[queue] = queueHelper;

                    return queueHelper;
                });
            }
        }
        #endregion

        #region 交换机
        /// <summary>
        /// 声明交换机，交换机可以重复声明，但是声明所使用的参数必须一致，否则会抛出异常。
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="exchangeType">交换机类型：
        /// 1、Direct Exchange – 处理路由键。需要将一个队列绑定到交换机上，要求该消息与一个特定的路由键完全
        /// 匹配。这是一个完整的匹配。如果一个队列绑定到该交换机上要求路由键 “dog”，则只有被标记为“dog”的
        /// 消息才被转发，不会转发dog.puppy，也不会转发dog.guard，只会转发dog
        /// 2、Fanout Exchange – 不处理路由键。你只需要简单的将队列绑定到交换机上。一个发送到交换机的消息都
        /// 会被转发到与该交换机绑定的所有队列上。很像子网广播，每台子网内的主机都获得了一份复制的消息。Fanout
        /// 交换机转发消息是最快的。
        /// 3、Topic Exchange – 将路由键和某模式进行匹配。此时队列需要绑定要一个模式上。符号“#”匹配一个或多
        /// 个词，符号“*”匹配不多不少一个词。因此“audit.#”能够匹配到“audit.irs.corporate”，但是“audit.*”
        /// 只会匹配到“audit.irs”。</param>
        /// <param name="durable">持久化</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        /// <returns>管道</returns>
        public IModel ExchangeDeclare(
            IModel channel,
            string exchange,
            string exchangeType = ExchangeType.Direct,
            bool durable = true,
            bool autoDelete = false,
            IDictionary<string, object> arguments = null)
        {
            channel = EnsureOpened(channel);
            channel.ExchangeDeclare(exchange, exchangeType, durable, autoDelete, arguments);

            return channel;
        }

        /// <summary>
        /// 声明交换机，交换机可以重复声明，但是声明所使用的参数必须一致，否则会抛出异常。
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="exchangeType">交换机类型：
        /// 1、Direct Exchange – 处理路由键。需要将一个队列绑定到交换机上，要求该消息与一个特定的路由键完全
        /// 匹配。这是一个完整的匹配。如果一个队列绑定到该交换机上要求路由键 “dog”，则只有被标记为“dog”的
        /// 消息才被转发，不会转发dog.puppy，也不会转发dog.guard，只会转发dog
        /// 2、Fanout Exchange – 不处理路由键。你只需要简单的将队列绑定到交换机上。一个发送到交换机的消息都
        /// 会被转发到与该交换机绑定的所有队列上。很像子网广播，每台子网内的主机都获得了一份复制的消息。Fanout
        /// 交换机转发消息是最快的。
        /// 3、Topic Exchange – 将路由键和某模式进行匹配。此时队列需要绑定要一个模式上。符号“#”匹配一个或多
        /// 个词，符号“*”匹配不多不少一个词。因此“audit.#”能够匹配到“audit.irs.corporate”，但是“audit.*”
        /// 只会匹配到“audit.irs”。</param>
        /// <param name="durable">持久化</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        /// <returns>管道</returns>
        public IModel ExchangeDeclareNoWait(
            IModel channel,
            string exchange,
            string exchangeType = ExchangeType.Direct,
            bool durable = true,
            bool autoDelete = false,
            IDictionary<string, object> arguments = null)
        {
            channel = EnsureOpened(channel);
            channel.ExchangeDeclareNoWait(exchange, exchangeType, durable, autoDelete, arguments);

            return channel;
        }

        /// <summary>
        /// 被动声明交换机，用于判断交换机是否存在，若存在则无异常，若不存在则抛出异常
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        public void ExchangeDeclarePassive(IModel channel, string exchange)
        {
            EnsureOpened(channel).ExchangeDeclarePassive(exchange);
        }

        /// <summary>
        /// 判断交换机是否存在
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <returns></returns>
        public bool IsExchangeExist(IModel channel, string exchange)
        {
            try
            {
                ExchangeDeclarePassive(channel, exchange);
                return true;
            }
            catch
            {
                RemoveIfNotExist(channel);
                return false;
            }
        }

        /// <summary>
        /// 删除交换机
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="ifUnused">是否没有被使用</param>
        public void ExchangeDelete(
            IModel channel,
            string exchange,
            bool ifUnused = false)
        {
            RouteKeyRemoveByExchange(exchange);
            EnsureOpened(channel).ExchangeDelete(exchange, ifUnused);
        }

        /// <summary>
        /// 删除交换机
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="ifUnused">是否没有被使用</param>
        public void ExchangeDeleteNoWait(
            IModel channel,
            string exchange,
            bool ifUnused = false)
        {
            RouteKeyRemoveByExchange(exchange);
            EnsureOpened(channel).ExchangeDeleteNoWait(exchange, ifUnused);
        }

        /// <summary>
        /// 绑定交换机
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="destinationExchange">目标交换机</param>
        /// <param name="sourceExchange">源交换机</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="arguments">参数</param>
        public void ExchangeBind(
            IModel channel,
            string destinationExchange,
            string sourceExchange,
            string routingKey,
            IDictionary<string, object> arguments = null)
        {
            EnsureOpened(channel).ExchangeBind(destinationExchange, sourceExchange, routingKey, arguments);
        }

        /// <summary>
        /// 绑定交换机
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="destinationExchange">目标交换机</param>
        /// <param name="sourceExchange">源交换机</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="arguments">参数</param>
        public void ExchangeBindNoWait(
            IModel channel,
            string destinationExchange,
            string sourceExchange,
            string routingKey,
            IDictionary<string, object> arguments = null)
        {
            EnsureOpened(channel).ExchangeBindNoWait(destinationExchange, sourceExchange, routingKey, arguments);
        }

        /// <summary>
        /// 解绑交换机
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="destinationExchange">目标交换机</param>
        /// <param name="sourceExchange">源交换机</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="arguments">参数</param>
        public void ExchangeUnbind(
            IModel channel,
            string destinationExchange,
            string sourceExchange,
            string routingKey,
            IDictionary<string, object> arguments = null)
        {
            EnsureOpened(channel).ExchangeUnbind(destinationExchange, sourceExchange, routingKey, arguments);
        }

        /// <summary>
        /// 解绑交换机
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="destinationExchange">目标交换机</param>
        /// <param name="sourceExchange">源交换机</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="arguments">参数</param>
        public void ExchangeUnbindNoWait(
            IModel channel,
            string destinationExchange,
            string sourceExchange,
            string routingKey,
            IDictionary<string, object> arguments = null)
        {
            EnsureOpened(channel).ExchangeUnbindNoWait(destinationExchange, sourceExchange, routingKey, arguments);
        }
        #endregion

        #region 队列
        /// <summary>
        /// 声明队列，队列可以重复声明，但是声明所使用的参数必须一致，否则会抛出异常。
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        /// <param name="durable">持久化</param>
        /// <param name="exclusive">排他队列，如果一个队列被声明为排他队列，该队列仅对首次声明它的连接可见，
        /// 并在连接断开时自动删除。这里需要注意三点：其一，排他队列是基于连接可见的，同一连接的不同信道是可
        /// 以同时访问同一个连接创建的排他队列的。其二，“首次”，如果一个连接已经声明了一个排他队列，其他连
        /// 接是不允许建立同名的排他队列的，这个与普通队列不同。其三，即使该队列是持久化的，一旦连接关闭或者
        /// 客户端退出，该排他队列都会被自动删除的。这种队列适用于只限于一个客户端发送读取消息的应用场景。</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        /// <returns>管道</returns>
        public IModel QueueDeclare(
            IModel channel,
            string queue,
            bool durable = true,
            bool exclusive = false,
            bool autoDelete = false,
            IDictionary<string, object> arguments = null)
        {
            channel = EnsureOpened(channel);
            channel.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);

            return channel;
        }

        /// <summary>
        /// 声明队列，队列可以重复声明，但是声明所使用的参数必须一致，否则会抛出异常。
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        /// <param name="durable">持久化</param>
        /// <param name="exclusive">排他队列，如果一个队列被声明为排他队列，该队列仅对首次声明它的连接可见，
        /// 并在连接断开时自动删除。这里需要注意三点：其一，排他队列是基于连接可见的，同一连接的不同信道是可
        /// 以同时访问同一个连接创建的排他队列的。其二，“首次”，如果一个连接已经声明了一个排他队列，其他连
        /// 接是不允许建立同名的排他队列的，这个与普通队列不同。其三，即使该队列是持久化的，一旦连接关闭或者
        /// 客户端退出，该排他队列都会被自动删除的。这种队列适用于只限于一个客户端发送读取消息的应用场景。</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        /// <returns>管道</returns>
        public IModel QueueDeclareNoWait(
            IModel channel,
            string queue,
            bool durable = true,
            bool exclusive = false,
            bool autoDelete = false,
            IDictionary<string, object> arguments = null)
        {
            channel = EnsureOpened(channel);
            channel.QueueDeclareNoWait(queue, durable, exclusive, autoDelete, arguments);

            return channel;
        }

        /// <summary>
        /// 被动声明队列，用于判断队列是否存在，若队列存在则无异常，若不存在则抛异常
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        public void QueueDeclarePassive(IModel channel, string queue)
        {
            EnsureOpened(channel).QueueDeclarePassive(queue);
        }

        /// <summary>
        /// 判断队列是否存在
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        /// <returns></returns>
        public bool IsQueueExist(IModel channel, string queue)
        {
            try
            {
                QueueDeclarePassive(channel, queue);
                return true;
            }
            catch
            {
                RemoveIfNotExist(channel);
                return false;
            }
        }

        /// <summary>
        /// 删除队列
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        /// <param name="ifUnused">是否没有被使用</param>
        /// <param name="ifEmpty">是否为空</param>
        /// <returns></returns>
        public uint QueueDelete(
            IModel channel,
            string queue,
            bool ifUnused = false,
            bool ifEmpty = false)
        {
            RouteKeyRemoveByQueue(queue);
            return EnsureOpened(channel).QueueDelete(queue, ifUnused, ifEmpty);
        }

        /// <summary>
        /// 删除队列
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        /// <param name="ifUnused">是否没有被使用</param>
        /// <param name="ifEmpty">是否为空</param>
        public void QueueDeleteNoWait(
            IModel channel,
            string queue,
            bool ifUnused = false,
            bool ifEmpty = false)
        {
            RouteKeyRemoveByQueue(queue);
            EnsureOpened(channel).QueueDeleteNoWait(queue, ifUnused, ifEmpty);
        }

        /// <summary>
        /// 绑定队列
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="arguments">参数</param>
        public void QueueBind(
            IModel channel,
            string exchange,
            string queue,
            string routingKey,
            IDictionary<string, object> arguments = null)
        {
            if (IsRouteKeyExist(exchange, queue, routingKey))
                return;

            EnsureOpened(channel).QueueBind(queue, exchange, routingKey, arguments);
        }

        /// <summary>
        /// 绑定队列
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="arguments">参数</param>
        public void QueueBindNoWait(
            IModel channel,
            string exchange,
            string queue,
            string routingKey,
            IDictionary<string, object> arguments = null)
        {
            if (IsRouteKeyExist(exchange, queue, routingKey))
                return;

            EnsureOpened(channel).QueueBindNoWait(queue, exchange, routingKey, arguments);
        }

        /// <summary>
        /// 解绑队列
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="arguments">参数</param>
        public void QueueUnbind(
            IModel channel,
            string exchange,
            string queue,
            string routingKey,
            IDictionary<string, object> arguments = null)
        {
            RouteKeyRemove(exchange, queue, routingKey);
            EnsureOpened(channel).QueueUnbind(queue, exchange, routingKey, arguments);
        }

        /// <summary>
        /// 清除队列
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        public void QueuePurge(IModel channel, string queue)
        {
            EnsureOpened(channel).QueuePurge(queue);
        }
        #endregion

        #region 路由
        /// <summary>
        /// 判断路由键是否存在，仅在内存中作判断
        /// </summary>
        /// <param name="exchange">交换机</param>
        /// <param name="queue">队列</param>
        /// <param name="routeKey">路由键</param>
        /// <returns></returns>
        public bool IsRouteKeyExist(string exchange, string queue, string routeKey)
        {
            var key = $"{exchange}.{queue}.{routeKey}";

            if (_routeDic.ContainsKey(key))
                return true;
            else
            {
                lock (_locker)
                {
                    if (!_routeDic.ContainsKey(key))
                    {
                        _routeDic.TryAdd(key, (exchange, queue, routeKey));
                        return false;
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 移除路由键缓存
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routeKey"></param>
        public void RouteKeyRemove(string exchange, string queue, string routeKey)
        {
            var key = $"{exchange}.{queue}.{routeKey}";
            if (_routeDic.ContainsKey(key))
            {
                lock (_locker)
                {
                    if (_routeDic.ContainsKey(key))
                        _routeDic.TryRemove(key, out var _);
                }
            }
        }

        /// <summary>
        /// 移除路由键缓存
        /// </summary>
        /// <param name="queue">队列</param>
        public void RouteKeyRemoveByQueue(string queue)
        {
            if (!_routeDic.IsEmpty && _routeDic.Values.Any(x => x.queue == queue))
            {
                lock (_locker)
                {
                    if (!_routeDic.IsEmpty && _routeDic.Values.Any(x => x.queue == queue))
                    {
                        var keys = _routeDic.Keys.Where(x => x.Split('.')[1] == queue);
                        foreach (var key in keys)
                        {
                            _routeDic.TryRemove(key, out var _);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除路由键缓存
        /// </summary>
        /// <param name="exchange">交换机</param>
        public void RouteKeyRemoveByExchange(string exchange)
        {
            if (!_routeDic.IsEmpty && _routeDic.Values.Any(x => x.exchange == exchange))
            {
                lock (_locker)
                {
                    if (!_routeDic.IsEmpty && _routeDic.Values.Any(x => x.exchange == exchange))
                    {
                        var keys = _routeDic.Keys.Where(x => x.Split('.')[0] == exchange);
                        foreach (var key in keys)
                        {
                            _routeDic.TryRemove(key, out var _);
                        }
                    }
                }
            }
        }
        #endregion

        #region 发布消息
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="command">消息指令</param>
        /// <param name="confirm">消息发送确认</param>
        /// <param name="expiration">单个消息过期时间，单位ms</param>
        /// <param name="priority">单个消息优先级，数值越大优先级越高，取值范围：0-9</param>
        /// <returns></returns>
        public bool Publish<T>(
            T command,
            bool confirm = false,
            string expiration = null,
            byte? priority = null)
            where T : class
        {
            var attribute = typeof(T).GetAttribute<RabbitMqAttribute>();
            if (attribute == null)
                throw new ArgumentException("RabbitMqAttribute Is Null!");

            return Publish(attribute, command, confirm, expiration, priority);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="attribute">RabbitMq特性配置</param>
        /// <param name="command">消息指令</param>
        /// <param name="confirm">消息发送确认</param>
        /// <param name="expiration">单个消息过期时间，单位ms</param>
        /// <param name="priority">单个消息优先级，数值越大优先级越高，取值范围：0-9</param>
        /// <returns></returns>
        public bool Publish<T>(
            RabbitMqAttribute attribute,
            T command,
            bool confirm = false,
            string expiration = null,
            byte? priority = null)
            where T : class
        {
            //消息内容
            var body = command.ToJson();

            //自定义队列参数
            var arguments = new Dictionary<string, object>();

            //设置队列消息过期时间，指整个队列的所有消息
            if (attribute.MessageTTL > 0)
                arguments["x-message-ttl"] = attribute.MessageTTL;

            //设置队列过期时间
            if (attribute.AutoExpire > 0)
                arguments["x-expires"] = attribute.AutoExpire;

            //设置队列最大长度
            if (attribute.MaxLength > 0)
                arguments["x-max-length"] = attribute.MaxLength;

            //设置队列占用最大空间
            if (attribute.MaxLengthBytes > 0)
                arguments["x-max-length-bytes"] = attribute.MaxLengthBytes;

            //设置队列溢出行为
            if (attribute.OverflowBehaviour == "drop-head" || attribute.OverflowBehaviour == "reject-publish")
                arguments["x-overflow"] = attribute.OverflowBehaviour;

            //设置死信交换机
            if (!attribute.DeadLetterExchange.IsNullOrEmpty())
                arguments["x-dead-letter-exchange"] = attribute.DeadLetterExchange;

            //设置死信路由键
            if (!attribute.DeadLetterRoutingKey.IsNullOrEmpty())
                arguments["x-dead-letter-routing-key"] = attribute.DeadLetterRoutingKey;

            //设置队列优先级
            if (attribute.MaximumPriority > 0 && attribute.MaximumPriority <= 10)
                arguments["x-max-priority"] = attribute.MaximumPriority;

            //设置队列惰性模式
            if (attribute.LazyMode == "default" || attribute.LazyMode == "lazy")
                arguments["x-queue-mode"] = attribute.LazyMode;

            //设置集群配置
            if (!attribute.MasterLocator.IsNullOrEmpty())
                arguments["x-queue-master-locator"] = attribute.MasterLocator;

            //发送消息
            return Publish(
                attribute.Exchange,
                attribute.Queue,
                attribute.RoutingKey,
                body,
                attribute.ExchangeType,
                attribute.Durable,
                confirm,
                expiration,
                priority,
                arguments,
                null,
                attribute.Header?.ToObject<Dictionary<string, object>>());
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="command">消息指令</param>
        /// <param name="confirm">消息发送确认</param>
        /// <param name="expiration">单个消息过期时间，单位ms</param>
        /// <param name="priority">单个消息优先级，数值越大优先级越高，取值范围：0-9</param>
        /// <returns></returns>
        public bool Publish<T>(
            IEnumerable<T> command,
            bool confirm = false,
            string expiration = null,
            byte? priority = null)
            where T : class
        {
            var attribute = typeof(T).GetAttribute<RabbitMqAttribute>();
            if (attribute == null)
                throw new ArgumentException("RabbitMqAttribute Is Null!");

            return Publish(attribute, command, confirm, expiration, priority);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="attribute">RabbitMq特性配置</param>
        /// <param name="command">消息指令</param>
        /// <param name="confirm">消息发送确认</param>
        /// <param name="expiration">单个消息过期时间，单位ms</param>
        /// <param name="priority">单个消息优先级，数值越大优先级越高，取值范围：0-9</param>
        /// <returns></returns>
        public bool Publish<T>(
            RabbitMqAttribute attribute,
            IEnumerable<T> command,
            bool confirm = false,
            string expiration = null,
            byte? priority = null)
            where T : class
        {
            //消息内容
            var body = command.Select(x => x.ToJson());

            //自定义队列参数
            var arguments = new Dictionary<string, object>();

            //设置队列消息过期时间，指整个队列的所有消息
            if (attribute.MessageTTL > 0)
                arguments["x-message-ttl"] = attribute.MessageTTL;

            //设置队列过期时间
            if (attribute.AutoExpire > 0)
                arguments["x-expires"] = attribute.AutoExpire;

            //设置队列最大长度
            if (attribute.MaxLength > 0)
                arguments["x-max-length"] = attribute.MaxLength;

            //设置队列占用最大空间
            if (attribute.MaxLengthBytes > 0)
                arguments["x-max-length-bytes"] = attribute.MaxLengthBytes;

            //设置队列溢出行为
            if (attribute.OverflowBehaviour == "drop-head" || attribute.OverflowBehaviour == "reject-publish")
                arguments["x-overflow"] = attribute.OverflowBehaviour;

            //设置死信交换机
            if (!attribute.DeadLetterExchange.IsNullOrEmpty())
                arguments["x-dead-letter-exchange"] = attribute.DeadLetterExchange;

            //设置死信路由键
            if (!attribute.DeadLetterRoutingKey.IsNullOrEmpty())
                arguments["x-dead-letter-routing-key"] = attribute.DeadLetterRoutingKey;

            //设置队列优先级
            if (attribute.MaximumPriority > 0 && attribute.MaximumPriority <= 10)
                arguments["x-max-priority"] = attribute.MaximumPriority;

            //设置队列惰性模式
            if (attribute.LazyMode == "default" || attribute.LazyMode == "lazy")
                arguments["x-queue-mode"] = attribute.LazyMode;

            //设置集群配置
            if (!attribute.MasterLocator.IsNullOrEmpty())
                arguments["x-queue-master-locator"] = attribute.MasterLocator;

            //发送消息
            return Publish(
                attribute.Exchange,
                attribute.Queue,
                attribute.RoutingKey,
                body,
                attribute.ExchangeType,
                attribute.Durable,
                confirm,
                expiration,
                priority,
                arguments,
                null,
                attribute.Header?.ToObject<Dictionary<string, object>>());
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">消息内容</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="durable">持久化</param>
        /// <param name="confirm">消息发送确认</param>
        /// <param name="expiration">单个消息过期时间，单位ms</param>
        /// <param name="priority">单个消息优先级，数值越大优先级越高，取值范围：0-9</param>
        /// <param name="queueArguments">队列参数</param>
        /// <param name="exchangeArguments">交换机参数</param>
        /// <param name="headers">消息头部</param>
        /// <returns></returns>
        public bool Publish(
            string exchange,
            string queue,
            string routingKey,
            string body,
            string exchangeType = ExchangeType.Direct,
            bool durable = true,
            bool confirm = false,
            string expiration = null,
            byte? priority = null,
            IDictionary<string, object> queueArguments = null,
            IDictionary<string, object> exchangeArguments = null,
            IDictionary<string, object> headers = null)
        {
            return Publish(
                exchange,
                queue,
                routingKey,
                new[] { body },
                exchangeType,
                durable,
                confirm,
                expiration,
                priority,
                queueArguments,
                exchangeArguments,
                headers);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">消息内容</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="durable">持久化</param>
        /// <param name="confirm">消息发送确认</param>
        /// <param name="expiration">单个消息过期时间，单位ms</param>
        /// <param name="priority">单个消息优先级，数值越大优先级越高，取值范围：0-9</param>
        /// <param name="queueArguments">队列参数</param>
        /// <param name="exchangeArguments">交换机参数</param>
        /// <param name="headers">消息头部</param>
        /// <returns></returns>
        public bool Publish(
            string exchange,
            string queue,
            string routingKey,
            IEnumerable<string> body,
            string exchangeType = ExchangeType.Direct,
            bool durable = true,
            bool confirm = false,
            string expiration = null,
            byte? priority = null,
            IDictionary<string, object> queueArguments = null,
            IDictionary<string, object> exchangeArguments = null,
            IDictionary<string, object> headers = null)
        {
            return GetQueueHelper(queue).Enqueue(
                new QueueMessage
                {
                    Exchange = exchange,
                    Queue = queue,
                    RoutingKey = routingKey,
                    Body = body,
                    ExchangeType = exchangeType,
                    Durable = durable,
                    Confirm = confirm,
                    Expiration = expiration,
                    Priority = priority,
                    QueueArguments = queueArguments,
                    ExchangeArguments = exchangeArguments,
                    Headers = headers
                });
        }

        /// <summary>
        /// 发布消息到RabbitMq，注意此方法使用IModel线程不安全，建议使用Publish方法，若要使用需要保证单线程调用
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">消息内容</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="durable">持久化</param>
        /// <param name="confirm">消息发送确认</param>
        /// <param name="expiration">单个消息过期时间，单位ms</param>
        /// <param name="priority">单个消息优先级，数值越大优先级越高，取值范围：0-9</param>
        /// <param name="queueArguments">队列参数</param>
        /// <param name="exchangeArguments">交换机参数</param>
        /// <param name="headers">消息头部</param>
        /// <returns></returns>
        public bool PublishRabbitMqMessage(
            string exchange,
            string queue,
            string routingKey,
            IEnumerable<string> body,
            string exchangeType = ExchangeType.Direct,
            bool durable = true,
            bool confirm = false,
            string expiration = null,
            byte? priority = null,
            IDictionary<string, object> queueArguments = null,
            IDictionary<string, object> exchangeArguments = null,
            IDictionary<string, object> headers = null)
        {
            //获取管道
            var channel = GetChannel(queue);

            //判断交换机是否存在
            if (!IsExchangeExist(channel, exchange))
                channel = ExchangeDeclare(channel, exchange, exchangeType, durable, arguments: exchangeArguments);

            //判断队列是否存在
            if (!IsQueueExist(channel, queue))
                channel = QueueDeclare(channel, queue, durable, arguments: queueArguments);

            //绑定交换机和队列
            QueueBind(channel, exchange, queue, routingKey);

            //声明消息属性
            var props = channel.CreateBasicProperties();

            //持久化
            props.Persistent = durable;

            //单个消息过期时间
            if (!expiration.IsNullOrEmpty())
                props.Expiration = expiration;

            //单个消息优先级
            if (priority >= 0 && priority <= 9)
                props.Priority = priority.Value;

            //消息头部
            if (headers != null)
                props.Headers = headers;

            //是否启用消息发送确认机制
            if (confirm)
                channel.ConfirmSelect();

            //发送消息
            foreach (var item in body)
            {
                var text = string.IsNullOrEmpty(item)? null: Encoding.UTF8.GetBytes(item);
                channel.BasicPublish(exchange, routingKey, props, text);
            }

            //消息发送失败处理
            if (confirm && !channel.WaitForConfirms())
                return false;

            return true;
        }

        /// <summary>
        /// 发布消息到内置死信队列
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">消息内容</param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="exception">异常</param>
        /// <returns></returns>
        private bool PublishToDead(
            string queue,
            string exchange,
            string routingKey,
            string body,
            int retryCount,
            Exception exception)
        {
            //内置死信队列、交换机、路由键
            var deadLetterQueue = $"{queue.ToLower()}.{(exception != null ? "error" : "fail")}";
            var deadLetterExchange = DefaultDeadLetterExchange;
            var deadLetterRoutingKey = $"{queue.ToLower()}.deadletter";

            //内置死信队列内容
            var deadLetterBody = new DeadLetterQueue
            {
                Body = body,
                CreateDateTime = DateTime.Now,
                Exception = exception,
                ExceptionMsg = exception?.Message,
                Queue = queue,
                RoutingKey = routingKey,
                Exchange = exchange,
                RetryCount = retryCount
            };

            //发送内置死信消息，注意此处与原生的死信消息无关
            return Publish(deadLetterExchange, deadLetterQueue, deadLetterRoutingKey, deadLetterBody.ToJson());
        }
        #endregion

        #region 订阅消息
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscriber">消费处理委托</param>
        /// <param name="handler">异常处理委托</param>
        /// <param name="registered">注册事件</param>
        /// <param name="unregistered">取消注册事件</param>
        /// <param name="shutdown">关闭事件</param>
        public void Subscribe<T>(
            Func<T, BasicDeliverEventArgs, bool> subscriber,
            Action<string, int, Exception> handler,
            EventHandler<ConsumerEventArgs> registered = null,
            EventHandler<ConsumerEventArgs> unregistered = null,
            EventHandler<ShutdownEventArgs> shutdown = null)
            where T : class
        {
            var attribute = typeof(T).GetAttribute<RabbitMqAttribute>();
            if (attribute == null)
                throw new ArgumentException("RabbitMqAttribute Is Null!");

            Subscribe(attribute, subscriber, handler, registered, unregistered, shutdown);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attribute">RabbitMq特性配置</param>
        /// <param name="subscriber">消费处理委托</param>
        /// <param name="handler">异常处理委托</param>
        /// <param name="registered">注册事件</param>
        /// <param name="unregistered">取消注册事件</param>
        /// <param name="shutdown">关闭事件</param>
        public void Subscribe<T>(
            RabbitMqAttribute attribute,
            Func<T, BasicDeliverEventArgs, bool> subscriber,
            Action<string, int, Exception> handler,
            EventHandler<ConsumerEventArgs> registered = null,
            EventHandler<ConsumerEventArgs> unregistered = null,
            EventHandler<ShutdownEventArgs> shutdown = null)
            where T : class
        {
            //自定义参数
            var arguments = new Dictionary<string, object>();

            //设置队列消息过期时间，指整个队列的所有消息
            if (attribute.MessageTTL > 0)
                arguments["x-message-ttl"] = attribute.MessageTTL;

            //设置队列过期时间
            if (attribute.AutoExpire > 0)
                arguments["x-expires"] = attribute.AutoExpire;

            //设置队列最大长度
            if (attribute.MaxLength > 0)
                arguments["x-max-length"] = attribute.MaxLength;

            //设置队列占用最大空间
            if (attribute.MaxLengthBytes > 0)
                arguments["x-max-length-bytes"] = attribute.MaxLengthBytes;

            //设置队列溢出行为
            if (attribute.OverflowBehaviour == "drop-head" || attribute.OverflowBehaviour == "reject-publish")
                arguments["x-overflow"] = attribute.OverflowBehaviour;

            //设置死信交换机
            if (!attribute.DeadLetterExchange.IsNullOrEmpty())
                arguments["x-dead-letter-exchange"] = attribute.DeadLetterExchange;

            //设置死信路由键
            if (!attribute.DeadLetterRoutingKey.IsNullOrEmpty())
                arguments["x-dead-letter-routing-key"] = attribute.DeadLetterRoutingKey;

            //设置队列优先级
            if (attribute.MaximumPriority > 0 && attribute.MaximumPriority <= 10)
                arguments["x-max-priority"] = attribute.MaximumPriority;

            //设置队列惰性模式
            if (attribute.LazyMode == "default" || attribute.LazyMode == "lazy")
                arguments["x-queue-mode"] = attribute.LazyMode;

            //设置集群配置
            if (!attribute.MasterLocator.IsNullOrEmpty())
                arguments["x-queue-master-locator"] = attribute.MasterLocator;

            //订阅消息
            Subscribe(
                attribute.Exchange,
                attribute.Queue,
                attribute.RoutingKey,
                subscriber,
                handler,
                attribute.RetryCount,
                attribute.PrefetchCount,
                attribute.DeadLetter,
                attribute.ExchangeType,
                attribute.Durable,
                attribute.ConsumerCount,
                arguments,
                registered: registered,
                unregistered: unregistered,
                shutdown: shutdown);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscriber">消费处理委托</param>
        /// <param name="handler">异常处理委托</param>
        /// <param name="registered">注册事件</param>
        /// <param name="unregistered">取消注册事件</param>
        /// <param name="shutdown">关闭事件</param>
        public void Subscribe<T>(
            Func<T, BasicDeliverEventArgs, Task<bool>> subscriber,
            Func<string, int, Exception, Task> handler,
            EventHandler<ConsumerEventArgs> registered = null,
            EventHandler<ConsumerEventArgs> unregistered = null,
            EventHandler<ShutdownEventArgs> shutdown = null)
            where T : class
        {
            var attribute = typeof(T).GetAttribute<RabbitMqAttribute>();
            if (attribute == null)
                throw new ArgumentException("RabbitMqAttribute Is Null!");

            Subscribe(attribute, subscriber, handler, registered, unregistered, shutdown);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attribute">RabbitMq特性配置</param>
        /// <param name="subscriber">消费处理委托</param>
        /// <param name="handler">异常处理委托</param>
        /// <param name="registered">注册事件</param>
        /// <param name="unregistered">取消注册事件</param>
        /// <param name="shutdown">关闭事件</param>
        public void Subscribe<T>(
            RabbitMqAttribute attribute,
            Func<T, BasicDeliverEventArgs, Task<bool>> subscriber,
            Func<string, int, Exception, Task> handler,
            EventHandler<ConsumerEventArgs> registered = null,
            EventHandler<ConsumerEventArgs> unregistered = null,
            EventHandler<ShutdownEventArgs> shutdown = null)
            where T : class
        {
            //自定义参数
            var arguments = new Dictionary<string, object>();

            //设置队列消息过期时间，指整个队列的所有消息
            if (attribute.MessageTTL > 0)
                arguments["x-message-ttl"] = attribute.MessageTTL;

            //设置队列过期时间
            if (attribute.AutoExpire > 0)
                arguments["x-expires"] = attribute.AutoExpire;

            //设置队列最大长度
            if (attribute.MaxLength > 0)
                arguments["x-max-length"] = attribute.MaxLength;

            //设置队列占用最大空间
            if (attribute.MaxLengthBytes > 0)
                arguments["x-max-length-bytes"] = attribute.MaxLengthBytes;

            //设置队列溢出行为
            if (attribute.OverflowBehaviour == "drop-head" || attribute.OverflowBehaviour == "reject-publish")
                arguments["x-overflow"] = attribute.OverflowBehaviour;

            //设置死信交换机
            if (!attribute.DeadLetterExchange.IsNullOrEmpty())
                arguments["x-dead-letter-exchange"] = attribute.DeadLetterExchange;

            //设置死信路由键
            if (!attribute.DeadLetterRoutingKey.IsNullOrEmpty())
                arguments["x-dead-letter-routing-key"] = attribute.DeadLetterRoutingKey;

            //设置队列优先级
            if (attribute.MaximumPriority > 0 && attribute.MaximumPriority <= 10)
                arguments["x-max-priority"] = attribute.MaximumPriority;

            //设置队列惰性模式
            if (attribute.LazyMode == "default" || attribute.LazyMode == "lazy")
                arguments["x-queue-mode"] = attribute.LazyMode;

            //设置集群配置
            if (!attribute.MasterLocator.IsNullOrEmpty())
                arguments["x-queue-master-locator"] = attribute.MasterLocator;

            //订阅消息
            Subscribe(
                attribute.Exchange,
                attribute.Queue,
                attribute.RoutingKey,
                subscriber,
                handler,
                attribute.RetryCount,
                attribute.PrefetchCount,
                attribute.DeadLetter,
                attribute.ExchangeType,
                attribute.Durable,
                attribute.ConsumerCount,
                arguments,
                registered: registered,
                unregistered: unregistered,
                shutdown: shutdown);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="subscriber">消费处理委托</param>
        /// <param name="handler">异常处理委托</param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="prefetchCount">预取数量</param>
        /// <param name="deadLetter">是否进入内置死信队列，此处与原生死信无关</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="durable">持久化</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="queueArguments">队列参数</param>
        /// <param name="exchangeArguments">交换机参数</param>
        /// <param name="registered">注册事件</param>
        /// <param name="unregistered">取消注册事件</param>
        /// <param name="shutdown">关闭事件</param>
        public void Subscribe<T>(
            string exchange,
            string queue,
            string routingKey,
            Func<T, BasicDeliverEventArgs, bool> subscriber,
            Action<string, int, Exception> handler,
            int retryCount = 5,
            ushort prefetchCount = 1,
            bool deadLetter = true,
            string exchangeType = ExchangeType.Direct,
            bool durable = true,
            int consumerCount = 1,
            IDictionary<string, object> queueArguments = null,
            IDictionary<string, object> exchangeArguments = null,
            EventHandler<ConsumerEventArgs> registered = null,
            EventHandler<ConsumerEventArgs> unregistered = null,
            EventHandler<ShutdownEventArgs> shutdown = null)
            where T : class
        {
            //获取管道
            var channel = GetChannel(queue);

            //判断交换机是否存在
            if (!IsExchangeExist(channel, exchange))
                channel = ExchangeDeclare(channel, exchange, exchangeType, durable, arguments: exchangeArguments);

            //判断队列是否存在
            if (!IsQueueExist(channel, queue))
                channel = QueueDeclare(channel, queue, durable, arguments: queueArguments);

            //绑定交换机和队列
            QueueBind(channel, exchange, queue, routingKey);

            //设置每次预取数量
            channel.BasicQos(0, prefetchCount, false);

            //根据设置的消费者数量创建消费者
            for (int i = 0; i < consumerCount; i++)
            {
                //创建消费者
                var consumer = new EventingBasicConsumer(channel);

                //接收消息事件
                consumer.Received += (sender, ea) =>
                {
                    var text= Encoding.UTF8.GetString(ea.Body.ToArray());
                    var body = text;
                    var numberOfRetries = 0;
                    Exception exception = null;
                    bool? result = false;

                    while (numberOfRetries <= retryCount)
                    {
                        try
                        {
                            var msg = body.ToObject<T>();

                            result = subscriber?.Invoke(msg, ea);

                            if (result == true)
                                channel.BasicAck(ea.DeliveryTag, false);
                            else
                                channel.BasicNack(ea.DeliveryTag, false, false);

                            //异常置空
                            exception = null;
                            break;
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                            handler?.Invoke(body, numberOfRetries, ex);
                            numberOfRetries++;
                        }
                    }

                    //重试后异常仍未解决
                    if (exception != null)
                        channel.BasicNack(ea.DeliveryTag, false, false);

                    //是否进入内置死信队列
                    if (deadLetter && (!(result == true) || exception != null))
                        PublishToDead(
                            queue,
                            ea.Exchange,
                            ea.RoutingKey,
                            body,
                            exception == null ? numberOfRetries : numberOfRetries - 1,
                            exception);
                };

                //注册事件
                if (registered != null)
                    consumer.Registered += registered;

                //取消注册事件
                if (unregistered != null)
                    consumer.Unregistered += unregistered;

                //关闭事件
                if (shutdown != null)
                    consumer.Shutdown += shutdown;

                //手动确认
                channel.BasicConsume(queue, false, consumer);
            }
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="subscriber">消费处理委托</param>
        /// <param name="handler">异常处理委托</param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="prefetchCount">预取数量</param>
        /// <param name="deadLetter">是否进入内置死信队列，此处与原生死信无关</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="durable">持久化</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="queueArguments">队列参数</param>
        /// <param name="exchangeArguments">交换机参数</param>
        /// <param name="registered">注册事件</param>
        /// <param name="unregistered">取消注册事件</param>
        /// <param name="shutdown">关闭事件</param>
        public void Subscribe<T>(
            string exchange,
            string queue,
            string routingKey,
            Func<T, BasicDeliverEventArgs, Task<bool>> subscriber,
            Func<string, int, Exception, Task> handler,
            int retryCount = 5,
            ushort prefetchCount = 1,
            bool deadLetter = true,
            string exchangeType = ExchangeType.Direct,
            bool durable = true,
            int consumerCount = 1,
            IDictionary<string, object> queueArguments = null,
            IDictionary<string, object> exchangeArguments = null,
            EventHandler<ConsumerEventArgs> registered = null,
            EventHandler<ConsumerEventArgs> unregistered = null,
            EventHandler<ShutdownEventArgs> shutdown = null)
            where T : class
        {
            //获取管道
            var channel = GetChannel(queue);

            //判断交换机是否存在
            if (!IsExchangeExist(channel, exchange))
                channel = ExchangeDeclare(channel, exchange, exchangeType, durable, arguments: exchangeArguments);

            //判断队列是否存在
            if (!IsQueueExist(channel, queue))
                channel = QueueDeclare(channel, queue, durable, arguments: queueArguments);

            //绑定交换机和队列
            QueueBind(channel, exchange, queue, routingKey);

            //设置每次预取数量
            channel.BasicQos(0, prefetchCount, false);

            //根据设置的消费者数量创建消费者
            for (int i = 0; i < consumerCount; i++)
            {
                //创建消费者
                var consumer = new EventingBasicConsumer(channel);

                //接收消息事件
                consumer.Received += async (sender, ea) =>
                {
                    var text = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var body = text;
                    var numberOfRetries = 0;
                    Exception exception = null;
                    bool? result = false;

                    while (numberOfRetries <= retryCount)
                    {
                        try
                        {
                            var msg = body.ToObject<T>();

                            if (subscriber != null)
                                result = await subscriber(msg, ea);

                            if (result == true)
                                channel.BasicAck(ea.DeliveryTag, false);
                            else
                                channel.BasicNack(ea.DeliveryTag, false, false);

                            //异常置空
                            exception = null;
                            break;
                        }
                        catch (Exception ex)
                        {
                            exception = ex;

                            if (handler != null)
                                await handler(body, numberOfRetries, ex);

                            numberOfRetries++;
                        }
                    }

                    //重试后异常仍未解决
                    if (exception != null)
                        channel.BasicNack(ea.DeliveryTag, false, false);

                    //是否进入内置死信队列
                    if (deadLetter && (!(result == true) || exception != null))
                        PublishToDead(
                            queue,
                            ea.Exchange,
                            ea.RoutingKey,
                            body,
                            exception == null ? numberOfRetries : numberOfRetries - 1,
                            exception);
                };

                //注册事件
                if (registered != null)
                    consumer.Registered += registered;

                //取消注册事件
                if (unregistered != null)
                    consumer.Unregistered += unregistered;

                //关闭事件
                if (shutdown != null)
                    consumer.Shutdown += shutdown;

                //手动确认
                channel.BasicConsume(queue, false, consumer);
            }
        }
        #endregion

        #region 获取消息
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">消费处理委托</param>
        public void Pull<T>(
            Action<T, BasicGetResult> handler)
            where T : class
        {
            var attribute = typeof(T).GetAttribute<RabbitMqAttribute>();
            if (attribute == null)
                throw new ArgumentException("RabbitMqAttribute Is Null!");

            Pull(attribute.Exchange, attribute.Queue, attribute.RoutingKey, handler);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="handler">消费处理委托</param>
        public void Pull<T>(
            string exchange,
            string queue,
            string routingKey,
            Action<T, BasicGetResult> handler)
            where T : class
        {
            //获取管道
            var channel = GetChannel(queue);

            //判断交换机是否存在
            if (!IsExchangeExist(channel, exchange))
                channel = ExchangeDeclare(channel, exchange);

            //判断队列是否存在
            if (!IsQueueExist(channel, queue))
                channel = QueueDeclare(channel, queue);

            //绑定交换机和队列
            QueueBind(channel, exchange, queue, routingKey);

            var result = channel.BasicGet(queue, false);
            if (result == null)
                return;
            var text = Encoding.UTF8.GetString(result.Body.ToArray());
            var msg = text.ToObject<T>();
            try
            {
                handler(msg, result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                channel.BasicAck(result.DeliveryTag, false);
            }
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">消费处理委托</param>
        public async Task PullAsync<T>(
            Func<T, BasicGetResult, Task> handler)
            where T : class
        {
            var attribute = typeof(T).GetAttribute<RabbitMqAttribute>();
            if (attribute == null)
                throw new ArgumentException("RabbitMqAttribute Is Null!");

            await PullAsync(attribute.Exchange, attribute.Queue, attribute.RoutingKey, handler);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="handler">消费处理委托</param>
        public async Task PullAsync<T>(
            string exchange,
            string queue,
            string routingKey,
            Func<T, BasicGetResult, Task> handler)
            where T : class
        {
            //获取管道
            var channel = GetChannel(queue);

            //判断交换机是否存在
            if (!IsExchangeExist(channel, exchange))
                channel = ExchangeDeclare(channel, exchange);

            //判断队列是否存在
            if (!IsQueueExist(channel, queue))
                channel = QueueDeclare(channel, queue);

            //绑定交换机和队列
            QueueBind(channel, exchange, queue, routingKey);

            var result = channel.BasicGet(queue, false);
            if (result == null)
                return;
            var text = Encoding.UTF8.GetString(result.Body.ToArray());
            var msg = text.ToObject<T>();
            try
            {
                if (handler != null)
                    await handler(msg, result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                channel.BasicAck(result.DeliveryTag, false);
            }
        }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <param name="channel">管道</param>
        /// <param name="queue">队列名称</param>
        /// <returns></returns>
        public uint GetMessageCount(
            IModel channel,
            string queue)
        {
            return (channel ?? GetChannel(queue)).MessageCount(queue);
        }
        #endregion

        #region 释放资源
        /// <summary>
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            foreach (var item in _channelDic)
            {
                item.Value?.Dispose();
            }

            foreach (var item in _queueHelperDic)
            {
                item.Value?.Dispose();
            }

            _connection?.Dispose();
            _channelDic.Clear();
            _queueHelperDic.Clear();
        }
        #endregion
    }

    /// <summary>
    /// RabbitMq连接配置
    /// </summary>
    public class MqConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; } = "localhost";

        /// <summary>
        /// 心跳时间
        /// </summary>
        public TimeSpan RequestedHeartbeat { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// 自动重连
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; } = true;

        /// <summary>
        /// 重连时间
        /// </summary>
        public TimeSpan NetworkRecoveryInterval { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "guest";

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = "guest";

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; } = 5672;

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; } = "/";
    }

    /// <summary>
    /// 自定义的RabbitMq队列信息实体特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RabbitMqAttribute : Attribute
    {
        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 交换机类型
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 是否持久化，默认true
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// 预取数量，默认1
        /// </summary>
        public ushort PrefetchCount { get; set; } = 1;

        /// <summary>
        /// 异常重试次数，默认5次
        /// </summary>
        public int RetryCount { get; set; } = 5;

        /// <summary>
        /// 消息过期时间，过期后队列中消息自动被删除，单位ms
        /// </summary>
        public int MessageTTL { get; set; }

        /// <summary>
        /// 消息头部对象json字符串
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 队列消费者数量，默认1
        /// </summary>
        public int ConsumerCount { get; set; } = 1;

        /// <summary>
        /// 队列过期时间，过期后队列自动被删除，单位ms
        /// </summary>
        public int AutoExpire { get; set; }

        /// <summary>
        /// 队列最大长度
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 队列占用的最大空间
        /// </summary>
        public int MaxLengthBytes { get; set; }

        /// <summary>
        /// 队列溢出行为，可选值：drop-head或者reject-publish，默认drop-head
        /// </summary>
        public string OverflowBehaviour { get; set; }

        /// <summary>
        /// 是否启用系统内置死信队列处理逻辑，默认true；注意：此处与原生的死信无关
        /// </summary>
        public bool DeadLetter { get; set; } = true;

        /// <summary>
        /// 死信交换机
        /// </summary>
        public string DeadLetterExchange { get; set; }

        /// <summary>
        /// 死信路由键
        /// </summary>
        public string DeadLetterRoutingKey { get; set; }

        /// <summary>
        /// 队列最大优先级，数值越大优先级越高，范围1-255，建议取值1-10
        /// </summary>
        public int MaximumPriority { get; set; }

        /// <summary>
        /// 队列惰性模式，可选值：default或者lazy
        /// </summary>
        public string LazyMode { get; set; }

        /// <summary>
        /// 主定位器，集群配置
        /// </summary>
        public string MasterLocator { get; set; }
    }

    /// <summary>
    /// 内置死信队列实体
    /// </summary>
    public class DeadLetterQueue
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 内存队列消息
    /// </summary>
    public class QueueMessage
    {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public IEnumerable<string> Body { get; set; }

        /// <summary>
        /// 交换机类型
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// 持久化
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>
        /// 消息发送确认
        /// </summary>
        public bool Confirm { get; set; }

        /// <summary>
        /// 单个消息过期时间，单位ms
        /// </summary>
        public string Expiration { get; set; }

        /// <summary>
        /// 单个消息优先级，数值越大优先级越高，取值范围：0-9
        /// </summary>
        public byte? Priority { get; set; }

        /// <summary>
        /// 队列参数
        /// </summary>
        public IDictionary<string, object> QueueArguments { get; set; }

        /// <summary>
        /// 交换机参数
        /// </summary>
        public IDictionary<string, object> ExchangeArguments { get; set; }

        /// <summary>
        /// 消息头部
        /// </summary>
        public IDictionary<string, object> Headers { get; set; }
    }
}