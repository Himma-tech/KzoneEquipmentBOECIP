using System;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc data model class
	/// </summary>
	public class PLCDataModel
	{
		/// <summary>
		/// get itemgroup collection
		/// </summary>
		public NameObjectCollection<ItemGroup> ItemGroupCollection
		{
			get;
			internal set;
		}

		/// <summary>
		/// get event collection
		/// </summary>
		public NameObjectCollection<Event> EventMap
		{
			get;
			internal set;
		}

		/// <summary>
		/// get trx collection
		/// </summary>
		public NameObjectCollection<Trx> Transaction
		{
			get;
			internal set;
		}

		/// <summary>
		/// get watchdata collection
		/// </summary>
		public NameObjectCollection<WatchData> Scan
		{
			get;
			internal set;
		}

		/// <summary>
		/// get trxcatalog collection
		/// </summary>
		public NameObjectCollection<TrxCatalog> Pair
		{
			get;
			internal set;
		}

		/// <summary>
		/// get itemdefine collection
		/// </summary>
		public NameObjectCollection<ItemDefine> ItemDefineCollection
		{
			get;
			internal set;
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public PLCDataModel()
		{
			this.ItemGroupCollection = new NameObjectCollection<ItemGroup>();
			this.EventMap = new NameObjectCollection<Event>();
			this.Transaction = new NameObjectCollection<Trx>();
			this.Scan = new NameObjectCollection<WatchData>();
			this.Pair = new NameObjectCollection<TrxCatalog>();
			this.ItemDefineCollection = new NameObjectCollection<ItemDefine>();
		}
	}
}
