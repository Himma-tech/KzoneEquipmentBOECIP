using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using KZONE.Log;
using KZONE.Work;
using KZONE.PLCAgent;
using KZONE.PLCAgent.PLC;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.MessageManager;
using KZONE.Service;
using EipTagLibrary;

namespace KZONE.UI
{
    public partial class FormInterface : FormBase
    {
        Dictionary<int, string> DicUp_Desc;
        Dictionary<int, string> DicDown_Desc;

        string UpNode = "";
        string UpUnit = "";
        string DownNode = "";
        string DownUnit = "";
        string UpSeqNo = "";
        string DnSeqNo = "";
        string PipeName = string.Empty;

        string upTrxName = string.Empty;
        string downTrxName = string.Empty;
        string upJobTrxName = string.Empty;
        string downJobTrxName = string.Empty;
        string eqPIOTrxName = string.Empty;

        private Equipment eq;

        string LinkType = string.Empty;
        string TimerCharType = string.Empty;
        OPIInfo opiInfo = OPIInfo.CreateInstance();
        Interface _if = new Interface();
        AbstractMethod method = AbstractMethod.CreateInstance();


        public FormInterface()
        {
            InitializeComponent();
        }

        public FormInterface(string strPipeName)
        {
            InitializeComponent();

            try
            {

                PipeName = strPipeName;

                //I0200030001
                UpNode = "L" + int.Parse(strPipeName.Substring(1, 2));
                UpUnit = strPipeName.Substring(3, 2);

                DownNode = "L" + int.Parse(strPipeName.Substring(5, 2));
                DownUnit = strPipeName.Substring(7, 2);

                UpSeqNo = strPipeName.Substring(9, 2);
                DnSeqNo = strPipeName.Substring(11, 2);

                lblCaption.Text = UpNode + "#" + UpSeqNo + "->" + DownNode + "#" + DnSeqNo;

                //if (!opiInfo.Dic_Node.ContainsKey(UpNode) || !opiInfo.Dic_Node.ContainsKey(DownNode)) return;

                //Node ndUp = opiInfo.Dic_Node[UpNode];

                //Node ndDown = opiInfo.Dic_Node[DownNode];

                //lblCaption.Text = ndUp.NodeName + "->" + ndDown.NodeName;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void FormInterface_Load(object sender, EventArgs e)
        {
            try
            {
                eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                tmrRefreshPLC.Interval = 10;
                tmrRefreshPLC.Enabled = true;


                #region Load link signal setting
                if (opiInfo.Dic_Pipe.ContainsKey(PipeName))
                {
                    Interface _if = opiInfo.Dic_Pipe[PipeName];

                    var _var = opiInfo.Lst_LinkSignal_Type.Find(r => r.UpStreamLocalNo.Equals(_if.UpstreamNodeNo) && r.DownStreamLocalNo.Equals(_if.DownstreamNodeNo) &&
                        r.SeqNo.Equals(_if.UpstreamSeqNo + _if.DownstreamSeqNo));

                    if (_var != null)
                    {
                        LinkType = _var.LinkType;
                        TimerCharType = _var.TimingChart;
                    }
                }

                if (LinkType != string.Empty)
                {
                    if (opiInfo.Dic_LinkSignal_Desc.ContainsKey(LinkType))
                    {
                        DicUp_Desc = opiInfo.Dic_LinkSignal_Desc[LinkType].UpStreamBit;
                        DicDown_Desc = opiInfo.Dic_LinkSignal_Desc[LinkType].DownStreamBit;
                    }
                    else
                    {
                        LinkType = string.Empty;
                    }
                }

                if (LinkType == string.Empty)
                {
                    DicUp_Desc = addUp();
                    DicDown_Desc = addDown();
                }
                #endregion

                foreach (int _seq in DicUp_Desc.Keys)
                {
                    Label lblitem = new Label();
                    lblitem.Name = "lblUpBit" + _seq.ToString("00");
                    lblitem.Tag = _seq.ToString();
                    lblitem.Image = Properties.Resources.Bit_Sliver;
                    lblitem.ImageAlign = ContentAlignment.MiddleLeft;
                    lblitem.Text = "            " + "B000  " + DicUp_Desc[_seq];
                    lblitem.Width = 230;
                    //lblitem.AutoSize = true;//家成偷改
                    lblitem.TextAlign = ContentAlignment.MiddleLeft;
                    lblitem.Font = new Font("Cambria", 10);

                    if (UpNode == "L3")
                    {
                        lblitem.Click += new EventHandler(Lable_Click);
                    }

                    flpUp.Controls.Add(lblitem);
                }

                foreach (int _seq in DicDown_Desc.Keys)
                {
                    Label lblitem = new Label();
                    lblitem.Name = "lblDownBit" + _seq.ToString("00");
                    lblitem.Tag = _seq.ToString();
                    lblitem.Image = Properties.Resources.Bit_Sliver;
                    lblitem.ImageAlign = ContentAlignment.MiddleLeft;
                    lblitem.Text = "            " + "B000  " + DicDown_Desc[_seq];
                    lblitem.Width = 230;
                    //lblitem.AutoSize = true;//家成偷改
                    lblitem.TextAlign = ContentAlignment.MiddleLeft;
                    lblitem.Font = new Font("Cambria", 10);

                    if (DownNode == "L3")
                    {
                        lblitem.Click += new EventHandler(Lable_Click);
                    }

                    flpDown.Controls.Add(lblitem);
                }

                //for (int i = 0; i < 32; i++)
                //{
                //    Label lblitem = new Label();
                //    lblitem.Name = "lblPIOBit" + i.ToString("00");
                //    lblitem.Tag = i.ToString();
                //    lblitem.Image = Properties.Resources.Bit_Sliver;
                //    lblitem.ImageAlign = ContentAlignment.MiddleLeft;
                //    //lblitem.Text = "            " + "B000  " + DicDown_Desc[_seq];
                //    lblitem.Width = 230;

                //    lblitem.TextAlign = ContentAlignment.MiddleLeft;
                //    lblitem.Font = new Font("Cambria", 10);

                //    flowLayoutPanel3.Controls.Add(lblitem);
                //}




                dgvJob.Rows.Clear();

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        string CountAddress(string Address, string BitOffset)
        {
            try
            {
                if (Address == string.Empty) return string.Empty;

                string strA = Convert.ToInt32(Address, 16).ToString();
                string strB = BitOffset.ToString();
                string sum = (int.Parse(strA) + int.Parse(strB)).ToString();
                string strAddr = Convert.ToString(int.Parse(sum), 16);
                return strAddr.PadLeft(4, '0').ToUpper();
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);

                return "";
            }
        }

        //讓WinForm Pop時不閃爍。 
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED (等同 cp.ExStyle = cp.ExStyle | 0x02000000)
                return cp;
            }
        }

        private void btnTimingChart_Click(object sender, EventArgs e)
        {
            FormTimingChart _frm = new FormTimingChart(TimerCharType);
            _frm.TopMost = true;

            _frm.ShowDialog();

            _frm.Dispose();
        }

        private void tmrRefreshPLC_Tick(object sender, EventArgs e)
        {
            try
            {
                eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");

                if (opiInfo.Dic_Pipe.ContainsKey(PipeName) == false) return;

                _if = opiInfo.Dic_Pipe[PipeName];

                DebugMode.BackColor = eq.File.DeBugMode == eEnableDisable.Enable ? Color.Green : Color.Red;

                if (UpNode != "L3")
                {
                    label1.BackColor = eq.File.UpInlineMode == eBitResult.ON ? Color.Green : Color.Red;
                    label2.BackColor = eq.File.UPActionMonitor[3] == "1" ? Color.Green : Color.Red;
                    label3.BackColor = eq.File.Status == eEQPStatus.Run || eq.File.Status == eEQPStatus.Idle ? Color.Green : Color.Red;
                    label4.Text = eq.File.ReceiveStep.ToString();
                    label4.Font = new Font("宋体" +
                        "", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    label4.TextAlign = ContentAlignment.MiddleCenter;
                    label4.BackColor = eq.File.ReceiveStep != eReceiveStep.End ? Color.Red : Color.Green;
                    button1.Text = "Force Complete Request To Upstream";
                    button1.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    button1.TextAlign = ContentAlignment.MiddleCenter;
                    button2.Text = "Force Initial Request To Upstream";
                    button2.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    button2.TextAlign = ContentAlignment.MiddleCenter;

                    label5.Text = "Receive Complete";
                    label5.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    label5.TextAlign = ContentAlignment.MiddleCenter;
                    label5.BackColor = eq.File.UPReciveComplete == eBitResult.OFF ? Color.Red : Color.Green;

                    textBox1.Text = eq.File.UPDataCheckResult;
                    if (!string.IsNullOrEmpty(eq.File.UPDataCheckResult))
                    {
                        textBox1.ForeColor = Color.Red;

                    }

                }
                else
                {
                    if (UpSeqNo == "01")
                    {
                        label1.BackColor = eq.File.DownInlineMode == eBitResult.ON ? Color.Green : Color.Red;
                        label2.BackColor = eq.File.DownActionMonitor[4] == "1" ? Color.Green : Color.Red;
                        label3.BackColor = eq.File.Status == eEQPStatus.Run || eq.File.Status == eEQPStatus.Idle ? Color.Green : Color.Red;
                        label4.Text = eq.File.SendStep.ToString();
                        label4.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        label4.TextAlign = ContentAlignment.MiddleCenter;
                        label4.BackColor = eq.File.SendStep != eSendStep.End ? Color.Red : Color.Green;
                        button1.Text = "Force Complete Request To Downstream";
                        button1.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        button1.TextAlign = ContentAlignment.MiddleCenter;
                        button2.Text = "Force Initial Request To Downstream";
                        button2.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        button2.TextAlign = ContentAlignment.MiddleCenter;

                        label5.Text = "Send Complete";
                        label5.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        label5.TextAlign = ContentAlignment.MiddleCenter;
                        label5.BackColor = eq.File.DownSendComplete == eBitResult.OFF ? Color.Red : Color.Green;

                    }
                    else
                    {


                        label1.BackColor = eq.File.DownInlineMode == eBitResult.ON ? Color.Green : Color.Red;
                        label2.BackColor = eq.File.DownActionMonitor[1] == "1" ? Color.Green : Color.Red;
                        label3.BackColor = eq.File.Status == eEQPStatus.Run || eq.File.Status == eEQPStatus.Idle ? Color.Green : Color.Red;
                        label4.Text = eq.File.SendStep.ToString();
                        label4.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        label4.TextAlign = ContentAlignment.MiddleCenter;
                        label4.BackColor = eq.File.SendStep != eSendStep.End ? Color.Red : Color.Green;
                        button1.Text = "Force Complete Request To Downstream";
                        button1.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        button1.TextAlign = ContentAlignment.MiddleCenter;
                        button2.Text = "Force Initial Request To Downstream";
                        button2.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        button2.TextAlign = ContentAlignment.MiddleCenter;

                        label5.Text = "Send Complete";
                        label5.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                        label5.TextAlign = ContentAlignment.MiddleCenter;
                        label5.BackColor = eq.File.DownSendComplete == eBitResult.OFF ? Color.Red : Color.Green;
                    }

                }

                #region 上下游的PIO信号要显示出来
                //string pIO = "00000000000000000000000000000000";

                //eqPIOTrxName = string.Format("{0}_EQDDownstreamPathPIO#{1}", "L3", "01");

                //Trx pioTrx = method.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { eqPIOTrxName }) as Trx;
                //if (pioTrx != null) pioTrx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { eqPIOTrxName, false }) as Trx;
                //if (pioTrx == null)
                //{
                //    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", eqPIOTrxName));

                //}
                //else
                //{
                //    for (int i = 0; i < pioTrx.EventGroups[0].Events[0].Items.Count; i++)
                //    {
                //        int iOffset = pioTrx.EventGroups[0].Events[0].Items[i].Metadata.BitOffset;
                //        string strValue = pioTrx.EventGroups[0].Events[0].Items[i].Value.ToString();
                //        pIO = pIO.Remove(iOffset, 1);
                //        pIO = pIO.Insert(iOffset, strValue);


                //    }
                //    _if.EQPIO = pIO;//.PadRight(32,'0');
                //    _if.PIOBitAddress = pioTrx.EventGroups[0].Events[0].Metadata.Address;

                //    //foreach (Label _lbl in flowLayoutPanel3.Controls.OfType<Label>())
                //    //{
                //    //    if (_lbl.Tag == null) continue;
                //    //    int _seqNo = 0;
                //    //    int.TryParse(_lbl.Tag.ToString(), out _seqNo);

                //    //    if (_if.EQPIO.Substring(_seqNo, 1) == "0") _lbl.Image = Properties.Resources.Bit_Sliver;
                //    //    else _lbl.Image = Properties.Resources.Bit_Green;

                //    //    _lbl.Text = "          " + (int.Parse(_if.PIOBitAddress)+_seqNo).ToString() + "  " + pioTrx.EventGroups[0].Events[0].Items[_seqNo].Name;
                //    //}


                //  }


                #endregion

                #region  扫描上下游Interface Bit信号
                string upLinkSignal = "00000000000000000000000000000000";
                string downLinkSignal = "00000000000000000000000000000000";

                //由PLCAgent取回link signal資料
                Block upTrx = null;
                if (!opiInfo.Dic_Node.ContainsKey(_if.UpstreamNodeNo))
                {
                    upTrx = EquipmentService.eipTagAccess.ReadBlockValues("RV_EQToEQ_LinkSignal_02_03_00", "UpstreamLinkSignal");
                }
                else
                {
                    upTrx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");
                }
                if (upTrx == null)
                {
                    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", "UpstreamLinkSignal"));

                }
                else
                {
                    for (int i = 0; i < 31; i++)
                    {
                        int iOffset = i;
                        string strValue = (bool)upTrx.Items[i].Value==true ? "1" : "0";
                        upLinkSignal = upLinkSignal.Remove(i, 1);
                        upLinkSignal = upLinkSignal.Insert(i, strValue);
                    }
                    _if.UpstreamSignal = upLinkSignal;//.PadRight(32,'0');
                    _if.UpstreamBitAddress = "00000000";

                }

                Block downTrx = null;

                if (!opiInfo.Dic_Node.ContainsKey(_if.DownstreamNodeNo))
                {
                 
                     downTrx = EquipmentService.eipTagAccess.ReadBlockValues("RV_EQToEQ_LinkSignal_02_03_00", "DownstreamLinkSignal");
                }
                else
                {
                    downTrx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "DownstreamLinkSignal");
                }

               
                if (downTrx == null)
                {
                    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", "RV_EQToEQ_LinkSignal_03_02_00"));

                }
                else
                {
                    for (int i = 0; i < 31; i++)
                    {
                        int iOffset =i;
                        string strValue = (bool)downTrx.Items[i].Value == true ? "1" : "0";
                        downLinkSignal = downLinkSignal.Remove(iOffset, 1);
                        downLinkSignal = downLinkSignal.Insert(iOffset, strValue);
                    }
                    _if.DownstreamSignal = downLinkSignal;//.PadRight(32,'0');
                    _if.DownstreamBitAddress = "00000000";
                }
                opiInfo.Dic_Pipe[PipeName] = _if;


                #endregion


                #region 扫描上下游的Word数据


                //由PLCAgent取回JobData資料 L2_SendingGlassDataReport#01
                if (!opiInfo.Dic_Node.ContainsKey(_if.UpstreamNodeNo))
                {
                
                    Block upJobTrx = EquipmentService.eipTagAccess.ReadBlockValues("RV_EQToEQ_LinkSignal_02_03_00", "JobData#1");

                    if (upJobTrx == null)
                    {
                        NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", upJobTrxName));
                    }
                    else
                    {

                        for (int i = 0; i <= 0; i++)
                        {
                            GlassData upjobData = new GlassData();
                            upjobData.GlassAddress = "00000000";
                            upjobData.CassetteSequenceNumber = upJobTrx["LotSequenceNumber"].Value.ToString();
                            upjobData.SlotSequenceNumber = upJobTrx["SlotSequenceNumber"].Value.ToString();
                            upjobData.JobID = upJobTrx["JobID"].Value.ToString();
                            upjobData.PPID = upJobTrx["PPID#3"].Value.ToString();

                            if (_if.UpstreamJobData.Keys.Contains(upJobTrxName + i.ToString()))
                            {
                                _if.UpstreamJobData[upJobTrxName + i.ToString()] = upjobData;
                            }
                            else
                            {
                                _if.UpstreamJobData.Add(upJobTrxName + i.ToString(), upjobData);
                            }
                        }


                    }
                }
                else
                {

                    for (int i = 0; i <= 0; i++)
                    {
                        Block upJobTrx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "JobData#1");

                    
                        if (upJobTrx == null)
                        {
                            NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", upJobTrxName));
                        }
                        else
                        {
                            GlassData upjobData = new GlassData();
                            upjobData.GlassAddress = "00000000";
                            upjobData.CassetteSequenceNumber = upJobTrx["LotSequenceNumber"].Value.ToString();
                            upjobData.SlotSequenceNumber = upJobTrx["SlotSequenceNumber"].Value.ToString();
                            upjobData.JobID = upJobTrx["JobID"].Value.ToString();
                            upjobData.PPID = upJobTrx["PPID#3"].Value.ToString();

                            if (_if.UpstreamJobData.Keys.Contains(upJobTrxName))
                            {
                                _if.UpstreamJobData[upJobTrxName] = upjobData;
                            }
                            else
                            {
                                _if.UpstreamJobData.Add(upJobTrxName, upjobData);
                            }
                        }

                    }

                }




                if (!opiInfo.Dic_Node.ContainsKey(_if.DownstreamNodeNo))
                {
                    Block downJobTrx = EquipmentService.eipTagAccess.ReadBlockValues("RV_EQToEQ_LinkSignal_02_03_00", "JobData#2");

                    if (downJobTrx == null)
                    {
                        NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", downJobTrxName));

                    }
                    else
                    {
                        for (int i = 0; i <= 0; i++)
                        {
                            GlassData downJobData = new GlassData();

                            downJobData.GlassAddress = "00000000";
                            downJobData.CassetteSequenceNumber = downJobTrx["LotSequenceNumber"].Value.ToString();
                            downJobData.SlotSequenceNumber = downJobTrx["SlotSequenceNumber"].Value.ToString();
                            downJobData.JobID = downJobTrx["JobID"].Value.ToString();
                            downJobData.PPID = downJobTrx["PPID#3"].Value.ToString();

                            if (_if.DownstreamJobData.Keys.Contains(downJobTrxName + i.ToString()))
                            {
                                _if.DownstreamJobData[downJobTrxName + i.ToString()] = downJobData;
                            }
                            else
                            {
                                _if.DownstreamJobData.Add(downJobTrxName + i.ToString(), downJobData);
                            }
                        }
                    }

                }
                else
                {


                    for (int i = 0; i <= 0; i++)
                    {
                        Block downJobTrx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "JobData#2");

                        if (downJobTrx == null)
                        {
                            NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", downJobTrxName));

                        }
                        else
                        {
                            GlassData downJobData = new GlassData();

                            downJobData.GlassAddress = "00000000";
                            downJobData.CassetteSequenceNumber = downJobTrx["LotSequenceNumber"].Value.ToString();
                            downJobData.SlotSequenceNumber = downJobTrx["SlotSequenceNumber"].Value.ToString();
                            downJobData.JobID = downJobTrx["JobID"].Value.ToString();
                            downJobData.PPID = downJobTrx["PPID#3"].Value.ToString();

                            if (_if.DownstreamJobData.Keys.Contains(downJobTrxName))
                            {
                                _if.DownstreamJobData[downJobTrxName] = downJobData;
                            }
                            else
                            {
                                _if.DownstreamJobData.Add(downJobTrxName, downJobData);
                            }
                        }
                    }

                }




                #endregion

                #region Bit
                int _seq = 0;
                foreach (Label _lbl in flpUp.Controls.OfType<Label>())
                {
                    if (_lbl.Tag == null) continue;

                    if ((eq.Data.NODENAME == "TEREWORK" || eq.Data.NODENAME == "TSREWORK") && (_lbl.Tag.ToString() == "31" || _lbl.Tag.ToString() == "32"))
                    {

                        int.TryParse(_lbl.Tag.ToString(), out _seq);

                        if (_if.UpstreamSignal.Substring(_seq - 1, 1) == "0") _lbl.Image = Properties.Resources.Bit_Sliver;
                        else _lbl.Image = Properties.Resources.Bit_Green;

                        _lbl.Text = "          " + CountAddress(_if.UpstreamBitAddress, (_seq - 1 + 16).ToString()) + "  " + DicUp_Desc[_seq];


                    }
                    else
                    {

                        int.TryParse(_lbl.Tag.ToString(), out _seq);

                        if (_if.UpstreamSignal.Substring(_seq - 1, 1) == "0") _lbl.Image = Properties.Resources.Bit_Sliver;
                        else _lbl.Image = Properties.Resources.Bit_Green;

                        _lbl.Text = "          " + CountAddress(_if.UpstreamBitAddress, (_seq - 1).ToString()) + "  " + DicUp_Desc[_seq];
                    }

                }

                foreach (Label _lbl in flpDown.Controls.OfType<Label>())
                {
                    if (_lbl.Tag == null) continue;

                    int.TryParse(_lbl.Tag.ToString(), out _seq);

                    if (_if.DownstreamSignal.Substring(_seq - 1, 1) == "0") _lbl.Image = Properties.Resources.Bit_Sliver;
                    else _lbl.Image = Properties.Resources.Bit_Green;

                    _lbl.Text = "          " + CountAddress(_if.DownstreamBitAddress, (_seq - 1).ToString()) + "  " + DicDown_Desc[_seq];
                }
                #endregion

                #region JobData

                if (dgvJob.Rows.Count == 0)
                {
                    #region Add Job Data
                    foreach (GlassData _job in _if.UpstreamJobData.Values)
                    {
                        dgvJob.Rows.Add("Detail", "UP",
                            _job.GlassAddress,
                            _job.CassetteSequenceNumber,
                            _job.SlotSequenceNumber,
                            _job.JobID,

                            _job.GroupNumber,
                            _job.GlassType,
                            _job.GlassJudge,
                            _job.ProcessSkipFlag,
                            _job.LastGlassFlag,
                            _job.CIMModeCreate,
                            _job.SamplingFlag,
                            _job.Reserved,
                            _job.InspectionJudgeResult,
                            _job.InspectionReservationSignal,
                            _job.ProcessReservationSignal,
                            _job.TrackingDataHistory,
                            _job.EquipmentSpecialFlag,
                            _job.GlassID,
                            _job.SorterGrade,
                            _job.GlassGrade,
                            _job.FromPortNo,
                            _job.TargetPortNo,
                            _job.TargetSlotNo,
                            _job.TargetCassetteID,
                            _job.Reserve,
                            _job.PPID
                            );
                    }

                    foreach (GlassData _job in _if.DownstreamJobData.Values)
                    {
                        dgvJob.Rows.Add("Detail", "DOWN",
                            _job.GlassAddress,
                            _job.CassetteSequenceNumber,
                            _job.SlotSequenceNumber,
                            _job.JobID,

                            _job.GroupNumber,
                            _job.GlassType,
                            _job.GlassJudge,
                            _job.ProcessSkipFlag,
                            _job.LastGlassFlag,
                            _job.CIMModeCreate,
                            _job.SamplingFlag,
                            _job.Reserved,
                            _job.InspectionJudgeResult,
                            _job.InspectionReservationSignal,
                            _job.ProcessReservationSignal,
                            _job.TrackingDataHistory,
                            _job.EquipmentSpecialFlag,
                            _job.GlassID,
                            _job.SorterGrade,
                            _job.GlassGrade,
                            _job.FromPortNo,
                            _job.TargetPortNo,
                            _job.TargetSlotNo,
                            _job.TargetCassetteID,
                            _job.Reserve,
                            _job.PPID
                            );
                    }
                    #endregion
                }
                else
                {
                    bool _find = false;

                    #region find UpstreamJobData

                    int _indexer = 0;

                    foreach (GlassData _job in _if.UpstreamJobData.Values)
                    {
                        _find = false;

                        if (dgvJob.Rows[_indexer].Cells[colAddress.Name].Value.ToString() == _job.GlassAddress)
                        {
                            dgvJob.Rows[_indexer].Cells[colCassetteSeqNo.Name].Value = _job.CassetteSequenceNumber;
                            dgvJob.Rows[_indexer].Cells[colJobSeqNo.Name].Value = _job.SlotSequenceNumber;
                            dgvJob.Rows[_indexer].Cells[colJobID.Name].Value = _job.JobID;

                            dgvJob.Rows[_indexer].Cells[colGroupNumber.Name].Value = _job.GroupNumber;
                            dgvJob.Rows[_indexer].Cells[colGlassType.Name].Value = _job.GlassType;
                            dgvJob.Rows[_indexer].Cells[colGlassJudge.Name].Value = _job.GlassJudge;
                            dgvJob.Rows[_indexer].Cells[colProcessSkipFlag.Name].Value = _job.ProcessSkipFlag;
                            dgvJob.Rows[_indexer].Cells[colLastGlassFlag.Name].Value = _job.LastGlassFlag;
                            dgvJob.Rows[_indexer].Cells[colCIMModeCreate.Name].Value = _job.CIMModeCreate;
                            dgvJob.Rows[_indexer].Cells[colSamplingFlag.Name].Value = _job.SamplingFlag;
                            dgvJob.Rows[_indexer].Cells[colReserved.Name].Value = _job.Reserved;
                            dgvJob.Rows[_indexer].Cells[colInspectionJudgeResult.Name].Value = _job.InspectionJudgeResult;
                            dgvJob.Rows[_indexer].Cells[colInspectionReservationSignal.Name].Value = _job.InspectionReservationSignal;
                            dgvJob.Rows[_indexer].Cells[colProcessReservationSignal.Name].Value = _job.ProcessReservationSignal;
                            dgvJob.Rows[_indexer].Cells[colTrackingDataHistory.Name].Value = _job.TrackingDataHistory;
                            dgvJob.Rows[_indexer].Cells[colEquipmentSpecialFlag.Name].Value = _job.EquipmentSpecialFlag;
                            dgvJob.Rows[_indexer].Cells[colGlassID.Name].Value = _job.GlassID;
                            dgvJob.Rows[_indexer].Cells[colSorterGrade.Name].Value = _job.SorterGrade;
                            dgvJob.Rows[_indexer].Cells[colGlassGrade.Name].Value = _job.GlassGrade;
                            dgvJob.Rows[_indexer].Cells[colFromPortNo.Name].Value = _job.FromPortNo;
                            dgvJob.Rows[_indexer].Cells[colTargetPortNo.Name].Value = _job.TargetPortNo;
                            dgvJob.Rows[_indexer].Cells[colTargetSlotNo.Name].Value = _job.TargetSlotNo;
                            dgvJob.Rows[_indexer].Cells[colTargetCassetteID.Name].Value = _job.TargetCassetteID;
                            dgvJob.Rows[_indexer].Cells[colReserve.Name].Value = _job.Reserve;
                            dgvJob.Rows[_indexer].Cells[colPPID.Name].Value = _job.PPID;


                            _find = true;
                            if (_indexer < _if.UpstreamJobData.Count)
                                _indexer++;
                        }

                        if (_find == false)
                        {
                            dgvJob.Rows.Add("Detail", "UP",
                                _job.GlassAddress,
                                _job.CassetteSequenceNumber,
                                _job.SlotSequenceNumber,
                                _job.JobID,

                            _job.GroupNumber,
                            _job.GlassType,
                            _job.GlassJudge,
                            _job.ProcessSkipFlag,
                            _job.LastGlassFlag,
                            _job.CIMModeCreate,
                            _job.SamplingFlag,
                            _job.Reserved,
                            _job.InspectionJudgeResult,
                            _job.InspectionReservationSignal,
                            _job.ProcessReservationSignal,
                            _job.TrackingDataHistory,
                            _job.EquipmentSpecialFlag,
                            _job.GlassID,
                            _job.SorterGrade,
                            _job.GlassGrade,
                            _job.FromPortNo,
                            _job.TargetPortNo,
                            _job.TargetSlotNo,
                            _job.TargetCassetteID,
                            _job.Reserve,
                            _job.PPID
                                );
                        }
                    }
                    #endregion

                    #region find DownstreamJobData



                    foreach (GlassData _job in _if.DownstreamJobData.Values)
                    {
                        _find = false;


                        if (dgvJob.Rows[_indexer].Cells[colAddress.Name].Value.ToString() == _job.GlassAddress)
                        {
                            dgvJob.Rows[_indexer].Cells[colCassetteSeqNo.Name].Value = _job.CassetteSequenceNumber;
                            dgvJob.Rows[_indexer].Cells[colJobSeqNo.Name].Value = _job.SlotSequenceNumber;
                            dgvJob.Rows[_indexer].Cells[colJobID.Name].Value = _job.JobID;

                            dgvJob.Rows[_indexer].Cells[colGroupNumber.Name].Value = _job.GroupNumber;
                            dgvJob.Rows[_indexer].Cells[colGlassType.Name].Value = _job.GlassType;
                            dgvJob.Rows[_indexer].Cells[colGlassJudge.Name].Value = _job.GlassJudge;
                            dgvJob.Rows[_indexer].Cells[colProcessSkipFlag.Name].Value = _job.ProcessSkipFlag;
                            dgvJob.Rows[_indexer].Cells[colLastGlassFlag.Name].Value = _job.LastGlassFlag;
                            dgvJob.Rows[_indexer].Cells[colCIMModeCreate.Name].Value = _job.CIMModeCreate;
                            dgvJob.Rows[_indexer].Cells[colSamplingFlag.Name].Value = _job.SamplingFlag;
                            dgvJob.Rows[_indexer].Cells[colReserved.Name].Value = _job.Reserved;
                            dgvJob.Rows[_indexer].Cells[colInspectionJudgeResult.Name].Value = _job.InspectionJudgeResult;
                            dgvJob.Rows[_indexer].Cells[colInspectionReservationSignal.Name].Value = _job.InspectionReservationSignal;
                            dgvJob.Rows[_indexer].Cells[colProcessReservationSignal.Name].Value = _job.ProcessReservationSignal;
                            dgvJob.Rows[_indexer].Cells[colTrackingDataHistory.Name].Value = _job.TrackingDataHistory;
                            dgvJob.Rows[_indexer].Cells[colEquipmentSpecialFlag.Name].Value = _job.EquipmentSpecialFlag;
                            dgvJob.Rows[_indexer].Cells[colGlassID.Name].Value = _job.GlassID;
                            dgvJob.Rows[_indexer].Cells[colSorterGrade.Name].Value = _job.SorterGrade;
                            dgvJob.Rows[_indexer].Cells[colGlassGrade.Name].Value = _job.GlassGrade;
                            dgvJob.Rows[_indexer].Cells[colFromPortNo.Name].Value = _job.FromPortNo;
                            dgvJob.Rows[_indexer].Cells[colTargetPortNo.Name].Value = _job.TargetPortNo;
                            dgvJob.Rows[_indexer].Cells[colTargetSlotNo.Name].Value = _job.TargetSlotNo;
                            dgvJob.Rows[_indexer].Cells[colTargetCassetteID.Name].Value = _job.TargetCassetteID;
                            dgvJob.Rows[_indexer].Cells[colReserve.Name].Value = _job.Reserve;
                            dgvJob.Rows[_indexer].Cells[colPPID.Name].Value = _job.PPID;


                            _find = true;
                            if (_indexer < _if.DownstreamJobData.Count + _if.UpstreamJobData.Count) _indexer++;
                        }
                        if (_find == false)
                        {
                            dgvJob.Rows.Add("Detail", "DOWN",
                                _job.GlassAddress,
                                _job.CassetteSequenceNumber,
                                _job.SlotSequenceNumber,
                               _job.JobID,

                           _job.GroupNumber,
                            _job.GlassType,
                            _job.GlassJudge,
                            _job.ProcessSkipFlag,
                            _job.LastGlassFlag,
                            _job.CIMModeCreate,
                            _job.SamplingFlag,
                            _job.Reserved,
                            _job.InspectionJudgeResult,
                            _job.InspectionReservationSignal,
                            _job.ProcessReservationSignal,
                            _job.TrackingDataHistory,
                            _job.EquipmentSpecialFlag,
                            _job.GlassID,
                            _job.SorterGrade,
                            _job.GlassGrade,
                            _job.FromPortNo,
                            _job.TargetPortNo,
                            _job.TargetSlotNo,
                            _job.TargetCassetteID,
                            _job.Reserve,
                            _job.PPID
                            );
                        }
                    }
                    #endregion
                }
                #endregion

            }
            catch (Exception ex)
            {
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void dgvJob_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dgvJob.CurrentRow != null)
                {
                    if (this.dgvJob.CurrentCell != null && dgvJob.CurrentCell.Value.ToString() == "Detail")
                    {
                        string _cstSeqNo = dgvJob.CurrentRow.Cells[colCassetteSeqNo.Name].Value.ToString();
                        string _jobSeqNo = dgvJob.CurrentRow.Cells[colJobSeqNo.Name].Value.ToString();
                        string _glassID = dgvJob.CurrentRow.Cells[colJobID.Name].Value.ToString();

                        if (_glassID == string.Empty || _cstSeqNo == string.Empty || _jobSeqNo == string.Empty)
                        {
                            ShowMessage(this, this.lblCaption.Text, "", "Cassette Seq No、Glass Seq No、Glass ID must be Required！", MessageBoxIcon.Warning);
                            return;
                        }
                        FormJobDataDetail _frm = null;


                        _frm = new FormJobDataDetail(_glassID, _cstSeqNo, _jobSeqNo);
                        _frm.TopMost = true;

                        _frm.ShowDialog();

                        _frm.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void FormInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                tmrRefreshPLC.Enabled = false;

                opiInfo.Dic_Pipe[PipeName].IsDisplay = false;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private Dictionary<int, string> addUp()
        {
            Dictionary<int, string> _upBit = new Dictionary<int, string>();

            _upBit.Add(1, "Equipment Ready");
            _upBit.Add(2, "Transfer Request");
            _upBit.Add(3, "Transfer Complete");
            _upBit.Add(4, "Pre-Ready to Send");
            _upBit.Add(5, "Stop Delivering");
            _upBit.Add(6, "Interlock 1");
            _upBit.Add(7, "Inactive 1");
            _upBit.Add(8, "Glass Presence 1");
            _upBit.Add(9, "Glass Presence 2");
            _upBit.Add(10, "Reserve");
            _upBit.Add(11, "Exchange Request");
            _upBit.Add(12, "Exchange Complete");
            _upBit.Add(13, "Exchange Deliver Only");
            _upBit.Add(14, "Force Complete Request");
            _upBit.Add(15, "Force Initial Request");
            _upBit.Add(16, "Force Initial Complete");
            _upBit.Add(17, "Slot Number#01");
            _upBit.Add(18, "Slot Number#02");
            _upBit.Add(19, "Slot Number#03");
            _upBit.Add(20, "Slot Number#04");
            _upBit.Add(21, "Slot Number#05");
            _upBit.Add(22, "Slot Number#06");
            _upBit.Add(23, "Robot Extend");
            _upBit.Add(24, "Pre-Ready Process Complete");
            _upBit.Add(25, "Robot Running");
            _upBit.Add(26, "Hardware Ready");
            return _upBit;
        }

        private Dictionary<int, string> addDown()
        {
            Dictionary<int, string> _downBit = new Dictionary<int, string>();
            _downBit.Add(1, "Active Standby Complete");
            _downBit.Add(2, "Receive Possible");
            _downBit.Add(3, "Receive Complete");
            _downBit.Add(4, "Pre-Ready to Receive");
            _downBit.Add(5, "Stop Receiving");
            _downBit.Add(6, "Interlock 2");
            _downBit.Add(7, "Inactive 2");
            _downBit.Add(8, "Glass Presence 1");
            _downBit.Add(9, "Glass Presence 2");
            _downBit.Add(10, "Reserve");
            _downBit.Add(11, "Exchange Possible");
            _downBit.Add(12, "Exchange Complete");
            _downBit.Add(13, "Exchange Impossible");
            _downBit.Add(14, "Force Complete Request");
            _downBit.Add(15, "Force Initial Request");
            _downBit.Add(16, "Spare");
            _downBit.Add(17, "Slot Number#01");
            _downBit.Add(18, "Slot Number#02");
            _downBit.Add(19, "Slot Number#03");
            _downBit.Add(20, "Slot Number#04");
            _downBit.Add(21, "Slot Number#05");
            _downBit.Add(22, "Slot Number#06");
            _downBit.Add(23, "Stage Pin");
            _downBit.Add(24, "Request PM Recovery Glass");
            _downBit.Add(25, "Exchange OK");
            _downBit.Add(26, "Exchange Cancel");
            _downBit.Add(27, "Robot Running");
            _downBit.Add(28, "Hardware Ready");
            return _downBit;
        }

        private void DebugMode_Click(object sender, EventArgs e)
        {
            FormPassWordConfirm passForm = new FormPassWordConfirm();
            passForm.StartPosition = FormStartPosition.CenterScreen;

            if (passForm.ShowDialog(this) == DialogResult.OK)
            {
                if (eq.File.DeBugMode == eEnableDisable.Disable)
                {
                    eq.File.DeBugMode = eEnableDisable.Enable;
                    DebugMode.BackColor = Color.Green;
                    eq.File.ReceiveStep = eReceiveStep.DebugMode;
                    eq.File.SendStep = eSendStep.DebugMode;

                    ObjectManager.EquipmentManager.EnqueueSave(eq.File);
                }
                else
                {
                    eq.File.DeBugMode = eEnableDisable.Disable;
                    DebugMode.BackColor = Color.Red;
                }

                ObjectManager.EquipmentManager.EnqueueSave(eq.File);
            }
        }

        private void Lable_Click(object sender, EventArgs e)
        {
            if (eq.File.DeBugMode == eEnableDisable.Enable)
            {
                if (UpNode == "L3")
                {
                    Block trx;
                   
                    if (UpSeqNo == "01")
                    {
                        trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");
                    }
                    else
                    {
                        trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");
                    }
                  
                    Label lb = sender as Label;

                    trx[int.Parse(lb.Tag.ToString()) - 1].Value = (bool)trx[int.Parse(lb.Tag.ToString()) - 1].Value == true ? false : true;


                    EquipmentService.eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", trx);
                }

                if (DownNode == "L3")
                {
                    Block trx;
                    trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "DownstreamLinkSignal");

                    Label lb = sender as Label;

                    trx[int.Parse(lb.Tag.ToString()) - 1].Value = (bool)trx[int.Parse(lb.Tag.ToString()) - 1].Value == true ? false :true;

                    EquipmentService.eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", trx);
                }
            }
        }

        private void BitAllON_Click(object sender, EventArgs e)
        {
            if (eq.File.DeBugMode == eEnableDisable.Enable)
            {
                Block trx;


                if (UpNode == "L3")
                {
                    if (UpSeqNo == "01")
                    {
                         trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");
                    }
                    else
                    {
                        trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");
                    }
                   
                }
                else
                {
                    trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "DownstreamLinkSignal");

                }

                for (int i = 0; i <= 13; i++)
                {
                    trx[i].Value = true;
                  
                }

                EquipmentService.eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", trx);

            }
            else
            {
                MessageBox.Show("Please Change to Debug Mode!", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void BitAllOff_Click(object sender, EventArgs e)
        {
            if (eq.File.DeBugMode == eEnableDisable.Enable)
            {
                Block trx;

                if (UpNode == "L3")
                {
                    if (UpSeqNo == "01")
                    {
                        trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");
                    }
                    else
                    {
                        trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "UpstreamLinkSignal");
                    }
                  

                }
                else
                {
                    trx = EquipmentService.eipTagAccess.ReadBlockValues("SD_EQToEQ_LinkSignal_03_02_00", "DownstreamLinkSignal");
                }

                for (int i = 0; i <= 13; i++)
                {
                    trx[i].Value = false;
                  
                }

                EquipmentService.eipTagAccess.WriteBlockValues("SD_EQToEQ_LinkSignal_03_02_00", trx);

            }
            else
            {
                MessageBox.Show("Please Change to Debug Mode!", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        public Trx GetTrxValues(string trxName)
        {
            Trx trx = method.GetServerAgent(eAgentName.PLCAgent).GetTransactionFormat(trxName) as Trx;
            if (trx == null) throw new Exception(string.Format("CAN'T FIND EQUIPMENT_NO=[{0}] TRX {1} IN PLCFmt.xml!", "L3", trxName));

            trx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { trxName, false }) as Trx;

            return trx;
        }
        private void SendToPLC(Trx trx)
        {
            //xMessage message = new xMessage()
            //{
            //    ToAgent = eAgentName.PLCAgent,
            //    Data = trx,
            //    Name = trx.Name,
            //    TransactionID = trx.TrackKey
            //};
            method.Invoke(eServiceName.EquipmentService, "SendToPLC", new object[] { trx });

            //  PutMessage(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Button but = sender as Button;

            FormAsk fa = new FormAsk(but.Text);
            fa.TopMost = true;
            if (fa.ShowDialog() != DialogResult.Yes)
            {
                return;
            }

            //if (but.Text == "Receive Cancel")
            //{
            //    eq.File.UPReciveCancel = eBitResult.ON;


            //}
            //if (but.Text == "Send Cancel")
            //{
            //    if (eq.Data.NODENAME != "CLEANER" && UpSeqNo == "01")
            //    {
            //        eq.File.DownSendCancelUP = eBitResult.ON;
            //    }
            //    else
            //    {
            //        eq.File.DownSendCancel = eBitResult.ON;
            //    }
            //}
            if (but.Text == "Force Complete Request To Upstream")
            {
                eq.File.ForceCompleteRequestToUpstream = eBitResult.ON;
            }
            else
            {
                eq.File.ForceCompleteRequestToDownstream = eBitResult.ON;
            }
            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Button but = sender as Button;

            //if (but.Text == "UP Resume Request")
            //{
            //    eq.File.UPReciveResum = eBitResult.ON;


            //}
            //if (but.Text == "Down Resume Request")
            //{
            //    if (eq.Data.NODENAME != "CLEANER" && UpSeqNo == "01")
            //    {
            //        eq.File.DownSendResumUP = eBitResult.ON;
            //    }
            //    else
            //    {
            //        eq.File.DownSendResum = eBitResult.ON;
            //    }
            //}
            FormAsk fa = new FormAsk(but.Text);
            fa.TopMost = true;
            if (fa.ShowDialog() != DialogResult.Yes)
            {
                return;
            }

            if (but.Text == "Force Initial Request To Upstream")
            {
                eq.File.ForceInitialRequestToUpstream = eBitResult.ON;
            }
            else
            {
                eq.File.ForceInitialRequestToDownstream = eBitResult.ON;
            }
            ObjectManager.EquipmentManager.EnqueueSave(eq.File);

        }
    }
}

