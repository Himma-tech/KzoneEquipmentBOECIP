using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace KZONE.Entity
{
    /// <summary>
    /// 對應File, 修改Property後呼叫Save(), 會序列化存檔
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class Cassette : EntityFile
    {
        private string _lineID = string.Empty;
        private string _nodeID = string.Empty;
        private string _nodeNo = string.Empty;
        private string _portID = string.Empty;
        private string _portNo = string.Empty;
        private string _cassetteID = string.Empty;
        private string _cassetteSequenceNo = "0";
        private string _lineRecipeName = string.Empty;
        private string _pPID = string.Empty;
        private bool _isProcessed = false;
        private string _trackingData = string.Empty;

        private IList<Job> _jobs = new List<Job>();
        private DateTime _loadTime = DateTime.Now;
        private DateTime _startTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now;
        private eCstCmd _cassetteCommand = eCstCmd.Unused;
    
        private string _mes_ValidateCassetteReply = string.Empty;
        private string _ldCassetteSettingCode = string.Empty;

        private string _reasonCode = string.Empty;
        private string _reasonText = string.Empty;

        private string _rawData = string.Empty;

        private string _crossLineFlag = "N";
        private string _firstGlassCheckReport = "N";  //Jun Add 20150404 Dense機台不會報First Glass Check Event

        private bool _coolRunStart = false;
        //private OFFLINE_CstBody _offline_CstData = new OFFLINE_CstBody();

        //Watson Add 20141212 For MES Spec 
        //If Lot started in Offline and ends normal in Online, report ABORTFLAG as blank.
        private bool _isOffLineProcessStarted = true; //內定為True.經LotProcessStart變更為False
        //Watson Add 20150102 For judge CELL DenseBox Processed 
        private eboxReport _cellBoxProcessed = eboxReport.NOProcess;
        private bool _oQCFlag = false;
        //2016/11/03 For Cassette Data Upload
        private IList<string> _mesDownloadJobsList = new List<string>();

        private string _portGrade = string.Empty;

        private string _boxRate = string.Empty; //M1OLB Box 抽检 Rate bruce.zhan 20170307


        [Category("Entity")]
        public IList<string> MesDownloadJobsList
        {
            get { return _mesDownloadJobsList; }
            set { _mesDownloadJobsList = value; }
        }

        //2016/11/07 For UPK Dense Verify NG 
        private string _denseVerifyNG = string.Empty;

        public string DenseVerifyNG
        {
            get { return _denseVerifyNG; }
            set { _denseVerifyNG = value; }
        }

       
        //Watson Add 20141212 For MES Spec 
        //If Lot started in Offline and ends normal in Online, report ABORTFLAG as blank.
        [Category("Entity")]
        public bool IsOffLineProcessStarted
        {
            get { return _isOffLineProcessStarted; }
            set { _isOffLineProcessStarted = value; }
        }
        [Category("Entity")]
        public string LineID
        {
            get { return _lineID; }
            set { _lineID = value; }
        }
        [Category("Entity")]
        public string NodeID
        {
            get { return _nodeID; }
            set { _nodeID = value; }
        }
        [Category("Entity")]
        public string NodeNo
        {
            get { return _nodeNo; }
            set { _nodeNo = value; }
        }
        [Category("Entity")]
        public string PortID
        {
            get { return _portID; }
            set { _portID = value; }
        }
        [Category("Entity")]
        public string PortNo
        {
            get { return _portNo; }
            set { _portNo = value; }
        }
        [Category("Entity"),ReadOnly(true)]
        public string CassetteID
        {
            get { return _cassetteID; }
            set { _cassetteID = value; }
        }
        [Category("Entity"), ReadOnly(true)]
        public string CassetteSequenceNo
        {
            get { return _cassetteSequenceNo; }
            set { _cassetteSequenceNo = value; }
        }
        [Category("Entity"), ReadOnly(true)]
        public string RawData {
            get { return _rawData; }
            set { _rawData = value; }
        }

        [Browsable(false)]
        public IList<Job> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; }
        }
         [Category("Entity")]
        public DateTime LoadTime
        {
            get { return _loadTime; }
            set { _loadTime = value; }
        }
         [Category("Entity")]
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
         [Category("Entity")]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
         [Category("Entity")]
        public eCstCmd CassetteCommand
        {
            get { return _cassetteCommand; }
            set { _cassetteCommand = value; }
        }

        /// <summary> CST Command EQ Reply ReasonCode
        /// 
        /// </summary>
         [Category("Entity")]
        public string ReasonCode
        {
            get { return _reasonCode; }
            set { _reasonCode = value; }
        }
         [Category("Entity")]
        public string ReasonText
        {
            get { return _reasonText; }
            set { _reasonText = value; }
        }
         [Category("Entity")]
        public string LineRecipeName
        {
            get { return _lineRecipeName; }
            set { _lineRecipeName = value; }
        }
         [Category("Entity")]
        public string PPID
        {
            get { return _pPID; }
            set { _pPID = value; }
        }     

        /// <summary>
        /// Cst 是否開始抽片
        /// </summary>
         [Category("Entity")]
        public bool IsProcessed
        {
            get { return _isProcessed; }
            set { _isProcessed = value; }
        }
     
        [Category("MES")]
        public string Mes_ValidateCassetteReply
        {
            get { return _mes_ValidateCassetteReply; }
            set { _mes_ValidateCassetteReply = value; }
        }
          [Category("Entity")]
        public string LDCassetteSettingCode
        {
            get { return _ldCassetteSettingCode; }
            set { _ldCassetteSettingCode = value; }
        }
          [Category("Entity")]
        public string CrossLineFlag
        {
            get { return _crossLineFlag; }
            set { _crossLineFlag = value; }
        }
          [Category("Entity")]
        public string FirstGlassCheckReport
        {
            get { return _firstGlassCheckReport; }
            set { _firstGlassCheckReport = value; }
        }
          [Category("Entity")]
        public eboxReport CellBoxProcessed 
        {
            get { return _cellBoxProcessed; }
            set { _cellBoxProcessed = value; }
        }
          [Category("Entity")]
        public string TrackingData
        {
            get { return _trackingData; }
            set { _trackingData = value; }
        }
          [Category("Entity")]
        public bool CoolRunStart
        {
            get { return _coolRunStart; }
            set { _coolRunStart = value; }
        }

        private string _userId = string.Empty;

        /// <summary> MES/OPI Send UserID
        /// 
        /// </summary>
        [Category("Entity")]
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _prty = string.Empty;

        /// <summary> MES/OPI Send Prty(Priority)
        /// 
        /// </summary>
        [Category("Entity")]
        public string Prty
        {
            get { return _prty; }
            set { _prty = value; }
        }

        private string _load_Prty = string.Empty;

        /// <summary> MES/OPI Send Load Prty(Carrier Load Priority(1-5) 1>2>...)
        /// 
        /// </summary>
        [Category("Entity")]
        public string Load_Prty
        {
            get { return _load_Prty; }
            set { _load_Prty = value; }
        }

        private string _sht_cnt = string.Empty;

        /// <summary> MES/OPI Send Sheet Count
        /// 
        /// </summary>
        [Category("Entity")]
        public string Sht_cnt
        {
            get { return _sht_cnt; }
            set { _sht_cnt = value; }
        }

        private string _ary_sht_cnt = string.Empty;

        /// <summary> MES/OPI Send oary1 Sheet Count
        /// 
        /// </summary>
        [Category("Entity")]
        public string Ary_sht_cnt
        {
            get { return _ary_sht_cnt; }
            set { _ary_sht_cnt = value; }
        }

        private string _cstEndReason = string.Empty;

        /// <summary> CST Data Upload End ReasonCode
        /// 
        /// </summary>
        [Category("Entity")]
        public string CstEndReason
        {
            get { return _cstEndReason; }
            set { _cstEndReason = value; }
        }
        [Category("Entity")]
        public bool OQCFlag {
            get { return _oQCFlag; }
            set { _oQCFlag = value; }
        }

        private string _acRecipeId = string.Empty; //CST Move In時,APCLOGON/CPCLOGON(ac_recipe_id)需報給MES,BC 真正download 給 EQP 的 actual recipe id (PPID)
        [Category("Entity")]
        public string AcRecipeId
        {
            get { return _acRecipeId; }
            set { _acRecipeId = value; }
        }

        private eSecondVerifyStatus _secondVerifyStatus = eSecondVerifyStatus.None; //用來判斷報的Operation Verify是一次還是二次
        [Category("Entity")]
        public eSecondVerifyStatus SecondVerifyStatus
        {
            get { return _secondVerifyStatus; }
            set { _secondVerifyStatus = value; }
        }

        //20160927 add for Keep MAPE ProcessFlag= Y Glas Count
        /// <summary>Mapping Download All ProcessFlag=Y Glass Count
        /// 
        /// </summary>
        private int _processGlassCount = 0;

        /// <summary>Mapping Download All ProcessFlag=Y Glass Count
        /// 
        /// </summary>
        [Category("Entity")]
        public int ProcessGlassCount
        {
            get { return _processGlassCount; }
            set { _processGlassCount = value; }
        }

        //20161109 add MES reply NG and Cancel Flag
        private bool _mesCancelFlag;

        /// <summary>MES Reply NG and Must Cancel CST Flag
        /// 
        /// </summary>
        [Category("Entity")]
        public bool MESCancelFlag
        {
            get { return _mesCancelFlag; }
            set { _mesCancelFlag = value; }
        }

        /// <summary>
        /// BC Donwnload  CST  Command  for Sort Mode 20161115
        /// </summary>
        [Category("Entity")]
        public string PortGrade
        {
            get { return _portGrade; }
            set { _portGrade = value; }
        }
        /// <summary>
        /// M1OLB Box 抽检 Rate
        /// </summary>
        [Category("Entity")]
        public string BoxRate
        {
            get { return _boxRate; }
            set { _boxRate = value; }
        }
        //20161207 add Dense Total Count
        private int _denseTotalCount;

        /// <summary> MES or OPI Set Dense Total Count
        /// 
        /// </summary>
        [Category("Entity")]
        public int DenseTotalCount
        {
            get { return _denseTotalCount; }
            set { _denseTotalCount = value; }
        }

        //20170129 add Mapping Downlod時SlotExistInfoList
        private IList<string> _mapeDownloadSlotExistInfo = new List<string>();

        /// <summary> Mapping Download Slot Exist Info
        /// 
        /// </summary>
        [Category("Entity")]
        public IList<string> MapeDownloadSlotExistInfo
        {
            get { return _mapeDownloadSlotExistInfo; }
            set { _mapeDownloadSlotExistInfo = value; }
        }

        //20170531 add mapeDownloadGlassType
        private int _mapeDownloadJobType = 0;

        /// <summary> Mapping Download Job Type
        /// 
        /// </summary>
        [Category("Entity")]
        public int MapeDownloadJobType
        {
            get { return _mapeDownloadJobType; }
            set { _mapeDownloadJobType = value; }
        }
    }
}
