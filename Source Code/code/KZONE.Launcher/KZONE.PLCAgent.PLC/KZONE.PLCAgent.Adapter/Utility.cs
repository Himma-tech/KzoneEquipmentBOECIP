using System;
using System.Collections;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// helper class to offer utility function
	/// </summary>
	internal static class Utility
	{
		/// <summary>
		/// get parameter int  vlaue  by key
		/// </summary>
		public static int GetParamterInt(string key, Hashtable paramTable, int replace = 0)
		{
			int result;
			try
			{
				if (paramTable.ContainsKey(key))
				{
					int val;
					if (!int.TryParse((string)paramTable[key], out val))
					{
						val = replace;
					}
					result = val;
				}
				else
				{
					result = replace;
				}
			}
			catch
			{
				result = replace;
			}
			return result;
		}

		/// <summary>
		/// get parameter string  vlaue  by key
		/// </summary>
		public static string GetParamterString(string key, Hashtable paramTable, string replace = "")
		{
			string result;
			try
			{
				if (paramTable.ContainsKey(key))
				{
					string str = (string)paramTable[key];
					result = str;
				}
				else
				{
					result = replace;
				}
			}
			catch
			{
				result = replace;
			}
			return result;
		}

		/// <summary>
		/// get parameter object  vlaue  by key
		/// </summary>
		public static object GetParamterObject(string key, Hashtable paramTable)
		{
			object result;
			try
			{
				object obj = null;
				if (paramTable.ContainsKey(key))
				{
					obj = paramTable[key];
				}
				result = obj;
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
