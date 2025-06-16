using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc disconnected event argurments
	/// </summary>
	public class PLCDisconnectedEventArgs : EventArgs
	{
		/// <summary>
		/// source station no
		/// </summary>
		public int SourceStationNo
		{
			get;
			set;
		}

		/// <summary>
		/// is default adapter
		/// </summary>
		public bool IsDefault
		{
			get;
			set;
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="station">source station no</param>
		/// <param name="isdefault">is default adapter</param>
		public PLCDisconnectedEventArgs(int station, bool isdefault)
		{
			this.SourceStationNo = station;
			this.IsDefault = isdefault;
		}
	}
}
