using System;
using System.Diagnostics;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// watchdata class
	/// </summary>
	public class WatchData
	{
		/// <summary>
		/// get item name(key)
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// logic station no (1-1023)
		/// </summary>
		public int LogicalStationNo
		{
			get;
			internal set;
		}

		/// <summary>
		/// 開始的Address,例如:0x100000, 001000 (16 or 10)
		/// </summary>
		public string Address
		{
			get;
			internal set;
		}

		/// <summary>
		/// 總共佔了多少的點位數. 如果deviceCode=B，Point單位=bit.如果deviceCode=W，Points單位=W."
		/// </summary>
		public int Points
		{
			get;
			internal set;
		}

		/// <summary>
		/// PLC的Device類型(B,W,D,..)
		/// </summary>
		public string DeviceCode
		{
			get;
			internal set;
		}

		/// <summary>
		/// get plc scan interval(ms)
		/// </summary>
		public int ScanIntervalMS
		{
			get;
			internal set;
		}

		/// <summary>
		/// get last scan datetime
		/// </summary>
		public DateTime LastScanDT
		{
			get;
			internal set;
		}

		/// <summary>
		/// scan stopwatch
		/// </summary>
		public Stopwatch ScanStopWatch
		{
			get;
			internal set;
		}

		/// <summary>
		/// get last scan spent time
		/// </summary>
		public TimeSpan LastScanSpentTime
		{
			get;
			internal set;
		}

		internal int TotalScan
		{
			get;
			set;
		}

		/// <summary>
		/// current scan per sec
		/// </summary>
		public int ScanPerSec
		{
			get;
			internal set;
		}

		/// <summary>
		/// max scan per sec
		/// </summary>
		public int MaxScanPerSec
		{
			get;
			internal set;
		}

		/// <summary>
		/// avarage scan per sec
		/// </summary>
		public double AvgScanPerSec
		{
			get;
			internal set;
		}

		/// <summary>
		/// get last scan Error datetime
		/// </summary>
		public DateTime LastScanErrorDT
		{
			get;
			internal set;
		}

		/// <summary>
		/// get last scan Error
		/// </summary>
		public string LastScanError
		{
			get;
			internal set;
		}

		/// <summary>
		/// get last putdata datetime
		/// </summary>
		public DateTime LastPutDataDT
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is bit device type
		/// </summary>
		public bool IsBitDeviceType
		{
			get;
			internal set;
		}

		/// <summary>
		/// new scan data snapshot
		/// </summary>
		public short[] NewSnapShot
		{
			get;
			internal set;
		}

		/// <summary>
		/// temp scan data snapshot
		/// </summary>
		public short[] TempSnapShot
		{
			get;
			internal set;
		}

		/// <summary>
		/// is data inited
		/// </summary>
		public bool IsDataInited
		{
			get;
			internal set;
		}

		/// <summary>
		/// is address is 16 base
		/// </summary>
		public bool IsAddressHex
		{
			get;
			internal set;
		}

		/// <summary>
		/// start address (10 base),fix 0
		/// </summary>
		public int StartAddress10
		{
			get;
			internal set;
		}

		/// <summary>
		/// get reference to scan buffer 
		/// </summary>
		public short[] ScanBuffer
		{
			get;
			internal set;
		}

		/// <summary>
		/// copy new scan data snapshot
		/// </summary>
		/// <param name="destArray">dest array</param>	
		/// <param name="reason">ng reason</param>
		/// <returns></returns>
		internal bool CopyNewSnapShot(short[] destArray, out string reason)
		{
			bool result;
			try
			{
				if (destArray == null)
				{
					throw new ArgumentException("destArray");
				}
				int wstart;
				int wpoints;
				if (this.IsBitDeviceType)
				{
					wstart = this.StartAddress10 / 16;
					wpoints = this.Points / 16;
				}
				else
				{
					wstart = this.StartAddress10;
					wpoints = this.Points;
				}
				if (destArray.Length < wstart + wpoints)
				{
					reason = string.Format("fail to copy NewSnapShot from watchdata={0} device={1} address=[2} point={3},destArray.Len={4} < (wstart+wpoints)=({5}+{6})", new object[]
					{
						this.Name,
						this.DeviceCode,
						this.Address,
						this.Points,
						destArray.Length,
						wstart,
						wpoints
					});
					result = false;
				}
				else
				{
					Buffer.BlockCopy(this.NewSnapShot, 0, destArray, wstart * 2, wpoints * 2);
					reason = string.Empty;
					result = true;
				}
			}
			catch (Exception ex)
			{
				reason = ex.ToString();
				result = false;
			}
			return result;
		}

		/// <summary>
		/// copy temp to new
		/// </summary>		
		/// <param name="reason">ng reason</param>
		/// <returns></returns>
		internal bool CopyTemp2New(out string reason)
		{
			bool result;
			try
			{
				Buffer.BlockCopy(this.TempSnapShot, 0, this.NewSnapShot, 0, this.TempSnapShot.Length * 2);
				reason = string.Empty;
				result = true;
			}
			catch (Exception ex)
			{
				reason = ex.ToString();
				result = false;
			}
			return result;
		}

		/// <summary>
		/// put new scan data
		/// </summary>
		/// <param name="newInput">new input scan data</param>
		/// <param name="reason">ng reason</param>
		/// <returns></returns>
		internal bool PutData(short[] newInput, out string reason)
		{
			bool result;
			try
			{
				if (newInput == null)
				{
					throw new ArgumentException("newInput");
				}
				if (newInput.Length != this.TempSnapShot.Length)
				{
					reason = string.Format("fail to put new data,newInput.Len={0} not match {1}", newInput.Length, this.TempSnapShot.Length);
					result = false;
				}
				else
				{
					Buffer.BlockCopy(newInput, 0, this.TempSnapShot, 0, this.TempSnapShot.Length * 2);
					int wstart;
					int wpoints;
					if (this.IsBitDeviceType)
					{
						wstart = this.StartAddress10 / 16;
						wpoints = this.Points / 16;
					}
					else
					{
						wstart = this.StartAddress10;
						wpoints = this.Points;
					}
					if (this.ScanBuffer.Length < wstart + wpoints)
					{
						reason = string.Format("fail to put data to scan buffers ,scanbuffer.Len={4} < (wstart+wpoints)=({5}+{6})", new object[]
						{
							this.Name,
							this.DeviceCode,
							this.Address,
							this.Points,
							this.ScanBuffer.Length,
							wstart,
							wpoints
						});
						result = false;
					}
					else
					{
						Buffer.BlockCopy(this.TempSnapShot, 0, this.ScanBuffer, wstart * 2, wpoints * 2);
						this.IsDataInited = true;
						reason = string.Empty;
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				reason = ex.ToString();
				result = false;
			}
			return result;
		}
	}
}
