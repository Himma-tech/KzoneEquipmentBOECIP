using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// trxcatalog menucatalog enumeration
	/// </summary>
	public enum TrxCatalogMenuCatalogEnum : byte
	{
		/// <summary>
		/// common function
		/// </summary>
		CommonFunction = 1,
		/// <summary>
		/// port management
		/// </summary>
		PortManagement,
		/// <summary>
		/// sepcial function
		/// </summary>
		SpecialFunction,
		/// <summary>
		/// line special function
		/// </summary>
		LineSpecialFunction
	}
}
