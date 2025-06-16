using System;
using System.Collections;
using System.Collections.Generic;

namespace KZONE.PLCAgent.PLC
{
	internal abstract class AbstractAdapter
	{
		/// <summary>
		/// get/set plc available device symbol table
		/// </summary>
		public Dictionary<string, DeviceSymbol> DeviceSymbolTable
		{
			get;
			set;
		}

		/// <summary>
		/// get/set is plc connected
		/// </summary>
		public bool IsConnected
		{
			get;
			set;
		}

		/// <summary>
		/// get/set is enable detach plc connection if operation error occured
		/// </summary>
		public bool DetachEnable
		{
			get;
			set;
		}

		/// <summary>
		/// get plc parameter table
		/// </summary>
		public Hashtable ParameterTable
		{
			get;
			set;
		}

		/// <summary>
		/// init plc with parameters
		/// </summary>
		/// <param name="paramTable">parameters</param>
		/// <returns>retcode(=0 OK else NG)</returns>
		public abstract int Init(Hashtable paramTable);

		/// <summary>
		/// connect plc,may throw exception
		/// </summary>
		/// <returns>retcode(=0 OK else NG)</returns>
		public abstract int Connect();

		/// <summary>
		/// disconnect plc,may throw exception
		/// </summary>
		/// <returns>retcode(=0 OK else NG)</returns>
		public abstract int Disconnect();

		/// <summary>
		/// block read data from plc,may throw exception
		/// *NOTE: when bit device type assigned,data are read from specific devno/points on a 16 point basis , ex.read B0 2 points=2 words buf[0]=BF~B0,buf[1]=B1F~B10)
		/// </summary>
		/// <param name="devtype">read device type</param>
		/// <param name="devno">read start device no</param>
		/// <param name="points">read points</param>
		/// <param name="buf">data buffer</param>
		/// <param name="offset">data buffer start offset</param>
		/// <returns>retcode(=0 OK else NG)</returns>
		public abstract int BlockRead(string devtype, string devno, int points, short[] buf, int offset);

		/// <summary>
		/// block write data to plc,may throw exception
		/// *NOTE: when bit device type assigned,data are written to specific devno/points on a 16 point basis , ex.write B0 32 points=2 words ,buf[0]=BF~B0,buf[1]=B1F~B10)
		/// </summary>
		/// <param name="devtype">write device type</param>
		/// <param name="devno">write start device no</param>
		/// <param name="points">write points</param> 
		/// <param name="buf">data buffer</param>	
		/// <param name="offset">data buffer start offset</param>
		/// <returns>retcode(=0 OK else NG)</returns>
		public abstract int BlockWrite(string devtype, string devno, int points, short[] buf, int offset);

		/// <summary>
		/// random read data from plc,may throw exception		
		/// </summary>
		/// <param name="blocks">random read block list</param>		
		/// <returns>retcode(=0 OK else NG)</returns>
		public abstract int RandomRead(List<PLCRandOp.RandBlock> blocks);

		/// <summary>
		/// random write data to plc,may throw exception		
		/// </summary>
		/// <param name="blocks">random write block list</param>		
		/// <returns>retcode(=0 OK else NG)</returns>
		public abstract int RandomWrite(List<PLCRandOp.RandBlock> blocks);

		/// <summary>
		/// get plc device symbol config
		/// </summary>
		/// <param name="devtype">device type key</param>
		/// <returns>device object</returns>
		public DeviceSymbol GetDeviceSymbol(string devtype)
		{
			DeviceSymbol result;
			if (this.DeviceSymbolTable.ContainsKey(devtype))
			{
				result = this.DeviceSymbolTable[devtype];
			}
			else
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// get plc operation error description
		/// </summary>
		public virtual string GetErrorDesc(int code)
		{
			return string.Empty;
		}
	}
}
