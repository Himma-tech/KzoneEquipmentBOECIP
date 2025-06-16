using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc device symbol definition
	/// </summary>
	public class DeviceSymbol : ICloneable
	{
		/// <summary>
		/// device type string
		/// </summary>
		public string stype = string.Empty;

		/// <summary>
		/// device type number
		/// </summary>
		public int ntype;

		/// <summary>
		/// device digital base(10 or 16)
		/// </summary>
		public int nbase;

		/// <summary>
		/// is bit device
		/// </summary>
		public bool isbit;

		/// <summary>
		/// is own station
		/// </summary>
		public bool isownst;

		/// <summary>
		/// clone devicesymbol
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
