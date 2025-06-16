using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using KZONE.Log;

namespace KZONE.MessageManager
{
    public class Partaker
    {
        private IService _service;

        private string _methodName;

        private string _objectId;

        private bool _isFirst;

        private MethodInfo _methodInfo;

        private object _syncObject = new object();

        private object _syncObject2 = new object();

        private bool _enabled;

        private long _runCount;

        private long _minimum;

        private long _maximum;

        public long Minimum
        {
            get
            {
                return this._minimum;
            }
            set
            {
                this._minimum = value;
            }
        }

        public long Maximum
        {
            get
            {
                return this._maximum;
            }
            set
            {
                this._maximum = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return this._enabled;
            }
        }

        public bool IsFirst
        {
            get
            {
                return this._isFirst;
            }
        }

        public long RunCount
        {
            get
            {
                return this._runCount;
            }
            set
            {
                if (value >= 9223372036854775807L)
                {
                    value = 0L;
                }
                this._runCount = value;
            }
        }

        public string ObjectId
        {
            get
            {
                return this._objectId;
            }
            set
            {
                this._objectId = value;
            }
        }

        public string MethodName
        {
            get
            {
                return this._methodName;
            }
            set
            {
                this._methodName = value;
            }
        }

        public IService Service
        {
            get
            {
                return this._service;
            }
            set
            {
                this._service = value;
            }
        }

        public Partaker(string objectId, string m)
        {
            this._objectId = objectId;
            this._methodName = m;
            this._isFirst = true;
            this._methodInfo = null;
            this._enabled = true;
        }

        public bool BeginInvoke(object[] param)
        {
            if (!this._enabled)
            {
                return false;
            }
            if (this._service == null)
            {
                throw new KZONEException("Handler  is not create.");
            }
            bool result;
            lock (this._syncObject)
            {
                if (this._isFirst)
                {
                    MethodInfo mi = this._service.GetType().GetMethod(this._methodName);
                    this._isFirst = false;
                    if (mi == null)
                    {
                        throw new KZONEException(string.Format("Methed {0} is not exist in class {1}.", this._methodName, this._service.GetType().ToString()));
                    }
                    this._methodInfo = mi;
                }
                if (this._methodInfo == null)
                {
                    throw new KZONEException(string.Format("Methed {0} is not exist in class {1}.", this._methodName, this._service.GetType().ToString()));
                }
                result = ThreadPool.QueueUserWorkItem(new WaitCallback(this.Execute), new object[]
				{
					this._methodInfo,
					param
				});
            }
            return result;
        }

        public object Invoke(object[] param)
        {
            if (this._service == null)
            {
                throw new KZONEException("Handler  is not create.");
            }
            MethodInfo mi = this._service.GetType().GetMethod(this._methodName);
            if (mi == null)
            {
                throw new KZONEException(string.Format("Methed {0} is not exist in class {1}.", this._methodName, this._service.GetType().ToString()));
            }
            return mi.Invoke(this._service, param);
        }

        private void Execute(object o)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string method = string.Empty;
            int param_count = 0;
            try
            {
                object[] objects = o as object[];
                object[] param = objects[1] as object[];
                MethodInfo mi = objects[0] as MethodInfo;
                method = mi.Name;
                param_count = ((param != null) ? param.Length : 0);
                mi.Invoke(this._service, param);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Method'{0}' ParamCount'{1}'", method, param_count));
                NLogManager.Logger.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            finally
            {
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds < 100L)
                {
                    NLogManager.Logger.LogDebugWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Execute {0} Method Spect Time {1} ms.", this._methodName, stopwatch.ElapsedMilliseconds));
                }
                else
                {
                    NLogManager.Logger.LogDebugWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Execute {0} Method Spect Time {1} ms > 100 ms. ", this._methodName, stopwatch.ElapsedMilliseconds));
                }
                lock (this._syncObject2)
                {
                    this.RunCount += 1L;
                    if (this.Minimum == 0L && this.Maximum == 0L)
                    {
                        this.Minimum = stopwatch.ElapsedMilliseconds;
                        this.Maximum = stopwatch.ElapsedMilliseconds;
                    }
                    else if (this.Minimum >= stopwatch.ElapsedMilliseconds)
                    {
                        this.Minimum = stopwatch.ElapsedMilliseconds;
                    }
                    else if (this.Maximum <= stopwatch.ElapsedMilliseconds)
                    {
                        this.Maximum = stopwatch.ElapsedMilliseconds;
                    }
                }
            }
        }

        private void Execute(object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this._methodInfo.Invoke(this._service, param);
            stopwatch.Stop();
            NLogManager.Logger.LogDebugWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("Execute {0} Method Spect Time {1}ms.", this._methodName, stopwatch.ElapsedMilliseconds));
        }

        public void Enable()
        {
            this._enabled = true;
        }

        public void Disable()
        {
            this._enabled = false;
        }
    }
}
