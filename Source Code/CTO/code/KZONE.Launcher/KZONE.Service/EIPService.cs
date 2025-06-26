using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
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
    /// <summary>
    /// Copyright (c) 2025 All Rights Reserved.	
    /// 描述：EIP服务类
    /// 创建人： Himma
    /// 创建时间：2025/6/15 13:14:52
    /// </summary>
    public partial class EquipmentService : AbstractService
    {
        public static EipTagAccess eipTagAccess;
        private CancellationTokenSource monitoringCts;
        private System.Timers.Timer heartbeatTimer; // 定义心跳计时器
        private bool isHeartBeatActive = false; // 当前状态标识
        //启动EIP
        public bool StartEIP()
        {
            try
            {
              
                eipTagAccess = new EipTagAccess("EipTag_Config.xml");
                eipTagAccess.OnTagValueChanged += EipTagAccess_OnTagValueChanged;
                StartHeartbeatSignal();
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
        /// EQToCIM_Hearet
        /// </summary>
        private void StartHeartbeatSignal()
        {
            heartbeatTimer = new System.Timers.Timer(4000); // 设置为每 4 秒触发一次
            heartbeatTimer.Elapsed += OnHeartbeatTimerElapsed;
            heartbeatTimer.AutoReset = true; // 重复触发
            heartbeatTimer.Enabled = true; // 启动计时器
        }
        /// <summary>
        /// EQToCIM_Hearet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeartbeatTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            HandleMachineHeartBeatSignal();
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


                            //20250625

                            #region Machine to Machine Event 分别是1、2、6、7项
                            //1.Inline Loading Stop Request
                            case "Loading_Stop_Request": HandleInlineLoadingStopRequestReply(e); break;

                            //2.Transfer Stop Request
                            case "Transfer_Stop_Request": HandleTransfer_Stop_RequestRequestReply(e); break;
                            //6.Job Manual Move Report
                            case "Job_Manual_Move_Report": HandleJobManualMoveReportReplyEx(e); break;
                            //7.Set First Or Last Job Command  Reply
                            case "Set_First_Or_Last_Job_Command": HandleSetFirstOrLastJobCommandReply(e); break;

                            #endregion

                            #region Machine Status Event 分别是8、9、10、11、12、15、16项
                            //8.Machine_Heart_Beat_Signal
                            //case "Machine_Heart_Beat_Signal": HandleMachineHeartBeatSignalReply(e); break;
                            //9.CIM Mode
                            case "CIM_Mode": HandleCIM_ModeRequestReply(e); break;
                            //10.Upstream Inline Mode
                            case "Upstream_Inline_Mode": HandleUpstream_Inline_ModeRequestReply(e); break;
                            //11.Downstream Inline Mode
                            case "Downstream_Inline_Mode": HandleDownstream_Inline_ModeRequestReply(e); break;
                            //12.Local Alarm State
                            case "Local_Alarm_State": HandleLocalAlarmStateReply(e); break;
                            //15.Auto Recipe Change Mode
                            case "Auto_Recipe_Change_Mode": HandleAuto_Recipe_Change_ModeReply(e); break;
                            //16.Ionizer Status Report
                            case "Ionizer_Status_Report": HandleIonizer_Status_ReportReply(e); break;

                            #endregion


                            #region 对EV_Reply的处理 Machine to Machine Event 分别是1、2、6、7项
                            //1.Inline Loading Stop Request


                            //2.Transfer Stop Request

                            //6.Job Manual Move Report

                            //7.Set First Or Last Job Command  Reply


                            #endregion

                            #region 对EV_Reply的处理 Machine Status Event 分别是8、9、10、11、12、15、16项
                            //8.Machine_Heart_Beat_Signal
                            //case "Machine_Heart_Beat_Signal": HandleMachineHeartBeatSignalReply(e); break;
                            //9.CIM Mode

                            //10.Upstream Inline Mode

                            //11.Downstream Inline Mode

                            //12.Local Alarm State

                            //15.Auto Recipe Change Mode

                            //16.Ionizer Status Report


                            #endregion






                            // Commands

                            #region //EAS Command
                            //CIM_Message_Clear_Command       0:1
                            //Date_Time_Set_Command           0:2
                            //CV_Report_Time_Change_Command
                            //Machine Mode Change Command
                            //Cassette Map Download Command
                            //CIM_Mode_Change_Command         0:6
                            //SV_Report_Time_Change_Command
                            //Loading_Stop_Change_Command
                            //Job_Reservation_Command         0:9
                            //Set_First_Or_Last_Job_Command
                            //EAP_Heart_Beat_Signal           0:11
                            //CIM_Message_Set_Command         0:15
                            #endregion
                            #region //EAS Command Handle
                            //CIM_Message_Clear_Command       0:1
                            case "CIM_Message_Clear_Command":
                                HandleCIM_Message_Clear_Command(e);
                                break;
                            //Date_Time_Set_Command           0:2
                            case "Date_Time_Set_Command":
                                HandleCIM_Message_Clear_Command(e);
                                break;
                            //CV_Report_Time_Change_Command

                            //Machine Mode Change Command

                            //Cassette Map Download Command

                            //CIM_Mode_Change_Command         0:6

                            //SV_Report_Time_Change_Command

                            //Loading_Stop_Change_Command

                            //Job_Reservation_Command         0:9

                            //Set_First_Or_Last_Job_Command

                            //EAP_Heart_Beat_Signal           0:11

                            //CIM_Message_Set_Command         0:15

                            #endregion

                            case "CIMModeChangeCommand":
                                HandleCIMModeChangeCommand(e);
                                break;


                            //这里改成对应的命令
                            case "CIM_Mode_Change_Command":
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


                            //FIX_WB begin
                            //TEST FOR MTL SHEET
                            //EAS Command
                            case "Machine Mode Change Command": HandleMachineModeChangeCommandReply(e); break;
                            //Recipe_Event_Command
                            case "Recipe_Register_Check_Command": HandleRecipeRegisterCheckCommandReply(e); break;
                            case "Recipe_Parameter_Request_Command": HandleRecipeParameterRequestCommandReply(e); break;
                            case "Recipe_List_Request_Command": HandleRecipeListRequestCommandReply(e); break;
                            case "Recipe_Step_Count_Request": HandleRecipeStepCountRequestReply(e); break;
                            case "CIM_Message_Set_Command": HandleCIMMessageSetCommandReply(e); break;
                            //case "CIM_Message_Clear_Command": break;//repeat
                            //case "Date_Time_Set_Command": break;//repeat
                            case "CV_Report_Time_Change_Command": HandleCVReportTimeChangeCommandReply(e); break;
                            //case "Recipe Validation Result Send": break;//not found
                            case "SV_Report_Time_Change_Command": HandleSVReportTimeChangeCommandReply(e); break;
                            case "EAP_Heart_Beat_Signal": HandleEAPHeartBeatSignalReply(e); break;//no reply
                            //EAS Special Command
                          
                            case "Material Check Request": HandleMaterialCheckRequestReply(e); break;
                            case "Material Control Request": HandleMaterialControlRequestReply(e); break;//no reply
                            case "Loading_Stop_Change_Command": HandleLoadingStopChangeCommandReply(e); break;
                            //Special Function
                            case "Job_Reservation_Command": HandleJobReservationCommandReply(e); break;
                            //TEST FOR LTM
                            //Machine to Machine Event
                        
                            //case "Lot End Glass Report":break;//not found
                            //case "Lot First Glass Report": break;
                            case "XFEDDownSignal": HandleXFEDDownSignalReply(e); break;
                           
                            //case "Set_First_Or_Last_Job_Command": break;//repeat
                            //Machine Status Event
                            case "Machine_Heart_Beat_Signal": HandleMachineHeartBeatSignalReply(e); break;//no reply
                         
                            case "VCR Status Report": HandleVCRStatusReportReply(e); break;
                            case "VCR Mismatch Report": HandleVCRMismatchReportReply(e); break;
                         
                            case "Ionizer Status Report": HandleIonizerStatusReportReplyEx(e); break;
                            case "File_Path_Info_Request": HandleFilePathInfoRequestReply(e); break;
                            //Job Event
                            case "Received_Report01": HandleReceivedReport01Reply(e); break;
                            case "Received_Report02": HandleReceivedReport02Reply(e); break;
                            case "Received_Report03": HandleReceivedReport03Reply(e); break;
                            case "Received_Report04": HandleReceivedReport04Reply(e); break;
                            case "Received_Report05": HandleReceivedReport05Reply(e); break;
                            case "Received_Report06": HandleReceivedReport06Reply(e); break;
                            case "Received_Report07": HandleReceivedReport07Reply(e); break;
                            case "Received_Report08": HandleReceivedReport08Reply(e); break;
                            case "Received_Report09": HandleReceivedReport09Reply(e); break;
                            case "Send_Out_Report01": HandleSendOutReport01Reply(e); break;
                            case "Send_Out_Report02": HandleSendOutReport02Reply(e); break;
                            case "Send_Out_Report03": HandleSendOutReport03Reply(e); break;
                            case "Send_Out_Report04": HandleSendOutReport04Reply(e); break;
                            case "Send_Out_Report05": HandleSendOutReport05Reply(e); break;
                            case "Send_Out_Report06": HandleSendOutReport06Reply(e); break;
                            case "Send_Out_Report07": HandleSendOutReport07Reply(e); break;
                            case "Send_Out_Report08": HandleSendOutReport08Reply(e); break;
                            case "Send_Out_Report09": HandleSendOutReport09Reply(e); break;
                            case "Stored_Job_Report01": HandleStoredJobReport01Reply(e); break;
                            case "Stored_Job_Report02": HandleStoredJobReport02Reply(e); break;
                            case "Stored_Job_Report03": HandleStoredJobReport03Reply(e); break;
                            case "Stored_Job_Report04": HandleStoredJobReport04Reply(e); break;
                            case "Stored_Job_Report05": HandleStoredJobReport05Reply(e); break;
                            case "Stored_Job_Report06": HandleStoredJobReport06Reply(e); break;
                            case "Stored_Job_Report07": HandleStoredJobReport07Reply(e); break;
                            case "Stored_Job_Report08": HandleStoredJobReport08Reply(e); break;
                            case "Stored_Job_Report09": HandleStoredJobReport09Reply(e); break;
                            case "Stored_Job_Report10": HandleStoredJobReport10Reply(e); break;
                            case "Stored_Job_Report11": HandleStoredJobReport11Reply(e); break;
                            case "Stored_Job_Report12": HandleStoredJobReport12Reply(e); break;
                            case "Fetched_Out_Job_Report01": HandleFetchedOutJobReport01Reply(e); break;
                            case "Fetched_Out_Job_Report02": HandleFetchedOutJobReport02Reply(e); break;
                            case "Fetched_Out_Job_Report03": HandleFetchedOutJobReport03Reply(e); break;
                            case "Fetched_Out_Job_Report04": HandleFetchedOutJobReport04Reply(e); break;
                            case "Fetched_Out_Job_Report05": HandleFetchedOutJobReport05Reply(e); break;
                            case "Fetched_Out_Job_Report06": HandleFetchedOutJobReport06Reply(e); break;
                            case "Fetched_Out_Job_Report07": HandleFetchedOutJobReport07eply(e); break;
                            case "Fetched_Out_Job_Report08": HandleFetchedOutJobReport08Reply(e); break;
                            case "Fetched_Out_Job_Report09": HandleFetchedOutJobReport09Reply(e); break;
                            case "Fetched_Out_Job_Report10": HandleFetchedOutJobReport10Reply(e); break;
                            case "Fetched_Out_Job_Report11": HandleFetchedOutJobReport11Reply(e); break;
                            case "Fetched_Out_Job_Report12": HandleFetchedOutJobReport12Reply(e); break;
                            //case "Job Manual Move Report": HandleReply(e);break;//repeat
                            //case "Job Process Start Report": HandleReply(e);break;//not found
                            //case "Job Process End Report": HandleReply(e);break;//not found
                            case "Job_Data_Request": HandleJobDataRequestReplyEx(e); break;
                            case "Job_Data_Change_Report": HandleJobDataChangeReportReplyEx(e); break;
                            case "Machine_Mode_Change_Report": HandleMachineModeChangeReportReplyEx(e); break;
                            case "Machine_Status_Change_Report": HandleMachineStatusChangeReportReplyEx(e); break;
                            case "Alarm_Report#1": HandleAlarmReport1Reply(e); break;
                            case "Alarm_Report#2": HandleAlarmReport2Reply(e); break;
                            case "Alarm_Report#3": HandleAlarmReport3Reply(e); break;
                            case "Alarm_Report#4": HandleAlarmReport4Reply(e); break;
                            case "Alarm_Report#5": HandleAlarmReport5Reply(e); break;
                            case "DV_Data_Report": HandleDVDataReportReply(e); break;
                            case "Auto_Recipe_Change_Mode_Report": HandleAutoRecipeChangeModeReportReplyEx(e); break;
                            case "Current_Recipe_ID_Change_Report": HandleCurrentRecipeIDChangeReportReply(e); break;
                            case "Recipe_List_Report": HandleRecipeListReportReply(e); break;
                            case "Recipe_Change_Report": HandleRecipeChangeReportReplyEx(e); break;
                            case "Recipe_Parameter_Report": HandleRecipeParameterReportReply(e); break;
                            //case "Transfer Time Data Report": HandleReply(e);break;//not found
                            case "CIM_Message_Confirm_Report": HandleCIMMessageConfirmReportReplyEx(e); break;
                            case "FAC_Data Report": HandleReportReply(e); break;
                            case "Date_Time_Request": HandleDateTimeRequestReplyEx(e); break;
                            //Machine Special Event
                            case "Panel_Judge_Data_Download_Request": HandlePanelJudgeDataDownloadRequestReply(e); break;
                            case "Panel_Data_Update_Report": HandlePanelDataUpdateReportReply(e); break;
                            case "Material Status Change Report": HandleMaterialStatusChangeReportReply(e); break;
                            case "Job Judge Result Report": HandleJobJudgeResultReportReplyEx(e); break;//no reply
                            case "Dummy Job Request": HandleDummyJobRequestReplyEx(e); break;
                            case "Material Validation Request": HandleMaterialValidationRequestReply(e); break;
                            case "SV_Data_Report": HandleSVDataReportReply(e); break;
                            case "CVData Report": HandleCVDataReportReply(e); break;//no reply
                            case "Operator_Login_Report": HandleOperatorLoginReportReplyEx(e); break;
                            //case "Loading Stop Request": HandleReply(e);break;//repeat
                            //EFEM Event
                            case "EFEM_Operation_Mode_Change_Report": HandleEFEMOperationModeChangeReportReply(e); break;//no reply
                            case "CST_Operation_Mode_Change_Report": HandleCSTOperationModeChangeReportReply(e); break;//tag list not contains the sheet which is CIMToEQ_PortManagement
                            case "Cancel Abort Request Event Report": HandleCancelAbortRequestEventReportReply(e); break;//tag list not contains the sheet which is CIMToEQ_PortManagement
                            //FIX_WB end

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
        /// <summary>
        /// 1.Inline Loading Stop RequestHandle
        /// </summary>
        /// <param name="e"></param>
        private void HandleInlineLoadingStopRequestReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Loading_Stop_Request",
                    true);

                eipTagAccess.WriteItemValue(
                  "SD_EQToCIM_Status_05_01_00",
                  "Loading_Stop_Request_Block",
                  "Loading_Stop_Status",
                1);

                eipTagAccess.WriteItemValue(
                  "SD_EQToCIM_Status_05_01_00",
                  "Loading_Stop_Request_Block",
                  "Loading_Stop_Reason_Code",
                2);
               


                Thread.Sleep(100);
                eipTagAccess.WriteItemValue(
                   "SD_EQToCIM_Status_05_01_00",
                   "Machine_Status_Event",
                   "Loading_Stop_Request",
                   false);

                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Inline Loading Stop:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 2.Transfer Stop Request
        /// </summary>
        /// <param name="e"></param>
        private void HandleTransfer_Stop_RequestRequestReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Loading_Stop_Request",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS_Command_Reply",
                    "Loading_Stop_Change_Command_Reply",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_ProcessSVData_05_01_00",
                    "Loading_Stop_Request_Block",
                    "Loading_Stop_Status",
                    "RUN");

                eipTagAccess.WriteItemValue(
                   "SD_EQToCIM_Status_05_01_00",
                   "Loading_Stop_Change_Command_Reply_Block",
                   "Return_Code",
                  0);


                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Transfer Stop Request:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 6.Job Manual Move Report
        /// </summary>
        /// <param name="e"></param>
        private void HandleJobManualMoveReportReplyEx(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_JobEvent01_05_01_00",
                    "Machine_Job_Event",
                    "Job_Manual_Move_Report",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_JobEvent01_05_01_00",
                    "Job_Manual_Move_Report_Block",
                    "JobID",
                    "No.1");

                eipTagAccess.WriteItemValue(
                   "SD_EQToCIM_JobEvent01_05_01_00",
                   "Job_Manual_Move_Report_Block",
                   "Lot_Sequence_Number",
                   1);
                eipTagAccess.WriteItemValue(
                  "SD_EQToCIM_JobEvent01_05_01_00",
                  "Job_Manual_Move_Report_Block",
                  "Slot_Sequence_Number",
                  2);
                eipTagAccess.WriteItemValue(
                   "SD_EQToCIM_JobEvent01_05_01_00",
                   "Job_Manual_Move_Report_Block",
                   "Job_Position",
                   0);


                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Job Manual Move Report:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 7.Set First Or Last Job Command  Reply
        /// </summary>
        /// <param name="e"></param>
        private void HandleSetFirstOrLastJobCommandReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS_Command_Reply",
                    "Set_FirstOr_Last_Job_Command_Reply",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Set_First_Or_Last_Job_Command_Reply_Block",
                    "Return_Code",
                    0);



                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Set First Or Last Job Command  Reply:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 8.Machine_Heart_Beat_Signal
        /// </summary>
        /// <param name="e"></param>
        private void HandleMachineHeartBeatSignal()
        {
            try {
                if (!isHeartBeatActive) {
                    eipTagAccess.WriteItemValue(
                         "SD_EQToCIM_Status_05_01_00",
                         "Machine_Status_Event",
                         "Machine_Heart_Beat_Signal",
                         true); // 开启心跳信号

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                            "Started Machine_Heart_Beat_Signal: Code=" + isHeartBeatActive.ToString());

                    isHeartBeatActive = true;   // 更新标记状态为开启

                }
                else {
                    eipTagAccess.WriteItemValue(
                        "SD_EQToCIM_Status_05_01_00",
                        "Machine_Status_Event",
                        "Machine_Heart_Beat_Signal",
                        false); //关闭心跳信号

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                            "Stopped Machine_Heart_Beat_Signal: Code=" + isHeartBeatActive.ToString());

                    isHeartBeatActive = false;   // 更新标记状态为关闭
                }

            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "() Exception:", ex);
            }
        }
        /// <summary>
        /// 9.CIM_Mode
        /// </summary>
        /// <param name="e"></param>
        private void HandleCIM_ModeRequestReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "CIM_Mode",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS_Command_Reply",
                    "CIM_Mode_Change_Command_Reply",
                    1);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_ProcessSVData_05_01_00",
                    "CIM_Mode_Change_Command_Reply_Block",
                    "Return_Code",
                    0);




                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "CIM_Mode:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 10.Upstream Inline Mode
        /// </summary>
        /// <param name="e"></param>
        private void HandleUpstream_Inline_ModeRequestReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Upstream_Inline_Mode",
                    true);





                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Upstream Inline Mode:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 11.Downstream Inline Mode
        /// </summary>
        /// <param name="e"></param>
        private void HandleDownstream_Inline_ModeRequestReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Downstream_Inline_Mode",
                    true);





                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Downstream Inline Mode:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 12.Local Alarm State
        /// </summary>
        /// <param name="e"></param>
        private void HandleLocalAlarmStateReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Local_Alarm_State",
                    true);





                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Local Alarm State:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 15.Auto Recipe Change Mode
        /// <param name="e"></param>
        private void HandleAuto_Recipe_Change_ModeReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Auto_Recipe_Change_Mode",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Set_First_Or_Last_Job_Command_Reply_Block",
                    "Return_Code",
                    0);



                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Auto Recipe Change Mode:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 16.Ionizer Status Report
        /// </summary>
        /// <param name="e"></param>
        private void HandleIonizer_Status_ReportReply(TagValueChangedEventArgs e)
        {
            try {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Ionizer_Status_Report",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Ionizer Status Report Block",
                    "Ionizer Status",
                    0);



                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Ionizer Status Report:" + returnCode.ToString());
            }
            catch (Exception ex) {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleOperatorLoginReportReplyEx(TagValueChangedEventArgs e)
        {
            //
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "Operator_Login_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// HandleDummyJobRequestReplyEx
        /// </summary>
        /// <param name="e"></param>
        private void HandleDummyJobRequestReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Dummy_Job_Request",
                    true);
                eipTagAccess.WriteItemValue(
                    "Dummy_Job_Request_Block",
                    "Machine_Status_Event",
                    "Remain_Dummy_Job_Count",
                    1);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Request_Dummy_Job_Count",
                    2);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Request_local_number",
                    3);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Dummy_Type",
                    4);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Port_Number",
                    5);

                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// HandleJobJudgeResultReportReplyEx
        /// </summary>
        /// <param name="e"></param>
        private void HandleJobJudgeResultReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Job_Judge_Result_Report",
                    true);


                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Job_Judge_Result_Report_Block",
                    "Job_ID",
                    1);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Job_Judge_Result_Report_Block",
                    "Lot_Sequence_Number",
                    2);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Job_Judge_Result_Report_Block",
                    "Slot_Sequence_Number",
                    3);


                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Job_Judge_Result_Report_Block",
                    "Unit_Number",
                    4);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Job_Judge_Result_Report_Block",
                    "Slot_Number",
                    5);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Job_Judge_Result_Report_Block",
                    "Job_Judge_Code",
                    6);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Job_Judge_Result_Report_Block",
                    "Job_Grade_Code",
                    7);

                int returnCode = 0;
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        private void HandleDateTimeRequestReplyEx(TagValueChangedEventArgs e)
        {

            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "Date_Time_Request_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void HandleCIMMessageConfirmReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "CIM_Message_Confirm_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        private void HandleRecipeChangeReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Recipe_Event_Reply",
                    "Recipe_Change_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void HandleAutoRecipeChangeModeReportReplyEx(TagValueChangedEventArgs e)
        {

            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Recipe_Event_Reply",
                    "Auto_Recipe_Change_Mode_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void HandleMachineStatusChangeReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "Machine_Status_Change_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void HandleMachineModeChangeReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "Machine_Mode_Change_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            ;
        }

        private void HandleJobDataChangeReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Status_Event_Reply",
                    "Job_Data_Change_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleJobDataRequestReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "Job_Data_Request_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
       
        private void HandleCancelAbortRequestEventReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                //eipTagAccess.WriteItemValue(
                //    "CIMToEQ_PortManagement",
                //    "Indexer Event Reply",
                //    "Cancel Abort Request Event Reply",
                //    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCSTOperationModeChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                //NO CIMToEQ_PortManagement  tag
                /* eipTagAccess.WriteItemValue(
                     "CIMToEQ_PortManagement",
                     "Indexer Event Reply",
                     "CST_Operation_Mode_Change_Report_Reply",
                     returnCode);*/
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleEFEMOperationModeChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCVDataReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSVDataReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_ProcessSVData_05_01_00",
                    "Machine_Variable_Event",
                    "SV_Data_Report",
                    true);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_ProcessSVData_05_01_00",
                    "Machine_Variable_Event",
                    "SV_Report_Time_Change_Command_Reply",
                    1);

                //eipTagAccess.WriteItemValue(
                //    "SD_EQToCIM_ProcessSVData_05_01_00",
                //    "SV_Data_Report_Block",
                //    "Total_Group_Number",
                //    1);

                //eipTagAccess.WriteItemValue(
                //    "SD_EQToCIM_ProcessSVData_05_01_00",
                //    "SV_Data_Report_Block",
                //    "Current_Group_Number",
                //    2);
                //eipTagAccess.WriteItemValue(
                //    "SD_EQToCIM_ProcessSVData_05_01_00",
                //    "SV_Data_Report_Block",
                //    "SV_Report_Time",
                //    3);
                //eipTagAccess.WriteItemValue(
                //    "SD_EQToCIM_ProcessSVData_05_01_00",
                //    "SV_Data_Block",
                //    "SVData",
                //    "SVData");

                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }



        /// <summary>
        /// HandleMaterialValidationRequestReply
        /// </summary>
        /// <param name="e"></param>
        private void HandleMaterialValidationRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Material_Status",
                    "Material_Validation_Request",
                    true);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Material_Type",
                    1);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Material_ID",
                    2);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Unit_Number",
                    3);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Unit_Number",
                    4);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Material_Use_Count",
                    5);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Material_Use_Life_Time",
                    6);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Concentration",
                    7);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Validation_Request_Block",
                    "Concentration",
                    8);

                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// HandleMaterialStatusChangeReportReply
        /// </summary>
        /// <param name="e"></param>
        private void HandleMaterialStatusChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Material_Status",
                    "Material_Status_Change_Report",
                    true);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "Material_Status_Change_Report:" + true.ToString());

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Material_Status",
                    1);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Material_ID",
                    135);


                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Material_Type",
                    1);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Unit_Number",
                    2);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Slot_Number",
                    11);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Material_Use_Count",
                    13);
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Unloading_Code",
                    14);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Material_Status_Change_Report_Block",
                    "Tank_ID",
                    15);

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandlePanelDataUpdateReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                string result = "true";
                eipTagAccess.WriteItemValue(
                    "SD_CIMToEQ_UnpackManagement_01_05_00",
                    "Panel_Data_Update_Report",
                    "Panel_Judge_Data_Download_Request",
                    result);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + result.ToString());

                eipTagAccess.WriteItemValue(
                    "SD_CIMToEQ_UnpackManagement_01_05_00",
                    "Panel_Data_Update_Report_Block",
                    "Job_ID",
                    "1");

                eipTagAccess.WriteItemValue(
                    "SD_CIMToEQ_UnpackManagement_01_05_00",
                    "Panel_Data_Update_Report_Block",
                    "Lot_Sequence_Number",
                    "L1");

                eipTagAccess.WriteItemValue(
                    "SD_CIMToEQ_UnpackManagement_01_05_00",
                    "Panel_Data_Update_Report_Block",
                    "Slot_Sequence_Number",
                    "S1");

                eipTagAccess.WriteItemValue(
                    "SD_CIMToEQ_UnpackManagement_01_05_00",
                    "Panel_Judge_Data_Download Request_Block",
                    "Slot_Sequence_Number",
                    "S1");

                eipTagAccess.WriteItemValue(
                    "SD_CIMToEQ_UnpackManagement_01_05_00",
                    "Panel_Data_Update_Report_Block",
                    "OPER_ID",
                    "O1");

                for (int i = 1; i <= 130; i++)
                {
                    eipTagAccess.WriteItemValue(
                        "SD_CIMToEQ_UnpackManagement_01_05_00",
                        "Panel_Data_Update_Report_Block",
                        $"Panel_Particel_Size_Data#{i}",
                        "true");
                }
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandlePanelJudgeDataDownloadRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                string result = "true";
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_UnpackManagement_05_01_00",
                    "Panel_Judge_Event",
                    "Panel_Judge_Data_Download_Request",
                    true);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + result.ToString());

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_UnpackManagement_05_01_00",
                    "Panel_Judge_Data_Download_Request_Block",
                    "Job_ID",
                    "1");

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_UnpackManagement_05_01_00",
                    "Panel_Judge_Data_Download_Request_Block",
                    "Lot_Sequence_Number",
                    1);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_UnpackManagement_05_01_00",
                    "Panel_Judge_Data_Download_Request_Block",
                    "Slot_Sequence_Number",
                    2);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_UnpackManagement_05_01_00",
                    "Panel_Judge_Data_Download_Request_Block",
                    "Slot_Sequence_Number",
                    3);

                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_UnpackManagement_05_01_00",
                    "Panel_Judge_Data_Download_Request_Block",
                    "Oper_ID",
                    "S1");

                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + result.ToString());

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Variable_Data_Event_Reply",
                    "FAC_Data Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeParameterReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Recipe_Event_Reply",
                    "Recipe_Parameter_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeListReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Recipe_Event_Reply",
                    "Recipe_List_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCurrentRecipeIDChangeReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Recipe_Event_Reply",
                    "Current_Recipe_Change_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleDVDataReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAlarmReport5Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Alarm_Event_Reply",
                    "Alarm_Report#5_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAlarmReport4Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Alarm_Event_Reply",
                    "Alarm_Report#4_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAlarmReport3Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Alarm_Event_Reply",
                    "Alarm_Report#3_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAlarmReport2Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Alarm_Event_Reply",
                    "Alarm_Report#2_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAlarmReport1Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Alarm_Event_Reply",
                    "Alarm_Report#1_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport12Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report12_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport11Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report11_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport10Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                   "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report10_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport09Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report09_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport08Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report08_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport07eply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report07_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport06Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report06_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport05Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report05_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport04Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report04_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport03Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report03_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport02Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report02_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFetchedOutJobReport01Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Fetched_Out_Job_Report01_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport12Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report12_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport11Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report11_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport10Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report10_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport09Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report09_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport08Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report08_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport07Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report07_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport06Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report06_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport05Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report05_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport04Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report04_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport03Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report03_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport02Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report02_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleStoredJobReport01Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Stored_Job_Report01_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport09Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report09_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport08Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report08_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport07Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report07_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport06Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report06_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport05Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report05_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport04Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report04_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport03Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                     "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report03_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSendOutReport02Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report02_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        private void HandleSendOutReport01Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Send_Out_Job_Report01_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport09Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report09_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport08Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report08_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport07Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report07_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport06Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report06_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport05Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report05_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport04Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report04_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport03Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                   "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report03_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport02Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report02_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleReceivedReport01Reply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_EventReply_01_05_00",
                    "Machine_Job_Event_Reply",
                    "Received Job Report01_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleFilePathInfoRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "File Path Info Request Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleAutoRecipeChangeModeReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleVCRMismatchReportReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "VCR_Mismatch_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

       

        private void HandleUpstreamInlineModeReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleMachineHeartBeatSignalReply(TagValueChangedEventArgs e)
        {
            try
            {
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + 0.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        
        private void HandleXFEDDownSignalReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_EQToEQ_Event_02_05_00",
                    "MachineToMachineEventReply",
                    "XFEDDownSignallReply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleJobReservationCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 2;
                eipTagAccess.WriteItemValue(
                    "EQToCIM_Status_05_01_00",
                    "EAS Command Reply",
                    "Job Reservation Command Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleLoadingStopChangeCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS Command Reply",
                    "Loading Stop Change Command Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleMaterialControlRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + 0.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleMaterialCheckRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS Command Reply",
                    "Material Check Request Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

       
        private void HandleEAPHeartBeatSignalReply(TagValueChangedEventArgs e)
        {
            try
            {
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "no reply:" + 0.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleSVReportTimeChangeCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_ProcessSVData_05_01_00",
                    "SV_Report_Time_Change_Command_Reply_Block",
                    "SV Command Return Code",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCVReportTimeChangeCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 2;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS Command Reply",
                    "CV_Report_Time_Change_Command_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleCIMMessageSetCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 2;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS Command Reply",
                    "CIM_Message_Set_Command_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeStepCountRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 1;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_RecipeManagement_05_01_00",
                    "Recipe_Command_Reply",
                    "Recipe_Step_Count_Request_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeListRequestCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 1;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_RecipeManagement_05_01_00",
                    "Recipe_Command_Reply",
                    "Recipe_List_Request_Command_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeParameterRequestCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 1;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_RecipeManagement_05_01_00",
                    "Recipe_Command_Reply",
                    "Recipe_Parameter_Request_Command_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleRecipeRegisterCheckCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 1;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_RecipeManagement_05_01_00",
                    "Recipe_Command_Reply",
                    "Recipe_Register_Check_Command_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void HandleMachineModeChangeCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 2;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "EAS_Command_Reply",
                    "Machine_Mode_Change_Command_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        #region //EAS Command Handle

        //CIM_Message_Clear_Command       0:1

        //Date_Time_Set_Command           0:2

        //CV_Report_Time_Change_Command

        //Machine Mode Change Command

        //Cassette Map Download Command

        //CIM_Mode_Change_Command         0:6

        //SV_Report_Time_Change_Command

        //Loading_Stop_Change_Command

        //Job_Reservation_Command         0:9

        //Set_First_Or_Last_Job_Command

        //EAP_Heart_Beat_Signal           0:11

        //CIM_Message_Set_Command         0:15

        /// <summary>
        /// 1;CIM_Message_Clear_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleCIM_Message_Clear_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "CIM_Message_Clear_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 2;Date_Time_Set_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleDate_Time_Set_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Date_Time_Set_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 3;CV_Report_Time_Change_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleCV_Report_Time_Change_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "CV_Report_Time_Change_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 4;Machine Mode Change Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleMachineModeChangeCommand(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Machine Mode Change Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 5;Cassette Map Download Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleCassetteMapDownloadCommand(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Cassette Map Download Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// 6;CIM_Mode_Change_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleCIM_Mode_Change_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "CIM_Message_Clear_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 7;SV_Report_Time_Change_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleSV_Report_Time_Change_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Date_Time_Set_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 8;Loading_Stop_Change_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleLoading_Stop_Change_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "CV_Report_Time_Change_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 9;Job_Reservation_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleJob_Reservation_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Job_Reservation_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 10;Set_First_Or_Last_Job_Command
        /// </summary>
        /// <param name="e"></param>
        private void HandleSet_First_Or_Last_Job_Command(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Cassette Map Download Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        /// <summary>
        /// 1;EAP_Heart_Beat_Signal
        /// </summary>
        /// <param name="e"></param>
        private void HandleEAP_Heart_Beat_Signal(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "CIM_Message_Clear_Command").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
                }

                #endregion



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        ///// <summary>
        ///// 2;Date_Time_Set_Command
        ///// </summary>
        ///// <param name="e"></param>
        //private void HandleDate_Time_Set_Command(TagValueChangedEventArgs e)
        //{
        //    try
        //    {

        //        string eqpNo = "L3";
        //        string strlog = string.Empty;
        //        string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
        //        if (eq == null)
        //        {
        //            throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
        //        }

        //        eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

        //        string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

        //        if (bitResult == eBitResult.OFF)
        //        {
        //            //bit off移除本次timer
        //            if (Timermanager.IsAliveTimer(timerID))
        //            {
        //                Timermanager.TerminateTimer(timerID);
        //            }

        //            LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
        //                eqpNo, TrackKey));

        //            CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

        //            return;
        //        }

        //        string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Date_Time_Set_Command").ToString();
        //        string no = "1";

        //        if (cIMModeCommand != "1" && cIMModeCommand != "2")
        //        {
        //            CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "3", no);
        //            LogError(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                    eq.Data.NODENO, TrackKey, cIMModeCommand));
        //        }
        //        else if (cIMModeCommand == "2")
        //        {
        //            if (eq.File.CIMMode == eBitResult.ON)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //        }
        //        else
        //        {
        //            if (eq.File.CIMMode == eBitResult.OFF)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "2");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }

        //        }

        //        #region 建立timer

        //        if (Timermanager.IsAliveTimer(timerID))
        //        {
        //            Timermanager.TerminateTimer(timerID);
        //        }
        //        if (bitResult == eBitResult.ON)
        //        {

        //            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
        //                new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
        //        }

        //        #endregion



        //    }
        //    catch (System.Exception ex)
        //    {
        //        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }
        //}
        ///// <summary>
        ///// 3;CV_Report_Time_Change_Command
        ///// </summary>
        ///// <param name="e"></param>
        //private void HandleCV_Report_Time_Change_Command(TagValueChangedEventArgs e)
        //{
        //    try
        //    {

        //        string eqpNo = "L3";
        //        string strlog = string.Empty;
        //        string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
        //        if (eq == null)
        //        {
        //            throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
        //        }

        //        eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

        //        string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

        //        if (bitResult == eBitResult.OFF)
        //        {
        //            //bit off移除本次timer
        //            if (Timermanager.IsAliveTimer(timerID))
        //            {
        //                Timermanager.TerminateTimer(timerID);
        //            }

        //            LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
        //                eqpNo, TrackKey));

        //            CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

        //            return;
        //        }

        //        string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "CV_Report_Time_Change_Command").ToString();
        //        string no = "1";

        //        if (cIMModeCommand != "1" && cIMModeCommand != "2")
        //        {
        //            CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "3", no);
        //            LogError(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                    eq.Data.NODENO, TrackKey, cIMModeCommand));
        //        }
        //        else if (cIMModeCommand == "2")
        //        {
        //            if (eq.File.CIMMode == eBitResult.ON)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //        }
        //        else
        //        {
        //            if (eq.File.CIMMode == eBitResult.OFF)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "2");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }

        //        }

        //        #region 建立timer

        //        if (Timermanager.IsAliveTimer(timerID))
        //        {
        //            Timermanager.TerminateTimer(timerID);
        //        }
        //        if (bitResult == eBitResult.ON)
        //        {

        //            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
        //                new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
        //        }

        //        #endregion



        //    }
        //    catch (System.Exception ex)
        //    {
        //        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }
        //}
        ///// <summary>
        ///// 4;Machine Mode Change Command
        ///// </summary>
        ///// <param name="e"></param>
        //private void HandleMachineModeChangeCommand(TagValueChangedEventArgs e)
        //{
        //    try
        //    {

        //        string eqpNo = "L3";
        //        string strlog = string.Empty;
        //        string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
        //        if (eq == null)
        //        {
        //            throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
        //        }

        //        eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

        //        string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

        //        if (bitResult == eBitResult.OFF)
        //        {
        //            //bit off移除本次timer
        //            if (Timermanager.IsAliveTimer(timerID))
        //            {
        //                Timermanager.TerminateTimer(timerID);
        //            }

        //            LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
        //                eqpNo, TrackKey));

        //            CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

        //            return;
        //        }

        //        string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Machine Mode Change Command").ToString();
        //        string no = "1";

        //        if (cIMModeCommand != "1" && cIMModeCommand != "2")
        //        {
        //            CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "3", no);
        //            LogError(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                    eq.Data.NODENO, TrackKey, cIMModeCommand));
        //        }
        //        else if (cIMModeCommand == "2")
        //        {
        //            if (eq.File.CIMMode == eBitResult.ON)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //        }
        //        else
        //        {
        //            if (eq.File.CIMMode == eBitResult.OFF)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "2");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }

        //        }

        //        #region 建立timer

        //        if (Timermanager.IsAliveTimer(timerID))
        //        {
        //            Timermanager.TerminateTimer(timerID);
        //        }
        //        if (bitResult == eBitResult.ON)
        //        {

        //            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
        //                new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
        //        }

        //        #endregion



        //    }
        //    catch (System.Exception ex)
        //    {
        //        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }
        //}
        ///// <summary>
        ///// 5;Cassette Map Download Command
        ///// </summary>
        ///// <param name="e"></param>
        //private void HandleCassetteMapDownloadCommand(TagValueChangedEventArgs e)
        //{
        //    try
        //    {

        //        string eqpNo = "L3";
        //        string strlog = string.Empty;
        //        string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
        //        if (eq == null)
        //        {
        //            throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
        //        }

        //        eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

        //        string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

        //        if (bitResult == eBitResult.OFF)
        //        {
        //            //bit off移除本次timer
        //            if (Timermanager.IsAliveTimer(timerID))
        //            {
        //                Timermanager.TerminateTimer(timerID);
        //            }

        //            LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
        //                eqpNo, TrackKey));

        //            CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

        //            return;
        //        }

        //        string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status01_01_05_00", "EAS_Command", "Cassette Map Download Command").ToString();
        //        string no = "1";

        //        if (cIMModeCommand != "1" && cIMModeCommand != "2")
        //        {
        //            CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "3", no);
        //            LogError(MethodBase.GetCurrentMethod().Name + "()",
        //                string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                    eq.Data.NODENO, TrackKey, cIMModeCommand));
        //        }
        //        else if (cIMModeCommand == "2")
        //        {
        //            if (eq.File.CIMMode == eBitResult.ON)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //        }
        //        else
        //        {
        //            if (eq.File.CIMMode == eBitResult.OFF)
        //            {
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
        //                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }
        //            else
        //            { // 切换CIM MODE
        //                CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

        //                CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "2");

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
        //                        eq.Data.NODENO, TrackKey, cIMModeCommand));
        //            }

        //        }

        //        #region 建立timer

        //        if (Timermanager.IsAliveTimer(timerID))
        //        {
        //            Timermanager.TerminateTimer(timerID);
        //        }
        //        if (bitResult == eBitResult.ON)
        //        {

        //            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
        //                new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
        //        }

        //        #endregion



        //    }
        //    catch (System.Exception ex)
        //    {
        //        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }
        //}





        #endregion

        #region Handler Methods
        private void HandleCIMModeChangeCommand(TagValueChangedEventArgs e)
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

                string timerID = string.Format("{0}_{1}", eqpNo, CIMModeChangeCommandTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  CIM Mode Change Command.",
                        eqpNo, TrackKey));

                    CPCCIMModeChangeCommandReply(eBitResult.OFF, TrackKey, "0", "0");

                    return;
                }

                string cIMModeCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status_01_03_00", "CIMModeChangeCommandBlock", "CIMMode").ToString();
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
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "2", no);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] CIM Mode Change Command CIMModeCommand=[{2}].",
                                eq.Data.NODENO, TrackKey, cIMModeCommand));
                    }
                    else
                    { // 切换CIM MODE
                        CPCCIMModeChangeCommandReply(eBitResult.ON, TrackKey, "1", no);

                        CPCCIMModeChangeCommand(eq, TrackKey, eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                        new System.Timers.ElapsedEventHandler(BCCIMModeChangeCommandTimeoutAction), TrackKey);
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

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC >- EC][{1}] BIT=[OFF] Message Display Command.",
                        eqpNo, TrackKey));

                    CPCMessageDisplayCommandReply(eBitResult.OFF, TrackKey);

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

                    //Timermanager.CreateTimer(timerID, false, T4,
                    //    new System.Timers.ElapsedEventHandler(BCMessageDisplayCommandTimeoutAction), TrackKey);
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


                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Message Display Command CIM Message ID=[{2}] CIMMessage=[{3}]  .",
                     eq.Data.NODENO, TrackKey, CIMMessageID, CIMMessageData));

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

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Recipe Parameter Request.",
                        eqpNo, TrackKey));

                    CPCRecipeParameterRequestReply(eq, eBitResult.OFF, null, "0", null);

                    return;
                }
                Block RecipeParameterRequestCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_RecipeManagement_01_03_00", "RecipeParameterRequestCommandBlock");

                string recipeID = RecipeParameterRequestCommandBlock[0].Value.ToString().Trim();


                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                //if (Timermanager.IsAliveTimer(timerID))
                //{
                //    Timermanager.TerminateTimer(timerID);
                //}
                //if (bitResult == eBitResult.ON)
                //{
                //    Timermanager.CreateTimer(timerID, false, T4,
                //        new System.Timers.ElapsedEventHandler(BCRecipeParameterRequestTimeoutAction), TrackKey);
                //}

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

                    //Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    //if (eq == null)
                    //{
                    //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    //}

                    //eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                    //string timerID = string.Format("{0}_{1}", eqpNo, CIMMessageClearCommandTimeout);

                    //if (bitResult == eBitResult.OFF)
                    //{
                    //    //bit off移除本次timer
                    //    if (Timermanager.IsAliveTimer(timerID))
                    //    {
                    //        Timermanager.TerminateTimer(timerID);
                    //    }

                    //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("[EQUIPMENT={0}] [BC >- EC][{1}] BIT=[OFF] CIM Message Clear Command.",
                    //        eqpNo, TrackKey));

                    //    CPCCIMMessageClearCommandReply(eBitResult.OFF, TrackKey);

                    //    return;
                    //}

                    //CPCCIMMessageClearCommandReply(eBitResult.ON, TrackKey);

                    //#region 建立timer

                    //if (Timermanager.IsAliveTimer(timerID))
                    //{
                    //    Timermanager.TerminateTimer(timerID);
                    //}
                    //if (bitResult == eBitResult.ON)
                    //{
                    //    //CPCMessageDisplayCommand(eq, inputData, eBitResult.ON);

                    //    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    //        new System.Timers.ElapsedEventHandler(BCCIMMessageClearCommandTimeoutAction), TrackKey);
                    //}

                    #endregion

                    //CIMMessageID
                    //TouchPanelNo

                    Block CIMMessageClearCommandBlock = eipTagAccess.ReadBlockValues("RV_CIMToEQ_Status_01_03_00", "CIMMessageClearCommandBlock");

                    string TouchPanelNo = CIMMessageClearCommandBlock["TouchPanelNo"].Value.ToString().Trim();
                    string CIMMessageID = CIMMessageClearCommandBlock["CIMMessageID"].Value.ToString().Trim();


                    ObjectManager.EquipmentManager.UpdateCIMMessage(CIMMessageID, "clear");


                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                    //Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    //if (eq == null)
                    //{
                    //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    //}
                    //if (eq.File.CIMMode == eBitResult.OFF)
                    //{
                    //    return;
                    //}
                    //eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                    //string timerID = string.Format("{0}_{1}", eqpNo, DateTimeCalibrationCommandTimeout);

                    //if (bitResult == eBitResult.OFF)
                    //{
                    //    //bit off移除本次timer
                    //    if (Timermanager.IsAliveTimer(timerID))
                    //    {
                    //        Timermanager.TerminateTimer(timerID);
                    //    }

                    //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  Date Time Calibration Command.",
                    //        eqpNo, TrackKey));

                    //    CPCDateTimeCalibrationCommandReply(eBitResult.OFF, TrackKey);

                    //    return;
                    //}

                    CPCDateTimeCalibrationCommandReply(eBitResult.ON, TrackKey);

                    #region 建立timer

                    //if (Timermanager.IsAliveTimer(timerID))
                    //{
                    //    Timermanager.TerminateTimer(timerID);
                    //}
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



                    //if (bitResult == eBitResult.ON)
                    //{
                    //    CPCDateTimeCalibrationCommand(eq, dateTime, eBitResult.ON, TrackKey);

                    //    Timermanager.CreateTimer(timerID, false, T4,
                    //        new System.Timers.ElapsedEventHandler(BCDateTimeCalibrationCommandTimeoutAction), TrackKey);
                    //}

                    #endregion

                    SetPCSystemTime(dateTime);


                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

        //private void HandleMachineModeChangeCommand(TagValueChangedEventArgs e)
        //{
        //    try
        //    {
        //        try
        //        {
        //            string strlog = string.Empty;
        //            string TrackKey = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

        //            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
        //            if (eq == null)
        //            {
        //                throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
        //            }
        //            if (eq.File.CIMMode == eBitResult.OFF)
        //            {
        //                return;
        //            }
        //            eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

        //            string timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeSetCommandTimeout);

        //            if (bitResult == eBitResult.OFF)
        //            {
        //                //bit off移除本次timer
        //                if (Timermanager.IsAliveTimer(timerID))
        //                {
        //                    Timermanager.TerminateTimer(timerID);
        //                }

        //                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF] Equipment Run Mode Set Command.",
        //                    eqpNo, TrackKey));

        //                CPCEquipmentRunModeSetCommandReply(eBitResult.OFF, TrackKey, "0");

        //                return;
        //            }
        //            string pauseCommand = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status_01_03_00", "MachineModeChangeCommandBlock", "MachineMode").ToString();

        //            if (pauseCommand == "1" || pauseCommand == "2" || pauseCommand == "5" || pauseCommand == "15")
        //            {
        //                if (pauseCommand == "5")
        //                {
        //                    pauseCommand = "3";
        //                }
        //                if (pauseCommand == "15")
        //                {
        //                    pauseCommand = "4";
        //                }

        //                CPCEquipmentRunModeSetCommand(eq, TrackKey, pauseCommand, eBitResult.ON);

        //                CPCEquipmentRunModeSetCommandReply(eBitResult.ON, TrackKey, "1");
        //            }
        //            //if ((eq.File.EquipmentRunMode == "3" && pauseCommand != "5")||((pauseCommand == "1" || pauseCommand == "2") && eq.File.EquipmentRunMode != pauseCommand ) )
        //            //{
        //            //    CPCEquipmentRunModeSetCommand(eq, TrackKey, pauseCommand,eBitResult.ON);

        //            //    CPCEquipmentRunModeSetCommandReply(eBitResult.ON, TrackKey, "1");
        //            //}
        //            else
        //            {
        //                CPCEquipmentRunModeSetCommandReply(eBitResult.ON, TrackKey, "2");

        //            }

        //            #region 建立timer

        //            if (Timermanager.IsAliveTimer(timerID))
        //            {
        //                Timermanager.TerminateTimer(timerID);
        //            }
        //            if (bitResult == eBitResult.ON)
        //            {

        //                Timermanager.CreateTimer(timerID, false, T4,
        //                    new System.Timers.ElapsedEventHandler(BCEquipmentRunModeSetCommandTimeoutAction),TrackKey);
        //            }

        //            #endregion

        //            LogDebug(MethodBase.GetCurrentMethod().Name + "()",
        //                   string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Equipment Run Mode Set Command RUN Mode=[{2}].",
        //                 eq.Data.NODENO,TrackKey, pauseCommand));



        //        }
        //        catch (System.Exception ex)
        //        {
        //            LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
        //    }
        //}

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

                    //Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    //if (eq == null)
                    //{
                    //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    //}

                    //if (eq.File.CIMMode == eBitResult.OFF)
                    //{
                    //    return;
                    //}
                    //eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                    //string timerID = string.Format("{0}_{1}", eqpNo, "SetLastJobCommandTimeout");

                    //if (bitResult == eBitResult.OFF)
                    //{
                    //    //bit off移除本次timer
                    //    if (Timermanager.IsAliveTimer(timerID))
                    //    {
                    //        Timermanager.TerminateTimer(timerID);
                    //    }

                    //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  Set Last Glass Command.",
                    //        eqpNo, TrackKey));

                    //    CPCSetLastGlassCommandReply(eBitResult.OFF, TrackKey, "0");

                    //    return;
                    //}
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

                    //if (Timermanager.IsAliveTimer(timerID))
                    //{
                    //    Timermanager.TerminateTimer(timerID);
                    //}
                    //if (bitResult == eBitResult.ON)
                    //{


                    //    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                    //        new System.Timers.ElapsedEventHandler(BCSetLastGlassCommandTimeoutAction), TrackKey);
                    //}

                    #endregion

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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

                //Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                //if (eq == null)
                //{
                //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                //}

                //if (eq.File.CIMMode == eBitResult.OFF)
                //{
                //    return;
                //}
                //eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                //string LoadingStopStatus = eipTagAccess.ReadItemValue("RV_CIMToEQ_Status_01_03_00", "LoadingStopChangeCommandBlock", "LoadingStopStatus").ToString();

                //string timerID = string.Format("{0}_{1}", eqpNo, "BCStopBitCommandTimeout");

                //if (bitResult == eBitResult.OFF)
                //{

                //    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //    //   string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Stop Bit Command Bit=[{2}].",
                //    // eq.Data.NODENO, TrackKey, "OFF"));

                //    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "LoadingStopChangeCommandReplyBlock", "ReturnCode", 0);
                //}
                //else
                //{
                //    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "LoadingStopChangeCommandReplyBlock", "ReturnCode", 1);
                //}
                //eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "CIMCommandReply", "LoadingStopChangeCommandReply", bitResult == eBitResult.ON ? "true" : "false");

                #region 建立timer

                //if (bitResult == eBitResult.ON && LoadingStopStatus == "1")
                //{

                //    lock (eq)
                //    {
                //        eq.File.StopBitCommand = true;
                //    }
                //    ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Stop Bit Command Bit=[{2}].",
                //     eq.Data.NODENO, TrackKey, "ON"));


                //}
                //else if (bitResult == eBitResult.ON && LoadingStopStatus == "2")
                //{

                //    lock (eq)
                //    {
                //        eq.File.StopBitCommand = false;
                //        eq.File.ReceiveStep = eReceiveStep.InlineMode;

                //    }
                //    ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //       string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Stop Bit Command Bit=[{2}].",
                //     eq.Data.NODENO, TrackKey, "OFF"));
                //}

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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                //eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                //string timerID = string.Format("{0}_{1}", eqpNo, CPCEquipmentStatusChangeReportReplyTimeout);

                //if (bitResult == eBitResult.OFF)
                //{
                //    //bit off移除本次timer
                //    if (Timermanager.IsAliveTimer(timerID))
                //    {
                //        Timermanager.TerminateTimer(timerID);
                //    }

                //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Equipment Status Change Report Reply.",
                //        eqpNo, TrackKey));

                //    return;
                //}
                #region 建立timer

                //if (Timermanager.IsAliveTimer(timerID))
                //{
                //    Timermanager.TerminateTimer(timerID);
                //}
                //if (bitResult == eBitResult.ON)
                //{
                //    CPCEquipmentStatusChangeReport(eqp, TrackKey, eBitResult.OFF);

                //    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                //        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                //}

                //#endregion


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Status Change Report Reply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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

                    //Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    //if (eq == null)
                    //{
                    //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    //}
                    //if (eq.File.CIMMode == eBitResult.OFF)
                    //{
                    //    return;
                    //}
                    //eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                    //string timerID = string.Format("{0}_{1}", eqpNo, DateTimeCalibrationCommandTimeout);

                    //if (bitResult == eBitResult.OFF)
                    //{
                    //    //bit off移除本次timer
                    //    if (Timermanager.IsAliveTimer(timerID))
                    //    {
                    //        Timermanager.TerminateTimer(timerID);
                    //    }

                    //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  Date Time Calibration Command.",
                    //        eqpNo, TrackKey));

                    //    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "DateTimeRequest", "false");

                    //    return;
                    //}

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "DateTimeRequest", "false");

                    #region 建立timer

                    //if (Timermanager.IsAliveTimer(timerID))
                    //{
                    //    Timermanager.TerminateTimer(timerID);
                    //}
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



                    //if (bitResult == eBitResult.ON)
                    //{
                    //    CPCDateTimeCalibrationCommand(eq, dateTime, eBitResult.ON, TrackKey);

                    //    Timermanager.CreateTimer(timerID, false, T4,
                    //        new System.Timers.ElapsedEventHandler(BCDateTimeCalibrationCommandTimeoutAction), TrackKey);
                    //}

                    #endregion

                    SetPCSystemTime(dateTime);


                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "RV_CIMToEQ_Status01_01_05_00",
                    "Machine_Status_Event_Reply",
                    "VCR_Status_Report_Reply",
                    returnCode);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
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
                //string timerID = string.Format("{0}_{1}", eqpNo, EquipmentRunModeChangeReportReplyTimeout);

                //if (Timermanager.IsAliveTimer(timerID))
                //{
                //    Timermanager.TerminateTimer(timerID);
                //}
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

        private void HandleIonizerStatusReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
                eipTagAccess.WriteItemValue("RV_CIMToEQ_Status01_01_05_00", "Machine_Status_Event_Reply", "Ionizer Status Report Reply", 0);
                LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                                    "reply:" + 0.ToString());
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
                    //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                    //if (eqp == null)
                    //{
                    //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                    //            string.Format("Not found Node =[{0}]", eqpNo));
                    //    return;
                    //}
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                    //string timerID = string.Format("{0}_{1}", eqpNo, CPCOperatorLoginLogoutReportReplyTimeout);

                    //if (bitResult == eBitResult.OFF)
                    //{
                    //    //bit off移除本次timer
                    //    if (Timermanager.IsAliveTimer(timerID))
                    //    {
                    //        Timermanager.TerminateTimer(timerID);
                    //    }

                    //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Operator Login Logout Report Reply.",
                    //        eqpNo, TrackKey));


                    //    return;
                    //}
                    #region 建立timer

                    //if (Timermanager.IsAliveTimer(timerID))
                    //{
                    //    Timermanager.TerminateTimer(timerID);
                    //}
                    //if (bitResult == eBitResult.ON)
                    //{
                    //    CPCOperatorLoginLogoutReport(eqp, new Trx(), eBitResult.OFF);

                    //    Timermanager.CreateTimer(timerID, false, T2,
                    //        new System.Timers.ElapsedEventHandler(BCOperatorLoginLogoutReportReplyAction), TrackKey);
                    //}

                    //#endregion


                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Operator Login Logout Report Reply BIT [{2}].",
                    //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                //string timerID = string.Format("{0}_{1}_{2}", eqpNo, "1", ReceiveGlassDataReportTimeout);


                //if (bitResult == eBitResult.OFF)
                //{
                //    //bit off移除本次timer
                //    if (Timermanager.IsAliveTimer(timerID))
                //    {
                //        Timermanager.TerminateTimer(timerID);
                //    }

                //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ReceivedJobReportReply.",
                //        eqpNo, TrackKey));

                //    return;
                //}
                #region 建立timer

                //if (Timermanager.IsAliveTimer(timerID))
                //{
                //    Timermanager.TerminateTimer(timerID);
                //}
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "ReceivedJobReport", false);

                }

                #endregion


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ReceivedJobReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                //string timerID = string.Format("{0}_{1}_{2}", eqpNo, "1", SendingGlassDataReportTimeout);


                //if (bitResult == eBitResult.OFF)
                //{
                //    //bit off移除本次timer
                //    if (Timermanager.IsAliveTimer(timerID))
                //    {
                //        Timermanager.TerminateTimer(timerID);
                //    }

                //    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //        string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] SentOutJobReportReply.",
                //        eqpNo, TrackKey));

                //    return;
                //}
                #region 建立timer

                //if (Timermanager.IsAliveTimer(timerID))
                //{
                //    Timermanager.TerminateTimer(timerID);
                //}
                //if (bitResult == eBitResult.ON)
                //{
                //    eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "SentOutJobReport", false);

                //}

                #endregion


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] SentOutJobReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = "L3_GlassDataRemoveRecoveryReportTimeout";
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] JobManualMoveReportReply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] JobManualMoveReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}", eqp.Data.NODENO, JobDataEditReportTimeout);
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] JobDataChangeReportReply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] JobDataChangeReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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

                    LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                    //CPCJobDataRequestReply(eBitResult.ON, JobDataRequestReplyBlock);

                    //eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataRequest", "false");

                    //Timermanager.CreateTimer(timerID, false, T2,
                    //    new System.Timers.ElapsedEventHandler(BCJobDataRequestReplyAction), TrackKey);
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

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                                          string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[ON] Job Data Request Reply CST_SEQNO=[{2}] JOB_SEQNO=[{3}] GLASS_ID=[{4}] Return Code=[{5}], Job Data Exsit.",
                                          eqp.Data.NODENO, TrackKey, cstSeq, slotNo, glassID, returncode.ToString()));

                        ObjectManager.JobManager.EnqueueSave(job);
                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Request.ToString(), eqp.Data.NODEID, eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");


                    }
                    else
                    {
                        //job = CreateJob(cstSeq, slotNo, JobDataRequestReplyBlock);
                        //UpdateJobData(eqp, job, JobDataRequestReplyBlock);

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}_{2}", eqp.Data.NODENO, "1", ProcessStartJobReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ProcessStartJobReportReply.",
                    //    eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //ProcessStartJobReport(eqp, null, eBitResult.OFF, "1");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ProcessStartJobReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}_{2}", eqp.Data.NODENO, "1", ProcessEndJobReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ProcessDataReportReply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ProcessDataReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    //LogError(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("Not found Node =[{0}]", eqpNo));
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

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] ProcessEndJobReportReply.",
                    //    eqpNo, TrackKey));

                    return;
                }
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    //ProcessEndJobReport(eqp, null, eBitResult.OFF, "1");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] ProcessEndJobReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    //LogError(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("Not found Node =[{0}]", eqpNo));
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

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] Equipment Status Change Report Reply.",
                    //    eqpNo, TrackKey));

                    return;
                }
                alarmChannel[int.Parse(e.Item.Name.Split('#')[1].Substring(0, 1))] = true;
                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_AlarmEvent_03_01_00", "AlarmEvent", $"AlarmReport#{e.Item.Name.Split('#')[1].Substring(0, 1)}", "false");

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(BCEquipmentStatusChangeReportReplyAction), TrackKey);
                }

                #endregion


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] Equipment Status Change Report Reply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                if (eqp == null)
                {
                    //LogError(MethodBase.GetCurrentMethod().Name + "()",
                    //        string.Format("Not found Node =[{0}]", eqpNo));
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

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] AutoRecipeChangeModeReportReply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] AutoRecipeChangeModeReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}", eqp.Data.NODENO, CPCCurrentRecipeIDReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] CurrentRecipeNumberChangeReportReply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] CurrentRecipeNumberChangeReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
                eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);
                string timerID = string.Format("{0}_{1}", eqp.Data.NODENO, CPCRecipeIDModifyReportTimeout);
                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] RecipeChangeReportReply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] RecipeChangeReportReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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

                    //Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);
                    //if (eq == null)
                    //{
                    //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", eqpNo));
                    //}
                    if (eq.File.CIMMode == eBitResult.OFF)
                    {
                        return;
                    }
                    eBitResult bitResult = (bool.Parse(e.Item.Value.ToString()) ? eBitResult.ON : eBitResult.OFF);

                    //string timerID = string.Format("{0}_{1}", eqpNo, DateTimeCalibrationCommandTimeout);

                    if (bitResult == eBitResult.OFF)
                    {
                        ////bit off移除本次timer
                        //if (Timermanager.IsAliveTimer(timerID))
                        //{
                        //    Timermanager.TerminateTimer(timerID);
                        //}

                        //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                        //    string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] BIT=[OFF]  RecipeRegisterCheckCommand.",
                        //    eqpNo, TrackKey));

                        //CPCRecipeRegisterValidationCommandReply(eBitResult.OFF, TrackKey);

                        return;
                    }

                    string recipeno = eipTagAccess.ReadItemValue("RV_CIMToEQ_RecipeManagement_01_03_00", "RecipeRegisterCheckCommandBlock", "RecipeNumber").ToString();

                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                                   ObjectManager.RecipeManager.ReloadRecipeByNo();

                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeno))
                    {

                        //CPCRecipeRegisterValidationCommandReply(eBitResult.ON, "1");

                        LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply OK .",
                                eq.Data.NODENO, TrackKey));

                    }
                    else
                    {
                        //CPCRecipeRegisterValidationCommandReply(eBitResult.ON, "2");

                        LogError(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Recipe Register Validation Command Reply NG Recipe ID=[{2}] Not exist.",
                                eq.Data.NODENO, TrackKey, recipeno));
                    }

                    #region 建立timer

                    //if (Timermanager.IsAliveTimer(timerID))
                    //{
                    //    Timermanager.TerminateTimer(timerID);
                    //}
                    //if (bitResult == eBitResult.ON)
                    //{
                    //    Timermanager.CreateTimer(timerID, false, T4,
                    //        new System.Timers.ElapsedEventHandler(BCRecipeRegisterValidationCommandTimeoutAction), TrackKey);
                    //}

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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
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

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] FetchedOutJobReport#1Reply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}]  FetchedOutJobReport#1Reply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
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
                //Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                //if (eqp == null)
                //{
                //    LogError(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("Not found Node =[{0}]", eqpNo));
                //    return;
                //}
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

                    //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [EC <- BC][{1}] BIT=[OFF] StoredJobEventReply.",
                    //    eqpNo, TrackKey));

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


                //LogDebug(MethodBase.GetCurrentMethod().Name + "()",
                //            string.Format("[EQUIPMENT={0}] [BC -> EC][{1}] StoredJobEventReply BIT [{2}].",
                //            eqpNo, TrackKey, bitResult));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }


        #endregion
    }
}
#endregion