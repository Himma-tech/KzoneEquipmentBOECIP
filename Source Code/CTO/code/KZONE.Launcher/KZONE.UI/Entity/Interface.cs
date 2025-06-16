using KZONE.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        public string FlowType
        {
            get;
            set;
        }
        public string PathPosition
        {
            get;
            set;
        }
        public Dictionary<string, Dictionary<string, List<string>>> TrxMatch = new Dictionary<string, Dictionary<string, List<string>>>();

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
        public string GlassAddress { get; set; }
        #region B20 JOB DATA A Commmon
        [Category("JobDataA_Common")]
        public string Cassette_Sequence_No { get; set; }
        [Category("JobDataA_Common")]
        public string Job_Sequence_No { get; set; }
        [Category("JobDataA_Common")]
        public string Lot_ID { get; set; }
        [Category("JobDataA_Common")]
        public string Product_ID { get; set; }
        [Category("JobDataA_Common")]
        public string Operation_ID { get; set; }
        [Category("JobDataA_Common")]
        public string GlassID_or_PanelID { get; set; }
        [Category("JobDataA_Common")]
        public string CST_Operation_Mode { get; set; }
        [Category("JobDataA_Common")]
        public string Substrate_Type { get; set; }
        [Category("JobDataA_Common")]
        public string Product_Type { get; set; }
        [Category("JobDataA_Common")]
        public string Job_Type { get; set; }
        [Category("JobDataA_Common")]
        public string Dummy_Type { get; set; }
        [Category("JobDataA_Common")]
        public string Skip_Flag { get; set; }
        [Category("JobDataA_Common")]
        public string Process_Flag { get; set; }
        [Category("JobDataA_Common")]
        public string Process_Reason_Code { get; set; }
        [Category("JobDataA_Common")]
        public string LOT_Code { get; set; }
        [Category("JobDataA_Common")]
        public string Glass_Thickness { get; set; }
        [Category("JobDataA_Common")]
        public string Glass_Degree { get; set; }
        [Category("JobDataA_Common")]
        public string Inspection_Flag { get; set; }
        [Category("JobDataA_Common")]
        public string Job_Judge { get; set; }
        [Category("JobDataA_Common")]
        public string Job_Grade { get; set; }
        [Category("JobDataA_Common")]
        public string Job_Recovery_Flag { get; set; }
        [Category("JobDataA_Common")]
        public string Mode { get; set; }
        [Category("JobDataA_Common")]
        public string Step_ID { get; set; }
        //[Category("JobDataA_Common")]
        //public string VCR_Read_ID { get; set; }
        [Category("JobDataA_Common")]
        public string Master_Recipe_ID { get; set; }


        [Category("JobDataA_Common")]
        public string Reserved1 { get; set; }
        [Category("JobDataA_Common")]
        public string Reserved2 { get; set; }

        #endregion

        #region JobDataA_Array_Special
        [Category("JobDataA_Array_Special")]
        public string Tray_ID { get; set; }
        [Category("JobDataA_Array_Special")]
        public string Tray_Life { get; set; }
        [Category("JobDataA_Array_Special")]
        public string Wait_Over_Flag { get; set; }
        [Category("JobDataA_Array_Special")]
        public string OffSetID1 { get; set; }
        [Category("JobDataA_Array_Special")]
        public string OffSetID2 { get; set; }
        [Category("JobDataA_Array_Special")]
        public string Test_Gap_Glass_Flag { get; set; }
        [Category("JobDataA_Array_Special")]
        public string Read_VCR_ID { get; set; }
        #endregion

        #region JobDataA_Cell_Special
        [Category("JobDataA_Cell_Special")]
        public string PPID01 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID02 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID03 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID04 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID05 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID06 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID07 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID08 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID09 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID10 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID11 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID12 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID13 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID14 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID15 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID16 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID17 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID18 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID19 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID20 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID21 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID22 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID23 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID24 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID25 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID26 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID27 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID28 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID29 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID30 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID31 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID32 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID33 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string PPID34 { get; set; }

        [Category("JobDataA_Cell_Special")]
        public string Pair_GlassID { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Pair_Cassette_Sequence_No { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Pair_Slot_Sequence_No { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Dummy_Use_Count { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Dummy_Rework_Count { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Reprocessing_Flag { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Trouble_Code1 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Trouble_Code2 { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Special_Skip_Flag { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Temp_Destination_Local { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Temp_Destination_Slot { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Inspection_Judge { get; set; }
        
        [Category("JobDataA_Cell_Special")]
        public string Macro_Flag { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Micro_Flag { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string After_Turn_Angle { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string Oven_Over_Bake_Flag { get; set; }
        [Category("JobDataA_Cell_Special")]
        public string USC_Clean_Flag { get; set; }
        #endregion

        #region JobDataA_CF_Special
        [Category("JobDataA_CF_Special")]
        public string Reprocess_Flag { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Reprocess_Count { get; set; }
        [Category("JobDataA_CF_Special")]
        public string AOI_Inspection_Flag { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Target_Port { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Rework_Count { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Line_Number { get; set; }
        [Category("JobDataA_CF_Special")]
        public string EXPO_Process { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Mura_Risk_Flag { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Batch_ID { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Reprocess_Reason_Code { get; set; }
        [Category("JobDataA_CF_Special")]
        public string AOI_Risk_Flag { get; set; }
        [Category("JobDataA_CF_Special")]
        public string AOI_First_Flag { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Inspection_Judge_Data { get; set; }
        //后续新增
        [Category("JobDataA_CF_Special")]
        public string NG_Judge_Flag { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Glass_Special_Data { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Glass_Corner { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Glass_fast_or_last_Sample_Flag { get; set; }
        [Category("JobDataA_CF_Special")]
        public string Offset_Group { get; set; }
        #endregion


        #region JobDataA_Sepcial_Dumplication
        [Category("Array_CF")]
        public string PPID { get; set; }
        [Category("Cell_CF")]
        public string Special_Process_Flag { get; set; }

        //原CELL独占
        [Category("Cell_CF")]
        public string Q_Time1 { get; set; }
        [Category("Cell_CF")]
        public string Q_Time2 { get; set; }
        [Category("Cell_CF")]
        public string Q_Time3 { get; set; }
        [Category("Cell_CF")]
        public string Q_Time_Over_Flag { get; set; }
        #endregion

    }

    public class LinkSignalType
    {
        public string UpStreamLocalNo = string.Empty;
        public string DownStreamLocalNo = string.Empty;
        public string SeqNo = string.Empty;
        public string LinkType = string.Empty;
        public string TimingChart = string.Empty;
        public string FlowType = string.Empty;
        public string PathPosition = string.Empty;
        public string LinkKey = string.Empty;
        public Dictionary<string, Dictionary<string, List<string>>> TrxMatch = new Dictionary<string, Dictionary<string, List<string>>>();
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
