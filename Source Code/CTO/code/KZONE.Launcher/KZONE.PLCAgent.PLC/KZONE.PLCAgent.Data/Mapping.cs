using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// mapping class
	/// </summary>
	public class Mapping
	{
		/// <summary>
		/// get from item
		/// </summary>
		public string FromItem
		{
			get;
			internal set;
		}

		/// <summary>
		/// get to item
		/// </summary>
		public string ToItem
		{
			get;
			internal set;
		}
	}
}
