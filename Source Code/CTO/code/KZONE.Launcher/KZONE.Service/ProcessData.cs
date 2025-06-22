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
using KZONE.PLCAgent.PLC;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using KZONE.PLCAgent.PLC.KZONE.PLCAgent.Data;
using System.IO;

namespace KZONE.Service
{
    public partial class EquipmentService
    {
        private const string ProcessDataReportTimeout = "ProcessDataReportTimeout";
        private const string ProcessDataReportReplyTimeout = "ProcessDataReportReplyTimeout";
        private const string CPCProcessDataReportTimeout = "CPCProcessDataReportTimeout";
        private const string CPCProcessDataReportReplyTimeout = "CPCProcessDataReportReplyTimeout";
        private Dictionary<string, string> DFSTransferFile = new Dictionary<string, string>();
        private bool DFSTransferFlag = false;

        public void CPCProcessDataReport()
        {

            while (true)
            {
                try
                {
                    Thread.Sleep(50);
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                    if (_isRuning && ProcessDataReportFlag && eq.File.ProcessDataJobs.Count > 0)
                    {
                        Job job = eq.File.ProcessDataJobs.First().Key;
                        Trx partrx = eq.File.ProcessDataJobs[job];

                        Trx trx = GetTrxValues(string.Format("{0}_ProcessDataReport", eq.Data.NODENO));
                        if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1}_ProcessDataReport IN PLCFmt.xml!", eq.Data.NODENO, eq.Data.NODENO));

                        trx.ClearTrxWith0();

                        //SetJobDataD(eq, job, trx);
                        Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                        trx[0][0]["GlassID"].Value = job.GlassID_or_PanelID;
                        trx[0][0]["CassetteSequenceNo"].Value = job.Cassette_Sequence_No;
                        trx[0][0]["SlotSequenceNo"].Value = job.Job_Sequence_No;
                        if (job.ProStratTime == null || job.ProStratTime == DateTime.MinValue)
                        {
                            job.ProStratTime = DateTime.Now;
                        }
                        string processStartTime = job.ProStratTime.ToString("yyyyMMddHHmmssfff");
                        trx[0][0]["ProcessStartTime_Year"].Value = processStartTime.Substring(0, 4);
                        trx[0][0]["ProcessStartTime_Month"].Value = processStartTime.Substring(4, 2);
                        trx[0][0]["ProcessStartTime_Day"].Value = processStartTime.Substring(6, 2);
                        trx[0][0]["ProcessStartTime_Hour"].Value = processStartTime.Substring(8, 2);
                        trx[0][0]["ProcessStartTime_Minute"].Value = processStartTime.Substring(10, 2);
                        trx[0][0]["ProcessStartTime_Second"].Value = processStartTime.Substring(12, 2);
                        //if (line.Data.FABTYPE == "CELL")
                        //{
                        //    trx[0][0]["ProcessStartTime_Millisecond"].Value = processStartTime.Substring(14, 3);
                        //}
                        if (job.ProEndTime == null || job.ProEndTime == DateTime.MinValue)
                        {
                            job.ProEndTime = DateTime.Now;
                        }
                        string processEndTime = job.ProEndTime.ToString("yyyyMMddHHmmssfff");
                        trx[0][0]["ProcessEndTime_Year"].Value = processEndTime.Substring(0, 4);
                        trx[0][0]["ProcessEndTime_Month"].Value = processEndTime.Substring(4, 2);
                        trx[0][0]["ProcessEndTime_Day"].Value = processEndTime.Substring(6, 2);
                        trx[0][0]["ProcessEndTime_Hour"].Value = processEndTime.Substring(8, 2);
                        trx[0][0]["ProcessEndTime_Minute"].Value = processEndTime.Substring(10, 2);
                        trx[0][0]["ProcessEndTime_Second"].Value = processEndTime.Substring(12, 2);
                        //if (line.Data.FABTYPE == "CELL")
                        //{
                        //    trx[0][0]["ProcessEndTime_Millisecond"].Value = processEndTime.Substring(14, 3);
                        //}

                        //添加参数

                        for (int j = 0; j < partrx[0][0].Items.AllValues.Length; j++)
                        {
                            trx[0][1][j].Value = partrx[0][0][j].Value.Trim();
                        }


                        string timerID = string.Format("{0}_{1}", eq.Data.NODENO, CPCProcessDataReportTimeout);

                        if (Timermanager.IsAliveTimer(timerID))
                        {
                            Timermanager.TerminateTimer(timerID);
                        }

                        if (eq.File.CIMMode == eBitResult.ON)
                        {
                            Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T1].GetInteger(),
                                new System.Timers.ElapsedEventHandler(CPCProcessDataReportTimeoutAction), trx.TrackKey);

                            trx[0][2][0].Value = "1";
                            trx[0][2].OpDelayTimeMS = ParameterManager[eParameterName.EventDelayTime].GetInteger();
                        }
                        else
                        {
                            trx[0][2][0].Value = "0";
                        }
                        trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                        SendToPLC(trx);

                        //设置为False
                        ProcessDataReportFlag = false;

                        job.StartTime = job.StartTime == DateTime.MinValue ? DateTime.Now : job.StartTime;
                        job.EndTime = job.EndTime == DateTime.MinValue ? DateTime.Now : job.EndTime;
                        string startTime = job.StartTime == DateTime.MinValue ? DateTime.Now.ToString() : job.StartTime.ToString();
                        string endTime = job.EndTime == DateTime.MinValue ? DateTime.Now.ToString() : job.EndTime.ToString();
                        string processTime = (job.EndTime - job.StartTime).TotalSeconds.ToString();

                        IList<ProcessDataReportItem> items = new List<ProcessDataReportItem>();


                        string history = ParserProcessDataItem(eq, job, partrx[0][0].RawData, ref items, trx.TrackKey);

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                           string.Format("[EQUIPMENT={0}] [TrackKey={1}] [CstSeqNo={2}] [JobSeqNo={3}] [GlassID={4}] [LocalProcessingTime={5}] [LocalProcessStartTime={6}] [LocalProcessEndTime={7}]",
                           trx.Metadata.NodeNo, trx.TrackKey, job.CassetteSequenceNo, job.JobSequenceNo, job.GlassID_or_PanelID, processTime, startTime, endTime));


                        Logger.LogTrxWrite(this.LogName,
                            string.Format("[EQUIPMENT={0}] [TrackKey={1}] [CstSeqNo={2}] [JobSeqNo={3}] [GlassID={4}] [ProcessingTime={5}] [StartTime={6}] [EndTime={7}]\r\n[RAWDATA={8}]",
                            trx.Metadata.NodeNo, trx.TrackKey, job.CassetteSequenceNo, job.JobSequenceNo, job.GlassID_or_PanelID, processTime, startTime, endTime, history.Replace(',', '\n')));

                        ObjectManager.ProcessDataManager.MakeProcessDataValuesToFile(eq.Data.NODEID, job.CassetteSequenceNo.ToString(), job.JobSequenceNo.ToString(), trx.TrackKey, history);

                        ObjectManager.ProcessDataManager.MakeProcessDataValuesToEXCEL(job.GlassID_or_PanelID, job.CassetteSequenceNo.ToString(), job.JobSequenceNo.ToString(), trx.TrackKey, history);

                        #region Save History
                        ProcessDataHistory processDataHis = new ProcessDataHistory();
                        processDataHis.CASSETTESEQNO = job.CassetteSequenceNo;
                        processDataHis.JOBID = job.GlassID_or_PanelID;
                        processDataHis.JOBSEQNO = job.JobSequenceNo;
                        processDataHis.PROCESSTIME = (int)double.Parse(processTime);
                        processDataHis.LOCALPROCESSSTARTTIME = startTime;
                        processDataHis.LOCALPROCSSSENDTIME = endTime;
                        processDataHis.NODEID = eq.Data.NODEID;
                        processDataHis.TRXID = trx.TrackKey;
                        processDataHis.UPDATETIME = DateTime.Now;
                        processDataHis.FILENAMA = string.Format("{0}_{1}_{2}_{3}", job.CassetteSequenceNo.ToString(), job.JobSequenceNo.ToString(), eq.Data.NODEID, trx.TrackKey);
                        ObjectManager.ProcessDataManager.SaveProcessDataHistory(processDataHis);
                        #endregion

                        eq.File.ProcessDataJobs.Remove(job);
                        ObjectManager.EquipmentManager.EnqueueSave(eq.File);

                        LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                            string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Process Data Report =[{2}].", trx.Metadata.NodeNo, trx.TrackKey, "ON"));

                        //SaveDVDataToDB(history);
                        string localFilePath = string.Empty;
                        bool res = DFSService.SaveDVFile(trx, out localFilePath);
                        if (res)
                            DFSTransferFile.Add(trx[0][0]["GlassID"].Value.Trim(), localFilePath);
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogErrorWrite(this.LogName, this.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", ex);

                    continue;
                }
            }

        }

        public void DFSSend()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    if (DFSTransferFile.Count > 0 && !DFSTransferFlag)
                    {
                        DFSTransferFlag = true;
                        string glassID = DFSTransferFile.FirstOrDefault().Key;
                        string localFilePath = DFSTransferFile.FirstOrDefault().Value;
                        DFSTransferFile.Remove(glassID);

                        DFSService.FtpUpload(glassID, localFilePath);

                        //新增上报dmy文件至DUMMY文件夹
                        string ftpRootAddress = DFSService.FtpFileDirectoryPath + DFSService.StepID + "/";//ftp://10.45.128.20/DATA/C30001/
                        string directoryName = Path.GetDirectoryName(localFilePath) + "\\DUMMY\\";
                        string fileName = Path.GetFileName(localFilePath);
                        string dmyFilePath = directoryName + fileName + ".dmy";
                        Ftp.FtpUpload(ftpRootAddress, DFSService.UserName, DFSService.Password, $"DUMMY/{fileName}.dmy", dmyFilePath);
                        DFSTransferFlag = false;
                    }

                }
                catch
                {
                    DFSTransferFlag = false;
                    continue;
                }
            }
        }

        public void SaveDVDataToDB(string strSaveData)
        {
            string sqlHead = "use KZONE_B20_01_HIS ";
            string sqlName = "insert [dbo].[SBCS_DVData] ([UpdateTime],[DataValues])";
            string sqlValue = $" values ('{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{strSaveData}')";
            SqlConnection mycon = GetConnection();
            mycon.Open();


            SqlCommand cmd = mycon.CreateCommand();
            cmd.CommandText = sqlHead + sqlName + sqlValue;
            cmd.ExecuteNonQuery();

            mycon.Close();
        }

        public void ProcessDataReport(Trx inputData)
        {

            try
            {
                if (inputData.IsInitTrigger)
                    return;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo(inputData.Metadata.NodeNo);
                if (eq == null)
                {
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] IN EQUIPMENTENTITY!", inputData.Metadata.NodeNo));
                }

                eBitResult bitResult = (eBitResult)int.Parse(inputData[0][2][0].Value);

                string timerID = string.Format("{0}_{1}", eq.Data.NODENO, ProcessDataReportTimeout);

                if (bitResult == eBitResult.OFF)
                {
                    //bit off移除本次timer
                    if (Timermanager.IsAliveTimer(timerID))
                    {
                        Timermanager.TerminateTimer(timerID);
                    }

                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}] BIT=[OFF]  Process Data Report .",
                         eq.Data.NODENO, inputData.TrackKey));

                    CPCProcessDataReportReply(eBitResult.OFF, inputData.TrackKey);
                    return;
                }
                CPCProcessDataReportReply(eBitResult.ON, inputData.TrackKey);

                string cstSeq = inputData[0][0]["CassetteSequenceNo"].Value;
                string slotNo = inputData[0][0]["SlotNo"].Value;
                string glassId = inputData[0][0]["GlassID"].Value;

                string startTime = inputData[0][0]["LocalProcessStartTime"].Value;
                string endTime = inputData[0][0]["LocalProcessEndTime"].Value;
                string processTime = GetProcessingTime(startTime, endTime);
                inputData[0][0]["LocalProcessingTime"].Value = processTime;
                // string processTime = inputData[0][0]["LocalProcessingTime"].Value = GetProcessingTime(startTime, endTime);

                string startTime01 = inputData[0][0]["Unit#01ProcessStartTime"].Value;
                string endTime01 = inputData[0][0]["Unit#01ProcessEndTime"].Value;
                string processTime01 = inputData[0][0]["Unit#01ProcessingTime"].Value = GetProcessingTime(startTime01, endTime01);

                string startTime02 = inputData[0][0]["Unit#02ProcessStartTime"].Value;
                string endTime02 = inputData[0][0]["Unit#02ProcessEndTime"].Value;
                string processTime02 = inputData[0][0]["Unit#02ProcessingTime"].Value = GetProcessingTime(startTime02, endTime02);

                string startTime03 = inputData[0][0]["Unit#03ProcessStartTime"].Value;
                string endTime03 = inputData[0][0]["Unit#03ProcessEndTime"].Value;
                string processTime03 = inputData[0][0]["Unit#03ProcessingTime"].Value = GetProcessingTime(startTime03, endTime03);



                #region 建立timer

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (bitResult == eBitResult.ON)
                {
                    CPCProcessDataReport(eq, inputData, eBitResult.ON);

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessDataReportTimeoutAction), inputData.TrackKey);
                }

                #endregion





                Job job = ObjectManager.JobManager.GetJob(cstSeq, slotNo);
                if (job == null)
                {
                    throw new Exception(string.Format("CAN'T FIND JOB CSTSEQ=[{0}],SLOTNO [{1}] IN JOBManager!", cstSeq, slotNo));
                }
                IList<ProcessDataReportItem> items = new List<ProcessDataReportItem>();
                string history = ParserProcessDataItem(eq, job, inputData[0][1].RawData, ref items, inputData.TrackKey);

                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                   string.Format("[EQUIPMENT={0}] [TrackKey={1}] [CstSeqNo={2}] [JobSeqNo={3}] [GlsID={4}] [LocalProcessingTime={5}] [LocalProcessStartTime={6}] [LocalProcessEndTime={7}]",
                   inputData.Metadata.NodeNo, inputData.TrackKey, cstSeq, slotNo, glassId, processTime, startTime, endTime));


                Logger.LogTrxWrite(this.LogName,
                    string.Format("[EQUIPMENT={0}] [TrackKey={1}] [CstSeqNo={2}] [JobSeqNo={3}] [GlsID={4}] [ProcessingTime={5}] [StartTime={6}] [EndTime={7}]\r\n[RAWDATA={8}]",
                    inputData.Metadata.NodeNo, inputData.TrackKey, cstSeq, slotNo, glassId, processTime, startTime, endTime, history.Replace(',', '\n')));

                ObjectManager.ProcessDataManager.MakeProcessDataValuesToFile(eq.Data.NODEID, cstSeq, slotNo, inputData.TrackKey, history);

                #region Save History
                ProcessDataHistory processDataHis = new ProcessDataHistory();
                processDataHis.CASSETTESEQNO = Convert.ToInt32(cstSeq);
                processDataHis.JOBID = glassId;
                processDataHis.JOBSEQNO = int.Parse(slotNo);
                processDataHis.PROCESSTIME = int.Parse(processTime);
                processDataHis.LOCALPROCESSSTARTTIME = startTime;
                processDataHis.LOCALPROCSSSENDTIME = endTime;
                processDataHis.NODEID = eq.Data.NODEID;
                processDataHis.TRXID = inputData.TrackKey;
                processDataHis.UPDATETIME = DateTime.Now;
                processDataHis.FILENAMA = string.Format("{0}_{1}_{2}_{3}", cstSeq, slotNo, eq.Data.NODEID, inputData.TrackKey);
                ObjectManager.ProcessDataManager.SaveProcessDataHistory(processDataHis);
                #endregion

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
        }

        private void ProcessDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EQ][{1}]  Process Data Report TIMEOUT.", sArray[0], trackKey));


            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCProcessDataReportReply(eBitResult result, string trxID)
        {
            try
            {
                Trx trx = null;

                trx = GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat("L3_EQDProcessDataReportReply") as Trx;
                string eqpNo = trx.Metadata.NodeNo;

                trx[0][0][0].Value = ((int)result).ToString();
                trx.TrackKey = trxID;
                SendToPLC(trx);

                string timerID = string.Empty;

                timerID = string.Format("{0}_{1}", eqpNo, ProcessDataReportReplyTimeout);

                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(CPCProcessDataReportReplyTimeoutAction), trxID);
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

        private void CPCProcessDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [EC -> EQ][{1}] Process Data Report Reply T2 TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));

                CPCProcessDataReportReply(eBitResult.OFF, trackKey);
            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        // TO BC 
        /// <summary>
        /// Process Data Report reply
        /// </summary>
        /// <param name="eqpNo"></param>
        /// <param name="result"></param>
        /// <param name="trxId"></param>
        ///          ProcessDataReportReply
        public void ProcessDataReportReply(Trx inputData)
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

                eBitResult result = (eBitResult)int.Parse(inputData.EventGroups[0].Events[0].Items[0].Value);

                string timerID = string.Format("{0}_{1}", eqpNo, ProcessDataReportReplyTimeout);
                if (Timermanager.IsAliveTimer(timerID))
                {
                    Timermanager.TerminateTimer(timerID);
                }
                if (result == eBitResult.ON)
                {

                    string timerID1 = string.Format("{0}_{1}", eqp.Data.NODENO, CPCProcessDataReportTimeout);

                    if (Timermanager.IsAliveTimer(timerID1))
                    {
                        Timermanager.TerminateTimer(timerID1);
                    }

                    Trx trx = GetTrxValues(string.Format("{0}_ProcessDataReport", eqp.Data.NODENO));

                    trx[0][2][0].Value = "0";
                    trx.TrackKey = UtilityMethod.GetAgentTrackKey();
                    SendToPLC(trx);
                    LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                        string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] BIT=[OFF] Process Data Report.",
                            eqp.Data.NODENO, trx.TrackKey));

                    Timermanager.CreateTimer(timerID, false, ParameterManager[eParameterName.T2].GetInteger(),
                        new System.Timers.ElapsedEventHandler(ProcessDataReportReplyTimeoutAction), inputData.TrackKey);

                    ProcessDataReportFlag = true;
                }
                LogInfo(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EQ][{1}] Process Data Report Reply SET BIT=[{2}].",
                    eqpNo, inputData.TrackKey, result.ToString()));
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }
        /// <summary>
        /// Process Data Report Reply Timeout
        /// </summary>
        /// <param name="sujbect"></param>
        /// <param name="e"></param>
        private void ProcessDataReportReplyTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');//eqpNo_processDataReport

                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC -> EQ][{1}] Process Data report Reply TIMEOUT, SET BIT=[OFF].", sArray[0], trackKey));


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        private void CPCProcessDataReportTimeoutAction(object subject, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UserTimer timer = subject as UserTimer;
                string tmp = timer.TimerId;
                string trackKey = timer.State.ToString();
                string[] sArray = tmp.Split('_');

                //T1超时EC OFF BIT
                Trx trx = GetTrxValues("L3_ProcessDataReport");
                // trx.ClearTrxWith0();
                trx[0][2][0].Value = "0";
                SendToPLC(trx);


                LogWarn(MethodBase.GetCurrentMethod().Name + "()",
                    string.Format("[EQUIPMENT={0}] [BC <- EC][{1}] Process Data Report T1 TIMEOUT.", sArray[0], trackKey));

                Thread.Sleep(1000);

                ProcessDataReportFlag = true;

            }
            catch (Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }



        /// <summary>
        /// Decode Process Data RawData return Parser List  Save Log 
        /// </summary>
        /// <param name="eqp"></param>
        /// <param name="job"></param>
        /// <param name="rawData"></param>
        /// <param name="trxId"></param>
        /// <returns></returns>
        private string ParserProcessDataItem(Equipment eq, Job job, short[] rawData, ref IList<ProcessDataReportItem> items, string trxId)
        {
            string parserList = string.Empty;
            string decodeItemName = "";
            try
            {
                IList<ProcessData> processDatas = ObjectManager.ProcessDataManager.GetProcessData(eq.Data.NODENO);
                if (processDatas == null)
                    return parserList;

                string value = string.Empty;
                int startaddress10 = 0;
                List<ProcessDataReportItem> tempList = new List<ProcessDataReportItem>();
                StringBuilder sb = new StringBuilder();
                foreach (ProcessData pd in processDatas)
                {
                    decodeItemName = pd.Data.PARAMETERNAME;

                    ItemExpressionEnum ie;
                    if (!Enum.TryParse(pd.Data.EXPRESSION.ToUpper(), out ie))
                    {
                        continue;
                    }
                    #region decode by expression
                    bool isOutRang = false;
                    switch (ie)
                    {
                        case ItemExpressionEnum.BIT:
                            value = ExpressionBIT.Decode(startaddress10, int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.ASCII:
                            value = ExpressionASCII.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            value = Regex.Replace(value, @"[^\x21-\x7E]|<|>|'", " ");//过滤不可显示的字符 20150211 tom
                            break;
                        case ItemExpressionEnum.BIN:
                            value = ExpressionBIN.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.EXP:
                            value = ExpressionEXP.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), rawData).ToString();
                            break;
                        case ItemExpressionEnum.HEX:
                            value = ExpressionHEX.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.INT:
                            value = ExpressionINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "65535")
                            if (value == "61439")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.LONG:
                            value = ExpressionLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "4294967295")
                            if (value == "4026527743")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SINT:
                            value = ExpressionSINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value == "-4097")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SLONG:
                            value = ExpressionSLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value == "-268439553")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.BCD:
                            value = ExpressionBCD.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value.Contains("9999"))
                            {
                                isOutRang = true;
                            }
                            break;
                        case ItemExpressionEnum.FLOAT:
                            value = ExpressionFLOAT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        case ItemExpressionEnum.DOUBLE:
                            value = ExpressionDOUBLE.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        default:
                            break;
                    }
                    #endregion

                    #region 算法
                    string itemValue = "0";
                    if (false == isOutRang)
                    {
                        switch (pd.Data.OPERATOR)
                        {
                            case ArithmeticOperator.PlusSign:
                                itemValue = (double.Parse(value) + double.Parse(pd.Data.DOTRATIO)).ToString(ReserveDotCountSet(double.Parse(pd.Data.DOTRATIO)));
                                break;
                            case ArithmeticOperator.MinusSign:
                                itemValue = (double.Parse(value) - double.Parse(pd.Data.DOTRATIO)).ToString(ReserveDotCountSet(double.Parse(pd.Data.DOTRATIO)));
                                break;
                            case ArithmeticOperator.TimesSign:
                                itemValue = (double.Parse(value) * double.Parse(pd.Data.DOTRATIO)).ToString(ReserveDotCountSet(double.Parse(pd.Data.DOTRATIO)));
                                break;
                            case ArithmeticOperator.DivisionSign:
                                itemValue = (double.Parse(value) / double.Parse(pd.Data.DOTRATIO)).ToString(ReserveDotCountSet(double.Parse(pd.Data.DOTRATIO)));
                                break;
                            default:
                                itemValue = value;
                                break;
                        }
                    }
                    else
                    {
                        itemValue = ParameterManager["PROCESSDATA_OUT_RAGE_REPORT"].GetString();
                    }
                    #endregion

                    #region Report Host List
                    if (pd.Data.REPORTTO != null)
                    {
                        string[] hostReportList = pd.Data.REPORTTO.Split(',');
                        if (hostReportList.Length > 0)
                        {
                            foreach (string report in hostReportList)
                            {
                                switch (report.ToUpper())
                                {
                                    case "MES":
                                        #region MES List
                                        ProcessDataReportItem item = new ProcessDataReportItem();
                                        item.Name = (pd.Data.ITEM == "" ? pd.Data.PARAMETERNAME : pd.Data.ITEM);
                                        item.Value = itemValue;
                                        tempList.Add(item);
                                        #endregion

                                        break;

                                    case "EDA":

                                        break;

                                    case "OEE":
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    #endregion

                    sb.AppendFormat("{0}={1},", pd.Data.PARAMETERNAME, itemValue);
                }
                items = tempList;
                parserList = sb.ToString();
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
            return parserList;
        }

        /// <summary>
        /// Parser Process Data item return String 
        /// </summary>
        /// <param name="eq"></param>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private string ParserProcessDataItem(Equipment eq, short[] rawData)
        {
            string parserList = string.Empty;
            string decodeItemName = "";
            try
            {
                IList<ProcessData> processDatas = ObjectManager.ProcessDataManager.GetProcessData(eq.Data.NODENO);
                if (processDatas == null)
                    return parserList;

                string value = string.Empty;
                int startaddress10 = 0;
                List<ProcessDataReportItem> tempList = new List<ProcessDataReportItem>();
                StringBuilder sb = new StringBuilder();
                foreach (ProcessData pd in processDatas)
                {
                    decodeItemName = pd.Data.PARAMETERNAME;

                    ItemExpressionEnum ie;
                    if (!Enum.TryParse(pd.Data.EXPRESSION.ToUpper(), out ie))
                    {
                        continue;
                    }
                    #region decode by expression
                    bool isOutRang = false;
                    switch (ie)
                    {
                        case ItemExpressionEnum.BIT:
                            value = ExpressionBIT.Decode(startaddress10, int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.ASCII:
                            value = ExpressionASCII.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            value = Regex.Replace(value, @"[^\x21-\x7E]|<|>|'", " ");//过滤不可显示的字符 20150211 tom
                            break;
                        case ItemExpressionEnum.BIN:
                            value = ExpressionBIN.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.EXP:
                            value = ExpressionEXP.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), rawData).ToString();
                            break;
                        case ItemExpressionEnum.HEX:
                            value = ExpressionHEX.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            if (value.Contains("EFFF"))
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.INT:
                            value = ExpressionINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "65535")
                            if (value == "61439")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.LONG:
                            value = ExpressionLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "4294967295")
                            if (value == "4026527743")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SINT:
                            value = ExpressionSINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "32767")
                            if (value == "-4097")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SLONG:
                            value = ExpressionSLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "2147483647")
                            if (value == "-268439553")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.BCD:
                            value = ExpressionBCD.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value.Contains("9999"))
                            {
                                isOutRang = true;
                            }
                            break;
                        case ItemExpressionEnum.FLOAT:
                            value = ExpressionFLOAT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        case ItemExpressionEnum.DOUBLE:
                            value = ExpressionDOUBLE.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        default:
                            break;
                    }
                    #endregion

                    #region 算法
                    string itemValue = string.Empty;
                    if (false == isOutRang)
                    {
                        switch (pd.Data.OPERATOR)
                        {
                            case ArithmeticOperator.PlusSign:
                                itemValue = (double.Parse(value) + double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.MinusSign:
                                itemValue = (double.Parse(value) - double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.TimesSign:
                                itemValue = (double.Parse(value) * double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.DivisionSign:
                                itemValue = (double.Parse(value) / double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            default:
                                itemValue = value;
                                break;
                        }
                    }
                    else
                    {
                        itemValue = ParameterManager["PROCESSDATA_OUT_RAGE_REPORT"].GetString();
                    }
                    #endregion

                    sb.AppendFormat("{0}={1},", pd.Data.PARAMETERNAME, itemValue);
                }

                parserList = sb.ToString(0, sb.Length - 1);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);

            }
            return parserList;
        }


        /// <summary>
        /// Process Data Request By Node No
        /// </summary>
        public bool ProcessDataRequestByNO(string eqpNo, out List<string> parameter, out string desc)
        {
            parameter = new List<string>();
            desc = "";

            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP(eqpNo);
                if (eq == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO =[{0}] IN EQUIPMENTENTITY!", eqpNo));

                string plc_trx_name = string.Format("{0}_ProcessDataReport", eqpNo);
                string paraList = string.Empty;

                // 機台以 PLC 上報 Process Data, OPI 查詢此機台的 Process Data 時, 直接從 PLC 讀取

                Trx trx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { plc_trx_name, false }) as Trx;
                if (trx == null)
                    throw new Exception(string.Format("CAN'T GET EQUIPMENT_NO =[{0}] DATA FROM PLC!", eqpNo));

                short[] processData = trx[0][1].RawData;

                paraList = ParserProcessDataItem(eq, processData);

                if (string.IsNullOrEmpty(paraList))
                    throw new Exception(string.Format("CAN'T DECODE EQUIPMENT_NO =[{0}] PLC DATA, OR EQUIPMENT_NO =[{0}] PROCESS DATA ITEM SETTING PROBLEM IN DB!", eqpNo));


                string[] tmpList = paraList.Split(',');
                foreach (string tmp in tmpList)
                {
                    parameter.Add(tmp);
                }
                return true;
            }
            catch (Exception ex)
            {
                desc = ex.Message.ToString();

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }

        //20161104 add mesid
        /// <summary>
        /// DailyCheckReport
        /// </summary>
        /// <param name="trxId"></param>
        /// <param name="lineId"></param>
        /// <param name="eqpID"></param>
        /// <param name="mesid"></param>
        public void DailyCheckRequest(string trxId, string lineId, string eqpID, string mesid)
        {

            string strlog = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(eqpID))
                {

                    //by Line
                    IList<Equipment> eqps = ObjectManager.EquipmentManager.GetEQPsByLine(lineId);

                    foreach (Equipment eq in eqps)
                    {
                        eReportMode reportMode = (eReportMode)Enum.Parse(typeof(eReportMode), eq.Data.REPORTMODE);
                        switch (reportMode)
                        {
                            case eReportMode.PLC:
                            case eReportMode.PLC_HSMS:
                                DailyCheckRequestForPLC(trxId, eq, mesid);
                                break;
                            case eReportMode.HSMS:
                            case eReportMode.HSMS_PLC:
                            case eReportMode.PLC_HSMS_R_D:
                            case eReportMode.PLC_HSMS_D:
                                //Todo SECS  Function 
                                Invoke(eServiceName.HSMSService, "S1F5_H_DailyCheckDataInquiry", new object[] { eq.Data.NODENO, eq.Data.NODEID, trxId, "MES", mesid });
                                break;
                        }
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    //20161104 add MES可以byLine or by EQ Daily Request Daily. for by EQP
                    Equipment rptNode = ObjectManager.EquipmentManager.GetEquipmentByID(eqpID);

                    if (rptNode != null)
                    {
                        eReportMode reportMode = (eReportMode)Enum.Parse(typeof(eReportMode), rptNode.Data.REPORTMODE);
                        switch (reportMode)
                        {
                            case eReportMode.PLC:
                            case eReportMode.PLC_HSMS:
                                DailyCheckRequestForPLC(trxId, rptNode, mesid);
                                break;
                            case eReportMode.HSMS:
                            case eReportMode.HSMS_PLC:
                            case eReportMode.PLC_HSMS_R_D:
                            case eReportMode.PLC_HSMS_D:
                                //Todo SECS  Function 
                                Invoke(eServiceName.HSMSService, "S1F5_H_DailyCheckDataInquiry", new object[] { rptNode.Data.NODENO, rptNode.Data.NODEID, trxId, "MES", mesid });
                                break;
                        }
                        Thread.Sleep(500);
                    }
                    else
                    {
                        //找不到就記錄Log
                        strlog = string.Format("[EQUIPMENT={0}] [BCS <- BCS][{1}] Can not Get Equipment Entity by EQPID=[{2}]!",
                                                eqpID, trxId, eqpID);
                        LogWarn(MethodBase.GetCurrentMethod().Name + "()", strlog);
                    }
                }


            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        //20161104 add MESID
        private void DailyCheckRequestForPLC(string trxId, Equipment eq, string mesId)
        {
            try
            {
                string decodeItemName;
                //20161104 modify Trx L3_DailyCheckDataReport
                Trx trx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { string.Format("{0}_DailyCheckDataReport", eq.Data.NODENO), false }) as Trx;
                if (trx == null)
                    throw new Exception(string.Format("CAN'T GET EQUIPMENT_NO =[{0}] DATA FROM PLC!", eq.Data.NODENO));

                short[] rawData = trx[0][1].RawData;

                IList<DailyCheckData> dailyCheckDatas = ObjectManager.DailyCheckManager.GetDailyCheckProfile(eq.Data.NODENO);
                if (dailyCheckDatas == null)
                    return;

                string value = string.Empty;
                int startaddress10 = 0;
                List<ProcessDataReportItem> items = new List<ProcessDataReportItem>();

                foreach (DailyCheckData pd in dailyCheckDatas)
                {
                    decodeItemName = pd.Data.PARAMETERNAME;

                    ItemExpressionEnum ie;
                    if (!Enum.TryParse(pd.Data.EXPRESSION.ToUpper(), out ie))
                    {
                        continue;
                    }
                    #region decode by expression
                    bool isOutRang = false;
                    switch (ie)
                    {
                        case ItemExpressionEnum.BIT:
                            value = ExpressionBIT.Decode(startaddress10, int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.ASCII:
                            value = ExpressionASCII.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            value = Regex.Replace(value, @"[^\x21-\x7E]|<|>|'", " ");//过滤不可显示的字符 20150211 tom
                            break;
                        case ItemExpressionEnum.BIN:
                            value = ExpressionBIN.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.EXP:
                            value = ExpressionEXP.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), rawData).ToString();
                            break;
                        case ItemExpressionEnum.HEX:
                            value = ExpressionHEX.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            if (value.Contains("EFFF"))
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.INT:
                            value = ExpressionINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value == "65535")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.LONG:
                            value = ExpressionLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value == "4294967295")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SINT:
                            value = ExpressionSINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value == "32767")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SLONG:
                            value = ExpressionSLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value == "2147483647")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.BCD:
                            value = ExpressionBCD.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            break;
                        case ItemExpressionEnum.FLOAT:
                            value = ExpressionFLOAT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        case ItemExpressionEnum.DOUBLE:
                            value = ExpressionDOUBLE.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        default:
                            break;
                    }
                    #endregion

                    #region 算法
                    string itemValue = string.Empty;
                    if (false == isOutRang)
                    {
                        switch (pd.Data.OPERATOR)
                        {
                            case ArithmeticOperator.PlusSign:
                                itemValue = (double.Parse(value) + double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.MinusSign:
                                itemValue = (double.Parse(value) - double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.TimesSign:
                                itemValue = (double.Parse(value) * double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.DivisionSign:
                                itemValue = (double.Parse(value) / double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            default:
                                itemValue = value;
                                break;
                        }
                    }
                    else
                    {
                        itemValue = "NA";
                    }
                    #endregion

                    #region Report Host List
                    if (pd.Data.REPORTTO != null)
                    {
                        string[] hostReportList = pd.Data.REPORTTO.Split(',');
                        if (hostReportList.Length > 0)
                        {
                            foreach (string report in hostReportList)
                            {
                                switch (report.ToUpper())
                                {
                                    case "MES":
                                        #region MES List
                                        ProcessDataReportItem item = new ProcessDataReportItem();
                                        item.Name = (pd.Data.ITEM == "" ? pd.Data.PARAMETERNAME : pd.Data.ITEM);
                                        item.Value = itemValue;
                                        items.Add(item);
                                        #endregion

                                        break;

                                    case "EDA":

                                        break;

                                    case "OEE":
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    #endregion
                }
                //20161104 add mesID for Daily Check report
                Invoke(eServiceName.MESService, "MESMeasurementDataReport", new object[] { CreateTrxID(), eq, items, mesId });
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
        }

        public bool DailyCheckRequestByNO(string eqpNo, out List<string> parameter, out string desc)
        {
            parameter = new List<string>();
            desc = "";

            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP(eqpNo);
                string paraList = string.Empty;
                if (eq == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO =[{0}] IN EQUIPMENTENTITY!", eqpNo));
                eReportMode reportMode = (eReportMode)Enum.Parse(typeof(eReportMode), eq.Data.REPORTMODE);
                switch (reportMode)
                {
                    case eReportMode.PLC:
                    case eReportMode.PLC_HSMS:
                        paraList = DailyCheckRequestForPLC(eq);

                        if (string.IsNullOrEmpty(paraList))
                            throw new Exception(string.Format("CAN'T DECODE EQUIPMENT_NO =[{0}] PLC DATA, OR EQUIPMENT_NO =[{0}] DAILY CHECK DATA ITEM SETTING PROBLEM IN DB!", eqpNo));

                        break;
                    case eReportMode.HSMS:
                    case eReportMode.HSMS_PLC:
                    case eReportMode.PLC_HSMS_R_D:
                    case eReportMode.PLC_HSMS_D:
                        string trxId = CreateTrxID();
                        //20161227 modify add MSGID=""
                        paraList = (string)Invoke(eServiceName.HSMSService, "S1F5_H_DailyCheckDataInquiry", new object[] { eq.Data.NODENO, eq.Data.NODEID, trxId, "OPI", "" });

                        if (string.IsNullOrEmpty(paraList))
                            throw new Exception(string.Format("CAN'T DECODE EQUIPMENT_NO =[{0}] SECS Data, OR EQUIPMENT_NO =[{0}] DAILY CHECK DATA ITEM HAS PROBLEM IN SECS REPLY!", eqpNo));

                        break;
                }

                //20161226 mark Log要分開寫
                //if (string.IsNullOrEmpty(paraList))
                //    throw new Exception(string.Format("CAN'T DECODE EQUIPMENT_NO =[{0}] PLC DATA, OR EQUIPMENT_NO =[{0}] DAILY CHECK DATA ITEM SETTING PROBLEM IN DB!", eqpNo));


                string[] tmpList = paraList.Split(',');
                foreach (string tmp in tmpList)
                {
                    parameter.Add(tmp);
                }
                return true;
            }
            catch (Exception ex)
            {
                desc = ex.Message.ToString();

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }

        private string DailyCheckRequestForPLC(Equipment eq)
        {

            string parserList = string.Empty;
            string decodeItemName = "";
            try
            {
                string plc_trx_name = string.Format("{0}_DailyCheckDataReport", eq.Data.NODENO);

                Trx trx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { plc_trx_name, false }) as Trx;
                if (trx == null)
                    throw new Exception(string.Format("CAN'T GET EQUIPMENT_NO =[{0}] DATA FROM PLC!", eq.Data.NODENO));

                short[] rawData = trx[0][1].RawData;
                IList<DailyCheckData> processDatas = ObjectManager.DailyCheckManager.GetDailyCheckProfile(eq.Data.NODENO);
                if (processDatas == null)
                    return parserList;

                string value = string.Empty;
                int startaddress10 = 0;
                List<ProcessDataReportItem> tempList = new List<ProcessDataReportItem>();
                StringBuilder sb = new StringBuilder();

                //20161226 add Header
                for (int i = 0; i < trx[0][0].Items.Count; i++)
                {
                    sb.AppendFormat("{0}={1},", trx[0][0][i].Name, trx[0][0][i].Value);
                }

                foreach (DailyCheckData pd in processDatas)
                {
                    decodeItemName = pd.Data.PARAMETERNAME;

                    ItemExpressionEnum ie;
                    if (!Enum.TryParse(pd.Data.EXPRESSION.ToUpper(), out ie))
                    {
                        continue;
                    }
                    #region decode by expression
                    bool isOutRang = false;
                    switch (ie)
                    {
                        case ItemExpressionEnum.BIT:
                            value = ExpressionBIT.Decode(startaddress10, int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.ASCII:
                            value = ExpressionASCII.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            value = Regex.Replace(value, @"[^\x21-\x7E]|<|>|'", " ");//过滤不可显示的字符 20150211 tom
                            break;
                        case ItemExpressionEnum.BIN:
                            value = ExpressionBIN.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.EXP:
                            value = ExpressionEXP.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), rawData).ToString();

                            break;
                        case ItemExpressionEnum.HEX:
                            value = ExpressionHEX.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData);
                            if (value.Contains("EFFF"))
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.INT:
                            value = ExpressionINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "65535")
                            if (value == "61439")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.LONG:
                            value = ExpressionLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "4294967295")
                            if (value == "4026527743")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SINT:
                            value = ExpressionSINT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "32767")
                            if (value == "-4097")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SLONG:
                            value = ExpressionSLONG.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            //if (value == "2147483647")
                            if (value == "-268439553")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.BCD:
                            value = ExpressionBCD.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString();
                            if (value.Contains("9999"))
                            {
                                isOutRang = true;
                            }
                            break;
                        case ItemExpressionEnum.FLOAT:
                            value = ExpressionFLOAT.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        case ItemExpressionEnum.DOUBLE:
                            value = ExpressionDOUBLE.Decode(startaddress10, int.Parse(pd.Data.WOFFSET), int.Parse(pd.Data.WPOINTS), int.Parse(pd.Data.BOFFSET), int.Parse(pd.Data.BPOINTS), rawData).ToString("R");
                            break;
                        default:
                            break;
                    }
                    #endregion

                    #region 算法
                    string itemValue = string.Empty;
                    if (false == isOutRang)
                    {
                        switch (pd.Data.OPERATOR)
                        {
                            case ArithmeticOperator.PlusSign:
                                itemValue = (double.Parse(value) + double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.MinusSign:
                                itemValue = (double.Parse(value) - double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.TimesSign:
                                itemValue = (double.Parse(value) * double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.DivisionSign:
                                itemValue = (double.Parse(value) / double.Parse(pd.Data.DOTRATIO)).ToString();
                                break;
                            default:
                                itemValue = value;
                                break;
                        }
                    }
                    else
                    {
                        itemValue = ParameterManager["PROCESSDATA_OUT_RAGE_REPORT"].GetString();
                    }
                    #endregion

                    sb.AppendFormat("{0}={1},", pd.Data.PARAMETERNAME, itemValue);
                }

                parserList = sb.ToString(0, sb.Length - 1);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return parserList;


        }


        /// <summary>
        /// FDC Request By Node No
        /// </summary>
        public bool FDCDataRequestByNO(string eqpNo, out List<string> parameter, out string desc)
        {
            parameter = new List<string>();
            desc = "";

            try
            {
                Equipment eq = ObjectManager.EquipmentManager.GetEQP(eqpNo);
                if (eq == null)
                    throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO =[{0}] IN EQUIPMENTENTITY!", eqpNo));
                string paraList = string.Empty;

                eReportMode reportMode = (eReportMode)Enum.Parse(typeof(eReportMode), eq.Data.REPORTMODE);
                switch (reportMode)
                {
                    case eReportMode.PLC:
                    case eReportMode.PLC_HSMS:
                        paraList = FDCCheckRequestForPLC(eq);
                        break;
                    case eReportMode.HSMS:
                    case eReportMode.HSMS_PLC:
                    case eReportMode.PLC_HSMS_R_D:
                    case eReportMode.PLC_HSMS_D:
                        string trxId = CreateTrxID();
                        paraList = (string)Invoke(eServiceName.HSMSService, "S1F5_H_FDCInquiry", new object[] { eq.Data.NODENO, eq.Data.NODEID, trxId, "OPI" });
                        break;
                }

                if (string.IsNullOrEmpty(paraList))
                    throw new Exception(string.Format("CAN'T DECODE EQUIPMENT_NO =[{0}] PLC DATA, OR EQUIPMENT_NO =[{0}] FDC DATA ITEM SETTING PROBLEM IN DB!", eqpNo));


                string[] tmpList = paraList.Split(',');
                foreach (string tmp in tmpList)
                {
                    parameter.Add(tmp);
                }
                return true;
            }
            catch (Exception ex)
            {
                desc = ex.Message.ToString();

                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
                return false;
            }
        }


        private string FDCCheckRequestForPLC(Equipment eq)
        {

            string parserList = string.Empty;
            string decodeItemName = "";
            try
            {
                string plc_trx_name = string.Format("{0}_FaultDetectDataReport", eq.Data.NODENO);

                Trx trx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { plc_trx_name, false }) as Trx;
                if (trx == null)
                    throw new Exception(string.Format("CAN'T GET EQUIPMENT_NO =[{0}] DATA FROM PLC!", eq.Data.NODENO));

                short[] rawData = trx[0][1].RawData;
                IList<FDCDataEntityData> processDatas = ObjectManager.DailyCheckManager.GetFDCDataProfile(eq.Data.NODENO);
                if (processDatas == null)
                    return parserList;

                string value = string.Empty;
                int startaddress10 = 0;
                StringBuilder sb = new StringBuilder();

                //20161226 add Header
                for (int i = 0; i < trx[0][0].Items.Count; i++)
                {
                    sb.AppendFormat("{0}={1},", trx[0][0][i].Name, trx[0][0][i].Value);
                }

                foreach (FDCDataEntityData pd in processDatas)
                {
                    decodeItemName = pd.PARAMETERNAME;

                    ItemExpressionEnum ie;
                    if (!Enum.TryParse(pd.EXPRESSION.ToUpper(), out ie))
                    {
                        continue;
                    }
                    #region decode by expression
                    bool isOutRang = false;
                    switch (ie)
                    {
                        case ItemExpressionEnum.BIT:
                            value = ExpressionBIT.Decode(startaddress10, int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.ASCII:
                            value = ExpressionASCII.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData);
                            value = System.Text.RegularExpressions.Regex.Replace(value, @"[^\x21-\x7E]|<|>|'", " ");//过滤不可显示的字符 20150211 tom
                            break;
                        case ItemExpressionEnum.BIN:
                            value = ExpressionBIN.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData);
                            break;
                        case ItemExpressionEnum.EXP:
                            value = ExpressionEXP.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), rawData).ToString();
                            break;
                        case ItemExpressionEnum.HEX:
                            value = ExpressionHEX.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData);
                            if (value.Contains("EFFF"))
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.INT:
                            value = ExpressionINT.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData).ToString();
                            //if (value == "65535")
                            if (value == "61439")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.LONG:
                            value = ExpressionLONG.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData).ToString();
                            //if (value == "4294967295")
                            if (value == "4026527743")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SINT:
                            value = ExpressionSINT.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData).ToString();
                            //if (value == "32767")
                            if (value == "-4097")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.SLONG:
                            value = ExpressionSLONG.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData).ToString();
                            //if (value == "2147483647")
                            if (value == "-268439553")
                                isOutRang = true;
                            break;
                        case ItemExpressionEnum.BCD:
                            value = ExpressionBCD.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData).ToString();
                            if (value.Contains("9999"))
                            {
                                isOutRang = true;
                            }
                            break;
                        case ItemExpressionEnum.FLOAT:
                            value = ExpressionFLOAT.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData).ToString("R");
                            break;
                        case ItemExpressionEnum.DOUBLE:
                            value = ExpressionDOUBLE.Decode(startaddress10, int.Parse(pd.WOFFSET), int.Parse(pd.WPOINTS), int.Parse(pd.BOFFSET), int.Parse(pd.BPOINTS), rawData).ToString("R");
                            break;
                        default:
                            break;
                    }
                    #endregion

                    #region 算法
                    string itemValue = string.Empty;
                    if (false == isOutRang)
                    {
                        switch (pd.OPERATOR)
                        {
                            case ArithmeticOperator.PlusSign:
                                itemValue = (double.Parse(value) + double.Parse(pd.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.MinusSign:
                                itemValue = (double.Parse(value) - double.Parse(pd.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.TimesSign:
                                itemValue = (double.Parse(value) * double.Parse(pd.DOTRATIO)).ToString();
                                break;
                            case ArithmeticOperator.DivisionSign:
                                itemValue = (double.Parse(value) / double.Parse(pd.DOTRATIO)).ToString();
                                break;
                            default:
                                itemValue = value;
                                break;
                        }
                    }
                    else
                    {
                        itemValue = ParameterManager["PROCESSDATA_OUT_RAGE_REPORT"].GetString();
                    }
                    #endregion

                    sb.AppendFormat("{0}={1},", pd.PARAMETERNAME, itemValue);
                }

                parserList = sb.ToString(0, sb.Length - 1);
            }
            catch (System.Exception ex)
            {
                LogError(MethodBase.GetCurrentMethod().Name + "()", ex);
            }
            return parserList;


        }

        private string ReserveDotCountSet(double rate)
        {
            int tmpRate = (int)rate;

            if (tmpRate < 1)
            {
                return "";
            }
            if (tmpRate == 1)
            {
                return "";
            }
            else if (tmpRate > 1 && tmpRate <= 10)
            {
                return "F1";
            }
            else if (tmpRate > 10 && tmpRate <= 100)
            {
                return "F2";
            }
            else if (tmpRate > 100 && tmpRate <= 1000)
            {
                return "F3";
            }
            else if (tmpRate > 1000 && tmpRate <= 10000)
            {
                return "F4";
            }
            else if (tmpRate > 10000 && tmpRate <= 100000)
            {
                return "F5";
            }
            else
            {
                return "F6";
            }
        }
    }

}