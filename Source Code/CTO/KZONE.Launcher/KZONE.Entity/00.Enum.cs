///Define  Enum

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    /// <summary>
    /// 0：Unused
    ///1：PR Coater
    ///2：Mask
    ///3：PIP For PIP EQP
    ///4：Seal For SDP EQP
    ///5：LCD For LCD EQP
    ///6：UV Mask For SUV EQP
    ///7：PAM For PAM EQP
    ///8：COF For OLB EQP
    ///9：ACF For OLB EQP
    ///10：PA For OLB EQP
    ///11：Nonwoven For OLB EQP
    ///12：Cushion For OLB EQP
    ///13：Tuffy For DISP EQP
    ///14：PCB For PCB EQP
    ///15：ACF For PCB EQP
    ///16：Cushion For PCB EQP
    /// </summary>
    public enum eMaterialType
    {
        UNSED = 0,
        COATER = 1,
        MASK = 2,
        PIP = 3,
        SEAL = 4,
        LCD = 5,
        UVMASK = 6,
        PAM = 7,
        COF = 8,
        ACF_OLB = 9,
        PA = 10,
        NONWOVEN = 11,
        CUSHION_OLB = 12,
        TUFFY = 13,
        PCB = 14,
        ACF_PCB = 15,
        CUSHION_PCB = 16,
        CUTTING_WHEEL = 17,
    }
    public enum eCreateModifyFlag
    {
        UNKNOW = 0,
        EQ_CREATE = 1,
        EQ_MODIFY = 2
    }

    //Glass Data Remove Recovery Report
    public enum eReportFlag
    {
        UNUSED = 0,
        NORMALREMOVED = 1,
        RECOVERY = 2,
        DELETE = 3,
        MANUALPORTOUT = 4
    }
    public enum eBitResult
    {
        OFF = 0,
        ON = 1
    }

    public enum eFabType
    {
        Unknown = 0,
        ARRAY = 1,
        CF = 2,
        CELL = 3,
        MOD = 4
    }

    public enum eCIMModeCmd
    {
        UNKNOWN = 0,
        CIM_ON = 1,
        CIM_OFF = 2
    }

    public enum eRTCRunMode
    {
        Unused = 0,
        Normal = 1,
        RTCLocal = 2,
        RTCRemote = 3,
        DummyLocal = 4,
        DummyRemote = 5
    }

    public enum eRTCDummyRunCommand
    {
        Unused = 0,
        StartContinue = 1,
        End = 2
    }

    public enum eEQPStatus
    {
        Unused = 0,
        Run = 5,
        Down = 2,
        Idle = 4,
        Stop = 3,
        Initial = 1
    }

    public enum eBUFStatus
    {
        /* Define by CIM
        0：Unused        
        1：Down
        2：Run in Buffering
        3：Run in Un-buffering
        4：Run in Pass
        5：Disable  */
        Unused = 0,
        Down = 1,
        RunInBuffering = 2,
        RunInUnBuffering = 3,
        RunInPass = 4,
        Diasble = 5
    }

    public enum ePortStatus
    {
        //LDRQ/LDCM/UDRQ/UDCM
        /// <summary>
        /// Unknown
        /// </summary>
        UN = 0,
        /// <summary>
        /// Load Request
        /// </summary>
        LDRQ = 1,
        /// <summary>
        /// Load Complete
        /// </summary>
        LDCM = 2,
        /// <summary>
        /// Unload Request
        /// </summary>
        UDRQ = 10,
        /// <summary>
        /// Unload Complete
        /// </summary>
        UDCM = 11
    }

    public enum eCassetteStatus
    {
        UNKNOWN = 0,
        MPEN = 3,
        WAIT = 4,
        CAEN = 5,
        INPR = 6,
        PREN = 7,
        PPEN = 8,
        FREN = 9,
        PAUS = 13
    }

    public enum ePortCassetteStatus
    {
        Unused = 0,
        LREQ = 1,
        LDCP = 2,
        MPEN = 3,
        WASC = 4,
        WAIT = 5,
        CAEN = 6,
        INPR = 7,
        PREN = 8,
        PPEN = 9,
        FREN = 10,
        UREQ = 11,
        UDCP = 12,
        DOWN = 13,
        PAUS = 14,
        REMP = 15,
        WFAE = 16
    }

    public enum ePortJudge
    {
        None = 0,
        OK = 1,
        NG = 2,
        Mix = 3
    }

    public enum eCassetteType
    {
        Unused = 0,
        A = 1,  //Romdan CST
        S = 2, //Sequence CST
        SS = 3, //After Assembled of Cassette
        B = 4, //After Cut of Cassette
        PPBOX = 5,
        Dense = 6
    }
    /*0：Unused
1：OK, Normal Completed
2：OK, BC Cancel
3：OK, BC Abort
4：OK, EQ Auto Cancel
5：OK, EQ Auto Abort
6：OK, Operator Cancel
7：OK, Operator Abort
8：NG, Error
9 ~：Other, Define by CIM*/
    public enum eCompletedCassetteData
    {
        Unknown = 0,
        NormalComplete = 1,
        BCForcedToCancel = 2,
        BCForcedToAbort = 3,
        EQAutoCancel = 4,
        EQAutoAbort = 5,
        OperatorCancel = 6,
        OperatorAbort = 7,
        NG = 8,
        Other = 9
    }

    public enum eCompleteCassetteReason
    {
        Normal = 0,
        OnPortQTimeOverCancel = 1,
        StoreQTimeOverAbort = 2,
        NextGlassPortModeMismatch = 3,
        NextGlassGradeMismatch = 4,
        ENGModeComplete = 5,
        GlassQTimeOverAbort = 6,
        NextGlassProductTypeMismatch = 7,
        NextGlassProductIDMismatch = 8,
        NextGlassGroupIndexMismatch = 9
    }

    public enum eLoadingCassetteType
    {
        ActualCassette = 1,
        EmptyCassette = 2
    }

    public enum eReportMode
    {
        PLC = 1,
        RS232 = 2,
        RS485 = 3,
        HSMS_PLC = 4,
        PLC_HSMS = 5,
        HSMS_NIKON = 6,
        HSMS = 7,
        PLC_HSMS_R = 8,
        PLC_HSMS_R_D = 9,
        PLC_HSMS_D = 10
    }

    public enum eEQPOperationMode
    {

        MANUAL = 1,
        AUTO = 2,
        SEMIAUTO = 3

    }

    public enum eHostMode
    {
        OFFLINE = 0,
        REMOTE = 2,
        LOCAL = 1
    }

    public enum ePLAN_STATUS
    {
        NO_PLAN = 0,
        START = 1,
        END = 2,
        READY = 3,
        REQUEST = 4,
        CANCEL = 5
    }

    public enum eReturnCode1
    {
        Unknown = 0,
        OK = 1,
        NG = 2
    }

    public enum eReturnCode2
    {
        Unknown = 0,
        Accept = 1,
        AlreadyInDesiredStatus = 2,
        NG = 3
    }

    public enum eReturnCode3
    {
        Unknown = 0,
        Accept = 1,
        NotAcceppt = 2
    }

    public enum eReturnCode4
    {
        Unused = 0,
        OK = 1,
        NGCommandError = 2,
        NGBufferSlotError = 3,
        NGOtherError = 4
    }

    public enum eLoadingCstType
    {
        Unknown = 0,
        Actual = 1,
        Empty = 2
    }

    public enum eQTime
    {
        Unknown = 0,
        NormalUnloading = 1,
        QTimeOver_Unloading = 2
    }

    public enum eCFQTime
    {
        Unused = 0,
        OK = 1,
        NG = 2,
    }

    public enum eParitalFull
    {
        Unknown = 0,
        PartialFull = 1,
        NoPartialFull = 2
    }


    /// <summary>
    ///  POL Line
    /// </summary>
    public enum eBACV_ByPass
    {
        Unknown = 0,
        NormalUnloading = 1,
        BACV_ByPass_Unloading = 2
    }

    public enum eDistortion
    {
        Unknown = 0,
        NotDistortion = 1,
        Distortion = 2
    }

    public enum eDirection
    {
        Unknown = 0,
        Normal = 1,
        Reverse = 2
    }

    public enum eGlassExist
    {
        Unknown = 0,
        NoExist = 1,
        Exist = 2
    }

    public enum ePortType
    {
        Unknown = 0,
        LoadingPort = 1,
        UnloadingPort = 2,
        CommonPort = 3,
        /*  0：Unused
            1：Loading Port
            2：Unloading Port
            3：Common Port
            */
    }

    public enum ePortMode
    {
        // ITO QC CA ST RP RJ SK Y L K C : MES有這些資料, 但PLC對不上或沒用到
        Unknown = 0,
        TFT = 1,                /*TFT*/
        CF = 2,                 /*CF*/
        Dummy = 3,              /*DM*/
        MQC = 4,                /*MQC*/
        HT = 5,
        LT = 6,
        ENG = 7,
        IGZO = 8,
        ILC = 9,
        FLC = 10,
        ThroughDummy = 11,         /*TR*/
        ThicknessDummy = 12,        /*TK*/
        UVMask = 13,
        ByGrade = 14,
        OK = 15,                /*OK*/
        NG = 16,                /*NG*/
        MIX = 17,               /*MIX*/
        EMPMode = 18,           /*EMP*/
        Rework = 19,             /*RW*/
        Mismatch = 20
    }


    public enum ePortOperMode
    {
        Unknown = 0,
        PACK = 1,                /*Packing*/
        UNPACK = 2                 /*Un-Packing*/
    }

    public enum eCFBufSlotStatus
    {
        Unused = 0,
        LREQ = 1,
        LDCP = 2,
        MPEN = 3,
        WAIT = 4,
        CAEN = 5,
        INPR = 6,
        PREN = 7,
        DOWN = 12,
    }

    public enum eCFBufSlotType
    {
        Unused = 0,
        NormalBufferSlot = 1,
        DailyBufferSlot = 2,
    }

    public enum eCFBufSlotCommandMode
    {
        Unused = 0,
        Enable = 1,
        Disable = 2,
    }

    public enum eCFBufSlotCommand
    {
        Unused = 0,
        Start = 1,
        Cancel = 2,
    }

    public enum eCFBufSlotCommandStatus
    {
        None = 0,
        Send = 1,
        ReplyOk = 2,
        ReplyNg = 3
    }

    public enum eCELL_TCVDispatchRule
    {
        NRP = 1,
        CUT = 2,
        POL = 3
    }

    public enum ePortTransferMode
    {
        Unknown = 0,
        AGV = 1,
        MGV = 2
        //0：Unused
        //1：AGV Mode
        //2：MGV Mode

    }

    public enum ePortEnableMode
    {
        Unknown = 0,
        Enabled = 1,
        Disabled = 2
        //0：Unused
        //1：Enable Mode
        //2：Disable Mode

    }

    public enum eCstCmd
    {
        //0：Unused
        //1：Start
        //2：Cancel
        //3：Abort 
        //4：Pause
        //5：Resume
        //6：Re-Mapping
        //7：Process End
        //8 : Mapping Data Download
        //9 : Wait For Abort End

        Unused = 0,
        Start = 1,
        Cancel = 2,
        Abort = 3,
        Pause = 4,
        Resume = 5,
        ReMapping = 6,
        ProcessEnd = 7,
        MappingDataDownload = 8,
        WaitForAbortEnd = 9

    }

    public enum eCstCmdRetCode
    {
        Unused = 0,
        OK = 1,
        NG_ERROR = 2,
        OTHER = 3
    }

    public enum eOPISendType
    {
        Local = 0,
        All = 1,
        Appoint = 2
    }

    public enum eSubstrateType
    {
        Glass = 0,
        Chip = 1,
        /// <summary>
        /// POL , CST Cleaner
        /// </summary>
        Cassette = 2,
        Cut = 3
    }

    public enum eEDCReportTo
    {
        Unknown = 0,
        MES = 1,
        EDA = 2,
        BOTH = 3
    }

    public enum ePortDown
    {
        Down = 0,
        Normal = 1
    }

    public enum eSamplingRule
    {
        ByCount = 1,
        ByUnit = 2,
        BySlot = 3,
        ByID = 4,
        FullInspection = 5,
        InspectionSkip = 6,
        NormalInspection = 7
    }

    public enum eSideUnit
    {
        CoaterVCD01 = 0,
        CoaterVCD02 = 1,
        ExposureCP01 = 2,
        ExposureCP02 = 3
    }
    public enum eMaterialEQtype
    {
        Normal = 0,
        MaskEQ = 1
    }


    public enum eMaterialMode
    {
        NORMAL = 1,
        ABNORMAL = 2,
        NONE = 3
    }

    public enum eReceiveStep
    {
        BitAllOff = 1,
        InlineMode = 2,
        EqTrouble = 3,
        UpEqTrouble = 4,
        ReceiveAble = 5,
        UpSendAble = 6,
        JobDataCheckNG = 7,
        JobDataCkeckOK = 8,
        ReceiveStart = 9,
        SendStart = 10,
        TransferOn = 11,
        SendComplete = 12,
        ReceiveComplete = 13,
        End = 14,
        DebugMode = 15,
        ReceiveCancel = 16,
        ReceiveResume = 17,
        ReceiveRoll = 18,
        GlassExist = 19,
        SendCancel = 20,
        SendResume = 21,
        PerReceive = 22,
        ForceCompleteRequest = 23,
        ForceInitialRequest = 24,
        UpForceInitialRequestAck = 25,
        UpForceCompleteRequestAck = 26

    }

    public enum eSendStep
    {
        BitAllOff = 0,
        InlineMode = 1,
        EqTrouble = 2,
        SendAble = 3,
        DownEqTrouble = 4,
        ReceiveAble = 5,
        SendStart = 6,
        ReceiveStart = 7,
        TansferOn = 8,
        ReceiveComplete = 9,
        SendComPlete = 10,
        End = 11,
        DownPIOTrouble = 12,
        WriteJobDataError = 13,
        DebugMode = 14,
        SendCancel = 15,
        SendResume = 16,
        ReceiveCancel = 17,
        ReceiveResume = 18,
        PerSend = 19,
        ForceCompleteRequest = 20,
        ForceInitialRequest = 21,
        DownForceInitialRequestAck = 22
    }






    public enum eMaterialStatus
    {
        // 0：Unused
        //1：Un-Mount
        //2：Mount
        //3：In-Use
        UNUSED = 0,
        UNMOUNT = 2,
        MOUNT = 1,
        IN_USE = 3,
        PREPARE = 4,//预留
    }

    public enum eCompleteStatus
    {
        NORMALEND = 1,
        ABNORMAL = 2
    }
    //目前EQP IO Report "1：Kind To Kind mode  2：Cassette To Cassette mode 3：Lot To Lot Mode"
    //但是JOB DATA IO is " 0: Kind to Kind   1: CST to CST 2：Lot To Lot"
    public enum eCSTOperationMode
    {
        KTOK = 0,
        CTOC = 1,
        LTOL = 2
    }

    public enum eEnableDisable
    {
        Enable = 1,
        Disable = 0
    }

    public enum eRecipeEvent
    {
        Unused = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        Request = 4,
        Regist = 5
    }
    public enum eTurnTableMode
    {
        Unused = 0,
        Enable = 1,
        Disable = 2
    }

    public enum eEQPMode
    {
        NORN = 0,
        PASS = 1,
        APAS = 2,
        CVD2S = 3,
        CVD2D = 4,
        CVD4P1 = 5,
        CVD4P2 = 6,
        CVD2O = 7,
        CVD2Q = 8,
        CVD4Q = 9,
        DRYENG = 10,
        DRYMQC = 11,
        DRYIGZO = 12,
        DRYA = 13,
        DRYB = 14,
        MIX = 15
    }

    public enum eRobotOperationMode
    {
        Unknown = 0,
        NormalMode = 1,
        DualMode = 2,
        SingleMode = 3
    }

    public enum eRobotOperationAction
    {
        Unknown = 0,
        Received = 1,
        Sent = 2,
        Both = 3
    }

    public enum eProportionalRuleName
    {
        Unknown = 0,
        Normal = 1,
        IGZO = 2,
        MQC = 3,
        ENG = 4,
        Reserved_A = 5,
        Reserved_B = 6,
        HT = 7,
        LT = 8
    }

    /// <summary>
    /// 20170110 Ray: eIndexerRunMode不使用,請改用eLineRunMode
    /// </summary>
    //public enum eIndexerRunMode
    //{ 
    //    UNKNOWN = 0,
    //    NORMAL_MODE = 1,
    //    COOL_RUN_MODE = 2,
    //    CHANGER_MODE = 3,
    //    COMBINE_MODE = 4,
    //    REWORK_MODE = 4,
    //    COMBINE_LOW_TEMP_MODE = 4,
    //    COMBINE_HIGH_TEMP_MODE = 5,
    //    ONLY_HDC_MODE = 5,
    //    ONLY_DDC_MODE = 5,
    //    ONLY_CHDC_MODE = 6,
    //    ONLY_DET_MODE = 6,
    //    ONLY_MSP_MODE = 6,
    //    ONLY_ISP_MODE = 6,
    //    MIX_COMBINE_MODE = 7,
    //    ONLY_CVD_LOW_TEMP_MODE = 7,
    //    MIX_DET_MODE = 8,
    //    ONLY_CVD_HIGH_TEMP_MODE = 8,
    //    SEPARATE_MODE = 5,
    //    SINGLE_MODE = 4, //CF F1TTT used
    //    COMPLEX_MODE = 5,//CF F1TTT used
    //    TFT_MODE = 4,
    //    CF_MOODE = 5,
    //    EASY_SORTER_MODE = 6,

    //    GLASS_CHANGER_A_TO_S_MODE = 4,
    //    CST_CHANGER_A_TO_S_MODE = 5,
    //    GLASS_CHANGER_S_TO_A_MODE = 6,
    //    CST_CHANGER_S_TO_A_MODE = 7,

    //    R_BACKUP_BM_MODE = 4,
    //    BM_BACKUP_R_Mode = 5,

    //    MQC_MODE = 4,
    //    F1FMA_CF_MODE = 4,
    //    ARRAY_MODE = 5,
    //    S_TO_A_MODE = 6,
    //    A_TO_S_MODE = 7,
    //    S_TO_S_MODE = 8,
    //}

    //20170110 Ray Modify
    public enum eLineRunMode
    {
        UNKNOWN = 0,
        NORMAL_MODE = 1,
        COOL_RUN_MODE = 2,
        CHANGER_MODE = 3,
        COMBINE_MODE = 4,
        REWORK_MODE = 5,
        COMBINE_LOW_TEMP_MODE = 6,
        COMBINE_HIGH_TEMP_MODE = 7,
        ONLY_HDC_MODE = 8,
        ONLY_DDC_MODE = 9,
        ONLY_CHDC_MODE = 10,
        ONLY_DET_MODE = 11,
        ONLY_MSP_MODE = 12,
        ONLY_ISP_MODE = 13,
        MIX_COMBINE_MODE = 14,
        ONLY_CVD_LOW_TEMP_MODE = 15,
        MIX_DET_MODE = 16,
        ONLY_CVD_HIGH_TEMP_MODE = 17,
        SEPARATE_MODE = 18,
        SINGLE_MODE = 19, //CF F1TTT used
        COMPLEX_MODE = 20,//CF F1TTT used
        TFT_MODE = 21,
        CF_MOODE = 22,
        EASY_SORTER_MODE = 23,

        GLASS_CHANGER_A_TO_S_MODE = 24,
        CST_CHANGER_A_TO_S_MODE = 25,
        GLASS_CHANGER_S_TO_A_MODE = 26,
        CST_CHANGER_S_TO_A_MODE = 27,

        R_BACKUP_BM_MODE = 28,
        BM_BACKUP_R_MODE = 29,

        MQC_MODE = 30,
        F1DMA_CF_MODE = 31,
        ARRAY_MODE = 32,
        S_TO_A_MODE = 33,
        A_TO_S_MODE = 34,
        S_TO_S_MODE = 35,

        MIX_RUN_MODE = 36,
        DUAL_MODE = 37,
        SORT_MODE = 38,
        MANUAL_MODE = 39,
        COUPLE_MODE = 40,

        PACKING_MODE = 41,
        UNPACKING_MODE = 42,

        AGV_MODE = 43,
        MGV_MODE = 44,

        StoSS = 45,
        SStoS = 46,

        SPUTTER_BY_PASS_MODE = 47, //F1ISP EQP RUN MODE
        MASK_REPARI_MODE = 48,       ////F1IPP EQP RUN MODE

        ONLY_CVD_LOW_TEMPERATURE_COT_MODE = 49,
        COMBINE_LOW_TEMPERATURE_COT_MODE = 50,
        CF_CHANGER_MODE = 51,
        //20170307 add Cell Changer Mode
        CELL_CHANGER_MODE = 52,
        //DFLT_MODE = ,
        //20170606 add for CF MX Mode , Only G Sorter Mode
        CF_MX_MODE = 53,
        ONLY_G_SORT_MODE = 54,
        //20170609 add for F1DMA New Run Mode
        CF_Turn_180_And_Turn_Back_Mode = 55,
        CF_Turn_180_Not_Turn_Back_Mode = 56,
        Array_Turn_180_And_Turn_Back_Mode = 57,
        Array_Turn_180_Not_Turn_Back_Mode = 58,
        S_To_A_Turn_180_And_Turn_Back_Mode = 59,
        S_To_A_Turn_180_Not_Turn_Back_Mode = 60
    }


    public enum eMESTraceLevel
    {
        M = 1, //‘M’ – Machine
        U = 2, //‘U’ – Unit
        P = 3  //‘P’ – Port
    }
    /* 0：Unused
 1：TFT Glass
 2：CF Glass
 3：Dummy Glass
 4：Daily Monitor Glass
 5：Through Dummy Glass
 6：Thickness Dummy Glass
 7：UV Mask Glass*/

    public enum eJobType
    {
        Unknown = 0,
        /// <summary>
        /// Normal TFT Product
        /// </summary>
        Production = 1,
        /// <summary>
        /// Normal CF Product
        /// </summary>
        Dummy = 2,
        /// <summary>
        /// General Dummy
        /// </summary>
        ITODummy = 3,
        /// <summary>
        /// Daily Monitor 
        /// </summary>
        LensCleanerDummy = 4,
        /// <summary>
        /// Through Dummy
        /// </summary>
        Coater1Dummy = 5,
        /// <summary>
        /// Thickness Dummy
        /// </summary>
        Coater2Dummy = 6,
        /// <summary>
        /// UV Mask
        /// </summary>
        UV = 7
    }

    public enum eJobJudge
    {
        /// <summary>
        /// 0
        /// </summary>
        Unused = 0,

        /// <summary>
        /// 1
        /// </summary>
        OK = 1,

        /// <summary>
        /// 2
        /// </summary>
        NG = 2,

        /// <summary>
        /// 3
        /// </summary>
        GrayU = 3,

        /// <summary>
        /// 4
        /// </summary>
        GrayV = 4,

        /// <summary>
        /// 5
        /// </summary>
        GrayW = 5,

        /// <summary>
        /// 6
        /// </summary>
        Repair = 6,

        /// <summary>
        /// 7
        /// </summary>
        InkRepair = 7,

        /// <summary>
        /// 8
        /// </summary>
        Scrap = 8,

        /// <summary>
        /// 9
        /// </summary>
        AbsoluteX = 9,

        /// <summary>
        /// 10
        /// </summary>
        GrayA = 10
    }

    public enum eOPISubCstState
    {
        NONE = 0,
        WACSTEDIT = 1,
        WAREMAPEDIT = 2,
        WASTART = 3
    }

    public enum eJobEvent
    {
        Deliver = 0, Receive = 1, Store = 2, FetchOut = 3, Remove = 4,
        Create = 5, Delete = 6, Recovery = 7, Hold = 8, Request = 9,
        Delete_CST_Complete = 10, EQP_NEW = 11, Edit = 12, CUT_CREATE_CHIP = 13,
        Assembly = 14, VCR_Report = 15, VCR_Mismatch = 16, VCR_Mismatch_Copy = 17, Assembly_NG = 18,
        Rework = 19, Dispatch = 20, Scrap = 21, Modify = 22
    }


    //0：Unused
    //1：OK
    //2：OK, Retry
    //3：NG, Key In
    //4：NG, Bypass
    //5~Other：By Engineer Define
    public enum eGLASSIDREAD_RESULT
    {
        UNUSED = 0,
        OK = 1,
        OK_RETRY = 2,
        NG_KEY_IN = 3,
        NG_BYPASS = 4,
        MISMATCH = 5,
        OTHER = 6
    }
    /*0：Not Use
       1：Job Remove
       2：Job Recovery*/
    public enum eJobCommand
    {
        NOUSE = 0,
        JOBREMOVE = 1,
        JOBRECOVERY = 2
    }

    /*- 0：Not Cutting
      - 1：OLS Cutting OK 
      - 2：LSC Cutting OK
      - 3：Cutting NG
    */
    public enum eCUTTING_FLAG
    {
        NOT_CUTTING = 0,
        OLS_CUTTING_OK = 1,
        LSC_CUTTING_OK = 2,
        CUTTING_NG = 3
    }

    /*- 1：OK, To Short Cut Equipment
      - 2：OK, To Unloader CST
      - 3：NG
    */
    public enum eGlassOutResult
    {
        OK_ToShortCut = 1,
        OK_ToUnloader = 2,
        NG = 3
    }

    public enum eShortCutMode
    {
        Enable = 1,
        Disable = 2
    }

    public enum eUPKEquipmentRunMode
    {
        TFT = 1,
        CF = 2
    }

    public enum ePermitFlag
    {
        Y = 1,
        N = 2,
        M = 3,
        F = 4
    }

    public enum eWaitCassetteStatus
    {
        UNKNOWN = 0,
        NotWaitCassette = 1,
        W_CST = 2
    }


    public enum ePalletMode
    {
        UNKNOWN = 0,
        PACK = 1,                /*Packing*/
        UNPACK = 2                 /*Un-Packing*/
    }

    public enum eCASSETTE_MAP_FLAG
    {
        CASSETTE_MAP_EMPTY = 0,
        CASSETTE_MAP_EXIST = 1
    }

    public enum eMask_Status
    {
        UNKNOW = 0,
        CLEAN = 1,
        WAIT = 2,
        MS_WAIT = 3,
        MS_DONE_WAIT = 4,
        MS_SPIN_IN = 5,
        MS_SPIN_OUT = 6,
        MS_STK_OUT = 7
    }


    public enum eCELLATSOperPermission
    {
        UNKNOW = 0,
        Request = 1,
        Complete = 2
    }
    public enum eboxReport  //Remove Report
    {
        NOReport = 0,
        NOProcess = 1,
        Processing = 2
    }
    public enum eVirtualPortMode  //VirtualPortMode For Robot
    {
        NotUse = 0,
        NormalMode = 1,
        LDVirtualPortMode = 2,
        ULDVirtualPortMode = 3
    }

    public enum eRecipeCheckMode
    {
        UNKNOW = 0,
        Auto = 1,
        Manual = 2
    }
    /// <summary>
    /// For ProductInOutTotal Message 使用，
    /// 正常情况使用NORMAL，CUT Line 使用Cutting，CUT Line Unload 使用CUT Unload
    /// 20150421 tom 
    /// </summary>
    public enum eProductInOutTotalFlag
    {
        NORMAL = 0,
        CUTTING = 1,
        UNLOAD = 2
    }


    public enum ePortModeProductTypeCheck
    {
        UNKNOWN = 0,
        ProductTypeCheck = 1,
        NoProductTypeCheck = 2
    }

    public enum eTankEvent
    {
        UNKNOW = 0,
        Start = 1,
        End = 2
    }

    public enum eDispatchMode
    {

        //0：Unused
        //1：Dispatch Local Mode
        //2：Dispatch Remote Mode
        UNUSED = 0,
        LOCAL_MODE = 1,
        REMOTE_MODE = 2

    }

    public enum eCSTDataUploadEndReasonCode
    {
        //0：Unused
        //1：OK, Normal Completed
        //2：OK, BC Cancel
        //3：OK, BC Abort
        //4：OK, EQ Auto Cancel
        //5：OK, EQ Auto Abort
        //6：OK, Operator Cancel
        //7：OK, Operator Abort
        //8：NG, Error
        //9 ~：Other, Define by CIM
        UNUSED = 0,
        OK_Normal_Completed = 1,
        OK_BC_Cancel = 2,
        OK_BC_Abort = 3,
        OK_EQ_Auto_Cancel = 4,
        OK_EQ_Auto_Abort = 5,
        OK_Operator_Cancel = 6,
        OK_Operator_Abort = 7,
        NG_Error = 8,
        Other = 9

    }
    public enum eForceClearOutMode
    {
        UNUSE = 0,
        ForceClearOutEnable = 1,
        ForceClearOutDisable = 2,
        ForceClearCommand = 3 //正在下Command
    }

    public enum eSupplyStop
    {
        NOSupplyStop = 0,
        SupplyStop = 1,
        Pause = 2,
    }

    public enum eSecondVerifyStatus
    {
        None = 0,       //未報過二次Verify
        Inprocess = 1,  //現在報的是二次Verify
        OK = 2,         //二次Verify MES Reply OK
        NG = 3,         //二次Verify MES Reply NG
        NoCassette = 4  //Cassette不存在
    }

    public enum eBatchStatus
    {
        Start = 0,
        Executing = 1,
        End = 2,
    }

    public enum eSamplingStatus
    {
        Start = 0,
        Executing = 1,
        End = 2
    }

    public enum eBufferMode
    {
        Unused = 0,
        Normal = 1,
        Dummy = 2
        /*  0：Unused
            1：Normal Mode
            2：Dummy Mode

            */
    }

    /* Sorter Plan Status
     0: Not Start
     1: Normal Complete
     2: Abort
     3: Slot mapping Error
     4: Others
     * */
    public enum ePlanStatus
    {
        NoStart = 0,
        NormalComplete = 1,
        Abort = 2,
        SlotMappingError = 3,
        Others = 4,
        Start = 5,
        ForceComplete = 6,
        Logon = 7,
        HavePlan = 8
    }

    public enum eSubscribeType
    {
        JobKey = 0,
        NGMark = 1,
        JobId = 2
    }


    public enum eSubscribeState
    {
        Create = 0,
        Wait = 1,
        Complete = 2
    }

    //20161130 add for Get Robot Abnormal StepID. 
    /// <summary>請與UniAuto.UniRCS.RobotDLL.HKC_ARRAY.D20169999_S999 public class E_STEPID同步更新
    /// 
    /// </summary>
    public class ROBOT_SPECIAL_STEPID
    {
        /// <summary>
        /// -5: Source Cassette Abort
        /// </summary>
        public const int SOURCE_CST_ABORT = -5;

        /// <summary>
        /// -4: SendOut 區塊的 GlassNumberCode 是 0_0
        /// </summary>
        public const int NO_GLASS_DATA = -4;

        /// <summary>
        /// -3: ForceCleanOut, 已經回放原 Cassette 原 Slot
        /// </summary>
        public const int FORCE_CLEAN_OUT = -3;

        /// <summary>
        /// -2: 無法計算 RealStepID
        /// </summary>
        public const int NO_REAL_STEP = -2;

        /// <summary>
        /// -1: 找不到 Job
        /// </summary>
        public const int MISS_JOB = -1;

        /// <summary>
        /// 0: 找不到 Route
        /// </summary>
        public const int NO_ROUTE = 0;

        /// <summary>
        /// 1: Route 第一步
        /// </summary>
        public const int INIT_STEP = 1;

        /// <summary>
        /// 65535: Route 最後一步
        /// </summary>
        public const int FINAL_STEP = 65535;
    }

    //20170331 add QTime Event
    public enum eQTimeEvent
    {
        START = 0, END = 1
    }

}
