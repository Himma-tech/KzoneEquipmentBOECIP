using System;
using System.Collections;
using System.Collections.Generic;

namespace KZONE.PLCAgent.PLC
{
	internal class MdFuncAdapter : AbstractAdapter
	{
		private short _Channel;

		private short _NetWorkNo;

		private short _StationNo;

		private int _Path;

		public override int Init(Hashtable paramTable)
		{
			this._Channel = (short)Utility.GetParamterInt("CHANNEL", paramTable, -1);
			this._NetWorkNo = (short)Utility.GetParamterInt("NETWORKNO", paramTable, -1);
			this._StationNo = (short)Utility.GetParamterInt("STATIONNO", paramTable, -1);
			base.DetachEnable = false;
			base.ParameterTable = paramTable;
			base.DeviceSymbolTable = new Dictionary<string, DeviceSymbol>
			{
				{
					"B",
					new DeviceSymbol
					{
						stype = "B",
						ntype = 23,
						nbase = 16,
						isbit = true,
						isownst = false
					}
				},
				{
					"X",
					new DeviceSymbol
					{
						stype = "X",
						ntype = 1,
						nbase = 16,
						isbit = true,
						isownst = false
					}
				},
				{
					"Y",
					new DeviceSymbol
					{
						stype = "Y",
						ntype = 2,
						nbase = 16,
						isbit = true,
						isownst = false
					}
				},
				{
					"L",
					new DeviceSymbol
					{
						stype = "L",
						ntype = 3,
						nbase = 10,
						isbit = true,
						isownst = false
					}
				},
				{
					"M",
					new DeviceSymbol
					{
						stype = "M",
						ntype = 4,
						nbase = 10,
						isbit = true,
						isownst = false
					}
				},
				{
					"SB",
					new DeviceSymbol
					{
						stype = "SB",
						ntype = 5,
						nbase = 16,
						isbit = true,
						isownst = true
					}
				},
				{
					"SM",
					new DeviceSymbol
					{
						stype = "SM",
						ntype = 5,
						nbase = 10,
						isbit = true,
						isownst = true
					}
				},
				{
					"W",
					new DeviceSymbol
					{
						stype = "W",
						ntype = 24,
						nbase = 16,
						isbit = false,
						isownst = false
					}
				},
				{
					"D",
					new DeviceSymbol
					{
						stype = "D",
						ntype = 13,
						nbase = 10,
						isbit = false,
						isownst = false
					}
				},
				{
					"R",
					new DeviceSymbol
					{
						stype = "R",
						ntype = 22,
						nbase = 10,
						isbit = false,
						isownst = false
					}
				},
				{
					"ZR",
					new DeviceSymbol
					{
						stype = "ZR",
						ntype = 220,
						nbase = 10,
						isbit = false,
						isownst = false
					}
				},
				{
					"Z",
					new DeviceSymbol
					{
						stype = "Z",
						ntype = 20,
						nbase = 10,
						isbit = false,
						isownst = false
					}
				},
				{
					"SW",
					new DeviceSymbol
					{
						stype = "SW",
						ntype = 14,
						nbase = 16,
						isbit = false,
						isownst = true
					}
				},
				{
					"SD",
					new DeviceSymbol
					{
						stype = "SD",
						ntype = 14,
						nbase = 16,
						isbit = false,
						isownst = true
					}
				},
				{
					"LB",
					new DeviceSymbol
					{
						stype = "B",
						ntype = 23,
						nbase = 16,
						isbit = true,
						isownst = true
					}
				},
				{
					"LX",
					new DeviceSymbol
					{
						stype = "X",
						ntype = 1,
						nbase = 16,
						isbit = true,
						isownst = true
					}
				},
				{
					"LY",
					new DeviceSymbol
					{
						stype = "Y",
						ntype = 2,
						nbase = 16,
						isbit = true,
						isownst = true
					}
				},
				{
					"LW",
					new DeviceSymbol
					{
						stype = "W",
						ntype = 24,
						nbase = 16,
						isbit = false,
						isownst = true
					}
				},
				{
					"RX",
					new DeviceSymbol
					{
						stype = "X",
						ntype = 1,
						nbase = 16,
						isbit = true,
						isownst = true
					}
				},
				{
					"RY",
					new DeviceSymbol
					{
						stype = "Y",
						ntype = 2,
						nbase = 16,
						isbit = true,
						isownst = true
					}
				},
				{
					"RW",
					new DeviceSymbol
					{
						stype = "RW",
						ntype = 24,
						nbase = 16,
						isbit = false,
						isownst = true
					}
				}
			};
			return 0;
		}

		public override int Connect()
		{
			return (int)MdFuncAPI.mdOpen(this._Channel, -1, ref this._Path);
		}

		public override int Disconnect()
		{
			return (int)MdFuncAPI.mdClose(this._Path);
		}

		public override int BlockRead(string devtype, string devno, int points, short[] buf, int offset)
		{
			DeviceSymbol device = base.GetDeviceSymbol(devtype);
			if (device == null)
			{
				throw new ArgumentException("unknown device type", devtype);
			}
			int dno = Convert.ToInt32(devno, device.nbase);
			int dtype = device.ntype;
			int bsize = points * 2;
			return MdFuncAPI.mdReceiveEx(this._Path, (int)this._NetWorkNo, (int)this._StationNo, dtype, dno, ref bsize, ref buf[offset]);
		}

		public override int BlockWrite(string devtype, string devno, int points, short[] buf, int offset)
		{
			DeviceSymbol device = base.GetDeviceSymbol(devtype);
			if (device == null)
			{
				throw new ArgumentException("unknown device type", devtype);
			}
			int dno = Convert.ToInt32(devno, device.nbase);
			int dtype = device.ntype;
			int bsize = points * 2;
			return MdFuncAPI.mdSendEx(this._Path, (int)this._NetWorkNo, (int)this._StationNo, dtype, dno, ref bsize, ref buf[offset]);
		}

		public override int RandomWrite(List<PLCRandOp.RandBlock> blocks)
		{
			int[] dblock = new int[blocks.Count * 3 + 1];
			dblock[0] = blocks.Count;
			int dsize = 0;
			foreach (PLCRandOp.RandBlock v in blocks)
			{
				//PLCRandOp.RandBlock v;
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				if (device.isbit)
				{
					dsize += (v.Points - 1) / 16 + 1;
				}
				else
				{
					dsize += v.Points;
				}
			}
			short[] dbuf = new short[dsize];
			int offset = 0;
			for (int i = 0; i < blocks.Count; i++)
			{
				PLCRandOp.RandBlock v = blocks[i];
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				dblock[i * 3 + 1] = device.ntype;
				dblock[i * 3 + 2] = Convert.ToInt32(v.DevNo, device.nbase);
				dblock[i * 3 + 3] = v.Points;
				if (device.isbit)
				{
					Buffer.BlockCopy(v.Buf, 0, dbuf, offset * 2, ((v.Points - 1) / 16 + 1) * 2);
					offset += (v.Points - 1) / 16 + 1;
				}
				else
				{
					Buffer.BlockCopy(v.Buf, 0, dbuf, offset * 2, v.Points * 2);
					offset += v.Points;
				}
			}
			return MdFuncAPI.mdRandWEx(this._Path, (int)this._NetWorkNo, (int)this._StationNo, ref dblock[0], ref dbuf[0], dsize * 2);
		}

		public override int RandomRead(List<PLCRandOp.RandBlock> blocks)
		{
			int[] dblock = new int[blocks.Count * 3 + 1];
			dblock[0] = blocks.Count;
			int dsize = 0;
			foreach (PLCRandOp.RandBlock v in blocks)
			{
				//PLCRandOp.RandBlock v;
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				if (device.isbit)
				{
					dsize += (v.Points - 1) / 16 + 1;
				}
				else
				{
					dsize += v.Points;
				}
			}
			short[] dbuf = new short[dsize];
			for (int i = 0; i < blocks.Count; i++)
			{
				PLCRandOp.RandBlock v = blocks[i];
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				dblock[i * 3 + 1] = device.ntype;
				dblock[i * 3 + 2] = Convert.ToInt32(v.DevNo, device.nbase);
				dblock[i * 3 + 3] = v.Points;
			}
			int ret = MdFuncAPI.mdRandREx(this._Path, (int)this._NetWorkNo, (int)this._StationNo, ref dblock[0], ref dbuf[0], dsize * 2);
			int srcOffset = 0;
			foreach (PLCRandOp.RandBlock v in blocks)
			{
				//PLCRandOp.RandBlock v;
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				if (device.isbit)
				{
					int cnt = ((v.Points - 1) / 16 + 1) * 2;
					Buffer.BlockCopy(dbuf, srcOffset, v.Buf, 0, cnt);
					srcOffset += cnt;
				}
				else
				{
					int cnt = v.Points * 2;
					Buffer.BlockCopy(dbuf, srcOffset, v.Buf, 0, cnt);
					srcOffset += cnt;
				}
			}
			return ret;
		}
	}
}
