///define Constant 


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    public class eAgentName
    {
        public const string PLCAgent = "PLCAgent";
        public const string DBAgent = "DBAgent";
        public const string TRVAgent = "TRVAgent";
        public const string SECSAgent = "SECSAgent";
        public const string MESAgent = "MESAgent";
        public const string OEEAgent = "OEEAgent";
        public const string EDAAgent = "EDAAgent";
        public const string OPIAgent = "OPIAgent";
        public const string APCAgent = "APCAgent";
        public const string SerialPortAgent = "SerialAgent";
        public const string ActiveSocketAgent = "ActiveSocketAgent";
        public const string PassiveSocketAgent = "PassiveSocketAgent";
    }
    public class eServiceName
    {
        public const string LineService = "LineService";
        public const string FDCService = "FDCService";
        public const string MESService = "MESService";
        public const string BCSSerivice = "BCSService";

        public const string EDAService = "EDAService";
        public const string EquipmentService = "EquipmentService";
        
        public const string DateTimeService = "DateTimeService";
        public const string UIService = "UIService";
        public const string ArraySpecialService = "ArraySpecialService";
        public const string CFSpecialService = "CFSpecialService";
        public const string CELLSpecialService = "CELLSpecialService";
        public const string MaterialService = "MaterialService";
        public const string AlarmService = "AlarmService";
        public const string RecipeService = "RecipeService";
        public const string PortService = "PortService";
        public const string CassetteService = "CassetteService";
        public const string JobService = "JobService";
        public const string CIMMessageService = "CIMMessageService";
        public const string SubBlockService = "SubBlockService";
        public const string VCRService = "VCRService";
        public const string ProcessDataService = "ProcessDataService";
        public const string DailyCheckService = "DailyCheckService";
        public const string DenseBoxService = "DenseBoxService";
        public const string PalletService = "PalletService";
        public const string ActiveSocketService = "ActiveSocketService";
        public const string PassiveSocketService = "PassiveSocketService";
        public const string HSMSService = "HSMSService";
        public const string DenseBoxCassetteService = "DenseBoxCassetteService";
        public const string DenseBoxPortService = "DenseBoxPortService";
        public const string RobotService = "RobotService";
        public const string EvisorService = "EvisorService";   



    }
    public class eParameterName
    {
      
        public const string RecipeParameterBCSTimeout = "RECIPEPARAMETERBCSTIMEOUT";
      
        public const string T1 = "T1";
        public const string T2 = "T2";
        public const string T3 = "T3";
        public const string T4 = "T4";
        public const string Tn = "Tn";

        public const string ReceiveTimer = "ReceiveTimer";
        public const string SendTimer = "SendTimer";
        public const string SendInterval = "SendInterval";
        public const string SendWaitTimer = "SendWaitTimer";


        public const string EventDelayTime = "EVENTDELAYTIME";
        public const string LinkSignalT1 = "LinkSignalT1";
        public const string LinkSignalT2 = "LinkSignalT2";
        public const string LinkSignalT3 = "LinkSignalT3";
        public const string LinkSignalT4 = "LinkSignalT4";
        public const string LinkSignalT5 = "LinkSignalT5";

        public const string TR01_02_TransferRequestInterval = "TR01_02_TransferRequestInterval";
        public const string TR01_02_TransferRequestTime = "TR01_02_TransferRequestTime";

        public const string MESTIMEOUT = "MESTIMEOUT";
        public const string SyncTime = "SYNCTIME";
        public const string SECSDATATIMEOUT = "SECSDATATIMEOUT";
        public const string NGMarkNotToEIT = "NGMARKNOTTOEIT";
        public const string NGMarkNotToFMA1 = "NGMARKNOTTOFMA1";
        public const string NGMarkNotToFMA2 = "NGMARKNOTTOFMA2";
        public const string NGMarkNotToAOI = "NGMARKNOTTOAOI";
        public const string NGMarkNeedToFMA1 = "NGMARKNEEDTOFMA1";
        public const string NGMarkNeedToFMA2 = "NGMARKNEEDTOFMA2";
        public const string NGMarkNeedToEIT = "NGMARKNEEDTOEIT";
        public const string NGMarkNeedToAOI = "NGMARKNEEDTOAOI";
        public const string NGMarkNotToNGOrRework = "NGMARKNOTTONGORREWOK";
        public const string CheckGlassSamplingFlag = "CheckGlassSamplingFlag";
    }
    public class eREPORT_SWITCH
    {
        public const string REMOTE_RECIPE_ID = "S_REMOTE_RECIPE_ID_CHECK";
        public const string LOCAL_RECIPE_ID = "S_LOCAL_RECIPE_ID_CHECK";

        public const string REMOTE_RECIPE_PARAMETER = "S_REMOTE_RECIPE_PARAMETER_CHECK";
        public const string LOCAL_RECIPE_PARAMETER = "S_LOCAL_RECIPE_PARAMETER_CHECK";

        public const string RECORD_OPI_STATUS_LOG = "S_RECORD_OPI_STATUS_LOG";
    }
    public class eRecoveryMode
    {
        public const string NormallyToRecovery = "1";
        public const string RecoveryToNormally = "2";
        public const string NormallyToPMRecovery = "3";
        public const string PMRecoveryToNormally = "4";
        public const string NormalToAOI = "5";
        public const string AOITONormal = "6";
    }

    public class ArithmeticOperator
    {
        public const string Empty = " ";
        public const string PlusSign = "+";
        public const string MinusSign = "-";
        public const string TimesSign = "*";
        public const string DivisionSign = "/";
    }

    [Serializable]
    public class CassetteMapException : Exception
    {
        private string _eqpNo = string.Empty;
        private string _portNo = string.Empty;

        public string EQPNo {
            get { return _eqpNo; }
            set { _eqpNo = value; }
        }

        public string PortNo {
            get { return _portNo; }
            set { _portNo = value; }
        }
        public CassetteMapException(string eqpNo, string portNo, string aMessage)
            : base(aMessage) {
            _eqpNo = eqpNo;
            _portNo = portNo;
        }
    }
   

    public class eLINE_STATUS
    {
        public const string DOWN = "DOWN";

        public const string RUN = "RUN";

        public const string IDLE = "IDLE";

        public eLINE_STATUS() {
        }
    }

    public class eJOBDATA
    {

        #region  B20 PLC DATA    
        [Category("JobDataA_Common")]
        public const string Cassette_Sequence_No = "Cassette_Sequence_No";
        [Category("JobDataA_Common")]
        public const string Job_Sequence_No = "Job_Sequence_No";
        [Category("JobDataA_Common")]
        public const string Lot_ID = "Lot_ID";
        [Category("JobDataA_Common")]
        public const string Product_ID = "Product_ID";
        [Category("JobDataA_Common")]
        public const string Operation_ID = "Operation_ID";
        [Category("JobDataA_Common")]
        public const string GlassID_or_PanelID = "GlassID_or_PanelID";
        [Category("JobDataA_Common")]
        public const string CST_Operation_Mode = "CST_Operation_Mode";
        [Category("JobDataA_Common")]
        public const string Substrate_Type = "Substrate_Type";
        [Category("JobDataA_Common")]
        public const string Product_Type = "Product_Type";
        [Category("JobDataA_Common")]
        public const string Job_Type = "Job_Type";
        [Category("JobDataA_Common")]
        public const string Dummy_Type = "Dummy_Type";
        [Category("JobDataA_Common")]
        public const string Skip_Flag = "Skip_Flag";
        [Category("JobDataA_Common")]
        public const string Process_Flag = "Process_Flag";
        [Category("JobDataA_Common")]
        public const string Process_Reason_Code = "Process_Reason_Code";
        [Category("JobDataA_Common")]
        public const string LOT_Code = "LOT_Code";
        [Category("JobDataA_Common")]
        public const string Glass_Thickness = "Glass_Thickness";
        [Category("JobDataA_Common")]
        public const string Glass_Degree = "Glass_Degree";
        [Category("JobDataA_Common")]
        public const string Inspection_Flag = "Inspection_Flag";
        [Category("JobDataA_Common")]
        public const string Job_Judge = "Job_Judge";
        [Category("JobDataA_Common")]
        public const string Job_Grade = "Job_Grade";
        [Category("JobDataA_Common")]
        public const string Job_Recovery_Flag = "Job_Recovery_Flag";
        [Category("JobDataA_Common")]
        public const string Mode = "Mode";
        [Category("JobDataA_Common")]
        public const string Step_ID = "Step_ID";
        [Category("JobDataA_Common")]
        public const string VCR_Read_ID = "VCR_Read_ID";
        [Category("JobDataA_Common")]
        public const string Master_Recipe_ID = "Master_Recipe_ID";
        [Category("JobDataA_Common")]
        public const string Reserved1 = "Reserved1";
        [Category("JobDataA_Common")]
        public const string Reserved2 = "Reserved2";


        public const string UnitNo = "UnitNo";
        #endregion

        #region Array Job Data Special
        [Category("JobDataA_Array_Special")]
        public const string Tray_ID = "Tray_ID";
        [Category("JobDataA_Array_Special")]
        public const string Tray_Life = "Tray_Life";
        [Category("JobDataA_Array_Special")]
        public const string Wait_Over_Flag = "Wait_Over_Flag";
        [Category("JobDataA_Array_Special")]
        public const string OffSetID1 = "OffSetID1";
        [Category("JobDataA_Array_Special")]
        public const string OffSetID2 = "OffSetID2";
        [Category("JobDataA_Array_Special")]
        public const string Test_Gap_Glass_Flag = "Test_Gap_Glass_Flag";
        [Category("JobDataA_Array_Special")]
        public const string Read_VCR_ID = "Read_VCR_ID";


        #endregion

        #region CF Job Data Special
        [Category("JobDataA_CF_Special")]
        public const string Reprocess_Flag = "Reprocess_Flag";
        [Category("JobDataA_CF_Special")]
        public const string Reprocess_Count = "Reprocess_Count";
        [Category("JobDataA_CF_Special")]
        public const string AOI_Inspection_Flag = "AOI_Inspection_Flag";
        [Category("JobDataA_CF_Special")]
        public const string Target_Port = "Target_Port";
        [Category("JobDataA_CF_Special")]
        public const string Rework_Count = "Rework_Count";
        [Category("JobDataA_CF_Special")]
        public const string Line_Number = "Line_Number";
        [Category("JobDataA_CF_Special")]
        public const string EXPO_Process = "EXPO_Process";
        [Category("JobDataA_CF_Special")]
        public const string Mura_Risk_Flag = "Mura_Risk_Flag";
        [Category("JobDataA_CF_Special")]
        public const string Batch_ID = "Batch_ID";
        [Category("JobDataA_CF_Special")]
        public const string Reprocess_Reason_Code = "Reprocess_Reason_Code";
        [Category("JobDataA_CF_Special")]
        public const string AOI_Risk_Flag = "AOI_Risk_Flag";
        [Category("JobDataA_CF_Special")]
        public const string AOI_First_Flag = "AOI_First_Flag";
        [Category("JobDataA_CF_Special")]
        public const string Inspection_Judge_Data = "Inspection_Judge_Data";

        #endregion

        #region Cell Job Data Special 
        public const string PPID01 = "PPID01";
        [Category("JobDataA_Cell_Special")]
        public const string PPID02 = "PPID02";
        [Category("JobDataA_Cell_Special")]
        public const string PPID03 = "PPID03";
        [Category("JobDataA_Cell_Special")]
        public const string PPID04 = "PPID04";
        [Category("JobDataA_Cell_Special")]
        public const string PPID05 = "PPID05";
        [Category("JobDataA_Cell_Special")]
        public const string PPID06 = "PPID06";
        [Category("JobDataA_Cell_Special")]
        public const string PPID07 = "PPID07";
        [Category("JobDataA_Cell_Special")]
        public const string PPID08 = "PPID08";
        [Category("JobDataA_Cell_Special")]
        public const string PPID09 = "PPID09";
        [Category("JobDataA_Cell_Special")]
        public const string PPID10 = "PPID10";
        [Category("JobDataA_Cell_Special")]
        public const string PPID11 = "PPID11";
        [Category("JobDataA_Cell_Special")]
        public const string PPID12 = "PPID12";
        [Category("JobDataA_Cell_Special")]
        public const string PPID13 = "PPID13";
        [Category("JobDataA_Cell_Special")]
        public const string PPID14 = "PPID14";
        [Category("JobDataA_Cell_Special")]
        public const string PPID15 = "PPID15";
        [Category("JobDataA_Cell_Special")]
        public const string PPID16 = "PPID16";
        [Category("JobDataA_Cell_Special")]
        public const string PPID17 = "PPID17";
        [Category("JobDataA_Cell_Special")]
        public const string PPID18 = "PPID18";
        [Category("JobDataA_Cell_Special")]
        public const string PPID19 = "PPID19";
        [Category("JobDataA_Cell_Special")]
        public const string PPID20 = "PPID20";
        [Category("JobDataA_Cell_Special")]
        public const string PPID21 = "PPID21";
        [Category("JobDataA_Cell_Special")]
        public const string PPID22 = "PPID22";
        [Category("JobDataA_Cell_Special")]
        public const string PPID23 = "PPID23";
        [Category("JobDataA_Cell_Special")]
        public const string PPID24 = "PPID24";
        [Category("JobDataA_Cell_Special")]
        public const string PPID25 = "PPID25";
        [Category("JobDataA_Cell_Special")]
        public const string PPID26 = "PPID26";
        [Category("JobDataA_Cell_Special")]
        public const string PPID27 = "PPID27";
        [Category("JobDataA_Cell_Special")]
        public const string PPID28 = "PPID28";
        [Category("JobDataA_Cell_Special")]
        public const string PPID29 = "PPID29";
        [Category("JobDataA_Cell_Special")]
        public const string PPID30 = "PPID30";
        [Category("JobDataA_Cell_Special")]
        public const string PPID31 = "PPID31";
        [Category("JobDataA_Cell_Special")]
        public const string PPID32 = "PPID32";
        [Category("JobDataA_Cell_Special")]
        public const string PPID33 = "PPID33";

        [Category("JobDataA_Cell_Special")]
        public const string Pair_GlassID = "Pair_GlassID";
        [Category("JobDataA_Cell_Special")]
        public const string Pair_Cassette_Sequence_No = "Pair_Cassette_Sequence_No";
        [Category("JobDataA_Cell_Special")]
        public const string Pair_Slot_Sequence_No = "Pair_Slot_Sequence_No";
        [Category("JobDataA_Cell_Special")]
        public const string Dummy_Use_Count = "Dummy_Use_Count";
        [Category("JobDataA_Cell_Special")]
        public const string Dummy_Rework_Count = "Dummy_Rework_Count";
        [Category("JobDataA_Cell_Special")]
        public const string Reprocessing_Flag = "Reprocessing_Flag";
        [Category("JobDataA_Cell_Special")]
        public const string Trouble_Code1 = "Trouble_Code1";
        [Category("JobDataA_Cell_Special")]
        public const string Trouble_Code2 = "Trouble_Code2";
        [Category("JobDataA_Cell_Special")]
        public const string Special_Skip_Flag = "Special_Skip_Flag";
        [Category("JobDataA_Cell_Special")]
        public const string Temp_Destination_Local = "Temp_Destination_Local";
        [Category("JobDataA_Cell_Special")]
        public const string Temp_Destination_Slot = "Temp_Destination_Slot";
        [Category("JobDataA_Cell_Special")]
        public const string Inspection_Judge = "Inspection_Judge";
        [Category("JobDataA_Cell_Special")]
        public const string Q_Time1 = "Q_Time1";
        [Category("JobDataA_Cell_Special")]
        public const string Q_Time2 = "Q_Time2";
        [Category("JobDataA_Cell_Special")]
        public const string Q_Time3 = "Q_Time3";
        [Category("JobDataA_Cell_Special")]
        public const string Q_Time_Over_Flag = "Q_Time_Over_Flag";
        [Category("JobDataA_Cell_Special")]
        public const string Macro_Flag = "Macro_Flag";
        [Category("JobDataA_Cell_Special")]
        public const string Micro_Flag = "Micro_Flag";

        #endregion

        #region JobDataA_Sepcial_Dumplication
        [Category("Array_CF")]
        public const string PPID = "PPID";
        [Category("Cell_CF")]
        public const string Special_Process_Flag = "Special_Process_Flag";
        #endregion
    }

    public class eFetchStoreContent
    {
        public const string UnitOrPort = "UnitOrPort";
        public const string UnitNo = "UnitNo";
        public const string PortNo = "PortNo";
        public const string SlotNo = "SlotNo";
    }
    public class eRemoveRecoveryContent
    {
        public const string OperatorID = "OperatorID";
        public const string ReportFlag = "ReportFlag";
        public const string UnitOrPort = "UnitOrPort";
        public const string UnitNo = "UnitNo";
        public const string PortNo = "PortNo";
        public const string SlotNo = "SlotNo";
    }

    public class eCreateModifyContent
    {
        public const string OperatorID = "OperatorID";
        public const string CreateModifyFlag = "CreateModifyFlag";
    }

    public class eGlassDataRequestContent
    {
        public const string CassetteSequenceNo = "CassetteSequenceNumber";
        public const string JobSequenceNo = "CassetteSlotNumber";
        public const string GlassID = "GlassID";
        public const string UsedFlag = "UsedFlag";
        public const string OperatorID = "OperatorID";
    }

    public class eKeyHost {

        public const string PRODUCTID = "PRODUCTID";
        public const string ECCODE = "ECCODE";
        public const string ROUTEID = "ROUTEID";
        public const string ROUTEVERSION = "ROUTEVERSION";
        public const string OPERATIONID = "OPERATIONID";
        public const string OWNERID = "OWNERID";
        public const string MATERIALPRODUCTID = "MATERIALPRODUCTID";
        public const string ABNORMALFLAG = "ABNORMALFLAG";
        public const string GRADE = "GRADE";
        public const string GROUPID = "GROUPID";
        public const string DESSHOPID = "DESSHOPID";
        
        //20170311 add by MES SPEC 1.36
        public const string CRITERIANUMBER = "CRITERIANUMBER";

        //20170226 add by MES SPEC 1.39
        //return_route_id
        //return_route_ver
        //return_ope_no
        public const string RETURNROUTEID = "RETURNROUTEID";
        public const string RETURNROUTEVER = "RETURNROUTEVER";
        public const string RETURNOPENO = "RETURNOPENO";
        //20170310 add by MES SPEC 1.43
        //Operation Version
        public const string OPERATIONVER = "OPERATIONVER";
    }

    public class eAlarmLevel
    {
        public const string ALARM = "ALARM";

        public const string WARNING = "WARNING";

        public eAlarmLevel()
        {
        }
    }

    public class eMESMessItem
    {
        public const string LINEID = "LineID";
        public const string EQPTID = "EQPT_ID";
        public const string PORTID = "PORTID";
        public const string CASSETTEID = "CASSETTEID";
        public const string CASSETTESEQNO = "CASSETTESEQNO";
        //-----------------For Mask Use----------
        public const string MASKSTATUS = "MASKSTATUS";
        public const string UNITNO = "UNITNO";
        public const string SLOTNO = "SLOTNO";
        //---------------------------------------
        
        //-----------------For Dense Upack Use----------
        public const string MTRLLOTID = "MTRLLOTID";
        //---------------------------------------
        //--------RUN MODE CHANGE AFTER PORTS STATUS--------
        public const string REPORTPORTST = "REPORTPORTST";

        //20170306 add for 紀錄EQP or Indexer Run Mode Changer 紀錄上報給MES的Run Mode以便MES Reply NG時通知EQP
        public const string RUNMODEREPORT = "RUNMODEREPORT";

        //20170327 add INDEXRUNMODE 为记录MES 返回的Index  Run Mode 可以直接Download 给机台 
        public const string INDEXRUNMODE = "INDEXRUNMODE";
    }

    public class eMESReCode
    {
        public const string RtnCodeOK = "0000000";
    }

    public class eLineType
    {
        public class Array
        {
            public const string A1PHL = "A1PHL";
            public const string A1CVD = "A1CVD";
            public const string A1MSP = "A1MSP";
            public const string A1ISP = "A1ISP";
            public const string A1OVN = "A1OVN";
            public const string A1UPK = "A1UPK";
            public const string A1DET = "A1DET";
            public const string A1WET = "A1WET";
            public const string A1STR = "A1STR";
            public const string A1AOH = "A1AOH";
            public const string A1MAC = "A1MAC";
            public const string A1ADS = "A1ADS";
            public const string A1SCN = "A1SCN";
            public const string A1ATT = "A1ATT";
            public const string A1TTP = "A1TTP";
            public const string A1TEG = "A1TEG";
            public const string A1NAN = "A1NAN";
            public const string A1OST = "A1OST";
            public const string A1REP = "A1REP";
            public const string A1LRP = "A1LRP";
            public const string A1ACR = "A1ACR";
            public const string A1SOR = "A1SOR";
            public const string A1CST = "A1CST";
            public const string A1GCV = "A1GCV";
        }
        public class CF
        {
            public const string F1PHL = "F1PHL";
            public const string F1UPK = "F1UPK";
            public const string F1ISP = "F1ISP";
            public const string F1RWK = "F1RWK";
            public const string F1TTT_1 = "F1SAP_1";
            public const string F1TTT_2 = "F1SAP_2";
            public const string F1TTT_3 = "F1SAP_3";
            public const string F1TTT_4 = "F1SAP_4";
            public const string F1ACM = "F1ACM";
            public const string F1FAH = "F1FAH";
            public const string F1FMA = "F1DMA";
            public const string F1FRE = "F1FRE";
            public const string F1IPP = "F1IRP";
            public const string F1SOR = "F1SOR";
            public const string F1MSC = "F1MSC";
        
        }
        public class CELL
        {
            public const string C1CUT = "C1CUT";
            public const string C1BPI = "C1BPI";
            public const string C1PIR = "C1PIR";
            public const string C1AMA = "C1AMA";
            public const string C1ODF = "C1ODF";
            public const string C1FSA = "C1FSA";
    
            public const string C1POL_1 = "C1POL_1";
            public const string C1POL_2 = "C1POL_2";
            public const string C1POL_3 = "C1POL_3";
            public const string C1LOI = "C1LOI";
            public const string C1NRP = "C1NRP";
            public const string C1GAP = "C1GAP";
            //20161223 modify  M1PST改為C1PST , M1DOM改為C1DOM
            public const string M1PST = "C1PST"; // "M1PST";
            public const string M1DOM = "C1DOM"; // "M1DOM";
            public const string C1LSC = "C1LSC";
            public const string C1CSC = "C1CSC";
            public const string C1GCV = "C1GCV";
            public const string C1RTP = "C1RTP";

            public const string M1OLB_1 = "M1OLB_1";
            public const string M1OLB_2 = "M1OLB_2";
            //public const string C1POL = "C1POL";
            
        }
    }
    public class eLOG_CONSTANT
    {
        public const string CAN_NOT_FIND_LINE = "CAN NOT FIND LINE_ID=[{0}] IN LINE OBJECT!";
        public const string CAN_NOT_FIND_LINE2 = "[{0}] CAN NOT FIND LINE_ID=[{1}] IN LINE OBJECT!";
        public const string CAN_NOT_FIND_EQP = "CAN NOT FIND EQUIPMENT_NO=[{0}] IN EQUIPMENT OBJECT!";
        public const string CAN_NOT_FIND_EQP2 = "[{0}] CAN NOT FIND EQUIPMENT_NO=[{1}] IN EQUIPMENT OBJECT!";
        public const string CAN_NOT_FIND_PORT = "CAN NOT FIND PORT=[{0}] IN PORT OBJECT!";
        public const string CAN_NOT_FIND_UNIT = "CAN NOT FIND UNIT=[{0}] IN UNIT OBJECT!";
        public const string EQP_REPORT_BIT_OFF = "[EQUIPMENT={0}] [BCS <- EQP][{1}] BIT=[OFF].";
        public const string MES_OFFLINE_SKIP = "[LINENAME={0}] [BCS -> MES][{1}] MES OFF-LINE, SKIP \"{2}\" REPORT.";
        public const string BCS_REPORT_MES = "[LINENAME={0}] [BCS -> MES][{1}] REPORT TO MES TRANSACTION=[{2}]";
    }

    public class eCSTVerifyType
    {
        public const string ACF_APVRYOPE = "APVRYOPE";
        public const string LCD_CPVRYOPE = "CPVRYOPE";
        public const string ACF_APIQCRRD = "APIQCRRD";
        public const string LCD_CPIQCRRD = "CPIQCRRD";
        public const string ACF_APCRVLTN = "APCRVLTN";
    }

    public class eCSTLogOffType
    {
        public const string ACF_APCLOGOF = "APCLOGOF";
        public const string ACF_APCCHGOF = "APCCHGOF";
        public const string LCD_CPCLOGOF = "CPCLOGOF";       
    }

    //20160930 整合到eJobData
    //public class eSubJobDataKey
    //{
    //    public const string COMMON_TrackingDataHistory = "Tracking Data History";
    //    public const string COMMON_EquipmentSpecialFlag = "Equipment Special Flag";
    //    public const string COMMON_InspectionJudgeResult = "Inspection Judge Result";
    //    public const string COMMON_InspectionReservationSignal = "Inspection Reservation Signal";
    //    public const string COMMON_ProcessReservationSignal = "Process Reservation Signal";
    //    //public const string COMMON_InspectionJudgeResult = "Inspection Judge Result";
    //}

    public class eMESEqpRunMode 
    {
        public const string UNKNOWN = "UNKNOWN";
        public const string DFLT = "DFLT";
        //Normal
        public const string NOML = "NOML";
        //Cool Run
        public const string CLRN = "CLRN";
        //Changer
        public const string CHNG = "CHNG";

        //AC 
        public const string TFTX = "TFTX";
        public const string CFXX = "CFXX";

        //Array Special
        public const string CBLT = "CBLT";
        public const string CBHT = "CBHT";
        public const string OHDC = "OHDC";
        public const string CVDL = "CVDL";
        public const string CVDH = "CVDH";
        public const string CMBI = "CMBI";
        public const string OMSP = "OMSP";
        public const string OISP = "OISP";
        
        public const string ODDC = "ODDC";
        public const string ODET = "ODET";
        public const string MCMB = "MCMB";
        public const string MDET = "MDET";
        public const string REWK = "REWK";
        public const string GA2S = "GA2S";
        public const string CA2S = "CA2S";
        public const string GS2A = "GS2A";
        public const string CS2A = "CS2A";
        public const string SNGL = "SNGL";
        public const string SEPT = "SEPT";

        //CF 
        public const string RBBM = "RBBM";
        public const string BMBR = "BMBR";
        public const string EZST = "EZST";
        public const string MQCX = "MQCX";
        public const string CPLX = "CPLX";
        public const string STOA = "STOA";
        public const string ATOS = "ATOS";
        public const string STOS = "STOS";
        public const string SPRT = "SPRT";
        
    }

    public class eQtimeEventType
    {
        public const string Unknown = "NONE";
        public const string FetchOutEvent = "FETCH";//DB SBRM_QTIME_DEF
        public const string SendOutEvent = "SEND";//DB SBRM_QTIME_DEF
        public const string ReceiveEvent = "RECEIVE"; //DB SBRM_QTIME_DEF
        public const string StoreEvent = "STORE";//DB SBRM_QTIME_DEF
        public const string VABBreakingVacuumStart = "VABBREAKINGVACUUMSTART"; //DB SBRM_QTIME_DEF CELL Special
        public const string VABBreakingVacuumEnd = "VABBREAKINGVACUUMEND"; //DB SBRM_QTIME_DEF CELL Special
        public const string VABBreakingVacuum = "PROCSTART"; 
    }

    public class ERR_CST_MAP
    {
        public const string INVALID_LINE_DATA = "INVALID LINE DATA";
        public const string INVALID_EQUIPMENT_DATA = "INVALID EQUIPMENT DATA";
        public const string VALIDATION_NG_FROM_MES = "VALIDATION NG FROM MES";
        public const string INVALID_PORT_DATA = "INVALID PORT DATA";
        public const string INVALID_PORT_STATUS = "INVALID PORT STATUS";
        public const string DIFFERENT_CST_SEQNO = "DIFFERENT_CST_SEQNO";
        public const string INVALID_CSTID = "INVALID CST ID";
        public const string UNEXPECTED_MES_MESSAGE = "UNEXPECTED MES MESSAGE";
        public const string SLOTMAP_MISMATCH = " SLOT MAPPING  ERROR";
        public const string CIM_MODE_OFF = "CIM MODE OFF";
        public const string INVALID_GLASS_SIZE = "INVALID GLASS SIZE";
        public const string INVALID_BAKING_PARA = "INVALID BAKING PARAMETER";
        public const string INVALID_RUBBING_PARA = "INVALID RUBBING PARAMETER";
        public const string INVALID_CSTSETTING_CODE = "INVALID CSTSETTING CODE";
        public const string INVALID_PRODUCT_ID = "INVALID PRODUCT ID";
        public const string PPID_NODEFINE = "PPID NOT DEFINE";
        public const string INVALID_PPID = "INVALID PPID";
        public const string RECIPE_MISMATCH = "RECIPE MISMATCH";
        public const string HAVE_DIFFERENT_PRODUCT = "HAVE DIFFERENT PRODUCT";
        public const string CST_MAP_TRANSFER_ERROR = "CST MAP TRANSFER ERROR";
        public const string GLASS_DATA_TRANSFER_ERROR = "GLASS DATA TRANSFERERROR";
        public const string INVALID_LINEOPERATIONMODE = "INVALID LINE OPERATION MODE";
        public const string INVALID_JOB_TYPE = "INVALID JOB TYPE";
        public const string INVALID_DUMMY_TYPE = "INVALID DUMMY TYPE";

        //20170118 add for A1STR special CST to CST Rule Flag
        public const string INVALID_UNLOADER_NOT_EMPTY = "INVALID UNLOADER NOT EMPTY";
        //20170126 add for A1STR Special CST To CST Rule Can not Use Common Port
        public const string INVALID_PORTTYPE_IS_COMMONPORT = "INVALID PORTTYPE IS COMMONPORT";
        //20170213 add for F1SAP_1,F1SAP_2,F1SAP_3 Complex Mode Port2 Can not User Common
        public const string INVALID_PORT02_PORTTYPE_IS_COMMONPORT = "INVALID PORT02 PORTTYPE IS COMMONPORT";
    }

    public class MES_ReasonCode
    {
        #region Loader
        #region---Cancel
        /// <summary>
        /// [Cancel] validate ng from mes at Loader
        /// </summary>
        public const string Loader_BC_Cancel_Validation_NG_From_MES = "LMESNG";
        /// <summary>
        /// [Cancel] cst map download, EQP reply NG at loader
        /// </summary>
        public const string Loader_BC_Cancel_EQ_Reply_CassetteControl_NG = "LCSTMAPNG";
        /// <summary>
        /// [Cancel] Data Transfer NG at Loader
        /// </summary>
        public const string Loader_BC_Cancel_Data_Transfer_NG = "LDTNG";
        /// <summary>
        /// [Cancel] BC Client Cancel at Loader
        /// </summary>
        public const string Loader_BC_Cancel_From_BC_Client = "LBCCS";
        /// <summary>
        /// [Cancel] EQP Cancel at Loader
        /// </summary>
        public const string Loader_EQ_Cancel = "LEQCS";
        /// <summary>
        /// [Cancel] OP Cancel at Loader
        /// </summary>
        public const string Loader_OP_Cancel = "LOPCS";

        /// <summary>
        ///  [Cancel] BC Cancel at Loader
        /// </summary>
        public const string Loader_BC_Cancel_GroupNo_Different = "LBCGD";
        /// <summary>
        /// [Cancel] BC Cancel at Loader
        /// </summary>
        public const string Loader_BC_Cancel_2T2C_Mode ="LBC2T2C";

        #endregion
        //======================================
        #region---Abort
        /// <summary>
        /// [Abort] BC abort in bc client at Loader.
        /// </summary>
        public const string Loader_BC_Abort = "LBCAB";
        /// <summary>
        /// [Abort] OP abort at loader
        /// </summary>
        public const string Loader_OP_Abort = "LOPAB";
        /// <summary>
        /// [Abort] EQP abort at loader
        /// </summary>
        public const string Loader_EQ_Abort = "LEQAB";
        #endregion
        #endregion

        //**************************************

        #region Unloader
        #region---Cancel
        /// <summary>
        /// [Cancel] validate ng from mes at Unloader
        /// </summary>
        public const string Unloader_BC_Cancel_Validation_NG_From_MES = "UMESNG";
        #endregion
        //======================================
        #region---Abort
        /// <summary>
        /// [Abort] BC abort in bc client at Unloader (no neet change cst dsp flag).
        /// </summary>
        public const string Unloader_BC_Abort = "UBCAB";
        /// <summary>
        /// [Abort] OP abort at Unloader (no need change cst dsp flag)
        /// </summary>
        public const string Unloader_OP_Abort = "UOPAB";
        /// <summary>
        /// [Abort] EQP abort at Unloader
        /// </summary>
        public const string Unloader_EQ_Abort = "UEQAB";
        /// <summary>
        /// [Cancel] cst map download, EQP reply NG at Unloader
        /// </summary>
        public const string Unloader_BC_Cancel_EQ_Reply_CassetteControl_NG = "UCSTMAPNG";
        /// <summary>
        /// [Cancel] Data Transfer NG at Unloader (no need change cst dsp flag)
        /// </summary>
        public const string Unloader_BC_Cancel_Data_Transfer_NG = "UDTNG";
        /// <summary>
        /// [Cancel] BC Client Cancel at Unloader (no need change cst dsp flag)
        /// </summary>
        public const string Unloader_BC_Cancel_From_BC_Client = "UBCCS";
        /// <summary>
        /// [Cancel] EQP Cancel at Unloader (no need change cst dsp flag)
        /// </summary>
        public const string Unloader_EQ_Cancel = "UEQCS";
        /// <summary>
        /// [Cancel] OP Cancel at Unloader (no need change cst dsp flag)
        /// </summary>
        public const string Unloader_OP_Cancel = "UOPCS";
        #endregion
        #endregion
    }

    //20161018 add for GroupNoInfo
    public class GroupNo_Def
    {
        public const int OFFLINE_DAILY_MIN_GROUPNO = 1;
        public const int OFFLINE_DAILY_MAX_GROUPNO = 20;
        public const int OFFLINE_DUMMY_MIN_GROUPNO = 21;
        public const int OFFLINE_DUMMY_MAX_GROUPNO = 30;
        public const int OFFLINE_NORMAL_MIN_GROUPNO = 31;
        public const int OFFLINE_NORMAL_MAX_GROUPNO = 100;
        public const int ONLINE_DAILY_MIN_GROUPNO = 101;
        public const int ONLINE_DAILY_MAX_GROUPNO = 120;
        public const int ONLINE_DUMMY_MIN_GROUPNO = 121;
        public const int ONLINE_DUMMY_MAX_GROUPNO = 130;
        public const int ONLINE_NORMAL_MIN_GROUPNO = 131;
        public const int ONLINE_NORMAL_MAX_GROUPNO = 65535;
    }

    public class eSubscribeKey
    {
        public const string JOBID = "JOBID";
        public const string NGMARK = "NGMARK";
        public const string CSTSEQNO = "CSTSEQNO";
        public const string SLOTNO = "SLOTNO";
    }

    //20161108 add Cell default sht Judge and Pnl Judge
    public class Cell_MES
    {
        public const string SHT_JUDGE_G = "G";
        public const char PNL_JUDGE_G = 'G';
        public const char PNL_GRADE_G = 'G';
             
    }

    //20161116 add for Sort Mode Use
    public class Sort_ULD_SendLREQStatus
    {
        public const string NONE = "NONE";
        public const string KEEP = "KEEP";
        public const string SEND = "SEND";
        
    }

    //20161210 add MixPort Sort Grade
    public class Sort_ULD_SpecialSortGrade
    {
        //20170310 modify by 會議決議M Port改為MX. 另外新增MS(Mismatch Port)
        public const string MIX_SortGrade = "MX";
        public const string MisMatch_SortGrade = "MS";
        public const string A1SOR100_NOTG_SortGrade = "M";
    }
  
    //20170103 add Sort Grade 最小(min) Priority
    public class eSortGradePriority
    {
        public const int Min_Priority = 99999;
    }

}
    
