using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
    /// <summary>
    /// trx class
    /// </summary>
    [Serializable]
    public class Trx : ICloneable
	{
		/// <summary>
		/// get trx name(key),must have
		/// </summary>
		public string Name
		{
			get
			{
				return this.Metadata.Name;
			}
			internal set
			{
				this.Metadata.Name = value;
			}
		}

		/// <summary>
		/// get eventgroup collection
		/// </summary>
		public NameObjectCollection<EventGroup> EventGroups
		{
			get;
			internal set;
		}

		/// <summary>
		/// get EventGroup 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public EventGroup this[int index]
		{
			get
			{
				return this.EventGroups[index];
			}
		}

		/// <summary>
		/// get eventgroup
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public EventGroup this[string index]
		{
			get
			{
				return this.EventGroups[index];
			}
		}

		/// <summary>
		/// get trx metadata
		/// </summary>
		public TrxMetadata Metadata
		{
			get;
			internal set;
		}

		/// <summary>
		/// get request sno
		/// </summary>
		public int ReqSNo
		{
			get;
			internal set;
		}

		/// <summary>
		/// get/set tracking key
		/// </summary>
		public string TrackKey
		{
			get;
			set;
		}

		/// <summary>
		/// get/set trx flags,must have
		/// </summary>
		public InternalFlagsEnum TrxFlags
		{
			get;
			set;
		}

		/// <summary>
		/// get/set user object
		/// </summary>
		public object Tag
		{
			get;
			set;
		}

		/// <summary>
		/// get dictionary for write complete stations with PLCOpReqResult
		/// </summary>
		public ConcurrentDictionary<int, object> WriteCompleteStations
		{
			get;
			internal set;
		}

		/// <summary>
		/// get dictionary for read complete stations with PLCOpReqResult
		/// </summary>
		public ConcurrentDictionary<int, object> ReadCompleteStations
		{
			get;
			internal set;
		}

		/// <summary>
		/// get is init trigger
		/// </summary>
		public bool IsInitTrigger
		{
			get;
			internal set;
		}

		/// <summary>
		/// get user define attribute collection
		/// </summary>
		public NameObjectCollection<string> UserAttributes
		{
			get;
			internal set;
		}

		internal ConcurrentDictionary<int, object> WriteRequestStations
		{
			get;
			set;
		}

		internal object WriteCompleteSync
		{
			get;
			set;
		}

		internal ConcurrentDictionary<int, object> ReadRequestStations
		{
			get;
			set;
		}

		internal object ReadCompleteSync
		{
			get;
			set;
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public Trx()
		{
			this.EventGroups = new NameObjectCollection<EventGroup>();
			this.UserAttributes = new NameObjectCollection<string>();
			this.Metadata = new TrxMetadata();
		}

		/// <summary>
		/// clone this trx
		/// </summary>
		/// <returns>a copy of trx object</returns>
		public object Clone()
		{
			Trx trx = (Trx)base.MemberwiseClone();
			trx.EventGroups = new NameObjectCollection<EventGroup>();
			object[] allValues = this.EventGroups.AllValues;
			for (int i = 0; i < allValues.Length; i++)
			{
				EventGroup v = (EventGroup)allValues[i];
				EventGroup evt = (EventGroup)v.Clone();
				trx.EventGroups.Add(evt.Name, evt);
			}
			trx.EventGroups.LookupOnly = true;
			return trx;
		}

		/// <summary>
		/// clear trx items with 0 values
		/// </summary>
		public void ClearTrxWith0()
		{
			object[] allValues = this.EventGroups.AllValues;
			for (int i = 0; i < allValues.Length; i++)
			{
				EventGroup eg = (EventGroup)allValues[i];
				object[] allValues2 = eg.Events.AllValues;
				for (int j = 0; j < allValues2.Length; j++)
				{
					Event evt = (Event)allValues2[j];
					object[] allValues3 = evt.Items.AllValues;
					int k = 0;
					while (k < allValues3.Length)
					{
						Item item = (Item)allValues3[k];
						ItemExpressionEnum expression = item.Metadata.Expression;
						if (expression <= ItemExpressionEnum.HEX)
						{
							if (expression != ItemExpressionEnum.ASCII)
							{
								if (expression != ItemExpressionEnum.HEX)
								{
									goto IL_135;
								}
								item.Value = new string('0', item.Metadata.BitPoints / 4);
							}
							else
							{
								item.Value = new string('\0', item.Metadata.BitPoints / 8);
							}
						}
						else if (expression != ItemExpressionEnum.BIT)
						{
							if (expression != ItemExpressionEnum.BIN)
							{
								if (expression != ItemExpressionEnum.BCD)
								{
									goto IL_135;
								}
								item.Value = new string('0', item.Metadata.BitPoints / 4);
							}
							else
							{
								item.Value = new string('0', item.Metadata.BitPoints);
							}
						}
						else
						{
							item.Value = new string('0', item.Metadata.BitPoints);
						}
						IL_143:
						k++;
						continue;
						IL_135:
						item.Value = "0";
						goto IL_143;
					}
				}
			}
		}
		/// <summary>
		/// ASCII Items Set Empty,Others Set 0
		/// </summary>
        public void ClearTrx()
        {
            object[] allValues = this.EventGroups.AllValues;
            for (int i = 0; i < allValues.Length; i++)
            {
                EventGroup eg = (EventGroup)allValues[i];
                object[] allValues2 = eg.Events.AllValues;
                for (int j = 0; j < allValues2.Length; j++)
                {
                    Event evt = (Event)allValues2[j];
                    object[] allValues3 = evt.Items.AllValues;
                    int k = 0;
                    while (k < allValues3.Length)
                    {
                        Item item = (Item)allValues3[k];
                        ItemExpressionEnum expression = item.Metadata.Expression;
                        if (expression <= ItemExpressionEnum.HEX)
                        {
                            if (expression != ItemExpressionEnum.ASCII)
                            {
                                if (expression != ItemExpressionEnum.HEX)
                                {
                                    goto IL_135;
                                }
                                item.Value = new string('0', item.Metadata.BitPoints / 4);
                            }
                            else
                            {
                                item.Value = new string(' ', item.Metadata.BitPoints / 8);
                            }
                        }
                        else if (expression != ItemExpressionEnum.BIT)
                        {
                            if (expression != ItemExpressionEnum.BIN)
                            {
                                if (expression != ItemExpressionEnum.BCD)
                                {
                                    goto IL_135;
                                }
                                item.Value = new string('0', item.Metadata.BitPoints / 4);
                            }
                            else
                            {
                                item.Value = new string('0', item.Metadata.BitPoints);
                            }
                        }
                        else
                        {
                            item.Value = new string('0', item.Metadata.BitPoints);
                        }
                    IL_143:
                        k++;
                        continue;
                    IL_135:
                        item.Value = "0";
                        goto IL_143;
                    }
                }
            }
        }

        /// <summary>
        /// to log string
        /// </summary>
        /// <returns>log string</returns>
        public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			bool readop = false;
			string header = string.Empty;
			if ((ushort)(this.TrxFlags & InternalFlagsEnum.IsTrxTrigger) == 8)
			{
				readop = true;
				header = string.Format("[Receive]-[{0}]-[{1}]-[{2}]-[Trigger],condition={3},IsInitTrigger={4}", new object[]
				{
					this.Name,
					this.TrackKey,
					this.ReqSNo,
					this.Metadata.TriggerCondition,
					this.IsInitTrigger
				});
			}
			else if ((ushort)(this.TrxFlags & InternalFlagsEnum.IsTrxRead) == 1)
			{
				readop = true;
				header = string.Format("[Receive]-[{0}]-[{1}]-[{2}]-[Read]", this.Name, this.TrackKey, this.ReqSNo);
			}
			else if ((ushort)(this.TrxFlags & InternalFlagsEnum.IsTrxWrite) == 2)
			{
				header = string.Format("[Send]-[{0}]-[{1}]-[{2}]-[Write]", this.Name, this.TrackKey, this.ReqSNo);
			}
			else if ((ushort)(this.TrxFlags & InternalFlagsEnum.IsTrxRawWrite) == 4)
			{
				header = string.Format("[Send]-[{0}]-[{1}]-[{2}]-[WriteRaw]", this.Name, this.TrackKey, this.ReqSNo);
			}
			else if ((ushort)(this.TrxFlags & InternalFlagsEnum.IsTrxRandWrite) == 128)
			{
				header = string.Format("[Send]-[{0}]-[{1}]-[{2}]-[RandWrite]", this.Name, this.TrackKey, this.ReqSNo);
			}
			sb.AppendLine(header);
			object[] allValues = this.EventGroups.AllValues;
			for (int i = 0; i < allValues.Length; i++)
			{
				EventGroup eg = (EventGroup)allValues[i];
				sb.AppendLine(string.Format("<{0} disable={1},mergeEvent={2}", eg.Name, eg.IsDisable, eg.IsMergeEvent));
				if (eg.IsDisable)
				{
					sb.AppendLine(">");
				}
				else if (eg.LogStyle == LogStyleEnum.HEAD)
				{
					sb.AppendLine(">");
				}
				else
				{
					object[] allValues2 = eg.Events.AllValues;
					for (int j = 0; j < allValues2.Length; j++)
					{
						Event evt = (Event)allValues2[j];
						bool triggerevt = false;
						if (eg.TriggerEventNames.AllKeys.Contains(evt.Name))
						{
							triggerevt = true;
						}
						sb.Append(string.Format("    <{0} ({1},{2}{3},{4},{5}) {6} trigger={7},disable={8},skipDecode={9},opDelayTimeMS={10}", new object[]
						{
							evt.Name,
							evt.Metadata.DeviceCode,
							evt.Metadata.IsAddressHex ? "0x" : "",
							evt.Metadata.Address,
							evt.Metadata.Points,
							evt.Metadata.LogicalStationNo,
							evt.Metadata.IsApplyTransform ? string.Format("Original=({0},{1}{2})", evt.Metadata.OriginalDeviceCode, evt.Metadata.IsOriginalAddressHex ? "0x" : "", evt.Metadata.OriginalAddress) : "",
							triggerevt,
							evt.IsDisable,
							evt.Metadata.SkipDecode,
							evt.OpDelayTimeMS
						}));
						if (readop && evt.Metadata.IsBitDeviceType)
						{
							sb.AppendLine();
						}
						else
						{
							sb.AppendLine(string.Format(",HexRawData={0}", evt.GetHexRawData()));
						}
						if (evt.Metadata.SkipDecode)
						{
							sb.AppendLine("    >");
						}
						else if (evt.IsDisable)
						{
							sb.AppendLine("    >");
						}
						else if (eg.LogStyle == LogStyleEnum.RAWDATA)
						{
							sb.AppendLine("    >");
						}
						else
						{
							object[] allValues3 = evt.Items.AllValues;
							for (int k = 0; k < allValues3.Length; k++)
							{
								Item item = (Item)allValues3[k];
								sb.AppendLine(string.Format("        <{0} {1}:{2} {3}:{4} {5} '{6}'>", new object[]
								{
									item.Metadata.Expression.ToString().PadRight(5, ' '),
									item.Metadata.WordOffset,
									item.Metadata.BitOffset,
									item.Metadata.WordPoints,
									item.Metadata.BitPoints,
									item.Name,
									item.Value
								}));
							}
							sb.AppendLine("    >");
						}
					}
					sb.AppendLine(">");
				}
			}
			return sb.ToString();
		}
	}
}
