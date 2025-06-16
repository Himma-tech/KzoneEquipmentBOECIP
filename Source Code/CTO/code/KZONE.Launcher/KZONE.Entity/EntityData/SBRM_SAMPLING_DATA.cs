using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMSAMPLINGDATA

	/// <summary>
	/// SBRMSAMPLINGDATA object for NHibernate mapped table 'SBRM_SAMPLING_DATA'.
	/// </summary>
	public class SamplingEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _sERVERNAME;
		protected string _sAMPLINGINDEX;
		protected string _sAMPLINGFENZI;
		protected string _sAMPLINGFENMU;
		protected DateTime _tRXDATE=DateTime.Now;
		protected string _tRXUSERID;
		protected string _tRXIP;
		protected string _rEMARK;

		#endregion

		#region Constructors

		public SamplingEntityData() { }

        public SamplingEntityData(string sERVERNAME, string sAMPLINGINDEX, string sAMPLINGFENZI, string sAMPLINGFENMU, DateTime tRXDATE, string tRXUSERID, string tRXIP, string rEMARK)
		{
			this._sERVERNAME = sERVERNAME;
			this._sAMPLINGINDEX = sAMPLINGINDEX;
			this._sAMPLINGFENZI = sAMPLINGFENZI;
			this._sAMPLINGFENMU = sAMPLINGFENMU;
			this._tRXDATE = tRXDATE;
			this._tRXUSERID = tRXUSERID;
			this._tRXIP = tRXIP;
			this._rEMARK = rEMARK;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual string SERVERNAME
		{
			get { return _sERVERNAME; }
			set
			{				
				_sERVERNAME = value;
			}
		}

		public virtual string SAMPLINGINDEX
		{
			get { return _sAMPLINGINDEX; }
			set
			{				
				_sAMPLINGINDEX = value;
			}
		}

		public virtual string SAMPLINGFENZI
		{
			get { return _sAMPLINGFENZI; }
			set
			{				
				_sAMPLINGFENZI = value;
			}
		}

		public virtual string SAMPLINGFENMU
		{
			get { return _sAMPLINGFENMU; }
			set
			{				
				_sAMPLINGFENMU = value;
			}
		}

		public virtual DateTime TRXDATE
		{
			get { return _tRXDATE; }
			set { _tRXDATE = value; }
		}

		public virtual string TRXUSERID
		{
			get { return _tRXUSERID; }
			set
			{				
				_tRXUSERID = value;
			}
		}

		public virtual string TRXIP
		{
			get { return _tRXIP; }
			set
			{				
				_tRXIP = value;
			}
		}

		public virtual string REMARK
		{
			get { return _rEMARK; }
			set
			{				
				_rEMARK = value;
			}
		}

		

		#endregion
	}
	#endregion
}