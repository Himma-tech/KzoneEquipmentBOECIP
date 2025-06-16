using System;

namespace KZONE.PLCAgent.PLC
{
	internal static class AdapterFactory
	{
		/// <summary>
		/// create adapter instance
		/// </summary>
		public static AbstractAdapter CreateAdapter(string plctype, string linktype, out string reason)
		{
			reason = string.Empty;
			AbstractAdapter result;
			if (plctype == "MITSUBISHI")
			{
                AbstractAdapter adapter;
				if (linktype.ToUpper() == "MX3")
                {
                    if (Environment.Is64BitProcess)
                    {
                        reason = "platform not x86";
                        result = null;
                        return result;
                    }
					adapter = new MX3Adapter();
				}
				else
				{
					if (!(linktype.ToUpper() == "MDFUNC"))
					{
						reason = "unknown linktype=" + linktype;
						result = null;
						return result;
					}
					adapter = new MdFuncAdapter();
				}
				result = adapter;
			}
			else
			{
				reason = "unknown plctype=" + plctype;
				result = null;
			}
			return result;
		}
	}
}
