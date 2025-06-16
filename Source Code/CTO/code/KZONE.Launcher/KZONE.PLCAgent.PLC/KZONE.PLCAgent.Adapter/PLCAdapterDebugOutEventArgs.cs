using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plcadapter debugout event argurments
	/// </summary>
	public class PLCAdapterDebugOutEventArgs : EventArgs
	{
		/// <summary>
		/// method name
		/// </summary>
		public string MethodName
		{
			get;
			set;
		}

		/// <summary>
		/// debug message
		/// </summary>
		public string Message
		{
			get;
			set;
		}

		/// <summary>
		/// debug code
		/// </summary>
		public int Code
		{
			get;
			set;
		}

		/// <summary>
		/// debug level
		/// </summary>
		public string LogLevel
		{
			get;
			set;
		}

		/// <summary>
		/// tracking key
		/// </summary>
		public string TrackKey
		{
			get;
			set;
		}

		/// <summary>
		/// source station no
		/// </summary>
		public int SourceStationNo
		{
			get;
			set;
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="level">debug out level</param>
		/// <param name="method">method name</param>
		/// <param name="msg">debug message</param>
		/// <param name="code">return code</param>
		/// <param name="trackKey">tracking key</param>
		/// <param name="station">source station no</param>
		public PLCAdapterDebugOutEventArgs(string level, string method, int code, string msg, string trackKey, int station)
		{
			this.MethodName = method;
			this.Code = code;
			this.Message = msg;
			this.LogLevel = level;
			this.TrackKey = trackKey;
			this.SourceStationNo = station;
		}
	}
}
