///Equipment Object
///
using KZONE.Entity;
using KZONE.PLCAgent.PLC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KZONE.Entity
{
    /// <summary>
    /// 對應File, 修改Property後呼叫Save(), 會序列化存檔
    /// </summary>
    [Serializable]
    public class EquipmentEntityFile : EntityFile
    {


        #region Common Data
        private eEQPStatus _status = eEQPStatus.Unused;
        private eBitResult _cIMMode = eBitResult.OFF;
        private eBitResult _connection = eBitResult.OFF;

        private eEQPOperationMode _equipmentOperationMode = eEQPOperationMode.MANUAL;
        private eWaitCassetteStatus _waitCassetteStatus = eWaitCassetteStatus.UNKNOWN;
        private eEnableDisable _autoRecipeChangeMode = eEnableDisable.Disable;
        private eEnableDisable _recipeCheckMode = eEnableDisable.Disable;
        private eEnableDisable _passMode = eEnableDisable.Disable;
        private List<eEnableDisable> _dcrEnableMode = new List<eEnableDisable>();
        private eTurnTableMode _turnTableMode = eTurnTableMode.Disable;
        private eEnableDisable _autoControlMode = eEnableDisable.Disable;
        private eForceClearOutMode _forceClearOutMode = eForceClearOutMode.UNUSE;
        private eEnableDisable _vcrMode = eEnableDisable.Disable;
        private eEnableDisable _tR01TransferEnable = eEnableDisable.Disable;
        private eEnableDisable _tR02TransferEnable = eEnableDisable.Disable;

        private eTransfer _lastRequestTransfer = eTransfer.NONE;

        private DateTime _lastAliveTime = DateTime.Now;
        private eBitResult _eqpAlive = eBitResult.OFF;
        private bool _aliveTimeout = false;
        private string cassetteOperationMode = string.Empty;

        private eBitResult _alarm = eBitResult.OFF;
        private eEnableDisable _glassCheckMode = eEnableDisable.Disable;
        private eEnableDisable _supplyStartSignal = eEnableDisable.Disable;

        private eReceiveStep _receiveStep = eReceiveStep.BitAllOff;
        private eReceiveStep _nextReceiveStep = eReceiveStep.BitAllOff;
        private eReceiveStep _receiveStepUP = eReceiveStep.BitAllOff;
        private eReceiveStep _nextReceiveStepUP = eReceiveStep.BitAllOff;
        private eReceiveStep _receiveStepReturn = eReceiveStep.BitAllOff;
        private eReceiveStep _nextReceiveStepReturn = eReceiveStep.BitAllOff;

        private eSendStep _sendStep = eSendStep.BitAllOff;
        private eSendStep _nextSendStep = eSendStep.BitAllOff;
        private eSendStep _sendStepUP = eSendStep.BitAllOff;
        private eSendStep _nextSendStepUP = eSendStep.BitAllOff;
        private eSendStep _sendStepReturn = eSendStep.BitAllOff;
        private eSendStep _nextSendStepReturn = eSendStep.BitAllOff;
        public eReceiveStep DownResum { get; set; }
        public eSendStep UPResum { get; set; }

        private eEnableDisable _deBugMode = eEnableDisable.Disable;
        private eBitResult upReciveComplete = eBitResult.OFF;
        private eBitResult upReciveCompleteUP = eBitResult.OFF;
        private eBitResult receiveCompleteReturn = eBitResult.OFF;
        private eBitResult downSendComplete = eBitResult.OFF;
        private eBitResult downSendCompleteUP = eBitResult.OFF;
        private eBitResult sendCompleteReturn = eBitResult.OFF;

        private eBitResult upReciveCancel = eBitResult.OFF;
        private eBitResult upReciveCancelUP = eBitResult.OFF;
        private eBitResult upReciveResum = eBitResult.OFF;
        private eBitResult upReciveResumUP = eBitResult.OFF;
        private eBitResult receiveCancelReturn = eBitResult.OFF;
        private eBitResult receiveResumReturn = eBitResult.OFF;

        private eBitResult downSendCancel = eBitResult.OFF;
        private eBitResult downSendCancelUP = eBitResult.OFF;
        private eBitResult downSendResum = eBitResult.OFF;
        private eBitResult downSendResumUP = eBitResult.OFF;
        private eBitResult sendCancelReturn = eBitResult.OFF;
        private eBitResult sendResumReturn = eBitResult.OFF;
        public string UPDataCheckResult { get; set; }
        public string GroupIndex { get; set; }

        public string PortLot01 { get; set; }
        public string PortLot02 { get; set; }
        public string PortLotCount01 { get; set; }
        public string PortLotCount02 { get; set; }

        private string _alarmID = String.Empty;
        private eBitResult stopCommand = eBitResult.OFF;

        private eEnableDisable _productTypeCheckMode = eEnableDisable.Disable;
        private eEnableDisable _groupIndexCheckMode = eEnableDisable.Disable;
        private eEnableDisable _productIDCheckMode = eEnableDisable.Disable;
        private eEnableDisable _jobDuplicateCheckMode = eEnableDisable.Disable;
        private eEnableDisable _upStreamPerMode = eEnableDisable.Disable;
        private eEnableDisable _dowmStreamPerMode = eEnableDisable.Disable;
        private eEnableDisable _loadingStopMode = eEnableDisable.Disable;
        private List<CIMMESSAGEHISTORY> _setCimMesg = new List<CIMMESSAGEHISTORY>();
        private List<RecipeValidationResultHistory> _setRMSMesg = new List<RecipeValidationResultHistory>();
        private bool _recipeValidationMessageDisplay = false;
        private bool _recipeValidationMessageUpdate = false;
        private string _plcSoftwareVersion = string.Empty;
        private string _touchPanelNo = string.Empty;

        private int _cvReportTime = 0;
        private int _cvReportCount = 0;
        private int _loadingStopCommandStatus = 0;

        private MaterialEntity _currentMaterial = new MaterialEntity();
        private MaterialEntity _reportMaterial = new MaterialEntity();

        private eMaterialStatus _currentMaterialStatus = eMaterialStatus.UNUSED;
        private string _currentMaterialID = string.Empty;
        public int LoadingStopCommandStatus
        {
            get { return _loadingStopCommandStatus; }
            set { _loadingStopCommandStatus = value; }
        }

        public int CVReportTime
        {
            get { return _cvReportTime; }
            set { _cvReportTime = value; }
        }

        public int CVReportCount
        {
            get { return _cvReportCount; }
            set { _cvReportCount = value; }
        }

        public string TouchPanelNo
        {
            get
            {
                return _touchPanelNo;
            }
            set
            {
                _touchPanelNo = value;
            }
        }
        public string PLCSoftwareVersion
        {
            get { return _plcSoftwareVersion; }
            set { _plcSoftwareVersion = value; }
        }

        public eEnableDisable LoadingStopMode
        {
            get { return _loadingStopMode; }
            set { _loadingStopMode = value; }
        }


        public eEnableDisable UpStreamPerMode
        {
            get { return _upStreamPerMode; }
            set { _upStreamPerMode = value; }

        }
        public eEnableDisable DowmStreamPerMode
        {
            get { return _dowmStreamPerMode; }
            set { _dowmStreamPerMode = value; }

        }


        public List<CIMMESSAGEHISTORY> SetCimMesg
        {
            get { return _setCimMesg; }
            set { _setCimMesg = value; }
        }

        public List<RecipeValidationResultHistory> SetRMSMesg
        {
            get { return _setRMSMesg; }
            set { _setRMSMesg = value; }
        }

        public bool RecipeValidationMessageDisplay
        {
            get { return _recipeValidationMessageDisplay; }
            set { _recipeValidationMessageDisplay = value; }
        }

        public bool RecipeValidationMessageUpdate
        {
            get { return _recipeValidationMessageUpdate; }
            set { _recipeValidationMessageUpdate= value; }
        }

        public eBitResult StopCommand
        {
            get { return stopCommand; }
            set { stopCommand = value; }
        }

        public eBitResult UPReciveComplete
        {
            get { return upReciveComplete; }
            set { upReciveComplete = value; }
        }

        public eBitResult UPReciveCompleteUP
        {
            get { return upReciveCompleteUP; }
            set { upReciveCompleteUP = value; }
        }

        public eBitResult ReceiveCompleteReturn
        {
            get { return receiveCompleteReturn; }
            set { receiveCompleteReturn = value; }
        }

        public eBitResult DownSendComplete
        {
            get { return downSendComplete; }
            set { downSendComplete = value; }
        }
        public eBitResult DownSendCompleteUP
        {
            get { return downSendCompleteUP; }
            set { downSendCompleteUP = value; }
        }
        public eBitResult SendCompleteReturn
        {
            get { return sendCompleteReturn; }
            set { sendCompleteReturn = value; }
        }
        public eBitResult UPReciveCancel
        {
            get { return upReciveCancel; }
            set { upReciveCancel = value; }
        }
        public eBitResult UPReceiveCancelUP
        {
            get { return upReciveCancelUP; }
            set { upReciveCancelUP = value; }
        }
        public eBitResult ReceiveCancelReturn
        {
            get { return receiveCancelReturn; }
            set { receiveCancelReturn = value; }
        }

        public eBitResult DownSendCancel
        {
            get { return downSendCancel; }
            set { downSendCancel = value; }
        }
        public eBitResult DownSendCancelUP
        {
            get { return downSendCancelUP; }
            set { downSendCancelUP = value; }
        }
        public eBitResult SendCancelReturn
        {
            get { return sendCancelReturn; }
            set { sendCancelReturn = value; }
        }
        public eBitResult UPReciveResum
        {
            get { return upReciveResum; }
            set { upReciveResum = value; }
        }
        public eBitResult UPReceiveResumUP
        {
            get { return upReciveResumUP; }
            set { upReciveResumUP = value; }
        }
        public eBitResult ReceiveResumReturn
        {
            get { return receiveResumReturn; }
            set { receiveResumReturn = value; }
        }

        public eBitResult DownSendResum
        {
            get { return downSendResum; }
            set { downSendResum = value; }
        }
        public eBitResult DownSendResumUP
        {
            get { return downSendResumUP; }
            set { downSendResumUP = value; }
        }
        public eBitResult SendResumReturn
        {
            get { return sendResumReturn; }
            set { sendResumReturn = value; }
        }
        public eEnableDisable ProductTypeCheckMode
        {
            get { return _productTypeCheckMode; }
            set { _productTypeCheckMode = value; }

        }
        public eEnableDisable GroupIndexCheckMode
        {
            get { return _groupIndexCheckMode; }
            set { _groupIndexCheckMode = value; }

        }

        public eEnableDisable ProductIDCheckMode
        {
            get { return _productIDCheckMode; }
            set { _productIDCheckMode = value; }

        }
        public eEnableDisable JobDuplicateCheckMode
        {
            get { return _jobDuplicateCheckMode; }
            set { _jobDuplicateCheckMode = value; }

        }

        public string AlarmID
        {
            get { return _alarmID; }
            set { _alarmID = value; }
        }

        public eEnableDisable PassMode
        {
            get { return _passMode; }
            set { _passMode = value; }
        }

        public eEnableDisable DeBugMode
        {
            get { return _deBugMode; }
            set { _deBugMode = value; }
        }

        public eReceiveStep ReceiveStep
        {
            get { return _receiveStep; }
            set { _receiveStep = value; }
        }

        public eReceiveStep NextReceiveStep
        {
            get { return _nextReceiveStep; }
            set { _nextReceiveStep = value; }
        }

        public eReceiveStep ReceiveStepUP
        {
            get { return _receiveStepUP; }
            set { _receiveStepUP = value; }
        }

        public eReceiveStep NextReceiveStepUP
        {
            get { return _nextReceiveStepUP; }
            set { _nextReceiveStepUP = value; }
        }

        public eReceiveStep ReceiveStepReturn
        {
            get { return _receiveStepReturn; }
            set { _receiveStepReturn = value; }
        }

        public eReceiveStep NextReceiveStepReturn
        {
            get { return _nextReceiveStepReturn; }
            set { _nextReceiveStepReturn = value; }
        }

        public eSendStep SendStep
        {
            get { return _sendStep; }
            set { _sendStep = value; }
        }

        public eSendStep NextSendStep
        {
            get { return _nextSendStep; }
            set { _nextSendStep = value; }
        }
        public eSendStep SendStepUP
        {
            get { return _sendStepUP; }
            set { _sendStepUP = value; }
        }

        public eSendStep NextSendStepUP
        {
            get { return _nextSendStepUP; }
            set { _nextSendStepUP = value; }
        }

        public eSendStep SendStepReturn
        {
            get { return _sendStepReturn; }
            set { _sendStepReturn = value; }
        }

        public eSendStep NextSendStepReturn
        {
            get { return _nextSendStepReturn; }
            set { _nextSendStepReturn = value; }
        }

        public eBitResult Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }


        public eBitResult Alarm
        {
            get { return _alarm; }
            set { _alarm = value; }
        }
        private eBitResult _bcAlive = eBitResult.OFF;

        public eBitResult BCAlive
        {
            get { return _bcAlive; }
            set { _bcAlive = value; }
        }

        private int waterLevel = 0;

        public int WaterLevel
        {
            get { return waterLevel; }
            set { waterLevel = value; }
        }

        private eBitResult addWater = eBitResult.OFF;

        public eBitResult AddWater
        {
            get { return addWater; }
            set { addWater = value; }
        }

        private eBitResult disWater = eBitResult.OFF;

        public eBitResult DisWater
        {
            get { return disWater; }
            set { disWater = value; }
        }

        private int daliyCount = 0;

        public int DaliyCount
        {
            get { return daliyCount; }
            set { daliyCount = value; }
        }
        private int eqpCount = 0;

        public int EqpCount
        {
            get { return eqpCount; }
            set { eqpCount = value; }
        }
        private string vcrReadID = string.Empty;

        public string VcrReadID
        {
            get { return vcrReadID; }
            set { vcrReadID = value; }
        }
        private string totelWater = string.Empty;

        public string TotelWater
        {
            get { return totelWater; }
            set { totelWater = value; }
        }

        private string waterFlow = string.Empty;

        public string WaterFlow
        {
            get { return waterFlow; }
            set { waterFlow = value; }
        }

        private string oprationName = string.Empty;

        public string OprationName
        {
            get { return oprationName; }
            set { oprationName = value; }
        }


        public string CassetteOperationMode
        {
            get { return cassetteOperationMode; }
            set { cassetteOperationMode = value; }
        }
        private string portQtime = string.Empty;

        public string PortQtime
        {
            get { return portQtime; }
            set { portQtime = value; }
        }


        private DateTime _bclastAliveTime = DateTime.Now;

        public DateTime BClastAliveTime
        {
            get { return _bclastAliveTime; }
            set { _bclastAliveTime = value; }
        }

        private bool _bcaliveTimeout = false;

        public bool BCaliveTimeout
        {
            get { return _bcaliveTimeout; }
            set { _bcaliveTimeout = value; }
        }

        private eBitResult _upInlineMode = eBitResult.OFF;

        public eBitResult UpInlineMode
        {
            get { return _upInlineMode; }
            set { _upInlineMode = value; }
        }

        private eBitResult _downInlineMode = eBitResult.OFF;

        public eBitResult DownInlineMode
        {
            get { return _downInlineMode; }
            set { _downInlineMode = value; }
        }

        public eEnableDisable VCRMode
        {
            get { return _vcrMode; }
            set { _vcrMode = value; }
        }

        private DateTime _fdcLastDT = DateTime.Now;
        private int _fdcIntervalMS;
        private DateTime _dailyCheckLastDT = DateTime.Now;
        private int _dailyCheckIntervalS;

        private eBitResult _currentAlarmStatus = eBitResult.OFF;
        private string _currentRecipeID = "AA";
        private string _equipmentRunMode = string.Empty;
        private string _indexerRunMode = string.Empty;
        private eRTCRunMode _rtcRunMode = eRTCRunMode.Unused;
        public string CurrentHostPPID { get; set; }
        public string DownLoadRecipe { get; set; }

        private int _totalTFTGlassCount = 0;
        private int _totalHFGlassCount = 0;
        private int _dummyGlassCount = 0;
        private int _recipeNo = 0;

        private int _mQCGlassCount = 0;
        private int _uVMaskCount = 0;
        private string _rtcRunModeStatus = string.Empty;

        private int _productType = 0;

        /// <summary>
        /// Normal  Buffer  Information
        /// </summary>
        //public IList<NormalBuffer> NormalBuffers
        //{
        //    get { return _normalBuffers; }
        //    set { _normalBuffers = value; }
        //}
        private IDictionary<int, NormalBuffer> _normalBufferDic = new Dictionary<int, NormalBuffer>();
        public IDictionary<int, NormalBuffer> NormalBufferDic
        {
            get
            {
                if (_normalBufferDic == null)
                {
                    _normalBufferDic = new Dictionary<int, NormalBuffer>();
                }
                return _normalBufferDic;
            }
            set { _normalBufferDic = value; }
        }


        /// <summary>
        /// CF Special Buffer Slot Information
        /// </summary>
        private int _normalBufferSlotCount = 0;
        private int _dailyMonitorBufferSlotCount = 0;
        private eCFBufSlotCommandMode _bufSlotCommandMode;
        //private IList<NormalBuffer> _normalBuffers = new List<NormalBuffer>();  //舊的寫法,新的寫法測試完成後改用Dictionary的方式
        private string _bufferSlotCommandCurretSlotNo = string.Empty;
        private eCFBufSlotCommandStatus _bufferSlotCommandStatus = eCFBufSlotCommandStatus.None;

        public eCFBufSlotCommandStatus BufferSlotCommandStatus
        {
            get { return _bufferSlotCommandStatus; }
            set { _bufferSlotCommandStatus = value; }
        }

        public string BufferSlotCommandCurretSlotNo
        {
            get { return _bufferSlotCommandCurretSlotNo; }
            set { _bufferSlotCommandCurretSlotNo = value; }
        }
        //20161214 Add
        //第1個Dictionary是用bufferNo做Key值,第2個Dictionary是用slotNo做Key值
        //bufferNo不是指Operation Scenario的bufferNo,是指真正有幾個buffer數量
        //HKC目前都只有1個buffer,未來如果增加為2個buffe,就增加Key值bufferNo=2
        private Dictionary<string, Dictionary<string, NormalBuffer>> _buffers;//20161214 Add
        public Dictionary<string, Dictionary<string, NormalBuffer>> Buffers
        {
            get
            {
                if (_buffers == null)
                {
                    _buffers = new Dictionary<string, Dictionary<string, NormalBuffer>>();
                }
                return _buffers;
            }
            set { _buffers = value; }
        }
        private Dictionary<string, NormalBuffer> _buffer;

        public Dictionary<string, NormalBuffer> Buffer
        {
            get
            {
                if (_buffer == null)
                {
                    _buffer = new Dictionary<string, NormalBuffer>();
                }
                return _buffer;
            }
            set { _buffer = value; }
        }

        public int NormalBufferSlotCount
        {
            get { return _normalBufferSlotCount; }
            set { _normalBufferSlotCount = value; }
        }

        public int DailyMonitorBufferSlotCount
        {
            get { return _dailyMonitorBufferSlotCount; }
            set { _dailyMonitorBufferSlotCount = value; }
        }

        public eCFBufSlotCommandMode BufSlotCommandMode
        {
            get { return _bufSlotCommandMode; }
            set { _bufSlotCommandMode = value; }
        }

        //private string _mplcInterlockState = string.Empty;
        private string _mesStatus = "DOWN";

        private string _hSMSControlMode = "OFF-LINE";

        private IDictionary<string, List<string>> _targetEqps = new Dictionary<string, List<string>>();

        private IDictionary<int, Tuple<string, int>> _currentBatchs = new Dictionary<int, Tuple<string, int>>();

        public IDictionary<string, List<string>> TargetEqps
        {
            get
            {
                if (_targetEqps == null)
                {
                    _targetEqps = new Dictionary<string, List<string>>();
                }
                return _targetEqps;
            }
            set { _targetEqps = value; }
        }

        /// <summary>
        /// 当前正在使用的Batch
        /// </summary>
        public IDictionary<int, Tuple<string, int>> CurrentBatchs
        {
            get
            {
                if (_currentBatchs == null)
                {
                    _currentBatchs = new Dictionary<int, Tuple<string, int>>();
                }
                return _currentBatchs;
            }
            set { _currentBatchs = value; }
        }

        //For SECS  Equipment    
        public string HSMSControlMode
        {
            get { return _hSMSControlMode; }
            set { _hSMSControlMode = value; }
        }

        public eEnableDisable AutoControlMode
        {
            get { return _autoControlMode; }
            set { _autoControlMode = value; }
        }

        /// <summary>
        /// Current Group No 
        /// </summary>
        public int CurrentRecipeNo
        {
            get { return _recipeNo; }
            set { _recipeNo = value; }
        }

        /// <summary>
        /// DCR(VCR) Enable Disable Mode
        /// </summary>
        public List<eEnableDisable> DCREnableMode
        {
            get { return _dcrEnableMode; }
            set { _dcrEnableMode = value; }
        }

        /// <summary>
        /// CIM  Mode
        /// </summary>
        public eBitResult CIMMode
        {
            get { return _cIMMode; }
            set { _cIMMode = value; }
        }

        public eBitResult EqpAlive
        {
            get { return _eqpAlive; }
            set { _eqpAlive = value; }
        }

        private eBitResult _eqpPassMode = eBitResult.OFF;

        /// <summary>
        /// Eqp Pass Mode
        /// </summary>
        public eBitResult EQPPassMode
        {
            get { return _eqpPassMode; }
            set { _eqpPassMode = value; }
        }

        public eEQPOperationMode EquipmentOperationMode
        {
            get { return _equipmentOperationMode; }
            set { _equipmentOperationMode = value; }
        }

        /// <summary>
        /// Auto Rcipe Change Mode 
        /// </summary>
        public eEnableDisable AutoRecipeChangeMode
        {
            get { return _autoRecipeChangeMode; }
            set { _autoRecipeChangeMode = value; }
        }

        public eTurnTableMode TurnTableMode
        {
            get { return _turnTableMode; }
            set { _turnTableMode = value; }
        }

        public DateTime DailyCheckLastDT
        {
            get { return _dailyCheckLastDT; }
            set { _dailyCheckLastDT = value; }
        }

        /// <summary>
        /// Daily Check处理间隔时间
        /// </summary>
        public int DailyCheckIntervalS
        {
            get { return _dailyCheckIntervalS; }
            set { _dailyCheckIntervalS = value; }
        }

        /// <summary>
        /// FDC最后一次Report时间
        /// </summary>
        public DateTime FDCLastDT
        {
            get { return _fdcLastDT; }
            set { _fdcLastDT = value; }
        }

        /// <summary>
        /// FDC Report间隔时间(ms)
        /// </summary>
        public int FDCIntervalMS
        {
            get { return _fdcIntervalMS; }
            set { _fdcIntervalMS = value; }
        }

        /// <summary>
        /// Equipment Status
        /// </summary>
        public eEQPStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public MaterialEntity CurrentMaterial
        {
            get
            {
                if (_currentMaterial == null)
                {
                    _currentMaterial = new MaterialEntity();
                }
                return _currentMaterial;
            }
            set { _currentMaterial = value; }
        }

        public MaterialEntity ReportMaterial
        {
            get
            {
                if (_reportMaterial == null)
                {
                    _reportMaterial = new MaterialEntity();
                }
                return _reportMaterial;
            }
            set { _reportMaterial = value; }
        }

        public eMaterialStatus CurrentMaterialStatus
        {
            get { return _currentMaterialStatus; }
            set { _currentMaterialStatus = value; }
        }

        public string CurrentMaterialID
        {
            get
            {
                if (_currentMaterialID == null)
                {
                    _currentMaterialID = string.Empty;
                }
                return _currentMaterialID;
            }
            set { _currentMaterialID = value; }
        }

        /// <summary>
        /// 最后Alive 上报的时间
        /// </summary>
        public DateTime LastAliveTime
        {
            get { return _lastAliveTime; }
            set { _lastAliveTime = value; }
        }

        /// <summary>
        /// BC Alive 是否有Time out
        /// </summary>
        public bool AliveTimeout
        {
            get { return _aliveTimeout; }
            set { _aliveTimeout = value; }
        }


        /// <summary>
        ///  机台的Alarm status
        /// </summary>
        public eBitResult CurrentAlarmStatus
        {
            get { return _currentAlarmStatus; }
            set { _currentAlarmStatus = value; }
        }

        /// <summary>
        /// 当前的Recipe
        /// </summary>
        public string CurrentRecipeID
        {
            get { return _currentRecipeID; }
            set { _currentRecipeID = value; }
        }

        /// <summary>
        /// 当前的机台的Run Mode
        /// </summary>
        public string EquipmentRunMode
        {
            get { return _equipmentRunMode; }
            set { _equipmentRunMode = value; }
        }

        /// <summary>
        /// 當前機台的IndexerRunMode
        /// </summary>
        public string IndexerRunMode
        {
            get { return _indexerRunMode; }
            set { _indexerRunMode = value; }
        }

        /// <summary>
        /// Recipe Check Mode 
        /// </summary>
        public eEnableDisable RecipeCheckMode
        {
            get { return _recipeCheckMode; }
            set { _recipeCheckMode = value; }
        }
        /// <summary>
        /// Glass Check Mode 
        /// </summary>
        public eEnableDisable GlassCheckMode
        {
            get { return _glassCheckMode; }
            set { _glassCheckMode = value; }
        }
        /// <summary>
        /// Supply Start Signal
        /// </summary>
        public eEnableDisable SupplyStartSignal
        {
            get { return _supplyStartSignal; }
            set { _supplyStartSignal = value; }
        }


        /// <summary>
        ///
        /// </summary>
        public int TotalTFTGlassCount
        {
            get { return _totalTFTGlassCount; }
            set { _totalTFTGlassCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalHFGlassCount
        {
            get { return _totalHFGlassCount; }
            set { _totalHFGlassCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DummyGlassCount
        {
            get { return _dummyGlassCount; }
            set { _dummyGlassCount = value; }
        }


        /// <summary>
        ///
        /// </summary>
        public int MQCGlassCount
        {
            get { return _mQCGlassCount; }
            set { _mQCGlassCount = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int UVMaskCount
        {
            get { return _uVMaskCount; }
            set { _uVMaskCount = value; }
        }

        public int ProductType
        {
            get { return _productType; }
            set { _productType = value; }
        }

        /// <summary>
        ///  RTC RUN MODE Status
        /// </summary>
        public string RtcRunModeStatus
        {
            get { return _rtcRunModeStatus; }
            set { _rtcRunModeStatus = value; }
        }

        /// <summary>
        /// MESStatus
        /// </summary>
        public string MESStatus
        {
            get { return _mesStatus; }
            set { _mesStatus = value; }
        }


        /// <summary>
        /// CF CV1  RTC  Run Mode 
        /// </summary>
        public eRTCRunMode RTCRunMode
        {
            get { return _rtcRunMode; }
            set { _rtcRunMode = value; }
        }


        public eForceClearOutMode ForceClearOutMode
        {
            get { return _forceClearOutMode; }
            set { _forceClearOutMode = value; }
        }
        public eEnableDisable TR01TransferEnable
        {
            get { return _tR01TransferEnable; }
            set { _tR01TransferEnable = value; }
        }
        public eEnableDisable TR02TransferEnable
        {
            get { return _tR02TransferEnable; }
            set { _tR02TransferEnable = value; }
        }

        public eTransfer LastRequestTransfer
        {
            get { return _lastRequestTransfer; }
            set { _lastRequestTransfer = value; }
        }
        private Dictionary<int, string> _reasonCode = new Dictionary<int, string>();
        private IDictionary<int, string> _downPIOBit = new Dictionary<int, string>();
        private IDictionary<int, string> _upInterface01 = new Dictionary<int, string>();
        private IDictionary<int, string> _upInterface02 = new Dictionary<int, string>();
        private IDictionary<int, string> _upInterface03 = new Dictionary<int, string>();

        private IDictionary<int, string> _downInterface01 = new Dictionary<int, string>();
        private IDictionary<int, string> _downInterface02 = new Dictionary<int, string>();
        private IDictionary<int, string> _downInterface03 = new Dictionary<int, string>();
        private IDictionary<int, string> _upActionMonitor = new Dictionary<int, string>();
        private IDictionary<int, string> _DownActionMonitor = new Dictionary<int, string>();
        private ConcurrentDictionary<int, Job> _positionJobs = new ConcurrentDictionary<int, Job>();
        private ConcurrentDictionary<int, Job> _storeJobs = new ConcurrentDictionary<int, Job>();
        private ConcurrentDictionary<int, Job> _fetchJobs = new ConcurrentDictionary<int, Job>();
        private Dictionary<int, TankHistory> _addtankHis = new Dictionary<int, TankHistory>();
        private Dictionary<int, TankHistory> _subtanHis = new Dictionary<int, TankHistory>();
        private Dictionary<Job, Trx> _processDataJobs = new Dictionary<Job, Trx>();
        private List<Job> _tackTimeJobs = new List<Job>();
        private ConcurrentDictionary<string, LotEndGlassInfos> _lotEndGlassInofs = new ConcurrentDictionary<string, LotEndGlassInfos>();
        private ConcurrentDictionary<string, PPIDPreDownloadInfos> _ppIDPreDownloadInfos = new ConcurrentDictionary<string, PPIDPreDownloadInfos>();
        public ConcurrentDictionary<string, PPIDPreDownloadInfos> PPIDPreDownloadInfos
        {
            get
            {
                if (_ppIDPreDownloadInfos == null)
                {
                    _ppIDPreDownloadInfos = new ConcurrentDictionary<string, PPIDPreDownloadInfos>();
                }
                return _ppIDPreDownloadInfos;
            }
            set
            {
                _ppIDPreDownloadInfos = value;
            }
        }

        public ConcurrentDictionary<string, LotEndGlassInfos> LotEndGlassInfos
        {
            get
            {
                if (_lotEndGlassInofs == null)
                {
                    _lotEndGlassInofs = new ConcurrentDictionary<string, LotEndGlassInfos>();
                }
                return _lotEndGlassInofs;
            }
            set
            {
                _lotEndGlassInofs = value;
            }
        }

        public List<Job> TackTimeJobs
        {
            get
            {
                if (_tackTimeJobs == null)
                {
                    _tackTimeJobs = new List<Job>();
                }
                return _tackTimeJobs;
            }
            set
            {
                _tackTimeJobs = value;
            }
        }
        public Dictionary<Job, Trx> ProcessDataJobs
        {
            get
            {
                if (_processDataJobs == null)
                {
                    _processDataJobs = new Dictionary<Job, Trx>();
                }
                return _processDataJobs;
            }
            set
            {
                _processDataJobs = value;
            }
        }

        public Dictionary<int, TankHistory> SubTankHistory
        {
            get
            {
                if (_subtanHis == null)
                {
                    _subtanHis = new Dictionary<int, TankHistory>();
                }

                return _subtanHis;
            }
            set { _subtanHis = value; }
        }
        public Dictionary<int, TankHistory> ADDTankHistory
        {
            get
            {
                if (_addtankHis == null)
                {
                    _addtankHis = new Dictionary<int, TankHistory>();
                }

                return _addtankHis;
            }
            set { _addtankHis = value; }
        }

        public ConcurrentDictionary<int, Job> StoreJobs
        {
            get
            {
                if (_storeJobs == null)
                {
                    _storeJobs = new ConcurrentDictionary<int, Job>();
                }

                return _storeJobs;
            }
            set { _storeJobs = value; }
        }
        public ConcurrentDictionary<int, Job> FetchJobs
        {
            get
            {
                if (_fetchJobs == null)
                {
                    _fetchJobs = new ConcurrentDictionary<int, Job>();
                }

                return _fetchJobs;
            }
            set { _fetchJobs = value; }
        }
        public ConcurrentDictionary<int, Job> PositionJobs
        {
            get
            {
                if (_positionJobs == null)
                {
                    _positionJobs = new ConcurrentDictionary<int, Job>();
                }

                return _positionJobs;
            }
            set { _positionJobs = value; }
        }
        public Dictionary<int, string> ReasonCode
        {
            get
            {
                if (_reasonCode == null)
                {
                    _reasonCode = new Dictionary<int, string>();
                }

                return _reasonCode;
            }
            set { _reasonCode = value; }
        }

        public IDictionary<int, string> DownPIOBit
        {
            get
            {
                if (_downPIOBit == null)
                {
                    _downPIOBit = new Dictionary<int, string>();
                }

                return _downPIOBit;
            }
            set { _downPIOBit = value; }
        }

        public IDictionary<int, string> UPInterface01
        {
            get
            {
                if (_upInterface01 == null)
                {
                    _upInterface01 = new Dictionary<int, string>();
                }

                return _upInterface01;
            }
            set { _upInterface01 = value; }
        }

        public IDictionary<int, string> UPInterface02
        {
            get
            {
                if (_upInterface02 == null)
                {
                    _upInterface02 = new Dictionary<int, string>();
                }

                return _upInterface02;
            }
            set { _upInterface02 = value; }
        }
        public IDictionary<int, string> UPInterface03
        {
            get
            {
                if (_upInterface03 == null)
                {
                    _upInterface03 = new Dictionary<int, string>();
                }

                return _upInterface03;
            }
            set { _upInterface03 = value; }
        }

        public IDictionary<int, string> DownInterface01
        {
            get
            {
                if (_downInterface01 == null)
                {
                    _downInterface01 = new Dictionary<int, string>();
                }

                return _downInterface01;
            }
            set { _downInterface01 = value; }
        }
        public IDictionary<int, string> DownInterface02
        {
            get
            {
                if (_downInterface02 == null)
                {
                    _downInterface02 = new Dictionary<int, string>();
                }

                return _downInterface02;
            }
            set { _downInterface02 = value; }
        }
        public IDictionary<int, string> DownInterface03
        {
            get
            {
                if (_downInterface03 == null)
                {
                    _downInterface03 = new Dictionary<int, string>();
                }

                return _downInterface03;
            }
            set { _downInterface03 = value; }
        }

        public IDictionary<int, string> UPActionMonitor
        {
            get
            {
                if (_upActionMonitor == null)
                {
                    _upActionMonitor = new Dictionary<int, string>();
                }

                return _upActionMonitor;
            }
            set { _upActionMonitor = value; }
        }

        public IDictionary<int, string> DownActionMonitor
        {
            get
            {
                if (_DownActionMonitor == null)
                {
                    _DownActionMonitor = new Dictionary<int, string>();
                }

                return _DownActionMonitor;
            }
            set { _DownActionMonitor = value; }
        }








        private IDictionary<string, DispatchPoint> _dispatchPoints = new Dictionary<string, DispatchPoint>();

        /// <summary> Dispatch Mode
        ///
        /// </summary>
        public IDictionary<string, DispatchPoint> DispatchPoints
        {
            get
            {
                if (_dispatchPoints == null)
                {
                    _dispatchPoints = new Dictionary<string, DispatchPoint>();
                }
                return _dispatchPoints;
            }
            set { _dispatchPoints = value; }
        }

        private eBitResult _eqpPreSputterMode = eBitResult.OFF;

        /// <summary>Eqp PreSputter Mode Enable/Disable for Sputter Handle
        /// 
        /// </summary>
        public eBitResult EqpPreSputterMode
        {
            get { return _eqpPreSputterMode; }
            set { _eqpPreSputterMode = value; }
        }

        private eBufferMode _eqpBufferMode = eBufferMode.Unused;

        /// <summary>Eqp Buffer Mode Normal/Dummy for F1ISP
        /// 
        /// </summary>
        public eBufferMode EqpBufferMode
        {
            get { return _eqpBufferMode; }
            set { _eqpBufferMode = value; }
        }

        //20161004 add For Block Control
        private int _lastProductGlassGroupNo = 0;

        /// <summary> EQP Recive Last Product Glass's GroupNo(Offline:31~100,Online:131~65535) for Sub Block Control
        ///
        /// </summary>
        public int LastProductGlassGroupNo
        {
            get { return _lastProductGlassGroupNo; }
            set { _lastProductGlassGroupNo = value; }
        }

        //20161209 add for Block Control Mulit Loader Port Node Use
        private int _reserveProductGlassGroupNo = 0;

        /// <summary> EQP Reserve Next Product Glass's GroupNo(Offline:31~100,Online:131~65535) for Sub Block Control Mulit Loader Port Node Use
        ///
        /// </summary>
        public int ReserveProductGlassGroupNo
        {
            get { return _reserveProductGlassGroupNo; }
            set { _reserveProductGlassGroupNo = value; }
        }


        private bool _hasForceClearOutCommand = false;
        /// <summary>
        /// 是否有下ForceClearCommand  2017-01-16 
        /// </summary>
        public bool HasForceClearOutCommand
        {
            get { return _hasForceClearOutCommand; }
            set { _hasForceClearOutCommand = value; }
        }

        //20170131 add Last RecipeName to Create Batch
        private string _currentBatchRecipeName = string.Empty;

        /// <summary> 最後編Batch的RecipeName
        /// 
        /// </summary>
        public string CurrentBatchRecipeName
        {
            get { return _currentBatchRecipeName; }
            set { _currentBatchRecipeName = value; }
        }

        //20170614 add PreEquipmentRunMode
        private string _preEquipmentRunMode = string.Empty;
        /// <summary>
        /// 當前机台的前一個EQP Run Mode
        /// </summary>
        public string PreEquipmentRunMode
        {
            get { return _preEquipmentRunMode; }
            set { _preEquipmentRunMode = value; }
        }

        #endregion
        
        private eEnableDisable _glassIDCheckMode = eEnableDisable.Disable;
        private eEnableDisable _cstSlotNoCheckMode = eEnableDisable.Disable;
        private eEnableDisable _groupNoCheckMode = eEnableDisable.Disable;
        public eEnableDisable GlassIDCheckMode
        {
            get { return _glassIDCheckMode; }
            set { _glassIDCheckMode = value; }
        }

        public eEnableDisable CstSlotNoCheckMode
        {
            get
            {
                return _cstSlotNoCheckMode;
            }
            set
            {
                _cstSlotNoCheckMode = value;
            }
        }
        public eEnableDisable GroupNoCheckMode
        {
            get
            {
                return _groupNoCheckMode;
            }
            set
            {
                _groupNoCheckMode = value;
            }
        }
        private Dictionary<int, eEnableDisable> _tankUseMode = new Dictionary<int, eEnableDisable>();
        public Dictionary<int, eEnableDisable> TankUseMode
        {
            get
            {
                if (_tankUseMode == null)
                {
                    return _tankUseMode = new Dictionary<int, eEnableDisable>();
                }
                return _tankUseMode;
            }
            set
            {
                _tankUseMode = value;
            }
        }
    }

    public class Equipment : Entity
    {
        public EquipmentEntityData Data { get; private set; }

        public EquipmentEntityFile File { get; private set; }

        #region[SECS Special]
        private bool _eventReportConfigurated = false;
        private bool _hsmsConnected = false;
        //private string _secsControlMode = "OFF-LINE";
        private bool _secsCommunicated = false;
        private bool _hsmsSelected = false;
        private string _mDLN = string.Empty;
        private string _sOFTREV = string.Empty;
        private string _hsmsConnStatus = "DISCONNECTED";
        private string _inUseMaskID = string.Empty;

        public string InUseMaskID
        {
            get { return _inUseMaskID; }
            set { _inUseMaskID = value; }
        }

        public string HsmsConnStatus
        {
            get { return _hsmsConnStatus; }
            set { _hsmsConnStatus = value; }
        }

        public string SOFTREV
        {
            get { return _sOFTREV; }
            set { _sOFTREV = value; }
        }

        public string MDLN
        {
            get { return _mDLN; }
            set { _mDLN = value; }
        }

        public bool HsmsSelected
        {
            get { return _hsmsSelected; }
            set { _hsmsSelected = value; }
        }

        public bool SecsCommunicated
        {
            get { return _secsCommunicated; }
            set { _secsCommunicated = value; }
        }

        //public string SecsControlMode
        //{
        //    get { return _secsControlMode; }
        //    set { _secsControlMode = value; }
        //}

        public bool HsmsConnected
        {
            get { return _hsmsConnected; }
            set { _hsmsConnected = value; }
        }

        public bool EventReportConfigurated
        {
            get { return _eventReportConfigurated; }
            set { _eventReportConfigurated = value; }
        }
        #endregion

        public Equipment(EquipmentEntityData data, EquipmentEntityFile file)
        {
            Data = data;
            File = file;
        }
    }
    [Serializable]
    public class DispatchPoint
    {
        private string _pointNo;

        public string PointNo
        {
            get { return _pointNo; }
            set { _pointNo = value; }
        }
        private eDispatchMode _dispatchMode = eDispatchMode.UNUSED;

        public eDispatchMode DispatchMode
        {
            get { return _dispatchMode; }
            set { _dispatchMode = value; }
        }
    }
    [Serializable]
    public class LotEndGlassInfos
    {
        public LotEndGlassInfos()
        {

        }
        public LotEndGlassInfos(string lotID, string cstNo, string slotNo, string glassID, DateTime addTime)
        {
            LotID = lotID;
            Cassette_Sequence_No = cstNo;
            Slot_Sequence_No = slotNo;
            GlassID = glassID;
            AddTime = addTime;
        }
        public string LotID { get; set; }
        public string Cassette_Sequence_No { get; set; }
        public string Slot_Sequence_No { get; set; }
        public string GlassID { get; set; }

        public DateTime AddTime { get; set; }

    }

    [Serializable]
    public class PPIDPreDownloadInfos
    {
        public PPIDPreDownloadInfos()
        {

        }
        public PPIDPreDownloadInfos(string ppID, string batchID, DateTime dateTime)
        {
            PPID = ppID;
            BatchID = batchID;
            AddTime = dateTime;
        }
        public string PPID { get; set; }
        public string BatchID { get; set; }
        public DateTime AddTime { get; set; }
    }



}
