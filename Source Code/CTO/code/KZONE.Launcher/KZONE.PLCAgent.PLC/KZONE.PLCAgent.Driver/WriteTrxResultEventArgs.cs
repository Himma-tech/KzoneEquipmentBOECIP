using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// WriteTrx Result event argurments
	/// </summary>
	public class WriteTrxResultEventArgs : EventArgs
	{
		/// <summary>
		/// source trx
		/// </summary>
		public Trx SourceTrx
		{
			get;
			set;
		}

		/// <summary>
		/// source station no
		/// </summary>
		public int SourceStation
		{
			get;
			set;
		}

		/// <summary>
		/// read operation has error occure
		/// </summary>
		public bool HasError
		{
			get;
			set;
		}

		/// <summary>
		/// return code
		/// </summary>
		public int RetCode
		{
			get;
			set;
		}

		/// <summary>
		/// return message
		/// </summary>
		public string RetMsg
		{
			get;
			set;
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="trx">source trx</param>		
		/// <param name="code">return code</param>
		/// <param name="hasErr">write operation has error occure</param>
		/// <param name="msg">return message</param>
		/// <param name="station">source station,0=station not specific</param>
		public WriteTrxResultEventArgs(Trx trx, int station, bool hasErr, int code, string msg)
		{
			this.SourceTrx = trx;
			this.SourceStation = station;
			this.HasError = hasErr;
			this.RetCode = code;
			this.RetMsg = msg;
		}
	}
}
