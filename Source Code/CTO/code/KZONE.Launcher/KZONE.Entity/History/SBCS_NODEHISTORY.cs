using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCSNODEHISTORY

	/// <summary>
	/// SBCSNODEHISTORY object for NHibernate mapped table 'SBCS_NODEHISTORY'.
	/// </summary>
	public class EquipmentHistory:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected string _lINEID;
		protected string _nODEID;
		protected string _nODENO;
		protected string _cIMMODE;
		protected string _cURRENTRECIPEID;
		protected string _cURRENTSTATUS;
        protected string _aUTOMANUEL;
        protected string _rECIPEAUTOCHANGE;
        protected string _rECIPECHECK;

        protected string _gLASSCHECKMODE;
		protected int _tFTJOBCOUNT;
		protected int _hFJOBCOUNT;
        protected int _dUMMYJOBCOUNT;
	    protected int _uVMASKCOUNT;
	    protected int _mQCJOBCOUNT;
	    protected int _pRODUCTTYPE;
	    protected string _uPINLINEMODE;
	    protected string _dOWNINLINEMODE;

	    protected string _pRODUCTTYPEMODE;
	    protected string _gROUPINDEXMODE;
	    protected string _pRODUCTIDMODE;
	    protected string _dUPLICATEMODE;
        protected string _aDDLIQUID;
        protected string _mINLIQUID;
        protected string _vCRID;
        protected string _vCRENBLE;
       


	
		#endregion

		#region Constructors

		public EquipmentHistory() { }

        public EquipmentHistory(DateTime uPDATETIME, 
                            string lINEID, 
                            string nODEID, 
                            string nODENO, 
                            string cIMMODE, 
                            string cURRENTRECIPEID, 
                            string cURRENTSTATUS,

                            int tFTJOBCOUNT,
                            int hFJOBCOUNT, 
                            int dUMMYJOBCOUNT,
                            int uVMASKCOUNT,
                            int mQCJOBCOUNT,
                            int pRODUCTTYPE,
                            string uPINLINEMODE,
                            string dOWNINLINEMODE,
                            string aDDLIQUID,
                            string mINLIQUID,
                            string vCRID,
                           string vCRENBLE,
                           string aUTOMANULE,
                           string rECIPEAUTOCHANGE,
                          string rECIPECHECK,
                          string gLASSCHECKMODE,
                          string pRODUCTTYPEMODE,
                          string gROUPINDEXMODE,
                           string pRODUCTIDMODE,
                           string dUPLICATEMODE
                           )
		{
			this._uPDATETIME = uPDATETIME;
			this._lINEID = lINEID;
			this._nODEID = nODEID;
			this._nODENO = nODENO;
			this._cIMMODE = cIMMODE;
			this._cURRENTRECIPEID = cURRENTRECIPEID;
			this._cURRENTSTATUS = cURRENTSTATUS;

            this._tFTJOBCOUNT = tFTJOBCOUNT;
            this._hFJOBCOUNT = hFJOBCOUNT;
            this._dUMMYJOBCOUNT = dUMMYJOBCOUNT;
            this._uVMASKCOUNT = uVMASKCOUNT;
            this._mQCJOBCOUNT = mQCJOBCOUNT;
            this._pRODUCTTYPE = pRODUCTTYPE;
		    this._uPINLINEMODE = uPINLINEMODE;
		    this._dOWNINLINEMODE = dOWNINLINEMODE;
            this._aDDLIQUID = aDDLIQUID;
            this._mINLIQUID = mINLIQUID;
            this._vCRID = vCRID;
            this._vCRENBLE = vCRENBLE;
            this._aUTOMANUEL = aUTOMANULE;
            this._rECIPEAUTOCHANGE = rECIPEAUTOCHANGE;
            this._rECIPECHECK = rECIPECHECK;
            this._gLASSCHECKMODE = gLASSCHECKMODE;

		    this._pRODUCTTYPEMODE = pRODUCTTYPEMODE;
		    this._gROUPINDEXMODE = gROUPINDEXMODE;
		    this._pRODUCTIDMODE = pRODUCTIDMODE;
		    this._dUPLICATEMODE = dUPLICATEMODE;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual DateTime UPDATETIME
		{
			get { return _uPDATETIME; }
			set { _uPDATETIME = value; }
		}

		public virtual string LINEID
		{
			get { return _lINEID; }
			set
			{				
				_lINEID = value;
			}
		}

		public virtual string NODEID
		{
			get { return _nODEID; }
			set
			{				
				_nODEID = value;
			}
		}

		public virtual string NODENO
		{
			get { return _nODENO; }
			set
			{				
				_nODENO = value;
			}
		}

		public virtual string CIMMODE
		{
			get { return _cIMMODE; }
			set
			{				
				_cIMMODE = value;
			}
		}

		public virtual string CURRENTRECIPEID
		{
			get { return _cURRENTRECIPEID; }
			set
			{				
				_cURRENTRECIPEID = value;
			}
		}

		public virtual string CURRENTSTATUS
		{
			get { return _cURRENTSTATUS; }
			set
			{				
				_cURRENTSTATUS = value;
			}
		}

		public virtual int TFTJOBCOUNT
		{
			get { return _tFTJOBCOUNT; }
            set { _tFTJOBCOUNT = value; }
		}

		public virtual int HFJOBCOUNT
		{
			get { return _hFJOBCOUNT; }
            set { _hFJOBCOUNT = value; }
		}

        public virtual int DUMMYJOBCOUNT
        {
            get { return _dUMMYJOBCOUNT; }
            set { _dUMMYJOBCOUNT = value; }
        }

	    public virtual int UVMASKCOUNT
	    {
	        get { return _uVMASKCOUNT; }
            set { _uVMASKCOUNT = value; }
	    }

	    public virtual int MQCJOBCOUNT
	    {
	        get { return _mQCJOBCOUNT; }
	        set { _mQCJOBCOUNT = value; }
	    }
	    public virtual int PRODUCTTYPE
	    {
            get { return _pRODUCTTYPE; }
            set { _pRODUCTTYPE = value; }
	    }

	    public virtual string UPINLINEMODE
	    {
	        get { return _uPINLINEMODE; }
	        set { _uPINLINEMODE = value; }
	    }

	    public virtual string DOWNINLINEMODE
	    {
	        get { return _dOWNINLINEMODE; }
	        set { _dOWNINLINEMODE = value; }
	    }

	    public virtual string ADDLIQUID
        {
            get { return _aDDLIQUID; }
            set { _aDDLIQUID = value; }
        }
        public virtual string MINLIQUID
        {
            get { return _mINLIQUID; }
            set { _mINLIQUID = value; }
        }
        public virtual string VCRID
        {
            get { return _vCRID; }
            set { _vCRID = value; }
        }
        public virtual string VCRENBLE
        {
            get { return _vCRENBLE; }
            set { _vCRENBLE = value; }
        }
        public virtual string AUTOMANUEL
        {
            get { return _aUTOMANUEL; }
            set { _aUTOMANUEL = value; }
        }
        public virtual string RECIPEAUTOCHANGE
        {
            get { return _rECIPEAUTOCHANGE; }
            set { _rECIPEAUTOCHANGE = value; }
        }

        public virtual string RECIPECHECK
        {
            get { return _rECIPECHECK; }
            set { _rECIPECHECK = value; }
        }

        public virtual string GLASSCHECKMODE
        {
            get { return _gLASSCHECKMODE; }
            set { _gLASSCHECKMODE = value; }
        }

	    public virtual string PRODUCTTYPECHECKMODE
	    {
	        get { return _pRODUCTTYPEMODE; }
	        set { _pRODUCTTYPEMODE = value; }
	    }

	    public virtual string GROUPINDEXCHECKMODE
	    {
	        get { return _gROUPINDEXMODE; }
	        set { _gROUPINDEXMODE = value; }
	    }

	    public virtual string PRODUCTIDCHECKMODE
	    {
	        get { return _pRODUCTIDMODE; }
            set { _pRODUCTIDMODE = value; }
	    }
	    public virtual string DUPLICATECHECKMODE
	    {
	        get { return _dUPLICATEMODE; }
            set { _dUPLICATEMODE = value; }
	    }



	    #endregion
	}
	#endregion
}