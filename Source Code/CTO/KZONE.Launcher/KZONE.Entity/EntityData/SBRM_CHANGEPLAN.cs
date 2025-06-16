using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMCHANGEPLAN

	/// <summary>
	/// SBRMCHANGEPLAN object for NHibernate mapped table 'SBRM_CHANGEPLAN'.
	/// </summary>
	public class ChangePlanEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _pLANID;
		protected string _sOURCECASSETTEID;
		protected string _sLOTNO;
		protected string _jOBID;
		protected string _tARGETASSETTEID;
		protected DateTime _uPDATETIME;
		protected string _oPERATORID;

		#endregion

		#region Constructors

		public ChangePlanEntityData() { }

        public ChangePlanEntityData(string lINEID, string pLANID, string sOURCECASSETTEID, string sLOTNO, string jOBID, string tARGETASSETTEID, DateTime uPDATETIME, string oPERATORID)
		{
			this._lINEID = lINEID;
			this._pLANID = pLANID;
			this._sOURCECASSETTEID = sOURCECASSETTEID;
			this._sLOTNO = sLOTNO;
			this._jOBID = jOBID;
			this._tARGETASSETTEID = tARGETASSETTEID;
			this._uPDATETIME = uPDATETIME;
			this._oPERATORID = oPERATORID;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual string LINEID
		{
			get { return _lINEID; }
			set
			{				
				_lINEID = value;
			}
		}

		public virtual string PLANID
		{
			get { return _pLANID; }
			set
			{				
				_pLANID = value;
			}
		}

		public virtual string SOURCECASSETTEID
		{
			get { return _sOURCECASSETTEID; }
			set
			{				
				_sOURCECASSETTEID = value;
			}
		}

		public virtual string SLOTNO
		{
			get { return _sLOTNO; }
			set
			{				
				_sLOTNO = value;
			}
		}

		public virtual string JOBID
		{
			get { return _jOBID; }
			set
			{				
				_jOBID = value;
			}
		}

		public virtual string TARGETASSETTEID
		{
			get { return _tARGETASSETTEID; }
			set
			{				
				_tARGETASSETTEID = value;
			}
		}

		public virtual DateTime UPDATETIME
		{
			get { return _uPDATETIME; }
			set { _uPDATETIME = value; }
		}

		public virtual string OPERATORID
		{
			get { return _oPERATORID; }
			set
			{				
				_oPERATORID = value;
			}
		}

		

		#endregion
	}
	#endregion
}