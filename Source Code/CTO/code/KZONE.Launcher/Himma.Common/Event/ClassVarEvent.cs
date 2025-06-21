using Himma.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common.Event
{
    public class ClassVarEvent<T> : ICloneable
    {
        /// <summary>
        /// 存储最新值
        /// </summary>
        private T myValue;
        /// <summary>
        /// 存储旧值
        /// </summary>
        public T OldValue;
        /// <summary>
        /// 序号
        /// </summary>
        public int Index;

        /// <summary>
        /// 存储最后变化时间
        /// </summary>
        private DateTime _lastUpdateTime;
        public DateTime LastUpdateTime { get => _lastUpdateTime; }
        /// <summary>
        /// 存储最后变化至现在的时间间隔
        /// </summary>
        public int TimeInterval => GetTimeInterval();
        public int MillisecondsTimeInterval => GetMillisecondsTimeInterval();

        public delegate void MyValueChanged(object sender, EventArgs e);

        public event MyValueChanged OnMyValueChanged;
        /// <summary>
        /// 信号量
        /// </summary>
        //private readonly Lazy<Semaphore> _semaphoreWrite = new Lazy<Semaphore>(() => new Semaphore(1, 1));
        //取消信号量，改成object锁
        protected object _objectWrite = new object();
        public T MyValue
        {
            get { return myValue; }
            set
            {
                //IStructuralEquatable a = (Array)Convert.ChangeType(value, typeof(T));
                //_semaphoreWrite.Value.WaitOne();
                try
                {
                    lock (_objectWrite)
                        if (!Compare(value, myValue))
                        {
                            OldValue = myValue;
                            myValue = value;
                            WhenMyValueChange();
                        }
                }
                catch (Exception)
                {

                    throw;
                }
                //finally { _semaphoreWrite.Value.Release(); }

            }
        }
        /// <summary>
        /// 对比数据是否改变
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool Compare(T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClassVarEvent()
        {
            _lastUpdateTime = DateTime.Now;
            myValue = default(T);
            OldValue = default(T);
        }
        /// <summary>
        /// 更改之后 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfterMyValueChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 改变时调用
        /// </summary>
        private void WhenMyValueChange()
        {
            _lastUpdateTime = DateTime.Now;
            //OnMyValueChanged?.Invoke(this, null);
            //OnMyValueChanged?.BeginInvoke(this.Clone(), null, CallBackEmpty, null);
        }
        /// <summary>
        /// 获取时间间隔
        /// </summary>
        /// <returns></returns>
        private int GetTimeInterval()
        {
            return Convert.ToInt32(DateTime.Now.Subtract(_lastUpdateTime).TotalSeconds);
        }
        /// <summary>
        /// 获取时间间隔
        /// </summary>
        /// <returns></returns>
        private int GetMillisecondsTimeInterval()
        {
            return DateTime.Now.Subtract(_lastUpdateTime).Seconds * 1000 + DateTime.Now.Subtract(_lastUpdateTime).Milliseconds;
        }
        /// <summary>
        /// 调用endinvoke https://stackoverflow.com/questions/15998934/does-begininvoke-always-need-endinvoke-for-threadpool-threads
        /// </summary>
        /// <param name="iasync"></param>
        private void CallBackEmpty(IAsyncResult iasync)
        {
            if (iasync != null)
            {
                string typeName = "";
                try
                {
                    //System.Runtime.Remoting.Messaging.AsyncResult aresult =
                    //    (System.Runtime.Remoting.Messaging.AsyncResult)iasync;
                    //object action1 = aresult.AsyncDelegate;
                    //Type actionType = action1.GetType();
                    //typeName = actionType.ToString();

                    //if (action1 != null)
                    //{
                    //    //action1.EndInvoke(iasync);
                    //    actionType.InvokeMember("EndInvoke",
                    //        System.Reflection.BindingFlags.InvokeMethod, null,
                    //        action1, new object[] { iasync });
                    //}
                }
                catch (Exception ex)
                {
                    string msg = "CallBackEmpty; for type: " + typeName +
                        " ;Exception: " + ex.ToString();
                    LogHelper.Error(msg, "BeginInvoke");
                }
            }
        }
        /// <summary>
        /// 调用endinvoke https://stackoverflow.com/questions/15998934/does-begininvoke-always-need-endinvoke-for-threadpool-threads
        /// </summary>
        /// <param name="iasync"></param>
        private void CallBackEmpty_easy(IAsyncResult iasync)
        {
            string typeName = "";
            if (iasync != null)
            {
                try
                {
                    MyValueChanged action1 = (MyValueChanged)iasync.AsyncState;
                    Type actionType = action1.GetType();
                    typeName = actionType.ToString();
                    action1.EndInvoke(iasync);
                }
                catch (Exception ex)
                {
                    string msg = "CallBackEmpty; for type: " + typeName +
                         " ;Exception: " + ex.ToString();
                    LogHelper.Error(msg, "BeginInvoke");
                }
            }
        }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var data = new ClassVarEvent<T>();
            data.OldValue = this.OldValue;
            data.myValue = this.myValue;
            data._lastUpdateTime = this._lastUpdateTime;
            data.Index = this.Index;
            return data;
        }
    }
}
