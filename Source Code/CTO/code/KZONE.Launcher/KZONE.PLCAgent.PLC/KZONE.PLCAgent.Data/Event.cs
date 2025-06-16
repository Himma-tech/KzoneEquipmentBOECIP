using System;
using System.Text;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// event class
	/// </summary>
	[Serializable]
	public class Event : ICloneable
	{
		/// <summary>
		/// get event name(key)
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
		/// get item collection
		/// </summary>
		public NameObjectCollection<Item> Items
		{
			get;
			internal set;
		}

		/// <summary>
		/// get item object
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Item this[int index]
		{
			get
			{
				return this.Items[index];
			}
		}

		/// <summary>
		/// get item object
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Item this[string index]
		{
			get
			{
				return this.Items[index];
			}
		}

		/// <summary>
		/// get event metadata
		/// </summary>
		public EventMetadata Metadata
		{
			get;
			internal set;
		}

		/// <summary>
		/// raw data buffer	
		/// <br>ex1:</br>
		/// <br>DeviceCode="B",Address="10",Point="1" (B10)</br>
		/// <br>RawData[0]=0x1=0000 0000 0000 0001 (offset0=B10=1)</br>
		/// <br>ex2:</br>
		/// <br>DeviceCode="B",Address="10",Point="5" (B10~B14)</br>
		/// <br>RawData[0]=0x1D=0000 0000 0001 1101 (offset0=B10=1,offset1=B11=0,offset2=B12=1,offset3=B13=1,offset4=B14=1)</br>
		/// <br>ex3:</br>
		/// <br>DeviceCode="B",Address="10",Point="17" (B10~B20)</br>
		/// <br>RawData[0]=0x1D=0000 0000 0001 1101 (offset0=B10=1,offset1=B11=0,offset2=B12=1,offset3=B13=1,offset4=B14=1,offset5=B15=0 .....offset15=B1F=0)</br>
		/// <br>RawData[1]=0x01=0000 0000 0000 0001 (offset0=B20=1)</br>
		/// <br>ex4:</br>
		/// <br>DeviceCode="W",Address="10",Point="5"  (W10~W14)</br>
		/// <br>RawData[0]=W10,RawData[1]=W11,RawData[2]=W12,RawData[3]=W13,RawData[4]=W14</br>
		/// </summary>
		public short[] RawData
		{
			get;
			set;
		}

		/// <summary>
		/// is event disable
		/// </summary>
		public bool IsDisable
		{
			get;
			set;
		}

		/// <summary>
		/// write operation delay time(ms),0=no delay
		/// </summary>
		public int OpDelayTimeMS
		{
			get;
			set;
		}

		/// <summary>
		/// write event raw data instead encode items,RawData must set first
		/// </summary>
		public bool WriteRaw
		{
			get;
			set;
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public Event()
		{
			this.Items = new NameObjectCollection<Item>();
			this.Metadata = new EventMetadata();
		}

		/// <summary>
		/// clone this event
		/// </summary>
		/// <returns>a copy of event object</returns>
		public object Clone()
		{
			Event evt = (Event)base.MemberwiseClone();
			evt.Items = new NameObjectCollection<Item>();
			object[] allValues = this.Items.AllValues;
			for (int i = 0; i < allValues.Length; i++)
			{
				Item v = (Item)allValues[i];
				Item item = (Item)v.Clone();
				evt.Items.Add(item.Name, item);
			}
			evt.Items.LookupOnly = true;
			return evt;
		}

		/// <summary>
		/// get hex string
		/// </summary>
		/// <returns>hex string</returns>
		public string GetHexRawData()
		{
			StringBuilder sb = new StringBuilder();
			string result;
			if (this.RawData == null)
			{
				result = sb.ToString();
			}
			else
			{
				short[] rawData = this.RawData;
				for (int i = 0; i < rawData.Length; i++)
				{
					short v = rawData[i];
					sb.Append(v.ToString("X4") + " ");
				}
				result = sb.ToString();
			}
			return result;
		}
	}
}
