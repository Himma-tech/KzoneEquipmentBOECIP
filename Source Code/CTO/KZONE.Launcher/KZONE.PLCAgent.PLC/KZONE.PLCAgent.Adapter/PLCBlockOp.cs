using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc block read/write operation
	/// </summary>
	public class PLCBlockOp
	{
		/// <summary>
		/// event key
		/// </summary>
		public string EventKey;

		/// <summary>
		/// operation delay time(ms),0=no delay
		/// </summary>
		public int OpDelayTimeMS;

		/// <summary>
		/// read/write device type
		/// </summary>
		public string DevType;

		/// <summary>
		/// read/write start device no (16 or 10 base)
		/// *NOTE: when bit device type assigned,devno must be Multiple of 16
		/// </summary>
		public string DevNo;

		/// <summary>
		/// read/write device points (10 base) 
		/// *NOTE: when bit device type assigned,points must be Multiple of 16
		/// </summary>
		public int Points;

		/// <summary>
		/// data buffer
		/// *NOTE: (1)buf must be pre-allocated (2)when bit device type assigned,data are read from/written to specific points on a 16 point basis)
		/// </summary>
		public short[] Buf;

		/// <summary>
		/// buffer start offset
		/// </summary>
		public int Offset;

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
