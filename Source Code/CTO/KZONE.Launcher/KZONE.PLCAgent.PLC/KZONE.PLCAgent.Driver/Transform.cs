using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// Transform class
	/// </summary>
	public class Transform
	{
		/// <summary>
		/// get/set Target NodeNo (trx's nodeno)
		/// </summary>
		public string TargetNodeNo
		{
			get;
			set;
		}

		/// <summary>
		/// get/set Target logical station no (1-1023)
		/// </summary>
		public int TargetLogicalStationNo
		{
			get;
			set;
		}

		/// <summary>
		/// get/set from DeviceCode
		/// </summary>
		public string FromDeviceCode
		{
			get;
			set;
		}

		/// <summary>
		/// get/set from Node StartAddress (16 or 10 base)
		/// </summary>
		public string FromNodeStartAddress
		{
			get;
			set;
		}

		/// <summary>
		/// get/set To DeviceCode
		/// </summary>
		public string ToDeviceCode
		{
			get;
			set;
		}

		/// <summary>
		/// get/set To Node StartAddress (16 or 10 base)
		/// </summary>
		public string ToNodeStartAddress
		{
			get;
			set;
		}

		internal DeviceSymbol FromDevice
		{
			get;
			set;
		}

		internal DeviceSymbol ToDevice
		{
			get;
			set;
		}

		internal int FromNodeStartAddr10
		{
			get;
			set;
		}

		internal int ToNodeStartAddr10
		{
			get;
			set;
		}
	}
}
