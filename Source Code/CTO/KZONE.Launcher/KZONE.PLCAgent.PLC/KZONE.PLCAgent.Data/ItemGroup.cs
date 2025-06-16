using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// itemgroup class
	/// </summary>
	[Serializable]
	public class ItemGroup
	{
		/// <summary>
		/// get itemgroup name(key)
		/// </summary>
		public string Name
		{
			get;
			internal set;
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
		/// default constructor
		/// </summary>
		public ItemGroup()
		{
			this.Items = new NameObjectCollection<Item>();
		}
	}
}
