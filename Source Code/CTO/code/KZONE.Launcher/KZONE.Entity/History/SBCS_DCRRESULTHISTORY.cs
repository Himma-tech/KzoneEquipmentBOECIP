using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBCSDCRRESULTHISTORY

	/// <summary>
	/// SBCSDCRRESULTHISTORY object for NHibernate mapped table 'SBCS_DCRRESULTHISTORY'.
	/// </summary>
	public class DCRResultHistory:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected int _cASSETTESEQNO;
		protected int _cASSETTESLOTNO;
		protected string _glASSID;
		protected string _rEADGLASSID;
		protected string _rESULT;
		protected string _dESCRIPTION;
        protected string _nodeNo;
        protected string _dCRNo;

		#endregion

		#region Constructors

		public DCRResultHistory() { }

        public DCRResultHistory(DateTime uPDATETIME, int cASSETTESEQNO, int cASSETTESLOTNO, string glASSID, string rEADGLASSID, string rESULT, string dESCRIPTION)
		{
			this._uPDATETIME = uPDATETIME;
			this._cASSETTESEQNO = cASSETTESEQNO;
			this._cASSETTESLOTNO = cASSETTESLOTNO;
			this._glASSID = glASSID;
			this._rEADGLASSID = rEADGLASSID;
			this._rESULT = rESULT;
			this._dESCRIPTION = dESCRIPTION;
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

		public virtual int CASSETTESEQNO
		{
			get { return _cASSETTESEQNO; }
			set { _cASSETTESEQNO = value; }
		}

		public virtual int CASSETTESLOTNO
		{
			get { return _cASSETTESLOTNO; }
			set { _cASSETTESLOTNO = value; }
		}

		public virtual string GlASSID
		{
			get { return _glASSID; }
			set
			{				
				_glASSID = value;
			}
		}

		public virtual string READGLASSID
		{
			get { return _rEADGLASSID; }
			set
			{				
				_rEADGLASSID = value;
			}
		}

		public virtual string RESULT
		{
			get { return _rESULT; }
			set
			{				
				_rESULT = value;
			}
		}

		public virtual string DESCRIPTION
		{
			get { return _dESCRIPTION; }
			set
			{				
				_dESCRIPTION = value;
			}
		}

        public virtual string NODENO {
            get { return _nodeNo; }
            set { _nodeNo = value; }
        }

        public virtual string DCRNO {
            get { return _dCRNo; }
            set { _dCRNo = value; }
        }
		

		#endregion
	}
	#endregion
}