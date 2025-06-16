using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCSPORTHISTORY

	/// <summary>
	/// SBCSPORTHISTORY object for NHibernate mapped table 'SBCS_PORTHISTORY'.
	/// </summary>
    public class PortHistory : EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected string _lINEID;
		protected string _nODEID;
		protected string _pORTID;
		protected int _pORTNO;
		protected string _pORTTYPE;

		protected string _pORTENABLEMODE;
		protected string _pORTTRANSFERMODE;
        //protected string _pORTSTATUS;
		protected int _cASSETTESEQNO;
		//protected string _cASSETTESTATUS;
        protected string _pORTCASSETTESTATUS;
        protected string _cASSETTEID;
        protected int _cRITERIALNUMBER;
        protected string _portGrade;
        protected string _portGroupNo;
        protected string _sortGrade;

		#endregion

		#region Constructors

		public PortHistory() { }

        public PortHistory(DateTime uPDATETIME, 
                            string lINEID, 
                            string nODEID, 
                            string pORTID, 
                            int pORTNO, 
                            string pORTTYPE, 
                            
                            string pORTENABLEMODE, 
                            string pORTTRANSFERMODE, 
                            string pORTSTATUS,
                            int cASSETTESEQNO, string cASSETTESTATUS, string pORTCASSETTESTATUS, string cASSETTEID)
		{
			this._uPDATETIME = uPDATETIME;
			this._lINEID = lINEID;
			this._nODEID = nODEID;
			this._pORTID = pORTID;
			this._pORTNO = pORTNO;
			this._pORTTYPE = pORTTYPE;
		
			this._pORTENABLEMODE = pORTENABLEMODE;
			this._pORTTRANSFERMODE = pORTTRANSFERMODE;
			//this._pORTSTATUS = pORTSTATUS;
			this._cASSETTESEQNO = cASSETTESEQNO;
			//this._cASSETTESTATUS = cASSETTESTATUS;
            this._pORTCASSETTESTATUS = pORTCASSETTESTATUS;
            this._cASSETTEID = cASSETTEID;
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

		public virtual string PORTID
		{
			get { return _pORTID; }
			set
			{				
				_pORTID = value;
			}
		}

		public virtual int PORTNO
		{
			get { return _pORTNO; }
			set { _pORTNO = value; }
		}

		public virtual string PORTTYPE
		{
			get { return _pORTTYPE; }
			set
			{				
				_pORTTYPE = value;
			}
		}

	
		public virtual string PORTENABLEMODE
		{
			get { return _pORTENABLEMODE; }
			set
			{				
				_pORTENABLEMODE = value;
			}
		}

		public virtual string PORTTRANSFERMODE
		{
			get { return _pORTTRANSFERMODE; }
			set
			{				
				_pORTTRANSFERMODE = value;
			}
		}

        //public virtual string PORTSTATUS
        //{
        //    get { return _pORTSTATUS; }
        //    set
        //    {				
        //        _pORTSTATUS = value;
        //    }
        //}

		public virtual int CASSETTESEQNO
		{
			get { return _cASSETTESEQNO; }
			set { _cASSETTESEQNO = value; }
		}

        //public virtual string CASSETTESTATUS
        //{
        //    get { return _cASSETTESTATUS; }
        //    set
        //    {				
        //        _cASSETTESTATUS = value;
        //    }
        //}

        public virtual string CASSETTEPORTSTATUS
        {
            get { return _pORTCASSETTESTATUS; }
            set { _pORTCASSETTESTATUS = value; }
        }

        public virtual string CASSETTEID
        {
            get { return _cASSETTEID; }
            set { _cASSETTEID = value; }

        }
        // Add For ODF Criterial No 20170222 Tom 
        public virtual int CRITERIALNUMBER {

            get { return _cRITERIALNUMBER; }
            set { _cRITERIALNUMBER = value; }
        }

        public virtual string PORTGRADE{
            get { return _portGrade; }
            set { _portGrade = value; }
        }

        public virtual string PORTGROUPNO {
            get { return _portGroupNo; }
            set { _portGroupNo = value; }
        }

        public virtual string SORTGRADE {
            get { return _sortGrade; }
            set { _sortGrade = value; }
        }
		#endregion
	}
	#endregion
}