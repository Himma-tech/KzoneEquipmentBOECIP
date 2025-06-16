using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// eventgroup class
	/// </summary>
   [Serializable]
	public class EventGroup
	{
		/// <summary>
		/// get eventgroup name(key)
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
		/// get event collection
		/// </summary>
		public NameObjectCollection<Event> Events
		{
			get;
			internal set;
		}

		/// <summary>
		/// get event object
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Event this[int index]
		{
			get
			{
				return this.Events[index];
			}
		}

		/// <summary>
		/// get event object
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Event this[string index]
		{
			get
			{
				return this.Events[index];
			}
		}

		/// <summary>
		/// get trigger event name collection
		/// </summary>
		public NameObjectCollection<string> TriggerEventNames
		{
			get;
			internal set;
		}

		/// <summary>
		/// get eventgroup metadata
		/// </summary>
		public EventGroupMetadata Metadata
		{
			get;
			internal set;
		}

		/// <summary>
		/// is eventgroup disable
		/// </summary>
		public bool IsDisable
		{
			get;
			set;
		}

		/// <summary>
		/// get/set is merge event option
		/// </summary>
		public bool IsMergeEvent
		{
			get;
			set;
		}

		/// <summary>
		/// get/set merge buffer
		/// </summary>
		public short[] MergeBuffer
		{
			get;
			set;
		}

		/// <summary>
		/// get/set Log Style defaule is Detial
		/// </summary>
		public LogStyleEnum LogStyle
		{
			get;
			set;
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public EventGroup()
		{
			this.Events = new NameObjectCollection<Event>();
			this.TriggerEventNames = new NameObjectCollection<string>();
			this.Metadata = new EventGroupMetadata();
			this.LogStyle = LogStyleEnum.DETAIL;
		}

		/// <summary>
		/// clone this eventgroup
		/// </summary>
		/// <returns>a copy of eventgroup object</returns>
		public object Clone()
		{
			EventGroup eg = (EventGroup)base.MemberwiseClone();
			eg.Events = new NameObjectCollection<Event>();
			object[] allValues = this.Events.AllValues;
			for (int i = 0; i < allValues.Length; i++)
			{
				Event v = (Event)allValues[i];
				Event evt = (Event)v.Clone();
				eg.Events.Add(evt.Name, evt);
			}
			eg.Events.LookupOnly = true;
			return eg;
		}
	}
}
