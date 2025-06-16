using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KZONE.MessageManager
{
    public interface IServerAgent
    {
        string Name
        {
            get;
            set;
        }

        string ConfigFileName
        {
            get;
            set;
        }

        string FormatFileName
        {
            get;
            set;
        }

        IConfiguration Configuration
        {
            get;
        }

        IMessageManager MessageManager
        {
            set;
        }

        IQueueManager QueueManager
        {
            set;
        }

        object Tag
        {
            get;
            set;
        }

        bool IsEnabled
        {
            get;
            set;
        }

        eAgentStatus AgentStatus
        {
            get;
        }

        IDictionary<string, object> RuntimeInfo
        {
            get;
        }

        string ConnectedState
        {
            get;
        }

        string MessageToken
        {
            get;
            set;
        }

        bool Init();

        bool Start();

        bool Stop();

        bool Reset();

        void Destroy();

        object GetTransactionFormat(string trxName);

        bool ContainsTransaction(string trxName);

        IEnumerable<string> GetTransactionKey();

        IEnumerable<object> GetTransactionValue();
    }
}
