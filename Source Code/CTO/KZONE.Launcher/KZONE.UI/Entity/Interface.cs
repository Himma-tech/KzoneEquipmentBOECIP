using System;
using System.Collections.Generic;

namespace KZONE.UI
{
    public class Interface
    {
        public string PipeKey = string.Empty;
        public string UpstreamNodeNo=string.Empty ;
        public string UpstreamUnitNo=string.Empty ;
        public string UpstreamBitAddress = string.Empty;
        public string UpstreamSignal=string.Empty ;
        public string UpstreamSeqNo = string.Empty;
        public Dictionary<string, GlassData> UpstreamJobData;
        //public List<GlassData> UpstreamJobData;
        public string DownstreamNodeNo=string.Empty ;
        public string DownstreamUnitNo=string.Empty ;
        public string DownstreamBitAddress = string.Empty;
        public string DownstreamSignal=string.Empty ;
        public string DownstreamSeqNo = string.Empty;
        public Dictionary<string, GlassData> DownstreamJobData;
        //public List<GlassData> DownstreamJobData;

        public string EQPIO = string.Empty;
        public string PIOBitAddress = string.Empty;

        public DateTime LastReceiveMsgDateTime;   //最後收到訊息的時間
        public bool IsReply = true;  //Trx 是否已經回覆
        public bool IsDisplay = false; //是否為開啟狀態

        public Interface()
        {
            UpstreamJobData = new Dictionary<string, GlassData>();
            DownstreamJobData = new Dictionary<string, GlassData>();

            UpstreamSignal = "0".PadLeft(32, '0');
            DownstreamSignal = "0".PadLeft(32, '0');
            EQPIO = "0".PadLeft(32, '0');

            UpstreamBitAddress = "0000";
            DownstreamBitAddress = "0000";
            PIOBitAddress = "0000";

            IsDisplay = true;
            IsReply = true;

            LastReceiveMsgDateTime = new DateTime();
        }
    }

    public class GlassData
    {
        public string GlassAddress = string.Empty;
        public string CassetteSequenceNumber = string.Empty;
        public string SlotSequenceNumber = string.Empty;
        public string JobID = string.Empty;

        public string GroupNumber = string.Empty;
        public string GlassType = string.Empty;
        public string GlassJudge = string.Empty;
        public string ProcessSkipFlag = string.Empty;
        public string LastGlassFlag = string.Empty;
        public string CIMModeCreate = string.Empty;
        public string SamplingFlag = string.Empty;
        public string Reserved = string.Empty;
        public string InspectionJudgeResult = string.Empty;
        public string InspectionReservationSignal = string.Empty;
        public string ProcessReservationSignal = string.Empty;
        public string TrackingDataHistory = string.Empty;
        public string EquipmentSpecialFlag = string.Empty;
        public string GlassID = string.Empty;
        public string SorterGrade = string.Empty;
        public string GlassGrade = string.Empty;
        public string FromPortNo = string.Empty;
        public string TargetPortNo = string.Empty;
        public string TargetSlotNo = string.Empty;   
        public string TargetCassetteID = string.Empty;
        public string Reserve = string.Empty;
        public string PPID = string.Empty;






    }

    public class LinkSignalType
    {
        public string UpStreamLocalNo = string.Empty;
        public string DownStreamLocalNo = string.Empty;
        public string SeqNo = string.Empty;
        public string LinkType = string.Empty;
        public string TimingChart = string.Empty;
    }

    public class LinkSignalBitDesc
    {
        public Dictionary<int,string> UpStreamBit;
        public Dictionary<int, string> DownStreamBit;

        public LinkSignalBitDesc()
        {
            UpStreamBit = new Dictionary<int, string>();
            DownStreamBit = new Dictionary<int, string>();
        }
    }

    public class LinkSignalBit
    {
        public int SeqNo = 0;
        public string Description = string.Empty;        
    }


}
