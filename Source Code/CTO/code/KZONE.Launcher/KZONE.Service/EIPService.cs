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
                StartHeartbeatSignal();
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
        /// <summary>
        /// 1.Inline Loading Stop RequestHandle
        /// </summary>
        /// <param name="e"></param>
        private void HandleInlineLoadingStopRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Loading_Stop_Request",
                    true);

                eipTagAccess.WriteItemValue(
                  "SD_EQToCIM_Status_05_01_00",
                  "Loading_Stop_Request_Block",
                  "Loading_Stop_Reason_Code",
                2);
                //eipTagAccess.WriteItemValue(
                //    "RV_CIMTOEQ_Status_05_01_00",
                //    "EAS_Command_Reply",
                //    "Loading_Stop_Change_Command_Reply",
                //    1);



                eipTagAccess.WriteItemValue(
                   "SD_EQToCIM_Status_05_01_00",
                   "Machine_Status_Event",
                   "Loading_Stop_Request",
                   false);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Inline Loading Stop:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 2.Transfer Stop Request
        /// </summary>
        /// <param name="e"></param>
        private void HandleTransfer_Stop_RequestRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
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
                    1);

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


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Transfer Stop Request:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 6.Job Manual Move Report
        /// </summary>
        /// <param name="e"></param>
        private void HandleJobManualMoveReportReplyEx(TagValueChangedEventArgs e)
        {
            try
            {
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


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Job Manual Move Report:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 7.Set First Or Last Job Command  Reply
        /// </summary>
        /// <param name="e"></param>
        private void HandleSetFirstOrLastJobCommandReply(TagValueChangedEventArgs e)
        {
            try
            {
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



                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Set First Or Last Job Command  Reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 8.Machine_Heart_Beat_Signal
        /// </summary>
        /// <param name="e"></param>
        private void HandleMachineHeartBeatSignal()
        {
            try
            {
                if (!isHeartBeatActive)
                {
                    eipTagAccess.WriteItemValue(
                         "SD_EQToCIM_Status_05_01_00",
                         "Machine_Status_Event",
                         "Machine_Heart_Beat_Signal",
                         true); // 开启心跳信号

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            "Started Machine_Heart_Beat_Signal: Code=" + isHeartBeatActive.ToString());

                    isHeartBeatActive = true;   // 更新标记状态为开启

                }
                else
                {
                    eipTagAccess.WriteItemValue(
                        "SD_EQToCIM_Status_05_01_00",
                        "Machine_Status_Event",
                        "Machine_Heart_Beat_Signal",
                        false); //关闭心跳信号

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            "Stopped Machine_Heart_Beat_Signal: Code=" + isHeartBeatActive.ToString());

                    isHeartBeatActive = false;   // 更新标记状态为关闭
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "() Exception:", ex);
            }
        }
        /// <summary>
        /// 9.CIM_Mode
        /// </summary>
        /// <param name="e"></param>
        private void HandleCIM_ModeRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
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




                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "CIM_Mode:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 10.Upstream Inline Mode
        /// </summary>
        /// <param name="e"></param>
        private void HandleUpstream_Inline_ModeRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Upstream_Inline_Mode",
                    true);





                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Upstream Inline Mode:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 11.Downstream Inline Mode
        /// </summary>
        /// <param name="e"></param>
        private void HandleDownstream_Inline_ModeRequestReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Downstream_Inline_Mode",
                    true);





                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Downstream Inline Mode:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 12.Local Alarm State
        /// </summary>
        /// <param name="e"></param>
        private void HandleLocalAlarmStateReply(TagValueChangedEventArgs e)
        {
            try
            {
                int returnCode = 0;
                eipTagAccess.WriteItemValue(
                    "SD_EQToCIM_Status_05_01_00",
                    "Machine_Status_Event",
                    "Local_Alarm_State",
                    true);





                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Local Alarm State:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 15.Auto Recipe Change Mode
        /// <param name="e"></param>
        private void HandleAuto_Recipe_Change_ModeReply(TagValueChangedEventArgs e)
        {
            try
            {
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



                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Auto Recipe Change Mode:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 16.Ionizer Status Report
        /// </summary>
        /// <param name="e"></param>
        private void HandleIonizer_Status_ReportReply(TagValueChangedEventArgs e)
        {
            try
            {
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



                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "Ionizer Status Report:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    "reply:" + returnCode.ToString());
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }





    }
}