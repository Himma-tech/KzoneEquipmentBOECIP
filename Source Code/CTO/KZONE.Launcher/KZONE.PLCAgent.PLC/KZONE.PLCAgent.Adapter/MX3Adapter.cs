
using ActSupportMsgLib;
using ActUtlTypeLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	internal class MX3Adapter : AbstractAdapter
	{
		private ActUtlTypeClass _Component;

		private ActMLSupportMsgClass _Support;

		public override int Init(Hashtable paramTable)
		{
			this._Component = new ActUtlTypeClass();
			this._Support = new ActMLSupportMsgClass();
			this._Component.ActLogicalStationNumber = Utility.GetParamterInt("LOGICALSTATIONNO", paramTable, -1);
			base.DetachEnable = true;
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
				}
			};
			return 0;
		}

		public override int Connect()
		{
			return this._Component.Open();
		}

		public override int Disconnect()
		{
			return this._Component.Close();
		}

		public override int BlockRead(string devtype, string devno, int points, short[] buf, int offset)
		{
			return this._Component.ReadDeviceBlock2(devtype + devno, points, out buf[offset]);
		}

		public override int BlockWrite(string devtype, string devno, int points, short[] buf, int offset)
		{
			return this._Component.WriteDeviceBlock2(devtype + devno, points, ref buf[offset]);
		}

		public override int RandomWrite(List<PLCRandOp.RandBlock> blocks)
		{
			int dsize = 0;
			foreach (PLCRandOp.RandBlock v in blocks)
			{
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				dsize += v.Points;
			}
			short[] dbuf = new short[dsize];
			int offset = 0;
			StringBuilder devlist = new StringBuilder();
			foreach (PLCRandOp.RandBlock v in blocks)
			{
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				int dno = Convert.ToInt32(v.DevNo, device.nbase);
				for (int i = 0; i < v.Points; i++)
				{
					devlist.Append(v.DevType);
					devlist.Append(Convert.ToString(dno, device.nbase));
					devlist.Append('\n');
					dno++;
				}
				if (device.isbit)
				{
					int pc = 0;
					for (int j = 0; j < v.Buf.Length; j++)
					{
						if (pc >= v.Points)
						{
							break;
						}
						for (int k = 0; k < 16; k++)
						{
							short f = (short)(1 << k);
                            dbuf[offset] = (short)((v.Buf[j] & f) / f);//BRUCE.ZHAN  20170403
							offset++;
							pc++;
							if (pc >= v.Points)
							{
								break;
							}
						}
					}
				}
				else
				{
					Buffer.BlockCopy(v.Buf, 0, dbuf, offset * 2, v.Points * 2);
					offset += v.Points;
				}
			}
			return this._Component.WriteDeviceRandom2(devlist.ToString(), dsize, ref dbuf[0]);
		}

		public override int RandomRead(List<PLCRandOp.RandBlock> blocks)
		{
			int dsize = 0;
			foreach (PLCRandOp.RandBlock v in blocks)
			{
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				dsize += v.Points;
			}
			short[] dbuf = new short[dsize];
			int offset = 0;
			StringBuilder devlist = new StringBuilder();
			foreach (PLCRandOp.RandBlock v in blocks)
			{
				DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
				int dno = Convert.ToInt32(v.DevNo, device.nbase);
				for (int i = 0; i < v.Points; i++)
				{
					devlist.Append(v.DevType);
					devlist.Append(Convert.ToString(dno, device.nbase));
					devlist.Append('\n');
					dno++;
				}
			}
			string[] str = new string[1];
			str[0] = devlist.ToString();
			short[] dbufTemp = new short[1];
			dbufTemp[0] = dbuf[0];

			int ret = this._Component.ReadDeviceRandom2(devlist.ToString(), dsize, out dbuf[0]);
			int result;
			if (ret != 0)
			{
				result = ret;
			}
			else
			{
				foreach (PLCRandOp.RandBlock v in blocks)
				{
					DeviceSymbol device = base.GetDeviceSymbol(v.DevType);
					if (device.isbit)
					{
						int pc = 0;
						for (int j = 0; j < v.Buf.Length; j++)
						{
							if (pc >= v.Points)
							{
								break;
							}
							for (int k = 0; k < 16; k++)
							{
								short f = (short)((dbuf[offset] & 1) << k);
								short[] expr_1AB_cp_0 = v.Buf;
								int expr_1AB_cp_1 = j;
								expr_1AB_cp_0[expr_1AB_cp_1] |= f;
								pc++;
								offset++;
								if (pc >= v.Points)
								{
									break;
								}
							}
						}
					}
					else
					{
						Buffer.BlockCopy(dbuf, offset * 2, v.Buf, 0, v.Points * 2);
						offset += v.Points;
					}
				}
				result = ret;
			}
			return result;
		}

		public override string GetErrorDesc(int code)
		{
			object msg;
			this._Support.GetErrorMessage(code, out msg);
			return msg.ToString();
		}
	}
}
