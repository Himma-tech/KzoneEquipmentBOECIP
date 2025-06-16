using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    [Serializable]
    public class CIMMessage : EntityData
    {

        public string EquipmentNo { get; set; }
        public string Message { get; set; }
        public string MessageStatus { get; set; }
        public DateTime OccurDateTime { get; set; }
        public bool IsSend { get; set; }
        public bool IsFinish { get; set; }
        public string TrxID { get; set; }
        public string OperatorId { get; set; }

        public CIMMessage(string  nodeNo,string msg,string state,string operatorId,bool IsSend=false,bool isFinish=false)
        {
            
            EquipmentNo = nodeNo;
            Message = msg;
            this.IsFinish = isFinish;
            this.IsSend = IsSend;
            OccurDateTime = DateTime.Now;
            TrxID = OccurDateTime.ToString("yyyyMMddHHmmssfff");
            OperatorId = operatorId;
        }

    }
}
