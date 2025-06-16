using EipTagLibrary;
using KZONE.ConstantParameter;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.MessageManager;
using KZONE.PLCAgent;
using KZONE.PLCAgent.PLC;
using KZONE.Work;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenSeparatorHandlers;
using Spring.Caching;
using Spring.Expressions;
using Spring.Expressions.Parser.antlr.debug;
using Spring.Objects.Factory.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace KZONE.Service
{
    public partial class EquipmentService : AbstractService
    {

        #region TimerOut 常数
        private const string BCAliveTimeout = "BCAliveTimeout";
        private const string EQAliveTimeout = "EQAliveTimeout";
        private const string CIMModeChangeCommandTimeout = "CIMModeChangeCommandTimeout";
        private const string CIMModeChangeCommandReplyTimeout = "CIMModeChangeCommandReplyTimeout";
        private const string CPCCIMModeChangeCommandTimeout = "CPCCIMModeChangeCommandTimeout";
        private const string CPCCIMModeChangeCommandReplyTimeout = "CPCCIMModeChangeCommandReplyTimeout";
        private const string SoftwareVersionChangeReportReplyTimeout = "SoftwareVersionChangeReportReplyTimeout";
        private const string CPCSoftwareVersionChangeReportTimeout = "CPCSoftwareVersionChangeReportTimeout";
        private const string OperatorLoginLogoutReportTimeout = "OperatorLoginLogoutReportTimeout";
        private const string OperatorLoginLogoutReportReplyTimeout = "OperatorLoginLogoutReportReplyTimeout";
        private const string CPCOperatorLoginLogoutReportTimeout = "CPCOperatorLoginLogoutReportTimeout";
        private const string CPCOperatorLoginLogoutReportReplyTimeout = "CPCOperatorLoginLogoutReportReplyTimeout";
        private const string CPCEquipmentRunModeChangeReportTimeout = "CPCEquipmentRunModeChangeReportTimeout";
        private const string EquipmentRunModeChangeReportReplyTimeout = "EquipmentRunModeChangeReportReplyTimeout";
        private const string EquipmentRunModeSetCommandTimeout = "EquipmentRunModeSetCommandTimeout";
        private const string EquipmentRunModeSetCommandReplyTimeout = "EquipmentRunModeSetCommandReplyTimeout";
        private const string RunModePossibleRequestTimeout = "RunModePossibleRequestTimeout";
        private const string RunModePossibleRequestReplyTimeout = "RunModePossibleRequestReplyTimeout";
        private const string CPCEquipmentRunModeSetCommandTimeout = "CPCEquipmentRunModeSetCommandTimeout";
        private const string CPCEquipmentRunModeSetCommandReplyTimeout = "CPCEquipmentRunModeSetCommandReplyTimeout";
        private const string CurrentRecipeIDReportTimeout = "CurrentRecipeIDReportTimeout";
        private const string CurrentRecipeIDReportReplyTimeout = "CurrentRecipeIDReportReplyTimeout";
        private const string CPCCurrentRecipeIDReportTimeout = "CPCCurrentRecipeIDReportTimeout";
        private const string CPCCurrentRecipeIDReportReplyTimeout = "CPCCurrentRecipeIDReportReplyTimeout";
        private const string RecipeIDModifyReportTimeout = "RecipeIDModifyReportTimeout";
        private const string RecipeIDModifyReportReplyTimeout = "RecipeIDModifyReportReplyTimeout";
        private const string CPCRecipeIDModifyReportTimeout = "CPCRecipeIDModifyReportTimeout";
        private const string CPCRecipeIDModifyReportReplyTimeout = "CPCRecipeIDModifyReportReplyTimeout";
        private const string CPCDateTimeCalibrationCommandReplyTimeout = "CPCDateTimeCalibrationCommandReplyTimeout";
        private const string CPCDateTimeCalibrationCommandTimeout = "CPCDateTimeCalibrationCommandTimeout";
        private const string DateTimeCalibrationCommandTimeout = "DateTimeCalibrationCommandTimeout";
        private const string DateTimeCalibrationCommandReplyTimeout = "DateTimeCalibrationCommandReplyTimeout";
        private const string JobDataRequestTimeout = "JobDataRequestTimeout";
        private const string JobDataRequestReplyTimeout = "JobDataRequestReplyTimeout";
        private const string CPCJobDataRequestTimeout = "CPCJobDataRequestTimeout";
        private const string CPCJobDataRequestReplyTimeout = "CPCJobDataRequestReplyTimeout";
        private const string MessageDisplayCommandTimeout = "MessageDisplayCommandTimeout";
        private const string MessageDisplayCommandReplyTimeout = "MessageDisplayCommandReplyTimeout";
        private const string CPCMessageDisplayCommandTimeout = "CPCMessageDisplayCommandTimeout";
        private const string CPCMessageDisplayCommandReplyTimeout = "CPCMessageDisplayCommandReplyTimeout";
        private const string CIMMessageClearCommandTimeout = "CIMMessageClearCommandTimeout";
        private const string CIMMessageClearCommandReplyTimeout = "CIMMessageClearCommandReplyTimeout";
        private const string CIMMessageConfirmReportTimeout = "CIMMessageConfirmReportTimeout";
        private const string CIMMessageConfirmReportReplyTimeout = "CIMMessageConfirmReportReplyTimeout";
        private const string RecipeStateChangeCommandTimeout = "RecipeStateChangeCommandTimeout";
        private const string RecipeStateChangeCommandReplyTimeout = "RecipeStateChangeCommandReplyTimeout";
        private const string CPCRecipeStateChangeCommandTimeout = "CPCRecipeStateChangeCommandTimeout";
        private const string CPCRecipeStateChangeCommandReplyTimeout = "CPCRecipeStateChangeCommandReplyTimeout";
        private const string AlarmStatusChangeReportTimeout = "AlarmStatusChangeReportTimeout";
        private const string AlarmStatusChangeReportReplyTimeout = "AlarmStatusChangeReportReplyTimeout";
        private const string CPCEquipmentStatusChangeReportTimeout = "CPCEquipmentStatusChangeReportTimeout";
        private const string CPCEquipmentStatusChangeReportReplyTimeout = "CPCEquipmentStatusChangeReportReplyTimeout";
        private const string CPCEquipmentReasonCodeChangeReportTimeout = "CPCEquipmentReasonCodeChangeReportTimeout";
        private const string CPCEquipmentReasonCodeChangeReportReplyTimeout = "CPCEquipmentReasonCodeChangeReportReplyTimeout";
        private const string CPCUnitStateChangeReportTimeout = "CPCUnitStateChangeReportTimeout";
        private const string CPCUnitStateChangeReportReplyTimeout = "CPCUnitStateChangeReportReplyTimeout";
        private const string CPCUnitReasonCodeReportTimeout = "CPCUnitReasonCodeReportTimeout";
        private const string CPCUnitReasonCodeReportReplyTimeout = "CPCUnitReasonCodeReportReplyTimeout";
        private const string CPCFetchGlassDataReportTimeout = "CPCFetchGlassDataReportTimeout";
        private const string CPCFetchGlassDataReportReplyTimeout = "CPCFetchGlassDataReportReplyTimeout";
        private const string CPCStoreGlassDataReportTimeout = "CPCStoreGlassDataReportTimeout";
        private const string CPCStoreGlassDataReportReplyTimeout = "CPCStoreGlassDataReportReplyTimeout";
        private const string RecipeParameterRequestTimeout = "RecipeParameterRequestTimeout";
        private const string RecipeParameterRequestReplyTimeout = "RecipeParameterRequestReplyTimeout";
        private const string RecipeRegisterValidationCommandTimeout = "RecipeRegisterValidationCommandTimeout";
        private const string RecipeRegisterValidationCommandReplyTimeout = "RecipeRegisterValidationCommandReplyTimeout";
        private const string RecipeParameterReportTimeout = "RecipeParameterReportTimeout";
        private const string RecipeParameterReportReplyTimeout = "RecipeParameterReportReplyTimeout";
        private const string UnitRecipeRequestCommandTimeout = "UnitRecipeRequestCommandTimeout";
        private const string UnitRecipeRequestCommandReplyTimeout = "UnitRecipeRequestCommandReplyTimeout";
        private const string UnitRecipeListReportTimeout = "UnitRecipeListReportTimeout";
        private const string UnitRecipeListReportReplyTimeout = "UnitRecipeListReportReplyTimeout";
        private const string UnitRecipeRequestTimeout = "UnitRecipeRequestTimeout";
        private const string UnitRecipeRequestReplyTimeout = "UnitRecipeRequestReplyTimeout";
        private const string RecipeVersionRequestTimeout = "RecipeVersionRequestTimeout";
        private const string RecipeVersionRequestReplyTimeout = "RecipeVersionRequestReplyTimeout";
        private const string JobDataEditReportTimeout = "JobDataEditReportTimeout";
        private const string JobDataEditReportReplyTimeout = "JobDataEditReportReplyTimeout";
        private const string RemovedJobReportTimeout = "RemovedJobReportTimeout";
        private const string RemovedJobReportReplyTimeout = "RemovedJobReportReplyTimeout";
        private const string RecoveredJobReportTimeout = "RecoveredJobReportTimeout";
        private const string RecoveredJobReportReplyTimeout = "RecoveredJobReportReplyTimeout";
        private const string TactTimeDataReportTimeout = "TactTimeDataReportTimeout";
        private const string TactTimeDataReportReplyTimeout = "TactTimeDataReportReplyTimeout";
        private const string ProcessStartJobReportTimeout = "ProcessStartJobReportTimeout";
        private const string ProcessStartJobReportReplyTimeout = "ProcessStartJobReportReplyTimeout";
        private const string ProcessEndJobReportTimeout = "ProcessEndJobReportTimeout";
        private const string ProcessEndJobReportReplyTimeout = "ProcessEndJobReportReplyTimeout";
        private const string JobHoldEventReportTimeout = "JobHoldEventReportTimeout";
        private const string JobHoldEventReportReplyTimeout = "JobHoldEventReportReplyTimeout";

        private const string ReceiveGlassDataReportTimeout = "ReceiveGlassDataReportTimeout";
        private const string SendingGlassDataReportTimeout = "SendingGlassDataReportTimeout";
        private const string ReceiveGlassDataReportReplyTimeout = "ReceiveGlassDataReportReplyTimeout";
        private const string SendingGlassDataReportReplyTimeout = "SendingGlassDataReportReplyTimeout";



        #endregion

        private Thread _eqAlive;
        private bool _isRuning = false;
        private string _aliveValue = "1";
        private Thread _receiveThread;
        private Thread _sendThread;
        private Thread _alarmThread;
        private Thread _unitStatusThread;
        private Thread _equipmentStatusThread;
        private Thread _unitReasonCodeThread;
        private Thread _storeThread;
        private Thread _fetchThread;
        private Thread _recipeReport;
        private Thread _processDataReport;
        private Thread _tackTimeReport;
        private Thread _equipmentStatusScan;

        private Thread _svreport;

        private Thread _receiveUpThread;
        private Thread _receiveReturnThread;
        private Thread _sendUpThread;
        private Thread _sendReturnThread;
        private Thread _facReport;
        private Thread _ppIDPreDownloadFlagReport;

        private Thread _transferTimeDataReport;
        private Thread _tR01_02_TransferRequest;

        private Thread _dFS_Service;

        private bool ReceiveThreadRefresh1 = false;
        private bool ReceiveThreadRefresh2 = false;
        private bool SendThreadRefresh1 = false;
        private bool SendThreadRefresh2 = false;

        List<string> oldAlarms = new List<string>();

        Dictionary<AlarmEntityData, String> alarmDic = new Dictionary<AlarmEntityData, string>();

        Dictionary<int, bool> alarmChannel = new Dictionary<int, bool>();
        Dictionary<int, Unit> unitDic = new Dictionary<int, Unit>();
        Dictionary<int, Unit> unitReasonCodeDic = new Dictionary<int, Unit>();
        Dictionary<int, bool> stroeChannel = new Dictionary<int, bool>();
        Dictionary<int, bool> fetchChannel = new Dictionary<int, bool>();
        ConcurrentDictionary<int, eEQPStatus> lastUnitDic = new ConcurrentDictionary<int, eEQPStatus>();


        private bool alarmReport = true;
        private bool UnitStatusReport = true;
        private bool UnitReasonCodeReport = true;
        private bool StroeReport = true;
        private bool FetchReport = true;
        private bool RecipeReport = false;
        private bool ProcessDataReportFlag = true;
        private bool TackTimeReportFlag = true;
        private string MessageSequenceNo = string.Empty;
        private bool UnitListRecipeReport = true;
        private bool FACReport = true;
        private bool PPIDPreDownloadFlagReport = true;
        private bool UnitRecipeRequestBool = true;//上报UnitRecipeRequest
        private bool TransferTimeDataReportFlag = false;
        private bool TR01_TR02_TransferRequestFlag = false;
        private bool TR01_TR02_TransferRequestReplyWaitFlag = false;
        private int svSaveInterval = 0;
        public eTransfer CurrentGlassSendToTR = eTransfer.NONE;

        private string _SVSavePath = string.Empty;//SV数据的保存路径
        private string _CVSavePath = string.Empty;//CV数据的保存路径
        private string _FACSavePath = string.Empty;//FAC数据的保存路径

        Dictionary<string, TransferTime> dicTransferTimes = new Dictionary<string, TransferTime>();
        List<string> lsGlassID = new List<string>();
        public string RequestMaterialID = string.Empty;
        public bool EQRequestMaterialFlag = false;

        private int EachGroupRecipeCount = 0;
        private int LastRecipeGroupReportIndex = 0;
        private int RecipeListReportIndex = 0;
        private string CurrentRecipeListReportType = string.Empty;
        public static List<string> DirectoryPathList = new List<string>();

        public override bool Init()
        {
            //20250616
            StartEIP();

            _eqAlive = new Thread(new ThreadStart(EQAlive)) { IsBackground = true };
            _eqAlive.Start();

            _receiveThread = new Thread(new ThreadStart(ReceiveGlassAction)) { IsBackground = true };
            _receiveThread.Start();

            _sendThread = new Thread(new ThreadStart(SendGlassAction)) { IsBackground = true };
            _sendThread.Start();

            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

            if (eq != null)
            {
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line != null)
                {
                    if (line.Data.FABTYPE == "ARRAY" || line.Data.FABTYPE == "CELL")
                    {
                        _sendUpThread = new Thread(new ThreadStart(UpSendGlassAction)) { IsBackground = true };
                        _sendUpThread.Start();
                        if (line.Data.FABTYPE == "CELL")
                        {
                            _tR01_02_TransferRequest = new Thread(new ThreadStart(CPCTR01_02_TransferRequest)) { IsBackground = true };
                            _tR01_02_TransferRequest.Start();

                            _receiveReturnThread = new Thread(new ThreadStart(ReturnReceiveGlassAction)) { IsBackground = true };
                            _receiveReturnThread.Start();
                        }
                    }
                    if (line.Data.FABTYPE == "CF")
                    {
                        _ppIDPreDownloadFlagReport = new Thread(new ThreadStart(CPCPPIDPreDownLoadFlagToDownstream)) { IsBackground = true };
                        _ppIDPreDownloadFlagReport.Start();
                    }
                }
            }

            _alarmThread = new Thread(new ThreadStart(CPCAlarmReportBC)) { IsBackground = true };
            _alarmThread.Start();

            //_unitStatusThread = new Thread(new ThreadStart(CPCUnitStateChangeReport)) { IsBackground = true };
            //_unitStatusThread.Start();

            //_unitReasonCodeThread = new Thread(new ThreadStart(CPCUnitReasonCodeReport)) { IsBackground = true };
            //_unitReasonCodeThread.Start();

            _equipmentStatusThread = new Thread(new ThreadStart(CPCEquipmentStatusChangeReport)) { IsBackground = true };
            _equipmentStatusThread.Start();

            _storeThread = new Thread(new ThreadStart(CPCUnitStoreReport)) { IsBackground = true };
            _storeThread.Start();

            _fetchThread = new Thread(new ThreadStart(CPCUnitFetchReport)) { IsBackground = true };
            _fetchThread.Start();

            //_recipeReport = new Thread(new ThreadStart(CPCUnitRecipeListReport)) { IsBackground = true };
            //_recipeReport.Start();

            _processDataReport = new Thread(new ThreadStart(CPCProcessDataReport)) { IsBackground = true };
            _processDataReport.Start();

            _tackTimeReport = new Thread(new ThreadStart(CPCTackTimeDataReport)) { IsBackground = true };
            _tackTimeReport.Start();

            _svreport = new Thread(new ThreadStart(CPCSVDataReport)) { IsBackground = true };
            _svreport.Start();

            _facReport = new Thread(new ThreadStart(CPCFACDataReport)) { IsBackground = true };
            _facReport.Start();

            _transferTimeDataReport = new Thread(new ThreadStart(CPCTransferTimeDataReport)) { IsBackground = true };
            _transferTimeDataReport.Start();

            //_equipmentStatusScan = new Thread(new ThreadStart(EquipmentStatusChangeScan)) { IsBackground = true };
            //_equipmentStatusScan.Start();

            _dFS_Service = new Thread(new ThreadStart(DFSSend)) { IsBackground = true };
            _dFS_Service.Start();

            bool IsOpen = DFSService.Init(eq.Data.LINEID);
            //if (IsOpen)
            //{
            //    //检索Server端所有文件夹路径
            //    ServerPathRefresh(DFSService.FtpFileDirectoryPath + DFSService.StepID + "/");
            //}
            return true;


        }

        #region PLC 连线

        /// <summary> PLC连线状态
        /// 
        /// </summary>
        public void PLCStatusChange(string StationNo, string Status)
        {
            try
            {
                StationNo = "3";
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo("L" + StationNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", StationNo));

                if (eAGENT_STATE.CONNECTED == Status)
                {
                    _isRuning = true;
                    Logger.LogInfoWrite(LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("PLC_STATION_NO=[{0}] IS CONNECTED.", StationNo));

                    lock (eqp) eqp.File.Connection = eBitResult.ON;
                    ObjectManager.EquipmentManager.EnqueueSave(eqp.File);


                }
                else
                {
                    _isRuning = false;

                    lock (eqp) eqp.File.Connection = eBitResult.OFF;
                    ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                    Logger.LogWarnWrite(LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("PLC STATION NO =[{0}] IS DISCONNECTED.", StationNo));

                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region BCAlive
        public void BCAlive(Trx inputData)
        {
            string strlog = string.Empty;

            try
            {

                eBitResult bcAlive = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));

                if (inputData.IsInitTrigger)
                {
                    lock (eqp) eqp.File.BCAlive = bcAlive;
                    return;
                }

                lock (eqp) eqp.File.BCAlive = bcAlive;

                string alive = "0";
                if (bcAlive == eBitResult.OFF)
                {
                    alive = "0";
                }
                else
                {
                    alive = "1";
                }

                //CPCEQDBCAlive(alive);

                //if (eqp.File.AliveTimeout == true)
                //{
                //    CPCBCAliveTimeout(eqp, eBitResult.OFF);

                //}
                ////去掉Timeout标记
                //eqp.File.AliveTimeout = false;
                //ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                //if (_timerManager.IsAliveTimer(inputData.Metadata.NodeNo + "_" + BCAliveTimeout))
                //{
                //    _timerManager.TerminateTimer(inputData.Metadata.NodeNo + "_" + BCAliveTimeout);
                //}

                //_timerManager.CreateTimer(inputData.Metadata.NodeNo + "_" + BCAliveTimeout, false, ParameterManager["BCALIVE"].GetInteger(),
                //                            new System.Timers.ElapsedEventHandler(CheckBCAliveTimeout), inputData.TrackKey);


            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CheckBCAliveTimeout(object subjet, System.Timers.ElapsedEventArgs e)
        {

            string strlog = string.Empty;

            try
            {
                UserTimer timer = subjet as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(sArray[0]);
                if (eq == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", sArray[0]));

                //inputData.Metadata.NodeNo + "_" + BCAliveTimeout
                string timeName = string.Format("{0}_{1}", sArray[0], BCAliveTimeout);

                if (_timerManager.IsAliveTimer(timeName))
                {
                    _timerManager.TerminateTimer(timeName);
                }
                //标记BC Alive Timeout
                eq.File.AliveTimeout = true;


                //上报BC Timeout
                AlarmEntityData alarm = ObjectManager.AlarmManager.GetAlarmProfile("L3", "65501");

                if (alarm != null)
                {
                    AlarmHistory his = new AlarmHistory();
                    his.EVENTNAME = "Alarm";
                    his.UPDATETIME = DateTime.Now;
                    his.ALARMID = alarm.BCALARMID.ToString();
                    his.ALARMCODE = alarm.ALARMCODE;
                    his.ALARMLEVEL = alarm.ALARMLEVEL;
                    his.ALARMTEXT = alarm.ALARMTEXT;
                    his.NODEID = alarm.NODENO;
                    his.ALARMUNIT = alarm.UNITNO;
                    his.ALARMSTATUS = "SET";
                    his.ALARMADDRESS = alarm.ALARMID;


                    Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                  string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", "L3", trackKey, his.ALARMID, his.ALARMTEXT));
                    ObjectManager.AlarmManager.SaveAlarmHistory(his);
                }



                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                //BC Alive Time out 通知设备
                if (eq.File.AliveTimeout == true)
                {
                    CPCBCAliveTimeout(eq, eBitResult.ON);
                }

                strlog = string.Format("[EQUIPMENT={0}] [BC-> EQ][{1}] BC ALIVE TIMEOUT.", sArray[0], trackKey);
                Logger.LogWarnWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region EQAlive
        public void EQAlive()
        {
            int count = 0;
            while (true)
            {
                try
                {
                    count++;
                    if (_isRuning)
                    {
                        if (count >= 15)
                        {
                            Trx outputdata = GetTrxValues("L3_EQAlive");
                            if (outputdata != null)//在BC關閉的過程會取不到trx
                            {
                                count = 0;
                                string bit = outputdata.EventGroups[0].Events[0].Items[0].Value;
                                outputdata.EventGroups[0].Events[0].Items[0].Value = bit.Equals("1") ? "0" : "1";
                                outputdata.TrackKey = UtilityMethod.GetAgentTrackKey();
                                xMessage msg = new xMessage();
                                msg.ToAgent = eAgentName.PLCAgent;

                                msg.Data = outputdata;
                                PutMessage(msg);

                            }
                        }
                        _aliveValue = _aliveValue.Equals("1") ? "0" : "1";
                    }
                    int param = ParameterManager["EQPALIVE"].GetInteger();

                    //M软元件 EQ Alive
                    CPCEQDEQAlive(_aliveValue);

                    Thread.Sleep(param);
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                }
            }
        }
        #endregion

        #region EquipmentCIMMode

        /// <summary> Equipment CIM Mode
        ///
        /// </summary>
        public void EQEquipmentCIMMode(Trx inputData)
        {
            string strlog = string.Empty;

            try
            {

                eBitResult cimMode = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                strlog = string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] CIM_MODE=[{2}]({3}).",
                                      inputData.Metadata.NodeNo, inputData.TrackKey, (int)cimMode, cimMode.ToString());

                Logger.LogInfoWrite(this.LogName, GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));


                lock (eqp) eqp.File.CIMMode = cimMode;

                //点亮Cim Mode
                CPCCimMode(eqp, inputData);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

            }

            catch (Exception ex)
            {

                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 用于PLC复位重连
        /// </summary>
        /// <param name="inputData"></param>
        public void ECEquipmentCIMMode(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                    return;
                string strlog = string.Empty;
                eBitResult cimMode = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));
                if (cimMode == eqp.File.CIMMode)
                {
                    return;
                }
                strlog = string.Format("[EQUIPMENT={0}] [EC Set][{1}] CIM_MODE=[{2}]({3}).",
                                      inputData.Metadata.NodeNo, inputData.TrackKey, (int)eqp.File.CIMMode, eqp.File.CIMMode);
                Logger.LogInfoWrite(this.LogName, GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);


                inputData.EventGroups[0].Events[0].Items[0].Value = ((int)eqp.File.CIMMode).ToString();


                //点亮Cim Mode
                CPCCimMode(eqp, inputData);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

            }

            catch (Exception ex)
            {

                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region EQDUpstreamInlineMode

        public void EQDUpstreamInlineMode(Trx inputData)
        {
            string strlog = string.Empty;

            try
            {

                eBitResult upInLineMode = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                strlog = string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Upstream Inline Mode=[{2}]({3}).",
                    inputData.Metadata.NodeNo, inputData.TrackKey, (int)upInLineMode, upInLineMode.ToString());

                Logger.LogInfoWrite(this.LogName, GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));


                lock (eqp) eqp.File.UpInlineMode = upInLineMode;


                CPCUpstreamInlineMode(eqp, inputData);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

            }

            catch (Exception ex)
            {

                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 用于PLC复位重连
        /// </summary>
        /// <param name="inputData"></param>
        public void ECUpstreamInlineMode(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                eBitResult upInLineMode = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));

                if (upInLineMode == eqp.File.UpInlineMode)
                {
                    return;
                }
                if (!ReceiveThreadRefresh1) ReceiveThreadRefresh1 = true;//Normal进片
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (line.Data.FABTYPE == "CELL")
                {
                    if (!ReceiveThreadRefresh2) ReceiveThreadRefresh2 = true;//Return进片
                }
                string strlog = string.Empty;
                strlog = string.Format("[EQUIPMENT={0}] [EC Set][{1}] Upstream Inline Mode=[{2}]({3}).",
                    inputData.Metadata.NodeNo, inputData.TrackKey, (int)eqp.File.UpInlineMode, eqp.File.UpInlineMode);

                Logger.LogInfoWrite(this.LogName, GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);


                CPCUpstreamInlineMode(eqp, inputData);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

            }

            catch (Exception ex)
            {

                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region EQDDownstreamInlineMode

        public void EQDDownstreamInlineMode(Trx inputData)
        {
            string strlog = string.Empty;

            try
            {

                eBitResult downInLineMode = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                strlog = string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Down stream Inline Mode=[{2}]({3}).",
                    inputData.Metadata.NodeNo, inputData.TrackKey, (int)downInLineMode, downInLineMode.ToString());

                Logger.LogInfoWrite(this.LogName, GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));


                lock (eqp) eqp.File.DownInlineMode = downInLineMode;


                CPCDownstreamInlineMode(eqp, inputData);
                TR01_TR02_TransferRequestFlag = false;
                TR01_TR02_TransferRequestReplyWaitFlag = false;
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

            }

            catch (Exception ex)
            {

                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 用于PLC复位重连
        /// </summary>
        /// <param name="inputData"></param>
        public void ECDownstreamInlineMode(Trx inputData)
        {
            try
            {

                eBitResult downInLineMode = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));

                if (downInLineMode == eqp.File.DownInlineMode)
                {
                    return;
                }
                if (!SendThreadRefresh1) SendThreadRefresh1 = true;//Normal出片
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (line.Data.FABTYPE == "ARRAY" || line.Data.FABTYPE == "CELL")
                {
                    if (!SendThreadRefresh2) SendThreadRefresh2 = true;//Array返流出片，CELL给TR02送片
                }
                string strlog = string.Empty;
                strlog = string.Format("[EQUIPMENT={0}] [EC Set][{1}] Down stream Inline Mode=[{2}]({3}).",
                    inputData.Metadata.NodeNo, inputData.TrackKey, (int)eqp.File.DownInlineMode, eqp.File.DownInlineMode);

                Logger.LogInfoWrite(this.LogName, GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);

                CPCDownstreamInlineMode(eqp, inputData);
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

            }

            catch (Exception ex)
            {

                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region EQDLocalAlarmStatus

        public void EQDLocalAlarmStatus(Trx inputData)
        {
            string strlog = string.Empty;

            try
            {

                eBitResult hasAlram = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                strlog = string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Local Alarm Status=[{2}]({3}).",
                    inputData.Metadata.NodeNo, inputData.TrackKey, (int)hasAlram, hasAlram.ToString());

                Logger.LogInfoWrite(this.LogName, GetType().Name, MethodBase.GetCurrentMethod().Name + "()", strlog);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);

                if (eqp == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));


                lock (eqp) eqp.File.Alarm = hasAlram;


                CPCLocalAlarmStatus(eqp, inputData);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                // ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp); //不用记录History,有警报直接在界面显示

            }

            catch (Exception ex)
            {

                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }



        #endregion

        #region CIM Mode Change Command

       
        private void BCCIMModeChangeCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] CIM Mode Change Command TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// CPCCIMModeChangeCommandReply
        /// </summary>
        /// <param name="result"></param>
        /// <param name="trxID"></param>
        /// <param name="returnCode"></param>
        /// <param name="no"></param>
        private void CPCCIMModeChangeCommandReply(eBitResult result, string trxID, string returnCode, string no)
        {
            try
            {

                eipTagAccess.WriteItemValue("EQToCIM_Status_05_01_00", "CIM_Mode_Change_Command_Reply_Block", "ReturnCode", int.Parse(returnCode));
                eipTagAccess.WriteItemValue("EQToCIM_Status_05_01_00", "EAS Command Reply", "CIM_Mode_Change_Command_Reply", result == eBitResult.ON ? 1 : 0);


                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", "L3", CIMModeChangeCommandReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCCIMModeChangeCommandReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                         "L3", trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCCIMModeChangeCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] CIM Mode Change Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCCIMModeChangeCommandReply(eBitResult.OFF, trackKey, "0", "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        // TO EQ
        public void EQCIMModeChangeCommandReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCCIMModeChangeCommandReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[OFF] CIM Mode Change Command  Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //CPCCIMModeChangeCommand(eqp, inputData, eBitResult.OFF, "0");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQCIMModeChangeCommandReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] CIM Mode Change Command Reply BIT [{2}] Return Code[{3}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString(), inputData.EventGroups[0].Events[0].Items[0].Value));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCCIMModeChangeCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EQDCIMModeChangeCommand");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] EQD CIM Mode Change Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void EQCIMModeChangeCommandReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] CIM Mode Change Command Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        #endregion

        #region Operator Login Logout Report

        public void EQOperatorLoginLogoutReport(Trx inputData)
        {
            try
            {

                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                if (inputData.IsInitTrigger)
                {

                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, OperatorLoginLogoutReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[OFF] Operator Login Logout Report.",
                        eqpNo, inputData.TrackKey));
                    CPCOperatorLoginLogoutReportReply(eBitResult.OFF, inputData.TrackKey);

                    return;
                }
                if (inputData[0][0][2].Value == "1")
                {
                    eq.File.OprationName = inputData[0][0][0].Value.Trim();
                }
                else
                {
                    eq.File.OprationName = "";
                }
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCOperatorLoginLogoutReport(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQOperatorLoginLogoutReportTimeoutAction), inputData.TrackKey);
                }

                #endregion


                CPCOperatorLoginLogoutReportReply(eBitResult.ON, inputData.TrackKey);


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                   string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Operator Login Logout Report OperatorID=[{2}] TouchPannelNo=[{3}] Mode[{4}][{5}]. ",
                   eqpNo, inputData.TrackKey, inputData[0][0][0].Value, inputData[0][0][1].Value, inputData[0][0][2].Value,
                   inputData[0][0][2].Value == "1" ? "Login" : inputData[0][0][2].Value == "2" ? "Logout" : inputData[0][0][2].Value == "3" ? "Auto Logout" : ""
                   ));
            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        private void EQOperatorLoginLogoutReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Operator Login Logout Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCOperatorLoginLogoutReportReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDOperatorLoginLogoutReportReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, OperatorLoginLogoutReportReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCOperatorLoginLogoutReportReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCOperatorLoginLogoutReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_StoreGlassDataReportTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Operator Login Logout Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCOperatorLoginLogoutReportReply(eBitResult.OFF, trackKey);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        //TO BC 
        public void BCOperatorLoginLogoutReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCOperatorLoginLogoutReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Operator Login Logout Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCOperatorLoginLogoutReport(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCOperatorLoginLogoutReportReplyAction), inputData.TrackKey);
                }

                #endregion
                //string touchPanelNo = inputData[0][0][0].Value;
                //string returnCode = inputData[0][0][1].Value;
                //string msg = string.Empty;
                //if (returnCode == "0")
                //{
                //    msg = "user Does not exist";
                //}
                //else if (returnCode == "1")
                //{
                //    msg = "User Level 1";
                //}
                //else if (returnCode == "2")
                //{
                //    msg = "User Level 2";
                //}
                //else if (returnCode == "3")
                //{
                //    msg = "User Level 3";
                //}
                //else if (returnCode == "4")
                //{
                //    msg = "User Level 4";
                //}
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Operator Login Logout Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCOperatorLoginLogoutReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_OperatorLoginRequest");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Operator Login Logout Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCOperatorLoginLogoutReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Job Data Edit Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Equipment Run Mode Change Report

        public void EQEquipmentRunModeChangeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                lock (eqp)
                {
                    eqp.File.EquipmentRunMode = inputData[0][0][0].Value;
                }

                CPCEquipmentRunModeChangeReport(eqp, inputData, eBitResult.ON);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Equipment Run Mode=[{2}].",
                        eqpNo, inputData.TrackKey, eqp.File.EquipmentRunMode.ToString()));
            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        //TO BC 
        public void BCEquipmentRunModeChangeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeChangeReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Equipment Run Mode Change Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCEquipmentRunModeChangeReport(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentRunModeChangeReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Run Mode Change Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCEquipmentRunModeChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_UnitModeChangeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Run Mode Change Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCEquipmentRunModeChangeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Equipment Run Mode Change Report Reply TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Equipment Operation Mode Report  BC没有此功能

        public void EQEquipmentOperationModeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                lock (eqp)
                {
                    eqp.File.EquipmentOperationMode = (eEQPOperationMode)int.Parse(inputData[0][0][0].Value);
                }

                //  CPCEquipmentOperationModeReport(eqp, inputData, eBitResult.ON);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Equipment Operation Mode=[{2}].",
                        eqpNo, inputData.TrackKey, eqp.File.EquipmentOperationMode.ToString()));
            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        //TO BC 
        public void BCEquipmentOperationModeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, "EquipmentOperationModeReportReplyTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Equipment Operation Mode Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    //  CPCEquipmentOperationModeReport(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentOperationModeReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Operation Mode Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCEquipmentOperationModeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EquipmentOperationModeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Operation Mode Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCEquipmentOperationModeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Equipment Operation Mode Report Reply TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Equipment Job Count Change Report  不关联

        public void EQEquipmentJobCountChangeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                lock (eqp)
                {
                    eqp.File.TotalTFTGlassCount = int.Parse(inputData[0][0][1].Value);
                }

                CPCEquipmentJobCountChangeReport(eqp, inputData);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Equipment Job Count=[{2}].",
                        eqpNo, inputData.TrackKey, eqp.File.TotalTFTGlassCount.ToString()));
            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        #endregion

        #region Current Recipe ID Report

        public void EQCurrentRecipeIDReport(Trx inputData)
        {
            try
            {

                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                string recipeID = inputData[0][0]["CurrentRecipeID"].Value.Trim();

                eq.File.CurrentRecipeID = recipeID;

                string recipeNO = inputData[0][0]["CurrentRecipeNO"].Value.Trim();

                eq.File.CurrentRecipeNo = int.Parse(recipeNO);

                ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                CPCCurrentRecipeIDReport(eq, inputData, eBitResult.ON);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eq);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Current Recipe No Report  RECIPE_NO=[{2}] .",
                     eq.Data.NODENO, inputData.TrackKey, recipeNO));


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }
        // TO BC
        public void CurrentRecipeIDReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCCurrentRecipeIDReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Current Recipe Change Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCCurrentRecipeIDReport(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CurrentRecipeIDReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Current Recipe Change Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCCurrentRecipeIDReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_CurrentRecipeChangeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Current Recipe ID/NO Report T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CurrentRecipeIDReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Current Recipe ID/NO Report Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Job Data Check Mode

        /// <summary>
        /// JobDataCheckMode
        /// </summary>
        /// <param name="inputData"></param>
        public void EQJobDataCheckMode(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (eqp.File.LoadingStopMode != (eEnableDisable)int.Parse(inputData[0][0][8].Value))
                {
                    Trx trxLoadingStopRequest = GetTrxValues(string.Format("{0}_LoadingStopRequest", eqp.Data.NODENO));
                    trxLoadingStopRequest[0][0][0].Value = inputData[0][0][8].Value == "1" ? "1" : "2";
                    trxLoadingStopRequest[0][0][1].Value = "2";
                    trxLoadingStopRequest[0][1][0].Value = "1";
                    trxLoadingStopRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trxLoadingStopRequest);

                    string t1TimerID = "L3_LoadingStopRequestTimeout";
                    if (Timermanager.IsAliveTimer(t1TimerID))
                    {
                        Timermanager.TerminateTimer(t1TimerID);
                    }

                    Timermanager.CreateTimer(t1TimerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                   new System.Timers.ElapsedEventHandler(LoadingStopRequestTimeoutAction), inputData.TrackKey);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] BIT=[ON] Loading Stop Request Status=[{2}].",
                    eqpNo, inputData.TrackKey, trxLoadingStopRequest[0][0][0].Value));
                    if (inputData[0][0][8].Value == "1")
                    {

                        Trx trxInlineLoadingStopRequest = GetTrxValues(string.Format("{0}_InlineLoadingStopRequest", eqp.Data.NODENO));

                        if (trxInlineLoadingStopRequest == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_LoadingStop IN PLCFmt.xml!", eqp.Data.NODENO, eqp.Data.NODENO));
                        if (line.Data.FABTYPE != "CF")
                        {
                            trxInlineLoadingStopRequest[0][0][0].Value = "3";
                            trxInlineLoadingStopRequest[0][0][1].Value = "2";
                        }
                        else
                        {
                            if (line.Data.ATTRIBUTE == "CLN")
                            {
                                trxInlineLoadingStopRequest[0][0][0].Value = "10";
                                trxInlineLoadingStopRequest[0][0][1].Value = "4";
                            }
                            else if (line.Data.ATTRIBUTE == "DEV")
                            {
                                trxInlineLoadingStopRequest[0][0][0].Value = "16";
                                trxInlineLoadingStopRequest[0][0][1].Value = "7";
                            }
                        }
                        trxInlineLoadingStopRequest[0][1][0].Value = "1";
                        trxInlineLoadingStopRequest.TrackKey = inputData.TrackKey;
                        SendToPLC(trxInlineLoadingStopRequest);
                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] BIT=[ON] InLine Loading Stop Request.",
                        eqpNo, inputData.TrackKey));

                        string timerID1 = "L3_InlineLoadingStopRequestTimeout";
                        if (Timermanager.IsAliveTimer(timerID1))
                        {
                            Timermanager.TerminateTimer(timerID1);
                        }
                        Timermanager.CreateTimer(timerID1, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(InlineLoadingStopRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());

                        Trx trxTransferStopRequest = GetTrxValues(string.Format("{0}_TransferStopRequest", eqp.Data.NODENO));
                        if (line.Data.FABTYPE != "CF")
                        {
                            trxTransferStopRequest[0][0][0].Value = "3";
                        }
                        else
                        {
                            //if (line.Data.ATTRIBUTE == "CLN")
                            //{
                            //    trxTransferStopRequest[0][0][0].Value = "4";
                            //}
                            //else if (line.Data.ATTRIBUTE == "DEV")
                            //{
                            //    trxTransferStopRequest[0][0][0].Value = "13";
                            //}

                        }
                        if (line.Data.FABTYPE != "CF" && line.Data.FABTYPE != "CELL")
                        {
                            trxTransferStopRequest[0][1][0].Value = "1";
                            trxTransferStopRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                            SendToPLC(trxTransferStopRequest);

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] BIT=[ON] Transfer Stop Request.",
                            eqpNo, inputData.TrackKey));

                            string timerID2 = "L3_TransferStopRequestTimeout";
                            if (Timermanager.IsAliveTimer(timerID2))
                            {
                                Timermanager.TerminateTimer(timerID2);
                            }
                            Timermanager.CreateTimer(timerID2, false, ParameterManager[eParameterName.T1].GetInteger(),
                                new System.Timers.ElapsedEventHandler(TransferStopRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
                        }

                    }
                }

                lock (eqp)
                {
                    eqp.File.AutoRecipeChangeMode = (eEnableDisable)int.Parse(inputData[0][0][0].Value);
                    eqp.File.GlassCheckMode = (eEnableDisable)int.Parse(inputData[0][0][1].Value);
                    eqp.File.RecipeCheckMode = (eEnableDisable)int.Parse(inputData[0][0][2].Value);
                    eqp.File.ProductTypeCheckMode = (eEnableDisable)int.Parse(inputData[0][0][3].Value);
                    eqp.File.ProductIDCheckMode = (eEnableDisable)int.Parse(inputData[0][0][4].Value);
                    eqp.File.JobDuplicateCheckMode = (eEnableDisable)int.Parse(inputData[0][0][5].Value);
                    eqp.File.UpStreamPerMode = (eEnableDisable)int.Parse(inputData[0][0][6].Value);
                    eqp.File.DowmStreamPerMode = (eEnableDisable)int.Parse(inputData[0][0][7].Value);
                    eqp.File.LoadingStopMode = (eEnableDisable)int.Parse(inputData[0][0][8].Value);
                    if (line.Data.FABTYPE == "CELL")//eqp.Data.LINEID == "KWF23633L"
                    {
                        eqp.File.VCRMode = (eEnableDisable)int.Parse(inputData[0][0][9].Value);
                    }
                }
                Trx trx = GetTrxValues("L3_AutoRecipeChangeMode");
                trx[0][0][0].Value = ((int)eqp.File.AutoRecipeChangeMode).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                if (line.Data.FABTYPE == "CELL")//eqp.Data.LINEID == "KWF23633L"
                {
                    Trx trxVCR = GetTrxValues("L3_VCR1EnableMode");
                    trxVCR[0][0][0].Value = ((int)eqp.File.VCRMode).ToString();
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trxVCR);
                }

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}]  Glass Check Mode=[{2}] Recipe Check Mode=[{3}] Product Type Check Mode=[{4}] Auto Recipe Change Mode=[{5}] Product ID Check Mode=[{6}] Job Duplicate Check Mode=[{7}] UpStream Per Mode=[{8}] DowmStream Per Mode=[{9}] Loading Stop=[{10}] VCR Mode=[{11}].",
                        eqpNo, inputData.TrackKey, eqp.File.GlassCheckMode, eqp.File.RecipeCheckMode, eqp.File.ProductTypeCheckMode, eqp.File.AutoRecipeChangeMode, eqp.File.ProductIDCheckMode, eqp.File.JobDuplicateCheckMode, eqp.File.UpStreamPerMode, eqp.File.DowmStreamPerMode, eqp.File.LoadingStopMode, eqp.File.VCRMode));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Software Version Change Report

        public void EQSoftwareVersionChangeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                string day01 = inputData[0][0][0].Value.Trim().PadLeft(4, '0');
                string day02 = inputData[0][0][1].Value.Trim().PadLeft(2, '0');
                string day03 = inputData[0][0][2].Value.Trim().PadLeft(2, '0');
                string day04 = inputData[0][0][3].Value.Trim().PadLeft(2, '0');
                string day05 = inputData[0][0][4].Value.Trim().PadLeft(2, '0');
                string day06 = inputData[0][0][5].Value.Trim().PadLeft(2, '0');



                lock (eqp)
                {
                    eqp.File.PLCSoftwareVersion = day01 + day02 + day03 + day04 + day05 + day06;
                }
                if (inputData[0][1][0].Value == "1")
                {
                    CPCSoftwareVersionChangeReport(eqp, inputData, eBitResult.ON);
                }
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Software Version Change Report=[{2}].",
                        eqpNo, inputData.TrackKey, eqp.File.PLCSoftwareVersion));
            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        //TO BC 
        public void BCSoftwareVersionChangeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, SoftwareVersionChangeReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Software Version Change Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    CPCSoftwareVersionChangeReport(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCSoftwareVersionChangeReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Software Version Change Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCSoftwareVersionChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_SoftwareVersionChangeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Software Version Change Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCSoftwareVersionChangeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Software Version Change Report Reply TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region  Recipe ID Modify Report

        public void EQRecipeIDModifyReport(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[2].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, RecipeIDModifyReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[OFF] Recipe ID Modify Report.",
                        eqpNo, inputData.TrackKey));

                    CPCRecipeIDModifyReportReply(eBitResult.OFF, inputData.TrackKey);
                    return;
                }

                CPCRecipeIDModifyReportReply(eBitResult.ON, inputData.TrackKey);

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCRecipeIDModifyReport(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQRecipeIDModifyReportTimeoutAction), inputData.TrackKey);
                }

                #endregion
                string recipeName = string.Empty;

                recipeName = inputData[0][0]["RecipeName"].Value.Trim();


                string recipeID = inputData[0][0]["RecipeID"].Value.Trim();
                string flag = inputData[0][0]["ModifyFlag"].Value.Trim();
                string day01 = inputData[0][0]["Year"].Value.Trim().PadLeft(2, '0');
                string day02 = inputData[0][0]["Month"].Value.Trim().PadLeft(2, '0');
                string day03 = inputData[0][0]["Day"].Value.Trim().PadLeft(2, '0');
                string day04 = inputData[0][0]["Hour"].Value.Trim().PadLeft(2, '0');
                string day05 = inputData[0][0]["Minute"].Value.Trim().PadLeft(2, '0');
                string day06 = inputData[0][0]["Second"].Value.Trim().PadLeft(2, '0');
                string versioNo = day01 + day02 + day03 + day04 + day05 + day06;
                string recipeNo = inputData[0][0]["RecipeNO"].Value.Trim();
                string recipeStatus = inputData[0][0]["RecipeStatus"].Value.Trim();

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[ON]  Recipe ID Modify Report  RECIPE_No=[{2}]  Modify Flag=[{3}].",
                     eq.Data.NODENO, inputData.TrackKey, recipeNo, flag));



                if (flag == "1") //create
                {
                    RecipeEntityData recipe = new RecipeEntityData();
                    recipe.FABTYPE = "ARRAY";
                    recipe.LINETYPE = eq.Data.NODENAME;
                    recipe.LINEID = eq.Data.LINEID;
                    recipe.NODENO = eq.Data.NODENO;
                    recipe.RECIPENO = recipeNo;
                    recipe.RECIPEID = recipeID.Trim();
                    recipe.RECIPENAME = recipeName;
                    recipe.CREATETIME = DateTime.Now;
                    recipe.VERSIONNO = versioNo;
                    recipe.RECIPESTATUS = recipeStatus == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                    recipe.OPERATORID = "1";//eq.File.OprationName;
                    recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipeNo, recipeID, recipe.RECIPESTATUS.ToString(), versioNo);

                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat("{0}={1},", "Recipe_NO", recipeNo);
                    sb.AppendFormat("{0}={1},", "Recipe_ID", recipeID.Trim());
                    sb.AppendFormat("{0}={1},", "Recipe_State", recipe.RECIPESTATUS);
                    sb.AppendFormat("{0}={1},", "Recipe_Version", recipe.VERSIONNO);
                    sb.AppendFormat("{0}={1},", "Recipe_Name", recipeName);

                    string paraValus = GetRecipeValus(eq, inputData[0][1], sb);




                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipeByNo();

                    if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(recipeNo))
                    {
                        //移动原来的Recipe File到Log History文件夹下，并记录履历

                        ObjectManager.RecipeManager.MoveRecipeDataValuesToFile(recipeDic[eq.Data.LINEID][recipeNo].FILENAME);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipeDic[eq.Data.LINEID][recipeNo], eRecipeEvent.Delete, eq.File.OprationName);
                        ObjectManager.RecipeManager.DeleteRecipeObject(recipe.RECIPENO, eq.Data.NODENO);
                        ObjectManager.RecipeManager.SaveRecipeOject(recipe);
                    }
                    else
                    {
                        ObjectManager.RecipeManager.SaveRecipeOject(recipe);
                    }
                    ObjectManager.RecipeManager.MakeRecipeDataValuesToFile(recipe.FILENAME, paraValus);
                    ObjectManager.RecipeManager.SaveRecipeHistory(recipe, eRecipeEvent.Create, eq.File.OprationName);
                    //RecipeParameterReport(eq, null, recipe, flag);
                }
                else if (flag == "2")//modify
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipeByNo();
                    RecipeEntityData recipePater = new RecipeEntityData();
                    if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(recipeNo))
                    {
                        recipePater = recipeDic[eq.Data.LINEID][recipeNo];

                        RecipeEntityData recipe = new RecipeEntityData();
                        recipe.FABTYPE = "ARRAY";
                        recipe.LINETYPE = eq.Data.NODENAME;
                        recipe.LINEID = eq.Data.LINEID;
                        recipe.NODENO = eq.Data.NODENO;
                        recipe.RECIPENO = recipeNo;
                        recipe.RECIPEID = recipeID.Trim();
                        recipe.RECIPENAME = recipeName;
                        recipe.CREATETIME = DateTime.Now;
                        recipe.VERSIONNO = versioNo;
                        recipe.RECIPESTATUS = recipeStatus == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                        int operatorID = string.IsNullOrEmpty(recipePater?.OPERATORID) == true ? 0 : int.Parse(recipePater.OPERATORID);
                        if (operatorID < 65535)
                        {
                            operatorID++;
                        }
                        else
                        {
                            operatorID = 1;
                        }
                        recipe.OPERATORID = operatorID.ToString();//eq.File.OprationName;
                        recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipeNo, recipeID, recipe.RECIPESTATUS.ToString(), versioNo);

                        StringBuilder sb = new StringBuilder();

                        sb.AppendFormat("{0}={1},", "Recipe_NO", recipeNo);
                        sb.AppendFormat("{0}={1},", "Recipe_ID", recipeID.Trim());
                        sb.AppendFormat("{0}={1},", "Recipe_State", recipe.RECIPESTATUS);
                        sb.AppendFormat("{0}={1},", "Recipe_Version", recipe.VERSIONNO);
                        sb.AppendFormat("{0}={1},", "Recipe_Name", recipeName);

                        string paraValus = GetRecipeValus(eq, inputData[0][1], sb);

                        // string paraValus = GetRecipeValus(eq, inputData[0][1]);

                        // ObjectManager.RecipeManager.UpateRecipeObject(recipePater);
                        //移动原来的Recipe File到Log History文件夹下，并记录履历

                        ObjectManager.RecipeManager.MoveRecipeDataValuesToFile(recipePater.FILENAME);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipePater, eRecipeEvent.Delete, eq.File.OprationName);
                        ObjectManager.RecipeManager.DeleteRecipeObject(recipePater.RECIPENO, eq.Data.NODENO);
                        ObjectManager.RecipeManager.SaveRecipeOject(recipe);
                        ObjectManager.RecipeManager.MakeRecipeDataValuesToFile(recipe.FILENAME, paraValus);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipe, eRecipeEvent.Create, eq.File.OprationName);
                        //RecipeParameterReport(eq, null, recipe, flag);
                    }
                    else
                    {
                        RecipeEntityData recipe = new RecipeEntityData();
                        recipe.FABTYPE = "ARRAY";
                        recipe.LINETYPE = eq.Data.NODENAME;
                        recipe.LINEID = eq.Data.LINEID;
                        recipe.NODENO = eq.Data.NODENO;
                        recipe.RECIPENO = recipeNo;
                        recipe.RECIPEID = recipeID.Trim();
                        recipe.RECIPENAME = recipeName;
                        recipe.CREATETIME = DateTime.Now;
                        recipe.VERSIONNO = versioNo;
                        recipe.RECIPESTATUS = recipeStatus == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                        int operatorID = string.IsNullOrEmpty(recipePater?.OPERATORID) == true ? 0 : int.Parse(recipePater.OPERATORID);
                        if (operatorID < 65535)
                        {
                            operatorID++;
                        }
                        else
                        {
                            operatorID = 1;
                        }
                        recipe.OPERATORID = operatorID.ToString();//eq.File.OprationName;
                        recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipeNo, recipeID, recipe.RECIPESTATUS.ToString(), versioNo);

                        StringBuilder sb = new StringBuilder();

                        sb.AppendFormat("{0}={1},", "Recipe_NO", recipeNo);
                        sb.AppendFormat("{0}={1},", "Recipe_ID", recipeID.Trim());
                        sb.AppendFormat("{0}={1},", "Recipe_State", recipe.RECIPESTATUS);
                        sb.AppendFormat("{0}={1},", "Recipe_Version", recipe.VERSIONNO);
                        sb.AppendFormat("{0}={1},", "Recipe_Name", recipeName);

                        string paraValus = GetRecipeValus(eq, inputData[0][1], sb);

                        // string paraValus = GetRecipeValus(eq, inputData[0][1]);

                        ObjectManager.RecipeManager.UpateRecipeObject(recipe);
                        ObjectManager.RecipeManager.MakeRecipeDataValuesToFile(recipe.FILENAME, paraValus);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipe, eRecipeEvent.Create, eq.File.OprationName);

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON]  Recipe ID Modify Report  RECIPE_ID=[{2}] Not Have Modify Flag=[{3}] Error.",
                           eq.Data.NODENO, inputData.TrackKey, recipeID, flag));
                        //RecipeParameterReport(eq, null, recipe, flag);
                    }

                }
                else if (flag == "3")
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                       ObjectManager.RecipeManager.ReloadRecipeByNo();

                    if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(recipeNo))
                    {
                        //RecipeParameterReport(eq, null, recipeDic[eq.Data.LINEID][recipeNo], flag);
                        ObjectManager.RecipeManager.MoveRecipeDataValuesToFile(recipeDic[eq.Data.LINEID][recipeNo].FILENAME);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipeDic[eq.Data.LINEID][recipeNo], eRecipeEvent.Delete, eq.File.OprationName);
                        ObjectManager.RecipeManager.DeleteRecipeObject(recipeNo, eq.Data.NODENO);

                    }

                }
                else
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON]  Recipe ID Modify Report  RECIPE_ID=[{2}]  Modify Flag=[{3}] Error.",
                            eq.Data.NODENO, inputData.TrackKey, recipeID, flag));
                }
                CPCUnitRecipeListReport(flag);
                CurrentRecipeListReportType = flag;

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void EQRecipeIDModifyReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}]  Recipe ID Modify Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRecipeIDModifyReportReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDRecipeIDModifyReportReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, RecipeIDModifyReportReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeIDModifyReportReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCRecipeIDModifyReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Recipe ID Modify Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCRecipeIDModifyReportReply(eBitResult.OFF, trackKey);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        // TO BC 
        public void BCRecipeIDModifyReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCRecipeIDModifyReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Local Recipe Modified Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCRecipeIDModifyReport(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeIDModifyReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Local Recipe Modified Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRecipeIDModifyReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_LocalRecipeModifiedReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recipe ID Modify Report T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCRecipeIDModifyReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EQ][{1}]   Recipe ID Modify Report Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private string GetRecipeValus(Equipment eq, Event ev, StringBuilder sb)
        {

            try
            {
                if (eq.Data.NODENAME == "CLEANER")
                {
                    for (int i = 0; i <= ev.Items.AllKeys.Length - 1; i++)
                    {
                        string name = ev[i].Name.Trim();
                        string valus = ev[i].Value.Trim();
                        //添加倍率的转换
                        switch (name)
                        {
                            #region
                            //case "Recipe_M06_Up_Brush1_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;
                            //case "Recipe_M06_Down_Brush1_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;
                            //case "Recipe_M06_Up_Brush2_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;
                            //case "Recipe_M06_Down_Brush2_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;
                            //case "Recipe_M06_Up_Brush3_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;
                            //case "Recipe_M06_Down_Brush3_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;
                            //case "Recipe_M06_Up_Brush4_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;
                            //case "Recipe_M06_Down_Brush4_Position":

                            //    valus = (int.Parse(valus) / 10).ToString();
                            //    break;



                            default:

                                break;

                                #endregion
                        }


                        sb.AppendFormat("{0}={1},", name, valus);
                    }

                }
                else
                {
                    for (int i = 0; i <= ev.Items.AllKeys.Length - 1; i++)
                    {
                        string name = ev[i].Name.Trim();
                        string valus = ev[i].Value.Trim();
                        int rate = 1;
                        //添加倍率的转换
                        if (name.Contains('/'))
                        {
                            if (name.Split('/').Length == 2)
                            {
                                rate = int.Parse(name.Split('/')[1]);
                                double real = double.Parse(valus) / rate;
                                valus = real.ToString();
                            }
                        }


                        sb.AppendFormat("{0}={1},", name, valus);
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return sb.ToString();
            }
        }

        public bool CheckIsRecipeParametersChange(Equipment eq, Trx inputData, RecipeEntityData recipe)
        {
            bool res = false;
            StringBuilder sb = new StringBuilder();

            //sb.AppendFormat("{0}={1},", "Recipe_NO", recipe.RECIPENO);
            //sb.AppendFormat("{0}={1},", "Recipe_ID", recipe.RECIPEID.Trim());
            //sb.AppendFormat("{0}={1},", "Recipe_State", recipe.RECIPESTATUS);
            //sb.AppendFormat("{0}={1},", "Recipe_Version", recipe.VERSIONNO);
            string[] currentParametrs = GetRecipeValus(eq, inputData[0][1], sb).Split(',');

            IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipe.FILENAME);
            int j = 0;
            for (int i = 0; i < paramter.Count; i++)
            {
                if (paramter[i].Contains("Recipe_NO"))
                {
                    continue;
                }
                if (paramter[i].Contains("Recipe_ID"))
                {
                    continue;
                }
                if (paramter[i].Contains("Recipe_State"))
                {
                    continue;
                }
                if (paramter[i].Contains("Recipe_Version"))
                {
                    continue;
                }
                if (paramter[i] == currentParametrs[j])
                {

                    j++;
                }
                else
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        #endregion

        #region Date Time Calibration Command

        public void BCDateTimeCalibrationCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }


                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, DateTimeCalibrationCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  Date Time Calibration Command.",
                        eqpNo, inputData.TrackKey));

                    CPCDateTimeCalibrationCommandReply(eBitResult.OFF, inputData.TrackKey);

                    return;
                }

                CPCDateTimeCalibrationCommandReply(eBitResult.ON, inputData.TrackKey);

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                string msg = string.Empty;
                string time = inputData[0][0][0].Value.PadLeft(4, '0') + inputData[0][0][1].Value.PadLeft(2, '0') + inputData[0][0][2].Value.PadLeft(2, '0') + inputData[0][0][3].Value.PadLeft(2, '0') + inputData[0][0][4].Value.PadLeft(2, '0') + inputData[0][0][5].Value.PadLeft(2, '0');
                if (bitResult == eBitResult.ON)
                {
                    if (CheckDatetimeValid(time, out msg))
                    {
                        CPCDateTimeCalibrationCommand(eq, inputData, eBitResult.ON);

                        SetPCSystemTime(time);
                    }

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCDateTimeCalibrationCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion





                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Date Time Calibration Command DateTime=[{2}] {3}.",
                     eq.Data.NODENO, inputData.TrackKey, time, msg));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCDateTimeCalibrationCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] Date Time Calibration Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCDateTimeCalibrationCommandReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_DateTimeCalibrationCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, DateTimeCalibrationCommandReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCDateTimeCalibrationCommandReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCDateTimeCalibrationCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Date Time Calibration Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCDateTimeCalibrationCommandReply(eBitResult.OFF, trackKey);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        // TO BC 
        public void EQDateTimeCalibrationCommandReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCDateTimeCalibrationCommandReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[OFF] Date Time Calibration Command  Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCDateTimeCalibrationCommand(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQDateTimeCalibrationCommandReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Date Time Calibration Command Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCDateTimeCalibrationCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EQDDateTimeCalibrationCommand");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] DateTime Calibration Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void EQDateTimeCalibrationCommandReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Date Time Calibration Command Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Equipment Run Mode Set Command

        public void BCEquipmentRunModeSetCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeSetCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Equipment Run Mode Set Command.",
                        eqpNo, inputData.TrackKey));

                    CPCEquipmentRunModeSetCommandReply(eBitResult.OFF, inputData, "0");

                    return;
                }
                string msg = string.Empty;
                string pauseCommand = inputData[0][0]["UnitMode"].Value.Trim();

                if (pauseCommand == "1" || pauseCommand == "2")//目前map种只定义1~4,设备只有1，2两种模式，后续有其他再继续添加。
                {
                    if (eq.File.EquipmentRunMode != pauseCommand)
                    {
                        if (eq.File.Status == eEQPStatus.Idle || eq.File.Status == eEQPStatus.Initial)
                        {
                            CPCEquipmentRunModeSetCommand(eq, inputData, eBitResult.ON);

                            CPCEquipmentRunModeSetCommandReply(eBitResult.ON, inputData, "1");
                        }
                        else
                        {
                            CPCEquipmentRunModeSetCommandReply(eBitResult.ON, inputData, "4");//非IDLE和PM状态不能切模式
                        }


                    }
                    else
                    {
                        CPCEquipmentRunModeSetCommandReply(eBitResult.ON, inputData, "2");
                    }

                }
                else
                {
                    CPCEquipmentRunModeSetCommandReply(eBitResult.ON, inputData, "3");
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentRunModeSetCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion
                if (pauseCommand == "1")
                {
                    msg = "Normal Mode";
                }
                else if (pauseCommand == "2")
                {
                    msg = "Force Clean Out Mode";
                }
                else if (pauseCommand == "3")
                {
                    msg = "Skip Mode";
                }
                else if (pauseCommand == "4")
                {
                    msg = "Cold Run Mode";
                }
                else
                {
                    msg = "Other Mode";
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Equipment Run Mode Set Command RUN Mode=[{2}][{3}].",
                     eq.Data.NODENO, inputData.TrackKey, pauseCommand, msg));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCEquipmentRunModeSetCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Equipment Run Mode Set Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCEquipmentRunModeSetCommandReply(eBitResult result, Trx trxID, string returnCode)
        {
            try
            {
                Trx trx = null;
                trx = GetTrxValues(string.Format("L3_RunModeChangeCommandReply"));
                // trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_RunModeChangeCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;
                if (trxID != null)
                {
                    trx[0][0][0].Value = returnCode;
                    trx[0][1][0].Value = ((int)result).ToString();
                    trx.TrackKey = trxID.TrackKey;

                }
                else
                {
                    trx[0][1][0].Value = ((int)result).ToString();
                }
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeSetCommandReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCEquipmentRunModeSetCommandReplyTimeoutAction), trxID.TrackKey);
                }
                string msg = string.Empty;
                if (returnCode == "1")
                {
                    msg = "Accept";
                }
                else if (returnCode == "2")
                {
                    msg = "Already this mode";
                }
                else if (returnCode == "3")
                {
                    msg = "Machine have no this mode";
                }
                else if (returnCode == "4")
                {
                    msg = "Other Error";
                }
                else if (returnCode == "0")
                {
                    msg = "Reset";
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] SET BIT=[{2}] ReturnCode[{3}][{4}].",
                        eqpNo, trx.TrackKey, result.ToString(), returnCode, msg));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCEquipmentRunModeSetCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Equipment Run Mode Set Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCEquipmentRunModeSetCommandReply(eBitResult.OFF, null, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        // TO EQ 
        public void EQEquipmentRunModeSetCommandReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCEquipmentRunModeSetCommandReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[OFF] Equipment Run Mode Set Command Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCEquipmentRunModeSetCommand(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQEquipmentRunModeSetCommandReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Equipment Run Mode Set Command Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCEquipmentRunModeSetCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EQDEquipmentRunModeSetCommand");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Equipment Run Mode Set Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void EQEquipmentRunModeSetCommandReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Run Mode Set Command Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region  Unit Mode Change Possible Request 


        public void BCRunModePossibleRequest(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, RunModePossibleRequestTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Run Mode Possible Request.",
                        eqpNo, inputData.TrackKey));

                    CPCRunModePossibleRequestReply(eBitResult.OFF, inputData, "0");

                    return;
                }

                string pauseCommand = inputData[0][0]["UnitMode"].Value.Trim();

                if ((pauseCommand == "1" || pauseCommand == "2" || pauseCommand == "3" || pauseCommand == "4") && eq.File.EquipmentRunMode != pauseCommand)
                {
                    if (eq.Data.LINEID == "KWF22090R" || eq.Data.LINEID == "KWF22091R")
                    {
                        if (eq.File.Status == eEQPStatus.Idle || eq.File.Status == eEQPStatus.Initial)
                        {
                            CPCRunModePossibleRequestReply(eBitResult.ON, inputData, "1");
                        }
                        else
                        {
                            CPCRunModePossibleRequestReply(eBitResult.ON, inputData, "2");
                        }
                    }
                    else
                    {
                        if (pauseCommand == "3" || pauseCommand == "4")
                        {
                            CPCRunModePossibleRequestReply(eBitResult.ON, inputData, "2");
                        }
                        else
                        {
                            CPCRunModePossibleRequestReply(eBitResult.ON, inputData, "1");
                        }

                    }

                }
                else
                {
                    CPCRunModePossibleRequestReply(eBitResult.ON, inputData, "2");

                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRunModePossibleRequestTimeoutAction), inputData.TrackKey);
                }

                #endregion

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Run Mode Possible Request  RUN Mode=[{2}].",
                     eq.Data.NODENO, inputData.TrackKey, pauseCommand));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCRunModePossibleRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Run Mode Possible Request TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRunModePossibleRequestReply(eBitResult result, Trx trxID, string returnCode)
        {
            try
            {
                Trx trx = null;

                //trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_RunModePossibleRequestReply") as Trx;
                trx = GetTrxValues(string.Format("L3_RunModeChangePossibleRequestReply"));

                string eqpNo = trx.Metadata.NodeNo;
                if (trxID != null)
                {
                    trx[0][0][0].Value = returnCode;
                    trx[0][0][1].Value = trxID[0][0][1].Value.Trim();
                    trx[0][1][0].Value = ((int)result).ToString();
                    trx.TrackKey = trxID.TrackKey;

                }
                else
                {
                    trx[0][1][0].Value = ((int)result).ToString();
                }
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, RunModePossibleRequestReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRunModePossibleRequestReplyTimeoutAction), trxID.TrackKey);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, trx.TrackKey, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCRunModePossibleRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Run Mode Possible Request Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCRunModePossibleRequestReply(eBitResult.OFF, null, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion


        #region Job Data Request

        public void EQJobDataRequest(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, JobDataRequestTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[OFF] Job Data Request .",
                        eqpNo, inputData.TrackKey));

                    CPCJobDataRequestReply(eBitResult.OFF, null);

                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCJobDataRequest(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQJobDataRequestTimeoutAction), inputData.TrackKey);
                }

                #endregion


                string glassID = inputData[0][0]["GlassID"].Value.Trim();

                Job job = ObjectManager.JobManager.GetJob(glassID);
                if (job != null)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                      string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Job Data Request GLASS_ID=[{2}] , But Job Data Exsit.",
                                      eq.Data.NODENO, inputData.TrackKey, glassID));

                }
                else
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                                       string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Job Data Request GLASS_ID=[{2}] .",
                                       eq.Data.NODENO, inputData.TrackKey, glassID));
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void EQJobDataRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Job Data Request TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCJobDataRequestReply(eBitResult result, Trx inputData)
        {
            try
            {
                Trx trx = null;

                trx = GetTrxValues("L3_EQDJobDataRequestReply");
                string eqpNo = trx.Metadata.NodeNo;

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, JobDataRequestReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // // trx.ClearTrxWith0();
                    trx[0][2][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Job Data Request Reply.",
                            eqpNo, trx.TrackKey));

                    return;
                }

                for (int i = 0; i < inputData[0].Events.AllValues.Length; i++)
                {
                    for (int j = 0; j < inputData[0][i].Items.AllValues.Length; j++)
                    {
                        trx[0][i][j].Value = inputData[0][i][j].Value.Trim();
                    }

                }

                if (inputData[0][1][0].Value.Trim() != "1")
                {
                    trx[0][1][0].Value = "2";
                }
                else
                {
                    trx[0][1][0].Value = "1";
                }
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    _recipeEntitys = ObjectManager.RecipeManager.ReloadRecipeByNo();
                }
                else
                {
                    _recipeEntitys = ObjectManager.RecipeManager.ReloadRecipe();
                }

                if (_recipeEntitys != null && _recipeEntitys.Count > 0)
                {
                    if (_recipeEntitys[eq.Data.LINEID] != null && _recipeEntitys[eq.Data.LINEID].Count > 0)
                    {
                        string PPID = string.Empty;
                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            PPID = inputData[0][0]["PPID"].Value.Substring(26, 26).Trim();
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (line.Data.ATTRIBUTE == "CLN")
                            {
                                PPID = inputData[0][0]["PPID"].Value.Substring(28, 4).Trim();
                            }
                            else if (line.Data.ATTRIBUTE == "DEV")
                            {
                                PPID = inputData[0][0]["PPID"].Value.Substring(52, 4).Trim();
                            }
                        }
                        else if (line.Data.FABTYPE == "CELL")
                        {
                            PPID = inputData[0][0]["PPID03"].Value.Trim();
                        }

                        if (_recipeEntitys[eq.Data.LINEID].ContainsKey(PPID))
                        {
                            trx[0][0]["CurrentRecipe"].Value = _recipeEntitys[eq.Data.LINEID][PPID].RECIPENO;
                        }
                    }

                }

                trx[0][2][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);


                if (result == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCJobDataRequestReplyTimeoutAction), inputData.TrackKey);
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  SET BIT=[{2}].",
                        eqpNo, inputData.TrackKey, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCJobDataRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_StoreGlassDataReportTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Job Data Request Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCJobDataRequestReply(eBitResult.OFF, null);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCJobDataRequestReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[2].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCJobDataRequestReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Job Data Request Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCJobDataRequestReply(eBitResult.ON, inputData);

                    CPCJobDataRequest(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCJobDataRequestReplyAction), inputData.TrackKey);
                }

                #endregion

                eReturnCode1 returncode = (eReturnCode1)int.Parse(inputData[0][1][0].Value);

                string cstSeq = inputData[0][0][eJOBDATA.Cassette_Sequence_No].Value;
                string slotNo = inputData[0][0][eJOBDATA.Job_Sequence_No].Value;
                string glassID = inputData[0][0][eJOBDATA.GlassID_or_PanelID].Value.Trim();

                Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                if (returncode == eReturnCode1.OK)
                {

                    if (job != null)
                    {
                        UpdateJobData(eqp, job, inputData);

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                          string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Job Data Request Reply CST_SEQNO=[{2}] JOB_SEQNO=[{3}] GLASS_ID=[{4}] Return Code=[{5}], Job Data Exsit.",
                                          eqp.Data.NODENO, inputData.TrackKey, cstSeq, slotNo, glassID, returncode.ToString()));

                        ObjectManager.JobManager.EnqueueSave(job);
                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Request.ToString(), eqp.Data.NODEID, eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");


                    }
                    else
                    {
                        job = CreateJob(cstSeq, slotNo, inputData);
                        UpdateJobData(eqp, job, inputData);

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                          string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Job Data Request Reply CST_SEQNO=[{2}] JOB_SEQNO=[{3}] GLASS_ID=[{4}] Return Code=[{5}].",
                                          eqp.Data.NODENO, inputData.TrackKey, cstSeq, slotNo, glassID, returncode.ToString()));

                        ObjectManager.JobManager.EnqueueSave(job);
                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Request.ToString(), eqp.Data.NODEID, eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");
                    }

                }
                else
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Job Data Request Reply CST_SEQNO=[{2}] JOB_SEQNO=[{3}] GLASS_ID=[{4}] Return Code=[{5}].",
                                            eqp.Data.NODENO, inputData.TrackKey, cstSeq, slotNo, glassID, returncode.ToString()));


                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCJobDataRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_JobDataRequest");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                Trx replyTrx = GetTrxValues("L3_EQDJobDataRequestReply");
                //replyTrx.ClearTrxWith0();
                replyTrx[0][1][0].Value = "2";
                replyTrx[0][2][0].Value = "0";
                //SendToPLC(replyTrx);

                CPCJobDataRequestReply(eBitResult.ON, replyTrx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Data Request T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCJobDataRequestReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Job Data Request Reply TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Message Set Command

        public void BCMessageDisplayCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, MessageDisplayCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC >- EC][{1}] BIT=[OFF] Message Display Command.",
                        eqpNo, inputData.TrackKey));

                    CPCMessageDisplayCommandReply(eBitResult.OFF, inputData.TrackKey);

                    return;
                }

                CPCMessageDisplayCommandReply(eBitResult.ON, inputData.TrackKey);

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //CPCMessageDisplayCommand(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCMessageDisplayCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion

                string cimMessageID = inputData[0][0]["CIMMessageID"].Value.Trim();
                string cIMMessage = inputData[0][0]["CIMMessage"].Value.Trim();
                eq.File.TouchPanelNo = inputData[0][0]["TouchPanelNo"].Value;

                CIMMESSAGEHISTORY cimMessage = new CIMMESSAGEHISTORY();
                cimMessage.NODEID = eq.Data.NODEID;
                cimMessage.NODENO = eq.Data.NODENO;
                cimMessage.OPERATORID = "BC";
                cimMessage.MESSAGETEXT = cIMMessage;
                cimMessage.UPDATETIME = DateTime.Now;
                cimMessage.MESSAGEID = cimMessageID;
                cimMessage.MESSAGESTATUS = "set";


                ObjectManager.EquipmentManager.SaveCIMMessageHistory(cimMessage);


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Message Display Command CIM Message ID=[{2}] CIMMessage=[{3}]  .",
                     eq.Data.NODENO, inputData.TrackKey, cimMessageID, cIMMessage));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCMessageDisplayCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Message Display Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCMessageDisplayCommandReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_CIMMessageSetCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, MessageDisplayCommandReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }


                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCMessageDisplayCommandReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCMessageDisplayCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Message Display Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCMessageDisplayCommandReply(eBitResult.OFF, UtilityMethod.GetAgentTrackKey());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }



        #endregion

        #region Message Clear Command

        public void BCCIMMessageClearCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, CIMMessageClearCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC >- EC][{1}] BIT=[OFF] CIM Message Clear Command.",
                        eqpNo, inputData.TrackKey));

                    CPCCIMMessageClearCommandReply(eBitResult.OFF, inputData.TrackKey);

                    return;
                }

                CPCCIMMessageClearCommandReply(eBitResult.ON, inputData.TrackKey);

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //CPCMessageDisplayCommand(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCCIMMessageClearCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion

                string cimMessageID = inputData[0][0]["CIMMessageID"].Value.Trim();


                ObjectManager.EquipmentManager.UpdateCIMMessage(cimMessageID, "clear");


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Message Clear Command CIM Message ID=[{2}].",
                     eq.Data.NODENO, inputData.TrackKey, cimMessageID));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCCIMMessageClearCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CIM Message Clear Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCCIMMessageClearCommandReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_CIMMessageClearCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, CIMMessageClearCommandReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }


                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCCIMMessageClearCommandReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCCIMMessageClearCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] CIM Message Clear Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCCIMMessageClearCommandReply(eBitResult.OFF, UtilityMethod.GetAgentTrackKey());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region CIM Message Confirm Report 

        public void CPCCIMMessageConfirmReport(string mesgID, eBitResult result)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("L3_CIMMessageConfirmReport")) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_CIMMessageConfirmReport IN PLCFmt.xml!", "L3"));

                string timerID = string.Format("{0}_{1}", "L3", CIMMessageConfirmReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] CIM Message Confirm Report.",
                            "L3", trx.TrackKey));

                    return;
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(BCCIMMessageConfirmReportTimeoutAction), trx.TrackKey);

                trx[0][0][0].Value = mesgID;
                trx[0][0][1].Value = eq.File.TouchPanelNo;

                trx[0][1][0].Value = "1";

                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] CIM Message Confirm Report Message ID=[{2}].", trx.Metadata.NodeNo, trx.TrackKey, mesgID));


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        private void BCCIMMessageConfirmReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] CIM Message Confirm Report TIMEOUT.", sArray[0], trackKey));

                CPCCIMMessageConfirmReport("0", eBitResult.OFF);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCCIMMessageConfirmReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CIMMessageConfirmReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] CIM Message Confirm Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    CPCCIMMessageConfirmReport("0", eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCCIMMessageConfirmReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] CIM Message Confirm Report Reply[{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCCIMMessageConfirmReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CIM Message Confirm Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        #endregion

        #region Job Hold Event Report

        public void EQJobHoldEventReport(Trx inputData)
        {
            try
            {

                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                if (inputData.IsInitTrigger)
                {

                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, JobHoldEventReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[OFF] Job Hold Event Report.",
                        eqpNo, inputData.TrackKey));
                    CPCJobHoldEventReportReply(eBitResult.OFF, inputData.TrackKey);

                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //   CPCJobHoldEventReport(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQJobHoldEventReportTimeoutAction), inputData.TrackKey);
                }

                #endregion


                CPCJobHoldEventReportReply(eBitResult.ON, inputData.TrackKey);


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                   string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Job Hold Event Report . ", eqpNo, inputData.TrackKey));
            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        private void EQJobHoldEventReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Job Hold Event Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCJobHoldEventReportReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDJobHoldEventReportReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, JobHoldEventReportReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCJobHoldEventReportReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCJobHoldEventReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_StoreGlassDataReportTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Job Hold Event Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCJobHoldEventReportReply(eBitResult.OFF, trackKey);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        //TO BC 

        #endregion

        #region Recipe State Change Command

        public void BCRecipeStateChangeCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, RecipeStateChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe State Change Command.",
                        eqpNo, inputData.TrackKey));

                    CPCRecipeStateChangeCommandReply(eBitResult.OFF, inputData, "0");

                    return;
                }

                string recipeID = inputData[0][0]["RecipeID"].Value.Trim();
                string recipeNO = inputData[0][0]["RecipeNo"].Value.Trim();
                string recipeType = inputData[0][0]["RecipeType"].Value.Trim();
                string RecipeState = inputData[0][0]["RecipeState"].Value.Trim();

                string oldFileName = string.Empty;

                RecipeEntityData recipe = new RecipeEntityData();

                if (recipeType == "1")
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                       ObjectManager.RecipeManager.ReloadRecipeByNo();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeNO))
                    {
                        recipe = recipeDic[eq.Data.LINEID][recipeNO];

                        oldFileName = string.Format("{0}_{1}_{2}_{3}", recipe.RECIPENO, recipe.RECIPEID, recipe.RECIPESTATUS.ToString(), recipe.VERSIONNO);

                        recipe.RECIPESTATUS = RecipeState == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                        recipe.OPERATORID = eq.File.OprationName;
                        recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipe.RECIPENO, recipe.RECIPEID, recipe.RECIPESTATUS.ToString(), recipe.VERSIONNO);


                        //ObjectManager.RecipeManager.UpateRecipeObject(recipe);

                        CPCRecipeStateChangeCommand(eq, inputData, eBitResult.ON);

                        CPCRecipeStateChangeCommandReply(eBitResult.ON, inputData, "1");

                    }
                    else
                    {

                        CPCRecipeStateChangeCommandReply(eBitResult.ON, inputData, "2");
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                      string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Table Not Exsit Recipe No[{2}].",
                      eqpNo, inputData.TrackKey, recipeNO));
                    }
                }
                else if (recipeType == "2")
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                       ObjectManager.RecipeManager.ReloadRecipe();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeID))
                    {
                        recipe = recipeDic[eq.Data.LINEID][recipeID];
                        oldFileName = string.Format("{0}_{1}_{2}_{3}", recipe.RECIPENO, recipe.RECIPEID, recipe.RECIPESTATUS.ToString(), recipe.VERSIONNO);

                        recipe.RECIPESTATUS = RecipeState == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                        recipe.OPERATORID = eq.File.OprationName;
                        recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipe.RECIPENO, recipe.RECIPEID, recipe.RECIPESTATUS.ToString(), recipe.VERSIONNO);

                        //ObjectManager.RecipeManager.UpateRecipeObject(recipe);

                        CPCRecipeStateChangeCommand(eq, inputData, eBitResult.ON);

                        CPCRecipeStateChangeCommandReply(eBitResult.ON, inputData, "1");

                    }
                    else
                    {

                        CPCRecipeStateChangeCommandReply(eBitResult.ON, inputData, "2");
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                      string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Table Not Exsit Recipe ID[{2}].",
                      eqpNo, inputData.TrackKey, recipeID));
                    }

                }
                else
                {
                    CPCRecipeStateChangeCommandReply(eBitResult.ON, inputData, "2");
                    LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                  string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] bC Send Recipe Flag[{2}] Error .",
                  eqpNo, inputData.TrackKey, recipeType));

                }

                string recipeParameters = File.ReadAllText("..\\Data\\" + $"{eq.Data.LINEID}\\RecipeManager\\" + oldFileName + ".txt");
                string[] recipeParametersArray = recipeParameters.Split(',');
                for (int i = 0; i < recipeParametersArray.Length; i++)
                {
                    if (recipeParametersArray[i].Contains("Recipe_State"))
                    {
                        recipeParametersArray[i] = recipeParametersArray[i].Substring(0, 13) + recipe.RECIPESTATUS.ToString();
                    }
                }
                string str = string.Empty;
                for (int i = 0; i < recipeParametersArray.Length; i++)
                {
                    str += recipeParametersArray[i] + ",";
                }
                ObjectManager.RecipeManager.MoveRecipeDataValuesToFile(oldFileName);
                ObjectManager.RecipeManager.MakeRecipeDataValuesToFile(recipe.FILENAME, str);
                ObjectManager.RecipeManager.UpateRecipeObject(recipe);

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeStateChangeCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion



                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe State Change Command .",
                     eq.Data.NODENO, inputData.TrackKey));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCRecipeStateChangeCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Recipe State Change Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRecipeStateChangeCommandReply(eBitResult result, Trx inputData, string returnCode)
        {
            try
            {
                Trx trx = null;

                // trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_RecipeStateChangeCommandReply") as Trx;

                trx = GetTrxValues(string.Format("L3_RecipeStateChangeCommandReply"));
                string eqpNo = trx.Metadata.NodeNo;

                trx.ClearTrxWith0();
                if (inputData != null)
                {
                    trx[0][0][0].Value = inputData[0][0][0].Value;
                    trx[0][0][1].Value = inputData[0][0][1].Value;
                    trx[0][0][2].Value = inputData[0][0][2].Value;
                    trx[0][0][3].Value = inputData[0][0][3].Value;
                    trx[0][0][4].Value = inputData[0][0][4].Value;
                    if (returnCode != "0")
                    {
                        trx[0][0][5].Value = returnCode;
                    }
                }
                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey(); ;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, RecipeStateChangeCommandReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeStateChangeCommandReplyTimeoutAction), inputData.TrackKey);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, trx.TrackKey, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCRecipeStateChangeCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Recipe State Change Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCRecipeStateChangeCommandReply(eBitResult.OFF, null, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        // TO EQ 
        public void EQRecipeStateChangeCommandReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCRecipeStateChangeCommandReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[OFF] Recipe State Change Command Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCRecipeStateChangeCommand(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQRecipeStateChangeCommandReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Recipe State Change Command  Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRecipeStateChangeCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EQDRecipeStateChangeCommand");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Recipe State Change Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void EQRecipeStateChangeCommandReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe State Change Command Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region  Alarm Status Change Report

        public void EQAlarmReport(Trx inputData)
        {
            try
            {
                #region[判断设备是否存在]
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                string[] trxArray = inputData.Name.Split('#');
                if (eqp == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                #endregion

                List<string> alarms = new List<string>();
                List<string> newAlarms = new List<string>();
                List<string> clearAlarms = new List<string>();
                //找出当前所有的Alarm存入到Alrams中
                foreach (Item item in inputData.EventGroups[0].Events[0].Items.AllValues)
                {
                    if (item.Value == "1")
                    {
                        alarms.Add(item.Name);
                    }
                }
                //与上一笔的alarm比较不存在的即为新增的Alarm,存入New Alarm 中
                foreach (string alarmID in alarms)
                {
                    if (!oldAlarms.Contains(alarmID))
                    {
                        newAlarms.Add(alarmID);
                    }
                }
                //找出所有清楚的Alarm,存入到ClearAlarms中
                if (oldAlarms != null && oldAlarms.Count != 0)
                {
                    foreach (string alarmID in oldAlarms)
                    {
                        if (!alarms.Contains(alarmID))
                        {
                            clearAlarms.Add(alarmID);
                        }

                    }
                }

                // 将当前的Alrams 赋予上一笔OldAlarm，等待与下一笔作比较
                oldAlarms = alarms;



                //上报alarm Clear
                if (clearAlarms != null && clearAlarms.Count != 0)
                {
                    List<AlarmEntityData> alarmList = new List<AlarmEntityData>();
                    foreach (string alarmID in clearAlarms)
                    {
                        AlarmEntityData alarm = ObjectManager.AlarmManager.GetAlarmProfile(eqpNo, alarmID);
                        if (alarm != null && !alarmDic.ContainsKey(alarm))
                        {
                            alarmList.Add(alarm);
                            alarmDic.Add(alarm, "0");
                        }

                    }

                    if (alarmList != null && alarmList.Count != 0)
                    {
                        List<AlarmHistory> alarmHislist = new List<AlarmHistory>();
                        foreach (AlarmEntityData alarm in alarmList)
                        {

                            AlarmHistory his = new AlarmHistory();
                            his.EVENTNAME = "Alarm";
                            his.UPDATETIME = DateTime.Now;
                            his.ALARMID = alarm.BCALARMID.ToString();
                            his.ALARMCODE = alarm.ALARMCODE;
                            his.ALARMLEVEL = alarm.ALARMLEVEL;
                            his.ALARMTEXT = alarm.ALARMTEXT;
                            his.NODEID = alarm.NODENO;
                            his.ALARMUNIT = alarm.UNITNO;
                            his.ALARMSTATUS = "CLEAR";
                            his.ALARMADDRESS = alarm.ALARMID;

                            alarmHislist.Add(his);

                            Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Alarm Report Clear AlarmID[{2}]  AlarmText[{3}] ", eqpNo, inputData.TrackKey, his.ALARMID, his.ALARMTEXT));
                        }
                        ObjectManager.AlarmManager.SaveAlarmHistory(alarmHislist);

                    }

                }


                //上报alarm Set 
                if (newAlarms != null && newAlarms.Count != 0)
                {
                    List<AlarmEntityData> alarmList = new List<AlarmEntityData>();
                    foreach (string alarmID in newAlarms)
                    {
                        AlarmEntityData alarm = ObjectManager.AlarmManager.GetAlarmProfile(eqpNo, alarmID);
                        if (alarm != null && !alarmDic.ContainsKey(alarm))
                        {
                            alarmList.Add(alarm);
                            alarmDic.Add(alarm, "1");
                        }

                    }

                    if (alarmList != null && alarmList.Count != 0)
                    {
                        List<AlarmHistory> alarmHislist = new List<AlarmHistory>();
                        foreach (AlarmEntityData alarm in alarmList)
                        {

                            AlarmHistory his = new AlarmHistory();
                            his.EVENTNAME = "Alarm";
                            his.UPDATETIME = DateTime.Now;
                            his.ALARMID = alarm.BCALARMID.ToString();
                            his.ALARMCODE = alarm.ALARMCODE;
                            his.ALARMLEVEL = alarm.ALARMLEVEL;
                            his.ALARMTEXT = alarm.ALARMTEXT;
                            his.NODEID = alarm.NODENO;
                            his.ALARMUNIT = alarm.UNITNO;
                            his.ALARMSTATUS = "SET";
                            his.ALARMADDRESS = alarm.ALARMID;

                            alarmHislist.Add(his);

                            Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", eqpNo, inputData.TrackKey, his.ALARMID, his.ALARMTEXT));
                        }
                        ObjectManager.AlarmManager.SaveAlarmHistory(alarmHislist);

                    }

                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void AlarmStatusChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues(string.Format("L3_AlarmStatusChangeReport#{0}", sArray[1].PadLeft(2, '0')));
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                alarmChannel[int.Parse(sArray[1])] = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Alarm Status Change Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCAlarmStatusChangeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split(new char[] { '#' })[1];

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, AlarmStatusChangeReportReplyTimeout);

                string timerIDT1 = string.Format("{0}_{1}_{2}", "L3", pathNo, AlarmStatusChangeReportTimeout);

                if (Timermanager.IsAliveTimer(timerIDT1))
                {
                    Timermanager.TerminateTimer(timerIDT1);
                }
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Alarm Status Change Report Reply#{2}.",
                        eqpNo, inputData.TrackKey, pathNo));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {


                    Trx trx = GetTrxValues(string.Format("L3_AlarmStatusChangeReport#{0}", pathNo.PadLeft(2, '0')));
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    SendToPLC(trx);
                    //模拟T3事件间隔上报
                    // trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                    Thread.Sleep(ParameterManager[eParameterName.T3].GetInteger());

                    alarmChannel[int.Parse(pathNo)] = true;


                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCAlarmStatusChangeReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Alarm Status Change Report Reply#{2} BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCAlarmStatusChangeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Alarm Status Change Report Reply#{2} T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region EQDDownstreamActionMonitor

        public void EQDDownstreamActionMonitor(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));

                    return;
                }

                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (!eqp.File.DownActionMonitor.ContainsKey(i + 1))
                    {
                        eqp.File.DownActionMonitor.Add(i + 1, it.Value);

                    }
                    else
                    {
                        eqp.File.DownActionMonitor[i + 1] = it.Value;


                    }

                    if (i == 0 && it.Value == "0")
                    {
                        Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#01");
                        EQDDownContrx[0][0][0].Value = "0";
                        if (line.Data.FABTYPE == "CELL")
                        {
                            EQDDownContrx[0][0][1].Value = "0";
                        }
                        if (line.Data.FABTYPE != "CELL")//eqp.Data.LINEID != "KWF23633L"
                        {
                            eqp.File.DownSendComplete = eBitResult.OFF;
                        }
                        else
                        {
                            eqp.File.DownSendComplete = eBitResult.OFF;
                            eqp.File.DownSendCompleteUP = eBitResult.OFF;
                        }

                        SendToPLC(EQDDownContrx);
                    }
                    if (i == 3 && it.Value == "0")
                    {
                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#02");
                            EQDDownContrx[0][0][0].Value = "0";

                            eqp.File.DownSendCompleteUP = eBitResult.OFF;

                            SendToPLC(EQDDownContrx);
                        }
                    }

                    if (i == 11 && it.Value == "0")
                    {
                        //if (eqp.Data.LINEID == "KWF22090R" || eqp.Data.LINEID == "KWF22091R")
                        //{
                        //    Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#03");
                        //    EQDDownContrx[0][0][0].Value = "0";

                        //    eqp.File.SendCompleteReturn = eBitResult.OFF;

                        //    SendToPLC(EQDDownContrx);
                        //}
                    }


                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        #endregion

        #region EQDUpstreamActionMonitor

        public void EQDUpstreamActionMonitor(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (!eqp.File.UPActionMonitor.ContainsKey(i + 1))
                    {
                        eqp.File.UPActionMonitor.Add(i + 1, it.Value);
                    }
                    else
                    {
                        eqp.File.UPActionMonitor[i + 1] = it.Value;

                    }

                    if (i == 2 && it.Value == "0")
                    {
                        Trx EQDUPContrx = GetTrxValues("L3_EQDReciveCompleteBit");
                        EQDUPContrx[0][0][0].Value = "0";
                        SendToPLC(EQDUPContrx);

                        eqp.File.UPReciveComplete = eBitResult.OFF;

                    }

                    if (i == 8 && it.Value == "0")//清上层完成
                    {
                        //if (eqp.Data.LINEID == "KWF22090R" || eqp.Data.LINEID == "KWF22091R")
                        //{
                        //    Trx EQDUPContrx = GetTrxValues("L3_EQDUpReciveCompleteBit");
                        //    EQDUPContrx[0][0][0].Value = "0";
                        //    SendToPLC(EQDUPContrx);
                        //    eqp.File.UPReciveCompleteUP = eBitResult.OFF;
                        //}
                    }

                    if (i == 10 && it.Value == "0")//返流完成
                    {
                        if (line.Data.FABTYPE == "CELL")//eqp.Data.LINEID == "KWF23633L"
                        {
                            Trx EQDUPContrx = GetTrxValues("L3_EQDReturnReciveCompleteBit");
                            EQDUPContrx[0][0][0].Value = "0";
                            SendToPLC(EQDUPContrx);
                            eqp.File.ReceiveCompleteReturn = eBitResult.OFF;
                        }
                    }

                    if (i == 5 && it.Value == "1")//滚轮转动命令OFF
                    {
                        Trx EQDUPContrx = GetTrxValues("L3_EQDRollRunCommand");
                        EQDUPContrx[0][0][0].Value = "0";
                        SendToPLC(EQDUPContrx);
                    }
                    i++;
                }

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        #endregion

        #region EQDUnitReasonCodeReport

        public void EQDUnitReasonCodeReport(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;

                string pathNo = inputData.Name.Split(new char[] { '#' })[1];

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Unit unit = ObjectManager.UnitManager.GetUnit(eqpNo, int.Parse(pathNo));

                if (unit == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Unit =[{0}]", pathNo));
                    return;
                }

                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (!unit.File.ReasonCode.ContainsKey(i + 1))
                    {
                        unit.File.ReasonCode.Add(i + 1, it.Value);
                    }
                    else
                    {
                        unit.File.ReasonCode[i + 1] = it.Value;

                    }
                    i++;
                }

                //查找出每个Unit第一个Reason Code添加到EQP Reason Code 。
                if (unit.File.ReasonCode.ContainsValue("1") && !eqp.File.ReasonCode.ContainsKey(unit.File.ReasonCode.Where(d => d.Value == "1").First().Key))
                {
                    eqp.File.ReasonCode.Clear();
                    eqp.File.ReasonCode.Add(unit.File.ReasonCode.Where(d => d.Value == "1").First().Key, "1");
                }

                ObjectManager.UnitManager.EnqueueSave(unit.File);
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        #endregion

        #region Unit State Change Report/EQ State Change Report / All Reason Code Report 

        public void EQUnitStateChangeReport(Trx inputData)
        {
            try
            {

                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split(new char[] { '#' })[1];

                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                Unit unit = ObjectManager.UnitManager.GetUnit(eqpNo, int.Parse(pathNo));

                if (unit == null)
                {
                    if (int.Parse(pathNo) > eq.Data.UNITCOUNT)
                    {
                        return;
                    }
                    throw new Exception(string.Format("CAN'T FIND UNIT_NO=[{0}] IN UNITENTITY!", pathNo));
                }

                string currentState = inputData[0][0]["CurrentState"].Value.Trim();
                string currentCount = inputData[0][0]["CurrentCount"].Value;

                eEQPStatus unitOldStatus = unit.File.Status;
                eEQPStatus eqOldStatus = eq.File.Status;

                //1:PM、2：Down、3：Pause、4:Ilde、5：Run    3不启用
                switch (currentState)
                {
                    case "1"://单元状态为运行，当有片时为RUN,当没有片时候为IDLE;
                        if (currentCount != "0")
                        {
                            unit.File.Status = eEQPStatus.Run;
                        }
                        else
                        {
                            unit.File.Status = eEQPStatus.Idle;
                        }
                        break;

                    case "2"://设备停机
                        unit.File.Status = eEQPStatus.Down;
                        break;

                    case "3"://设备待机
                        unit.File.Status = eEQPStatus.Idle;
                        break;

                    case "4"://设备准备状态
                        unit.File.Status = eEQPStatus.Initial;
                        break;

                    case "5"://设备手动状态
                        unit.File.Status = eEQPStatus.Initial;
                        break;
                    case "6":
                        unit.File.Status = eEQPStatus.Pause;
                        break;

                    default:

                        break;
                }
                //更新unit状态，并上报设备状态
                if (lastUnitDic.ContainsKey(unit.Data.UNITNO))
                {
                    lastUnitDic[unit.Data.UNITNO] = unitOldStatus;
                }
                else
                {
                    lastUnitDic.TryAdd(unit.Data.UNITNO, unitOldStatus);
                }
                if (unit.File.Status != unitOldStatus)
                {
                    //上报 Unit Status/Unit Reason Code/EQ Status/EQ Reason Code 
                    //CPCUnitReasonCodeReport(unit, inputData, eBitResult.ON);
                    //CPCUnitStateChangeReport(unit, inputData, eBitResult.ON);
                    if (unitDic.ContainsKey(unit.Data.UNITNO))
                    {
                        unitDic[unit.Data.UNITNO] = unit;
                    }
                    else
                    {
                        unitDic.Add(unit.Data.UNITNO, unit);
                    }

                    //if (unitReasonCodeDic.ContainsKey(unit.Data.UNITNO))
                    //{
                    //    unitReasonCodeDic[unit.Data.UNITNO] = unit;
                    //}
                    //else
                    //{
                    //    unitReasonCodeDic.Add(unit.Data.UNITNO, unit);
                    //}

                }


                Dictionary<int, eEQPStatus> unitStatusDic = new Dictionary<int, eEQPStatus>();

                foreach (Unit u in ObjectManager.UnitManager.GetUnits())
                {
                    unitStatusDic.Add(u.Data.UNITNO, u.File.Status);
                }

                if (unitStatusDic.ContainsValue(eEQPStatus.Initial))
                {
                    eq.File.Status = eEQPStatus.Initial;
                }
                else if (unitStatusDic.ContainsValue(eEQPStatus.Down) && !unitStatusDic.ContainsValue(eEQPStatus.Initial))
                {
                    eq.File.Status = eEQPStatus.Down;
                }
                else if (unitStatusDic.ContainsValue(eEQPStatus.Pause) && !unitStatusDic.ContainsValue(eEQPStatus.Initial) && !unitStatusDic.ContainsValue(eEQPStatus.Down))
                {
                    eq.File.Status = eEQPStatus.Pause;
                }
                else if (unitStatusDic.ContainsValue(eEQPStatus.Run) && !unitStatusDic.ContainsValue(eEQPStatus.Initial) && !unitStatusDic.ContainsValue(eEQPStatus.Down) && !unitStatusDic.ContainsValue(eEQPStatus.Pause))
                {
                    eq.File.Status = eEQPStatus.Run;
                }
                else if (unitStatusDic.ContainsValue(eEQPStatus.Idle) && !unitStatusDic.ContainsValue(eEQPStatus.Initial) && !unitStatusDic.ContainsValue(eEQPStatus.Down) && !unitStatusDic.ContainsValue(eEQPStatus.Pause) && !unitStatusDic.ContainsValue(eEQPStatus.Run))
                {
                    //if (eq.File.TotalTFTGlassCount == 0)
                    //{
                    eq.File.Status = eEQPStatus.Idle;
                    //}
                    //else
                    //{
                    //    if (eq.File.Status == eEQPStatus.Initial || eq.File.Status == eEQPStatus.Down || eq.File.Status == eEQPStatus.Pause)
                    //    {
                    //        eq.File.Status = eEQPStatus.Idle;
                    //    }
                    //}
                }
                else
                {
                    eq.File.Status = eEQPStatus.Unused;
                }
                //上报设备的状态和Reason Code
                if (eq.File.Status != eqOldStatus)
                {
                    //CPCEquipmentReasonCodeChangeReport(eq, inputData, eBitResult.ON);
                    //CPCEquipmentStatusChangeReport(eq, inputData, eBitResult.ON);
                    //Thread.Sleep(200);
                    ObjectManager.EquipmentManager.SaveEquipmentHistory(eq);
                }
                //保存本次的状态
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                ObjectManager.UnitManager.EnqueueSave(unit.File);

            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        private void EquipmentStatusChangeScan()
        {
            Thread.Sleep(5000);

            while (true)
            {
                Thread.Sleep(3000);
                List<Unit> unitList = ObjectManager.UnitManager.GetUnits().ToList();
                for (int i = 0; i < unitList.Count; i++)
                {
                    if (lastUnitDic.Count > 0 && lastUnitDic.ContainsKey(i + 1))
                    {
                        if (unitList[i].File.Status != lastUnitDic[i + 1])
                        {
                            //上报
                            CPCEquipmentStatusChangeReportByThread(eBitResult.ON, unitList[i].File.Status, i + 1);
                            lastUnitDic[i + 1] = unitList[i].File.Status;
                            goto next;
                        }
                    }
                }
            next:;
            }
        }
        private void CPCEquipmentStatusChangeReportByThread(eBitResult result, eEQPStatus unitStatus, int unitNo)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("L3_EquipmentStatusChangeReport"));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[L3] TRX L3_EquipmentStatusChangeReport IN PLCFmt.xml!"));

                string timerID = string.Format("{0}_{1}", "L3", CPCEquipmentStatusChangeReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Equipment Status Change Report.",
                            "L3", trx.TrackKey));

                    return;
                }
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                trx[0][0][0].Value = ((int)eq.File.Status).ToString();
                trx[0][0][1].Value = ObjectManager.UnitManager.GetUnits().Count.ToString();

                foreach (Unit u in ObjectManager.UnitManager.GetUnits())
                {
                    trx[0][0][u.Data.UNITNO + 1].Value = ((int)u.File.Status).ToString();
                }
                //设备初始化
                if (eq.File.Status == eEQPStatus.Initial)
                {
                    trx[0][0]["ReasonCode"].Value = "1500";
                }//设备停机
                else if (eq.File.Status == eEQPStatus.Down)
                {     // 查看子单元ReasonCode
                    if (eq.File.ReasonCode.Count > 0)
                    {
                        if (eq.File.ReasonCode.ContainsKey(1))
                        {
                            trx[0][0]["ReasonCode"].Value = "2202";
                        }
                        else if (eq.File.ReasonCode.ContainsKey(2))
                        {
                            trx[0][0]["ReasonCode"].Value = "2203";
                        }
                        else if (eq.File.ReasonCode.ContainsKey(3))
                        {
                            trx[0][0]["ReasonCode"].Value = "2500";
                        }
                        else if (eq.File.ReasonCode.ContainsKey(4))
                        {
                            trx[0][0]["ReasonCode"].Value = "2204";
                        }
                        else
                        {
                            trx[0][0]["ReasonCode"].Value = "2105";
                        }
                    }
                    else
                    {
                        trx[0][0]["ReasonCode"].Value = "2900";
                    }
                }//设备IDLE
                else if (eq.File.Status == eEQPStatus.Idle)
                {
                    trx[0][0]["ReasonCode"].Value = "4002";
                }//设备RUN 
                else if (eq.File.Status == eEQPStatus.Run)
                {
                    trx[0][0]["ReasonCode"].Value = "5000";
                }  //设备其他状态
                else
                {
                    trx[0][0]["ReasonCode"].Value = "5008";
                }
                trx[0][1][0].Value = "1";
                trx[0][1].OpDelayTimeMS = 500;

                trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCEquipmentStatusChangeReportTimeoutAction), trx.TrackKey);


                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Status Change Report =[{2}] Equipment=[{3}] Unit[{4}]=[{5}].", trx.Metadata.NodeNo, trx.TrackKey, "ON", eq.File.Status, unitNo, unitStatus));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Equipment Status Change Report
        public void BCEquipmentStatusChangeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCEquipmentStatusChangeReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Equipment Status Change Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //CPCEquipmentStatusChangeReport(eqp, inputData, eBitResult.OFF);
                    CPCEquipmentStatusChangeReportByThread(eBitResult.OFF, 0, 0);

                    Thread.Sleep(500);
                    UnitStatusReport = true;
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), inputData.TrackKey);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Status Change Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCEquipmentStatusChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EquipmentStatusChangeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);
                Thread.Sleep(500);
                UnitStatusReport = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Status Change Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCEquipmentStatusChangeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_EquipmentStatusChangeReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Equipment Status Change Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region  Equipment Reason Code Change Report
        public void BCEquipmentReasonCodeChangeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCEquipmentReasonCodeChangeReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Equipment Reason Code Change Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCEquipmentReasonCodeChangeReport(eqp, inputData, eBitResult.OFF);
                    //Thread.Sleep(200);
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentReasonCodeChangeReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Reason Code Change Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCEquipmentReasonCodeChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EquipmentReasonCodeChangeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Reason Code Change Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCEquipmentReasonCodeChangeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_EquipmentReasonCodeChangeReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Equipment Reason Code Change Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Unit State Change Report
        public void BCUnitStateChangeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCUnitStateChangeReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Unit State Change Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    string timerID1 = string.Format("{0}_{1}", "L3", CPCUnitStateChangeReportTimeout);
                    if (Timermanager.IsAliveTimer(timerID1))
                    {
                        Timermanager.TerminateTimer(timerID1);
                    }

                    Trx trx = GetTrxValues("L3_UnitStatusChangeReport");
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    SendToPLC(trx);

                    Thread.Sleep(500);
                    UnitStatusReport = true;

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCUnitStateChangeReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Unit State Change Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCUnitStateChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_UnitStatusChangeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                UnitStatusReport = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Unit State Change Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCUnitStateChangeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_UnitStateChangeReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Unit State Change Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region  Unit Reason Code Report
        public void BCUnitReasonCodeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, CPCUnitReasonCodeReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Unit Reason Code Report Reply.",
                        eqpNo, inputData.TrackKey));


                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    string timerID1 = string.Format("{0}_{1}", "L3", CPCUnitReasonCodeReportTimeout);
                    if (Timermanager.IsAliveTimer(timerID1))
                    {
                        Timermanager.TerminateTimer(timerID1);
                    }


                    Trx trx = GetTrxValues("L3_UnitReasonCodeChangeReport");
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    SendToPLC(trx);

                    UnitReasonCodeReport = true;
                    Thread.Sleep(200);
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCUnitReasonCodeReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Unit Reason Code Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCUnitReasonCodeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_UnitReasonCodeChangeReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                UnitReasonCodeReport = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Unit Reason Code Report Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCUnitReasonCodeReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_UnitReasonCodeChangeReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Unit Reason Code Change Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Position Glass Change Report 更新Position信息，刷新Fetch\Store信息
        public void EQPositionGlassChangeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                string position = inputData.Name.Split('#')[1];

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                //检查是否需要上报Store、Fetch
                //  1、获取Position 信息 2、判断是否符合临界条件
                Dictionary<int, PositionEntityData> positionDic = new Dictionary<int, PositionEntityData>();
                positionDic = ObjectManager.EquipmentManager.GetPositionData(eqp.Data.LINEID);

                //更新Positin Job
                Job job;

                if (bitResult == eBitResult.ON)
                {
                    job = ObjectManager.JobManager.GetJob(inputData[0][0]["Cassette_Sequence_No"].Value, inputData[0][0]["Job_Sequence_No"].Value);

                    if (job == null)
                    {
                        job = new Job(int.Parse(inputData[0][0]["Cassette_Sequence_No"].Value), int.Parse(inputData[0][0]["Job_Sequence_No"].Value));
                        UpdateJobData(eqp, job, inputData[0][0]);

                        //设备新增的玻璃信息
                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Create.ToString(),
                                    eqp.Data.NODEID,
                                    eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");
                    }
                    else
                    {
                        if (job.Cassette_Sequence_No == inputData[0][0]["Cassette_Sequence_No"].Value && job.Job_Sequence_No == inputData[0][0]["Job_Sequence_No"].Value)
                        {
                            UpdateJobData(eqp, job, inputData[0][0]);
                        }
                    }

                    if (!eqp.File.PositionJobs.ContainsKey(int.Parse(position)))
                    {
                        eqp.File.PositionJobs.TryAdd(int.Parse(position), job);
                    }
                    else
                    {
                        eqp.File.PositionJobs[int.Parse(position)] = job;
                    }

                    //判断是否需要上报Store Envent
                    //判断事件针对单元是否上报

                    if ((eqp.Data.NODENAME == "TEREWORK" && position == "15") || (eqp.Data.NODENAME == "TSREWORK" && (position == "27" || position == "28")))
                    {
                        if (job.processFlow == null)
                        {
                            if (eqp.File.PositionJobs.ContainsKey(int.Parse(position)) && !eqp.File.FetchJobs.ContainsKey(int.Parse(position)))
                            {
                                eqp.File.FetchJobs.TryAdd(int.Parse(position) - 2, eqp.File.PositionJobs[int.Parse(position)]);
                            }
                        }
                        else if (job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position) - 2].UnitNo).Count() > 0 &&
                            job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position) - 2].UnitNo).First().EndTime == DateTime.MinValue)
                        {
                            if (eqp.File.PositionJobs.ContainsKey(int.Parse(position)) && !eqp.File.FetchJobs.ContainsKey(int.Parse(position)))
                            {
                                eqp.File.FetchJobs.TryAdd(int.Parse(position) - 2, eqp.File.PositionJobs[int.Parse(position)]);
                            }
                        }



                        if (job.processFlow == null && positionDic.ContainsKey(int.Parse(position)))
                        {
                            if (!eqp.File.StoreJobs.ContainsKey(int.Parse(position)) && positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "I")
                            {
                                eqp.File.StoreJobs.TryAdd(int.Parse(position), job);
                            }

                        }
                        else if (job.processFlow != null && job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position)].UnitNo).ToList().Count <= 0 && positionDic.ContainsKey(int.Parse(position)))
                        {
                            if (!eqp.File.StoreJobs.ContainsKey(int.Parse(position)) && positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "I")
                            {
                                eqp.File.StoreJobs.TryAdd(int.Parse(position), job);
                            }
                        }

                    }
                    else
                    {
                        if (job.processFlow == null && positionDic.ContainsKey(int.Parse(position)))
                        {
                            if (!eqp.File.StoreJobs.ContainsKey(int.Parse(position)) && positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "I")
                            {
                                eqp.File.StoreJobs.TryAdd(int.Parse(position), job);
                            }

                        }
                        else if (job.processFlow != null && job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position)].UnitNo).ToList().Count <= 0 && positionDic.ContainsKey(int.Parse(position)))
                        {
                            if (!eqp.File.StoreJobs.ContainsKey(int.Parse(position)) && positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "I")
                            {
                                eqp.File.StoreJobs.TryAdd(int.Parse(position), job);
                            }
                        }
                    }

                    //添加position Info 
                    job.PrositonNo = position;
                    job.CurrentUnitNo = int.Parse(positionDic[int.Parse(position)].UnitNo);

                    ObjectManager.JobManager.AddJob(job);


                }
                else
                {
                    Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                    if ((line.Data.ATTRIBUTE == "ETCH" && position == "28")
                        || (line.Data.ATTRIBUTE == "STRIP" && position == "25")
                        || (line.Data.ATTRIBUTE == "CLN" && ((position == "11" && eqp.Data.LINEID == "KWF23637R") || (position == "9" && eqp.Data.LINEID != "KWF23637R")))
                        || (line.Data.ATTRIBUTE == "DEV" && position == "12")
                        || (line.Data.ATTRIBUTE == "ODF" && position == "15"))
                    {
                        return;
                    }

                    //判断是否需要上报Fetch,添加Job进Fetch字典
                    if (eqp.File.PositionJobs.ContainsKey(int.Parse(position)))
                    {
                        job = eqp.File.PositionJobs[int.Parse(position)];
                    }
                    else
                    {
                        job = null;

                    }


                    if (job != null)
                    {
                        if ((eqp.Data.NODENAME == "TEREWORK" && position == "12") || (eqp.Data.NODENAME == "TSREWORK" && position == "24"))
                        {

                            if (job.processFlow == null && positionDic.ContainsKey(int.Parse(position)))
                            {
                                if (!eqp.File.StoreJobs.ContainsKey(int.Parse(position)))
                                {
                                    eqp.File.StoreJobs.TryAdd(int.Parse(position) + 1, job);
                                }

                            }
                            else if (job.processFlow != null && job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position) + 1].UnitNo).ToList().Count <= 0 && positionDic.ContainsKey(int.Parse(position)))
                            {
                                if (!eqp.File.StoreJobs.ContainsKey(int.Parse(position)))
                                {
                                    eqp.File.StoreJobs.TryAdd(int.Parse(position) + 1, job);
                                }
                            }


                            if (job.processFlow == null)
                            {
                                if (eqp.File.PositionJobs.ContainsKey(int.Parse(position)) && !eqp.File.FetchJobs.ContainsKey(int.Parse(position)) && (positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "O"))
                                {
                                    eqp.File.FetchJobs.TryAdd(int.Parse(position), eqp.File.PositionJobs[int.Parse(position)]);
                                }
                            }
                            else if (job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position)].UnitNo).Count() > 0 &&
                                job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position)].UnitNo).First().EndTime == DateTime.MinValue)
                            {
                                if (eqp.File.PositionJobs.ContainsKey(int.Parse(position)) && !eqp.File.FetchJobs.ContainsKey(int.Parse(position)) && (positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "O"))
                                {
                                    eqp.File.FetchJobs.TryAdd(int.Parse(position), eqp.File.PositionJobs[int.Parse(position)]);
                                }
                            }


                        }
                        else
                        {
                            if (job.processFlow == null)
                            {
                                if (eqp.File.PositionJobs.ContainsKey(int.Parse(position)) && !eqp.File.FetchJobs.ContainsKey(int.Parse(position)) && (positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "O"))
                                {
                                    eqp.File.FetchJobs.TryAdd(int.Parse(position), eqp.File.PositionJobs[int.Parse(position)]);
                                }
                            }
                            else if (job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position)].UnitNo).Count() > 0 &&
                                job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == positionDic[int.Parse(position)].UnitNo).First().EndTime == DateTime.MinValue)
                            {
                                if (eqp.File.PositionJobs.ContainsKey(int.Parse(position)) && !eqp.File.FetchJobs.ContainsKey(int.Parse(position)) && (positionDic[int.Parse(position)].UnitType == "IO" || positionDic[int.Parse(position)].UnitType == "O"))
                                {
                                    eqp.File.FetchJobs.TryAdd(int.Parse(position), eqp.File.PositionJobs[int.Parse(position)]);
                                }
                            }
                        }
                        // eqp.File.PositionJobs.Remove(int.Parse(position));
                    }
                }

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Fetch Glass Data Report

        //BC TO EC

        public void BCFetchGlassDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split(new char[] { '#' })[1];
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, CPCFetchGlassDataReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Fetch Glass Data Report Reply#{2}.",
                        eqpNo, inputData.TrackKey, pathNo));
                    return;
                }

                string timerID1 = string.Format("{0}_{1}_{2}", "L3", pathNo, CPCFetchGlassDataReportTimeout);
                if (Timermanager.IsAliveTimer(timerID1))
                {
                    Timermanager.TerminateTimer(timerID1);
                }

                Trx trx = GetTrxValues(string.Format("{0}_FetchedOutJobReport#{1}", eqp.Data.NODENO, pathNo.PadLeft(2, '0')));
                // trx.ClearTrxWith0();                  
                trx[0][2][0].Value = "0";
                SendToPLC(trx);




                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCFetchGlassDataReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Fetch Glass Data Report Reply[{2}] BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, (eBitResult)int.Parse(inputData[0][0][0].Value)));
                Thread.Sleep(300);
                fetchChannel[int.Parse(pathNo)] = true;
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCFetchGlassDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_FetchGlassDataReportTimeout


                //T1超时EC OFF BIT
                Trx trx = GetTrxValues(string.Format("L3_FetchedOutJobReport#{0}", sArray[1].PadLeft(2, '0')));
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);
                Thread.Sleep(300);
                fetchChannel[int.Parse(sArray[1])] = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Fetch Glass Data Report  BC Reply#{2} T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCFetchGlassDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_FetchGlassDataReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Fetch Glass Data Report Reply TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region StoreGlassDataReport

        //EC TO BC 
        public void BCStoreGlassDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split(new char[] { '#' })[1];
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, CPCStoreGlassDataReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Store Glass Data Report Reply#{2}.",
                        eqpNo, inputData.TrackKey, pathNo));
                    return;
                }
                string timerID1 = string.Format("{0}_{1}_{2}", "L3", pathNo, CPCStoreGlassDataReportTimeout);

                if (Timermanager.IsAliveTimer(timerID1))
                {
                    Timermanager.TerminateTimer(timerID1);
                }

                Trx trx = GetTrxValues(string.Format("{0}_StoredJobReport#{1}", eqp.Data.NODENO, pathNo.PadLeft(2, '0')));
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);



                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCStoreGlassDataReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Store Glass Data Report Reply[{2}] BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, bitResult.ToString()));
                Thread.Sleep(300);
                stroeChannel[int.Parse(pathNo)] = true;
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCStoreGlassDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_StoreGlassDataReportTimeout

                //T1超时EC OFF BIT                       
                Trx trx = GetTrxValues(string.Format("L3_StoredJobReport#{0}", sArray[1].PadLeft(2, '0')));
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);
                Thread.Sleep(300);
                stroeChannel[int.Parse(sArray[1])] = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Store Glass Data Report BC Reply#{2} T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCStoreGlassDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_StoreGlassDataReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Store Glass Data Report Reply#{2} TIMEOUT.", sArray[0], trackKey, sArray[1]));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region OtherUpstreamPath
        //下层，90和91算上层
        public void OtherUpstreamPath01(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {
                    if (eqp.Data.LINEID == "KWF22090R" || eqp.Data.LINEID == "KWF22091R")
                    {
                        if (!eqp.File.UPInterface02.ContainsKey(i + 1))
                        {
                            eqp.File.UPInterface02.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.UPInterface02[i + 1] = it.Value;

                        }
                    }
                    else
                    {
                        if (!eqp.File.UPInterface01.ContainsKey(i + 1))
                        {
                            eqp.File.UPInterface01.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.UPInterface01[i + 1] = it.Value;

                        }
                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        //90和91算下层
        public void OtherUpstreamPath02(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (eqp.Data.LINEID == "KWF22090R" || eqp.Data.LINEID == "KWF22091R")
                    {
                        if (!eqp.File.UPInterface01.ContainsKey(i + 1))
                        {
                            eqp.File.UPInterface01.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.UPInterface01[i + 1] = it.Value;

                        }
                    }
                    else
                    {
                        if (!eqp.File.UPInterface02.ContainsKey(i + 1))
                        {
                            eqp.File.UPInterface02.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.UPInterface02[i + 1] = it.Value;

                        }
                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        //返流
        public void OtherUpstreamPath03(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (!eqp.File.UPInterface03.ContainsKey(i + 1))
                    {
                        eqp.File.UPInterface03.Add(i + 1, it.Value);
                    }
                    else
                    {
                        eqp.File.UPInterface03[i + 1] = it.Value;

                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        #endregion

        #region OtherDownstreamPath
        //ETCH和STRIP算上层
        public void OtherDownstreamPath01(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {
                    if (line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP")
                    {
                        if (!eqp.File.DownInterface02.ContainsKey(i + 1))
                        {
                            eqp.File.DownInterface02.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.DownInterface02[i + 1] = it.Value;

                        }
                    }
                    else
                    {
                        if (!eqp.File.DownInterface01.ContainsKey(i + 1))
                        {
                            eqp.File.DownInterface01.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.DownInterface01[i + 1] = it.Value;

                        }
                    }

                    i++;
                }

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        //ETCH和STRIP算下层
        public void OtherDownstreamPath02(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {
                    if (line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP")
                    {
                        if (!eqp.File.DownInterface01.ContainsKey(i + 1))
                        {
                            eqp.File.DownInterface01.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.DownInterface01[i + 1] = it.Value;

                        }
                    }
                    else
                    {
                        if (!eqp.File.DownInterface02.ContainsKey(i + 1))
                        {
                            eqp.File.DownInterface02.Add(i + 1, it.Value);
                        }
                        else
                        {
                            eqp.File.DownInterface02[i + 1] = it.Value;

                        }
                    }
                    i++;
                }

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        //返流
        public void OtherDownstreamPath03(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                int i = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (!eqp.File.DownInterface03.ContainsKey(i + 1))
                    {
                        eqp.File.DownInterface03.Add(i + 1, it.Value);
                    }
                    else
                    {
                        eqp.File.DownInterface03[i + 1] = it.Value;

                    }
                    i++;
                }

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        #endregion

        #region Recipe Register Validation Command

        public void BCRecipeRegisterValidationCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, RecipeRegisterValidationCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Recipe Register Validation Command.",
                        eqpNo, inputData.TrackKey));

                    CPCRecipeRegisterValidationCommandReply(eBitResult.OFF, inputData, "0");

                    return;
                }


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeRegisterValidationCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion


                string recipeType = inputData[0][0]["RecipeType"].Value.Trim();
                string recipeNo = inputData[0][0]["RecipeNo"].Value.Trim();
                string recipeID = inputData[0][0]["RecipeID"].Value.Trim();
                string messageSequenceNo = inputData[0][0]["MessageSequenceNo"].Value.Trim();

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command RecipeType=[{2}] RecipeNo=[{3}] RecipeID=[{4}] MessageSequenceNo=[{5}].",
                        eq.Data.NODENO, inputData.TrackKey, recipeType, recipeNo, recipeID, messageSequenceNo));
                if (recipeType == "2" && !string.IsNullOrEmpty(recipeID))
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipe();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeID))
                    {

                        CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "1");

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply OK .",
                                eq.Data.NODENO, inputData.TrackKey));

                    }
                    else
                    {
                        CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "2");

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply NG Recipe ID=[{2}] Not exist.",
                                eq.Data.NODENO, inputData.TrackKey, recipeID));
                    }
                }
                else if (recipeType == "1" && !string.IsNullOrEmpty(recipeNo))
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipeByNo();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeNo))
                    {
                        if (recipeDic[eq.Data.LINEID][recipeNo].RECIPESTATUS == "Enable")
                        {
                            CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "1");

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply OK .",
                                    eq.Data.NODENO, inputData.TrackKey));

                        }
                        else
                        {

                            CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "3");

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply NG .",
                                    eq.Data.NODENO, inputData.TrackKey));

                        }

                    }
                    else
                    {
                        CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "2");

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply NG Recipe NO=[{2}] Not exist.",
                                eq.Data.NODENO, inputData.TrackKey, recipeNo));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(recipeNo))
                    {
                        Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                       ObjectManager.RecipeManager.ReloadRecipeByNo();

                        if (recipeDic[eq.Data.LINEID].ContainsKey(recipeNo))
                        {

                            CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "1");

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply OK .",
                                    eq.Data.NODENO, inputData.TrackKey));

                        }
                        else
                        {
                            CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "2");

                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply NG Recipe NO=[{2}] Not exist.",
                                    eq.Data.NODENO, inputData.TrackKey, recipeNo));
                        }
                    }

                    //CPCRecipeRegisterValidationCommandReply(eBitResult.ON, inputData, "2");

                    //LogError(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply NG Registration Conditions is Error.",
                    //        eq.Data.NODENO, inputData.TrackKey));

                }
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCRecipeRegisterValidationCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Recipe Register Validation Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRecipeRegisterValidationCommandReply(eBitResult result, Trx inputData, string returnCode)
        {
            try
            {
                Trx trx = null;
                trx = GetTrxValues("L3_RecipeRegisterValidationCommandReply");

                string eqpNo = trx.Metadata.NodeNo;
                string timerID = string.Empty;
                timerID = string.Format("{0}_{1}", eqpNo, RecipeRegisterValidationCommandReplyTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    if (inputData != null)
                    {
                        trx.TrackKey = inputData.TrackKey;
                    }
                    else
                    {
                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    }

                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Recipe Register Validation Command Reply.",
                            eqpNo, trx.TrackKey));

                    return;
                }

                trx[0][0][0].Value = inputData[0][0]["MessageSequenceNo"].Value.Trim();
                trx[0][0][1].Value = returnCode;


                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;

                SendToPLC(trx);

                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeRegisterValidationCommandReplyTimeoutAction), inputData.TrackKey);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, inputData.TrackKey, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCRecipeRegisterValidationCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Recipe Register Validation Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCRecipeRegisterValidationCommandReply(eBitResult.OFF, null, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Recipe Parameter Request

      
        private void BCRecipeParameterRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Parameter Request TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eq"></param>
        /// <param name="result"></param>
        /// <param name="inputData"></param>
        /// <param name="returnCode"></param>
        /// <param name="recipe"></param>
        private void CPCRecipeParameterRequestReply(Equipment eq, eBitResult result, Block inputData, string returnCode, RecipeEntityData recipe)
        {
            try
            {


                Block RecipeParameterRequestCommandReplyBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeParameterRequestCommandReplyBlock");

                string eqpNo = "L3";
                string timerID = string.Empty;
                timerID = string.Format("{0}_{1}", eqpNo, RecipeParameterRequestReplyTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {

                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeCommandReply", "RecipeParameterRequestCommandReply", false);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Recipe Parameter Request Reply.",
                            eqpNo, DateTime.Now.ToString("yyyyMMddHHmmss")));

                    return;
                }
                int j = 0;
                foreach (ItemBase item in inputData.Items)
                {
                    RecipeParameterRequestCommandReplyBlock[j].Value = item.Value;
                    j++;
                }

                if (returnCode != "2" && recipe != null)
                {
                    IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipe.FILENAME);

                    if (paramter != null && paramter.Count != 0)
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                        foreach (var data in paramter)
                        {
                            string[] splitc;
                            if (!string.IsNullOrEmpty(data) && data.Contains("="))
                            {
                                splitc = data.Trim().Split(new string[] { "=" }, StringSplitOptions.None);
                                if (splitc[0].Trim() != "Recipe_Verson" && splitc[0].Trim() != "Recipe_State" && splitc[0].Trim() != "Recipe_NO" && splitc[0].Trim() != "Recipe_ID")
                                {
                                    if (!keyValuePairs.ContainsKey(splitc[0].Trim()))
                                    {
                                        keyValuePairs.Add(splitc[0].Trim(), splitc[1].Trim());
                                    }
                                    else
                                    {
                                        keyValuePairs[splitc[0].Trim()] = splitc[1].Trim();
                                    }
                                }
                            }
                        }
                        Block LC_EQToCIM_ParameterINT_01_03_00 = eipTagAccess.ReadBlockValues("LC_EQToCIM_ParameterINT_01_03_00", "ParameterINTFormatDataBlock#");

                        int i = 0;
                        foreach (string key in keyValuePairs.Keys)
                        {
                            LC_EQToCIM_ParameterINT_01_03_00[(i + 1) * 2 - 2].Value = i + 1;
                            LC_EQToCIM_ParameterINT_01_03_00[(i + 1) * 2 - 1].Value = keyValuePairs[key];
                            i++;
                        }

                        eipTagAccess.WriteBlockValues("LC_EQToCIM_ParameterINT_01_03_00", LC_EQToCIM_ParameterINT_01_03_00);

                    }
                    RecipeParameterRequestCommandReplyBlock[9].Value = returnCode;
                }
                else
                {
                    RecipeParameterRequestCommandReplyBlock[9].Value = returnCode;
                }
                eipTagAccess.WriteBlockValues("SD_EQToCIM_RecipeManagement_03_01_00", RecipeParameterRequestCommandReplyBlock);

                eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeCommandReply", "RecipeParameterRequestCommandReply", true);



                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeParameterRequestReplyTimeoutAction), DateTime.Now.ToString("yyyyMMddHHmmss"));
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, DateTime.Now.ToString("yyyyMMddHHmmss"), result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void CPCRecipeParameterRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Parameter Request Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                //CPCRecipeParameterRequestReply(eBitResult.OFF, null, null);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Recipe Key Parameter Request

        public void BCRecipeKeyParameterRequest(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CF")
                {
                    return;
                }

                string strlog = string.Empty;



                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, RecipeParameterRequestTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe Key Parameter Request.",
                        eqpNo, inputData.TrackKey));

                    //CPCRecipeParameterRequestReply(eBitResult.OFF, inputData, "0");

                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeKeyParameterRequestTimeoutAction), inputData.TrackKey);
                }

                #endregion

                string masterRecipeID = inputData[0][0][0].Value.Trim();//Master Recipe ID
                string localRecipeID = inputData[0][0][1].Value.Trim();//Local Recipe ID
                string unitNoorLocalNo = inputData[0][0][2].Value.Trim();//Unit No or Local No

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Key Parameter MasterRecipeID=[{2}] LocalRecipeID=[{3}] UnitNoorLocalNo=[{4}].",
                     eq.Data.NODENO, inputData.TrackKey, masterRecipeID, localRecipeID, unitNoorLocalNo));

                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipeByNo();
                }
                else
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipe();
                }

                if (recipeDic[eq.Data.LINEID].ContainsKey(localRecipeID))
                {
                    RecipeEntityData recipe = recipeDic[eq.Data.LINEID][localRecipeID];
                    CPCRecipeKeyParameterRequestReply(eBitResult.ON, inputData, "1", recipe);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Key Parameter Validation Command Reply OK .",
                            eq.Data.NODENO, inputData.TrackKey));

                }
                else
                {
                    CPCRecipeKeyParameterRequestReply(eBitResult.ON, inputData, "2", null);

                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Key Parameter Validation Command Reply NG Local RecipeID=[{2}] Not exist.",
                            eq.Data.NODENO, inputData.TrackKey, localRecipeID));
                }


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }
        private void BCRecipeKeyParameterRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Key Parameter Request TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCRecipeKeyParameterRequestReply(eBitResult result, Trx inputData, string returnCode, RecipeEntityData recipe)
        {
            try
            {
                Trx trx = null;
                trx = GetTrxValues("L3_RecipeKeyParameterRequestCommandReply");

                string eqpNo = trx.Metadata.NodeNo;
                string timerID = string.Empty;
                timerID = string.Format("{0}_{1}", eqpNo, RecipeParameterRequestReplyTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][2][0].Value = "0";
                    if (inputData == null)
                    {
                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    }
                    else
                    {
                        trx.TrackKey = inputData.TrackKey;
                    }
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Recipe Key Parameter Request Reply.",
                            eqpNo, trx.TrackKey));

                    return;
                }

                trx[0][0]["MasterRecipeID"].Value = inputData[0][0]["MasterRecipeID"].Value;
                trx[0][0]["LocalRecipeID"].Value = inputData[0][0]["LocalRecipeID"].Value;
                trx[0][0]["UnitNoOrLocalNo"].Value = inputData[0][0]["UnitNoOrLocalNo"].Value;
                trx[0][0][3].Value = returnCode;

                if (recipe != null)
                {
                    IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipe.FILENAME);
                    if (paramter != null && paramter.Count != 0)
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                        foreach (var data in paramter)
                        {
                            string[] splitc;
                            if (!string.IsNullOrEmpty(data) && data.Contains("="))
                            {
                                splitc = data.Trim().Split(new string[] { "=" }, StringSplitOptions.None);

                                if (!keyValuePairs.ContainsKey(splitc[0].Trim()))
                                {
                                    keyValuePairs.Add(splitc[0].Trim(), splitc[1].Trim());
                                }
                                if (splitc[0].Trim() == "Recipe_State")
                                {

                                }
                                else
                                {
                                    string res = trx[0][1].Items.AllKeys.Where(x => x.Contains(splitc[0].Trim().Split('/')[0]) == true).FirstOrDefault();
                                    if (res != string.Empty)
                                    {
                                        //添加倍率的转换

                                        string name = res.Split('/')[0].Trim();
                                        string value = splitc[1].Trim();
                                        if (res.Contains('/'))
                                        {
                                            if (res.Split('/').Length == 2)
                                            {
                                                int rate = int.Parse(res.Split('/')[1]);
                                                double real = double.Parse(value) * rate;
                                                value = real.ToString();
                                            }
                                        }
                                        trx[0][1][res].Value = value;
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < trx[0][1].Items.Count; i++)
                    {
                        if (trx[0][1][i].Metadata.Expression == ItemExpressionEnum.ASCII)
                        {
                            trx[0][1][i].Value = string.Empty;
                        }
                        else
                        {
                            trx[0][1][i].Value = "0";
                        }
                    }
                }

                trx[0][2][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);


                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeKeyParameterRequestReplyTimeoutAction), inputData.TrackKey);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, inputData.TrackKey, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void CPCRecipeKeyParameterRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Key Parameter Request Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCRecipeKeyParameterRequestReply(eBitResult.OFF, null, null, null);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Recipe Parameter Report
        private void RecipeParameterReport(Equipment eq, Trx inputData, RecipeEntityData recipe, string reportType)
        {
            try
            {
                Trx trx = null;
                trx = GetTrxValues("L3_RecipeParameterReport");
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_RecipeParameterReport IN PLCFmt.xml!", "L3"));

                string eqpNo = trx.Metadata.NodeNo;
                string timerID = string.Empty;
                timerID = string.Format("{0}_{1}", eqpNo, RecipeParameterReportTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                string transactionID = DateTime.Now.ToString("yyyyMMddHHmmss");
                trx[0][0][0].Value = transactionID.Substring(0, 4);//year
                trx[0][0][1].Value = transactionID.Substring(4, 2);//month
                trx[0][0][2].Value = transactionID.Substring(6, 2);//day
                trx[0][0][3].Value = transactionID.Substring(8, 2);//hour
                trx[0][0][4].Value = transactionID.Substring(10, 2);//minute
                trx[0][0][5].Value = transactionID.Substring(12, 2);//second

                trx[0][0][6].Value = recipe.RECIPENAME;//MasterID
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    trx[0][0][7].Value = recipe.RECIPENO;//LocalRecipeID
                    trx[0][0][8].Value = recipe.RECIPENO;//OldLocalRecipeID
                }
                else
                {
                    trx[0][0][7].Value = recipe.RECIPEID;//LocalRecipeID
                    trx[0][0][8].Value = recipe.RECIPEID;//OldLocalRecipeID
                }
                trx[0][0][9].Value = recipe.OPERATORID;//RecipeVerSeqNumber
                trx[0][0][10].Value = "0";//UnitNoorLocalNo
                trx[0][0][11].Value = reportType;//ReportType

                //Recipe Parameter 为临时参数，实际参数待定

                IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipe.FILENAME);

                if (eq.Data.LINEID == "KWF21038L")
                {
                    trx[0][0][12].Value = (paramter.Count - 5).ToString();
                    trx[0][0][12].Value = (paramter.Count - 5).ToString();
                }
                else//参数中不含Recipe_ID,Recipe_NO,Recipe_State,Recipe_Version,Recipe_Name,""
                {
                    trx[0][0][12].Value = (paramter.Count - 6).ToString();
                    trx[0][0][12].Value = (paramter.Count - 6).ToString();
                }
                trx[0][0][13].Value = "1";//TotalGroupCount;
                trx[0][0][14].Value = "1";//CurrentGroupCount;
                trx[0][0][15].Value = "1";//TotalStepCount;
                trx[0][0][16].Value = "1";//CurrentStepCount;

                if (paramter != null && paramter.Count != 0)
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                    foreach (var data in paramter)
                    {
                        string[] splitc;
                        if (!string.IsNullOrEmpty(data) && data.Contains("="))
                        {
                            splitc = data.Trim().Split(new string[] { "=" }, StringSplitOptions.None);

                            if (!keyValuePairs.ContainsKey(splitc[0].Trim()))
                            {
                                keyValuePairs.Add(splitc[0].Trim(), splitc[1].Trim());
                            }
                            if (splitc[0].Trim() == "Recipe_State")
                            {

                            }
                            else
                            {
                                string res = trx[0][1].Items.AllKeys.Where(x => x.ToString().Trim().Equals(splitc[0].Trim()) == true).FirstOrDefault();
                                if (res != null && res != string.Empty)
                                {
                                    //添加倍率的转换

                                    string name = res.Split('/')[0].Trim();
                                    string value = splitc[1].Trim();
                                    if (res.Contains('/'))
                                    {
                                        if (res.Split('/').Length == 2)
                                        {
                                            int rate = int.Parse(res.Split('/')[1]);
                                            double real = double.Parse(value) * rate;
                                            value = real.ToString();
                                        }
                                    }
                                    trx[0][1][res].Value = value;
                                }
                            }

                        }
                    }


                }

                trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    trx[0][3][0].Value = "1";
                    trx[0][3].OpDelayTimeMS = 500;

                }
                else
                {
                    trx[0][3][0].Value = "0";
                }

                SendToPLC(trx);


                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCRecipeParameterReportTimeoutAction), trx.TrackKey);
                string msg = string.Empty;
                if (reportType == "1")
                {
                    msg = "Create";
                }
                else if (reportType == "2")
                {
                    msg = "Modify";
                }
                else if (reportType == "3")
                {
                    msg = "Delete";
                }
                else if (reportType == "4")
                {
                    msg = "Request From EAS";
                }

                Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Parameter Report LocalRecipeID[{2}] MasterRecipeID[{3}] ReportType[{4}][{5}]", "L3", trx.TrackKey, recipe.RECIPENO, recipe.RECIPEID, reportType, msg));

            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }
        private void CPCRecipeParameterReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_RecipeParameterReport");
                // trx.ClearTrxWith0();
                trx[0][3][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recipe Parameter Report T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCRecipeParameterReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, RecipeParameterReportReplyTimeout);

                string timerIDT1 = string.Format("{0}_{1}", "L3", RecipeParameterReportTimeout);

                if (Timermanager.IsAliveTimer(timerIDT1))
                {
                    Timermanager.TerminateTimer(timerIDT1);
                }
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Recipe Parameter Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {


                    Trx trx = GetTrxValues("L3_RecipeParameterReport");
                    // trx.ClearTrxWith0();
                    trx[0][3][0].Value = "0";
                    SendToPLC(trx);


                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeParameterReportReplyAction), inputData.TrackKey);
                }

                #endregion
                string returnCode = inputData[0][0][0].Value;
                string msg = string.Empty;
                if (returnCode == "1")
                {
                    msg = "OK";
                }
                else if (returnCode == "2")
                {
                    msg = "NG";
                }
                else if (returnCode == "3")
                {
                    msg = "Continue";
                }
                else if (returnCode == "4")
                {
                    msg = "Parameter Count NG";
                }
                else if (returnCode == "5")
                {
                    msg = "Group Count NG";
                }
                else if (returnCode == "6")
                {
                    msg = "Dumplication Group Count NG";
                }
                else if (returnCode == "7")
                {
                    msg = "Order By Group Count NG";
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Parameter Report Reply BIT [{2}] RetrunCode [{3}][{4}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][1][0].Value), returnCode, msg));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void BCRecipeParameterReportReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Recipe Parameter Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Local Recipe List Request Command

        public void LocalRecipeListRequestCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, UnitRecipeRequestCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Local Recipe List Request Command.",
                        eqpNo, inputData.TrackKey));

                    CPCLocalRecipeListRequestCommandReply(eBitResult.OFF, inputData);

                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(LocalRecipeListRequestCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion


                string unitNoOrLocalNo = inputData[0][0][0].Value.Trim();

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Local Recipe List Request Command UnitNoOrLocalNo=[{2}] .",
                     eq.Data.NODENO, inputData.TrackKey, unitNoOrLocalNo));

                CPCLocalRecipeListRequestCommandReply(eBitResult.ON, inputData);
                CPCUnitRecipeListReport("0");
                CurrentRecipeListReportType = "0";
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }
        private void LocalRecipeListRequestCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Local Recipe List Request Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCLocalRecipeListRequestCommandReply(eBitResult result, Trx inputData)
        {
            try
            {
                Trx trx = null;
                trx = GetTrxValues("L3_LocalRecipeListRequestCommandReply");

                string eqpNo = trx.Metadata.NodeNo;
                string timerID = string.Empty;
                timerID = string.Format("{0}_{1}", eqpNo, UnitRecipeRequestCommandReplyTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][0][0].Value = "0";
                    if (inputData == null)
                    {
                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    }
                    else
                    {
                        trx.TrackKey = inputData.TrackKey;
                    }
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Local Recipe List Request Command Reply.",
                            eqpNo, trx.TrackKey));

                    return;
                }


                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);


                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCLocalRecipeListRequestCommandReplyTimeoutAction), inputData.TrackKey);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, inputData.TrackKey, result.ToString()));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void CPCLocalRecipeListRequestCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Local Recipe List Request Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCLocalRecipeListRequestCommandReply(eBitResult.OFF, null);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Unit Recipe List Report

        public void BCUnitRecipeListReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, UnitRecipeListReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Local Recipe List Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                string timerID1 = $"{eqpNo}_" + UnitRecipeListReportTimeout;
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID1);
                }
                Trx trx = GetTrxValues(string.Format("{0}_LocalRecipeListReport", eqp.Data.NODENO));
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                //UnitListRecipeReport = true;

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(LocalRecipeListReportReplyTimeoutAction), inputData.TrackKey);
                    if (RecipeListReportIndex != 0)
                    {
                        CPCUnitRecipeListReport(CurrentRecipeListReportType);
                    }
                }

                #endregion
                string returnCode = inputData[0][0][0].Value;
                string msg = string.Empty;
                if (returnCode == "1")
                {
                    msg = "OK";
                }
                else if (returnCode == "2")
                {
                    msg = "Continue";
                }
                else if (returnCode == "3")
                {
                    msg = "Local Recipe Count NG";
                }
                else if (returnCode == "4")
                {
                    msg = "Group Count NG";
                }
                else if (returnCode == "5")
                {
                    msg = "Dumplication Group Count NG";
                }
                else if (returnCode == "6")
                {
                    msg = "Order By Group Count NG";
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Local Recipe List Report Reply BIT [{2}] Return Code[{3}][{4}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString(), returnCode, msg));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCUnitRecipeListReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_LocalRecipeListReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                //UnitListRecipeReport = true;


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Local Recipe List Report BC Reply T1 TIMEOUT.", sArray[0], trackKey));
                if (RecipeListReportIndex != 0)
                {
                    CPCUnitRecipeListReport(CurrentRecipeListReportType);
                }
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LocalRecipeListReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Local Recipe List Report Reply TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Unit Recipe Request//不使用

        private void UnitRecipeRequest(Equipment eq, string hostPPID)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_UnitRecipeRequest", eq.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_JobDataRequest IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, UnitRecipeRequestTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }


                trx[0][0][0].Value = hostPPID;


                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCUnitRecipeRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][1][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Data Request =[{2}].", eq.Data.NODENO, trx.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void BCUnitRecipeRequestReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, UnitRecipeRequestReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Unit Recipe Request Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                string timerID1 = string.Format("{0}_{1}", eqp.Data.NODENO, UnitRecipeRequestTimeout);

                if (Timermanager.IsAliveTimer(timerID1))
                {
                    Timermanager.TerminateTimer(timerID1);
                }

                string recipeID = inputData[0][0]["RecipeID"].Value.Trim();
                string hostPPID = inputData[0][0]["HostPPID"].Value;

                //记录BC回复的RECIPE
                if (!string.IsNullOrEmpty(recipeID))
                {
                    eqp.File.DownLoadRecipe = recipeID;
                }
                else
                {
                    eqp.File.DownLoadRecipe = "NG";
                }
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);


                Trx trx = GetTrxValues(string.Format("{0}_UnitRecipeRequest", eqp.Data.NODENO));
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    UnitRecipeRequestBool = true;

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(UnitRecipeRequestReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Unit Recipe Request Reply RecipeID[{2}] HostPPID[{3}] BIT [{4}].",
                            eqpNo, inputData.TrackKey, recipeID, hostPPID, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCUnitRecipeRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_UnitRecipeRequest");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                UnitRecipeRequestBool = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Unit Recipe Request BC Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void UnitRecipeRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Unit Recipe Request Reply TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Recipe Version Request

        public void BCRecipeVersionRequest(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, RecipeVersionRequestTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe Version Request.",
                        eqpNo, inputData.TrackKey));

                    CPCRecipeVersionRequestReply(eBitResult.OFF, inputData, "0", "", "", "");

                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeVersionRequestTimeoutAction), inputData.TrackKey);
                }

                #endregion


                string localRecipeID = inputData[0][0]["LocalRecipeID"].Value.Trim();
                string masterRecipeID = inputData[0][0]["MasterRecipeID"].Value.Trim();

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Version Request LocalRecipeID=[{2}] MasterRecipeID=[{3}].",
                     eq.Data.NODENO, inputData.TrackKey, localRecipeID, masterRecipeID));

                //优先使用LocalRecipeID
                if (!string.IsNullOrEmpty(localRecipeID) && localRecipeID != "0" || !string.IsNullOrEmpty(masterRecipeID))
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                ObjectManager.RecipeManager.ReloadRecipeByNo();


                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDicByID =
                  ObjectManager.RecipeManager.ReloadRecipe();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(localRecipeID))
                    {
                        RecipeEntityData recipe = recipeDic[eq.Data.LINEID][localRecipeID];
                        CPCRecipeVersionRequestReply(eBitResult.ON, inputData, recipe.OPERATORID, recipe.RECIPENO, recipe.RECIPEID, "0");


                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply.",
                                eq.Data.NODENO, inputData.TrackKey));

                    }
                    else if (recipeDicByID[eq.Data.LINEID].ContainsKey(localRecipeID))
                    {
                        RecipeEntityData recipeByID = recipeDicByID[eq.Data.LINEID][localRecipeID];
                        CPCRecipeVersionRequestReply(eBitResult.ON, inputData, recipeByID.OPERATORID, recipeByID.RECIPEID, recipeByID.RECIPENAME, "0");

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply.",
                                eq.Data.NODENO, inputData.TrackKey));

                    }
                    else
                    {
                        CPCRecipeVersionRequestReply(eBitResult.ON, inputData, "0", localRecipeID, masterRecipeID, "1");

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply LocalRecipeID=[{2}] And MasterRecipeID=[{3}] Not exist.",
                                eq.Data.NODENO, inputData.TrackKey, localRecipeID, masterRecipeID));
                    }



                }
                else
                {

                    CPCRecipeVersionRequestReply(eBitResult.ON, inputData, "0", localRecipeID, masterRecipeID, "1");

                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply MasterRecipeID=[{2}] And LocalRecipeID=[{3}] Invalid.",
                            eq.Data.NODENO, inputData.TrackKey, masterRecipeID, localRecipeID));
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }
        private void BCRecipeVersionRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Version Request TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCRecipeVersionRequestReply(eBitResult result, Trx inputData, string RecipeVerSeqNumber, string LocalRecipeID, string MasterRecipeID, string returnCode)
        {
            try
            {
                Trx trx = null;
                trx = GetTrxValues("L3_RecipeVersionRequestReply");

                string eqpNo = trx.Metadata.NodeNo;
                string timerID = string.Empty;
                timerID = string.Format("{0}_{1}", eqpNo, RecipeVersionRequestReplyTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    trx[0][1][0].Value = "0";
                    if (inputData == null)
                    {
                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    }
                    else
                    {
                        trx.TrackKey = inputData.TrackKey;
                    }
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Recipe Version Request Reply.",
                            eqpNo, trx.TrackKey));

                    return;
                }

                trx[0][0]["MasterRecipeID"].Value = MasterRecipeID;//inputData[0][0]["MasterRecipeID"].Value.Trim();
                trx[0][0]["LocalRecipeID"].Value = LocalRecipeID;//inputData[0][0]["LocalRecipeID"].Value.Trim();
                trx[0][0][2].Value = RecipeVerSeqNumber;
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);

                if (line.Data.FABTYPE != "CF")
                {
                    if (returnCode == "0")//OK
                    {
                        trx[0][0][3].Value = returnCode;
                    }
                    else if (returnCode == "1")//NG
                    {
                        trx[0][0][3].Value = returnCode;
                    }

                }

                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);


                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeVersionRequestReplyTimeoutAction), inputData.TrackKey);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] SET BIT=[{2}].",
                        eqpNo, inputData.TrackKey, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void CPCRecipeVersionRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Version Request Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCRecipeVersionRequestReply(eBitResult.OFF, null, "0", "", "", "");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Job Data Change Report
        public void UnitEditGlassReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 1;
                Job job = null;

                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (it.Value == "1")
                    {
                        if (eqp.File.PositionJobs.ContainsKey(i))
                        {
                            job = eqp.File.PositionJobs[i];

                            //更新编辑的资料
                            Trx jobData = GetTrxValues(string.Format("L3_EQDPositionGlassChangeReport#{0}", (i).ToString().PadLeft(2, '0')));
                            UpdateJobData(eqp, job, jobData);

                            eqp.File.PositionJobs[i] = job;

                            ObjectManager.JobManager.AddJob(job);
                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                            //上报编辑事件
                            JobDataEditReport(eqp, job, i);
                        }
                        else
                        {
                            //Position 不存在玻璃但是有编辑资料
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                              string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Edit Glass Report,But Position[{2}] Job Not Exsit! ",
                                  eqpNo, inputData.TrackKey, i));
                        }
                    }
                    else
                    {


                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void JobDataEditReport(Equipment eq, Job job, int position)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_JobDataEditReport", eq.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_JobDataEditReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, JobDataEditReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }


                SetJobData(eq, job, trx);


                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(JobDataEditReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][1][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                //资料编辑记录
                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Edit.ToString(),
                    eq.Data.NODEID,
                    eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Data Edit Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void JobDataEditReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx trx = GetTrxValues("L3_JobDataEditReport");
                trx.TrackKey = trackKey;
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Job Data Edit Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCJobDataEditReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, JobDataEditReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Job Data Edit Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                Trx trx = GetTrxValues(string.Format("{0}_JobDataEditReport", eqp.Data.NODENO));
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(JobDataEditReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Store Glass Data Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void JobDataEditReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Job Data Edit Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Removed Job Report
        public void UnitRemovedGlassReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 1;
                Job job = null;

                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (it.Value == "1")
                    {
                        if (eqp.File.PositionJobs.ContainsKey(i))
                        {
                            job = eqp.File.PositionJobs[i];

                            //移除Position Job防止上报Fetch事件
                            eqp.File.PositionJobs.TryRemove(i, out job);

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                            RemovedJobReport(eqp, job, i, eBitResult.ON);
                        }
                        else
                        {
                            //Position 不存在玻璃但是有编辑资料
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                              string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Removed Job Report,But Position[{2}] Job Not Exsit! ",
                                  eqpNo, inputData.TrackKey, i));
                        }
                    }
                    else
                    {


                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void RemovedJobReport(Equipment eq, Job job, int position, eBitResult eBitResult)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_RemovedJobReport", eq.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_RemovedJobReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, RemovedJobReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    trx[0][2][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Removed Job Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "OFF"));
                    return;
                }

                SetJobData(eq, job, trx);

                trx[0][1][0].Value = "1";
                trx[0][1][1].Value = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[position].UnitNo;
                trx[0][1][2].Value = "0";
                trx[0][1][3].Value = "0";
                trx[0][1][4].Value = "0";
                trx[0][1][5].Value = "2";




                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(RemovedJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][2][0].Value = "1";
                    trx[0][2].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][2][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                //标记此玻璃为Remove
                job.RemoveFlag = true;
                ObjectManager.JobManager.AddJob(job);

                //删除资料记录
                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Remove.ToString(),
                  eq.Data.NODEID,
                  eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Removed Job Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void RemovedJobReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx trx = GetTrxValues("L3_RemovedJobReport");
                trx[0][2][0].Value = "0";
                trx.TrackKey = trackKey;
                SendToPLC(trx);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Removed Job Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCRemovedJobReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, RemovedJobReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Removed Job Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                RemovedJobReport(eqp, null, 0, eBitResult.OFF);


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(RemovedJobReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Removed Job Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void RemovedJobReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Removed Job Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Recovered Job Report
        public void UnitRecoveryGlassReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 1;
                Job job = null;

                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (it.Value == "1")
                    {
                        Trx readData = GetTrxValues(string.Format("L3_EQDJobDataRequest"));

                        if (readData[0][0][3].Value == "1")
                        {
                            job = ObjectManager.JobManager.GetJob(readData[0][0][0].Value.Trim(), readData[0][0][1].Value.Trim());
                        }
                        else
                        {
                            job = ObjectManager.JobManager.GetJob(readData[0][0][2].Value.Trim());
                        }
                        if (job != null)
                        {
                            Trx jobDataReply = GetTrxValues(string.Format("L3_EQDPositionGlassChangeReport#{0}", (i).ToString().PadLeft(2, '0')));
                            //写入回复的Job,置ON玻璃在席
                            SetJobData(eqp, job, jobDataReply);
                            jobDataReply.TrackKey = UtilityMethod.GetAgentTrackKey();
                            //写配方号
                            Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                            Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                            if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                            {
                                _recipeEntitys = ObjectManager.RecipeManager.ReloadRecipeByNo();
                            }
                            else
                            {
                                _recipeEntitys = ObjectManager.RecipeManager.ReloadRecipe();
                            }

                            if (_recipeEntitys != null && _recipeEntitys.Count > 0)
                            {
                                if (_recipeEntitys[eq.Data.LINEID] != null && _recipeEntitys[eq.Data.LINEID].Count > 0)
                                {
                                    string PPID = string.Empty;

                                    if (line.Data.FABTYPE == "ARRAY")
                                    {
                                        PPID = job.PPID.Substring(26, 26).Trim();
                                    }
                                    else if (line.Data.FABTYPE == "CF")
                                    {
                                        if (line.Data.ATTRIBUTE == "CLN")
                                        {
                                            PPID = job.PPID.Substring(28, 4).Trim();
                                        }
                                        else if (line.Data.ATTRIBUTE == "DEV")
                                        {
                                            PPID = job.PPID.Substring(52, 4).Trim();
                                        }
                                    }
                                    else if (line.Data.FABTYPE == "CELL")
                                    {
                                        PPID = job.PPID03.Trim();
                                    }

                                    if (_recipeEntitys[eq.Data.LINEID].ContainsKey(PPID))
                                    {
                                        jobDataReply[0][0]["CurrentRecipe"].Value = _recipeEntitys[eq.Data.LINEID][PPID].RECIPENO;
                                    }
                                }

                            }


                            SendToPLC(jobDataReply);

                            //回复请求结果
                            Trx bitDataReply = GetTrxValues(string.Format("L3_EQDJobDataRequestReply"));
                            bitDataReply[0][1][1].Value = "1";
                            bitDataReply.TrackKey = UtilityMethod.GetAgentTrackKey();
                            SendToPLC(bitDataReply);
                            job.RemoveFlag = false;
                            RecoveredJobReport(eqp, job, i, eBitResult.ON);

                            //资料编辑记录
                            ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Recovery.ToString(),
                                eqp.Data.NODEID,
                                eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Recovered Job Report Reply =[{2}] CassetteNo=[{3}] JobSequenceNo=[{4}] GlassID=[{5}]."
                                , eqp.Data.NODENO, jobDataReply.TrackKey, "ON", job.CassetteSequenceNo, job.JobSequenceNo, job.JobId));

                        }
                        else
                        {
                            //回复请求结果
                            Trx bitDataReply = GetTrxValues(string.Format("L3_EQDJobDataRequestReply"));
                            bitDataReply[0][1][1].Value = "2";
                            bitDataReply.TrackKey = UtilityMethod.GetAgentTrackKey();
                            SendToPLC(bitDataReply);

                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] CassetteNo=[{2}] JobSequenceNo=[{3}] GlassID=[{4}] JobData Not Exist!"
                                , eqp.Data.NODENO, bitDataReply.TrackKey, readData[0][0][0].Value.Trim(), readData[0][0][1].Value.Trim(), readData[0][0][2].Value.Trim()));
                        }

                    }
                    else
                    {


                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void RecoveredJobReport(Equipment eq, Job job, int position, eBitResult eBitResult)
        {
            try
            {
                //Trx trx = GetTrxValues(string.Format("{0}_RecoveredJobReport", eq.Data.NODENO));
                //if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_RecoveredJobReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));
                Trx trx = GetTrxValues(string.Format("{0}_RemovedJobReport", eq.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_RemovedJobReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, RecoveredJobReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (eBitResult == eBitResult.OFF)
                {
                    trx[0][2][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recovered Job Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "OFF"));
                    return;
                }
                SetJobData(eq, job, trx);

                trx[0][1][0].Value = "1";
                trx[0][1][1].Value = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[position].UnitNo;
                trx[0][1][2].Value = "0";
                trx[0][1][3].Value = "0";
                trx[0][1][4].Value = "0";
                trx[0][1][5].Value = "3";


                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(RecoveredJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][2][0].Value = "1";
                    trx[0][2].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][2][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recovered Job Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void RecoveredJobReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //Trx trx = GetTrxValues("L3_RecoveredJobReport");
                Trx trx = GetTrxValues("L3_RemovedJobReport");
                trx[0][2][0].Value = "0";
                trx.TrackKey = trackKey;
                SendToPLC(trx);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recovered Job Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCRecoveredJobReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, RecoveredJobReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Recovered Job Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                RecoveredJobReport(eqp, null, 0, eBitResult.OFF);


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(RecoveredJobReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recovered Job Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void RecoveredJobReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recovered Job Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Unit Scrap Glass Report 
        public void UnitScrapGlassReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 0;
                Job job = null;

                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (it.Value == "1")
                    {
                        if (eqp.File.PositionJobs.ContainsKey(i + 1))
                        {
                            job = eqp.File.PositionJobs[i + 1];

                            //破片资料记录
                            ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Scrap.ToString(),
                              eqp.Data.NODEID,
                              eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

                            //移除破片Job
                            eqp.File.PositionJobs.TryRemove(i + 1, out job);

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                        }
                        else
                        {
                            //Position 不存在玻璃但是有编辑资料
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                              string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Scrap Glass Report,But Position[{2}] Job Not Exsit! ",
                                  eqpNo, inputData.TrackKey, i));
                        }
                    }
                    else
                    {


                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Delete Job Report
        public void UnitDeleteGlassReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 1;
                Job job = null;

                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (it.Value == "1")
                    {
                        if (eqp.File.PositionJobs.ContainsKey(i))
                        {
                            job = eqp.File.PositionJobs[i];

                            //移除Position Job防止上报Fetch事件
                            eqp.File.PositionJobs.TryRemove(i, out job);

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                            DeleteJobReport(eqp, job, i, eBitResult.ON);
                        }
                        else
                        {
                            //Position 不存在玻璃但是有编辑资料
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                              string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Removed Job Report,But Position[{2}] Job Not Exsit! ",
                                  eqpNo, inputData.TrackKey, i));
                        }
                    }
                    else
                    {


                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void DeleteJobReport(Equipment eq, Job job, int position, eBitResult eBitResult)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_RemovedJobReport", eq.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_RemovedJobReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, RemovedJobReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    trx[0][2][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Removed Job Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "OFF"));
                    return;
                }

                SetJobData(eq, job, trx);

                trx[0][1][0].Value = "1";
                trx[0][1][1].Value = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[position].UnitNo;
                trx[0][1][2].Value = "0";
                trx[0][1][3].Value = "0";
                trx[0][1][4].Value = "0";
                trx[0][1][5].Value = "1";




                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(DeleteJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][2][0].Value = "1";
                    trx[0][2].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][2][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                //标记此玻璃为Remove
                job.RemoveFlag = true;
                ObjectManager.JobManager.AddJob(job);

                //删除资料记录

                ObjectManager.JobManager.DeleteJob(job);

                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Delete.ToString(),
                  eq.Data.NODEID,
                  eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Removed Job Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void DeleteJobReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx trx = GetTrxValues("L3_RemovedJobReport");
                trx[0][2][0].Value = "0";
                trx.TrackKey = trackKey;
                SendToPLC(trx);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Removed Job Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCDeleteJobReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, RemovedJobReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Removed Job Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                DeleteJobReport(eqp, null, 0, eBitResult.OFF);


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(DeleteJobReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Removed Job Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void DeleteJobReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Removed Job Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Unit Glass Have Report
        public void UnitGlassHaveReport(Trx inputData)
        {
            try
            {
                Thread.Sleep(1000);
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (line.Data.FABTYPE != "CELL")
                {
                    return;
                }

                int i = 1;

                Trx trx = GetTrxValues(string.Format("{0}_TFTGlassCount", eqp.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_TFTGlassCount IN PLCFmt.xml!", eqp.Data.NODENO, eqp.Data.NODENO));
                Dictionary<string, int> dicTmp = new Dictionary<string, int>();
                foreach (Item it in inputData[0][0].Items.AllValues)
                {


                    if (eqp.File.PositionJobs.ContainsKey(i) && it.Value == "1")
                    {
                        Job job = eqp.File.PositionJobs[i];
                        if (dicTmp.ContainsKey(job.Master_Recipe_ID))
                        {
                            dicTmp[job.Master_Recipe_ID]++;
                        }
                        else
                        {
                            dicTmp.Add(job.Master_Recipe_ID, 1);
                        }


                    }
                    else
                    {

                    }
                    i++;
                }

                if (dicTmp.Count > 0)
                {
                    for (int j = 0; j < dicTmp.Count; j++)
                    {
                        if (j > 1)
                        {
                            break;
                        }
                        if (j == 0)
                        {
                            trx[0][0][0].Value = dicTmp.ElementAt(j).Key;
                            trx[0][0][1].Value = dicTmp.ElementAt(j).Value.ToString();
                        }
                        else if (j == 1)
                        {
                            trx[0][0][2].Value = dicTmp.ElementAt(j).Key;
                            trx[0][0][3].Value = dicTmp.ElementAt(j).Value.ToString();
                        }
                    }
                }
                else
                {
                    trx.ClearTrx();
                }

                //写入PLC
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Unit Scrap Delete Glass Report   破片要让上游补片
        public void UnitScrapDeleteGlassReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 0;
                Job job = null;

                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (it.Value == "1")
                    {
                        if (eqp.File.PositionJobs.ContainsKey(i + 1))
                        {
                            job = eqp.File.PositionJobs[i + 1];

                            if (!string.IsNullOrEmpty(job.Lot_ID.Trim()))
                            {
                                //读取joblotCount 
                                Trx trx = GetTrxValues(string.Format("{0}_JobLotCountReport", eqp.Data.NODENO));

                                if (eqp.File.PortLot01 == eqp.File.PortLot02)
                                {
                                    trx[0][0][1].Value = (int.Parse(trx[0][0][1].Value) + 1).ToString();

                                    trx.TrackKey = inputData.TrackKey;

                                    SendToPLC(trx);
                                }
                                else
                                {
                                    if (job.Lot_ID.Trim() == eqp.File.PortLot01)
                                    {
                                        trx[0][0][1].Value = (int.Parse(trx[0][0][1].Value) + 1).ToString();

                                        trx.TrackKey = inputData.TrackKey;

                                        SendToPLC(trx);
                                    }
                                    else if (job.Lot_ID.Trim() == eqp.File.PortLot02)
                                    {
                                        trx[0][0][3].Value = (int.Parse(trx[0][0][3].Value) + 1).ToString();

                                        trx.TrackKey = inputData.TrackKey;

                                        SendToPLC(trx);
                                    }
                                    else
                                    {

                                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Scrap Delete Glass Report,But Job LotID[{2}]  Not Exsit! Port#01[{3}]  Port#02[{4}]",
                                        eqpNo, inputData.TrackKey, job.Lot_ID, eqp.File.PortLot01, eqp.File.PortLot02));
                                    }
                                }

                            }
                            //破片除账资料记录
                            ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Delete.ToString(),
                              eqp.Data.NODEID,
                              eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

                            //移除破片Job
                            eqp.File.PositionJobs.TryRemove(i + 1, out job);

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                        }
                        else
                        {
                            //Position 不存在玻璃但是有编辑资料
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                              string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Scrap Delete Glass Report,But Position[{2}] Job Not Exsit! ",
                                  eqpNo, inputData.TrackKey, i));
                        }
                    }
                    else
                    {


                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Load Job Lot Count Report

        public void LoadJobLotCount(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eq.File.PortLot01 = inputData[0][0]["LotID#01"].Value.Trim();
                eq.File.PortLot02 = inputData[0][0]["LotID#02"].Value.Trim();
                eq.File.PortLotCount01 = inputData[0][0]["CurrentCount#01"].Value.Trim();
                eq.File.PortLotCount02 = inputData[0][0]["CurrentCount#02"].Value.Trim();

                Trx trx = GetTrxValues(string.Format("{0}_JobLotCountReport", eq.Data.NODENO));

                trx[0][0][0].Value = eq.File.PortLot01;
                trx[0][0][2].Value = eq.File.PortLot02;
                if (eq.File.PortLotCount01 == "0")
                {
                    trx[0][0][1].Value = "0";
                }
                if (eq.File.PortLotCount02 == "0")
                {
                    trx[0][0][3].Value = "0";
                }
                trx.TrackKey = inputData.TrackKey;

                SendToPLC(trx);


                ObjectManager.EquipmentManager.EnqueueSave(eq.File);

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }


        #endregion

        #region Unit Tack Time Report
        public void UnitTackTimeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 1;
                Job job = null;
                Dictionary<int, TactTimeEntityData> TactDataDic = ObjectManager.EquipmentManager.GetTackTimeData(eqp.Data.LINEID);
                foreach (Item it in inputData[0][0].Items.AllValues)
                {
                    if (i > TactDataDic.Count)
                    {
                        continue;
                    }

                    TactTimeEntityData tack = TactDataDic[i];

                    if (it.Value == "1")
                    {

                        if (eqp.File.PositionJobs.ContainsKey(tack.PositionNo))
                        {
                            job = eqp.File.PositionJobs[tack.PositionNo];
                            if (job.processFlow == null)
                            {
                                job.processFlow = new ProcessFlow();
                            }
                            ProcessFlow pf = new ProcessFlow();
                            pf.MachineName = tack.TackName;
                            pf.StartTime = DateTime.Now;
                            if (!job.processFlow.UnitProcessFlows.ContainsKey(tack.TackName))
                            {
                                job.processFlow.UnitProcessFlows.Add(tack.TackName, pf);
                            }

                        }
                        else
                        {
                            //Position 不存在玻璃但是有编辑资料
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                              string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Unit Tack Time Report,But Position[{2}] Job Not Exsit! ",
                                  eqpNo, inputData.TrackKey, tack.PositionNo));
                        }
                    }
                    else
                    {
                        if (eqp.File.PositionJobs.ContainsKey(tack.PositionNo))
                        {
                            job = eqp.File.PositionJobs[tack.PositionNo];

                            if (job.processFlow == null)
                            {
                                job.processFlow = new ProcessFlow();
                            }

                            if (job.processFlow.UnitProcessFlows.ContainsKey(tack.TackName))
                            {
                                job.processFlow.UnitProcessFlows[tack.TackName].EndTime = DateTime.Now;
                            }
                            else
                            {
                                ProcessFlow pf = new ProcessFlow();
                                pf.MachineName = tack.TackName;
                                pf.StartTime = DateTime.Now;
                                pf.EndTime = DateTime.Now.AddSeconds(60);
                                job.processFlow.UnitProcessFlows.Add(tack.TackName, pf);

                            }

                        }
                        else
                        {
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Unit Tack Time Report,But Position[{2}] Job Not Exsit! ",
                                     eqpNo, inputData.TrackKey, tack.PositionNo));
                        }
                        //else if (job.processFlow != null && job.processFlow.UnitProcessFlows != null && job.processFlow.UnitProcessFlows.ContainsKey(tack.TackName))
                        //{
                        //    job.processFlow.UnitProcessFlows[tack.TackName].EndTime = DateTime.Now;
                        //}

                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void TactTimeDataReport(Equipment eq, Job job, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_TactTimeDataReport", eq.Data.NODENO));


                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_TactTimeDataReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, TactTimeDataReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    trx.ClearTrxWith0();
                    trx[0][2][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Tact Time Data Report.",
                            eq.Data.NODENO, trx.TrackKey));

                    TackTimeReportFlag = true;

                    return;
                }

                trx[0][0][0].Value = job.JobId;

                //if (eq.Data.NODENAME != "CLEANER")
                //{
                trx[0][0][1].Value = "1";
                trx[0][0][2].Value = eq.File.CurrentRecipeNo.ToString();
                trx[0][0][3].Value = "";
                // }
                //else {
                //    trx[0][0][1].Value = "1";
                //    trx[0][0][2].Value = "0";
                //    trx[0][0][3].Value = eq.File.CurrentRecipeID;
                //}

                StringBuilder sb = new StringBuilder();

                if (job.processFlow != null && job.processFlow.ExtendUnitProcessFlows.Count > 0)
                {
                    foreach (ProcessFlow key in job.processFlow.ExtendUnitProcessFlows)
                    {
                        if (key.MachineName == "1")
                        {
                            trx[0][1]["UnitNo#01"].Value = Math.Round((key.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#01", trx[0][1]["UnitNo#01"].Value);
                        }
                        else if (key.MachineName == "2")
                        {
                            trx[0][1]["UnitNo#02"].Value = Math.Round((key.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#02", trx[0][1]["UnitNo#02"].Value);
                        }
                        else if (key.MachineName == "3")
                        {
                            key.EndTime = key.EndTime == DateTime.MinValue ? DateTime.Now : key.EndTime;

                            trx[0][1]["UnitNo#03"].Value = Math.Round((key.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#03", trx[0][1]["UnitNo#03"].Value);
                        }

                        if (key.MachineName == "4")
                        {
                            trx[0][1]["UnitNo#04"].Value = Math.Round((key.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#04", trx[0][1]["UnitNo#04"].Value);
                        }
                        else if (key.MachineName == "5")
                        {
                            trx[0][1]["UnitNo#05"].Value = Math.Round((key.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#05", trx[0][1]["UnitNo#05"].Value);
                        }
                        else if (key.MachineName == "6")
                        {
                            trx[0][1]["UnitNo#06"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#06", trx[0][1]["UnitNo#06"].Value);
                        }
                        else if (key.MachineName == "7")
                        {
                            trx[0][1]["UnitNo#07"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#07", trx[0][1]["UnitNo#07"].Value);
                        }
                        else if (key.MachineName == "8")
                        {
                            trx[0][1]["UnitNo#08"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#08", trx[0][1]["UnitNo#08"].Value);
                        }
                        else if (key.MachineName == "9")
                        {
                            trx[0][1]["UnitNo#09"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#09", trx[0][1]["UnitNo#09"].Value);
                        }
                        else if (key.MachineName == "10")
                        {
                            trx[0][1]["UnitNo#10"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#10", trx[0][1]["UnitNo#10"].Value);
                        }
                        else if (key.MachineName == "11")
                        {
                            trx[0][1]["UnitNo#11"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#11", trx[0][1]["UnitNo#11"].Value);
                        }
                        else if (key.MachineName == "12")
                        {
                            trx[0][1]["UnitNo#12"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#12", trx[0][1]["UnitNo#12"].Value);
                        }
                        else if (key.MachineName == "13")
                        {
                            trx[0][1]["UnitNo#13"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#13", trx[0][1]["UnitNo#13"].Value);
                        }
                        else if (key.MachineName == "14")
                        {
                            trx[0][1]["UnitNo#14"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#14", trx[0][1]["UnitNo#14"].Value);
                        }
                        else if (key.MachineName == "15")
                        {
                            trx[0][1]["UnitNo#15"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#15", trx[0][1]["UnitNo#15"].Value);
                        }
                        else if (key.MachineName == "16")
                        {
                            trx[0][1]["UnitNo#16"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#16", trx[0][1]["UnitNo#16"].Value);
                        }
                        else if (key.MachineName == "17")
                        {
                            trx[0][1]["UnitNo#17"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#17", trx[0][1]["UnitNo#17"].Value);
                        }
                        else if (key.MachineName == "18")
                        {
                            trx[0][1]["UnitNo#18"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#18", trx[0][1]["UnitNo#18"].Value);
                        }
                        else if (key.MachineName == "19")
                        {
                            trx[0][1]["UnitNo#19"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#19", trx[0][1]["UnitNo#19"].Value);
                        }
                        else if (key.MachineName == "20")
                        {
                            trx[0][1]["UnitNo#20"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#20", trx[0][1]["UnitNo#20"].Value);
                        }
                        else if (key.MachineName == "21")
                        {
                            trx[0][1]["UnitNo#21"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#21", trx[0][1]["UnitNo#21"].Value);
                        }
                        else if (key.MachineName == "22")
                        {
                            trx[0][1]["UnitNo#22"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#22", trx[0][1]["UnitNo#22"].Value);
                        }
                        else if (key.MachineName == "23")
                        {
                            trx[0][1]["UnitNo#23"].Value = Math.Round((job.EndTime - key.StartTime).TotalSeconds).ToString();

                            sb.AppendFormat("{0}={1},", "UnitNo#23", trx[0][1]["UnitNo#23"].Value);
                        }
                    }
                }

                if (job.processFlow != null && job.processFlow.UnitProcessFlows.Count > 0)
                {
                    foreach (string key in job.processFlow.UnitProcessFlows.Keys)
                    {
                        job.processFlow.UnitProcessFlows[key].EndTime = job.processFlow.UnitProcessFlows[key].EndTime == DateTime.MinValue ? DateTime.Now : job.processFlow.UnitProcessFlows[key].EndTime;

                        trx[0][1][key].Value = Math.Round((job.processFlow.UnitProcessFlows[key].EndTime - job.processFlow.UnitProcessFlows[key].StartTime).TotalSeconds).ToString();

                        sb.AppendFormat("{0}={1},", key.ToUpper(), trx[0][1][key].Value);
                    }
                }


                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(TactTimeDataReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][2][0].Value = "1";
                    trx[0][2].OpDelayTimeMS = 1000;
                }
                else
                {
                    trx[0][2][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                ObjectManager.ProcessDataManager.MakeTactDataValuesToFile(job.CassetteSequenceNo.ToString(), job.JobSequenceNo.ToString(), trx.TrackKey, sb.ToString());
                #region Save History
                TactDataHistory processDataHis = new TactDataHistory();
                processDataHis.CASSETTESEQNO = job.CassetteSequenceNo;
                processDataHis.JOBID = job.JobId;
                processDataHis.JOBSEQNO = job.JobSequenceNo;
                processDataHis.PROCESSTIME = int.Parse(Math.Round((job.EndTime - job.StartTime).TotalSeconds).ToString());
                processDataHis.LOCALPROCESSSTARTTIME = job.StartTime.ToString();
                processDataHis.LOCALPROCSSSENDTIME = job.EndTime.ToString();

                processDataHis.UPDATETIME = DateTime.Now;
                processDataHis.FILENAMA = string.Format("{0}_{1}_{2}", job.CassetteSequenceNo.ToString(), job.JobSequenceNo.ToString(), trx.TrackKey);
                ObjectManager.ProcessDataManager.SaveTactDataHistory(processDataHis);
                #endregion



                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}]  Tact Time Data Report =[{2}].", eq.Data.NODENO, trx.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void TactTimeDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(sArray[0]);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", sArray[0]));
                    return;
                }
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Tact Time Data Report TIMEOUT.", sArray[0], trackKey));

                TactTimeDataReport(eqp, null, eBitResult.OFF);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCTactTimeDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, TactTimeDataReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Tact Time Data Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                TactTimeDataReport(eqp, null, eBitResult.OFF);


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(TactTimeDataReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Tact Time Data Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void TactTimeDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Tact Time Data Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Process Start JobReport
        private void ProcessStartJobReport(Equipment eq, Job job, eBitResult result, string pathNo, string unitNo)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_ProcessStartJobReport#{1}", eq.Data.NODENO, pathNo));


                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_ProcessStartJobReport{2} IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO, pathNo));

                string timerID = string.Format("{0}_{1}_{2}", eq.Data.NODENO, pathNo, ProcessStartJobReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Process Start Job Report#{2}.",
                            eq.Data.NODENO, trx.TrackKey, pathNo));

                    return;
                }
                job.ProStratTime = DateTime.Now;

                trx[0][0][0].Value = job.Cassette_Sequence_No;
                trx[0][0][1].Value = job.Job_Sequence_No;
                trx[0][0][2].Value = job.GlassID_or_PanelID;
                trx[0][0][3].Value = unitNo;
                trx[0][0][4].Value = "0";
                trx[0][0][5].Value = "0";
                string processStartTime = job.ProStratTime.ToString("yyyyMMddHHmmss");
                trx[0][0][6].Value = processStartTime.Substring(0, 4);
                trx[0][0][7].Value = processStartTime.Substring(4, 2);
                trx[0][0][8].Value = processStartTime.Substring(6, 2);
                trx[0][0][9].Value = processStartTime.Substring(8, 2);
                trx[0][0][10].Value = processStartTime.Substring(10, 2);
                trx[0][0][11].Value = processStartTime.Substring(12, 2);

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessStartJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = 500;

                    ObjectManager.JobManager.AddJob(job);
                }
                else
                {
                    trx[0][1][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey(); ;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}]  Process Start Job Report#{2} =[{3}].", eq.Data.NODENO, trx.TrackKey, pathNo, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void ProcessStartJobReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(sArray[0]);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", sArray[0]));
                    return;
                }
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Process Start Job Report#{2} TIMEOUT.", sArray[0], trackKey, sArray[1]));

                ProcessStartJobReport(eqp, null, eBitResult.OFF, sArray[1], "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCProcessStartJobReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split('#')[1];
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, ProcessStartJobReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Process Start Job Report Reply#{2}.",
                        eqpNo, inputData.TrackKey, pathNo));
                    return;
                }

                ProcessStartJobReport(eqp, null, eBitResult.OFF, pathNo, "0");


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessStartJobReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Process Start Job Report Reply#{2} BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void ProcessStartJobReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Process Start Job Report Reply#{2} T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey, sArray[1]));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Process End Job Report
        private void ProcessEndJobReport(Equipment eq, Job job, eBitResult result, string pathNo, string unitNo)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_ProcessEndJobReport#{1}", eq.Data.NODENO, pathNo));


                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_ProcessEndJobReport#{2} IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO, pathNo));

                string timerID = string.Format("{0}_{1}_{2}", eq.Data.NODENO, pathNo, ProcessEndJobReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Process End Job Report#{2} .",
                            eq.Data.NODENO, trx.TrackKey, pathNo));

                    return;
                }

                job.ProEndTime = DateTime.Now;

                trx[0][0][0].Value = job.Cassette_Sequence_No;
                trx[0][0][1].Value = job.Job_Sequence_No;
                trx[0][0][2].Value = job.GlassID_or_PanelID;
                trx[0][0][3].Value = unitNo;
                trx[0][0][4].Value = "0";
                trx[0][0][5].Value = "0";
                string processEndTime = job.ProEndTime.ToString("yyyyMMddHHmmss");
                trx[0][0][6].Value = processEndTime.Substring(0, 4);
                trx[0][0][7].Value = processEndTime.Substring(4, 2);
                trx[0][0][8].Value = processEndTime.Substring(6, 2);
                trx[0][0][9].Value = processEndTime.Substring(8, 2);
                trx[0][0][10].Value = processEndTime.Substring(10, 2);
                trx[0][0][11].Value = processEndTime.Substring(12, 2);

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessEndJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = 500;


                    ObjectManager.JobManager.AddJob(job);
                }
                else
                {
                    trx[0][1][0].Value = "0";
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey(); ;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}]  Process End Job Report#{2} =[{3}].", eq.Data.NODENO, trx.TrackKey, pathNo, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void ProcessEndJobReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(sArray[0]);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", sArray[0]));
                    return;
                }
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Process End Job Report#{2} TIMEOUT.", sArray[0], trackKey, sArray[1]));

                ProcessEndJobReport(eqp, null, eBitResult.OFF, sArray[1], "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCProcessEndJobReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split('#')[1];
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, ProcessEndJobReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Process End Job Report Reply#{2}.",
                        eqpNo, inputData.TrackKey, pathNo));
                    return;
                }

                ProcessEndJobReport(eqp, null, eBitResult.OFF, pathNo, "0");


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessEndJobReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Process End Job Report Reply#{2} BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void ProcessEndJobReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Process End Job Report Reply#{2} T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey, sArray[1]));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region SendingGlassDataReport

        private void CPCSendingGlassDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_SendingGlassDataReportTimeout
                                                 //T1超时EC OFF BIT
                Trx trx = GetTrxValues($"L3_SendingGlassDataReport#{sArray[1]}");

                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Sending Glass Data Report BC Reply T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCSendingGlassDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split(new char[] { '#' })[1];

                List<int> pathNos = new List<int>();
                pathNos.Add(int.Parse(pathNo));

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, SendingGlassDataReportReplyTimeout);


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Sending Glass Data Report Reply#{2}.",
                        eqpNo, inputData.TrackKey, pathNo));
                    return;
                }

                SubSendReport(pathNos, eBitResult.OFF);

                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCSendingGlassDataReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Sending Glass Data Report Reply[{2}] BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCSendingGlassDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_SendingGlassDataReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Sending Glass Data Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Receive Glass Data Report

        private void CPCReceiveGlassDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_ReceiveGlassDataReportTimeout

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues($"L3_ReceiveGlassDataReport#{sArray[1]}");
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Reeive Glass Data Report BC Reply{2} T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCReceiveGlassDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                string pathNo = inputData.Name.Split(new char[] { '#' })[1];
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, ReceiveGlassDataReportReplyTimeout);


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Reeive Glass Data Report Reply#{2}.",
                        eqpNo, inputData.TrackKey, pathNo));
                    return;
                }

                CPCReceiveJobDataReport(eBitResult.OFF, int.Parse(pathNo));

                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCReceiveGlassDataReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Receive Glass Report Reply[{2}] BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCReceiveGlassDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_ReceiveGlassDataReportReplyTimeout

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Reeive Glass Data Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Tank History
        //补液记录
        public void TankAddReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 0;


                foreach (Item it in inputData[0][0].Items.AllValues)
                {
                    try
                    {
                        if (it.Value == "1")
                        {
                            if (eqp.File.ADDTankHistory.ContainsKey(i + 1))
                            {
                                //已经记录过补液开始
                            }
                            else
                            {
                                TankHistory tankHis = new TankHistory();
                                tankHis.NODEID = eqp.Data.NODEID;
                                tankHis.TANKID = (i + 1).ToString();
                                tankHis.TANKEVENT = "ADD";
                                tankHis.STARTTIME = DateTime.Now;
                                tankHis.OPERATORID = eqp.File.OprationName;

                                eqp.File.ADDTankHistory.Add(i + 1, tankHis);

                                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                            }
                        }
                        else
                        {    //补液结束并记录
                            if (eqp.File.ADDTankHistory.ContainsKey(i + 1))
                            {
                                TankHistory tankHisEnd = new TankHistory();
                                tankHisEnd = eqp.File.ADDTankHistory[i + 1];
                                Trx readData = GetTrxValues(string.Format("L3_EQDTankAddReport#{0}", (i + 1).ToString().PadLeft(2, '0')));
                                if (readData != null)
                                {
                                    tankHisEnd.ENDTIME = DateTime.Now;
                                    tankHisEnd.QUANTITY = (double.Parse(readData[0][0][0].Value.Trim()) / 10).ToString();
                                    tankHisEnd.TOTALTIME = string.Format("{0:F}", (tankHisEnd.ENDTIME - tankHisEnd.STARTTIME).TotalMinutes);// (tankHisEnd.ENDTIME - tankHisEnd.STARTTIME).TotalMinutes.ToString();
                                    tankHisEnd.SPEED = Math.Round((double.Parse(tankHisEnd.QUANTITY) / double.Parse(tankHisEnd.TOTALTIME)), 2).ToString();


                                }
                                else
                                {
                                    //取值为空直接移除不记录
                                    eqp.File.ADDTankHistory.Remove(i + 1);
                                }

                                eqp.File.ADDTankHistory.Remove(i + 1);
                                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                                ObjectManager.EquipmentManager.SaveTankHistory(tankHisEnd);

                            }
                            else
                            {
                                //tank没有补液
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                        i++;
                        continue;
                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        //排液记录
        public void TankSubtractionReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                int i = 0;


                foreach (Item it in inputData[0][0].Items.AllValues)
                {
                    try
                    {
                        if (it.Value == "1")
                        {
                            if (eqp.File.SubTankHistory.ContainsKey(i + 1))
                            {
                                //已经记录过排液开始
                            }
                            else
                            {
                                TankHistory tankHis = new TankHistory();
                                tankHis.NODEID = eqp.Data.NODEID;
                                tankHis.TANKID = (i + 1).ToString();
                                tankHis.TANKEVENT = "Subtraction";
                                tankHis.STARTTIME = DateTime.Now;
                                tankHis.OPERATORID = eqp.File.OprationName;

                                eqp.File.SubTankHistory.Add(i + 1, tankHis);

                                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                            }
                        }
                        else
                        {    //排液结束并记录
                            if (eqp.File.SubTankHistory.ContainsKey(i + 1))
                            {
                                TankHistory tankHisEnd = new TankHistory();
                                tankHisEnd = eqp.File.SubTankHistory[i + 1];

                                tankHisEnd.ENDTIME = DateTime.Now;
                                tankHisEnd.QUANTITY = "0";
                                tankHisEnd.TOTALTIME = Math.Round((tankHisEnd.ENDTIME - tankHisEnd.STARTTIME).TotalMinutes, 2).ToString();
                                tankHisEnd.SPEED = "0";

                                eqp.File.SubTankHistory.Remove(i + 1);
                                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                                ObjectManager.EquipmentManager.SaveTankHistory(tankHisEnd);



                            }
                            else
                            {
                                //tank没有排液
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                        i++;
                        continue;
                    }

                    i++;
                }


                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region  SV Report
        public void EQDEquipmentSVReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }


                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_TactDataReport", eqp.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX TactDataReport IN PLCFmt.xml!", inputData.Metadata.NodeNo));

                for (int i = 0; i < inputData[0].Events.AllValues.Length; i++)
                {
                    for (int j = 0; j < inputData[0][i].Items.AllValues.Length; j++)
                    {
                        trx[0][1][j].Value = inputData[0][i][j].Value.Trim();
                    }

                }
                trx[0][0][0].Value = "1";
                trx[0][0][1].Value = eqp.File.CurrentRecipeNo.ToString();

                trx[0][1].OpDelayTimeMS = 1000;

                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);


            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        //public void EquipmentSVReport()
        //{

        //    while (true)
        //    {
        //        try
        //        {
        //            Thread.Sleep(1000);
        //            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

        //            if (eq == null)
        //            {
        //                LogError(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("Not found Node =[{0}]", "L3"));
        //                return;
        //            }

        //            Trx inputData = GetTrxValues("L3_EQDEquipmentSVReport");

        //            Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_TactDataReport", eq.Data.NODENO)) as Trx;
        //            if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX TactDataReport IN PLCFmt.xml!", inputData.Metadata.NodeNo));

        //            StringBuilder sb = new StringBuilder();

        //            for (int i = 0; i < inputData[0].Events.AllValues.Length; i++)
        //            {
        //                for (int j = 0; j < inputData[0][i].Items.AllValues.Length; j++)
        //                {
        //                    trx[0][1][j].Value = inputData[0][i][j].Value.Trim();

        //                    sb.Append($"{inputData[0][i][j].Name.Split('/')[0]}={(double.Parse(inputData[0][i][j].Value)/ int.Parse(inputData[0][i][j].Name.Split('/')[1])).ToString("F2")},");
        //                }

        //            }
        //            trx[0][0][0].Value = "1";
        //            trx[0][0][1].Value = eq.File.CurrentRecipeNo.ToString();

        //            trx[0][1].OpDelayTimeMS = 100;

        //            trx.TrackKey = inputData.TrackKey;
        //            SendToPLC(trx);

        //          ObjectManager.ProcessDataManager.MakeSVDataValuesToEXCEL(inputData.TrackKey,sb.ToString());





        //        }
        //        catch (Exception ex)
        //        {
        //            this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

        //            continue;


        //        }
        //    }
        //}

        public void MakeSVDataValuesToEXCEL(Trx trx)
        {
            try
            {
                string timeKey = trx.TrackKey.Trim();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string directoryPath = this.SVLogPath + timeKey.Substring(0, 8);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                FileInfo newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 10)}.xlsx");
                if (!newFile.Exists)
                {

                    newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 10)}.xlsx");
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet;
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        worksheet = package.Workbook.Worksheets.Add($"{timeKey.Substring(0, 8)}");
                        worksheet.Cells.Style.ShrinkToFit = true;
                    }
                    else
                    {
                        worksheet = package.Workbook.Worksheets[0];
                    }


                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "DateTime";
                        worksheet.Cells[2, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Column(1).AutoFit();//自动列宽
                        for (int i = 0; i < trx[0][1].Items.Count; i++)
                        {

                            string SVDataName = trx[0][1][i].Name;//item名
                            string[] saveData = SVDataName.Split('/');//根据'/'分割数据名和数据倍率
                            worksheet.Cells[1, i + 2].Value = saveData[0].Trim();//写入数据名
                            int recieveData = int.Parse(trx[0][1][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[2, i + 2].Value = (recieveData * 1.0 / rate).ToString(ReserveDotCountSet(rate));//写入实际数据做记录，保留两位小数

                            worksheet.Column(i + 2).AutoFit();//自动列宽
                        }
                    }
                    else
                    {
                        worksheet.Cells[worksheet.Dimension.End.Row + 1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        for (int i = 0; i < trx[0][1].Items.Count; i++)
                        {

                            string SVDataName = trx[0][1][i].Name;
                            string[] saveData = SVDataName.Split('/');
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = saveData[0].Trim();
                            int recieveData = int.Parse(trx[0][1][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = (recieveData * 1.0 / rate).ToString(ReserveDotCountSet(rate));

                        }
                    }
                    package.Save();
                }
                DeleteSVData();
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public string SVLogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_SVSavePath))
                {
                    _SVSavePath = $"D:\\KZONELOG\\{eq.Data.LINEID}\\SVData\\";
                }

                return _SVSavePath;
            }
            set
            {
                _SVSavePath = value;
                if (_SVSavePath[_SVSavePath.Length - 1] != '\\')
                {
                    _SVSavePath = _SVSavePath + "\\";
                }
            }
        }
        public void DeleteSVData()
        {
            if (Directory.Exists(_SVSavePath) == false)
            {
                return;
            }
            else
            {
                string[] SVDataDicFiles = Directory.GetDirectories(_SVSavePath);
                foreach (string FilePath in SVDataDicFiles)
                {
                    DateTime checkTime = Directory.GetCreationTime(FilePath).AddDays(15);//设定保存时长，暂定15天
                    if (checkTime < DateTime.Now)//检查当前时间，若超过15天，则删除
                    {
                        Directory.Delete(FilePath, true);//删除该目录及目录下的子文件
                    }
                }
            }
        }

        #endregion

        #region Loading Stop/Start Request
        public void LoadingStopRequestReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}", eqpNo, "LoadingStopRequestReplyTimeout");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Loading Stop Request Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    string t1TimerID = string.Format("{0}_{1}", eqpNo, "LoadingStopRequestTimeout");
                    if (Timermanager.IsAliveTimer(t1TimerID))
                    {
                        Timermanager.TerminateTimer(t1TimerID);
                    }

                    Trx trxLoadingStopRequest = GetTrxValues(string.Format("{0}_LoadingStopRequest", eqp.Data.NODENO));
                    trxLoadingStopRequest[0][1][0].Value = "0";
                    trxLoadingStopRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trxLoadingStopRequest);
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(LoadingStopRequestReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Loading Stop Request Reply BIT [{2}] ReturnCode=[{3}][{4}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][1][0].Value), inputData[0][0][0].Value, inputData[0][0][0].Value == "1" ? "OK" : inputData[0][0][0].Value == "2" ? "NG" : ""));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LoadingStopRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Loading Stop Request Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void LoadingStartRequestReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}", eqpNo, "LoadingStartRequestReplyTimeout");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Loading Start Request Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    string t1TimerID = string.Format("{0}_{1}", eqpNo, "LoadingStartRequestTimeout");
                    if (Timermanager.IsAliveTimer(t1TimerID))
                    {
                        Timermanager.TerminateTimer(t1TimerID);
                    }

                    Trx trxLoadingStartRequest = GetTrxValues(string.Format("{0}_LoadingStartRequest", eqp.Data.NODENO));
                    trxLoadingStartRequest[0][0][0].Value = "0";
                    trxLoadingStartRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trxLoadingStartRequest);
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(LoadingStartRequestReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Loading Stop Request Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LoadingStartRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Loading Start Request Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LoadingStopRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Loading Stop Request T1 TIMEOUT.", sArray[0], trackKey));

                Trx trxLoadingStopRequest = GetTrxValues(string.Format("{0}_LoadingStopRequest", sArray[0]));
                //trxLoadingStopRequest[0][0][0].Value = "2";
                trxLoadingStopRequest[0][1][0].Value = "0";
                trxLoadingStopRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trxLoadingStopRequest);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LoadingStartRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Loading Start Request T1 TIMEOUT.", sArray[0], trackKey));

                Trx trxLoadingStartRequest = GetTrxValues(string.Format("{0}_LoadingStartRequest", sArray[0]));
                trxLoadingStartRequest[0][0][0].Value = "0";
                trxLoadingStartRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trxLoadingStartRequest);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void InlineLoadingStopRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Inline Loading Stop Request Bit Auto Reset.", sArray[0], trackKey));

                Trx trxInlineLoadingStopRequest = GetTrxValues(string.Format("{0}_InlineLoadingStopRequest", sArray[0]));
                trxInlineLoadingStopRequest[0][1][0].Value = "0";
                trxInlineLoadingStopRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trxInlineLoadingStopRequest);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void TransferStopRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Transfer Stop Request Bit Auto Reset.", sArray[0], trackKey));

                Trx trxTransferStopRequest = GetTrxValues(string.Format("{0}_TransferStopRequest", sArray[0]));
                trxTransferStopRequest[0][1][0].Value = "0";
                trxTransferStopRequest.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trxTransferStopRequest);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Job Reservation Request
        public void BCJobReservationRequest(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}", eqpNo, "JobReservationRequestTimeout");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }
                    CPCJobReservationRequestReply(eBitResult.OFF, inputData);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Job Reservation Request.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCJobReservationRequestReply(eBitResult.ON, inputData);
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCJobReservationRequestTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Job Reservation Request BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][1][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCJobReservationRequestReply(eBitResult result, Trx inputData)
        {
            try
            {

                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_JobReservationRequestReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                string timerID = string.Format("{0}_{1}", eqpNo, "JobReservationRequestReplyTimeout");

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                string ngResult = string.Empty;
                if (result == eBitResult.ON)
                {
                    if (eq.File.TotalTFTGlassCount != 0)
                    {
                        //ng
                        trx[0][0]["ReturnCode"].Value = "2";
                        ngResult = "NG Reason: EQ exist glass, wait all glass send to upstream";
                        goto skip;
                    }

                    string requestPPID = inputData[0][0]["RecipeNo"].Value.Trim();

                    if (string.IsNullOrEmpty(requestPPID))
                    {
                        //ng
                        trx[0][0]["ReturnCode"].Value = "2";
                        ngResult = "NG Reason: BC Request PPID is empty";
                        goto skip;
                    }

                    Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                                       ObjectManager.RecipeManager.ReloadRecipeByNo();
                    if (_recipeEntitys.Count == 0 || _recipeEntitys[eq.Data.LINEID].Count == 0)
                    {
                        //ng
                        trx[0][0]["ReturnCode"].Value = "2";
                        ngResult = "NG Reason: CIM PC has no any recipe";
                        goto skip;
                    }

                    if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(requestPPID))
                    {
                        //ng
                        trx[0][0]["ReturnCode"].Value = "2";
                        ngResult = $"NG Reason: BC Request PPID={requestPPID} is not exist";
                        goto skip;
                    }
                    else
                    {
                        RecipeEntityData recipeEntityData = _recipeEntitys[eq.Data.LINEID][requestPPID];
                        IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipeEntityData.FILENAME);
                        string nextGlassUseRunMode = string.Empty;
                        if (paramter != null && paramter.Count != 0)
                        {
                            foreach (var data in paramter)
                            {
                                if (data.Contains("Run_Mode") && data.Split('=').Length > 1)
                                {
                                    nextGlassUseRunMode = data.Split('=')[1].Trim();
                                }

                            }
                            if (nextGlassUseRunMode != string.Empty)
                            {
                                string pauseCommand = nextGlassUseRunMode;

                                if (pauseCommand == "1" || pauseCommand == "2" || pauseCommand == "3" || pauseCommand == "4")
                                {
                                    if (eq.Data.LINEID == "KWF22090R" || eq.Data.LINEID == "KWF22091R")
                                    {
                                        if (eq.File.Status == eEQPStatus.Idle || eq.File.Status == eEQPStatus.Initial)
                                        {
                                            //ok
                                            trx[0][0]["ReturnCode"].Value = "1";
                                            CPCEquipmentRunModeSetCommand(eq, pauseCommand, eBitResult.ON);
                                        }
                                        else
                                        {
                                            //ng
                                            trx[0][0]["ReturnCode"].Value = "2";
                                            ngResult = $"NG Reason: Current Equipment Status={eq.File.Status} can't change run mode, must at idle or initial";
                                        }
                                    }
                                    else
                                    {

                                        if (pauseCommand == "3" || pauseCommand == "4")
                                        {
                                            //ng
                                            trx[0][0]["ReturnCode"].Value = "2";
                                            ngResult = $"NG Reason: This RecipeNo={recipeEntityData.RECIPENO} Run_Mode={pauseCommand} is invalid";
                                        }
                                        else
                                        {
                                            if (eq.File.Status == eEQPStatus.Idle || eq.File.Status == eEQPStatus.Initial)
                                            {
                                                //ok
                                                trx[0][0]["ReturnCode"].Value = "1";
                                                CPCEquipmentRunModeSetCommand(eq, pauseCommand, eBitResult.ON);
                                            }
                                            else
                                            {
                                                //ng
                                                trx[0][0]["ReturnCode"].Value = "2";
                                                ngResult = $"NG Reason: Current Equipment Status={eq.File.Status} can't change run mode, must at idle or initial";
                                            }

                                        }

                                    }

                                }
                                else
                                {
                                    //ng
                                    trx[0][0]["ReturnCode"].Value = "2";
                                    ngResult = $"NG Reason: This RecipeNo={recipeEntityData.RECIPENO} Run_Mode={pauseCommand} is invalid";
                                }
                            }
                            else
                            {
                                //ng
                                trx[0][0]["ReturnCode"].Value = "2";
                                ngResult = $"NG Reason: This RecipeNo={recipeEntityData.RECIPENO} has no Run_Mode parmeter";
                                goto skip;
                            }
                        }
                        else
                        {
                            //ng
                            trx[0][0]["ReturnCode"].Value = "2";
                            ngResult = $"NG Reason: CIM PC has no parameters of recipeNo={recipeEntityData.RECIPENO}";
                            goto skip;
                        }

                    }

                skip:;
                    trx[0][0][0].Value = inputData[0][0][0].Value;
                    trx[0][0][1].Value = inputData[0][0][1].Value;
                    trx[0][0][2].Value = inputData[0][0][2].Value;
                    trx[0][0][3].Value = inputData[0][0][3].Value;
                    trx[0][0][4].Value = inputData[0][0][4].Value;
                    trx[0][0][5].Value = inputData[0][0][7].Value;

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = 200;
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCJobReservationRequestReplyTimeoutAction), trx.TrackKey);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}] {3}.",
                        eqpNo, trx.TrackKey, result.ToString(), ngResult));
                }
                else
                {
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}] {3}.",
                        eqpNo, trx.TrackKey, result.ToString(), ngResult));
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCJobReservationRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Job Reservation Request Reply T2 TIMEOUT.", sArray[0], trackKey));

                CPCJobReservationRequestReply(eBitResult.OFF, null);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCJobReservationRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Job Reservation Request T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Recipe Parameter Download Command
        public void BCRecipeParameterDownloadCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[2].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, "RecipeParameterDownloadCommandTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe Parameter Download Command.",
                        eqpNo, inputData.TrackKey));

                    CPCRecipeParameterDownloadCommandReply(eBitResult.OFF, inputData.TrackKey, "0");

                    return;
                }

                string masterRecipeID = inputData[0][0]["MasterRecipeID"].Value.Trim();
                string localRecipeID = inputData[0][0]["LocalRecipeID"].Value.Trim();
                string flag = inputData[0][0]["ReportType"].Value.Trim();
                string trasactionID = string.Empty;
                trasactionID = inputData[0][0][0].Value.PadLeft(4, '0') + inputData[0][0][1].Value.PadLeft(2, '0') + inputData[0][0][2].Value.PadLeft(2, '0')
                    + inputData[0][0][3].Value.PadLeft(2, '0') + inputData[0][0][4].Value.PadLeft(2, '0') + inputData[0][0][5].Value.PadLeft(2, '0');
                string msg = string.Empty;
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipeByNo();
                }
                else
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipe();
                }

                if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(localRecipeID))
                {
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        if (eq.File.CurrentRecipeNo.ToString() == localRecipeID && eq.File.EquipmentOperationMode == eEQPOperationMode.AUTO)
                        {
                            CPCRecipeParameterDownloadCommandReply(eBitResult.ON, inputData.TrackKey, "2");
                            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Parameter Download Command ,EquipmentOperation=[{2}] Can't Modify ,Current Use LocalRecipeID[{3}].",
                     eq.Data.NODENO, inputData.TrackKey, eq.File.EquipmentOperationMode, eq.File.CurrentRecipeNo));
                            RecipeParameterCheckNGSetBit("BC_Download_Data");
                            return;
                        }
                    }
                    else
                    {
                        if (eq.File.CurrentRecipeID == localRecipeID && eq.File.EquipmentOperationMode == eEQPOperationMode.AUTO)
                        {
                            CPCRecipeParameterDownloadCommandReply(eBitResult.ON, inputData.TrackKey, "2");
                            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Parameter Download Command ,EquipmentOperation=[{2}] Can't Modify ,Current Use LocalRecipeID[{3}].",
                     eq.Data.NODENO, inputData.TrackKey, eq.File.EquipmentOperationMode, eq.File.CurrentRecipeID));
                            RecipeParameterCheckNGSetBit("BC_Download_Data");
                            return;
                        }
                    }

                    if (CheckDatetimeValid(trasactionID, out msg) && RecipeparameterDataCheck(eq, inputData) && (flag == "1" || flag == "2" || flag == "4"))
                    {
                        CPCRecipeParameterDownloadCommandReply(eBitResult.ON, inputData.TrackKey, "1");
                        CPCRecipeParameterDownloadCommand(eq, inputData, eBitResult.ON, recipeDic[eq.Data.LINEID][localRecipeID].RECIPENO);
                    }
                    else
                    {
                        CPCRecipeParameterDownloadCommandReply(eBitResult.ON, inputData.TrackKey, "2");
                        RecipeParameterCheckNGSetBit("BC_Download_Data");
                    }
                }
                else
                {
                    CPCRecipeParameterDownloadCommandReply(eBitResult.ON, inputData.TrackKey, "2");
                    RecipeParameterCheckNGSetBit("BC_Download_Data");
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeParameterDownloadCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Parameter Download Command LocalRecipeID=[{2}] ReportType=[{3}] TrasactionID=[{4}] {5} .",
                     eq.Data.NODENO, inputData.TrackKey, localRecipeID, flag, trasactionID, msg));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCRecipeParameterDownloadCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Recipe Parameter Download Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRecipeParameterDownloadCommandReply(eBitResult result, string trxID, string returnCode)
        {
            try
            {
                Trx trx = null;

                trx = GetTrxValues("L3_RecipeParameterDownloadCommandReply");
                string eqpNo = trx.Metadata.NodeNo;
                if (result == eBitResult.ON)
                {
                    trx[0][0][0].Value = returnCode;
                }
                trx[0][1][0].Value = ((int)result).ToString();
                trx[0][1].OpDelayTimeMS = 500;

                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, "RecipeParameterDownloadCommandReplyTimeout");

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeParameterDownloadCommandReplyTimeoutAction), trxID);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] SET BIT=[{2}] Recipe Parameter Download Command Reply {3}.",
                        eqpNo, trxID, result.ToString(), returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : returnCode == "0" ? "Reset" : "Other"));
                }
                else
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Parameter Download Command Reply SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCRecipeParameterDownloadCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Recipe Parameter Download Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCRecipeParameterDownloadCommandReply(eBitResult.OFF, trackKey, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        private void CPCRecipeParameterDownloadCommand(Equipment eq, Trx inputData, eBitResult bit, string recipeNo)
        {
            Trx sendTrx = GetTrxValues("L3_EQDRecipeIDModifyReport");
            string flag = string.Empty;
            if (inputData[0][0]["ReportType"].Value == "4")
            {
                flag = "2";
            }
            else
            {
                flag = inputData[0][0]["ReportType"].Value;
            }

            string year = inputData[0][0][0].Value;
            string month = inputData[0][0][1].Value;
            string day = inputData[0][0][2].Value;
            string hour = inputData[0][0][3].Value;
            string minute = inputData[0][0][4].Value;
            string second = inputData[0][0][5].Value;

            sendTrx[0][0]["RecipeName"].Value = inputData[0][0]["MasterRecipeID"].Value;
            sendTrx[0][0]["RecipeID"].Value = inputData[0][0]["LocalRecipeID"].Value;
            sendTrx[0][0]["ModifyFlag"].Value = flag;
            sendTrx[0][0]["RecipeNO"].Value = recipeNo;

            sendTrx[0][0]["Year"].Value = year;
            sendTrx[0][0]["Month"].Value = month;
            sendTrx[0][0]["Day"].Value = day;
            sendTrx[0][0]["Hour"].Value = hour;
            sendTrx[0][0]["Minute"].Value = minute;
            sendTrx[0][0]["Second"].Value = second;

            for (int i = 0; i < inputData[0][1].Items.Count; i++)
            {
                if (inputData[0][1][i].Name.Trim() == "Recipe_NO" || inputData[0][1][i].Name.Trim() == "Recipe_ID" || inputData[0][1][i].Name.Trim() == "Recipe_Version" || inputData[0][1][i].Name.Trim() == "Recipe_Name")
                {
                    continue;
                }
                sendTrx[0][1][inputData[0][1][i].Name.Trim()].Value = inputData[0][1][i].Value;
            }
            sendTrx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(sendTrx);

            Trx trxRecipeDownloadCommand = GetTrxValues("L3_EQDRecipeIDModifyCommand");
            trxRecipeDownloadCommand.TrackKey = UtilityMethod.GetAgentTrackKey();
            trxRecipeDownloadCommand[0][0][0].Value = "1";

            SendToPLC(trxRecipeDownloadCommand);

            string timerID = "L3_EQDRecipeIDModifyCommandTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(EQDRecipeIDModifyCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  Recipe Parameter Download Command BIT=[ON].", eq.Data.NODENO, UtilityMethod.GetAgentTrackKey()));
        }


        private void CPCRecipeParameterDownloadCommand(Equipment eq, Trx inputData, eBitResult bit, string recipeNo, string flag)
        {
            Trx sendTrx = GetTrxValues("L3_EQDRecipeIDModifyReport");

            string version = string.Empty;
            if (!string.IsNullOrEmpty(inputData[0][0]["RecipeVersion"].Value) && inputData[0][0]["RecipeVersion"].Value != "000000000000")
            {
                version = "20" + inputData[0][0]["RecipeVersion"].Value;
            }
            else
            {
                version = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            string year = version.Substring(0, 4);
            string month = version.Substring(4, 2);
            string day = version.Substring(6, 2);
            string hour = version.Substring(8, 2);
            string minute = version.Substring(10, 2);
            string second = version.Substring(12, 2);

            sendTrx[0][0]["RecipeID"].Value = inputData[0][0]["RecipeID"].Value;
            sendTrx[0][0]["ModifyFlag"].Value = flag;
            sendTrx[0][0]["RecipeNO"].Value = recipeNo;

            sendTrx[0][0]["Year"].Value = year;
            sendTrx[0][0]["Month"].Value = month;
            sendTrx[0][0]["Day"].Value = day;
            sendTrx[0][0]["Hour"].Value = hour;
            sendTrx[0][0]["Minute"].Value = minute;
            sendTrx[0][0]["Second"].Value = second;

            for (int i = 0; i < inputData[0][1].Items.Count; i++)
            {
                if (inputData[0][1][i].Name.Trim() == "Recipe_NO" || inputData[0][1][i].Name.Trim() == "Recipe_ID" || inputData[0][1][i].Name.Trim() == "Recipe_Version")
                {
                    continue;
                }
                sendTrx[0][1][inputData[0][1][i].Name.Trim()].Value = inputData[0][1][i].Value;
            }
            sendTrx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(sendTrx);

            Trx trxRecipeDownloadCommand = GetTrxValues("L3_EQDRecipeIDModifyCommand");
            trxRecipeDownloadCommand.TrackKey = UtilityMethod.GetAgentTrackKey();
            trxRecipeDownloadCommand[0][0][0].Value = "1";

            SendToPLC(trxRecipeDownloadCommand);

            string timerID = "L3_EQDRecipeIDModifyCommandTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(EQDRecipeIDModifyCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  Recipe Parameter Download Command BIT=[ON].", eq.Data.NODENO, UtilityMethod.GetAgentTrackKey()));
        }

        private void EQDRecipeIDModifyCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx trxRecipeDownloadCommand = GetTrxValues("L3_EQDRecipeIDModifyCommand");
                trxRecipeDownloadCommand.TrackKey = trackKey;
                trxRecipeDownloadCommand[0][0][0].Value = "0";

                SendToPLC(trxRecipeDownloadCommand);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  Recipe Parameter Download Command BIT=[OFF].", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private bool RecipeparameterDataCheck(Equipment eq, Trx inputData)
        {
            //tips:格式  名字(实际合法值下限-实际合法值上限)/倍率
            for (int i = 0; i < inputData[0][1].Items.Count; i++)
            {
                if (inputData[0][1][i].Name.Trim() == "Recipe_NO" || inputData[0][1][i].Name.Trim() == "Recipe_ID" || inputData[0][1][i].Name.Trim() == "Recipe_Version" || inputData[0][1][i].Name.Trim() == "Recipe_Name")
                {
                    continue;
                }
                double rate = double.Parse(inputData[0][1][i].Name.Split('/')[1]);
                double min = 0;
                double max = 0;
                if (inputData[0][1][i].Name.Contains('(') && inputData[0][1][i].Name.Contains(')') && inputData[0][1][i].Name.Contains('-'))
                {
                    int[] indexes = new int[2];
                    for (int j = 0; j < inputData[0][1][i].Name.Length; j++)
                    {
                        if (inputData[0][1][i].Name[j] == '(')
                        {
                            indexes[0] = j;
                        }
                        if (inputData[0][1][i].Name[j] == ')')
                        {
                            indexes[1] = j;
                        }
                    }

                    string strRound = string.Empty;
                    if (indexes[1] - indexes[0] > 3)
                    {
                        strRound = inputData[0][1][i].Name.Substring(indexes[0] + 1, indexes[1] - indexes[0] - 1);
                    }
                    if (!string.IsNullOrEmpty(strRound))
                    {
                        if (strRound.Split('-').Length == 2)
                        {
                            min = double.Parse(strRound.Split('-')[0]);
                            max = double.Parse(strRound.Split('-')[1]);
                        }
                        else if (strRound.Split('-').Length == 3)
                        {
                            min = double.Parse("-" + strRound.Split('-')[1]);
                            max = double.Parse(strRound.Split('-')[2]);
                        }
                        else if (strRound.Split('-').Length == 4)
                        {
                            min = double.Parse("-" + strRound.Split('-')[1]);
                            max = double.Parse("-" + strRound.Split('-')[3]);
                        }
                        else
                        {

                        }

                    }
                }
                double temp = double.Parse(inputData[0][1][i].Value) / rate;
                if (temp < min || temp > max)
                {
                    return false;
                }
            }
            return true;
        }


        private void RecipeParameterCheckNGSetBit(string NG_Data_Type)
        {
            Trx trx = GetTrxValues("L3_RecipeParameterCheckNGBit");
            if (NG_Data_Type == "BC_Download_Data")
            {
                trx[0][0][0].Value = "1";
            }
            //else
            //{
            //    trx[0][0][1].Value = "1";
            //}
            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(trx);
        }
        #endregion

        #region  Recipe Validation Result Send
        public void RecipeValidationResultSend(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}", eqpNo, "RecipeValidationResultSendTimeout");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }
                    CPCRecipeValidationResultSendReply(eBitResult.OFF, inputData);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe Validation Result Send.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCRecipeValidationResultSendReply(eBitResult.ON, inputData);
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(RecipeValidationResultSendTimeoutAction), inputData.TrackKey);
                }

                #endregion

                RecipeValidationResultHistory rECIPEVALIDATIONRESULTHISTORY = new RecipeValidationResultHistory();
                //rECIPEVALIDATIONRESULTHISTORY.NO = 1;
                rECIPEVALIDATIONRESULTHISTORY.RECEIVETIME = DateTime.Now;
                rECIPEVALIDATIONRESULTHISTORY.MASTERRECIPEID = inputData[0][0]["MasterRecipeID"].Value;
                rECIPEVALIDATIONRESULTHISTORY.LOCALRECIPEID = inputData[0][0]["LocalRecipeID"].Value;
                rECIPEVALIDATIONRESULTHISTORY.RMS_RESULT = inputData[0][0]["Result"].Value == "1" ? "OK" : inputData[0][0]["Result"].Value == "2" ? "NG" : $"Other Value:{inputData[0][0]["Result"].Value}";
                rECIPEVALIDATIONRESULTHISTORY.RMS_RESULTTEXT = inputData[0][0]["ResultText"].Value;
                ObjectManager.EquipmentManager.SaveRMSMessageHistory(rECIPEVALIDATIONRESULTHISTORY);
                if (!eqp.File.RecipeValidationMessageDisplay)
                {
                    eqp.File.RecipeValidationMessageDisplay = true;
                }
                else
                {
                    eqp.File.RecipeValidationMessageUpdate = true;
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Validation Result Send BIT [{2}] MasterRecipeID=[{3}] LocalRecipeID=[{4}] Result=[{5}][{6}] ResultText=[{7}].",
                            eqpNo, inputData.TrackKey, bitResult, inputData[0][0][0].Value, inputData[0][0][1].Value, inputData[0][0][2].Value, inputData[0][0][2].Value == "1" ? "OK" : "NG", inputData[0][0][3].Value));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void CPCRecipeValidationResultSendReply(eBitResult eBitResult, Trx inputData)
        {
            try
            {
                Trx trx = GetTrxValues("L3_RecipeValidationResultSendReply");
                trx[0][0][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Validation Result Send Reply BIT [{2}].",
                            "L3", trx.TrackKey, eBitResult));
                string timerID = "L3_CPCRecipeValidationResultSendReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    return;
                }
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCRecipeValidationResultSendReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCRecipeValidationResultSendReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCRecipeValidationResultSendReply(eBitResult.OFF, null);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Validation Result Send Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void RecipeValidationResultSendTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Validation Result Send T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region CV Report
        public void EquipmentCVReport(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (eq.File.CVReportCount > 0)
                {
                    Trx trxReceive = GetTrxValues("L3_EQDCVDataReport");
                    Trx trxSend = GetTrxValues("L3_CVDataReport");
                    if (trxReceive == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_EQDCVDataReport IN PLCFmt.xml!", "L3"));
                    }
                    if (trxSend == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_CVDataReport IN PLCFmt.xml", "L3"));
                    }
                    for (int i = 0; i < trxReceive[0][0].Items.Count; i++)
                    {
                        trxSend[0][0][i].Value = trxReceive[0][0][i].Value;
                    }
                    trxSend.TrackKey = UtilityMethod.GetAgentTrackKey();
                    trxSend[0][1][0].Value = "1";
                    SendToPLC(trxSend);
                    eq.File.CVReportCount--;
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] CV Data Report BIT=[{2}] RemainCVReportCount[{3}] CVReportTime[{4}]",
                                     "L3", trxSend.TrackKey, "ON", eq.File.CVReportCount
                                     , eq.File.CVReportTime));

                    string timerID = "L3_CVDataReportTimeout";
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCCVDataReportTimeoutAction), UtilityMethod.GetAgentTrackKey());
                    MakeCVDataValuesToEXCEL(trxReceive);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCCVDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_CVDataReport");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] CV Data Report T1 TIMEOUT",
                                "L3", trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CVDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                string timerID = "L3_CVDataReportReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                         string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] CV Data Report BIT=[{2}]",
                             "L3", inputData.TrackKey, eBitResult));
                if (eBitResult == eBitResult.OFF)
                {
                    return;
                }
                Trx trx = GetTrxValues("L3_CVDataReport");
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);
                string timerID1 = "L3_CVDataReportTimeout";
                if (Timermanager.IsAliveTimer(timerID1))
                {
                    Timermanager.TerminateTimer(timerID1);
                }
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CVDataReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CVDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                             string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] CV Data Report Reply T2 TIMEOUT",
                                 "L3", trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region CV Report Time Change Command

        public void CVReportTimeChangeCommandSend(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (inputData.IsInitTrigger)
                {
                    if (Timermanager.IsAliveTimer("CVReport"))
                    {
                        Timermanager.TerminateTimer("CVReport");
                    }
                    Timermanager.CreateTimer("CVReport", true, eq.File.CVReportTime * 1000,
                        new System.Timers.ElapsedEventHandler(EquipmentCVReport), UtilityMethod.GetAgentTrackKey());
                    return;
                }


                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }

                string timerID = "L3_BCCVReportTimeChangeCommandTimeout";
                eBitResult bit = (eBitResult)int.Parse(inputData[0][1][0].Value);
                int cvReportCount = int.Parse(inputData[0][0][0].Value);
                int cvReportTime = int.Parse(inputData[0][0][1].Value);
                if (bit == eBitResult.OFF)
                {
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CV Report Time Change Command BIT=[{2}]",
                                     inputData.Metadata.NodeNo, inputData.TrackKey, bit));
                    CPCCVReportTimeChangeCommandReply(0, 0, eBitResult.OFF);
                    return;
                }
                else
                {

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    int oldInterval = eq.File.CVReportTime;

                    CPCCVReportTimeChangeCommandReply(cvReportCount, cvReportTime, eBitResult.ON);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CV Report Time Change Command BIT=[{2}] CVReportCount=[{3}] CVReportTime=[{4}].",
                                     inputData.Metadata.NodeNo, inputData.TrackKey, bit, cvReportCount, cvReportTime));
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCCVReportTimeChangeCommandReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    if (cvReportTime != oldInterval)
                    {
                        if (Timermanager.IsAliveTimer("CVReport"))
                        {
                            Timermanager.TerminateTimer("CVReport");
                        }
                        Timermanager.CreateTimer("CVReport", true, eq.File.CVReportTime * 1000,
                        new System.Timers.ElapsedEventHandler(EquipmentCVReport), UtilityMethod.GetAgentTrackKey());

                    }
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCCVReportTimeChangeCommandReply(int cvReportCount, int cvReportTime, eBitResult bit)
        {
            try
            {
                Trx trx = GetTrxValues("L3_CVReportTimeChangeCommandSendReply");
                if (trx == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_CVReportTimeChangeCommandSendReply IN PLCFmt.xml!", "L3"));
                }
                if (bit == eBitResult.ON)
                {
                    int time = cvReportTime * 1000;
                    if (time < 1000)
                    {
                        trx[0][0][0].Value = "2";
                    }
                    else
                    {
                        trx[0][0][0].Value = "1";
                    }
                    eq.File.CVReportCount = cvReportCount;
                    eq.File.CVReportTime = cvReportTime;
                    ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                }
                trx[0][1][0].Value = ((int)bit).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] CV Report Time Change Command Reply BIT=[{2}] ReturnCode=[{3}][{4}].",
                                     "L3", trx.TrackKey, bit, trx[0][0][0].Value, trx[0][0][0].Value == "1" ? "OK" : "NG"));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void BCCVReportTimeChangeCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BC CV Report Time Change Command Reply T1 Timeout.",
                                     "L3", trackKey));

                CPCCVReportTimeChangeCommandReply(0, 0, eBitResult.OFF);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void MakeCVDataValuesToEXCEL(Trx trx)
        {
            try
            {
                string timeKey = trx.TrackKey.Trim();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string directoryPath = this.CVLogPath + timeKey.Substring(0, 8);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                FileInfo newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 10)}.xlsx");
                if (!newFile.Exists)
                {

                    newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 10)}.xlsx");
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet;
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        worksheet = package.Workbook.Worksheets.Add($"{timeKey.Substring(0, 8)}");
                        worksheet.Cells.Style.ShrinkToFit = true;
                    }
                    else
                    {
                        worksheet = package.Workbook.Worksheets[0];
                    }


                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "DateTime";
                        worksheet.Cells[2, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Column(1).AutoFit();//自动列宽
                        for (int i = 0; i < trx[0][0].Items.Count; i++)
                        {

                            string CVDataName = trx[0][0][i].Name;//item名
                            string[] saveData = CVDataName.Split('/');//根据'/'分割数据名和数据倍率
                            worksheet.Cells[1, i + 2].Value = saveData[0].Trim();//写入数据名
                            int recieveData = int.Parse(trx[0][0][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[2, i + 2].Value = (recieveData * 1.0 / rate).ToString(ReserveDotCountSet(rate));//写入实际数据做记录，保留两位小数

                            worksheet.Column(i + 2).AutoFit();//自动列宽
                        }
                    }
                    else
                    {
                        worksheet.Cells[worksheet.Dimension.End.Row + 1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        for (int i = 0; i < trx[0][0].Items.Count; i++)
                        {

                            string CVDataName = trx[0][0][i].Name;
                            string[] saveData = CVDataName.Split('/');
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = saveData[0].Trim();
                            int recieveData = int.Parse(trx[0][0][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = (recieveData * 1.0 / rate).ToString(ReserveDotCountSet(rate));

                        }
                    }
                    package.Save();
                }
                DeleteCVData();
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public string CVLogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_CVSavePath))
                {
                    _CVSavePath = $"D:\\KZONELOG\\{eq.Data.LINEID}\\CVData\\";
                }

                return _CVSavePath;
            }
            set
            {
                _CVSavePath = value;
                if (_CVSavePath[_CVSavePath.Length - 1] != '\\')
                {
                    _CVSavePath = _CVSavePath + "\\";
                }
            }
        }

        public void DeleteCVData()
        {
            if (Directory.Exists(_CVSavePath) == false)
            {
                return;
            }
            else
            {
                string[] SVDataDicFiles = Directory.GetDirectories(_CVSavePath);
                foreach (string FilePath in SVDataDicFiles)
                {
                    DateTime checkTime = Directory.GetCreationTime(FilePath).AddDays(60);//设定保存时长，暂定60天
                    if (checkTime < DateTime.Now)//检查当前时间，若超过60天，则删除
                    {
                        Directory.Delete(FilePath, true);//删除该目录及目录下的子文件
                    }
                }
            }
        }

        #endregion

        #region CV Data Request Command
        public void CVDataRequestCommand(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }

                string timerID = "L3_BCCVDataRequestCommandTimeout";
                eBitResult bit = (eBitResult)int.Parse(inputData[0][0][0].Value);
                if (bit == eBitResult.OFF)
                {
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CV Data Request Command BIT=[{2}]",
                                     inputData.Metadata.NodeNo, inputData.TrackKey, bit));
                    CPCCVDataRequetCommandReply(eBitResult.OFF);
                    return;
                }
                else
                {

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    CPCCVDataRequetCommandReply(eBitResult.ON);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CV Data Request Command BIT=[{2}].",
                                     inputData.Metadata.NodeNo, inputData.TrackKey, bit));
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CVDataRequestCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    //Trx trxReceive = GetTrxValues("L3_EQDCVDataReport");
                    //Trx trxSend = GetTrxValues("L3_CVDataReport");
                    //if (trxReceive == null)
                    //{
                    //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_EQDCVDataReport IN PLCFmt.xml!", "L3"));
                    //}
                    //if (trxSend == null)
                    //{
                    //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_CVDataReport IN PLCFmt.xml", "L3"));
                    //}
                    //for (int i = 0; i < trxReceive[0][0].Items.Count; i++)
                    //{
                    //    trxSend[0][0][i].Value = trxReceive[0][0][i].Value;
                    //}
                    //trxSend.TrackKey = UtilityMethod.GetAgentTrackKey();
                    //trxSend[0][1][0].Value = "1";
                    //SendToPLC(trxSend);
                    //string timer2ID = "L3_CVDataReportTimeout";
                    //if (Timermanager.IsAliveTimer(timer2ID))
                    //{
                    //    Timermanager.TerminateTimer(timer2ID);
                    //}
                    //Timermanager.CreateTimer(timer2ID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    //    new System.Timers.ElapsedEventHandler(CPCCVDataReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    //MakeCVDataValuesToEXCEL(trxReceive);
                    //LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    //             string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CV Data Report BIT=[{2}].",
                    //                 inputData.Metadata.NodeNo, inputData.TrackKey, bit));
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCCVDataRequetCommandReply(eBitResult eBitResult)
        {
            string timerID = "L3_CVDataRequetCommandReplyTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            Trx trxSend = GetTrxValues("L3_CVDataRequestCommandReply");
            if (eBitResult == eBitResult.OFF)
            {
                trxSend.TrackKey = UtilityMethod.GetAgentTrackKey();
                trxSend[0][1][0].Value = "0";
                SendToPLC(trxSend);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                         string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CV Data Request Command Reply BIT=[{2}].",
                             "L3", UtilityMethod.GetAgentTrackKey(), eBitResult));
                return;
            }

            Trx trxReceive = GetTrxValues("L3_EQDCVDataReport");
            if (trxReceive == null)
            {
                throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_EQDCVDataReport IN PLCFmt.xml!", "L3"));
            }
            for (int i = 0; i < trxReceive[0][0].Items.Count; i++)
            {
                trxSend[0][0][i].Value = trxReceive[0][0][i].Value;
            }
            trxSend.TrackKey = UtilityMethod.GetAgentTrackKey();
            trxSend[0][1][0].Value = "1";
            SendToPLC(trxSend);

            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                new System.Timers.ElapsedEventHandler(CPCCVDataRequestCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

            MakeCVDataValuesToEXCEL(trxReceive);
            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                         string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CV Data Request Command Reply BIT=[{2}].",
                             trxSend.Metadata.NodeNo, trxSend.TrackKey, eBitResult));


        }

        private void CVDataRequestCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BC CV Data Request Command T1 Timeout.",
                                     "L3", trackKey));

                CPCCVDataRequetCommandReply(eBitResult.OFF);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCCVDataRequestCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                 string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BC CV Data Request Command Reply T2 Timeout.",
                                     "L3", trackKey));

                CPCCVDataRequetCommandReply(eBitResult.OFF);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Set Last Glass Command 

        public void SetLastGlassCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, "SetLastGlassCommandTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  Set Last Glass Command.",
                        eqpNo, inputData.TrackKey));

                    CPCSetLastGlassCommandReply(eBitResult.OFF, inputData.TrackKey, "0");

                    return;
                }

                string glassID = inputData[0][0][0].Value.Trim();
                string cstNo = inputData[0][0][1].Value.Trim();
                string slotNo = inputData[0][0][2].Value.Trim();
                string commandType = inputData[0][0][3].Value.Trim();
                Job job = ObjectManager.JobManager.GetJob(cstNo, slotNo);
                if (job != null)
                {
                    if (job.GlassID_or_PanelID.Trim() == glassID)
                    {
                        CPCSetLastGlassCommandReply(eBitResult.ON, inputData.TrackKey, "1");
                    }
                    else
                    {
                        CPCSetLastGlassCommandReply(eBitResult.ON, inputData.TrackKey, "2");
                    }
                }
                else
                {
                    CPCSetLastGlassCommandReply(eBitResult.ON, inputData.TrackKey, "2");
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {


                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(SetLastGlassCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Set Last Glass Command GlassID=[{2}] CstNo=[{3}] SlotNo=[{4}] CommandType=[{5}][{6}].",
                     eq.Data.NODENO, inputData.TrackKey, glassID, cstNo, slotNo, commandType, commandType == "1" ? "Set Last Glass" : "Set Pair Glass"));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void SetLastGlassCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Set Last Glass Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCSetLastGlassCommandReply(eBitResult result, string trxID, string returnCode)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_SetLastGlassCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = returnCode;
                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, "SetLastGlassCommandReplyTimeout");

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCSetLastGlassCommandReplyTimeoutAction), trxID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] SET BIT=[{2}] ReturnCode=[{3}][{4}].",
                        eqpNo, trxID, result.ToString(), returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : ""));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCSetLastGlassCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Set Last Glass Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCSetLastGlassCommandReply(eBitResult.OFF, trackKey, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        #endregion

        #region Loading Stop Command
        public void LoadingStopCommand(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                if (inputData.IsInitTrigger)
                {
                    BCStopCommandSatusCheck();
                    return;
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string t2TimerID = string.Format("{0}_{1}", eqpNo, "LoadingStopCommandTimeout");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }
                    CPCLoadingStopCommandReply(eBitResult.OFF, "0");
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Loading Stop Command.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                int loadingStopCommandStatus = int.Parse(inputData[0][0][0].Value);
                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                //if (eqp.File.LoadingStopCommandStatus == loadingStopCommandStatus)
                //{
                //    CPCLoadingStopCommandReply(eBitResult.ON, "2");
                //}
                //else
                //{
                if (loadingStopCommandStatus == 1 || loadingStopCommandStatus == 2)
                {
                    CPCLoadingStopCommandReply(eBitResult.ON, "1");
                    eqp.File.LoadingStopCommandStatus = loadingStopCommandStatus;
                    eqp.File.StopCommand = loadingStopCommandStatus == 1 ? eBitResult.ON : eBitResult.OFF;
                    ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                    if (eqp.File.StopCommand == eBitResult.ON)
                    {
                        EQDLoadingStopCommand(eBitResult.ON, "1");
                    }
                    else
                    {
                        EQDLoadingStopCommand(eBitResult.ON, "2");
                    }
                }
                else
                {
                    CPCLoadingStopCommandReply(eBitResult.ON, "2");
                }

                //}
                Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(LoadingStopCommandTimeoutAction), inputData.TrackKey);

                BCStopCommandSatusCheck();
                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Loading Stop Command BIT [{2}] LoadingStopStatus=[{3}][{4}].",
                            eqpNo, inputData.TrackKey, bitResult, loadingStopCommandStatus, loadingStopCommandStatus == 1 ? "Set Loading Stop" : loadingStopCommandStatus == 2 ? "Cleared Loading Stop" : ""));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void LoadingStopCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Loading Stop Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void CPCLoadingStopCommandReply(eBitResult eBitResult, string returnCode)
        {
            try
            {
                Trx trx = GetTrxValues("L3_LoadingStopCommandReply");
                if (eBitResult == eBitResult.ON)
                {
                    //off时不清零
                    trx[0][0][0].Value = returnCode;
                }
                trx[0][1][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                string timerID = "L3_LoadingStopCommandReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Loading Stop Command Reply Bit[{2}] ReturnCode=[{3}][{4}].", "L3", trx.TrackKey, eBitResult, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : ""));
                if (eBitResult == eBitResult.OFF)
                {
                    return;
                }
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCLoadingStopCommandReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCLoadingStopCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Loading Stop Command Reply T1 TIMEOUT.", sArray[0], trackKey));
                CPCLoadingStopCommandReply(eBitResult.OFF, "0");

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void BCStopCommandSatusCheck()
        {
            try
            {
                if (eqp.File.StopCommand == eBitResult.ON)
                {

                    //触发报警
                    Trx trx = GetTrxValues("L3_BCSetLoadingStopCommand");
                    trx[0][0][0].Value = "1";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ_Check][{1}] EAS Loading Stop Command,Wait Release.", eqp.Data.ATTRIBUTE, trx.TrackKey));
                }
                else
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ_Check][{1}] EAS Loading Stop Command Released,EQ Can Receive Glass.", eqp.Data.ATTRIBUTE, UtilityMethod.GetAgentTrackKey()));
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Local Recipe Request
        private void CPCLocalRecipeRequest(string masterRecipeID, eBitResult bitResult)
        {
            try
            {
                Trx trx = GetTrxValues("L3_LocalRecipeRequest");
                string timerID = "L3_LocalRecipeRequestTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.OFF)
                {
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] BIT=[OFF] Local Recipe Request.",
                        "L3", trx.TrackKey));
                    return;
                }
                trx[0][0][0].Value = masterRecipeID;
                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCLocalRecipeRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] BIT=[ON] Local Recipe Request MasterRecipeID=[{2}].",
                        "L3", trx.TrackKey, masterRecipeID));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCLocalRecipeRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Local Recipe Request T1 TIMEOUT.", sArray[0], trackKey));
                CPCLocalRecipeRequest("", eBitResult.OFF);

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void LocalRecipeRequestReply(Trx inputData)
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_LocalRecipeRequestReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Local Recipe Request Bit[{2}].", eq.Data.NODENO, inputData.TrackKey, "OFF"));

                    return;
                }
                string masterRecipeID = inputData[0][0]["MasterRecipeID"].Value.Trim();
                string localRecipeID = inputData[0][0]["LocalRecipeID"].Value.Trim();
                string returnCode = inputData[0][0]["ReturnCode"].Value;
                CPCLocalRecipeRequest(masterRecipeID, eBitResult.OFF);

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(LocalRecipeRequestReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());

                if (returnCode != "1")
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Local Recipe Request Reply Bit[{2}] ReturnCode[{3}][{4}].", eq.Data.NODENO, inputData.TrackKey, "ON", returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG There are no recipe in recipe list" : returnCode == "3" ? "NG There is not data format" : ""));
                    return;
                }



                LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.DownStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Lower").FirstOrDefault();
                string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Send") == true).FirstOrDefault().ToString();
                Trx receviceTrx = GetTrxValues(trxName);
                Trx trx = new Trx();
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (linkSignalType.FlowType.ToUpper().Trim() == "NORMAL")
                {
                    if (linkSignalType.PathPosition.ToUpper().Trim() == "LOWER")
                    {
                        trx = GetTrxValues("L3_EQDReceiveGlassDataReport#01");
                    }
                    else
                    {
                        trx = GetTrxValues("L3_EQDReceiveGlassDataReport#02");
                    }
                }
                else if (linkSignalType.FlowType.ToUpper().Trim() == "RETURN")
                {
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        trx = GetTrxValues("L3_EQDReceiveGlassDataReport#01");
                    }
                }

                for (int i = 0; i < receviceTrx[0][0].Items.Count; i++)
                {
                    trx[0][0][i].Value = receviceTrx[0][0][i].Value;
                }
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeEntitys = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    recipeEntitys = ObjectManager.RecipeManager.ReloadRecipeByNo();
                }
                else
                {
                    recipeEntitys = ObjectManager.RecipeManager.ReloadRecipe();
                }

                if (recipeEntitys[eq.Data.LINEID].Count == 0)
                {
                    trx[0][0]["CurrentRecipe"].Value = "0";
                }
                else
                {
                    if (recipeEntitys[eq.Data.LINEID].ContainsKey(localRecipeID))
                    {
                        trx[0][0]["CurrentRecipe"].Value = recipeEntitys[eq.Data.LINEID][localRecipeID].RECIPENO;
                    }
                    else
                    {
                        trx[0][0]["CurrentRecipe"].Value = "0";
                    }
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Local Recipe Request Reply Bit[{2}] ReturnCode[{3}][{4}].", eq.Data.NODENO, inputData.TrackKey, "ON", returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG There are no recipe in recipe list" : returnCode == "3" ? "NG There is not data format" : ""));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void LocalRecipeRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Local Recipe Request Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region FAC Data Report
        public void FACDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, "FACDataReportReplyTimeout");
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] FAC Data Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                string timerID1 = "L3_FACDataReportTimeout";
                if (Timermanager.IsAliveTimer(timerID1))
                {
                    Timermanager.TerminateTimer(timerID1);
                }
                Trx trx = GetTrxValues("L3_FACDataReport");
                trx[0][1][0].Value = "0";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(FACDataReportReplyTimeoutAction), inputData.TrackKey);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] FAC Data Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
                Thread.Sleep(1000);
                FACReport = true;
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCFACDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx trx = GetTrxValues("L3_FACDataReport");
                trx[0][1][0].Value = "0";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                Thread.Sleep(1000);
                FACReport = true;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void FACDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] FAC Data Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void MakeFACDataValuesToEXCEL(Trx trx)
        {
            try
            {
                string timeKey = trx.TrackKey.Trim();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string directoryPath = this.FACLogPath + timeKey.Substring(0, 8);
                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                FileInfo newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 10)}.xlsx");
                if (!newFile.Exists)
                {

                    newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 10)}.xlsx");
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet;
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        worksheet = package.Workbook.Worksheets.Add($"{timeKey.Substring(0, 8)}");
                        worksheet.Cells.Style.ShrinkToFit = true;
                    }
                    else
                    {
                        worksheet = package.Workbook.Worksheets[0];
                    }


                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "DateTime";
                        worksheet.Cells[2, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Column(1).AutoFit();//自动列宽
                        for (int i = 0; i < trx[0][0].Items.Count; i++)
                        {

                            string FACDataName = trx[0][0][i].Name;//item名
                            string[] saveData = FACDataName.Split('/');//根据'/'分割数据名和数据倍率
                            worksheet.Cells[1, i + 2].Value = saveData[0].Trim();//写入数据名
                            int recieveData = int.Parse(trx[0][0][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[2, i + 2].Value = (recieveData * 1.0 / rate).ToString(ReserveDotCountSet(rate));//写入实际数据做记录，保留两位小数

                            worksheet.Column(i + 2).AutoFit();//自动列宽
                        }
                    }
                    else
                    {
                        worksheet.Cells[worksheet.Dimension.End.Row + 1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        for (int i = 0; i < trx[0][0].Items.Count; i++)
                        {

                            string FACDataName = trx[0][0][i].Name;
                            string[] saveData = FACDataName.Split('/');
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = saveData[0].Trim();
                            int recieveData = int.Parse(trx[0][0][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = (recieveData * 1.0 / rate).ToString(ReserveDotCountSet(rate));

                        }
                    }
                    package.Save();
                }
                DeleteFACData();
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public string FACLogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_FACSavePath))
                {
                    _FACSavePath = $"D:\\KZONELOG\\{eq.Data.LINEID}\\FACData\\";
                }

                return _FACSavePath;
            }
            set
            {
                _FACSavePath = value;
                if (_FACSavePath[_FACSavePath.Length - 1] != '\\')
                {
                    _FACSavePath = _FACSavePath + "\\";
                }
            }
        }
        public void DeleteFACData()
        {
            if (Directory.Exists(_FACSavePath) == false)
            {
                return;
            }
            else
            {
                string[] SVDataDicFiles = Directory.GetDirectories(_FACSavePath);
                foreach (string FilePath in SVDataDicFiles)
                {
                    DateTime checkTime = Directory.GetCreationTime(FilePath).AddDays(15);//设定保存时长，暂定100天
                    if (checkTime < DateTime.Now)//检查当前时间，若超过100天，则删除
                    {
                        Directory.Delete(FilePath, true);//删除该目录及目录下的子文件
                    }
                }
            }
        }
        #endregion

        #region Transfer Time Data Report
        private void TransferTimeDataReport(eBitResult eBitResult, string glassID, string cstNo, string slotNo)
        {
            try
            {
                Trx trx = GetTrxValues("L3_TransferTimeDataReport");
                string timerID = "L3_TransferTimeDataReportTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                             string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Transfer Time Data Report BIT=[{2}]",
                                 "L3", trx.TrackKey, "OFF"));
                    return;
                }

                var res = dicTransferTimes.Where(x => x.Value.GlassID == glassID).FirstOrDefault();
                string transactionID = res.Key;
                trx[0][0]["GlassID"].Value = glassID;
                trx[0][0]["CSTSequenceNumber"].Value = cstNo;
                trx[0][0]["SlotSequenceNumber"].Value = slotNo;
                trx[0][0]["TransferType"].Value = "1";
                string receiveAbleTime = dicTransferTimes[transactionID].ReceiveAbleTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].ReceiveAbleTime.ToString("yyyyMMddHHmmssfff");
                string receiveStartTime = dicTransferTimes[transactionID].ReceiveStartTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].ReceiveStartTime.ToString("yyyyMMddHHmmssfff");
                string receiveCompleteTime = dicTransferTimes[transactionID].ReceiveCompleteTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].ReceiveCompleteTime.ToString("yyyyMMddHHmmssfff");
                string receiveReadyTime = dicTransferTimes[transactionID].ReceiveReadyTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].ReceiveReadyTime.ToString("yyyyMMddHHmmssfff");
                string sendAbleTime = dicTransferTimes[transactionID].SendAbleTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].SendAbleTime.ToString("yyyyMMddHHmmssfff");
                string sendStartTime = dicTransferTimes[transactionID].SendStartTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].SendStartTime.ToString("yyyyMMddHHmmssfff");
                string sendCompleteTime = dicTransferTimes[transactionID].SendCompleteTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].SendCompleteTime.ToString("yyyyMMddHHmmssfff");
                string sendReadyTime = dicTransferTimes[transactionID].SendReadyTime == DateTime.MinValue ? "00000000000000000" : dicTransferTimes[transactionID].SendReadyTime.ToString("yyyyMMddHHmmssfff");

                trx[0][0]["ReceiveAbleTimeYear"].Value = receiveAbleTime.Substring(0, 4);
                trx[0][0]["ReceiveAbleTimeMonth"].Value = receiveAbleTime.Substring(4, 2);
                trx[0][0]["ReceiveAbleTimeDay"].Value = receiveAbleTime.Substring(6, 2);
                trx[0][0]["ReceiveAbleTimeHour"].Value = receiveAbleTime.Substring(8, 2);
                trx[0][0]["ReceiveAbleTimeMinute"].Value = receiveAbleTime.Substring(10, 2);
                trx[0][0]["ReceiveAbleTimeSecond"].Value = receiveAbleTime.Substring(12, 2);
                trx[0][0]["ReceiveAbleTimeMillisecond"].Value = "0";//receiveAbleTime.Substring(14, 3);

                trx[0][0]["ReceiveStartTimeYear"].Value = receiveStartTime.Substring(0, 4);
                trx[0][0]["ReceiveStartTimeMonth"].Value = receiveStartTime.Substring(4, 2);
                trx[0][0]["ReceiveStartTimeDay"].Value = receiveStartTime.Substring(6, 2);
                trx[0][0]["ReceiveStartTimeHour"].Value = receiveStartTime.Substring(8, 2);
                trx[0][0]["ReceiveStartTimeMinute"].Value = receiveStartTime.Substring(10, 2);
                trx[0][0]["ReceiveStartTimeSecond"].Value = receiveStartTime.Substring(12, 2);
                trx[0][0]["ReceiveStartTimeMillisecond"].Value = "0";//receiveStartTime.Substring(14, 3);

                trx[0][0]["ReceiveCompleteTimeYear"].Value = receiveCompleteTime.Substring(0, 4);
                trx[0][0]["ReceiveCompleteTimeMonth"].Value = receiveCompleteTime.Substring(4, 2);
                trx[0][0]["ReceiveCompleteTimeDay"].Value = receiveCompleteTime.Substring(6, 2);
                trx[0][0]["ReceiveCompleteTimeHour"].Value = receiveCompleteTime.Substring(8, 2);
                trx[0][0]["ReceiveCompleteTimeMinute"].Value = receiveCompleteTime.Substring(10, 2);
                trx[0][0]["ReceiveCompleteTimeSecond"].Value = receiveCompleteTime.Substring(12, 2);
                trx[0][0]["ReceiveCompleteTimeMillisecond"].Value = "0";//receiveCompleteTime.Substring(14, 3);

                trx[0][0]["ReceiveReadyTimeYear"].Value = receiveReadyTime.Substring(0, 4);
                trx[0][0]["ReceiveReadyTimeMonth"].Value = receiveReadyTime.Substring(4, 2);
                trx[0][0]["ReceiveReadyTimeDay"].Value = receiveReadyTime.Substring(6, 2);
                trx[0][0]["ReceiveReadyTimeHour"].Value = receiveReadyTime.Substring(8, 2);
                trx[0][0]["ReceiveReadyTimeMinute"].Value = receiveReadyTime.Substring(10, 2);
                trx[0][0]["ReceiveReadyTimeSecond"].Value = receiveReadyTime.Substring(12, 2);
                trx[0][0]["ReceiveReadyTimeMillisecond"].Value = "0";//receiveReadyTime.Substring(14, 3);

                trx[0][0]["SendAbleTimeYear"].Value = sendAbleTime.Substring(0, 4);
                trx[0][0]["SendAbleTimeMonth"].Value = sendAbleTime.Substring(4, 2);
                trx[0][0]["SendAbleTimeDay"].Value = sendAbleTime.Substring(6, 2);
                trx[0][0]["SendAbleTimeHour"].Value = sendAbleTime.Substring(8, 2);
                trx[0][0]["SendAbleTimeMinute"].Value = sendAbleTime.Substring(10, 2);
                trx[0][0]["SendAbleTimeSecond"].Value = sendAbleTime.Substring(12, 2);
                trx[0][0]["SendAbleTimeMillisecond"].Value = "0";//sendAbleTime.Substring(14, 3);

                trx[0][0]["SendStartTimeYear"].Value = sendStartTime.Substring(0, 4);
                trx[0][0]["SendStartTimeMonth"].Value = sendStartTime.Substring(4, 2);
                trx[0][0]["SendStartTimeDay"].Value = sendStartTime.Substring(6, 2);
                trx[0][0]["SendStartTimeHour"].Value = sendStartTime.Substring(8, 2);
                trx[0][0]["SendStartTimeMinute"].Value = sendStartTime.Substring(10, 2);
                trx[0][0]["SendStartTimeSecond"].Value = sendStartTime.Substring(12, 2);
                trx[0][0]["SendStartTimeMillisecond"].Value = "0";//sendStartTime.Substring(14, 3);

                trx[0][0]["SendCompleteTimeYear"].Value = sendCompleteTime.Substring(0, 4);
                trx[0][0]["SendCompleteTimeMonth"].Value = sendCompleteTime.Substring(4, 2);
                trx[0][0]["SendCompleteTimeDay"].Value = sendCompleteTime.Substring(6, 2);
                trx[0][0]["SendCompleteTimeHour"].Value = sendCompleteTime.Substring(8, 2);
                trx[0][0]["SendCompleteTimeMinute"].Value = sendCompleteTime.Substring(10, 2);
                trx[0][0]["SendCompleteTimeSecond"].Value = sendCompleteTime.Substring(12, 2);
                trx[0][0]["SendCompleteTimeMillisecond"].Value = "0";//sendCompleteTime.Substring(14, 3);

                trx[0][0]["SendReadyTimeYear"].Value = sendReadyTime.Substring(0, 4);
                trx[0][0]["SendReadyTimeMonth"].Value = sendReadyTime.Substring(4, 2);
                trx[0][0]["SendReadyTimeDay"].Value = sendReadyTime.Substring(6, 2);
                trx[0][0]["SendReadyTimeHour"].Value = sendReadyTime.Substring(8, 2);
                trx[0][0]["SendReadyTimeMinute"].Value = sendReadyTime.Substring(10, 2);
                trx[0][0]["SendReadyTimeSecond"].Value = sendReadyTime.Substring(12, 2);
                trx[0][0]["SendReadyTimeMillisecond"].Value = "0";//sendReadyTime.Substring(14, 3);


                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                trx[0][1][0].Value = "1";
                SendToPLC(trx);
                dicTransferTimes.Remove(transactionID);

                for (int i = 0; i < dicTransferTimes.Count; i++)
                {
                Skip:
                    string key = dicTransferTimes.ElementAt(i).Key;
                    DateTime dtKey = DateTime.ParseExact(key, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.CurrentCulture);
                    if (dtKey.AddDays(1) < DateTime.Now)
                    {
                        dicTransferTimes.Remove(key);
                        goto Skip;
                    }
                }


                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCTransferTimeDataReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                         string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Transfer Time Data Report BIT=[{2}] GlassID[{3}] CstNo[{4}] SlotNo[{5}]",
                             "L3", trx.TrackKey, "ON", glassID, cstNo, slotNo));

            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCTransferTimeDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                TransferTimeDataReport(eBitResult.OFF, "", "", "");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Transfer Time Data Report T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));
                Thread.Sleep(300);
                TransferTimeDataReportFlag = true;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void TransferTimeDataReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, "TransferTimeDataReportReplyTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Transfer Time Data Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                TransferTimeDataReport(eBitResult.OFF, "", "", "");
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(TransferTimeDataReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Transfer Time Data Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
                Thread.Sleep(300);
                TransferTimeDataReportFlag = true;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void TransferTimeDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Transfer Time Data Report Reply T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Lot End Glass Report
        public void LotEndGlassReportByUpstream(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}", eqpNo, "LotEndGlassReportByUpstream");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }
                    CPCLotEndGlassReportReplyToUpstream(eBitResult.OFF);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] BIT=[OFF] Lot End Glass Report By Upstream.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                string lotID = inputData[0][0][0].Value.Trim();
                string cstNo = inputData[0][0][1].Value.Trim();
                string slotNo = inputData[0][0][2].Value.Trim();
                string glassID = inputData[0][0][3].Value.Trim();
                if (!eqp.File.LotEndGlassInfos.ContainsKey(glassID))
                {

                    eqp.File.LotEndGlassInfos.TryAdd(glassID, new LotEndGlassInfos(lotID, cstNo, slotNo, glassID, DateTime.Now));
                }
                else
                {
                    eqp.File.LotEndGlassInfos[glassID].LotID = lotID;
                    eqp.File.LotEndGlassInfos[glassID].Cassette_Sequence_No = cstNo;
                    eqp.File.LotEndGlassInfos[glassID].Slot_Sequence_No = slotNo;
                    eqp.File.LotEndGlassInfos[glassID].GlassID = glassID;
                    eqp.File.LotEndGlassInfos[glassID].AddTime = DateTime.Now;
                }
                //定期删除残帐
                var res = eqp.File.LotEndGlassInfos.Where(x => x.Value.AddTime.AddDays(1) < DateTime.Now).ToList();
                if (res != null && res.Count > 0)
                {
                    LotEndGlassInfos lotEndGlassInfos = new LotEndGlassInfos();
                    foreach (var item in res)
                    {
                        eqp.File.LotEndGlassInfos.TryRemove(item.Key, out lotEndGlassInfos);
                    }
                }
                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCLotEndGlassReportReplyToUpstream(eBitResult.ON);
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(LotEndGlassReportByUpstreamTimeoutAction), inputData.TrackKey);
                }
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LotEndGlassReportByUpstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Lot End Glass Report By Upstream T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void CPCLotEndGlassReportReplyToUpstream(eBitResult eBitResult)
        {
            try
            {
                Trx trx = GetTrxValues("L3_LotEndGlassReportReplyToUpstream");
                trx[0][0][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                string timerID = "L3_CPCLotEndGlassReportReplyToUpstreamTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCLotEndGlassReportReplyToUpstreamTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] BIT=[{2}] Lot End Glass Report Reply To Upstream.",
                        "L3", trx.TrackKey, eBitResult));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCLotEndGlassReportReplyToUpstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCLotEndGlassReportReplyToUpstream(eBitResult.OFF);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Lot End Glass Report Reply To Upstream T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void LotEndGlassReportReplyByDownstream(Trx inputData)
        {
            try
            {
                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
                string timerID = "L3_LotEndGlassReportReplyByDownstreamTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] BIT=[OFF] Lot End Glass Report Reply By Downstream.",
                        "L3", inputData.TrackKey));
                    return;
                }
                LotEndGlassReportToDownstream(eBitResult.OFF, "", "", "", "");
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(LotEndGlassReportReplyByDownstreamTimeoutAction), UtilityMethod.GetAgentTrackKey());
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] BIT=[ON] Lot End Glass Report Reply By Downstream.",
                        "L3", inputData.TrackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LotEndGlassReportToDownstream(eBitResult eBitResult, string lotID, string cstNo, string slotNo, string glassID)
        {
            try
            {
                Trx trx = GetTrxValues("L3_LotEndGlassReport");
                string timerID = "L3_LotEndGlassReportTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    trx[0][1][0].Value = "0";
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] BIT=[OFF] Lot End Glass Report To Downstream.",
                        "L3", trx.TrackKey));
                    return;
                }

                trx[0][0][0].Value = lotID;
                trx[0][0][1].Value = cstNo;
                trx[0][0][2].Value = slotNo;
                trx[0][0][3].Value = glassID;
                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] BIT=[ON] Lot End Glass Report To Downstream LotID=[{2}] CstNo=[{3}] SlotNo=[{4}] GlassID=[{5}].",
                        "L3", trx.TrackKey, lotID, cstNo, slotNo, glassID));
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(LotEndGlassReportToDownstreamTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void LotEndGlassReportToDownstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                LotEndGlassReportToDownstream(eBitResult.OFF, "", "", "", "");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Lot End Glass Report To Downstream T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void LotEndGlassReportReplyByDownstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Lot End Glass Report Reply By Downstream T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region PPID PreDownload Flag
        public void PPIDPreDownloadFlagByUpstream(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (line.Data.FABTYPE == "ODF")
                {
                    return;
                }
                //if (inputData.IsInitTrigger)
                //{
                //    //初始化时获取数据
                //    if (eqp.File.PPIDPreDownloadInfos != null && eqp.File.PPIDPreDownloadInfos.Count > 0)
                //    {
                //        PPIDPreDownloadInfos = eqp.File.PPIDPreDownloadInfos;
                //    }
                //    return;
                //}
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string t2TimerID = string.Format("{0}_{1}", eqpNo, "PPIDPreDownloadFlagByUpstreamTimeout");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t2TimerID))
                    {
                        Timermanager.TerminateTimer(t2TimerID);
                    }
                    CPCPPIDPreDownloadFlagReplyToUpstream(eBitResult.OFF);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] BIT=[OFF] PPID PreDownload Flag By Upstream.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                string PPID = inputData[0][0][0].Value.Trim();
                string BatchID = inputData[0][0][1].Value.Trim();
                if (!eqp.File.PPIDPreDownloadInfos.ContainsKey(PPID))
                {
                    eqp.File.PPIDPreDownloadInfos.TryAdd(PPID, new PPIDPreDownloadInfos(PPID, BatchID, DateTime.Now));
                }
                else
                {
                    eqp.File.PPIDPreDownloadInfos[PPID].PPID = PPID;
                    eqp.File.PPIDPreDownloadInfos[PPID].BatchID = BatchID;
                    eqp.File.PPIDPreDownloadInfos[PPID].AddTime = DateTime.Now;
                }
                //定期删除残帐
                var res = eqp.File.LotEndGlassInfos.Where(x => x.Value.AddTime.AddDays(1) > DateTime.Now).ToList();
                if (res != null && res.Count > 0)
                {
                    PPIDPreDownloadInfos PPIDPreDownloadInfos = new PPIDPreDownloadInfos();
                    foreach (var item in res)
                    {
                        eqp.File.PPIDPreDownloadInfos.TryRemove(item.Key, out PPIDPreDownloadInfos);
                    }
                }
                ////更新数据
                //PPIDPreDownloadInfos = eqp.File.PPIDPreDownloadInfos;
                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCPPIDPreDownloadFlagReplyToUpstream(eBitResult.ON);
                    Timermanager.CreateTimer(t2TimerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(PPIDPreDownloadFlagByUpstreamTimeoutAction), inputData.TrackKey);
                }
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void PPIDPreDownloadFlagByUpstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] PPID PreDownload Flag By Upstream T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void CPCPPIDPreDownloadFlagReplyToUpstream(eBitResult eBitResult)
        {
            try
            {
                Trx trx = GetTrxValues("L3_PPIDPreDownloadFlagReplyToUpstream");
                trx[0][0][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                string timerID = "L3_CPCPPIDPreDownloadFlagReplyToUpstreamTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCPPIDPreDownloadFlagReplyToUpstreamTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] BIT=[{2}] PPID PreDownload Flag Reply To Upstream.",
                        "L3", trx.TrackKey, eBitResult));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCPPIDPreDownloadFlagReplyToUpstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCPPIDPreDownloadFlagReplyToUpstream(eBitResult.OFF);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] PPID PreDownload Flag Reply To Upstream T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void PPIDPreDownloadFlagReplyByDownstream(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                if (line.Data.FABTYPE == "ODF")
                {
                    return;
                }
                string timerID = "L3_PPIDPreDownloadFlagReplyFromDownstreamTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
                if (eBitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] PPID PreDownload Flag Reply By Downstream Bit OFF.", eqpNo, inputData.TrackKey));
                    return;
                }
                string timerID2 = "L3_PPIDPreDownLoadFlagToDownstreamTimeout";
                if (Timermanager.IsAliveTimer(timerID2))
                {
                    Timermanager.TerminateTimer(timerID2);
                }
                Trx trx = GetTrxValues("L3_PPIDPreDownLoadFlag");
                trx[0][1][0].Value = "0";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCPPIDPreDownloadFlagReplyByDownstreamTimeoutAction), UtilityMethod.GetAgentTrackKey());

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] PPID PreDownload Flag Reply By Downstream Bit ON.", eqpNo, inputData.TrackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCPPIDPreDownloadFlagReplyByDownstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] PPID PreDownload Flag Reply From Downstream T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCPPIDPreDownloadFlagToDownstreamTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx trx = GetTrxValues(string.Format("L3_PPIDPreDownLoadFlag"));
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] PPID PreDownload Flag To Downstream T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));
                Thread.Sleep(300);
                PPIDPreDownloadFlagReport = true;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Removed Job Signal(未做)


        #endregion

        #region EC Change Command
        public void ECChangeCommand(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[2].Items[0].Value);
                string t1TimerID = string.Format("{0}_{1}", eqpNo, "ECChangeCommandTimeout");


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(t1TimerID))
                    {
                        Timermanager.TerminateTimer(t1TimerID);
                    }
                    CPCECChangeCommandReply(eBitResult.OFF, "0");
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] EC Change Command.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                string localRecipeID = inputData[0][0][0].Value.Trim();
                string msg = string.Empty;
                #region 建立timer

                if (Timermanager.IsAliveTimer(t1TimerID))
                {
                    Timermanager.TerminateTimer(t1TimerID);
                }
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipeByNo();
                }
                else
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipe();
                }

                if (recipeDic[eq.Data.LINEID].ContainsKey(localRecipeID))
                {

                    //校验下发的参数是否合法
                    if (ECDataCheck(inputData))
                    {
                        //写入Recipe Parameter
                        RecipeEntityData recipe = recipeDic[eq.Data.LINEID][localRecipeID];
                        IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipe.FILENAME);
                        if (paramter.Count > 0)
                        {
                            CPCRecipeParameterDownloadCommandByEC(eq, recipe, eBitResult.ON, paramter, inputData);
                            CPCECChangeCommandReply(eBitResult.ON, "1");
                            msg = "Recipe Parameter Updated!";
                        }
                        else
                        {
                            CPCECChangeCommandReply(eBitResult.ON, "2");
                            msg = "CIM PC Hasn't Recipe Parameter .txt File!";
                        }

                    }
                    else
                    {
                        CPCECChangeCommandReply(eBitResult.ON, "2");
                        msg = "EC Download Data Invalid!";
                    }

                    //RecipeEntityData recipeEntityData = recipeDic[eq.Data.LINEID][localRecipeID];
                    //Trx trx = GetTrxValues(string.Format("{0}_LocalRecipeModifiedReport", eq.Data.NODENO));
                    //trx[0][0][0].Value = recipeEntityData.RECIPEID;
                    //trx[0][0][1].Value = localRecipeID;
                    //trx[0][0][2].Value = string.Empty;
                    //trx[0][0][3].Value = "0";
                    //trx[0][0][4].Value = "2";
                    //trx[0][0][5].Value = "1";
                    //trx[0][1][0].Value = "1";
                    //trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    //SendToPLC(trx);

                    //RecipeParameterReport(eq, null, recipeEntityData, "2");
                    //CPCUnitRecipeListReport("2");
                }
                else
                {
                    CPCECChangeCommandReply(eBitResult.ON, "2");
                    msg = "Recipe ID isn't exist!";
                }
                Timermanager.CreateTimer(t1TimerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ECChangeCommandTimeoutAction), inputData.TrackKey);
                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] EC Change Command BIT [{2}] Recipe ID=[{3}] Msg=[{4}].",
                            eqpNo, inputData.TrackKey, bitResult, localRecipeID, msg));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void ECChangeCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] EC Change Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCECChangeCommandReply(eBitResult eBitResult, string returnCode)
        {
            try
            {
                Trx trx = GetTrxValues("L3_ECChangeCommandReply");
                trx[0][0][0].Value = returnCode;
                trx[0][1][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                string timerID = "L3_ECChangeCommandReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] EC Change Command Reply Bit[{2}] ReturnCode=[{3}][{4}].", "L3", trx.TrackKey, eBitResult, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : ""));
                if (eBitResult == eBitResult.OFF)
                {
                    return;
                }

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCECChangeCommandReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCECChangeCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] EC Change Command Reply T2 TIMEOUT.", sArray[0], trackKey));
                CPCECChangeCommandReply(eBitResult.OFF, "0");

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private bool ECDataCheck(Trx inputData)
        {
            //tips:parameterName格式  名字(实际合法值下限-实际合法值上限)/倍率

            Trx trx = GetTrxValues("L3_RecipeParameterReport");
            for (int i = 0; i < inputData[0][1].Items.Count; i++)
            {
                string parameterName = inputData[0][1][i].Name.Trim();
                if (parameterName.Contains("/"))
                {
                    double rate = double.Parse(parameterName.Split('/')[1]);
                    double min = 0;
                    double max = 0;
                    if (parameterName.Contains("(") && parameterName.Contains(")") && parameterName.Contains("-"))
                    {
                        int[] indexes = new int[2];
                        for (int j = 0; j < parameterName.Length; j++)
                        {
                            if (parameterName[j] == '(')
                            {
                                indexes[0] = j;
                            }
                            if (parameterName[j] == ')')
                            {
                                indexes[1] = j;
                            }
                        }

                        string strRound = string.Empty;
                        if (indexes[1] - indexes[0] > 3)
                        {
                            strRound = parameterName.Substring(indexes[0] + 1, indexes[1] - indexes[0] - 1);
                        }
                        if (!string.IsNullOrEmpty(strRound))
                        {
                            if (strRound.Split('-').Length == 2)
                            {
                                min = double.Parse(strRound.Split('-')[0]);
                                max = double.Parse(strRound.Split('-')[1]);
                            }
                            else if (strRound.Split('-').Length == 3)
                            {
                                min = double.Parse("-" + strRound.Split('-')[1]);
                                max = double.Parse(strRound.Split('-')[2]);
                            }
                            else if (strRound.Split('-').Length == 4)
                            {
                                min = double.Parse("-" + strRound.Split('-')[1]);
                                max = double.Parse("-" + strRound.Split('-')[3]);
                            }
                            else
                            {

                            }

                        }
                        double temp = double.Parse(inputData[0][1][i].Value) / rate;
                        if (temp < min || temp > max)
                        {
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        private void CPCRecipeParameterDownloadCommandByEC(Equipment eq, RecipeEntityData recipeEntity, eBitResult bit, IList<string> parameter, Trx inputData)
        {
            Trx sendTrx = GetTrxValues("L3_EQDECRecipeModifyCommandBlock");

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");

            string year = datetime.Substring(0, 4);
            string month = datetime.Substring(4, 2);
            string day = datetime.Substring(6, 2);
            string hour = datetime.Substring(8, 2);
            string minute = datetime.Substring(10, 2);
            string second = datetime.Substring(12, 2);

            sendTrx[0][0]["RecipeName"].Value = recipeEntity.RECIPENAME.Trim();
            sendTrx[0][0]["RecipeID"].Value = recipeEntity.RECIPEID.Trim();
            sendTrx[0][0]["ModifyFlag"].Value = "2";
            sendTrx[0][0]["RecipeNO"].Value = recipeEntity.RECIPENO.Trim();

            sendTrx[0][0]["Year"].Value = year;
            sendTrx[0][0]["Month"].Value = month;
            sendTrx[0][0]["Day"].Value = day;
            sendTrx[0][0]["Hour"].Value = hour;
            sendTrx[0][0]["Minute"].Value = minute;
            sendTrx[0][0]["Second"].Value = second;

            for (int i = 0; i < parameter.Count; i++)
            {
                if (parameter[i].Contains("="))
                {
                    string name = parameter[i].Split('=')[0];
                    string value = parameter[i].Split('=')[1];
                    if (name.Trim() == "Recipe_NO" || name.Trim() == "Recipe_ID" || name.Trim() == "Recipe_Version" || name.Trim() == "Recipe_Name")
                    {
                        continue;
                    }
                    if (name.Trim() == "Recipe_State")
                    {
                        if (value == "Enable")
                        {
                            sendTrx[0][0]["RecipeStatus"].Value = "1";
                        }
                        else
                        {
                            sendTrx[0][0]["RecipeStatus"].Value = "0";
                        }
                        continue;
                    }
                    sendTrx[0][1][name].Value = value;
                }
            }
            for (int i = 0; i < inputData[0][1].Items.Count; i++)
            {
                string name = inputData[0][1][i].Name;
                if (sendTrx[0][1].Items.AllKeys.Contains(name))
                {
                    sendTrx[0][1][name].Value = inputData[0][1][i].Value;
                }

            }
            sendTrx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(sendTrx);

            Trx trxRecipeDownloadCommand = GetTrxValues("L3_EQDRecipeIDModifyCommand");
            trxRecipeDownloadCommand.TrackKey = UtilityMethod.GetAgentTrackKey();
            trxRecipeDownloadCommand[0][0][0].Value = "1";

            SendToPLC(trxRecipeDownloadCommand);

            string timerID = "L3_EQDRecipeIDModifyCommandTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            Timermanager.CreateTimer(timerID, false, 2000, new System.Timers.ElapsedEventHandler(EQDRecipeIDModifyCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  Recipe Parameter Download Command By EC Data BIT=[ON].", eq.Data.NODENO, UtilityMethod.GetAgentTrackKey()));
        }
        #endregion

        #region TR01/TR02 Transfer Monitor
        public void EQTR01TransferMonitor(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE != "CELL")//eq.Data.LINEID != "KWF23633L"
                {
                    return;
                }
                eq.File.TR01TransferEnable = (eEnableDisable)int.Parse(inputData[0][0][0].Value);
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] TR01 Transfer [{2}].",
                            eqpNo, inputData.TrackKey, eq.File.TR01TransferEnable));
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void TR01TransferRequestReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE != "CELL")//eq.Data.LINEID != "KWF23633L"
                {
                    return;
                }
                if (eq.File.TR01TransferEnable == eEnableDisable.Disable)
                {
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
                string timerID = $"{inputData.Name}" + "Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [L4 -> L3][{1}] TR01 Transfer Request Reply [{2}].",
                            eqpNo, inputData.TrackKey, bitResult));
                    return;
                }
                string timerID2 = "L3_TR01TransferFirstToTR01_Timeout";
                if (Timermanager.IsAliveTimer(timerID2))
                {
                    Timermanager.TerminateTimer(timerID2);
                }
                Trx trx = GetTrxValues("L3_TR01TransferFirstToTR01");
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                trx[0][0][0].Value = "0";
                SendToPLC(trx);
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(TR01TransferRequestReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [L4 -> L3][{1}] TR01 Transfer Request Reply [{2}].",
                        eqpNo, inputData.TrackKey, bitResult));

                CurrentGlassSendToTR = eTransfer.TR01;
                TR01_TR02_TransferRequestReplyWaitFlag = false;
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void TR01TransferRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [L4 -> L3][{1}] TR01 Transfer Request Reply T2 TIMEOUT.", sArray[0], trackKey));

            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQTR02TransferMonitor(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE != "CELL")//eq.Data.LINEID != "KWF23633L"
                {
                    return;
                }
                eq.File.TR02TransferEnable = (eEnableDisable)int.Parse(inputData[0][0][0].Value);
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] TR02 Transfer [{2}].",
                            eqpNo, inputData.TrackKey, eq.File.TR02TransferEnable));
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void TR02TransferRequestReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE != "CELL")//eq.Data.LINEID != "KWF23633L"
                {
                    return;
                }
                if (eq.File.TR02TransferEnable == eEnableDisable.Disable)
                {
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
                string timerID = $"{inputData.Name}" + "Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [L6 -> L3][{1}] TR02 Transfer Request Reply [{2}].",
                            eqpNo, inputData.TrackKey, bitResult));
                    return;
                }
                string timerID2 = "L3_TR02TransferFirstToTR02_Timeout";
                if (Timermanager.IsAliveTimer(timerID2))
                {
                    Timermanager.TerminateTimer(timerID2);
                }
                Trx trx = GetTrxValues("L3_TR02TransferFirstToTR02");
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                trx[0][0][0].Value = "0";
                SendToPLC(trx);
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(TR02TransferRequestReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [L6 -> L3][{1}] TR02 Transfer Request Reply [{2}].",
                        eqpNo, inputData.TrackKey, bitResult));
                CurrentGlassSendToTR = eTransfer.TR02;
                TR01_TR02_TransferRequestReplyWaitFlag = false;
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void TR02TransferRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [L6 -> L3][{1}] TR02 Transfer Request Reply T2 TIMEOUT.", sArray[0], trackKey));

            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCTR01_02_TransferRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                if (Timermanager.IsAliveTimer(tmp))
                {
                    Timermanager.TerminateTimer(tmp);
                }

                Trx trx = GetTrxValues($"{sArray[0]}_{sArray[1]}");
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                trx[0][0][0].Value = "0";
                SendToPLC(trx);

                //请求超时，TR置None
                CurrentGlassSendToTR = eTransfer.NONE;
                string downStreamName = sArray[1].Substring(0, 4);
                string downStreamLocalNo = downStreamName == "TR01" ? "L4" : downStreamName == "TR02" ? "L6" : "";
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [L3 -> {1}][{2}] TR01_02_TransferRequest TIMEOUT.", sArray[0], downStreamLocalNo, trackKey));
                Thread.Sleep(ParameterManager[eParameterName.TR01_02_TransferRequestInterval].GetInteger());

                TR01_TR02_TransferRequestFlag = true;
                //TR01_TR02_TransferRequestReplyWaitFlag = false;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Material Status Change Report
        public void EQMaterialStatusChangeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_EQMaterialStatusChangeReportTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    EQMaterialStatusChangeReportReply(eBitResult.OFF, "0");
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Material Status Change Report Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }

                string materialID = inputData[0][0][0].Value.Trim();
                string materialStatus = inputData[0][0][1].Value;
                string materialType = inputData[0][0][2].Value;
                string PRID = inputData[0][0][3].Value.Trim();
                string PRWeight = inputData[0][0][4].Value;

                #region 不管BC回復，上報並更新狀態 (不使用)
                //MaterialEntity material = ObjectManager.MaterialManager.GetMaterialByID(eqpNo, materialID);
                //if (material == null)//新建并上報
                //{
                //    material = new MaterialEntity(materialID);
                //    material.MaterialStatus = (eMaterialStatus)int.Parse(materialStatus);
                //    material.MaterialType = (eMaterialType)int.Parse(materialType);
                //    material.PRID = PRID;
                //    material.Weight = int.Parse(PRWeight);
                //    material.NodeNo = eqpNo;
                //    material.UnitNo = 3;
                //    if (material.MaterialStatus == eMaterialStatus.MOUNT)
                //    {
                //        string datetime = inputData.TrackKey.Substring(0, 14);
                //        material.MountTime.Year = int.Parse(datetime.Substring(0, 4));
                //        material.MountTime.Month = int.Parse(datetime.Substring(4, 2));
                //        material.MountTime.Day = int.Parse(datetime.Substring(6, 2));
                //        material.MountTime.Hour = int.Parse(datetime.Substring(8, 2));
                //        material.MountTime.Minute = int.Parse(datetime.Substring(10, 2));
                //        material.MountTime.Second = int.Parse(datetime.Substring(12, 2));
                //    }
                //    else if (material.MaterialStatus == eMaterialStatus.UNMOUNT)
                //    {
                //        string datetime = inputData.TrackKey.Substring(0, 14);
                //        material.UnmountTime.Year = int.Parse(datetime.Substring(0, 4));
                //        material.UnmountTime.Month = int.Parse(datetime.Substring(4, 2));
                //        material.UnmountTime.Day = int.Parse(datetime.Substring(6, 2));
                //        material.UnmountTime.Hour = int.Parse(datetime.Substring(8, 2));
                //        material.UnmountTime.Minute = int.Parse(datetime.Substring(10, 2));
                //        material.UnmountTime.Second = int.Parse(datetime.Substring(12, 2));
                //    }

                //    if (eq.File.CIMMode == eBitResult.ON)
                //    {
                //        material.LastMaterialStatus = material.MaterialStatus;
                //        CPCMaterialStatusChangeReport(eBitResult.ON, material);
                //        Timermanager.CreateTimer(timerID, false, 10000,
                //            new System.Timers.ElapsedEventHandler(EQMaterialDataChangeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());
                //    }

                //    ObjectManager.MaterialManager.SaveMaterialHistory(eq.Data.LINEID, "3", material, eq.File.CurrentMaterialID, $"{material.NodeNo}_{material.UnitNo}_{material.MaterialId}", material.UsedCount.ToString());
                //    eq.File.CurrentMaterialID = material.MaterialId.Trim();
                //    ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                //}
                //else
                //{



                //    //判斷ID,Status是否一致，不一致上報
                //    if (material.MaterialId.Trim() != eq.File.CurrentMaterialID.Trim() || (eMaterialStatus)int.Parse(materialStatus) != material.MaterialStatus)
                //    {
                //        if ((eMaterialStatus)int.Parse(materialStatus) != material.MaterialStatus)
                //        {
                //            if ((eMaterialStatus)int.Parse(materialStatus) == eMaterialStatus.MOUNT)
                //            {
                //                string datetime = inputData.TrackKey.Substring(0, 14);
                //                material.MountTime.Year = int.Parse(datetime.Substring(0, 4));
                //                material.MountTime.Month = int.Parse(datetime.Substring(4, 2));
                //                material.MountTime.Day = int.Parse(datetime.Substring(6, 2));
                //                material.MountTime.Hour = int.Parse(datetime.Substring(8, 2));
                //                material.MountTime.Minute = int.Parse(datetime.Substring(10, 2));
                //                material.MountTime.Second = int.Parse(datetime.Substring(12, 2));
                //            }
                //            else if ((eMaterialStatus)int.Parse(materialStatus) == eMaterialStatus.UNMOUNT)
                //            {
                //                string datetime = inputData.TrackKey.Substring(0, 14);
                //                material.UnmountTime.Year = int.Parse(datetime.Substring(0, 4));
                //                material.UnmountTime.Month = int.Parse(datetime.Substring(4, 2));
                //                material.UnmountTime.Day = int.Parse(datetime.Substring(6, 2));
                //                material.UnmountTime.Hour = int.Parse(datetime.Substring(8, 2));
                //                material.UnmountTime.Minute = int.Parse(datetime.Substring(10, 2));
                //                material.UnmountTime.Second = int.Parse(datetime.Substring(12, 2));
                //            }
                //            material.LastMaterialStatus = material.MaterialStatus;
                //            material.MaterialStatus = (eMaterialStatus)int.Parse(materialStatus);
                //            material.MaterialType = (eMaterialType)int.Parse(materialType);
                //            material.PRID = PRID;
                //            material.Weight = int.Parse(PRWeight);
                //            material.NodeNo = eqpNo;
                //            material.UnitNo = 3;

                //            if (eq.File.CIMMode == eBitResult.ON)
                //            {

                //                CPCMaterialStatusChangeReport(eBitResult.ON, material);
                //                Timermanager.CreateTimer(timerID, false, 10000,
                //                    new System.Timers.ElapsedEventHandler(EQMaterialDataChangeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());
                //            }

                //            ObjectManager.MaterialManager.SaveMaterialHistory(eq.Data.LINEID, "3", material, eq.File.CurrentMaterialID, $"{material.NodeNo}_{material.UnitNo}_{material.MaterialId}", material.UsedCount.ToString());
                //            eq.File.CurrentMaterialID = material.MaterialId.Trim();
                //            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                //        }
                //    }

                //}

                #endregion

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    MaterialEntity material = ObjectManager.MaterialManager.GetMaterialByID(eqpNo, materialID);
                    if (material == null)
                    {
                        CPCMaterialStatusChangeReport(eBitResult.ON, inputData);
                        eq.File.ReportMaterial.MaterialId = materialID;
                        eq.File.ReportMaterial.MaterialStatus = (eMaterialStatus)int.Parse(materialStatus);
                        eq.File.ReportMaterial.MaterialType = (eMaterialType)int.Parse(materialType);
                        eq.File.ReportMaterial.PRID = PRID;
                        eq.File.ReportMaterial.Weight = int.Parse(PRWeight);
                        ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                    }
                    else
                    {
                        if (material.MaterialId.Trim() != materialID || material.MaterialStatus != (eMaterialStatus)int.Parse(materialStatus)
                            || material.MaterialType != (eMaterialType)int.Parse(materialType) || material.PRID != PRID || material.Weight != int.Parse(PRWeight))
                        {
                            CPCMaterialStatusChangeReport(eBitResult.ON, inputData);
                            eq.File.ReportMaterial.MaterialId = materialID;
                            eq.File.ReportMaterial.MaterialStatus = (eMaterialStatus)int.Parse(materialStatus);
                            eq.File.ReportMaterial.MaterialType = (eMaterialType)int.Parse(materialType);
                            eq.File.ReportMaterial.PRID = PRID;
                            eq.File.ReportMaterial.Weight = int.Parse(PRWeight);
                            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                        }
                    }
                    Timermanager.CreateTimer(timerID, false, 10000,
                        new System.Timers.ElapsedEventHandler(EQMaterialDataChangeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Material Status Change Report Bit=[{2}] MaterialID=[{3}] MaterialStatus=[{4}][{5}] MaterialType=[{6}][{7}] PRID=[{8}] PRWeight=[{9}].",
                                    eqpNo, inputData.TrackKey, eBitResult, materialID, materialStatus, (eMaterialStatus)int.Parse(materialStatus), materialType, (eMaterialType)int.Parse(materialType), PRID, PRWeight));



            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void MaterialDataUpdate(string eqpNo, string materialID, string materialStatus, string materialType, string PRID, string PRWeight)
        {
            MaterialEntity material = ObjectManager.MaterialManager.GetMaterialByID(eqpNo, materialID);
            if (material == null)//新建并上報
            {
                material = new MaterialEntity(materialID);
                material.MaterialStatus = (eMaterialStatus)int.Parse(materialStatus);
                material.MaterialType = (eMaterialType)int.Parse(materialType);
                material.PRID = PRID;
                material.Weight = int.Parse(PRWeight);
                material.NodeNo = eqpNo;
                material.UnitNo = 3;
                if (material.MaterialStatus == eMaterialStatus.MOUNT)
                {
                    string datetime = UtilityMethod.GetAgentTrackKey().Substring(0, 14);
                    material.MountTime.Year = int.Parse(datetime.Substring(0, 4));
                    material.MountTime.Month = int.Parse(datetime.Substring(4, 2));
                    material.MountTime.Day = int.Parse(datetime.Substring(6, 2));
                    material.MountTime.Hour = int.Parse(datetime.Substring(8, 2));
                    material.MountTime.Minute = int.Parse(datetime.Substring(10, 2));
                    material.MountTime.Second = int.Parse(datetime.Substring(12, 2));
                }
                else if (material.MaterialStatus == eMaterialStatus.UNMOUNT)
                {
                    string datetime = UtilityMethod.GetAgentTrackKey().Substring(0, 14);
                    material.UnmountTime.Year = int.Parse(datetime.Substring(0, 4));
                    material.UnmountTime.Month = int.Parse(datetime.Substring(4, 2));
                    material.UnmountTime.Day = int.Parse(datetime.Substring(6, 2));
                    material.UnmountTime.Hour = int.Parse(datetime.Substring(8, 2));
                    material.UnmountTime.Minute = int.Parse(datetime.Substring(10, 2));
                    material.UnmountTime.Second = int.Parse(datetime.Substring(12, 2));
                }

                eq.File.CurrentMaterialID = materialID;
                eq.File.CurrentMaterialStatus = material.MaterialStatus;
                eq.File.CurrentMaterial = material;
                material.UpdateTime = DateTime.Now;
                material.Event = "EQ_Create_Data";
                ObjectManager.MaterialManager.SaveMaterialHistory(eq.Data.LINEID, "3", material, eq.File.CurrentMaterialID, $"{material.NodeNo}_{material.UnitNo}_{material.MaterialId}_{material.UpdateTime.ToString("yyyyMMddHHmmss")}", material.UsedCount.ToString());
                eq.File.CurrentMaterialID = material.MaterialId.Trim();
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
            }
            else
            {
                if ((eMaterialStatus)int.Parse(materialStatus) != material.MaterialStatus || material.MaterialId.Trim() != materialID ||
                    material.MaterialType != (eMaterialType)int.Parse(materialType) || material.PRID != PRID || material.Weight != int.Parse(PRWeight))
                {
                    if ((eMaterialStatus)int.Parse(materialStatus) != material.MaterialStatus)
                    {
                        if ((eMaterialStatus)int.Parse(materialStatus) == eMaterialStatus.MOUNT)
                        {
                            string datetime = UtilityMethod.GetAgentTrackKey().Substring(0, 14);
                            material.MountTime.Year = int.Parse(datetime.Substring(0, 4));
                            material.MountTime.Month = int.Parse(datetime.Substring(4, 2));
                            material.MountTime.Day = int.Parse(datetime.Substring(6, 2));
                            material.MountTime.Hour = int.Parse(datetime.Substring(8, 2));
                            material.MountTime.Minute = int.Parse(datetime.Substring(10, 2));
                            material.MountTime.Second = int.Parse(datetime.Substring(12, 2));
                        }
                        else if ((eMaterialStatus)int.Parse(materialStatus) == eMaterialStatus.UNMOUNT)
                        {
                            string datetime = UtilityMethod.GetAgentTrackKey().Substring(0, 14);
                            material.UnmountTime.Year = int.Parse(datetime.Substring(0, 4));
                            material.UnmountTime.Month = int.Parse(datetime.Substring(4, 2));
                            material.UnmountTime.Day = int.Parse(datetime.Substring(6, 2));
                            material.UnmountTime.Hour = int.Parse(datetime.Substring(8, 2));
                            material.UnmountTime.Minute = int.Parse(datetime.Substring(10, 2));
                            material.UnmountTime.Second = int.Parse(datetime.Substring(12, 2));
                        }
                        material.LastMaterialStatus = material.MaterialStatus;
                        material.MaterialStatus = (eMaterialStatus)int.Parse(materialStatus);
                    }

                    material.MaterialType = (eMaterialType)int.Parse(materialType);
                    material.PRID = PRID;
                    material.Weight = int.Parse(PRWeight);
                    material.NodeNo = eqpNo;
                    material.UnitNo = 3;

                    eq.File.CurrentMaterialID = materialID;
                    eq.File.CurrentMaterialStatus = material.MaterialStatus;
                    eq.File.CurrentMaterial = material;
                    material.UpdateTime = DateTime.Now;
                    material.Event = "Changed_For_EQ_Modify_Data";
                    ObjectManager.MaterialManager.SaveMaterialHistory(eq.Data.LINEID, "3", material, eq.File.CurrentMaterialID, $"{material.NodeNo}_{material.UnitNo}_{material.MaterialId}_{material.UpdateTime.ToString("yyyyMMddHHmmss")}", material.UsedCount.ToString());
                    eq.File.CurrentMaterialID = material.MaterialId.Trim();
                    ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                }


            }
            ObjectManager.MaterialManager.AddMaterial(material);
        }

        private void EQMaterialDataChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Material Data Change Report TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCMaterialStatusChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                CPCMaterialStatusChangeReport(eBitResult.OFF, null);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Material Status Change Report T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));
                EQMaterialStatusChangeReportReply(eBitResult.ON, "2");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQMaterialStatusChangeReportReply(eBitResult bitResult, string returnCode)
        {
            try
            {
                Trx trx = GetTrxValues("L3_EQDMaterialStatusChangeReportReply");
                string timerID = "L3_EQDMaterialStatusChangeReportReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.OFF)
                {
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQ Material Data Change Report Reply Bit OFF.", trx.Metadata.NodeNo, trx.TrackKey));
                    return;
                }
                else
                {
                    trx[0][0][0].Value = returnCode;
                    trx[0][1][0].Value = "1";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQ Material Data Change Report Reply Bit ON ReturnCode=[{2}][{3}].", trx.Metadata.NodeNo, trx.TrackKey, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : "Other"));
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQMaterialStatusChangeReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void EQMaterialStatusChangeReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                EQMaterialStatusChangeReportReply(eBitResult.OFF, "0");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQ Material Data Change Report Reply T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCMaterialStatusChangeReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);
                string t1TimerID = string.Format("{0}_{1}", eqpNo, "BCMaterialStatusChangeReportReplyTimeout");


                if (Timermanager.IsAliveTimer(t1TimerID))
                {
                    Timermanager.TerminateTimer(t1TimerID);
                }
                if (bitResult == eBitResult.OFF)
                {

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Material Status Change Report Bit OFF.",
                        eqpNo, inputData.TrackKey));
                    return;
                }
                CPCMaterialStatusChangeReport(eBitResult.OFF, null);
                string returnCode = inputData[0][0][1].Value.Trim();
                string unitNo = inputData[0][0][0].Value.Trim();
                string msg = string.Empty;

                if (returnCode == "1")
                {
                    EQMaterialStatusChangeReportReply(eBitResult.ON, "1");
                    //更新material
                    string materialID = eq.File.ReportMaterial.MaterialId;
                    string materialStatus = ((int)eq.File.ReportMaterial.MaterialStatus).ToString();
                    string materialType = ((int)eq.File.ReportMaterial.MaterialType).ToString();
                    string PRID = eq.File.ReportMaterial.PRID;
                    string PRWeight = eq.File.ReportMaterial.Weight.ToString();

                    MaterialDataUpdate(eqpNo, materialID, materialStatus, materialType, PRID, PRWeight);
                }
                else
                {
                    EQMaterialStatusChangeReportReply(eBitResult.ON, "2");
                }

                #region 建立timer


                Timermanager.CreateTimer(t1TimerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCMaterialStatusChangeReportReplyTimeoutAction), inputData.TrackKey);
                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BC Material Status Change Report BIT [{2}] UnitNo=[{3}] ReturnCode=[{4}][{5}].",
                            eqpNo, inputData.TrackKey, bitResult, unitNo, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : "Other"));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void BCMaterialStatusChangeReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BC Material Data Change Report Reply T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion


        #region Material Validation Request
        public void EQMaterialValidationRequest(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_EQMaterialValidationRequestTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Material Validation Request Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }

                string materialID = inputData[0][0][0].Value.Trim();
                string materialPosition = inputData[0][0][1].Value;
                if (eq.File.CIMMode == eBitResult.ON)
                {
                    CPCMaterialValidationRequest(eBitResult.ON, materialID, materialPosition);
                    Timermanager.CreateTimer(timerID, false, 10000,
                        new System.Timers.ElapsedEventHandler(EQMaterialValidationRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Material Validation Request Bit=[{2}] Material ID=[{3}].",
                            eqpNo, inputData.TrackKey, eBitResult, materialID));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void CPCMaterialValidationRequestReply(Trx inputData, eBitResult eBitResult)
        {
            try
            {
                Trx trx = GetTrxValues("L3_EQDMaterialValidationRequestReply");
                if (trx == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[L3] TRX L3_EQDMaterialValidationRequestReply IN PLCFmt.xml!"));
                string timerID = "L3_EQDMaterialValidationRequestReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQ Material Validation Request Reply Bit=[{2}].",
                            "L3", trx.TrackKey, eBitResult));
                    return;
                }
                for (int i = 0; i < trx[0][0].Items.Count; i++)
                {
                    trx[0][0][i].Value = inputData[0][0][i].Value;
                }
                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCMaterialValidationRequestReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQ Material Validation Request Reply Bit=[{2}] Material ID=[{3}].",
                            "L3", trx.TrackKey, eBitResult, RequestMaterialID));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void EQMaterialValidationRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Material Validation Request TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCMaterialValidationRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                CPCMaterialValidationRequest(eBitResult.OFF, "", "");
                EQRequestMaterialFlag = false;
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Material Validation Request T1 TIMEOUT.", sArray[0], trackKey, sArray[1]));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCMaterialValidationRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                CPCMaterialValidationRequestReply(null, eBitResult.OFF);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQD Material Validation Request Reply T2 TIMEOUT.", sArray[0], trackKey, sArray[1]));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCMaterialValidationRequestReply(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }

                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                string timerID = "L3_BCMaterialValidationRequestReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                if (eBitResult == eBitResult.OFF)
                {

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BC Material Validation Request Reply Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }
                string returnCode = inputData[0][0][0].Value;
                if (!EQRequestMaterialFlag)
                {
                    LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BC Material Validation Request Reply Bit=[{2}] ReturnCode=[{3}][{4}],But EQRequestMaterialFlag=[{5}].",
                            eqpNo, inputData.TrackKey, eBitResult, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : "NONE", EQRequestMaterialFlag));
                    return;
                }

                CPCMaterialValidationRequest(eBitResult.OFF, "", "");
                CPCMaterialValidationRequestReply(inputData, eBitResult.ON);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BC Material Validation Request Reply Bit=[{2}] ReturnCode=[{3}][{4}].",
                            eqpNo, inputData.TrackKey, eBitResult, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : "NONE"));


                if (returnCode == "1")
                {
                    //更新Material信息
                    MaterialEntity material = ObjectManager.MaterialManager.GetMaterialByID(eqpNo, RequestMaterialID);
                    if (material == null)
                    {
                        material = new MaterialEntity(RequestMaterialID);
                        material.NodeNo = "L3";
                        material.UnitNo = 3;
                    }

                    material.UsedCount = int.Parse(inputData[0][0][1].Value);
                    material.Thickness = int.Parse(inputData[0][0][2].Value);
                    material.LimitedCount = int.Parse(inputData[0][0][3].Value);
                    material.LifeTime.Year = int.Parse(inputData[0][0][4].Value);
                    material.LifeTime.Month = int.Parse(inputData[0][0][5].Value);
                    material.LifeTime.Day = int.Parse(inputData[0][0][6].Value);
                    material.LifeTime.Hour = int.Parse(inputData[0][0][7].Value);
                    material.LifeTime.Minute = int.Parse(inputData[0][0][8].Value);
                    material.LifeTime.Second = int.Parse(inputData[0][0][9].Value);

                    material.DueTime.Year = int.Parse(inputData[0][0][10].Value);
                    material.DueTime.Month = int.Parse(inputData[0][0][11].Value);
                    material.DueTime.Day = int.Parse(inputData[0][0][12].Value);
                    material.DueTime.Hour = int.Parse(inputData[0][0][13].Value);
                    material.DueTime.Minute = int.Parse(inputData[0][0][14].Value);
                    material.DueTime.Second = int.Parse(inputData[0][0][15].Value);
                    material.UpdateTime = DateTime.Now;
                    material.Event = "Changed_For_EQ_Request_Data";
                    ObjectManager.MaterialManager.AddMaterial(material);
                    ObjectManager.MaterialManager.SaveMaterialHistory(eq.Data.LINEID, material.UnitNo.ToString(), material, material.MaterialId, $"{material.NodeNo}_{material.UnitNo}_{material.MaterialId}_{material.UpdateTime.ToString("yyyyMMddHHmmss")}", material.UsedCount.ToString());
                }
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Inline Loading Stop Request From Other EQ
        public void InlineLoadingStopRequestFromDownstreamEQ(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_InlineLoadingStopRequestFromDownstreamEQTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                //判断
                string msg = string.Empty;
                string LoadingStopTarget = inputData[0][0][1].Value;
                string LoadingStopSource = inputData[0][0][0].Value;
                if (line.Data.FABTYPE == "CELL" || line.Data.FABTYPE == "ARRAY")
                {
                    if (LoadingStopTarget == "3")
                    {

                        if (LoadingStopSource == "4")
                        {
                            msg = "TR01";
                        }
                        else if (LoadingStopSource == "6")
                        {
                            msg = "TR02";
                        }
                        else if (LoadingStopSource == "2")
                        {
                            msg = "Indexer";
                        }
                    }
                }
                else if (line.Data.FABTYPE == "CF")
                {
                    if (line.Data.ATTRIBUTE == "CLN")
                    {
                        if (LoadingStopTarget == "10" && LoadingStopSource == "5")
                        {
                            msg = "TR02";
                        }
                    }
                    else if (line.Data.ATTRIBUTE == "DEV" && LoadingStopSource == "8")
                    {
                        if (LoadingStopTarget == "14")
                        {
                            msg = "TR05";
                        }
                    }
                }


                if (eBitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                               string.Format("[EQUIPMENT={0}] [EQ -> EQ][{1}] Inline Loading Stop Request From [{2}] BIT=[{3}] LoadingStopSource=[{4}] LoadingStopTarget=[{5}].",
                               eqpNo, inputData.TrackKey, msg == string.Empty ? "Request Data Error !" : msg, eBitResult, LoadingStopSource, LoadingStopTarget));
                    return;
                }
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(InlineLoadingStopRequestFromDownstreamEQTimeoutAction), UtilityMethod.GetAgentTrackKey());
                //触发
                if (msg != string.Empty)
                {
                    EQDLoadingStopCommand(eBitResult.ON, "1");
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                               string.Format("[EQUIPMENT={0}] [EQ -> EQ][{1}] Inline Loading Stop Request From [{2}] BIT=[{3}] LoadingStopSource=[{4}] LoadingStopTarget=[{5}].",
                               eqpNo, inputData.TrackKey, msg, eBitResult, LoadingStopSource, LoadingStopTarget));
                }
                else
                {
                    LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                               string.Format("[EQUIPMENT={0}] [EQ -> EQ][{1}] Inline Loading Stop Request From [{2}] BIT=[{3}] LoadingStopSource=[{4}] LoadingStopTarget=[{5}].",
                               eqpNo, inputData.TrackKey, "Request Data Error !", eBitResult, LoadingStopSource, LoadingStopTarget));
                }
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void InlineLoadingStopRequestFromDownstreamEQTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EQ][{1}] Inline Loading Stop Request From Downstream EQ T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQDLoadingStopCommand(eBitResult eBitResult, string status)
        {
            string timerID = "L3_EQDLoadingStopCommandTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            Trx trx = GetTrxValues("L3_EQDLoadingStopCommand");
            trx[0][0][0].Value = status;
            if (eBitResult == eBitResult.ON)
            {
                trx[0][1][0].Value = "1";
                Timermanager.CreateTimer(timerID, false, 4000, new System.Timers.ElapsedEventHandler(EQDLoadingStopCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            else
            {
                trx[0][1][0].Value = "0";
            }
            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(trx);
            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Loading Stop Command BIT=[{2}] Status=[{3}][{4}].",
                            trx.Metadata.NodeNo, trx.TrackKey, eBitResult, status, status == "1" ? "Stop" : "Resume"));
        }

        public void EQDLoadingStopCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                EQDLoadingStopCommand(eBitResult.OFF, "0");
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Loading Stop Command BIT Auto Reset.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Dummy Glass Request
        public void EQDummyGlassRequest(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_EQDummyGlassRequestTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Dummy Glasss Request Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                string dummyGlassCount = inputData[0][0][0].Value.Trim();
                string dummyType = inputData[0][0][1].Value;
                string targetLocalNumber = inputData[0][0][2].Value;
                string recipeID = string.Empty;
                string DummyTargetBufferNo = "0";
                string DummySlotNo = "0";
                if (line.Data.FABTYPE != "CF")
                {
                    recipeID = inputData[0][0][3].Value.Trim();
                }
                if (line.Data.FABTYPE == "CELL")
                {
                    DummyTargetBufferNo = inputData[0][0][4].Value;
                    DummySlotNo = inputData[0][0][5].Value;
                }

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    if (line.Data.FABTYPE != "CELL")
                    {
                        CPCDummyGlassRequest(eBitResult.ON, dummyGlassCount, dummyType, targetLocalNumber, recipeID);
                    }
                    else
                    {
                        CPCDummyGlassRequestEQP(eBitResult.ON, dummyGlassCount, dummyType, targetLocalNumber, recipeID, DummyTargetBufferNo, DummySlotNo);
                    }
                    Timermanager.CreateTimer(timerID, false, 10000,
                        new System.Timers.ElapsedEventHandler(EQDummyGlassRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ Dummy Glass Request Bit=[{2}] DummyGlassCount=[{3}] DummyType=[{4}] TargetLocalNumber=[{5}].",
                            eqpNo, inputData.TrackKey, eBitResult, dummyGlassCount, dummyType, targetLocalNumber));
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQDummyGlassRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EQ][{1}] Dummy Glass Request TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCDummyGlassRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCDummyGlassRequest(eBitResult.OFF, "", "", "", "");
                CPCDummyGlassRequestReply(eBitResult.ON, "2");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Dummy Glass Request T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCDummyGlassRequestReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_BCDummyGlassRequestReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Dummy Glasss Request Reply Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }

                string returnCode = inputData[0][0][0].Value.Trim();
                if (eq.File.CIMMode == eBitResult.ON)
                {
                    CPCDummyGlassRequest(eBitResult.OFF, "", "", "", "");
                    CPCDummyGlassRequestReply(eBitResult.ON, returnCode);
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCDummyGlassRequestReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Dummy Glass Request Reply Bit=[{2}] ReturnCode=[{3}][{4}].",
                            eqpNo, inputData.TrackKey, eBitResult, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : "Other"));
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void BCDummyGlassRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Dummy Glass Request Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCDummyGlassRequestReply(eBitResult eBitResult, string returnCode)
        {
            try
            {
                Trx trx = GetTrxValues("L3_EQDummyGlassRequestReply");
                if (trx == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[L3] TRX L3_EQDummyGlassRequestReply IN PLCFmt.xml!"));
                string timerID = "L3_CPCDummyGlassRequestReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Dummy Glass Request Reply BIT=[{2}].", "L3", trx.TrackKey, "OFF"));
                    return;
                }

                trx[0][0][0].Value = returnCode;
                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Dummy Glass Request Reply BIT=[{2}]. ReturnCode=[{3}][{4}]", "L3", trx.TrackKey, "ON", returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : "Other"));
                Timermanager.CreateTimer(timerID, false, 3000,
                    new System.Timers.ElapsedEventHandler(CPCDummyGlassRequestReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public void CPCDummyGlassRequestReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCDummyGlassRequestReply(eBitResult.OFF, "0");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Dummy Glass Request Reply BIT Auto Reset.", sArray[0], trackKey));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region VCR Event Report
        public void EQVCREventReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_EQVCREventReportTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (eBitResult == eBitResult.OFF)
                {
                    EQVCREventReportReply(eBitResult.OFF);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ VCR Event Report Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }

                string jobID = inputData[0][0][0].Value;
                string cassetteSequenceNo = inputData[0][0][1].Value;
                string slotSequenceNo = inputData[0][0][2].Value;
                string unitNo = inputData[0][0][3].Value;
                string VCRNo = inputData[0][0][4].Value;
                string VCRResult = inputData[0][0][5].Value;
                EQVCREventReportReply(eBitResult.ON);
                string msg = string.Empty;
                if (VCRResult == "1")
                {
                    msg = "VCR Reading OK, Match With Job Data Job ID";
                }
                else if (VCRResult == "2")
                {
                    msg = "VCR Reading OK, Miss Match With Job Data Job ID";
                }
                else if (VCRResult == "3")
                {
                    msg = "VCR Reading Fail, Match With Job Data Job ID";
                }
                else if (VCRResult == "4")
                {
                    msg = "VCR Reading Fail, Miss Match With Job Data Job ID";
                }
                CPCVCREventReport(eBitResult.ON, jobID, cassetteSequenceNo, slotSequenceNo, unitNo, VCRNo, VCRResult, msg);
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(EQVCREventReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ VCR Event Report Bit=[{2}] JobID=[{3}] CstNo=[{4}] SlotNo=[{5}] UnitNo=[{6}] VCRNo=[{7}] VCRResult=[{8}][{9}].",
                            eqpNo, inputData.TrackKey, eBitResult, jobID, cassetteSequenceNo, slotSequenceNo, unitNo, VCRNo, VCRResult, msg));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQVCREventReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] EQ VCR Event Report T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCVCREventReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCVCREventReport(eBitResult.OFF, "", "0", "0", "0", "0", "0", "");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] EQ VCR Event Report T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQVCREventReportReply(eBitResult eBitResult)
        {
            try
            {
                Trx trx = GetTrxValues("L3_EQVCREventReportReply");
                string timerID = "L3_EQVCREventReportReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                trx[0][0][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                if (eBitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQVCREventReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQ VCR Event Report Reply Bit=[{2}].",
                            trx.Metadata.NodeNo, trx.TrackKey, eBitResult));
            }
            catch (Exception ex)
            {

            }
        }

        public void EQVCREventReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                EQVCREventReportReply(eBitResult.OFF);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] EQ VCR Event Report Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCVCREventReportReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);

                string timerID = "L3_BCVCREventReportReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (eBitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] VCR Event Report Reply Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }
                CPCVCREventReport(eBitResult.OFF, "", "0", "0", "0", "0", "0", "");
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(BCVCREventReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] VCR Event Report Reply Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void BCVCREventReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] VCR Event Report Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region VCR Enable Mode Change Command
        public void BCVCREnableModeChangeCommand(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }

                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                string timerID = "L3_BCVCREnableModeChangeCommandTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eBitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] VCR Enable Mode Change Command Bit=[{2}].",
                            eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }
                string VCRNo = inputData[0][0][0].Value;
                string VCRMode = inputData[0][0][1].Value;

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    CPCVCREnableModeChangeCommandReply(eBitResult.ON);
                    if (VCRMode == "1" || VCRMode == "2")
                    {
                        EQDVCRModeChangeCommand(eBitResult.ON, VCRMode);
                    }
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCVCREnableModeChangeCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] VCR Enable Mode Change Command Bit=[{2}] VCRNo=[{3}] VCRMode=[{4}].",
                            eqpNo, inputData.TrackKey, eBitResult, VCRNo, VCRMode));
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCVCREnableModeChangeCommandReply(eBitResult eBitResult)
        {
            try
            {
                string timerID = "L3_VCR1EnableModeChangeCommandReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                Trx trx = GetTrxValues("L3_VCR1EnableModeChangeCommandReply");

                trx[0][0][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);


                if (eBitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCVCREnableModeChangeCommandReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] VCR Enable Mode Change Command Reply Bit=[{2}].",
                            trx.Metadata.NodeNo, trx.TrackKey, eBitResult));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCVCREnableModeChangeCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCVCREnableModeChangeCommandReply(eBitResult.OFF);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] VCR Enable Mode Change Command Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }



        public void BCVCREnableModeChangeCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] VCR Enable Mode Change Command T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQDVCRModeChangeCommand(eBitResult eBitResult, string VCRMode)
        {
            try
            {
                string timerID = "L3_EQDVCRModeChangeCommandTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                Trx trx = GetTrxValues("L3_EQDVCRModeChangeCommand");
                trx[0][0][0].Value = VCRMode;
                trx[0][1][0].Value = ((int)eBitResult).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                if (eBitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQDVCRModeChangeCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] VCR Enable Mode Change Command BIT=[{2}] VCRMode=[{3}].", trx.Metadata.NodeNo, trx.TrackKey, eBitResult, VCRMode));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQDVCRModeChangeCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                EQDVCRModeChangeCommand(eBitResult.OFF, "0");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] VCR Enable Mode Change Command T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQDVCRModeChangeCommandReply(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eq == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                string timerID = "L3_EQDVCRModeChangeCommandReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                eBitResult eBitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                if (eBitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] VCR Enable Mode Change Command Reply BIT=[{2}].", eqpNo, inputData.TrackKey, eBitResult));
                    return;
                }

                string returnCode = inputData[0][0][0].Value;
                EQDVCRModeChangeCommand(eBitResult.OFF, "0");

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    new System.Timers.ElapsedEventHandler(EQDVCRModeChangeCommandReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] VCR Enable Mode Change Command Reply BIT=[{2}] ReturnCode=[{3}][{4}].", eqpNo, inputData.TrackKey, eBitResult, returnCode, returnCode == "1" ? "OK" : returnCode == "2" ? "NG" : "Other"));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void EQDVCRModeChangeCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] VCR Enable Mode Change Command Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Recipe Step Count Request Command
        public void RecipeStepCountRequestCommand(Trx inputData)
        {
            try
            {
                if (inputData.IsInitTrigger)
                {
                    return;
                }
                string eqpNo = inputData.Metadata.NodeNo;
                string strlog = string.Empty;

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[1].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, "RecipeStepCountRequestCommandTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe Step Count Request Command.",
                        eqpNo, inputData.TrackKey));

                    CPCRecipeStepCountRequestCommandReply(eBitResult.OFF, "", "", "0", "0");

                    return;
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(RecipeStepCountRequestCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion

                string masterRecipeID = inputData[0][0][0].Value.Trim();//Master Recipe ID
                string localRecipeID = inputData[0][0][1].Value.Trim();//Local Recipe ID



                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Step Count Request Command MasterRecipeID=[{2}] LocalRecipeID=[{3}].",
                     eq.Data.NODENO, inputData.TrackKey, masterRecipeID, localRecipeID));
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipeByNo();
                }
                else
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipe();
                }

                if (recipeDic[eq.Data.LINEID].ContainsKey(localRecipeID))
                {

                    CPCRecipeStepCountRequestCommandReply(eBitResult.ON, masterRecipeID, localRecipeID, "1", recipeDic[eq.Data.LINEID][localRecipeID].OPERATORID);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Step Count Request Command Reply OK.",
                            eq.Data.NODENO, inputData.TrackKey));

                }
                else
                {
                    CPCRecipeStepCountRequestCommandReply(eBitResult.ON, masterRecipeID, localRecipeID, "2", "0");

                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Step Count Request Command Reply NG Local RecipeID=[{2}] Not exist.",
                            eq.Data.NODENO, inputData.TrackKey, localRecipeID));
                }


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        public void CPCRecipeStepCountRequestCommandReply(eBitResult eBitResult, string masterRecipeID, string localRecipeID, string returnCode, string VerSeq)
        {
            try
            {
                string timerID = "L3_RecipeStepCountRequestCommandReplyTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Trx trx = GetTrxValues("L3_RecipeStepCountRequestCommandReply");
                if (eBitResult == eBitResult.ON)
                {
                    trx[0][0][0].Value = masterRecipeID;
                    trx[0][0][1].Value = localRecipeID;
                    trx[0][0][2].Value = VerSeq;
                    trx[0][0][3].Value = "1";
                    trx[0][0][4].Value = returnCode;
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    trx[0][1][0].Value = "1";
                }
                else
                {
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    trx[0][1][0].Value = "0";
                }
                trx[0][1].OpDelayTimeMS = 200;
                SendToPLC(trx);

                if (eBitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(), new System.Timers.ElapsedEventHandler(CPCRecipeStepCountRequestCommandReplyimeoutAction), UtilityMethod.GetAgentTrackKey());
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[{2}] Recipe Step Count Request Command Reply.",
                            eq.Data.NODENO, trx.TrackKey, eBitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void RecipeStepCountRequestCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Step Count Request Command T1 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCRecipeStepCountRequestCommandReplyimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                CPCRecipeStepCountRequestCommandReply(eBitResult.OFF, "", "", "0", "0");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recipe Step Count Request Command Reply T2 TIMEOUT.", sArray[0], trackKey));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        public Job CreateJob(string casseqno, string jobseqno, Trx inputData)
        {
            try
            {

                int c, j;
                if (!int.TryParse(casseqno, out c))
                    throw new Exception(string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!", casseqno));

                if (c == 0)
                    throw new Exception(string.Format("Create Job Failed Cassette Sequence No ({0})!!", casseqno));

                if (!int.TryParse(jobseqno, out j))
                    throw new Exception(string.Format("Create Job Failed Job Sequence No isn't number ({0})!!", jobseqno));

                if (j == 0) throw new Exception(string.Format("Create Job Failed Job Sequence No ({0})!!", jobseqno));

                Job job = new Job(c, j);

                if (inputData == null)
                {

                    ObjectManager.JobManager.AddJob(job);
                    return job;
                }


                Event eVent = inputData.EventGroups[0].Events[0];
                job.CurrentEQPNo = eq.Data.ATTRIBUTE;//inputData.Metadata.NodeNo;
                job.GlassID_or_PanelID = inputData[0][0][eJOBDATA.GlassID_or_PanelID].Value.ToString().Trim();
                job.JobId = inputData[0][0][eJOBDATA.GlassID_or_PanelID].Value.ToString().Trim();

                ObjectManager.JobManager.AddJob(job);

                return job;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return null;
        }

        public Job CreateJob(string casseqno, string jobseqno, Event evt)
        {
            try
            {

                int c, j;
                if (!int.TryParse(casseqno, out c))
                    throw new Exception(string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!", casseqno));

                if (c == 0)
                    throw new Exception(string.Format("Create Job Failed Cassette Sequence No ({0})!!", casseqno));

                if (!int.TryParse(jobseqno, out j))
                    throw new Exception(string.Format("Create Job Failed Job Sequence No isn't number ({0})!!", jobseqno));

                if (j == 0) throw new Exception(string.Format("Create Job Failed Job Sequence No ({0})!!", jobseqno));

                Job job = new Job(c, j);

                if (evt == null)
                {

                    ObjectManager.JobManager.AddJob(job);
                    return job;
                }



                job.CurrentEQPNo = eq.Data.ATTRIBUTE;//"L3";
                job.GlassID_or_PanelID = evt[eJOBDATA.GlassID_or_PanelID].Value.ToString().Trim();
                job.JobId = evt[eJOBDATA.GlassID_or_PanelID].Value.ToString().Trim();

                ObjectManager.JobManager.AddJob(job);

                return job;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return null;
        }

        /// <summary>
        /// Update Job Data
        /// </summary>
        /// <param name="eq"></param>
        /// <param name="job"></param>
        /// <param name="inputData"></param>
        public void UpdateJobData(Equipment eq, Job job, Trx inputData)
        {
            try
            {

                #region JobData Common
                lock (job)
                {

                    Type type = typeof(Job);
                    PropertyInfo propertyInfo = null;
                    for (int i = 0; i < inputData[0][0].Items.Count; i++)
                    {
                        propertyInfo = type?.GetProperty(inputData[0][0][i].Name);
                        if (propertyInfo == null)
                            continue;
                        string valueType = propertyInfo?.PropertyType.Name;
                        if (valueType == null)
                            continue;
                        if (valueType.Contains("Int"))
                        {
                            propertyInfo?.SetValue(job, int.Parse(inputData[0][0][i].Value), null);
                        }
                        else
                        {
                            propertyInfo?.SetValue(job, inputData[0][0][i].Value, null);
                        }
                    }

                }

                #endregion
                #region By EQ Update Job Data
                //eFabType fabtype;
                //Enum.TryParse<eFabType>(line.Data.FABTYPE, out fabtype);
                //switch (fabtype)
                //{
                //    case eFabType.ARRAY:

                //       // UpdateJobData_Array(line, job, inputData[0][0]);
                //        break;
                //    case eFabType.CF:

                //       // UpdateJobData_CF(line, job, inputData[0][0]);
                //        break;
                //    case eFabType.CELL:

                //       // UpdateJobData_Cell(line, job, inputData[0][0]);
                //        break;
                //    case eFabType.MOD:
                //        // To Do Mod 
                //        break;
                //    default:
                //        break;

                //  }
                #endregion

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void UpdateJobData(Equipment eq, Job job, Event evt)
        {
            try
            {

                #region JobData Common
                lock (job)
                {
                    Type type = typeof(Job);
                    PropertyInfo propertyInfo = null;
                    for (int i = 0; i < evt.Items.Count; i++)
                    {
                        propertyInfo = type?.GetProperty(evt[i].Name);
                        if (propertyInfo == null)
                            continue;
                        string valueType = propertyInfo?.PropertyType.Name;
                        if (valueType == null)
                            continue;
                        if (valueType.Contains("Int"))
                        {
                            propertyInfo?.SetValue(job, int.Parse(evt[i].Value), null);
                        }
                        else
                        {
                            propertyInfo?.SetValue(job, evt[i].Value, null);
                        }
                    }
                }

                #endregion
                job.CurrentEQPNo = eq.Data.ATTRIBUTE;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void SetJobData(Equipment eq, Job job, Trx inputData)
        {
            try
            {

                #region JobData Common
                lock (job)
                {
                    Type type = typeof(Job);
                    PropertyInfo propertyInfo;
                    for (int i = 0; i < inputData[0][0].Items.Count; i++)
                    {
                        propertyInfo = type?.GetProperty(inputData[0][0][i].Name);

                        inputData[0][0][i].Value = propertyInfo?.GetValue(job, null)?.ToString();

                    }

                }

                #endregion

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void SetJobDataD(Equipment eq, Job job, Trx inputData)
        {
            try
            {


                lock (job)
                {
                    //inputData[0][0][eJOBDATA.CassetteSequenceNumber].Value = job.CassetteSequenceNo.ToString();
                    //inputData[0][0][eJOBDATA.SlotSequenceNumber].Value = job.JobSequenceNo.ToString();
                    //inputData[0][0][eJOBDATA.JobID].Value = job.JobId.Trim();
                    //inputData[0][0][eJOBDATA.GlassJudgeCode].Value = job.GlassJudgeCode.Trim();
                    //inputData[0][0][eJOBDATA.GlassGradeCode].Value = job.GlassGradeCode.Trim();

                }



            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        public static void ServerPathRefresh(string path)
        {
            string[] res = Ftp.GetListDirectoryDetails(DFSService.UserName, DFSService.Password, path);
            for (int i = 0; i < res.Length - 1; i++)
            {
                string name = res[i];
                if (res[i].Contains("<DIR>"))
                {
                    name = res[i].Split(new string[] { "<DIR>" }, StringSplitOptions.None)[1].Trim() + "/";
                    DirectoryPathList.Add($"{path}{name}");
                }
                else
                {
                    string[] strTmp = res[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    name = strTmp[strTmp.Length - 1].Trim();
                }
            }
            if (res.Length <= 2)
            {
                if (res[res.Length - 1] == "")
                {
                    if (res.Length == 1)
                    {

                    }
                    else
                    {
                        string name = res[0];
                        if (name.Contains("<DIR>"))
                        {
                            string olname = name.Split(new string[] { "<DIR>" }, StringSplitOptions.None)[1].Trim();
                            ServerPathRefresh(path + olname + "/");
                        }
                    }
                }
            }
            else
            {
                foreach (string name in res)
                {
                    if (name.Contains("<DIR>"))
                    {
                        string olname = name.Split(new string[] { "<DIR>" }, StringSplitOptions.None)[1].Trim();
                        ServerPathRefresh(path + olname + "/");
                    }
                }
            }
        }

        #region Import Library

        [StructLayout(LayoutKind.Sequential)]
        public class SystemTime
        {
            public ushort year;
            public ushort month;
            public ushort dayOfWeek;
            public ushort day;
            public ushort hour;
            public ushort minute;
            public ushort second;
            public ushort milliseconds;
        }

        public class Win32API
        {
            [DllImport("Kernel32.dll")]
            public static extern void GetSystemTime([In, Out] SystemTime st);

            [DllImport("Kernel32.dll")]
            public static extern void SetSystemTime([In, Out] SystemTime st);
        }

        #endregion

        public void SetPCSystemTime(string dateTime)
        {
            try
            {
                DateTime utcTime;
                utcTime = DateTime.ParseExact(dateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                utcTime = utcTime.ToUniversalTime();
                SystemTime systime = new SystemTime();
                systime.year = (ushort)utcTime.Year;
                systime.month = (ushort)utcTime.Month;
                systime.dayOfWeek = (ushort)utcTime.DayOfWeek;
                systime.day = (ushort)utcTime.Day;
                systime.hour = (ushort)utcTime.Hour;
                systime.minute = (ushort)utcTime.Minute;
                systime.second = (ushort)utcTime.Second;
                systime.milliseconds = 0;

                Win32API.SetSystemTime(systime);

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        public string GetProcessingTime(string StartTime, string EndTime)
        {
            try
            {
                DateTime utcStartTime;
                DateTime utcEndTime;
                utcStartTime = DateTime.ParseExact("20" + StartTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                utcStartTime = utcStartTime.ToUniversalTime();

                utcEndTime = DateTime.ParseExact("20" + EndTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                utcEndTime = utcEndTime.ToUniversalTime();

                // return utcEndTime.Subtract(utcStartTime).TotalSeconds.ToString();


                TimeSpan ts1 = new TimeSpan(utcEndTime.Ticks);
                TimeSpan ts2 = new TimeSpan(utcStartTime.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();

                return ts.TotalSeconds.ToString();


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return "0";
            }
        }
        public void SendToPLC(Trx trx)
        {
            xMessage message = new xMessage()
            {
                ToAgent = eAgentName.PLCAgent,
                Data = trx,
                Name = trx.Name,
                TransactionID = trx.TrackKey
            };
            PutMessage(message);
        }
        /// <summary>
        /// 判断写入PLC的时间是否合法，范围(1980年1月1日~2079年12月31日)
        /// </summary>
        /// <param name="time">格式 yyyyMMddHHmmss</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public bool CheckDatetimeValid(string time, out string msg)
        {
            if (time.Length != 14)
            {
                msg = "Check NG,time type isn't yyyyMMddHHmmss";
                return false;
            }
            string strYear = time.Substring(0, 4);
            string strMonth = time.Substring(4, 2);
            string strDay = time.Substring(6, 2);
            string strHour = time.Substring(8, 2);
            string strMinute = time.Substring(10, 2);
            string strSecond = time.Substring(12, 2);

            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;
            int second = 0;

            if (int.TryParse(strYear, out year) && int.TryParse(strMonth, out month) && int.TryParse(strDay, out day) && int.TryParse(strHour, out hour) && int.TryParse(strMinute, out minute) && int.TryParse(strSecond, out second))
            {
                if (year < 1980 || year > 2079)
                {
                    msg = $"Check NG,year={year} over range[1980~2079]";
                    return false;
                }
                if (month < 1 || month > 12)
                {
                    msg = "Check NG,month over range[1~12]";
                    return false;
                }
                if (day < 1 || day > 31)
                {
                    msg = "Check NG,day over range[1~31]";
                    return false;
                }
                else
                {
                    if (month == 4 || month == 6 || month == 9 || month == 11)
                    {
                        if (day == 31)
                        {
                            msg = $"Check NG,month={month} day={day} over range[1~30]";
                            return false;
                        }
                    }
                    else if (month == 2)
                    {
                        if (day > 29)
                        {
                            msg = $"Check NG,month={month} day={day} over range[1~28/29]";
                            return false;
                        }
                        else if (day == 29)
                        {
                            //闰年能被4整除同时不能被100整除，或者能被400整除的年份
                            if (!(year % 4 == 0 && year % 100 != 0 || year % 400 == 0))
                            {
                                msg = $"Check NG,year={year} isn't a leap year,month={month} day={day} over range[1~28]";
                                return false;
                            }
                        }
                    }
                }
                if (hour < 0 || hour > 23)
                {
                    msg = "Check NG,hour over range";
                    return false;
                }
                if (minute < 0 || minute > 59)
                {
                    msg = "Check NG,minute over range";
                    return false;
                }
                if (second < 0 || second > 59)
                {
                    msg = "Check NG,second over range";
                    return false;
                }
            }
            else
            {
                msg = "Check NG,time string convert to int failed";
                return false;
            }

            msg = "Check OK";
            return true;
        }
    }
}



