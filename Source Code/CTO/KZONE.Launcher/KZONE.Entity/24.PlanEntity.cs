using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace KZONE.Entity
{
    [Serializable]
    public class Plan : EntityFile
    {
        private string _pLANNAME = string.Empty;

        private ePlanStatus _planStatus = ePlanStatus.NoStart;
        private List<PortCSTInfo> _portCstInfos = new List<PortCSTInfo>();
        private List<string> _cstInfos = new List<string>();
        private List<string> _portInfos = new List<string>();

      
        public string PLAN_NAME
        {
            get { return _pLANNAME; }
            set { _pLANNAME = value; }
        }
        public ePlanStatus PLAN_STATUS
        {
            get { return _planStatus; }
            set { _planStatus = value; }
        }

        

        public List<PortCSTInfo> PortCstInfos
        {
            get { return _portCstInfos; }
            set { _portCstInfos = value; }
        }

        public List<string> CSTInfos
        {
            get { return _cstInfos; }
            set { _cstInfos = value; }
        }

        public List<string> PortInfos
        {
            get { return _portInfos; }
            set { _portInfos = value; }
        }
    }

    [Serializable]
    public class SLOTPLAN 
    {
        private string _pRODUCTNAME = string.Empty;
        private int _tARGETSLOTNO = 0;
        private int __sOURCESLOTNO = 0;
        private bool _hAVEBEENUSED = false;
        private string _sOURCEPORTID= string.Empty;
        private string _tARGETPORTID = string.Empty;
        private string _sOURCECSTID = string.Empty;
        private string _tARGETCSTID = string.Empty;

        public string PRODUCT_NAME
        {
            get { return _pRODUCTNAME; }
            set { _pRODUCTNAME = value; }
        }

        public int TARGETSLOTNO
        {
            get { return _tARGETSLOTNO; }
            set { _tARGETSLOTNO = value; }
        }

        public int SOURCESLOTNO
        {
            get { return __sOURCESLOTNO; }
            set { __sOURCESLOTNO = value; }
        }

        public bool HAVE_BEEN_USED
        {
            get { return _hAVEBEENUSED; }
            set { _hAVEBEENUSED = value; }
        }
        //20170713 bruce.zhan
        public string SOURCE_PORT_ID
        {
            get { return _sOURCEPORTID; }
            set { _sOURCEPORTID = value; }
        }

        public string TARGET_PORT_ID
        {
            get { return _tARGETPORTID; }
            set { _tARGETPORTID = value; }
        }

        public string SOURCE_CST_ID
        {
            get { return _sOURCECSTID; }
            set { _sOURCECSTID = value; }
        }

        public string TARGET_CST_ID
        {
            get { return _tARGETCSTID; }
            set { _tARGETCSTID = value; }
        }

    }
  
    [Serializable]
    public class PortCSTInfo
    {
        private string _portID = string.Empty;
        private string _portType = string.Empty;
        private string _cstID = string.Empty;
        private string _cstType = string.Empty;
        private bool _cstEnd = false;
        private List<SLOTPLAN> _slotPlan = new List<SLOTPLAN>();
        //20170713 bruce.zhan
        public string PortID
        {
            get { return _portID; }
            set { _portID = value; }
        }
        public string PortType
        {
            get { return _portType; }
            set { _portType = value; }
        }
        public string CSTID
        {
            get { return _cstID; }
            set { _cstID = value; }
        }
        //20170713增加CST TYPE S/T  bruce.zhan
        public string CSTTYPE
        {
            get { return _cstType; }
            set { _cstType = value; }
        }
        public bool CSTEND
        {
            get { return _cstEnd; }
            set { _cstEnd = value; }
        }
        public List<SLOTPLAN> SlotPlans
        {
            get { return _slotPlan; }
            set { _slotPlan = value; }
        }
    }
   
}
