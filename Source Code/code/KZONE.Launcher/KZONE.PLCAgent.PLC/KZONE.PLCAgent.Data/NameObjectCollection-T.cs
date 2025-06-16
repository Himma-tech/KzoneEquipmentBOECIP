using System;
using System.Collections.Specialized;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// string keyed object collection
	/// </summary>
	/// <typeparam name="T">object type</typeparam>
	[Serializable]
	public sealed class NameObjectCollection<T> : NameObjectCollectionBase
	{
		/// <summary>
		/// get all keys in collection
		/// </summary>
		public string[] AllKeys
		{
			get
			{
				return base.BaseGetAllKeys();
			}
		}

		/// <summary>
		/// get all objects in collection
		/// </summary>
		public object[] AllValues
		{
			get
			{
				return base.BaseGetAllValues();
			}
		}

		/// <summary>
		/// get T object by numeric index
		/// </summary>
		/// <param name="index">numeric index</param>
		/// <returns>T object</returns>
		public T this[int index]
		{
			get
			{
				return (T)((object)base.BaseGet(index));
			}
			set
			{
				base.BaseSet(index, value);
			}
		}

		/// <summary>
		/// get T object by string key
		/// </summary>
		/// <param name="name">string key</param>
		/// <returns>T object</returns>
		public T this[string name]
		{
			get
			{
				return (T)((object)base.BaseGet(name));
			}
			set
			{
				base.BaseSet(name, value);
			}
		}

		/// <summary>
		/// let collection read only
		/// </summary>
		internal bool LookupOnly
		{
			set
			{
				base.IsReadOnly = value;
			}
		}

		/// <summary>
		/// copy to dest array
		/// </summary>
		/// <param name="dest">destination array object</param>
		/// <param name="index">start index</param>
		public void CopyTo(Array dest, int index)
		{
			Array.Copy(base.BaseGetAllValues(), dest, index);
		}

		/// <summary>
		/// get T object by string key 
		/// Add throw KeyNotFoundException Notify User Key Name; 20150303 Tom
		/// </summary>
		/// <param name="name">string key</param>
		/// <returns>T object</returns>
		public T Get(string name)
		{
			object obj = base.BaseGet(name);
			return (T)((object)obj);
		}

		/// <summary>
		/// get T object by numeric index
		/// Add Throw IndexOutOfRangeExecption Notity User Index;  20150303 Tom
		/// </summary>
		/// <param name="index">numeric index</param>
		/// <returns>T object</returns>
		public T Get(int index)
		{
			return (T)((object)base.BaseGet(index));
		}

		/// <summary>
		/// get string key by numeric index
		/// </summary>
		/// <param name="index">numeric index</param>
		/// <returns>string key</returns>
		public string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		/// <summary>
		/// add T object to collection
		/// </summary>
		/// <param name="key">string key</param>
		/// <param name="value">T object</param>
		internal void Add(string key, T value)
		{
			base.BaseAdd(key, value);
		}
	}
}
