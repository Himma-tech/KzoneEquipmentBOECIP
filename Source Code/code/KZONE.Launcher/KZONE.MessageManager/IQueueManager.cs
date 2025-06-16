using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.MessageManager
{
    public interface IQueueManager
    {
        void CreateQueue(string agentName);

        void RemoveQueue(string agentName);

        void PutMessage(xMessage msg);

        xMessage GetMessage(string agentName);

        int GetCount(string agentName);

        void ClearMessage(string agentName);
    }
}
