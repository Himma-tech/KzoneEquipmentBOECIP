using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
//using UniAuto.UniBCS.HKC.OpiSpec;

namespace KZONE.UI
{
    public enum eLanguage
    {
        [Description("ENG")]
        ENG = 0,
        [Description("CHS")]
        CHS = 1
    }

    public enum eOPIMessageType
    {
        Warning = 0,
        Error = 1,
        Question = 2,
        Information = 3
    }

    //for CF Photo CV#03
    public enum eHighCVMode
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("NormalMode")]
        NormalMode = 1,
        [Description("One by One Mode")]
        OnebyOneMode = 2,
        [Description("By Side Mode")]
        BySideMode = 3
    }

    public enum ePalletMode
    {
        UnKnown = 0,
        PackMode = 1,
        UnpackMode = 2
    }

    public enum ePackingMode
    {
        UnKnown = 0,
        PackMode = 1,
        UnpackMode = 2
    }

    public enum eUnpackSource
    {
        UnKnown = 0,
        DPK = 1,
        DPS = 2
    }

    public enum eObjectType
    {
        TextBox,
        ComboBox,
        CheckBox
    }

    public enum UpdateResult
    {
        None,
        CheckFail,
        UpdateFail,
        UpdateSuccess
    }

    public enum BufferStatus
    {
        Unused = 0,
        LoadRequest = 1,
        LoadComplete = 2,
        MappingEnd = 3,
        WaitForRun = 4,
        CancelEnd = 5,
        InProcess = 6,
        ProcessEnd = 7,
        Unknown = 8
    }

    public enum BufferSlotType
    {
        Unused = 0,
        NormalBuffer = 1,
        DailyBuffer = 2,
        Unknown = 3
    }

    //public enum eEQPStatus
    //{
    //    Unused = 0,
    //    Run = 1,
    //    Down = 2,
    //    Idle = 3,
    //    Stop = 4,
    //    Initial = 5
    //}

    public enum eBufferSlotControlMode
    {
        Unused = 0,
        Enable = 1,
        Disable = 2,
    }

    public enum eBufferStatus
    {
        Unused = 0,
        Down = 1,
        RuninBuffering = 2,
        RuninUnbuffering = 3,
        RuninPass = 4,
        Disable = 5
    }

    public enum eCassetteType
    {
        Unused = 0,
        AType_RandomCassette = 1,
        SType_SequenceCassette = 2,
        SSType_AfterAssembledofCassette = 3,
        BType_AfterCutofCassette = 4,
        PPBOX = 5,
        DenseBox = 6

    }

    public enum ePortStatus
    {
        UnKnown = 0,
        LoadRequest = 1,
        LoadComplete = 2,
        UnloadRequest = 3,
        UnloadComplete = 4
    }

    public enum eCassetteStatus
    {
        UnKnown = 0,
        NoCassetteExist = 1,
        WaitingforCassetteData = 2,
        WaitingforStartCommand = 3,
        WaitingforProcessing = 4,
        InProcessing = 5,
        ProcessPaused = 6,
        ProcessCompleted = 7,
        CassetteReMap = 8,
        InAborting = 9
    }

    public enum eCassettePortStatus
    {
        Unused = 0,
        LREQ = 1, //Load Request
        LDCP = 2, //Load Complete
        MPEN = 3, //Mapping End
        WASC = 4, //Wait for Start Command
        WAIT = 5, //Wait for Run
        CAEN = 6, //Cancel End
        INPR = 7, //In Process
        PREN = 8, //Process End
        PPEN = 9, //Partial Process End
        FREN = 10, //Force End
        UREQ = 11, //Unload Request
        UDCP = 12, //Unload Complete
        DOWN = 13, //Port Down
        PAUS = 14, //Port Pause
        REMP = 15, //Re-Mapping
        WFAE = 16  //(Wait For Abort End)
    }

    public enum eRobotFetchSequenceMode
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("From Lower to Upper")]
        FromLowerToUpper = 1,
        [Description("From Upper to Lower")]
        FromUpperToLower = 2
    }

    public enum eRobotOperMode
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("NormalMode")]
        NormalMode = 1,
        [Description("D(Dual)Mode")]
        DualMode = 2,
        [Description("S (Single)")]
        SingleMode = 3
    }

    public enum eRobotStatus
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("Init")]
        Init = 1,
        [Description("Stop")]
        Stop = 2,
        [Description("Pause")]
        Pause = 3,
        [Description("Idle")]
        Idle = 4,
        [Description("Running")]
        Running = 5
    }

    public enum eRobotJobStatus
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("NoExist")]
        NoExist = 1,
        [Description("Exist")]
        Exist = 2,
        [Description("Arm Disabled")]
        ArmDisabled = 4,
        [Description("Arm Disabled & No Exist Job")]
        ArmDisabledNoExist = 5,
        [Description("Arm Disable & Exist Job")]
        ArmDisableExist = 6
    }

    public enum eRobotStageStatus
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("NoRquest")] //無RB服務
        NoRquest = 1,
        [Description("LDRQ")] //可收片
        LDRQ = 2,
        [Description("UDRQ")] //有片
        UDRQ = 3,
        [Description("LDRQ_UDRQ")]
        LDRQ_UDRQ = 4
    }

    public enum eControlMode
    {
        Unused = 0,
        NomalMode = 1,
        RTCLocalMode = 2,
        RTCRemoteMode = 3,
        DummyLocalMode = 4,
        DummyRemoteMode = 5
    }

    public enum eChangerPlanStatus
    {
        //NoPlan = 0,
        //Start = 1,
        //End = 2,
        //Ready = 3,
        //Request = 4,
        //Cancel = 5,
        //Aborting = 6,
        //Abort = 7
        NoStart = 0,
        NormalComplete = 1,
        Abort = 2,
        SlotMappingError = 3,
        Other = 4,
        Start = 5,
        HavePlan = 6,
        Login = 7,
        NoPlan =8
    }

    public enum eRobotOperationMode
    {
        UnKnown = 0,
        NormalMode = 1,
        DualMode = 2,
        SingleMode = 3
    }

    public class CassetteDataCheck
    {
        public string ObjectType { get; set; } //Cassette , Slot

        public TextBox TxtObject { get; set; }
        public ComboBox CboObject { get; set; }

        public bool NumberOnly { get; set; }

        public string ObjectName { get; set; }
        public string RegularExpression { get; set; }
        public string RegularText { get; set; }
        public bool CheckEmpty { get; set; }
    }

    //public enum eIndexerMode
    //{
    //    [Description("Unused")]
    //    Unused = 0,
    //    [Description("Normal Mode")]
    //    NormalMode = 1
    //[Description("Sorter Mode")]
    //SorterMode = 2,
    //[Description("Changer Mode")]
    //ChangerMode = 3,
    //[Description("Cool Run Mode")]
    //CoolRunMode = 4,
    //[Description("Force Clean Out Mode")]
    //ForceCleanOut = 5,
    //[Description("Abnormal Force Clean Out Mode")]
    //MixRunMode = 6,
    //[Description("MQC Mode")]
    //MQCMode = 7,
    //[Description("Through Mode")]
    //ThroughMode = 8,
    //[Description("Fix Mode")]
    //FixMode = 9,
    //[Description("Random Mode")]
    //RandomMode = 10,
    //[Description("Normal Mode")]
    //NormalMode = 11,
    //[Description("Mix Mode")]
    //MixMode = 12
    //}

    public enum ePortAssignment
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("for GAP")]
        GAP = 1,
        [Description("for GMI")]
        GMI = 2
    }

    public enum ePortType
    {
        [Description("Unused")]
        Unused = 0,
        [Description("Loading Port")]
        LoadingPort = 1,
        [Description("Unloading Port")]
        UnloadingPort = 2,
        [Description("Common Port")]
        CommonPort = 3,
        [Description("UnKnown")]
        UnKnown = 4
        //[Description("Both Port")]
        //BothPort = 3,
        //[Description("Buffer Port - Buffer Type")]
        //BufferType = 4,
        //[Description("Buffer Port - Loader in Buffer Type")]
        //LoaderinBufferType = 5,
        //[Description("Buffer Port - Un-loader in Buffer Type")]
        //UnloaderinBufferType = 6
    }

    //public enum ePortDown
    //{
    //    [Description("No Use")]
    //    NoUse = 0,
    //    [Description("Normal")]
    //    Normal = 1,
    //    [Description("Down")]
    //    Down = 2
    //}

    public enum ePortJudge
    {
        [Description("None")]
        None = 0,
        [Description("OK Port")]
        OKPort = 1,
        [Description("NG Port")]
        NGPort = 2,
        [Description("Mix Port")]
        MixPort = 3
        //[Description("TFT Mode")]
        //TFTMode = 1,
        //[Description("CF Mode")]
        //CFMode = 2,
        //[Description("Dummy Mode")]
        //DummyMode = 3,
        //[Description("MQC Mode")]
        //MQCMode = 4,
        //[Description("HT Mode")]
        //HTMode = 5,
        //[Description("LT Mode")]
        //LTMode = 6,
        //[Description("ENG Mode")]
        //ENGMode = 7,
        //[Description("IGZO Mode")]
        //IGZOMode = 8,
        //[Description("ILC Mode")]
        //ILCMode = 9,
        //[Description("FLC Mode")]
        //FLCMode = 10,
        //[Description("Through Dummy Mode")]
        //ThroughDummyMode = 11,
        //[Description("Thickness Dummy Mode")]
        //ThicknessDummyMode = 12,
        //[Description("UV Mask Mode")]
        //UVMaskMode = 13,
        //[Description("By Grade Mode")]
        //ByGradeMode = 14,
        //[Description("OK Mode")]
        //OKMode = 15,
        //[Description("NG Mode")]
        //NGMode = 16,
        //[Description("MIX Mode")]
        //MIXMode = 17,
        //[Description("EMP Mode")]
        //EMPMode = 18,
        //[Description("RW Mode")]
        //ReworkMode = 19,
        //[Description("Mismatch Mode")]
        //MismatchMode = 20,
        //[Description("PD Mode")]
        //PDMode = 21,
        //[Description("IR Mode")]
        //IRMode = 22,
        //[Description("RP Mode")]
        //RPMode = 23,
        //[Description("Re-Judge Mode")]
        //ReJudgeMode = 24
    }

    public enum ePartialFull
    {
        Unknown = 0,
        PartialFull = 1,
        NoPartialFull = 2
    }

    public enum eLoadingCassetteType
    {
        Unused = 0,
        AType_RandomCassette = 1,
        SType_SequenceCassette = 2,
        SSType_AfterAssembledofCassette = 3,
        BType_AfterCutofCassette = 4,
        PPBOX = 5,
        DenseBox = 6
    }

    public enum ePortTransfer
    {
        [Description("Unused")]
        Unused = 0,
        [Description("AGV Mode")]
        AGVMode = 1,
        [Description("MGV Mode")]
        MGVMode = 2,
        //[Description("Stocker Inline Mode")]
        //StockerInlineMode = 3
    }

    public enum ePortEnable
    {
        [Description("Unused")]
        Unused = 0,
        [Description("Enable Mode")]
        EnableMode = 1,
        [Description("Disable Mode")]
        DisableMode = 2
    }

    public enum eBoxType
    {
        [Description("InBox")]
        InBox = 1,
        [Description("OutBox")]
        OutBox = 2,
        [Description("Unknown")]
        Unknown = 0
    }

    public enum eEQPOperationMode
    {
        [Description("Manual Mode")]
        MANUAL = 0,
        [Description("Automatic Mode")]
        AUTO = 1,
    }

    public enum eAutoControlMode
    {
        [Description("Enable")]
        Disable = 0,
        [Description("Disable")]
        Enable = 1,
        [Description("Unkown")]
        Unkown = 2,

    }

    ////for ATS Loader Operation Mode (CBATS )
    //public enum eLoaderOperationMode
    //{
    //    [Description("Unknown")]
    //    Unknown = 0,
    //    [Description("t1 Loader Mode")]
    //    T1LoaderMode = 1,
    //    [Description("t2 Loader Mode")]
    //    T2LoaderMode = 2,
    //    [Description("Auto Change Mode")]
    //    AutoChangeMode = 3
    //}

    public enum eCIMMode
    {
        OFF = 0,
        ON = 1
    }

    public enum eTurnTable
    {
        Disable = 0,
        Enable = 1,
        Unused = 2
    }

    public enum eStopBitMode
    {
        OFF = 0,
        ON = 1
    }

    public enum eBufferMode
    {
        Unused = 0,
        NormalMode = 1,
        DummyMode = 2,
        Unknown = 3
    }

    public enum ePreSputterMode
    {
        OFF = 0,
        ON = 1,
        Unknown = 2
    }

    public enum eDCRMode
    {
        [Description("Disable")]
        DISABLE = 0,
        [Description("Enable")]
        ENABLE = 1
    }

    public enum eDispatchMode
    {
        [Description("Unused")]
        Unused = 0,
        [Description("Local")]
        Local = 1,
        [Description("Remote")]
        Remote = 2
    }

    public enum eForceVCRMode_SOR
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Enable")]
        ENABLE = 1,
        [Description("Disable")]
        DISABLE = 2
    }

    public enum eVirualPortOpMode_SOR
    {
        [Description("No Use")]
        NoUse = 0,
        [Description("Normal Mode")]
        NormalMode = 1,
        [Description("LD Virtual Port Mode")]
        LDVirualPortMode = 2,
        [Description("ULD Virtual Port Mode")]
        ULDVirualPortMode = 3
    }

    public enum eCSTOperationMode
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Kind To Kind Mode")]
        KindToKind = 1,
        [Description("Cassette To Cassette Mode")]
        CassetteToCassette = 2,
        [Description("Lot to Lot Mode")]
        LotToLot = 3
    }

    public class BatchSampling
    {
        //public List<string> InspectionName;
        public List<int> BatchSize;
        public List<int> SamplingRate;

        public BatchSampling()
        {
            //InspectionName = new List<string>();
            BatchSize = new List<int>();
            SamplingRate = new List<int>();
        }

        public object CloneObject()
        {
            BatchSampling _batch = new BatchSampling();

            List<int> _rate = new List<int>();
            foreach (int _item in this.SamplingRate)
            {
                _rate.Add(_item);
            }
            _batch.SamplingRate = _rate;


            List<int> _size = new List<int>();
            foreach (int _item in this.BatchSize)
            {
                _size.Add(_item);
            }
            _batch.BatchSize = _size;

            return _batch;
        }

    }

    struct OPIConst
    {
        #region 定義OPI會使用到的常數值
        public static string LayoutFolder = ConfigurationManager.AppSettings["LayoutFolder"]; //記錄Layout.xml放置資料夾
        public static string ParamFolder = ""; //記錄 DBConfig.xml放置資料夾
        public static string RobotFolder = "";  //紀錄robot image放置的資料夾
        public static string ImageFolder = "";//记录Image Folder放置的资料夹
        public static string TimingChartFolder = string.Empty; //紀錄 timing chart 存放的資料夾

      
        #endregion
    }

    public enum eRobotMode
    {
        [Description("None")]
        None = 0,
        [Description("Semi Single")]
        SemiSingle = 1,
        [Description("Semi Auto")]
        SemiAuto = 2,
        [Description("Auto")]
        Auto = 3
    }

    public enum eRobotControlMode
    {
        [Description("None")]
        None = 0,
        [Description("Local")]
        Local = 1,
        [Description("Remote")]
        Remote = 2,
        [Description("Manual")]
        Manual = 4,
        [Description("Error")]
        Error = 99
    }

    public class RobotSlotInfo
    {
        //00-99
        public string SlotNo { get; set; }

        //RCS_SLOT_STATUS : Error,Exchange Request,Exchange Request and Plan Receive,Get Put Request,Get Put Request and Plan Receive...
        public string SlotStatus { get; set; }

        //PROCESS_STATUS : Empty, End ,Force Clean Out,Lost BC WIP, Processing, Removed, Skip, To Other Port, Wait For Process
        public string ProcessStatus { get; set; }

        //Slot 在席 Glass 的 Robot Route
        public string PresenceRoute { get; set; }

        //Slot 在席 Glass 的 Last Step ID
        public string PresenceLastStepID { get; set; }

        //Slot 在席 Glass 的 Real Step ID
        public string PresenceRealStepID { get; set; }

        ////Slot 在席 Glass
        //public JobKey PresenceJobKeys { get; set; }

        ////Slot 預計要收的 Glass
        //public JobKey ReserveJobKeys { get; set; }

        public RobotSlotInfo()
        {
            SlotNo = string.Empty;
            SlotStatus = string.Empty;
            PresenceRoute = string.Empty;
            PresenceLastStepID = string.Empty;
            PresenceRealStepID = string.Empty;
            //PresenceJobKeys = new JobKey();
            //ReserveJobKeys = new JobKey();
        }
    }
}




