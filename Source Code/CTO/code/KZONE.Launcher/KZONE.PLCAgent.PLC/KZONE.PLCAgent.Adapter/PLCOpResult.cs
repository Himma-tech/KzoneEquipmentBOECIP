using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc operation result
	/// </summary>
	public class PLCOpResult
	{
		/// <summary>
		/// OK constant=0
		/// </summary>
		public const int OK = 0;

		/// <summary>
		/// NG constant=-1
		/// </summary>
		public const int NG = -1;

		/// <summary>
		/// EXCEPTION constant=-1
		/// </summary>
		public const int EXCEPTION = -2;

		/// <summary>
		/// return code
		/// </summary>
		public int RetCode = 0;

		/// <summary>
		/// return error message
		/// </summary>
		public string RetErrMsg = string.Empty;

		/// <summary>
		/// default constructor
		/// </summary>
		public PLCOpResult()
		{
		}

		/// <summary>
		/// constructor with code and message
		/// </summary>
		/// <param name="code"></param>
		/// <param name="errMsg"></param>
		public PLCOpResult(int code, string errMsg)
		{
			this.RetCode = code;
			this.RetErrMsg = errMsg;
		}

		/// <summary>
		/// set code and message
		/// </summary>
		/// <param name="code"></param>
		/// <param name="errMsg"></param>
		public void Set(int code, string errMsg)
		{
			this.RetCode = code;
			this.RetErrMsg = errMsg;
		}
	}
}
