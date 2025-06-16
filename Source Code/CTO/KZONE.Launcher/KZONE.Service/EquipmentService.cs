using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using KZONE.ConstantParameter;
using KZONE.MessageManager;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.PLCAgent;
using KZONE.Work;
using System.Reflection;
using KZONE.PLCAgent.PLC;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using OfficeOpenXml;
using EipTagLibrary;
using static OfficeOpenXml.ExcelErrorValue;

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
        private Thread _unitReasonCodeThread;
        private Thread _storeThread;
        private Thread _fetchThread;
        private Thread _recipeReport;
        private Thread _processDataReport;
        private Thread _tackTimeReport;

        private Thread _svreport;
        private Thread _cvreport;

        List<string> oldAlarms = new List<string>();

        Dictionary<AlarmEntityData, String> alarmDic = new Dictionary<AlarmEntityData, string>();

        Dictionary<int, bool> alarmChannel = new Dictionary<int, bool>();
        Dictionary<int, Unit> unitDic = new Dictionary<int, Unit>();
        Dictionary<int, Unit> unitReasonCodeDic = new Dictionary<int, Unit>();
        Dictionary<int, bool> stroeChannel = new Dictionary<int, bool>();
        Dictionary<int, bool> fetchChannel = new Dictionary<int, bool>();



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

        private bool NGbyStopBit = false;

        private bool UnitRecipeRequestBool = true;//上报UnitRecipeRequest

        private string _SVSavePath = string.Empty;//SV数据的保存路径

        private string _CVSavePath = string.Empty;//CV数据保存的路径

        private int AlarmCount = 0;

        #region HKC新增
        private int T1 = 4000;
        private int T2 = 2000;
        private int T3 = 500;
        private int T4 = 2000;
        private int Tm = 200;
        private int Tn = 200;

        private int LinkSignalT2 = 60000;
        private int LinkSignalT3 = 2000;
        private int LinkSignalT4 = 60000;
        private int LinkSignalT5 = 2000;


        #endregion



        public override bool Init()
        {

            StartEIP();


            _eqAlive = new Thread(new ThreadStart(EQAlive)) { IsBackground = true };
            _eqAlive.Start();

            _receiveThread = new Thread(new ThreadStart(ReceiveGlassAction)) { IsBackground = true };
            _receiveThread.Start();
            _sendThread = new Thread(new ThreadStart(SendGlassAction)) { IsBackground = true };
            _sendThread.Start();
            _alarmThread = new Thread(new ThreadStart(CPCAlarmReportBC)) { IsBackground = true };
            _alarmThread.Start();

            _storeThread = new Thread(new ThreadStart(CPCUnitStoreReport)) { IsBackground = true };
            _storeThread.Start();

            _fetchThread = new Thread(new ThreadStart(CPCUnitFetchReport)) { IsBackground = true };
            _fetchThread.Start();

            _processDataReport = new Thread(new ThreadStart(CPCProcessDataReport)) { IsBackground = true };
            _processDataReport.Start();

            _svreport = new Thread(new ThreadStart(EquipmentSVReport)) { IsBackground = true };
            _svreport.Start();

            //_cvreport = new Thread(new ThreadStart(EquipmentCVReport)) { IsBackground = true };
            //_cvreport.Start();

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

                CPCEQDBCAlive(alive);

                if (eqp.File.AliveTimeout == true)
                {
                    CPCBCAliveTimeout(eqp, eBitResult.OFF);

                }
                //去掉Timeout标记
                eqp.File.AliveTimeout = false;
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                if (_timerManager.IsAliveTimer(inputData.Metadata.NodeNo + "_" + BCAliveTimeout))
                {
                    _timerManager.TerminateTimer(inputData.Metadata.NodeNo + "_" + BCAliveTimeout);
                }

                _timerManager.CreateTimer(inputData.Metadata.NodeNo + "_" + BCAliveTimeout, false, ParameterManager["BCALIVE"].GetInteger(),
                                            new System.Timers.ElapsedEventHandler(CheckBCAliveTimeout), inputData.TrackKey);


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
            while (true)
            {
                try
                {
                    if (_isRuning)
                    {


                        Trx outputdata = GetServerAgent("PLCAgent").GetTransactionFormat("L3_EQAlive") as Trx;
                        if (outputdata != null)//在BC關閉的過程會取不到trx
                        {
                            outputdata.EventGroups[0].Events[0].Items[0].Value = _aliveValue;
                            outputdata.TrackKey = UtilityMethod.GetAgentTrackKey();
                            xMessage msg = new xMessage();
                            msg.ToAgent = eAgentName.PLCAgent;

                            msg.Data = outputdata;
                            PutMessage(msg);
                        }
                    }
                    //M软元件 EQ Alive
                    CPCEQDEQAlive(_aliveValue);

                    _aliveValue = _aliveValue.Equals("1") ? "0" : "1";

                    int param = ParameterManager["EQPALIVE"].GetInteger();
                    if (_aliveValue == "1")
                    {
                        Thread.Sleep(60000);
                    }
                    else
                    {
                        Thread.Sleep(4000);
                    }
                   // Block receiveData = eipTagAccess.ReadBlockValues("SD_EQToCIM_MachineJobEvent_03_01_00", "ReceivedJobDataBlock#1");


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

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "DateTimeRequest", "false");
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

        #region Equipment Operation Mode Report  

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

                if (eqp.File.EquipmentOperationMode == eEQPOperationMode.AUTO)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineAutoMode", "true");

                    //AlarmEntityData alarm = ObjectManager.AlarmManager.GetAlarmProfile(eqpNo, "M7886");
                    //if (alarm != null && !alarmDic.ContainsKey(alarm))
                    //{

                    //    alarmDic.Add(alarm, "0");
                    //}

                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineAutoMode", "false");
                }

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

        #region Unit State Change Report/EQ State Change Report / All Reason Code Report 
        eEQPStatus eqOldStatus = eEQPStatus.Unused;
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
                    if ((eq.Data.LINEID == "KWF22003L" || eq.Data.LINEID == "KWF22004L") && int.Parse(pathNo) > 3)
                    {
                        return;
                    }
                    throw new Exception(string.Format("CAN'T FIND UNIT_NO=[{0}] IN UNITENTITY!", pathNo));
                }

                string currentState = inputData[0][0]["CurrentState"].Value.Trim();
                string currentCount = inputData[0][0]["CurrentCount"].Value;

                eEQPStatus unitOldStatus = unit.File.Status;
               //  eqOldStatus = eq.File.Status;

                //1:RUN、2：Down、3：IDLE、4:PM、5:PM、6:STOP 
                switch (currentState)
                {
                    case "1"://单元状态为运行，当有片时为RUN,当没有片时候为IDLE;
                        //if (currentCount != "0")
                        //{
                            unit.File.Status = eEQPStatus.Run;
                        //}
                        //else
                        //{
                        //    unit.File.Status = eEQPStatus.Idle;
                        //}
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

                    case "6"://设备暂停状态
                        unit.File.Status = eEQPStatus.Stop;
                        break;
                    default:

                        break;
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

                    if (unitReasonCodeDic.ContainsKey(unit.Data.UNITNO))
                    {
                        unitReasonCodeDic[unit.Data.UNITNO] = unit;
                    }
                    else
                    {
                        unitReasonCodeDic.Add(unit.Data.UNITNO, unit);
                    }

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
                else if (unitStatusDic.ContainsValue(eEQPStatus.Stop) && !unitStatusDic.ContainsValue(eEQPStatus.Down) && !unitStatusDic.ContainsValue(eEQPStatus.Initial))
                {
                    eq.File.Status = eEQPStatus.Stop;
                }
                else if (unitStatusDic.ContainsValue(eEQPStatus.Run) && !unitStatusDic.ContainsValue(eEQPStatus.Initial) && !unitStatusDic.ContainsValue(eEQPStatus.Down) && !unitStatusDic.ContainsValue(eEQPStatus.Stop))
                {
                    eq.File.Status = eEQPStatus.Run;
                }
                else if (unitStatusDic.ContainsValue(eEQPStatus.Idle) && !unitStatusDic.ContainsValue(eEQPStatus.Initial) && !unitStatusDic.ContainsValue(eEQPStatus.Down) && !unitStatusDic.ContainsValue(eEQPStatus.Run) && !unitStatusDic.ContainsValue(eEQPStatus.Stop))
                {
                    eq.File.Status = eEQPStatus.Idle;
                }
                else
                {
                    eq.File.Status = eEQPStatus.Unused;
                }
                //if (eq.File.Status != eqOldStatus)

                //{
                //    CPCEquipmentStatusChangeReport(eq, inputData.TrackKey, eBitResult.ON);

                //}
                //保存本次的状态
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                ObjectManager.UnitManager.EnqueueSave(unit.File);

            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        #endregion

        #region Equipment Status Change Report
       

        private void CPCEquipmentStatusChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineStatusChangeReport", false);

                //AlarmEntityData alarm = ObjectManager.AlarmManager.GetAlarmProfile(eqpNo, "M7886");
                //if (alarm != null && !alarmDic.ContainsKey(alarm))
                //{
                 
                //    alarmDic.Add(alarm, "1");
                //}



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
                    eqp.File.LastEquipmentRunMode = eqp.File.EquipmentRunMode;
                    eqp.File.EquipmentRunMode = inputData[0][0][0].Value;
                }
                string mode = "0";
                if (eqp.File.EquipmentRunMode == "1")
                {
                    mode = "1";
                }
                else if (eqp.File.EquipmentRunMode == "2")
                {
                    mode = "2";
                }
                else if (eqp.File.EquipmentRunMode == "3")
                {
                    mode = "5";
                }
                else if (eqp.File.EquipmentRunMode == "4")
                {
                    mode = "15";
                }
                else {
                    mode = "1";
                }
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineModeChangeReportBlock", "MachineMode", mode);

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineModeChangeReport", true);

              string  timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeChangeReportReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCEquipmentRunModeChangeReportTimeoutAction), inputData.TrackKey);   

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
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

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineModeChangeReport", false);


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
                    // bit off移除本次timer
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
                   string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Operator Login Logout Report . ", eqpNo, inputData.TrackKey));
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
       
        private void CPCOperatorLoginLogoutReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "OperatorLoginReport", false);

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

        #region SendingGlassDataReport

        private void CPCSendingGlassDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_PathNo_SendingGlassDataReportTimeout
                eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "SentOutJobReport", false);

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
                    Timermanager.CreateTimer(t2TimerID, false, T2,
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
                E2BTimeoutCommand(eBitResult.ON, "T2");
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

                eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "ReceivedJobReport", false);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Reeive Glass Data Report BC Reply T1 TIMEOUT.", sArray[0], trackKey));


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

                CPCReceiveJobDataReport(eBitResult.OFF);

                #region 建立timer

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(t2TimerID, false, T2,
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
                E2BTimeoutCommand(eBitResult.ON, "T2");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Reeive Glass Data Report Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
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

        #region Message Set Command

        private void BCMessageDisplayCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                E2BTimeoutCommand(eBitResult.ON, "T4");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Message Display Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        string eqpNo = "L3";

        private void CPCMessageDisplayCommandReply(eBitResult result, string trxID)
        {
            try
            {
               

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

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMMessageSetCommandReplyBlock", "ReturnCode", 1);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMMessageSetCommandReplyBlock", "ReturnCode", 0);
                }
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMCommandReply", "CIMMessageSetCommandReply", result == eBitResult.ON ? "true" : "false");

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
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMMessageClearCommandReplyBlock", "ReturnCode", 1);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMMessageClearCommandReplyBlock", "ReturnCode", 0);
                }
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMCommandReply", "CIMMessageClearCommandReply", result == eBitResult.ON ? "true" : "false");
                
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

        #region  CIM Message Confirm Report 

        public void CPCCIMMessageConfirmReport(string mesgID, eBitResult result)
        {
            try
            {

                //CIMMessageID
                //TouchPanelNumber

                Block CIMMessageSetCommandBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_Status_03_01_00", "CIMMessageConfirmReportBlock");

                string timerID = string.Format("{0}_{1}", "L3", CIMMessageConfirmReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "CIMMessageConfirmReport", false);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] CIM Message Confirm Report.",
                            "L3", UtilityMethod.GetAgentTrackKey()));

                    return;
                }

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(BCCIMMessageConfirmReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                CIMMessageSetCommandBlock["CIMMessageID"].Value = mesgID;
                CIMMessageSetCommandBlock["TouchPanelNumber"].Value = "1";

                eipTagAccess.WriteBlockValues("SD_EQToCIM_Status_03_01_00", CIMMessageSetCommandBlock);

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "CIMMessageConfirmReport", true);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] CIM Message Confirm Report Message ID=[{2}].", eqpNo, UtilityMethod.GetAgentTrackKey(), mesgID));


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

        #region Date Time Calibration Command

      

        private void BCDateTimeCalibrationCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                E2BTimeoutCommand(eBitResult.ON, "T4");
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
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMCommandReply", "DateTimeSetCommandReply", result == eBitResult.ON ? "true" : "false");

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
                    CPCDateTimeCalibrationCommand(eqp, "", eBitResult.OFF, inputData.TrackKey);

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


        private void BCEquipmentRunModeSetCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                E2BTimeoutCommand(eBitResult.ON, "T4");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Equipment Run Mode Set Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCEquipmentRunModeSetCommandReply(eBitResult result, string trxID, string returnCode)
        {
            try
            {
                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeSetCommandReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCEquipmentRunModeSetCommandReplyTimeoutAction), trxID);

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineModeChangeCommandReplyBlock", "MachineModeChangeReturnCode", returnCode);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineModeChangeCommandReplyBlock", "MachineModeChangeReturnCode", 0);
                }
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMCommandReply", "MachineModeChangeCommandReply", result == eBitResult.ON ? "true" : "false");

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
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
                    CPCEquipmentRunModeSetCommand(eqp, inputData.TrackKey,"0", eBitResult.OFF);

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

        #region Set Last Glass Command 

        private void BCSetLastGlassCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Set Last Glass Command TIMEOUT.", sArray[0], trackKey));


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

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "SetLastJobCommandReplyBlock", "SetLastJobCommandReturnCode", returnCode);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "SetLastJobCommandReplyBlock", "SetLastJobCommandReturnCode", 0);
                }
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMCommandReply", "SetLastJobCommandReply", result == eBitResult.ON ? "true" : "false");

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
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

        private void EQSetLastGlassCommandReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Set Last Glass Command Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region LoadingStopChangeCommand

        private void CPCStopBitCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            Trx trx = GetTrxValues("L3_StopBitcommandReply");
            trx[0][0][0].Value = "0";
            trx.TrackKey = trackKey;
            SendToPLC(trx);
            LogWarn(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Stop Bit Reply T1 Timeout", sArray[0], trackKey));
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
                int flag = 0;
                foreach (Item it in inputData[0][0].Items.AllValues)
                {

                    if (it.Value == "1")
                    {
                        //if (eqp.File.PositionJobs.ContainsKey(i))
                        //{
                        //job = eqp.File.PositionJobs[i];

                        //更新编辑的资料
                        Trx jobData = GetTrxValues(string.Format("L3_EQDPositionGlassChangeReport#{0}", (i).ToString().PadLeft(2, '0')));
                        job = ObjectManager.JobManager.GetJobs().Where(j => j.GlassID == jobData[0][0][15].Value).FirstOrDefault();

                        //判断job是否为null
                        if (job == null)
                        {
                            //null create new job 上报create
                            job = new Job(int.Parse(jobData[0][0]["LotSequenceNumber"].Value), int.Parse(jobData[0][0]["SlotSequenceNumber"].Value));
                            UpdateJobData(eqp, job, jobData);
                            flag = 1;
                        }

                        //否则 更新 上报 modify
                        else
                        {
                            UpdateJobData(eqp, job, jobData);
                            eqp.File.PositionJobs[i] = job;
                            //上报编辑事件
                            flag = 2;
                        }
                        JobDataEditReport(eqp, job, flag);
                        ObjectManager.JobManager.AddJob(job);
                        ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                        //}
                        //else
                        //{
                        //    //Position 不存在玻璃但是有编辑资料
                        //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        //      string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Edit Glass Report,But Position[{2}] Job Not Exsit! ",
                        //          eqpNo, inputData.TrackKey, i));
                        //}
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
        private void JobDataEditReport(Equipment eq, Job job, int flag)
        {
            try
            {
                Block JobDataChangeReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "JobDataChangeReportBlock");

              
                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, JobDataEditReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }


                SetJobData(eq, job, JobDataChangeReportBlock);

                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", JobDataChangeReportBlock);


                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(JobDataEditReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataChangeReport", "true");

                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataChangeReport", "false");

                }
               

                //资料编辑记录
                ObjectManager.JobManager.SaveJobHistory(job, flag == 1 ? eJobEvent.Create.ToString() : eJobEvent.Modify.ToString(),
                    eq.Data.NODEID,
                    eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Data Edit Report =[{2}] Flag=[{3}].", eq.Data.NODENO, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "ON", flag));

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

                eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataChangeReport", "false");

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Job Data Edit Report T1 TIMEOUT.", sArray[0], trackKey));


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
                string timer1ID = "L3_JobDataEditReportTimeout";
                if (Timermanager.IsAliveTimer(timer1ID))
                {
                    Timermanager.TerminateTimer(timer1ID);
                }
                Trx trx = GetTrxValues(string.Format("{0}_JobDataEditReport", eqp.Data.NODENO));
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T2,
                        new System.Timers.ElapsedEventHandler(JobDataEditReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Job Data Edit Report Reply BIT [{2}].",
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
                E2BTimeoutCommand(eBitResult.ON, "T2");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] Job Data Edit Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Removed Job Report
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

                            RemovedJobReport(eqp, job, i);
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
        private void RemovedJobReport(Equipment eq, Job job, int position)
        {
            try
            {

                Block JobManualMoveReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "JobManualMoveReportBlock");
                Block JobManualMoveReportSubBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "JobManualMoveReportSubBlock");

                JobManualMoveReportBlock["JobID"].Value = job.JobID;
                JobManualMoveReportBlock["LotSequenceNumber"].Value = job.LotSequenceNumber;
                JobManualMoveReportBlock["SlotSequenceNumber"].Value = job.SlotSequenceNumber;

                JobManualMoveReportSubBlock["ReportOption"].Value = "2";
                JobManualMoveReportSubBlock["OperatorID"].Value = eq.File.OprationName;
                JobManualMoveReportSubBlock["UnitorPort"].Value = "1";
                JobManualMoveReportSubBlock["UnitNumberorPortNo"].Value = "0";
                JobManualMoveReportSubBlock["SlotNumber"].Value = "1";

                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", JobManualMoveReportBlock);
                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", JobManualMoveReportSubBlock);

                string timerID = "L3_GlassDataRemoveRecoveryReportTimeout";

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(RemovedJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobManualMoveReport", "true" );

                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobManualMoveReport", "false");

                }
               
                //标记此玻璃为Remove
                job.RemoveFlag = true;
                ObjectManager.JobManager.AddJob(job);

                //删除资料记录
                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Delete.ToString(),
                  eq.Data.NODEID,
                  eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Removed Job Report =[{2}].", eq.Data.NODENO, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ON"));

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

                eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobManualMoveReport", "false");

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Removed Job Report T1 TIMEOUT.", sArray[0], trackKey));


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

                Trx trx = GetTrxValues(string.Format("{0}_RemovedJobReport", eqp.Data.NODENO));
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
                            SendToPLC(jobDataReply);

                            //回复请求结果
                            Trx bitDataReply = GetTrxValues(string.Format("L3_EQDJobDataRequestReply"));
                            bitDataReply[0][1][1].Value = "1";
                            //bitDataReply[0][2][0].Value = "1";
                            bitDataReply.TrackKey = UtilityMethod.GetAgentTrackKey();
                            SendToPLC(bitDataReply);

                            RecoveredJobReport(eqp, job, i);

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
                            //bitDataReply[0][2][0].Value = "1";
                            bitDataReply.TrackKey = UtilityMethod.GetAgentTrackKey();
                            SendToPLC(bitDataReply);

                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] CassetteNo=[{2}] JobSequenceNo=[{3}] GlassID=[{4}] JobData Not Exist!"
                                , eqp.Data.NODENO, bitDataReply.TrackKey, readData[0][0][0].Value.Trim(), readData[0][0][1].Value.Trim(), readData[0][0][2].Value.Trim()));
                        }

                        //string timerID = "L3_EQDJobDataRequestReplyTimeout";
                        //if (Timermanager.IsAliveTimer(timerID))
                        //{
                        //    Timermanager.TerminateTimer(timerID);
                        //}
                        //Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(EQDJobDataRequestReplyAutoReset), UtilityMethod.GetAgentTrackKey());

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
        private void EQDJobDataRequestReplyAutoReset(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            Trx trx = GetTrxValues("L3_EQDJobDataRequestReply");
            trx[0][2][0].Value = "0";
            trx.TrackKey = trackKey;
            SendToPLC(trx);

            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Recovered Job Report Reply Timeout.", sArray[0], trackKey));
        }
        private void RecoveredJobReport(Equipment eq, Job job, int position)
        {
            try
            {
                Block JobManualMoveReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "JobManualMoveReportBlock");
                Block JobManualMoveReportSubBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "JobManualMoveReportSubBlock");

                JobManualMoveReportBlock["JobID"].Value = job.JobID;
                JobManualMoveReportBlock["LotSequenceNumber"].Value = job.LotSequenceNumber;
                JobManualMoveReportBlock["SlotSequenceNumber"].Value = job.SlotSequenceNumber;

                JobManualMoveReportSubBlock["ReportOption"].Value = "3";
                JobManualMoveReportSubBlock["OperatorID"].Value = eq.File.OprationName;
                JobManualMoveReportSubBlock["UnitorPort"].Value = "1";
                JobManualMoveReportSubBlock["UnitNumberorPortNo"].Value = "0";
                JobManualMoveReportSubBlock["SlotNumber"].Value = "1";

                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", JobManualMoveReportBlock);
                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", JobManualMoveReportSubBlock);

                string timerID = "L3_GlassDataRemoveRecoveryReportTimeout";

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

             
                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(RecoveredJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobManualMoveReport", "true");

                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobManualMoveReport", "false");

                }


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recovered Job Report =[{2}].", eq.Data.NODENO, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ON"));

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

                eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobManualMoveReport", "false");

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recovered Job Report T1 TIMEOUT.", sArray[0], trackKey));


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

                Trx trx = GetTrxValues(string.Format("{0}_RecoveredJobReport", eqp.Data.NODENO));
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

                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(EQJobDataRequestTimeoutAction), inputData.TrackKey);
                }

                #endregion


                string glassID = inputData[0][0]["GlassID"].Value.Trim();
                string cstNo = inputData[0][0]["CassetteSequenceNo"].Value;
                string slotNo = inputData[0][0]["JobSequenceNo"].Value;
                string useFlag = inputData[0][0]["UsedFlag"].Value;

                Job job;
                if (useFlag == "1")
                {
                    job = ObjectManager.JobManager.GetJob(cstNo, slotNo);
                }
                else
                {
                    job = ObjectManager.JobManager.GetJob(glassID);
                }
                if (job != null)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                      string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Job Data Request GLASS_ID=[{2}] CstNo=[{3}] SlotNo=[{4}], But Job Data Exsit.",
                                      eq.Data.NODENO, inputData.TrackKey, glassID, cstNo, slotNo));

                }
                else
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                       string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Job Data Request GLASS_ID=[{2}] CstNo=[{3}] SlotNo=[{4}].",
                                       eq.Data.NODENO, inputData.TrackKey, glassID, cstNo, slotNo));
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

        private void CPCJobDataRequestReply(eBitResult result, Block inputData)
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

               
                    for (int j = 0; j < inputData.Items.Count; j++)
                    {
                    if (j == 60)
                    { 
                    }
                        trx[0][0][j].Value = inputData[j].Value.ToString();
                    }



                if (inputData["JobID"].Value.ToString() == string.Empty)
                {
                    trx[0][1][0].Value = "2";
                }
                else
                {
                    trx[0][1][0].Value = "1";
                }

                trx[0][2][0].Value = ((int)result).ToString();
                trx.TrackKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                SendToPLC(trx);


                if (result == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCJobDataRequestReplyTimeoutAction), DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}]  SET BIT=[{2}].",
                        eqpNo, DateTime.Now.ToString("yyyyMMddHHmmssfff"), result.ToString()));
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

       
        private void CPCJobDataRequestTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataRequest", "false");

                //CPCJobDataRequestReply(eBitResult.ON, replyTrx);
                //E2BTimeoutCommand(eBitResult.ON, "T1");

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
                E2BTimeoutCommand(eBitResult.ON, "T2");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Job Data Request Reply T2 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Process Start JobReport
        private void ProcessStartJobReport(Equipment eq, Job job, eBitResult result, string pathNo)
        {
            try
            {
                //Trx trx = GetTrxValues(string.Format("{0}_ProcessStartJobReport#{1}", eq.Data.NODENO, pathNo));


                //if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_ProcessStartJobReport{2} IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO, pathNo));


                Block ProcessStartReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "ProcessStartReportBlock");

                Block ProcessStartReportSubBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "ProcessStartReportSubBlock");


                string timerID = string.Format("{0}_{1}_{2}", eq.Data.NODENO, pathNo, ProcessStartJobReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                   eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "ProcessStartReport", "false");

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Process Start Job Report#{2}.",
                            eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmssfff"), pathNo));

                    return;
                }
                ProcessStartReportBlock[0].Value = job.JobID;
                ProcessStartReportBlock[1].Value = job.LotSequenceNumber;
                ProcessStartReportBlock[2].Value = job.SlotSequenceNumber;

                ProcessStartReportSubBlock[0].Value = job.CurrentUnitNo.ToString();
                ProcessStartReportSubBlock[1].Value = "1";
                ProcessStartReportSubBlock[2].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(0, 4);
                ProcessStartReportSubBlock[3].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(4, 2);
                ProcessStartReportSubBlock[4].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(6, 2);
                ProcessStartReportSubBlock[5].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(8, 2);
                ProcessStartReportSubBlock[6].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(10, 2);
                ProcessStartReportSubBlock[7].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(12, 2);
               
                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", ProcessStartReportBlock);
                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00",  ProcessStartReportSubBlock);


                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessStartJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                  eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "ProcessStartReport", "true");

                    job.ProStratTime = DateTime.Now;

                    ObjectManager.JobManager.AddJob(job);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "ProcessStartReport", "false");
                }
           
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}]  Process Start Job Report#{2} =[{3}].", eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmssfff"), pathNo, "ON"));

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

                ProcessStartJobReport(eqp, null, eBitResult.OFF, sArray[1]);
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

                ProcessStartJobReport(eqp, null, eBitResult.OFF, pathNo);


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
        private void ProcessEndJobReport(Equipment eq, Job job, eBitResult result, string pathNo)
        {
            try
            {
                //Trx trx = GetTrxValues(string.Format("{0}_ProcessEndJobReport#{1}", eq.Data.NODENO, pathNo));


                //if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_ProcessEndJobReport#{2} IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO, pathNo));

                Block ProcessEndReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "ProcessEndReportBlock");

                Block ProcessEndReportSubBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "ProcessEndReportSubBlock");

                string timerID = string.Format("{0}_{1}_{2}", eq.Data.NODENO, pathNo, ProcessEndJobReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "ProcessEndReport", "false");
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Process End Job Report#{2} .",
                            eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmssfff"), pathNo));

                    return;
                }
                ProcessEndReportBlock[0].Value = job.JobID;
                ProcessEndReportBlock[1].Value = job.LotSequenceNumber;
                ProcessEndReportBlock[2].Value = job.SlotSequenceNumber;

                ProcessEndReportSubBlock[0].Value = job.CurrentUnitNo.ToString();
                ProcessEndReportSubBlock[1].Value = "1";
                ProcessEndReportSubBlock[2].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(0, 4);
                ProcessEndReportSubBlock[3].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(4, 2);
                ProcessEndReportSubBlock[4].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(6, 2);
                ProcessEndReportSubBlock[5].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(8, 2);
                ProcessEndReportSubBlock[6].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(10, 2);
                ProcessEndReportSubBlock[7].Value = DateTime.Now.ToString("yyyyMMddHHmmssfff").Substring(12, 2);

                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", ProcessEndReportBlock);
                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", ProcessEndReportSubBlock);

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessEndJobReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "ProcessEndReport", "true");

                    job.ProEndTime = DateTime.Now;

                    ObjectManager.JobManager.AddJob(job);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "ProcessEndReport", "false");
                }
             

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}]  Process End Job Report#{2} =[{3}].", eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmssfff"), pathNo, "ON"));

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

                ProcessEndJobReport(eqp, null, eBitResult.OFF, sArray[1]);
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

                ProcessEndJobReport(eqp, null, eBitResult.OFF, pathNo);


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
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Clear AlarmID[{2}]  AlarmText[{3}] ", eqpNo, inputData.TrackKey, his.ALARMID, his.ALARMTEXT));
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
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", eqpNo, inputData.TrackKey, his.ALARMID, his.ALARMTEXT));
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

             

                eipTagAccess.WriteItemValue("SD_EQToCIM_AlarmEvent_03_01_00", "AlarmEvent", $"AlarmReport#{sArray[1]}", "false");

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

        #region AutoRecipeChangeMode
        public void EQAutoRecipeChangeModeReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Not found Node=[{0}]", eqpNo));
                    return;
                }
                CPCAutoRecipeChangeModeReport(eqp, inputData);
                lock (eqp)
                {
                    eqp.File.AutoRecipeChangeMode = (eEnableDisable)int.Parse(inputData[0][0][0].Value);
                }

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPEMT={0}] [EQ -> EC][{1}] Auto Recipe Change Mode = [{2}]", eqpNo, inputData.TrackKey, inputData[0][0][0].Value));
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
                eq.File.CurrentRecipeID = recipeID;
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                CPCCurrentRecipeIDReport(eq, inputData, eBitResult.ON);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON] Current Recipe No Report  RECIPE_NO=[{2}] .",
                     eq.Data.NODENO, inputData.TrackKey, recipeNO));


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
                eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "CurrentRecipeNumberChangeReport", false);

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
                lock (eq)
                {
                    eq.File.OprationName = inputData[0][0]["OperatorID"].Value;
                }
                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
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


                string recipeID = inputData[0][0]["RecipeID"].Value.Trim();
                string flag = inputData[0][0]["ModifyFlag"].Value.Trim();
                string day01 = inputData[0][0][1].Value.Trim().PadLeft(2, '0');
                string day02 = inputData[0][0][2].Value.Trim().PadLeft(2, '0');
                string day03 = inputData[0][0][3].Value.Trim().PadLeft(2, '0');
                string day04 = inputData[0][0][4].Value.Trim().PadLeft(2, '0');
                string day05 = inputData[0][0][5].Value.Trim().PadLeft(2, '0');
                string day06 = inputData[0][0][6].Value.Trim().PadLeft(2, '0');
                string versioNo = day01 + day02 + day03 + day04 + day05 + day06;
                string recipeNo = inputData[0][0]["RecipeNO"].Value.Trim();
                string recipeStatus = inputData[0][0]["RecipeStatus"].Value.Trim();

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[ON]  Recipe ID Modify Report  RecipeNo=[{2}] RecipeID=[{3}] Modify Flag=[{4}].",
                     eq.Data.NODENO, inputData.TrackKey, recipeNo, recipeID, flag));


                if (flag == "1") //create
                {
                    RecipeEntityData recipe = new RecipeEntityData();
                    recipe.FABTYPE = "ARRAY";
                    recipe.LINETYPE = eq.Data.NODENAME;
                    recipe.LINEID = eq.Data.LINEID;
                    recipe.NODENO = eq.Data.NODENO;
                    recipe.RECIPENO = recipeNo;
                    recipe.RECIPEID = recipeID.Trim();
                    recipe.CREATETIME = DateTime.Now;
                    recipe.VERSIONNO = versioNo;
                    recipe.RECIPESTATUS = recipeStatus == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                    recipe.OPERATORID = eq.File.OprationName;
                    recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipeNo, recipeID, recipe.RECIPESTATUS.ToString(), versioNo);

                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat("{0}={1},", "Recipe_NO", recipeNo);
                    sb.AppendFormat("{0}={1},", "Recipe_ID", recipeID.Trim());
                    sb.AppendFormat("{0}={1},", "Recipe_State", recipe.RECIPESTATUS);
                    sb.AppendFormat("{0}={1},", "Recipe_Verson", recipe.VERSIONNO);

                    string paraValus = GetRecipeValus(eq, inputData[0][1], sb);




                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipe();

                    if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(recipeID.Trim()))
                    {
                        //移动原来的Recipe File到Log History文件夹下，并记录履历

                        ObjectManager.RecipeManager.MoveRecipeDataValuesToFile(recipeDic[eq.Data.LINEID][recipeID.Trim()].FILENAME);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipeDic[eq.Data.LINEID][recipeID.Trim()], eRecipeEvent.Delete, eq.File.OprationName);
                        ObjectManager.RecipeManager.DeleteRecipeObject(recipe.RECIPEID, eq.Data.NODENO);
                        ObjectManager.RecipeManager.SaveRecipeOject(recipe);
                    }
                    else
                    {
                        ObjectManager.RecipeManager.SaveRecipeOject(recipe);
                    }
                    ObjectManager.RecipeManager.MakeRecipeDataValuesToFile(recipe.FILENAME, paraValus);
                    ObjectManager.RecipeManager.SaveRecipeHistory(recipe, eRecipeEvent.Create, eq.File.OprationName);
                }
                else if (flag == "2")//modify
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipe();
                    RecipeEntityData recipePater = new RecipeEntityData();
                    if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(recipeID.Trim()))
                    {
                        recipePater = recipeDic[eq.Data.LINEID][recipeID.Trim()];

                        RecipeEntityData recipe = new RecipeEntityData();
                        recipe.FABTYPE = "ARRAY";
                        recipe.LINETYPE = eq.Data.NODENAME;
                        recipe.LINEID = eq.Data.LINEID;
                        recipe.NODENO = eq.Data.NODENO;
                        recipe.RECIPENO = recipeNo;
                        recipe.RECIPEID = recipeID.Trim();
                        recipe.CREATETIME = DateTime.Now;
                        recipe.VERSIONNO = versioNo;
                        recipe.RECIPESTATUS = recipeStatus == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                        recipe.OPERATORID = eq.File.OprationName;
                        recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipeNo, recipeID, recipe.RECIPESTATUS.ToString(), versioNo);

                        StringBuilder sb = new StringBuilder();

                        sb.AppendFormat("{0}={1},", "Recipe_NO", recipeNo);
                        sb.AppendFormat("{0}={1},", "Recipe_ID", recipeID.Trim());
                        sb.AppendFormat("{0}={1},", "Recipe_State", recipe.RECIPESTATUS);
                        sb.AppendFormat("{0}={1},", "Recipe_Verson", recipe.VERSIONNO);

                        string paraValus = GetRecipeValus(eq, inputData[0][1], sb);

                        // string paraValus = GetRecipeValus(eq, inputData[0][1]);

                        // ObjectManager.RecipeManager.UpateRecipeObject(recipePater);
                        //移动原来的Recipe File到Log History文件夹下，并记录履历

                        ObjectManager.RecipeManager.MoveRecipeDataValuesToFile(recipePater.FILENAME);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipePater, eRecipeEvent.Delete, eq.File.OprationName);
                        ObjectManager.RecipeManager.DeleteRecipeObject(recipePater.RECIPEID, eq.Data.NODENO);
                        ObjectManager.RecipeManager.SaveRecipeOject(recipe);
                        ObjectManager.RecipeManager.MakeRecipeDataValuesToFile(recipe.FILENAME, paraValus);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipe, eRecipeEvent.Create, eq.File.OprationName);
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
                        recipe.CREATETIME = DateTime.Now;
                        recipe.VERSIONNO = versioNo;
                        recipe.RECIPESTATUS = recipeStatus == "1" ? eEnableDisable.Enable.ToString() : eEnableDisable.Disable.ToString();
                        recipe.OPERATORID = eq.File.OprationName;
                        recipe.FILENAME = string.Format("{0}_{1}_{2}_{3}", recipeNo, recipeID, recipe.RECIPESTATUS.ToString(), versioNo);

                        StringBuilder sb = new StringBuilder();

                        sb.AppendFormat("{0}={1},", "Recipe_NO", recipeNo);
                        sb.AppendFormat("{0}={1},", "Recipe_ID", recipeID.Trim());
                        sb.AppendFormat("{0}={1},", "Recipe_State", recipe.RECIPESTATUS);
                        sb.AppendFormat("{0}={1},", "Recipe_Verson", recipe.VERSIONNO);

                        string paraValus = GetRecipeValus(eq, inputData[0][1], sb);

                        // string paraValus = GetRecipeValus(eq, inputData[0][1]);

                        ObjectManager.RecipeManager.UpateRecipeObject(recipe);
                        ObjectManager.RecipeManager.MakeRecipeDataValuesToFile(recipe.FILENAME, paraValus);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipe, eRecipeEvent.Create, eq.File.OprationName);

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON]  Recipe ID Modify Report  RECIPE_ID=[{2}] Not Have Modify Flag=[{3}] Error.",
                           eq.Data.NODENO, inputData.TrackKey, recipeID, flag));
                    }
                }
                else if (flag == "3")
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                       ObjectManager.RecipeManager.ReloadRecipe();

                    if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(recipeID.Trim()))
                    {
                        ObjectManager.RecipeManager.MoveRecipeDataValuesToFile(recipeDic[eq.Data.LINEID][recipeID.Trim()].FILENAME);
                        ObjectManager.RecipeManager.SaveRecipeHistory(recipeDic[eq.Data.LINEID][recipeID.Trim()], eRecipeEvent.Delete, eq.File.OprationName);
                        ObjectManager.RecipeManager.DeleteRecipeObject(recipeID.Trim(), eq.Data.NODENO);
                    }
                }
                else
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[ON]  Recipe ID Modify Report  RECIPE_ID=[{2}]  Modify Flag=[{3}] Error.",
                            eq.Data.NODENO, inputData.TrackKey, recipeID, flag));
                }
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
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Recipe ID Modify Report Reply.",
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

                    Timermanager.CreateTimer(timerID, false, T2,
                        new System.Timers.ElapsedEventHandler(BCRecipeIDModifyReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  Recipe ID Modify Report Reply BIT [{2}].",
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

                eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "RecipeChangeReport", false);

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
                E2BTimeoutCommand(eBitResult.ON, "T2");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EQ][{1}]   Recipe ID Modify Report Reply T2 TIMEOUT.", sArray[0], trackKey));


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

                    Block LC_EQToCIM_ParameterINT_01_03_00 = eipTagAccess.ReadBlockValues("LC_EQToCIM_ParameterINT_01_03_00", "ParameterINTFormatDataBlock#");


                    for (int i = 0; i <= ev.Items.AllKeys.Length - 1; i++)
                    {
                        string name = ev[i].Name.Trim();
                        string valus = ev[i].Value.Trim();

                        LC_EQToCIM_ParameterINT_01_03_00[(i+1)*2-2].Value = i + 1;
                        LC_EQToCIM_ParameterINT_01_03_00[(i + 1) * 2 - 1].Value = valus;
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

                    eipTagAccess.WriteBlockValues("LC_EQToCIM_ParameterINT_01_03_00", LC_EQToCIM_ParameterINT_01_03_00);

                

                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return sb.ToString();
            }
        }

        #endregion

        #region Recipe Register Validation Command

     
        private void BCRecipeRegisterValidationCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                E2BTimeoutCommand(eBitResult.ON, "T4");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Recipe Register Validation Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCRecipeRegisterValidationCommandReply(eBitResult result, string returnCode)
        {
            try
            {
                //Trx trx = null;
                //trx = GetTrxValues("L3_RecipeRegisterValidationCommandReply");

                string eqpNo = "L3";
                string timerID = string.Empty;
                timerID = string.Format("{0}_{1}", eqpNo, RecipeRegisterValidationCommandReplyTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeCommandReply", "RecipeRegisterCheckCommandReply", false);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Recipe Register Validation Command Reply.",
                            eqpNo, DateTime.Now.ToString("yyyyMMddHHmmssfff")));

                    return;
                }

               

                if (result == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeRegisterCheckCommandReplyBlock", "ReturnCode", returnCode);

                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeCommandReply", "RecipeRegisterCheckCommandReply", false);



                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeRegisterValidationCommandReplyTimeoutAction), DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo,DateTime.Now.ToString("yyyyMMddHHmmssfff"), result.ToString()));
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

                CPCRecipeRegisterValidationCommandReply(eBitResult.OFF,  "0");
            }
            catch (Exception ex)
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

                fetchChannel[int.Parse(pathNo)] = true;


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T2,
                        new System.Timers.ElapsedEventHandler(BCFetchGlassDataReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Fetch Glass Data Report Reply[{2}] BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, (eBitResult)int.Parse(inputData[0][0][0].Value)));
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


                eipTagAccess.WriteItemValue("SD_EQToCIM_FetchedEvent01_03_01_00", "FetchedJobEvent", "FetchedOutJobReport#1", false);

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

                stroeChannel[int.Parse(pathNo)] = true;

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T2,
                        new System.Timers.ElapsedEventHandler(BCStoreGlassDataReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Store Glass Data Report Reply[{2}] BIT [{3}].",
                            eqpNo, inputData.TrackKey, pathNo, bitResult.ToString()));
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

                eipTagAccess.WriteItemValue("SD_EQToCIM_StoredEvent01_03_01_00", "StoredJobEvent", "StoredJobReport#1", false);

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

        #region IonizerStatus

        public void IonizerStatus(Trx inputData)
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
                Trx glassNumberTrx1 = GetTrxValues("L3_EQDPositionGlass");
                Block PositionStatus1 = eipTagAccess.ReadBlockValues("SD_EQToCIM_Status_03_01_00", "IonizerStatusReportBlock");
                for (int i = 0; i <= 14; i++)
                {
                    PositionStatus1[i].Value = glassNumberTrx1[0][0][i].Value == "1" ? "true" : "false";

                }
                eipTagAccess.WriteBlockValues("SD_EQToCIM_Status_03_01_00", PositionStatus1);

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "IonizerStatusReport", "true");
                Thread.Sleep(1000);

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "IonizerStatusReport", "false");
               
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        #endregion

        #region OtherDownstreamPath

        //public void OtherDownstreamPath01(Trx inputData)
        //{
        //    try
        //    {
        //        string eqpNo = inputData.Metadata.NodeNo;

        //        Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

        //        if (eqp == null)
        //        {
        //            LogError(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("Not found Node =[{0}]", eqpNo));
        //            return;
        //        }

        //        int i = 0;
        //        foreach (Item it in inputData[0][0].Items.AllValues)
        //        {

        //            if (!eqp.File.DownInterface01.ContainsKey(i + 1))
        //            {
        //                eqp.File.DownInterface01.Add(i + 1, it.Value);
        //            }
        //            else
        //            {
        //                eqp.File.DownInterface01[i + 1] = it.Value;

        //            }
        //            i++;
        //        }

        //        ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }

        //}
        //public void OtherDownstreamPath02(Trx inputData)
        //{
        //    try
        //    {
        //        string eqpNo = inputData.Metadata.NodeNo;

        //        Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

        //        if (eqp == null)
        //        {
        //            LogError(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("Not found Node =[{0}]", eqpNo));
        //            return;
        //        }

        //        int i = 0;
        //        foreach (Item it in inputData[0][0].Items.AllValues)
        //        {

        //            if (!eqp.File.DownInterface02.ContainsKey(i + 1))
        //            {
        //                eqp.File.DownInterface02.Add(i + 1, it.Value);
        //            }
        //            else
        //            {
        //                eqp.File.DownInterface02[i + 1] = it.Value;

        //            }
        //            i++;
        //        }

        //        ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }

        //}

        #endregion

        #region Position Glass Change Report 更新Position信息，刷新Fecth\Store信息
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
                    job = ObjectManager.JobManager.GetJob(inputData[0][0]["LotSequenceNumber"].Value, inputData[0][0]["SlotSequenceNumber"].Value);

                    if (job == null)
                    {
                        job = new Job(int.Parse(inputData[0][0]["LotSequenceNumber"].Value), int.Parse(inputData[0][0]["SlotSequenceNumber"].Value));
                        UpdateJobData(eqp, job, inputData[0][0]);

                        //设备新增的玻璃信息
                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Create.ToString(),
                                    eqp.Data.NODEID,
                                    eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");
                    }
                    else
                    {
                        if (job.CassetteSequenceNo == int.Parse(inputData[0][0]["LotSequenceNumber"].Value) && job.JobSequenceNo == int.Parse(inputData[0][0]["SlotSequenceNumber"].Value))
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
                    

                    //添加position Info 
                    job.PrositonNo = position;
                    job.CurrentUnitNo = int.Parse(positionDic[int.Parse(position)].UnitNo);

                    ObjectManager.JobManager.AddJob(job);


                }
                else
                {
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
                        
                        // eqp.File.PositionJobs.Remove(int.Parse(position));
                    }
                }



                Trx glassNumberTrx = GetTrxValues("L3_EQDPositionGlass");
                Block PositionStatus = eipTagAccess.ReadBlockValues("SD_EQToCIM_PositionStatus_03_01_00", "PositionStatus");
                Block PositionGlassCodeBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_PositionStatus_03_01_00", "PositionGlassCodeBlock");
                int count =0;
                int j = 1;
                for (int i = 0; i <= 24; i++)
                {
                    PositionStatus[i].Value= glassNumberTrx[0][0][i].Value == "1" ? "true" : "false";
                    if (glassNumberTrx[0][0][i].Value == "1")
                    {
                        count++;
                        PositionGlassCodeBlock[j * 2 - 1].Value = eqp.File.PositionJobs[j].LotSequenceNumber;
                        PositionGlassCodeBlock[j * 2].Value = eqp.File.PositionJobs[j].SlotSequenceNumber;
                        j++;
                    }
                    else {
                        PositionGlassCodeBlock[j * 2 - 1].Value = 0;
                        PositionGlassCodeBlock[j * 2].Value = 0;
                        j++;
                    }

                }
                PositionGlassCodeBlock[0].Value = count;

                eipTagAccess.WriteBlockValues("SD_EQToCIM_PositionStatus_03_01_00", PositionStatus);

                eipTagAccess.WriteBlockValues("SD_EQToCIM_PositionStatus_03_01_00", PositionGlassCodeBlock);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);


              

            }
            catch (System.Exception ex)
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
                E2BTimeoutCommand(eBitResult.ON, "T4");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Recipe Parameter Request TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
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
                   
                   eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeCommandReply","RecipeParameterRequestCommandReply",false);
                   
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
                        foreach ( string key in keyValuePairs.Keys)
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

                eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeCommandReply", "RecipeParameterRequestCommandReply", false);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Parameter Request Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                //CPCRecipeParameterRequestReply(null, eBitResult.OFF, null, null);
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



                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //        string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                //lock (eqp)
                //{
                //    eqp.File.TotalTFTGlassCount = int.Parse(inputData[0][0][1].Value);
                //    if (eqp.File.TotalTFTGlassCount == 0)
                //    {
                //        eqp.File.GroupNo = "0";
                //        Trx glassNumberTrx = GetTrxValues("L3_EquipmentGroupNumberReportData");
                //        glassNumberTrx[0][0][0].Value = eqp.File.GroupNo;
                //        glassNumberTrx.TrackKey = UtilityMethod.GetAgentTrackKey();
                //        SendToPLC(glassNumberTrx);
                //    }
                //}

                //CPCEquipmentJobCountChangeReport(eqp, inputData);

                //ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                //ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

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

        # region History Flow Glass Count

        public void EQHistoryFlowGlassCount(Trx inputData)
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

                CPCHistoryFlowGlassCountReport(eqp, inputData);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] Equipment Job Flow Count=[{2}].",
                        eqpNo, inputData.TrackKey, inputData[0][0][0].Value));
            }

            catch (System.Exception ex)
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
                //if (eqp.File.LoadingStopMode != (eEnableDisable)int.Parse(inputData[0][0][8].Value))
                //{
                //    Trx trx = GetTrxValues(string.Format("{0}_LoadingStop", eqp.Data.NODENO));

                //    if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_LoadingStop IN PLCFmt.xml!", eqp.Data.NODENO, eqp.Data.NODENO));

                //    trx[0][0][0].Value = inputData[0][0][8].Value;
                //    trx.TrackKey = inputData.TrackKey;
                //    SendToPLC(trx);
                //}

                lock (eqp)
                {
                    eqp.File.GlassCheckMode = (eEnableDisable)int.Parse(inputData[0][0][0].Value);
                    eqp.File.RecipeCheckMode = (eEnableDisable)int.Parse(inputData[0][0][1].Value);
                    eqp.File.ProductTypeCheckMode = (eEnableDisable)int.Parse(inputData[0][0][2].Value);
                    eqp.File.CstSlotNoCheckMode = (eEnableDisable)int.Parse(inputData[0][0][3].Value);
                    eqp.File.ProductIDCheckMode = (eEnableDisable)int.Parse(inputData[0][0][4].Value);
                    eqp.File.JobDuplicateCheckMode = (eEnableDisable)int.Parse(inputData[0][0][5].Value);
                    eqp.File.UpStreamPerMode = (eEnableDisable)int.Parse(inputData[0][0][6].Value);
                    eqp.File.DowmStreamPerMode = (eEnableDisable)int.Parse(inputData[0][0][7].Value);
                    eqp.File.LoadingStopMode = (eEnableDisable)int.Parse(inputData[0][0][8].Value);
                    eqp.File.GroupNoCheckMode = (eEnableDisable)int.Parse(inputData[0][0][9].Value);
                    eqp.File.GlassIDCheckMode = (eEnableDisable)int.Parse(inputData[0][0][10].Value);
                }

               // CPCJobDataCheckModeReport(eqp, inputData);

                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}]  Glass Check Mode=[{2}] Recipe Check Mode=[{3}] Product Type Check Mode=[{4}] Cst Slot No Check Mode=[{5}] Product ID Check Mode=[{6}] Job Duplicate Check Mode=[{7}] UpStream Per Mode=[{8}] DowmStream Per Mode=[{9}] Group No Check Mode=[{10}].",
                        eqpNo, inputData.TrackKey, eqp.File.GlassCheckMode, eqp.File.RecipeCheckMode, eqp.File.ProductTypeCheckMode, eqp.File.CstSlotNoCheckMode, eqp.File.ProductIDCheckMode, eqp.File.JobDuplicateCheckMode, eqp.File.UpStreamPerMode, eqp.File.DowmStreamPerMode, eqp.File.GroupNoCheckMode));
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

                //CPCSoftwareVersionChangeReport(eqp, inputData, eBitResult.ON);

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

                    //CPCSoftwareVersionChangeReport(eqp, inputData, eBitResult.OFF);

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

                if ((pauseCommand == "1" || pauseCommand == "2") && eq.File.EquipmentRunMode != pauseCommand)
                {
                    //   CPCEquipmentRunModeSetCommand(eq, inputData, eBitResult.ON);

                    CPCRunModePossibleRequestReply(eBitResult.ON, inputData, "1");
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
                trx = GetTrxValues(string.Format("L3_RunModePossibleRequestReply"));

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
                        new System.Timers.ElapsedEventHandler(CPCRunModePossibleRequestReplyTimeoutAction), trxID);
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

        #region EQDDownstreamActionMonitor

        public void EQDDownstreamActionMonitor(Trx inputData)
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

                        eqp.File.DownSendComplete = eBitResult.OFF;

                        SendToPLC(EQDDownContrx);
                    }
                    if (i == 3 && it.Value == "0")
                    {
                        Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#02");
                        EQDDownContrx[0][0][0].Value = "0";

                        eqp.File.DownSendCompleteUP = eBitResult.OFF;

                        SendToPLC(EQDDownContrx);
                    }
                    if (eqp.Data.LINEID == "KWF22003L" || eqp.Data.LINEID == "KWF22004L")
                    {
                        if (i == 8 && it.Value == "1")
                        {
                            Trx actionTRX = GetTrxValues("L3_DownEQDRollRunCommand");
                            actionTRX[0][0][0].Value = "0";
                            SendToPLC(actionTRX);
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

        #endregion

        #region EQDUpstreamActionMonitor

        public void EQDUpstreamActionMonitor(Trx inputData)
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

                    //if (i == 3 && it.Value == "1")//Recipe change完成 OFF COMMAND
                    //{
                    //    Trx EQDUPContrx = GetTrxValues("L3_EQDUpstreamPathControl#01");
                    //    EQDUPContrx[0][0][1].Value = "0";
                    //    SendToPLC(EQDUPContrx);
                    //}

                    if (i == 5 && it.Value == "1")//滚轮转动命令OFF
                    {
                        Trx EQDUPContrx = GetTrxValues("L3_EQDRollRunCommand");
                        EQDUPContrx[0][0][0].Value = "0";
                        SendToPLC(EQDUPContrx);
                    }

                    if (i == 5 && it.Value == "1")
                    {

                        Block LoadingStopRequestBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_Status_03_01_00", "LoadingStopRequestBlock");

                        LoadingStopRequestBlock[0].Value = "1";
                        LoadingStopRequestBlock[1].Value = "1";

                        eipTagAccess.WriteBlockValues("SD_EQToCIM_Status_03_01_00",  LoadingStopRequestBlock);

                        eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "LoadingStopRequest",true);
                    }
                    if (i == 5 && it.Value == "0")
                    {

                        Block LoadingStopRequestBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_Status_03_01_00", "LoadingStopRequestBlock");

                        LoadingStopRequestBlock[0].Value = "2";
                        LoadingStopRequestBlock[1].Value = "0";

                        eipTagAccess.WriteBlockValues("SD_EQToCIM_Status_03_01_00", LoadingStopRequestBlock);

                        eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "LoadingStopRequest", true);
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

        #region Recipe Parameter Report
        private void RecipeParameterReport(Equipment eq, Trx inputData, RecipeEntityData recipe)
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
                trx[0][0][0].Value = inputData[0][0][0].Value;
                trx[0][0][1].Value = recipe.VERSIONNO.Substring(2, 12);
                trx[0][0][2].Value = eq.File.OprationName;

                //Recipe Parameter 为临时参数，实际参数待定

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
                            if (splitc[0].Trim() != "Recipe_Verson")
                            {
                                if (!keyValuePairs.ContainsKey(splitc[0].Trim()))
                                {
                                    keyValuePairs.Add(splitc[0].Trim(), splitc[1].Trim());
                                }
                                if (splitc[0].Trim() == "Recipe_State")
                                {
                                    trx[0][1][splitc[0].Trim()].Value = splitc[1].Trim() == "Enable" ? "1" : "2";
                                }
                                else
                                {
                                    if (trx[0][1].Items.AllKeys.Contains(splitc[0].Trim()))
                                    {
                                        trx[0][1][splitc[0].Trim()].Value = splitc[1].Trim();
                                    }
                                }
                            }
                            //else if (eq.Data.NODEID == "CLEANER100")
                            //{
                            //    trx[0][1][3].Value = recipe.FILENAME.Split('_')[3].Substring(0, 4);
                            //    trx[0][1][4].Value = recipe.FILENAME.Split('_')[3].Substring(4, 2);
                            //    trx[0][1][5].Value = recipe.FILENAME.Split('_')[3].Substring(6, 2);
                            //    trx[0][1][6].Value = recipe.FILENAME.Split('_')[3].Substring(8, 2);
                            //    trx[0][1][7].Value = recipe.FILENAME.Split('_')[3].Substring(10, 2);
                            //    trx[0][1][8].Value = recipe.FILENAME.Split('_')[3].Substring(12, 2);

                            //}
                        }
                    }


                }








                trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    trx[0][2][0].Value = "1";
                    trx[0][2].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();

                }
                else
                {
                    trx[0][2][0].Value = "0";
                }

                SendToPLC(trx);


                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCRecipeParameterReportTimeoutAction), trx.TrackKey);

                Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Recipe Parameter Report Recipe No[{2}] Recipe ID[{3}] Recipe Type[{4}]", "L3", trx.TrackKey, trx[0][0][2].Value, trx[0][0][3].Value, trx[0][0][1].Value));

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
                trx[0][2][0].Value = "0";
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
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
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
                    trx[0][2][0].Value = "0";
                    SendToPLC(trx);


                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCRecipeParameterReportReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Recipe Parameter Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
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

        #region Unit Recipe Request Command

        public void BCUnitRecipeRequestCommand(Trx inputData)
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
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Unit Recipe Request Command.",
                        eqpNo, inputData.TrackKey));

                    CPCUnitRecipeRequestCommandReply(eBitResult.OFF, inputData, "0");

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
                        new System.Timers.ElapsedEventHandler(BCUnitRecipeRequestCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion


                string messageSequenceNo = inputData[0][0]["MessageSequenceNo"].Value.Trim();

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Unit Recipe Request Command MessageSequenceNo=[{2}] .",
                     eq.Data.NODENO, inputData.TrackKey, messageSequenceNo));

                CPCUnitRecipeRequestCommandReply(eBitResult.ON, inputData, "1");
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }
        private void BCUnitRecipeRequestCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Unit Recipe Request Command TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCUnitRecipeRequestCommandReply(eBitResult result, Trx inputData, string returnCode)
        {
            try
            {
                Trx trx = null;
                trx = GetTrxValues("L3_UnitRecipeRequestCommandReply");

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
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Unit Recipe Request Command Reply.",
                            eqpNo, trx.TrackKey));

                    return;
                }

                trx[0][0]["RecipeType"].Value = "1";
                trx[0][0]["MessageSequenceNo"].Value = inputData[0][0][0].Value.Trim(); ;
                trx[0][0]["ReturnCode"].Value = returnCode;
                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);


                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCUnitRecipeRequestCommandReplyTimeoutAction), inputData.TrackKey);
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, inputData.TrackKey, result.ToString()));

                RecipeReport = true;
                MessageSequenceNo = trx[0][0]["MessageSequenceNo"].Value;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        private void CPCUnitRecipeRequestCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Unit Recipe Request Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCUnitRecipeRequestCommandReply(eBitResult.OFF, null, "0");
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
                eBitResult bitResult = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);
                string timerID = string.Format("{0}_{1}", eqpNo, UnitRecipeListReportReplyTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Unit Recipe List Report Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                Trx trx = GetTrxValues(string.Format("{0}_UnitRecipeListReport", eqp.Data.NODENO));
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);

                UnitListRecipeReport = true;

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(UnitRecipeListReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Unit Recipe List Report Reply[ BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
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
                Trx trx = GetTrxValues("L3_UnitRecipeListReport");
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);

                UnitListRecipeReport = true;

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Unit Recipe List Report BC Reply T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void UnitRecipeListReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Unit Recipe List Report Reply TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Unit Recipe Request//请求时间节点待添加

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

                    CPCRecipeVersionRequestReply(eBitResult.OFF, inputData, "0", null);

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


                string recipeType = inputData[0][0]["RecipeType"].Value.Trim();
                string recipeNo = inputData[0][0]["RecipeNo"].Value.Trim();
                string recipeID = inputData[0][0]["RecipeID"].Value.Trim();
                string messageSequenceNo = inputData[0][0]["MessageSequenceNo"].Value.Trim();



                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Version Request RecipeType=[{2}] RecipeNo=[{3}] RecipeID=[{4}] MessageSequenceNo=[{5}] .",
                     eq.Data.NODENO, inputData.TrackKey, recipeType, recipeNo, recipeID, messageSequenceNo));

                if (recipeType == "2" && !string.IsNullOrEmpty(recipeID))
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                  ObjectManager.RecipeManager.ReloadRecipe();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeID))
                    {

                        CPCRecipeVersionRequestReply(eBitResult.ON, inputData, "1", recipeDic[eq.Data.LINEID][recipeID]);


                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Parameter Validation Command Reply OK .",
                                eq.Data.NODENO, inputData.TrackKey));

                    }
                    else
                    {
                        CPCRecipeVersionRequestReply(eBitResult.ON, inputData, "2", null);

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply NG Recipe ID=[{2}] Not exist.",
                                eq.Data.NODENO, inputData.TrackKey, recipeID));
                    }

                }
                else if (recipeType == "1" && !string.IsNullOrEmpty(recipeNo))
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                ObjectManager.RecipeManager.ReloadRecipeByNo();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeNo))
                    {

                        CPCRecipeVersionRequestReply(eBitResult.ON, inputData, "1", recipeDic[eq.Data.LINEID][recipeNo]);


                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply OK .",
                                eq.Data.NODENO, inputData.TrackKey));

                    }
                    else
                    {
                        CPCRecipeVersionRequestReply(eBitResult.ON, inputData, "2", null);

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply NG Recipe NO=[{2}] Not exist.",
                                eq.Data.NODENO, inputData.TrackKey, recipeNo));
                    }

                }
                else
                {

                    CPCRecipeVersionRequestReply(eBitResult.ON, inputData, "2", null);

                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Version Request Reply NG Registration Conditions is Error.",
                            eq.Data.NODENO, inputData.TrackKey));
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
                    string.Format("[EQUIPMENT={0}] [BC - EC][{1}] Recipe Version Request TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void CPCRecipeVersionRequestReply(eBitResult result, Trx inputData, string returnCode, RecipeEntityData recipe)
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
                    // trx.ClearTrxWith0();
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
                if (returnCode == "1" && recipe != null)
                {
                    trx[0][0][0].Value = recipe.VERSIONNO.Substring(0, 4);
                    trx[0][0][1].Value = recipe.VERSIONNO.Substring(4, 2);
                    trx[0][0][2].Value = recipe.VERSIONNO.Substring(6, 2);
                    trx[0][0][3].Value = recipe.VERSIONNO.Substring(8, 2);
                    trx[0][0][4].Value = recipe.VERSIONNO.Substring(10, 2);
                    trx[0][0][5].Value = recipe.VERSIONNO.Substring(12, 2);

                }
                else
                {
                    trx[0][0][0].Value = "0";
                    trx[0][0][1].Value = "0";
                    trx[0][0][2].Value = "0";
                    trx[0][0][3].Value = "0";
                    trx[0][0][4].Value = "0";
                    trx[0][0][5].Value = "0";

                }
                trx[0][0][6].Value = inputData[0][0]["RecipeType"].Value.Trim();
                trx[0][0][7].Value = inputData[0][0]["RecipeNo"].Value.Trim();
                trx[0][0][8].Value = inputData[0][0]["RecipeID"].Value.Trim();
                trx[0][0][9].Value = inputData[0][0]["MessageSequenceNo"].Value.Trim();
                trx[0][0][10].Value = "0";
                trx[0][0]["ReturnCode"].Value = returnCode;


                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);


                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeVersionRequestReplyTimeoutAction), inputData.TrackKey);
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

                CPCRecipeVersionRequestReply(eBitResult.OFF, null, "0", null);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Glass Data Remove Recovery Report Reply
        public void BCGlassDataRemoveRecoveryReportReply(Trx inputData)
        {
            string eqpNo = inputData.Metadata.NodeNo;

            Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
            if (eqp == null)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("Not found Node=[{0}]", eqpNo));
                return;
            }

            string timer1ID = "L3_GlassDataRemoveRecoveryReportTimeout";
            if (Timermanager.IsAliveTimer(timer1ID))
            {
                Timermanager.TerminateTimer(timer1ID);
            }
            Trx trx = GetTrxValues(string.Format("{0}_GlassDataRemoveRecoveryReport", eqp.Data.NODENO));
            // trx.ClearTrxWith0();
            trx[0][2][0].Value = "0";
            SendToPLC(trx);

            eBitResult bitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
            string timerID = "L3_GlassDataRemoveRecoveryReportReplyTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            if (bitResult == eBitResult.OFF)
            {
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT=[{0}]] [EC <- BC][{1}] Glass Data Remove Recovery Report Reply BIT [{2}]", eqpNo, inputData.TrackKey, "OFF"));
                return;
            }

            Timermanager.CreateTimer(timerID, false, T2,
                new System.Timers.ElapsedEventHandler(BCGlassDataRemoveRecoveryReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT=[{0}]] [EC <- BC][{1}] Glass Data Remove Recovery Report Reply BIT [{2}]", eqpNo, inputData.TrackKey, "ON"));

        }

        private void BCGlassDataRemoveRecoveryReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                E2BTimeoutCommand(eBitResult.ON, "T2");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT=[{0}]] [EC <- BC][{1}] Glass Data Remove Recovery Report Reply T2 Timeout", sArray[0], trackKey));
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

        #region Unit Glass Have Report
        public void UnitGlassHaveReport(Trx inputData)
        {
            //try
            //{
            //    string eqpNo = inputData.Metadata.NodeNo;

            //    Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

            //    if (eqp == null)
            //    {
            //        LogError(MethodBase.GetCurrentMethod().Name + "()",
            //            string.Format("Not found Node =[{0}]", eqpNo));
            //        return;
            //    }
            //    int i = 1;
            //    // Job job = null;
            //    //Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_JobEachPositionBlockReport", eqp.Data.NODENO)) as Trx;
            //    Trx trx = GetTrxValues("L3_EQDPositionGlass");

            //    if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_GlassEachPositionBlock IN PLCFmt.xml!", eqp.Data.NODENO, eqp.Data.NODENO));

            //    foreach (Item it in inputData[0][0].Items.AllValues)
            //    {

            //        if (it.Value == "1")
            //        {
            //            Thread.Sleep(200);
            //            if (eqp.File.PositionJobs.ContainsKey(i) && i * 2 - 1 < 80)
            //            {
            //                //更新Job Position Info


            //                trx[0][0][i * 2 - 2].Value = eqp.File.PositionJobs[i].CassetteSequenceNo.ToString();
            //                trx[0][0][i * 2 - 1].Value = eqp.File.PositionJobs[i].JobSequenceNo.ToString();


            //            }
            //        }
            //        else
            //        {
            //            trx[0][0][i * 2 - 2].Value = "0";//eqp.File.PositionJobs[i].CassetteSequenceNo.ToString();
            //            trx[0][0][i * 2 - 1].Value = "0";//eqp.File.PositionJobs[i].JobSequenceNo.ToString();
            //        }

            //        i++;
            //    }

            //    //写入PLC
            //    trx.TrackKey = inputData.TrackKey;
            //    SendToPLC(trx);

            //}
            //catch (System.Exception ex)
            //{
            //    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            //}
        }
        #endregion

        #region Unit Scrap Delete Glass Report   破片要让上游补片
        public void UnitScrapDeleteGlassReport(Trx inputData)
        {
            //try
            //{
            //    string eqpNo = inputData.Metadata.NodeNo;

            //    Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

            //    if (eqp == null)
            //    {
            //        LogError(MethodBase.GetCurrentMethod().Name + "()",
            //            string.Format("Not found Node =[{0}]", eqpNo));
            //        return;
            //    }
            //    int i = 0;
            //    Job job = null;

            //    foreach (Item it in inputData[0][0].Items.AllValues)
            //    {

            //        if (it.Value == "1")
            //        {
            //            if (eqp.File.PositionJobs.ContainsKey(i + 1))
            //            {
            //                job = eqp.File.PositionJobs[i + 1];

            //                if (!string.IsNullOrEmpty(job.LOTID.Trim()))
            //                {
            //                    //读取joblotCount 
            //                    Trx trx = GetTrxValues(string.Format("{0}_JobLotCountReport", eqp.Data.NODENO));

            //                    if (eqp.File.PortLot01 == eqp.File.PortLot02)
            //                    {
            //                        trx[0][0][1].Value = (int.Parse(trx[0][0][1].Value) + 1).ToString();

            //                        trx.TrackKey = inputData.TrackKey;

            //                        SendToPLC(trx);
            //                    }
            //                    else
            //                    {
            //                        if (job.LOTID.Trim() == eqp.File.PortLot01)
            //                        {
            //                            trx[0][0][1].Value = (int.Parse(trx[0][0][1].Value) + 1).ToString();

            //                            trx.TrackKey = inputData.TrackKey;

            //                            SendToPLC(trx);
            //                        }
            //                        else if (job.LOTID.Trim() == eqp.File.PortLot02)
            //                        {
            //                            trx[0][0][3].Value = (int.Parse(trx[0][0][3].Value) + 1).ToString();

            //                            trx.TrackKey = inputData.TrackKey;

            //                            SendToPLC(trx);
            //                        }
            //                        else
            //                        {

            //                            LogError(MethodBase.GetCurrentMethod().Name + "()",
            //                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Scrap Delete Glass Report,But Job LotID[{2}]  Not Exsit! Port#01[{3}]  Port#02[{4}]",
            //                            eqpNo, inputData.TrackKey, job.LOTID, eqp.File.PortLot01, eqp.File.PortLot02));
            //                        }
            //                    }

            //                }
            //                //破片除账资料记录
            //                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Delete.ToString(),
            //                  eqp.Data.NODEID,
            //                  eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

            //                //移除破片Job
            //                eqp.File.PositionJobs.TryRemove(i + 1, out job);

            //                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            //            }
            //            else
            //            {
            //                //Position 不存在玻璃但是有编辑资料
            //                LogError(MethodBase.GetCurrentMethod().Name + "()",
            //                  string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Scrap Delete Glass Report,But Position[{2}] Job Not Exsit! ",
            //                      eqpNo, inputData.TrackKey, i));
            //            }
            //        }
            //        else
            //        {


            //        }

            //        i++;
            //    }


            //    ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            //}
            //catch (System.Exception ex)
            //{
            //    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            //}
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
                    // trx.ClearTrxWith0();
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

                Block EQToCIM_ProcessDataSVINT = eipTagAccess.ReadBlockValues("EQToCIM_ProcessDataSVINT", "SVINTFormatDataBlock#");


                for (int j = 0; j < inputData[0][0].Items.AllValues.Length; j++)
                {
                    EQToCIM_ProcessDataSVINT[(j + 1) * 2 - 2].Value = j + 1;
                    EQToCIM_ProcessDataSVINT[(j + 1) * 2 - 1].Value = inputData[0][0][j].Value.Trim();
                }

             eipTagAccess.WriteBlockValues("EQToCIM_ProcessDataSVINT", EQToCIM_ProcessDataSVINT);

            }

            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        public void EquipmentSVReport()
        {

            while (true)
            {
                try
                {
                    Thread.Sleep(10000);
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                    if (eq == null)
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", "L3"));
                        return;
                    }

                    Trx inputData = GetTrxValues("L3_EQDEquipmentSVReport");



                   // Block ParameterINTFormatDataBlock = eipTagAccess.ReadBlockValues("LC_EQToCIM_ProcessDataSVINT_01_03_00", "ParameterINTFormatDataBlock#");

                    Block SVFLOATFormatDataBlock = eipTagAccess.ReadBlockValues("LC_EQToCIM_ProcessDataSVFLOAT_01_03_00", "SVFLOATFormatDataBlock#");
                 
                    for (int j = 0; j < inputData[0][0].Items.AllValues.Length; j++)
                    {
                        //if (inputData[0][0][j].Name.Split('/')[1] == "1")
                        //{
                        //    ParameterINTFormatDataBlock[(i + 1) * 2 - 2].Value = i + 1;
                        //    ParameterINTFormatDataBlock[(i + 1) * 2 - 1].Value = inputData[0][0][j].Value.Trim();
                        //    i++;
                        //}
                        //else
                        //{
                        float rate = int.Parse(inputData[0][0][j].Name.Split('/')[1].Trim());
                        SVFLOATFormatDataBlock[(j + 1) * 2 - 2].Value = j + 1;
                        SVFLOATFormatDataBlock[(j + 1) * 2 - 1].Value = (float)Math.Round(int.Parse(inputData[0][0][j].Value.Trim()) / rate,3);
                        //    f++;
                        //}

                    }

                   // eipTagAccess.WriteBlockValues("LC_EQToCIM_ProcessDataDVINT_01_03_00", ParameterINTFormatDataBlock);
                    eipTagAccess.WriteBlockValues("LC_EQToCIM_ProcessDataSVFLOAT_01_03_00", SVFLOATFormatDataBlock);


                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

                    continue;


                }
            }
        }

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
                //if (!newFile.Exists)
                //{

                //    newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 8)}.xlsx");
                //}
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
                            worksheet.Cells[2, i + 2].Value = (recieveData * 1.0 / rate).ToString("F3");//写入实际数据做记录，保留两位小数

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
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = (recieveData * 1.0 / rate).ToString("F3");

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
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                    _SVSavePath = "D:\\KZONELOG\\" + eq.Data.LINEID + "\\SVData\\";
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
                    DateTime checkTime = Directory.GetCreationTime(FilePath).AddDays(180);//设定保存时长，暂定180天
                    if (checkTime < DateTime.Now)//检查当前时间，若超过180天，则删除
                    {
                        Directory.Delete(FilePath, true);//删除该目录及目录下的子文件
                    }
                }
            }
        }

        #endregion

        #region  CV Report
        public void EQDEquipmentCVReport(Trx inputData)
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
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_TactDataReport IN PLCFmt.xml!", inputData.Metadata.NodeNo));

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


        public void EquipmentCVReport()
        {

            while (false)
            {
                try
                {
                    Thread.Sleep(10000);
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                    if (eq == null)
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", "L3"));
                        return;
                    }

                    Trx inputData = GetTrxValues("L3_EQDEquipmentCVReport");

                    Trx trx = GetTrxValues("L3_CVDataReport");
                    if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX CVDataReport IN PLCFmt.xml!", inputData.Metadata.NodeNo));

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < inputData[0][0].Items.Count; i++)
                    {

                        trx[0][1][i].Value = inputData[0][0][i].Value.Trim();

                        sb.Append($"{inputData[0][0][i].Name.Split('/')[0]}={(double.Parse(inputData[0][0][i].Value) / int.Parse(inputData[0][0][i].Name.Split('/')[1])).ToString("F2")},");


                    }
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic = ObjectManager.RecipeManager.ReloadRecipe();
                    if (recipeDic.Count != 0)
                    {


                        if (recipeDic[eq.Data.LINEID].ContainsKey(eq.File.CurrentRecipeID))
                        {
                            RecipeEntityData recipeData = recipeDic[eq.Data.LINEID][eq.File.CurrentRecipeID];
                            trx[0][0][0].Value = eq.File.CurrentRecipeID;
                            trx[0][0][1].Value = recipeData.VERSIONNO.Substring(2, 12);

                            //ObjectManager.ProcessDataManager.MakeSVDataValuesToEXCEL(inputData.TrackKey, sb.ToString());

                        }
                        else
                        {
                            trx[0][0][0].Value = eq.File.CurrentRecipeID;
                            trx[0][0][1].Value = "0";
                            //LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Can't Find Version Of Current RecipeID=[{2}]", eq.Data.NODENO, trx.TrackKey, eq.File.CurrentRecipeID));
                        }
                    }
                    else
                    {
                        trx[0][0][0].Value = eq.File.CurrentRecipeID;
                        trx[0][0][1].Value = "0";
                    }

                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    MakeCVDataValuesToEXCEL(trx);

                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

                    continue;


                }
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
                //if (!newFile.Exists)
                //{

                //    newFile = new FileInfo($"{directoryPath}\\{timeKey.Substring(0, 8)}.xlsx");
                //}
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

                            string CVDataName = trx[0][1][i].Name;//item名
                            string[] saveData = CVDataName.Split('/');//根据'/'分割数据名和数据倍率
                            worksheet.Cells[1, i + 2].Value = saveData[0].Trim();//写入数据名
                            int recieveData = int.Parse(trx[0][1][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[2, i + 2].Value = (recieveData * 1.0 / rate).ToString("F3");//写入实际数据做记录，保留两位小数

                            worksheet.Column(i + 2).AutoFit();//自动列宽
                        }
                    }
                    else
                    {
                        worksheet.Cells[worksheet.Dimension.End.Row + 1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        for (int i = 0; i < trx[0][1].Items.Count; i++)
                        {

                            string CVDataName = trx[0][1][i].Name;
                            string[] saveData = CVDataName.Split('/');
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = saveData[0].Trim();
                            int recieveData = int.Parse(trx[0][1][i].Value);
                            int rate = int.Parse(saveData[1].Trim());
                            worksheet.Cells[worksheet.Dimension.Rows, i + 2].Value = (recieveData * 1.0 / rate).ToString("F3");

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
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                    _CVSavePath = "D:\\KZONELOG\\" + eq.Data.LINEID + "\\CVData\\";
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
                string[] CVDataDicFiles = Directory.GetDirectories(_CVSavePath);
                foreach (string FilePath in CVDataDicFiles)
                {
                    DateTime checkTime = Directory.GetCreationTime(FilePath).AddDays(180);//设定保存时长，暂定180天
                    if (checkTime < DateTime.Now)//检查当前时间，若超过180天，则删除
                    {
                        Directory.Delete(FilePath, true);//删除该目录及目录下的子文件
                    }
                }
            }
        }

        #endregion

        #region Force Clean Out Mode Report
        private void CPCForceCleanOutModeReport(eEnableDisable res)
        {
            Trx trx = GetTrxValues("L3_ForceClearOutModeReport");
            if (trx == null)
            {
                throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_ForceClearOutModeReport IN PLCFmt.xml!", "L3"));
            }
            trx[0][0][0].Value = res == eEnableDisable.Enable ? "1" : "2";
            trx[0][1][0].Value = "1";
            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(trx);

            string timerID = "L3_ForceClearOutModeReportTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            Timermanager.CreateTimer(timerID, false, T1,
                new System.Timers.ElapsedEventHandler(CPCForceCleanOutModeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());
        }

        public void CPCForceCleanOutModeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            Trx trx = GetTrxValues("L3_ForceClearOutModeReport");
            trx[0][1][0].Value = "0";
            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(trx);
            E2BTimeoutCommand(eBitResult.ON, "T1");
            LogWarn(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Force Clean Out Mode Report T1 Timeout", "L3", trx.TrackKey));
        }

        public void BCForeClearOutReportReply(Trx inputData)
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

                eBitResult bitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);

                #region 建立timer
                string t2TimerID = "L3_BCForeClearOutReportReplyTimeout";

                if (Timermanager.IsAliveTimer(t2TimerID))
                {
                    Timermanager.TerminateTimer(t2TimerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    string timerID = "L3_ForceClearOutModeReportTimeout";
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    Trx trx = GetTrxValues("L3_ForceClearOutModeReport");
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);

                    Timermanager.CreateTimer(t2TimerID, false, T2,
                        new System.Timers.ElapsedEventHandler(BCForeClearOutReportReplyTimeoutAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BC Force Clear Out Report Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void BCForeClearOutReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            E2BTimeoutCommand(eBitResult.ON, "T2");
            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                string.Format("[EQUIPMNET={0}] [BC -> EC][{1}] BC Force Clear Out Mode Report T2 Timeout", sArray[0], trackKey));
        }

        #endregion

        #region Force Clear Out Mode Command
        public void BCForceCleanOutCommand(Trx inputData)
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
                string timerID = "L3_BCForceCleanOutCommandTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (bitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                         string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Force Clean Out Command.",
                         eqpNo, inputData.TrackKey));

                    CPCForceCleanOutCommandReply(eBitResult.OFF, inputData.TrackKey);


                    //eq.File.ForceClearOutMode = eForceClearOutMode.UNUSE;
                    //ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                    return;
                }

                string useCommand = inputData.EventGroups[0].Events[0].Items[0].Value;
                if (useCommand == "1")
                {
                    CPCForceCleanOutCommand(eq, inputData, eBitResult.ON);
                }
                else
                {
                    CPCForceCleanOutCommand(eq, inputData, eBitResult.OFF);
                }
                CPCForceCleanOutCommandReply(eBitResult.ON, inputData.TrackKey);

                #region 建立timer               
                Timermanager.CreateTimer(timerID, false, T4,
                    new System.Timers.ElapsedEventHandler(BCForceCleanOutCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

                #endregion

                //ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Force Clean Out Command Mode=[{2}].",
                     eq.Data.NODENO, inputData.TrackKey, useCommand == "1" ? eEnableDisable.Enable : eEnableDisable.Disable));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }
        private void BCForceCleanOutCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            CPCForceCleanOutCommandReply(eBitResult.OFF, trackKey);
            E2BTimeoutCommand(eBitResult.ON, "T4");
            LogWarn(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMNET={0}] [EC <- BC][{1}] Force Clean Out Command T1 Timeout", sArray[0], trackKey));
        }

        private void CPCForceCleanOutCommandReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_ForceClearOutModeCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = "1";
                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  SET BIT=[{2}].",
                        eqpNo, trxID, result.ToString()));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void CPCForceCleanOutCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            Trx trx = GetTrxValues("L3_EQDEquipmentRunModeSetCommand");
            trx[0][1][0].Value = "0";
            trx.TrackKey = trackKey;
            SendToPLC(trx);

            LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMNET={0}] [EQ <- BC][{1}] Force Clean Out Command Bit Off", sArray[0], trackKey));
        }
        // TO EQ
        public void EQForceCleanOutCommandReply(Trx inputData)
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

                if (bitResult == eBitResult.OFF)
                {

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- EQ][{1}] BIT=[OFF] Force Clean Out Command Reply.",
                        eqpNo, inputData.TrackKey));
                    return;
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Force Clean Out Command Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, bitResult.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        #endregion

        #region Tank Change Report
        public void EQTankChangeReport(Trx inputData)
        {
            string eqpNo = inputData.Metadata.NodeNo;
            Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
            if (eqp == null)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Not found Node=[{0}]", eqpNo));
                return;
            }
            int tankNo = 0;
            foreach (Item item in inputData[0][0].Items.AllValues)
            {
                if (int.TryParse(item.Name.Split('#')[1], out tankNo))
                {
                    if (!eqp.File.TankUseMode.ContainsKey(tankNo))
                    {
                        eqp.File.TankUseMode.Add(tankNo, item.Value == "1" ? eEnableDisable.Enable : eEnableDisable.Disable);
                        //上报一次(初始化)
                     //   CPCTankChangeReport(eqp, inputData, tankNo);
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        if ((int)eqp.File.TankUseMode[tankNo] == int.Parse(item.Value))
                        {
                            //值不变不上报
                        }
                        else
                        {
                            eqp.File.TankUseMode[tankNo] = item.Value == "1" ? eEnableDisable.Enable : eEnableDisable.Disable;
                            //更新并且上报
                         //   CPCTankChangeReport(eqp, inputData, tankNo);
                            Thread.Sleep(5000);
                        }
                    }
                }
                else
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Get Tank No Failure", eqpNo, UtilityMethod.GetAgentTrackKey()));
                }

            }
            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
            ObjectManager.EquipmentManager.SaveEquipmentHistory(eqp);

        }
        private void CPCTankChangeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            Trx trx = GetTrxValues("L3_TankChangeReport");
            trx[0][1][0].Value = "0";
            trx.TrackKey = trackKey;
            SendToPLC(trx);
            LogWarn(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMNET={0}] [EC -> BC][{1}] Tank Change Report T1 Timeout", sArray[0], trackKey));
        }

        public void BCTankChangeReportReply(Trx inputData)
        {
            string eqpNo = inputData.Metadata.NodeNo;
            Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
            if (eqp == null)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Not found Node=[{0}]", eqpNo));
                return;
            }

            string timer1ID = "L3_TankChangeReportTimeout";
            if (Timermanager.IsAliveTimer(timer1ID))
            {
                Timermanager.TerminateTimer(timer1ID);
            }
            eBitResult bitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
            string timerID = "L3_BCTankChangeReportReplyTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            if (bitResult == eBitResult.OFF)
            {
                LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] Tank Change Report Reply BIT=[{2}]", eqpNo, inputData.TrackKey, bitResult));
                return;
            }
            Trx trx = GetTrxValues("L3_TankChangeReport");
            trx[0][1][0].Value = "0";
            trx.TrackKey = inputData.TrackKey;
            SendToPLC(trx);

            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                new System.Timers.ElapsedEventHandler(BCTankChangeReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());

            LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] Tank Change Report Reply BIT=[{2}]", eqpNo, inputData.TrackKey, bitResult));

        }
        private void BCTankChangeReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            LogWarn(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMNET={0}] [BC -> EC][{1}] Tank Change Report Reply T2 Timeout", sArray[0], trackKey));
        }
        #endregion

        #region Process Stop Command
        public void BCProcessStopCommand(Trx inputData)
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

                string timerID = string.Format("{0}_{1}", eqpNo, "BCProcessStopCommandTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Process Pause Command.",
                        eqpNo, inputData.TrackKey));

                    CPCProcessStopCommandReply(eBitResult.OFF, inputData.TrackKey, "0");

                    return;
                }
                string pauseCommand = inputData[0][0]["ProcessStopCommand"].Value.Trim();
                string unitNo = inputData[0][0]["UnitNo"].Value.Trim();

                if (eq.File.Status == eEQPStatus.Idle && eq.File.TotalTFTGlassCount == 0)
                {
                    if (pauseCommand == "1")
                    {
                        CPCProcessStopCommand(eq, inputData, eBitResult.ON);
                    }
                    else
                    {
                        CPCProcessStopCommand(eq, inputData, eBitResult.OFF);
                    }
                    CPCProcessStopCommandReply(eBitResult.ON, inputData.TrackKey, "1");
                }
                else
                {
                    CPCProcessStopCommand(eq, inputData, eBitResult.OFF);
                    CPCProcessStopCommandReply(eBitResult.ON, inputData.TrackKey, "2");
                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, T4,
                        new System.Timers.ElapsedEventHandler(BCProcessStopCommandTimeoutAction), inputData.TrackKey);
                }

                #endregion




                if (pauseCommand == "2")
                {
                    CPCProcessStopCommand(eq, inputData, eBitResult.OFF);

                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Process Stop Command Process Stop=[{2}] unitNo=[{3}] .",
                     eq.Data.NODENO, inputData.TrackKey, pauseCommand, unitNo));



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void BCProcessStopCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                E2BTimeoutCommand(eBitResult.ON, "T4");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Process Stop Command T4 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCProcessStopCommandReply(eBitResult result, string trxID, string returnCode)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_ProcessStopCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = returnCode;
                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, "ProcessStopCommandReplyTimeout");

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, T2,
                        new System.Timers.ElapsedEventHandler(CPCProcessStopCommandReplyTimeoutAction), trxID);
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

        private void CPCProcessStopCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Process Stop Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCProcessStopCommandReply(eBitResult.OFF, trackKey, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        // TO BC 
        public void EQProcessStopCommandReply(Trx inputData)
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
                string timerID = string.Format("{0}_{1}", eqpNo, "CPCProcessStopCommandReplyTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[OFF] Process Stop Command Reply.",
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
                    //   CPCProcessStopCommand(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQProcessStopCommandReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Process Stop Command Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCProcessStopCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EQDProcessStopCommand");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Process Stop Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void EQProcessStopCommandReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Process Stop Command Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Transfer Stop Command

        private void BCTransferStopCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');
                E2BTimeoutCommand(eBitResult.ON, "T4");
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Transfer Stop Command T4 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCTransferStopCommandReply(eBitResult result, string trxID, string returnCode)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_TransferStopCommandReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = returnCode;
                trx[0][1][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, "BCTransferStopCommandReplyTimeout");

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, T2,
                        new System.Timers.ElapsedEventHandler(CPCTransferStopCommandReplyTimeoutAction), trxID);
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

        private void CPCTransferStopCommandReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Transfer Stop Command Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCTransferStopCommandReply(eBitResult.OFF, trackKey, "0");
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        // TO BC 
        public void EQTransferStopCommandReply(Trx inputData)
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
                string timerID = string.Format("{0}_{1}", eqpNo, "CPCTransferStopCommandReplyTimeout");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[OFF] Transfer Stop Command Reply.",
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
                    //    CPCTransferStopCommand(eqp, inputData, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(EQTransferStopCommandReplyAction), inputData.TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Transfer Stop Command Reply BIT [{2}].",
                            eqpNo, inputData.TrackKey, (eBitResult)int.Parse(inputData[0][0][0].Value)));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCTransferStopCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_EQDTransferStopCommand");
                // trx.ClearTrxWith0();
                trx[0][1][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Transfer Stop Command T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void EQTransferStopCommandReplyAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Transfer Stop Command Reply  TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #endregion

        #region Glass NG Mark Report
        public void EQGlassNGMarkReport(Trx inputData)
        {
            string eqpNo = inputData.Metadata.NodeNo;
            Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
            if (eqp == null)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Not found Node=[{0}]", eqpNo));
                return;
            }
            Job job = ObjectManager.JobManager.GetJob(inputData[0][0][0].Value, inputData[0][0][1].Value);
            if (job == null)
            {
                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Can't find glass data cassette No = [{2}] slot No=[{3}]", eqpNo, inputData.TrackKey, inputData[0][0][0].Value, inputData[0][0][1].Value));
                return;
            }
            string strNGMarkBit = string.Empty;
            eBitResult bitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
            if (eqp.File.CIMMode == eBitResult.ON)
            {
                char[] NGMarkBit = job.NGMark.ToCharArray();
                for (int i = 0; i < NGMarkBit.Length; i++)
                {

                    if (eqp.Data.NODENAME == "F2ISP_3_Pre_CLN" && i == 21)
                    {
                        NGMarkBit[i] = '1';
                    }
                    else if (eqp.Data.NODENAME == "F2ISP_3_Post_CLN" && i == 22)
                    {
                        NGMarkBit[i] = '1';
                    }
                    strNGMarkBit += NGMarkBit[i];
                }
                job.NGMark = strNGMarkBit;
                ObjectManager.JobManager.AddJob(job);

                CPCGlassNGMarkReport(eqp, inputData, bitResult, strNGMarkBit);
            }
            else
            {
                CPCGlassNGMarkReport(eqp, inputData, eBitResult.OFF, strNGMarkBit);
            }
            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] Glass NG Mark Report cassette No = [{2}] slot No=[{3}]", eqpNo, inputData.TrackKey, inputData[0][0][0].Value, inputData[0][0][1].Value));
        }
        private void CPCGlassNGMarkReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            Trx trx = GetTrxValues("L3_GlassNGMarkReport");
            trx[0][1][0].Value = "0";
            trx.TrackKey = trackKey;
            SendToPLC(trx);
            E2BTimeoutCommand(eBitResult.ON, "T1");
            LogWarn(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Glass NG Mark Report T1 Timeout", sArray[0], trackKey));
        }

        public void BCGlassNGMarkReportReply(Trx inputData)
        {
            string eqpNo = inputData.Metadata.NodeNo;
            Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
            if (eqp == null)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Not found Node=[{0}]", eqpNo));
                return;
            }
            string timer1ID = "L3_GlassNGMarkReportTimeout";
            if (Timermanager.IsAliveTimer(timer1ID))
            {
                Timermanager.TerminateTimer(timer1ID);
            }
            string timerID = "L3_BCGlassNGMarkReportReplyTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            eBitResult bitResult = (eBitResult)int.Parse(inputData[0][0][0].Value);
            if (bitResult == eBitResult.OFF)
            {
                LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] Glass NG Mark Report Reply BIT=[{2}]", eqpNo, inputData.TrackKey, bitResult));
                return;
            }
            Trx trx = GetTrxValues("L3_GlassNGMarkReport");
            trx[0][1][0].Value = "0";
            trx.TrackKey = inputData.TrackKey;
            SendToPLC(trx);
            LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] Glass NG Mark Report Reply BIT=[{2}]", eqpNo, inputData.TrackKey, bitResult));
            Timermanager.CreateTimer(timerID, false, T2,
                new System.Timers.ElapsedEventHandler(BCGlassNGMarkReportReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());
        }
        private void BCGlassNGMarkReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            E2BTimeoutCommand(eBitResult.ON, "T2");
            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] Glass NG Mark Report Reply T1 Timeout", sArray[0], trackKey));
        }

        #endregion

        #region E2B Timeout Setting Modify Report
        public void E2BTimeoutSettingModifyReport(Trx inputData)
        {
            try
            {
                string eqpNo = inputData.Metadata.NodeNo;
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Not found Node=[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (eBitResult)int.Parse(inputData[0][1][0].Value);
                if (bitResult == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] BIT=[OFF] E2B Timeout Setting Modify Report", eqpNo, inputData.TrackKey));
                    return;
                }
                SetTimeout(inputData);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}] BIT=[ON] E2B Timeout Setting Modify Report T1=[{2}] T2=[{3}] T3=[{4}] T4=[{5}] Tm=[{6}] Tn=[{7}] LinkSignalT2=[{8}] LinkSignalT3=[{9}] LinkSignalT4=[{10}] LinkSignalT5=[{11}]", eqpNo, inputData.TrackKey, T1, T2, T3, T4, Tm, Tn, LinkSignalT2, LinkSignalT3, LinkSignalT4, LinkSignalT5));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void SetTimeout(Trx inputData)
        {
            if (inputData[0][0].Items.AllKeys.Contains("T1"))
            {
                if (inputData[0][0]["T1"].Value != "0")
                {
                    T1 = int.Parse(inputData[0][0]["T1"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("T2"))
            {
                if (inputData[0][0]["T2"].Value != "0")
                {
                    T2 = int.Parse(inputData[0][0]["T2"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("T3"))
            {
                if (inputData[0][0]["T3"].Value != "0")
                {
                    T3 = int.Parse(inputData[0][0]["T3"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("T4"))
            {
                if (inputData[0][0]["T4"].Value != "0")
                {
                    T4 = int.Parse(inputData[0][0]["T4"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("Tm"))
            {
                if (inputData[0][0]["Tm"].Value != "0")
                {
                    Tm = int.Parse(inputData[0][0]["Tm"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("Tn"))
            {
                if (inputData[0][0]["Tn"].Value != "0")
                {
                    Tn = int.Parse(inputData[0][0]["Tn"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("LinkSignalT2"))
            {
                if (inputData[0][0]["LinkSignalT2"].Value != "0")
                {
                    LinkSignalT2 = int.Parse(inputData[0][0]["LinkSignalT2"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("LinkSignalT3"))
            {
                if (inputData[0][0]["LinkSignalT3"].Value != "0")
                {
                    LinkSignalT3 = int.Parse(inputData[0][0]["LinkSignalT3"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("LinkSignalT4"))
            {
                if (inputData[0][0]["LinkSignalT4"].Value != "0")
                {
                    LinkSignalT4 = int.Parse(inputData[0][0]["LinkSignalT4"].Value);
                }
            }
            if (inputData[0][0].Items.AllKeys.Contains("LinkSignalT5"))
            {
                if (inputData[0][0]["LinkSignalT5"].Value != "0")
                {
                    LinkSignalT5 = int.Parse(inputData[0][0]["LinkSignalT5"].Value);
                }
            }
        }
        public void E2BTimeoutCommand(eBitResult bitResult, string Name)
        {
            try
            {
                Trx trx = GetTrxValues("L3_E2BTimeoutCommand");
                if (trx == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_E2BTimeoutCommand IN PLCFmt.xml!", "L3", "L3"));
                }
                string timerID = string.Format("L3_{0}_TimeoutCommandBitAutoReset", Name);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.OFF)
                {
                    trx.ClearTrxWith0();
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] E2B Timeout Command", "L3", trx.TrackKey));
                    return;
                }
                string cmdName = Name + "_Timeout_Command";
                if (trx[0][0].Items.AllKeys.ToList<string>().Contains(cmdName))
                {
                    trx[0][0][cmdName].Value = "1";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(E2BTimeoutCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[ON] E2B Timeout Command", "L3", trx.TrackKey));
                }


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void E2BTimeoutCommandTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            Trx trx = GetTrxValues("L3_E2BTimeoutCommand");
            string cmdName = sArray[1] + "_Timeout_Command";
            if (trx[0][0].Items.AllKeys.ToList<string>().Contains(cmdName))
            {
                trx[0][0][cmdName].Value = "0";
                trx.TrackKey = trackKey;
                SendToPLC(trx);
                LogWarn(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] E2B {2} Timeout Command Bit=[OFF]", sArray[0], trx.TrackKey, sArray[1]));
            }
            else
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Not Find Item=[{2}]", sArray[0], trx.TrackKey, cmdName));
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
                job.CurrentEQPNo = inputData.Metadata.NodeNo;
                job.JobId = inputData[0][0][eJOBDATA.JobID].Value.ToString().Trim();

                ObjectManager.JobManager.AddJob(job);
                return job;
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return null;
        }

        public Job CreateJob(string casseqno, string jobseqno, Block inputData)
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


             
                job.CurrentEQPNo = "L3";
                job.JobId = inputData["JobID"].Value.ToString().Trim();

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



                job.CurrentEQPNo = "L3";
                job.JobId = evt[eJOBDATA.JobID].Value.ToString().Trim();

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
                    job.CassetteSequenceNo = int.Parse(inputData[0][0]["LotSequenceNumber"].Value);
                    job.JobSequenceNo = int.Parse(inputData[0][0]["SlotSequenceNumber"].Value);
                    job.JobId = inputData[0][0]["JobID"].Value;

                    job.PRODID = inputData[0][0]["PRODID"].Value;
                    job.OperID = inputData[0][0]["OperID"].Value;
                    job.LotID = inputData[0][0]["LotID"].Value;
                    job.PPID1 = inputData[0][0]["PPID#1"].Value;
                    job.PPID2 = inputData[0][0]["PPID#2"].Value;
                    job.PPID3 = inputData[0][0]["PPID#3"].Value;
                    job.PPID4 = inputData[0][0]["PPID#4"].Value;
                    job.PPID5 = inputData[0][0]["PPID#5"].Value;
                    job.PPID6 = inputData[0][0]["PPID#6"].Value;
                    job.PPID7 = inputData[0][0]["PPID#7"].Value;
                    job.PPID8 = inputData[0][0]["PPID#8"].Value;
                    job.PPID9 = inputData[0][0]["PPID#9"].Value;
                    job.PPID10 = inputData[0][0]["PPID#10"].Value;
                    job.PPID11 = inputData[0][0]["PPID#11"].Value;
                    job.PPID12 = inputData[0][0]["PPID#12"].Value;
                    job.PPID13 = inputData[0][0]["PPID#13"].Value;
                    job.PPID14 = inputData[0][0]["PPID#14"].Value;
                    job.PPID15 = inputData[0][0]["PPID#15"].Value;
                    job.PPID16 = inputData[0][0]["PPID#16"].Value;
                    job.PPID17 = inputData[0][0]["PPID#17"].Value;
                    job.PPID18 = inputData[0][0]["PPID#18"].Value;
                    job.PPID19 = inputData[0][0]["PPID#19"].Value;
                    job.PPID20 = inputData[0][0]["PPID#20"].Value;
                    job.PPID21 = inputData[0][0]["PPID#21"].Value;
                    job.PPID22 = inputData[0][0]["PPID#22"].Value;
                    job.PPID23 = inputData[0][0]["PPID#23"].Value;
                    job.PPID24 = inputData[0][0]["PPID#24"].Value;
                    job.PPID25 = inputData[0][0]["PPID#25"].Value;
                    job.PPID26 = inputData[0][0]["PPID#26"].Value;
                    job.PPID27 = inputData[0][0]["PPID#27"].Value;
                    job.PPID28 = inputData[0][0]["PPID#28"].Value;
                    job.PPID29 = inputData[0][0]["PPID#29"].Value;
                    job.PPID30 = inputData[0][0]["PPID#30"].Value;
                    job.PPID31 = inputData[0][0]["PPID#31"].Value;
                    job.PPID32 = inputData[0][0]["PPID#32"].Value;
                    job.PPID33 = inputData[0][0]["PPID#33"].Value;
                    job.PPID34 = inputData[0][0]["PPID#34"].Value;
                    job.PPID35 = inputData[0][0]["PPID#35"].Value;
                    job.PPID36 = inputData[0][0]["PPID#36"].Value;
                    job.PPID37 = inputData[0][0]["PPID#37"].Value;
                    job.PPID38 = inputData[0][0]["PPID#38"].Value;
                    job.PPID39 = inputData[0][0]["PPID#39"].Value;
                    job.PPID40 = inputData[0][0]["PPID#40"].Value;
                    job.JobType = inputData[0][0]["JobType"].Value;
                    job.JobID = inputData[0][0]["JobID"].Value;
                    job.LotSequenceNumber = inputData[0][0]["LotSequenceNumber"].Value;
                    job.SlotSequenceNumber = inputData[0][0]["SlotSequenceNumber"].Value;
                    job.PropertyCode = inputData[0][0]["PropertyCode"].Value;
                    job.JobJudgeCode = inputData[0][0]["JobJudgeCode"].Value;
                    job.JobGradeCode = inputData[0][0]["JobGradeCode"].Value;
                    job.SubstrateType = inputData[0][0]["SubstrateType"].Value;
                    job.ProcessingFlagMachineLocalNo = inputData[0][0]["ProcessingFlagMachineLocalNo"].Value;
                    job.InspectionFlagMachineLocalNo = inputData[0][0]["InspectionFlagMachineLocalNo"].Value;
                    job.SkipFlagMachineLocalNo = inputData[0][0]["SkipFlagMachineLocalNo"].Value;
                    job.JobSize = inputData[0][0]["JobSize"].Value;
                    job.GlassThickness = inputData[0][0]["GlassThickness"].Value;
                    job.JobAngle = inputData[0][0]["JobAngle"].Value;
                    job.JobFlip = inputData[0][0]["JobFlip"].Value;
                    job.ProcessingCount = inputData[0][0]["ProcessingCount"].Value;
                    job.InspectionJudgeData1 = inputData[0][0]["InspectionJudge"].Value;
                    //job.InspectionJudgeData2 = inputData[0][0]["InspectionJudgeData#2"].Value;
                    //job.InspectionJudgeData3 = inputData[0][0]["InspectionJudgeData#3"].Value;
                    //job.InspectionJudgeDataNotUsed1 = inputData[0][0]["InspectionJudgeDataNotUsed#1"].Value;
                    //job.InspectionJudgeData4 = inputData[0][0]["InspectionJudgeData#4"].Value;
                    //job.InspectionJudgeData5 = inputData[0][0]["InspectionJudgeData#5"].Value;
                    //job.InspectionJudgeData6 = inputData[0][0]["InspectionJudgeData#6"].Value;
                    //job.InspectionJudgeDataNotUsed2 = inputData[0][0]["InspectionJudgeDataNotUsed#2"].Value;
                    //job.InspectionJudgeData7 = inputData[0][0]["InspectionJudgeData#7"].Value;
                    //job.InspectionJudgeData8 = inputData[0][0]["InspectionJudgeData#8"].Value;
                    //job.InspectionJudgeData9 = inputData[0][0]["InspectionJudgeData#9"].Value;
                    //job.InspectionJudgeDataNotUsed3 = inputData[0][0]["InspectionJudgeDataNotUsed#3"].Value;
                    job.RepairMode = inputData[0][0]["RepairMode"].Value;
                    job.JobIDPair = inputData[0][0]["JobIDPair"].Value;
                    job.PairLotSequenceNumber = inputData[0][0]["PairLotSequenceNumber"].Value;
                    job.PairSlotSequenceNumber = inputData[0][0]["PairSlotSequenceNumber"].Value;
                    job.JobJudgeCodePair = inputData[0][0]["JobJudgeCodePair"].Value;
                    job.MMGCode = inputData[0][0]["MMGCode"].Value;
                    job.PanelInchSizeX = inputData[0][0]["PanelInchSizeX"].Value;
                    job.PanelInchSizeY = inputData[0][0]["PanelInchSizeY"].Value;
                    job.PanelGradeCode = inputData[0][0]["PanelGradeCode"].Value;
                    job.SpecialFlag = inputData[0][0]["SpecialFlag"].Value;
                    job.ProductionType = inputData[0][0]["ProductionType"].Value;
                    job.PhotoMode = inputData[0][0]["PhotoMode"].Value;
                    job.LineCode = inputData[0][0]["LineCode"].Value;
                    job.AbnormalFlag = inputData[0][0]["AbnormalFlag"].Value;
                    job.NGMark = inputData[0][0]["NGMark"].Value;
                    job.CellProcessingFlag = inputData[0][0]["CellProcessingFlag"].Value;
                    job.CellFlag = inputData[0][0]["CellFlag"].Value;
                    job.OptionValue = inputData[0][0]["OptionValue"].Value;
                    job.Reserved = inputData[0][0]["Reserved"].Value;

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
        public void UpdateJobData(Equipment eq, Job job, Block inputData)
        {
            try
            {

                #region JobData Common
                lock (job)
                {
                    job.CassetteSequenceNo = int.Parse(inputData["LotSequenceNumber"].Value.ToString());
                    job.JobSequenceNo = int.Parse(inputData["SlotSequenceNumber"].Value.ToString());
                    job.JobId = inputData["JobID"].Value.ToString();

                    job.PRODID = inputData["PRODID"].Value.ToString();
                    job.OperID = inputData["OperID"].Value.ToString();
                    job.LotID = inputData["LotID"].Value.ToString();
                    job.PPID1 = inputData["PPID#1"].Value.ToString();
                    job.PPID2 = inputData["PPID#2"].Value.ToString();
                    job.PPID3 = inputData["PPID#3"].Value.ToString();
                    job.PPID4 = inputData["PPID#4"].Value.ToString();
                    job.PPID5 = inputData["PPID#5"].Value.ToString();
                    job.PPID6 = inputData["PPID#6"].Value.ToString();
                    job.PPID7 = inputData["PPID#7"].Value.ToString();
                    job.PPID8 = inputData["PPID#8"].Value.ToString();
                    job.PPID9 = inputData["PPID#9"].Value.ToString();
                    job.PPID10 = inputData["PPID#10"].Value.ToString();
                    job.PPID11 = inputData["PPID#11"].Value.ToString();
                    job.PPID12 = inputData["PPID#12"].Value.ToString();
                    job.PPID13 = inputData["PPID#13"].Value.ToString();
                    job.PPID14 = inputData["PPID#14"].Value.ToString();
                    job.PPID15 = inputData["PPID#15"].Value.ToString();
                    job.PPID16 = inputData["PPID#16"].Value.ToString();
                    job.PPID17 = inputData["PPID#17"].Value.ToString();
                    job.PPID18 = inputData["PPID#18"].Value.ToString();
                    job.PPID19 = inputData["PPID#19"].Value.ToString();
                    job.PPID20 = inputData["PPID#20"].Value.ToString();
                    job.PPID21 = inputData["PPID#21"].Value.ToString();
                    job.PPID22 = inputData["PPID#22"].Value.ToString();
                    job.PPID23 = inputData["PPID#23"].Value.ToString();
                    job.PPID24 = inputData["PPID#24"].Value.ToString();
                    job.PPID25 = inputData["PPID#25"].Value.ToString();
                    job.PPID26 = inputData["PPID#26"].Value.ToString();
                    job.PPID27 = inputData["PPID#27"].Value.ToString();
                    job.PPID28 = inputData["PPID#28"].Value.ToString();
                    job.PPID29 = inputData["PPID#29"].Value.ToString();
                    job.PPID30 = inputData["PPID#30"].Value.ToString();
                    job.PPID31 = inputData["PPID#31"].Value.ToString();
                    job.PPID32 = inputData["PPID#32"].Value.ToString();
                    job.PPID33 = inputData["PPID#33"].Value.ToString();
                    job.PPID34 = inputData["PPID#34"].Value.ToString();
                    job.PPID35 = inputData["PPID#35"].Value.ToString();
                    job.PPID36 = inputData["PPID#36"].Value.ToString();
                    job.PPID37 = inputData["PPID#37"].Value.ToString();
                    job.PPID38 = inputData["PPID#38"].Value.ToString();
                    job.PPID39 = inputData["PPID#39"].Value.ToString();
                    job.PPID40 = inputData["PPID#40"].Value.ToString();
                    job.JobType = inputData["JobType"].Value.ToString();
                    job.JobID = inputData["JobID"].Value.ToString();
                    job.LotSequenceNumber = inputData["LotSequenceNumber"].Value.ToString();
                    job.SlotSequenceNumber = inputData["SlotSequenceNumber"].Value.ToString();
                    job.PropertyCode = inputData["PropertyCode"].Value.ToString();
                    job.JobJudgeCode = inputData["JobJudgeCode"].Value.ToString();
                    job.JobGradeCode = inputData["JobGradeCode"].Value.ToString();
                    job.SubstrateType = inputData["SubstrateType"].Value.ToString();
                    job.ProcessingFlagMachineLocalNo = inputData["ProcessingFlagMachineLocalNo"].Value.ToString();
                    job.InspectionFlagMachineLocalNo = inputData["InspectionFlagMachineLocalNo"].Value.ToString();
                    job.SkipFlagMachineLocalNo = inputData["SkipFlagMachineLocalNo"].Value.ToString();
                    job.JobSize = inputData["JobSize"].Value.ToString();
                    job.GlassThickness = inputData["GlassThickness"].Value.ToString();
                    job.JobAngle = inputData["JobAngle"].Value.ToString();
                    job.JobFlip = inputData["JobFlip"].Value.ToString();
                    job.ProcessingCount = inputData["ProcessingCount"].Value.ToString();
                    job.InspectionJudgeData1 = inputData["InspectionJudge"].Value.ToString();
                    //job.InspectionJudgeData2 = inputData["InspectionJudgeData#2"].Value.ToString();
                    //job.InspectionJudgeData3 = inputData["InspectionJudgeData#3"].Value.ToString();
                    //job.InspectionJudgeDataNotUsed1 = inputData["InspectionJudgeDataNotUsed#1"].Value.ToString();
                    //job.InspectionJudgeData4 = inputData["InspectionJudgeData#4"].Value.ToString();
                    //job.InspectionJudgeData5 = inputData["InspectionJudgeData#5"].Value.ToString();
                    //job.InspectionJudgeData6 = inputData["InspectionJudgeData#6"].Value.ToString();
                    //job.InspectionJudgeDataNotUsed2 = inputData["InspectionJudgeDataNotUsed#2"].Value.ToString();
                    //job.InspectionJudgeData7 = inputData["InspectionJudgeData#7"].Value.ToString();
                    //job.InspectionJudgeData8 = inputData["InspectionJudgeData#8"].Value.ToString();
                    //job.InspectionJudgeData9 = inputData["InspectionJudgeData#9"].Value.ToString();
                    //job.InspectionJudgeDataNotUsed3 = inputData["InspectionJudgeDataNotUsed#3"].Value.ToString();
                    job.RepairMode = inputData["RepairMode"].Value.ToString();
                    job.JobIDPair = inputData["JobIDPair"].Value.ToString();
                    job.PairLotSequenceNumber = inputData["PairLotSequenceNumber"].Value.ToString();
                    job.PairSlotSequenceNumber = inputData["PairSlotSequenceNumber"].Value.ToString();
                    job.JobJudgeCodePair = inputData["JobJudgeCodePair"].Value.ToString();
                    job.MMGCode = inputData["MMGCode"].Value.ToString();
                    job.PanelInchSizeX = inputData["PanelInchSizeX"].Value.ToString();
                    job.PanelInchSizeY = inputData["PanelInchSizeY"].Value.ToString();
                    job.PanelGradeCode = inputData["PanelGradeCode"].Value.ToString();
                    job.SpecialFlag = inputData["SpecialFlag"].Value.ToString();
                    job.ProductionType = inputData["ProductionType"].Value.ToString();
                    job.PhotoMode = inputData["PhotoMode"].Value.ToString();
                    job.LineCode = inputData["LineCode"].Value.ToString();
                    job.AbnormalFlag = inputData["AbnormalFlag"].Value.ToString();
                    job.NGMark = inputData["NGMark"].Value.ToString();
                    job.CellProcessingFlag = inputData["CellProcessingFlag"].Value.ToString();
                    job.CellFlag = inputData["CellFlag"].Value.ToString();
                    job.OptionValue = inputData["OptionValue"].Value.ToString();
                    job.Reserved = inputData["Reserved"].Value.ToString();

                }

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
                    job.CassetteSequenceNo = int.Parse(evt["LotSequenceNumber"].Value.ToString());
                    job.JobSequenceNo = int.Parse(evt["SlotSequenceNumber"].Value.ToString());
                    job.JobId = evt["JobID"].Value;

                    job.PRODID = evt["PRODID"].Value;
                    job.OperID = evt["OperID"].Value;
                    job.LotID = evt["LotID"].Value;
                    job.PPID1 = evt["PPID#1"].Value;
                    job.PPID2 = evt["PPID#2"].Value;
                    job.PPID3 = evt["PPID#3"].Value;
                    job.PPID4 = evt["PPID#4"].Value;
                    job.PPID5 = evt["PPID#5"].Value;
                    job.PPID6 = evt["PPID#6"].Value;
                    job.PPID7 = evt["PPID#7"].Value;
                    job.PPID8 = evt["PPID#8"].Value;
                    job.PPID9 = evt["PPID#9"].Value;
                    job.PPID10 = evt["PPID#10"].Value;
                    job.PPID11 = evt["PPID#11"].Value;
                    job.PPID12 = evt["PPID#12"].Value;
                    job.PPID13 = evt["PPID#13"].Value;
                    job.PPID14 = evt["PPID#14"].Value;
                    job.PPID15 = evt["PPID#15"].Value;
                    job.PPID16 = evt["PPID#16"].Value;
                    job.PPID17 = evt["PPID#17"].Value;
                    job.PPID18 = evt["PPID#18"].Value;
                    job.PPID19 = evt["PPID#19"].Value;
                    job.PPID20 = evt["PPID#20"].Value;
                    job.PPID21 = evt["PPID#21"].Value;
                    job.PPID22 = evt["PPID#22"].Value;
                    job.PPID23 = evt["PPID#23"].Value;
                    job.PPID24 = evt["PPID#24"].Value;
                    job.PPID25 = evt["PPID#25"].Value;
                    job.PPID26 = evt["PPID#26"].Value;
                    job.PPID27 = evt["PPID#27"].Value;
                    job.PPID28 = evt["PPID#28"].Value;
                    job.PPID29 = evt["PPID#29"].Value;
                    job.PPID30 = evt["PPID#30"].Value;
                    job.PPID31 = evt["PPID#31"].Value;
                    job.PPID32 = evt["PPID#32"].Value;
                    job.PPID33 = evt["PPID#33"].Value;
                    job.PPID34 = evt["PPID#34"].Value;
                    job.PPID35 = evt["PPID#35"].Value;
                    job.PPID36 = evt["PPID#36"].Value;
                    job.PPID37 = evt["PPID#37"].Value;
                    job.PPID38 = evt["PPID#38"].Value;
                    job.PPID39 = evt["PPID#39"].Value;
                    job.PPID40 = evt["PPID#40"].Value;
                    job.JobType = evt["JobType"].Value;
                    job.JobID = evt["JobID"].Value;
                    job.LotSequenceNumber = evt["LotSequenceNumber"].Value;
                    job.SlotSequenceNumber = evt["SlotSequenceNumber"].Value;
                    job.PropertyCode = evt["PropertyCode"].Value;
                    job.JobJudgeCode = evt["JobJudgeCode"].Value;
                    job.JobGradeCode = evt["JobGradeCode"].Value;
                    job.SubstrateType = evt["SubstrateType"].Value;
                    job.ProcessingFlagMachineLocalNo = evt["ProcessingFlagMachineLocalNo"].Value;
                    job.InspectionFlagMachineLocalNo = evt["InspectionFlagMachineLocalNo"].Value;
                    job.SkipFlagMachineLocalNo = evt["SkipFlagMachineLocalNo"].Value;
                    job.JobSize = evt["JobSize"].Value;
                    job.GlassThickness = evt["GlassThickness"].Value;
                    job.JobAngle = evt["JobAngle"].Value;
                    job.JobFlip = evt["JobFlip"].Value;
                    job.ProcessingCount = evt["ProcessingCount"].Value;
                    job.InspectionJudgeData1 = evt["InspectionJudge"].Value;
                    //job.InspectionJudgeData2 = evt["InspectionJudgeData#2"].Value;
                    //job.InspectionJudgeData3 = evt["InspectionJudgeData#3"].Value;
                    //job.InspectionJudgeDataNotUsed1 = evt["InspectionJudgeDataNotUsed#1"].Value;
                    //job.InspectionJudgeData4 = evt["InspectionJudgeData#4"].Value;
                    //job.InspectionJudgeData5 = evt["InspectionJudgeData#5"].Value;
                    //job.InspectionJudgeData6 = evt["InspectionJudgeData#6"].Value;
                    //job.InspectionJudgeDataNotUsed2 = evt["InspectionJudgeDataNotUsed#2"].Value;
                    //job.InspectionJudgeData7 = evt["InspectionJudgeData#7"].Value;
                    //job.InspectionJudgeData8 = evt["InspectionJudgeData#8"].Value;
                    //job.InspectionJudgeData9 = evt["InspectionJudgeData#9"].Value;
                    //job.InspectionJudgeDataNotUsed3 = evt["InspectionJudgeDataNotUsed#3"].Value;
                    job.RepairMode = evt["RepairMode"].Value;
                    job.JobIDPair = evt["JobIDPair"].Value;
                    job.PairLotSequenceNumber = evt["PairLotSequenceNumber"].Value;
                    job.PairSlotSequenceNumber = evt["PairSlotSequenceNumber"].Value;
                    job.JobJudgeCodePair = evt["JobJudgeCodePair"].Value;
                    job.MMGCode = evt["MMGCode"].Value;
                    job.PanelInchSizeX = evt["PanelInchSizeX"].Value;
                    job.PanelInchSizeY = evt["PanelInchSizeY"].Value;
                    job.PanelGradeCode = evt["PanelGradeCode"].Value;
                    job.SpecialFlag = evt["SpecialFlag"].Value;
                    job.ProductionType = evt["ProductionType"].Value;
                    job.PhotoMode = evt["PhotoMode"].Value;
                    job.LineCode = evt["LineCode"].Value;
                    job.AbnormalFlag = evt["AbnormalFlag"].Value;
                    job.NGMark = evt["NGMark"].Value;
                    job.CellProcessingFlag = evt["CellProcessingFlag"].Value;
                    job.CellFlag = evt["CellFlag"].Value;
                    job.OptionValue = evt["OptionValue"].Value;
                    job.Reserved = evt["Reserved"].Value;



                    #endregion
                    job.CurrentEQPNo = "L3";
                }
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
                    inputData[0][0]["PRODID"].Value = job.PRODID;
                    inputData[0][0]["OperID"].Value = job.OperID;
                    inputData[0][0]["LotID"].Value = job.LotID;
                    inputData[0][0]["PPID#1"].Value = job.PPID1;
                    inputData[0][0]["PPID#2"].Value = job.PPID2;
                    inputData[0][0]["PPID#3"].Value = job.PPID3;
                    inputData[0][0]["PPID#4"].Value = job.PPID4;
                    inputData[0][0]["PPID#5"].Value = job.PPID5;
                    inputData[0][0]["PPID#6"].Value = job.PPID6;
                    inputData[0][0]["PPID#7"].Value = job.PPID7;
                    inputData[0][0]["PPID#8"].Value = job.PPID8;
                    inputData[0][0]["PPID#9"].Value = job.PPID9;
                    inputData[0][0]["PPID#10"].Value = job.PPID10;
                    inputData[0][0]["PPID#11"].Value = job.PPID11;
                    inputData[0][0]["PPID#12"].Value = job.PPID12;
                    inputData[0][0]["PPID#13"].Value = job.PPID13;
                    inputData[0][0]["PPID#14"].Value = job.PPID14;
                    inputData[0][0]["PPID#15"].Value = job.PPID15;
                    inputData[0][0]["PPID#16"].Value = job.PPID16;
                    inputData[0][0]["PPID#17"].Value = job.PPID17;
                    inputData[0][0]["PPID#18"].Value = job.PPID18;
                    inputData[0][0]["PPID#19"].Value = job.PPID19;
                    inputData[0][0]["PPID#20"].Value = job.PPID20;
                    inputData[0][0]["PPID#21"].Value = job.PPID21;
                    inputData[0][0]["PPID#22"].Value = job.PPID22;
                    inputData[0][0]["PPID#23"].Value = job.PPID23;
                    inputData[0][0]["PPID#24"].Value = job.PPID24;
                    inputData[0][0]["PPID#25"].Value = job.PPID25;
                    inputData[0][0]["PPID#26"].Value = job.PPID26;
                    inputData[0][0]["PPID#27"].Value = job.PPID27;
                    inputData[0][0]["PPID#28"].Value = job.PPID28;
                    inputData[0][0]["PPID#29"].Value = job.PPID29;
                    inputData[0][0]["PPID#30"].Value = job.PPID30;
                    inputData[0][0]["PPID#31"].Value = job.PPID31;
                    inputData[0][0]["PPID#32"].Value = job.PPID32;
                    inputData[0][0]["PPID#33"].Value = job.PPID33;
                    inputData[0][0]["PPID#34"].Value = job.PPID34;
                    inputData[0][0]["PPID#35"].Value = job.PPID35;
                    inputData[0][0]["PPID#36"].Value = job.PPID36;
                    inputData[0][0]["PPID#37"].Value = job.PPID37;
                    inputData[0][0]["PPID#38"].Value = job.PPID38;
                    inputData[0][0]["PPID#39"].Value = job.PPID39;
                    inputData[0][0]["PPID#40"].Value = job.PPID40;
                    inputData[0][0]["JobType"].Value = job.JobType;
                    inputData[0][0]["JobID"].Value = job.JobID;
                    inputData[0][0]["LotSequenceNumber"].Value = job.LotSequenceNumber;
                    inputData[0][0]["SlotSequenceNumber"].Value = job.SlotSequenceNumber;
                    inputData[0][0]["PropertyCode"].Value = job.PropertyCode;
                    inputData[0][0]["JobJudgeCode"].Value = job.JobJudgeCode;
                    inputData[0][0]["JobGradeCode"].Value = job.JobGradeCode;
                    inputData[0][0]["SubstrateType"].Value = job.SubstrateType;
                    inputData[0][0]["ProcessingFlagMachineLocalNo"].Value = job.ProcessingFlagMachineLocalNo;
                    inputData[0][0]["InspectionFlagMachineLocalNo"].Value = job.InspectionFlagMachineLocalNo;
                    inputData[0][0]["SkipFlagMachineLocalNo"].Value = job.SkipFlagMachineLocalNo;
                    inputData[0][0]["JobSize"].Value = job.JobSize;
                    inputData[0][0]["GlassThickness"].Value = job.GlassThickness;
                    inputData[0][0]["JobAngle"].Value = job.JobAngle;
                    inputData[0][0]["JobFlip"].Value = job.JobFlip;
                    inputData[0][0]["ProcessingCount"].Value = job.ProcessingCount;
                    inputData[0][0]["InspectionJudge"].Value = job.InspectionJudgeData1;
                    //inputData[0][0]["InspectionJudgeData#2"].Value = job.InspectionJudgeData2;
                    //inputData[0][0]["InspectionJudgeData#3"].Value = job.InspectionJudgeData3;
                    //inputData[0][0]["InspectionJudgeDataNotUsed#1"].Value = job.InspectionJudgeDataNotUsed1;
                    //inputData[0][0]["InspectionJudgeData#4"].Value = job.InspectionJudgeData4;
                    //inputData[0][0]["InspectionJudgeData#5"].Value = job.InspectionJudgeData5;
                    //inputData[0][0]["InspectionJudgeData#6"].Value = job.InspectionJudgeData6;
                    //inputData[0][0]["InspectionJudgeDataNotUsed#2"].Value = job.InspectionJudgeDataNotUsed2;
                    //inputData[0][0]["InspectionJudgeData#7"].Value = job.InspectionJudgeData7;
                    //inputData[0][0]["InspectionJudgeData#8"].Value = job.InspectionJudgeData8;
                    //inputData[0][0]["InspectionJudgeData#9"].Value = job.InspectionJudgeData9;
                    //inputData[0][0]["InspectionJudgeDataNotUsed#3"].Value = job.InspectionJudgeDataNotUsed3;
                    inputData[0][0]["RepairMode"].Value = job.RepairMode;
                    inputData[0][0]["JobIDPair"].Value = job.JobIDPair;
                    inputData[0][0]["PairLotSequenceNumber"].Value = job.PairLotSequenceNumber;
                    inputData[0][0]["PairSlotSequenceNumber"].Value = job.PairSlotSequenceNumber;
                    inputData[0][0]["JobJudgeCodePair"].Value = job.JobJudgeCodePair;
                    inputData[0][0]["MMGCode"].Value = job.MMGCode;
                    inputData[0][0]["PanelInchSizeX"].Value = job.PanelInchSizeX;
                    inputData[0][0]["PanelInchSizeY"].Value = job.PanelInchSizeY;
                    inputData[0][0]["PanelGradeCode"].Value = job.PanelGradeCode;
                    inputData[0][0]["SpecialFlag"].Value = job.SpecialFlag;
                    inputData[0][0]["ProductionType"].Value = job.ProductionType;
                    inputData[0][0]["PhotoMode"].Value = job.PhotoMode;
                    inputData[0][0]["LineCode"].Value = job.LineCode;
                    inputData[0][0]["AbnormalFlag"].Value = job.AbnormalFlag;
                    inputData[0][0]["NGMark"].Value = job.NGMark;
                    inputData[0][0]["CellProcessingFlag"].Value = job.CellProcessingFlag;
                    inputData[0][0]["CellFlag"].Value = job.CellFlag;
                    inputData[0][0]["OptionValue"].Value = job.OptionValue;
                    inputData[0][0]["Reserved"].Value = job.Reserved;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void SetJobData(Equipment eq, Job job, Block inputData)
        {
            try
            {

                #region JobData Common
                lock (job)
                {
                    inputData["PRODID"].Value = job.PRODID;
                    inputData["OperID"].Value = job.OperID;
                    inputData["LotID"].Value = job.LotID;
                    inputData["PPID#1"].Value = job.PPID1;
                    inputData["PPID#2"].Value = job.PPID2;
                    inputData["PPID#3"].Value = job.PPID3;
                    inputData["PPID#4"].Value = job.PPID4;
                    inputData["PPID#5"].Value = job.PPID5;
                    inputData["PPID#6"].Value = job.PPID6;
                    inputData["PPID#7"].Value = job.PPID7;
                    inputData["PPID#8"].Value = job.PPID8;
                    inputData["PPID#9"].Value = job.PPID9;
                    inputData["PPID#10"].Value = job.PPID10;
                    inputData["PPID#11"].Value = job.PPID11;
                    inputData["PPID#12"].Value = job.PPID12;
                    inputData["PPID#13"].Value = job.PPID13;
                    inputData["PPID#14"].Value = job.PPID14;
                    inputData["PPID#15"].Value = job.PPID15;
                    inputData["PPID#16"].Value = job.PPID16;
                    inputData["PPID#17"].Value = job.PPID17;
                    inputData["PPID#18"].Value = job.PPID18;
                    inputData["PPID#19"].Value = job.PPID19;
                    inputData["PPID#20"].Value = job.PPID20;
                    inputData["PPID#21"].Value = job.PPID21;
                    inputData["PPID#22"].Value = job.PPID22;
                    inputData["PPID#23"].Value = job.PPID23;
                    inputData["PPID#24"].Value = job.PPID24;
                    inputData["PPID#25"].Value = job.PPID25;
                    inputData["PPID#26"].Value = job.PPID26;
                    inputData["PPID#27"].Value = job.PPID27;
                    inputData["PPID#28"].Value = job.PPID28;
                    inputData["PPID#29"].Value = job.PPID29;
                    inputData["PPID#30"].Value = job.PPID30;
                    inputData["PPID#31"].Value = job.PPID31;
                    inputData["PPID#32"].Value = job.PPID32;
                    inputData["PPID#33"].Value = job.PPID33;
                    inputData["PPID#34"].Value = job.PPID34;
                    inputData["PPID#35"].Value = job.PPID35;
                    inputData["PPID#36"].Value = job.PPID36;
                    inputData["PPID#37"].Value = job.PPID37;
                    inputData["PPID#38"].Value = job.PPID38;
                    inputData["PPID#39"].Value = job.PPID39;
                    inputData["PPID#40"].Value = job.PPID40;
                    inputData["JobType"].Value = job.JobType;
                    inputData["JobID"].Value = job.JobID;
                    inputData["LotSequenceNumber"].Value = job.LotSequenceNumber;
                    inputData["SlotSequenceNumber"].Value = job.SlotSequenceNumber;
                    inputData["PropertyCode"].Value = job.PropertyCode;
                    inputData["JobJudgeCode"].Value = job.JobJudgeCode;
                    inputData["JobGradeCode"].Value = job.JobGradeCode;
                    inputData["SubstrateType"].Value = job.SubstrateType;
                    inputData["ProcessingFlagMachineLocalNo"].Value = job.ProcessingFlagMachineLocalNo;
                    inputData["InspectionFlagMachineLocalNo"].Value = job.InspectionFlagMachineLocalNo;
                    inputData["SkipFlagMachineLocalNo"].Value = job.SkipFlagMachineLocalNo;
                    inputData["JobSize"].Value = job.JobSize;
                    inputData["GlassThickness"].Value = job.GlassThickness;
                    inputData["JobAngle"].Value = job.JobAngle;
                    inputData["JobFlip"].Value = job.JobFlip;
                    inputData["ProcessingCount"].Value = job.ProcessingCount;
                    inputData["InspectionJudge"].Value = job.InspectionJudgeData1;
                    //inputData["InspectionJudgeData#2"].Value = job.InspectionJudgeData2;
                    //inputData["InspectionJudgeData#3"].Value = job.InspectionJudgeData3;
                    //inputData["InspectionJudgeDataNotUsed#1"].Value = job.InspectionJudgeDataNotUsed1;
                    //inputData["InspectionJudgeData#4"].Value = job.InspectionJudgeData4;
                    //inputData["InspectionJudgeData#5"].Value = job.InspectionJudgeData5;
                    //inputData["InspectionJudgeData#6"].Value = job.InspectionJudgeData6;
                    //inputData["InspectionJudgeDataNotUsed#2"].Value = job.InspectionJudgeDataNotUsed2;
                    //inputData["InspectionJudgeData#7"].Value = job.InspectionJudgeData7;
                    //inputData["InspectionJudgeData#8"].Value = job.InspectionJudgeData8;
                    //inputData["InspectionJudgeData#9"].Value = job.InspectionJudgeData9;
                    //inputData["InspectionJudgeDataNotUsed#3"].Value = job.InspectionJudgeDataNotUsed3;
                    inputData["RepairMode"].Value = job.RepairMode;
                    inputData["JobIDPair"].Value = job.JobIDPair;
                    inputData["PairLotSequenceNumber"].Value = job.PairLotSequenceNumber;
                    inputData["PairSlotSequenceNumber"].Value = job.PairSlotSequenceNumber;
                    inputData["JobJudgeCodePair"].Value = job.JobJudgeCodePair;
                    inputData["MMGCode"].Value = job.MMGCode;
                    inputData["PanelInchSizeX"].Value = job.PanelInchSizeX;
                    inputData["PanelInchSizeY"].Value = job.PanelInchSizeY;
                    inputData["PanelGradeCode"].Value = job.PanelGradeCode;
                    inputData["SpecialFlag"].Value = job.SpecialFlag;
                    inputData["ProductionType"].Value = job.ProductionType;
                    inputData["PhotoMode"].Value = job.PhotoMode;
                    inputData["LineCode"].Value = job.LineCode;
                    inputData["AbnormalFlag"].Value = job.AbnormalFlag;
                    inputData["NGMark"].Value = job.NGMark;
                    inputData["CellProcessingFlag"].Value = job.CellProcessingFlag;
                    inputData["CellFlag"].Value = job.CellFlag;
                    inputData["OptionValue"].Value = job.OptionValue;
                    inputData["Reserved"].Value = job.Reserved;
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
                    inputData[0][0][eJOBDATA.CassetteSequenceNumber].Value = job.CassetteSequenceNo.ToString();
                    inputData[0][0][eJOBDATA.SlotSequenceNumber].Value = job.JobSequenceNo.ToString();
                    inputData[0][0][eJOBDATA.JobID].Value = job.JobId.Trim();
                    //inputData[0][0][eJOBDATA.GlassJudgeCode].Value = job.GlassJudgeCode.Trim();
                    //inputData[0][0][eJOBDATA.GlassGradeCode].Value = job.GlassGradeCode.Trim();

                }



            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
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
    }
}



