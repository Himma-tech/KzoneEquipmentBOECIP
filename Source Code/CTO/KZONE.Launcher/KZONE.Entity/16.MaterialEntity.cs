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
        private eMaterialType _materialType = eMaterialType.UNSED;
        private string _materialId = string.Empty;
        private eMaterialStatus _materialStatus = eMaterialStatus.UNUSED;
        private string _nodeNo = string.Empty;
        private int _unitNo = 0;
        private int _slotNo = 0;
        private string _operatorID = string.Empty;
        private int _count = 0;
        private int _weight = 0;
        private string _position = string.Empty;
        private string _name = string.Empty;

        private List<MaterialLot> _lots = new List<MaterialLot>();

        public List<MaterialLot> Lots {
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

        public int Weight {
            get { return _weight; }
            set { _weight = value; }
        }

        public int Count {
            get { return _count; }
            set { _count = value; }
        }

        public string OperatorID {
            get { return _operatorID; }
            set { _operatorID = value; }
        }


        public int SlotNo {
            get { return _slotNo; }
            set { _slotNo = value; }
        }
        public int UnitNo {
            get { return _unitNo; }
            set { _unitNo = value; }
        }
        public string NodeNo {
            get { return _nodeNo; }
            set { _nodeNo = value; }
        }


        public eMaterialStatus MaterialStatus {
            get { return _materialStatus; }
            set { _materialStatus = value; }
        }
        public string MaterialId {
            get { return _materialId; }
            set { _materialId = value; }
        }

        public eMaterialType MaterialType {
            get { return _materialType; }
            set { _materialType = value; }
        }
    }
    [Serializable]
    public class MaterialLot
    {
        private string _lotId=string.Empty;
        private string _lotNo = string.Empty;

        public string LotNo {
            get { return _lotNo; }
            set { _lotNo = value; }
        }
        private string _lotType = string.Empty;

        public string LotType {
            get { return _lotType; }
            set { _lotType = value; }
        }
        private int _count = 0;

        public int Count {
            get { return _count; }
            set { _count = value; }
        }

        public string LotId {
            get { return _lotId; }
            set { _lotId = value; }
        }
    }
}
