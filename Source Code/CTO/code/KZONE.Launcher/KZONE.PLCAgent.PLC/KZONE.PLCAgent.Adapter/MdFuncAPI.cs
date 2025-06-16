using System;
using System.Runtime.InteropServices;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// MdFunc api to access and operate Mitsubishi PC Interface Board
	/// </summary>
	internal static class MdFuncAPI
	{
		/// Return Type: SHORT-&gt;short
		///             chan: SHORT-&gt;short
		///             mode: SHORT-&gt;short
		///             path: LPLONG-&gt;int*
		[DllImport("MdFunc32.dll")]
		public static extern short mdOpen(short chan, short mode, ref int path);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		[DllImport("MdFunc32.dll")]
		public static extern short mdClose(int path);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             devtype: SHORT-&gt;short
		///             devno: SHORT-&gt;short
		///             bytesize: PSHORT-&gt;SHORT*
		///             data: LPVOID-&gt;void*
		[DllImport("MdFunc32.dll")]
		public static extern short mdSend(int path, short stno, short devtype, short devno, ref short bytesize, ref short data);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             evtype: SHORT-&gt;short
		///             devno: SHORT-&gt;short
		///             bytesize: PSHORT-&gt;SHORT*
		///             data: LPVOID-&gt;void*
		[DllImport("MdFunc32.dll")]
		public static extern short mdReceive(int path, short stno, short evtype, short devno, ref short bytesize, ref short data);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             devtype: SHORT-&gt;short
		///             devno: SHORT-&gt;short
		[DllImport("MdFunc32.dll")]
		public static extern short mdDevSet(int path, short stno, short devtype, short devno);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             devtype: SHORT-&gt;short
		///             devno: SHORT-&gt;short
		[DllImport("MdFunc32.dll")]
		public static extern short mdDevRst(int path, short stno, short devtype, short devno);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             dev: LPVOID-&gt;void*
		///             buf: LPVOID-&gt;void*
		///             bufsize: SHORT-&gt;short
		[DllImport("MdFunc32.dll")]
		public static extern short mdRandW(int path, short stno, ref short dev, ref short buf, short bufsize);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             dev: LPVOID-&gt;void*
		///             buf: LPVOID-&gt;void*
		///             bufsize: SHORT-&gt;short
		[DllImport("MdFunc32.dll")]
		public static extern short mdRandR(int path, short stno, ref short dev, ref short buf, short bufsize);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             buf: SHORT-&gt;short
		[DllImport("MdFunc32.dll")]
		public static extern short mdControl(int path, short stno, ref short buf);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             stno: SHORT-&gt;short
		///             buf: PSHORT-&gt;SHORT*
		[DllImport("MdFunc32.dll")]
		public static extern short mdTypeRead(int path, short stno, ref short buf);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             buf: PSHORT-&gt;SHORT*
		[DllImport("MdFunc32.dll")]
		public static extern short mdBdLedRead(int path, ref short buf);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             mode: PSHORT-&gt;SHORT*
		[DllImport("MdFunc32.dll")]
		public static extern short mdBdModRead(int path, ref short mode);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             mode: SHORT-&gt;short
		[DllImport("MdFunc32.dll")]
		public static extern short mdBdModSet(int path, short mode);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		[DllImport("MdFunc32.dll")]
		public static extern short mdBdRst(int path);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             buf: PSHORT-&gt;SHORT*
		[DllImport("MdFunc32.dll")]
		public static extern short mdBdSwRead(int path, ref short buf);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             buf: PSHORT-&gt;SHORT*
		[DllImport("MdFunc32.dll")]
		public static extern short mdBdVerRead(int path, ref short buf);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		[DllImport("MdFunc32.dll")]
		public static extern short mdInit(int path);

		/// Return Type: SHORT-&gt;short
		///             path: LONG-&gt;int
		///             eventno: PSHORT-&gt;SHORT*
		///             timeout: LONG-&gt;int
		///             signaledno: PSHORT-&gt;SHORT*
		///             details: PSHORT-&gt;SHORT*
		[DllImport("MdFunc32.dll")]
		public static extern short mdWaitBdEvent(int path, ref short eventno, int timeout, ref short signaledno, ref short details);

		/// Return Type: LONG-&gt;int
		///             path: LONG-&gt;int
		///             netno: LONG-&gt;int
		///             stno: LONG-&gt;int
		///             devtype: LONG-&gt;int
		///             devno: LONG-&gt;int
		///             bytesize: LPLONG-&gt;int*
		///             data: LPVOID-&gt;void*
		[DllImport("MdFunc32.dll")]
		public static extern int mdSendEx(int path, int netno, int stno, int devtype, int devno, ref int bytesize, ref short data);

		/// Return Type: LONG-&gt;int
		///             path: LONG-&gt;int
		///             netno: LONG-&gt;int
		///             stno: LONG-&gt;int
		///             devtype: LONG-&gt;int
		///             devno: LONG-&gt;int
		///             bytesize: LPLONG-&gt;int*
		///             data: LPVOID-&gt;void*
		[DllImport("MdFunc32.dll")]
		public static extern int mdReceiveEx(int path, int netno, int stno, int devtype, int devno, ref int bytesize, ref short data);

		/// Return Type: LONG-&gt;int
		///             path: LONG-&gt;int
		///             netno: LONG-&gt;int
		///             stno: LONG-&gt;int
		///             devtype: LONG-&gt;int
		///             devno: LONG-&gt;int
		[DllImport("MdFunc32.dll")]
		public static extern int mdDevSetEx(int path, int netno, int stno, int devtype, int devno);

		/// Return Type: LONG-&gt;int
		///             path: LONG-&gt;int
		///             netno: LONG-&gt;int
		///             stno: LONG-&gt;int
		///             devtype: LONG-&gt;int
		///             devno: LONG-&gt;int
		[DllImport("MdFunc32.dll")]
		public static extern int mdDevRstEx(int path, int netno, int stno, int devtype, int devno);

		/// Return Type: LONG-&gt;int
		///             path: LONG-&gt;int
		///             netno: LONG-&gt;int
		///             stno: LONG-&gt;int
		///             dev: LPVOID-&gt;void*
		///             buf: LPVOID-&gt;void*
		///             bufsize: LONG-&gt;int
		[DllImport("MdFunc32.dll")]
		public static extern int mdRandWEx(int path, int netno, int stno, ref int dev, ref short buf, int bufsize);

		/// Return Type: LONG-&gt;int
		///             path: LONG-&gt;int
		///             netno: LONG-&gt;int
		///             stno: LONG-&gt;int
		///             dev: LPVOID-&gt;void*
		///             buf: LPVOID-&gt;void*
		///             bufsize: LONG-&gt;int
		[DllImport("MdFunc32.dll")]
		public static extern int mdRandREx(int path, int netno, int stno, ref int dev, ref short buf, int bufsize);
	}
}
