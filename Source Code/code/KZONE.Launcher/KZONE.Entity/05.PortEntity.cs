using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.ConstantParameter;
using System.ComponentModel;

namespace KZONE.Entity
{
	/// <summary>
	/// 對應File, 修改Property後呼叫Save(), 會序列化存檔
	/// </summary>
	[Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
	public class PortEntityFile : EntityFile
	{
        private ePortStatus _portStatus = ePortStatus.UN;
        private eCassetteStatus _cassetteStatus = eCassetteStatus.UNKNOWN;

        public eCassetteStatus CassetteStatus
        {
            get { return _cassetteStatus; }
            set { _cassetteStatus = value; }
        }
        private eCompletedCassetteData _completedCassetteData = eCompletedCassetteData.Unknown;
        private ePortType _portType = ePortType.Unknown;
        private string _portMode = string.Empty;
        private ePortTransferMode _transferMode = ePortTransferMode.Unknown;
        private ePortEnableMode _enableMode = ePortEnableMode.Unknown;
    
        private eOPISubCstState _oPI_SubCstState = eOPISubCstState.NONE;
        private ePortCassetteStatus _portCassetteStatus = ePortCassetteStatus.Unused;

        private string _cassetteSequenceNo = "0";
        private string _cassetteID = string.Empty;
        private string _jobCountInCassette = "0";

        private string _startByCount = "0";
      
        private string _operationID = string.Empty;
        private string _jobExistenceSlot; //= new String('0', 32);
  
        private string _cassetteSetCode = string.Empty;
        private string _groupNo = "0";

        private string _portOperationMode = string.Empty;
        private ePortJudge _portJudge = ePortJudge.None;
        private string _maxSlotCount = "0";
        private int _targetPortNo = 0;
        private bool _oQCFlag = false;
        private int _criterialNo = 0;
        private string _portTypeAutoChangeMode = string.Empty;
        private string _portCassetteTypeChangeMode = string.Empty;
        private string _loadingCasstteType = string.Empty;

        public string LoadingCasstteType
        {
            get { return _loadingCasstteType; }
            set { _loadingCasstteType = value; }
        }
        private string _qTimeFlag = string.Empty;

        public string QTimeFlag
        {
            get { return _qTimeFlag; }
            set { _qTimeFlag = value; }
        }

        public string PortCassetteTypeChangeMode
        {
            get { return _portCassetteTypeChangeMode; }
            set { _portCassetteTypeChangeMode = value; }
        }

        public string PortTypeAutoChangeMode
        {
            get { return _portTypeAutoChangeMode; }
            set { _portTypeAutoChangeMode = value; }
        }


        public int TargetPortNo
        {
            get { return _targetPortNo; }
            set { _targetPortNo = value; }
        }

        /// <summary> Report To MES Port Status 
        /// 
        /// </summary>
        public ePortStatus ToMESPortStatus
        {
            get { return _portStatus; }
            set { _portStatus = value; }
        }

        //public eCassetteStatus CassetteStatus
        //{
        //    get { return _cassetteStatus; }
        //    set { _cassetteStatus = value; }
        //}

        public string CassetteSequenceNo
        {
            get { return _cassetteSequenceNo; }
            set { _cassetteSequenceNo = value; }
        }

        public string CassetteID
        {
            get { return _cassetteID; }
            set { _cassetteID = value; }
        }

        public string JobCountInCassette
        {
            get { return _jobCountInCassette; }
            set { _jobCountInCassette = value; }
        }

        public string StartByCount
        {
            get { return _startByCount; }
            set { _startByCount = value; }
        }

       
        public eCompletedCassetteData CompletedCassetteData
        {
            get { return _completedCassetteData; }
            set { _completedCassetteData = value; }
        }

        public string OperationID
        {
            get { return _operationID; }
            set { _operationID = value; }
        }

        public string GroupNo
        {
            get { return _groupNo; }
            set { _groupNo = value; }
        }

        public string JobExistenceSlot
        {
            get { return _jobExistenceSlot; }
            set 
            { 
                _jobExistenceSlot = value;

               
            }
        }    

        public ePortType PortType
        {
            get { return _portType; }
            set { _portType = value; }
        }    

        public string PortMode
        {
            get { return _portMode; }
            set { _portMode = value; }
        }
        public ePortTransferMode TransferMode
        {
            get { return _transferMode; }
            set { _transferMode = value; }
        }

        public ePortEnableMode EnableMode
        {
            get { return _enableMode; } 
            set { _enableMode = value; } 
        }

        public string CassetteSetCode
        {
            get { return _cassetteSetCode; }
            set { _cassetteSetCode = value; }
        }

        public eOPISubCstState OPI_SubCstState
        {
            get { return _oPI_SubCstState; }
            set { _oPI_SubCstState = value; }
        }

        public ePortCassetteStatus PortCassetteStatus {
            get { return _portCassetteStatus; }
            set { _portCassetteStatus = value; }
        }

        public string PortOperationMode
        {
            get { return _portOperationMode; }
            set { _portOperationMode = value; }
        }

        public ePortJudge PortJudge {
            get { return _portJudge; }
            set { _portJudge = value; }
        }

        public string MaxSlotCount
        {
            get { return _maxSlotCount; }
            set { _maxSlotCount = value; }
        }

        //public ePortMode PortMode 
        //{
        //    get { return _portMode; }
        //    set { _portMode = value; }
        //}

        private eCassetteType _cassetteType = eCassetteType.Unused;

        public eCassetteType CassetteType
        {
            get { return _cassetteType; }
            set { _cassetteType = value; }
        }

        private DateTime _waitTime = DateTime.MaxValue;

        /// <summary>
        /// Port Status 變成 5:WaitForRun 的時間, 由 Robot Service 更新.
        /// </summary>
        public DateTime WaitTime
        {
            get { return _waitTime; }
            set { _waitTime = value; }
        }

        public PortEntityFile() {}

        public PortEntityFile(int maxCount)
        {
            _jobExistenceSlot = new String('0', maxCount);
            
        }

        //20161004 add For Block Control
        private string _portLastProductGlassGroupNo = string.Empty;

        /// <summary> Port Last Product Glass's GroupNo(Offline:31~100,Online:131~65535) for Sub Block Control
        ///
        /// </summary>
        public string PortLastProductGlassGroupNo
        {
            get { return _portLastProductGlassGroupNo; }
            set { _portLastProductGlassGroupNo = value; }
        }

        //20161116 add for Sort Mode Use
        private string _sortULDSendLREQFlag = Sort_ULD_SendLREQStatus.NONE;

        /// <summary> Sort Line or Sort Mode Unlaoder Port Send LREQ Status.
        /// 
        /// </summary>
        public string SortULDSendLREQFlag
        {
            get { return _sortULDSendLREQFlag; }
            set { _sortULDSendLREQFlag = value; }
        }

        //2016116 add for Sort Mode BC Set Sort Grade
        private string _sortULD_BcSetSortGrade = string.Empty;

        /// <summary> Sort Line or Sort Mode Unlaoder BC Set Port Sort Grade
        /// 
        /// </summary>
        public string SortULD_BcSetSortGrade
        {
            get { return _sortULD_BcSetSortGrade; }
            set { _sortULD_BcSetSortGrade = value; }
        }

        //2016116 add for Sort Mode BC Set LREQ GlassID
        private string _sortULD_BcSetLREQGlassID = string.Empty;

        /// <summary> Sort Line or Sort Mode Unlaoder BC Set LREQ GlassID
        /// 
        /// </summary>
        public string SortULD_BcSetLREQGlassID
        {
            get { return _sortULD_BcSetLREQGlassID; }
            set { _sortULD_BcSetLREQGlassID = value; }
        }

        //20161118 add for紀錄是哪個Loader PortID來設定目前Unloader Port的Sort Grade
        private string _sortULD_BcSetSortGradeByLDPortID = string.Empty;

        /// <summary> Sort Line or Sort Mode Unlaoder BC Set Port Sort Grade By Loader PortID
        /// 
        /// </summary>
        public string SortULD_BcSetSortGradeByLDPortID
        {
            get { return _sortULD_BcSetSortGradeByLDPortID; }
            set { _sortULD_BcSetSortGradeByLDPortID = value; }
        }

        //20161120 add for Sort SEPARATE_MODE GroupDefine
        private string _sort_SEPARATE_MODE_GroupNo = string.Empty;

        /// <summary>定義 Sort Line SEPARATE_MODE Port GroupNo  
        /// 
        /// </summary>
        public string Sort_SEPARATE_MODE_GroupNo 
        {
            get { return _sort_SEPARATE_MODE_GroupNo; }
            set { _sort_SEPARATE_MODE_GroupNo = value; }
        }

        private SerializableDictionary<string, string> _sortULD_BCSetSortGradeByLDInfoList=new SerializableDictionary<string,string>() ;

        /// <summary> BC Set Unloader Sort Grade by Loader Info List. Key:LDPortID, Value:LD Sort Grade First GlassID
        /// 
        /// </summary>
        public SerializableDictionary<string, string> SortULD_BCSetSortGradeByLDInfoList
        {
            get { return _sortULD_BCSetSortGradeByLDInfoList; }
            set { _sortULD_BCSetSortGradeByLDInfoList = value; }
        }

        /// <summary>
        /// OLB 抽检Box使用
        /// </summary>
        public bool OQCFlag
        {
            get { return _oQCFlag; }
            set { _oQCFlag = value; }
        }

        private int _offlineGlassIDSeqNo = 0;

        //20170201 add for Offline Create GlassID SeqNo. 1~255
        /// <summary>for Offline Create GlassID SeqNoKey. 1~255
        /// 
        /// </summary>
        public int OfflineGlassIDSeqNo
        {
            get { return _offlineGlassIDSeqNo; }
            set { _offlineGlassIDSeqNo = value; }
        }

        //Add for  Cell ODF  Criterial No 20170222 Tom 
        public int CriterialNo {
            get {
                return _criterialNo;
            }
            set { _criterialNo = value; }
        }

        //20170411 add
        private bool _emptyCSTFlag =false ;

        /// <summary> True:Port Status= WASC , CST is EMPTY CST
        /// 
        /// </summary>
        public bool EmptyCSTFlag
        {
            get
            {
                return _emptyCSTFlag;
            }
            set { _emptyCSTFlag = value; }
        }

	}

    public class Port : Entity
	{
		public PortEntityData Data { get; private set; }

		public PortEntityFile File { get; private set; }

		public Port(PortEntityData data, PortEntityFile file)
		{
			Data = data;
			File = file;
		}
	}
}
