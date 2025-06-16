using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCSPROCESSDATAHISTORY

	/// <summary>
	/// SBCSPROCESSDATAHISTORY object for NHibernate mapped table 'SBCS_PROCESSDATAHISTORY'.
	/// </summary>
    public class ProcessDataHistory : EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected int _cASSETTESEQNO;
		protected int _jOBSEQNO;
		protected string _jOBID;
		protected string _tRXID;
		protected string _mESCONTROLSTATE;
		protected string _nODEID;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected string _fILENAMA;
		protected int _pROCESSTIME;
		protected string _lOCALPROCESSSTARTTIME;
		protected string _lOCALPROCSSSENDTIME;

		#endregion

		#region Constructors

		public ProcessDataHistory() { }

        public ProcessDataHistory(int cASSETTESEQNO, int jOBSEQNO, string jOBID, string tRXID, string mESCONTROLSTATE, string nODEID, DateTime uPDATETIME, string fILENAMA, int pROCESSTIME, string lOCALPROCESSSTARTTIME, string lOCALPROCSSSENDTIME)
		{
			this._cASSETTESEQNO = cASSETTESEQNO;
			this._jOBSEQNO = jOBSEQNO;
			this._jOBID = jOBID;
			this._tRXID = tRXID;
			this._mESCONTROLSTATE = mESCONTROLSTATE;
			this._nODEID = nODEID;
			this._uPDATETIME = uPDATETIME;
			this._fILENAMA = fILENAMA;
			this._pROCESSTIME = pROCESSTIME;
			this._lOCALPROCESSSTARTTIME = lOCALPROCESSSTARTTIME;
			this._lOCALPROCSSSENDTIME = lOCALPROCSSSENDTIME;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual int CASSETTESEQNO
		{
			get { return _cASSETTESEQNO; }
			set { _cASSETTESEQNO = value; }
		}

		public virtual int JOBSEQNO
		{
			get { return _jOBSEQNO; }
			set { _jOBSEQNO = value; }
		}

		public virtual string JOBID
		{
			get { return _jOBID; }
			set
			{				
				_jOBID = value;
			}
		}

		public virtual string TRXID
		{
			get { return _tRXID; }
			set
			{				
				_tRXID = value;
			}
		}

		public virtual string MESCONTROLSTATE
		{
			get { return _mESCONTROLSTATE; }
			set
			{				
				_mESCONTROLSTATE = value;
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

		public virtual DateTime UPDATETIME
		{
			get { return _uPDATETIME; }
			set { _uPDATETIME = value; }
		}

		public virtual string FILENAMA
		{
			get { return _fILENAMA; }
			set
			{				
				_fILENAMA = value;
			}
		}

		public virtual int PROCESSTIME
		{
			get { return _pROCESSTIME; }
			set { _pROCESSTIME = value; }
		}

		public virtual string LOCALPROCESSSTARTTIME
		{
			get { return _lOCALPROCESSSTARTTIME; }
			set
			{				
				_lOCALPROCESSSTARTTIME = value;
			}
		}

		public virtual string LOCALPROCSSSENDTIME
		{
			get { return _lOCALPROCSSSENDTIME; }
			set
			{				
				_lOCALPROCSSSENDTIME = value;
			}
		}

		

		#endregion
	}
	#endregion
}