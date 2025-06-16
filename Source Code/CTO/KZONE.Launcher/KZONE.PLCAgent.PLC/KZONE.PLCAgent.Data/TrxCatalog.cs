using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// trxcatalog class
	/// </summary>
	public class TrxCatalog
	{
		/// <summary>
		/// get mapping collection
		/// </summary>
		public NameObjectCollection<Mapping> Mappings
		{
			get;
			internal set;
		}

		/// <summary>
		/// get from trxcatalog
		/// </summary>
		public string FromTrxCatalog
		{
			get;
			internal set;
		}

		/// <summary>
		/// get to trxcatalog
		/// </summary>
		public string ToTrxCatalog
		{
			get;
			internal set;
		}

		/// <summary>
		/// get primary (BCS/EQP)
		/// </summary>
		public TrxCatalogPrimaryEnum Primary
		{
			get;
			internal set;
		}

		/// <summary>
		/// get action (PBW_SBW,PBW_SB,PBW,PB,PW,PB_SBW,PB_SB)
		/// </summary>
		public TrxCatalogActionEnum TrxCatalogAction
		{
			get;
			internal set;
		}

		/// <summary>
		/// get menucatalog (CommonFunction,PortManagement,SpecialFunction,LineSpecialFunction)
		/// </summary>
		public string MenuCatalog
		{
			get;
			internal set;
		}

		/// <summary>
		/// get chart
		/// </summary>
		public string Chart
		{
			get;
			internal set;
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public TrxCatalog()
		{
			this.Mappings = new NameObjectCollection<Mapping>();
		}
	}
}
