using System;
using System.Collections.Generic;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc read/write/scan operation request
	/// </summary>
	public class PLCOpRequest
	{
		/// <summary>
		/// request sno
		/// </summary>
		public int ReqSNo;

		/// <summary>
		/// user object
		/// </summary>
		public object Tag;

		/// <summary>
		/// has delay read/write operation
		/// </summary>
		public bool HasOpDelay;

		/// <summary>
		/// logiccal station no(1-1023)
		/// </summary>
		public int LogicalStationNo;

		/// <summary>
		/// op list for block read/write operation
		/// </summary>
		public List<PLCBlockOp> BlockOps = new List<PLCBlockOp>();

		/// <summary>
		/// op list for random read/write operation
		/// </summary>
		public List<PLCRandOp> RandOps = new List<PLCRandOp>();

		/// <summary>
		/// request start datetime
		/// </summary>
		public DateTime ReqStartDT;

		/// <summary>
		/// request complete datetime
		/// </summary>
		public DateTime ReqCompDT;

		/// <summary>
		/// request operation type(READ,WRITE)
		/// </summary>
		public string ReqOpType;

		/// <summary>
		/// delay start time
		/// </summary>
		public DateTime DelayStartTime;

		/// <summary>
		/// is request timeout 
		/// </summary>
		public bool IsReqTimeOut;
	}
}
