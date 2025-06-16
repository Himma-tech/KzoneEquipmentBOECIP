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
using System.Xml.Linq;
using System.IO;
using static KZONE.Service.EquipmentService;

namespace KZONE.Service
{
    public partial class EquipmentService : AbstractService
    {

        public bool ReceiveFlag = false;

        public int requestCount = 0;

        public static List<LinkSignalType> Lst_LinkSignal_Type = new List<LinkSignalType>();
        private Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");

        private Equipment eqp = ObjectManager.EquipmentManager.GetEQP("L3");

        private List<int> SendGlassPositionBit = new List<int>();

        private Dictionary<int, bool> PositionStatus = new Dictionary<int, bool>() { { 1, false }, { 2, false } };

        #region Receive Glass Thread
        //下层
        public void ReceiveGlassAction()
        {

            Trx trx = null;
            //Trx eqdTrx = null;
            Trx actionTRX = null;

            IList<int> BitOnList = new List<int>();

            Thread.Sleep(2000);
            LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.DownStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Lower").FirstOrDefault();
            string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["DownStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault().ToString();
            trx = GetTrxValues(trxName);
            string transactionID = string.Empty;
            Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
            while (true)
            {
                try
                {
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!",
                            "L3"));
                    }
                    Thread.Sleep(50);
                    if (_isRuning && eq.File.DeBugMode == eEnableDisable.Disable) //与PLC是否连线
                    {


                        BitOnList.Clear();


                        //if (eq.File.ReceiveStep == eReceiveStep.ReceiveCancel)
                        //{
                        //    Thread.Sleep(2000);
                        //}
                        //if (eq.File.ReceiveStep == eReceiveStep.ReceiveResume)
                        //{
                        //    Thread.Sleep(2000);
                        //}

                        if (eq.File.UpInlineMode == eBitResult.ON) //检查InlineMode
                        {

                            eq.File.NextReceiveStep = eReceiveStep.InlineMode;


                            if (eq.File.UPActionMonitor[3] == "1") //设备收片锁定，玻璃到位，交握信号结束再复位
                            {

                                if (CheckEqUpActionTrouble(eq.File.UPActionMonitor, "Down", eq))
                                {
                                    if (eq.File.UPReciveComplete == eBitResult.OFF)
                                    {

                                        #region 信号判断

                                        if (linkSignalType.LinkType.ToUpper() == "CVTOCV")
                                        {
                                            if ((eqp.File.LoadingStopMode == eEnableDisable.Enable || eqp.File.StopCommand == eBitResult.ON) && eq.File.ReceiveStep == eReceiveStep.InlineMode)
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                            }
                                            //当前状态为InLine On,上游设备为InLine On 或者设备为预放模式
                                            else if ((eq.File.ReceiveStep == eReceiveStep.PerReceive || eq.File.ReceiveStep == eReceiveStep.InlineMode) &&
                                             GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(4) && GetOnBits(eq.File.UPInterface01).Contains(28) && eq.File.UPActionMonitor[1] == "0" && eq.File.UPActionMonitor[2] == "0")
                                            {

                                                //当前收片状态为InlineMode、上游的Upstream Inline、Send Able为ON 说明上游有片要传送过来
                                                // 1、检查进料口是否有玻璃基板，2、检查上游来料信息是否有问题，
                                                if (eq.File.UPActionMonitor[1] == "0")
                                                {
                                                    if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                                                    {
                                                        if (JobDataCheck(eq, linkSignalType))
                                                        {
                                                            int i = JobDataRecipeCheck(eq);

                                                            switch (i)
                                                            {
                                                                case 0:
                                                                    eq.File.NextReceiveStep = eReceiveStep.ReceiveAble;
                                                                    break;

                                                                case 1:
                                                                    eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                                                    break;

                                                                case 2:
                                                                    eq.File.NextReceiveStep = eReceiveStep.JobDataCheckNG;
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            eq.File.NextReceiveStep = eReceiveStep.JobDataCheckNG;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eq.File.NextReceiveStep = eReceiveStep.ReceiveAble;
                                                    }
                                                }
                                                else
                                                {
                                                    //检测到进料口有片
                                                    eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                                    eq.File.UPDataCheckResult = "Input Port Has Glass!!!";
                                                }
                                            }
                                            else if(eq.File.ReceiveStep==eReceiveStep.JobDataCheckNG && eq.File.GlassCheckMode==eEnableDisable.Disable&&eq.File.AutoRecipeChangeMode==eEnableDisable.Disable&&eq.File.JobDuplicateCheckMode==eEnableDisable.Disable)
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveAble && GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(4)
                                                     && GetOnBits(eq.File.UPInterface01).Contains(5) && GetOnBits(eq.File.UPInterface01).Contains(28))
                                            {

                                                eq.File.NextReceiveStep = eReceiveStep.ReceiveStart;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveStart && GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(4) && GetOnBits(eq.File.UPInterface01).Contains(5) && eq.File.UPActionMonitor[7] == "1")
                                            {

                                                eq.File.NextReceiveStep = eReceiveStep.GlassExist;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.GlassExist && GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(4) && GetOnBits(eq.File.UPInterface01).Contains(5) && eq.File.UPActionMonitor[7] == "0")
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.ReceiveComplete;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveComplete && GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(6))
                                            {

                                                eq.File.NextReceiveStep = eReceiveStep.End;
                                            }
                                            else
                                            {
                                                if (eq.File.ReceiveStep != eReceiveStep.BitAllOff)
                                                {
                                                    //上游设备信号异常时，保持状态不变 
                                                    if (eq.File.ReceiveStep != eReceiveStep.SendResume && eq.File.ReceiveStep != eReceiveStep.ReceiveResume)
                                                    {
                                                        eq.File.NextReceiveStep = eq.File.ReceiveStep;
                                                    }
                                                }
                                            }
                                        }
                                        else if (linkSignalType.LinkType.ToUpper() == "RBTOCV")
                                        {
                                            //当前状态为InLine On,上游设备为InLine On 或者设备为预放模式
                                            if ((eq.File.ReceiveStep == eReceiveStep.PerReceive || eq.File.ReceiveStep == eReceiveStep.InlineMode) &&
                                             GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(9) && eq.File.UPActionMonitor[1] == "0" && (eqp.File.StopCommand == eBitResult.OFF && eqp.File.LoadingStopMode == eEnableDisable.Disable))//感应器无片
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.ReceiveAble;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveAble && (eqp.File.StopCommand == eBitResult.ON || eqp.File.LoadingStopMode == eEnableDisable.Enable) && !GetOnBits(eq.File.UPInterface01).Contains(4))
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveAble && GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(4))
                                            {

                                                //当前收片状态为InlineMode、上游的Upstream Inline、Send Able为ON 说明上游有片要传送过来
                                                // 1、检查进料口是否有玻璃基板，2、检查上游来料信息是否有问题，
                                                if (eq.File.UPActionMonitor[1] == "0")
                                                {
                                                    if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                                                    {
                                                        if (JobDataCheck(eq, linkSignalType))
                                                        {
                                                            int i = JobDataRecipeCheck(eq);

                                                            switch (i)
                                                            {
                                                                case 0:
                                                                    eq.File.NextReceiveStep = eReceiveStep.ReceiveStart;
                                                                    break;

                                                                case 1:
                                                                    eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                                                    break;

                                                                case 2:
                                                                    eq.File.NextReceiveStep = eReceiveStep.JobDataCheckNG;
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            eq.File.NextReceiveStep = eReceiveStep.JobDataCheckNG;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eq.File.NextReceiveStep = eReceiveStep.ReceiveStart;
                                                    }
                                                }
                                                else
                                                {
                                                    //检测到进料口有片
                                                    eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                                    eq.File.UPDataCheckResult = "Input Port Has Glass!!!";
                                                }
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.JobDataCheckNG && eq.File.GlassCheckMode == eEnableDisable.Disable && eq.File.AutoRecipeChangeMode == eEnableDisable.Disable && eq.File.JobDuplicateCheckMode == eEnableDisable.Disable)
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveStart && GetOnBits(eq.File.UPInterface01).Contains(1) && GetOnBits(eq.File.UPInterface01).Contains(4)
                                                     && GetOnBits(eq.File.UPInterface01).Contains(5) && GetOnBits(eq.File.UPInterface01).Contains(6))
                                            {

                                                eq.File.NextReceiveStep = eReceiveStep.ReceiveComplete;
                                            }
                                            else if (eq.File.ReceiveStep == eReceiveStep.ReceiveComplete && GetOnBits(eq.File.UPInterface01).Contains(1) && !GetOnBits(eq.File.UPInterface01).Contains(6))
                                            {

                                                eq.File.NextReceiveStep = eReceiveStep.End;
                                            }
                                            else
                                            {
                                                if (eq.File.ReceiveStep != eReceiveStep.BitAllOff)
                                                {
                                                    //上游设备信号异常时，保持状态不变 
                                                    if (eq.File.ReceiveStep != eReceiveStep.SendResume && eq.File.ReceiveStep != eReceiveStep.ReceiveResume)
                                                    {
                                                        eq.File.NextReceiveStep = eq.File.ReceiveStep;
                                                    }
                                                }
                                            }

                                        }

                                        #endregion



                                        if (eq.File.ReceiveStep == eReceiveStep.ReceiveCancel && GetOnBits(eq.File.UPInterface01).Contains(36))//Cancel Ack
                                        {
                                            eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                            eq.File.UPReciveCancel = eBitResult.OFF;
                                        }
                                        //else if (eq.File.ReceiveStep == eReceiveStep.ReceiveResume && GetOnBits(eq.File.UPInterface01).Contains(21))//Resume Ack
                                        //{
                                        //    eq.File.NextReceiveStep = eq.File.DownResum;
                                        //    eq.File.UPReciveResum = eBitResult.OFF;
                                        //}
                                        else if (eq.File.UPReciveCancel == eBitResult.ON)//异常流程Receive Cancel
                                        {
                                            eq.File.NextReceiveStep = eReceiveStep.ReceiveCancel;

                                        }
                                        //else if (eq.File.UPReciveResum == eBitResult.ON)//异常流程Receive Resume
                                        //{
                                        //    eq.File.NextReceiveStep = eReceiveStep.ReceiveResume;

                                        //}
                                        else if (eq.File.ReceiveStep == eReceiveStep.SendCancel)
                                        {
                                            eq.File.NextReceiveStep = eReceiveStep.InlineMode;
                                        }
                                        //else if (eq.File.ReceiveStep == eReceiveStep.SendResume && !GetOnBits(eq.File.UPInterface01).Contains(20))
                                        //{
                                        //    eq.File.NextReceiveStep = eq.File.DownResum;
                                        //}

                                        //if (GetOnBits(eq.File.UPInterface01).Contains(20))//UP Resume
                                        //{
                                        //    eq.File.NextReceiveStep = eReceiveStep.SendResume;
                                        //}
                                        if (GetOnBits(eq.File.UPInterface01).Contains(35))//UP Cancel
                                        {
                                            eq.File.NextReceiveStep = eReceiveStep.SendCancel;
                                        }
                                        if (GetOnBits(eq.File.UPInterface01).Contains(2) && eq.File.ReceiveStep != eReceiveStep.ReceiveAble && eq.File.ReceiveStep != eReceiveStep.InlineMode && eq.File.ReceiveStep != eReceiveStep.BitAllOff) //UP Trouble
                                        {
                                            if (line.Data.FABTYPE != "CF")
                                            {
                                                eq.File.NextReceiveStep = eReceiveStep.UpEqTrouble;
                                            }
                                        }
                                        else if (eq.File.ReceiveStep == eReceiveStep.End)
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
                                eq.File.UPReciveResum = eBitResult.OFF;
                                eq.File.UPReciveCancel = eBitResult.OFF;

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
                        if ((eq.File.ReceiveStep != eq.File.NextReceiveStep && eq.File.DeBugMode != eEnableDisable.Enable) || ReceiveThreadRefresh1)
                        {
                            ReceiveThreadRefresh1 = false;
                            if (eq.File.NextReceiveStep == eReceiveStep.UpEqTrouble || eq.File.NextReceiveStep == eReceiveStep.EqTrouble)
                            {
                                eq.File.DownResum = eq.File.ReceiveStep;
                            }
                            eq.File.ReceiveStep = eq.File.NextReceiveStep;

                            ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                            //if (eq.Data.LINEID == "KWF22090R" || eq.Data.LINEID == "KWF22091R")
                            //{
                            //    trx = GetTrxValues("L3_DownstreamPath#02");
                            //}
                            //else
                            //{
                            //    trx = GetTrxValues("L3_DownstreamPath#01");
                            //}


                            if (eq.Data.NODENAME == "CLEANER")
                            {

                            }
                            else
                            {

                                //Rework 设备信号写入
                                switch (eq.File.NextReceiveStep)
                                {
                                    case eReceiveStep.BitAllOff:

                                        trx = SetTrxValue(trx, "0");

                                        transactionID = string.Empty;
                                        break;

                                    case eReceiveStep.EqTrouble:
                                        trx[0][0][0].Value = "0";
                                        trx[0][0][1].Value = "1";

                                        break;

                                    case eReceiveStep.UpEqTrouble:
                                        trx[0][0][0].Value = "1";
                                        trx[0][0][1].Value = "1";

                                        break;

                                    case eReceiveStep.InlineMode:

                                        BitOnList.Add(0);
                                        trx = SetTrxValue(trx, BitOnList);

                                        //信号初始化时候要清除检查结果
                                        eq.File.UPDataCheckResult = String.Empty;

                                        transactionID = string.Empty;
                                        break;

                                    //预放模式要Receive Ready On
                                    case eReceiveStep.PerReceive:
                                        BitOnList.Add(0);
                                        BitOnList.Add(2);

                                        trx = SetTrxValue(trx, BitOnList);

                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveReadyTime = DateTime.Now;
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveReadyTime = DateTime.Now;
                                            }
                                        }

                                        break;
                                    case eReceiveStep.ReceiveCancel:
                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);
                                        BitOnList.Add(34);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveResume:

                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(19);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendCancel:
                                        BitOnList.Add(0);
                                        BitOnList.Add(35);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendResume:
                                        trx[0][0][0].Value = "1";
                                        trx[0][0][1].Value = "0";
                                        trx[0][0][20].Value = "1";

                                        break;

                                    case eReceiveStep.ReceiveAble:


                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        //预放模式要Receive Ready On
                                        //if (eq.File.UpStreamPerMode == eEnableDisable.Enable)
                                        //{
                                        //    BitOnList.Add(10);
                                        //}
                                        trx = SetTrxValue(trx, BitOnList);
                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                            dicTransferTimes[transactionID].ReceiveAbleTime = DateTime.Now;
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveAbleTime = DateTime.Now;
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveAbleTime = DateTime.Now;
                                            }
                                        }

                                        break;

                                    case eReceiveStep.JobDataCheckNG:

                                        BitOnList.Add(0);
                                        BitOnList.Add(1);
                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveStart:


                                        WriteEQDJobData();

                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);
                                        if (linkSignalType.LinkType.ToUpper() == "CVTOCV")
                                        {
                                            BitOnList.Add(51);
                                        }
                                        trx = SetTrxValue(trx, BitOnList);



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

                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveStartTime = DateTime.Now;
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveStartTime = DateTime.Now;
                                            }
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
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);
                                        BitOnList.Add(27);//玻璃存在
                                        BitOnList.Add(51);//滚轮转动

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
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);
                                        BitOnList.Add(5);
                                        if (linkSignalType.LinkType.ToUpper() == "CVTOCV")
                                        {
                                            BitOnList.Add(27);//玻璃存在
                                            BitOnList.Add(51);//滚轮转动
                                        }
                                        trx = SetTrxValue(trx, BitOnList);
                                        Trx trxGlassData = GetTrxValues("L3_EQDReceiveGlassDataReport#01");
                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveCompleteTime = DateTime.Now;
                                                dicTransferTimes[transactionID].GlassID = trxGlassData[0][0]["GlassID_or_PanelID"].Value.Trim();
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveCompleteTime = DateTime.Now;
                                                dicTransferTimes[transactionID].GlassID = trxGlassData[0][0]["GlassID_or_PanelID"].Value.Trim();
                                            }
                                        }
                                        break;

                                    case eReceiveStep.End:

                                        BitOnList.Add(0);
                                        trx = SetTrxValue(trx, BitOnList);

                                        eq.File.UPReciveComplete = eBitResult.ON;
                                        ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                                        //收片完成解锁
                                        string timerid = "L3_EQDReciveCompleteBit";
                                        if (Timermanager.IsAliveTimer(timerid))
                                        {
                                            Timermanager.TerminateTimer(timerid);
                                        }
                                        Timermanager.CreateTimer(timerid, false, 1000, new System.Timers.ElapsedEventHandler(ReceiveCompleteToEQ), UtilityMethod.GetAgentTrackKey());


                                        //ReceiveTimer  解除
                                        string ReceiveTime = string.Format("{0}_ReceiveTimer", "L3");

                                        if (Timermanager.IsAliveTimer(ReceiveTime))
                                        {
                                            Timermanager.TerminateTimer(ReceiveTime);
                                        }

                                        CPCReceiveJobDataReport(eBitResult.ON, 1);
                                        break;
                                    default:
                                        trx = SetTrxValue(trx, "0");

                                        break;

                                }

                            }
                            SendToPLC(trx);

                        }

                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                    continue;
                }
            }



        }
        //上层
        public void UpReceiveGlassAction()
        {

            Trx trx = null;
            //Trx eqdTrx = null;
            Trx actionTRX = null;

            IList<int> BitOnList = new List<int>();

            Thread.Sleep(2000);
            LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.DownStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Upper").FirstOrDefault();
            string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["DownStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault().ToString();
            trx = GetTrxValues(trxName);
            Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
            while (true)
            {
                try
                {
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!",
                            "L3"));
                    }
                    Thread.Sleep(50);
                    if (_isRuning && eq.File.DeBugMode == eEnableDisable.Disable) //与PLC是否连线
                    {


                        BitOnList.Clear();


                        //if (eq.File.ReceiveStep == eReceiveStep.ReceiveCancel)
                        //{
                        //    Thread.Sleep(2000);
                        //}
                        //if (eq.File.ReceiveStep == eReceiveStep.ReceiveResume)
                        //{
                        //    Thread.Sleep(2000);
                        //}

                        if (eq.File.UpInlineMode == eBitResult.ON) //检查InlineMode
                        {

                            eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;


                            if (eq.File.UPActionMonitor[9] == "1") //设备收片锁定，玻璃到位，交握信号结束再复位
                            {

                                if (CheckEqUpActionTrouble(eq.File.UPActionMonitor, "Up", eq))
                                {
                                    if (eq.File.UPReciveCompleteUP == eBitResult.OFF)
                                    {


                                        #region 信号判断


                                        //当前状态为InLine On,上游设备为InLine On 或者设备为预放模式
                                        if ((eq.File.ReceiveStepUP == eReceiveStep.PerReceive || eq.File.ReceiveStepUP == eReceiveStep.InlineMode) &&
                                         GetOnBits(eq.File.UPInterface02).Contains(1) && GetOnBits(eq.File.UPInterface02).Contains(9) && eq.File.UPActionMonitor[2] == "0")//感应器无片
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.ReceiveAble;
                                        }
                                        else if (eq.File.ReceiveStepUP == eReceiveStep.ReceiveAble && GetOnBits(eq.File.UPInterface02).Contains(1) && GetOnBits(eq.File.UPInterface02).Contains(4))
                                        {

                                            //当前收片状态为InlineMode、上游的Upstream Inline、Send Able为ON 说明上游有片要传送过来
                                            // 1、检查进料口是否有玻璃基板，2、检查上游来料信息是否有问题，
                                            if (eq.File.UPActionMonitor[2] == "0")
                                            {
                                                if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                                                {
                                                    if (JobDataCheck(eq, linkSignalType))
                                                    {
                                                        int i = JobDataRecipeCheck(eq);

                                                        switch (i)
                                                        {
                                                            case 0:
                                                                eq.File.NextReceiveStepUP = eReceiveStep.ReceiveStart;
                                                                break;

                                                            case 1:
                                                                eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;
                                                                break;

                                                            case 2:
                                                                eq.File.NextReceiveStepUP = eReceiveStep.JobDataCheckNG;
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eq.File.NextReceiveStepUP = eReceiveStep.JobDataCheckNG;
                                                    }
                                                }
                                                else
                                                {
                                                    eq.File.NextReceiveStepUP = eReceiveStep.ReceiveStart;
                                                }
                                            }
                                            else
                                            {
                                                //检测到进料口有片
                                                eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;
                                                eq.File.UPDataCheckResult = "Input Port Has Glass!!!";
                                            }
                                        }
                                        else if (eq.File.ReceiveStepUP == eReceiveStep.ReceiveStart && GetOnBits(eq.File.UPInterface02).Contains(1) && GetOnBits(eq.File.UPInterface02).Contains(4)
                                                 && GetOnBits(eq.File.UPInterface02).Contains(5) && GetOnBits(eq.File.UPInterface02).Contains(6))
                                        {

                                            eq.File.NextReceiveStepUP = eReceiveStep.ReceiveComplete;
                                        }
                                        else if (eq.File.ReceiveStepUP == eReceiveStep.ReceiveComplete && GetOnBits(eq.File.UPInterface02).Contains(1) && !GetOnBits(eq.File.UPInterface02).Contains(6))
                                        {

                                            eq.File.NextReceiveStepUP = eReceiveStep.End;
                                        }
                                        else
                                        {
                                            if (eq.File.ReceiveStepUP != eReceiveStep.BitAllOff)
                                            {
                                                //上游设备信号异常时，保持状态不变 
                                                if (eq.File.ReceiveStepUP != eReceiveStep.SendResume && eq.File.ReceiveStepUP != eReceiveStep.ReceiveResume)
                                                {
                                                    eq.File.NextReceiveStepUP = eq.File.ReceiveStepUP;
                                                }
                                            }
                                        }



                                        #endregion



                                        if (eq.File.ReceiveStepUP == eReceiveStep.ReceiveCancel && GetOnBits(eq.File.UPInterface02).Contains(24))//Cancel Ack
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;
                                            eq.File.UPReceiveCancelUP = eBitResult.OFF;
                                        }
                                        else if (eq.File.ReceiveStepUP == eReceiveStep.ReceiveResume && GetOnBits(eq.File.UPInterface02).Contains(21))//Resume Ack
                                        {
                                            eq.File.NextReceiveStepUP = eq.File.DownResum;
                                            eq.File.UPReceiveResumUP = eBitResult.OFF;
                                        }
                                        else if (eq.File.UPReceiveCancelUP == eBitResult.ON)//异常流程Receive Cancel
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.ReceiveCancel;

                                        }
                                        else if (eq.File.UPReceiveResumUP == eBitResult.ON)//异常流程Receive Resume
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.ReceiveResume;

                                        }
                                        else if (eq.File.ReceiveStepUP == eReceiveStep.SendCancel)
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;
                                        }
                                        else if (eq.File.ReceiveStepUP == eReceiveStep.SendResume && !GetOnBits(eq.File.UPInterface02).Contains(20))
                                        {
                                            eq.File.NextReceiveStepUP = eq.File.DownResum;
                                        }

                                        if (GetOnBits(eq.File.UPInterface02).Contains(20))//UP Resume
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.SendResume;
                                        }
                                        else if (GetOnBits(eq.File.UPInterface02).Contains(23))//UP Cancel
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.SendCancel;
                                        }
                                        else if (GetOnBits(eq.File.UPInterface02).Contains(2) && eq.File.ReceiveStepUP != eReceiveStep.ReceiveAble && eq.File.ReceiveStepUP != eReceiveStep.InlineMode && eq.File.ReceiveStepUP != eReceiveStep.BitAllOff) //UP Trouble
                                        {
                                            if (line.Data.FABTYPE != "CF")
                                            {
                                                eq.File.NextReceiveStepUP = eReceiveStep.UpEqTrouble;
                                            }
                                        }
                                        else if (eq.File.ReceiveStepUP == eReceiveStep.End)
                                        {
                                            eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;
                                        }

                                    }
                                    else
                                    {
                                        eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;
                                    }
                                }
                                else
                                {
                                    eq.File.NextReceiveStepUP = eReceiveStep.EqTrouble;
                                }

                            }
                            else
                            {
                                eq.File.NextReceiveStepUP = eReceiveStep.InlineMode;
                                eq.File.UPReceiveResumUP = eBitResult.OFF;
                                eq.File.UPReceiveCancelUP = eBitResult.OFF;

                                //预放模式打开，设备Inline Mode On，设备机构开始动作
                                if (eq.Data.NODENAME != "CLEANER" && eq.File.UpStreamPerMode == eEnableDisable.Enable && eq.File.UPActionMonitor[8] == "1")
                                {
                                    eq.File.NextReceiveStepUP = eReceiveStep.PerReceive;
                                }
                            }
                        }
                        else
                        {
                            eq.File.NextReceiveStepUP = eReceiveStep.BitAllOff;
                        }

                        #region  根据动作写入信号
                        if (eq.File.ReceiveStepUP != eq.File.NextReceiveStepUP && eq.File.DeBugMode != eEnableDisable.Enable)
                        {
                            if (eq.File.NextReceiveStepUP == eReceiveStep.UpEqTrouble || eq.File.NextReceiveStepUP == eReceiveStep.EqTrouble)
                            {
                                eq.File.DownResum = eq.File.ReceiveStepUP;
                            }
                            eq.File.ReceiveStepUP = eq.File.NextReceiveStepUP;

                            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                            //写入W区域
                            //trx = GetTrxValues("L3_DownstreamPath#01");


                            if (eq.Data.NODENAME == "CLEANER")
                            {


                            }
                            else
                            {

                                //Rework 设备信号写入
                                switch (eq.File.NextReceiveStepUP)
                                {
                                    case eReceiveStep.BitAllOff:

                                        trx = SetTrxValue(trx, "0");

                                        break;

                                    case eReceiveStep.EqTrouble:
                                        trx[0][0][0].Value = "0";
                                        trx[0][0][1].Value = "1";

                                        break;

                                    case eReceiveStep.UpEqTrouble:
                                        trx[0][0][0].Value = "1";
                                        trx[0][0][1].Value = "1";

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
                                        BitOnList.Add(2);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveCancel:
                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(22);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveResume:

                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(19);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendCancel:
                                        BitOnList.Add(0);
                                        BitOnList.Add(23);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendResume:
                                        trx[0][0][0].Value = "1";
                                        trx[0][0][1].Value = "0";
                                        trx[0][0][20].Value = "1";

                                        break;

                                    case eReceiveStep.ReceiveAble:


                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        //预放模式要Receive Ready On
                                        if (eq.File.UpStreamPerMode == eEnableDisable.Enable)
                                        {
                                            BitOnList.Add(10);
                                        }
                                        trx = SetTrxValue(trx, BitOnList);

                                        break;

                                    case eReceiveStep.JobDataCheckNG:

                                        BitOnList.Add(0);
                                        BitOnList.Add(1);
                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveStart:


                                        UPWriteEQDJobDta();

                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);

                                        trx = SetTrxValue(trx, BitOnList);



                                        //Receive Timer
                                        if (ParameterManager[eParameterName.ReceiveTimer].GetInteger() != 0)
                                        {
                                            string ReceiveTimer = string.Format("{0}_UpReceiveTimer", "L3");

                                            if (Timermanager.IsAliveTimer(ReceiveTimer))
                                            {
                                                Timermanager.TerminateTimer(ReceiveTimer);
                                            }
                                            Timermanager.CreateTimer(ReceiveTimer, false, ParameterManager[eParameterName.ReceiveTimer].GetInteger(),
                                         new System.Timers.ElapsedEventHandler(UpReceiveTimerTimeoutAction), UtilityMethod.GetAgentTrackKey());
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
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);
                                        BitOnList.Add(5);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;

                                    case eReceiveStep.End:

                                        BitOnList.Add(0);
                                        trx = SetTrxValue(trx, BitOnList);

                                        //收片完成解锁
                                        //actionTRX = GetTrxValues("L3_EQDUpReciveCompleteBit");
                                        //actionTRX[0][0][0].Value = "1";
                                        //actionTRX[0][0].OpDelayTimeMS = 500;
                                        //SendToPLC(actionTRX);
                                        eq.File.UPReciveCompleteUP = eBitResult.ON;
                                        ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                                        string timerid = "L3_EQDReciveCompleteBit";
                                        if (Timermanager.IsAliveTimer(timerid))
                                        {
                                            Timermanager.TerminateTimer(timerid);
                                        }
                                        Timermanager.CreateTimer(timerid, false, 1000, new System.Timers.ElapsedEventHandler(UpReceiveCompleteToEQ), UtilityMethod.GetAgentTrackKey());


                                        //ReceiveTimer  解除
                                        string ReceiveTime = string.Format("{0}_UpReceiveTimer", "L3");

                                        if (Timermanager.IsAliveTimer(ReceiveTime))
                                        {
                                            Timermanager.TerminateTimer(ReceiveTime);
                                        }

                                        CPCReceiveJobDataReport(eBitResult.ON, 1);
                                        break;
                                    default:
                                        trx = SetTrxValue(trx, "0");

                                        break;

                                }

                            }
                            SendToPLC(trx);

                        }

                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                    continue;
                }
            }



        }
        //返流
        public void ReturnReceiveGlassAction()
        {

            Trx trx = null;
            //Trx eqdTrx = null;
            Trx actionTRX = null;

            IList<int> BitOnList = new List<int>();

            Thread.Sleep(2000);
            LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.DownStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Return" && x.PathPosition == "Upper").FirstOrDefault();
            string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["DownStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault().ToString();
            trx = GetTrxValues(trxName);
            string transactionID = string.Empty;
            Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
            while (true)
            {
                try
                {
                    if (eq == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!",
                            "L3"));
                    }
                    Thread.Sleep(50);
                    if (_isRuning && eq.File.DeBugMode == eEnableDisable.Disable) //与PLC是否连线
                    {


                        BitOnList.Clear();


                        //if (eq.File.ReceiveStep == eReceiveStep.ReceiveCancel)
                        //{
                        //    Thread.Sleep(2000);
                        //}
                        //if (eq.File.ReceiveStep == eReceiveStep.ReceiveResume)
                        //{
                        //    Thread.Sleep(2000);
                        //}

                        if (eq.File.UpInlineMode == eBitResult.ON) //检查InlineMode
                        {

                            eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;


                            if (eq.File.UPActionMonitor[11] == "1") //设备收片锁定，玻璃到位，交握信号结束再复位
                            {

                                if (CheckEqUpActionTrouble(eq.File.UPActionMonitor, "Return", eq))
                                {
                                    if (eq.File.ReceiveCompleteReturn == eBitResult.OFF)
                                    {


                                        #region 信号判断



                                        //当前状态为InLine On,上游设备为InLine On 或者设备为预放模式
                                        if ((eq.File.ReceiveStepReturn == eReceiveStep.PerReceive || eq.File.ReceiveStepReturn == eReceiveStep.InlineMode) &&
                                         GetOnBits(eq.File.UPInterface03).Contains(1) && GetOnBits(eq.File.UPInterface03).Contains(9) && eq.File.UPActionMonitor[10] == "0")//感应器无片
                                        {
                                            eq.File.NextReceiveStepReturn = eReceiveStep.ReceiveAble;
                                        }
                                        else if (eq.File.ReceiveStepReturn == eReceiveStep.ReceiveAble && GetOnBits(eq.File.UPInterface03).Contains(1) && GetOnBits(eq.File.UPInterface03).Contains(4))
                                        {

                                            //当前收片状态为InlineMode、上游的Upstream Inline、Send Able为ON 说明上游有片要传送过来
                                            // 1、检查进料口是否有玻璃基板，2、检查上游来料信息是否有问题，
                                            if (eq.File.UPActionMonitor[10] == "0")
                                            {
                                                if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                                                {
                                                    if (JobDataCheck(eq, linkSignalType))
                                                    {
                                                        int i = JobDataRecipeCheck(eq);

                                                        switch (i)
                                                        {
                                                            case 0:
                                                                eq.File.NextReceiveStepReturn = eReceiveStep.ReceiveStart;
                                                                break;

                                                            case 1:
                                                                eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                                                break;

                                                            case 2:
                                                                eq.File.NextReceiveStepReturn = eReceiveStep.JobDataCheckNG;
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eq.File.NextReceiveStepReturn = eReceiveStep.JobDataCheckNG;
                                                    }
                                                }
                                                else
                                                {
                                                    eq.File.NextReceiveStepReturn = eReceiveStep.ReceiveStart;
                                                }
                                            }
                                            else
                                            {
                                                //检测到进料口有片
                                                eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                                eq.File.UPDataCheckResult = "Input Port Has Glass!!!";
                                            }
                                        }
                                        else if (eq.File.ReceiveStepReturn == eReceiveStep.JobDataCheckNG && eq.File.GlassCheckMode == eEnableDisable.Disable && eq.File.AutoRecipeChangeMode == eEnableDisable.Disable && eq.File.JobDuplicateCheckMode == eEnableDisable.Disable)
                                        {
                                            eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                        }
                                        else if (eq.File.ReceiveStepReturn == eReceiveStep.ReceiveStart && GetOnBits(eq.File.UPInterface03).Contains(1) && GetOnBits(eq.File.UPInterface03).Contains(4)
                                                 && GetOnBits(eq.File.UPInterface03).Contains(5) && GetOnBits(eq.File.UPInterface03).Contains(6))
                                        {

                                            eq.File.NextReceiveStepReturn = eReceiveStep.ReceiveComplete;
                                        }
                                        else if (eq.File.ReceiveStepReturn == eReceiveStep.ReceiveComplete && GetOnBits(eq.File.UPInterface03).Contains(1) && !GetOnBits(eq.File.UPInterface03).Contains(6))
                                        {

                                            eq.File.NextReceiveStepReturn = eReceiveStep.End;
                                        }
                                        else
                                        {
                                            if (eq.File.ReceiveStepReturn != eReceiveStep.BitAllOff)
                                            {
                                                //上游设备信号异常时，保持状态不变 
                                                if (eq.File.ReceiveStepReturn != eReceiveStep.SendResume && eq.File.ReceiveStepReturn != eReceiveStep.ReceiveResume)
                                                {
                                                    eq.File.NextReceiveStepReturn = eq.File.ReceiveStepReturn;
                                                }
                                            }
                                        }


                                        #endregion



                                        if (eq.File.ReceiveStepReturn == eReceiveStep.ReceiveCancel && GetOnBits(eq.File.UPInterface03).Contains(36))//Cancel Ack
                                        {
                                            eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                            eq.File.ReceiveCancelReturn = eBitResult.OFF;
                                        }
                                        //else if (eq.File.ReceiveStepReturn == eReceiveStep.ReceiveResume && GetOnBits(eq.File.UPInterface03).Contains(21))//Resume Ack
                                        //{
                                        //    eq.File.NextReceiveStepReturn = eq.File.DownResum;
                                        //    eq.File.ReceiveResumReturn = eBitResult.OFF;
                                        //}
                                        else if (eq.File.ReceiveCancelReturn == eBitResult.ON)//异常流程Receive Cancel
                                        {
                                            eq.File.NextReceiveStepReturn = eReceiveStep.ReceiveCancel;

                                        }
                                        //else if (eq.File.ReceiveResumReturn == eBitResult.ON)//异常流程Receive Resume
                                        //{
                                        //    eq.File.NextReceiveStepReturn = eReceiveStep.ReceiveResume;

                                        //}
                                        else if (eq.File.ReceiveStepReturn == eReceiveStep.SendCancel)
                                        {
                                            eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                        }
                                        //else if (eq.File.ReceiveStepReturn == eReceiveStep.SendResume && !GetOnBits(eq.File.UPInterface03).Contains(20))
                                        //{
                                        //    eq.File.NextReceiveStepReturn = eq.File.DownResum;
                                        //}

                                        //if (GetOnBits(eq.File.UPInterface03).Contains(20))//UP Resume
                                        //{
                                        //    eq.File.NextReceiveStepReturn = eReceiveStep.SendResume;
                                        //}
                                        if (GetOnBits(eq.File.UPInterface03).Contains(35))//UP Cancel
                                        {
                                            eq.File.NextReceiveStepReturn = eReceiveStep.SendCancel;
                                        }
                                        else if (GetOnBits(eq.File.UPInterface03).Contains(2) && eq.File.ReceiveStepReturn != eReceiveStep.ReceiveAble && eq.File.ReceiveStepReturn != eReceiveStep.InlineMode && eq.File.ReceiveStepReturn != eReceiveStep.BitAllOff) //UP Trouble
                                        {
                                            if (line.Data.FABTYPE != "CF")
                                            {
                                                eq.File.NextReceiveStepReturn = eReceiveStep.UpEqTrouble;
                                            }
                                        }
                                        else if (eq.File.ReceiveStepReturn == eReceiveStep.End)
                                        {
                                            eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                        }

                                    }
                                    else
                                    {
                                        eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                    }
                                }
                                else
                                {
                                    eq.File.NextReceiveStepReturn = eReceiveStep.EqTrouble;
                                }

                            }
                            else
                            {
                                eq.File.NextReceiveStepReturn = eReceiveStep.InlineMode;
                                eq.File.ReceiveResumReturn = eBitResult.OFF;
                                eq.File.ReceiveCancelReturn = eBitResult.OFF;

                                //预放模式打开，设备Inline Mode On，设备机构开始动作
                                if (eq.Data.NODENAME != "CLEANER" && eq.File.UpStreamPerMode == eEnableDisable.Enable && eq.File.UPActionMonitor[8] == "1")
                                {
                                    eq.File.NextReceiveStepReturn = eReceiveStep.PerReceive;
                                }
                            }
                        }
                        else
                        {
                            eq.File.NextReceiveStepReturn = eReceiveStep.BitAllOff;
                        }

                        #region  根据动作写入信号
                        if ((eq.File.ReceiveStepReturn != eq.File.NextReceiveStepReturn && eq.File.DeBugMode != eEnableDisable.Enable) || ReceiveThreadRefresh2)
                        {
                            ReceiveThreadRefresh2 = false;
                            if (eq.File.NextReceiveStepReturn == eReceiveStep.UpEqTrouble || eq.File.NextReceiveStepReturn == eReceiveStep.EqTrouble)
                            {
                                eq.File.DownResum = eq.File.ReceiveStepReturn;
                            }
                            eq.File.ReceiveStepReturn = eq.File.NextReceiveStepReturn;

                            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                            //写入W区域
                            //trx = GetTrxValues("L3_DownstreamPath#03");


                            if (eq.Data.NODENAME == "CLEANER")
                            {


                            }
                            else
                            {

                                //Rework 设备信号写入
                                switch (eq.File.NextReceiveStepReturn)
                                {
                                    case eReceiveStep.BitAllOff:

                                        trx = SetTrxValue(trx, "0");
                                        transactionID = string.Empty;
                                        break;

                                    case eReceiveStep.EqTrouble:
                                        trx[0][0][0].Value = "0";
                                        trx[0][0][1].Value = "1";

                                        break;

                                    case eReceiveStep.UpEqTrouble:
                                        trx[0][0][0].Value = "1";
                                        trx[0][0][1].Value = "1";

                                        break;

                                    case eReceiveStep.InlineMode:

                                        BitOnList.Add(0);
                                        trx = SetTrxValue(trx, BitOnList);

                                        //信号初始化时候要清除检查结果
                                        eq.File.UPDataCheckResult = String.Empty;

                                        transactionID = string.Empty;
                                        break;

                                    //预放模式要Receive Ready On
                                    case eReceiveStep.PerReceive:
                                        BitOnList.Add(0);
                                        BitOnList.Add(2);

                                        trx = SetTrxValue(trx, BitOnList);

                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveReadyTime = DateTime.Now;
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveReadyTime = DateTime.Now;
                                            }
                                        }

                                        break;
                                    case eReceiveStep.ReceiveCancel:
                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);
                                        BitOnList.Add(34);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveResume:

                                        BitOnList.Add(0);
                                        BitOnList.Add(14);
                                        BitOnList.Add(15);
                                        BitOnList.Add(19);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendCancel:
                                        BitOnList.Add(0);
                                        BitOnList.Add(35);

                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.SendResume:
                                        trx[0][0][0].Value = "1";
                                        trx[0][0][1].Value = "0";
                                        trx[0][0][20].Value = "1";

                                        break;

                                    case eReceiveStep.ReceiveAble:


                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        //预放模式要Receive Ready On
                                        //if (eq.File.UpStreamPerMode == eEnableDisable.Enable)
                                        //{
                                        //    BitOnList.Add(10);
                                        //}
                                        trx = SetTrxValue(trx, BitOnList);
                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                            dicTransferTimes[transactionID].ReceiveAbleTime = DateTime.Now;
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveAbleTime = DateTime.Now;
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveAbleTime = DateTime.Now;
                                            }
                                        }

                                        break;

                                    case eReceiveStep.JobDataCheckNG:

                                        BitOnList.Add(0);
                                        BitOnList.Add(1);
                                        trx = SetTrxValue(trx, BitOnList);

                                        break;
                                    case eReceiveStep.ReceiveStart:


                                        ReturnWriteEQDJobDta();

                                        BitOnList.Add(0);
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);

                                        trx = SetTrxValue(trx, BitOnList);



                                        //Receive Timer
                                        if (ParameterManager[eParameterName.ReceiveTimer].GetInteger() != 0)
                                        {
                                            string ReceiveTimer = string.Format("{0}_ReturnReceiveTimer", "L3");

                                            if (Timermanager.IsAliveTimer(ReceiveTimer))
                                            {
                                                Timermanager.TerminateTimer(ReceiveTimer);
                                            }
                                            Timermanager.CreateTimer(ReceiveTimer, false, ParameterManager[eParameterName.ReceiveTimer].GetInteger(),
                                         new System.Timers.ElapsedEventHandler(ReturnReceiveTimerTimeoutAction), UtilityMethod.GetAgentTrackKey());
                                        }

                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveStartTime = DateTime.Now;
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveStartTime = DateTime.Now;
                                            }
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
                                        BitOnList.Add(3);
                                        BitOnList.Add(4);
                                        BitOnList.Add(5);

                                        trx = SetTrxValue(trx, BitOnList);
                                        Trx trxGlassData = GetTrxValues("L3_EQDReceiveGlassDataReport#03");
                                        if (string.IsNullOrEmpty(transactionID))
                                        {
                                            transactionID = UtilityMethod.GetAgentTrackKey();
                                        }
                                        if (dicTransferTimes.Count == 0)
                                        {
                                            dicTransferTimes.Add(transactionID, new TransferTime());
                                        }
                                        else if (dicTransferTimes.Count > 0)
                                        {
                                            if (dicTransferTimes.ContainsKey(transactionID))
                                            {
                                                dicTransferTimes[transactionID].ReceiveCompleteTime = DateTime.Now;
                                                dicTransferTimes[transactionID].GlassID = trxGlassData[0][0]["GlassID_or_PanelID"].Value.Trim();
                                            }
                                            else
                                            {
                                                dicTransferTimes.Add(transactionID, new TransferTime());
                                                dicTransferTimes[transactionID].ReceiveCompleteTime = DateTime.Now;
                                                dicTransferTimes[transactionID].GlassID = trxGlassData[0][0]["GlassID_or_PanelID"].Value.Trim();
                                            }
                                        }
                                        break;

                                    case eReceiveStep.End:

                                        BitOnList.Add(0);
                                        trx = SetTrxValue(trx, BitOnList);

                                        //收片完成解锁
                                        //actionTRX = GetTrxValues("L3_EQDReturnReciveCompleteBit");
                                        //actionTRX[0][0][0].Value = "1";
                                        //actionTRX[0][0].OpDelayTimeMS = 500;
                                        //SendToPLC(actionTRX);
                                        eq.File.ReceiveCompleteReturn = eBitResult.ON;
                                        ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                                        string timerid = "L3_EQDReciveCompleteBit";
                                        if (Timermanager.IsAliveTimer(timerid))
                                        {
                                            Timermanager.TerminateTimer(timerid);
                                        }
                                        Timermanager.CreateTimer(timerid, false, 1000, new System.Timers.ElapsedEventHandler(ReturnReceiveCompleteToEQ), UtilityMethod.GetAgentTrackKey());


                                        //ReceiveTimer  解除
                                        string ReceiveTime = string.Format("{0}_ReturnReceiveTimer", "L3");

                                        if (Timermanager.IsAliveTimer(ReceiveTime))
                                        {
                                            Timermanager.TerminateTimer(ReceiveTime);
                                        }

                                        CPCReceiveJobDataReport(eBitResult.ON, 2);
                                        break;
                                    default:
                                        trx = SetTrxValue(trx, "0");

                                        break;

                                }

                            }
                            SendToPLC(trx);

                        }

                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                }
            }



        }

        #endregion

        #region  Send Glass Thread 
        //下层出片,ODF CLN时作为与TR01交互
        public void SendGlassAction()
        {
            Trx trx = null;
            // Trx eqdTrx = null;

            Thread.Sleep(2000);
            LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Lower").FirstOrDefault();
            string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault().ToString();
            trx = GetTrxValues(trxName);
            IList<int> SendBitOnList = new List<int>();

            string glassID = string.Empty;
            string cstSeq = string.Empty;
            string slotNo = string.Empty;
            string transactionID = string.Empty;
            //Job job;

            Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
            if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
            {
                SendGlassPositionBit.Clear();
                if (eqp.File.DownActionMonitor[2] == "1")
                {
                    PositionStatus[1] = true;
                    SendBitOnList.Add(13);
                    SendGlassPositionBit.Add(13);
                    if (eqp.File.DownActionMonitor[3] == "1")
                    {
                        PositionStatus[2] = true;
                        SendBitOnList.Add(11);
                        SendGlassPositionBit.Add(11);
                    }
                }
                else
                {
                    if (eqp.File.DownActionMonitor[3] == "1")
                    {
                        PositionStatus[2] = true;
                        SendBitOnList.Add(14);
                        SendGlassPositionBit.Add(14);
                    }
                }
            }


            while (true)
            {
                try
                {
                    if (eqp == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", "L3"));
                    }


                    Thread.Sleep(50);
                    if (_isRuning && eqp.File.DeBugMode == eEnableDisable.Disable)
                    {
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
                                    if (CheckEqDownActionTrouble(eqp.File.DownActionMonitor, "Down", eqp))
                                    {
                                        if (linkSignalType.LinkType.ToUpper().Trim() == "CVTORB")
                                        {
                                            //if (eqp.File.DownSendCancel == eBitResult.ON)//Send Cancel 
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.SendCancel;
                                            //    eqp.File.DownSendCancel = eBitResult.OFF;
                                            //}
                                            //else if (eqp.File.DownSendResum == eBitResult.ON)//Send Resum 
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.SendResume;
                                            //    eqp.File.DownSendResum = eBitResult.OFF;
                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.ReceiveResume && !GetOnBits(eqp.File.DownInterface01).Contains(20))
                                            //{
                                            //    eqp.File.NextSendStep = eqp.File.UPResum;
                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.ReceiveCancel && !GetOnBits(eqp.File.DownInterface01).Contains(23))
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.InlineMode;
                                            //}
                                            //else if (GetOnBits(eqp.File.DownInterface01).Contains(20))//下游设备Resume
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.ReceiveResume;
                                            //}
                                            //else if (GetOnBits(eqp.File.DownInterface01).Contains(23))//下游设备Cancel
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.ReceiveCancel;
                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.SendCancel && GetOnBits(eqp.File.DownInterface01).Contains(24))
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.InlineMode;

                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.SendResume && GetOnBits(eqp.File.DownInterface01).Contains(21))
                                            //{
                                            //    eqp.File.NextSendStep = eqp.File.UPResum;

                                            //}
                                            if ((eqp.File.SendStep == eSendStep.InlineMode || eqp.File.SendStep == eSendStep.PerSend) && eqp.File.DownSendComplete == eBitResult.OFF && (eqp.File.DownActionMonitor[2] == "1" || eqp.File.DownActionMonitor[3] == "1") && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(9))//欲取模式
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendAble;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(4)
                                                 )
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendStart;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendStart && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(4)
                                                 && GetOnBits(eqp.File.DownInterface01).Contains(5) && (GetOnBits(eqp.File.DownInterface01).Contains(7) || eqp.File.DownActionMonitor[2] == "0"))
                                            {
                                                eqp.File.NextSendStep = eSendStep.TansferOn;//上报Send Out
                                            }

                                            else if (eqp.File.SendStep == eSendStep.TansferOn && GetOnBits(eqp.File.DownInterface01).Contains(6))
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendComPlete;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendComPlete && !GetOnBits(eqp.File.DownInterface01).Contains(6))
                                            {
                                                eqp.File.NextSendStep = eSendStep.End;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.End)
                                            {
                                                eqp.File.NextSendStep = eSendStep.InlineMode;
                                            }
                                            else if (eqp.File.DownInterface01[2] == "1" && eqp.File.SendStep != eSendStep.SendAble && eqp.File.SendStep != eSendStep.InlineMode && eqp.File.SendStep != eSendStep.BitAllOff)
                                            {
                                                eqp.File.NextSendStep = eSendStep.DownEqTrouble;
                                            }
                                            else
                                            {
                                                if (eqp.File.SendStep != eSendStep.BitAllOff)
                                                {
                                                    eqp.File.NextSendStep = eqp.File.SendStep;
                                                }
                                            }
                                        }
                                        else if (linkSignalType.LinkType.ToUpper().Trim() == "CVTOCV")
                                        {
                                            //if (eqp.File.DownSendCancel == eBitResult.ON)//Send Cancel 
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.SendCancel;
                                            //    eqp.File.DownSendCancel = eBitResult.OFF;
                                            //}
                                            //else if (eqp.File.DownSendResum == eBitResult.ON)//Send Resum 
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.SendResume;
                                            //    eqp.File.DownSendResum = eBitResult.OFF;
                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.ReceiveResume && !GetOnBits(eqp.File.DownInterface01).Contains(20))
                                            //{
                                            //    eqp.File.NextSendStep = eqp.File.UPResum;
                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.ReceiveCancel && !GetOnBits(eqp.File.DownInterface01).Contains(23))
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.InlineMode;
                                            //}
                                            //else if (GetOnBits(eqp.File.DownInterface01).Contains(20))//下游设备Resume
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.ReceiveResume;
                                            //}
                                            //else if (GetOnBits(eqp.File.DownInterface01).Contains(23))//下游设备Cancel
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.ReceiveCancel;
                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.SendCancel && GetOnBits(eqp.File.DownInterface01).Contains(24))
                                            //{
                                            //    eqp.File.NextSendStep = eSendStep.InlineMode;

                                            //}
                                            //else if (eqp.File.SendStep == eSendStep.SendResume && GetOnBits(eqp.File.DownInterface01).Contains(21))
                                            //{
                                            //    eqp.File.NextSendStep = eqp.File.UPResum;

                                            //}
                                            if ((eqp.File.SendStep == eSendStep.InlineMode || eqp.File.SendStep == eSendStep.PerSend) && eqp.File.DownSendComplete == eBitResult.OFF && (eqp.File.DownActionMonitor[2] == "1" || eqp.File.DownActionMonitor[3] == "1") && GetOnBits(eqp.File.DownInterface01).Contains(1))//欲取模式
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendAble;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(4)
                                                 )
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendStart;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendStart && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(4)
                                                 && GetOnBits(eqp.File.DownInterface01).Contains(5))
                                            {
                                                eqp.File.NextSendStep = eSendStep.TansferOn;//上报Send Out
                                            }

                                            else if (eqp.File.SendStep == eSendStep.TansferOn && GetOnBits(eqp.File.DownInterface01).Contains(6))
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendComPlete;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendComPlete && !GetOnBits(eqp.File.DownInterface01).Contains(6))
                                            {
                                                eqp.File.NextSendStep = eSendStep.End;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.End)
                                            {
                                                eqp.File.NextSendStep = eSendStep.InlineMode;
                                            }
                                            else if (eqp.File.DownInterface01[2] == "1" && eqp.File.SendStep != eSendStep.SendAble && eqp.File.SendStep != eSendStep.InlineMode && eqp.File.SendStep != eSendStep.BitAllOff)
                                            {
                                                eqp.File.NextSendStep = eSendStep.DownEqTrouble;
                                            }
                                            else
                                            {
                                                if (eqp.File.SendStep != eSendStep.BitAllOff)
                                                {
                                                    eqp.File.NextSendStep = eqp.File.SendStep;
                                                }
                                            }
                                        }
                                        else if (linkSignalType.LinkType.ToUpper().Trim() == "CVTORBDUAL")
                                        {
                                            if ((eqp.File.SendStep == eSendStep.InlineMode || eqp.File.SendStep == eSendStep.PerSend) && eqp.File.DownSendComplete == eBitResult.OFF && (eqp.File.DownActionMonitor[2] == "1" || eqp.File.DownActionMonitor[3] == "1") && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(9))//欲取模式
                                            {

                                                if (eq.File.TR01TransferEnable == eEnableDisable.Enable && eq.File.TR02TransferEnable == eEnableDisable.Disable)
                                                {
                                                    if (CurrentGlassSendToTR != eTransfer.TR01)
                                                    {
                                                        if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag && eq.File.SendStepUP != eSendStep.SendAble)
                                                        {
                                                            TR01_TR02_TransferRequestFlag = true;
                                                            TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eqp.File.NextSendStep = eSendStep.SendAble;
                                                    }
                                                }
                                                else if (eq.File.TR01TransferEnable == eEnableDisable.Enable && eq.File.TR02TransferEnable == eEnableDisable.Enable && eqp.File.DownSendCompleteUP == eBitResult.OFF)
                                                {
                                                    if (eqp.File.LastRequestTransfer == eTransfer.TR02)
                                                    {
                                                        if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag)
                                                        {
                                                            TR01_TR02_TransferRequestFlag = true;
                                                            TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                        }
                                                    }
                                                    else if (eqp.File.LastRequestTransfer == eTransfer.TR01)
                                                    {
                                                        if (CurrentGlassSendToTR != eTransfer.TR01)
                                                        {
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            eqp.File.NextSendStep = eSendStep.SendAble;
                                                        }
                                                    }
                                                    else if (eqp.File.LastRequestTransfer == eTransfer.NONE)
                                                    {
                                                        if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag)
                                                        {
                                                            TR01_TR02_TransferRequestFlag = true;
                                                            TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                        }
                                                    }
                                                }
                                                else if (eq.File.TR01TransferEnable == eEnableDisable.Disable && eq.File.TR02TransferEnable == eEnableDisable.Disable)
                                                {
                                                    eqp.File.NextSendStep = eSendStep.InlineMode;
                                                }
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(4)
                                                 )
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendStart;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendStart && GetOnBits(eqp.File.DownInterface01).Contains(1) && GetOnBits(eqp.File.DownInterface01).Contains(4)
                                                 && GetOnBits(eqp.File.DownInterface01).Contains(5) && CheckTransferOnForCVToRBDual_TR01())
                                            {
                                                eqp.File.NextSendStep = eSendStep.TansferOn;//上报Send Out
                                            }

                                            else if (eqp.File.SendStep == eSendStep.TansferOn && GetOnBits(eqp.File.DownInterface01).Contains(6))
                                            {
                                                eqp.File.NextSendStep = eSendStep.SendComPlete;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.SendComPlete && !GetOnBits(eqp.File.DownInterface01).Contains(6))
                                            {
                                                eqp.File.NextSendStep = eSendStep.End;
                                            }
                                            else if (eqp.File.SendStep == eSendStep.End)
                                            {
                                                eqp.File.NextSendStep = eSendStep.InlineMode;
                                            }
                                            else if (eqp.File.DownInterface01[2] == "1" && eqp.File.SendStep != eSendStep.SendAble && eqp.File.SendStep != eSendStep.InlineMode && eqp.File.SendStep != eSendStep.BitAllOff)
                                            {
                                                if (line.Data.FABTYPE != "CF")
                                                {
                                                    eqp.File.NextSendStep = eSendStep.DownEqTrouble;
                                                }
                                            }
                                            else
                                            {
                                                if (eqp.File.SendStep != eSendStep.BitAllOff)
                                                {
                                                    eqp.File.NextSendStep = eqp.File.SendStep;
                                                }
                                            }
                                        }
                                        if (eqp.File.DownSendCancel == eBitResult.ON)//Send Cancel 
                                        {
                                            eqp.File.NextSendStep = eSendStep.SendCancel;
                                            eqp.File.DownSendCancel = eBitResult.OFF;
                                        }
                                        else if (eqp.File.SendStep == eSendStep.ReceiveCancel && !GetOnBits(eqp.File.DownInterface01).Contains(35))
                                        {
                                            eqp.File.NextSendStep = eSendStep.InlineMode;
                                        }
                                        else if (GetOnBits(eqp.File.DownInterface01).Contains(35))//下游设备Cancel
                                        {
                                            eqp.File.NextSendStep = eSendStep.ReceiveCancel;
                                        }
                                        else if (eqp.File.SendStep == eSendStep.SendCancel && GetOnBits(eqp.File.DownInterface01).Contains(36))
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
                                eqp.File.DownSendResum = eBitResult.OFF;
                                eqp.File.DownSendCancel = eBitResult.OFF;

                                //检查设备是否为欲取模式
                                if (eqp.File.DowmStreamPerMode == eEnableDisable.Enable && eqp.File.DownActionMonitor[7] == "1")
                                {
                                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                                    {
                                        if (eq.File.TR01TransferEnable == eEnableDisable.Enable && eq.File.TR02TransferEnable == eEnableDisable.Disable)
                                        {
                                            if (CurrentGlassSendToTR != eTransfer.TR01)
                                            {
                                                if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag)
                                                {
                                                    TR01_TR02_TransferRequestFlag = true;
                                                    TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                }
                                            }
                                            else
                                            {
                                                eqp.File.NextSendStep = eSendStep.PerSend;
                                            }
                                        }
                                        else if (eq.File.TR01TransferEnable == eEnableDisable.Enable && eq.File.TR02TransferEnable == eEnableDisable.Enable)
                                        {
                                            if (eqp.File.LastRequestTransfer == eTransfer.TR02)
                                            {
                                                if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag && eq.File.SendStepUP != eSendStep.PerSend)
                                                {
                                                    TR01_TR02_TransferRequestFlag = true;
                                                    TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                }
                                            }
                                            else if (eqp.File.LastRequestTransfer == eTransfer.TR01)
                                            {
                                                if (CurrentGlassSendToTR != eTransfer.TR01)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    eqp.File.NextSendStep = eSendStep.PerSend;
                                                }
                                            }
                                            else if (eqp.File.LastRequestTransfer == eTransfer.NONE)
                                            {
                                                if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag)
                                                {
                                                    TR01_TR02_TransferRequestFlag = true;
                                                    TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                }
                                            }

                                        }
                                        else if (eq.File.TR01TransferEnable == eEnableDisable.Disable && eq.File.TR02TransferEnable == eEnableDisable.Disable)
                                        {
                                            eqp.File.NextSendStep = eSendStep.InlineMode;
                                        }
                                    }
                                    else
                                    {
                                        eqp.File.NextSendStep = eSendStep.PerSend;
                                    }
                                }
                            }
                        }
                        else
                        {
                            eqp.File.NextSendStep = eSendStep.BitAllOff;
                            TR01_TR02_TransferRequestFlag = false;
                            TR01_TR02_TransferRequestReplyWaitFlag = false;
                            CurrentGlassSendToTR = eTransfer.NONE;
                        }


                        if ((eqp.File.SendStep != eqp.File.NextSendStep && eqp.File.DeBugMode != eEnableDisable.Enable) || SendThreadRefresh1)
                        {
                            SendThreadRefresh1 = false;
                            if (eqp.File.NextSendStep == eSendStep.DownEqTrouble || eqp.File.NextSendStep == eSendStep.EqTrouble)
                            {
                                eqp.File.UPResum = eqp.File.SendStep;
                            }

                            eqp.File.SendStep = eqp.File.NextSendStep;

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                            //写入W区域
                            //if (eqp.Data.LINEID == "KWF22090R" || eqp.Data.LINEID == "KWF22091R")
                            //{
                            //    trx = GetTrxValues("L3_UpstreamPath#03");
                            //}
                            //else if (eqp.Data.LINEID == "KWF22092R" || eqp.Data.LINEID == "KWF22093L")
                            //{
                            //    trx = GetTrxValues("L3_UpstreamPath#02");
                            //}
                            //else
                            //{
                            //    trx = GetTrxValues("L3_UpstreamPath#01");
                            //}
                            Trx eqdata = GetTrxValues("L3_EQDSendingGlassDataReport#01");

                            //KeyValuePair<string, TransferTime> res = new KeyValuePair<string, TransferTime>();
                            switch (eqp.File.NextSendStep)
                            {

                                case eSendStep.BitAllOff:

                                    trx = SetTrxValue(trx, "0");

                                    //    eqdTrx = SetTrxValue(eqdTrx, "0");
                                    break;
                                case eSendStep.InlineMode:

                                    SendBitOnList.Add(0);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    //  eqdTrx = SetTrxValue(eqdTrx, SendBitOnList);

                                    break;
                                case eSendStep.PerSend:
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        PositionStatus[1] = false;
                                        PositionStatus[2] = false;
                                        if (eqp.File.DownActionMonitor[2] == "1")
                                        {
                                            PositionStatus[1] = true;
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;
                                            }
                                        }
                                        else
                                        {
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;

                                            }
                                        }
                                    }
                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(2);
                                    trx = SetTrxValue(trx, SendBitOnList);
                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.PerSend);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.PerSend);
                                    }

                                    //glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                    //res = dicTransferTimes.Where(x => x.Value.GlassID == glassID).FirstOrDefault();
                                    //job = ObjectManager.JobManager.GetJob(glassID);
                                    //if (res.Key != null && res.Value != null)
                                    //{
                                    //    dicTransferTimes[res.Key].SendReadyTime = DateTime.Now;
                                    //    transactionID = res.Key;
                                    //}
                                    //else
                                    //{
                                    //    if (job == null)
                                    //    {
                                    //        transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    //        dicTransferTimes.Add(transactionID, new TransferTime());
                                    //        dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                                    //        dicTransferTimes[transactionID].GlassID = glassID;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (string.IsNullOrEmpty(job.TransactionID))
                                    //        {
                                    //            transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    //            dicTransferTimes.Add(transactionID, new TransferTime());
                                    //            dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                                    //            dicTransferTimes[transactionID].GlassID = glassID;
                                    //        }
                                    //        else
                                    //        {
                                    //            transactionID = job.TransactionID;
                                    //            if (dicTransferTimes.ContainsKey(transactionID))
                                    //            {
                                    //                dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                                    //            }
                                    //            else
                                    //            {
                                    //                dicTransferTimes.Add(transactionID, new TransferTime());
                                    //                dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                                    //                dicTransferTimes[transactionID].GlassID = glassID;
                                    //            }
                                    //        }
                                    //        dicTransferTimes[transactionID].ReceiveAbleTime = job.ReceiveAbleTime;
                                    //        dicTransferTimes[transactionID].ReceiveStartTime = job.ReceiveStartTime;
                                    //        dicTransferTimes[transactionID].ReceiveCompleteTime = job.ReceiveCompleteTime;
                                    //        dicTransferTimes[transactionID].ReceiveReadyTime = job.ReceiveReadyTime;
                                    //    }

                                    //}

                                    break;
                                case eSendStep.SendCancel:
                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "0";
                                    trx[0][0][3].Value = "1";
                                    trx[0][0][4].Value = "1";
                                    trx[0][0][34].Value = "1";

                                    //    eqdTrx[0][0][22].Value = "1";
                                    break;
                                case eSendStep.SendResume:
                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "0";
                                    trx[0][0][14].Value = "1";
                                    trx[0][0][15].Value = "1";
                                    trx[0][0][19].Value = "1";

                                    //  eqdTrx[0][0][19].Value = "1";
                                    break;

                                case eSendStep.ReceiveCancel:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(35);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    // eqdTrx[0][0][23].Value = "1";

                                    break;
                                case eSendStep.ReceiveResume:
                                    //trx[0][0][0].Value = "1";
                                    trx[0][0][20].Value = "1";

                                    //   eqdTrx[0][0][21].Value = "1";

                                    break;
                                case eSendStep.DownEqTrouble:
                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "1";

                                    //  eqdTrx[0][0][1].Value = "1";

                                    break;
                                case eSendStep.DownPIOTrouble:

                                    trx[0][0][1].Value = "1";

                                    // eqdTrx[0][0][1].Value = "1";
                                    break;
                                case eSendStep.EqTrouble:
                                    trx[0][0][0].Value = "0";
                                    trx[0][0][1].Value = "1";

                                    //  eqdTrx[0][0][1].Value = "1";
                                    break;

                                case eSendStep.SendAble:

                                    WriteWEQDJobDta();

                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        SendGlassPositionBit.Clear();
                                        PositionStatus[1] = false;
                                        PositionStatus[2] = false;
                                        if (eqp.File.DownActionMonitor[2] == "1")
                                        {
                                            PositionStatus[1] = true;
                                            SendBitOnList.Add(13);
                                            SendGlassPositionBit.Add(13);
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;
                                                SendBitOnList.Add(11);
                                                SendGlassPositionBit.Add(11);
                                            }
                                        }
                                        else
                                        {
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;
                                                SendBitOnList.Add(14);
                                                SendGlassPositionBit.Add(14);
                                            }
                                        }
                                    }

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    if (linkSignalType.LinkType.ToUpper() == "CVTOCV")
                                    {
                                        SendBitOnList.Add(27);
                                    }
                                    trx = SetTrxValue(trx, SendBitOnList);
                                    //   eqdTrx = SetTrxValue(eqdTrx, SendBitOnList);
                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendAble);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendAble);
                                    }
                                    //glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                    //job = ObjectManager.JobManager.GetJob(glassID);
                                    //res = dicTransferTimes.Where(x => x.Value.GlassID == glassID).FirstOrDefault();
                                    //if (res.Key != null && res.Value != null)
                                    //{
                                    //    dicTransferTimes[res.Key].SendAbleTime = DateTime.Now;
                                    //    transactionID = res.Key;
                                    //}
                                    //else
                                    //{
                                    //    if (job == null)
                                    //    {
                                    //        transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    //        dicTransferTimes.Add(transactionID, new TransferTime());
                                    //        dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                                    //        dicTransferTimes[transactionID].GlassID = glassID;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (string.IsNullOrEmpty(job.TransactionID))
                                    //        {
                                    //            transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    //            dicTransferTimes.Add(transactionID, new TransferTime());
                                    //            dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                                    //            dicTransferTimes[transactionID].GlassID = glassID;
                                    //        }
                                    //        else
                                    //        {
                                    //            transactionID = job.TransactionID;
                                    //            if (dicTransferTimes.ContainsKey(transactionID))
                                    //            {
                                    //                dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                                    //            }
                                    //            else
                                    //            {
                                    //                dicTransferTimes.Add(transactionID, new TransferTime());
                                    //                dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                                    //                dicTransferTimes[transactionID].GlassID = glassID;
                                    //            }
                                    //        }
                                    //        dicTransferTimes[transactionID].ReceiveAbleTime = job.ReceiveAbleTime;
                                    //        dicTransferTimes[transactionID].ReceiveStartTime = job.ReceiveStartTime;
                                    //        dicTransferTimes[transactionID].ReceiveCompleteTime = job.ReceiveCompleteTime;
                                    //        dicTransferTimes[transactionID].ReceiveReadyTime = job.ReceiveReadyTime;
                                    //    }
                                    //}
                                    break;

                                case eSendStep.SendStart:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    SendBitOnList.Add(4);
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < SendGlassPositionBit.Count; i++)
                                        {
                                            SendBitOnList.Add(SendGlassPositionBit[i]);
                                        }
                                    }
                                    if (linkSignalType.LinkType.ToUpper() == "CVTOCV")
                                    {
                                        SendBitOnList.Add(27);
                                    }
                                    trx = SetTrxValue(trx, SendBitOnList);

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
                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendStart);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendStart);
                                    }

                                    //if (dicTransferTimes.ContainsKey(transactionID))
                                    //{
                                    //    dicTransferTimes[transactionID].SendStartTime = DateTime.Now;
                                    //}
                                    //else
                                    //{
                                    //    transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    //    dicTransferTimes.Add(transactionID, new TransferTime());
                                    //    dicTransferTimes[transactionID].SendStartTime = DateTime.Now;
                                    //}

                                    break;

                                case eSendStep.SendComPlete:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    SendBitOnList.Add(4);
                                    SendBitOnList.Add(5);
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < SendGlassPositionBit.Count; i++)
                                        {
                                            SendBitOnList.Add(SendGlassPositionBit[i]);
                                        }
                                    }
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    //出片解锁,使CELL可提前下降
                                    if (line.Data.FABTYPE=="CELL")
                                    {
                                        Trx EQDDownContrxtoCELL = GetTrxValues("L3_EQDDownstreamPathControl#01");
                                        EQDDownContrxtoCELL[0][0][1].Value = "1";
                                        SendToPLC(EQDDownContrxtoCELL);
                                    }
                                    



                                    // eqdTrx = SetTrxValue(eqdTrx, SendBitOnList);

                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendComPlete);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendComPlete);
                                    }

                                    //if (dicTransferTimes.ContainsKey(transactionID))
                                    //{
                                    //    dicTransferTimes[transactionID].SendCompleteTime = DateTime.Now;
                                    //}
                                    //else
                                    //{
                                    //    transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                    //    dicTransferTimes.Add(transactionID, new TransferTime());
                                    //    dicTransferTimes[transactionID].SendCompleteTime = DateTime.Now;
                                    //}
                                    if (linkSignalType.LinkType.ToUpper().Trim() == "CVTOCV")
                                    {
                                        CPCSendOutReport(false, eBitResult.ON, string.Empty);
                                    }
                                    break;

                                case eSendStep.TansferOn:


                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    SendBitOnList.Add(4);
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < SendGlassPositionBit.Count; i++)
                                        {
                                            SendBitOnList.Add(SendGlassPositionBit[i]);
                                        }
                                    }
                                    if (linkSignalType.LinkType.ToUpper() == "CVTOCV")
                                    {
                                        SendBitOnList.Add(27);
                                        SendBitOnList.Add(51);
                                    }

                                    trx = SetTrxValue(trx, SendBitOnList);
                                    if (linkSignalType.LinkType.ToUpper().Trim() != "CVTOCV")
                                    {
                                        CPCSendOutReport(false, eBitResult.ON, string.Empty);
                                    }

                                    // send out Report
                                    break;

                                case eSendStep.End:

                                    SendBitOnList.Add(0);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    //出片解锁
                                    Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#01");
                                    EQDDownContrx[0][0][0].Value = "1";
                                    SendToPLC(EQDDownContrx);
                                    lsGlassID.Clear();
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {

                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                if (!lsGlassID.Contains(glassID))
                                                {
                                                    lsGlassID.Add(glassID);
                                                }
                                            }
                                        }
                                        CurrentGlassSendToTR = eTransfer.NONE;
                                        if (TR01_TR02_TransferRequestReplyWaitFlag)
                                            TR01_TR02_TransferRequestReplyWaitFlag = false;
                                        if (TR01_TR02_TransferRequestFlag)
                                            TR01_TR02_TransferRequestFlag = false;
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        lsGlassID.Add(glassID);
                                    }
                                    TransferTimeDataReportFlag = true;
                                    //glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                    //cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                    //slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                    //TransferTimeDataReport(eBitResult.ON, glassID, cstSeq, slotNo);

                                    if (eqp.File.LotEndGlassInfos.ContainsKey(glassID))
                                    {
                                        LotEndGlassInfos lotEndGlassInfos = new LotEndGlassInfos();
                                        eqp.File.LotEndGlassInfos.TryRemove(glassID, out lotEndGlassInfos);
                                        LotEndGlassReportToDownstream(eBitResult.ON, lotEndGlassInfos.LotID, lotEndGlassInfos.Cassette_Sequence_No, lotEndGlassInfos.Slot_Sequence_No, lotEndGlassInfos.GlassID);
                                    }
                                    //CPCSendOutReport(false, eBitResult.ON);
                                    //// send out Report


                                    //eqdTrx = SetTrxValue(eqdTrx, SendBitOnList);

                                    ////出片解锁
                                    //Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#01");
                                    //EQDDownContrx[0][0][0].Value = "1";
                                    //SendToPLC(EQDDownContrx);

                                    eqp.File.DownSendComplete = eBitResult.ON;

                                    ObjectManager.EquipmentManager.EnqueueSave(eqp.File);


                                    string SendTime = string.Format("{0}_SendTimer", "L3");

                                    if (Timermanager.IsAliveTimer(SendTime))
                                    {
                                        Timermanager.TerminateTimer(SendTime);
                                    }




                                    break;

                                default:

                                    trx = SetTrxValue(trx, "0");

                                    break;
                            }

                            SendToPLC(trx);

                            // SendToPLC(eqdTrx);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                }
            }


        }

        //Array返流时使用上层出片,ODF CLN时作为与TR02交互
        public void UpSendGlassAction()
        {
            Trx trx = null;

            Thread.Sleep(2000);
            LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Upper").FirstOrDefault();
            string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault().ToString();
            trx = GetTrxValues(trxName);
            IList<int> SendBitOnList = new List<int>();
            string glassID = string.Empty;
            string cstSeq = string.Empty;
            string slotNo = string.Empty;
            string transactionID = string.Empty;
            //Job job;

            Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);

            while (true)
            {
                try
                {
                    if (eqp == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", "L3"));
                    }


                    Thread.Sleep(50);

                    //PLC 连线正常 DebugMode为关闭 设备Run Mode为2
                    if (_isRuning && eqp.File.DeBugMode == eEnableDisable.Disable)
                    {
                        SendBitOnList.Clear();

                        if (eqp.File.NextSendStepUP == eSendStep.SendCancel)
                        {
                            Thread.Sleep(2000);
                        }

                        if (eqp.File.DownInlineMode == eBitResult.ON)
                        {
                            eqp.File.NextSendStepUP = eSendStep.InlineMode;
                            if (line.Data.FABTYPE != "CELL")//eqp.Data.LINEID != "KWF23633L"
                            {
                                if (eqp.File.DownActionMonitor[4] == "1") //锁定   上层出片口有玻璃有账料
                                {
                                    if (CheckDownStreamPIOTrouble(eqp.File.DownPIOBit))
                                    {
                                        if (CheckEqDownActionTrouble(eqp.File.DownActionMonitor, "Up", eqp))
                                        {
                                            //if (eqp.File.DownSendCancelUP == eBitResult.ON)//Send Cancel 
                                            //{
                                            //    eqp.File.NextSendStepUP = eSendStep.SendCancel;
                                            //    eqp.File.DownSendCancelUP = eBitResult.OFF;
                                            //}
                                            //else if (eqp.File.DownSendResumUP == eBitResult.ON)//Send Resum 
                                            //{
                                            //    eqp.File.NextSendStepUP = eSendStep.SendResume;
                                            //    eqp.File.DownSendResumUP = eBitResult.OFF;
                                            //}
                                            //else if (eqp.File.SendStepUP == eSendStep.ReceiveResume && !GetOnBits(eqp.File.DownInterface02).Contains(20))
                                            //{
                                            //    eqp.File.NextSendStepUP = eqp.File.UPResum;
                                            //}
                                            //else if (eqp.File.SendStepUP == eSendStep.ReceiveCancel && !GetOnBits(eqp.File.DownInterface02).Contains(23))
                                            //{
                                            //    eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                            //}
                                            //else if (GetOnBits(eqp.File.DownInterface02).Contains(20) && eqp.File.SendStepUP == eSendStep.DownEqTrouble)//下游设备Resume
                                            //{
                                            //    eqp.File.NextSendStepUP = eSendStep.ReceiveResume;
                                            //}
                                            //else if (GetOnBits(eqp.File.DownInterface02).Contains(23))//下游设备Cancel
                                            //{
                                            //    eqp.File.NextSendStepUP = eSendStep.ReceiveCancel;
                                            //}
                                            //else if (eqp.File.SendStepUP == eSendStep.SendCancel && GetOnBits(eqp.File.DownInterface02).Contains(24))
                                            //{
                                            //    eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                            //}
                                            //else if (eqp.File.SendStepUP == eSendStep.SendResume && GetOnBits(eqp.File.DownInterface02).Contains(21))
                                            //{
                                            //    eqp.File.NextSendStepUP = eqp.File.UPResum;
                                            //}



                                            if ((eqp.File.SendStepUP == eSendStep.InlineMode || eqp.File.SendStepUP == eSendStep.PerSend) && eqp.File.DownActionMonitor[5] == "1" && eqp.File.DownSendCompleteUP == eBitResult.OFF && GetOnBits(eqp.File.DownInterface02).Contains(1) && GetOnBits(eqp.File.DownInterface02).Contains(9))//欲取的模式打开
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.SendAble;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface02).Contains(1) && GetOnBits(eqp.File.DownInterface02).Contains(4)
                                                 )
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.SendStart;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendStart && GetOnBits(eqp.File.DownInterface02).Contains(1) && GetOnBits(eqp.File.DownInterface02).Contains(4)
                                                 && GetOnBits(eqp.File.DownInterface02).Contains(5) && (GetOnBits(eqp.File.DownInterface02).Contains(7) || eqp.File.DownActionMonitor[5] == "0"))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.TansferOn;//上报Send Out
                                            }

                                            else if (eqp.File.SendStepUP == eSendStep.TansferOn && GetOnBits(eqp.File.DownInterface02).Contains(1) && GetOnBits(eqp.File.DownInterface02).Contains(6))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.SendComPlete;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendComPlete && !GetOnBits(eqp.File.DownInterface02).Contains(6))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.End;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.End)
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                            }
                                            else if (eqp.File.DownInterface02[2] == "1" && eqp.File.SendStepUP != eSendStep.SendAble && eqp.File.SendStepUP != eSendStep.InlineMode && eqp.File.SendStepUP != eSendStep.BitAllOff)
                                            {
                                                if (line.Data.FABTYPE != "CF")
                                                {
                                                    eqp.File.NextSendStepUP = eSendStep.DownEqTrouble;
                                                }
                                            }
                                            else
                                            {
                                                if (eqp.File.SendStepUP != eSendStep.BitAllOff)
                                                {
                                                    eqp.File.NextSendStepUP = eqp.File.SendStepUP;
                                                }
                                            }

                                            if (eqp.File.DownSendCancelUP == eBitResult.ON)//Send Cancel 
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.SendCancel;
                                                eqp.File.DownSendCancelUP = eBitResult.OFF;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.ReceiveCancel && !GetOnBits(eqp.File.DownInterface02).Contains(35))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                            }
                                            else if (GetOnBits(eqp.File.DownInterface02).Contains(35))//下游设备Cancel
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.ReceiveCancel;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendCancel && GetOnBits(eqp.File.DownInterface02).Contains(36))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.InlineMode;

                                            }

                                        }
                                        else
                                        {
                                            eqp.File.NextSendStepUP = eSendStep.EqTrouble;

                                        }
                                    }
                                    else
                                    {
                                        eqp.File.NextSendStepUP = eSendStep.DownPIOTrouble;

                                    }
                                }
                                else
                                {
                                    eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                    eqp.File.DownSendResum = eBitResult.OFF;
                                    eqp.File.DownSendCancel = eBitResult.OFF;
                                    //检查设备是否为欲取模式
                                    if (eqp.File.DowmStreamPerMode == eEnableDisable.Enable && eqp.File.DownActionMonitor[8] == "1")
                                    {

                                        eqp.File.NextSendStepUP = eSendStep.PerSend;
                                    }
                                }
                            }
                            else
                            {
                                if (eqp.File.DownActionMonitor[1] == "1") //锁定   出片口有玻璃有账料
                                {
                                    if (CheckDownStreamPIOTrouble(eqp.File.DownPIOBit))
                                    {
                                        if (CheckEqDownActionTrouble(eqp.File.DownActionMonitor, "Down", eqp))
                                        {


                                            if ((eqp.File.SendStepUP == eSendStep.InlineMode || eqp.File.SendStepUP == eSendStep.PerSend) && eqp.File.DownSendCompleteUP == eBitResult.OFF && (eqp.File.DownActionMonitor[2] == "1" || eqp.File.DownActionMonitor[3] == "1") && GetOnBits(eqp.File.DownInterface02).Contains(1) && GetOnBits(eqp.File.DownInterface02).Contains(9))//欲取模式
                                            {
                                                if (eqp.File.TR01TransferEnable == eEnableDisable.Disable && eqp.File.TR02TransferEnable == eEnableDisable.Enable)
                                                {
                                                    if (CurrentGlassSendToTR != eTransfer.TR02)
                                                    {
                                                        if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag)
                                                        {
                                                            TR01_TR02_TransferRequestFlag = true;
                                                            TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eqp.File.NextSendStepUP = eSendStep.SendAble;
                                                    }
                                                }
                                                else if (eqp.File.TR01TransferEnable == eEnableDisable.Enable && eqp.File.TR02TransferEnable == eEnableDisable.Enable && eqp.File.DownSendComplete == eBitResult.OFF)
                                                {
                                                    if (eqp.File.LastRequestTransfer == eTransfer.TR01)
                                                    {
                                                        if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag && eqp.File.SendStep != eSendStep.SendAble)
                                                        {
                                                            TR01_TR02_TransferRequestFlag = true;
                                                            TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                        }
                                                    }
                                                    else if (eqp.File.LastRequestTransfer == eTransfer.TR02)
                                                    {
                                                        if (CurrentGlassSendToTR != eTransfer.TR02)
                                                        {
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            eqp.File.NextSendStepUP = eSendStep.SendAble;
                                                        }
                                                    }

                                                }
                                                else if (eq.File.TR01TransferEnable == eEnableDisable.Disable && eq.File.TR02TransferEnable == eEnableDisable.Disable)
                                                {
                                                    continue;
                                                }
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface02).Contains(1) && GetOnBits(eqp.File.DownInterface02).Contains(4)
                                                 )
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.SendStart;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendStart && GetOnBits(eqp.File.DownInterface02).Contains(1) && GetOnBits(eqp.File.DownInterface02).Contains(4)
                                                 && GetOnBits(eqp.File.DownInterface02).Contains(5) && CheckTransferOnForCVToRBDual_TR02())
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.TansferOn;//上报Send Out
                                            }

                                            else if (eqp.File.SendStepUP == eSendStep.TansferOn && GetOnBits(eqp.File.DownInterface02).Contains(6))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.SendComPlete;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendComPlete && !GetOnBits(eqp.File.DownInterface02).Contains(6))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.End;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.End)
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                            }
                                            else if (eqp.File.DownInterface01[2] == "1" && eqp.File.SendStep != eSendStep.SendAble && eqp.File.SendStepUP != eSendStep.InlineMode && eqp.File.SendStepUP != eSendStep.BitAllOff)
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.DownEqTrouble;
                                            }
                                            else
                                            {
                                                if (eqp.File.SendStepUP != eSendStep.BitAllOff)
                                                {
                                                    eqp.File.NextSendStepUP = eqp.File.SendStepUP;
                                                }
                                            }

                                            if (eqp.File.DownSendCancelUP == eBitResult.ON)//Send Cancel 
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.SendCancel;
                                                eqp.File.DownSendCancelUP = eBitResult.OFF;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.ReceiveCancel && !GetOnBits(eqp.File.DownInterface02).Contains(35))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                            }
                                            else if (GetOnBits(eqp.File.DownInterface02).Contains(35))//下游设备Cancel
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.ReceiveCancel;
                                            }
                                            else if (eqp.File.SendStepUP == eSendStep.SendCancel && GetOnBits(eqp.File.DownInterface02).Contains(36))
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.InlineMode;

                                            }
                                        }
                                        else
                                        {
                                            eqp.File.NextSendStepUP = eSendStep.EqTrouble;

                                        }
                                    }
                                    else
                                    {
                                        eqp.File.NextSendStepUP = eSendStep.DownPIOTrouble;

                                    }
                                }
                                else
                                {
                                    eqp.File.NextSendStepUP = eSendStep.InlineMode;
                                    eqp.File.DownSendResum = eBitResult.OFF;
                                    eqp.File.DownSendCancel = eBitResult.OFF;

                                    //检查设备是否为欲取模式
                                    if (eqp.File.DowmStreamPerMode == eEnableDisable.Enable && eqp.File.DownActionMonitor[7] == "1")
                                    {
                                        if (eqp.File.TR01TransferEnable == eEnableDisable.Disable && eqp.File.TR02TransferEnable == eEnableDisable.Enable)
                                        {
                                            if (CurrentGlassSendToTR != eTransfer.TR02)
                                            {
                                                if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag)
                                                {
                                                    TR01_TR02_TransferRequestFlag = true;
                                                    TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                }
                                            }
                                            else
                                            {
                                                eqp.File.NextSendStepUP = eSendStep.PerSend;
                                            }
                                        }
                                        else if (eqp.File.TR01TransferEnable == eEnableDisable.Enable && eqp.File.TR02TransferEnable == eEnableDisable.Enable)
                                        {
                                            if (eqp.File.LastRequestTransfer == eTransfer.TR01)
                                            {
                                                if (!TR01_TR02_TransferRequestFlag && !TR01_TR02_TransferRequestReplyWaitFlag && eqp.File.SendStep != eSendStep.PerSend)
                                                {
                                                    TR01_TR02_TransferRequestFlag = true;
                                                    TR01_TR02_TransferRequestReplyWaitFlag = true;
                                                }
                                            }
                                            else if (eqp.File.LastRequestTransfer == eTransfer.TR02)
                                            {
                                                if (CurrentGlassSendToTR != eTransfer.TR02)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    eqp.File.NextSendStepUP = eSendStep.PerSend;
                                                }
                                            }

                                        }
                                        else if (eqp.File.TR01TransferEnable == eEnableDisable.Disable && eqp.File.TR02TransferEnable == eEnableDisable.Disable)
                                        {
                                            continue;
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            eqp.File.NextSendStepUP = eSendStep.BitAllOff;
                            TR01_TR02_TransferRequestFlag = false;
                            TR01_TR02_TransferRequestReplyWaitFlag = false;
                            CurrentGlassSendToTR = eTransfer.NONE;
                        }


                        if ((eqp.File.SendStepUP != eqp.File.NextSendStepUP && eqp.File.DeBugMode != eEnableDisable.Enable) || SendThreadRefresh2)
                        {
                            SendThreadRefresh2 = false;
                            if (eqp.File.NextSendStepUP == eSendStep.DownEqTrouble || eqp.File.NextSendStepUP == eSendStep.EqTrouble)
                            {
                                eqp.File.UPResum = eqp.File.SendStepUP;
                            }
                            eqp.File.SendStepUP = eqp.File.NextSendStepUP;

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                            Trx eqdata = new Trx();
                            if (line.Data.FABTYPE != "CELL")//eqp.Data.NODENAME != "KWF23633L"
                            {
                                eqdata = GetTrxValues("L3_EQDSendingGlassDataReport#02");
                            }
                            else
                            {
                                eqdata = GetTrxValues("L3_EQDSendingGlassDataReport#01");
                            }

                            //KeyValuePair<string, TransferTime> res = new KeyValuePair<string, TransferTime>();
                            switch (eqp.File.NextSendStepUP)
                            {

                                case eSendStep.BitAllOff:

                                    trx = SetTrxValue(trx, "0");

                                    break;
                                case eSendStep.InlineMode:

                                    SendBitOnList.Add(0);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    break;
                                case eSendStep.PerSend:
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        PositionStatus[1] = false;
                                        PositionStatus[2] = false;
                                        if (eqp.File.DownActionMonitor[2] == "1")
                                        {
                                            PositionStatus[1] = true;
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;
                                            }
                                        }
                                        else
                                        {
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;

                                            }
                                        }
                                    }
                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(2);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.PerSend);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.PerSend);
                                    }

                                    break;
                                case eSendStep.SendCancel:

                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "0";
                                    trx[0][0][3].Value = "1";
                                    trx[0][0][4].Value = "1";
                                    trx[0][0][34].Value = "1";

                                    break;
                                case eSendStep.SendResume:

                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "0";
                                    trx[0][0][14].Value = "1";
                                    trx[0][0][15].Value = "1";
                                    trx[0][0][19].Value = "1";

                                    break;

                                case eSendStep.ReceiveCancel:
                                    //trx[0][0][0].Value = "1";
                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(35);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    break;
                                case eSendStep.ReceiveResume:
                                    //trx[0][0][0].Value = "1";
                                    trx[0][0][20].Value = "1";

                                    break;
                                case eSendStep.DownEqTrouble:
                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "1";

                                    break;
                                case eSendStep.DownPIOTrouble:
                                    trx[0][0][0].Value = "0";
                                    trx[0][0][1].Value = "1";

                                    break;
                                case eSendStep.EqTrouble:
                                    trx[0][0][0].Value = "0";
                                    trx[0][0][1].Value = "1";

                                    break;

                                case eSendStep.SendAble:
                                    if (line.Data.FABTYPE != "CELL")//eqp.Data.LINEID != "KWF23633L"
                                    {
                                        UPWriteWEQDJobDta();
                                    }
                                    else
                                    {
                                        WriteWEQDJobDta();
                                    }

                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        SendGlassPositionBit.Clear();
                                        PositionStatus[1] = false;
                                        PositionStatus[2] = false;
                                        if (eqp.File.DownActionMonitor[2] == "1")
                                        {
                                            PositionStatus[1] = true;
                                            SendBitOnList.Add(13);
                                            SendGlassPositionBit.Add(13);
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;
                                                SendBitOnList.Add(11);
                                                SendGlassPositionBit.Add(11);
                                            }
                                        }
                                        else
                                        {
                                            if (eqp.File.DownActionMonitor[3] == "1")
                                            {
                                                PositionStatus[2] = true;
                                                SendBitOnList.Add(14);
                                                SendGlassPositionBit.Add(14);
                                            }
                                        }
                                    }

                                    if (eqp.Data.NODENAME == "TEREWORK" || eqp.Data.NODENAME == "TSREWORK" || eqp.Data.NODENAME == "KWF22093L")
                                    {
                                        if (eqp.File.DownActionMonitor[5] == "1")
                                        {
                                            SendBitOnList.Add(30);
                                        }
                                        if (eqp.File.DownActionMonitor[6] == "1")
                                        {
                                            SendBitOnList.Add(31);
                                        }
                                    }

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendAble);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendAble);
                                    }
                                    break;

                                case eSendStep.SendStart:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    SendBitOnList.Add(4);
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < SendGlassPositionBit.Count; i++)
                                        {
                                            SendBitOnList.Add(SendGlassPositionBit[i]);
                                        }
                                    }
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    if (ParameterManager[eParameterName.SendTimer].GetInteger() != 0)
                                    {
                                        string SendTimer = string.Format("{0}_UPSendTimer", "L3");

                                        if (Timermanager.IsAliveTimer(SendTimer))
                                        {
                                            Timermanager.TerminateTimer(SendTimer);
                                        }
                                        Timermanager.CreateTimer(SendTimer, false, ParameterManager[eParameterName.SendTimer].GetInteger(),
                                     new System.Timers.ElapsedEventHandler(UPSendTimerTimeoutAction), UtilityMethod.GetAgentTrackKey());
                                    }

                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendStart);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendStart);
                                    }

                                    break;

                                case eSendStep.SendComPlete:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    SendBitOnList.Add(4);
                                    SendBitOnList.Add(5);
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < SendGlassPositionBit.Count; i++)
                                        {
                                            SendBitOnList.Add(SendGlassPositionBit[i]);
                                        }
                                    }
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    if(line.Data.FABTYPE=="CELL")
                                    {
                                        Trx EQDDownContrxtoCELL = GetTrxValues("L3_EQDDownstreamPathControl#02");
                                        EQDDownContrxtoCELL[0][0][1].Value = "1";
                                        SendToPLC(EQDDownContrxtoCELL);
                                    }
                                    

                                    //设定TransferTimeData
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                cstSeq = eqdata[0][i]["Cassette_Sequence_No"].Value.Trim();
                                                slotNo = eqdata[0][i]["Job_Sequence_No"].Value.Trim();
                                                SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendComPlete);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                        slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                        SetSendTransferTimeData(glassID, cstSeq, slotNo, eSendStep.SendComPlete);
                                    }
                                    break;

                                case eSendStep.TansferOn:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(3);
                                    SendBitOnList.Add(4);
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {
                                        for (int i = 0; i < SendGlassPositionBit.Count; i++)
                                        {
                                            SendBitOnList.Add(SendGlassPositionBit[i]);
                                        }
                                    }
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    CPCSendOutReport(true, eBitResult.ON, "Up_Output");

                                    break;

                                case eSendStep.End:

                                    SendBitOnList.Add(0);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    //出片解锁 出片完成
                                    Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#02");
                                    EQDDownContrx[0][0][0].Value = "1";
                                    SendToPLC(EQDDownContrx);

                                    lsGlassID.Clear();
                                    if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                                    {

                                        for (int i = 0; i < PositionStatus.Count; i++)
                                        {
                                            if (PositionStatus.ElementAt(i).Value)
                                            {
                                                glassID = eqdata[0][i]["GlassID_or_PanelID"].Value.Trim();
                                                if (!lsGlassID.Contains(glassID))
                                                {
                                                    lsGlassID.Add(glassID);
                                                }
                                            }
                                        }
                                        CurrentGlassSendToTR = eTransfer.NONE;
                                        if (TR01_TR02_TransferRequestReplyWaitFlag)
                                            TR01_TR02_TransferRequestReplyWaitFlag = false;
                                        if (TR01_TR02_TransferRequestFlag)
                                            TR01_TR02_TransferRequestFlag = false;
                                    }
                                    else
                                    {
                                        glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                        lsGlassID.Add(glassID);
                                    }
                                    TransferTimeDataReportFlag = true;

                                    //glassID = eqdata[0][0]["GlassID_or_PanelID"].Value.Trim();
                                    //cstSeq = eqdata[0][0]["Cassette_Sequence_No"].Value.Trim();
                                    //slotNo = eqdata[0][0]["Job_Sequence_No"].Value.Trim();
                                    //TransferTimeDataReport(eBitResult.ON, glassID, cstSeq, slotNo);

                                    if (eqp.File.LotEndGlassInfos.ContainsKey(glassID))
                                    {
                                        LotEndGlassInfos lotEndGlassInfos = new LotEndGlassInfos();
                                        eqp.File.LotEndGlassInfos.TryRemove(glassID, out lotEndGlassInfos);
                                        //返流回Index不上报
                                        //LotEndGlassReportToDownstream(eBitResult.ON, lotEndGlassInfos.LotID, lotEndGlassInfos.Cassette_Sequence_No, lotEndGlassInfos.Slot_Sequence_No, lotEndGlassInfos.GlassID);
                                    }
                                    eqp.File.UPResum = eSendStep.InlineMode;
                                    eqp.File.DownSendCompleteUP = eBitResult.ON;

                                    ObjectManager.EquipmentManager.EnqueueSave(eqp.File);



                                    string SendTime = string.Format("{0}_UPSendTimer", "L3");

                                    if (Timermanager.IsAliveTimer(SendTime))
                                    {
                                        Timermanager.TerminateTimer(SendTime);
                                    }

                                    break;

                                default:

                                    trx = SetTrxValue(trx, "0");

                                    break;
                            }

                            SendToPLC(trx);

                        }
                    }
                    else
                    {
                        //if (eqp.File.EquipmentRunMode == "1")
                        //{
                        //    Thread.Sleep(1000);
                        //    trx = GetTrxValues("L3_UpstreamPath#02");
                        //    trx = SetTrxValue(trx, "0");
                        //    SendToPLC(trx);

                        //}

                    }
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                }
            }


        }
        //返流
        public void ReturnSendGlassAction()
        {
            Trx trx = null;

            Thread.Sleep(2000);
            LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Return" && x.PathPosition == "Upper").FirstOrDefault();
            string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault().ToString();
            trx = GetTrxValues(trxName);
            IList<int> SendBitOnList = new List<int>();

            Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
            while (true)
            {
                try
                {
                    if (eqp == null)
                    {
                        throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", "L3"));
                    }


                    Thread.Sleep(50);

                    //PLC 连线正常 DebugMode为关闭 设备Run Mode为2
                    if (_isRuning && eqp.File.DeBugMode == eEnableDisable.Disable)
                    {
                        SendBitOnList.Clear();

                        if (eqp.File.NextSendStepReturn == eSendStep.SendCancel)
                        {
                            Thread.Sleep(2000);
                        }

                        if (eqp.File.DownInlineMode == eBitResult.ON)
                        {
                            eqp.File.NextSendStepReturn = eSendStep.InlineMode;

                            if (eqp.File.DownActionMonitor[12] == "1") //锁定   返流出片口有玻璃有账料
                            {
                                if (CheckDownStreamPIOTrouble(eqp.File.DownPIOBit))
                                {
                                    if (CheckEqDownActionTrouble(eqp.File.DownActionMonitor, "Return", eqp))
                                    {
                                        if (eqp.File.SendCancelReturn == eBitResult.ON)//Send Cancel 
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.SendCancel;
                                            eqp.File.SendCancelReturn = eBitResult.OFF;
                                        }
                                        else if (eqp.File.SendResumReturn == eBitResult.ON)//Send Resum 
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.SendResume;
                                            eqp.File.SendResumReturn = eBitResult.OFF;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.ReceiveResume && !GetOnBits(eqp.File.DownInterface03).Contains(20))
                                        {
                                            eqp.File.NextSendStepReturn = eqp.File.UPResum;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.ReceiveCancel && !GetOnBits(eqp.File.DownInterface03).Contains(23))
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.InlineMode;
                                        }
                                        else if (GetOnBits(eqp.File.DownInterface03).Contains(20) && eqp.File.SendStepReturn == eSendStep.DownEqTrouble)//下游设备Resume
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.ReceiveResume;
                                        }
                                        else if (GetOnBits(eqp.File.DownInterface03).Contains(23))//下游设备Cancel
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.ReceiveCancel;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.SendCancel && GetOnBits(eqp.File.DownInterface03).Contains(24))
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.InlineMode;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.SendResume && GetOnBits(eqp.File.DownInterface03).Contains(21))
                                        {
                                            eqp.File.NextSendStepReturn = eqp.File.UPResum;
                                        }
                                        else if ((eqp.File.SendStepReturn == eSendStep.InlineMode || eqp.File.SendStepReturn == eSendStep.PerSend) && eqp.File.DownActionMonitor[11] == "1" && eqp.File.SendCompleteReturn == eBitResult.OFF && GetOnBits(eqp.File.DownInterface03).Count == 2)//欲取的模式打开
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.SendAble;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.SendAble && GetOnBits(eqp.File.DownInterface03).Count == 3 && GetOnBits(eqp.File.DownInterface03).Contains(1) && GetOnBits(eqp.File.DownInterface03).Contains(15)
                                             && GetOnBits(eqp.File.DownInterface03).Contains(29))
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.SendStart;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.SendStart && GetOnBits(eqp.File.DownInterface03).Contains(1) && GetOnBits(eqp.File.DownInterface03).Contains(15)
                                             && GetOnBits(eqp.File.DownInterface03).Contains(16) && (GetOnBits(eqp.File.DownInterface03).Contains(13) || eqp.File.DownActionMonitor[5] == "0"))
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.TansferOn;//上报Send Out
                                        }

                                        else if (eqp.File.SendStepReturn == eSendStep.TansferOn && GetOnBits(eqp.File.DownInterface03).Contains(1) && GetOnBits(eqp.File.DownInterface03).Contains(17))
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.SendComPlete;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.SendComPlete && !GetOnBits(eqp.File.DownInterface03).Contains(17))
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.End;
                                        }
                                        else if (eqp.File.SendStepReturn == eSendStep.End)
                                        {
                                            eqp.File.NextSendStepReturn = eSendStep.InlineMode;
                                        }
                                        else if (eqp.File.DownInterface03[2] == "1" && eqp.File.SendStepReturn != eSendStep.SendAble && eqp.File.SendStepReturn != eSendStep.InlineMode && eqp.File.SendStepReturn != eSendStep.BitAllOff)
                                        {
                                            if (line.Data.FABTYPE != "CF")
                                            {
                                                eqp.File.NextSendStepReturn = eSendStep.DownEqTrouble;
                                            }
                                        }
                                        else
                                        {
                                            if (eqp.File.SendStepReturn != eSendStep.BitAllOff)
                                            {
                                                eqp.File.NextSendStepReturn = eqp.File.SendStepReturn;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        eqp.File.NextSendStepReturn = eSendStep.EqTrouble;

                                    }
                                }
                                else
                                {
                                    eqp.File.NextSendStepReturn = eSendStep.DownPIOTrouble;

                                }
                            }
                            else
                            {
                                eqp.File.NextSendStepReturn = eSendStep.InlineMode;

                                //检查设备是否为欲取模式
                                if (eqp.File.UpStreamPerMode == eEnableDisable.Enable && eqp.File.DownActionMonitor[8] == "1")
                                {

                                    eqp.File.NextSendStepReturn = eSendStep.PerSend;
                                }
                            }
                        }
                        else
                        {
                            eqp.File.NextSendStepReturn = eSendStep.BitAllOff;
                        }


                        if (eqp.File.SendStepReturn != eqp.File.NextSendStepReturn && eqp.File.DeBugMode != eEnableDisable.Enable)
                        {
                            if (eqp.File.NextSendStepReturn == eSendStep.DownEqTrouble || eqp.File.NextSendStepReturn == eSendStep.EqTrouble)
                            {
                                eqp.File.UPResum = eqp.File.SendStepReturn;
                            }
                            eqp.File.SendStepReturn = eqp.File.NextSendStepReturn;

                            ObjectManager.EquipmentManager.EnqueueSave(eqp.File);
                            //写入W区域
                            //trx = GetTrxValues("L3_UpstreamPath#01");

                            if (eqp.Data.NODENAME == "TEREWORK" || eqp.Data.NODENAME == "TSREWORK")
                            {
                                if (eqp.File.DownActionMonitor[5] == "1")
                                {
                                    SendBitOnList.Add(30);
                                }
                                if (eqp.File.DownActionMonitor[6] == "1")
                                {
                                    SendBitOnList.Add(31);
                                }
                            }


                            switch (eqp.File.NextSendStepReturn)
                            {

                                case eSendStep.BitAllOff:

                                    trx = SetTrxValue(trx, "0");

                                    break;
                                case eSendStep.InlineMode:

                                    SendBitOnList.Add(0);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    break;
                                case eSendStep.PerSend:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(9);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    break;
                                case eSendStep.SendCancel:

                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "0";
                                    trx[0][0][14].Value = "1";
                                    trx[0][0][15].Value = "1";
                                    trx[0][0][22].Value = "1";

                                    break;
                                case eSendStep.SendResume:

                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "0";
                                    trx[0][0][14].Value = "1";
                                    trx[0][0][15].Value = "1";
                                    trx[0][0][19].Value = "1";

                                    break;

                                case eSendStep.ReceiveCancel:
                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(23);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    break;
                                case eSendStep.ReceiveResume:
                                    //trx[0][0][0].Value = "1";
                                    trx[0][0][20].Value = "1";

                                    break;
                                case eSendStep.DownEqTrouble:
                                    trx[0][0][0].Value = "1";
                                    trx[0][0][1].Value = "1";

                                    break;
                                case eSendStep.DownPIOTrouble:

                                    trx[0][0][1].Value = "1";

                                    break;
                                case eSendStep.EqTrouble:
                                    trx[0][0][0].Value = "0";
                                    trx[0][0][1].Value = "1";

                                    break;

                                case eSendStep.SendAble:

                                    ReturnWriteWEQDJobDta();


                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(14);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    break;

                                case eSendStep.SendStart:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(14);
                                    SendBitOnList.Add(15);

                                    trx = SetTrxValue(trx, SendBitOnList);

                                    if (ParameterManager[eParameterName.SendTimer].GetInteger() != 0)
                                    {
                                        string SendTimer = string.Format("{0}_ReturnSendTimer", "L3");

                                        if (Timermanager.IsAliveTimer(SendTimer))
                                        {
                                            Timermanager.TerminateTimer(SendTimer);
                                        }
                                        Timermanager.CreateTimer(SendTimer, false, ParameterManager[eParameterName.SendTimer].GetInteger(),
                                     new System.Timers.ElapsedEventHandler(ReturnSendTimerTimeoutAction), UtilityMethod.GetAgentTrackKey());
                                    }

                                    break;

                                case eSendStep.SendComPlete:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(14);
                                    SendBitOnList.Add(15);
                                    SendBitOnList.Add(16);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    break;

                                case eSendStep.TansferOn:

                                    SendBitOnList.Add(0);
                                    SendBitOnList.Add(14);
                                    SendBitOnList.Add(15);

                                    trx = SetTrxValue(trx, SendBitOnList);

                                    CPCSendOutReport(true, eBitResult.ON, "Up_Input");

                                    break;

                                case eSendStep.End:

                                    SendBitOnList.Add(0);
                                    trx = SetTrxValue(trx, SendBitOnList);

                                    //出片解锁 出片完成
                                    Trx EQDDownContrx = GetTrxValues("L3_EQDDownstreamPathControl#03");
                                    EQDDownContrx[0][0][0].Value = "1";
                                    SendToPLC(EQDDownContrx);

                                    eqp.File.UPResum = eSendStep.InlineMode;
                                    eqp.File.SendCompleteReturn = eBitResult.ON;

                                    ObjectManager.EquipmentManager.EnqueueSave(eqp.File);



                                    string SendTime = string.Format("{0}_ReturnSendTimer", "L3");

                                    if (Timermanager.IsAliveTimer(SendTime))
                                    {
                                        Timermanager.TerminateTimer(SendTime);
                                    }

                                    break;

                                default:

                                    trx = SetTrxValue(trx, "0");

                                    break;
                            }

                            SendToPLC(trx);

                        }
                    }
                    else
                    {
                        //if (eqp.File.EquipmentRunMode == "1")
                        //{
                        //    Thread.Sleep(1000);
                        //    trx = GetTrxValues("L3_UpstreamPath#01");
                        //    trx = SetTrxValue(trx, "0");
                        //    SendToPLC(trx);

                        //}

                    }
                }
                catch (System.Exception ex)
                {
                    LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                }
            }


        }

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


        public Trx SetTrxValue(Trx trx, string value)
        {
            if (value == "1" || value == "0")
            {
                for (int i = 0; i < trx[0][0].Items.AllKeys.Length; i++)
                {
                    trx[0][0][i].Value = value;
                }
                trx.TrackKey = UtilityMethod.GetAgentTrackKey();
            }

            return trx;
        }

        public Trx SetTrxValue(Trx trx, IList<int> OnList)
        {
            for (int i = 0; i < trx[0][0].Items.AllKeys.Length; i++)
            {
                if (OnList.Contains(i))
                {
                    trx[0][0][i].Value = "1";
                }
                else
                {
                    trx[0][0][i].Value = "0";
                }
            }
            trx.TrackKey = UtilityMethod.GetAgentTrackKey();

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

        public bool CheckEqUpActionTrouble(IDictionary<int, string> UpActionMonitor, string portName, Equipment eq)
        {
            //to do
            string tmp = string.Empty;
            lock (tmp)
            {
                //if (eq != null)
                //{
                //    if (eq.Data.LINEID == "KWF22090R" || eq.Data.LINEID == "KWF22091R")
                //    {
                //        if (portName == "Down")
                //        {
                //            Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 1);
                //            if (unit != null && unit.File.Status == eEQPStatus.Down)
                //            {
                //                return false;
                //            }
                //        }
                //        else if (portName == "Up")
                //        {
                //            if (eq.Data.LINEID == "KWF22090R")
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 10);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }
                //            else
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 11);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }
                //        }
                //        else if (portName == "Return")
                //        {
                //            if (eq.Data.LINEID == "KWF22090R")
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 15);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }
                //            else
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 16);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }

                //        }

                //    }
                //    else
                //    {
                //        if (portName == "Down")
                //        {
                //            Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 1);
                //            if (unit != null && unit.File.Status == eEQPStatus.Down)
                //            {
                //                return false;
                //            }
                //        }
                //    }
                //}

                //if (eq.File.Status == eEQPStatus.Down)
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                return true;
            }
        }

        public bool CheckEqDownActionTrouble(IDictionary<int, string> DownActionMonitor, string portName, Equipment eq)
        {
            //to do
            string tmp = string.Empty;
            lock (tmp)
            {
                //if (eq != null)
                //{
                //    if (eq.Data.LINEID == "KWF22090R" || eq.Data.LINEID == "KWF22091R")
                //    {

                //        if (portName == "Down")
                //        {
                //            if (eq.Data.LINEID == "KWF22090R")
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 9);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }
                //            else
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 10);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }
                //        }
                //        else if (portName == "Up")
                //        {
                //            if (eq.Data.LINEID == "KWF22090R")
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 15);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }
                //            else
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 16);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }

                //        }
                //        else if (portName == "Return")
                //        {
                //            if (eq.Data.LINEID == "KWF22090R")
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 10);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }
                //            else
                //            {
                //                Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 11);
                //                if (unit != null && unit.File.Status == eEQPStatus.Down)
                //                {
                //                    return false;
                //                }
                //            }

                //        }

                //    }
                //    else if (eq.Data.LINEID == "KWF22092R")
                //    {
                //        if (portName == "Up")
                //        {
                //            Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, 1);
                //            if (unit != null && unit.File.Status == eEQPStatus.Down)
                //            {
                //                return false;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (portName == "Down")
                //        {
                //            Unit unit = ObjectManager.UnitManager.GetUnit(eq.Data.NODENO, eq.Data.UNITCOUNT);
                //            if (unit != null && unit.File.Status == eEQPStatus.Down)
                //            {
                //                return false;
                //            }
                //        }
                //    }
                //}

                //if (eq.File.Status == eEQPStatus.Down)
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                return true;
            }
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
        public bool JobDataCheck(Equipment eq, LinkSignalType linkSignalType)
        {
            //return true;//测试
            string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Send") == true).FirstOrDefault().ToString();

            Trx trx = GetTrxValues(trxName);

            eq.File.UPDataCheckResult = String.Empty;
            Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
            if (trx != null)
            {
                if (eq.Data.NODENAME == "TEREWORK" || eq.Data.NODENAME == "TSREWORK")
                {
                    //bool jobA = false;
                    //bool jobB = false;

                    //if (GetOnBits(eq.File.UPInterface).Contains(31))
                    //{
                    //    #region  //检查Sequence No是否为空
                    //    int c, j;
                    //    if (!int.TryParse(trx[0][0]["CassetteSequenceNo"].Value, out c))
                    //    {
                    //        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!",
                    //            trx[0][0][0].Value));
                    //        jobA = false;
                    //    }
                    //    if (!int.TryParse(trx[0][0]["SlotSequenceNo"].Value, out j))
                    //    {
                    //        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Job Sequence No isn't number ({0})!!",
                    //            trx[0][0][1].Value));
                    //        jobA = false;
                    //    }

                    //    Trx SequenceNoTRX = GetTrxValues("L3_EQDSequenceNoCheckNGBit");

                    //    if (c == 0 || j == 0)
                    //    {
                    //        SequenceNoTRX[0][0][0].Value = "1";

                    //        eq.File.UPDataCheckResult = "Sequence No NG: Cassette Sequence No: " + c.ToString() + " ,Job Sequence No:" + j.ToString() + "\r\n";

                    //    }
                    //    else
                    //    {
                    //        SequenceNoTRX[0][0][0].Value = "0";
                    //    }

                    //    SendToPLC(SequenceNoTRX);

                    //    #endregion

                    //    #region  //检查Job ID 是否为空

                    //    Trx jobIDTRX = GetTrxValues("L3_EQDJobIDCheckNGBit");

                    //    if (string.IsNullOrEmpty(trx[0][0]["JobID"].Value.Trim()))
                    //    {
                    //        jobIDTRX[0][0][0].Value = "1";

                    //        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job ID NG: Job ID: " + trx[0][0]["JobID"].Value + "\r\n";

                    //    }
                    //    else
                    //    {
                    //        jobIDTRX[0][0][0].Value = "0";
                    //    }

                    //    SendToPLC(jobIDTRX);

                    //    #endregion

                    //    #region  //检查Recipe是否为空

                    //    Trx ppidTRX = GetTrxValues("L3_EQDPPIDCheckNGBit");

                    //    if (string.IsNullOrEmpty(trx[0][0]["PPID#02"].Value.Trim()))
                    //    {
                    //        ppidTRX[0][0][0].Value = "1";

                    //        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "PPID#02 NG: PPID: " + trx[0][0]["PPID#02"].Value.Trim() + "\r\n";

                    //    }
                    //    else
                    //    {
                    //        ppidTRX[0][0][0].Value = "0";
                    //    }

                    //    SendToPLC(ppidTRX);

                    //    #endregion

                    //    //检查上游设备送过来的资料
                    //    if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                    //    {


                    //        #region  检查Recipe 是否一致

                    //        Trx recipeNGTRX = GetTrxValues("L3_EQDRecipeCheckNGBit");
                    //        if (eq.File.RecipeCheckMode == eEnableDisable.Enable)
                    //        {

                    //            string upDataPpid = trx[0][0]["PPID#02"].Value.Trim();

                    //            string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                    //            if (int.Parse(upDataRecipe) != eq.File.CurrentRecipeNo)
                    //            {
                    //                //要通知设备Recipe Check NG.
                    //                recipeNGTRX[0][0][0].Value = "1";

                    //                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe NG: " + eq.File.CurrentRecipeNo + " != " + upDataRecipe + "\r\n";
                    //            }
                    //            else
                    //            {
                    //                //Recipe Check OK 
                    //                recipeNGTRX[0][0][0].Value = "0";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            recipeNGTRX[0][0][0].Value = "0";
                    //        }
                    //        SendToPLC(recipeNGTRX);


                    //        #endregion

                    //        #region 检查Recipe Auto Change 

                    //        Trx recipeAutoNGTRX = GetTrxValues("L3_EQDRecipeAUTOChangeNGBit");
                    //        if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable)
                    //        {

                    //            string upDataPpid = trx[0][0]["PPID#02"].Value.Trim();

                    //            string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                    //            Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                    //                        ObjectManager.RecipeManager.ReloadRecipeByNo();


                    //            if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataPpid))
                    //            {
                    //                //要通知设备Recipe Check NG.
                    //                recipeAutoNGTRX[0][0][0].Value = "1";

                    //                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe Auto Change NG: " + string.Format("Recipe Table Not Contain {0} ", upDataRecipe) + "\r\n";
                    //            }
                    //            else
                    //            {
                    //                //Recipe Check OK 
                    //                recipeAutoNGTRX[0][0][0].Value = "0";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            recipeAutoNGTRX[0][0][0].Value = "0";
                    //        }
                    //        SendToPLC(recipeAutoNGTRX);

                    //        #endregion

                    //        #region  //检查设备内的Job data 是否有Duplicate 

                    //        Trx DupllicateCheck = GetTrxValues("L3_EQDJobDupllicateCheckNGBit");

                    //        if (eq.File.JobDuplicateCheckMode == eEnableDisable.Enable)
                    //        {
                    //            string cstSeq = trx[0][0][eJOBDATA.CassetteSequenceNumber].Value;
                    //            string slotNo = trx[0][0][eJOBDATA.SlotSequenceNumber].Value;

                    //            Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                    //            if (job != null)
                    //            {
                    //                //要通知设备 Job Duplicate Check NG.
                    //                DupllicateCheck[0][0][0].Value = "1";

                    //                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Duplicate Check  NG: " + cstSeq + ", " + slotNo + "\r\n";

                    //            }
                    //            else
                    //            {
                    //                DupllicateCheck[0][0][0].Value = "0";
                    //            }

                    //        }
                    //        else
                    //        {
                    //            DupllicateCheck[0][0][0].Value = "0";
                    //        }

                    //        #endregion


                    //        if (eq.File.UPDataCheckResult == string.Empty)
                    //        {
                    //            jobA = true;
                    //        }
                    //        else
                    //        {
                    //            jobA = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        jobA = true;
                    //    }


                    //}

                    //if (GetOnBits(eq.File.UPInterface).Contains(32))
                    //{
                    //    #region  //检查Sequence No是否为空
                    //    int c, j;
                    //    if (!int.TryParse(trx[0][1]["CassetteSequenceNo"].Value, out c))
                    //    {
                    //        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!",
                    //            trx[0][1][0].Value));
                    //        jobB = false;
                    //    }
                    //    if (!int.TryParse(trx[0][1]["SlotSequenceNo"].Value, out j))
                    //    {
                    //        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Job Sequence No isn't number ({0})!!",
                    //            trx[0][1][1].Value));
                    //        jobB = false;
                    //    }

                    //    Trx SequenceNoTRX = GetTrxValues("L3_EQDSequenceNoCheckNGBit");

                    //    if (c == 0 || j == 0)
                    //    {
                    //        SequenceNoTRX[0][0][0].Value = "1";

                    //        eq.File.UPDataCheckResult = "Sequence No NG: Cassette Sequence No: " + c.ToString() + " ,Job Sequence No:" + j.ToString() + "\r\n";

                    //    }
                    //    else
                    //    {
                    //        SequenceNoTRX[0][0][0].Value = "0";
                    //    }

                    //    SendToPLC(SequenceNoTRX);

                    //    #endregion

                    //    #region  //检查Job ID 是否为空

                    //    Trx jobIDTRX = GetTrxValues("L3_EQDJobIDCheckNGBit");

                    //    if (string.IsNullOrEmpty(trx[0][1]["JobID"].Value.Trim()))
                    //    {
                    //        jobIDTRX[0][0][0].Value = "1";

                    //        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job ID NG: Job ID: " + trx[0][1]["JobID"].Value + "\r\n";

                    //    }
                    //    else
                    //    {
                    //        jobIDTRX[0][0][0].Value = "0";
                    //    }

                    //    SendToPLC(jobIDTRX);

                    //    #endregion

                    //    #region  //检查Recipe是否为空

                    //    Trx ppidTRX = GetTrxValues("L3_EQDPPIDCheckNGBit");

                    //    if (string.IsNullOrEmpty(trx[0][1]["PPID#02"].Value.Trim()))
                    //    {
                    //        ppidTRX[0][0][0].Value = "1";

                    //        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "PPID#02 NG: PPID: " + trx[0][1]["PPID#02"].Value.Trim() + "\r\n";

                    //    }
                    //    else
                    //    {
                    //        ppidTRX[0][0][0].Value = "0";
                    //    }

                    //    SendToPLC(ppidTRX);

                    //    #endregion

                    //    //检查上游设备送过来的资料
                    //    if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                    //    {


                    //        #region  检查Recipe 是否一致

                    //        Trx recipeNGTRX = GetTrxValues("L3_EQDRecipeCheckNGBit");
                    //        if (eq.File.RecipeCheckMode == eEnableDisable.Enable)
                    //        {

                    //            string upDataPpid = trx[0][1]["PPID#02"].Value.Trim();

                    //            string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                    //            if (int.Parse(upDataRecipe) != eq.File.CurrentRecipeNo)
                    //            {
                    //                //要通知设备Recipe Check NG.
                    //                recipeNGTRX[0][0][0].Value = "1";

                    //                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe NG: " + eq.File.CurrentRecipeNo + " != " + upDataRecipe + "\r\n";
                    //            }
                    //            else
                    //            {
                    //                //Recipe Check OK 
                    //                recipeNGTRX[0][0][0].Value = "0";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            recipeNGTRX[0][0][0].Value = "0";
                    //        }
                    //        SendToPLC(recipeNGTRX);


                    //        #endregion

                    //        #region 检查Recipe Auto Change 

                    //        Trx recipeAutoNGTRX = GetTrxValues("L3_EQDRecipeAUTOChangeNGBit");
                    //        if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable)
                    //        {

                    //            string upDataPpid = trx[0][1]["PPID#02"].Value.Trim();

                    //            string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();

                    //            Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                    //                        ObjectManager.RecipeManager.ReloadRecipeByNo();


                    //            if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataPpid))
                    //            {
                    //                //要通知设备Recipe Check NG.
                    //                recipeAutoNGTRX[0][0][0].Value = "1";

                    //                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe Auto Change NG: " + string.Format("Recipe Table Not Contain {0} ", upDataRecipe) + "\r\n";
                    //            }
                    //            else
                    //            {
                    //                //Recipe Check OK 
                    //                recipeAutoNGTRX[0][0][0].Value = "0";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            recipeAutoNGTRX[0][0][0].Value = "0";
                    //        }
                    //        SendToPLC(recipeAutoNGTRX);

                    //        #endregion

                    //        #region  //检查设备内的Job data 是否有Duplicate 

                    //        Trx DupllicateCheck = GetTrxValues("L3_EQDJobDupllicateCheckNGBit");

                    //        if (eq.File.JobDuplicateCheckMode == eEnableDisable.Enable)
                    //        {
                    //            string cstSeq = trx[0][1][eJOBDATA.CassetteSequenceNumber].Value;
                    //            string slotNo = trx[0][1][eJOBDATA.SlotSequenceNumber].Value;

                    //            Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                    //            if (job != null)
                    //            {
                    //                //要通知设备 Job Duplicate Check NG.
                    //                DupllicateCheck[0][0][0].Value = "1";

                    //                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Duplicate Check  NG: " + cstSeq + ", " + slotNo + "\r\n";

                    //            }
                    //            else
                    //            {
                    //                DupllicateCheck[0][0][0].Value = "0";
                    //            }

                    //        }
                    //        else
                    //        {
                    //            DupllicateCheck[0][0][0].Value = "0";
                    //        }

                    //        #endregion


                    //        if (eq.File.UPDataCheckResult == string.Empty)
                    //        {
                    //            jobB = true;
                    //        }
                    //        else
                    //        {
                    //            jobB = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        jobB = true;
                    //    }




                    //}

                    //if (GetOnBits(eq.File.UPInterface).Contains(31) && GetOnBits(eq.File.UPInterface).Contains(32))
                    //{
                    //    if (jobA && jobB)
                    //    {
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else if (GetOnBits(eq.File.UPInterface).Contains(31))
                    //{
                    //    if (jobA)
                    //    {
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        return false;
                    //    }

                    //}
                    //else if (GetOnBits(eq.File.UPInterface).Contains(32))
                    //{
                    //    if (jobB)
                    //    {
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    return true;
                }
                else
                {
                    #region  检查Sequence No是否为空
                    int c, j;
                    if (!int.TryParse(trx[0][0]["Cassette_Sequence_No"].Value, out c))
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Cassette Sequence No ({0}) isn't number!!",
                            trx[0][0][0].Value));
                        return false;
                    }
                    if (!int.TryParse(trx[0][0]["Job_Sequence_No"].Value, out j))
                    {
                        LogError(MethodBase.GetCurrentMethod().Name + "()", string.Format("Create Job Failed Job Sequence No isn't number ({0})!!",
                            trx[0][0][1].Value));
                        return false;
                    }

                    Trx SequenceNoTRX = GetTrxValues("L3_EQDSequenceNoCheckNGBit");

                    if (c == 0 || j == 0)
                    {
                        SequenceNoTRX[0][0][0].Value = "1";

                        eq.File.UPDataCheckResult = "Sequence No NG: Cassette Sequence No: " + c.ToString() + " ,Job Sequence No:" + j.ToString() + "\r\n";

                    }
                    else
                    {
                        SequenceNoTRX[0][0][0].Value = "0";
                    }

                    SendToPLC(SequenceNoTRX);

                    #endregion

                    #region //检查LotID 是否为空或者不存在
                    if (eq.Data.NODENAME == "CLEANER")
                    {
                        Trx jobLotIDTRX = GetTrxValues("L3_EQDLOTIDNGBit");

                        if (eq.File.PortLot01 != trx[0][0]["Lot_ID"].Value.Trim() && eq.File.PortLot02 != trx[0][0]["Lot_ID"].Value.Trim())
                        {
                            jobLotIDTRX[0][0][0].Value = "1";

                            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job LOT ID NG: Job Lot ID: " + trx[0][0]["Lot_ID"].Value + "\r\n";

                        }
                        else
                        {
                            jobLotIDTRX[0][0][0].Value = "0";
                        }

                        SendToPLC(jobLotIDTRX);
                    }
                    #endregion

                    #region  检查Job ID 是否为空

                    Trx jobIDTRX = GetTrxValues("L3_EQDJobIDCheckNGBit");

                    if (string.IsNullOrEmpty(trx[0][0]["GlassID_or_PanelID"].Value.Trim()))
                    {
                        jobIDTRX[0][0][0].Value = "1";

                        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Glass ID NG: Glass ID: " + trx[0][0]["GlassID_or_PanelID"].Value + "\r\n";

                    }
                    else
                    {
                        jobIDTRX[0][0][0].Value = "0";
                    }

                    SendToPLC(jobIDTRX);

                    #endregion

                    #region  检查Recipe是否为空

                    Trx ppidTRX = GetTrxValues("L3_EQDPPIDCheckNGBit");
                    if (line.Data.FABTYPE == "ARRAY" || line.Data.FABTYPE == "CF")
                    {
                        if (string.IsNullOrEmpty(trx[0][0]["PPID"].Value.Trim()))
                        {
                            ppidTRX[0][0][0].Value = "1";

                            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "PPID NG: PPID: " + trx[0][0]["PPID"].Value.Trim() + "\r\n";

                        }
                        else
                        {
                            ppidTRX[0][0][0].Value = "0";
                        }
                    }
                    else if (line.Data.FABTYPE == "CELL")
                    {
                        if (string.IsNullOrEmpty(trx[0][0]["PPID03"].Value.Trim()))
                        {
                            ppidTRX[0][0][0].Value = "1";

                            eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "PPID03 NG: PPID03: " + trx[0][0]["PPID03"].Value.Trim() + "\r\n";

                        }
                        else
                        {
                            ppidTRX[0][0][0].Value = "0";
                        }
                    }
                    else
                    {

                    }


                    SendToPLC(ppidTRX);

                    #endregion

                    #region  //检查run mode和Recipe是否匹配

                    //string upDataPpid01 = trx[0][0]["PPID"].Value.Trim();

                    //string upDataRecipe01 = string.IsNullOrEmpty(upDataPpid01) ? "" : upDataPpid01.Substring(2, 4).ToString();

                    //Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                    //           ObjectManager.RecipeManager.ReloadRecipe();

                    //if ((eq.File.EquipmentRunMode == "2" && _recipeEntitys[eq.Data.LINEID].ContainsKey(upDataRecipe01) &&
                    //    _recipeEntitys[eq.Data.LINEID][upDataRecipe01].Recipe_Work_Mode == "1")|| 
                    //  (eq.File.EquipmentRunMode == "3" && _recipeEntitys[eq.Data.LINEID].ContainsKey(upDataRecipe01) &&
                    //    _recipeEntitys[eq.Data.LINEID][upDataRecipe01].Recipe_Work_Mode == "0") )
                    //{

                    //}
                    //else {
                    //    actionTRX[0][0][11].Value = "1";

                    //    eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "RECIPE MISMATECH RUN MODE "  + "\r\n";

                    //}

                    #endregion

                    //检查上游设备送过来的资料
                    if (eq.File.GlassCheckMode == eEnableDisable.Enable)
                    {


                        #region  检查Recipe 是否一致

                        Trx recipeNGTRX = GetTrxValues("L3_EQDRecipeCheckNGBit");
                        if (eq.File.RecipeCheckMode == eEnableDisable.Enable && eq.File.AutoRecipeChangeMode == eEnableDisable.Disable)
                        {
                            string upDataPpid = string.Empty;
                            if (line.Data.FABTYPE == "ARRAY")
                            {
                                upDataPpid = trx[0][0]["PPID"].Value.Substring(26, 26).Trim();
                            }
                            else if (line.Data.FABTYPE == "CF")
                            {
                                if (line.Data.ATTRIBUTE == "CLN")
                                {
                                    if (line.Data.LINEID == "KWF23637R")
                                    {
                                        upDataPpid = trx[0][0]["PPID"].Value.Substring(28, 4).Trim();//PH07 28
                                    }
                                    else
                                    {
                                        upDataPpid = trx[0][0]["PPID"].Value.Substring(32, 4).Trim();//PH01~06 28+4
                                    }
                                }
                                else if (line.Data.ATTRIBUTE == "DEV")
                                {
                                    upDataPpid = trx[0][0]["PPID"].Value.Substring(56, 4).Trim();//52+4
                                }

                            }
                            else
                            {
                                upDataPpid = trx[0][0]["PPID03"].Value.Trim();
                            }


                            string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();
                            if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                            {
                                upDataRecipe = upDataRecipe.Trim();
                            }
                            else
                            {
                                Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys = ObjectManager.RecipeManager.ReloadRecipe();

                                upDataRecipe = _recipeEntitys[eq.Data.LINEID][upDataRecipe.Trim()].RECIPENO;
                            }
                            if (int.Parse(upDataRecipe) != eq.File.CurrentRecipeNo)
                            {
                                //判断当前recipe是否能用该path收片

                                //Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys =
                                //       ObjectManager.RecipeManager.ReloadRecipeByNo();
                                //if (_recipeEntitys.Count > 0 && _recipeEntitys[eq.Data.LINEID].Count > 0)
                                //{
                                //    if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataPpid))//PC无该RecipeID资料
                                //    {
                                //        //要通知设备Recipe Check NG.
                                //        recipeNGTRX[0][0][0].Value = "1";

                                //        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe NG: " + upDataRecipe + "is not exixt" + "\r\n";
                                //    }
                                //    else
                                //    {
                                //        RecipeEntityData recipeEntityData = _recipeEntitys[eq.Data.LINEID][upDataPpid];
                                //        IList<string> paramter = ObjectManager.RecipeManager.RecipeDataValues(false, recipeEntityData.FILENAME);
                                //        if (paramter != null && paramter.Count != 0)
                                //        {
                                //            string upDataPPIDBindRunMode = string.Empty;
                                //            foreach (var data in paramter)
                                //            {
                                //                if (data.Contains("Run_Mode") && data.Split('=').Length > 1)
                                //                {
                                //                    upDataPPIDBindRunMode = data.Split('=')[1].Trim();
                                //                }

                                //            }
                                //            if (upDataPPIDBindRunMode == string.Empty || upDataPPIDBindRunMode == eq.File.EquipmentRunMode)//不存在Run Mode项或者帐料中recipe对应的run mode与设备当前一致
                                //            {
                                //                recipeNGTRX[0][0][0].Value = "0";
                                //            }
                                //            if (eq.File.EquipmentRunMode != upDataPPIDBindRunMode)//不一致
                                //            {
                                //                if (eq.File.EquipmentRunMode == "1" || eq.File.EquipmentRunMode == "4")
                                //                {
                                //                    if (upDataPPIDBindRunMode == "3")//normal与mqc下不能收skip的玻璃
                                //                    {
                                //                        //要通知设备Recipe Check NG.
                                //                        recipeNGTRX[0][0][0].Value = "1";

                                //                        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe NG: " + eq.File.CurrentRecipeNo + " != " + upDataRecipe + "\r\n" + $"Current Run Mode={eq.File.EquipmentRunMode} can't change to {upDataPPIDBindRunMode}" + "\r\n";
                                //                    }
                                //                    else
                                //                    {
                                //                        recipeNGTRX[0][0][0].Value = "0";
                                //                    }
                                //                }
                                //                if (eq.File.EquipmentRunMode == "3")
                                //                {
                                //                    if (upDataPPIDBindRunMode == "1" || upDataPPIDBindRunMode == "4")//skip下不能收normal和mqc的玻璃
                                //                    {
                                //                        //要通知设备Recipe Check NG.
                                //                        recipeNGTRX[0][0][0].Value = "1";

                                //                        eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe NG: " + eq.File.CurrentRecipeNo + " != " + upDataRecipe + "\r\n" + $"Current Run Mode={eq.File.EquipmentRunMode} can't change to {upDataPPIDBindRunMode}" + "\r\n";
                                //                    }
                                //                    else
                                //                    {
                                //                        recipeNGTRX[0][0][0].Value = "0";
                                //                    }
                                //                }
                                //            }
                                //        }
                                //        else//PC无Recipe参数资料，不检查
                                //        {

                                //            recipeNGTRX[0][0][0].Value = "0";
                                //        }
                                //    }
                                //}
                                //else//PC无Recipe资料，不检查
                                //{
                                //    recipeNGTRX[0][0][0].Value = "0";
                                //}

                                recipeNGTRX[0][0][0].Value = "1";
                                if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                                {
                                    eq.File.UPDataCheckResult = $"From upstream sent job data,PPID={upDataPpid} is different with EQ's current Recipe ID(No)={eq.File.CurrentRecipeNo}";
                                }
                                else
                                {
                                    eq.File.UPDataCheckResult = $"From upstream sent job data,PPID={upDataPpid} is different with EQ's current Recipe ID={eq.File.CurrentRecipeID}";
                                }
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


                        #endregion

                        #region 检查Recipe Auto Change 

                        Trx recipeAutoNGTRX = GetTrxValues("L3_EQDRecipeAUTOChangeNGBit");
                        if (eq.File.AutoRecipeChangeMode == eEnableDisable.Enable && eq.File.RecipeCheckMode == eEnableDisable.Disable)
                        {

                            string upDataPpid = string.Empty;
                            if (line.Data.FABTYPE == "ARRAY")
                            {
                                upDataPpid = trx[0][0]["PPID"].Value.Substring(26, 26).Trim();
                            }
                            else if (line.Data.FABTYPE == "CF")
                            {
                                if (line.Data.ATTRIBUTE == "CLN")
                                {
                                    if (line.Data.LINEID == "KWF23637R")
                                    {
                                        upDataPpid = trx[0][0]["PPID"].Value.Substring(28, 4).Trim();//PH07 28
                                    }
                                    else
                                    {
                                        upDataPpid = trx[0][0]["PPID"].Value.Substring(32, 4).Trim();//PH01~06 28+4
                                    }
                                }
                                else if (line.Data.ATTRIBUTE == "DEV")
                                {
                                    upDataPpid = trx[0][0]["PPID"].Value.Substring(56, 4).Trim();//56
                                }
                            }
                            else if (line.Data.FABTYPE == "CELL")
                            {
                                upDataPpid = trx[0][0]["PPID03"].Value.Trim();
                            }

                            string upDataRecipe = string.IsNullOrEmpty(upDataPpid) ? "" : upDataPpid.ToString();
                            Dictionary<string, Dictionary<string, RecipeEntityData>> _recipeEntitys = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                            if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                            {


                                _recipeEntitys = ObjectManager.RecipeManager.ReloadRecipeByNo();
                            }
                            else
                            {
                                _recipeEntitys = ObjectManager.RecipeManager.ReloadRecipe();
                            }


                            if (!_recipeEntitys[eq.Data.LINEID].ContainsKey(upDataPpid))
                            {
                                //要通知设备Recipe Check NG.
                                recipeAutoNGTRX[0][0][0].Value = "1";

                                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Recipe Auto Change NG: " + string.Format("Recipe Table Not Contain {0} ", upDataRecipe) + "\r\n";
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

                        #region  检查设备内的Job data 是否有Duplicate 

                        Trx DupllicateCheck = GetTrxValues("L3_EQDJobDupllicateCheckNGBit");

                        if (eq.File.JobDuplicateCheckMode == eEnableDisable.Enable)
                        {
                            string cstSeq = trx[0][0][eJOBDATA.Cassette_Sequence_No].Value;
                            string slotNo = trx[0][0][eJOBDATA.Job_Sequence_No].Value;

                            Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                            if (job != null)
                            {
                                //要通知设备 Job Duplicate Check NG.
                                DupllicateCheck[0][0][0].Value = "1";

                                eq.File.UPDataCheckResult = eq.File.UPDataCheckResult + "Job Duplicate Check  NG: " + "JobData with CstSeq:" + cstSeq + ", " + "SlotNo:" + slotNo + "\r\n" + "was exist!";

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

                        #endregion




                        //值不同再写入，防止一直刷Log

                        //if (CheckTRX(actionTRX, oldTrx))
                        //{
                        //    SendToPLC(actionTRX);
                        //}

                        if (eq.File.UPDataCheckResult == string.Empty)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
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
        //正常进片
        public void WriteEQDJobData()
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDReceiveGlassDataReport#01") as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_EQDReceiveGlassDataReport#01 IN PLCFmt.xml!", "L3"));
                Trx receviceTrx = new Trx();

                LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.DownStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Lower").FirstOrDefault();
                string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Send") == true).FirstOrDefault().ToString();
                receviceTrx = GetTrxValues(trxName);

                trx.ClearTrxWith0();

                if (receviceTrx != null)
                {
                    Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                    string masterRecipeID = string.Empty;
                    string PPID = string.Empty;
                    string currentRecipe = string.Empty;
                    for (int i = 0; i < receviceTrx[0][0].Items.AllValues.Length; i++)
                    {
                        trx[0][0][i].Value = receviceTrx[0][0][i].Value;
                        if (trx[0][0][i].Name == "Master_Recipe_ID")
                        {
                            masterRecipeID = trx[0][0][i].Value;
                        }
                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            if (trx[0][0][i].Name == "PPID")
                            {
                                PPID = trx[0][0][i].Value;
                                if (PPID != string.Empty)
                                {
                                    currentRecipe = PPID.Substring(26, 26).Trim();
                                }
                            }
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (trx[0][0][i].Name == "PPID")
                            {
                                PPID = trx[0][0][i].Value;
                                if (PPID != string.Empty)
                                {
                                    if (line.Data.ATTRIBUTE == "CLN")
                                    {
                                        if (line.Data.LINEID == "KWF23637R")
                                        {
                                            currentRecipe = PPID.Substring(28, 4).Trim();//PH07 28
                                        }
                                        else
                                        {
                                            currentRecipe = PPID.Substring(32, 4).Trim();//PH01~06 28+4
                                        }

                                    }
                                    else if (line.Data.ATTRIBUTE == "DEV")
                                    {
                                        currentRecipe = PPID.Substring(56, 4).Trim();//52+4
                                    }

                                }
                            }
                        }
                        else if (line.Data.FABTYPE == "CELL")
                        {
                            if (trx[0][0][i].Name == "PPID03")
                            {
                                PPID = trx[0][0][i].Value;
                                if (PPID != "0")
                                {
                                    currentRecipe = PPID.Trim();
                                }
                            }
                        }

                    }
                    //优先通过PPID获取LocalRecipeID
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeEntitys = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        recipeEntitys = ObjectManager.RecipeManager.ReloadRecipeByNo();
                    }
                    else
                    {
                        recipeEntitys = ObjectManager.RecipeManager.ReloadRecipe();
                    }
                    if (recipeEntitys.Count == 0)
                    {
                        trx[0][0]["CurrentRecipe"].Value = "0";
                    }
                    else
                    {
                        if (recipeEntitys[eq.Data.LINEID].Keys.Contains(currentRecipe))
                        {
                            trx[0][0]["CurrentRecipe"].Value = recipeEntitys[eq.Data.LINEID][currentRecipe].RECIPENO;
                        }
                        else
                        {
                            trx[0][0]["CurrentRecipe"].Value = "0";
                        }

                    }
                    //如果通过PPID获取LocalRecipeID失败，则使用LocalRecipeIDRequest向BC请求
                    if (trx[0][0]["CurrentRecipe"].Value == "0")
                    {
                        if (line.Data.FABTYPE != "CELL")
                        {
                            CPCLocalRecipeRequest(masterRecipeID, eBitResult.ON);
                        }
                    }



                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);

                    List<string> trxNameList = linkSignalType.TrxMatch[linkSignalType.LinkKey]["DownStreamTrx"].Where(x => x.Contains("Receive") == true).ToList();

                    for (int i = 0; i < receviceTrx[0].Events.AllValues.Length; i++)
                    {
                        Trx receviceJobDataTrx = GetTrx(trxNameList[i]);

                        for (int j = 0; j < receviceTrx[0][i].Items.AllValues.Length; j++)
                        {
                            receviceJobDataTrx[0][0][j].Value = receviceTrx[0][i][j].Value.Trim();
                        }
                        receviceJobDataTrx[0][1][0].Value = int.Parse(trxName.Split('#')[1]).ToString();
                        receviceJobDataTrx[0][2][0].Value = "0";
                        receviceJobDataTrx.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(receviceJobDataTrx);

                    }
                }
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }
        //上层进片
        public void UPWriteEQDJobDta()
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDReceiveGlassDataReport#02") as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_EQDReceiveGlassDataReport#02 IN PLCFmt.xml!", "L3"));

                Trx receviceTrx = GetTrxValues("L3_OtherSendingGlassDataReport#01");

                trx.ClearTrxWith0();

                if (receviceTrx != null)
                {
                    for (int i = 0; i < receviceTrx[0].Events.AllValues.Length; i++)
                    {
                        for (int j = 0; j < receviceTrx[0][i].Items.AllValues.Length; j++)
                        {
                            trx[0][i][j].Value = receviceTrx[0][i][j].Value.Trim();
                        }
                        trx[0][i]["CurrentRecipe"].Value = receviceTrx[0][i]["PPID#02"].Value.Trim();
                    }

                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeEntitys = ObjectManager.RecipeManager.ReloadRecipeByNo();
                    if (!recipeEntitys[eq.Data.LINEID].Keys.Contains(receviceTrx[0][0]["PPID#02"].Value.Trim()))
                    {
                        trx[0][0]["CurrentRecipe"].Value = "0";
                    }

                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);



                    for (int i = 0; i < receviceTrx[0].Events.AllValues.Length; i++)
                    {
                        //写入到收片信息区域
                        Trx receviceJobDataTrx = GetTrx(string.Format("L3_ReceiveGlassDataReport#0{0}", (1).ToString()));

                        for (int j = 0; j < receviceTrx[0][i].Items.AllValues.Length; j++)//因为设备Job Data 多一个当前Recipe栏位
                        {
                            receviceJobDataTrx[0][0][j].Value = receviceTrx[0][i][j].Value.Trim();
                        }
                        receviceJobDataTrx[0][1][0].Value = "0";
                        receviceJobDataTrx.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(receviceJobDataTrx);

                    }

                    //}

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }
        //返流进片
        public void ReturnWriteEQDJobDta()
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                Trx trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDReceiveGlassDataReport#03") as Trx;
                if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX L3_EQDReceiveGlassDataReport#02 IN PLCFmt.xml!", "L3"));
                Trx receviceTrx = new Trx();

                LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.DownStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Return" && x.PathPosition == "Upper").FirstOrDefault();
                string trxName = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Send") == true).FirstOrDefault().ToString();
                receviceTrx = GetTrxValues(trxName);

                trx.ClearTrxWith0();

                if (receviceTrx != null)
                {
                    Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                    string masterRecipeID = string.Empty;
                    string PPID = string.Empty;
                    string currentRecipe = string.Empty;
                    for (int i = 0; i < receviceTrx[0][0].Items.AllValues.Length; i++)
                    {
                        trx[0][0][i].Value = receviceTrx[0][0][i].Value;
                        if (trx[0][0][i].Name == "Master_Recipe_ID")
                        {
                            masterRecipeID = trx[0][0][i].Value;
                        }
                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            if (trx[0][0][i].Name == "PPID")
                            {
                                PPID = trx[0][0][i].Value;
                                if (PPID != string.Empty)
                                {
                                    currentRecipe = PPID.Substring(26, 26).Trim();
                                }
                            }
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (trx[0][0][i].Name == "PPID")
                            {
                                PPID = trx[0][0][i].Value;
                                if (PPID != string.Empty)
                                {
                                    if (line.Data.ATTRIBUTE == "CLN")
                                    {
                                        if (line.Data.LINEID == "KWF23637R")
                                        {
                                            currentRecipe = PPID.Substring(28, 4).Trim();//PH07 28
                                        }
                                        else
                                        {
                                            currentRecipe = PPID.Substring(32, 4).Trim();//PH01~06 28+4
                                        }
                                    }
                                    else if (line.Data.ATTRIBUTE == "DEV")
                                    {
                                        currentRecipe = PPID.Substring(56, 4).Trim();//52+4
                                    }

                                }
                            }
                        }
                        else if (line.Data.FABTYPE == "CELL")
                        {
                            if (trx[0][0][i].Name == "PPID03")
                            {
                                PPID = trx[0][0][i].Value;
                                if (PPID != "0")
                                {
                                    currentRecipe = PPID.Trim();
                                }
                            }
                        }

                    }
                    //优先通过PPID获取LocalRecipeID
                    Dictionary<string, Dictionary<string, RecipeEntityData>> recipeEntitys = new Dictionary<string, Dictionary<string, RecipeEntityData>>();
                    if (line.Data.FABTYPE == "CELL")//eq.Data.LINEID == "KWF23633L"
                    {
                        recipeEntitys = ObjectManager.RecipeManager.ReloadRecipeByNo();
                    }
                    else
                    {
                        recipeEntitys = ObjectManager.RecipeManager.ReloadRecipe();
                    }
                    if (recipeEntitys.Count == 0)
                    {
                        trx[0][0]["CurrentRecipe"].Value = "0";
                    }
                    else
                    {
                        if (recipeEntitys[eq.Data.LINEID].Keys.Contains(currentRecipe))
                        {
                            trx[0][0]["CurrentRecipe"].Value = recipeEntitys[eq.Data.LINEID][currentRecipe].RECIPENO;
                        }
                        else
                        {
                            trx[0][0]["CurrentRecipe"].Value = "0";
                        }

                    }
                    //如果通过PPID获取LocalRecipeID失败，则使用LocalRecipeIDRequest向BC请求
                    if (trx[0][0]["CurrentRecipe"].Value == "0")
                    {
                        if (line.Data.FABTYPE != "CELL")
                        {
                            CPCLocalRecipeRequest(masterRecipeID, eBitResult.ON);
                        }
                    }



                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);

                    List<string> trxNameList = linkSignalType.TrxMatch[linkSignalType.LinkKey]["DownStreamTrx"].Where(x => x.Contains("Receive") == true).ToList();

                    for (int i = 0; i < receviceTrx[0].Events.AllValues.Length; i++)
                    {
                        Trx receviceJobDataTrx = GetTrx(trxNameList[i]);

                        for (int j = 0; j < receviceTrx[0][i].Items.AllValues.Length; j++)
                        {
                            receviceJobDataTrx[0][0][j].Value = receviceTrx[0][i][j].Value.Trim();
                        }
                        receviceJobDataTrx[0][1][0].Value = int.Parse(trxName.Split('#')[1]).ToString();
                        receviceJobDataTrx[0][2][0].Value = "0";
                        receviceJobDataTrx.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(receviceJobDataTrx);

                    }
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }

        //正常出片写资料
        public void WriteWEQDJobDta()
        {
            try
            {
                Trx eqdata = GetTrxValues("L3_EQDSendingGlassDataReport#01");
                if (eqdata != null)
                {
                    Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");
                    Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);

                    LinkSignalType linkSignalType = new LinkSignalType();
                    if (line.Data.FABTYPE != "CELL")
                    {
                        linkSignalType = Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Lower").FirstOrDefault();
                    }
                    else
                    {
                        if (CurrentGlassSendToTR == eTransfer.TR02)
                        {
                            linkSignalType = Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Upper").FirstOrDefault();
                        }
                        else
                        {
                            linkSignalType = Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Lower").FirstOrDefault();
                        }
                    }

                    List<string> trxNameList = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Send") == true).ToList();
                    for (int i = 0; i < trxNameList.Count; i++)
                    {
                        Trx sendData = GetTrxValues(trxNameList[i]);

                        if (line.Data.FABTYPE == "CELL")//eqp.Data.NODENAME == "KWF23633L"
                        {
                            //if (eq.File.TR02TransferEnable == eEnableDisable.Enable)
                            //{
                            //    if (i == 0 || i == 2)
                            //    {
                            //        continue;
                            //    }
                            //}
                            //else
                            //{
                            //    if (i == 1 || i == 3)
                            //    {
                            //        continue;
                            //    }
                            //}
                            if (i == 0 && eqp.File.DownActionMonitor[2] == "0")//清除资料
                            {
                                sendData.ClearTrx();
                                SendToPLC(sendData);
                                continue;
                            }
                            else if (i == 1 && eqp.File.DownActionMonitor[3] == "0")
                            {
                                sendData.ClearTrx();
                                SendToPLC(sendData);
                                continue;
                            }

                        }
                        for (int j = 0; j < sendData[0][0].Items.Count; j++)
                        {
                            sendData[0][0][j].Value = eqdata[0][i][j].Value;
                        }
                        if (line.Data.FABTYPE.ToUpper() == "ARRAY")
                        {
                            //sendData[0][0]["Process_Flag"].Value = "0010000000000000";
                            string res = SetBitOn(eqdata[0][i]["Process_Flag"].Value, 2);
                            sendData[0][0]["Process_Flag"].Value = res;
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            //sendData[0][0]["Process_Flag"].Value = "0100000000000000";
                            if (sendData[0][0]["Skip_Flag"].Value.Substring(1, 1) != "1")
                            {
                                string res = string.Empty;
                                if (line.Data.ATTRIBUTE.ToUpper() == "CLN")
                                {
                                    res = SetBitOn(eqdata[0][i]["Process_Flag"].Value, 1);
                                }
                                else if (line.Data.ATTRIBUTE.ToUpper() == "DEV")
                                {
                                    res = SetBitOn(eqdata[0][i]["Process_Flag"].Value, 9);
                                }

                                sendData[0][0]["Process_Flag"].Value = res;
                            }
                        }
                        else if (line.Data.FABTYPE == "CELL")
                        {
                            string res = string.Empty;
                            //sendData[0][0]["Process_Flag"].Value = "0010000000000000";
                            res = SetBitOn(eqdata[0][i]["Process_Flag"].Value, 2);
                            sendData[0][0]["Process_Flag"].Value = res;
                        }
                        sendData[0][1][0].Value = int.Parse(sendData.Name.Split('#')[1]).ToString();
                        sendData[0][2][0].Value = "0";
                        sendData.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(sendData);
                    }

                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }


        }
        //上层出片写资料
        public void UPWriteWEQDJobDta()
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");
                Trx eqdata = GetTrxValues("L3_EQDSendingGlassDataReport#02");

                if (eqdata != null)
                {
                    LinkSignalType linkSignalType = Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == $"{eq.Data.ATTRIBUTE}" && x.FlowType == "Normal" && x.PathPosition == "Upper").FirstOrDefault();
                    List<string> trxNameList = linkSignalType.TrxMatch[linkSignalType.LinkKey]["UpStreamTrx"].Where(x => x.Contains("Send") == true).ToList();
                    for (int i = 0; i < trxNameList.Count; i++)
                    {
                        Trx sendData = GetTrxValues(trxNameList[i]);
                        for (int j = 0; j < sendData[0][0].Items.Count; j++)
                        {
                            sendData[0][0][j].Value = eqdata[0][i][j].Value;
                        }
                        //sendData[0][0]["Process_Flag"].Value = "0000000000000000";
                        sendData[0][1][0].Value = int.Parse(sendData.Name.Split('#')[1]).ToString();
                        sendData[0][2][0].Value = "0";
                        sendData.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(sendData);
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
        //返流出片写资料
        public void ReturnWriteWEQDJobDta()
        {
            try
            {
                Trx eqdata = GetTrxValues("L3_EQDSendingGlassDataReport#03");

                if (eqdata != null)
                {
                    Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");
                    Trx sendData = GetTrx("L3_SendingGlassDataReport#01");
                    for (int i = 0; i < sendData[0][0].Items.Count; i++)
                    {
                        sendData[0][0][i].Value = eqdata[0][0][i].Value;
                    }
                    sendData[0][0]["ProcessingFlag"].Value = "0100000000000000";
                    sendData[0][1][0].Value = "0";
                    sendData.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(sendData);
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

        public void CPCSendOutReport(bool up, eBitResult result, string position)
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");
                Line line = ObjectManager.LineManager.GetLine();
                List<int> pathNo = new List<int>();

                if (line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP")
                {
                    if (up)
                    {
                        pathNo.Add(1);//上层返流出
                    }
                    else
                    {
                        pathNo.Add(2);
                    }
                }
                else if (line.Data.ATTRIBUTE == "CLN" || line.Data.ATTRIBUTE == "DEV")
                {
                    pathNo.Add(1);
                }
                else if (line.Data.ATTRIBUTE == "ODF")
                {
                    if (SendGlassPositionBit.Contains(11))//双片
                    {
                        if (CurrentGlassSendToTR == eTransfer.TR02)
                        {
                            pathNo.Add(2);
                            pathNo.Add(4);
                        }
                        else
                        {
                            pathNo.Add(1);
                            pathNo.Add(3);
                        }
                    }
                    else
                    {
                        if (CurrentGlassSendToTR == eTransfer.TR02)
                        {
                            if (SendGlassPositionBit.Contains(13))//下层
                            {
                                pathNo.Add(2);
                            }
                            else if (SendGlassPositionBit.Contains(14))//上层
                            {
                                pathNo.Add(4);
                            }
                        }
                        else
                        {
                            if (SendGlassPositionBit.Contains(13))//下层
                            {
                                pathNo.Add(1);
                            }
                            else if (SendGlassPositionBit.Contains(14))//上层
                            {
                                pathNo.Add(3);
                            }
                        }
                    }

                }
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

                    Trx sendData = GetTrxValues(string.Format("L3_SendingGlassDataReport#0{0}", No));

                    string eqpNo = sendData.Metadata.NodeNo;
                    string pathNo = sendData.Name.Split(new char[] { '#' })[1];
                    string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, SendingGlassDataReportTimeout);

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    if (sendData != null)
                    {
                        if (result == eBitResult.OFF)
                        {
                            //sendData.ClearTrxWith0();
                            sendData[0][2][0].Value = "0";
                            SendToPLC(sendData);


                            // Thread.Sleep(1000);
                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format(
                                    "[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Sending Glass Data Report#{2}.",
                                    eqpNo, sendData.TrackKey, pathNo));

                            return;
                        }
                        else
                        {
                            string cstSeq = sendData[0][0][eJOBDATA.Cassette_Sequence_No].Value;
                            string slotNo = sendData[0][0][eJOBDATA.Job_Sequence_No].Value;
                            string glassID = sendData[0][0][eJOBDATA.GlassID_or_PanelID].Value.Trim(); //取出glass ID

                            Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(eqpNo);

                            Job job = ObjectManager.JobManager.GetJob(glassID); //通过glass ID 找到JOB


                            if (job != null)
                            {
                                job.StartTime = job.StartTime == DateTime.MinValue ? DateTime.Now : job.StartTime;
                                //记录玻璃出片的时间
                                job.EndTime = DateTime.Now;

                                UpdateJobData(eq, job, sendData); // Update 全部资料
                                                                  //   job.CurrentUnitNo = int.Parse(sendData[0][0][eJOBDATA.UnitNo].Value.Trim());
                                Trx processData;
                                int position;

                                //添加上报ProcessDataReport&&TackTimeReport
                                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                                if (line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP")
                                {
                                    if (No == 2)//下层
                                    {
                                        processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", 1));
                                    }
                                    else
                                    {
                                        processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", 2));
                                    }
                                }
                                else
                                {
                                    processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", No));
                                }

                                if (line.Data.ATTRIBUTE == "ODF")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "11").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "CLN")
                                {

                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "6").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "DEV")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "9").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "ETCH")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "24").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "STRIP")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "25").Select(q => q.Key).First();
                                }
                                else
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Keys.Max();
                                }
                                //}
                                //}
                                List<ProcessDataReportItem> tempList = new List<ProcessDataReportItem>();

                                foreach (Item item in processData[0][0].Items.AllValues)
                                {
                                    ProcessDataReportItem itemTemp = new ProcessDataReportItem();
                                    itemTemp.Name = item.Name;
                                    itemTemp.Value = item.Value;

                                    tempList.Add(itemTemp);
                                }

                                //添加最后一单元Fecth上报
                                if (!eq.File.FetchJobs.ContainsKey(position))
                                {
                                    if ((line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP") && No == 1)
                                    {
                                        //返流不上报fetch
                                    }
                                    else
                                    {
                                        eq.File.FetchJobs.TryAdd(position, job);
                                    }
                                }

                                if ((line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP") && No == 1)
                                {
                                    //返流不上报DV
                                }
                                else
                                {
                                    eq.File.ProcessDataJobs.Add(job, processData);
                                }
                                //eq.File.TackTimeJobs.Add(job);
                                ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                                Thread.Sleep(500);
                                Timermanager.CreateTimer(timerID, false, 6000,
                                    new System.Timers.ElapsedEventHandler(CPCSendingGlassDataReportTimeoutAction),
                                    UtilityMethod.GetAgentTrackKey());

                                sendData[0][2][0].Value = "1";
                                sendData[0][2].OpDelayTimeMS = 300;

                                SendToPLC(sendData);

                                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                  string.Format(
                                      "[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[ON]  Sending Glass Data Report#{2} CST_SEQNO=[{3}] JOB_SEQNO=[{4}] GLASS_ID=[{5}].",
                                      eq.Data.NODENO, sendData.TrackKey, pathNo, cstSeq, slotNo, glassID));


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


                                //   job.CurrentUnitNo = int.Parse(sendData[0][0][eJOBDATA.UnitNo].Value.Trim());

                                Trx processData;
                                int position;

                                //添加上报ProcessDataReport&&TackTimeReport
                                Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);

                                if (line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP")
                                {
                                    if (No == 2)//下层
                                    {
                                        processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", 1));
                                    }
                                    else
                                    {
                                        processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", 2));
                                    }
                                }
                                else
                                {
                                    processData = GetTrxValues(string.Format("L3_EQDProcessDataReport#0{0}", No));
                                }

                                if (line.Data.ATTRIBUTE == "ODF")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "11").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "CLN")
                                {

                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "6").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "DEV")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "9").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "ETCH")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "24").Select(q => q.Key).First();
                                }
                                else if (line.Data.ATTRIBUTE == "STRIP")
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Where(q => q.Value.UnitNo == "25").Select(q => q.Key).First();
                                }
                                else
                                {
                                    position = ObjectManager.EquipmentManager.GetPositionData(eq.Data.LINEID).Keys.Max();
                                }
                                List<ProcessDataReportItem> tempList = new List<ProcessDataReportItem>();

                                foreach (Item item in processData[0][0].Items.AllValues)
                                {
                                    ProcessDataReportItem itemTemp = new ProcessDataReportItem();
                                    itemTemp.Name = item.Name;
                                    itemTemp.Value = item.Value;

                                    tempList.Add(itemTemp);
                                }

                                //添加最后一单元Fecth上报

                                if (!eq.File.FetchJobs.ContainsKey(position))
                                {
                                    if ((line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP") && No == 1)
                                    {
                                        //返流不上报fetch
                                    }
                                    else
                                    {
                                        eq.File.FetchJobs.TryAdd(position, job);
                                    }
                                }

                                if ((line.Data.ATTRIBUTE == "ETCH" || line.Data.ATTRIBUTE == "STRIP") && No == 1)
                                {
                                    //返流不上报DV
                                }
                                else
                                {
                                    eq.File.ProcessDataJobs.Add(job, processData);
                                }
                                //eq.File.TackTimeJobs.Add(job);

                                Thread.Sleep(1000);
                                Timermanager.CreateTimer(timerID, false, 6000,
                                    new System.Timers.ElapsedEventHandler(CPCSendingGlassDataReportTimeoutAction),
                                    UtilityMethod.GetAgentTrackKey());

                                sendData[0][2][0].Value = "1";
                                sendData[0][2].OpDelayTimeMS = 300;

                                SendToPLC(sendData);


                                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                                  string.Format(
                                      "[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[ON]  Sending Glass Data Report#{2} CST_SEQNO=[{3}] JOB_SEQNO=[{4}] GLASS_ID=[{5}],Job NO Exist ",
                                      eq.Data.NODENO, sendData.TrackKey, pathNo, cstSeq, slotNo, glassID));


                                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Deliver.ToString(),
                                    eq.Data.NODEID,
                                    eq.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");


                                ObjectManager.JobManager.DeleteJob(job);
                            }
                        }
                    }


                    Thread.Sleep(500);
                }

            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        public void CPCReceiveJobDataReport(eBitResult result, int No)
        {
            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP("L3");

                List<int> pathNo = new List<int>();

                if (eq.Data.LINEID == "KWF22093L")//双片
                {
                    if (result == eBitResult.ON)
                    {
                        pathNo.Add(1);
                        pathNo.Add(2);
                    }
                    else
                    {
                        pathNo.Add(No);
                    }
                }
                else
                {
                    pathNo.Add(No);
                }


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

                    Trx receiveData = GetTrxValues(string.Format("L3_ReceiveGlassDataReport#0{0}", No));

                    string eqpNo = receiveData.Metadata.NodeNo;
                    string pathNo = receiveData.Name.Split(new char[] { '#' })[1];

                    string timerID = string.Format("{0}_{1}_{2}", eqpNo, pathNo, ReceiveGlassDataReportTimeout);

                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    if (receiveData != null)
                    {
                        if (result == eBitResult.OFF)
                        {
                            // receiveData.ClearTrxWith0();
                            receiveData[0][2][0].Value = "0";
                            SendToPLC(receiveData);
                            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                                string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF]  Receive Job Data Report#{2}.",
                                    eqpNo, receiveData.TrackKey, pathNo));
                            return;

                        }

                        receiveData[0][2][0].Value = "1";
                        receiveData[0][2].OpDelayTimeMS = 200;//ParameterManager[eParameterName.LinkSignalT1].GetInteger();

                        SendToPLC(receiveData);

                        Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                            new System.Timers.ElapsedEventHandler(CPCReceiveGlassDataReportTimeoutAction), UtilityMethod.GetAgentTrackKey());



                        #region [拆出PLCAgent Data]  Word
                        string cstSeq = receiveData[0][0][eJOBDATA.Cassette_Sequence_No].Value;
                        string slotNo = receiveData[0][0][eJOBDATA.Job_Sequence_No].Value;
                        string glassID = receiveData[0][0][eJOBDATA.GlassID_or_PanelID].Value.Trim();

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
                                    eq.Data.NODENO, receiveData.TrackKey, eq.File.CIMMode, pathNo, cstSeq, slotNo, glassID));

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
                                    eq.Data.NODENO, receiveData.TrackKey, eq.File.CIMMode, pathNo, cstSeq, slotNo, glassID));

                            ObjectManager.JobManager.EnqueueSave(job);
                            ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Receive.ToString(), eq.Data.NODEID, eq.Data.NODENO, "1", "", "", "");
                        }
                        var res = dicTransferTimes.Where(x => x.Value.GlassID == job.GlassID_or_PanelID.Trim()).FirstOrDefault();
                        job.ReceiveAbleTime = dicTransferTimes[res.Key].ReceiveAbleTime;
                        job.ReceiveStartTime = dicTransferTimes[res.Key].ReceiveStartTime;
                        job.ReceiveCompleteTime = dicTransferTimes[res.Key].ReceiveCompleteTime;
                        job.ReceiveReadyTime = dicTransferTimes[res.Key].ReceiveReadyTime;
                        job.TransactionID = res.Key;
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

                string timerID = "L3_EQDDownSendOutTimeOutBitBit_Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, TimeoutBitAutoReset, UtilityMethod.GetAgentTrackKey());


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

                string timerID = "L3_EQDUpSendOutTimeOutBit_Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, TimeoutBitAutoReset, UtilityMethod.GetAgentTrackKey());

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
        private void ReturnSendTimerTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Return Send Timer[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.SendTimer].GetInteger().ToString()));

                Trx actionTRX = GetTrxValues("L3_EQDReturnSendOutTimeOutBitBit");
                actionTRX[0][0][0].Value = "1";

                SendToPLC(actionTRX);

                string timerID = "L3_EQDReturnSendOutTimeOutBitBit_Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, TimeoutBitAutoReset, UtilityMethod.GetAgentTrackKey());

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

                SendToPLC(actionTRX);

                string timerID = "L3_EQDReciveTimeOutBit_Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, TimeoutBitAutoReset, UtilityMethod.GetAgentTrackKey());

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

        private void UpReceiveTimerTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Up Receive Timer[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.ReceiveTimer].GetInteger().ToString()));

                Trx actionTRX = GetTrxValues("L3_EQDUpReciveTimeOutBit");
                actionTRX[0][0][0].Value = "1";

                SendToPLC(actionTRX);

                string timerID = "L3_EQDUpReciveTimeOutBit_Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, TimeoutBitAutoReset, UtilityMethod.GetAgentTrackKey());

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
        private void ReturnReceiveTimerTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] Return Receive Timer[{2}] TIMEOUT.", sArray[0], trackKey, ParameterManager[eParameterName.ReceiveTimer].GetInteger().ToString()));

                Trx actionTRX = GetTrxValues("L3_EQDReturnReciveTimeOutBit");
                actionTRX[0][0][0].Value = "1";

                SendToPLC(actionTRX);

                string timerID = "L3_EQDReturnReciveTimeOutBit_Timeout";
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                Timermanager.CreateTimer(timerID, false, 3000, TimeoutBitAutoReset, UtilityMethod.GetAgentTrackKey());

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
        private void TimeoutBitAutoReset(object subject, System.Timers.ElapsedEventArgs e)
        {
            UserTimer timer = subject as UserTimer;
            string tmp = timer.TimerId;
            string trackKey = timer.State.ToString();
            string[] sArray = tmp.Split('_');

            string trxName = sArray[0] + "_" + sArray[1];

            Trx actionTRX = GetTrxValues(trxName);
            actionTRX[0][0][0].Value = "0";
            actionTRX.TrackKey = UtilityMethod.GetAgentTrackKey();
            SendToPLC(actionTRX);
            LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EQ <- EC][{1}] {2} Bit Auto Reset.", sArray[0], actionTRX.TrackKey, sArray[1]));

        }

        public LinkSignalType GetSignalType(object obj)
        {
            LinkSignalType linkSignalType = new LinkSignalType();
            if ((obj as LinkSignalType) == null)
            {
                return null;
            }
            linkSignalType = obj as LinkSignalType;
            return linkSignalType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBin">需要处理的BIN格式字符串</param>
        /// <param name="index">index∈[0,+∞)</param>
        /// <returns>处理结果</returns>
        public string SetBitOn(string strBin, int index)
        {
            string res = string.Empty;
            char[] chArray = strBin.ToCharArray();
            chArray[index] = '1';
            res = new string(chArray);
            return res;
        }


        public class LinkSignalType
        {
            public string UpStreamLocalNo = string.Empty;
            public string DownStreamLocalNo = string.Empty;
            public string SeqNo = string.Empty;
            public string LinkType = string.Empty;
            public string TimingChart = string.Empty;
            public string FlowType = string.Empty;
            public string PathPosition = string.Empty;
            public string LinkKey = string.Empty;
            public Dictionary<string, Dictionary<string, List<string>>> TrxMatch = new Dictionary<string, Dictionary<string, List<string>>>();
        }
        public class TransferTime
        {
            public TransferTime()
            {

            }

            public DateTime ReceiveAbleTime { get; set; }

            public DateTime ReceiveStartTime { get; set; }

            public DateTime ReceiveCompleteTime { get; set; }

            public DateTime SendAbleTime { get; set; }

            public DateTime SendStartTime { get; set; }

            public DateTime SendCompleteTime { get; set; }

            public DateTime ReceiveReadyTime { get; set; }

            public DateTime SendReadyTime { get; set; }

            public string GlassID { get; set; } = string.Empty;

            public string CassetteSequenceNo { get; set; } = "0";

            public string SlotSequenceNo { get; set; } = "0";

            public string TransactionID { get; set; } = string.Empty;
        }


        private bool CheckTransferOnForCVToRBDual_TR01()
        {
            bool res = false;
            if (SendGlassPositionBit.Contains(11))//双片出
            {
                if ((GetOnBits(eqp.File.DownInterface01).Contains(7) || GetOnBits(eqp.File.DownInterface01).Contains(8)) || (eqp.File.DownActionMonitor[2] == "0" && eqp.File.DownActionMonitor[3] == "0"))
                {
                    res = true;
                }
            }
            else//单片出
            {
                if (SendGlassPositionBit.Contains(13))//下单片
                {
                    if (GetOnBits(eqp.File.DownInterface01).Contains(7) || GetOnBits(eqp.File.DownInterface01).Contains(8) || eqp.File.DownActionMonitor[2] == "0")
                    {
                        res = true;
                    }
                }
                else if (SendGlassPositionBit.Contains(14))//上单片
                {
                    if (GetOnBits(eqp.File.DownInterface01).Contains(7) || GetOnBits(eqp.File.DownInterface01).Contains(8) || eqp.File.DownActionMonitor[3] == "0")
                    {
                        res = true;
                    }
                }
            }

            return res;

        }

        private bool CheckTransferOnForCVToRBDual_TR02()
        {
            bool res = false;
            if (SendGlassPositionBit.Contains(11))//双片出
            {
                if ((GetOnBits(eqp.File.DownInterface02).Contains(7) || GetOnBits(eqp.File.DownInterface02).Contains(8)) || (eqp.File.DownActionMonitor[2] == "0" && eqp.File.DownActionMonitor[3] == "0"))
                {
                    res = true;
                }
            }
            else//单片出
            {
                if (SendGlassPositionBit.Contains(13))//下单片
                {
                    if (GetOnBits(eqp.File.DownInterface02).Contains(7) || GetOnBits(eqp.File.DownInterface02).Contains(8) || eqp.File.DownActionMonitor[2] == "0")
                    {
                        res = true;
                    }
                }
                else if (SendGlassPositionBit.Contains(14))//上单片
                {
                    if (GetOnBits(eqp.File.DownInterface02).Contains(7) || GetOnBits(eqp.File.DownInterface02).Contains(8) || eqp.File.DownActionMonitor[3] == "0")
                    {
                        res = true;
                    }
                }
            }

            return res;

        }

        private void SetSendTransferTimeData(string glassID, string cstNo, string slotNo, eSendStep sendStep)
        {
            Job job = ObjectManager.JobManager.GetJob(glassID);
            KeyValuePair<string, TransferTime> res = dicTransferTimes.Where(x => x.Value.GlassID == glassID).FirstOrDefault();
            string transactionID = string.Empty;
            if (res.Key != null && res.Value != null)
            {
                transactionID = res.Key;
                switch (sendStep)
                {
                    case eSendStep.PerSend:
                        dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                        dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                        dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                        break;
                    case eSendStep.SendAble:
                        dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                        dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                        dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                        break;
                    case eSendStep.SendStart:
                        dicTransferTimes[transactionID].SendStartTime = DateTime.Now;
                        dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                        dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                        break;
                    case eSendStep.SendComPlete:
                        dicTransferTimes[transactionID].SendCompleteTime = DateTime.Now;
                        dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                        dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                        break;
                }
            }
            else
            {
                if (job == null)
                {
                    transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    if (!dicTransferTimes.ContainsKey(transactionID))
                    {
                        dicTransferTimes.Add(transactionID, new TransferTime());
                    }
                    switch (sendStep)
                    {
                        case eSendStep.PerSend:
                            dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                            dicTransferTimes[transactionID].GlassID = glassID;
                            dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                            dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                            break;
                        case eSendStep.SendAble:
                            dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                            dicTransferTimes[transactionID].GlassID = glassID;
                            dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                            dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                            break;
                        case eSendStep.SendStart:
                            dicTransferTimes[transactionID].SendStartTime = DateTime.Now;
                            dicTransferTimes[transactionID].GlassID = glassID;
                            dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                            dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                            break;
                        case eSendStep.SendComPlete:
                            dicTransferTimes[transactionID].SendCompleteTime = DateTime.Now;
                            dicTransferTimes[transactionID].GlassID = glassID;
                            dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                            dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                            break;
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(job.TransactionID))
                    {
                        transactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        if (!dicTransferTimes.ContainsKey(transactionID))
                        {
                            dicTransferTimes.Add(transactionID, new TransferTime());
                        }
                        switch (sendStep)
                        {
                            case eSendStep.PerSend:
                                dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                                dicTransferTimes[transactionID].GlassID = glassID;
                                dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                break;
                            case eSendStep.SendAble:
                                dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                                dicTransferTimes[transactionID].GlassID = glassID;
                                dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                break;
                            case eSendStep.SendStart:
                                dicTransferTimes[transactionID].SendStartTime = DateTime.Now;
                                dicTransferTimes[transactionID].GlassID = glassID;
                                dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                break;
                            case eSendStep.SendComPlete:
                                dicTransferTimes[transactionID].SendCompleteTime = DateTime.Now;
                                dicTransferTimes[transactionID].GlassID = glassID;
                                dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                break;
                        }
                    }
                    else
                    {
                        transactionID = job.TransactionID;
                        if (dicTransferTimes.ContainsKey(transactionID))
                        {
                            switch (sendStep)
                            {
                                case eSendStep.PerSend:
                                    dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                                    break;
                                case eSendStep.SendAble:
                                    dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                                    break;
                                case eSendStep.SendStart:
                                    dicTransferTimes[transactionID].SendStartTime = DateTime.Now;
                                    break;
                                case eSendStep.SendComPlete:
                                    dicTransferTimes[transactionID].SendCompleteTime = DateTime.Now;
                                    break;
                            }
                        }
                        else
                        {
                            dicTransferTimes.Add(transactionID, new TransferTime());
                            switch (sendStep)
                            {
                                case eSendStep.PerSend:
                                    dicTransferTimes[transactionID].SendReadyTime = DateTime.Now;
                                    dicTransferTimes[transactionID].GlassID = glassID;
                                    dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                    dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                    break;
                                case eSendStep.SendAble:
                                    dicTransferTimes[transactionID].SendAbleTime = DateTime.Now;
                                    dicTransferTimes[transactionID].GlassID = glassID;
                                    dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                    dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                    break;
                                case eSendStep.SendStart:
                                    dicTransferTimes[transactionID].SendStartTime = DateTime.Now;
                                    dicTransferTimes[transactionID].GlassID = glassID;
                                    dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                    dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                    break;
                                case eSendStep.SendComPlete:
                                    dicTransferTimes[transactionID].SendCompleteTime = DateTime.Now;
                                    dicTransferTimes[transactionID].GlassID = glassID;
                                    dicTransferTimes[transactionID].CassetteSequenceNo = cstNo;
                                    dicTransferTimes[transactionID].SlotSequenceNo = slotNo;
                                    break;
                            }
                        }
                    }
                    dicTransferTimes[transactionID].ReceiveAbleTime = job.ReceiveAbleTime;
                    dicTransferTimes[transactionID].ReceiveStartTime = job.ReceiveStartTime;
                    dicTransferTimes[transactionID].ReceiveCompleteTime = job.ReceiveCompleteTime;
                    dicTransferTimes[transactionID].ReceiveReadyTime = job.ReceiveReadyTime;
                }
            }
        }

        private void ReceiveCompleteToEQ(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx actionTRX = GetTrxValues("L3_EQDReciveCompleteBit");
                actionTRX[0][0][0].Value = "1";
                SendToPLC(actionTRX);


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Receive Complete To EQ Bit On.", sArray[0], trackKey));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void UpReceiveCompleteToEQ(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx actionTRX = GetTrxValues("L3_EQDUpReciveCompleteBit");
                actionTRX[0][0][0].Value = "1";
                SendToPLC(actionTRX);


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Up Receive Complete To EQ Bit On.", sArray[0], trackKey));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        private void ReturnReceiveCompleteToEQ(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                Trx actionTRX = GetTrxValues("L3_EQDReturnReciveCompleteBit");
                actionTRX[0][0][0].Value = "1";
                SendToPLC(actionTRX);


                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Return Receive Complete To EQ Bit On.", sArray[0], trackKey));

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
    }
}
