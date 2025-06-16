using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// static plc driver factory class 
	/// </summary>
	public static class PLCDriverFactory
	{
		/// <summary>
		/// create plc driver instance
		/// </summary>
		/// <param name="id">driver id</param>
		/// <returns>runtime instance,null if err occur</returns>
		public static IPLCDriver CreatePLCDriver(string id = "")
		{
			return new PLCDriverRuntime
			{
				DriverId = id
			};
		}
	}
}
