using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// event metadata class
	/// </summary>
	[Serializable]
	public class EventMetadata
	{
		/// <summary>
		/// get Event的名稱
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// get logical station no (1-1023)
		/// </summary>
		public int LogicalStationNo
		{
			get;
			internal set;
		}

		/// <summary>
		/// get PLC的Device類型(B,W,D,..)
		/// </summary>
		public string DeviceCode
		{
			get;
			internal set;
		}

		/// <summary>
		/// get 開始的Address,例如:0x1000, 0x2000, (16 or 10 base)
		/// </summary>
		public string Address
		{
			get;
			internal set;
		}

		/// <summary>
		/// get 總共佔了多少的點位數. 如果deviceCode=B，Point單位=bit.如果deviceCode=W，Points單位=W." (10 base)
		/// </summary>
		public int Points
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is skip deocde item
		/// </summary>
		public bool SkipDecode
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is bit device type
		/// </summary>
		public bool IsBitDeviceType
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is address is 16 base
		/// </summary>
		public bool IsAddressHex
		{
			get;
			internal set;
		}

		/// <summary>
		/// get start address (10 base)
		/// </summary>
		public int StartAddress10
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is apply Transform
		/// </summary>
		public bool IsApplyTransform
		{
			get;
			internal set;
		}

		/// <summary>
		/// get Original開始的Address,例如:0x1000, 0x2000, (16 or 10 base)
		/// </summary>
		public string OriginalAddress
		{
			get;
			internal set;
		}

		/// <summary>
		/// get Original start address (10 base)
		/// </summary>
		public int OriginalStartAddress10
		{
			get;
			internal set;
		}

		/// <summary>
		/// get Original PLC的Device類型(B,W,D,..)
		/// </summary>
		public string OriginalDeviceCode
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is Original address is 16 base
		/// </summary>
		public bool IsOriginalAddressHex
		{
			get;
			internal set;
		}
	}
}
