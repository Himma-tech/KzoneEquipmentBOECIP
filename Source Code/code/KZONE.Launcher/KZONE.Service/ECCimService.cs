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
using System.Data.SqlClient;

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
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDEQAlive") as Trx;
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

        public void CPCCimMode(Equipment eq, Trx inputData)
        {
            try
            {
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_EquipmentCIMMode", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX EquipmentCIMMode IN PLCFmt.xml!", inputData.Metadata.NodeNo));

                trx[0][0][0].Value = ((int)eq.File.CIMMode).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] CIM_MODE=[{2}]({3}).",
                        inputData.Metadata.NodeNo, inputData.TrackKey, (int)eq.File.CIMMode, eq.File.CIMMode.ToString()));

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
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_UpstreamInlineMode", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX UpstreamInlineMode IN PLCFmt.xml!", inputData.Metadata.NodeNo));

                trx[0][0][0].Value = ((int)eq.File.UpInlineMode).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

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
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_DownstreamInlineMode", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX DownstreamInlineMode IN PLCFmt.xml!", inputData.Metadata.NodeNo));

                trx[0][0][0].Value = ((int)eq.File.DownInlineMode).ToString();
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Down stream Inline Mode=[{2}]({3}).",
                        inputData.Metadata.NodeNo, inputData.TrackKey, (int)eq.File.DownInlineMode, eq.File.DownInlineMode.ToString()));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

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
                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_EquipmentJobCountChangeReport", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX EquipmentJobCountChangeReport IN PLCFmt.xml!", inputData.Metadata.NodeNo));
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CF")
                {
                    trx[0][0][1].Value = inputData[0][0][1].Value;
                }
                else
                {
                    trx[0][0][0].Value = inputData[0][0][1].Value;
                }

                //trx[0][0][1].Value = inputData[0][0][0].Value;
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Count Change Report CurrentJobCount[{2}]MAXJobCount[{3}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, inputData[0][0][1].Value, inputData[0][0][0].Value));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }
        public void CPCOperatorLoginLogoutReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_OperatorLoginRequest", eq.Data.NODENO));


                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_OperatorLoginRequest IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCOperatorLoginLogoutReportTimeout);

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
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Operator Login Logout Request.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }
                trx[0][0][0].Value = inputData[0][0][0].Value;
                trx[0][0][1].Value = inputData[0][0][1].Value;
                trx[0][0][2].Value = inputData[0][0][2].Value == "1" ? "1" : "2";

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCOperatorLoginLogoutReportTimeoutAction), inputData.TrackKey);

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
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Operator Login Logout Request[{2}] OperatorID=[{3}] TouchPanelNo=[{4}] RepotType[{5}][{6}]."
                    , inputData.Metadata.NodeNo, inputData.TrackKey, "ON", trx[0][0][0].Value, trx[0][0][1].Value, trx[0][0][2].Value, trx[0][0][2].Value == "1" ? "Login" : trx[0][0][2].Value == "2" ? "Logout" : ""));

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

                //for (int i = 0; i < inputData[0].Events.AllValues.Length; i++)
                //{
                //    for (int j = 0; j < inputData[0][i].Items.AllValues.Length; j++)
                //    {
                //        trx[0][i][j].Value = inputData[0][i][j].Value.Trim();
                //    }

                //}
                trx[0][0][0].Value = inputData[0][0][0].Value;
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

        public void CPCCurrentRecipeIDReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_CurrentRecipeChangeReport", eq.Data.NODENO));

                // Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_CurrentRecipeIDChangeReport", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX CurrentRecipeChangeReport IN PLCFmt.xml!", eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCCurrentRecipeIDReportTimeout);

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
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Current Recipe ID Report.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }

                trx[0][0][0].Value = "0";
                string currentMasterID = string.Empty;

                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipeByNo();
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (recipeDic[eq.Data.LINEID].Count > 0 && recipeDic[eq.Data.LINEID].ContainsKey(eq.File.CurrentRecipeNo.ToString()))
                {
                    RecipeEntityData recipeEntityData = recipeDic[eq.Data.LINEID][eq.File.CurrentRecipeNo.ToString()];
                    //自动流片情况下MasterRecipeID上报需要使用上游JobData中的MasterRecipeID，手动切换可以随意
                    if (eq.File.EquipmentOperationMode == eEQPOperationMode.AUTO)
                    {
                        Trx trxJobData = new Trx();
                        if (line.Data.FABTYPE == "CELL")
                        {
                            if (eq.File.UPActionMonitor[11] == "1" && eq.File.UPActionMonitor[3] == "0")
                            {
                                trxJobData = GetTrxValues("L3_OtherSendingGlassDataReport#05");
                            }
                            else if (eq.File.UPActionMonitor[11] == "0" && eq.File.UPActionMonitor[3] == "1")
                            {
                                trxJobData = GetTrxValues("L3_OtherSendingGlassDataReport#01");
                            }
                        }
                        else
                        {
                            trxJobData = GetTrxValues("L3_OtherSendingGlassDataReport#01");
                        }
                        if (trxJobData != null)
                        {
                            trx[0][0][1].Value = trxJobData[0][0]["Master_Recipe_ID"]?.Value;
                        }
                    }
                    else
                    {
                        trx[0][0][1].Value = recipeEntityData.RECIPENAME.Trim();
                    }
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        trx[0][0][2].Value = eq.File.CurrentRecipeNo.ToString();
                    }
                    else
                    {
                        trx[0][0][2].Value = eq.File.CurrentRecipeID.ToString();
                    }
                    trx[0][0][3].Value = string.IsNullOrEmpty(recipeEntityData?.OPERATORID) == true ? "0" : recipeEntityData.OPERATORID;
                }
                else
                {
                    //自动流片情况下MasterRecipeID上报需要使用上游JobData中的MasterRecipeID，手动切换可以随意
                    if (eq.File.EquipmentOperationMode == eEQPOperationMode.AUTO)
                    {
                        Trx trxJobData = new Trx();
                        if (line.Data.FABTYPE == "CELL")
                        {
                            if (eq.File.UPActionMonitor[11] == "1" && eq.File.UPActionMonitor[3] == "0")
                            {
                                trxJobData = GetTrxValues("L3_OtherSendingGlassDataReport#05");
                            }
                            else if (eq.File.UPActionMonitor[11] == "0" && eq.File.UPActionMonitor[3] == "1")
                            {
                                trxJobData = GetTrxValues("L3_OtherSendingGlassDataReport#01");
                            }
                        }
                        else
                        {
                            trxJobData = GetTrxValues("L3_OtherSendingGlassDataReport#01");
                        }
                        if (trxJobData != null)
                        {
                            trx[0][0][1].Value = trxJobData[0][0]["Master_Recipe_ID"]?.Value;
                        }
                    }
                    else
                    {
                        trx[0][0][1].Value = currentMasterID;
                    }
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        trx[0][0][2].Value = eq.File.CurrentRecipeNo.ToString();
                    }
                    else
                    {
                        trx[0][0][2].Value = eq.File.CurrentRecipeID.ToString();
                    }
                    trx[0][0][3].Value = "0";
                }

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
               string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] Current Recipe Change Report Recipe ID[{2} Recipe No[{3}] Recipe VersionNo[{4}]].", inputData.Metadata.NodeNo, inputData.TrackKey, trx[0][0][1].Value, trx[0][0][2].Value, trx[0][0][3].Value));



                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCCurrentRecipeIDReportTimeoutAction), inputData.TrackKey);

                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                }
                else
                {
                    trx[0][1][0].Value = "0";
                }
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);



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

        public void CPCRecipeIDModifyReport(Equipment eq, Trx inputData, eBitResult result)
        {
            try
            {
                Trx trx = GetTrxValues(string.Format("{0}_LocalRecipeModifiedReport", eq.Data.NODENO));

                if (trx == null)
                    throw new Exception(string.Format(
                        "CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_LocalRecipeModifiedReport IN PLCFmt.xml!", eq.Data.NODENO,
                        eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCRecipeIDModifyReportTimeout);

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
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Local Recipe Modified Report.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }
                string localRecipeID = string.Empty;
                string recipeNO = inputData[0][0]["RecipeNO"].Value.Trim();
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                {
                    localRecipeID = inputData[0][0]["RecipeNO"].Value.Trim();
                }
                else
                {
                    localRecipeID = inputData[0][0]["RecipeID"].Value.Trim();
                }

                string masterRecipeID = inputData[0][0]["RecipeName"].Value.Trim();
                string flag = inputData[0][0]["ModifyFlag"].Value.Trim();
                string day01 = inputData[0][0][1].Value.Trim().PadLeft(2, '0');
                string day02 = inputData[0][0][2].Value.Trim().PadLeft(2, '0');
                string day03 = inputData[0][0][3].Value.Trim().PadLeft(2, '0');
                string day04 = inputData[0][0][4].Value.Trim().PadLeft(2, '0');
                string day05 = inputData[0][0][5].Value.Trim().PadLeft(2, '0');
                string day06 = inputData[0][0][6].Value.Trim().PadLeft(2, '0');
                string versionNo = day01 + day02 + day03 + day04 + day05 + day06;

                trx.ClearTrxWith0();

                if (flag == "1")//Create
                {
                    trx[0][0][0].Value = masterRecipeID;
                    trx[0][0][1].Value = localRecipeID;
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        trx[0][0][2].Value = "0";
                    }
                    else
                    {
                        trx[0][0][2].Value = string.Empty;
                    }
                    trx[0][0][3].Value = "0";
                    trx[0][0][4].Value = flag;
                    trx[0][0][5].Value = "1";
                }
                else if (flag == "2")//Modify
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipeByNo();


                    RecipeEntityData recipeEntityData = recipeDic[eq.Data.LINEID][recipeNO];
                    //检查参数是否修改
                    bool recipeParametersChange = CheckIsRecipeParametersChange(eq, inputData, recipeEntityData);
                    bool recipeIDChange = false;

                    //检查ID是否修改
                    if (inputData[0][0]["RecipeID"].Value.Trim() != recipeEntityData.RECIPEID)
                    {
                        recipeIDChange = true;
                    }


                    //if (recipeDic[eq.Data.LINEID].ContainsKey(recipeNO))//包含此Recipe
                    //{
                    trx[0][0][0].Value = masterRecipeID;//Modify Recipe
                    trx[0][0][1].Value = localRecipeID;
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        trx[0][0][2].Value = recipeEntityData.RECIPENO;
                    }
                    else
                    {
                        trx[0][0][2].Value = recipeEntityData.RECIPEID;
                    }
                    trx[0][0][3].Value = "0";
                    if (recipeParametersChange)
                    {
                        trx[0][0][4].Value = "2";
                    }
                    else
                    {
                        if (recipeIDChange)
                        {
                            trx[0][0][4].Value = "4";
                        }
                        else
                        {
                            trx[0][0][4].Value = "2";
                        }
                    }
                    int operatorID = string.IsNullOrEmpty(recipeEntityData.OPERATORID) == true ? 0 : int.Parse(recipeEntityData.OPERATORID);
                    if (operatorID < 65535)
                    {
                        operatorID++;
                    }
                    else
                    {
                        operatorID = 1;
                    }
                    trx[0][0][5].Value = operatorID.ToString();

                    //}//不包含此Recipe
                    //else
                    //{
                    //    trx[0][0][0].Value = "2";//Modify Recipe
                    //    trx[0][0][1].Value = "0";

                    //    trx[0][0][2].Value = "1";
                    //    trx[0][0][3].Value = inputData[0][0]["RecipeNO"].Value.Trim();
                    //    trx[0][0][4].Value = "0";


                    //    trx[0][0][5].Value = inputData[0][0]["RecipeStatus"].Value.Trim() == "1" ? "1" : "0";
                    //    trx[0][0][6].Value = day01;
                    //    trx[0][0][7].Value = day02;
                    //    trx[0][0][8].Value = day03;
                    //    trx[0][0][9].Value = day04;
                    //    trx[0][0][10].Value = day05;
                    //    trx[0][0][11].Value = day06;
                    //    LogWarn(MethodInfo.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recipe Table Not Exsit Recipe =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, recipeNO));
                    //}
                }
                else if (flag == "3")//Delete
                {
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                       ObjectManager.RecipeManager.ReloadRecipeByNo();
                    RecipeEntityData recipeEntityData = recipeDic[eq.Data.LINEID][recipeNO];
                    if (recipeDic[eq.Data.LINEID].ContainsKey(recipeNO))//包含此Recipe
                    {
                        trx[0][0][0].Value = masterRecipeID;
                        trx[0][0][1].Value = localRecipeID;
                        if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                        {
                            trx[0][0][2].Value = "0";
                        }
                        else
                        {
                            trx[0][0][2].Value = string.Empty;
                        }
                        trx[0][0][3].Value = "0";
                        trx[0][0][4].Value = flag;
                        trx[0][0][5].Value = recipeEntityData.OPERATORID;

                    }//不包含此Recipe
                    else
                    {
                        LogWarn(MethodInfo.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recipe Table Not Exsit Recipe =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, recipeNO));
                    }
                }
                else
                {
                    LogWarn(MethodInfo.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Recipe Flag Not Exsit Flag =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, flag));
                }


                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCRecipeIDModifyReportTimeoutAction), inputData.TrackKey);

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
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Local Recipe Modified Report =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        public void CPCDateTimeCalibrationCommand(Equipment eq, Trx inputData, eBitResult result)
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
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] Date Time Calibration Command.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }
                string datetime = inputData[0][0][0].Value.Trim().PadLeft(4, '0') + inputData[0][0][1].Value.Trim().PadLeft(2, '0') + inputData[0][0][2].Value.Trim().PadLeft(2, '0') + inputData[0][0][3].Value.Trim().PadLeft(2, '0') + inputData[0][0][4].Value.Trim().PadLeft(2, '0') + inputData[0][0][5].Value.Trim().PadLeft(2, '0');

                trx[0][0][0].Value = datetime.Substring(0, 4);
                trx[0][0][1].Value = datetime.Substring(4, 2);
                trx[0][0][2].Value = datetime.Substring(6, 2);
                trx[0][0][3].Value = datetime.Substring(8, 2);
                trx[0][0][4].Value = datetime.Substring(10, 2);
                trx[0][0][5].Value = datetime.Substring(12, 2);



                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCDateTimeCalibrationCommandTimeoutAction), inputData.TrackKey);

                trx[0][1][0].Value = "1";
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Date Time Calibration Command =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCCIMModeChangeCommand(Equipment eq, Trx inputData, eBitResult result, string cIMModeCommand)
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
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] CIM Mode Change Command.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCCIMModeChangeCommandTimeoutAction), inputData.TrackKey);

                trx[0][0][0].Value = cIMModeCommand;
                trx[0][1][0].Value = "1";
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] CIM Mode Change Command =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, cIMModeCommand));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCEquipmentRunModeSetCommand(Equipment eq, Trx inputData, eBitResult result)
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
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] Equipment Run Mode Set Command.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }

                for (int i = 0; i < inputData[0].Events.AllValues.Length; i++)
                {
                    for (int j = 0; j < trx[0][i].Items.AllValues.Length; j++)
                    {
                        trx[0][i][j].Value = inputData[0][i][j].Value.Trim();
                    }

                }

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCEquipmentRunModeSetCommandTimeoutAction), inputData.TrackKey);

                trx[0][1][0].Value = "1";
                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Equipment Run Mode Set Command =[{2}].",
                        inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public void CPCEquipmentRunModeSetCommand(Equipment eq, string runMode, eBitResult result)
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
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] BIT=[OFF] Equipment Run Mode Set Command.",
                            eq.Data.NODENO, trx.TrackKey));

                    return;
                }

                trx[0][0][0].Value = runMode;
                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                   new System.Timers.ElapsedEventHandler(CPCEquipmentRunModeSetCommandTimeoutAction), trx.TrackKey);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] Equipment Run Mode Set Command =[{2}].",
                        trx.Metadata.NodeNo, trx.TrackKey, "ON"));

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
                Trx trx = GetTrxValues(string.Format("{0}_JobDataRequest", eq.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_JobDataRequest IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCJobDataRequestTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }

                if (result == eBitResult.OFF)
                {
                    // trx.ClearTrxWith0();
                    trx[0][2][0].Value = "0";
                    trx.TrackKey = inputData.TrackKey;
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Job Data Request.",
                            eq.Data.NODENO, trx.TrackKey));

                    return;
                }
                trx[0][0][0].Value = inputData[0][0]["UsedFlag"].Value == "1" ? "3" : "1";
                trx[0][0][1].Value = inputData[0][0]["GlassID"].Value;
                trx[0][0][2].Value = inputData[0][0]["CassetteSequenceNo"].Value;
                trx[0][0][3].Value = inputData[0][0]["JobSequenceNo"].Value;

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCJobDataRequestTimeoutAction), inputData.TrackKey);

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
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Job Data Request =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

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
                    string pathNo = "1";
                    if (alarmChannel.ContainsValue(true))
                    {
                        alarmReport = true;

                        pathNo = alarmChannel.Where(d => d.Value == true).First().Key.ToString();
                    }
                    else
                    {
                        alarmReport = false;
                    }

                    Thread.Sleep(50);
                    if (_isRuning && alarmReport && alarmDic.Count > 0)
                    {
                        //Trx trx01 = GetTrxValues("L3_LocalAlarmStatus");

                        //int e = alarmDic.Keys.Select(d => d.ALARMLEVEL == "2").Count();

                        //trx01[0][0][0].Value = alarmDic.Keys.Select(d =>d.ALARMLEVEL == "2").Count() > 0 ? "1" : "0";
                        //SendToPLC(trx01);



                        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                        AlarmEntityData alarm = alarmDic.First().Key;
                        string alarmStatus = alarmDic.First().Value;

                        Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("L3_AlarmStatusChangeReport#{0}", pathNo.PadLeft(2, '0'))) as Trx;
                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_AlarmStatusChangeReport#{0} IN PLCFmt.xml!", "L3", pathNo.PadLeft(2, '0')));

                        trx[0][0][0].Value = alarmStatus;
                        trx[0][0][1].Value = alarm.UNITNO;
                        trx[0][0][2].Value = alarm.BCALARMID.ToString();
                        trx[0][0][3].Value = alarm.ALARMCODE;//alarm.ALARMCODE;//"0000000000000010";//Convert.ToString(int.Parse(alarm.ALARMCODE),2);
                        trx[0][0][4].Value = alarm.ALARMLEVEL;
                        trx[0][0][5].Value = alarm.ALARMTEXT;
                        trx[0][0][6].Value = alarm.ALARMTYPECODE.ToString();

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

                        alarmDic.Remove(alarm);

                        string timerID = string.Format("{0}_{1}_{2}", "L3", pathNo, AlarmStatusChangeReportTimeout);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(AlarmStatusChangeReportTimeoutAction), trx.TrackKey);


                        alarmChannel[int.Parse(pathNo)] = false;

                        if (alarmStatus == "1")
                        {
                            Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  Alarm Report#{2} Set AlarmID[{3}]  AlarmText[{4}] ", "L3", trx.TrackKey, pathNo, alarm.ALARMID, alarm.ALARMTEXT));
                        }
                        if (alarmStatus == "0")
                        {
                            Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}]  Alarm Report#{2} Clear AlarmID[{3}]  AlarmText[{4}] ", "L3", trx.TrackKey, pathNo, alarm.ALARMID, alarm.ALARMTEXT));
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

        public void CPCEquipmentStatusChangeReport(Equipment eq, Trx inputData, eBitResult result)
        {

            try
            {
                Trx trx = GetTrxValues((string.Format("{0}_EquipmentStatusChangeReport", eq.Data.NODENO)));

                //   Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_EquipmentStatusChangeReport", eq.Data.NODENO)) as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EquipmentStatusChangeReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCEquipmentStatusChangeReportTimeout);

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
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Equipment Status Change Report.",
                            eq.Data.NODENO, inputData.TrackKey));

                    return;
                }
                trx[0][0][0].Value = ((int)eq.File.Status).ToString();
                trx[0][0][2].Value = "0";//ObjectManager.UnitManager.GetUnits().Count.ToString();

                foreach (Unit u in ObjectManager.UnitManager.GetUnits())
                {
                    trx[0][0][u.Data.UNITNO * 2 + 1].Value = ((int)u.File.Status).ToString();
                    if (u.File.Status == eEQPStatus.Initial)
                    {
                        trx[0][0][u.Data.UNITNO * 2 + 2].Value = "14001";
                    }
                    else if (u.File.Status == eEQPStatus.Down)
                    {
                        trx[0][0][u.Data.UNITNO * 2 + 2].Value = "23104";
                    }
                    else if (u.File.Status == eEQPStatus.Idle)
                    {
                        trx[0][0][u.Data.UNITNO * 2 + 2].Value = "11001";
                    }
                    else if (u.File.Status == eEQPStatus.Run)
                    {
                        trx[0][0][u.Data.UNITNO * 2 + 2].Value = "10001";
                    }
                    else if (u.File.Status == eEQPStatus.Pause)
                    {
                        trx[0][0][u.Data.UNITNO * 2 + 2].Value = "12001";
                    }
                    else
                    {
                        trx[0][0][u.Data.UNITNO * 2 + 2].Value = "0";
                    }
                }
                //设备初始化
                if (eq.File.Status == eEQPStatus.Initial)
                {
                    trx[0][0][1].Value = "14001";
                }//设备停机
                else if (eq.File.Status == eEQPStatus.Down)
                {     // 查看子单元ReasonCode
                    //if (eq.File.ReasonCode.Count > 0)
                    //{
                    //    if (eq.File.ReasonCode.ContainsKey(1))
                    //    {
                    //        trx[0][0]["ReasonCode"].Value = "2400";
                    //    }
                    //    else if (eq.File.ReasonCode.ContainsKey(2))
                    //    {
                    //        trx[0][0]["ReasonCode"].Value = "2100";
                    //    }
                    //    else if (eq.File.ReasonCode.ContainsKey(3))
                    //    {
                    //        trx[0][0]["ReasonCode"].Value = "2000";
                    //    }
                    //    else if (eq.File.ReasonCode.ContainsKey(4))
                    //    {
                    //        trx[0][0]["ReasonCode"].Value = "2100";
                    //    }
                    //    else
                    //    {
                    //        trx[0][0]["ReasonCode"].Value = "2105";
                    //    }
                    //}
                    //else
                    //{
                    //    trx[0][0]["ReasonCode"].Value = "2900";
                    //}
                    trx[0][0][1].Value = "23104";
                }//设备IDLE
                else if (eq.File.Status == eEQPStatus.Idle)
                {
                    trx[0][0][1].Value = "11001";
                }//设备RUN 
                else if (eq.File.Status == eEQPStatus.Run)
                {
                    trx[0][0][1].Value = "10001";
                }
                else if (eq.File.Status == eEQPStatus.Pause)
                {
                    trx[0][0][1].Value = "12001";
                }
                //设备其他状态
                else
                {
                    trx[0][0][1].Value = "0";
                }

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCEquipmentStatusChangeReportTimeoutAction), inputData.TrackKey);

                trx[0][1][0].Value = "1";
                trx[0][1].OpDelayTimeMS = 500;

                trx.TrackKey = inputData.TrackKey;
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Equipment Status Change Report =[{2}].", inputData.Metadata.NodeNo, inputData.TrackKey, "ON"));

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCEquipmentStatusChangeReport()
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

                        Trx trx = GetTrxValues((string.Format("{0}_EquipmentStatusChangeReport", eq.Data.NODENO)));

                        //   Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_EquipmentStatusChangeReport", eq.Data.NODENO)) as Trx;
                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_EquipmentStatusChangeReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                        string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCEquipmentStatusChangeReportTimeout);

                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }

                        //trx[0][0][0].Value = ((int)eq.File.Status).ToString();
                        trx[0][0][2].Value = "0";//ObjectManager.UnitManager.GetUnits().Count.ToString();

                        foreach (Unit u in ObjectManager.UnitManager.GetUnits())
                        {
                            //trx[0][0][u.Data.UNITNO * 2 + 1].Value = ((int)u.File.Status).ToString();
                            if (u.File.Status == eEQPStatus.Initial)
                            {
                                trx[0][0][u.Data.UNITNO * 2 + 1].Value = "1";
                                trx[0][0][u.Data.UNITNO * 2 + 2].Value = "14001";
                            }
                            else if (u.File.Status == eEQPStatus.Down)
                            {
                                trx[0][0][u.Data.UNITNO * 2 + 1].Value = "3";
                                trx[0][0][u.Data.UNITNO * 2 + 2].Value = "33104";
                            }
                            else if (u.File.Status == eEQPStatus.Idle)
                            {
                                trx[0][0][u.Data.UNITNO * 2 + 1].Value = "6";
                                trx[0][0][u.Data.UNITNO * 2 + 2].Value = "61001";
                            }
                            else if (u.File.Status == eEQPStatus.Run)
                            {
                                trx[0][0][u.Data.UNITNO * 2 + 1].Value = "7";
                                trx[0][0][u.Data.UNITNO * 2 + 2].Value = "70001";
                            }
                            else if (u.File.Status == eEQPStatus.Pause)
                            {
                                trx[0][0][u.Data.UNITNO * 2 + 1].Value = "4";
                                trx[0][0][u.Data.UNITNO * 2 + 2].Value = "42001";
                            }
                            else
                            {
                                trx[0][0][u.Data.UNITNO * 2 + 1].Value = "0";
                                trx[0][0][u.Data.UNITNO * 2 + 2].Value = "0";
                            }
                        }
                        //设备初始化
                        if (eq.File.Status == eEQPStatus.Initial)
                        {
                            trx[0][0][0].Value = "1";
                            trx[0][0][1].Value = "14001";
                        }//设备停机
                        else if (eq.File.Status == eEQPStatus.Down)
                        {
                            trx[0][0][0].Value = "3";
                            trx[0][0][1].Value = "33104";
                        }//设备IDLE
                        else if (eq.File.Status == eEQPStatus.Idle)
                        {
                            trx[0][0][0].Value = "6";
                            trx[0][0][1].Value = "61001";
                        }//设备RUN 
                        else if (eq.File.Status == eEQPStatus.Run)
                        {
                            trx[0][0][0].Value = "7";
                            trx[0][0][1].Value = "70001";
                        }
                        else if (eq.File.Status == eEQPStatus.Pause)
                        {
                            trx[0][0][0].Value = "4";
                            trx[0][0][1].Value = "42001";
                        }
                        //设备其他状态
                        else
                        {
                            trx[0][0][0].Value = "0";
                            trx[0][0][1].Value = "0";
                        }
                        trx[0][1][0].Value = "1";
                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(trx);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCEquipmentStatusChangeReportTimeoutAction), trx.TrackKey);


                        UnitStatusReport = false;


                        Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Equipment Status[{4}] Unit[{2}] Status[{3}] Change Report ", "L3", trx.TrackKey, unitNo, unit.File.Status.ToString(), eq.File.Status));


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
                                trx[0][0][2].Value = "2900";
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
                    Thread.Sleep(50);
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

        public void CPCUnitStoreReport()
        {
            StroeReport = true;

            stroeChannel.Add(1, true);
            stroeChannel.Add(2, true);
            stroeChannel.Add(3, true);
            stroeChannel.Add(4, true);
            stroeChannel.Add(5, true);
            stroeChannel.Add(6, true);
            stroeChannel.Add(7, true);
            stroeChannel.Add(8, true);
            stroeChannel.Add(9, true);
            stroeChannel.Add(10, true);
            Thread.Sleep(2000);
            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
            while (true)
            {



                try
                {

                    Thread.Sleep(50);

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




                    if (_isRuning && StroeReport && eq.File.StoreJobs.Count > 0)
                    {

                        int positon = eq.File.StoreJobs.First().Key;
                        Job job = eq.File.StoreJobs[positon];

                        eq.File.StoreJobs.TryRemove(positon, out job);

                        Trx trx = GetTrxValues(string.Format("{0}_StoredJobReport#{1}", eq.Data.NODENO, pathNo.PadLeft(2, '0')));
                        // Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("_JobHoldEventReport") as Trx;
                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_StoredJobReport#{1} IN PLCFmt.xml!", "L3", pathNo.PadLeft(2, '0')));

                        string unitNo = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[positon].UnitNo;
                        //添加玻璃进入单元
                        ProcessFlow pf = new ProcessFlow();
                        pf.MachineName = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[positon].UnitNo;
                        pf.StartTime = DateTime.Now;
                        lock (job)
                        {
                            if (job.processFlow == null)
                            {
                                job.processFlow = new ProcessFlow();
                                job.processFlow.ExtendUnitProcessFlows.Add(pf);
                            }
                            else
                            {
                                job.processFlow.ExtendUnitProcessFlows.Add(pf);
                            }
                        }
                        ObjectManager.JobManager.AddJob(job);

                        trx[0][0][0].Value = job.Cassette_Sequence_No;
                        trx[0][0][1].Value = job.Job_Sequence_No.ToString();
                        trx[0][0][2].Value = job.GlassID_or_PanelID.ToString();
                        trx[0][0][3].Value = job.Job_Judge;
                        trx[0][0][4].Value = job.Job_Grade;

                        trx[0][1][0].Value = "1";
                        trx[0][1][1].Value = unitNo;
                        trx[0][1][2].Value = "0";
                        trx[0][1][3].Value = "0";


                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                        if (eq.File.CIMMode == eBitResult.ON)
                        {
                            trx[0][2][0].Value = "1";
                            trx[0][2].OpDelayTimeMS = 500;

                        }
                        else
                        {
                            trx[0][2][0].Value = "0";
                        }

                        SendToPLC(trx);

                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Store.ToString(),
                                   eq.Data.NODEID,
                                   eq.Data.NODENO, unitNo, "", "", "");

                        string timerID = string.Format("{0}_{1}_{2}", "L3", pathNo.PadLeft(2, '0'), CPCStoreGlassDataReportTimeout);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCStoreGlassDataReportTimeoutAction), trx.TrackKey);


                        stroeChannel[int.Parse(pathNo)] = false;


                        Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Store Glass Data Report CassetteSequenceNo[{2}] JobSequenceNo[{3}] GlassID[{4}]", "L3", trx.TrackKey, trx[0][0][0].Value, trx[0][0][1].Value, trx[0][0][2].Value));


                        Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                        if (line?.Data.ATTRIBUTE == "ETCH")
                        {
                            if (unitNo == "13")
                            {
                                ProcessStartJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "STRIP")
                        {
                            if (unitNo == "14")
                            {
                                ProcessStartJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "CLN")
                        {
                            if (unitNo == "2")
                            {
                                ProcessStartJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "DEV")
                        {
                            if (unitNo == "2")
                            {
                                ProcessStartJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "ODF")
                        {
                            if (unitNo == "3")
                            {
                                ProcessStartJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
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
            fetchChannel.Add(2, true);
            fetchChannel.Add(3, true);
            fetchChannel.Add(4, true);
            fetchChannel.Add(5, true);
            fetchChannel.Add(6, true);
            fetchChannel.Add(7, true);
            fetchChannel.Add(8, true);
            fetchChannel.Add(9, true);
            fetchChannel.Add(10, true);

            Thread.Sleep(2000);
            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

            while (true)
            {
                try
                {
                    Thread.Sleep(50);

                    string pathNo = "1";
                    if (fetchChannel.ContainsValue(true))
                    {
                        FetchReport = true;

                        pathNo = fetchChannel.Where(d => d.Value == true).First().Key.ToString();
                    }
                    else
                    {
                        FetchReport = false;
                    }




                    if (_isRuning && FetchReport && eq.File.FetchJobs.Count > 0)
                    {

                        int positon = eq.File.FetchJobs.First().Key;

                        Job job = eq.File.FetchJobs[positon];

                        eq.File.FetchJobs.TryRemove(positon, out job);

                        Trx trx = GetTrxValues(string.Format("{0}_FetchedOutJobReport#{1}", eq.Data.NODENO, pathNo.PadLeft(2, '0')));
                        // Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("_JobHoldEventReport") as Trx;
                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_FetchedOutJobReport#{1} IN PLCFmt.xml!", "L3", pathNo.PadLeft(2, '0')));

                        string unitNo = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID)[positon].UnitNo;

                        lock (job)
                        {
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
                        }

                        trx[0][0][0].Value = job.Cassette_Sequence_No;
                        trx[0][0][1].Value = job.Job_Sequence_No.ToString();
                        trx[0][0][2].Value = job.GlassID_or_PanelID.ToString();
                        trx[0][0][3].Value = job.Job_Judge;
                        trx[0][0][4].Value = job.Job_Grade;

                        trx[0][1][0].Value = "1";
                        trx[0][1][1].Value = unitNo;
                        trx[0][1][2].Value = "0";
                        trx[0][1][3].Value = "0";


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



                        ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.FetchOut.ToString(),
                                  eq.Data.NODEID,
                                  eq.Data.NODENO, unitNo, "", "", "");

                        string timerID = string.Format("{0}_{1}_{2}", "L3", pathNo.PadLeft(2, '0'), CPCFetchGlassDataReportTimeout);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCFetchGlassDataReportTimeoutAction), trx.TrackKey);


                        fetchChannel[int.Parse(pathNo)] = false;


                        Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Fetch Glass Data Report CassetteSequenceNo[{2}] JobSequenceNo[{3}] GlassID[{4}]", "L3", trx.TrackKey, trx[0][0][0].Value, trx[0][0][1].Value, trx[0][0][2].Value));

                        Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);

                        if (line?.Data.ATTRIBUTE == "ETCH")
                        {
                            if (unitNo == "22")
                            {
                                ProcessEndJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "STRIP")
                        {
                            if (unitNo == "23")
                            {
                                ProcessEndJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "CLN")
                        {
                            if (unitNo == "5")
                            {
                                ProcessEndJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "DEV")
                        {
                            if (unitNo == "8")
                            {
                                ProcessEndJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
                        }
                        else if (line?.Data.ATTRIBUTE == "ODF")
                        {
                            if (unitNo == "10")
                            {
                                ProcessEndJobReport(eq, job, eBitResult.ON, "01", unitNo);
                            }
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

        public void CPCUnitRecipeListReport()
        {

            while (true)
            {
                try
                {
                    Thread.Sleep(50);

                    if (_isRuning && RecipeReport)
                    {
                        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                        Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic =
                        ObjectManager.RecipeManager.ReloadRecipeByNo();

                        Trx trx = GetTrxValues(string.Format("{0}_LocalRecipeListReport", eq.Data.NODENO));

                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_LocalRecipeListReport IN PLCFmt.xml!", "L3"));

                        int i = 1;
                        List<RecipeEntityData> recipelist = recipeDic[eq.Data.LINEID].Values.ToList();

                        while (i <= 1)
                        {
                            if (UnitListRecipeReport)
                            {
                                trx.ClearTrxWith0();

                                trx[0][0][0].Value = "0";
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

        public void CPCUnitRecipeListReport(string reportType)
        {
            try
            {

                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");


                Trx trx = GetTrxValues(string.Format("{0}_LocalRecipeListReport", eq.Data.NODENO));

                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_LocalRecipeListReport IN PLCFmt.xml!", "L3"));
                Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE == "CELL")
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipeByNo();
                }
                else
                {
                    recipeDic = ObjectManager.RecipeManager.ReloadRecipe();
                }
                trx.ClearTrx();
                int count = 0;
                int TotalGroupCount = 0;
                if (recipeDic == null || !recipeDic.ContainsKey(eq.Data.LINEID))
                {
                    trx[0][0][0].Value = "0";
                    trx[0][0]["ReportType"].Value = reportType;
                    trx[0][0]["TotalLocalRecipeCount"].Value = count.ToString();//所有Recipe组数
                    trx[0][0]["TotalGroupCount"].Value = TotalGroupCount.ToString();//count == 0 ? "0" : "1";
                    trx[0][0]["CurrentGroupCount"].Value = LastRecipeGroupReportIndex.ToString();//count == 0 ? "0" : "1";
                }
                else
                {
                    List<RecipeEntityData> recipelist = recipeDic[eq.Data.LINEID].Values.ToList();
                    count = recipelist.Count;

                    EachGroupRecipeCount = (trx[0][0].Items.Count - 5) / 2;
                    TotalGroupCount = recipelist.Count / EachGroupRecipeCount + (recipelist.Count > EachGroupRecipeCount ? recipelist.Count % EachGroupRecipeCount : 1);

                    for (int i = RecipeListReportIndex; i < recipelist.Count; i++)
                    {
                        if ((RecipeListReportIndex - EachGroupRecipeCount * LastRecipeGroupReportIndex) > 0 && (RecipeListReportIndex - EachGroupRecipeCount * LastRecipeGroupReportIndex) % EachGroupRecipeCount == 0)
                        {
                            break;
                        }
                        if (line.Data.FABTYPE == "CELL")
                        {
                            //trx[0][0][1].Value += recipelist[i].RECIPENO;
                            trx[0][0][1 + (i - EachGroupRecipeCount * LastRecipeGroupReportIndex) * 2].Value = recipelist[i].OPERATORID;
                            trx[0][0][1 + (i - EachGroupRecipeCount * LastRecipeGroupReportIndex) * 2 + 1].Value = recipelist[i].RECIPENO;
                        }
                        else if (line.Data.FABTYPE == "ARRAY")
                        {

                            //trx[0][0][1].Value += recipelist[i].RECIPEID.PadRight(26, ' ');
                            trx[0][0][1 + (i - EachGroupRecipeCount * LastRecipeGroupReportIndex) * 2].Value = recipelist[i].OPERATORID;
                            trx[0][0][1 + (i - EachGroupRecipeCount * LastRecipeGroupReportIndex) * 2 + 1].Value = recipelist[i].RECIPEID.PadRight(26, ' ');
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {

                            //trx[0][0][1].Value += recipelist[i].RECIPEID.PadRight(4, ' ');
                            trx[0][0][1 + (i - EachGroupRecipeCount * LastRecipeGroupReportIndex) * 2].Value = recipelist[i].OPERATORID;
                            trx[0][0][1 + (i - EachGroupRecipeCount * LastRecipeGroupReportIndex) * 2 + 1].Value = recipelist[i].RECIPEID.PadRight(4, ' ');
                        }
                        RecipeListReportIndex++;
                    }
                    LastRecipeGroupReportIndex++;

                    trx[0][0][0].Value = "0";
                    trx[0][0]["ReportType"].Value = reportType;
                    trx[0][0]["TotalLocalRecipeCount"].Value = count.ToString();//所有Recipe组数
                    trx[0][0]["TotalGroupCount"].Value = TotalGroupCount.ToString();//count == 0 ? "0" : "1";
                    trx[0][0]["CurrentGroupCount"].Value = LastRecipeGroupReportIndex.ToString();//count == 0 ? "0" : "1";
                    if (recipelist.Count <= RecipeListReportIndex)
                    {
                        RecipeListReportIndex = 0;
                        LastRecipeGroupReportIndex = 0;
                    }
                }

                //trx.ClearTrxWith0();






                trx.TrackKey = UtilityMethod.GetAgentTrackKey();

                if (eq.File.CIMMode == eBitResult.ON)
                {
                    trx[0][1][0].Value = "1";
                    trx[0][1].OpDelayTimeMS = 500;//ParameterManager[eParameterName.EventDelayTime].GetInteger();

                }
                else
                {
                    trx[0][1][0].Value = "0";
                }

                SendToPLC(trx);

                string timerID = string.Format("{0}_{1}", "L3", UnitRecipeListReportTimeout);

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCUnitRecipeListReportTimeoutAction), trx.TrackKey);
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
                    msg = "Request From RMS";
                }

                Logger.LogInfoWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] Local Recipe List Report BIT ON CurrentGroupCount[{2}] TotalGroupCount[{3}] TotalLocalRecipeCount[{4}] ReportType=[{5}][{6}]", "L3",
                        trx.TrackKey, trx[0][0]["CurrentGroupCount"].Value, trx[0][0]["TotalGroupCount"].Value, count, reportType, msg));


            }
            catch (Exception ex)
            {
                this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);


            }


        }

        public void CPCTackTimeDataReport()
        {

            while (true)
            {
                try
                {
                    Thread.Sleep(50);
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


        public void CPCSVDataReport()
        {

            Thread.Sleep(5000);
            //Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
            while (true)
            {
                try
                {

                    Trx trxRecieve = GetTrxValues("L3_EQDEquipmentSVReport");
                    Trx trx = GetTrxValues(string.Format("{0}_TactDataReport", "L3"));

                    // Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(string.Format("{0}_SVDataReport", eq.Data.NODENO)) as Trx;
                    if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX SVDataReport IN PLCFmt.xml!", "L3"));

                    string strSaveData = string.Empty;
                    int reportData = 0;
                    int rate = 1;
                    double realData = 0;
                    for (int i = 1; i < trx[0].Events.AllValues.Length; i++)
                    {
                        for (int j = 0; j < trx[0][i].Items.AllValues.Length; j++)
                        {
                            string itemValue = trxRecieve[0][0][j].Value;
                            trx[0][i][j].Value = itemValue.ToString();

                            reportData = int.Parse(trx[0][i][j].Value);
                            if (trx[0][i][j].Name.Contains('/'))
                            {
                                rate = int.Parse(trx[0][i][j].Name.Split('/')[1]);
                                realData = reportData * 1.0 / rate;
                            }
                            else
                            {
                                realData = reportData;
                                rate = 1;
                            }

                            if (j < trx[0][i].Items.AllValues.Length - 1)
                            {
                                if (trx[0][i][j].Name.Contains('/'))
                                {
                                    strSaveData += trx[0][i][j].Name.Split('/')[0] + "=" + realData + ",";
                                }
                                else
                                {
                                    strSaveData += trx[0][i][j].Name + "=" + realData + ",";
                                }

                            }
                            else
                            {
                                if (trx[0][i][j].Name.Contains('/'))
                                {
                                    strSaveData += trx[0][i][j].Name.Split('/')[0] + "=" + realData;
                                }
                                else
                                {
                                    strSaveData += trx[0][i][j].Name + "=" + realData;
                                }
                            }
                        }

                    }
                    string reportTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    trx[0][0][0].Value = reportTime.Substring(0, 4);
                    trx[0][0][1].Value = reportTime.Substring(4, 2);
                    trx[0][0][2].Value = reportTime.Substring(6, 2);
                    trx[0][0][3].Value = reportTime.Substring(8, 2);
                    trx[0][0][4].Value = reportTime.Substring(10, 2);
                    trx[0][0][5].Value = reportTime.Substring(12, 2);

                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    MakeSVDataValuesToEXCEL(trx);
                    Thread.Sleep(2000);
                    //svSaveInterval++;
                    //if (svSaveInterval >= 5)
                    //{
                    //    //SaveSVDataToDB(strSaveData);
                    //}

                    //LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    //    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}]  SV Data Report.", inputData.Metadata.NodeNo, inputData.TrackKey));
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                    continue;
                }
            }



        }

        public void SaveSVDataToDB(string strSaveData)
        {
            svSaveInterval = 0;

            string sqlHead = "use KZONE_B20_01_HIS ";
            string sqlName = "insert [dbo].[SBCS_SVData] ([UpdateTime],[DataValues])";
            string sqlValue = $" values ('{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{strSaveData}')";
            SqlConnection mycon = GetConnection();
            mycon.Open();


            SqlCommand cmd = mycon.CreateCommand();
            cmd.CommandText = sqlHead + sqlName + sqlValue;
            cmd.ExecuteNonQuery();

            mycon.Close();
        }

        private SqlConnection GetConnection()
        {
            string constr = "data source=.; Database=KZONE_B20_01_HIS;user id=sa; password=12345678";
            SqlConnection mycon = new SqlConnection(constr);
            return mycon;
        }

        public void CPCFACDataReport()
        {
            Thread.Sleep(2000);
            while (true)
            {
                try
                {
                    Thread.Sleep(60000);
                    if (FACReport)
                    {
                        Trx trxRecieve = GetTrxValues("L3_EQDFACDataReport");
                        Trx trx = GetTrxValues(string.Format("{0}_FACDataReport", "L3"));

                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_FACDataReport IN PLCFmt.xml!", "L3"));

                        string strSaveData = string.Empty;
                        int reportData = 0;
                        int rate = 1;
                        double realData = 0;
                        for (int i = 0; i < trx[0].Events.AllValues.Length - 1; i++)
                        {
                            for (int j = 0; j < trx[0][i].Items.AllValues.Length; j++)
                            {
                                string itemValue = trxRecieve[0][0][j].Value;
                                trx[0][i][j].Value = itemValue.ToString();

                                reportData = int.Parse(trx[0][i][j].Value);
                                if (trx[0][i][j].Name.Contains('/'))
                                {
                                    rate = int.Parse(trx[0][i][j].Name.Split('/')[1]);
                                    realData = reportData * 1.0 / rate;
                                }
                                else
                                {
                                    realData = reportData;
                                    rate = 1;
                                }

                                if (j < trx[0][i].Items.AllValues.Length - 1)
                                {
                                    if (trx[0][i][j].Name.Contains('/'))
                                    {
                                        strSaveData += trx[0][i][j].Name.Split('/')[0] + "=" + realData + ",";
                                    }
                                    else
                                    {
                                        strSaveData += trx[0][i][j].Name + "=" + realData + ",";
                                    }

                                }
                                else
                                {
                                    if (trx[0][i][j].Name.Contains('/'))
                                    {
                                        strSaveData += trx[0][i][j].Name.Split('/')[0] + "=" + realData;
                                    }
                                    else
                                    {
                                        strSaveData += trx[0][i][j].Name + "=" + realData;
                                    }
                                }
                            }

                        }

                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                        trx[0][1][0].Value = "1";
                        SendToPLC(trx);

                        string timerID = "L3_FACDataReportTimeout";
                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }
                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCFACDataReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

                        FACReport = false;
                        MakeFACDataValuesToEXCEL(trx);
                        //svSaveInterval++;
                        //if (svSaveInterval >= 5)
                        //{
                        //    svSaveInterval = 0;
                        //}

                        //LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        //    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}]  SV Data Report.", inputData.Metadata.NodeNo, inputData.TrackKey));
                    }
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                    continue;
                }
            }


        }


        public void CPCPPIDPreDownLoadFlagToDownstream()
        {
            try
            {

                Thread.Sleep(2000);
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                while (true)
                {
                    Thread.Sleep(1000);
                    if (PPIDPreDownloadFlagReport && eq.File.TotalTFTGlassCount == 0 && eq.File.PPIDPreDownloadInfos.Count > 0)
                    {
                        Trx trx = GetTrxValues(string.Format("L3_PPIDPreDownLoadFlag"));

                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_PPIDPreDownLoadFlag IN PLCFmt.xml!", "L3"));
                        string PPID = eq.File.PPIDPreDownloadInfos.First().Key;
                        PPIDPreDownloadInfos pPIDPreDownloadInfos = new PPIDPreDownloadInfos();
                        eq.File.PPIDPreDownloadInfos.TryRemove(PPID, out pPIDPreDownloadInfos);

                        trx[0][0][0].Value = pPIDPreDownloadInfos.PPID;
                        trx[0][0][1].Value = pPIDPreDownloadInfos.BatchID;
                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                        trx[0][1][0].Value = "1";
                        SendToPLC(trx);

                        string timerID = "L3_PPIDPreDownLoadFlagToDownstreamTimeout";
                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCPPIDPreDownloadFlagToDownstreamTimeoutAction), UtilityMethod.GetAgentTrackKey());
                        PPIDPreDownloadFlagReport = false;


                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] PPID PreDownload Flag To Downstream Bit ON PPID=[{2}] BatchID=[{3}].", trx.Metadata.NodeNo, trx.TrackKey, trx[0][0][0].Value, trx[0][0][1].Value));
                    }
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCTransferTimeDataReport()
        {
            try
            {
                Thread.Sleep(2000);
                while (true)
                {
                    Thread.Sleep(200);
                    if (TransferTimeDataReportFlag && dicTransferTimes.Count > 0 && lsGlassID.Count > 0)
                    {
                        TransferTimeDataReportFlag = false;
                        string glassID = lsGlassID.FirstOrDefault();
                        string trasactionID = dicTransferTimes.Where(x => x.Value.GlassID?.Trim() == glassID.Trim()).FirstOrDefault().Key;
                        if (!string.IsNullOrEmpty(trasactionID))
                        {
                            string cstNo = dicTransferTimes[trasactionID].CassetteSequenceNo;
                            string slotNo = dicTransferTimes[trasactionID].SlotSequenceNo;
                            TransferTimeDataReport(eBitResult.ON, glassID, cstNo, slotNo);
                        }
                        lsGlassID.RemoveAt(0);
                    }
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCTR01_02_TransferRequest()
        {
            try
            {
                Thread.Sleep(5000);
                while (true)
                {
                    Thread.Sleep(200);
                    if ((eq.File.TR01TransferEnable == eEnableDisable.Enable || eq.File.TR02TransferEnable == eEnableDisable.Enable) && TR01_TR02_TransferRequestFlag && (eq.File.DownActionMonitor[1] == "1" || eq.File.DownActionMonitor[7] == "1"))
                    {
                        TR01_TR02_TransferRequestFlag = false;
                        Trx trx = new Trx();
                        //判断一下，避免误请求，两个出口都是inlineMode时才能发送Request
                        if (eq.File.SendStep == eSendStep.InlineMode && eq.File.SendStepUP == eSendStep.InlineMode && CurrentGlassSendToTR == eTransfer.NONE)
                        {

                            //TR01,TR02都可送片
                            if (eq.File.TR01TransferEnable == eEnableDisable.Enable && eq.File.TR02TransferEnable == eEnableDisable.Enable)
                            {
                                if (eq.File.LastRequestTransfer == eTransfer.TR01)
                                {
                                    trx = GetTrxValues("L3_TR02TransferFirstToTR02");
                                    eq.File.LastRequestTransfer = eTransfer.TR02;
                                }
                                else if (eq.File.LastRequestTransfer == eTransfer.TR02)
                                {
                                    trx = GetTrxValues("L3_TR01TransferFirstToTR01");
                                    eq.File.LastRequestTransfer = eTransfer.TR01;
                                }
                                else
                                {
                                    trx = GetTrxValues("L3_TR01TransferFirstToTR01");
                                    eq.File.LastRequestTransfer = eTransfer.TR01;
                                }

                                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                            }//仅给TR01送片
                            else if (eq.File.TR01TransferEnable == eEnableDisable.Enable && eq.File.TR02TransferEnable == eEnableDisable.Disable)
                            {
                                trx = GetTrxValues("L3_TR01TransferFirstToTR01");
                                eq.File.LastRequestTransfer = eTransfer.TR01;
                            }//仅给TR02送片
                            else if (eq.File.TR01TransferEnable == eEnableDisable.Disable && eq.File.TR02TransferEnable == eEnableDisable.Enable)
                            {
                                trx = GetTrxValues("L3_TR02TransferFirstToTR02");
                                eq.File.LastRequestTransfer = eTransfer.TR02;
                            }
                            else
                            {
                                continue;
                            }
                            trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                            trx[0][0][0].Value = "1";
                            SendToPLC(trx);
                            string timerID = trx.Name + "_Timeout";
                            if (Timermanager.IsAliveTimer(timerID))
                            {
                                Timermanager.TerminateTimer(timerID);
                            }
                            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.TR01_02_TransferRequestTime].GetInteger(),
                                new System.Timers.ElapsedEventHandler(CPCTR01_02_TransferRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
                            string downStreamName = trx.Name.Split('_')[1].Substring(0, 4);
                            string downStreamLocalNo = downStreamName == "TR01" ? "L4" : downStreamName == "TR02" ? "L6" : "";
                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [L3 -> {1}][{2}] {3} Transfer Request Bit [{4}].",
                                trx.Metadata.NodeNo, downStreamLocalNo, trx.TrackKey, downStreamName, "ON"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCMaterialValidationRequest(eBitResult bitResult, string materialID, string materialPosition)
        {
            try
            {
                Trx trx = GetTrxValues("L3_MaterialValidationRequest");
                if (trx == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[L3] TRX L3_MaterialValidationRequest IN PLCFmt.xml!"));
                string timerID = "L3_MaterialValidationRequestTimeout";
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
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Material Validation Request =[{2}].", "L3", trx.TrackKey, "OFF"));
                    return;
                }

                MaterialEntity material = ObjectManager.MaterialManager.GetMaterialByID("L3", materialID);
                if (material == null)
                {
                    material = new MaterialEntity(materialID);
                    material.NodeNo = "L3";
                    material.Position = materialPosition;
                    material.UnitNo = 3;
                    material.OperatorID = eq.File.OprationName;
                }
                else
                {
                    material.NodeNo = "L3";
                    material.Position = materialPosition;
                    material.UnitNo = 3;
                    material.OperatorID = eq.File.OprationName;
                }
                //ObjectManager.MaterialManager.AddMaterial(material);
                //ObjectManager.MaterialManager.SaveMaterialHistory(eq.Data.LINEID, material.UnitNo.ToString(), material, material.MaterialId, $"{material.NodeNo}_{material.UnitNo}_{material.MaterialId}");
                RequestMaterialID = materialID;
                EQRequestMaterialFlag = true;
                trx[0][0][0].Value = materialID;
                trx[0][0][1].Value = materialPosition;
                trx[0][0][2].Value = "3";//UnitNo
                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Material Validation Request =[{2}]. Material ID=[{3}]", "L3", trx.TrackKey, "ON", materialID));
                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCMaterialValidationRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCMaterialStatusChangeReport(eBitResult bitResult, Trx inputData)
        {
            try
            {
                Trx trx = GetTrxValues("L3_MaterialStatusChangeReport");
                if (trx == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[L3] TRX L3_MaterialStatusChangeReport IN PLCFmt.xml!"));
                string timerID = "L3_MaterialStatusChangeReportTimeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.OFF)
                {
                    trx[0][3][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Material Status Change Report =[{2}].", "L3", trx.TrackKey, "OFF"));
                    return;
                }

                trx[0][0][0].Value = "3";//UnitNo
                trx[0][0][1].Value = "0";//StageNo
                trx[0][0][2].Value = inputData[0][0]["MaterialID"].Value;
                trx[0][0][3].Value = inputData[0][0]["MaterialStatus"].Value;
                trx[0][0][4].Value = inputData[0][0]["MaterialType"].Value;
                trx[0][1][0].Value = "1";//TankNo
                trx[0][1][1].Value = "0";//StageNo
                trx[0][1][2].Value = inputData[0][0]["PRID"].Value;
                trx[0][1][3].Value = eq.File.OprationName;
                trx[0][1][4].Value = inputData[0][0]["PRWeight"].Value;

                //设备不用报。MES自行记录
                trx[0][1][5].Value = "0";//material.MountTime.Year.ToString();
                trx[0][1][6].Value = "0"; //material.MountTime.Month.ToString();
                trx[0][1][7].Value = "0"; //material.MountTime.Day.ToString();
                trx[0][1][8].Value = "0"; //material.MountTime.Hour.ToString();
                trx[0][1][9].Value = "0"; //material.MountTime.Minute.ToString();
                trx[0][1][10].Value = "0"; //material.MountTime.Second.ToString();

                trx[0][1][11].Value = "0"; //material.UnmountTime.Year.ToString();
                trx[0][1][12].Value = "0"; //material.UnmountTime.Month.ToString();
                trx[0][1][13].Value = "0"; //material.UnmountTime.Day.ToString();
                trx[0][1][14].Value = "0"; //material.UnmountTime.Hour.ToString();
                trx[0][1][15].Value = "0"; //material.UnmountTime.Minute.ToString();
                trx[0][1][16].Value = "0"; //material.UnmountTime.Second.ToString();

                trx[0][2][0].Value = eq.File.CurrentRecipeNo.ToString();
                trx[0][2][1].Value = eq.File.OprationName;

                trx[0][3][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Material Status Change Report Bit=[{2}]. MaterialID=[{3}] MaterialStatus=[{4}][{5}] MaterialType=[{6}][{7}] PRID=[{8}] PRWeight=[{9}]",
                    "L3", trx.TrackKey, "ON", inputData[0][0]["MaterialID"].Value.Trim(), inputData[0][0]["MaterialStatus"].Value, (eMaterialStatus)int.Parse(inputData[0][0]["MaterialStatus"].Value), inputData[0][0]["MaterialType"].Value, (eMaterialType)int.Parse(inputData[0][0]["MaterialType"].Value), inputData[0][0]["PRID"].Value.Trim(), inputData[0][0]["PRWeight"].Value.Trim()));
                Timermanager.CreateTimer(timerID, false, 6000,
                    new System.Timers.ElapsedEventHandler(CPCMaterialStatusChangeReportTimeoutAction), UtilityMethod.GetAgentTrackKey());

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCDummyGlassRequest(eBitResult eBitResult, string dummyGlassCount, string dummyType, string targetLocalNumber, string recipeID)
        {
            try
            {
                Trx trx = GetTrxValues("L3_DummyGlassRequest");
                if (trx == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[L3] TRX L3_DummyGlassRequest IN PLCFmt.xml!"));
                string timerID = "L3_DummyGlassRequestTimeout";
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
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Dummy Glass Request BIT=[{2}].", "L3", trx.TrackKey, "OFF"));
                    return;
                }

                trx[0][0][0].Value = dummyGlassCount;
                trx[0][0][1].Value = dummyType;
                trx[0][0][2].Value = targetLocalNumber;
                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (line.Data.FABTYPE != "CF")
                {
                    trx[0][0][3].Value = recipeID;
                }
                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);
                if (line.Data.FABTYPE != "CF")
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Dummy Glass Request BIT=[{2}]. DummyGlassCount=[{3}] DummyType=[{4}] TargetLocalNumber=[{5}] DummyGlassRecipeID=[{6}]", "L3", trx.TrackKey, "ON", dummyGlassCount, dummyType, targetLocalNumber, recipeID));
                }
                else
                {
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Dummy Glass Request BIT=[{2}]. DummyGlassCount=[{3}] DummyType=[{4}] TargetLocalNumber=[{5}]", "L3", trx.TrackKey, "ON", dummyGlassCount, dummyType, targetLocalNumber));
                }

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCDummyGlassRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCDummyGlassRequestEQP(eBitResult eBitResult, string dummyGlassCount, string dummyType, string targetLocalNumber, string recipeID, string dummyTargetBufferNo, string dummySlotNo)
        {
            try
            {
                Trx trx = GetTrxValues("L3_DummyGlassRequest");
                if (trx == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[L3] TRX L3_DummyGlassRequest IN PLCFmt.xml!"));
                string timerID = "L3_DummyGlassRequestTimeout";
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
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Dummy Glass Request BIT=[{2}].", "L3", trx.TrackKey, "OFF"));
                    return;
                }

                trx[0][0][0].Value = dummyTargetBufferNo;
                trx[0][0][1].Value = dummySlotNo;
                trx[0][0][2].Value = dummyType;
                trx[0][0][3].Value = targetLocalNumber;
                trx[0][0][4].Value = dummyGlassCount;
                trx[0][0][5].Value = recipeID;

                trx[0][1][0].Value = "1";
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(trx);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Dummy Glass Request BIT=[{2}]. DummyGlassCount=[{3}] DummyType=[{4}] TargetLocalNumber=[{5}] DummyGlassRecipeID=[{6}] DummyTargetBufferNo[{7}] DummySlotNo[{8}]", "L3", trx.TrackKey, "ON", dummyGlassCount, dummyType, targetLocalNumber, recipeID, dummyTargetBufferNo, dummySlotNo));


                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCDummyGlassRequestTimeoutAction), UtilityMethod.GetAgentTrackKey());
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public void CPCVCREventReport(eBitResult eBitResult, string jobID, string cstNo, string slotNo, string unitNo, string VCRNo, string VCRResult, string msg)
        {
            try
            {
                Trx trx = GetTrxValues("L3_VCR1EventReport");
                string timerID = "L3_CPCVCREventReportTimeout";
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
                            string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] EQ VCR Event Report Bit=[{2}].",
                            trx.Metadata.NodeNo, trx.TrackKey, eBitResult));
                    return;
                }
                trx[0][0][0].Value = jobID;
                trx[0][0][1].Value = cstNo;
                trx[0][0][2].Value = slotNo;
                trx[0][0][3].Value = unitNo;
                trx[0][0][4].Value = VCRNo;
                trx[0][0][5].Value = VCRResult;

                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                trx[0][1][0].Value = "1";
                SendToPLC(trx);

                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                    new System.Timers.ElapsedEventHandler(CPCVCREventReportTimeoutAction), UtilityMethod.GetAgentTrackKey());


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [EC -> BC][{1}] EQ VCR Event Report Bit=[{2}] JobID=[{3}] CstNo=[{4}] SlotNo=[{5}] UnitNo=[{6}] VCRNo=[{7}] VCRResult=[{8}][{9}].",
                            trx.Metadata.NodeNo, trx.TrackKey, eBitResult, jobID, cstNo, slotNo, unitNo, VCRNo, VCRResult, msg));
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
    }
}
