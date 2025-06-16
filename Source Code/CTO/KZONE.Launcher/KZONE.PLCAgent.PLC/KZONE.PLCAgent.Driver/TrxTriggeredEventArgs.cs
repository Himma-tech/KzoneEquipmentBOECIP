using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// trx triggered event argurments
	/// </summary>
	public class TrxTriggeredEventArgs : EventArgs
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
		/// constructor
		/// </summary>
		/// <param name="trx">source trx</param>
		public TrxTriggeredEventArgs(Trx trx)
		{
			this.SourceTrx = trx;
		}
	}
}
