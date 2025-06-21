using Himma.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Himma.Common.Service.Base
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public abstract class BaseService
    {
        public readonly string ServiceName;
        public readonly bool EnabledCached;
        public List<Action> _cachedActions;

        private static readonly Lazy<System.Threading.Tasks.TaskFactory> _globalTaskFactory = new Lazy<System.Threading.Tasks.TaskFactory>(() => new System.Threading.Tasks.TaskFactory());


        public BaseService()
        {

        }

        public BaseService(bool iscached)
        {
            EnabledCached = iscached;
        }

        public BaseService(bool iscached, string serviceName) : this(iscached)
        {
            ServiceName = serviceName;
        }
        private int _errorSleep = 1000;

        public virtual void Cache()
        {

        }

        public void Cache(string key, object value)
        {
            CacheManager.Save(key, value);
        }

        private string ExecuteAction(Action func, int tryCountMax, int currentTry = 0)
        {
            try
            {
                func();
                return null;
            }
            catch (Exception ex)
            {
                currentTry++;
                if (currentTry <= tryCountMax)
                {
                    Thread.Sleep(_errorSleep);
                    return ExecuteAction(func, tryCountMax, currentTry);
                }
                else return ex.ToString();
            }
        }


        protected virtual Task<bool> ExecuteFuncAsync(Action func, Action<Exception> onError, int maxErrorCount = 0)
        {

            return Task.Run(() => {
                var msg = ExecuteAction(func, maxErrorCount);
                var prevMsg = maxErrorCount > 0 ? $"发生异常，已重试：{maxErrorCount}次，" : "发生异常：";
                if (msg != null)
                    onError(new Exception(prevMsg + msg));
                return msg == null;
            });
        }

        protected virtual bool ExecuteFunc(Action func, Action<Exception> onError, int maxErrorCount = 0)
        {
            var msg = ExecuteAction(func, maxErrorCount);
            var prevMsg = maxErrorCount > 0 ? $"发生异常，已重试：{maxErrorCount}次，" : "发生异常：";
            if (msg != null)
                onError(new Exception(prevMsg + msg));
            return msg == null;
        }

        protected virtual ResultModel ExecuteFunc(Func<string> func)
        {
            try
            {
                var msg = func();
                return new ResultModel { Message = msg, IsSuccess = string.IsNullOrEmpty(msg) };
            }
            catch (Exception ex)
            {
                return new ResultModel { Message = $"发生异常: " + ex };
            }

        }

        protected void StartThread(Action act, bool IsBackground = true)
        {
            //var thread = new Thread(new ThreadStart(act));
            //thread.IsBackground = IsBackground;
            //thread.Start();
            if (IsBackground)
            {
                _globalTaskFactory.Value.StartNew(act, TaskCreationOptions.LongRunning);
            }
            else
            {
                _globalTaskFactory.Value.StartNew(act);
            }

        }
        protected void StartThread(Action<object> act, object parm, bool IsBackground = true)
        {
            //var thread = new Thread(x => act(x));
            //thread.IsBackground = IsBackground;
            //thread.Start(parm);
            if (IsBackground)
            {
                _globalTaskFactory.Value.StartNew(() => act(parm), TaskCreationOptions.LongRunning)
              .ContinueWith(task =>
              {
                  // 处理可能的异常，具体操作可以根据需求进行调整
                  if (task.Exception != null)
                  {
                      Console.WriteLine($"Exception: {task.Exception}");
                  }
              }, TaskContinuationOptions.OnlyOnFaulted);
            }
            else
            {
                _globalTaskFactory.Value.StartNew(() => act(parm))
              .ContinueWith(task =>
              {
                  // 处理可能的异常，具体操作可以根据需求进行调整
                  if (task.Exception != null)
                  {
                      Console.WriteLine($"Exception: {task.Exception}");
                  }
              }, TaskContinuationOptions.OnlyOnFaulted);
            }

        }

        protected virtual string ExecuteFunc(Action func, int maxTry = 0)
        {
            var msg = ExecuteAction(func, maxTry);
            var prevMsg = maxTry > 0 ? $"发生异常，已重试：{maxTry}次，" : "发生异常：";
            if (!string.IsNullOrEmpty(msg)) msg = prevMsg + msg;
            return msg;
        }
    }
}
