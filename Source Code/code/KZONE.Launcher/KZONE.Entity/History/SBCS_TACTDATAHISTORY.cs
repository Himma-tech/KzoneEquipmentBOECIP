using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCTACTDATAHISTORY

	/// <summary>
	/// SBCSPROCESSDATAHISTORY object for NHibernate mapped table 'SBCS_TACTDATAHISTORY'.
	/// </summary>
    public class TactDataHistory : EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected int _cASSETTESEQNO;
		protected int _jOBSEQNO;
		protected string _jOBID;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected string _fILENAMA;
		protected int _pROCESSTIME;
		protected string _lOCALPROCESSSTARTTIME;
		protected string _lOCALPROCSSSENDTIME;

		#endregion

		#region Constructors

		public TactDataHistory() { }

        public TactDataHistory(int cASSETTESEQNO, int jOBSEQNO, string jOBID, DateTime uPDATETIME, string fILENAMA, int pROCESSTIME, string lOCALPROCESSSTARTTIME, string lOCALPROCSSSENDTIME)
		{
			this._cASSETTESEQNO = cASSETTESEQNO;
			this._jOBSEQNO = jOBSEQNO;
			this._jOBID = jOBID;
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