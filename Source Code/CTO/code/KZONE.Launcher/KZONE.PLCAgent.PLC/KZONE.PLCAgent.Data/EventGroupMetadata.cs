using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// eventgroup metadata class
	/// </summary>
	[Serializable]
	public class EventGroupMetadata
	{
		/// <summary>
		/// get EventGroup的名稱
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// get eventgroup dir (B2E,E2B)
		/// </summary>
		public EventGroupDirEnum Dir
		{
			get;
			internal set;
		}
	}
}
