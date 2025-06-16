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
using NPOI.HSSF.Record;

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
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                    if (UpNode == eq.Data.ATTRIBUTE)
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
                    Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                    if (DownNode == eq.Data.ATTRIBUTE)
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
                KZONE.Entity.Line line = ObjectManager.LineManager.GetLine(eq.Data.LINEID);
                if (opiInfo.Dic_Pipe.ContainsKey(PipeName) == false) return;

                _if = opiInfo.Dic_Pipe[PipeName];

                DebugMode.BackColor = eq.File.DeBugMode == eEnableDisable.Enable ? Color.Green : Color.Red;
                if (UpNode != eq.Data.ATTRIBUTE)//收片
                {
                    label1.BackColor = eq.File.UpInlineMode == eBitResult.ON ? Color.Green : Color.Red;

                    if (_if.FlowType == "Normal")
                    {
                        if (_if.PathPosition == "Upper")//上层
                        {


                            label2.BackColor = eq.File.UPActionMonitor[9] == "1" ? Color.Green : Color.Red;
                            label4.Text = eq.File.ReceiveStepUP.ToString();
                            label5.BackColor = eq.File.UPReciveCompleteUP == eBitResult.OFF ? Color.Red : Color.Green;
                        }
                        else if (_if.PathPosition == "Lower")//下层
                        {
                            label2.BackColor = eq.File.UPActionMonitor[3] == "1" ? Color.Green : Color.Red;
                            label4.Text = eq.File.ReceiveStep.ToString();
                            label5.BackColor = eq.File.UPReciveComplete == eBitResult.OFF ? Color.Red : Color.Green;
                        }
                    }
                    else if (_if.FlowType == "Return")//返流
                    {
                        if (_if.PathPosition == "Upper")
                        {
                            label2.BackColor = eq.File.UPActionMonitor[11] == "1" ? Color.Green : Color.Red;
                            label4.Text = eq.File.ReceiveStepReturn.ToString();
                            label5.BackColor = eq.File.ReceiveCompleteReturn == eBitResult.OFF ? Color.Red : Color.Green;
                        }
                    }

                    label3.BackColor = eq.File.Status == eEQPStatus.Run || eq.File.Status == eEQPStatus.Idle ? Color.Green : Color.Red;
                    //label4.Text = _if.DownstreamSeqNo == "01" ? eq.File.ReceiveStep.ToString() : eq.File.ReceiveStepUP.ToString();
                    label4.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    label4.TextAlign = ContentAlignment.MiddleCenter;
                    label4.BackColor = eq.File.ReceiveStep != eReceiveStep.End ? Color.Red : Color.Green;
                    button1.Text = "Receive Cancel";
                    button1.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    button1.TextAlign = ContentAlignment.MiddleCenter;
                    button2.Text = "UP Resume Request";
                    button2.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    button2.TextAlign = ContentAlignment.MiddleCenter;

                    label5.Text = "Receive Complete";
                    label5.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    label5.TextAlign = ContentAlignment.MiddleCenter;


                    textBox1.Text = eq.File.UPDataCheckResult;
                    if (!string.IsNullOrEmpty(eq.File.UPDataCheckResult))
                    {
                        textBox1.ForeColor = Color.Red;

                    }

                }
                else
                {
                    if (_if.FlowType == "Normal")
                    {
                        if (_if.PathPosition == "Upper")
                        {
                            if (line.Data.FABTYPE != "CELL")//eq.Data.LINEID != "KWF23633L"
                            {
                                label2.BackColor = eq.File.DownActionMonitor[4] == "1" ? Color.Green : Color.Red;
                                label4.Text = eq.File.SendStepUP.ToString();
                                label4.BackColor = eq.File.SendStepUP != eSendStep.End ? Color.Red : Color.Green;
                                label5.BackColor = eq.File.DownSendCompleteUP == eBitResult.OFF ? Color.Red : Color.Green;
                            }
                            else
                            {
                                label2.BackColor = eq.File.DownActionMonitor[1] == "1" ? Color.Green : Color.Red;
                                label4.Text = eq.File.SendStepUP.ToString();
                                label4.BackColor = eq.File.SendStepUP != eSendStep.End ? Color.Red : Color.Green;
                                label5.BackColor = eq.File.DownSendCompleteUP == eBitResult.OFF ? Color.Red : Color.Green;
                            }
                        }
                        else if (_if.PathPosition == "Lower")
                        {
                            label2.BackColor = eq.File.DownActionMonitor[1] == "1" ? Color.Green : Color.Red;
                            label4.Text = eq.File.SendStep.ToString();
                            label4.BackColor = eq.File.SendStep != eSendStep.End ? Color.Red : Color.Green;
                            label5.BackColor = eq.File.DownSendComplete == eBitResult.OFF ? Color.Red : Color.Green;
                        }
                    }
                    else if (_if.FlowType == "Return")
                    {
                        if (_if.PathPosition == "Upper")
                        {
                            label2.BackColor = eq.File.DownActionMonitor[12] == "1" ? Color.Green : Color.Red;
                            label4.Text = eq.File.SendStepReturn.ToString();
                            label4.BackColor = eq.File.SendStepReturn != eSendStep.End ? Color.Red : Color.Green;
                            label5.BackColor = eq.File.SendCompleteReturn == eBitResult.OFF ? Color.Red : Color.Green;
                        }
                        else if (_if.PathPosition == "Lower")
                        {

                        }
                    }
                    label1.BackColor = eq.File.DownInlineMode == eBitResult.ON ? Color.Green : Color.Red;
                    label3.BackColor = eq.File.Status == eEQPStatus.Run || eq.File.Status == eEQPStatus.Idle ? Color.Green : Color.Red;
                    label4.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    label4.TextAlign = ContentAlignment.MiddleCenter;
                    button1.Text = "Send Cancel";
                    button1.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    button1.TextAlign = ContentAlignment.MiddleCenter;
                    button2.Text = "Down Resume Request";
                    button2.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    button2.TextAlign = ContentAlignment.MiddleCenter;

                    label5.Text = "Send Complete";
                    label5.Font = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
                    label5.TextAlign = ContentAlignment.MiddleCenter;
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
                string upLinkSignal = "0000000000000000000000000000000000000000000000000000000000000000";//"00000000000000000000000000000000"
                string downLinkSignal = "0000000000000000000000000000000000000000000000000000000000000000";

                //由PLCAgent取回link signal資料

                upTrxName = _if.TrxMatch[lblCaption.Text.Trim()]["UpStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault();

                Trx upTrx = method.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { upTrxName }) as Trx;
                if (upTrx != null) upTrx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { upTrxName, false }) as Trx;
                if (upTrx == null)
                {
                    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", upTrxName));

                }
                else
                {
                    for (int i = 0; i < upTrx.EventGroups[0].Events[0].Items.Count; i++)
                    {
                        int iOffset = upTrx.EventGroups[0].Events[0].Items[i].Metadata.BitOffset;
                        string strValue = upTrx.EventGroups[0].Events[0].Items[i].Value.ToString();
                        upLinkSignal = upLinkSignal.Remove(i, 1);
                        upLinkSignal = upLinkSignal.Insert(i, strValue);
                    }
                    _if.UpstreamSignal = upLinkSignal;//.PadRight(32,'0');
                    _if.UpstreamBitAddress = upTrx.EventGroups[0].Events[0].Metadata.Address;

                }


                downTrxName = _if.TrxMatch[lblCaption.Text.Trim()]["DownStreamTrx"].Where(x => x.Contains("Path") == true).FirstOrDefault();

                Trx downTrx = method.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { downTrxName }) as Trx;
                if (downTrx != null) downTrx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { downTrxName, false }) as Trx;
                if (downTrx == null)
                {
                    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", downTrxName));

                }
                else
                {
                    for (int i = 0; i < downTrx.EventGroups[0].Events[0].Items.Count; i++)
                    {
                        int iOffset = downTrx.EventGroups[0].Events[0].Items[i].Metadata.BitOffset;
                        string strValue = downTrx.EventGroups[0].Events[0].Items[i].Value.ToString();
                        downLinkSignal = downLinkSignal.Remove(iOffset, 1);
                        downLinkSignal = downLinkSignal.Insert(iOffset, strValue);
                    }
                    _if.DownstreamSignal = downLinkSignal;//.PadRight(32,'0');
                    _if.DownstreamBitAddress = downTrx.EventGroups[0].Events[0].Metadata.Address;
                }
                opiInfo.Dic_Pipe[PipeName] = _if;


                #endregion


                #region 扫描上下游的Word数据


                //由PLCAgent取回JobData資料 L2_SendingGlassDataReport#01
                if (!opiInfo.Dic_Node.ContainsKey(_if.UpstreamNodeNo))
                {

                    List<string> trxNameList = _if.TrxMatch[lblCaption.Text.Trim()]["UpStreamTrx"].Where(x => x.Contains("Send") == true).ToList();
                    //upJobTrxName = _if.TrxMatch[lblCaption.Text.Trim()]["UpStreamTrx"].Where(x => x.Contains("Send") == true).FirstOrDefault();
                    for (int n = 0; n < trxNameList.Count; n++)
                    {
                        upJobTrxName = trxNameList[n];

                        Trx upJobTrx = method.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { upJobTrxName }) as Trx;
                        //if(upJobTrx != null) upJobTrx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { upJobTrxName,true }) as Trx;
                        if (upJobTrx != null) upJobTrx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { upJobTrxName, false }) as Trx;//此处不用PLCAgent 记录Log  20150211  Tom
                        if (upJobTrx == null)
                        {
                            NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", upJobTrxName));
                        }
                        else
                        {

                            for (int i = 0; i < upJobTrx[0].Events.AllKeys.Length; i++)
                            {
                                GlassData upjobData = new GlassData();
                                upjobData.GlassAddress = upJobTrx.EventGroups[0].Events[i].Metadata.Address;

                                Type type = typeof(GlassData);
                                PropertyInfo property;
                                for (int j = 0; j < upJobTrx[0][i].Items.Count; j++)
                                {
                                    property = type?.GetProperty(upJobTrx[0][i][j].Name.Trim());
                                    if (property == null)
                                    {
                                        continue;
                                    }
                                    string valueType = property?.PropertyType.Name;
                                    if (valueType.Contains("Int"))
                                    {
                                        property?.SetValue(upjobData, int.Parse(upJobTrx[0][i][j].Value), null);
                                    }
                                    else
                                    {
                                        property?.SetValue(upjobData, upJobTrx[0][i][j].Value, null);
                                    }
                                }

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
                }
                else
                {
                    //if (eq.Data.LINEID == "KWF22092R")
                    //{
                    //    upJobTrxName = string.Format("{0}_SendingGlassDataReport#{1}", _if.UpstreamNodeNo, _if.UpstreamSeqNo);
                    //}
                    //else
                    //{
                    //    upJobTrxName = string.Format("{0}_SendingGlassDataReport#{1}", _if.UpstreamNodeNo, _if.UpstreamSeqNo);
                    //}

                    List<string> trxNameList = _if.TrxMatch[lblCaption.Text.Trim()]["UpStreamTrx"].Where(x => x.Contains("Send") == true).ToList();
                    for (int i = 0; i < trxNameList.Count; i++)
                    {
                        upJobTrxName = trxNameList[i];

                        Trx upJobTrx = method.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { upJobTrxName }) as Trx;
                        //if(upJobTrx != null) upJobTrx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { upJobTrxName,true }) as Trx;
                        if (upJobTrx != null) upJobTrx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { upJobTrxName, false }) as Trx;//此处不用PLCAgent 记录Log  20150211  Tom
                        if (upJobTrx == null)
                        {
                            NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", upJobTrxName));
                        }
                        else
                        {
                            int n = upJobTrx[0].Events.AllKeys.Length;

                            GlassData upjobData = new GlassData();
                            upjobData.GlassAddress = upJobTrx.EventGroups[0].Events[0].Metadata.Address;

                            Type type = typeof(GlassData);
                            PropertyInfo property;
                            for (int j = 0; j < upJobTrx[0][0].Items.Count; j++)
                            {
                                property = type?.GetProperty(upJobTrx[0][0][j].Name.Trim());
                                if (property == null)
                                {
                                    continue;
                                }
                                string valueType = property?.PropertyType.Name;
                                if (valueType.Contains("Int"))
                                {
                                    property?.SetValue(upjobData, int.Parse(upJobTrx[0][0][j].Value), null);
                                }
                                else
                                {
                                    property?.SetValue(upjobData, upJobTrx[0][0][j].Value, null);
                                }
                            }

                            if (_if.UpstreamJobData.Keys.Contains(upJobTrx.Name))
                            {
                                _if.UpstreamJobData[upJobTrx.Name] = upjobData;
                            }
                            else
                            {
                                _if.UpstreamJobData.Add(upJobTrx.Name, upjobData);
                            }


                        }

                    }

                }




                if (!opiInfo.Dic_Node.ContainsKey(_if.DownstreamNodeNo))
                {

                    //downJobTrxName = _if.TrxMatch[lblCaption.Text.Trim()]["DownStreamTrx"].Where(x => x.Contains("Receive") == true).FirstOrDefault();
                    List<string> trxNameList = _if.TrxMatch[lblCaption.Text.Trim()]["DownStreamTrx"].Where(x => x.Contains("Receive") == true).ToList();
                    for (int n = 0; n < trxNameList.Count; n++)
                    {
                        downJobTrxName = trxNameList[n];

                        Trx downJobTrx = method.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { downJobTrxName }) as Trx;
                        //if(downJobTrx != null) downJobTrx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { downJobTrxName,true }) as Trx;
                        if (downJobTrx != null) downJobTrx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { downJobTrxName, false }) as Trx; //此处不用PLCAgent 记录Log  20150211 Tom
                        if (downJobTrx == null)
                        {
                            NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", downJobTrxName));

                        }
                        else
                        {
                            for (int i = 0; i < downJobTrx[0].Events.Count; i++)
                            {
                                GlassData downJobData = new GlassData();
                                downJobData.GlassAddress = downJobTrx.EventGroups[0].Events[i].Metadata.Address;

                                Type type = typeof(GlassData);
                                PropertyInfo property;
                                for (int j = 0; j < downJobTrx[0][i].Items.Count; j++)
                                {
                                    property = type?.GetProperty(downJobTrx[0][i][j].Name.Trim());
                                    if (property == null)
                                    {
                                        continue;
                                    }
                                    string valueType = property?.PropertyType.Name;
                                    if (valueType.Contains("Int"))
                                    {
                                        property?.SetValue(downJobData, int.Parse(downJobTrx[0][i][j].Value), null);
                                    }
                                    else
                                    {
                                        property?.SetValue(downJobData, downJobTrx[0][i][j].Value, null);
                                    }
                                }

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
                }
                else
                {

                    List<string> trxNameList = _if.TrxMatch[lblCaption.Text.Trim()]["DownStreamTrx"].Where(x => x.Contains("Receive") == true).ToList();
                    for (int i = 0; i < trxNameList.Count; i++)
                    {
                        downJobTrxName = trxNameList[i];

                        Trx downJobTrx = method.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { downJobTrxName }) as Trx;
                        //if(downJobTrx != null) downJobTrx = Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { downJobTrxName,true }) as Trx;
                        if (downJobTrx != null) downJobTrx = method.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { downJobTrxName, false }) as Trx; //此处不用PLCAgent 记录Log  20150211 Tom
                        if (downJobTrx == null)
                        {
                            NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, string.Format("Can't get PLC value from Trx[{0}].", downJobTrxName));

                        }
                        else
                        {


                            GlassData downJobData = new GlassData();
                            downJobData.GlassAddress = downJobTrx.EventGroups[0].Events[i].Metadata.Address;

                            Type type = typeof(GlassData);
                            PropertyInfo property;
                            for (int j = 0; j < downJobTrx[0][0].Items.Count; j++)
                            {
                                property = type?.GetProperty(downJobTrx[0][0][j].Name.Trim());
                                if (property == null)
                                {
                                    continue;
                                }
                                string valueType = property?.PropertyType.Name;
                                if (valueType.Contains("Int"))
                                {
                                    property?.SetValue(downJobData, int.Parse(downJobTrx[0][0][j].Value), null);
                                }
                                else
                                {
                                    property?.SetValue(downJobData, downJobTrx[0][0][j].Value, null);
                                }
                            }

                            if (_if.DownstreamJobData.Keys.Contains(downJobTrx.Name))
                            {
                                _if.DownstreamJobData[downJobTrx.Name] = downJobData;
                            }
                            else
                            {
                                _if.DownstreamJobData.Add(downJobTrx.Name, downJobData);
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

                    if ((eq.Data.NODENAME == "TEREWORK" || eq.Data.NODENAME == "TSREWORK" || eq.Data.NODENAME == "KWF22093L") && (_lbl.Tag.ToString() == "31" || _lbl.Tag.ToString() == "32"))
                    {

                        int.TryParse(_lbl.Tag.ToString(), out _seq);

                        if (_if.UpstreamSignal.Substring(_seq - 1, 1) == "0") _lbl.Image = Properties.Resources.Bit_Sliver;
                        else _lbl.Image = Properties.Resources.Bit_Green;

                        _lbl.Text = "          " + CountAddress(_if.UpstreamBitAddress, (_seq - 1 + 17).ToString()) + "  " + DicUp_Desc[_seq];


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


                    if ((eq.Data.NODENAME == "TEREWORK" || eq.Data.NODENAME == "TSREWORK" || eq.Data.NODENAME == "KWF22093L") && (_lbl.Tag.ToString() == "31" || _lbl.Tag.ToString() == "32"))
                    {

                        int.TryParse(_lbl.Tag.ToString(), out _seq);

                        if (_if.DownstreamSignal.Substring(_seq - 1, 1) == "0") _lbl.Image = Properties.Resources.Bit_Sliver;
                        else _lbl.Image = Properties.Resources.Bit_Green;

                        _lbl.Text = "          " + CountAddress(_if.DownstreamBitAddress, (_seq - 1 + 17).ToString()) + "  " + DicDown_Desc[_seq];


                    }
                    else
                    {

                        int.TryParse(_lbl.Tag.ToString(), out _seq);

                        if (_if.DownstreamSignal.Substring(_seq - 1, 1) == "0") _lbl.Image = Properties.Resources.Bit_Sliver;
                        else _lbl.Image = Properties.Resources.Bit_Green;

                        _lbl.Text = "          " + CountAddress(_if.DownstreamBitAddress, (_seq - 1).ToString()) + "  " + DicDown_Desc[_seq];
                    }
                }
                #endregion

                #region JobData
                string kzonePPID = string.Empty;
                if (dgvJob.Rows.Count == 0)
                {
                    #region Add Job Data
                    foreach (GlassData _job in _if.UpstreamJobData.Values)
                    {
                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            kzonePPID = _job.PPID.Substring(26, 26);
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (line.Data.ATTRIBUTE == "CLN")
                            {
                                if(line.Data.LINEID=="KWF23637R")
                                {
                                    kzonePPID = _job.PPID.Substring(28, 4);//PH07 28
                                }
                                else
                                {
                                    kzonePPID = _job.PPID.Substring(32, 4);//PH01~06 28+4
                                }
                                
                            }
                            else if (line.Data.ATTRIBUTE == "DEV")
                            {
                                kzonePPID = _job.PPID.Substring(56, 4);//52+4
                            }

                        }
                        else
                        {
                            kzonePPID = _job.PPID;
                        }
                        dgvJob.Rows.Add("Detail", "UP",
                            _job.GlassAddress,
                            _job.Cassette_Sequence_No,
                            _job.Job_Sequence_No,
                            _job.Lot_ID,
                            _job.Product_ID,
                            _job.Operation_ID,
                            _job.GlassID_or_PanelID,
                            _job.CST_Operation_Mode,
                            _job.Substrate_Type,
                            _job.Product_Type,
                            _job.Job_Type,
                            _job.Dummy_Type,
                            _job.Skip_Flag,
                            _job.Process_Flag,
                            _job.Process_Reason_Code,
                            _job.LOT_Code,
                            _job.Glass_Thickness,
                            _job.Glass_Degree,
                            _job.Inspection_Flag,
                            _job.Job_Judge,
                            _job.Job_Grade,
                            _job.Job_Recovery_Flag,
                            _job.Mode,
                            _job.Step_ID,
                            //_job.VCR_Read_ID,
                            _job.Master_Recipe_ID,
                            kzonePPID
                            );
                    }

                    foreach (GlassData _job in _if.DownstreamJobData.Values)
                    {
                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            kzonePPID = _job.PPID.Substring(26, 26);
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (line.Data.ATTRIBUTE == "CLN")
                            {
                                if (line.Data.LINEID == "KWF23637R")
                                {
                                    kzonePPID = _job.PPID.Substring(28, 4);//PH07 28
                                }
                                else
                                {
                                    kzonePPID = _job.PPID.Substring(32, 4);//PH01~06 28+4
                                }
                            }
                            else if (line.Data.ATTRIBUTE == "DEV")
                            {
                                kzonePPID = _job.PPID.Substring(56, 4);//52+4
                            }

                        }
                        else
                        {
                            kzonePPID = _job.PPID;
                        }
                        dgvJob.Rows.Add("Detail", "DOWN",
                            _job.GlassAddress,
                               _job.Cassette_Sequence_No,
                            _job.Job_Sequence_No,
                            _job.Lot_ID,
                            _job.Product_ID,
                            _job.Operation_ID,
                            _job.GlassID_or_PanelID,
                            _job.CST_Operation_Mode,
                            _job.Substrate_Type,
                            _job.Product_Type,
                            _job.Job_Type,
                            _job.Dummy_Type,
                            _job.Skip_Flag,
                            _job.Process_Flag,
                            _job.Process_Reason_Code,
                            _job.LOT_Code,
                            _job.Glass_Thickness,
                            _job.Glass_Degree,
                            _job.Inspection_Flag,
                            _job.Job_Judge,
                            _job.Job_Grade,
                            _job.Job_Recovery_Flag,
                            _job.Mode,
                            _job.Step_ID,
                            //_job.VCR_Read_ID,
                            _job.Master_Recipe_ID,
                            kzonePPID
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
                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            kzonePPID = _job.PPID.Substring(26, 26);
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (line.Data.ATTRIBUTE == "CLN")
                            {
                                if (line.Data.LINEID == "KWF23637R")
                                {
                                    kzonePPID = _job.PPID.Substring(28, 4);//PH07 28
                                }
                                else
                                {
                                    kzonePPID = _job.PPID.Substring(32, 4);//PH01~06 28+4
                                }
                            }
                            else if (line.Data.ATTRIBUTE == "DEV")
                            {
                                kzonePPID = _job.PPID.Substring(56, 4);//52+4
                            }

                        }
                        else
                        {
                            kzonePPID = _job.PPID;
                        }
                        if (dgvJob.Rows[_indexer].Cells[colAddress.Name].Value.ToString() == _job.GlassAddress)
                        {

                            for (int i = 0; i < dgvJob.Columns.Count; i++)
                            {
                                Type type = typeof(GlassData);
                                string colName = dgvJob.Columns[i].Name.Trim();
                                string dataName = colName;
                                if (dataName.Contains("col"))
                                {
                                    dataName = dataName.Remove(0, 3);
                                }
                                PropertyInfo property = type.GetProperty(dataName);
                                string value = property?.GetValue(_job, null)?.ToString();
                                if (dataName == "Detail")
                                {
                                    value = "Detail";
                                }
                                if (dataName == "Stream")
                                {
                                    value = "UP";
                                }
                                if (dataName == "Address")
                                {
                                    value = _job.GlassAddress;
                                }
                                if (dataName == "PPID")
                                {
                                    value = kzonePPID;
                                }
                                dgvJob.Rows[_indexer].Cells[colName].Value = value;

                            }


                            _find = true;
                            if (_indexer < _if.UpstreamJobData.Count)
                                _indexer++;
                        }

                        if (_find == false)
                        {
                            dgvJob.Rows.Add("Detail", "UP",
                                _job.GlassAddress,
                                _job.Cassette_Sequence_No,
                            _job.Job_Sequence_No,
                            _job.Lot_ID,
                            _job.Product_ID,
                            _job.Operation_ID,
                            _job.GlassID_or_PanelID,
                            _job.CST_Operation_Mode,
                            _job.Substrate_Type,
                            _job.Product_Type,
                            _job.Job_Type,
                            _job.Dummy_Type,
                            _job.Skip_Flag,
                            _job.Process_Flag,
                            _job.Process_Reason_Code,
                            _job.LOT_Code,
                            _job.Glass_Thickness,
                            _job.Glass_Degree,
                            _job.Inspection_Flag,
                            _job.Job_Judge,
                            _job.Job_Grade,
                            _job.Job_Recovery_Flag,
                            _job.Mode,
                            _job.Step_ID,
                            //_job.VCR_Read_ID,
                            _job.Master_Recipe_ID,
                            kzonePPID
                                );
                        }
                    }
                    #endregion

                    #region find DownstreamJobData



                    foreach (GlassData _job in _if.DownstreamJobData.Values)
                    {
                        _find = false;

                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            kzonePPID = _job.PPID.Substring(26, 26);
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (line.Data.ATTRIBUTE == "CLN")
                            {
                                if (line.Data.LINEID == "KWF23637R")
                                {
                                    kzonePPID = _job.PPID.Substring(28, 4);//PH07 28
                                }
                                else
                                {
                                    kzonePPID = _job.PPID.Substring(32, 4);//PH01~06 28+4
                                }
                            }
                            else if (line.Data.ATTRIBUTE == "DEV")
                            {
                                kzonePPID = _job.PPID.Substring(56, 4);//52+4
                            }

                        }
                        else
                        {
                            kzonePPID = _job.PPID;
                        }
                        if (dgvJob.Rows[_indexer].Cells[colAddress.Name].Value.ToString() == _job.GlassAddress)
                        {

                            for (int i = 0; i < dgvJob.Columns.Count; i++)
                            {
                                Type type = typeof(GlassData);
                                string colName = dgvJob.Columns[i].Name.Trim();
                                string dataName = colName;
                                if (dataName.Contains("col"))
                                {
                                    dataName = dataName.Remove(0, 3);
                                }
                                PropertyInfo property = type.GetProperty(dataName);
                                string value = property?.GetValue(_job, null)?.ToString();
                                if (dataName == "Detail")
                                {
                                    value = "Detail";
                                }
                                if (dataName == "Stream")
                                {
                                    value = "DOWN";
                                }
                                if (dataName == "Address")
                                {
                                    value = _job.GlassAddress;
                                }
                                if (dataName == "PPID")
                                {
                                    value = kzonePPID;
                                }
                                dgvJob.Rows[_indexer].Cells[colName].Value = value;
                            }

                            _find = true;
                            if (_indexer < _if.DownstreamJobData.Count + _if.UpstreamJobData.Count) _indexer++;
                        }
                        if (_find == false)
                        {
                            dgvJob.Rows.Add("Detail", "DOWN",
                                _job.GlassAddress,
                                _job.Cassette_Sequence_No,
                            _job.Job_Sequence_No,
                            _job.Lot_ID,
                            _job.Product_ID,
                            _job.Operation_ID,
                            _job.GlassID_or_PanelID,
                            _job.CST_Operation_Mode,
                            _job.Substrate_Type,
                            _job.Product_Type,
                            _job.Job_Type,
                            _job.Dummy_Type,
                            _job.Skip_Flag,
                            _job.Process_Flag,
                            _job.Process_Reason_Code,
                            _job.LOT_Code,
                            _job.Glass_Thickness,
                            _job.Glass_Degree,
                            _job.Inspection_Flag,
                            _job.Job_Judge,
                            _job.Job_Grade,
                            _job.Job_Recovery_Flag,
                            _job.Mode,
                            _job.Step_ID,
                            //_job.VCR_Read_ID,
                            _job.Master_Recipe_ID,
                            kzonePPID
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
                        string _cstSeqNo = dgvJob.CurrentRow.Cells[colCassette_Sequence_No.Name].Value.ToString();
                        string _jobSeqNo = dgvJob.CurrentRow.Cells[colJob_Sequence_No.Name].Value.ToString();
                        string _glassID = dgvJob.CurrentRow.Cells[colGlassID_or_PanelID.Name].Value.ToString();

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

            _upBit.Add(1, "Upstream Inline");
            _upBit.Add(2, "Upstream Trouble");
            _upBit.Add(3, "Send Ready");
            _upBit.Add(4, "Send");
            _upBit.Add(5, "Job Transfer");
            _upBit.Add(6, "Send Cancel");
            _upBit.Add(7, "Exchange Execute");
            _upBit.Add(8, "Double Glass");
            _upBit.Add(9, "Send Job Reserve");
            _upBit.Add(10, "Receive OK");
            _upBit.Add(11, "Spare");
            _upBit.Add(12, "Spare");
            _upBit.Add(13, "Spare");
            _upBit.Add(14, "Spare");
            _upBit.Add(15, "Spare");
            _upBit.Add(16, "Spare");
            _upBit.Add(17, "Slot Number#01");
            _upBit.Add(18, "Slot Number#02");
            _upBit.Add(19, "Slot Number#03");
            _upBit.Add(20, "Slot Number#04");
            _upBit.Add(21, "Slot Number#05");
            _upBit.Add(22, "Slot Number#06");
            _upBit.Add(23, "Spare");
            _upBit.Add(24, "Spare");
            _upBit.Add(25, "Spare");
            _upBit.Add(26, "Short Vicinity");
            _upBit.Add(27, "Long Vicinity");
            _upBit.Add(28, "Preparation Completion");
            _upBit.Add(29, "In Inspecting");
            _upBit.Add(30, "In Inspecting");
            _upBit.Add(31, "In Inspecting");
            _upBit.Add(32, "Return Mode");
            return _upBit;
        }

        private Dictionary<int, string> addDown()
        {
            Dictionary<int, string> _downBit = new Dictionary<int, string>();
            _downBit.Add(1, "Downstream Inline");
            _downBit.Add(2, "Downstream Trouble");
            _downBit.Add(3, "Receive Able");
            _downBit.Add(4, "Receive");
            _downBit.Add(5, "Job Transfer");
            _downBit.Add(6, "Receive Cancel");
            _downBit.Add(7, "Exchange Possible");
            _downBit.Add(8, "Double Glass");
            _downBit.Add(9, "Receive Job Reserve");
            _downBit.Add(10, "Spare");
            _downBit.Add(11, "Transfer Stop Request");
            _downBit.Add(12, "Dummy Glass Request");
            _downBit.Add(13, "Glass Exist");
            _downBit.Add(14, "Spare");
            _downBit.Add(15, "Spare");
            _downBit.Add(16, "Spare");
            _downBit.Add(17, "Slot Number#01");
            _downBit.Add(18, "Slot Number#02");
            _downBit.Add(19, "Slot Number#03");
            _downBit.Add(20, "Slot Number#04");
            _downBit.Add(21, "Slot Number#05");
            _downBit.Add(22, "Slot Number#06");
            _downBit.Add(23, "Glass Count#01");
            _downBit.Add(24, "Glass Count#02");
            _downBit.Add(25, "Glass Count#03");
            _downBit.Add(26, "Glass Count#04");
            _downBit.Add(27, "Glass Count#05");
            _downBit.Add(28, "Preparation Permission");
            _downBit.Add(29, "Inspection Result Update");
            _downBit.Add(30, "Spare");
            _downBit.Add(31, "Spare");
            _downBit.Add(32, "Spare");
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
                    if (eq.Data.LINEID == "KWF22090R" || eq.Data.LINEID == "KWF22091R")
                    {
                        eq.File.ReceiveStepUP = eReceiveStep.DebugMode;
                        eq.File.ReceiveStepReturn = eReceiveStep.DebugMode;
                    }
                    eq.File.SendStep = eSendStep.DebugMode;
                    if (eq.Data.LINEID == "KWF22090R" || eq.Data.LINEID == "KWF22091R")
                    {
                        eq.File.SendStepUP = eSendStep.DebugMode;
                        eq.File.SendStepReturn = eSendStep.DebugMode;
                    }

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
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                if (UpNode == eq.Data.ATTRIBUTE)
                {
                    Trx trx;
                    string trxName = _if.TrxMatch[lblCaption.Text.Trim()]["UpStreamTrx"].Where(x => x.Contains("Path")).FirstOrDefault();
                    trx = GetTrxValues(trxName);

                    Label lb = sender as Label;

                    trx[0][0][int.Parse(lb.Tag.ToString()) - 1].Value = trx[0][0][int.Parse(lb.Tag.ToString()) - 1].Value == "1" ? "0" : "1";

                    SendToPLC(trx);
                }

                if (DownNode == eq.Data.ATTRIBUTE)
                {
                    Trx trx;
                    string trxName = _if.TrxMatch[lblCaption.Text.Trim()]["DownStreamTrx"].Where(x => x.Contains("Path")).FirstOrDefault();
                    trx = GetTrxValues(trxName);

                    Label lb = sender as Label;

                    trx[0][0][int.Parse(lb.Tag.ToString()) - 1].Value = trx[0][0][int.Parse(lb.Tag.ToString()) - 1].Value == "1" ? "0" : "1";

                    SendToPLC(trx);
                }
            }
        }

        private void BitAllON_Click(object sender, EventArgs e)
        {
            if (eq.File.DeBugMode == eEnableDisable.Enable)
            {
                Trx trx;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                if (UpNode == eq.Data.ATTRIBUTE)
                {
                    string trxName = _if.TrxMatch[this.lblCaption.Text.Trim()]["UpStreamTrx"].Where(x => x.Contains("Path")).FirstOrDefault();
                    trx = GetTrxValues(trxName);

                }
                else
                {

                    string trxName = _if.TrxMatch[this.lblCaption.Text.Trim()]["DownStreamTrx"].Where(x => x.Contains("Path")).FirstOrDefault();
                    trx = GetTrxValues(trxName);
                }

                for (int i = 0; i <= 31; i++)
                {
                    trx[0][0][i].Value = "1";
                }

                SendToPLC(trx);

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
                Trx trx;
                Equipment eq = ObjectManager.EquipmentManager.GetEquipmentByNo("L3");
                if (UpNode == eq.Data.ATTRIBUTE)
                {
                    string trxName = _if.TrxMatch[this.lblCaption.Text.Trim()]["UpStreamTrx"].Where(x => x.Contains("Path")).FirstOrDefault();
                    trx = GetTrxValues(trxName);

                }
                else
                {
                    string trxName = _if.TrxMatch[this.lblCaption.Text.Trim()]["DownStreamTrx"].Where(x => x.Contains("Path")).FirstOrDefault();
                    trx = GetTrxValues(trxName);

                }

                for (int i = 0; i <= 31; i++)
                {
                    trx[0][0][i].Value = "0";
                }

                SendToPLC(trx);


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

            if (but.Text == "Receive Cancel")
            {
                string linkKey = this.lblCaption.Text.Trim();
                LinkSignalType linkSignalType = opiInfo.Lst_LinkSignal_Type.Where(x => x.LinkKey == linkKey).FirstOrDefault();
                string flowType = linkSignalType.FlowType;
                string pathPosition = linkSignalType.PathPosition;

                if (flowType == "Return")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.ReceiveCancelReturn = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {

                    }
                }
                else if (flowType == "Normal")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.UPReceiveCancelUP = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {
                        eq.File.UPReciveCancel = eBitResult.ON;
                    }
                }

            }

            if (but.Text == "Send Cancel")
            {
                string linkKey = this.lblCaption.Text.Trim();
                LinkSignalType linkSignalType = opiInfo.Lst_LinkSignal_Type.Where(x => x.LinkKey == linkKey).FirstOrDefault();
                string flowType = linkSignalType.FlowType;
                string pathPosition = linkSignalType.PathPosition;

                if (flowType == "Return")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.SendCancelReturn = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {

                    }
                }
                else if (flowType == "Normal")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.DownSendCancelUP = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {
                        eq.File.DownSendCancel = eBitResult.ON;
                    }
                }
            }

            ObjectManager.EquipmentManager.EnqueueSave(eq.File);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            Button but = sender as Button;

            if (but.Text == "UP Resume Request")
            {
                string linkKey = this.lblCaption.Text.Trim();
                LinkSignalType linkSignalType = opiInfo.Lst_LinkSignal_Type.Where(x => x.LinkKey == linkKey).FirstOrDefault();
                string flowType = linkSignalType.FlowType;
                string pathPosition = linkSignalType.PathPosition;

                if (flowType == "Return")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.ReceiveResumReturn = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {

                    }
                }
                else if (flowType == "Normal")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.UPReceiveResumUP = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {
                        eq.File.UPReciveResum = eBitResult.ON;
                    }
                }

            }

            if (but.Text == "Down Resume Request")
            {

                string linkKey = this.lblCaption.Text.Trim();
                LinkSignalType linkSignalType = opiInfo.Lst_LinkSignal_Type.Where(x => x.LinkKey == linkKey).FirstOrDefault();
                string flowType = linkSignalType.FlowType;
                string pathPosition = linkSignalType.PathPosition;

                if (flowType == "Return")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.SendResumReturn = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {

                    }
                }
                else if (flowType == "Normal")
                {
                    if (pathPosition == "Upper")
                    {
                        eq.File.DownSendResumUP = eBitResult.ON;
                    }
                    else if (pathPosition == "Lower")
                    {
                        eq.File.DownSendResum = eBitResult.ON;
                    }
                }


            }

            ObjectManager.EquipmentManager.EnqueueSave(eq.File);

        }

        private void dgvJob_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

