using System;
using System.Collections.Generic;

namespace KZONE.PLCAgent.PLC
{
	/// <summary>
	/// plc driver operation interface
	/// </summary>
	public interface IPLCDriver : IDisposable
	{
		/// <summary>
		/// plc driver log event
		/// </summary>
		event PLCDriverDebugOutEventHandler PLCDriverDebugOutEvent;

		/// <summary>
		/// plc connected event
		/// </summary>
		event PLCConnectedEventHandler PLCConnectedEvent;

		/// <summary>
		/// plc disconnected event
		/// </summary>
		event PLCDisconnectedEventHandler PLCDisconnectedEvent;

		/// <summary>
		/// trx triggered event
		/// </summary>
		event TrxTriggeredEventHandler TrxTriggeredEvent;

		/// <summary>
		/// readtrx result event
		/// </summary>
		event ReadTrxResultEventHandler ReadTrxResultEvent;

		/// <summary>
		/// writetrx result event
		/// </summary>
		event WriteTrxResultEventHandler WriteTrxResultEvent;

		/// <summary>
		/// get driver id
		/// </summary>
		string DriverId
		{
			get;
		}

		/// <summary>
		/// get config file url
		/// </summary>
		string ConfigFileUrl
		{
			get;
		}

		/// <summary>
		/// get trx format file url
		/// </summary>
		string FormatFileUrl
		{
			get;
		}

		/// <summary>
		/// get is driver inited(can init only once)
		/// </summary>
		bool IsInited
		{
			get;
		}

		/// <summary>
		/// get is driver started(true=Start,false=Stop)
		/// </summary>
		bool IsStarted
		{
			get;
		}

		/// <summary>
		/// get is driver disposed
		/// </summary>
		bool IsDisposed
		{
			get;
		}

		/// <summary>
		/// get plc driver runtime info
		/// </summary>
		IDictionary<string, object> RuntimeInfo
		{
			get;
		}

		/// <summary>
		/// get plc state (LogicalStationNo,isconnected(true/false)) pair
		/// </summary>
		IDictionary<string, object> PLCState
		{
			get;
		}

		/// <summary>
		/// get/set enable dump scan buffer
		/// </summary>
		bool EnableScanBufferDump
		{
			get;
			set;
		}

		/// <summary>
		/// get/set config content
		/// </summary>
		string ConfigContent
		{
			get;
			set;
		}

		/// <summary>
		/// get iodatas
		/// </summary>
		Dictionary<string, PLCScanBuffer> IODatas
		{
			get;
		}

		/// <summary>
		/// get plcadapter
		/// </summary>
		Dictionary<int, PLCAdapter> Adapters
		{
			get;
		}

		int ScanTaskWaitFrequency
		{
			get;
			set;
		}

		int ScanTaskWaitPeriodMS
		{
			get;
			set;
		}

		int WriteTaskWaitPeriodMS
		{
			get;
			set;
		}

		int TriggerTaskWaitFrequency
		{
			get;
			set;
		}

		int TriggerTaskWaitPeriodMS
		{
			get;
			set;
		}

		int TriggerMaxWorkIterateTimes
		{
			get;
			set;
		}

		int PLCAdapterMaxThreadSleepTimeMS
		{
			get;
			set;
		}

		int PLCAdapterMaxIterateTimes
		{
			get;
			set;
		}

		bool DebugOutEnable
		{
			get;
			set;
		}

		bool FlipBCDValue
		{
			get;
			set;
		}

		/// <summary>
		/// init plc driver(can init only once),may throw exception
		/// </summary>
		/// <param name="configFileUrl">config file url</param>
		/// <param name="formatFileUrl">trx format file url</param>
		/// <param name="reason">NG reason</param>
		/// <returns>true=OK,false=NG</returns>
		bool Init(string configFileUrl, string formatFileUrl, out string reason);

		/// <summary>
		/// init plc driver(can init only once),may throw exception
		/// </summary>
		/// <param name="configFileUrl">config file url</param>
		/// <param name="formatFileUrl">trx format file url</param>
		/// <param name="reason">NG reason</param>
		/// <param name="transforms">transform list</param> 
		/// <returns>true=OK,false=NG</returns>		
		bool Init(string configFileUrl, string formatFileUrl, out string reason, List<Transform> transforms);

		/// <summary>
		/// start plc driver,may throw exception
		/// </summary>
		/// <param name="reason">NG reason</param>
		/// <returns>true=OK,false=NG</returns>
		bool Start(out string reason);

		/// <summary>
		/// stop plc driver
		/// </summary>
		void Stop();

		/// <summary>
		/// destroy plc driver
		/// </summary>
		void Destroy();

		/// <summary>
		/// get empty trx object
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trxName">trx name</param>
		/// <returns>trx object,null if err occur</returns>
		Trx GetEmptyTrx(string trxName);

		/// <summary>
		/// reqeust to read trx data from scan buffer,ReadTrxResult event will fire after buffer read operation comp		
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trxName">trx name</param>		
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool ReadTrx(string trxName, out string reason);

		/// <summary>
		/// reqeust to read trx data from scan buffer,ReadTrxResult event will fire after buffer read operation comp		
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trx">trx object</param>
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool ReadTrx(Trx trx, out string reason);

		/// <summary>
		/// reqeust to read trx data direct from plc,ReadTrxResult event will fire after plc read operation comp		
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trx">trx object</param>
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool DirectReadTrx(Trx trx, out string reason);

		/// <summary>
		/// reqeust to read trx data from scan buffer synchronously,trx items value will be filled before return
		/// <br>may throw exception</br>
		/// </summary>		
		/// <param name="trx">trx object</param>
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool SyncReadTrx(Trx trx, out string reason);

		/// <summary>
		/// reqeust to read trx data from scan buffer synchronously,trx items value will be filled before return
		/// <br>may throw exception</br>
		/// </summary>		
		/// <param name="trxName">trx name</param>		
		/// <param name="reason">reject reason</param>
		/// <returns>trx object,null if error occur</returns>
		Trx SyncReadTrx(string trxName, out string reason);

		/// <summary>
		/// request to write trx data to plc synchronously,the thread will block until write comp
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trx">trx object</param>
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool SyncWriteTrx(Trx trx, out string reason);

		/// <summary>
		/// request to write trx data to plc,WriteTrxResult event will fire after plc write operation comp		
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trx">trx object</param>
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool WriteTrx(Trx trx, out string reason);

		/// <summary>
		/// request to write trx data to plc(one random write cmd only),WriteTrxResult event will fire after plc write operation comp		
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trx">trx object</param>
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool RandomWriteTrx(Trx trx, out string reason);

		/// <summary>
		/// request to write trx raw data to plc,WriteTrxResult event will fire after plc write operation comp		
		/// <br>may throw exception</br>
		/// </summary>
		/// <param name="trx">trx object</param>
		/// <param name="reason">reject reason</param>
		/// <returns>true=accept,false=reject</returns>
		bool WriteTrxRaw(Trx trx, out string reason);

		/// <summary>
		/// get plc data model
		/// </summary>
		/// <returns>model object,null if err occur</returns>
		PLCDataModel GetModel();
	}
}
