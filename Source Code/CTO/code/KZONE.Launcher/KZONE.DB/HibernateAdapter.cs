using NHibernate;
using NHibernate.Engine;
using NHibernate.Transform;
using Spring.Context;
using Spring.Data.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using KZONE.Log;

namespace KZONE.DB
{
	public class HibernateAdapter : IApplicationContextAware
	{
		private ILogManager _logger = NLogManager.Logger;

		private bool _alwaysNewSession;

		private IApplicationContext _applicationContext;

		private HibernateTemplate _hibernateTemplate;

		private ISessionFactoryImplementor _sessionFactory;

		private HibernateTransactionManager _transactionManager;

		private string _connectionString;

		private string _dataBaseName;

		private string _sourceName;

		private string _currentDataBaseName;

		private string _userID;

		private string _password;

		private int _connectionTimeout;

		private string _trustedConnection;

		private bool _usePooling;

		private int _maxPoolSize;

		private int _minPoolSize;

		private System.Threading.ReaderWriterLock dbLock = new System.Threading.ReaderWriterLock();

		public ILogManager Logger
		{
			get
			{
				return this._logger;
			}
		}

		public bool AlwaysNewSession
		{
			get
			{
				return this._alwaysNewSession;
			}
			set
			{
				this._alwaysNewSession = value;
			}
		}

		public IApplicationContext ApplicationContext
		{
			set
			{
				this._applicationContext = value;
			}
		}

		public ISessionFactoryImplementor SessionFactory
		{
			get
			{
				return this._sessionFactory;
			}
			set
			{
				this._sessionFactory = value;
			}
		}

		public HibernateTransactionManager TransactionManager
		{
			get
			{
				return this._transactionManager;
			}
			set
			{
				this._transactionManager = value;
			}
		}

		public string ConnectionString
		{
			get
			{
				return this._connectionString;
			}
			set
			{
				this._connectionString = value;
			}
		}

		public string DataBaseName
		{
			get
			{
				return this._dataBaseName;
			}
			set
			{
				this._dataBaseName = value;
			}
		}

		public string SourceName
		{
			get
			{
				return this._sourceName;
			}
			set
			{
				this._sourceName = value;
			}
		}

		public string CurrentDataBaseName
		{
			get
			{
				return this._currentDataBaseName;
			}
			set
			{
				this._currentDataBaseName = value;
			}
		}

		public string UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				this._userID = value;
			}
		}

		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}

		public int ConnectionTimeout
		{
			get
			{
				return this._connectionTimeout;
			}
			set
			{
				this._connectionTimeout = value;
			}
		}

		public string TrustedConnection
		{
			get
			{
				return this._trustedConnection;
			}
			set
			{
				this._trustedConnection = value;
			}
		}

		public bool UsePool
		{
			get
			{
				return this._usePooling;
			}
			set
			{
				this._usePooling = value;
			}
		}

		public int MaxPoolSize
		{
			get
			{
				return this._maxPoolSize;
			}
			set
			{
				this._maxPoolSize = value;
			}
		}

		public int MinPoolSize
		{
			get
			{
				return this._minPoolSize;
			}
			set
			{
				this._minPoolSize = value;
			}
		}

		public void CheckHiberNateTemplate()
		{
			if (this._hibernateTemplate == null)
			{
				this._hibernateTemplate = new HibernateTemplate(this._sessionFactory);
				this._hibernateTemplate.AlwaysUseNewSession = this.GetAlwaysNewSession();
			}
		}

		public bool GetAlwaysNewSession()
		{
			return this.AlwaysNewSession;
		}

		private void AcquireReaderLock()
		{
			this.dbLock.AcquireReaderLock(10);
		}

		private void AcquireWriterLock()
		{
			this.dbLock.AcquireWriterLock(10);
		}

		private HibernateTemplate GetHibernateTemplate()
		{
			return new HibernateTemplate(this._sessionFactory);
		}

		public void Init()
		{
			this.InitSession();
		}

		private void InitSession()
		{
			try
			{
				if (this._sessionFactory != null)
				{
					ISession session = this._sessionFactory.OpenSession();
					this._hibernateTemplate = this.GetHibernateTemplate();
					this._hibernateTemplate.AlwaysUseNewSession = this.GetAlwaysNewSession();
					this.SetConnectionString(session.Connection.ConnectionString);
					this.Logger.LogDebugWrite("", "HibernateAdapter", "Init()", "Session is open.");
				}
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", "Session isn't open.", exception);
			}
		}

		public ISession GetSession()
		{
			return this._sessionFactory.OpenSession();
		}

		private void SetConnectionString(string sConnectionstring)
		{
			try
			{
				string[] strArray = sConnectionstring.Split(new char[]
				{
					';'
				});
				for (int i = 0; i < strArray.Length; i++)
				{
					string[] strArray2 = strArray[i].ToString().Split(new char[]
					{
						'='
					});
					string key;
					switch (key = strArray2[0].Trim().ToString().ToLower())
					{
					case "data source":
						this._sourceName = strArray2[1].ToString();
						break;
					case "database":
						if (this._dataBaseName == "")
						{
							this._dataBaseName = strArray2[1].ToString();
						}
						this.CurrentDataBaseName = strArray2[1].ToString();
						break;
					case "user id":
						this._userID = strArray2[1].ToString();
						break;
					case "password":
						this._password = strArray2[1].ToString();
						break;
					case "connect timeout":
						this._connectionTimeout = int.Parse(strArray2[1].ToString());
						break;
					case "trusted_connection":
						this._trustedConnection = strArray2[1].ToString();
						break;
					case "pooling":
						if (strArray2[1].ToString().ToLower() == "true")
						{
							this._usePooling = true;
						}
						else
						{
							this._usePooling = false;
						}
						break;
					case "max pool size":
						this._maxPoolSize = int.Parse(strArray2[1].ToString());
						break;
					case "min pool size":
						this._minPoolSize = int.Parse(strArray2[1].ToString());
						break;
					}
				}
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception);
			}
		}

		public bool SaveObject(object obj)
		{
			bool result;
			try
			{
				this.CheckHiberNateTemplate();
				this._hibernateTemplate.Save(obj);
				bool flag = true;
				result = flag;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", string.Concat(new string[]
				{
					"ObjectType:",
					obj.ToString(),
					", ",
					exception.Message,
					exception.StackTrace
				}));
				throw;
			}
			return result;
		}

		public bool SaveObjectAll(object[] objs)
		{
			IStatelessSession session = this._sessionFactory.OpenStatelessSession();
			ITransaction transaction = session.BeginTransaction();
			bool flag = false;
			try
			{
				for (int i = 0; i < objs.Length; i++)
				{
					object obj2 = objs[i];
					session.Insert(obj2);
				}
				transaction.Commit();
				flag = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				transaction.Rollback();
				throw;
			}
			finally
			{
				if (session != null)
				{
					session.Close();
				}
			}
			return flag;
		}

		public bool SaveOrUpdateObject(object obj)
		{
			bool result;
			try
			{
				this.CheckHiberNateTemplate();
				this._hibernateTemplate.SaveOrUpdate(obj);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw;
			}
			return result;
		}

		public bool UpdateObject(object[] objs)
		{
			IStatelessSession session = this._sessionFactory.OpenStatelessSession();
			ITransaction transaction = session.BeginTransaction();
			bool result;
			try
			{
				for (int i = 0; i < objs.Length; i++)
				{
					object obj2 = objs[i];
					session.Update(obj2);
				}
				transaction.Commit();
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw;
			}
			finally
			{
				if (session != null)
				{
					session.Close();
				}
			}
			return result;
		}

		public bool UpdateObject(object obj)
		{
			bool result;
			try
			{
				this.CheckHiberNateTemplate();
				this._hibernateTemplate.Update(obj);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw;
			}
			return result;
		}

		public bool DeleteAllObject(System.Type objectType)
		{
			bool result;
			try
			{
				this.CheckHiberNateTemplate();
				this._hibernateTemplate.Delete("From " + objectType);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw;
			}
			return result;
		}

		public bool ExecuteObjectNonQuery(string query)
		{
			ISession dBSession = this._sessionFactory.OpenSession();
			ITransaction transaction = dBSession.BeginTransaction();
			bool result;
			try
			{
				System.Data.IDbCommand command = dBSession.Connection.CreateCommand();
				command.Connection = dBSession.Connection;
				transaction.Enlist(command);
				command.CommandText = query;
				command.ExecuteNonQuery();
				dBSession.Flush();
				transaction.Commit();
				result = true;
			}
			catch (System.Exception exception)
			{
				transaction.Rollback();
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			finally
			{
				if (dBSession != null)
				{
					dBSession.Close();
				}
				if (transaction != null)
				{
					transaction = null;
				}
			}
			return result;
		}

		private string SetCompareType(string name, object value, DBCompareType compareType)
		{
			string str = "";
			try
			{
				if (name == null || name.Trim() == "" || value == null)
				{
					string result = str;
					return result;
				}
				switch (compareType)
				{
				case DBCompareType.GREAT:
				{
					string result = string.Concat(new object[]
					{
						name,
						">'",
						value,
						"'"
					});
					return result;
				}
				case DBCompareType.SMALL:
				{
					string result = string.Concat(new object[]
					{
						name,
						"<'",
						value,
						"'"
					});
					return result;
				}
				case DBCompareType.NOT:
				{
					string result = string.Concat(new object[]
					{
						name,
						"!='",
						value,
						"'"
					});
					return result;
				}
				case DBCompareType.LIKE:
				{
					string result = string.Concat(new object[]
					{
						name,
						"'",
						value,
						"'"
					});
					return result;
				}
				case DBCompareType.EQUALGREAT:
				{
					string result = string.Concat(new object[]
					{
						name,
						">='",
						value,
						"'"
					});
					return result;
				}
				case DBCompareType.EQUALSMALL:
				{
					string result = string.Concat(new object[]
					{
						name,
						"<='",
						value,
						"'"
					});
					return result;
				}
				default:
					str = string.Concat(new object[]
					{
						name,
						"='",
						value,
						"'"
					});
					break;
				}
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return str;
		}

		private void SetCompareTypeByDelete(System.Type objType, string[] names, object[] values, DBCompareType[] compareTypes, bool andflag)
		{
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType;
				string str2 = "";
				if (compareTypes != null)
				{
					str2 += this.SetCompareType(names[0], values[0], compareTypes[0]);
				}
				else
				{
					str2 += this.SetCompareType(names[0], values[0], DBCompareType.NONE);
				}
				if (names.Length != 1)
				{
					for (int i = 1; i < names.Length; i++)
					{
						string str3;
						if (compareTypes != null)
						{
							str3 = this.SetCompareType(names[i], values[i], compareTypes[i]);
						}
						else
						{
							str3 = this.SetCompareType(names[i], values[i], DBCompareType.NONE);
						}
						if (str3.Trim() != "")
						{
							if (str2.Trim() != "")
							{
								if (andflag)
								{
									str2 = str2 + " And " + str3;
								}
								else
								{
									str2 = str2 + " Or " + str3;
								}
							}
							else
							{
								str2 += str3;
							}
						}
					}
				}
				if (str2.Trim() != "")
				{
					queryString = queryString + " Where " + str2;
				}
				this._hibernateTemplate.Delete(queryString);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
		}

		private void SetCompareTypeByDelete(System.Type objType, string name, object[] values, DBCompareType[] compareTypes, bool andflag)
		{
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType;
				string str2 = "";
				if (compareTypes != null)
				{
					str2 += this.SetCompareType(name, values[0], compareTypes[0]);
				}
				else
				{
					str2 += this.SetCompareType(name, values[0], DBCompareType.NONE);
				}
				if (values.Length != 1)
				{
					for (int i = 1; i < values.Length; i++)
					{
						string str3;
						if (compareTypes != null)
						{
							str3 = this.SetCompareType(name, values[i], compareTypes[i]);
						}
						else
						{
							str3 = this.SetCompareType(name, values[i], DBCompareType.NONE);
						}
						if (str3.Trim() != "")
						{
							if (str2.Trim() != "")
							{
								if (andflag)
								{
									str2 = str2 + " And " + str3;
								}
								else
								{
									str2 = str2 + " Or " + str3;
								}
							}
							else
							{
								str2 += str3;
							}
						}
					}
				}
				if (str2.Trim() != "")
				{
					queryString = queryString + " Where " + str2;
				}
				this._hibernateTemplate.Delete(queryString);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
		}

		private System.Collections.IList SetCompareType(System.Type objType, string[] names, object[] values, DBCompareType[] compareTypes, DBSortType[] sortTypes, bool andflag)
		{
			System.Collections.IList list2;
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType;
				string str2 = "";
				if (compareTypes != null)
				{
					str2 += this.SetCompareType(names[0], values[0], compareTypes[0]);
				}
				else
				{
					str2 += this.SetCompareType(names[0], values[0], DBCompareType.NONE);
				}
				if (names.Length != 1)
				{
					for (int i = 1; i < names.Length; i++)
					{
						string str3;
						if (compareTypes != null)
						{
							str3 = this.SetCompareType(names[i], values[i], compareTypes[i]);
						}
						else
						{
							str3 = this.SetCompareType(names[i], values[i], DBCompareType.NONE);
						}
						if (str3.Trim() != "")
						{
							if (str2.Trim() != "")
							{
								if (andflag)
								{
									str2 = str2 + " And " + str3;
								}
								else
								{
									str2 = str2 + " Or " + str3;
								}
							}
							else
							{
								str2 += str3;
							}
						}
					}
				}
				if (str2.Trim() != "")
				{
					queryString = queryString + " Where " + str2;
				}
				queryString += this.SetSortType(names, sortTypes);
				list2 = this._hibernateTemplate.Find(queryString);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return list2;
		}

		private System.Collections.IList SetCompareType(System.Type objType, string name, object[] values, DBCompareType[] compareTypes, DBSortType sortType, bool andflag)
		{
			System.Collections.IList list2;
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType;
				string str2 = "";
				if (compareTypes != null)
				{
					str2 += this.SetCompareType(name, values[0], compareTypes[0]);
				}
				else
				{
					str2 += this.SetCompareType(name, values[0], DBCompareType.NONE);
				}
				if (values.Length != 1)
				{
					for (int i = 1; i < values.Length; i++)
					{
						string str3;
						if (compareTypes != null)
						{
							str3 = this.SetCompareType(name, values[i], compareTypes[i]);
						}
						else
						{
							str3 = this.SetCompareType(name, values[i], DBCompareType.NONE);
						}
						if (str3.Trim() != "")
						{
							if (str2.Trim() != "")
							{
								if (andflag)
								{
									str2 = str2 + " And " + str3;
								}
								else
								{
									str2 = str2 + " Or " + str3;
								}
							}
							else
							{
								str2 += str3;
							}
						}
					}
				}
				if (str2.Trim() != "")
				{
					queryString = queryString + " Where " + str2;
				}
				queryString += this.SetSortType(name, sortType);
				list2 = this._hibernateTemplate.Find(queryString);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return list2;
		}

		private string SetSortType(string name, DBSortType sortType)
		{
			string str2;
			try
			{
				string str;
				if (sortType == DBSortType.DESCENDING)
				{
					str = name + " DESC";
				}
				else
				{
					str = name;
				}
				str2 = " ORDER BY " + str;
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return str2;
		}

		private string SetSortBy(string name, DBSortType sortType)
		{
			string str2;
			try
			{
				if (sortType == DBSortType.NONE)
				{
					return "";
				}
				string str = " ORDER BY ";
				if (sortType == DBSortType.DESCENDING)
				{
					str = str + name + " DESC";
				}
				else
				{
					str += name;
				}
				str2 = str;
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return str2;
		}

		private string SetSortType(string[] names, DBSortType[] sortTypes)
		{
			string str = "";
			string str2;
			try
			{
				if (sortTypes == null)
				{
					return str;
				}
				System.Collections.ArrayList list = new System.Collections.ArrayList();
				bool flag = true;
				for (int i = 0; i < names.Length; i++)
				{
					if (!list.Contains(names[i]) && sortTypes[i] != DBSortType.NONE)
					{
						if (flag)
						{
							str = this.SetSortType(names[i], sortTypes[i]);
							flag = false;
						}
						else
						{
							str = str + ", " + this.SetSortType(names[i], sortTypes[i]);
							list.Add(names[i]);
						}
					}
				}
				str2 = str.Substring(1);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return str2;
		}

		public bool DeleteAllObjectNonQuery(string tableName)
		{
			bool result;
			try
			{
				string query = "Delete From " + tableName;
				result = this.ExecuteObjectNonQuery(query);
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			return result;
		}

		public bool DeleteObject(object[] objs)
		{
			ISession session = this._sessionFactory.OpenSession();
			ITransaction transaction = session.BeginTransaction();
			bool result;
			try
			{
				for (int i = 0; i < objs.Length; i++)
				{
					object obj2 = objs[i];
					session.Delete(obj2);
				}
				session.Flush();
				transaction.Commit();
				result = true;
			}
			catch (System.Exception ex)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex);
				throw;
			}
			finally
			{
				if (session != null)
				{
					session.Close();
				}
			}
			return result;
		}

		public bool DeleteObject(object obj)
		{
			bool result;
			try
			{
				this.CheckHiberNateTemplate();
				this._hibernateTemplate.Delete(obj);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw;
			}
			return result;
		}

		public bool DeleteObject(System.Type objectType, string name, object value, DBCompareType compareType)
		{
			bool result;
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objectType;
				string str2 = this.SetCompareType(name, value, compareType);
				if (str2.Trim() != "")
				{
					queryString = queryString + " Where " + str2;
				}
				this._hibernateTemplate.Delete(queryString);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			return result;
		}

		public bool DeleteObject_AND(System.Type objectType, string[] names, object[] values, DBCompareType[] compareTypes)
		{
			bool result;
			try
			{
				this.SetCompareTypeByDelete(objectType, names, values, compareTypes, true);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			return result;
		}

		public bool DeleteObject_AND(System.Type objectType, string name, object[] values, DBCompareType[] compareTypes)
		{
			this.AcquireWriterLock();
			bool result;
			try
			{
				this.SetCompareTypeByDelete(objectType, name, values, compareTypes, true);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			return result;
		}

		public bool DeleteObject_OR(System.Type objectType, string[] names, object[] values, DBCompareType[] compareTypes)
		{
			bool result;
			try
			{
				this.SetCompareTypeByDelete(objectType, names, values, compareTypes, false);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			return result;
		}

		public bool DeleteObject_OR(System.Type objectType, string name, object[] values, DBCompareType[] compareTypes)
		{
			bool result;
			try
			{
				this.SetCompareTypeByDelete(objectType, name, values, compareTypes, false);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			return result;
		}

		public bool DeleteObjectByQuery(string query)
		{
			bool result;
			try
			{
				this.CheckHiberNateTemplate();
				this._hibernateTemplate.Delete("From " + query);
				result = true;
			}
			catch (System.Exception exception)
			{
				this.Logger.LogErrorWrite("", "HibernateAdapter", System.Reflection.MethodBase.GetCurrentMethod().Name + "()", exception.Message);
				throw exception;
			}
			return result;
		}

		public System.Collections.IList GetObject(System.Type objType)
		{
			System.Collections.IList list2;
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType.ToString();
				list2 = this._hibernateTemplate.Find(queryString);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return list2;
		}

		public System.Collections.IList GetObject(System.Type objType, string name, DBSortType sortType)
		{
			System.Collections.IList list2;
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType + this.SetSortBy(name, sortType);
				list2 = this._hibernateTemplate.Find(queryString);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return list2;
		}

		public System.Collections.IList GetObject(System.Type objType, int start, int max)
		{
			ISession session = this._sessionFactory.OpenSession();
			System.Collections.IList list2;
			try
			{
				string queryString = "From " + objType.ToString();
				IQuery query = session.CreateQuery(queryString).SetFirstResult(start).SetMaxResults(max);
				list2 = query.List();
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (session != null)
				{
					session.Close();
				}
			}
			return list2;
		}

		public int GetCount(System.Type objType)
		{
			ISession session = this._sessionFactory.OpenSession();
			int result;
			try
			{
				ICriteria criteria = session.CreateCriteria(objType.ToString());
				int count = criteria.List().Count;
				result = count;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (session != null)
				{
					session.Close();
				}
			}
			return result;
		}

		public System.Collections.IList GetObject(System.Type objType, string[] names, DBSortType[] sortTypes)
		{
			System.Collections.IList list2;
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType + this.SetSortType(names, sortTypes);
				list2 = this._hibernateTemplate.Find(queryString);
			}
			catch (System.Exception exception)
			{
				throw new System.Exception(exception.Message);
			}
			return list2;
		}

		public System.Collections.IList GetObject(System.Type objType, string name, object value, DBCompareType compareType, DBSortType sortType)
		{
			System.Collections.IList list2;
			try
			{
				this.CheckHiberNateTemplate();
				string queryString = "From " + objType;
				string str2 = this.SetCompareType(name, value, compareType);
				if (str2.Trim() != "")
				{
					queryString = queryString + " Where " + str2;
				}
				queryString += this.SetSortBy(name, sortType);
				list2 = this._hibernateTemplate.Find(queryString);
			}
			catch (System.Exception exception)
			{
				throw exception;
			}
			return list2;
		}

		public System.Collections.IList GetObject_AND(System.Type objType, string[] names, object[] values, DBCompareType[] compareTypes, DBSortType[] sortTypes)
		{
			System.Collections.IList list2;
			try
			{
				list2 = this.SetCompareType(objType, names, values, compareTypes, sortTypes, true);
			}
			catch (System.Exception exception)
			{
				throw exception;
			}
			return list2;
		}

		public System.Collections.IList GetObject_AND(System.Type objType, string name, object[] values, DBCompareType[] compareTypes, DBSortType sortType)
		{
			System.Collections.IList list2 = null;
			try
			{
				list2 = this.SetCompareType(objType, name, values, compareTypes, sortType, true);
			}
			catch (System.Exception exception)
			{
				throw exception;
			}
			return list2;
		}

		public System.Collections.IList GetObject_OR(System.Type objType, string[] names, object[] values, DBCompareType[] compareTypes)
		{
			System.Collections.IList list2;
			try
			{
				list2 = this.SetCompareType(objType, names, values, compareTypes, null, false);
			}
			catch (System.Exception exception)
			{
				throw exception;
			}
			return list2;
		}

		public System.Collections.IList GetObject_OR(System.Type objType, string name, object[] values, DBCompareType[] compareTypes, DBSortType sortType)
		{
			System.Collections.IList list2;
			try
			{
				list2 = this.SetCompareType(objType, name, values, compareTypes, sortType, false);
			}
			catch (System.Exception exception)
			{
				throw exception;
			}
			return list2;
		}

		public System.Collections.IList GetObjectByQuery(string query)
		{
			ISession dBSession = this._sessionFactory.OpenSession();
			System.Collections.IList list2;
			try
			{
				System.Collections.IList list = dBSession.CreateQuery(query).List();
				if (list.Count == 0)
				{
					return null;
				}
				list2 = list;
			}
			catch (System.Exception exception)
			{
				throw exception;
			}
			finally
			{
				if (dBSession != null)
				{
					dBSession.Close();
				}
			}
			return list2;
		}

		public System.Collections.Generic.IList<System.Collections.Hashtable> ExecuteSQL(string sql)
		{
			ISession dBSession = this._sessionFactory.OpenSession();
			System.Collections.Generic.IList<System.Collections.Hashtable> result = null;
			try
			{
				result = dBSession.CreateSQLQuery(sql).SetResultTransformer(Transformers.AliasToEntityMap).List<System.Collections.Hashtable>();
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dBSession != null)
				{
					dBSession.Close();
				}
			}
			return result;
		}

		public System.Collections.Generic.IList<T> ExecuteSQL<T>(string sql, System.Type t)
		{
			ISession dBSession = this._sessionFactory.OpenSession();
			System.Collections.Generic.IList<T> result = null;
			try
			{
				result = dBSession.CreateSQLQuery(sql).AddEntity(t).List<T>();
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dBSession != null)
				{
					dBSession.Close();
				}
			}
			return result;
		}

		public int ExecuteNoSQL(string sql)
		{
			ISession dBSession = this._sessionFactory.OpenSession();
			int result = -1;
			try
			{
				result = dBSession.CreateSQLQuery(sql).ExecuteUpdate();
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dBSession != null)
				{
					dBSession.Close();
				}
			}
			return result;
		}

		public int ExecuteNoSQL(string[] sqls)
		{
			int result = -1;
			ISession dBSession = this._sessionFactory.OpenSession();
			using (ITransaction trx = dBSession.BeginTransaction())
			{
				try
				{
					for (int i = 0; i < sqls.Length; i++)
					{
						string str = sqls[i];
						result += dBSession.CreateSQLQuery(str).ExecuteUpdate();
					}
					trx.Commit();
				}
				catch (System.Exception ex)
				{
					trx.Rollback();
					throw ex;
				}
				finally
				{
					if (dBSession != null)
					{
						dBSession.Close();
					}
				}
			}
			return result;
		}
	}
}
