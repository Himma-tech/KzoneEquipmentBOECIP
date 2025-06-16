using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// item class
	/// </summary>
	[Serializable]
	public class Item : ICloneable
	{
		/// <summary>
		/// get item name(key)
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
		/// get/set item value
		/// </summary>
		public string Value
		{
			get;
			set;
		}

		/// <summary>
		/// get item metadata
		/// </summary>
		public ItemMetadata Metadata
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

		/// <summary>
		/// default constructor
		/// </summary>
		public Item()
		{
			this.UserAttributes = new NameObjectCollection<string>();
			this.Metadata = new ItemMetadata();
		}

		/// <summary>
		/// clone this item
		/// </summary>
		/// <returns>a copy of item object</returns>
		public object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
