using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
namespace WaterCloud.Code
{
    /// <summary>
    /// 队列工具类，用于另起线程处理执行类型数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueHelper<T> : IDisposable
    {
        #region Private Field
        /// <summary>
        /// The inner queue.
        /// </summary>
        private readonly ConcurrentQueue<T> _innerQueue;

        /// <summary>
        /// The deal task.
        /// </summary>
        private readonly Task _dealTask;

        /// <summary>
        /// The flag for end thread.
        /// </summary>
        private bool _endThreadFlag = false;

        /// <summary>
        /// The auto reset event.
        /// </summary>
        private readonly AutoResetEvent _autoResetEvent = new(true);
        #endregion

        #region Public Property
        /// <summary>
        /// The deal action.
        /// </summary>
        public Action<T> DealAction { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the QueueHelper`1 class.
        /// </summary>
        public QueueHelper()
        {
            this._innerQueue = new();
            this._dealTask = Task.Run(() => this.DealQueue());
        }

        /// <summary>
        /// Initializes a new instance of the QueueHelper&lt;T&gt; class.
        /// </summary>
        /// <param name="DealAction">The deal action.</param>
        public QueueHelper(Action<T> DealAction)
        {
            this.DealAction = DealAction;
            this._innerQueue = new();
            this._dealTask = Task.Run(() => this.DealQueue());
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Save entity to Queue.
        /// </summary>
        /// <param name="entity">The entity what will be deal.</param>
        public bool Enqueue(T entity)
        {
            if (!this._endThreadFlag)
            {
                this._innerQueue.Enqueue(entity);
                this._autoResetEvent.Set();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Disposes current instance, end the deal thread and inner queue.
        /// </summary>
        public void Dispose()
        {
            if (!this._endThreadFlag)
            {
                this._endThreadFlag = true;
                this._innerQueue.Enqueue(default);
                this._autoResetEvent.Set();

                if (!this._dealTask.IsCompleted)
                    this._dealTask.Wait();

                this._dealTask.Dispose();

                this._autoResetEvent.Dispose();
                this._autoResetEvent.Close();
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Out Queue.
        /// </summary>
        /// <param name="entity">The init entity.</param>
        /// <returns>The entity what will be deal.</returns>
        private bool Dequeue(out T entity)
        {
            return this._innerQueue.TryDequeue(out entity);
        }

        /// <summary>
        /// Deal entity in Queue.
        /// </summary>
        private void DealQueue()
        {
            try
            {
                while (true)
                {
                    if (this.Dequeue(out T entity))
                    {
                        if (this._endThreadFlag && Equals(entity, default(T)))
                            return;

                        try
                        {
                            this.DealAction?.Invoke(entity);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Write(ex);
                        }
                    }
                    else
                    {
                        this._autoResetEvent.WaitOne();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
            }
        }
        #endregion
    }
}