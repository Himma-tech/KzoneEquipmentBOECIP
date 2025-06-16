using System;
using System.Collections.Generic;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc random read/write operation
	/// </summary>
	public class PLCRandOp
	{
		/// <summary>
		/// nested class to represet random read/write block
		/// </summary>
		public class RandBlock
		{
			/// <summary>
			/// read/write device type
			/// </summary>
			public string DevType;

			/// <summary>
			/// read/write start device no (16 or 10 base)
			/// </summary>
			public string DevNo;

			/// <summary>
			/// read/write device points (10 base)
			/// </summary>
			public int Points;

			/// <summary>
			/// data buffer
			/// *NOTE: (1)buf must be pre-allocated (2)bit device type buf,data are group by on a 16 point basis, ex. B0 write 17 points buf[0]=BF~B0,buf[1]=B10)
			/// </summary>
			public short[] Buf;
		}

		/// <summary>
		/// event key
		/// </summary>
		public string EventKey;

		/// <summary>
		/// operation delay time(ms),0=no delay
		/// </summary>
		public int OpDelayTimeMS;

		/// <summary>
		/// block group
		/// </summary>
		public List<PLCRandOp.RandBlock> Blocks;

		/// <summary>
		/// is operation complete
		/// </summary>
		public bool IsOpComp;

		/// <summary>
		/// operation start datetime
		/// </summary>
		public DateTime OpStartDT;

		/// <summary>
		/// operation complete datetime
		/// </summary>
		public DateTime OpCompDT;
	}
}
