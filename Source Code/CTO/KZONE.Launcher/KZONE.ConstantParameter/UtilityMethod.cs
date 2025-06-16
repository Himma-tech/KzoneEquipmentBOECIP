using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using KZONE.Log;
using System.Globalization;
namespace KZONE.ConstantParameter
{
    public class UtilityMethod
    {
        private struct SYSTEMTIME
        {
            public ushort wYear;

            public ushort wMonth;

            public ushort wDayOfWeek;

            public ushort wDay;

            public ushort wHour;

            public ushort wMinute;

            public ushort wSecond;

            public ushort wMilliseconds;
        }

        private static ILogManager logger = NLogManager.Logger;

        private static object _lockObj = new object();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int SetSystemTime(ref UtilityMethod.SYSTEMTIME sysTime);

        public static bool SetSysDateTime(string dateTime)
        {
            bool result;
            try
            {
                DateTime arg_05_0 = DateTime.Now;
                UtilityMethod.SYSTEMTIME sysTime = default(UtilityMethod.SYSTEMTIME);
                DateTime updateTime;
                if (!DateTime.TryParseExact(dateTime.Trim(), "yyyyMMddHHmmss", null, DateTimeStyles.None, out updateTime))
                {
                    result = false;
                }
                else
                {
                    DateTime utcTime = updateTime.AddHours(0.0 - TimeZone.CurrentTimeZone.GetUtcOffset(updateTime).TotalHours);
                    sysTime.wYear = (ushort)utcTime.Year;
                    sysTime.wMonth = (ushort)utcTime.Month;
                    sysTime.wDay = (ushort)utcTime.Day;
                    sysTime.wDayOfWeek = (ushort)utcTime.DayOfWeek;
                    sysTime.wHour = (ushort)utcTime.Hour;
                    sysTime.wMinute = (ushort)utcTime.Minute;
                    sysTime.wSecond = (ushort)utcTime.Second;
                    sysTime.wMilliseconds = (ushort)utcTime.Millisecond;
                    UtilityMethod.SetSystemTime(ref sysTime);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                UtilityMethod.logger.LogErrorWrite("", "UtilityMethod", MethodBase.GetCurrentMethod().Name + "()", ex);
                result = false;
            }
            return result;
        }

        public static string GetAgentTrackKey()
        {
            string result;
            lock (UtilityMethod._lockObj)
            {
                result = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            }
            return result;
        }

        public static string Reverse(string original)
        {
            char[] arr = original.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
