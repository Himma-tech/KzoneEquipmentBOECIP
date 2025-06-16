using System;
using System.Collections.Generic;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plcscanbuffer define whole continuous big block,and watchdata is like segment,PLCScanBuffer can contain multiple segement
	/// </summary>
	public class PLCScanBuffer
	{
		//public static int MAX_BIT_POINTS = 32768;
        public static int MAX_BIT_POINTS = 40000;
        //public static int MAX_WORD_POINTS = 131072;
		public static int MAX_WORD_POINTS = 350000;

		/// <summary>
		/// get/set logical station no (1-1023)
		/// </summary>
		public int LogicalStationNo
		{
			get;
			set;
		}

		/// <summary>
		/// get/set device type
		/// </summary>
		public string DeviceCode
		{
			get;
			set;
		}

		/// <summary>
		/// get/set total points,fix Bit device=MAX_BIT_POINTS,Word device=MAX_WORD_POINTS
		/// </summary>
		public int Points
		{
			get;
			set;
		}

		/// <summary>
		/// start/set address (10 base),fix=0
		/// </summary>
		public int StartAddress10
		{
			get;
			set;
		}

		/// <summary>
		/// get/set is bit device type
		/// </summary>
		public bool IsBitDeviceType
		{
			get;
			internal set;
		}

		/// <summary>
		/// get/set new scan data snapshot
		/// </summary>
		public short[] NewSnapShot
		{
			get;
			set;
		}

		/// <summary>
		/// get/set old scan data snapshot
		/// </summary>
		public short[] OldSnapShot
		{
			get;
			set;
		}

		/// <summary>
		/// get/set scan buffer
		/// </summary>
		public short[] ScanBuffer
		{
			get;
			set;
		}

		/// <summary>
		/// get reference to watchdata object,watchdata is like segment,PLCScanBuffer can contain multiple segement
		/// </summary>
		public List<WatchData> Source
		{
			get;
			set;
		}

		/// <summary>
		/// get last refresh datetime
		/// </summary>
		public DateTime LastRefreshDT
		{
			get;
			set;
		}

		/// <summary>
		/// get last refresh Error datetime
		/// </summary>
		public DateTime LastRefreshErrorDT
		{
			get;
			set;
		}

		/// <summary>
		/// get last refresh Error
		/// </summary>
		public string LastRefreshError
		{
			get;
			set;
		}

		/// <summary>
		/// refresh snapshot data
		/// </summary>				
		public void Refresh()
		{
			try
			{
				this.LastRefreshDT = DateTime.Now;
				Buffer.BlockCopy(this.NewSnapShot, 0, this.OldSnapShot, 0, this.NewSnapShot.Length * 2);
				foreach (WatchData watch in this.Source)
				{
					string reason;
					if (!watch.CopyNewSnapShot(this.NewSnapShot, out reason))
					{
						this.LastRefreshErrorDT = this.LastRefreshDT;
						this.LastRefreshError = reason;
					}
				}
			}
			catch (Exception ex)
			{
				this.LastRefreshErrorDT = this.LastRefreshDT;
				this.LastRefreshError = ex.Message;
			}
		}

		/// <summary>
		/// new shapshot hex dump
		/// </summary>
		/// <returns>hex string</returns>
		public string NewSnapShotHexDump()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("({0}_{1}_{2}_{3}),HexRawData=", new object[]
			{
				this.LogicalStationNo,
				this.DeviceCode,
				this.StartAddress10,
				this.Points
			}));
			string result;
			if (this.NewSnapShot == null)
			{
				result = sb.ToString();
			}
			else
			{
				short[] newSnapShot = this.NewSnapShot;
				for (int i = 0; i < newSnapShot.Length; i++)
				{
					short v = newSnapShot[i];
					sb.Append(v.ToString("X4") + " ");
				}
				result = sb.ToString();
			}
			return result;
		}
	}
}
