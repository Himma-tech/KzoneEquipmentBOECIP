using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EipTagLibrary;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.PLCAgent.PLC;
using KZONE.Work;

namespace KZONE.Service
{
    public partial class EquipmentService : AbstractService
    {
        public static EipTagAccess eipTagAccess;
        private CancellationTokenSource monitoringCts;
        //启动EIP
        public bool StartEIP()
        {
            try
            {
                eipTagAccess = new EipTagAccess("EipTag_Config.xml");
                eipTagAccess.OnTagValueChanged += EipTagAccess_OnTagValueChanged;

                // 通过异步方式启动监控
                monitoringCts = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                         eipTagAccess.StartMonitoring();
                    }
                    catch (System.Exception ex)
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                    }
                }, monitoringCts.Token);

                return true;
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }

        /// <summary>
        /// 处理Tag值变化事件
        /// </summary>
        private void EipTagAccess_OnTagValueChanged(object sender, TagValueChangedEventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        switch (e.Item.Name)
                        {
                            // Commands
                            case "CIMModeChangeCommand":
                                HandleCIMModeChangeCommand(e);
                                break;
                            case "CIMMessageSetCommand":
                                HandleCIMMessageSetCommand(e);
                                break;
                            case "CIMMessageClearCommand":
                                HandleCIMMessageClearCommand(e);
                                break;
                            case "DateTimeSetCommand":
                                HandleDateTimeSetCommand(e);
                                break;
                            case "MachineModeChangeCommand":
                                HandleMachineModeChangeCommand(e);
                                break;
                            case "JobReservationCommand":
                                HandleJobReservationCommand(e);
                                break;
                            case "MixRunUnitCountRequest":
                                HandleMixRunUnitCountRequest(e);
                                break;
                            case "SetLastJobCommand":
                                HandleSetLastJobCommand(e);
                                break;
                            case "NoneProcessGlassSignal":
                                HandleNoneProcessGlassSignal(e);
                                break;
                            case "LoadingStopChangeCommand":
                                HandleLoadingStopChangeCommand(e);
                                break;

                            // Reports and Replies
                            case "MachineStatusChangeReportReply":
                                HandleMachineStatusChangeReportReply(e);
                                break;
                            case "CIMMessageConfirmReportReply":
                                HandleCIMMessageConfirmReportReply(e);
                                break;
                            case "DateTimeRequestReply":
                                HandleDateTimeRequestReply(e);
                                break;
                            case "VCRStatusReportReply":
                                HandleVCRStatusReportReply(e);
                                break;
                            case "VCRReadCompleteReportReply":
                                HandleVCRReadCompleteReportReply(e);
                                break;
                            case "JobJudgeResultReportReply":
                                HandleJobJudgeResultReportReply(e);
                                break;
                            case "DummyJobRequestReply":
                                HandleDummyJobRequestReply(e);
                                break;
                            case "MachineModeChangeReportReply":
                                HandleMachineModeChangeReportReply(e);
                                break;
                            case "LoadingStopRequestReply":
                                HandleLoadingStopRequestReply(e);
                                break;
                            case "CSTMoveInReportReply":
                                HandleCSTMoveInReportReply(e);
                                break;
                            case "CSTMoveOutReportReply":
                                HandleCSTMoveOutReportReply(e);
                                break;
                            case "IonizerStatusReportReply":
                                HandleIonizerStatusReportReply(e);
                                break;
                            case "CoolingCompleteReportReply":
                                HandleCoolingCompleteReportReply(e);
                                break;
                            case "OperatorLoginReportReply":
                                HandleOperatorLoginReportReply(e);
                                break;
                            case "CuttingReportReply":
                                HandleCuttingReportReply(e);
                                break;
                            case "CrateDataMapRequestReply":
                                HandleCrateDataMapRequestReply(e);
                                break;
                            case "LiquidMedicineChangeReportReply":
                                HandleLiquidMedicineChangeReportReply(e);
                                break;
                            case "GECDCheckResultReportReply":
                                HandleGECDCheckResultReportReply(e);
                                break;
                            case "OperatorLoginRequestReply":
                                HandleOperatorLoginRequestReply(e);
                                break;
                            case "PanelDarkSpotReportReply":
                                HandlePanelDarkSpotReportReply(e);
                                break;
                            case "FFUSpeedReportReply":
                                HandleFFUSpeedReportReply(e);
                                break;
                            case "ReceivedJobReportReply":
                                HandleReceivedJobReportReply(e);
                                break;
                            case "SentOutJobReportReply":
                                HandleSentOutJobReportReply(e);
                                break;

                            case "JobManualMoveReportReply":
                                HandleJobManualMoveReportReply(e);
                                break;
                            case "JobDataChangeReportReply":
                                HandleJobDataChangeReportReply(e);
                                break;
                            case "JobDataRequestReply":
                                HandleJobDataRequestReply(e);
                                break;
                            case "ProcessStartReportReply":
                                HandleProcessStartReportReply(e);
                                break;
                            case "ProcessEndReportReply":
                                HandleProcessEndReportReply(e);
                                break;
                            case "AlarmReport#1Reply":
                            case "AlarmReport#2Reply":
                            case "AlarmReport#3Reply":
                            case "AlarmReport#4Reply":
                            case "AlarmReport#5Reply":
                                HandleAlarmReportReply(e);
                                break;
                            case "AutoRecipeChangeModeReportReply":
                                HandleAutoRecipeChangeModeReportReply(e);
                                break;
                            case "RecipeChangeReportReply":
                                HandleRecipeChangeReportReply(e);
                                break;
                            case "HandleRecipeRegisterCheckCommand":
                                HandleRecipeRegisterCheckCommand(e);
                                break;
                            case "StoredJobReport#1Reply":
                                HandleStoredJobReportReply(e);
                                break;
                            case "FetchedJobEvent#1Reply":
                                HandleFetchedOutJobReportReply(e);
                                break;
                            case "CurrentRecipeNumberChangeReportReply":
                                HandleCurrentRecipeNumberChangeReportReply(e);
                                break;
                            case "RecipeParameterRequestCommand":
                                HandleRecipeParameterRequestCommand(e);
                                break;
                            case "DVDataReportReply":
                                DVDataReportReply(e);
                                break;

                              
                            case "Reserved":
                        
                                break;
                                

                            default:
                                LogError(MethodBase.GetCurrentMethod().Name + "()", "Unhandled command: " + e.Item.Name);
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                    }
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #region Handler Methods
        private void HandleCIMModeChangeCommand(TagValueChangedEventArgs e)
        {
            try
            {
              
                string eqpNo ="L3";
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
               
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF); 

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand =  eipTagAccess.ReadItemValue("RV_CIMToEQ_Status_01_03_00", "CIMModeChangeCommandBlock", "CIMMode").ToString();
                string no = "1";

                if (cIMModeCommand != "1" && cIMModeCommand != "2")
                {
                    CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "3", no);
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                            eq.Data.NODENO, TrackKey, cIMModeCommand));
                }
                else if (cIMModeCommand == "2")
                {
                    if (eq.File.CIMMode == eBitResult.ON)
                    {
                        CPCCIMModeChangeCommandReply(eBitResult.ON,TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                }
                else
                {
                    if (eq.File.CIMMode == eBitResult.OFF)
                    {
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "2");

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }

                }

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                   
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction),  TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCIMMessageSetCommand(TagValueChangedEventArgs e)
        {
            try
            {
                string eqpNo = "L3";
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
               
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

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
                        eqpNo, TrackKey));

                    CPCMessageDisplayCommandReply(eBitResult.OFF,TrackKey);

                    return;
                }

                CPCMessageDisplayCommandReply(eBitResult.ON, TrackKey);

                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //CPCMessageDisplayCommand(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, T4,
                        new System.Timers.ElapsedEventHandler(BCMessageDisplayCommandTimeoutAction),TrackKey);
                }

                #endregion

                //CIMMessageType
                //CIMMessageID
                //TouchPanelNumber
                //CIMMessageData

                Block CIMMessageSetCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_Status_01_03_00", "CIMMessageSetCommandBlock");

                string CIMMessageType = CIMMessageSetCommandBlock["CIMMessageType"].Value.ToString().Trim();
                string CIMMessageID = CIMMessageSetCommandBlock["CIMMessageID"].Value.ToString().Trim();
                string TouchPanelNumber = CIMMessageSetCommandBlock["TouchPanelNumber"].Value.ToString().Trim();
                string CIMMessageData = CIMMessageSetCommandBlock["CIMMessageData"].Value.ToString().Trim();


                CIMMESSAGEHISTORY cimMessage = new CIMMESSAGEHISTORY();
                cimMessage.NODEID = eq.Data.NODEID;
                cimMessage.NODENO = eq.Data.NODENO;
                cimMessage.OPERATORID = "BC";
                cimMessage.MESSAGETEXT = CIMMessageData;
                cimMessage.UPDATETIME = DateTime.Now;
                cimMessage.MESSAGEID = CIMMessageID;
                cimMessage.MESSAGESTATUS = "set";


                ObjectManager.EquipmentManager.SaveCIMMessageHistory(cimMessage);


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Message Display Command CIM Message ID=[{2}] CIMMessage=[{3}]  .",
                     eq.Data.NODENO,TrackKey, CIMMessageID, CIMMessageData));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        private void HandleRecipeParameterRequestCommand(TagValueChangedEventArgs e)
        {
            try
            {
                string eqpNo = "L3";
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

               
                string timerID = string.Format("{0}_{1}", eqpNo, RecipeParameterRequestTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe Parameter Request.",
                        eqpNo, TrackKey));

                    CPCRecipeParameterRequestReply(eq, eBitResult.OFF, null, "0", null);

                    return;
                }
                Block RecipeParameterRequestCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_RecipeManagement_01_03_00", "RecipeParameterRequestCommandBlock");

                string recipeID = RecipeParameterRequestCommandBlock[0].Value.ToString().Trim();
               

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Recipe Parameter RecipeID=[{2}].",
                     eq.Data.NODENO, TrackKey, recipeID));

                //if (recipeType == "2" && !string.IsNullOrEmpty(recipeID))
                //{
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
              ObjectManager.RecipeManager.ReloadRecipeByNo();

                if (recipeDic[eq.Data.LINEID].ContainsKey(recipeID))
                {

                    CPCRecipeParameterRequestReply(eq, eBitResult.ON, RecipeParameterRequestCommandBlock, "1", recipeDic[eq.Data.LINEID][recipeID]);

                    //RecipeParameterReport(eq, inputData, recipeDic[eq.Data.LINEID][recipeID]);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[ON] Recipe Parameter Validation Command Reply OK .",
                            eq.Data.NODENO, TrackKey));

                }
                else
                {
                    CPCRecipeParameterRequestReply(eq, eBitResult.ON, RecipeParameterRequestCommandBlock, "2", null);

                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[ON] Recipe Parameter Validation Command Reply NG Recipe ID=[{2}] Not exist.",
                            eq.Data.NODENO, TrackKey, recipeID));
                }
       
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T4,
                        new System.Timers.ElapsedEventHandler(BCRecipeParameterRequestTimeoutAction),TrackKey);
                }

                #endregion


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }



        private void HandleCIMMessageClearCommand(TagValueChangedEventArgs e)
        {
            try
            {
                try
                {
                   
                    string strlog = string.Empty;
                    string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    }

                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

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
                            eqpNo, TrackKey));

                        CPCCIMMessageClearCommandReply(eBitResult.OFF, TrackKey);

                        return;
                    }

                    CPCCIMMessageClearCommandReply(eBitResult.ON, TrackKey);

                    #region 建立timer

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    if (bitResult == eBitResult.ON)
                    {
                        //CPCMessageDisplayCommand(eq, inputData, eBitResult.ON);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                            new System.Timers.ElapsedEventHandler(BCCIMMessageClearCommandTimeoutAction), TrackKey);
                    }

                    #endregion

                    //CIMMessageID
                    //TouchPanelNo

                    Block CIMMessageClearCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_Status_01_03_00", "CIMMessageClearCommandBlock");

                    string TouchPanelNo = CIMMessageClearCommandBlock["TouchPanelNo"].Value.ToString().Trim();
                    string CIMMessageID = CIMMessageClearCommandBlock["CIMMessageID"].Value.ToString().Trim();

                    
                    ObjectManager.EquipmentManager.UpdateCIMMessage(CIMMessageID, "clear");


                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                           string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Message Clear Command CIM Message ID=[{2}].",
                         eq.Data.NODENO, TrackKey, CIMMessageID));

                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleDateTimeSetCommand(TagValueChangedEventArgs e)
        {
            try
            {
                try
                {
                    string strlog = string.Empty;
                    string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    }
                    if (eq.File.CIMMode == eBitResult.OFF)
                    {
                        return;
                    }
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

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
                            eqpNo, TrackKey));

                        CPCDateTimeCalibrationCommandReply(eBitResult.OFF, TrackKey);

                        return;
                    }

                    CPCDateTimeCalibrationCommandReply(eBitResult.ON, TrackKey);

                    #region 建立timer

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    //DateTimeYear
                    //DateTimeMonth
                    //DateTimeDay
                    //DateTimeHour
                    //DateTimeMinute
                    //DateTimeSecond


                    Block DateTimeSetCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_Status_01_03_00", "DateTimeSetCommandBlock");

                    string DateTimeYear = DateTimeSetCommandBlock["DateTimeYear"].Value.ToString().Trim();
                    string DateTimeMonth = DateTimeSetCommandBlock["DateTimeMonth"].Value.ToString().Trim();
                    string DateTimeDay = DateTimeSetCommandBlock["DateTimeDay"].Value.ToString().Trim();
                    string DateTimeHour = DateTimeSetCommandBlock["DateTimeHour"].Value.ToString().Trim();
                    string DateTimeMinute = DateTimeSetCommandBlock["DateTimeMinute"].Value.ToString().Trim();
                    string DateTimeSecond = DateTimeSetCommandBlock["DateTimeSecond"].Value.ToString().Trim();
                    //组合为时间戳
                    string dateTime = DateTimeYear + DateTimeMonth.PadLeft(2, '0') + DateTimeDay.PadLeft(2, '0') + DateTimeHour.PadLeft(2, '0') + DateTimeMinute.PadLeft(2, '0') + DateTimeSecond.PadLeft(2, '0');

                    // DateTime inputData = DateTime.ParseExact(dateTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);



                    if (bitResult == eBitResult.ON)
                    {
                        CPCDateTimeCalibrationCommand(eq, dateTime, eBitResult.ON, TrackKey);

                        Timermanager.CreateTimer(timerID, false, T4,
                            new System.Timers.ElapsedEventHandler(BCDateTimeCalibrationCommandTimeoutAction), TrackKey);
                    }

                    #endregion

                    SetPCSystemTime(dateTime);


                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                           string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Date Time Calibration Command DateTime=[{2}]  .",
                         eq.Data.NODENO, TrackKey, dateTime));



                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleMachineModeChangeCommand(TagValueChangedEventArgs e)
        {
            try
            {
                try
                {
                    string strlog = string.Empty;
                    string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    }
                    if (eq.File.CIMMode == eBitResult.OFF)
                    {
                        return;
                    }
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

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
                            eqpNo, TrackKey));

                        CPCEquipmentRunModeSetCommandReply(eBitResult.OFF, TrackKey, "0");

                        return;
                    }
                    string pauseCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status_01_03_00", "MachineModeChangeCommandBlock", "MachineMode").ToString();

                    if (pauseCommand == "1" || pauseCommand == "2" || pauseCommand == "5" || pauseCommand == "15")
                    {
                        if (pauseCommand == "5")
                        {
                            pauseCommand = "3";
                        }
                        if (pauseCommand == "15")
                        {
                            pauseCommand = "4";
                        }

                        CPCEquipmentRunModeSetCommand(eq, TrackKey, pauseCommand, eBitResult.ON);

                        CPCEquipmentRunModeSetCommandReply(eBitResult.ON, TrackKey, "1");
                    }
                    //if ((eq.File.EquipmentRunMode == "3" && pauseCommand != "5")||((pauseCommand == "1" || pauseCommand == "2") && eq.File.EquipmentRunMode != pauseCommand ) )
                    //{
                    //    CPCEquipmentRunModeSetCommand(eq, TrackKey, pauseCommand,eBitResult.ON);

                    //    CPCEquipmentRunModeSetCommandReply(eBitResult.ON, TrackKey, "1");
                    //}
                    else
                    {
                        CPCEquipmentRunModeSetCommandReply(eBitResult.ON, TrackKey, "2");

                    }

                    #region 建立timer

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    if (bitResult == eBitResult.ON)
                    {

                        Timermanager.CreateTimer(timerID, false, T4,
                            new System.Timers.ElapsedEventHandler(BCEquipmentRunModeSetCommandTimeoutAction),TrackKey);
                    }

                    #endregion

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                           string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Equipment Run Mode Set Command RUN Mode=[{2}].",
                         eq.Data.NODENO,TrackKey, pauseCommand));



                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleJobReservationCommand(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleMixRunUnitCountRequest(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSetLastJobCommand(TagValueChangedEventArgs e)
        {
            try
            {
                try
                {
                    string strlog = string.Empty;
                    string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    }

                    if (eq.File.CIMMode == eBitResult.OFF)
                    {
                        return;
                    }
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                    string timerID = string.Format("{0}_{1}", eqpNo, "SetLastJobCommandTimeout");

                    if (bitResult == eBitResult.OFF)
                    {
                        //bit off移除本次timer
                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  Set Last Glass Command.",
                            eqpNo, TrackKey));

                        CPCSetLastGlassCommandReply(eBitResult.OFF, TrackKey, "0");

                        return;
                    }
                    //LotSequenceNumber
                    //SlotSequenceNumber

                    Block SetLastJobCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_Status_01_03_00", "SetLastJobCommandBlock");

                    string csn = SetLastJobCommandBlock["LotSequenceNumber"].Value.ToString().Trim();
                    string jsn = SetLastJobCommandBlock["SlotSequenceNumber"].Value.ToString().Trim();

                    //查找对应的JOB更新JobData

                    Job job = ObjectManager.JobManager.GetJob(csn, jsn);

                    if (job != null)
                    {

                        job.LastFlag = "1";

                        CPCSetLastGlassCommandReply(eBitResult.ON, TrackKey, "1");
                        ObjectManager.JobManager.EnqueueSave(job);

                        JobDataEditReport(eq, job, 0);
                    }
                    else
                    {
                        CPCSetLastGlassCommandReply(eBitResult.ON, TrackKey, "2");
                    }
                    #region 建立timer

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    if (bitResult == eBitResult.ON)
                    {


                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                            new System.Timers.ElapsedEventHandler(BCSetLastGlassCommandTimeoutAction), TrackKey);
                    }

                    #endregion

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                           string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Set Last Glass Command.",
                         eq.Data.NODENO, TrackKey));
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleNoneProcessGlassSignal(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleLoadingStopChangeCommand(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                }

                if (eq.File.CIMMode == eBitResult.OFF)
                {
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                string LoadingStopStatus = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status_01_03_00", "LoadingStopChangeCommandBlock", "LoadingStopStatus").ToString();

                string timerID = string.Format("{0}_{1}", eqpNo, "BCStopBitCommandTimeout");

                if (bitResult == eBitResult.OFF)
                {
                   
                    //LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    //   string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Stop Bit Command Bit=[{2}].",
                    // eq.Data.NODENO, TrackKey, "OFF"));

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "LoadingStopChangeCommandReplyBlock", "ReturnCode", 0);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "LoadingStopChangeCommandReplyBlock", "ReturnCode", 1);
                }
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMCommandReply", "LoadingStopChangeCommandReply", bitResult == eBitResult.ON ? "true" : "false");

                #region 建立timer

                if (bitResult == eBitResult.ON && LoadingStopStatus == "1")
                {
                   
                    lock (eq)
                    {
                        eq.File.StopBitCommand = true;
                    }
                    ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Stop Bit Command Bit=[{2}].",
                     eq.Data.NODENO, TrackKey, "ON"));


                }
                else if (bitResult == eBitResult.ON && LoadingStopStatus == "2")
                {
                  
                    lock (eq)
                    {
                        eq.File.StopBitCommand = false;
                        eq.File.ReceiveStep = eReceiveStep.InlineMode;

                    }
                    ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Stop Bit Command Bit=[{2}].",
                     eq.Data.NODENO, TrackKey, "OFF"));
                }

                    #endregion
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        private void HandleMachineStatusChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
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
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCEquipmentStatusChangeReport(eqp, TrackKey, eBitResult.OFF);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Status Change Report Reply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCIMMessageConfirmReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string timerID = string.Format("{0}_{1}", "L3", CIMMessageConfirmReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "CIMMessageConfirmReport", false);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleDateTimeRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                try
                {
                    string strlog = string.Empty;
                    string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    }
                    if (eq.File.CIMMode == eBitResult.OFF)
                    {
                        return;
                    }
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

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
                            eqpNo, TrackKey));

                        eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "DateTimeRequest", "false");

                        return;
                    }

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "DateTimeRequest", "false");

                    #region 建立timer

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    //DateTimeYear
                    //DateTimeMonth
                    //DateTimeDay
                    //DateTimeHour
                    //DateTimeMinute
                    //DateTimeSecond


                    Block DateTimeSetCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_Status_01_03_00", "DateTimeRequestReplyBlock");

                    string DateTimeYear = DateTimeSetCommandBlock["DateTimeYear"].Value.ToString().Trim();
                    string DateTimeMonth = DateTimeSetCommandBlock["DateTimeMonth"].Value.ToString().Trim();
                    string DateTimeDay = DateTimeSetCommandBlock["DateTimeDay"].Value.ToString().Trim();
                    string DateTimeHour = DateTimeSetCommandBlock["DateTimeHour"].Value.ToString().Trim();
                    string DateTimeMinute = DateTimeSetCommandBlock["DateTimeMinute"].Value.ToString().Trim();
                    string DateTimeSecond = DateTimeSetCommandBlock["DateTimeSecond"].Value.ToString().Trim();
                    //组合为时间戳
                    string dateTime = DateTimeYear + DateTimeMonth.PadLeft(2, '0') + DateTimeDay.PadLeft(2, '0') + DateTimeHour.PadLeft(2, '0') + DateTimeMinute.PadLeft(2, '0') + DateTimeSecond.PadLeft(2, '0');

                    // DateTime inputData = DateTime.ParseExact(dateTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);



                    if (bitResult == eBitResult.ON)
                    {
                        CPCDateTimeCalibrationCommand(eq, dateTime, eBitResult.ON, TrackKey);

                        Timermanager.CreateTimer(timerID, false, T4,
                            new System.Timers.ElapsedEventHandler(BCDateTimeCalibrationCommandTimeoutAction), TrackKey);
                    }

                    #endregion

                    SetPCSystemTime(dateTime);


                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                           string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Date Time Calibration Command DateTime=[{2}]  .",
                         eq.Data.NODENO, TrackKey, dateTime));



                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleVCRStatusReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleVCRReadCompleteReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleJobJudgeResultReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleDummyJobRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleMachineModeChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeChangeReportReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineModeChangeReport", false);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleLoadingStopRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "LoadingStopRequest", false);
            
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCSTMoveInReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCSTMoveOutReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleIonizerStatusReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "IonizerStatusReport", "false");

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCoolingCompleteReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleOperatorLoginReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                try
                {
                    string strlog = string.Empty;
                    string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                    if (eqp == null)
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("Not found Node =[{0}]", eqpNo));
                        return;
                    }
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
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
                            eqpNo, TrackKey));


                        return;
                    }
                    #region 建立timer

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    if (bitResult == eBitResult.ON)
                    {
                        CPCOperatorLoginLogoutReport(eqp, new Trx() , eBitResult.OFF);

                        Timermanager.CreateTimer(timerID, false, T2,
                            new System.Timers.ElapsedEventHandler(BCOperatorLoginLogoutReportReplyAction), TrackKey);
                    }

                    #endregion


                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Operator Login Logout Report Reply BIT [{2}].",
                                eqpNo, TrackKey, bitResult));
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                }
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCuttingReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCrateDataMapRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleLiquidMedicineChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleGECDCheckResultReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleOperatorLoginRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandlePanelDarkSpotReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFFUSpeedReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                var value = e.Value;
                // Implement your logic here
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        private void HandleReceivedJobReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}_{2}", eqpNo, "1", ReceiveGlassDataReportTimeout);

               
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ReceivedJobReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "ReceivedJobReport", false);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ReceivedJobReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSentOutJobReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}_{2}", eqpNo, "1", SendingGlassDataReportTimeout);


                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] SentOutJobReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "SentOutJobReport", false);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] SentOutJobReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleJobManualMoveReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = "L3_GlassDataRemoveRecoveryReportTimeout";
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] JobManualMoveReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobManualMoveReport", "false");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(RemovedJobReportReplyTimeoutAction), TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] JobManualMoveReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleJobDataChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}", eqp.Data.NODENO, JobDataEditReportTimeout);
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] JobDataChangeReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataChangeReport", "false");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] JobDataChangeReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleJobDataRequestReply(TagValueChangedEventArgs e)

        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string eqpNo = "L3";
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
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
                        eqpNo, TrackKey));
                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Block JobDataRequestReplyBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_EventReply_01_03_00", "JobDataRequestReplyBlock");

                if (bitResult == eBitResult.ON)
                {
                    CPCJobDataRequestReply(eBitResult.ON, JobDataRequestReplyBlock);

                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataRequest", "false");

                    Timermanager.CreateTimer(timerID, false, T2,
                        new System.Timers.ElapsedEventHandler(BCJobDataRequestReplyAction), TrackKey);
                }

                #endregion

                eReturnCode1 returncode = JobDataRequestReplyBlock["JobDataRequestAck"].Value.ToString() == "1" ? eReturnCode1.OK : eReturnCode1.NG;

                string cstSeq = JobDataRequestReplyBlock["LotSequenceNumber"].Value.ToString();
                string slotNo = JobDataRequestReplyBlock["SlotSequenceNumber"].Value.ToString();
                string glassID = JobDataRequestReplyBlock["JobID"].Value.ToString().Trim();

                Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                if (returncode == eReturnCode1.OK)
                {

                    if (job != null)
                    {
                       // UpdateJobData(eqp, job, JobDataRequestReplyBlock);

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                          string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Job Data Request Reply CST_SEQNO=[{2}] JOB_SEQNO=[{3}] GLASS_ID=[{4}] Return Code=[{5}], Job Data Exsit.",
                                          eqp.Data.NODENO, TrackKey, cstSeq, slotNo, glassID, returncode.ToString()));

                        ObjectManager.JobManager.EnqueueSave(job);
                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Request.ToString(), eqp.Data.NODEID, eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");


                    }
                    else
                    {
                        job = CreateJob(cstSeq, slotNo, JobDataRequestReplyBlock);
                        //UpdateJobData(eqp, job, JobDataRequestReplyBlock);

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                          string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Job Data Request Reply CST_SEQNO=[{2}] JOB_SEQNO=[{3}] GLASS_ID=[{4}] Return Code=[{5}].",
                                          eqp.Data.NODENO, TrackKey, cstSeq, slotNo, glassID, returncode.ToString()));

                        ObjectManager.JobManager.EnqueueSave(job);
                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Request.ToString(), eqp.Data.NODEID, eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");
                    }

                }
                else
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Job Data Request Reply CST_SEQNO=[{2}] JOB_SEQNO=[{3}] GLASS_ID=[{4}] Return Code=[{5}].",
                                            eqp.Data.NODENO, TrackKey, cstSeq, slotNo, glassID, returncode.ToString()));


                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleProcessStartReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}_{2}", eqp.Data.NODENO, "1", ProcessStartJobReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ProcessStartJobReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    ProcessStartJobReport(eqp, null, eBitResult.OFF, "1");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ProcessStartJobReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void DVDataReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}_{2}", eqp.Data.NODENO, "1", ProcessEndJobReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ProcessDataReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_MachineVariable_03_01_00", "VariableDataEvent", "DVDataReport", false);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ProcessDataReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void HandleProcessEndReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}_{2}", eqp.Data.NODENO, "1", ProcessEndJobReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ProcessEndJobReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    ProcessEndJobReport(eqp, null, eBitResult.OFF, "1");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ProcessEndJobReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAlarmReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                string timerID = string.Format("{0}_{1}_{2}", eqp.Data.NODENO, e.Item.Name.Split('#')[1], "AlarmReport");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Equipment Status Change Report Reply.",
                        eqpNo, TrackKey));

                    return;
                }
                alarmChannel[int.Parse(e.Item.Name.Split('#')[1].Substring(0,1))] = true;
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_AlarmEvent_03_01_00", "AlarmEvent", $"AlarmReport#{e.Item.Name.Split('#')[1].Substring(0,1)}", "false");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Status Change Report Reply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAutoRecipeChangeModeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}", eqp.Data.NODENO, "AutoRecipeChangeModeReport");

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] AutoRecipeChangeModeReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    eipTagAccess.WriteItemValue("RV_CIMToEQ_RecipeManagement_01_03_00", "RecipeEventReply", "AutoRecipeChangeModeReportReply", false);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] AutoRecipeChangeModeReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCurrentRecipeNumberChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}", eqp.Data.NODENO, CPCCurrentRecipeIDReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] CurrentRecipeNumberChangeReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    eipTagAccess.WriteItemValue("RV_CIMToEQ_RecipeManagement_01_03_00", "RecipeEventReply", "CurrentRecipeNumberChangeReportReply", false);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CurrentRecipeNumberChangeReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}", eqp.Data.NODENO, CPCRecipeIDModifyReportTimeout);
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] RecipeChangeReportReply.",
                        eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    eipTagAccess.WriteItemValue("RV_CIMToEQ_RecipeManagement_01_03_00", "RecipeEventReply", "RecipeChangeReportReply", false);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] RecipeChangeReportReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeRegisterCheckCommand(TagValueChangedEventArgs e)
        {
            try
            {
                try
                {
                    string strlog = string.Empty;
                    string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    }
                    if (eq.File.CIMMode == eBitResult.OFF)
                    {
                        return;
                    }
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                    string timerID = string.Format("{0}_{1}", eqpNo, DateTimeCalibrationCommandTimeout);

                    if (bitResult == eBitResult.OFF)
                    {
                        //bit off移除本次timer
                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  RecipeRegisterCheckCommand.",
                            eqpNo, TrackKey));

                        CPCRecipeRegisterValidationCommandReply(eBitResult.OFF, TrackKey);

                        return;
                    }

                    string recipeno = eipTagAccess.ReadItemValue("RV_CIMToEQ_RecipeManagement_01_03_00", "RecipeRegisterCheckCommandBlock", "RecipeNumber").ToString();

                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                                   ObjectManager.RecipeManager.ReloadRecipeByNo();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeno))
                    {

                        CPCRecipeRegisterValidationCommandReply(eBitResult.ON,  "1");

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply OK .",
                                eq.Data.NODENO,TrackKey));

                    }
                    else
                    {
                        CPCRecipeRegisterValidationCommandReply(eBitResult.ON,  "2");

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply NG Recipe ID=[{2}] Not exist.",
                                eq.Data.NODENO, TrackKey, recipeno));
                    }

                    #region 建立timer

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }
                    if (bitResult == eBitResult.ON)
                    {
                        Timermanager.CreateTimer(timerID, false, T4,
                            new System.Timers.ElapsedEventHandler(BCRecipeRegisterValidationCommandTimeoutAction), TrackKey);
                    }

                    #endregion

                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                string pathNo = e.Item.Name.Split('#')[1].Substring(0, 1);

          
                string timerID = string.Format("{0}_{1}_{2}", "L3", pathNo.PadLeft(2, '0'), CPCFetchGlassDataReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] FetchedOutJobReport#1Reply.",
                        eqpNo, TrackKey));

                    return;
                }
                fetchChannel[int.Parse(pathNo)] = true;


                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    eipTagAccess.WriteItemValue("SD_EQToCIM_FetchedEvent01_03_01_00", "FetchedJobEvent", $"FetchedOutJobReport#{e.Item.Name.Split('#')[1].Substring(0, 1)}", false);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  FetchedOutJobReport#1Reply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void HandleStoredJobReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string strlog = string.Empty;
                string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("Not found Node =[{0}]", eqpNo));
                    return;
                }
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                string pathNo = e.Item.Name.Split('#')[1].Substring(0, 1);

                string timerID = string.Format("{0}_{1}_{2}", "L3", pathNo.PadLeft(2, '0'), CPCStoreGlassDataReportTimeout);
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] StoredJobEventReply.",
                        eqpNo, TrackKey));

                    return;
                }
                stroeChannel[int.Parse(pathNo)] = true;
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {

                    eipTagAccess.WriteItemValue("SD_EQToCIM_StoredEvent01_03_01_00", "StoredJobEvent", $"StoredJobReport#{e.Item.Name.Split('#')[1].Substring(0, 1)}", false);

                }

                #endregion


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] StoredJobEventReply BIT [{2}].",
                            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        #endregion
    }
}


