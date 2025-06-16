using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc op request result pair
	/// </summary>
	public class PLCOpRequestResult
	{
		/// <summary>
		/// get/set plc op request
		/// </summary>
		public PLCOpRequest request
		{
			get;
			set;
		}

		/// <summary>
		/// get/set plc op result
		/// </summary>
		public PLCOpResult result
		{
			get;
			set;
		}
	}
}
