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
    public class MaterialEntity : EntityFile
    {
        private eMaterialType _materialType = eMaterialType.UNUSED;
        private string _materialId = string.Empty;
        private eMaterialStatus _materialStatus = eMaterialStatus.UNUSED;
        private eMaterialStatus _lastMaterialStatus = eMaterialStatus.UNUSED;
        private string _nodeNo = string.Empty;
        private int _unitNo = 0;
        private string _event = string.Empty;
        private string _operatorID = string.Empty;
        private int _usedCount = 0;
        private int _limitedCount = 0;
        private int _weight = 0;
        private string _position = string.Empty;
        private string _name = string.Empty;

        private MaterialTimeData _lifeTime;
        private MaterialTimeData _dueTime;
        private MaterialTimeData _mountTime;
        private MaterialTimeData _unmountTime;

        private DateTime upDateTime;

        private int _thickness;

        private string _prid = string.Empty;

        private List<MaterialLot> _lots = new List<MaterialLot>();

        public MaterialEntity() { }

        public MaterialEntity(string materialID)
        {
            MaterialId = materialID;
        }

        public DateTime UpdateTime
        {
            get { return upDateTime; }
            set { upDateTime = value; }
        }

        public List<MaterialLot> Lots
        {
            get { return _lots; }
            set { _lots = value; }
        }
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }

        }

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public int Thickness
        {
            get { return _thickness; }
            set { _thickness = value; }
        }

        public int UsedCount
        {
            get { return _usedCount; }
            set { _usedCount = value; }
        }

        public int LimitedCount
        {
            get { return _limitedCount; }
            set { _limitedCount = value; }
        }

        public string OperatorID
        {
            get { return _operatorID; }
            set { _operatorID = value; }
        }


        public string Event
        {
            get { return _event; }
            set { _event = value; }
        }
        public int UnitNo
        {
            get { return _unitNo; }
            set { _unitNo = value; }
        }
        public string NodeNo
        {
            get { return _nodeNo; }
            set { _nodeNo = value; }
        }

        public string PRID
        {
            get { return _prid; }
            set { _prid = value; }
        }

        public eMaterialStatus MaterialStatus
        {
            get { return _materialStatus; }
            set { _materialStatus = value; }
        }
        public eMaterialStatus LastMaterialStatus
        {
            get { return _lastMaterialStatus; }
            set { _lastMaterialStatus = value; }
        }
        public string MaterialId
        {
            get { return _materialId; }
            set { _materialId = value; }
        }

        public eMaterialType MaterialType
        {
            get { return _materialType; }
            set { _materialType = value; }
        }

        public MaterialTimeData LifeTime
        {
            get
            {
                if (_lifeTime == null)
                {
                    _lifeTime = new MaterialTimeData();
                }
                return _lifeTime;
            }
            set { _lifeTime = value; }
        }

        public MaterialTimeData DueTime
        {
            get
            {
                if (_dueTime == null)
                {
                    _dueTime = new MaterialTimeData();
                }
                return _dueTime;
            }
            set { _dueTime = value; }
        }

        public MaterialTimeData MountTime
        {
            get
            {
                if (_mountTime == null)
                {
                    _mountTime = new MaterialTimeData();
                }
                return _mountTime;
            }
            set { _mountTime = value; }
        }

        public MaterialTimeData UnmountTime
        {
            get
            {
                if (_unmountTime == null)
                {
                    _unmountTime = new MaterialTimeData();
                }
                return _unmountTime;
            }
            set { _unmountTime = value; }
        }


    }
    [Serializable]
    public class MaterialLot
    {
        private string _lotId = string.Empty;
        private string _lotNo = string.Empty;

        public string LotNo
        {
            get { return _lotNo; }
            set { _lotNo = value; }
        }
        private string _lotType = string.Empty;

        public string LotType
        {
            get { return _lotType; }
            set { _lotType = value; }
        }
        private int _count = 0;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public string LotId
        {
            get { return _lotId; }
            set { _lotId = value; }
        }
    }
    [Serializable]
    public class MaterialTimeData
    {
        private int _year;
        private int _month;
        private int _day;
        private int _hour;
        private int _minute;
        private int _second;

        public MaterialTimeData()
        { }

        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int Day
        {
            get { return _day; }
            set { _day = value; }
        }

        public int Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }

        public int Minute
        {
            get { return _minute; }
            set { _minute = value; }
        }

        public int Second
        {
            get { return _second; }
            set { _second = value; }
        }

    }
}
