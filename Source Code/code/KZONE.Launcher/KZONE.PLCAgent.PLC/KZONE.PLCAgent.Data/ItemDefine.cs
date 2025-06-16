using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// itemdefine class
	/// </summary>
	public class ItemDefine
	{
		/// <summary>
		/// get itemdefine name(key)
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// get id collection
		/// </summary>
		public NameObjectCollection<Id> IDs
		{
			get;
			internal set;
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public ItemDefine()
		{
			this.IDs = new NameObjectCollection<Id>();
		}
	}
}
