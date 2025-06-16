using System;
using System.Collections;

namespace KZONE.Entity
{
	#region SBRMSUBJOBDATA

	/// <summary>
	/// SBRMSUBJOBDATA object for NHibernate mapped table 'SBRM_SUBJOBDATA'.
	/// </summary>
	public class SubJobDataEntityData:EntityData
	{
		#region Member Variables
		
		protected long _id;
		protected string _lINETYPE;
		protected string _iTEMNAME;
		protected int _iTEMLENGTH;
		protected string _sUBITEMNAME;
		protected string _sUBITEMDESC;
		protected int _sUBITEMLOFFSET;
		protected int _sUBITEMLENGTH;
		protected string _mEMO;

		#endregion

		#region Constructors

		public SubJobDataEntityData() { }

        public SubJobDataEntityData(string lINETYPE, string iTEMNAME, int iTEMLENGTH, string sUBITEMNAME, string sUBITEMDESC, int sUBITEMLOFFSET, int sUBITEMLENGTH, string mEMO)
		{
			this._lINETYPE = lINETYPE;
			this._iTEMNAME = iTEMNAME;
			this._iTEMLENGTH = iTEMLENGTH;
			this._sUBITEMNAME = sUBITEMNAME;
			this._sUBITEMDESC = sUBITEMDESC;
			this._sUBITEMLOFFSET = sUBITEMLOFFSET;
			this._sUBITEMLENGTH = sUBITEMLENGTH;
			this._mEMO = mEMO;
		}

		#endregion

		#region Public Properties

		public virtual long Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public virtual string LINETYPE
		{
			get { return _lINETYPE; }
			set
			{				
				_lINETYPE = value;
			}
		}

		public virtual string ITEMNAME
		{
			get { return _iTEMNAME; }
			set
			{				
				_iTEMNAME = value;
			}
		}

		public virtual int ITEMLENGTH
		{
			get { return _iTEMLENGTH; }
			set { _iTEMLENGTH = value; }
		}

		public virtual string SUBITEMNAME
		{
			get { return _sUBITEMNAME; }
			set
			{				
				_sUBITEMNAME = value;
			}
		}

		public virtual string SUBITEMDESC
		{
			get { return _sUBITEMDESC; }
			set
			{				
				_sUBITEMDESC = value;
			}
		}

		public virtual int SUBITEMLOFFSET
		{
			get { return _sUBITEMLOFFSET; }
			set { _sUBITEMLOFFSET = value; }
		}

		public virtual int SUBITEMLENGTH
		{
			get { return _sUBITEMLENGTH; }
			set { _sUBITEMLENGTH = value; }
		}

		public virtual string MEMO
		{
			get { return _mEMO; }
			set
			{				
				_mEMO = value;
			}
		}

		

		#endregion
	}
	#endregion
}