using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// trxcatalog action enumeration
	/// </summary>
	public enum TrxCatalogActionEnum : byte
	{
		/// <summary>
		/// PBW/SBW
		/// </summary>
		PBW_SBW = 1,
		/// <summary>
		/// PBW/SB
		/// </summary>
		PBW_SB,
		/// <summary>
		/// PBW
		/// </summary>
		PBW,
		/// <summary>
		/// PB
		/// </summary>
		PB,
		/// <summary>
		/// PW
		/// </summary>
		PW,
		/// <summary>
		/// PB/SBW
		/// </summary>
		PB_SBW,
		/// <summary>
		/// PB/SB
		/// </summary>
		PB_SB,
		/// <summary>
		/// PWI/SWI
		/// </summary>
		PWI_SWI,
		/// <summary>
		/// PWI
		/// </summary>
		PWI
	}
}
