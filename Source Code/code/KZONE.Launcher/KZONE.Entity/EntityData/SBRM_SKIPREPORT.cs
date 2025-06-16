using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMSKIPREPORT

	/// <summary>
	/// SBRMSKIPREPORT object for NHibernate mapped table 'SBRM_SKIPREPORT'.
	/// </summary>
	public class SkipReportEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _nODEID;
		protected string _nODENO;
		protected string _uNITNO;
		protected string _uNITID;
		protected string _sKIPREPORTTRX;
		protected string _sKIPAGENT;
		protected string _sKIPCONDITION;

		#endregion

		#region Constructors

		public SkipReportEntityData() { }

        public SkipReportEntityData(string lINEID, string nODEID, string nODENO, string uNITNO, string uNITID, string sKIPREPORTTRX, string sKIPAGENT, string sKIPCONDITION)
		{
			this._lINEID = lINEID;
			this._nODEID = nODEID;
			this._nODENO = nODENO;
			this._uNITNO = uNITNO;
			this._uNITID = uNITID;
			this._sKIPREPORTTRX = sKIPREPORTTRX;
			this._sKIPAGENT = sKIPAGENT;
			this._sKIPCONDITION = sKIPCONDITION;
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

		public virtual string UNITNO
		{
			get { return _uNITNO; }
			set
			{				
				_uNITNO = value;
			}
		}

		public virtual string UNITID
		{
			get { return _uNITID; }
			set
			{				
				_uNITID = value;
			}
		}

		public virtual string SKIPREPORTTRX
		{
			get { return _sKIPREPORTTRX; }
			set
			{				
				_sKIPREPORTTRX = value;
			}
		}

		public virtual string SKIPAGENT
		{
			get { return _sKIPAGENT; }
			set
			{				
				_sKIPAGENT = value;
			}
		}

		public virtual string SKIPCONDITION
		{
			get { return _sKIPCONDITION; }
			set
			{				
				_sKIPCONDITION = value;
			}
		}

		

		#endregion
	}
	#endregion
}