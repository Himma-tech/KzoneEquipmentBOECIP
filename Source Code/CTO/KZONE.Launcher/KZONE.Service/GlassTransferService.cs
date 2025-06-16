using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KZONE.ConstantParameter;
using KZONE.MessageManager;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.PLCAgent;
using KZONE.Work;
using System.Reflection;
using System.Runtime.CompilerServices;
using KZONE.PLCAgent.PLC;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml;
using EipTagLibrary;

namespace KZONE.Service
{
    public partial class EquipmentService : AbstractService
    {

        public bool ReceiveFlag = false;

        public int requestCount = 0;

        public bool glassTransferFlag = false;

        #region Receive Glass Thread

        public void ReceiveGlassAction()
        {
            //1、获取L3_UpstreamPath#01 TRX,当设备状态为Run、Idle\ UpStreamInmode On ，
            try
            {
              
                Trx actionTRX = null;

                IList<int> BitOnList = new List<int>();

                Thread.Sleep(2000);

                while (true)
                {
                  

                    Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!",
                            "L3"));
                    }
                    Block CIMMessageSetCommandBlock = eipTagAccess.ReadBlockValues("RV_EQToEQ_LinkSignal_02_03_00", "UpstreamLinkSignal");
                    int i = 0;
                    foreach (ItemBase it in CIMMessageSetCommandBlock.Items)
                    {
                        if (!eq.File.UPInterface.ContainsKey(i + 1))
                        {
                            eq.File.UPInterface.Add(i + 1, (bool)it.Value==true ?"1":"0");
                        }
                        else
                        {
                            eq.File.UPInterface[i + 1] = (bool)it.Value == true ? "1" : "0";

                        }
                        i++;
                    }
                    ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                    if (_isRuning && eq.File.DeBugMode == eEnableDisable.Disable) //与PLC是否连线
                    {
                        Thread.Sleep(5);

                        BitOnList.Clear();


                        if (eq.File.ReceiveStep == eReceiveStep.ReceiveCancel)
                        {
                            Thread.Sleep(2000);
                        }
                        if (eq.File.ReceiveStep == eReceiveStep.ReceiveResume)
                        {
                            Thread.Sleep(2000);
                        }

                        if (eq.File.UpInlineMode == eBitResult.ON) //检查InlineMode
                        {
                            eq.File.NextReceiveStep = eReceiveStep.InlineMode;


                            if (eq.File.UPActionMonitor[3] == "1") //设备收片锁定，玻璃到位，交握信号结束再复位
                            {

                                if (CheckEqUpActionTrouble(eq.File.UPActionMonitor))
                                {
                                    if (eq.File.UPReciveComplete == eBitResult.OFF)
                                    {

                                    
                                            #region Robot-CV信号判断

                                            if (eq.Data.NODENAME == "A2CVD_CDC")  //22005R
                                            {
                                            //当前状态为InLine On,上游设备为InLine On 或者设备为预放模式
                                            if (eq.File.ReceiveStep == eReceiveStep.InlineMode && GetOnBits(eq.File.UPInterface).Contains(1) && eq.File.UPActionMonitor[1] == "0" && eq.File.StopBitCommand == false)//感应器无片
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.ReceiveAble;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveAble && eq.File.StopBitCommand == true)
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveAble && GetOnBits(eq.File.UPInterface).Contains(1) && GetOnBits(eq.File.UPInterface).Contains(15))
                                            {

                                                //当前收片状态为InlineMode、上游的Upstream Inline、Send Able为ON 说明上游有片要传送过来
                                                // 1、检查进料口是否有玻璃基板，2、检查上游来料信息是否有问题，
                                                if (eq.File.UPActionMonitor[1] == "0")
                                                {
                                                    //if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                                                    //{
                                                    //    if (JobDataCheck(eq, out NGbyStopBit))
                                                    //    {
                                                    //        int j = JobDataRecipeCheck(eq);

                                                    //        switch (j)
                                                    //        {
                                                    //            case 0:
                                                    //                eq.File.NextReceiveStep = eReceiveStep.ReceiveStart;
                                                    //                LogInfo(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMNET={0}] [{1}] Job Data Check OK!", eq.Data.NODENO, UtilityMethod.GetAgentTrackKey()));
                                                    //                break;

                                                    //            case 1:
                                                    //                eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                                    //                break;

                                                    //            case 2:
                                                    //                eq.File.NextReceiveStep = eReceiveStep.JobDataCheckNG;
                                                    //                break;
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        eq.File.NextReceiveStep = eReceiveStep.JobDataCheckNG;
                                                    //        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("[EQUIPMENT={0}][{1}] Job Data Check NG!", eq.Data.NODENO, UtilityMethod.GetAgentTrackKey()));
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                        eq.File.NextReceiveStep = eReceiveStep.ReceiveStart;
                                                   // }
                                                }
                                                else
                                                {
                                                    //检测到进料口有片
                                                    eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                                    eq.File.UPDataCheckResult = "Input Port Has Glass!!!";
                                                }
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveStart && GetOnBits(eq.File.UPInterface).Contains(16) && GetOnBits(eq.File.UPInterface).Contains(17))
                                            {

                                                eq.File.NextReceiveStep = eReceiveStep.ReceiveComplete;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveComplete && GetOnBits(eq.File.UPInterface).Contains(1) && !GetOnBits(eq.File.UPInterface).Contains(15))
                                            {

                                                eq.File.NextReceiveStep = eReceiveStep.End;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.End)
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.JobDataCheckNG && eq.File.StopBitCommand == false && NGbyStopBit == true)
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.ReceiveAble;
                                                NGbyStopBit = false;
                                            }
                                            else
                                            {
                                                if (eq.File.ReceiveStep != eReceiveStep.BitAllOff)
                                                {
                                                    //上游设备信号异常时，保持状态不变 
                                                    eq.File.NextReceiveStep = eq.File.ReceiveStep;
                                                }
                                            }
                                                //异常请求
                                                //设备请求
                                                if (eq.File.ForceCompleteRequestToUpstream == eBitResult.ON)//异常流程Force Complete Request 感应有片可切complete
                                                {
                                                    if (eq.File.UPActionMonitor[1] == "1")
                                                    {
                                                        eq.File.NextReceiveStep = eReceiveStep.ForceCompleteRequest;
                                                    }
                                                    eq.File.ForceCompleteRequestToUpstream = eBitResult.OFF;
                                                }
                                                else if (eq.File.ForceInitialRequestToUpstream == eBitResult.ON)//异常流程Force Initial Request 感应无片可初始化
                                                {
                                                    if (eq.File.UPActionMonitor[1] == "0")
                                                    {
                                                        eq.File.NextReceiveStep = eReceiveStep.ForceInitialRequest;
                                                    }
                                                    eq.File.ForceInitialRequestToUpstream = eBitResult.OFF;
                                                }
                                                //上游设备请求
                                                //if (GetOnBits(eq.File.UPInterface).Contains(33))//异常流程上游设备请求complete，片子已经到设备内可切完成
                                                //{
                                                //    if (eq.File.UPActionMonitor[1] == "1" && eq.File.UpstreamForceCompleteRequest == eBitResult.OFF && GetOnBits(eq.File.UPInterface).Contains(3))
                                                //    {
                                                //        eq.File.NextReceiveStep = eReceiveStep.ReceiveComplete;
                                                //        eq.File.UpstreamForceCompleteRequest = eBitResult.ON;
                                                //    }
                                                //}
                                                //if (GetOnBits(eq.File.UPInterface).Contains(33))//异常流程上游设备请求Initial，片子未到设备可以切
                                                //{
                                                //    if (eq.File.UPActionMonitor[1] == "0" && eq.File.ReceiveStep != eReceiveStep.InlineMode && eq.File.UpstreamForceInitialRequest == eBitResult.OFF)
                                                //    {
                                                //        eq.File.NextReceiveStep = eReceiveStep.UpForceInitialRequestAck;
                                                //        eq.File.UpstreamForceInitialRequest = eBitResult.ON;
                                                //    }
                                                //}
                                                //if (eq.File.ReceiveStep == eReceiveStep.UpForceInitialRequestAck && !GetOnBits(eq.File.UPInterface).Contains(15))
                                                //{
                                                //    eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                                //}
                                                //if (eq.File.UpstreamForceInitialRequest == eBitResult.ON && !GetOnBits(eq.File.UPInterface).Contains(15))
                                                //{
                                                //    eq.File.UpstreamForceInitialRequest = eBitResult.OFF;
                                                //}
                                                //if (eq.File.UpstreamForceCompleteRequest == eBitResult.ON && !GetOnBits(eq.File.UPInterface).Contains(14))
                                                //{
                                                //    eq.File.UpstreamForceCompleteRequest = eBitResult.OFF;
                                                //}


                                            }

                                            #endregion
                                        

                                        if (eq.File.ReceiveStep == eReceiveStep.ForceCompleteRequest && GetOnBits(eq.File.UPInterface).Contains(3))//上游切换成Complete触发
                                        {
                                            eq.File.NextReceiveStep = eReceiveStep.ReceiveComplete;
                                        }
                                        if (eq.File.ReceiveStep == eReceiveStep.ForceInitialRequest && GetOnBits(eq.File.UPInterface).Contains(16))//上游初切换Force Initial Complete触发
                                        {
                                            eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                        }


                                    }
                                    else
                                    {
                                        eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                    }
                                }
                                else
                                {
                                    eq.File.NextReceiveStep = eReceiveStep.EqTrouble;
                                }

                            }
                            else
                            {
                                eq.File.NextReceiveStep = eReceiveStep.InlineMode;

                                //预放模式打开，设备Inline Mode On，设备机构开始动作
                                if (eq.Data.NODENAME != "CLEANER" && eq.File.UpStreamPerMode == eEnableDisable.Enable && eq.File.UPActionMonitor[8] == "1")
                                {
                                    eq.File.NextReceiveStep = eReceiveStep.PerReceive;
                                }
                            }
                        }
                        else
                        {
                            eq.File.NextReceiveStep = eReceiveStep.BitAllOff;
                        }

                        #region  根据动作写入信号
                        if (eq.File.ReceiveStep != eq.File.NextReceiveStep && eq.File.DeBugMode != eEnableDisable.Enable)
                        {
                            if (eq.File.NextReceiveStep == eReceiveStep.UpEqTrouble || eq.File.NextReceiveStep == eReceiveStep.EqTrouble)
                            {
                                eq.File.DownResum = eq.File.ReceiveStep;
                            }
                            eq.File.ReceiveStep = eq.File.NextReceiveStep;

                            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                            //写入W区域
                            Block trx = eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "DownstreamLinkSignal");

                           
                                //21005R设备信号写入
                                switch (eq.File.NextReceiveStep)
                                {
                                    case eReceiveStep.BitAllOff:

                                        trx = SetTrxValue(trx, "0");

                                        break;

                                    case eReceiveStep.EqTrouble:
                                        trx[0].Value = "false";
                                        trx[1].Value = "true";

                                        break;

                                    case eReceiveStep.UpEqTrouble:

                                        // trx[0][0][1].Value = "1";

                                        break;

                                    case eReceiveStep.InlineMode:

                                        BitOnList.Add(0);
                                        trx = SetTrxValue(trx, BitOnList);

                                        //信号初始化时候要清除检查结果
                                        eq.File.UPDataCheckResult = String.Empty;
                                        break;

                                    //预放模式要Receive Ready On
                                    case eReceiveStep.PerReceive:
                                        BitOnList.Add(0);
                                        BitOnList.Add(9);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveCancel:
                                        BitOnList.Add(0);
                                        BitOnList.Add(22);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveResume:

                                        BitOnList.Add(0);
                                        BitOnList.Add(19);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendCancel:

                                        BitOnList.Add(23);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendResume:

                                        trx[20].Value = "1";

                                        break;

                                    case eReceiveStep.ReceiveAble:


                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                       
                                        
                                        trx = SetTrxValue(trx, BitOnList);

                                        break;

                                    case eReceiveStep.JobDataCheckNG:

                                        BitOnList.Add(0);
                                        BitOnList.Add(4);
                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveStart:


                                        WriteEQDJobData();

                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(24);

                                    trx = SetTrxValue(trx, BitOnList);

                                        string timer2ID = "L3_LinkSignalT2Timeout";
                                        if (Timermanager.IsAliveTimer(timer2ID))
                                        {
                                            Timermanager.TerminateTimer(timer2ID);
                                        }
                                        Timermanager.CreateTimer(timer2ID, false, LinkSignalT2, new System.Timers.ElapsedEventHandler(LinkSignalT2TimeoutAction), UtilityMethod.GetAgentTrackKey());

                                        //Receive Timer
                                        if (ParameterManager[eParameterName.ReceiveTimer].GetInteger() != 0)
                                        {
                                            string ReceiveTimer = string.Format("{0}_ReceiveTimer", "L3");

                                            if (Timermanager.IsAliveTimer(ReceiveTimer))
                                            {
                                                Timermanager.TerminateTimer(ReceiveTimer);
                                            }
                                            Timermanager.CreateTimer(ReceiveTimer, false, ParameterManager[eParameterName.ReceiveTimer].GetInteger(),
                                         new System.Timers.ElapsedEventHandler(ReceiveTimerTimeoutAction), UtilityMethod.GetAgentTrackKey());
                                        }



                                        break;
                                    case eReceiveStep.ReceiveRoll:

                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(25);//滚轮转动

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.GlassExist:

                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(25);//滚轮转动
                                        BitOnList.Add(3);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;

                                    case eReceiveStep.TransferOn:

                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(12);//Transfer On

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;


                                    case eReceiveStep.ReceiveComplete:

                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(16);
                                        BitOnList.Add(24);

                                    trx = SetTrxValue(trx, BitOnList);

                                        timer2ID = "L3_LinkSignalT2Timeout";
                                        if (Timermanager.IsAliveTimer(timer2ID))
                                        {
                                            Timermanager.TerminateTimer(timer2ID);
                                        }

                                        string timer3ID = "L3_LinkSignalT3Timeout";
                                        if (Timermanager.IsAliveTimer(timer3ID))
                                        {
                                            Timermanager.TerminateTimer(timer3ID);
                                        }
                                        Timermanager.CreateTimer(timer3ID, false, LinkSignalT3, new System.Timers.ElapsedEventHandler(LinkSignalT3TimeoutAction), UtilityMethod.GetAgentTrackKey());

                                        break;

                                    case eReceiveStep.End:

                                        BitOnList.Add(0);

                                        timer3ID = "L3_LinkSignalT3Timeout";//清除timer
                                        if (Timermanager.IsAliveTimer(timer3ID))
                                        {
                                            Timermanager.TerminateTimer(timer3ID);
                                        }

                                        trx = SetTrxValue(trx, BitOnList);

                                        //收片完成解锁
                                        actionTRX = GetTrxValues("L3_EQDReciveCompleteBit");
                                        actionTRX[0][0][0].Value = "1";
                                        SendToPLC(actionTRX);
                                        eq.File.UPReciveComplete = eBitResult.ON;
                                        ObjectManager.EquipmentManager.EnqueueSave(eq.File);



                                        //ReceiveTimer  解除
                                        string ReceiveTime = string.Format("{0}_ReceiveTimer", "L3");

                                        if (Timermanager.IsAliveTimer(ReceiveTime))
                                        {
                                            Timermanager.TerminateTimer(ReceiveTime);
                                        }

                                        CPCReceiveJobDataReport(eBitResult.ON);
                                        break;

                                    case eReceiveStep.ForceCompleteRequest:
                                        BitOnList.Add(0);
                                        BitOnList.Add(13);
                                        trx = SetTrxValue(trx, BitOnList);
                                        break;
                                    case eReceiveStep.ForceInitialRequest:
                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        trx = SetTrxValue(trx, BitOnList);
                                        break;
                                    case eReceiveStep.UpForceInitialRequestAck:
                                        BitOnList.Add(0);
                                        BitOnList.Add(15);
                                        trx = SetTrxValue(trx, BitOnList);
                                        break;
                                    default:
                                        trx = SetTrxValue(trx, "0");

                                        break;

                                }
                            eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", trx);

                        }
                            //SendToPLC(trx);

                        }

                        #endregion
                    }


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        #endregion


        #region  Send Glass Thread 

        public void SendGlassAction()
        {
           // Trx trx = null;
            // Trx eqdTrx = null;

            Thread.Sleep(2000);

            IList<int> SendBitOnList = new List<int>();
            try
            {

                while (true)
                {
                    Equipment eqp = ObjectManager.EquipmentManager.GetEQP("L3");
                    if (eqp == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", "L3"));
                    }


                    Thread.Sleep(5);
                    if (_isRuning && eqp.File.DeBugMode == eEnableDisable.Disable)
                    {

                        Block CIMMessageSetCommandBlock = eipTagAccess.ReadBlockValues("RV_EQToEQ_LinkSignal_02_03_00", "DownstreamLinkSignal");
                        int i = 0;
                        foreach (ItemBase it in CIMMessageSetCommandBlock.Items)
                        {
                            if (!eqp.File.DownInterface01.ContainsKey(i + 1))
                            {
                                eqp.File.DownInterface01.Add(i + 1, (bool)it.Value == true ? "1" : "0");
                            }
                            else
                            {
                                eqp.File.DownInterface01[i + 1] = (bool)it.Value == true ? "1" : "0";

                            }
                            i++;
                        }
                        ObjectManager.EquipmentManager.EnqueueSave(eqp.File);

                        SendBitOnList.Clear();

                        if (eqp.File.NextSendStep == eSendStep.SendCancel)
                        {
                            Thread.Sleep(2000);
                        }

                        if (eqp.File.DownInlineMode == eBitResult.ON)
                        {
                            eqp.File.NextSendStep = eSendStep.InlineMode;


                            if (eqp.File.DownActionMonitor[1] == "1") //锁定   出片口有玻璃有账料
                            {
                                if (CheckDownStreamPIOTrouble(eqp.File.DownPIOBit))
                                {
                                    if (CheckEqDownActionTrouble(eqp.File.DownActionMonitor))
                                    {
                                        if (eqp.Data.NODENAME == "A2CVD_CDC")
                                        {



                                            if ((eqp.File.SendStep == eSendStep.InlineMode || eqp.File.SendStep == eSendStep.PerSend) && eqp.File.DownSendComplete == eBitResult.OFF && (eqp.File.DownActionMonitor[2] == "1" || eqp.File.DownActionMonitor[3] == "1") && GetOnBits(eqp.File.DownInterface01).Contains(1))//欲取模式
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendAble;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(15))
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendStart;
                                            }
                                          

                                            else if (eqp.File.SendStep == eSendStep.SendStart  && GetOnBits(eqp.File.DownInterface01).Contains(16) && GetOnBits(eqp.File.DownInterface01).Contains(17))
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendComPlete;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendComPlete && !GetOnBits(eqp.File.DownInterface01).Contains(15))
                                            {
                                                eqp.File.NextSendStep = eSendStep.End;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.End)
                                            {
                                                eqp.File.NextSendStep = eSendStep.InlineMode;
                                            }

                                            //else if (eqp.File.DownInterface01[2] == "1")
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.DownEqTrouble;
                                            //}
                                            else
                                            {
                                                if (eqp.File.SendStep != eSendStep.BitAllOff)
                                                {
                                                    eqp.File.NextSendStep = eqp.File.SendStep;
                                                }
                                            }
                                            //设备请求
                                            if (eqp.File.ForceCompleteRequestToDownstream == eBitResult.ON)//异常流程Force Complete Request 感应无片可切complete
                                            {
                                                if (eqp.File.DownActionMonitor[2] == "0")
                                                {
                                                    eqp.File.NextSendStep = eSendStep.ForceCompleteRequest;
                                                }
                                                eqp.File.ForceCompleteRequestToDownstream = eBitResult.OFF;
                                            }
                                            else if (eqp.File.ForceInitialRequestToDownstream == eBitResult.ON)//异常流程Force Initial Request 感应有片可初始化
                                            {
                                                if (eqp.File.DownActionMonitor[2] == "1")
                                                {
                                                    eqp.File.NextSendStep = eSendStep.ForceInitialRequest;
                                                }
                                                eqp.File.ForceInitialRequestToDownstream = eBitResult.OFF;
                                            }
                                            //下游设备请求
                                            if (GetOnBits(eqp.File.DownInterface01).Contains(33))
                                            {
                                                if (eqp.Data.NODENAME == "A2CVD_CDC")
                                                {
                                                    if (eqp.File.DownActionMonitor[1] == "0" && eqp.File.DownstreamForceCompleteRequest == eBitResult.OFF)
                                                    {
                                                        eqp.File.NextSendStep = eSendStep.SendComPlete;
                                                        eqp.File.DownstreamForceCompleteRequest = eBitResult.ON;
                                                    }
                                                }
                                                else if (eqp.Data.NODENAME == "F2ISP_3_Pre_CLN" || eqp.Data.NODENAME == "F2ISP_3_Post_CLN")
                                                {
                                                    if (eqp.File.DownActionMonitor[2] == "0" && eqp.File.DownstreamForceCompleteRequest == eBitResult.OFF)
                                                    {
                                                        eqp.File.NextSendStep = eSendStep.SendComPlete;
                                                        eqp.File.DownstreamForceCompleteRequest = eBitResult.ON;
                                                    }
                                                }
                                            }
                                            if (GetOnBits(eqp.File.DownInterface01).Contains(33) && eqp.File.DownstreamForceInitialRequest == eBitResult.OFF)
                                            {
                                                if (eqp.File.DownActionMonitor[2] == "1")
                                                {
                                                    eqp.File.NextSendStep = eSendStep.DownForceInitialRequestAck;
                                                    eqp.File.DownstreamForceInitialRequest = eBitResult.ON;
                                                }
                                            }
                                            //if (eqp.File.SendStep == eSendStep.DownForceInitialRequestAck && !GetOnBits(eqp.File.DownInterface01).Contains(15))
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.InlineMode;
                                            //}
                                            //if (eqp.File.DownstreamForceInitialRequest == eBitResult.ON && !GetOnBits(eqp.File.DownInterface01).Contains(15))
                                            //{
                                            //    eqp.File.DownstreamForceInitialRequest = eBitResult.OFF;
                                            //}
                                            //if (eqp.File.DownstreamForceCompleteRequest == eBitResult.ON && !GetOnBits(eqp.File.DownInterface01).Contains(14))
                                            //{
                                            //    eqp.File.DownstreamForceCompleteRequest = eBitResult.OFF;
                                            //}
                                        }
                                        else if (eqp.Data.NODENAME == "F2ISP_3_Pre_CLN" || eqp.Data.NODENAME == "F2ISP_3_Post_CLN")
                                        {
                                            if ((eqp.File.SendStep == eSendStep.InlineMode || eqp.File.SendStep == eSendStep.PerSend) && eqp.File.DownSendComplete == eBitResult.OFF && (eqp.File.DownActionMonitor[2] == "1" || eqp.File.DownActionMonitor[3] == "1") && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(4))//欲取模式
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendAble;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(2))
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendStart;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendStart && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(2)
                                                 && GetOnBits(eqp.File.DownInterface01).Contains(6) && eqp.File.DownActionMonitor[9] == "1" && eqp.File.DownActionMonitor[10] == "1")//CV启动并且出料sensor有感应到玻璃
                                            {
                                                eqp.File.NextSendStep = eSendStep.TansferOn;
                                            }

                                            else if (eqp.File.SendStep == eSendStep.TansferOn && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(2) && GetOnBits(eqp.File.DownInterface01).Contains(6) && eqp.File.DownActionMonitor[10] == "0")//sensor无感应，出片complete
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendComPlete;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendComPlete && GetOnBits(eqp.File.DownInterface01).Contains(3))
                                            {
                                                eqp.File.NextSendStep = eSendStep.End;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.End)
                                            {
                                                eqp.File.NextSendStep = eSendStep.InlineMode;
                                            }

                                            //else if (eqp.File.DownInterface01[2] == "1")
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.DownEqTrouble;
                                            //}
                                            else
                                            {
                                                if (eqp.File.SendStep != eSendStep.BitAllOff)
                                                {
                                                    eqp.File.NextSendStep = eqp.File.SendStep;
                                                }
                                            }
                                            //设备请求
                                            if (eqp.File.ForceCompleteRequestToDownstream == eBitResult.ON)//异常流程Force Complete Request 感应无片可切complete
                                            {
                                                if (eqp.File.DownActionMonitor[2] == "0")
                                                {
                                                    eqp.File.NextSendStep = eSendStep.ForceCompleteRequest;
                                                }
                                                eqp.File.ForceCompleteRequestToDownstream = eBitResult.OFF;
                                            }
                                            else if (eqp.File.ForceInitialRequestToDownstream == eBitResult.ON)//异常流程Force Initial Request 感应有片可初始化
                                            {
                                                if (eqp.File.DownActionMonitor[2] == "1")
                                                {
                                                    eqp.File.NextSendStep = eSendStep.ForceInitialRequest;
                                                }
                                                eqp.File.ForceInitialRequestToDownstream = eBitResult.OFF;
                                            }
                                            //下游设备请求
                                            if (GetOnBits(eqp.File.DownInterface01).Contains(14))
                                            {
                                                if (eqp.Data.NODENAME == "A2CVD_CDC")
                                                {
                                                    if (eqp.File.DownActionMonitor[2] == "0" && eqp.File.DownstreamForceCompleteRequest == eBitResult.OFF && GetOnBits(eqp.File.DownInterface01).Contains(3))
                                                    {
                                                        eqp.File.NextSendStep = eSendStep.SendComPlete;
                                                        eqp.File.DownstreamForceCompleteRequest = eBitResult.ON;
                                                    }
                                                }
                                                else if (eqp.Data.NODENAME == "F2ISP_3_Pre_CLN" || eqp.Data.NODENAME == "F2ISP_3_Post_CLN")
                                                {
                                                    if (eqp.File.DownActionMonitor[2] == "0" && eqp.File.DownstreamForceCompleteRequest == eBitResult.OFF && eqp.File.DownActionMonitor[10] == "0")
                                                    {
                                                        eqp.File.NextSendStep = eSendStep.SendComPlete;
                                                        eqp.File.DownstreamForceCompleteRequest = eBitResult.ON;
                                                    }
                                                }
                                            }
                                            if (GetOnBits(eqp.File.DownInterface01).Contains(15) && eqp.File.DownstreamForceInitialRequest == eBitResult.OFF)
                                            {
                                                if (eqp.File.DownActionMonitor[2] == "1")
                                                {
                                                    eqp.File.NextSendStep = eSendStep.DownForceInitialRequestAck;
                                                    eqp.File.DownstreamForceInitialRequest = eBitResult.ON;
                                                }
                                            }
                                            if (eqp.File.SendStep == eSendStep.DownForceInitialRequestAck && !GetOnBits(eqp.File.DownInterface01).Contains(15))
                                            {
                                                eqp.File.NextSendStep = eSendStep.InlineMode;
                                            }
                                            if (eqp.File.DownstreamForceInitialRequest == eBitResult.ON && !GetOnBits(eqp.File.DownInterface01).Contains(15))
                                            {
                                                eqp.File.DownstreamForceInitialRequest = eBitResult.OFF;
                                            }
                                            if (eqp.File.DownstreamForceCompleteRequest == eBitResult.ON && !GetOnBits(eqp.File.DownInterface01).Contains(14))
                                            {
                                                eqp.File.DownstreamForceCompleteRequest = eBitResult.OFF;
                                            }
                                            if (!GetOnBits(eqp.File.DownInterface01).Contains(3) && glassTransferFlag == true)
                                            {
                                                string timer5ID = "L3_LinkSignalT5Timeout";
                                                if (Timermanager.IsAliveTimer(timer5ID))
                                                {
                                                    Timermanager.TerminateTimer(timer5ID);
                                                }
                                                glassTransferFlag = false;
                                            }
                                        }
                                        if (eqp.File.SendStep == eSendStep.ForceCompleteRequest)
                                        {
                                            if (eqp.Data.NODENAME == "A2CVD_CDC" && GetOnBits(eqp.File.DownInterface01).Contains(3))
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendComPlete;
                                            }
                                            else if ((eqp.Data.NODENAME == "F2ISP_3_Pre_CLN" || eqp.Data.NODENAME == "F2ISP_3_Post_CLN") && eqp.File.DownActionMonitor[10] == "0")
                                            {
                                                Thread.Sleep(1000);
                                                eqp.File.NextSendStep = eSendStep.SendComPlete;
                                            }
                                        }
                                        if (eqp.File.SendStep == eSendStep.ForceInitialRequest && GetOnBits(eqp.File.DownInterface01).Contains(16))//上游初切换Force Initial Complete触发
                                        {
                                            eqp.File.NextSendStep = eSendStep.InlineMode;
                                        }
                                    }
                                    else
                                    {
                                        eqp.File.NextSendStep = eSendStep.EqTrouble;

                                    }
                                }
                                else
                                {
                                    eqp.File.NextSendStep = eSendStep.DownPIOTrouble;

                                }
                            }
                            else
                            {
                                eqp.File.NextSendStep = eSendStep.InlineMode;

                                //检查设备是否为欲取模式
                                if (eqp.File.DowmStreamPerMode == eEnableDisable.Enable && eqp.File.DownActionMonitor[7] == "1")
                                {
                                    eqp.File.NextSendStep = eSendStep.PerSend;
                                }
                            }
                        }
                        else
                        {
                            eqp.File.NextSendStep = eSendStep.BitAllOff;
                        }


                        if (eqp.File.SendStep != eqp.File.NextSendStep && eqp.File.DeBugMode != eEnableDisable.Enable)
                        {
                            if (eqp.File.NextSendStep == eSendStep.DownEqTrouble || eqp.File.NextSendStep == eSendStep.EqTrouble)
                            {
                                eqp.File.UPResum = eqp.File.SendStep;
                            }

                            eqp.File.SendStep = eqp.File.NextSendStep;

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                            if (eqp.Data.NODENAME == "A2CVD_CDC")
                            {
                                Block trx = eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");

                      
                                switch (eqp.File.NextSendStep)
                                {

                                    case eSendStep.BitAllOff:

                                        trx = SetTrxValue(trx, "0");

                                      
                                        break;
                                    case eSendStep.InlineMode:

                                        SendBitOnList.Add(0);
                                        trx = SetTrxValue(trx, SendBitOnList);

                                    
                                        break;
                                    case eSendStep.PerSend:

                                     
                                        trx = SetTrxValue(trx, SendBitOnList);

                                        break;
                                    case eSendStep.SendCancel:
                                        trx[0].Value = "1";
                                        trx[22].Value = "1";

                                        //    eqdTrx[0][0][22].Value = "1";
                                        break;
                                    case eSendStep.SendResume:
                                        trx[0].Value = "1";
                                        trx[19].Value = "1";

                                        //  eqdTrx[0][0][19].Value = "1";
                                        break;

                                    case eSendStep.ReceiveCancel:

                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(23);
                                        trx = SetTrxValue(trx, SendBitOnList);

                                        // eqdTrx[0][0][23].Value = "1";

                                        break;
                                    case eSendStep.ReceiveResume:

                                        trx[20].Value = "1";

                                        //   eqdTrx[0][0][21].Value = "1";

                                        break;
                                    case eSendStep.DownEqTrouble:

                                        // trx[0][0][1].Value = "1";

                                        //  eqdTrx[0][0][1].Value = "1";

                                        break;
                                    case eSendStep.DownPIOTrouble:

                                        trx[1].Value = "1";

                                        // eqdTrx[0][0][1].Value = "1";
                                        break;
                                    case eSendStep.EqTrouble:
                                        trx[0].Value = "0";
                                        trx[1].Value = "1";

                                        //  eqdTrx[0][0][1].Value = "1";
                                        break;

                                    case eSendStep.SendAble:

                                        WriteWEQDJobDta();
                                        Thread.Sleep(Tm);

                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(14);
                                       
                                        trx = SetTrxValue(trx, SendBitOnList);
                                        //   eqdTrx = SetTrxValue(eqdTrx, SendBitOnList);

                                        string timer4ID = "L3_LinkSignalT4Timeout";
                                        if (Timermanager.IsAliveTimer(timer4ID))
                                        {
                                            Timermanager.TerminateTimer(timer4ID);
                                        }
                                        Timermanager.CreateTimer(timer4ID, false, LinkSignalT4, new System.Timers.ElapsedEventHandler(LinkSignalT4TimeoutAction), UtilityMethod.GetAgentTrackKey());

                                        break;

                                    case eSendStep.SendStart:

                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(14);
                                        SendBitOnList.Add(15);
                                        SendBitOnList.Add(24);

                                        trx = SetTrxValue(trx, SendBitOnList);

                                        timer4ID = "L3_LinkSignalT4Timeout";
                                        if (Timermanager.IsAliveTimer(timer4ID))
                                        {
                                            Timermanager.TerminateTimer(timer4ID);
                                        }

                                        if (ParameterManager[eParameterName.SendTimer].GetInteger() != 0)
                                        {
                                            string SendTimer = string.Format("{0}_SendTimer", "L3");

                                            if (Timermanager.IsAliveTimer(SendTimer))
                                            {
                                                Timermanager.TerminateTimer(SendTimer);
                                            }
                                            Timermanager.CreateTimer(SendTimer, false, ParameterManager[eParameterName.SendTimer].GetInteger(),
                                         new System.Timers.ElapsedEventHandler(SendTimerTimeoutAction), UtilityMethod.GetAgentTrackKey());
                                        }
                                        break;

                                    case eSendStep.SendComPlete:

                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(14);
                                        SendBitOnList.Add(15);
                                        SendBitOnList.Add(16);
                                        SendBitOnList.Add(24);

                                        trx = SetTrxValue(trx, SendBitOnList);

                                        eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", trx);

                                        CPCSendOutReport(false, eBitResult.ON);

                                        ////出片解锁
                                        //Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#01");
                                        //EQDDownContrx[0][0][0].Value = "1";
                                        //SendToPLC(EQDDownContrx);


                                        // eqdTrx = SetTrxValue(eqdTrx, SendBitOnList);
                                        break;

                                    case eSendStep.TansferOn:


                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(14);
                                        SendBitOnList.Add(15);

                                        trx = SetTrxValue(trx, SendBitOnList);


                                        // send out Report
                                        break;

                                    case eSendStep.End:

                                        SendBitOnList.Add(0);
                                        trx = SetTrxValue(trx, SendBitOnList);


                                        //CPCSendOutReport(false, eBitResult.ON);
                                        //// send out Report


                                        //eqdTrx = SetTrxValue(eqdTrx, SendBitOnList);

                                        //出片解锁
                                        Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#01");
                                        EQDDownContrx[0][0][0].Value = "1";
                                        SendToPLC(EQDDownContrx);

                                        eqp.File.DownSendComplete = eBitResult.ON;

                                        ObjectManager.EquipmentManager.EnqueueSave(eqp.File);


                                        string SendTime = string.Format("{0}_SendTimer", "L3");

                                        if (Timermanager.IsAliveTimer(SendTime))
                                        {
                                            Timermanager.TerminateTimer(SendTime);
                                        }




                                        break;
                                    case eSendStep.ForceCompleteRequest:
                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(13);
                                        trx = SetTrxValue(trx, SendBitOnList);
                                        break;
                                    case eSendStep.ForceInitialRequest:
                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(14);
                                        trx = SetTrxValue(trx, SendBitOnList);
                                        break;
                                    case eSendStep.DownForceInitialRequestAck:
                                        SendBitOnList.Add(0);
                                        SendBitOnList.Add(15);
                                        trx = SetTrxValue(trx, SendBitOnList);
                                        break;

                                    default:

                                        trx = SetTrxValue(trx, "0");

                                        break;
                                }

                                eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", trx); 
                            }
                          
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }

        }

        //Force Clear Out 上层回流线程
    
        #endregion


        #region Other Funtion

        public bool CheckTRX(Trx newtrx, Trx oldtrx)
        {
            for (int i = 0; i < newtrx[0][0].Items.Count; i++)
            {
                if (newtrx[0][0][i].Value != oldtrx[0][0][i].Value)
                {
                    return true;
                }

            }

            return false;
        }


        public Block SetTrxValue(Block trx, string value)
        {
            if (value == "1" || value == "0")
            {
                foreach (var item in trx.Items)
                {
                    item.Value = value=="1"? "true" : "false";
                }
            }

            return trx;
        }

        public Block SetTrxValue(Block trx, IList<int> OnList)
        {
            for (int i = 0; i < trx.Items.Count; i++)
            {
                if (OnList.Contains(i))
                {
                    trx[i].Value = "true";
                }
                else
                {
                    trx[i].Value = "false";
                }
            }
            return trx;
        }

        public bool CheckUpStreamPIOTrouble(IDictionary<int, string> upPIODic)
        {
            //to do
            return true;
        }


        public bool CheckDownStreamPIOTrouble(IDictionary<int, string> DowmPIODic)
        {
            //to do
            return true;
        }

        public bool CheckEqUpActionTrouble(IDictionary<int, string> UpActionMonitor)
        {
            //to do
            return true;
        }

        public bool CheckEqDownActionTrouble(IDictionary<int, string> DownActionMonitor)
        {
            //to do
            return true;
        }

        public List<int> GetOnBits(IDictionary<int, string> UpStreamInterFace)
        {
            List<int> bitOn = new List<int>();
            for (int i = 1; i <= UpStreamInterFace.Keys.Count; i++)
            {
                if (UpStreamInterFace[i] == "1")
                {
                    bitOn.Add(i);
                }
            }

            return bitOn;

        }

        public bool CheckDownStreamInterFace(IDictionary<int, string> DownStreamInterFace)
        {
            //to do
            return true;

        }

        public void GetUpJobData(Equipment eq)
        {
            try
            {
                Job job;
                Trx trx = GetTrxValues("L3_OtherSendingGlassDataReport#01");
                if (trx != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        job = CreateJob(trx[0][i][0].Value, trx[0][i][1].Value, trx[0][i]);
                        UpdateJobData(eq, job, trx[0][i]);
                    }

                }
                else
                {

                }


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }

        }

        public int JobDataRecipeCheck(Equipment eq)
        {
            //Trx recipeTrx = GetTrxValues("L3_EQDRecipeChangeCommand");
            //Trx actionTRX = GetTrxValues("L3_EQDUpstreamPathControl#01");
            //Trx oldTrx = actionTRX;

            if (eq.Data.NODENAME == "CLEANER")
            {
                return 0;//可以收片

                //#region
                //Trx trx = GetTrxValues("L3_OtherSendingGlassDataReport#01");

                //string hostPPID = trx[0][0]["HostPPID"].Value.Trim();
                //if (!string.IsNullOrEmpty(hostPPID))
                //{
                //    if ((hostPPID != eq.File.CurrentHostPPID) && UnitRecipeRequestBool)
                //    {
                //        UnitRecipeRequestBool = false;

                //        eq.File.DownLoadRecipe = "";

                //        requestCount++;


                //        UnitRecipeRequest(eq, hostPPID);

                //        ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                //        return 1;
                //    }
                //    else
                //    {
                //        Thread.Sleep(1000);

                //        if (!string.IsNullOrEmpty(eq.File.DownLoadRecipe))
                //        {
                //            if (eq.File.DownLoadRecipe != "NG")
                //            {
                //                Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                //                      ObjectManager.RecipeManager.ReloadRecipe();
                //                if (_recipeEntitys[eq.Data.LINEID].ContainsKey(eq.File.DownLoadRecipe))
                //                {

                //                    requestCount = 0;

                //                    return 0;
                //                }
                //                else
                //                {
                //                    requestCount = 0;

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + string.Format("UP Send Job Data HostPPID=[{0}] Unit Recipe Request Reply Recipe=[{1}] Not In RecipeTable!", hostPPID, eq.File.DownLoadRecipe) + "\r\n";

                //                    return 2;
                //                }
                //            }
                //            else {

                //                requestCount = 0;
                //                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Host Unit Recipe Request Reply Recipe IS Empty !" + "\r\n";

                //                return 2;
                //            }
                //        }
                //        else {
                //            eq.File.UPDataCheckResult = string.Format("Unit Recipe Request  Recipe {0} Time !", requestCount);
                //            return 1;
                //        }
                //    }
                //}
                //else
                //{
                //    requestCount = 0;

                //    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "UP Send Job Data HostPPID IS Empty !" + "\r\n";

                //    return 2;
                //}



            }
            else
            {

                return 0;//可以收片

                #region Recipe Auto Change 

                //if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable)
                //{
                //    Trx trx = GetTrxValues("L3_OtherSendingGlassDataReport#01");
                //    if (trx != null)
                //    {
                //        string upDataPpid = trx[0][0]["PPID"].Value.Trim();

                //        string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.Substring(2, 4).ToString();

                //        if (upDataRecipe != eq.File.CurrentRecipeID)
                //        {

                //            if (actionTRX[0][0][1].Value == "0")
                //            {

                //                Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                //                    ObjectManager.RecipeManager.ReloadRecipe();
                //                if (_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataRecipe))
                //                {
                //                    //通知设备切换Recipe 

                //                    //to do eq


                //                    if (recipeTrx != null)
                //                    {
                //                        if (actionTRX != null)
                //                        {
                //                            if (actionTRX[0][0][1].Value == "0")
                //                            {
                //                                recipeTrx[0][0][0].Value = _recipeEntitys[eq.Data.LINEID][upDataRecipe].RECIPENO;
                //                                actionTRX[0][0][1].Value = "1";

                //                                SendToPLC(recipeTrx);
                //                                SendToPLC(actionTRX);

                //                                eq.File.UPDataCheckResult = "Recipe Auto Change In........";
                //                                return 1;
                //                            }

                //                            eq.File.UPDataCheckResult = "Recipe Auto Change In........";
                //                            return 1;
                //                        }
                //                        else
                //                        {
                //                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                //                                string.Format("L3_EQDUpstreamPathControl#01 Is Null!!!"));
                //                            eq.File.UPDataCheckResult = "L3_EQDUpstreamPathControl#01 TRX ERROR";
                //                            return 2;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("L3_EQDRecipeChangeCommand Is Null!!!"));

                //                        eq.File.UPDataCheckResult = "L3_EQDRecipeChangeCommand TRX ERROR";
                //                        return 2;
                //                    }


                //                }
                //                else
                //                {
                //                    //    LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format(
                //                    //        "UpStream Send Out Job Data Recipe ID Mismatch Current Recipe List!!!"));
                //                    //    //Recipe Auto Change 失败，recipe 不存在。

                //                    actionTRX[0][0][10].Value = "1";

                //                    if (CheckTRX(actionTRX, oldTrx))
                //                    {
                //                        SendToPLC(actionTRX);
                //                    }

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + upDataRecipe + " Dont In Recipe Table !!!";
                //                    return 2;
                //                }
                //            }
                //            else
                //            {

                //                // throw new Exception(string.Format("UpStream Send Out Job Data Recipe ID Mismatch Current Recipe ID!!!"));
                //                eq.File.UPDataCheckResult = "Recipe Auto Change In........";
                //                return 1;//正在切换
                //            }
                //        }
                //        else
                //        {
                //            actionTRX[0][0][10].Value = "0";

                //            if (CheckTRX(actionTRX, oldTrx))
                //            {
                //                SendToPLC(actionTRX);
                //            }

                //            return 0;
                //        }


                //    }
                //    else
                //    {
                //        // 收不到时候也返回False
                //        eq.File.UPDataCheckResult = "L3_OtherSendingGlassDataReport#01 TRX ERROR";
                //        return 2;
                //    }

                //}
                //else
                //{
                //    actionTRX[0][0][10].Value = "0";

                //    if (CheckTRX(actionTRX, oldTrx))
                //    {
                //        SendToPLC(actionTRX);
                //    }

                //    return 0;//可以收片
                //}

                #endregion

            }


        }
        public bool JobDataCheck(Equipment eq, out bool NGbyStopBit)
        {
           // return true;//测试

            Trx trx = GetTrxValues("L3_OtherSendingGlassDataReport#01");


            eq.File.UPDataCheckResult = String.Empty;

            if (trx != null)
            {

                #region 弃
                //if (eq.Data.NODENAME == "TEREWORK" || eq.Data.NODENAME == "TSREWORK")
                //{
                //    bool jobA = false;
                //    bool jobB = false;

                //    if (GetOnBits(eq.File.UPInterface).Contains(31))
                //    {
                //        #region  //检查Sequence No是否为空
                //        int c, j;
                //        if (!int.TryParse(trx[0][0]["CassetteSequenceNo"].Value, out c))
                //        {
                //            LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!",
                //                trx[0][0][0].Value));
                //            jobA = false;
                //        }
                //        if (!int.TryParse(trx[0][0]["SlotSequenceNo"].Value, out j))
                //        {
                //            LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Job Sequence No isn't number ({0})!!",
                //                trx[0][0][1].Value));
                //            jobA = false;
                //        }

                //        Trx SequenceNoTRX = GetTrxValues("L3_EQDSequenceNoCheckNGBit");

                //        if (c == 0 || j == 0)
                //        {
                //            SequenceNoTRX[0][0][0].Value = "1";

                //            eq.File.UPDataCheckResult = "Sequence No NG: Cassette Sequence No: " + c.ToString() + " ,Job Sequence No:" + j.ToString() + "\r\n";

                //        }
                //        else
                //        {
                //            SequenceNoTRX[0][0][0].Value = "0";
                //        }

                //        SendToPLC(SequenceNoTRX);

                //        #endregion

                //        #region  //检查Job ID 是否为空

                //        Trx jobIDTRX = GetTrxValues("L3_EQDJobIDCheckNGBit");

                //        if (string.IsNullOrEmpty(trx[0][0]["JobID"].Value.Trim()))
                //        {
                //            jobIDTRX[0][0][0].Value = "1";

                //            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job ID NG: Job ID: " + trx[0][0]["JobID"].Value + "\r\n";

                //        }
                //        else
                //        {
                //            jobIDTRX[0][0][0].Value = "0";
                //        }

                //        SendToPLC(jobIDTRX);

                //        #endregion

                //        #region  //检查Recipe是否为空

                //        Trx ppidTRX = GetTrxValues("L3_EQDPPIDCheckNGBit");

                //        if (string.IsNullOrEmpty(trx[0][0]["PPID#02"].Value.Trim()))
                //        {
                //            ppidTRX[0][0][0].Value = "1";

                //            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "PPID#02 NG: PPID: " + trx[0][0]["PPID#02"].Value.Trim() + "\r\n";

                //        }
                //        else
                //        {
                //            ppidTRX[0][0][0].Value = "0";
                //        }

                //        SendToPLC(ppidTRX);

                //        #endregion

                //        //检查上游设备送过来的资料
                //        if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                //        {


                //            #region  检查Recipe 是否一致

                //            Trx recipeNGTRX = GetTrxValues("L3_EQDRecipeCheckNGBit");
                //            if (eq.File.RecipeCheckMode == eEnableDisable.Enable)
                //            {

                //                string upDataPpid = trx[0][0]["PPID#02"].Value.Trim();

                //                string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                //                if (int.Parse(upDataRecipe) != eq.File.CurrentRecipeNo)
                //                {
                //                    //要通知设备Recipe Check NG.
                //                    recipeNGTRX[0][0][0].Value = "1";

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe NG: " + eq.File.CurrentRecipeNo + " != " + upDataRecipe + "\r\n";
                //                }
                //                else
                //                {
                //                    //Recipe Check OK 
                //                    recipeNGTRX[0][0][0].Value = "0";
                //                }
                //            }
                //            else
                //            {
                //                recipeNGTRX[0][0][0].Value = "0";
                //            }
                //            SendToPLC(recipeNGTRX);


                //            #endregion

                //            #region 检查Recipe Auto Change 

                //            Trx recipeAutoNGTRX = GetTrxValues("L3_EQDRecipeAUTOChangeNGBit");
                //            if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable)
                //            {

                //                string upDataPpid = trx[0][0]["PPID#02"].Value.Trim();

                //                string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                //                Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                //                            ObjectManager.RecipeManager.ReloadRecipeByNo();


                //                if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataPpid))
                //                {
                //                    //要通知设备Recipe Check NG.
                //                    recipeAutoNGTRX[0][0][0].Value = "1";

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe Auto Change NG: " + string.Format("Recipe Table Not Contain {0} ", upDataRecipe) + "\r\n";
                //                }
                //                else
                //                {
                //                    //Recipe Check OK 
                //                    recipeAutoNGTRX[0][0][0].Value = "0";
                //                }
                //            }
                //            else
                //            {
                //                recipeAutoNGTRX[0][0][0].Value = "0";
                //            }
                //            SendToPLC(recipeAutoNGTRX);

                //            #endregion

                //            #region  //检查设备内的Job data 是否有Duplicate 

                //            Trx DupllicateCheck = GetTrxValues("L3_EQDJobDupllicateCheckNGBit");

                //            if (eq.File.JobDuplicateCheckMode == eEnableDisable.Enable)
                //            {
                //                string cstSeq = trx[0][0][eJOBDATA.CassetteSequenceNumber].Value;
                //                string slotNo = trx[0][0][eJOBDATA.SlotSequenceNumber].Value;

                //                Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                //                if (job != null)
                //                {
                //                    //要通知设备 Job Duplicate Check NG.
                //                    DupllicateCheck[0][0][0].Value = "1";

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Duplicate Check  NG: " + cstSeq + ", " + slotNo + "\r\n";

                //                }
                //                else
                //                {
                //                    DupllicateCheck[0][0][0].Value = "0";
                //                }

                //            }
                //            else
                //            {
                //                DupllicateCheck[0][0][0].Value = "0";
                //            }

                //            #endregion


                //            if (eq.File.UPDataCheckResult == string.Empty)
                //            {
                //                jobA = true;
                //            }
                //            else
                //            {
                //                jobA = false;
                //            }
                //        }
                //        else
                //        {
                //            jobA = true;
                //        }


                //    }

                //    if (GetOnBits(eq.File.UPInterface).Contains(32))
                //    {
                //        #region  //检查Sequence No是否为空
                //        int c, j;
                //        if (!int.TryParse(trx[0][1]["CassetteSequenceNo"].Value, out c))
                //        {
                //            LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!",
                //                trx[0][1][0].Value));
                //            jobB = false;
                //        }
                //        if (!int.TryParse(trx[0][1]["SlotSequenceNo"].Value, out j))
                //        {
                //            LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Job Sequence No isn't number ({0})!!",
                //                trx[0][1][1].Value));
                //            jobB = false;
                //        }

                //        Trx SequenceNoTRX = GetTrxValues("L3_EQDSequenceNoCheckNGBit");

                //        if (c == 0 || j == 0)
                //        {
                //            SequenceNoTRX[0][0][0].Value = "1";

                //            eq.File.UPDataCheckResult = "Sequence No NG: Cassette Sequence No: " + c.ToString() + " ,Job Sequence No:" + j.ToString() + "\r\n";

                //        }
                //        else
                //        {
                //            SequenceNoTRX[0][0][0].Value = "0";
                //        }

                //        SendToPLC(SequenceNoTRX);

                //        #endregion

                //        #region  //检查Job ID 是否为空

                //        Trx jobIDTRX = GetTrxValues("L3_EQDJobIDCheckNGBit");

                //        if (string.IsNullOrEmpty(trx[0][1]["JobID"].Value.Trim()))
                //        {
                //            jobIDTRX[0][0][0].Value = "1";

                //            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job ID NG: Job ID: " + trx[0][1]["JobID"].Value + "\r\n";

                //        }
                //        else
                //        {
                //            jobIDTRX[0][0][0].Value = "0";
                //        }

                //        SendToPLC(jobIDTRX);

                //        #endregion

                //        #region  //检查Recipe是否为空

                //        Trx ppidTRX = GetTrxValues("L3_EQDPPIDCheckNGBit");

                //        if (string.IsNullOrEmpty(trx[0][1]["PPID#02"].Value.Trim()))
                //        {
                //            ppidTRX[0][0][0].Value = "1";

                //            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "PPID#02 NG: PPID: " + trx[0][1]["PPID#02"].Value.Trim() + "\r\n";

                //        }
                //        else
                //        {
                //            ppidTRX[0][0][0].Value = "0";
                //        }

                //        SendToPLC(ppidTRX);

                //        #endregion

                //        //检查上游设备送过来的资料
                //        if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                //        {


                //            #region  检查Recipe 是否一致

                //            Trx recipeNGTRX = GetTrxValues("L3_EQDRecipeCheckNGBit");
                //            if (eq.File.RecipeCheckMode == eEnableDisable.Enable)
                //            {

                //                string upDataPpid = trx[0][1]["PPID#02"].Value.Trim();

                //                string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                //                if (int.Parse(upDataRecipe) != eq.File.CurrentRecipeNo)
                //                {
                //                    //要通知设备Recipe Check NG.
                //                    recipeNGTRX[0][0][0].Value = "1";

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe NG: " + eq.File.CurrentRecipeNo + " != " + upDataRecipe + "\r\n";
                //                }
                //                else
                //                {
                //                    //Recipe Check OK 
                //                    recipeNGTRX[0][0][0].Value = "0";
                //                }
                //            }
                //            else
                //            {
                //                recipeNGTRX[0][0][0].Value = "0";
                //            }
                //            SendToPLC(recipeNGTRX);


                //            #endregion

                //            #region 检查Recipe Auto Change 

                //            Trx recipeAutoNGTRX = GetTrxValues("L3_EQDRecipeAUTOChangeNGBit");
                //            if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable)
                //            {

                //                string upDataPpid = trx[0][1]["PPID#02"].Value.Trim();

                //                string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                //                Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                //                            ObjectManager.RecipeManager.ReloadRecipeByNo();


                //                if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataPpid))
                //                {
                //                    //要通知设备Recipe Check NG.
                //                    recipeAutoNGTRX[0][0][0].Value = "1";

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe Auto Change NG: " + string.Format("Recipe Table Not Contain {0} ", upDataRecipe) + "\r\n";
                //                }
                //                else
                //                {
                //                    //Recipe Check OK 
                //                    recipeAutoNGTRX[0][0][0].Value = "0";
                //                }
                //            }
                //            else
                //            {
                //                recipeAutoNGTRX[0][0][0].Value = "0";
                //            }
                //            SendToPLC(recipeAutoNGTRX);

                //            #endregion

                //            #region  //检查设备内的Job data 是否有Duplicate 

                //            Trx DupllicateCheck = GetTrxValues("L3_EQDJobDupllicateCheckNGBit");

                //            if (eq.File.JobDuplicateCheckMode == eEnableDisable.Enable)
                //            {
                //                string cstSeq = trx[0][1][eJOBDATA.CassetteSequenceNumber].Value;
                //                string slotNo = trx[0][1][eJOBDATA.SlotSequenceNumber].Value;

                //                Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                //                if (job != null)
                //                {
                //                    //要通知设备 Job Duplicate Check NG.
                //                    DupllicateCheck[0][0][0].Value = "1";

                //                    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Duplicate Check  NG: " + cstSeq + ", " + slotNo + "\r\n";

                //                }
                //                else
                //                {
                //                    DupllicateCheck[0][0][0].Value = "0";
                //                }

                //            }
                //            else
                //            {
                //                DupllicateCheck[0][0][0].Value = "0";
                //            }

                //            #endregion


                //            if (eq.File.UPDataCheckResult == string.Empty)
                //            {
                //                jobB = true;
                //            }
                //            else
                //            {
                //                jobB = false;
                //            }
                //        }
                //        else
                //        {
                //            jobB = true;
                //        }




                //    }

                //    if (GetOnBits(eq.File.UPInterface).Contains(31) && GetOnBits(eq.File.UPInterface).Contains(32))
                //    {
                //        if (jobA && jobB)
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //    else if (GetOnBits(eq.File.UPInterface).Contains(31))
                //    {
                //        if (jobA)
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }

                //    }
                //    else if (GetOnBits(eq.File.UPInterface).Contains(32))
                //    {
                //        if (jobB)
                //        {
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //    else
                //    {
                //        return false;
                //    }

                //}
                #endregion
                #region  //检查Sequence No是否为空
                if (eq.File.CstSlotNoCheckMode == eEnableDisable.Enable)
                {
                    int c, j;
                    if (!int.TryParse(trx[0][0]["CassetteSequenceNumber"].Value, out c))
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!",
                            trx[0][0][0].Value));

                        NGbyStopBit = false;
                        return false;
                    }
                    if (!int.TryParse(trx[0][0]["CassetteSlotNumber"].Value, out j))
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Job Sequence No isn't number ({0})!!",
                            trx[0][0][1].Value));
                        NGbyStopBit = false;
                        return false;
                    }

                    Trx SequenceNoTRX = GetTrxValues("L3_EQDSequenceNoCheckNGBit");

                    if (c == 0 || j == 0)
                    {
                        SequenceNoTRX[0][0][0].Value = "1";
                        SequenceNoTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(SequenceNoTRX);

                        eq.File.UPDataCheckResult = "Sequence No NG: " + "\r\n" + "Cassette Sequence No = " + c.ToString() + "\r\n" + "Job Sequence No = " + j.ToString() + "!\r\n";
                        string timerID = "L3_EQDSequenceNoCheckNGBit";
                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }
                        Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(JobCheckNGBitAutoReset), UtilityMethod.GetAgentTrackKey());
                        NGbyStopBit = false;
                        return false;
                    }
                    else
                    {
                        SequenceNoTRX[0][0][0].Value = "0";
                    }

                    SendToPLC(SequenceNoTRX);
                }
                #endregion

                #region 检查GroupNumber 
                if (eq.File.GroupNoCheckMode == eEnableDisable.Enable)
                {
                    int c = 0;
                    Trx jobLotIDTRX = GetTrxValues("L3_EQDGroupNumberCheckNGBit");

                    if (int.TryParse(trx[0][0]["GroupNumber"].Value.Trim(), out c) && trx[0][0]["GroupNumber"].Value != eq.File.GroupNo)
                    {

                        if (eq.File.GroupNo != "0")
                        {
                            if (trx[0][0]["GroupNumber"].Value == "0")
                            {
                                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Group Number NG: " + "\r\n" + "Upstream Send Group Number = " + trx[0][0]["GroupNumber"].Value + "!\r\n";
                            }
                            else
                            {
                                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Group Number NG: " + "\r\n" + "Upstream Send Group Number = " + trx[0][0]["GroupNumber"].Value + "!=" + "Equipment Current Group Number " + eq.File.GroupNo + "!\r\n";
                            }
                            jobLotIDTRX[0][0][0].Value = "1";
                            jobLotIDTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
                            SendToPLC(jobLotIDTRX);

                            string timerID = "L3_EQDGroupNumberCheckNGBit";
                            if (Timermanager.IsAliveTimer(timerID))
                            {
                                Timermanager.TerminateTimer(timerID);
                            }
                            Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(JobCheckNGBitAutoReset), UtilityMethod.GetAgentTrackKey());

                            NGbyStopBit = false;
                            return false;
                        }
                        else//设备group no为0，测可以接受任何上游给的group no，并且更新
                        {
                            jobLotIDTRX[0][0][0].Value = "0";
                            lock (eq)
                            {
                                eq.File.GroupNo = trx[0][0]["GroupNumber"].Value;
                            }
                            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                        }
                    }
                    else
                    {

                        jobLotIDTRX[0][0][0].Value = "0";
                    }

                    SendToPLC(jobLotIDTRX);
                }
                #endregion

                #region  //检查Job ID 是否为空
                Trx jobIDTRX = GetTrxValues("L3_EQDJobIDCheckNGBit");
                if (eq.File.GlassIDCheckMode == eEnableDisable.Enable)
                {

                    if (string.IsNullOrEmpty(trx[0][0]["GlassID"].Value.Trim()))
                    {
                        jobIDTRX[0][0][0].Value = "1";
                        jobIDTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(jobIDTRX);

                        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job ID NG: " + "\r\n" + "Job ID Empty" + "!\r\n";
                        string timerID = "L3_EQDJobIDCheckNGBit";
                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }
                        Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(JobCheckNGBitAutoReset), UtilityMethod.GetAgentTrackKey());

                        NGbyStopBit = false;
                        return false;
                    }
                    else
                    {
                        jobIDTRX[0][0][0].Value = "0";
                    }
                }
                else
                {
                    jobIDTRX[0][0][0].Value = "0";
                }
                jobIDTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(jobIDTRX);

                #endregion

                #region  //检查Recipe是否为空

                //Trx ppidTRX = GetTrxValues("L3_EQDPPIDCheckNGBit");

                //if (string.IsNullOrEmpty(trx[0][0]["PPID"].Value.Trim()))
                //{
                //    ppidTRX[0][0][0].Value = "1";

                //    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "PPID NG: PPID: " + trx[0][0]["PPID"].Value.Trim() + "!\r\n";

                //}
                //else
                //{
                //    ppidTRX[0][0][0].Value = "0";
                //}

                //SendToPLC(ppidTRX);

                #endregion

                //#region  //检查run mode和Recipe是否匹配

                ////string upDataPpid01 = trx[0][0]["PPID"].Value.Trim();

                ////string upDataRecipe01 = string.IsNullOrEmpty(upDataPpid01) ? "" : upDataPpid01.Substring(2, 4).ToString();

                ////Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                ////           ObjectManager.RecipeManager.ReloadRecipe();

                ////if ((eq.File.EquipmentRunMode == "2" && _recipeEntitys[eq.Data.LINEID].ContainsKey(upDataRecipe01) &&
                ////    _recipeEntitys[eq.Data.LINEID][upDataRecipe01].Recipe_Work_Mode == "1")|| 
                ////  (eq.File.EquipmentRunMode == "3" && _recipeEntitys[eq.Data.LINEID].ContainsKey(upDataRecipe01) &&
                ////    _recipeEntitys[eq.Data.LINEID][upDataRecipe01].Recipe_Work_Mode == "0") )
                ////{

                ////}
                ////else {
                ////    actionTRX[0][0][11].Value = "1";

                ////    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "RECIPE MISMATECH RUN MODE "  + "\r\n";

                ////}

                //#endregion

                //检查上游设备送过来的资料
                if (eq.File.GlassCheckMode == eEnableDisable.Enable && eq.File.EquipmentRunMode != "3")
                {
                    #region  检查Recipe
                    if (eq.File.AutoRecipeChangeMode == eEnableDisable.Disable)//不自动切配方时检查，不一致就报警
                    {
                        Trx recipeNGTRX = GetTrxValues("L3_EQDRecipeCheckNGBit");
                        if (eq.File.RecipeCheckMode == eEnableDisable.Enable)
                        {

                            string upDataPpid = trx[0][0]["PPID"].Value;
                            string upDataRecipe = string.Empty;
                            if (eq.Data.LINEID == "KWF22005R")
                            {
                                upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString().Substring(2, 16).Trim();
                            }
                            else if (eq.Data.LINEID == "KWF22003L")
                            {
                                upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString().Substring(4, 5).Trim();
                            }
                            else if (eq.Data.LINEID == "KWF22004L")
                            {
                                upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString().Substring(22, 5).Trim();
                            }

                            if (upDataRecipe != eq.File.CurrentRecipeID)
                            {

                                //要通知设备Recipe Check NG.
                                recipeNGTRX[0][0][0].Value = "1";
                                recipeNGTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
                                SendToPLC(recipeNGTRX);
                                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe Check NG: " + "\r\n" + "EQ Current RecipeID = " + eq.File.CurrentRecipeID + "\r\n" + "Upstream Transfer RecipeID = " + upDataRecipe + " " + "!\r\n";
                                string timerID = "L3_EQDRecipeCheckNGBit";
                                if (Timermanager.IsAliveTimer(timerID))
                                {
                                    Timermanager.TerminateTimer(timerID);
                                }
                                Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(JobCheckNGBitAutoReset), UtilityMethod.GetAgentTrackKey());
                                NGbyStopBit = false;
                                return false;
                            }
                            else
                            {

                                //Recipe Check OK 
                                recipeNGTRX[0][0][0].Value = "0";
                            }
                        }
                        else
                        {
                            recipeNGTRX[0][0][0].Value = "0";
                        }
                        SendToPLC(recipeNGTRX);
                    }

                    #endregion

                    #region 检查Recipe Auto Change 

                    Trx recipeAutoNGTRX = GetTrxValues("L3_EQDRecipeAUTOChangeNGBit");
                    if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable)
                    {

                        //清除RecipeIDCheckNG的bit
                        Trx recipeNGTRX = GetTrxValues("L3_EQDRecipeCheckNGBit");
                        if (recipeNGTRX[0][0][0].Value == "1")
                        {
                            recipeNGTRX[0][0][0].Value = "0";
                            SendToPLC(recipeNGTRX);
                        }

                        string upDataPpid = trx[0][0]["PPID"].Value;
                        string upDataRecipe = string.Empty;
                        if (eq.Data.LINEID == "KWF22005R")
                        {
                            upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString().Substring(2, 16).Trim();
                        }
                        else if (eq.Data.LINEID == "KWF22003L")
                        {
                            upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString().Substring(4, 5).Trim();
                        }
                        else if (eq.Data.LINEID == "KWF22004L")
                        {
                            upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString().Substring(22, 5).Trim();
                        }

                        Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                                    ObjectManager.RecipeManager.ReloadRecipe();


                        if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataRecipe))
                        {
                            //要通知设备Recipe Check NG.
                            recipeAutoNGTRX[0][0][0].Value = "1";
                            recipeAutoNGTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
                            SendToPLC(recipeAutoNGTRX);
                            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe Auto Change NG: " + "\r\n" + string.Format("Recipe Table Not Contain RecipeID = {0} ", upDataRecipe) + "!\r\n";
                            string timerID = "L3_EQDRecipeAUTOChangeNGBit";
                            if (Timermanager.IsAliveTimer(timerID))
                            {
                                Timermanager.TerminateTimer(timerID);
                            }
                            Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(JobCheckNGBitAutoReset), UtilityMethod.GetAgentTrackKey());
                            NGbyStopBit = false;
                            return false;
                        }
                        else
                        {
                            //Recipe Check OK 
                            recipeAutoNGTRX[0][0][0].Value = "0";
                        }
                    }
                    else
                    {
                        recipeAutoNGTRX[0][0][0].Value = "0";
                    }
                    SendToPLC(recipeAutoNGTRX);

                    #endregion


                    #region 过期不使用
                    ////检查Product Type是否一致
                    //if (eq.File.ProductTypeCheckMode == eEnableDisable.Enable)
                    //{
                    //    string upDataProductType = trx[0][0]["ProductType"].Value.Trim();

                    //    if (eq.File.ProductType.ToString() != upDataProductType)
                    //    {
                    //        //要通知设备Product Type Check NG.
                    //        actionTRX[0][0][3].Value = "1";

                    //        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Product Type  NG: " + eq.File.ProductType.ToString() + " != " + upDataProductType+ "\r\n";

                    //    }
                    //    else
                    //    {
                    //        actionTRX[0][0][3].Value = "0";
                    //    }

                    //}
                    //else
                    //{
                    //    actionTRX[0][0][3].Value = "0";
                    //}

                    ////检查Group Index是否一致
                    //if (eq.File.GroupIndexCheckMode == eEnableDisable.Enable)
                    //{
                    //    string upDataGroupIndex = trx[0][0]["GroupIndex"].Value.Trim();

                    //    if (eq.File.TotalTFTGlassCount ==0&& eq.File.TotalHFGlassCount ==0 )
                    //    {
                    //        actionTRX[0][0][4].Value = "0";
                    //    }
                    //    else
                    //    {
                    //        if (eq.File.GroupIndex != upDataGroupIndex)
                    //        {
                    //            //要通知设备Group Index Check NG.
                    //            actionTRX[0][0][4].Value = "1";

                    //            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Group Index  NG: " + eq.File.GroupIndex + " != " + upDataGroupIndex + "\r\n";

                    //        }
                    //        else {
                    //            actionTRX[0][0][4].Value = "0";
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    actionTRX[0][0][4].Value = "0";
                    //}
                    #endregion

                    #region  //检查设备内的Job data 是否有Duplicate 
                    if (eq.File.JobDuplicateCheckMode == eEnableDisable.Enable)
                    {
                        Trx DupllicateCheck = GetTrxValues("L3_EQDJobDupllicateCheckNGBit");

                        if (eq.File.JobDuplicateCheckMode == eEnableDisable.Enable)
                        {
                            string cstSeq = trx[0][0][eJOBDATA.CassetteSequenceNumber].Value;
                            string slotNo = trx[0][0][eJOBDATA.SlotSequenceNumber].Value;

                            Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                            if (job != null)
                            {
                                //要通知设备 Job Duplicate Check NG.
                                DupllicateCheck[0][0][0].Value = "1";
                                DupllicateCheck.TrackKey = UtilityMethod.GetAgentTrackKey();
                                SendToPLC(DupllicateCheck);

                                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Duplicate Check NG: " + "\r\n" + "CstSeq: " + cstSeq + "\r\n" + "SlotNo: " + slotNo + "\r\n" + "GlassID: " + trx[0][0][eJOBDATA.GlassID].Value + "\r\n" + "This glass data dumplication!" + "\r\n";
                                string timerID = "L3_EQDJobDupllicateCheckNGBit";
                                if (Timermanager.IsAliveTimer(timerID))
                                {
                                    Timermanager.TerminateTimer(timerID);
                                }
                                Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(JobCheckNGBitAutoReset), UtilityMethod.GetAgentTrackKey());
                                NGbyStopBit = false;
                                return false;
                            }
                            else
                            {
                                DupllicateCheck[0][0][0].Value = "0";
                            }

                        }
                        else
                        {
                            DupllicateCheck[0][0][0].Value = "0";
                        }
                        SendToPLC(DupllicateCheck);
                    }

                    #endregion


                    if (eq.File.StopBitCommand == true)
                    {
                        Trx BCStopBitCommand = GetTrxValues("L3_EQDBCStopBitCommand");
                        //StopCommand时检查GlassType                  
                        if (trx[0][0]["GlassType"].Value != "1" && trx[0][0]["GlassType"].Value != "2")
                        {
                            //dummy 正常收
                            BCStopBitCommand.TrackKey = UtilityMethod.GetAgentTrackKey();
                            BCStopBitCommand[0][0][0].Value = "0";
                            SendToPLC(BCStopBitCommand);
                        }
                        if (trx[0][0]["GlassType"].Value == "1" || trx[0][0]["GlassType"].Value == "2")
                        {
                            //不收片
                            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "BC Stop Bit Command On: " + "\r\n" + string.Format("Current GlassType = {0} Not Dummy Glass", trx[0][0]["GlassType"].Value) + "!\r\n";
                            NGbyStopBit = true;
                            BCStopBitCommand.TrackKey = UtilityMethod.GetAgentTrackKey();
                            BCStopBitCommand[0][0][0].Value = "1";
                            SendToPLC(BCStopBitCommand);
                            string timerID = "L3_EQDBCStopBitCommand";
                            if (Timermanager.IsAliveTimer(timerID))
                            {
                                Timermanager.TerminateTimer(timerID);
                            }
                            Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(JobCheckNGBitAutoReset), UtilityMethod.GetAgentTrackKey());
                            
                            return false;
                        }
                    }

                    //值不同再写入，防止一直刷Log

                    //if (CheckTRX(actionTRX, oldTrx))
                    //{
                    //    SendToPLC(actionTRX);
                    //}

                    if (eq.File.UPDataCheckResult == string.Empty)
                    {
                        NGbyStopBit = false;
                        return true;
                    }
                    else
                    {
                        NGbyStopBit = false;
                        return false;
                    }
                }
                else
                {
                    NGbyStopBit = false;
                    return true;
                }

            }
            else
            {
                NGbyStopBit = false;
                return false;
            }


        }
        //L3_OtherSendingGlassDataReport#01
        public Trx GetTrxValues(string trxName)
        {
            Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(trxName) as Trx;
            if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1} IN PLCFmt.xml!", "L3", trxName));

            trx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { trxName, false }) as Trx;

            return trx;
        }
        // 接片写入Job Data 数据到D区域和W区域
        public void WriteEQDJobData()
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                Trx trx = GetTrxValues("L3_EQDReceiveGlassDataReport#01") as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_EQDReceiveGlassDataReport#01 IN PLCFmt.xml!", "L3"));

                Block receviceTrx = eipTagAccess.ReadBlockValues("RV_EQToEQ_LinkSignal_02_03_00", "JobData#1");

                if (receviceTrx != null)
                {


                    for (int i = 0; i < receviceTrx.Items.Count; i++)
                    {
                        if (receviceTrx[i].Value != null)
                        {
                            trx[0][0][i].Value = receviceTrx[i].Value.ToString().Trim();
                        }

                    }

                    if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable && eq.File.GlassCheckMode == eEnableDisable.Enable)
                    {
                        Dictionary<string, Dictionary<string, RecipeEntityData>> recipeDic = ObjectManager.RecipeManager.ReloadRecipe();

                        RecipeEntityData recipeData = recipeDic[eq.Data.LINEID][receviceTrx["PPID#3"].Value.ToString().Trim()];
                        trx[0][0]["CurrentRecipe"].Value = recipeData.RECIPENO;
                    }
                    else
                    {
                        trx[0][0]["CurrentRecipe"].Value = "0";
                    }

                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);




                    //写入到收片信息区域
                    // Trx receviceJobDataTrx = GetTrx(string.Format("L3_ReceiveGlassDataReport#0{0}", (i + 1).ToString()));
                    Block receviceJobDataTrx = eipTagAccess.ReadBlockValues("SD_EQToCIM_MachineJobEvent_03_01_00", "ReceivedJobDataBlock#1");
                    for (int j = 0; j < receviceTrx.Items.Count; j++)//因为设备Job Data 多一个当前Recipe栏位
                    {
                        receviceJobDataTrx[j].Value = receviceTrx[j].Value;
                    }
                    eipTagAccess.WriteBlockValues("SD_EQToCIM_MachineJobEvent_03_01_00", receviceJobDataTrx);
                    eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "ReceivedJobDataSubBlock", "UpstreamPathNumber", 1);
                }
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        public bool WriteWEQDJobDta()
        {
            try
            {
                Trx eqdata = GetTrxValues("L3_EQDSendingGlassDataReport#01");

                if (eqdata != null)
                {
                    Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");

                    Block sendToDownstreamData = eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "JobData#1");
                    //Trx sendToDownstreamData = GetTrxValues("L3_SendingGlassDataToDownstream");
                    for (int i = 0; i < sendToDownstreamData.Items.Count; i++)
                    {
                        if (i == 60)
                        { 
                        
                        }
                        sendToDownstreamData[i].Value = eqdata[0][0][i].Value;
                    }
                    sendToDownstreamData["ProcessingFlagMachineLocalNo"].Value = sendToDownstreamData["ProcessingFlagMachineLocalNo"].Value.ToString().Substring(0, 13) + "1" + sendToDownstreamData["ProcessingFlagMachineLocalNo"].Value.ToString().Substring(14, 34);//"0000000000000000";
                    
                    eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", sendToDownstreamData);

                   // Trx sendData = GetTrx("L3_SendingGlassDataReport#01");
                    Block sendData = eipTagAccess.ReadBlockValues("SD_EQToCIM_MachineJobEvent_03_01_00", "SentOutJobDataBlock#1");
                    for (int i = 0; i < sendData.Items.Count; i++)
                    {
                        sendData[i].Value = eqdata[0][0][i].Value;
                    }
                    sendData["ProcessingFlagMachineLocalNo"].Value = sendData["ProcessingFlagMachineLocalNo"].Value.ToString().Substring(0, 13) + "1" + sendData["ProcessingFlagMachineLocalNo"].Value.ToString().Substring(14, 34);//"0000000000000000";

                    eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "SentOutJobDataSubBlock", "DownstreamPathNumber",1);
                    eipTagAccess.WriteBlockValues("SD_EQToCIM_MachineJobEvent_03_01_00", sendData);
                 
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }


        }

     
        public Trx GetTrx(string trxName)
        {
            Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(trxName) as Trx;
            if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1} IN PLCFmt.xml!", "L3", trxName));

            return trx;
        }

        public Trx GetTRXDownstreamPath()
        {
            Trx trx = GetTrx("L3_DownstreamPath#01");
            trx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { "L3_DownstreamPath#01", false }) as Trx;
            return trx;
        }

        public void CPCSendOutReport(bool up, eBitResult result)
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");

                List<int> pathNo = new List<int>();
              
                pathNo.Add(1);
          

                SubSendReport(pathNo, result);

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        private void SubSendReport(List<int> pathNos, eBitResult result)
        {
            try
            {
                foreach (int No in pathNos)
                {

                   // Trx sendData = GetTrxValues(string.Format("L3_SendingGlassDataReport#0{0}", No));
                    Block sendData = eipTagAccess.ReadBlockValues("SD_EQToCIM_MachineJobEvent_03_01_00", "SentOutJobDataBlock#1");


                    string eqpNo = "L3";
                    string pathNo = sendData.Type.Split(new char[] { '#' })[1];
                    string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, SendingGlassDataReportTimeout);

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    if (sendData != null)
                    {
                        if (result == eBitResult.OFF)
                        {
                            eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "SentOutJobReport", false );

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format(
                                    "[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Sending Glass Data Report#{2}.",
                                    eqpNo, DateTime.Now.ToString("yyyyMMddHHmmssfff"), pathNo));

                            return;
                        }
                        else
                        {
                            string cstSeq = sendData["LotSequenceNumber"].Value.ToString().Trim();
                            string slotNo = sendData["SlotSequenceNumber"].Value.ToString().Trim();
                            string glassID = sendData["JobID"].Value.ToString().Trim();

                            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                            Job job = ObjectManager.JobManager.GetJob(glassID); //通过glass ID 找到JOB


                            if (job != null)
                            {
                                job.StartTime = job.StartTime == DateTime.MinValue ? DateTime.Now : job.StartTime;
                                //记录玻璃出片的时间
                                job.EndTime = DateTime.Now;

                                Trx processData;
                                int prosition;


                                    processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", 1));

                                    prosition = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Keys.Max();

                                List<ProcessDataReportItem> tempList = new List<ProcessDataReportItem>();

                                foreach (Item item in processData[0][0].Items.AllValues)
                                {
                                    ProcessDataReportItem itemTemp = new ProcessDataReportItem();
                                    itemTemp.Name = item.Name;
                                    itemTemp.Value = item.Value;

                                    tempList.Add(itemTemp);
                                }

                                //添加最后一单元Fecth上报
                                if (!eq.File.FetchJobs.ContainsKey(prosition))
                                {
                                    eq.File.FetchJobs.TryAdd(prosition, job);
                                }


                                eq.File.ProcessDataJobs.Add(job, processData);

                                //eq.File.TackTimeJobs.Add(job);

                                ObjectManager.EquipmentManager.EnqueueSave(eq.File);


                                Timermanager.CreateTimer(timerID, false, T1,
                                    new System.Timers.ElapsedEventHandler(CPCSendingGlassDataReportTimeoutAction),
                                    UtilityMethod.GetAgentTrackKey());
                                Thread.Sleep(600);
                                eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "SentOutJobReport", true);

                                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                  string.Format(
                                      "[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[ON]  Sending Glass Data Report#{2} CST_SEQNO=[{3}] JOB_SEQNO=[{4}] GLASS_ID=[{5}].",
                                      eq.Data.NODENO, UtilityMethod.GetAgentTrackKey(), pathNo, cstSeq, slotNo, glassID));


                                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Deliver.ToString(),
                                    eq.Data.NODEID,
                                    eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");



                                ObjectManager.JobManager.DeleteJob(job);

                            }
                            else
                            {


                                job = CreateJob(cstSeq, slotNo, sendData);
                                UpdateJobData(eq, job, sendData);

                                job.StartTime = DateTime.Now;
                                //记录玻璃出片的时间
                                job.EndTime = DateTime.Now;

                                Trx processData;
                                int prosition;

                                    processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", 1));

                                    prosition = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Keys.Max();
                              
                                List<ProcessDataReportItem> tempList = new List<ProcessDataReportItem>();

                                foreach (Item item in processData[0][0].Items.AllValues)
                                {
                                    ProcessDataReportItem itemTemp = new ProcessDataReportItem();
                                    itemTemp.Name = item.Name;
                                    itemTemp.Value = item.Value;

                                    tempList.Add(itemTemp);
                                }

                                //添加最后一单元Fecth上报

                                if (!eq.File.FetchJobs.ContainsKey(prosition))
                                {
                                    eq.File.FetchJobs.TryAdd(prosition, job);
                                }

                                eq.File.ProcessDataJobs.Add(job, processData);

                                //eq.File.TackTimeJobs.Add(job);

                                Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                                    new System.Timers.ElapsedEventHandler(CPCSendingGlassDataReportTimeoutAction),
                                    UtilityMethod.GetAgentTrackKey());

                                eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "SentOutJobReport", true);

                                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                                  string.Format(
                                      "[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[ON]  Sending Glass Data Report#{2} CST_SEQNO=[{3}] JOB_SEQNO=[{4}] GLASS_ID=[{5}],Job NO Exist ",
                                      eq.Data.NODENO, UtilityMethod.GetAgentTrackKey(), pathNo, cstSeq, slotNo, glassID));


                                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Deliver.ToString(),
                                    eq.Data.NODEID,
                                    eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");



                                ObjectManager.JobManager.DeleteJob(job);

                            }
                        }
                    }


                    Thread.Sleep(1000);
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        public void CPCReceiveJobDataReport(eBitResult result)
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");

                List<int> pathNo = new List<int>();
                pathNo.Add(1);
                SubReceiveJobDataReport(pathNo, result);


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }


        }

        private void SubReceiveJobDataReport(List<int> pathNos, eBitResult result)
        {
            try
            {
                foreach (int No in pathNos)
                {

                   // Trx receiveData = GetTrxValues(string.Format("L3_ReceiveGlassDataReport#0{0}", No));
                    Block receiveData = eipTagAccess.ReadBlockValues("SD_EQToCIM_MachineJobEvent_03_01_00", "ReceivedJobDataBlock#1");

                    string eqpNo = "L3";
                    string pathNo = receiveData.Type.Split(new char[] { '#' })[1];

                    string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, ReceiveGlassDataReportTimeout);

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    if (receiveData != null)
                    {
                        if (result == eBitResult.OFF)
                        {
                            eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "ReceivedJobReport", false);
                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Receive Job Data Report#{2}.",
                                    eqpNo, DateTime.Now.ToString("yyyyMMddHHmmssfff"), pathNo));
                            return;

                        }
                        eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "ReceivedJobDataSubBlock", "UpstreamPathNumber", 1);
                        eipTagAccess.WriteItemValue("SD_EQToCIM_MachineJobEvent_03_01_00", "MachineJobEvent", "ReceivedJobReport", true);

                        Timermanager.CreateTimer(timerID, false, T1,
                            new System.Timers.ElapsedEventHandler(CPCReceiveGlassDataReportTimeoutAction), UtilityMethod.GetAgentTrackKey());



                        #region [拆出PLCAgent Data]  Word
                        string cstSeq = receiveData["LotSequenceNumber"].Value.ToString();
                        string slotNo = receiveData["SlotSequenceNumber"].Value.ToString();
                        string glassID = receiveData["JobID"].Value.ToString().Trim();

                        #endregion

                        Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                        Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);


                        ObjectManager.EquipmentManager.EnqueueSave(eq.File);


                        if (job == null)
                        {


                            job = CreateJob(cstSeq, slotNo, receiveData);
                            UpdateJobData(eq, job, receiveData);
                            //记录玻璃进入设备内的时间
                            job.StartTime = DateTime.Now;

                            ObjectManager.JobManager.AddJob(job);

                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] CIM_MODE=[{2}] Reeive Glass Data Report#{3} CST_SEQNO=[{4}] JOB_SEQNO=[{5}] GLASS_ID=[{6}].",
                                    eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmssfff"), eq.File.CIMMode, pathNo, cstSeq, slotNo, glassID));

                            ObjectManager.JobManager.EnqueueSave(job);
                            ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Receive.ToString(), eq.Data.NODEID, eq.Data.NODENO, "1", "", "", "");
                        }
                        else
                        {
                            //记录玻璃进入设备内的时间
                            job.StartTime = DateTime.Now;

                            UpdateJobData(eq, job, receiveData);
                            LogError(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[ON] CIM_MODE=[{2}] Reeive Glass Data Report#{3} CST_SEQNO=[{4}] JOB_SEQNO=[{5}] GLASS_ID=[{6}],BUT JOB DATA DUMPLICATION, PLEASE CHECK",
                                    eq.Data.NODENO, DateTime.Now.ToString("yyyyMMddHHmmssfff"), eq.File.CIMMode, pathNo, cstSeq, slotNo, glassID));

                            ObjectManager.JobManager.EnqueueSave(job);
                            ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Receive.ToString(), eq.Data.NODEID, eq.Data.NODENO, "1", "", "", "");
                        }

                    }
                }


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }


        #endregion

        private void SendTimerTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Down Send Timer[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.SendTimer].GetInteger().ToString()));

                Trx actionTRX = GetTrxValues("L3_EQDDownSendOutTimeOutBitBit");
                actionTRX[0][0][0].Value = "1";

                SendToPLC(actionTRX);

                string timerID = "L3_EQDDownSendOutTimeOutBitBit";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(BitAutoReset), UtilityMethod.GetAgentTrackKey());


                AlarmEntityData alarm = null;
                alarm = ObjectManager.AlarmManager.GetAlarmProfile(sArray[0], "65502");
                if (alarm != null)
                {
                    alarmDic.Add(alarm, "1");
                }
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
                  string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", sArray[0], trackKey, his.ALARMID, his.ALARMTEXT));
                    ObjectManager.AlarmManager.SaveAlarmHistory(his);
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void UPSendTimerTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] UP Send Timer[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.SendTimer].GetInteger().ToString()));

                Trx actionTRX = GetTrxValues("L3_EQDUpSendOutTimeOutBit");
                actionTRX[0][0][0].Value = "1";

                SendToPLC(actionTRX);



                AlarmEntityData alarm = null;
                alarm = ObjectManager.AlarmManager.GetAlarmProfile(sArray[0], "65502");
                if (alarm != null)
                {
                    alarmDic.Add(alarm, "1");
                }
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
                  string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", sArray[0], trackKey, his.ALARMID, his.ALARMTEXT));
                    ObjectManager.AlarmManager.SaveAlarmHistory(his);
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void ReceiveTimerTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Receive Timer[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.ReceiveTimer].GetInteger().ToString()));

                Trx actionTRX = GetTrxValues("L3_EQDReciveTimeOutBit");
                actionTRX[0][0][0].Value = "1";
                actionTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
                SendToPLC(actionTRX);

                string timerID = "L3_EQDReciveTimeOutBit";
                if(Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, new System.Timers.ElapsedEventHandler(BitAutoReset), UtilityMethod.GetAgentTrackKey());
                


                AlarmEntityData alarm = null;
                alarm = ObjectManager.AlarmManager.GetAlarmProfile(sArray[0], "65505");
                if (alarm != null)
                {
                    alarmDic.Add(alarm, "1");
                }
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
                  string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", sArray[0], trackKey, his.ALARMID, his.ALARMTEXT));
                    ObjectManager.AlarmManager.SaveAlarmHistory(his);
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void SendIntervalTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Send Interval[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.SendInterval].GetInteger().ToString()));

                AlarmEntityData alarm = null;
                alarm = ObjectManager.AlarmManager.GetAlarmProfile(sArray[0], "65504");
                if (alarm != null)
                {
                    alarmDic.Add(alarm, "1");
                }
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
                  string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", sArray[0], trackKey, his.ALARMID, his.ALARMTEXT));
                    ObjectManager.AlarmManager.SaveAlarmHistory(his);
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void SendWaitTimerTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Send Wait Timer[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.SendWaitTimer].GetInteger().ToString()));

                AlarmEntityData alarm = null;
                alarm = ObjectManager.AlarmManager.GetAlarmProfile(sArray[0], "65503");
                if (alarm != null)
                {
                    alarmDic.Add(alarm, "1");
                }
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
                  string.Format("[EQUIPMENT={0}] [EQ -> EC][{1}]  Alarm Report Set AlarmID[{2}]  AlarmText[{3}] ", sArray[0], trackKey, his.ALARMID, his.ALARMTEXT));
                    ObjectManager.AlarmManager.SaveAlarmHistory(his);
                }

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void LinkSignalT2TimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] LinkSignal T2 Timeout Receive Complete Not ON.", sArray[0], trackKey));
            //反馈给PLC
            E2BTimeoutCommand(eBitResult.ON, "LinkSignalT2");
        }

        private void LinkSignalT3TimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] LinkSignal T3 Timeout Transfer Complete Not OFF.", sArray[0], trackKey));

            //反馈给PLC
            E2BTimeoutCommand(eBitResult.ON, "LinkSignalT3");
        }

        private void LinkSignalT4TimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] LinkSignal T4 Timeout Receive Possible Not ON.", sArray[0], trackKey));
            //反馈给PLC
            E2BTimeoutCommand(eBitResult.ON, "LinkSignalT4");
        }

        private void LinkSignalT5TimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] LinkSignal T5 Timeout Receive Complete Not OFF.", sArray[0], trackKey));
            //反馈给PLC
            E2BTimeoutCommand(eBitResult.ON, "LinkSignalT5");
        }

        private void JobCheckNGBitAutoReset(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            Trx trx = GetTrxValues(tmp);
            trx[0][0][0].Value = "0";
            trx.TrackKey = trackKey;
            SendToPLC(trx);
            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] {2} OFF.", sArray[0], trackKey, sArray[1]));
        }
        private void BitAutoReset(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');
            Trx trx = GetTrxValues(tmp);
            trx[0][0][0].Value = "0";
            trx.TrackKey = trackKey;
            SendToPLC(trx);
            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] {2} OFF.", sArray[0], trackKey, sArray[1]));
        }
    }
}
