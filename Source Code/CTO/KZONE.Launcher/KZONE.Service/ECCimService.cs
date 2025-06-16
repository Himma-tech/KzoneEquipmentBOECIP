using System;
using System.Linq;
using KZONE.ConstantParameter;
using KZONE.Entity;
using KZONE.Work;
using System.Reflection;
using System.Threading;
using KZONE.PLCAgent.PLC;
using KZONE.EntityManager;
using System.Collections.Generic;
using System.Text;
using EipTagLibrary;

namespace KZONE.Service
{
    public partial class EquipmentService : AbstractService
    {
        public void CPCEQDBCAlive(String aliveBit)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDBCAlive") as Trx;
                if (trx == null) throw new Exception("CAN'T FIND TRX L3_EQDEQAlive IN PLCFmt.xml!");

                trx[0][0][0].Value = aliveBit;
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                eqp.File.EqpAlive = aliveBit.Equals("1") ? eBitResult.ON : eBitResult.OFF;
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCBCAliveTimeout(Equipment eq, eBitResult bcAliveTimeOutbit)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_BCAliveTimeOut", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX BCAliveTimeOut IN PLCFmt.xml!", eq.Data.NODENO));

                trx[0][0][0].Value = ((int)bcAliveTimeOutbit).ToString();
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BC Alive TimeOut=[{2}]({3}).",
                        eq.Data.NODENO, trx.TrackKey, (int)bcAliveTimeOutbit,
                        bcAliveTimeOutbit.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void CPCEQDEQAlive(String aliveBit)
        {
            try
            {
                Equipment eqp = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                eqp.File.EqpAlive = aliveBit.Equals("1") ? eBitResult.ON : eBitResult.OFF;
                ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                if (eipTagAccess != null)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineHeartBeatSignal", eqp.File.EqpAlive == eBitResult.ON ? "true" : "false");
                }
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCCimMode(Equipment eq, Trx inputData)
        {
            try
            {
           
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "CIMMode", eq.File.CIMMode == eBitResult.ON ? "true" : "false");

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] CIM_MODE=[{2}]({3}).",
                        inputData.Metadata.NodeNo, inputData.TrackKey, (int)eq.File.CIMMode, eq.File.CIMMode.ToString()));

                Thread.Sleep(100);
                if (eq.File.CIMMode == eBitResult.ON)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "DateTimeRequestBlock", "TouchPanelNumber", "1");

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "DateTimeRequest", "true");
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void CPCUpstreamInlineMode(Equipment eq, Trx inputData)
        {
            try
            {
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "UpstreamInlineMode", eq.File.UpInlineMode == eBitResult.ON ? "true" : "false");

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Upstream Inline Mode=[{2}]({3}).",
                        inputData.Metadata.NodeNo, inputData.TrackKey, (int)eq.File.UpInlineMode, eq.File.UpInlineMode.ToString()));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void CPCDownstreamInlineMode(Equipment eq, Trx inputData)
        {
            try
            {
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "DownstreamInlineMode", eq.File.DownInlineMode == eBitResult.ON ? "true" : "false");


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Down stream Inline Mode=[{2}]({3}).",
                        inputData.Metadata.NodeNo, inputData.TrackKey, (int)eq.File.DownInlineMode, eq.File.DownInlineMode.ToString()));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void CPCDateTimeCalibrationCommand(Equipment eq, string inputData, eBitResult result, string TrackKey)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent)
                    .GetTransactionFormat(string.Format("{0}_EQDDateTimeCalibrationCommand", eq.Data.NODENO)) as Trx;
                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EQDDateTimeCalibrationCommand IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCDateTimeCalibrationCommandTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = TrackKey;
                    SendToPLC(trx);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] Date Time Calibration Command.",
                            eq.Data.NODENO, TrackKey));

                    return;
                }
                string datetime = inputData;

                trx[0][0][0].Value = datetime.Substring(0, 4);
                trx[0][0][1].Value = datetime.Substring(4, 2);
                trx[0][0][2].Value = datetime.Substring(6, 2);
                trx[0][0][3].Value = datetime.Substring(8, 2);
                trx[0][0][4].Value = datetime.Substring(10, 2);
                trx[0][0][5].Value = datetime.Substring(12, 2);



                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCDateTimeCalibrationCommandTimeoutAction), TrackKey);

                trx[0][1][0].Value = "1";
                trx.TrackKey = TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Date Time Calibration Command =[{2}].",
                        eqpNo, TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCCIMModeChangeCommand(Equipment eq, string TrackKey, eBitResult result, string cIMModeCommand)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent)
                    .GetTransactionFormat(string.Format("{0}_EQDCIMModeChangeCommand", eq.Data.NODENO)) as Trx;
                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EQDCIMModeChangeCommand IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCCIMModeChangeCommandTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] CIM Mode Change Command.",
                            eq.Data.NODENO, TrackKey));

                    return;
                }

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCCIMModeChangeCommandTimeoutAction), TrackKey);

                trx[0][0][0].Value = cIMModeCommand;
                trx[0][1][0].Value = "1";
                trx.TrackKey = TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] CIM Mode Change Command =[{2}].",
                        trx.Metadata.NodeNo, TrackKey, cIMModeCommand));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCEquipmentRunModeSetCommand(Equipment eq, string TrackKey, string runmode, eBitResult result)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent)
                    .GetTransactionFormat(string.Format("{0}_EQDEquipmentRunModeSetCommand", eq.Data.NODENO)) as Trx;
                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EQDEquipmentRunModeSetCommand IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCEquipmentRunModeSetCommandTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] Equipment Run Mode Set Command.",
                            eq.Data.NODENO, TrackKey));

                    return;
                }

              
                        trx[0][0][0].Value = runmode;
              

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCEquipmentRunModeSetCommandTimeoutAction), TrackKey);

                trx[0][1][0].Value = "1";
                trx.TrackKey = TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Equipment Run Mode Set Command =[{2}].",
                       eqpNo, TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCStopBitCommandReply(eBitResult res, Equipment eq)
        {
            Trx trx = GetTrxValues("L3_StopBitcommandReply");
            if (trx == null)
                throw new Exception(string.Format(
                    "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_GlassNGMarkReport IN PLCFmt.xml!", eq.Data.NODENO,
                    eq.Data.NODENO));

            trx[0][0][0].Value = ((int)res).ToString();
            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(trx);

            //string timerID = "L3_StopBitcommandReplyTimeout";
            //if(Timermanager.IsAliveTimer(timerID))
            //{
            //    Timermanager.TerminateTimer(timerID);
            //}
            //Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(CPCStopBitCommandReplyTimeoutAction), UtilityMethod.GetAgentTrackKey());

            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                   string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Stop Bit Reply BIT=[{2}]", eq.Data.NODENO, trx.TrackKey, res));

        }

        public void CPCEquipmentStatusChangeReport(Equipment eq, string TrackKey, eBitResult result)
        {

            try
            {
                Block MachineStatusChangeReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_Status_03_01_00", "MachineStatusChangeReportBlock");

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCEquipmentStatusChangeReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Equipment Status Change Report.",
                            eq.Data.NODENO, TrackKey));
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineStatusChangeReport", false);
                    return;
                }
                if (eq.File.Status == eEQPStatus.Run)
                {
                    MachineStatusChangeReportBlock["MachineStatus"].Value = 5;
                    MachineStatusChangeReportBlock["MachinestatusReasonCode"].Value = 301;
                }

                else if (eq.File.Status == eEQPStatus.Down)
                {
                    MachineStatusChangeReportBlock["MachineStatus"].Value = 2;
                    MachineStatusChangeReportBlock["MachinestatusReasonCode"].Value = 914;

                }
                else if (eq.File.Status == eEQPStatus.Idle)
                {
                    MachineStatusChangeReportBlock["MachineStatus"].Value = 4;
                    MachineStatusChangeReportBlock["MachinestatusReasonCode"].Value = 401;
                }
                else if (eq.File.Status == eEQPStatus.Stop)
                {
                    MachineStatusChangeReportBlock["MachineStatus"].Value = 3;
                    MachineStatusChangeReportBlock["MachinestatusReasonCode"].Value = 201;
                }
                else if (eq.File.Status == eEQPStatus.Initial)
                {
                    MachineStatusChangeReportBlock["MachineStatus"].Value = 1;
                    MachineStatusChangeReportBlock["MachinestatusReasonCode"].Value = 105;
                }
                else
                {
                    MachineStatusChangeReportBlock["MachineStatus"].Value = 1;
                    MachineStatusChangeReportBlock["MachinestatusReasonCode"].Value = 0;
                }

                foreach (Unit unit in ObjectManager.UnitManager.GetUnits())
                {
                    MachineStatusChangeReportBlock[$"Unitstatus#{unit.Data.UNITNO}"].Value = GetUnitStatus(unit);
                }
            
                    eipTagAccess.WriteBlockValues("SD_EQToCIM_Status_03_01_00", MachineStatusChangeReportBlock);
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCEquipmentStatusChangeReportTimeoutAction), TrackKey);
               // Thread.Sleep(3000);
                eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "MachineStatusChangeReport", true);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Status Change Report Status=[{2}].",eqpNo, TrackKey, eq.File.Status));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private int GetUnitStatus(Unit unit)
        {
            int status = 0;
            if (unit.File.Status == eEQPStatus.Run)
            {
                status = 5;
            }
            else if (unit.File.Status == eEQPStatus.Down)
            {
                status = 2;
            }
            else if (unit.File.Status == eEQPStatus.Idle)
            {
                status = 4;
            }
            else if (unit.File.Status == eEQPStatus.Stop)
            {
                status = 3;
            }
            else if (unit.File.Status == eEQPStatus.Initial)
            {
                status = 1;
            }
            else
            {
                status = 1;
            }
            return status;
        }

        public void CPCOperatorLoginLogoutReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCOperatorLoginLogoutReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                  
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Operator Login Logout Report.",
                            eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmss")));
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "OperatorLoginReport", false);
                    return;
                }
                //OperatorID
                //TouchPanelNumber
                //ReportOption
                //LoginLogoutTimeYear
                //LoginLogoutTimeMonth
                //LoginLogoutTimeDay
                //LoginLogoutTimeHour
                //LoginLogoutTimeMinute
                //LoginLogoutTimeSecond

                Block CIMMessageSetCommandBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_Status_03_01_00", "OperatorLoginReportBlock");

                CIMMessageSetCommandBlock["OperatorID"].Value = inputData[0][0][0].Value;
                CIMMessageSetCommandBlock["TouchPanelNumber"].Value = inputData[0][0][1].Value;
                CIMMessageSetCommandBlock["ReportOption"].Value = inputData[0][0][2].Value;
                CIMMessageSetCommandBlock["LoginLogoutTimeYear"].Value = DateTime.Now.Year.ToString();
                CIMMessageSetCommandBlock["LoginLogoutTimeMonth"].Value = DateTime.Now.Month.ToString();
                CIMMessageSetCommandBlock["LoginLogoutTimeDay"].Value = DateTime.Now.Day.ToString();
                CIMMessageSetCommandBlock["LoginLogoutTimeHour"].Value = DateTime.Now.Hour.ToString();
                CIMMessageSetCommandBlock["LoginLogoutTimeMinute"].Value = DateTime.Now.Minute.ToString();
                CIMMessageSetCommandBlock["LoginLogoutTimeSecond"].Value = DateTime.Now.Second.ToString();

                eipTagAccess.WriteBlockValues("SD_EQToCIM_Status_03_01_00", CIMMessageSetCommandBlock);

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(CPCOperatorLoginLogoutReportTimeoutAction), inputData.TrackKey);

                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "OperatorLoginReport", true);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_Status_03_01_00", "MachineStatus", "OperatorLoginReport", false);
                }


                ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Operator Login Logout Report.", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCJobDataRequest(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
// RequestJobID
//LotSequenceNumber
//SlotSequenceNumber
//RequestOption


                Block JobDataRequestBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_JobEvent_03_01_00", "JobDataRequestBlock");

              
                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCJobDataRequestTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataRequest", "false");

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Job Data Request.",
                            eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmss")));

                    return;
                }
                JobDataRequestBlock["LotSequenceNumber"].Value = inputData[0][0]["CassetteSequenceNo"].Value;
                JobDataRequestBlock["SlotSequenceNumber"].Value= inputData[0][0]["JobSequenceNo"].Value;
                JobDataRequestBlock["RequestJobID"].Value = inputData[0][0]["GlassID"].Value;
                JobDataRequestBlock["RequestOption"].Value = inputData[0][0]["UsedFlag"].Value == "1" ? "2" : "1";
                eipTagAccess.WriteBlockValues("SD_EQToCIM_JobEvent_03_01_00", JobDataRequestBlock);

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(CPCJobDataRequestTimeoutAction), inputData.TrackKey);

                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataRequest", "true");
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_JobEvent_03_01_00", "JobEvent", "JobDataRequest", "false");
                }
               

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Data Request =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        object ob = new object();
        public void CPCAlarmReportBC()
        {
            alarmReport = true;

            alarmChannel.Add(1, true);
            alarmChannel.Add(2, true);
            alarmChannel.Add(3, true);
            alarmChannel.Add(4, true);
            alarmChannel.Add(5, true);



            while (true)
            {
                try
                {
                    Thread.Sleep(50);
                    //string pathNo = "1";
                    lock (ob)
                    {
                        if (alarmChannel.ContainsValue(true))
                        {

                            alarmReport = true;

                            //if (alarmDic.Count == 0)
                            //{
                            //    continue;
                            //}
                           // int channelIndex = alarmChannel.Where(d => d.Value == true).First().Key;
                          //  alarmChannel[channelIndex] = false;
                        }
                        else
                        {
                            alarmReport = false;
                        }

                        if (_isRuning && alarmReport && alarmDic.Count > 0)
                        {
                            alarmReport = false;
                            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                            int channelIndex = alarmChannel.Where(d => d.Value == true).First().Key;
                            alarmChannel[channelIndex] = false;

                            Block CIMMessageSetCommandBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_AlarmEvent_03_01_00", $"AlarmReport#{channelIndex}Block");


                            AlarmEntityData alarm = alarmDic.First().Key;
                            string res = alarmDic.First().Value;
                            alarmDic.Remove(alarm);


                            CIMMessageSetCommandBlock["AlarmID"].Value = alarm.BCALARMID;
                            CIMMessageSetCommandBlock["AlarmType"].Value = alarm.ALARMLEVEL == "2" ? "1" : "0";
                            CIMMessageSetCommandBlock["AlarmStatus"].Value = res;
                            CIMMessageSetCommandBlock["AlarmUnitNumber"].Value = alarm.UNITNO;
                            CIMMessageSetCommandBlock["AlarmCode"].Value = alarm.ALARMCODE;
                            CIMMessageSetCommandBlock["AlarmText"].Value = alarm.ALARMTEXT;


                            eipTagAccess.WriteBlockValues("SD_EQToCIM_AlarmEvent_03_01_00", CIMMessageSetCommandBlock);

                            eipTagAccess.WriteItemValue("SD_EQToCIM_AlarmEvent_03_01_00", "AlarmEvent", $"AlarmReport#{channelIndex}", "true");

                            

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Alarm Report To BC", eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmss")));

                            string timerID = string.Format("{0}_{1}_{2}", eq.Data.NODENO, channelIndex, "AlarmReport");
                            if (Timermanager.IsAliveTimer(timerID))
                            {
                                Timermanager.TerminateTimer(timerID);
                            }
                            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T3].GetInteger(), new System.Timers.ElapsedEventHandler(AlarmStatusChangeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());


                            Thread.Sleep(T3);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                    continue;
                }
            }

        }

        public void CPCAutoRecipeChangeModeReport(Equipment eq, Trx inputData)
        {
            //Trx trx = GetTrxValues("L3_RecipeAutoChangeMode");
            //if (trx == null)
            //{
            //    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_[{0}] TRX L3_RecipeAutoChangeMode IN PLCFmt.xml", eq.Data.NODENO));
            //}
            //trx[0][0][0].Value = inputData[0][0][0].Value;
            //trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            //SendToPLC(trx);
            eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "AutoRecipeChangeModeReportBlock", "AutoRecipeChangeMode", inputData[0][0][0].Value == "1" ? "1" : "2");

            eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "AutoRecipeChangeModeReport",true);

            string  timerID = string.Format("{0}_{1}", eq.Data.NODENO, "AutoRecipeChangeModeReport");
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(), new System.Timers.ElapsedEventHandler(CPCAutoRecipeChangeModeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());


            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Auto Recipe Change Mode = [{2}]", eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmss"),  inputData[0][0][0].Value == "1" ? "Enable" : "Disable"));
        }

        private void CPCAutoRecipeChangeModeReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');



                eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "AutoRecipeChangeModeReport", false);

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Auto Recipe Change Mode Report  T1 TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCCurrentRecipeIDReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {

                if (result == eBitResult.OFF)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "CurrentRecipeNumberChangeReport", false);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Current Recipe ID Report.",
                            eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmss")));

                    return;
                }

                Block CurrentRecipeNumberChangeReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_RecipeManagement_03_01_00", $"CurrentRecipeNumberChangeReportBlock");

                CurrentRecipeNumberChangeReportBlock[0].Value = inputData[0][0][1].Value;

                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                       ObjectManager.RecipeManager.ReloadRecipeByNo();

                if (recipeDic.Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(inputData[0][0][1].Value))
                {
                    CurrentRecipeNumberChangeReportBlock[1].Value = recipeDic[eq.Data.LINEID][inputData[0][0][1].Value].VERSIONNO.Substring(0, 4);
                    CurrentRecipeNumberChangeReportBlock[2].Value = recipeDic[eq.Data.LINEID][inputData[0][0][1].Value].VERSIONNO.Substring(4, 2);
                    CurrentRecipeNumberChangeReportBlock[3].Value = recipeDic[eq.Data.LINEID][inputData[0][0][1].Value].VERSIONNO.Substring(6, 2);
                    CurrentRecipeNumberChangeReportBlock[4].Value = recipeDic[eq.Data.LINEID][inputData[0][0][1].Value].VERSIONNO.Substring(8, 2);
                    CurrentRecipeNumberChangeReportBlock[5].Value = recipeDic[eq.Data.LINEID][inputData[0][0][1].Value].VERSIONNO.Substring(10, 2);
                    CurrentRecipeNumberChangeReportBlock[6].Value = recipeDic[eq.Data.LINEID][inputData[0][0][1].Value].VERSIONNO.Substring(12, 2);
                }
                else { 
                
                }
                eipTagAccess.WriteBlockValues("SD_EQToCIM_RecipeManagement_03_01_00", CurrentRecipeNumberChangeReportBlock);

                    string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCCurrentRecipeIDReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(CPCCurrentRecipeIDReportTimeoutAction), inputData.TrackKey);

                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "CurrentRecipeNumberChangeReport", true);
                }

               
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
               string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Current Recipe ID Report Recipe ID[{2}] Recipe No[{3}].", inputData.Metadata.NodeNo, inputData.TrackKey, inputData[0][0][0].Value, inputData[0][0][1].Value));

             
               



            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void CPCRecipeIDModifyReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                //Trx trx = GetTrxValues(string.Format("{0}_RecipeNumberModifyReport", eq.Data.NODENO));

                //if (trx == null)
                //    throw new Exception(string.Format(
                //        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_RecipeNumberModifyReport IN PLCFmt.xml!", eq.Data.NODENO,
                //        eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCRecipeIDModifyReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "RecipeChangeReport", false);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Recipe Number Modify Report.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }

                Block RecipeChangeReportBlock = eipTagAccess.ReadBlockValues("SD_EQToCIM_RecipeManagement_03_01_00", $"RecipeChangeReportBlock");

// RecipeChangeType
//RecipeVersionTimeYear
//RecipeVersionTimeMonth
//RecipeVersionTimeDay
//RecipeVersionTimeHour
//RecipeVersionTimeMinute
//RecipeVersionTimeSecond
//OperatorID
//UnitNumber
//RecipeNumber

                RecipeChangeReportBlock["RecipeChangeType"].Value = inputData[0][0]["ModifyFlag"].Value.Trim();
                RecipeChangeReportBlock["RecipeVersionTimeYear"].Value = inputData[0][0][1].Value.Trim();
                RecipeChangeReportBlock["RecipeVersionTimeMonth"].Value = inputData[0][0][2].Value.Trim();
                RecipeChangeReportBlock["RecipeVersionTimeDay"].Value = inputData[0][0][3].Value.Trim();
                RecipeChangeReportBlock["RecipeVersionTimeHour"].Value = inputData[0][0][4].Value.Trim();
                RecipeChangeReportBlock["RecipeVersionTimeMinute"].Value = inputData[0][0][5].Value.Trim();
                RecipeChangeReportBlock["RecipeVersionTimeSecond"].Value = inputData[0][0][6].Value.Trim();
                RecipeChangeReportBlock["OperatorID"].Value = eq.File.OprationName;
                RecipeChangeReportBlock["UnitNumber"].Value = 3;
                RecipeChangeReportBlock["RecipeNumber"].Value = inputData[0][0]["RecipeNO"].Value.Trim();


                eipTagAccess.WriteBlockValues("SD_EQToCIM_RecipeManagement_03_01_00", RecipeChangeReportBlock);

                string recipeID = inputData[0][0]["RecipeID"].Value.Trim();
                string flag = inputData[0][0]["ModifyFlag"].Value.Trim();
                string day01 = inputData[0][0][1].Value.Trim().Substring(2, 2).PadLeft(2, '0');
                string day02 = inputData[0][0][2].Value.Trim().PadLeft(2, '0');
                string day03 = inputData[0][0][3].Value.Trim().PadLeft(2, '0');
                string day04 = inputData[0][0][4].Value.Trim().PadLeft(2, '0');
                string day05 = inputData[0][0][5].Value.Trim().PadLeft(2, '0');
                string day06 = inputData[0][0][6].Value.Trim().PadLeft(2, '0');
                string versioNo = day01 + day02 + day03 + day04 + day05 + day06;

               

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, T1,
                        new System.Timers.ElapsedEventHandler(CPCRecipeIDModifyReportTimeoutAction), inputData.TrackKey);

                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "RecipeChangeReport", true);
                }
                else
                {
                    eipTagAccess.WriteItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "RecipeEvent", "RecipeChangeReport", false);
                }
               

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recipe Number Modify Report =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void CPCUnitStoreReport()
        {
            StroeReport = true;

            stroeChannel.Add(1, true);
            //stroeChannel.Add(2, true);

            while (true)
            {

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                try
                {

                    Thread.Sleep(5000);

                    string pathNo = "1";
                    if (stroeChannel.ContainsValue(true))
                    {
                        StroeReport = true;

                        pathNo = stroeChannel.Where(d => d.Value == true).First().Key.ToString();
                    }
                    else
                    {
                        StroeReport = false;
                    }

                    if (eq.File.Status != eqOldStatus)

                    {
                        eqOldStatus = eq.File.Status;

                        CPCEquipmentStatusChangeReport(eq, DateTime.Now.ToString("yyyyMMddhhmmssfff"), eBitResult.ON);

                    }


                    if (_isRuning && StroeReport && eq.File.StoreJobs.Count > 0)
                    {

                        int positon = eq.File.StoreJobs.First().Key;
                        Job job = eq.File.StoreJobs[positon];

                        eq.File.StoreJobs.TryRemove(positon, out job);

                        //Trx trx = GetTrxValues(string.Format("{0}_StoredJobReport#{1}", eq.Data.NODENO, pathNo.PadLeft(2, '0')));
                        //// Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("_JobHoldEventReport") as Trx;
                        //if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_StoredJobReport#{1} IN PLCFmt.xml!", "L3", pathNo.PadLeft(2, '0')));


                        Block StoredJobReportBlock1 = eipTagAccess.ReadBlockValues("SD_EQToCIM_StoredEvent01_03_01_00", "StoredJobReport#1Block#1");

                        Block StoredJobReportBlock2 = eipTagAccess.ReadBlockValues("SD_EQToCIM_StoredEvent01_03_01_00", "StoredJobReport#1SubBlock#1");

                        string unitNo = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[positon].UnitNo;
                        //添加玻璃进入单元
                        ProcessFlow pf = new ProcessFlow();
                        pf.MachineName = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[positon].UnitNo;
                        pf.StartTime = DateTime.Now;
                        if (job.processFlow == null)
                        {
                            job.processFlow = new ProcessFlow();
                            job.processFlow.ExtendUnitProcessFlows.Add(pf);
                        }
                        else
                        {
                            job.processFlow.ExtendUnitProcessFlows.Add(pf);
                        }
                        ObjectManager.JobManager.AddJob(job);

                        StoredJobReportBlock1["JobID"].Value = job.JobID;
                        StoredJobReportBlock1["LotSequenceNumber"].Value = job.LotSequenceNumber;
                        StoredJobReportBlock1["SlotSequenceNumber"].Value = job.SlotSequenceNumber;

                        StoredJobReportBlock2["UnitorPort"].Value = "1";
                        StoredJobReportBlock2["UnitNumber"].Value = unitNo;
                        StoredJobReportBlock2["PortNo"].Value ="0";
                        StoredJobReportBlock2["SlotNumber"].Value = "1";

                        eipTagAccess.WriteBlockValues("SD_EQToCIM_StoredEvent01_03_01_00", StoredJobReportBlock1);
                        eipTagAccess.WriteBlockValues("SD_EQToCIM_StoredEvent01_03_01_00", StoredJobReportBlock2);



                        if (eq.File.CIMMode == eBitResult.ON)
                        {
                           eipTagAccess.WriteItemValue("SD_EQToCIM_StoredEvent01_03_01_00", "StoredJobEvent", "StoredJobReport#1", true);

                        }
                        else
                        {
                            eipTagAccess.WriteItemValue("SD_EQToCIM_StoredEvent01_03_01_00", "StoredJobEvent", "StoredJobReport#1", false);
                        }

                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Store.ToString(),
                                   eq.Data.NODEID,
                                   eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");

                        string timerID = string.Format("{0}_{1}_{2}", "L3", pathNo.PadLeft(2, '0'), CPCStoreGlassDataReportTimeout);

                        Timermanager.CreateTimer(timerID, false, T1,
                            new System.Timers.ElapsedEventHandler(CPCStoreGlassDataReportTimeoutAction), DateTime.Now.ToString("yyyyMMddHHmmss"));


                        stroeChannel[int.Parse(pathNo)] = false;


                        Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Store Glass Data Report CassetteSequenceNo[{2}] JobSequenceNo[{3}]", "L3", DateTime.Now.ToString("yyyyMMddHHmmss"), job.LotSequenceNumber, job.SlotSequenceNumber));

                        if (unitNo == "4")
                        {
                            ProcessStartJobReport(eq, job, eBitResult.ON, "1");
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

                    continue;
                }
            }

        }

        public void CPCUnitFetchReport()
        {
            FetchReport = true;

            fetchChannel.Add(1, true);
            //fetchChannel.Add(2, true);

            while (true)
            {
                try
                {
                    Thread.Sleep(20);

                    string pathNo = "1";
                    if (fetchChannel.ContainsValue(true))
                    {
                        FetchReport = true;

                        pathNo = "1";// fetchChannel.Where(d => d.Value == true).First().Key.ToString();
                    }
                    else
                    {
                        FetchReport = false;
                    }


                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                    if (_isRuning && FetchReport && eq.File.FetchJobs.Count > 0)
                    {

                        int positon = eq.File.FetchJobs.First().Key;

                        Job job = eq.File.FetchJobs[positon];

                        eq.File.FetchJobs.TryRemove(positon, out job);

                        //Trx trx = GetTrxValues(string.Format("{0}_FetchedOutJobReport#{1}", eq.Data.NODENO, pathNo.PadLeft(2, '0')));
                        //// Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("_JobHoldEventReport") as Trx;
                        //if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_FetchedOutJobReport#{1} IN PLCFmt.xml!", "L3", pathNo.PadLeft(2, '0')));

                        Block FetchedOutJobReport1 = eipTagAccess.ReadBlockValues("SD_EQToCIM_FetchedEvent01_03_01_00", "FetchedOutJobReport#1Block#1");

                        Block FetchedOutJobReport2 = eipTagAccess.ReadBlockValues("SD_EQToCIM_FetchedEvent01_03_01_00", "FetchedOutJobReport#1SubBlock#1");



                        string unitNo = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[positon].UnitNo;

                        //添加出单元时间
                        if (job.processFlow != null && job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == unitNo).ToList().Count > 0)
                        {
                            job.processFlow.ExtendUnitProcessFlows.Where(d => d.MachineName == unitNo).First().EndTime = DateTime.Now;
                        }
                        else
                        {
                            if (job.processFlow == null)
                            {
                                job.processFlow = new ProcessFlow();
                            }
                            ProcessFlow pf = new ProcessFlow();
                            pf.MachineName = unitNo;
                            pf.StartTime = DateTime.Now;
                            pf.EndTime = DateTime.Now.AddSeconds(60);
                            job.processFlow.ExtendUnitProcessFlows.Add(pf);

                            ObjectManager.JobManager.AddJob(job);

                        }

                        FetchedOutJobReport1["JobID"].Value = job.JobID;
                        FetchedOutJobReport1["LotSequenceNumber"].Value = job.LotSequenceNumber;
                        FetchedOutJobReport1["SlotSequenceNumber"].Value = job.SlotSequenceNumber;

                        FetchedOutJobReport2["UnitorPort"].Value = "1";
                        FetchedOutJobReport2["UnitNumber"].Value = unitNo;
                        FetchedOutJobReport2["PortNo"].Value = "0";
                        FetchedOutJobReport2["SlotNumber"].Value ="1";

                        eipTagAccess.WriteBlockValues("SD_EQToCIM_FetchedEvent01_03_01_00", FetchedOutJobReport1);
                        eipTagAccess.WriteBlockValues("SD_EQToCIM_FetchedEvent01_03_01_00", FetchedOutJobReport2);



                        if (eq.File.CIMMode == eBitResult.ON)
                        {
                            eipTagAccess.WriteItemValue("SD_EQToCIM_FetchedEvent01_03_01_00", "FetchedJobEvent", "FetchedOutJobReport#1", true);

                        }
                        else
                        {
                            eipTagAccess.WriteItemValue("SD_EQToCIM_FetchedEvent01_03_01_00", "FetchedJobEvent", "FetchedOutJobReport#1", false);
                        }




                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.FetchOut.ToString(),
                                  eq.Data.NODEID,
                                  eq.Data.NODENO, unitNo, "", "", "");

                        string timerID = string.Format("{0}_{1}_{2}", "L3", pathNo.PadLeft(2, '0'), CPCFetchGlassDataReportTimeout);

                        Timermanager.CreateTimer(timerID, false, T1,
                            new System.Timers.ElapsedEventHandler(CPCFetchGlassDataReportTimeoutAction), DateTime.Now.ToString("yyyyMMddHHmmss"));


                        fetchChannel[int.Parse(pathNo)] = false;


                        Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Fetch Glass Data Report CassetteSequenceNo[{2}] JobSequenceNo[{3}]", "L3", DateTime.Now.ToString("yyyyMMddHHmmss"), job.LotSequenceNumber, job.SlotSequenceNumber));

                        if (unitNo == "4")
                        {
                            ProcessEndJobReport(eq, job, eBitResult.ON, "1");
                        }

                      
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

                    continue;
                }
            }

        }


        public void CPCJobDataCheckModeReport(Equipment eq, Trx inputData)
        {
            Trx trx = GetTrxValues("L3_JobDataCheckModeBlock");
            if (trx == null)
            {
                throw new Exception(string.Format("CAN'T FIND EQUIPMENT_[{0}] TRX L3_JobDataCheckModeBlock IN PLCFmt.xml", eq.Data.NODENO));
            }
            trx[0][0][0].Value = inputData[0][0]["RecipeIDCheckMode"].Value;//Recipe ID Check
            trx[0][1][0].Value = inputData[0][0]["CstSlotNoCheckMode"].Value;//Cst && Slot No Check
            trx[0][2][0].Value = ((int)eq.File.GlassIDCheckMode).ToString();//Glass ID Check
            trx[0][3][0].Value = inputData[0][0]["GroupNoCheckMode"].Value;//Group No Check
            trx[0][4][0].Value = inputData[0][0]["JobDuplicateCheckMode"].Value; //Job Duplicate Check

            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(trx);

            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Job Data Check Mode Change Report RecipeIDCheckMode=[{2}] CstSlotNoCheckMode=[{3}] GlassIDCheckMode=[{4}] GroupNoCheckMode=[{5}] JobDuplicateCheckMode=[{6}]",
                eq.Data.NODENO, trx.TrackKey, trx[0][0][0].Value == "1" ? "Enable" : "Disable", trx[0][1][0].Value == "1" ? "Enable" : "Disable", trx[0][2][0].Value == "1" ? "Enable" : "Disable", trx[0][3][0].Value == "1" ? "Enable" : "Disable", trx[0][4][0].Value == "1" ? "Enable" : "Disable"));

            string cIMModeCommand = eipTagAccess.ReadItemValue("SD_EQToCIM_RecipeManagement_03_01_00", "AutoRecipeChangeModeReportBlock", "AutoRecipeChangeMode").ToString();
           
        }











        public void CPCLocalAlarmStatus(Equipment eq, Trx inputData)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_LocalAlarmStatus", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX LocalAlarmStatus IN PLCFmt.xml!", inputData.Metadata.NodeNo));

                trx[0][0][0].Value = ((int)eq.File.Alarm).ToString();
                trx.TrackKey = inputData.TrackKey;
                trx[0][0].OpDelayTimeMS = 500;

                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Local Alarm Status=[{2}]({3}).",
                        inputData.Metadata.NodeNo, inputData.TrackKey, (int)eq.File.Alarm, eq.File.Alarm.ToString()));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        public void CPCEquipmentJobCountChangeReport(Equipment eq, Trx inputData)
        {
            try
            {
                //Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_EquipmentJobCountChangeReport", eq.Data.NODENO)) as Trx;
                Trx trx1 = GetTrxValues("L3_ProductGlassCountReportData");
                Trx trx2 = GetTrxValues("L3_TotalGlassCountReportData");

                trx1[0][0][0].Value = inputData[0][0][1].Value;
                trx1.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx1);
                trx2[0][0][0].Value = inputData[0][0][0].Value;
                trx2.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx2);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Count Change Report ProductJobCount[{2}]TotalJobCount[{3}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, inputData[0][0][1].Value, inputData[0][0][0].Value));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        public void CPCHistoryFlowGlassCountReport(Equipment eq, Trx inputData)
        {
            try
            {
                //Trx trx1 = GetTrxValues("L3_HistoryGlassCountReportData");
                ////Trx trx2 = GetTrxValues("L3_ProductGlassCountReportData");
                ////Trx trx3 = GetTrxValues("L3_TotalGlassCountReportData");
                //int count = 0;
                //count = int.Parse(inputData[0][0][0].Value);
                //if (int.Parse(inputData[0][0][0].Value)>=65536)
                //{
                //    count = count % 65536;
                //}
                //trx1[0][0][0].Value = count.ToString();
                //trx1.TrackKey = UtilityMethod.GetAgentTrackKey();
                //SendToPLC(trx1);
                ////trx2[0][0][0].Value = count.ToString();
                ////trx2.TrackKey = UtilityMethod.GetAgentTrackKey();
                ////SendToPLC(trx2);
                ////trx3[0][0][0].Value = count.ToString();
                ////trx3.TrackKey = UtilityMethod.GetAgentTrackKey();
                ////SendToPLC(trx3);

                //LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                //    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] History Flow Glass Count = [{2}].",
                //        inputData.Metadata.NodeNo, inputData.TrackKey, trx1[0][0][0].Value));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
     
        public void CPCEquipmentRunModeChangeReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_UnitModeChangeReport", eq.Data.NODENO));


                //  Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_EquipmentRunModeChangeReport", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_UnitModeChangeReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCEquipmentRunModeChangeReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Equipment Run Mode Change Report.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }

                for (int i = 0; i < inputData[0].Events.AllValues.Length; i++)
                {
                    for (int j = 0; j < inputData[0][i].Items.AllValues.Length; j++)
                    {
                        trx[0][i][j].Value = inputData[0][i][j].Value.Trim();
                    }

                }
                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCEquipmentRunModeChangeReportTimeoutAction), inputData.TrackKey);

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][1][0].Value = "0";
                }
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Run Mode Change Report =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

   
        public void CPCSoftwareVersionChangeReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_SoftwareVersionChangeReport", eq.Data.NODENO));


                //  Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_OperatorLoginLogoutReport", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_SoftwareVersionChangeReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCSoftwareVersionChangeReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Software Version Change Report.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }

                trx[0][0][0].Value = inputData[0][0][0].Value.Trim().PadLeft(4, '0');
                trx[0][0][1].Value = inputData[0][0][1].Value.Trim().PadLeft(2, '0');
                trx[0][0][2].Value = inputData[0][0][2].Value.Trim().PadLeft(2, '0');
                trx[0][0][3].Value = inputData[0][0][3].Value.Trim().PadLeft(2, '0');
                trx[0][0][4].Value = inputData[0][0][4].Value.Trim().PadLeft(2, '0');
                trx[0][0][5].Value = inputData[0][0][5].Value.Trim().PadLeft(2, '0');



                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCSoftwareVersionChangeReportTimeoutAction), inputData.TrackKey);

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][1][0].Value = "0";
                }
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Software Version Change Report.", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }



 
   
        public void CPCRecipeStateChangeCommand(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent)
                    .GetTransactionFormat(string.Format("{0}_EQDRecipeStateChangeCommand", eq.Data.NODENO)) as Trx;
                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EQDRecipeStateChangeCommand IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCRecipeStateChangeCommandTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                trx.ClearTrxWith0();

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] Recipe State Change Command.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }


                trx[0][0][0].Value = inputData[0][0]["RecipeType"].Value;
                trx[0][0][1].Value = inputData[0][0]["RecipeNo"].Value;
                trx[0][0][2].Value = inputData[0][0]["RecipeID"].Value;
                trx[0][0][3].Value = inputData[0][0]["RecipeState"].Value;




                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCRecipeStateChangeCommandTimeoutAction), inputData.TrackKey);

                trx[0][1][0].Value = "1";
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Recipe State Change Command =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

 
        public void CPCEquipmentReasonCodeChangeReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues((string.Format("{0}_EquipmentReasonCodeChangeReport", eq.Data.NODENO)));

                //   Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_EquipmentStatusChangeReport", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EquipmentReasonCodeChangeReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCEquipmentReasonCodeChangeReportTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][1][0].Value = "0";
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Equipment Reason Code Change Report.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }

                trx[0][0][1].Value = ((int)eq.File.Status).ToString();
                //设备初始化
                if (eq.File.Status == eEQPStatus.Initial)
                {
                    trx[0][0][0].Value = "1500";
                }//设备停机
                else if (eq.File.Status == eEQPStatus.Down)
                {     // 查看子单元ReasonCode
                    if (eq.File.ReasonCode.Count > 0)
                    {
                        if (eq.File.ReasonCode.ContainsKey(1))
                        {
                            trx[0][0][0].Value = "2400";
                        }
                        else if (eq.File.ReasonCode.ContainsKey(2))
                        {
                            trx[0][0][0].Value = "2100";
                        }
                        else if (eq.File.ReasonCode.ContainsKey(3))
                        {
                            trx[0][0][0].Value = "2000";
                        }
                        else if (eq.File.ReasonCode.ContainsKey(4))
                        {
                            trx[0][0][0].Value = "2100";
                        }
                        else
                        {
                            trx[0][0][0].Value = "2105";
                        }
                    }
                    else
                    {
                        trx[0][0][0].Value = "2900";
                    }
                }//设备IDLE
                else if (eq.File.Status == eEQPStatus.Idle)
                {
                    trx[0][0][0].Value = "4002";
                }//设备RUN 
                else if (eq.File.Status == eEQPStatus.Run)
                {
                    trx[0][0][0].Value = "5000";
                }  //设备其他状态
                else
                {
                    trx[0][0][0].Value = "5008";
                }



                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCEquipmentReasonCodeChangeReportTimeoutAction), inputData.TrackKey);

                trx[0][1][0].Value = "1";
                trx[0][1].OpDelayTimeMS = 500;

                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Reason Code Change Report =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCUnitStateChangeReport()
        {
            UnitStatusReport = true;
            while (true)
            {
                try
                {
                    Thread.Sleep(50);
                    if (_isRuning && UnitStatusReport && unitDic.Count > 0)
                    {
                        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                        int unitNo = unitDic.First().Key;
                        Unit unit = unitDic[unitNo];


                        unitDic.Remove(unitNo);

                        Trx trx = GetTrxValues(string.Format("{0}_UnitStatusChangeReport", eq.Data.NODENO));
                        // Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("_JobHoldEventReport") as Trx;
                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_UnitStatusChangeReport IN PLCFmt.xml!", "L3"));

                        trx[0][0][0].Value = ((int)unit.File.Status).ToString();
                        trx[0][0][1].Value = unitNo.ToString();

                        if (unit.File.Status == eEQPStatus.Initial)
                        {
                            trx[0][0][2].Value = "1500";
                        }//设备停机
                        else if (unit.File.Status == eEQPStatus.Down)
                        {     // 查看子单元ReasonCode
                            if (unit.File.ReasonCode.Count > 0)
                            {
                                if (unit.File.ReasonCode.ContainsKey(1))
                                {
                                    trx[0][0][2].Value = "2202";
                                }
                                else if (unit.File.ReasonCode.ContainsKey(2))
                                {
                                    trx[0][0][2].Value = "2203";
                                }
                                else if (unit.File.ReasonCode.ContainsKey(3))
                                {
                                    trx[0][0][2].Value = "2500";
                                }
                                else if (unit.File.ReasonCode.ContainsKey(4))
                                {
                                    trx[0][0][2].Value = "2204";
                                }
                                else
                                {
                                    trx[0][0][2].Value = "2105";
                                }
                            }
                            else
                            {
                                trx[0][0][2].Value = "1900";
                            }
                        }//设备IDLE
                        else if (unit.File.Status == eEQPStatus.Idle)
                        {
                            trx[0][0][2].Value = "4002";
                        }//设备RUN 
                        else if (unit.File.Status == eEQPStatus.Run)
                        {
                            trx[0][0][2].Value = "5000";
                        }  //设备其他状态
                        else
                        {
                            trx[0][0][2].Value = "5008";
                        }



                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                        if (eq.File.CIMMode == eBitResult.ON)
                        {
                            trx[0][1][0].Value = "1";
                            trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();

                        }
                        else
                        {
                            trx[0][1][0].Value = "0";
                        }

                        SendToPLC(trx);


                        string timerID = string.Format("{0}_{1}", "L3", CPCUnitStateChangeReportTimeout);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCUnitStateChangeReportTimeoutAction), trx.TrackKey);


                        UnitStatusReport = false;


                        Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Unit[{2}] State[{3}] Change Report ", "L3", trx.TrackKey, unitNo, unit.File.Status.ToString()));


                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                    unitDic.Clear();
                    continue;
                }
            }

        }

        public void CPCUnitReasonCodeReport()
        {
            UnitReasonCodeReport = true;
            while (true)
            {
                try
                {
                    Thread.Sleep(5);
                    if (_isRuning && UnitReasonCodeReport && unitReasonCodeDic.Count > 0)
                    {
                        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                        int unitNo = unitReasonCodeDic.First().Key;
                        Unit unit = unitReasonCodeDic[unitNo];

                        unitReasonCodeDic.Remove(unitNo);

                        Trx trx = GetTrxValues(string.Format("{0}_UnitReasonCodeChangeReport", eq.Data.NODENO));
                        // Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("_JobHoldEventReport") as Trx;
                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_UnitReasonCodeChangeReport IN PLCFmt.xml!", "L3"));

                        trx[0][0][2].Value = ((int)unit.File.Status).ToString();
                        trx[0][0][1].Value = unitNo.ToString();

                        if (unit.File.Status == eEQPStatus.Initial)
                        {
                            trx[0][0][0].Value = "1500";
                        }//设备停机
                        else if (unit.File.Status == eEQPStatus.Down)
                        {     // 查看子单元ReasonCode
                            if (unit.File.ReasonCode.Count > 0)
                            {
                                if (unit.File.ReasonCode.ContainsKey(1) && unit.File.ReasonCode[1] == "1")
                                {
                                    trx[0][0][0].Value = "2400";
                                }
                                else if (unit.File.ReasonCode.ContainsKey(2) && unit.File.ReasonCode[2] == "1")
                                {
                                    trx[0][0][0].Value = "2100";
                                }
                                else if (unit.File.ReasonCode.ContainsKey(3) && unit.File.ReasonCode[3] == "1")
                                {
                                    trx[0][0][0].Value = "2000";
                                }
                                else if (unit.File.ReasonCode.ContainsKey(4) && unit.File.ReasonCode[4] == "1")
                                {
                                    trx[0][0][0].Value = "2100";
                                }
                                else
                                {
                                    trx[0][0][0].Value = "2105";
                                }
                            }
                            else
                            {
                                trx[0][0][0].Value = "2900";
                            }
                        }//设备IDLE
                        else if (unit.File.Status == eEQPStatus.Idle)
                        {
                            trx[0][0][0].Value = "4002";
                        }//设备RUN 
                        else if (unit.File.Status == eEQPStatus.Run)
                        {
                            trx[0][0][0].Value = "5000";
                        }  //设备其他状态
                        else
                        {
                            trx[0][0][0].Value = "5008";
                        }



                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                        if (eq.File.CIMMode == eBitResult.ON)
                        {
                            trx[0][1][0].Value = "1";
                            trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();

                        }
                        else
                        {
                            trx[0][1][0].Value = "0";
                        }

                        SendToPLC(trx);



                        string timerID = string.Format("{0}_{1}", "L3", CPCUnitReasonCodeReportTimeout);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCUnitReasonCodeReportTimeoutAction), trx.TrackKey);


                        UnitReasonCodeReport = false;


                        Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Unit Reason Code[{2}] Change Report ", "L3", trx.TrackKey, trx[0][0][0].Value));


                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);
                    unitReasonCodeDic.Clear();
                    continue;
                }
            }

        }


        public void CPCUnitRecipeListReport()
        {

            while (true)
            {
                try
                {
                    Thread.Sleep(5);

                    if (_isRuning && RecipeReport)
                    {
                        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                        Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipeByNo();

                        Trx trx = GetTrxValues(string.Format("{0}_UnitRecipeListReport", eq.Data.NODENO));

                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_UnitRecipeListReport IN PLCFmt.xml!", "L3"));

                        int i = 1;
                        List<RecipeEntityData> recipelist = recipeDic[eq.Data.LINEID].Values.ToList();

                        while (i <= 1)
                        {
                            if (UnitListRecipeReport)
                            {
                                trx.ClearTrxWith0();

                                trx[0][0][0].Value = "1";
                                trx[0][0][1].Value = "1";
                                trx[0][0][2].Value = "1";
                                trx[0][0][3].Value = recipeDic[eq.Data.LINEID].Count.ToString();//所有Recipe组数
                                trx[0][0][4].Value = recipeDic[eq.Data.LINEID].Count.ToString();//所有Recipe组数
                                trx[0][0][5].Value = MessageSequenceNo;


                                for (int j = 0; j < recipelist.Count; j++)
                                {

                                    trx[0][1][j].Value = recipelist[j].RECIPENO;

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

                                string timerID = string.Format("{0}_{1}", "L3", UnitRecipeListReportTimeout);

                                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                                    new System.Timers.ElapsedEventHandler(CPCUnitRecipeListReportTimeoutAction), trx.TrackKey);


                                Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Unit Recipe List Report Recipe No[{2}] Recipe ID[{3}] Count[{4}] BIT ON ", "L3",
                                        trx.TrackKey, recipelist[i - 1].RECIPENO, recipelist[i - 1].RECIPEID, $"{i}/{recipelist.Count}"));

                                UnitListRecipeReport = false;


                                #region 注释掉防止启用
                                //IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipelist[i-1].FILENAME);
                                //if (paramter != null && paramter.Count != 0)
                                //{
                                //    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                                //    foreach (var data in paramter)
                                //    {
                                //        string[] splitc;
                                //        if (!string.IsNullOrEmpty(data) && data.Contains("="))
                                //        {
                                //            splitc = data.Trim().Split(new string[] { "=" }, StringSplitOptions.None);
                                //            if (splitc[0].Trim() != "Recipe_Verson")
                                //            {
                                //                if (!keyValuePairs.ContainsKey(splitc[0].Trim()))
                                //                {
                                //                    keyValuePairs.Add(splitc[0].Trim(), splitc[1].Trim());
                                //                }
                                //                if (splitc[0].Trim() == "Recipe_State")
                                //                {
                                //                    trx[0][1][splitc[0].Trim()].Value = splitc[1].Trim() == "Enable" ? "1" : "2";
                                //                }
                                //                else
                                //                {
                                //                    trx[0][1][splitc[0].Trim()].Value = splitc[1].Trim();
                                //                }
                                //            }
                                //        }
                                //    }
                                //    trx[0][1][3].Value = recipelist[i - 1].VERSIONNO.Substring(0, 4);
                                //    trx[0][1][4].Value = recipelist[i - 1].VERSIONNO.Substring(4, 2);
                                //    trx[0][1][5].Value = recipelist[i - 1].VERSIONNO.Substring(6, 2);
                                //    trx[0][1][6].Value = recipelist[i - 1].VERSIONNO.Substring(8, 2);
                                //    trx[0][1][7].Value = recipelist[i - 1].VERSIONNO.Substring(10, 2);
                                //    trx[0][1][8].Value = recipelist[i - 1].VERSIONNO.Substring(12, 2);


                                //    trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                                //    if (eq.File.CIMMode == eBitResult.ON)
                                //    {
                                //        trx[0][2][0].Value = "1";
                                //        trx[0][2].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();

                                //    }
                                //    else
                                //    {
                                //        trx[0][2][0].Value = "0";
                                //    }

                                //    SendToPLC(trx);

                                //    string timerID = string.Format("{0}_{1}", "L3", UnitRecipeListReportTimeout);

                                //    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                                //        new System.Timers.ElapsedEventHandler(CPCUnitRecipeListReportTimeoutAction), trx.TrackKey);


                                //    Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                //            string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Unit Recipe List Report Recipe No[{2}] Recipe ID[{3}] Count[{4}] BIT ON ", "L3",
                                //            trx.TrackKey, recipelist[i - 1].RECIPENO, recipelist[i - 1].RECIPEID,$"{i}/{recipelist.Count}"));

                                //    UnitListRecipeReport = false;

                                #endregion


                                i++;
                            }

                        }

                        RecipeReport = false;

                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);


                }
            }

        }

        public void CPCTackTimeDataReport()
        {

            while (true)
            {
                try
                {
                    Thread.Sleep(5);
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                    if (_isRuning && TackTimeReportFlag && eq.File.TackTimeJobs.Count > 0)
                    {
                        Job job = eq.File.TackTimeJobs.First();

                        TackTimeReportFlag = false;
                        TactTimeDataReport(eq, job, eBitResult.ON);

                        eq.File.TackTimeJobs.Remove(job);
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

                    continue;


                }
            }

        }


        //不使用此方法
        public void CPCProcessDataReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_ProcessDataReport", eq.Data.NODENO));

                //  Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_ProcessDataReport", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_ProcessDataReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCProcessDataReportTimeout);

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
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Process Data Report.",
                            eq.Data.NODENO, trx.TrackKey));

                    return;
                }

                for (int i = 0; i < inputData[0].Events.AllValues.Length; i++)
                {
                    for (int j = 0; j < inputData[0][i].Items.AllValues.Length; j++)
                    {
                        trx[0][i][j].Value = inputData[0][i][j].Value.Trim();
                    }

                }
                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCProcessDataReportTimeoutAction), inputData.TrackKey);

                    trx[0][2][0].Value = "1";
                    trx[0][2].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][2][0].Value = "0";
                }
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Process Data Report =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }



        }


 


        public void CPCTankChangeReport(Equipment eq, Trx inpuTData, int tankNo)
        {
            Trx trx = GetTrxValues("L3_TankChangeReport");
            if (trx == null)
            {
               // throw new Exception(string.Format("CAN'T FIND EQUIPMENT_[{0}] TRX L3_TankChangeReport IN PLCFmt.xml", eq.Data.NODENO));
            }
            trx[0][0][0].Value = tankNo.ToString();
            trx[0][0][1].Value = eq.File.TankUseMode[tankNo] == eEnableDisable.Enable ? "1" : "2";
            string timerID = "L3_TankChangeReportTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }
            if (eq.File.CIMMode == eBitResult.ON)
            {
                trx[0][1][0].Value = "1";
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCTankChangeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            else
            {
                trx[0][1][0].Value = "0";
            }
            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            trx[0][1].OpDelayTimeMS = 200;
            SendToPLC(trx);
            LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Tank Change Report BIT=[{2}]", eq.Data.NODENO, trx.TrackKey, (eBitResult)int.Parse(trx[0][0][0].Value)));
            return;
        }

        public void CPCProcessStopCommand(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent)
                    .GetTransactionFormat(string.Format("{0}_EQDProcessStopCommand", eq.Data.NODENO)) as Trx;
                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EQDProcessStopCommand IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));
                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Process Stop Command =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, result.ToString()));


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCForceCleanOutCommand(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                //Trx trx = GetServerAgent(eAgentName.PLCAgent)
                //    .GetTransactionFormat(string.Format("{0}_EQDForceCleanOutCommand", eq.Data.NODENO)) as Trx;
                Trx trx = GetTrxValues("L3_EQDEquipmentRunModeSetCommand");
                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EQDForceCleanOutCommand IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));


                if (result == eBitResult.OFF)
                {
                    trx[0][0][0].Value = "1";
                }
                else
                {
                    trx[0][0][0].Value = "2";
                }
                trx[0][1][0].Value = "1";
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Force Clean Out Command =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, result.ToString()));

                string timerID = "L3_EQDEquipmentRunModeSetCommandTimeout";
                if(Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(CPCForceCleanOutCommandTimeoutAction), UtilityMethod.GetAgentTrackKey());

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCTransferStopCommand(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent)
                    .GetTransactionFormat(string.Format("{0}_EQDTransferStopCommand", eq.Data.NODENO)) as Trx;
                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EQDTransferStopCommand IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Transfer Stop Command =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, result.ToString()));


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCGlassNGMarkReport(Equipment eq, Trx inputData, eBitResult bit, string strNGMarkBit)
        {
            Trx trx = GetTrxValues("L3_GlassNGMarkReport");
            if (trx == null)
                throw new Exception(string.Format(
                    "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_GlassNGMarkReport IN PLCFmt.xml!", eq.Data.NODENO,
                    eq.Data.NODENO));

            string timerID = "L3_GlassNGMarkReportTimeout";
            if (Timermanager.IsAliveTimer(timerID))
            {
                Timermanager.TerminateTimer(timerID);
            }

            if (bit == eBitResult.OFF)
            {
                trx[0][1][0].Value = "0";
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Glass NG Mark Report BIT=[{2}]", eq.Data.NODENO, inputData.TrackKey, bit));
                return;
            }
            trx[0][0][0].Value = inputData[0][0][0].Value;
            trx[0][0][1].Value = inputData[0][0][1].Value;
            trx[0][0][2].Value = strNGMarkBit;
            trx[0][0][3].Value = eq.File.OprationName;
            trx[0][1][0].Value = "1";
            trx.TrackKey = inputData.TrackKey;
            SendToPLC(trx);
            Timermanager.CreateTimer(timerID, false, T1,
                new System.Timers.ElapsedEventHandler(CPCGlassNGMarkReportTimeoutAction), UtilityMethod.GetAgentTrackKey());
        }

       
    }
}
