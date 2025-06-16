using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// id class
	/// </summary>
	public class Id
	{
		/// <summary>
		/// get id name(key)
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// get id value
		/// </summary>
		public string Value
		{
			get;
			internal set;
		}
	}
}
