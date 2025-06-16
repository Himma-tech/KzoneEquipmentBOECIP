using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
	/// <summary>
	/// 對應File, 修改Property後呼叫Save(), 會序列化存檔
	/// </summary>
	[Serializable]
	public class UnitEntityFile : EntityFile
	{
        private eEQPStatus _status = eEQPStatus.Unused;

        private string _currentAlarmCode = string.Empty;
        private int _productGlassCount = 0 ;

        private int _groupNo = 0;
        private string _mesStatus = "DOWN";
        private string _runMode = string.Empty;
        private string _chamberRunMode = string.Empty;
        private string _unitRecipeId = string.Empty;

        private eBUFStatus _bufferStatus = eBUFStatus.Unused;
        private int _bufferGlassCount = 0;
        private int _settingOfUpLevelCount = 0;
        private int _settingOfDownLevelCount = 0;
        private int _upLevelCount = 0;
        private int _downLevelCount = 0;

        private eBitResult _unitAlram = eBitResult.OFF;
	   
	    private string _alarmID = String.Empty;

	    private int _totalTFTGlassCount = 0;
	    private int _totalHFGlassCount = 0;
	    private int _dummyGlassCount = 0;
	    private int _mQCGlassCount = 0;
	    private int _uVMaskCount = 0;
	    private int _productType = 0;

        private Dictionary<int, string> _reasonCode = new Dictionary<int, string>();
        public string AlarmID
	    {
	        get { return _alarmID; }
	        set { _alarmID = value; }
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
        public eBitResult UnitAlram
        {
            get { return _unitAlram; }
            set { _unitAlram = value; }
        }


        private string setCVSpeed = string.Empty;

        public string SetCVSpeed
        {
            get { return setCVSpeed; }
            set { setCVSpeed = value; }
        }

        private string cvSpeed = string.Empty;

        public string CvSpeed
        {
            get { return cvSpeed; }
            set { cvSpeed = value; }
        }

        private string c01Flow = string.Empty;

        public string C01Flow
        {
            get { return c01Flow; }
            set { c01Flow = value; }
        }

        private string c02Flow = string.Empty;

        public string C02Flow
        {
            get { return c02Flow; }
            set { c02Flow = value; }
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


        public string RunMode
        {
            get { return _runMode; }
            set { _runMode = value; }
        }
        public string ChamberRunMode
        {
            get { return _chamberRunMode; }
            set { _chamberRunMode = value;}
        }

        public eEQPStatus Status
		{
			get {  return _status;  }
			set { _status = value;  }
		}

        public string CurrentAlarmCode
        {
            get { return _currentAlarmCode; }
            set { _currentAlarmCode = value; }
        }

        public int  ProductGlassCount
        {
            get { return _productGlassCount; }
            set { _productGlassCount = value; }
        }
       
        public int GroupNo
        {
            get { return _groupNo; }
            set { _groupNo = value; }
        }
        public string MESStatus
        {
            get { return _mesStatus; }
            set { _mesStatus = value; }
        }

        public string UnitRecipeID {
            get { return _unitRecipeId; }
            set { _unitRecipeId = value; }
        }

        /// <summary>
        /// Buffer Glass Count
        /// </summary>
        public int BufferGlassCount
        {
            get { return _bufferGlassCount; }
            set { _bufferGlassCount = value; }
        }

        /// <summary>
        /// Setting Of Up Level Count
        /// </summary>
        public int SettingOfUpLevelCount
        {
            get { return _settingOfUpLevelCount; }
            set { _settingOfUpLevelCount = value; }
        }

        /// <summary>
        /// Setting Of Down Level Count
        /// </summary>
        public int SettingOfDownLevelCount
        {
            get { return _settingOfDownLevelCount; }
            set { _settingOfDownLevelCount = value; }
        }

        /// <summary>
        /// Up Level Count
        /// </summary>
        public int UpLevelCount
        {
            get { return _upLevelCount; }
            set { _upLevelCount = value; }
        }

        /// <summary>
        /// Down Level Count
        /// </summary>
        public int DownLevelCount
        {
            get { return _downLevelCount; }
            set { _downLevelCount = value; }
        }

        /// <summary> BufferStatus
        ///
        /// </summary>
        public eBUFStatus BufferStatus
        {
            get { return _bufferStatus; }
            set { _bufferStatus = value; }
        }
	}

    public class Unit : Entity
	{
		public UnitEntityData Data { get; private set; }

		public UnitEntityFile File { get; private set; }

       
		public Unit(UnitEntityData data, UnitEntityFile file)
		{
			Data = data;
			File = file;
		}
	}
}
