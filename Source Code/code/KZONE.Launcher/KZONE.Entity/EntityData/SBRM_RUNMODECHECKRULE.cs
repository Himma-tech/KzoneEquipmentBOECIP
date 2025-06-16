using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMRUNMODECHECKRULE

	/// <summary>
	/// SBRMRUNMODECHECKRULE object for NHibernate mapped table 'SBRM_RUNMODECHECKRULE'.
	/// </summary>
     [Serializable]
	public class RunModeCheckRuleEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINEID;
		protected string _rUNMODE;
		protected string _eQPNOLIST;
		protected DateTime _uPDATETIME=DateTime.Now;
		protected string _oPERATORID;

		#endregion

		#region Constructors

		public RunModeCheckRuleEntityData() { }

		public RunModeCheckRuleEntityData( string lINETYPE, string rUNMODE, string eQPNOLIST, DateTime uPDATETIME, string oPERATORID )
		{
			this._lINEID = lINETYPE;
			this._rUNMODE = rUNMODE;
			this._eQPNOLIST = eQPNOLIST;
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

		public virtual string RUNMODE
		{
			get { return _rUNMODE; }
			set
			{				
				_rUNMODE = value;
			}
		}

		public virtual string EQPNOLIST
		{
			get { return _eQPNOLIST; }
			set
			{				
				_eQPNOLIST = value;
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