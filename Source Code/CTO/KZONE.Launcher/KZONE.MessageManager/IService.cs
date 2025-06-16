using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.MessageManager
{
    public interface IService
    {
        string LogName
        {
            get;
            set;
        }

        string ServerName
        {
            get;
        }

        IQueueManager QueueManager
        {
            set;
        }

        bool Init();

        IServerAgent GetServerAgent(string agentName);

        bool PutMessage(xMessage msg);

        object Invoke(string className, string methodName, object[] @params);

        IAsyncResult BeginInvoke(string className, string methodName, object[] @params, out MessageHandler handler);

        object EndInvoke(IAsyncResult ar, ref MessageHandler handler);

        object GetPropertyValue(string className, string PropertyName);

        bool SetPropertyValue(string className, string PropertyName, object value);
    }
}
