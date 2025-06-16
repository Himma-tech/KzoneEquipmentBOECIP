using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using KZONE.Log;

namespace KZONE.DB
{
	public class BaseDao<T>
	{
		private HibernateAdapter _hibernateAdapter;

		private ILogManager _logger = NLogManager.Logger;

		public HibernateAdapter NHibernateAdapter
		{
			get
			{
				return this._hibernateAdapter;
			}
			set
			{
				this._hibernateAdapter = value;
			}
		}

		public BaseDao(HibernateAdapter adapter)
		{
			this._hibernateAdapter = adapter;
		}

		public bool Save(object[] objs)
		{
			bool result;
			try
			{
				result = this.NHibernateAdapter.SaveObjectAll(objs);
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
				result = false;
			}
			return result;
		}

		public bool Save(object obj)
		{
			bool result;
			try
			{
				result = this.NHibernateAdapter.SaveObject(obj);
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
				result = false;
			}
			return result;
		}

		public System.Collections.Generic.IList<T> Find(string name, object value)
		{
			try
			{
				System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list2 = this._hibernateAdapter.GetObject(typeof(T), name, value, DBCompareType.NONE, DBSortType.NONE);
					System.Collections.Generic.IList<T> result;
					if (list2 == null)
					{
						result = list;
						return result;
					}
					System.Collections.IEnumerator enumerator = list2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T local = (T)((object)enumerator.Current);
							list.Add(local);
						}
					}
					finally
					{
						System.IDisposable disposable = enumerator as System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					result = list;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return null;
		}

		public System.Collections.Generic.IList<T> Find(string[] namelist, object[] valuelist)
		{
			try
			{
				System.Collections.Generic.IList<T> list = new System.Collections.Generic.List<T>();
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list2 = this._hibernateAdapter.GetObject_AND(typeof(T), namelist, valuelist, null, null);
					System.Collections.Generic.IList<T> result;
					if (list2 == null)
					{
						result = list;
						return result;
					}
					System.Collections.IEnumerator enumerator = list2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T local = (T)((object)enumerator.Current);
							list.Add(local);
						}
					}
					finally
					{
						System.IDisposable disposable = enumerator as System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					result = list;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return null;
		}

		public System.Collections.Generic.IList<T> Find(string name, object[] valueList)
		{
			try
			{
				System.Collections.Generic.IList<T> list = new System.Collections.Generic.List<T>();
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list2 = this._hibernateAdapter.GetObject_OR(typeof(T), name, valueList, null, DBSortType.ASCENDING);
					System.Collections.Generic.IList<T> result;
					if (list2 == null)
					{
						result = list;
						return result;
					}
					System.Collections.IEnumerator enumerator = list2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T local = (T)((object)enumerator.Current);
							list.Add(local);
						}
					}
					finally
					{
						System.IDisposable disposable = enumerator as System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					result = list;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return null;
		}

		public System.Collections.Generic.IList<T> FindAll()
		{
			try
			{
				System.Collections.Generic.IList<T> list = new System.Collections.Generic.List<T>();
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list2 = this._hibernateAdapter.GetObject(typeof(T));
					System.Collections.Generic.IList<T> result;
					if (list2 == null)
					{
						result = list;
						return result;
					}
					System.Collections.IEnumerator enumerator = list2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T local = (T)((object)enumerator.Current);
							list.Add(local);
						}
					}
					finally
					{
						System.IDisposable disposable = enumerator as System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					result = list;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return null;
		}

		public System.Collections.Generic.IList<T> FindByPage(int start, int max)
		{
			try
			{
				System.Collections.Generic.IList<T> list = new System.Collections.Generic.List<T>();
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list2 = this._hibernateAdapter.GetObject(typeof(T), start, max);
					System.Collections.Generic.IList<T> result;
					if (list2 == null)
					{
						result = list;
						return result;
					}
					System.Collections.IEnumerator enumerator = list2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T local = (T)((object)enumerator.Current);
							list.Add(local);
						}
					}
					finally
					{
						System.IDisposable disposable = enumerator as System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					result = list;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return null;
		}

		public int GetCount()
		{
			int result;
			try
			{
				result = this.NHibernateAdapter.GetCount(typeof(T));
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
				result = 0;
			}
			return result;
		}

		public System.Collections.Generic.IList<T> FindByQuery(string hql)
		{
			System.Collections.Generic.IList<T> list = new System.Collections.Generic.List<T>();
			try
			{
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list2 = this._hibernateAdapter.GetObjectByQuery(hql);
					if (list2 != null)
					{
						System.Collections.IEnumerator enumerator = list2.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								T t_data = (T)((object)enumerator.Current);
								list.Add(t_data);
							}
						}
						finally
						{
							System.IDisposable disposable = enumerator as System.IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return list;
		}

		public T FindOne(string[] namelist, object[] valuelist)
		{
			try
			{
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list2 = this._hibernateAdapter.GetObject_AND(typeof(T), namelist, valuelist, null, null);
					if (list2 != null && list2.Count > 0)
					{
						return (T)((object)list2[0]);
					}
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return default(T);
		}

		public T FindOne(string name, object value)
		{
			try
			{
				if (this._hibernateAdapter != null)
				{
					System.Collections.IList list = this._hibernateAdapter.GetObject(typeof(T), name, value, DBCompareType.NONE, DBSortType.NONE);
					if (list != null && list.Count > 0)
					{
						return (T)((object)list[0]);
					}
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return default(T);
		}

		public bool Remove(object obj)
		{
			try
			{
				if (this._hibernateAdapter != null)
				{
					this._hibernateAdapter.DeleteObject(obj);
					return true;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return false;
		}

		public bool Remove(string name, object value)
		{
			try
			{
				if (this._hibernateAdapter != null)
				{
					this._hibernateAdapter.DeleteObject(typeof(T), name, value, DBCompareType.NONE);
					return true;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return false;
		}

		public bool Remove(string[] namelist, object[] valuelist)
		{
			try
			{
				if (this._hibernateAdapter != null)
				{
					this._hibernateAdapter.DeleteObject_AND(typeof(T), namelist, valuelist, null);
					return true;
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return false;
		}

		public bool SaveAll(object[] objs)
		{
			try
			{
				if (this._hibernateAdapter != null)
				{
					return this._hibernateAdapter.SaveObjectAll(objs);
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return false;
		}

		public bool Update(object obj)
		{
			try
			{
				if (this._hibernateAdapter != null)
				{
					return this._hibernateAdapter.UpdateObject(obj);
				}
			}
			catch (System.Exception ex)
			{
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			return false;
		}

		public int Update(long objectNo, System.Collections.SortedList AttributeMap)
		{
			return this.UpdateByAttributes(AttributeMap, "ObjectId", objectNo.ToString());
		}

		private int UpdateAllByAttributes(System.Collections.SortedList updateList, string conditionName)
		{
			int num = -1;
			ISession dBSession = this._hibernateAdapter.GetSession();
			ITransaction transaction = dBSession.BeginTransaction();
			try
			{
				for (int i = 0; i < updateList.Count; i++)
				{
					string key = (string)updateList.GetKey(i);
					System.Collections.SortedList attributeMap = (System.Collections.SortedList)updateList[key];
					string queryString = this.SetUpdateQueryByAttributes(attributeMap, conditionName, key);
					num = dBSession.CreateQuery(queryString).ExecuteUpdate();
				}
				transaction.Commit();
			}
			catch (System.Exception ex)
			{
				transaction.Rollback();
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			finally
			{
				if (dBSession != null)
				{
					dBSession.Close();
				}
			}
			return num;
		}

		private string SetUpdateQueryByAttributes(System.Collections.SortedList AttributeMap, string conditionName, string conditionValue)
		{
			string str = "";
			if (AttributeMap.Count <= 0)
			{
				return str;
			}
			str = "UPDATE " + typeof(T).ToString() + " SET ";
			for (int i = 0; i < AttributeMap.Count; i++)
			{
				string key = (string)AttributeMap.GetKey(i);
				str = ((i + 1 < AttributeMap.Count) ? string.Concat(new string[]
				{
					str,
					key,
					" = '",
					AttributeMap[key].ToString(),
					"', "
				}) : string.Concat(new string[]
				{
					str,
					key,
					" = '",
					AttributeMap[key].ToString(),
					"'"
				}));
			}
			string str2 = str;
			return string.Concat(new string[]
			{
				str2,
				" WHERE ",
				conditionName,
				" = '",
				conditionValue,
				"'"
			});
		}

		public int UpdateByAttributes(System.Collections.SortedList AttributeMap, string conditionName, string conditionValue)
		{
			int num = -1;
			ISession dBSession = this._hibernateAdapter.GetSession();
			ITransaction transaction = dBSession.BeginTransaction();
			try
			{
				if (AttributeMap.Count > 0)
				{
					string queryString = "UPDATE " + typeof(T).ToString() + " SET ";
					for (int i = 0; i < AttributeMap.Count; i++)
					{
						string key = (string)AttributeMap.GetKey(i);
						queryString = ((i + 1 < AttributeMap.Count) ? string.Concat(new string[]
						{
							queryString,
							key,
							" = '",
							AttributeMap[key].ToString(),
							"', "
						}) : string.Concat(new string[]
						{
							queryString,
							key,
							" = '",
							AttributeMap[key].ToString(),
							"'"
						}));
					}
					string str3 = queryString;
					queryString = string.Concat(new string[]
					{
						str3,
						" WHERE ",
						conditionName,
						" = '",
						conditionValue,
						"'"
					});
					num = dBSession.CreateQuery(queryString).ExecuteUpdate();
					transaction.Commit();
				}
			}
			catch (System.Exception ex)
			{
				transaction.Rollback();
				this._logger.LogErrorWrite("", base.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
			}
			finally
			{
				if (dBSession != null)
				{
					dBSession.Close();
				}
			}
			return num;
		}

		public int UpdateAll(System.Collections.SortedList updateList)
		{
			return this.UpdateAllByAttributes(updateList, "objectId");
		}

		public int UpdateByNameAll(System.Collections.SortedList updateList)
		{
			return this.UpdateAllByAttributes(updateList, "Name");
		}
	}
}
