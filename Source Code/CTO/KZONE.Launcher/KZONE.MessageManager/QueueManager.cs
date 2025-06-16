using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace KZONE.MessageManager
{
    public class QueueManager : IQueueManager
    {
        private Dictionary<string, ConcurrentQueue<xMessage>> _messageQueueList;

        public void Init()
        {
            this._messageQueueList = new Dictionary<string, ConcurrentQueue<xMessage>>();
        }

        public void CreateQueue(string agentName)
        {
            if (string.IsNullOrEmpty(agentName))
            {
                throw new ArgumentNullException();
            }
            if (this._messageQueueList.ContainsKey(agentName))
            {
                throw new Exception(string.Format("Server ID{0} is exist.", agentName));
            }
            ConcurrentQueue<xMessage> queue = new ConcurrentQueue<xMessage>();
            lock (this._messageQueueList)
            {
                this._messageQueueList.Add(agentName, queue);
            }
        }

        public void RemoveQueue(string agentName)
        {
            if (string.IsNullOrEmpty(agentName))
            {
                throw new ArgumentNullException();
            }
            if (!this._messageQueueList.ContainsKey(agentName))
            {
                throw new Exception(string.Format("Server ID{0} is not exist.", agentName));
            }
            lock (this._messageQueueList)
            {
                this._messageQueueList.Remove(agentName);
            }
        }

        public void PutMessage(xMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException();
            }
            if (!this._messageQueueList.ContainsKey(msg.ToAgent))
            {
                throw new Exception(string.Format("Not found {0} Server .", msg.ToAgent));
            }
            this._messageQueueList[msg.ToAgent].Enqueue(msg);
        }

        public xMessage GetMessage(string agentName)
        {
            if (string.IsNullOrEmpty(agentName))
            {
                throw new ArgumentNullException();
            }
            if (!this._messageQueueList.ContainsKey(agentName))
            {
                throw new Exception(string.Format("Not found {0} Server .", agentName));
            }
            xMessage msg = null;
            if (this._messageQueueList[agentName].Count > 0)
            {
                this._messageQueueList[agentName].TryDequeue(out msg);
            }
            return msg;
        }

        public int GetCount(string agentName)
        {
            if (string.IsNullOrEmpty(agentName))
            {
                throw new ArgumentNullException();
            }
            if (!this._messageQueueList.ContainsKey(agentName))
            {
                throw new Exception(string.Format("Server ID{0} is not exist.", agentName));
            }
            int count;
            lock (this._messageQueueList)
            {
                count = this._messageQueueList[agentName].Count;
            }
            return count;
        }

        public void ClearMessage(string agentName)
        {
            if (string.IsNullOrEmpty(agentName))
            {
                throw new ArgumentNullException();
            }
            if (!this._messageQueueList.ContainsKey(agentName))
            {
                throw new Exception(string.Format("Server ID{0} is not exist.", agentName));
            }
            lock (this._messageQueueList)
            {
                this._messageQueueList[agentName] = new ConcurrentQueue<xMessage>();
            }
        }
    }
}
