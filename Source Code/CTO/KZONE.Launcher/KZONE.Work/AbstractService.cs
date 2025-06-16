using Spring.Context;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.Log;
using KZONE.MessageManager;
using KZONE.ConstantParameter;

namespace KZONE.Work
{
    public abstract class AbstractService : IService, IApplicationContextAware
    {
        protected string _logName;

        protected IQueueManager _queueManager;

        protected IApplicationContext _applicationContext;

        protected ILogManager Logger = NLogManager.Logger;

        protected TimerManager _timerManager;

        protected ParameterManager _parameterManager;

        protected ConstantManager _constManager;

        public ParameterManager ParameterManager
        {
            get
            {
                return this._parameterManager;
            }
            set
            {
                this._parameterManager = value;
            }
        }

        public ConstantManager ConstantManager
        {
            get
            {
                return this._constManager;
            }
            set
            {
                this._constManager = value;
            }
        }

        public TimerManager Timermanager
        {
            get
            {
                return this._timerManager;
            }
            set
            {
                this._timerManager = value;
            }
        }

        public string LogName
        {
            get
            {
                return this._logName;
            }
            set
            {
                this._logName = value;
            }
        }

        public string ServerName
        {
            get
            {
                return Workbench.ServerName;
            }
        }

        public IQueueManager QueueManager
        {
            get
            {
                return this._queueManager;
            }
            set
            {
                this._queueManager = value;
            }
        }

        public IApplicationContext ApplicationContext
        {
            set
            {
                this._applicationContext = value;
            }
        }

        public abstract bool Init();

        public object GetObject(string name)
        {
            object result;
            try
            {
                object obj = this._applicationContext.GetObject(name);
                result = obj;
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
                result = null;
            }
            return result;
        }

        public IServerAgent GetServerAgent(string agentName)
        {
            return this.GetObject(agentName) as IServerAgent;
        }

        public bool PutMessage(xMessage msg)
        {
            bool result;
            try
            {
                this.QueueManager.PutMessage(msg);
                result = true;
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.ToString());
                result = false;
            }
            return result;
        }

        public object Invoke(string className, string methodName, object[] @params)
        {
            try
            {
                object obj = this.GetObject(className);
                if (obj != null)
                {
                    Type[] typeList = this.GetParameterType(@params);
                    MethodInfo mi = obj.GetType().GetMethod(methodName, typeList);
                    object result;
                    if (mi == null)
                    {
                        this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Methed {0} is not exist in class {1}.", methodName, obj.GetType().ToString()));
                        result = null;
                        return result;
                    }
                    result = mi.Invoke(obj, @params);
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return null;
        }

        public object Invoke(string className, string methodName, object[] @params, Type[] types)
        {
            try
            {
                object obj = this.GetObject(className);
                if (obj != null)
                {
                    MethodInfo mi = obj.GetType().GetMethod(methodName, types);
                    object result;
                    if (mi == null)
                    {
                        this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Methed {0} is not exist in class {1}.", methodName, obj.GetType().ToString()));
                        result = null;
                        return result;
                    }
                    result = mi.Invoke(obj, @params);
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return null;
        }

        private Type[] GetParameterType(object[] param)
        {
            Type[] typeList = new Type[param.Length];
            for (int i = 0; i < param.Length; i++)
            {
                typeList[i] = param[i].GetType();
            }
            return typeList;
        }

        public IAsyncResult BeginInvoke(string className, string methodName, object[] @params, out MessageHandler handler)
        {
            handler = null;
            try
            {
                object obj = this.GetObject(className);
                if (obj != null)
                {
                    Type[] types = this.GetParameterType(@params);
                    MethodInfo mi = obj.GetType().GetMethod(methodName, types);
                    IAsyncResult result;
                    if (mi == null)
                    {
                        this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Methed {0} is not exist in class {1}.", methodName, obj.GetType().ToString()));
                        result = null;
                        return result;
                    }
                    handler = new MessageHandler(this.Execute);
                    result = handler.BeginInvoke(new object[]
					{
						mi,
						obj,
						@params
					}, null, null);
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return null;
        }

        public IAsyncResult BeginInvoke(string className, string methodName, object[] @params, Type[] types, out MessageHandler handler)
        {
            handler = null;
            try
            {
                object obj = this.GetObject(className);
                if (obj != null)
                {
                    MethodInfo mi = obj.GetType().GetMethod(methodName, types);
                    IAsyncResult result;
                    if (mi == null)
                    {
                        this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Methed {0} is not exist in class {1}.", methodName, obj.GetType().ToString()));
                        result = null;
                        return result;
                    }
                    handler = new MessageHandler(this.Execute);
                    result = handler.BeginInvoke(new object[]
					{
						mi,
						obj,
						@params
					}, null, null);
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return null;
        }

        public object EndInvoke(IAsyncResult ar, ref MessageHandler handler)
        {
            try
            {
                if (handler != null && ar != null)
                {
                    return handler.EndInvoke(ar);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return null;
        }

        private object Execute(object[] Parameters)
        {
            object[] param = Parameters[2] as object[];
            MethodInfo mi = Parameters[0] as MethodInfo;
            return mi.Invoke(Parameters[1], param);
        }

        protected void LogInfo(string methodInfo, string log)
        {
            this.Logger.LogInfoWrite(this.LogName, base.GetType().Name, methodInfo, log);
        }

        protected void LogTrx(string methodInfo, string log)
        {
            this.Logger.LogTrxWrite(this.LogName, log);
        }

        protected void LogDebug(string methodInfo, string log)
        {
            this.Logger.LogDebugWrite(this.LogName, base.GetType().Name, methodInfo, log);
        }

        protected void LogError(string methodInfo, string log)
        {
            this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, methodInfo, log);
        }

        protected void LogError(string methodInfo, Exception ex)
        {
            this.Logger.LogErrorWrite(this.LogName, base.GetType().Name, methodInfo, ex);
        }

        protected void LogWarn(string methodInfo, string log)
        {
            this.Logger.LogWarnWrite(this.LogName, base.GetType().Name, methodInfo, log);
        }

        public string CreateTrxID()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmddssfff");
        }

        public object GetPropertyValue(string className, string propertyName)
        {
            object obj = this.GetObject(className);
            if (obj != null)
            {
                PropertyInfo info = null;
                try
                {
                    info = obj.GetType().GetProperty(propertyName);
                }
                catch (Exception ex)
                {
                    this.LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                }
                object value = null;
                if (info != null)
                {
                    value = info.GetValue(obj, null);
                }
                return value;
            }
            return null;
        }

        public bool SetPropertyValue(string className, string propertyName, object value)
        {
            object obj = this.GetObject(className);
            if (obj != null)
            {
                PropertyInfo info = null;
                try
                {
                    info = obj.GetType().GetProperty(propertyName);
                }
                catch (Exception ex)
                {
                    this.LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                    bool result = false;
                    return result;
                }
                if (value != null)
                {
                    try
                    {
                        info.SetValue(obj, value, null);
                    }
                    catch (Exception ex2)
                    {
                        this.LogError(MethodBase.GetCurrentMethod().Name + "()", ex2);
                        bool result = false;
                        return result;
                    }
                    return true;
                }
                return false;
            }
            return false;
        }
    }

    public  class AbstractMethod : AbstractService
    {
        
        private volatile static AbstractMethod _instance = null;
        private static readonly object lockHelper = new object();
       // private AbstractMethod() { }
        public override bool Init()
        {
            return true;
        }
        public static AbstractMethod CreateInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new AbstractMethod();
                        _instance._applicationContext = Workbench._applicationContext;
                      
                }
            }
          
           return _instance;
        }
    }
}
