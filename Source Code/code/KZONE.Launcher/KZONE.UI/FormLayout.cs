using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using KZONE.Log;
using UNILAYOUT;
using KZONE.ConstantParameter;
using KZONE.Service;
using KZONE.EntityManager;
using KZONE.Entity;

namespace KZONE.UI
{
    public partial class FormLayout : FormBase
    {
        Port CurPort;
        Dense CurDense;
        //Pallet CurPallet;
        Equipment eqp;
        KZONE.Entity.Unit unit;

        //  Node CurNode;
        //  Unit CurUnit;
        DCR CurVCR = new DCR();
        ToolTip Tip;

        OPIInfo OPIAp = OPIInfo.CreateInstance();

        Dictionary<string, csLabel> Dic_SECSLabel = new Dictionary<string, csLabel>();  //key: NodeNo

        public delegate void AddLogHandler(string Msg);

        private List<string> hideContrlNames;//記錄已隱藏的控制項
        private string[] layoutsize; //取出layout預設大小
        private float layoutfontsize;//取出layout預設文字大小



        public string LoggerName { get; set; }

        public ConstantManager ConstantManager
        {
            get;
            set;
        }

        public ParameterManager ParameterManager
        {
            get;
            set;
        }

        public EquipmentService EquipmentService
        {
            get;
            set;
        }

        public void Init()
        {

        }

        public FormLayout()
        {
            this.InitializeComponent();
            this.lblCaption.Text = "LAYOUT";
        }


        public FormLayout(OPIInfo apInfo)
        {
            try
            {


                OPIAp = apInfo;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void FormLayout_Load(object sender, EventArgs e)
        {
            try
            {
                string strLayoutPath = string.Format("{0}{1}\\{2}.xml", OPIConst.LayoutFolder, OPIAp.CurLine.LineType, OPIAp.CurLine.LineType);  //OPIConst.LayoutFolder + OPIAp.CurLine.FabType + string.Format(OPIAp.CurLine.ServerName + ".xml");
                if (File.Exists(strLayoutPath))
                {
                    LoadXMLFile LayoutDesign = new LoadXMLFile(strLayoutPath);
                    LayoutDesign.Create(pnlLayout);
                }
                layoutsize = UniTools.GetLayoutSize(pnlLayout, out layoutfontsize);
                //rdoSzie125.Checked = true;
                #region 左側顯示區域設定

                flpEQP.Location = new Point(0, 0);

                flpUnit.Location = new Point(0, 0);
                flpVCR.Location = new Point(0, 0);



                flpEQP.Height = 980;

                flpUnit.Height = 600;
                flpVCR.Height = 600;


                #region Set Line info for 左側區塊
                //txtServerName.Text = OPIAp.CurLine.ServerName;
                //txtLineID.Text = OPIAp.CurLine.LineID;
                //txtLineType.Text = OPIAp.CurLine.LineType;
                //txtFactoryType.Text = OPIAp.CurLine.FabType;

                #endregion

                #endregion


                Tip = new ToolTip();

                InitialLayout();
                // DataSetting_Line();
                //  SetPanel_Visible(flpLine);


                bgwRefresh.RunWorkerAsync();

                hideContrlNames = new List<string>();

                SetPanel_Visible(flpEQP);

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", "", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void InitialLayout()
        {
            ToolTip tip = new ToolTip();

            try
            {
                #region Init csLabel
                string _lblName = string.Empty;
                string _lblNo = string.Empty;
                foreach (csLabel label in pnlLayout.Controls.OfType<csLabel>())
                {
                    if (label.Name.Length < 6) continue;

                    label.Tag = string.Empty;
                    label.BackColor = Color.Silver;

                    _lblName = label.Name.Substring(0, 2);
                    _lblNo = label.Name.Substring(2, 2);
                    switch (_lblName)
                    {
                        case "SC": //SECS Staus - 當SECS Disconnect時顯示通知

                            label.Visible = false;
                            Dic_SECSLabel.Add(_lblNo, label);

                            break;

                        default:
                            break;
                    }
                }
                #endregion

                #region Init Pipe
                foreach (ucPipe pipe in pnlLayout.Controls.OfType<ucPipe>())
                {
                    pipe.Tag = string.Empty;
                    pipe.pictureBox1.Click += new EventHandler(Pipe_Click);
                }
                #endregion

                #region Init Node
                int _num = 0;
                string _localNo = string.Empty;

                foreach (ucEQ eq in pnlLayout.Controls.OfType<ucEQ>())
                {
                    #region 取得local no
                    int.TryParse(eq.Name.Substring(1, 2), out _num);

                    _localNo = string.Format("L{0}", _num.ToString());

                    #endregion

                    #region csPictureBox
                    List<csPictureBox> _hasParentPic = new List<csPictureBox>();
                    foreach (csPictureBox _pic in eq.Controls.OfType<csPictureBox>())
                    {
                        _pic.BackColor = _pic.BackColor = Color.DarkGray;

                        _pic.Tag = _localNo;

                        if (_pic.Name.Length < 4) continue;

                        string _type = _pic.Name.Substring(0, 2).ToUpper();

                        int _seqNo = 0;

                        int.TryParse(_pic.Name.Substring(2, 2), out _seqNo);

                        if ((string)_pic.PropertyData.GetPropertyVale("ParentName") != string.Empty) _hasParentPic.Add(_pic);

                        switch (_type)
                        {
                            #region VCR
                            case "VR": //VCR
                                _pic.BackgroundImage = Properties.Resources.Layout_VCROff;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(VCR_Click);
                                break;
                            #endregion

                            #region Buffer Slot Infomation
                            case "FS":
                                _pic.BackgroundImage = Properties.Resources.CSTBuffer_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                //    _pic.Click += new EventHandler(BFSlotInfo_Click);
                                break;
                            #endregion

                            #region Robot 非unit 純圖片顯示
                            case "RD": //Robot- Double Arm
                                _pic.BackgroundImage = Properties.Resources.Layout_DoubleArm;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;

                            case "RS": //Robot- single Arm
                                _pic.BackgroundImage = Properties.Resources.Layout_SingleArm;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;
                            #endregion

                            #region  Stage 非unit 純圖片顯示
                            case "TF": //Stage Turn Fix
                                _pic.BackgroundImage = Properties.Resources.Layout_StageFixed;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;

                            case "TT": //Stage Turn Table
                                _pic.BackgroundImage = Properties.Resources.Layout_StageTurn_OFF;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;

                            case "TO": //Stage Turn Over
                                _pic.BackgroundImage = Properties.Resources.Layout_StageTurnOver_OFF;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;
                            #endregion

                            #region  Conveyor
                            case "CV": //Conveyor  橫向
                                _pic.BackgroundImage = Properties.Resources.Layout_Conveyor1;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;

                            case "CB": //Conveyor  橫向加長
                                _pic.BackgroundImage = Properties.Resources.Layout_Conveyor1_1;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;

                            case "CL": //Conveyor  縱向
                                _pic.BackgroundImage = Properties.Resources.Layout_Conveyor2;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;

                            case "CM": //Conveyor  縱向加長
                                _pic.BackgroundImage = Properties.Resources.Layout_Conveyor2_1;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;
                            #endregion

                            #region PPK&QPP Dense / Pallet
                            case "PK": //PPK Dense Port
                                _pic.BackgroundImage = Properties.Resources.Dense_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(Dense_Click);
                                break;

                            case "TL": //台車 Trolley
                                _pic.BackgroundImage = Properties.Resources.Layout_Trolley;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;

                            case "TR":
                                _pic.BackgroundImage = Properties.Resources.Layout_Trolley_Track;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(EQ_Click);
                                break;
                            #endregion

                            #region Port
                            case "CN": // Normal Cassette 
                                _pic.BackgroundImage = Properties.Resources.CSTNormal_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(Port_Click);
                                break;

                            case "CS": //Scrap
                            case "CC": // Cell Cassette
                                _pic.BackgroundImage = Properties.Resources.CSTCell_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(Port_Click);
                                break;

                            case "CW": // Wire Cassette
                                _pic.BackgroundImage = Properties.Resources.CSTWire_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(Port_Click);
                                break;

                            case "DS":  //Dense Box

                                _pic.BackgroundImage = Properties.Resources.Dense_NONE;
                                _pic.BorderStyle = BorderStyle.None;

                                if (_seqNo < 50)
                                {
                                    _pic.Click += new EventHandler(Port_Click);
                                }
                                else
                                {
                                    _pic.Click += new EventHandler(EQ_Click);
                                }
                                break;

                            case "BF": //Buffer有下貨功能
                            case "BW": //Wire Buffer 有下貨功能
                                _pic.BackgroundImage = Properties.Resources.CSTBuffer_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(Port_Click);
                                break;

                            case "MP": //M Port
                                _pic.BackgroundImage = Properties.Resources.CSTMPortr_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(Port_Click);
                                break;

                            case "TP": //Tray Port
                                _pic.BackgroundImage = Properties.Resources.CSTTray_NONE;
                                _pic.BorderStyle = BorderStyle.None;
                                _pic.Click += new EventHandler(Port_Click);
                                break;

                            #endregion

                            #region  Unit
                            case "UN":  //for unit內的物件背景顯示用 EX:UN02:RD01 Robot顏色會與unit 02相同 

                                if (_pic.Name.Length == 9)
                                {
                                    string[] _data = _pic.Name.Split(':');

                                    switch (_data[1].Substring(0, 2))
                                    {
                                        #region Robot
                                        case "RD": //Robot- Double Arm
                                            _pic.BackgroundImage = Properties.Resources.Layout_DoubleArm;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;

                                        case "RS": //Robot- single Arm
                                            _pic.BackgroundImage = Properties.Resources.Layout_SingleArm;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;
                                        #endregion

                                        #region  Stage

                                        case "TF": //Stage Turn Fix
                                            _pic.BackgroundImage = Properties.Resources.Layout_StageFixed;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;

                                        case "TT": //Stage Turn Table
                                            _pic.BackgroundImage = Properties.Resources.Layout_StageTurn_OFF;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;

                                        case "TO": //Stage Turn Over
                                            _pic.BackgroundImage = Properties.Resources.Layout_StageTurnOver_OFF;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;

                                        case "TU": //Stage Turn Over
                                            _pic.BackgroundImage = Properties.Resources.Layout_StageTurnOver;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;

                                        #endregion

                                        #region  Conveyor
                                        case "CV": //Conveyor  橫向
                                            _pic.BackgroundImage = Properties.Resources.Layout_Conveyor1;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);

                                            break;

                                        case "CB": //Conveyor  橫向加長
                                            _pic.BackgroundImage = Properties.Resources.Layout_Conveyor1_1;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);

                                            break;

                                        case "CL": //Conveyor  縱向
                                            _pic.BackgroundImage = Properties.Resources.Layout_Conveyor2;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);

                                            break;

                                        case "CM": //Conveyor  縱向加長
                                            _pic.BackgroundImage = Properties.Resources.Layout_Conveyor2_1;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);

                                            break;
                                        #endregion

                                        #region  台車 Trolley
                                        case "TL": //台車 Trolley
                                            _pic.BackgroundImage = Properties.Resources.Layout_Trolley;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;
                                        #endregion

                                        #region  Buffer Unit
                                        case "BF": //Buffer Unit
                                            _pic.BackgroundImage = Properties.Resources.CSTBuffer_UC;
                                            _pic.BorderStyle = BorderStyle.None;
                                            _pic.Click += new EventHandler(Unit_Click);
                                            break;
                                        #endregion

                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            default:
                                break;
                        }
                    }
                    #endregion

                    #region csLabel
                    foreach (csLabel _lbl in eq.Controls.OfType<csLabel>())
                    {
                        _lbl.Tag = _localNo;

                        if (_lbl.Name.Length < 4) continue;

                        int _seqNo = 0;

                        int.TryParse(_lbl.Name.Substring(2, 2), out _seqNo);

                        switch (_lbl.Name.ToUpper())
                        {
                            case "NODENAME":
                                _lbl.Click += new EventHandler(EQ_Click);
                                _lbl.Text = _localNo;
                                _lbl.BackColor = Color.Red;
                                break;

                            case "EQDIS":   //新加Layout 描述样图 Lable，前荣 2017/1/2
                                _lbl.Click += new EventHandler(EQ_Click);
                                break;

                            case "TOTALCOUNT":
                                _lbl.BackColor = Color.LawnGreen;
                                _lbl.Text = "0";

                                break;

                            default:
                                _lbl.BackColor = Color.DarkGray;


                                if (_lbl.Name.Substring(0, 2).ToUpper() == "UN")  //Unit Object
                                {
                                    if (_seqNo < 50)
                                    {
                                        _lbl.Click += new EventHandler(Unit_Click);
                                    }
                                    else
                                    {
                                        _lbl.Click += new EventHandler(EQ_Click);
                                    }

                                }
                                else if (_lbl.Name.Substring(0, 2).ToUpper() == "PK")  //Unit Object
                                {
                                    if (_seqNo < 50)
                                    {
                                        _lbl.Click += new EventHandler(Dense_Click);
                                    }
                                    else
                                    {
                                        _lbl.Click += new EventHandler(EQ_Click);
                                    }
                                }
                                break;
                        }
                    }
                    #endregion

                    #region csShape
                    foreach (csShape _shp in eq.Controls.OfType<csShape>())
                    {
                        switch (_shp.Name.ToUpper())
                        {
                            case "EQSTATUS":
                                _shp.Click += new EventHandler(EQ_Click);
                                _shp.Tag = _localNo;
                                _shp.BackColor = Color.DarkGray;
                                break;


                            default:
                                break;
                        }
                    }
                    #endregion

                    #region Set Parent
                    foreach (csPictureBox _pic in _hasParentPic)
                    {
                        string _parent = (string)_pic.PropertyData.GetPropertyVale("ParentName");

                        if (_parent != string.Empty)
                        {
                            object _parentObj = eq.Controls.Find(_parent, true).First();
                            if (_parentObj != null)
                            {
                                switch (_parentObj.GetType().Name)
                                {
                                    case "csPictureBox":
                                        csPictureBox _parentPic = (csPictureBox)_parentObj;
                                        _pic.Parent = _parentPic;
                                        _pic.Location = new Point(_pic.Location.X - _parentPic.Location.X, _pic.Location.Y - _parentPic.Location.Y);
                                        _pic.BackColor = Color.Transparent;
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }

                    }

                    #endregion
                }
                #endregion

                HandleControl("L3");
                DataSetting_EQP();
                SetPanel_Visible(flpEQP);

                // SetPanel_Visible(flpLine);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", "", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void SetPanel_Visible(FlowLayoutPanel _pnl)
        {
            foreach (Control _ctrl in pnlStatusInfo.Controls)
            {
                if (_ctrl.GetType().Name == "FlowLayoutPanel")
                {
                    if (((FlowLayoutPanel)_ctrl).Name == _pnl.Name) _ctrl.Visible = true;
                    else _ctrl.Visible = false;
                }
            }
        }

        private void Pipe_Click(object sender, EventArgs e)
        {
            try
            {
                Control pipe = ((PictureBox)sender).Parent;

                var _disVar = OPIAp.Dic_Pipe.Values.Where(r => r.IsDisplay == true);

                //I020003000101
                if (pipe.Name.Length != 13)
                {
                    ShowMessage(this, lblCaption.Text, "", "Interface key is error, Please check Layout.xml", MessageBoxIcon.Error);
                    return;
                }

                if (OPIAp.Dic_Pipe.ContainsKey(pipe.Name))
                {
                    if (OPIAp.Dic_Pipe[pipe.Name].IsDisplay == true)
                    {
                        var _var = Application.OpenForms.Cast<Form>().Where(x => x.Name == pipe.Name).FirstOrDefault();

                        if (_var == null) return;

                        FormInterface _frm = (FormInterface)_var;

                        _frm.TopMost = true;

                        return;
                    }
                    else
                    {
                        if (_disVar.Count() < 2)
                        {
                            FormInterface _interface = new FormInterface(pipe.Name);

                            _interface.Name = pipe.Name;

                            _interface.Show();

                            _interface.TopMost = true;

                            OPIAp.Dic_Pipe[pipe.Name].IsDisplay = true;
                            OPIAp.Dic_Pipe[pipe.Name].IsReply = true;
                            return;
                        }
                        else
                        {
                            ShowMessage(this, lblCaption.Text, "", "Too Much Link Signal Form", MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else
                {
                    if (_disVar.Count() < 2)
                    {
                        #region New Pipe
                        Interface _if = new Interface();

                        _if.PipeKey = pipe.Name;
                        _if.UpstreamNodeNo = "L" + int.Parse(pipe.Name.Substring(1, 2));
                        _if.UpstreamUnitNo = pipe.Name.Substring(3, 2);
                        _if.DownstreamNodeNo = "L" + int.Parse(pipe.Name.Substring(5, 2));
                        _if.DownstreamUnitNo = pipe.Name.Substring(7, 2);

                        _if.UpstreamSeqNo = pipe.Name.Substring(9, 2);
                        _if.DownstreamSeqNo = pipe.Name.Substring(11, 2);

                        LinkSignalType linkSignalType = OPIAp.Lst_LinkSignal_Type.Where(x => x.UpStreamLocalNo == _if.UpstreamNodeNo && x.DownStreamLocalNo == _if.DownstreamNodeNo && x.SeqNo == _if.UpstreamSeqNo + _if.DownstreamSeqNo).FirstOrDefault();

                        _if.PathPosition = linkSignalType.PathPosition.Trim();
                        _if.FlowType = linkSignalType.FlowType.Trim();
                        _if.TrxMatch = linkSignalType.TrxMatch;

                        OPIAp.Dic_Pipe.Add(pipe.Name, _if);

                        FormInterface _interface = new FormInterface(pipe.Name);

                        _interface.Name = pipe.Name;

                        _interface.Show();

                        _interface.TopMost = true;
                        #endregion
                    }
                    else
                    {
                        ShowMessage(this, lblCaption.Text, "", "Too Much Link Signal Form", MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void pnlLayout_Click(object sender, EventArgs e)
        {
            try
            {
                DataSetting_Line();

                // SetPanel_Visible(flpLine);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            try
            {
                string _unitKey = string.Empty;
                string _err = string.Empty;

                Control _ctrl = (Control)sender;

                if (_ctrl.GetType().Name.ToString() == "csPictureBox")
                {
                    csPictureBox _pic = (csPictureBox)_ctrl;

                    // Key: NODENO(3) + UNITNO(2)
                    _unitKey = _pic.Tag.ToString().PadRight(3, ' ') + _pic.PropertyData.myID.Substring(2, 2).Trim();
                }
                else if (_ctrl.GetType().Name.ToString() == "csLabel")
                {
                    csLabel _lbl = (csLabel)_ctrl;

                    // Key: NODENO(3) + UNITNO(2)
                    _unitKey = _lbl.Tag.ToString().PadRight(3, ' ') + _lbl.PropertyData.myID.Substring(2, 2).Trim();
                }

                if (OPIAp.Dic_Unit.ContainsKey(_unitKey))
                {
                    unit = ObjectManager.UnitManager.GetUnit(_unitKey.Substring(0, 3).Trim(), int.Parse(_unitKey.Substring(3, 2)));



                    #region Set Unit info for 左側區塊
                    txUnitID.Text = unit.Data.UNITID == null ? "" : unit.Data.UNITID;
                    txtUnitNo.Text = unit.Data.UNITNO.ToString() == null ? "" : unit.Data.UNITNO.ToString();

                    #endregion

                    GetJobDatatodgvJobData(_unitKey.Substring(0, 3).Trim(), _unitKey.Substring(3, 2));
                }
                else
                {
                    unit = null;
                }

                DataSetting_Unit();

                SetPanel_Visible(flpUnit);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void EQ_Click(object sender, EventArgs e)
        {
            try
            {
                string _err = string.Empty;
                string _localNo = string.Empty;

                switch (sender.GetType().Name)
                {
                    case "csLabel":
                        Label _lbl = (Label)sender;
                        _localNo = _lbl.Tag.ToString().Trim();//記錄選取的NodeNo
                        break;

                    case "csShape":
                        csShape _shp = (csShape)sender;
                        _localNo = _shp.Tag.ToString().Trim();//記錄選取的NodeNo
                        break;

                    case "csPictureBox":

                        csPictureBox _pic = (csPictureBox)sender;
                        _localNo = _pic.Tag.ToString().Trim();
                        break;

                    default:
                        break;
                }


                if (_localNo != string.Empty)
                {
                    eqp = ObjectManager.EquipmentManager.GetEQP(_localNo);
                    if (!OPIAp.Dic_Node.ContainsKey(_localNo) && !OPIAp.Dic_Node.ContainsKey(eqp.Data.ATTRIBUTE))
                    {
                        eqp = null;

                        SetPanel_Visible(flpEQP);

                        return;
                    }



                    GetJobDatatodgvJobData(_localNo, "");

                    #region Set EQ info for 左側區塊

                    #region 特殊欄位顯示


                    #region  ReportMode = HSMS顯示 SECS status
                    //if (OPIAp.Dic_Node[_localNo].ReportMode.Contains("HSMS"))
                    //{
                    //    flpHSMS.Visible = true;
                    //}
                    //else
                    //{
                    //    flpHSMS.Visible = false;
                    //}

                    #endregion

                    #region Auto Control Mode

                    // txtAutoControlMode.Visible = OPIAp.CurLine.FabType == "CF" && OPIAp.Dic_Node.ContainsKey(_localNo) && _localNo == "L2" && OPIAp.CurLine.LineType != "F1SOR" && OPIAp.CurLine.LineType != "F1MSC" ? true : false;
                    //   labAutoControlMode.Visible = OPIAp.CurLine.FabType == "CF" && OPIAp.Dic_Node.ContainsKey(_localNo) && _localNo == "L2" && OPIAp.CurLine.LineType != "F1SOR" && OPIAp.CurLine.LineType != "F1MSC" ? true : false;

                    #endregion

                    #region 特殊欄位顯示 -- run mode
                    //lblRunMode.Visible = OPIAp.Dic_Node[_localNo].UseRunMode == "Y" || OPIAp.Dic_Node[_localNo].UseRunMode == "R";
                    //txtRunMode.Visible = OPIAp.Dic_Node[_localNo].UseRunMode == "Y" || OPIAp.Dic_Node[_localNo].UseRunMode == "R";
                    #endregion

                    #region 特殊欄位顯示 -- indexer mode
                    //lblEQIndexerMode.Visible = OPIAp.Dic_Node[_localNo].UseIndexerMode;
                    //txtEQIndexerMode.Visible = OPIAp.Dic_Node[_localNo].UseIndexerMode;
                    #endregion

                    #region 特殊栏位 -- ISP Line Buffer Mode/Pre Sputter Mode

                    //txtBufferMode.Visible = OPIAp.CurLine.LineType == "F1ISP" && OPIAp.Dic_Node.ContainsKey(_localNo) && _localNo == "L3" ? true : false;
                    //lblBufferMode.Visible = OPIAp.CurLine.LineType == "F1ISP" && OPIAp.Dic_Node.ContainsKey(_localNo) && _localNo == "L3" ? true : false;
                    //txtPreSputterMode.Visible = OPIAp.CurLine.LineType == "F1ISP" && OPIAp.Dic_Node.ContainsKey(_localNo) && _localNo == "L5" ? true : false;
                    //lblPreSputterMode.Visible = OPIAp.CurLine.LineType == "F1ISP" && OPIAp.Dic_Node.ContainsKey(_localNo) && _localNo == "L5" ? true : false;

                    #endregion

                    #region 显示Dispatch

                    //labDispachMode1.Visible = false;
                    //txtDispatchMode1.Visible = false;
                    //labDispachMode2.Visible = false;
                    //txtDispatchMode2.Visible = false;
                    //labDispachMode3.Visible = false;
                    //txtDispatchMode3.Visible = false;
                    //foreach (var _dis in CurNode.Dispatches)
                    //{
                    //    if (_dis == null) continue;

                    //    if (_dis.DISPATCHNO == "01")
                    //    {
                    //        labDispachMode1.Visible = true;
                    //        txtDispatchMode1.Visible = true;
                    //        txtDispatchMode1.Text = _dis.Status.ToString();
                    //    }
                    //    if (_dis.DISPATCHNO == "02")
                    //    {
                    //        labDispachMode2.Visible = true;
                    //        txtDispatchMode2.Visible = true;
                    //        txtDispatchMode2.Text = _dis.Status.ToString();
                    //    }
                    //    if (_dis.DISPATCHNO == "03")
                    //    {
                    //        labDispachMode3.Visible = true;
                    //        txtDispatchMode3.Visible = true;
                    //        txtDispatchMode3.Text = _dis.Status.ToString();
                    //    }
                    //}

                    #endregion

                    #region Array 显示Farce Clear Out
                    //if (FormMainMDI.G_OPIAp.CurLine.LineType == "A1PHL" || FormMainMDI.G_OPIAp.CurLine.LineType == "A1CVD" ||
                    //            FormMainMDI.G_OPIAp.CurLine.LineType == "A1MSP" || FormMainMDI.G_OPIAp.CurLine.LineType == "A1ISP" ||
                    //            FormMainMDI.G_OPIAp.CurLine.LineType == "A1MSP" || FormMainMDI.G_OPIAp.CurLine.LineType == "A1UPK" ||
                    //            FormMainMDI.G_OPIAp.CurLine.LineType == "A1DET" || FormMainMDI.G_OPIAp.CurLine.LineType == "A1WET" ||
                    //            FormMainMDI.G_OPIAp.CurLine.LineType == "A1STR")
                    //    pnlForceClearOut.Visible = true;
                    //else pnlForceClearOut.Visible = false;


                    #endregion

                    #region Turn Tanle 是否要显示
                    // labTurnTable.Visible = OPIAp.Dic_Node[_localNo].TurnTableMode == eTurnTable.Unused ? false : true;
                    // txtTurnTable.Visible = OPIAp.Dic_Node[_localNo].TurnTableMode == eTurnTable.Unused ? false : true;
                    #endregion
                    txtEQPID.Text = eqp.Data.NODEID;
                    txtEQPName.Text = eqp.Data.NODENAME;

                    #endregion
                    #endregion

                }
                else eqp = null;

                HandleControl(_localNo);

                DataSetting_EQP();

                SetPanel_Visible(flpEQP);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void Port_Click(object sender, EventArgs e)
        {
            try
            {
                csPictureBox _pic = (csPictureBox)sender;
                string strPortKey;
                if (Convert.ToInt32(_pic.PropertyData.myID.Substring(2, 1)) > 0) strPortKey = _pic.PropertyData.myID.Substring(2, 2);

                else strPortKey = _pic.PropertyData.myID.Substring(3, 1);
                string strNodeNo = _pic.Tag.ToString().PadRight(3, ' ');

                if (OPIAp.Dic_Port.ContainsKey(strNodeNo + strPortKey))
                {
                    CurPort = OPIAp.Dic_Port[strNodeNo + strPortKey];

                    #region Send to BC CSTPortStatusRequest
                    //SendtoBC_CSTPortStatusRequest(CurPort.NodeNo, CurPort.PortNo.ToString().PadLeft(2, '0')); //disable by sy.wu 2016/07/29

                    //SendtoBC_JobDataCategoryRequest(CurPort.NodeNo, "00", CurPort.PortNo.ToString().PadLeft(2, '0'));

                    #endregion

                    #region Set Port info for 左側區塊

                    //   txtPortEQPID.Text = CurPort.NodeID == null ? "" : CurPort.NodeID;

                    // txtPortID.Text = CurPort.PortNo.ToString().PadLeft(2, '0') == null ? "" : CurPort.PortID.ToString();

                    #region 判斷有兩條lineid 時再顯示line id
                    if (OPIAp.CurLine.LineID2 == string.Empty)
                    {
                        //   lblPortLineID.Visible = false;
                        //   txtPortLineID.Visible = false;
                    }
                    else
                    {
                        //  lblPortLineID.Visible = true;
                        //   txtPortLineID.Visible = true;
                    }
                    #endregion

                    #region 只有CELL 才有CST Setting Code
                    //if (FormMainMDI.G_OPIAp.CurLine.FabType == "CELL")
                    //{
                    //    lblCSTSettingCode.Visible = true;
                    //    txtCSTSettingCode.Visible = true;
                    //}
                    //else
                    //{
                    //    lblCSTSettingCode.Visible = false;
                    //    txtCSTSettingCode.Visible = false;
                    //}
                    #endregion

                    #region CCGAP Line 顯示Port Assignment
                    //if (OPIAp.CurLine.LineType == "GAP")
                    //{
                    //    lblCSTMAXSLOTCOUNT.Visible = true;
                    //    txtCSTMaxSlotCount.Visible = true;
                    //}
                    //else
                    //{
                    //    lblCSTMAXSLOTCOUNT.Visible = false;
                    //    txtCSTMaxSlotCount.Visible = false;
                    //}
                    #endregion

                    #region SortGrade
                    //if (OPIAp.CurLine.LineType == "A1SOR" || OPIAp.CurLine.LineType == "F1SOR")
                    //{
                    //    labSortGrade.Visible = true;
                    //    txtSortGrade.Visible = true;
                    //}
                    //else
                    //{
                    //    labSortGrade.Visible = false;
                    //    txtSortGrade.Visible = false;
                    //}

                    #endregion

                    #endregion
                }
                else CurPort = null;

                //   DataSetting_Port();

                //   SetPanel_Visible(flpPort);

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void Dense_Click(object sender, EventArgs e)
        {
            try
            {
                string strDenseKey = string.Empty;
                string strNodeNo = string.Empty;

                if (sender.GetType().Name == "csLabel")
                {
                    csLabel _lbl = (csLabel)sender;

                    strDenseKey = _lbl.PropertyData.myID;
                    strNodeNo = _lbl.Tag.ToString().PadRight(3, ' ');
                }
                else
                {
                    csPictureBox _pic = (csPictureBox)sender;

                    strDenseKey = _pic.PropertyData.myID;
                    strNodeNo = _pic.Tag.ToString().PadRight(3, ' ');
                }

                if (OPIAp.Dic_Dense.ContainsKey(strNodeNo + strDenseKey.Substring(2, 2)))
                {
                    CurDense = OPIAp.Dic_Dense[strNodeNo + strDenseKey.Substring(2, 2)];

                    #region Send to BC DenseStatusRequest

                    // SendtoBC_DenseStatusRequest(CurDense.NodeNo, CurDense.PortNo);

                    #endregion

                    #region Set Dense info for 左側區塊

                    //txtDenseEQPID.Text = CurDense.NodeID == null ? "" : CurDense.NodeID;

                    //txtDensePortID.Text = CurDense.PortID;

                    //txtDensePortEnable.Text = CurDense.PortEnable.ToString();

                    //txtDensePackingMode.Text = CurDense.PackingMode.ToString();

                    //txtDenseBoxID01.Text = CurDense.BoxID01.ToString();

                    //txtDenseBoxID02.Text = CurDense.BoxID02.ToString();

                    //txtDenseSource.Text = CurDense.UnpackSource.ToString();

                    //txtPaperBoxID.Text = CurDense.PaperBoxID.ToString();

                    //txtBoxType.Text = CurDense.BoxType.ToString();

                    //txtDenseRequest.Text = CurDense.DenseDataRequest ? "ON" : "OFF";
                    #endregion

                }
                else CurDense = null;

                //  DataSetting_Dense();

                //  SetPanel_Visible(flpDense);

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void VCR_Click(object sender, EventArgs e)
        {
            try
            {
                csPictureBox _pic = (csPictureBox)sender;

                if (_pic.Tag != null)
                {
                    string _localNo = _pic.Tag.ToString().Trim();//記錄選取的NodeNo

                    if (!OPIAp.Dic_Node.ContainsKey(_localNo))
                    {
                        eqp = null;

                        SetPanel_Visible(flpVCR);

                        return;
                    }

                    eqp = ObjectManager.EquipmentManager.GetEQP(_localNo);

                    string _vcrNo = _pic.PropertyData.myID.Substring(2, 2);

                    CurVCR.Status = eqp.File.DCREnableMode[int.Parse(_vcrNo) - 1].ToString();
                    CurVCR.DCRNO = _vcrNo;
                    if (CurVCR != null)
                    {
                        #region Set VCR info for 左側區塊
                        txtVCRNo.Text = CurVCR.DCRNO;
                        txtVCRLocalNo.Text = eqp.Data.NODENO;
                        #endregion
                    }

                    GetJobDatatodgvJobData(_localNo, "");
                }
                else CurVCR = null;

                DataSetting_VCR();

                SetPanel_Visible(flpVCR);

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        //private void BFSlotInfo_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        csPictureBox _pic = (csPictureBox)sender;

        //        if (_pic.Tag != null)
        //        {
        //            string _localNo = _pic.Tag.ToString();//记录选取的NodeNo

        //            if (!OPIAp.Dic_Node.ContainsKey(_localNo))
        //            {
        //                CurNode = null;

        //             //   SetPanel_Visible(flpBuffer);

        //                return;
        //            }

        //            CurNode = OPIAp.Dic_Node[_localNo];

        //            string _bufferNo = _pic.PropertyData.myID.Substring(2, 2);

        //            if (_bufferNo != null)
        //            {
        //                #region Send to BC EquipmentStatusRequest

        //            //    SendtoBC_BufferSlotIofoRequest(_localNo);

        //                #endregion

        //                #region Set Buffer info for 左側區塊
        //              //  txtLocalNo.Text = CurNode.NodeNo;
        //                #endregion
        //            }
        //        }

        //        //SetPanel_Visible(flpBuffer);

        //    }
        //    catch (Exception ex)
        //    {
        //        NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
        //        ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
        //    }
        //}

        private void DataSetting_Line()
        {
            try
            {

                string _indexerDesc = UniTools.GetEquipmentIndexerModeDesc(OPIAp.CurLine.IndexerMode.ToString());
                //
                //  if (txtIndexerMode.Text != _indexerDesc) txtIndexerMode.Text = _indexerDesc;

                //IndexerMode = 3:changer mode
                //if (OPIAp.CurLine.IndexerMode == 3)
                //{
                //    if (txtPlanStatus.Text != OPIAp.CurLine.ChangerPlanStatus.ToString()) txtPlanStatus.Text = OPIAp.CurLine.ChangerPlanStatus.ToString();
                //    if (txtPlanID.Text != OPIAp.CurLine.ChangerPlanID) txtPlanID.Text = OPIAp.CurLine.ChangerPlanID;
                //    flpChangerPlan.Visible = true;
                //}
                //else
                //{
                //    txtPlanStatus.Text = string.Empty;
                //    txtPlanID.Text = string.Empty;
                //    flpChangerPlan.Visible = false;
                //}

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void DataSetting_EQP()
        {
            try
            {
                eqp = ObjectManager.EquipmentManager.GetEQP("L3");
                if (eqp != null)
                {

                    #region Set node real data

                    txtEQPID.Text = eqp.Data.NODEID;
                    txtEQPName.Text = eqp.Data.NODENAME;


                    //EQP Status
                    if (txtCurrentStatus.Text != eqp.File.Status.ToString()) txtCurrentStatus.Text = eqp.File.Status.ToString();

                    if (eqp.File.EqpAlive == null)
                    {
                        if (txtEQPAlive.Text != string.Empty) txtEQPAlive.Text = string.Empty;
                    }
                    else
                    {
                        if (txtEQPAlive.Text != eqp.File.EqpAlive.ToString()) txtEQPAlive.Text = eqp.File.EqpAlive.ToString();
                    }
                    //CIM Mode
                    if (txtCimMode.Text != eqp.File.CIMMode.ToString()) txtCimMode.Text = eqp.File.CIMMode.ToString();


                    if (txtCurrentRecipeNo.Text != eqp.File.CurrentRecipeNo.ToString()) txtCurrentRecipeNo.Text = eqp.File.CurrentRecipeNo.ToString();


                    if (eqp.File.CurrentRecipeID == null)
                    {
                        if (txtCurrentRecipeID.Text != string.Empty) txtCurrentRecipeID.Text = string.Empty;
                    }
                    else
                    {
                        if (txtCurrentRecipeID.Text != eqp.File.CurrentRecipeID) txtCurrentRecipeID.Text = eqp.File.CurrentRecipeID;
                    }

                    ////Product Glass Count
                    //if (txtTotalGlassCount.Text != eqp.File.TotalHFGlassCount.ToString()) txtTotalGlassCount.Text = eqp.File.TotalHFGlassCount.ToString();

                    ////Total Glass Count
                    //if (txtProductGlassCount.Text != eqp.File.TotalTFTGlassCount.ToString()) txtProductGlassCount.Text = eqp.File.TotalTFTGlassCount.ToString();

                    ////Hisotry Glass Count
                    //if (txtHistoryGlassCount.Text != eqp.File.DummyGlassCount.ToString()) txtHistoryGlassCount.Text = eqp.File.DummyGlassCount.ToString();


                    txtGroupIndexMode.Text = "0";

                    //Equipment Operation Mode
                    if (txtAutoManual.Text != eqp.File.EquipmentOperationMode.ToString()) txtAutoManual.Text = eqp.File.EquipmentOperationMode.ToString();

                    //Auto Recipe Change
                    if (txtAutoRecipeChange.Text != eqp.File.AutoRecipeChangeMode.ToString()) txtAutoRecipeChange.Text = eqp.File.AutoRecipeChangeMode.ToString();

                    if (txtRecipeCheckMode.Text != eqp.File.RecipeCheckMode.ToString()) txtRecipeCheckMode.Text = eqp.File.RecipeCheckMode.ToString();

                    if (txtUpInlineMode.Text != eqp.File.UpInlineMode.ToString()) txtUpInlineMode.Text = eqp.File.UpInlineMode.ToString();

                    if (txtDownInlineMode.Text != eqp.File.DownInlineMode.ToString()) txtDownInlineMode.Text = eqp.File.DownInlineMode.ToString();


                    if (txtGlassCheckMode.Text != eqp.File.GlassCheckMode.ToString()) txtGlassCheckMode.Text = eqp.File.GlassCheckMode.ToString();

                    if (txtProductTypeMode.Text != eqp.File.ProductTypeCheckMode.ToString()) txtProductTypeMode.Text = eqp.File.ProductTypeCheckMode.ToString();

                    if (txtGroupIndexMode.Text != eqp.File.GroupIndexCheckMode.ToString()) txtGroupIndexMode.Text = eqp.File.GroupIndexCheckMode.ToString();

                    if (txtProductIDMode.Text != eqp.File.ProductIDCheckMode.ToString()) txtProductIDMode.Text = eqp.File.ProductIDCheckMode.ToString();

                    if (txtDuplicateMode.Text != eqp.File.JobDuplicateCheckMode.ToString()) txtDuplicateMode.Text = eqp.File.JobDuplicateCheckMode.ToString();

                    if (txtBCAlive.Text != eqp.File.BCAlive.ToString()) txtBCAlive.Text = eqp.File.BCAlive.ToString();

                    if (txtStopcom.Text != eqp.File.StopCommand.ToString()) txtStopcom.Text = eqp.File.StopCommand.ToString();
                    if (eqp.File.StopCommand == eBitResult.ON)
                    {
                        txtStopcom.BackColor = Color.Red;
                    }
                    else
                    {
                        txtStopcom.BackColor = txtDuplicateMode.BackColor;
                    }

                    if (txtTR01TransferEnable.Text != eqp.File.TR01TransferEnable.ToString()) txtTR01TransferEnable.Text = eqp.File.TR01TransferEnable.ToString();

                    if (txtTR02TransferEnable.Text != eqp.File.TR02TransferEnable.ToString()) txtTR02TransferEnable.Text = eqp.File.TR02TransferEnable.ToString();

                    //AlarmCode //當機台狀態顯示Down時才show出Alarm Code
                    if (eqp.File.Status.ToString() == "Down")
                    {
                        lblAlarmCode.Visible = true;
                        txtAlarmCode.Visible = true;
                        if (txtAlarmCode.Text != eqp.File.Alarm.ToString()) txtAlarmCode.Text = eqp.File.Alarm.ToString();
                    }
                    else
                    {
                        lblAlarmCode.Visible = false;
                        txtAlarmCode.Visible = false;
                    }
                    if (txtVCRMode.Text != eqp.File.VCRMode.ToString()) txtVCRMode.Text = eqp.File.VCRMode.ToString();

                    if (txtMaterialID.Text != eqp.File.CurrentMaterialID.Trim()) txtMaterialID.Text = eqp.File.CurrentMaterialID.Trim();

                    if (txtMaterialStatus.Text != eqp.File.CurrentMaterialStatus.ToString()) txtMaterialStatus.Text = eqp.File.CurrentMaterialStatus.ToString();

                    if (txtMaterialType.Text != eqp.File.CurrentMaterial.MaterialType.ToString()) txtMaterialType.Text = eqp.File.CurrentMaterial.MaterialType.ToString();

                    if (txtCurrentGlassSendToTR.Text != EquipmentService.CurrentGlassSendToTR.ToString()) txtCurrentGlassSendToTR.Text = EquipmentService.CurrentGlassSendToTR.ToString();

                    #region Run Mode
                    if (eqp.File.EquipmentRunMode == null)
                    {
                        if (txtRunMode.Text != string.Empty) txtRunMode.Text = string.Empty;
                    }
                    else
                    {
                        if (eqp.File.EquipmentRunMode == "1")
                        {
                            txtRunMode.Text = "Normal Mode";
                        }
                        else if (eqp.File.EquipmentRunMode == "2")
                        {
                            txtRunMode.Text = "Force Clean Mode";
                        }
                        else if (eqp.File.EquipmentRunMode == "3")
                        {
                            txtRunMode.Text = "Skip Mode";
                        }
                        else if (eqp.File.EquipmentRunMode == "4")
                        {
                            txtRunMode.Text = "Cold Run Mode";
                        }
                        else if (eqp.File.EquipmentRunMode == "5")
                        {
                            txtRunMode.Text = "Return Mode";
                        }
                        else
                        {
                            txtRunMode.Text = "Other Mode";
                        }

                        //  if (txtRunMode.Text != eqp.File.EquipmentRunMode) txtRunMode.Text = eqp.File.EquipmentRunMode;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    #region Set empty
                    txtEQPID.Text = string.Empty;
                    txtEQPName.Text = string.Empty;
                    txtCurrentStatus.Text = string.Empty;
                    txtCimMode.Text = string.Empty;
                    txtRunMode.Text = string.Empty;
                    txtCurrentRecipeNo.Text = "0";
                    txtCurrentRecipeID.Text = string.Empty;
                    //txtProductGlassCount.Text = "0";
                    //txtTotalGlassCount.Text = "0";
                    //txtHistoryGlassCount.Text = "0";
                    txtEQPAlive.Text = string.Empty;
                    txtAutoManual.Text = string.Empty;
                    txtGroupNumber.Text = "0";
                    //txtBufferMode.Text = string.Empty;
                    //txtPreSputterMode.Text = string.Empty;
                    //txtAutoControlMode.Text = string.Empty;
                    //txtTurnTable.Text = string.Empty;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }
        #region
        //private void DataSetting_Port()
        //{
        //    try
        //    {
        //        if (CurPort != null)
        //        {
        //            //Line id
        //            if (txtPortLineID.Text != CurPort.LineID.ToString()) txtPortLineID.Text = CurPort.LineID.ToString();

        //            // Port Glass Count
        //            if (txtGlassCountInCST.Text != CurPort.PortGlassCount.ToString()) txtGlassCountInCST.Text = CurPort.PortGlassCount.ToString();

        //            // Port Group No
        //            if (txtGroupNo.Text != CurPort.GroupNumber.ToString()) txtGroupNo.Text = CurPort.GroupNumber.ToString();

        //            //Cassette Port Status
        //            if (txtCSTPortStatus.Text != CurPort.CassettePortStatus.ToString()) txtCSTPortStatus.Text = (int)Enum.Parse(typeof(eCassettePortStatus), CurPort.CassettePortStatus.ToString()) + "_" + CurPort.CassettePortStatus.ToString();
        //            #region 添加CSTPort Status Tag
        //            switch (CurPort.CassettePortStatus.ToString())
        //            {
        //                case "Unused":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Unused");
        //                    break;
        //                case "LREQ":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Load Request");
        //                    break;
        //                case "LDCP":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Load Complete");
        //                    break;
        //                case "MPEN":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Mapping End");
        //                    break;
        //                case "WASC":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Wait for Start Command");
        //                    break;
        //                case "WAIT":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Wait for Run");
        //                    break;
        //                case "CAEN":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Cancel End");
        //                    break;
        //                case "INPR":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "In Process");
        //                    break;
        //                case "PREN":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Process End");
        //                    break;
        //                case "PPEN":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Partial Process End");
        //                    break;
        //                case "FREN":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Force End");
        //                    break;
        //                case "UREQ":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Unload Request");
        //                    break;
        //                case "UDCP":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Unload Complete");
        //                    break;
        //                case "DOWN":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Port Down");
        //                    break;
        //                case "PAUS":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Port Pause");
        //                    break;
        //                case "REMP":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Re-Mapping");
        //                    break;
        //                case "WFAE":
        //                    tlpTip.SetToolTip(txtCSTPortStatus, "Wait For Abort End");
        //                    break;
        //                default: tlpTip.SetToolTip(txtCSTPortStatus, "Unknown");
        //                    break;

        //            }
        //            #endregion

        //            //Port Type
        //            if (txtPortType.Text != CurPort.PortType.ToString()) txtPortType.Text = CurPort.PortType.ToString();

        //            // CST Type
        //            if (txtLoadingCassetteType.Text != CurPort.LoadingCassetteType.ToString()) txtLoadingCassetteType.Text = CurPort.LoadingCassetteType.ToString();
        //            #region Port Judge
        //            if (txtPortJudge.Text != CurPort.PortJudge.ToString()) txtPortJudge.Text = CurPort.PortJudge.ToString();
        //            #endregion

        //            #region Port Grade
        //            if (txtPortGrade.Text != CurPort.PortGrade.ToString()) txtPortGrade.Text = CurPort.PortGrade.ToString();
        //            #endregion

        //            #region SortGrade
        //            if (txtSortGrade.Text != CurPort.SortGrade.ToString()) txtSortGrade.Text = CurPort.SortGrade.ToString();
        //            #endregion

        //            #region Port Enable
        //            if (txtPortEnableMode.Text != CurPort.PortEnable.ToString()) txtPortEnableMode.Text = CurPort.PortEnable.ToString();
        //            #endregion

        //            #region CSTSettingCode
        //            if (CurPort.CassetteID == null)
        //            {
        //                if (txtCSTSettingCode.Text != string.Empty) txtCSTSettingCode.Text = string.Empty;
        //            }
        //            else
        //            {
        //                if (txtCSTSettingCode.Text != CurPort.CassetteSettingCode) txtCSTSettingCode.Text = CurPort.CassetteSettingCode;
        //            }

        //            #endregion

        //            #region Cassette ID
        //            if (CurPort.CassetteID == null)
        //            {
        //                if (txtCassetteID.Text != string.Empty) txtCassetteID.Text = string.Empty;
        //            }
        //            else
        //            {
        //                if (txtCassetteID.Text != CurPort.CassetteID) txtCassetteID.Text = CurPort.CassetteID;
        //            }
        //            #endregion

        //            #region Cassette Seq No
        //            if (CurPort.CassetteSeqNo == null)
        //            {
        //                if (CurPort.CassetteSeqNo != string.Empty) CurPort.CassetteSeqNo = string.Empty;
        //            }
        //            else
        //            {
        //                if (txtCassetteSeqNo.Text != CurPort.CassetteSeqNo) txtCassetteSeqNo.Text = CurPort.CassetteSeqNo;
        //            }
        //            #endregion

        //            #region Port Transfer Mode
        //            if (txtPortTransfer.Text != CurPort.PortTransfer.ToString()) txtPortTransfer.Text = CurPort.PortTransfer.ToString();
        //            #endregion

        //            #region Cassette Command下拉選單設定 -- cassette port status變更時重新設定下拉選單
        //            if ((cboCassetteCmd.Tag.ToString() != CurPort.CassettePortStatus.ToString()))
        //            {

        //                OPIAp.RunModeHaveChange = false;
        //                OPIAp.IndexerModeHaveChange = false;

        //                cboCassetteCmd.Tag = CurPort.CassettePortStatus.ToString();
        //                //‘0’：Unused
        //                //‘1’：Start
        //                //‘2’：Cancel
        //                //‘3’：Abort
        //                //‘4’：Pause
        //                //‘5’：Resume
        //                //‘6’：Re-Mapping
        //                //‘7’：Process End
        //                //‘8’：Mapping Data Download
        //                //‘9’：Wait For Abort End
        //                List<comboxInfo> _lstCSTCmd = new List<comboxInfo>();

        //                #region Cassettee status
        //                switch (CurPort.CassettePortStatus)
        //                {
        //                    case eCassettePortStatus.Unused: break;
        //                    //0
        //                    case eCassettePortStatus.LREQ:
        //                        //1

        //                        break;

        //                    case eCassettePortStatus.LDCP:
        //                        //2
        //                        _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "2", ITEM_NAME = "Cancel" });
        //                        break;


        //                    case eCassettePortStatus.MPEN:
        //                        //3

        //                        #region WaitingforCassetteData
        //                        _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "2", ITEM_NAME = "Cancel" });

        //                        break;
        //                        #endregion

        //                    case eCassettePortStatus.WASC:
        //                        //4

        //                        #region WaitingforStartCommand

        //                        _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "1", ITEM_NAME = "Start" });
        //                        _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "2", ITEM_NAME = "Cancel" });

        //                        break;
        //                        #endregion

        //                    case eCassettePortStatus.WAIT:
        //                        //5
        //                        #region WaitingforProcessing
        //                        _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "2", ITEM_NAME = "Cancel" });
        //                        break;
        //                        #endregion

        //                    case eCassettePortStatus.CAEN:
        //                        //6

        //                        break;

        //                    case eCassettePortStatus.INPR:
        //                        //7
        //                        #region InProcessing
        //                        _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "4", ITEM_NAME = "Pause" });
        //                        if (CurPort.PortType != ePortType.CommonPort)
        //                        {
        //                            _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "3", ITEM_NAME = "Abort" });
        //                        }
        //                        else
        //                        {
        //                            //if (FormMainMDI.G_OPIAp.IsAbort.Contains(FormMainMDI.G_OPIAp.LoginGroupID) == true)
        //                            //{
        //                            //    _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "3", ITEM_NAME = "Abort" });
        //                            //}
        //                            if (OPIAp.CurLine.IndexerMode == 2 || OPIAp.CurLine.IndexerMode == 3)
        //                            {
        //                                if (OPIAp.CurLine.IndexerMode == 2)
        //                                    _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "9", ITEM_NAME = "Wait For Abort End" });
        //                            }
        //                            else
        //                            {
        //                                _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "6", ITEM_NAME = "Re-Mapping" });
        //                                _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "9", ITEM_NAME = "Wait For Abort End" });
        //                            }
        //                        }
        //                        break;
        //                        #endregion

        //                    case eCassettePortStatus.PREN:

        //                        break;


        //                    case eCassettePortStatus.PAUS:
        //                        //14
        //                        #region ProcessPaused //disable by sy.wu 2016/07/29
        //                        _lstCSTCmd.Add(new comboxInfo { ITEM_ID = "5", ITEM_NAME = "Resume" });

        //                        break;
        //                        #endregion

        //                    case eCassettePortStatus.REMP:
        //                        //15
        //                        break;
        //                    case eCassettePortStatus.WFAE:
        //                        //16
        //                        break;
        //                    default: break;
        //                }
        //                #endregion

        //                cboCassetteCmd.Enabled = false;
        //                cboCassetteCmd.DataSource = _lstCSTCmd.ToList();
        //                cboCassetteCmd.DisplayMember = "ITEM_NAME";
        //                cboCassetteCmd.ValueMember = "ITEM_ID";
        //                cboCassetteCmd.SelectedIndex = -1;
        //                cboCassetteCmd.Enabled = true;
        //            }
        //            #endregion

        //            #region Port Start Button -- 只有wait edit狀態才能點選button
        //            if (CurPort.SubCassetteStatus != null)
        //            {
        //                switch (CurPort.SubCassetteStatus)
        //                {
        //                    case "WACSTEDIT":
        //                    case "WAREMAPEDIT":
        //                        if (!btnStart.Enabled) btnStart.Enabled = true;
        //                        btnStart.BackColor = (btnStart.BackColor == Color.Green ? Color.DimGray : Color.Green);
        //                        break;

        //                    default:
        //                        btnStart.BackColor = Color.DimGray;
        //                        if (btnStart.Enabled) btnStart.Enabled = false;
        //                        break;
        //                }

        //                if (btnStart.Tag.ToString() != CurPort.SubCassetteStatus)
        //                {
        //                    btnStart.Tag = CurPort.SubCassetteStatus;
        //                    ToolTip _tip = new ToolTip();
        //                    _tip.SetToolTip(btnStart, btnStart.Tag.ToString());
        //                }
        //            }
        //            else
        //            {
        //                btnStart.BackColor = Color.DimGray;
        //                btnStart.Enabled = false;
        //            }
        //            #endregion

        //            #region Cassette Port Status為 Waiting for Start Command時，send 按鈕閃爍
        //            if (CurPort.CassettePortStatus == eCassettePortStatus.WASC)
        //            {
        //                btnCassetteCmd.BackColor = (btnCassetteCmd.BackColor == Color.Green ? Color.DimGray : Color.Green);
        //            }
        //            else
        //            {
        //                btnCassetteCmd.BackColor = Color.DimGray;
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region Set empty
        //            txtPortID.Text = string.Empty;
        //            txtPortEQPID.Text = string.Empty;
        //            txtCSTSettingCode.Text = string.Empty;
        //            txtCassetteID.Text = string.Empty;
        //            txtCassetteSeqNo.Text = string.Empty;
        //            txtPortType.Text = string.Empty;
        //            txtCSTPortStatus.Text = string.Empty;
        //            txtLoadingCassetteType.Text = string.Empty;
        //            txtPortTransfer.Text = string.Empty;
        //            txtPortJudge.Text = string.Empty;
        //            txtPortEnableMode.Text = string.Empty;
        //            txtGlassCountInCST.Text = string.Empty;
        //            btnStart.Enabled = false;
        //            txtGlassCountInCST.Text = string.Empty;
        //            txtCSTMaxSlotCount.Text = string.Empty;
        //            txtPortCount.Text = string.Empty;
        //            txtCSTSettingCode.Text = string.Empty;
        //            txtPortGrade.Text = string.Empty;
        //            txtGroupNo.Text = string.Empty;
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
        //        ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
        //    }
        //}

        //private void DataSetting_Dense()
        //{
        //    try
        //    {
        //        if (CurDense == null)
        //        {
        //            #region Set empty
        //            txtDensePortEnable.Text = string.Empty;
        //            txtDensePackingMode.Text = string.Empty;
        //            txtDenseBoxID01.Text = string.Empty;
        //            txtDenseBoxID02.Text = string.Empty;
        //            txtDenseSource.Text = string.Empty;
        //            txtPaperBoxID.Text = string.Empty;
        //            txtBoxType.Text = string.Empty;
        //            txtDenseRequest.Text = string.Empty;
        //            btnDenseControl.Enabled = false;
        //            #endregion

        //            return;
        //        }

        //        #region Update Dense Info
        //        // Port Enable
        //        if (txtDensePortEnable.Text != CurDense.PortEnable.ToString()) txtDensePortEnable.Text = CurDense.PortEnable.ToString();

        //        // Dense Packing Mode
        //        if (txtDensePackingMode.Text != CurDense.PackingMode.ToString()) txtDensePackingMode.Text = CurDense.PackingMode.ToString();

        //        //Dense Box #01
        //        if (txtDenseBoxID01.Text != CurDense.BoxID01.ToString()) txtDenseBoxID01.Text = CurDense.BoxID01.ToString();

        //        //Dense Box #02
        //        if (txtDenseBoxID02.Text != CurDense.BoxID02.ToString()) txtDenseBoxID02.Text = CurDense.BoxID02.ToString();

        //        //Dense Source
        //        if (txtDenseSource.Text != CurDense.UnpackSource.ToString()) txtDenseSource.Text = CurDense.UnpackSource.ToString();

        //        //Paper Box ID
        //        if (txtPaperBoxID.Text != CurDense.PaperBoxID.ToString()) txtPaperBoxID.Text = CurDense.PaperBoxID.ToString();

        //        //Box Type
        //        if (txtBoxType.Text != CurDense.BoxType.ToString()) txtBoxType.Text = CurDense.BoxType.ToString();

        //        //Dense Request
        //        if (txtDenseRequest.Text != CurDense.DenseDataRequest.ToString()) txtDenseRequest.Text = CurDense.DenseDataRequest ? "ON" : "OFF";
        //        #endregion

        //        if (CurDense.DenseDataRequest)
        //        {
        //            btnDenseControl.BackColor = (btnDenseControl.BackColor == Color.Green ? Color.DimGray : Color.Green);
        //            if (!btnDenseControl.Enabled) btnDenseControl.Enabled = true;
        //        }
        //        else
        //        {
        //            btnDenseControl.BackColor = Color.DimGray;
        //            if (btnDenseControl.Enabled) btnDenseControl.Enabled = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
        //        ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
        //    }
        //}
        #endregion
        private void DataSetting_Unit()
        {
            try
            {
                if (unit != null)
                {
                    if (txtUnitStatus.Text != unit.File.Status.ToString()) txtUnitStatus.Text = unit.File.Status.ToString();

                    if (txtUnitAlarmCode.Text != unit.File.UnitAlram.ToString()) txtUnitAlarmCode.Text = unit.File.UnitAlram.ToString();

                    if (txtUnitTFTJobCnt.Text != unit.File.TotalTFTGlassCount.ToString()) txtUnitTFTJobCnt.Text = unit.File.TotalTFTGlassCount.ToString();

                    if (txtunitHFJobCnt.Text != unit.File.TotalHFGlassCount.ToString()) txtunitHFJobCnt.Text = unit.File.TotalHFGlassCount.ToString();

                    if (txtunitDummyCnt.Text != unit.File.DummyGlassCount.ToString()) txtunitDummyCnt.Text = unit.File.DummyGlassCount.ToString();

                    if (txtunitProductType.Text != unit.File.ProductType.ToString()) txtunitProductType.Text = unit.File.ProductType.ToString();


                    //if (txtUnitRunMode.Text != CurUnit.UnitRunMode.ToString()) txtUnitRunMode.Text = CurUnit.UnitRunMode.ToString();

                }
                else
                {
                    #region Set empty
                    txUnitID.Text = "";
                    txtUnitNo.Text = "";
                    txtUnitAlarmCode.Text = "0";
                    txtUnitStatus.Text = "";
                    txtUnitTFTJobCnt.Text = "0";
                    //   txtUnitRunMode.Text = string.Empty;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void DataSetting_VCR()
        {
            try
            {
                if (CurVCR != null)
                {
                    if (txtVCRStatus.Text != CurVCR.Status) txtVCRStatus.Text = CurVCR.Status;
                }
                else
                {
                    #region Set empty
                    txtVCRStatus.Text = string.Empty;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void HandleControl(string NodeNo)
        {
            if (hideContrlNames != null && hideContrlNames.Count > 0)//先將前次隱藏的顯示
            {
                foreach (string ctrlName in hideContrlNames)
                {
                    Control[] ctrls = this.Controls.Find(ctrlName, true);
                    if (ctrls.Length > 0) ctrls[0].Visible = true;
                }
                hideContrlNames.Clear();
            }

        }

        private void Refresh_LayoutInfo()
        {
            int _num = 0;
            string _localNo = string.Empty;
            string _unitNo = string.Empty;
            string _itemType = string.Empty;
            string _itemNo = string.Empty;
            string _itemKey = string.Empty;
            // Node _node = null;
            Equipment _eqp = null;
            // Unit _unit = null;
            Control _ctrl = null;
            int _totalCnt = 0;

            try
            {
                foreach (ucEQ _eq in pnlLayout.Controls.OfType<ucEQ>())
                {
                    int.TryParse(_eq.Name.Substring(1, 2), out _num);

                    _localNo = string.Format("L{0}", _num.ToString());

                    if (ObjectManager.EquipmentManager.GetEQP(_localNo) == null) continue;

                    _eqp = ObjectManager.EquipmentManager.GetEQP(_localNo);


                    #region EQP Information
                    #region EQP Status

                    _ctrl = _eq.Controls["EQSTATUS"];

                    if (_ctrl != null)
                    {
                        EQPStatusColor(_ctrl, _eqp.File.Status);

                        // 學名+俗名+Status(Run，Idle，STOP，Pause)
                        Tip.SetToolTip(_ctrl, _eqp.Data.NODENAME + "-" + _eqp.Data.NODEID + "-" + _eqp.File.Status.ToString());
                    }

                    #region 非unit 的picture背景與EQP Status相同
                    foreach (csPictureBox _picObj in _eq.Controls.OfType<csPictureBox>())
                    {
                        //若為Unit 編碼為九碼 EX:UNXX:RDXX
                        if (_picObj.Name.Length < 9)
                        {
                            EQPStatusColor(_picObj, _eqp.File.Status);
                        }
                    }
                    #endregion

                    #endregion

                    #region TotalCount

                    _totalCnt = _eqp.File.TotalTFTGlassCount;//修改只显示机台上报的数量

                    //_node.TotalCount = _totalCnt;

                    _ctrl = _eq.Controls["TotalCount"];

                    if (_ctrl != null) ((csLabel)_ctrl).Text = _eqp.File.TotalTFTGlassCount.ToString();

                    #endregion

                    #region CIM Mode

                    _ctrl = _eq.Controls["NodeName"];

                    if (_ctrl != null)
                    {
                        _ctrl.BackColor = (_eqp.File.CIMMode == eBitResult.ON ? Color.LawnGreen : Color.Red);

                    }
                    #endregion

                    #region Alarm

                    if (_eqp.File.Alarm == eBitResult.ON)
                    {
                        _eq.BackColor = _eq.BackColor == Color.Red ? Color.DarkGray : Color.Red;
                    }
                    else
                    {
                        if (_eq.BackColor != Color.DarkGray) _eq.BackColor = Color.DarkGray;
                    }

                    #endregion

                    #endregion

                    #region ucEQP 內的物件處理  - Unit & Port & VCR
                    foreach (Control _subCtrl in _eq.Controls)
                    {
                        switch (_subCtrl.GetType().Name.ToString())
                        {
                            case "csPictureBox":

                                #region csPictureBox
                                csPictureBox _pic = (csPictureBox)_subCtrl;

                                SetPictureImage(_eqp, _pic);

                                foreach (csPictureBox _subPic in _pic.Controls.OfType<csPictureBox>())
                                {
                                    SetPictureImage(_eqp, _subPic);
                                }
                                #endregion
                                break;

                            case "csLabel":

                                #region Unit
                                csLabel _lbl = (csLabel)_subCtrl;

                                if (_lbl.PropertyData.myID.Length < 4) continue;
                                if (_lbl.PropertyData.myID.Length != 4 && !_lbl.PropertyData.myID.Contains(":")) continue;
                                _itemType = _lbl.PropertyData.myID.Substring(0, 2);  //UN...
                                _itemNo = _lbl.PropertyData.myID.Substring(2, 2);

                                KZONE.Entity.Unit unit = ObjectManager.UnitManager.GetUnit(_eqp.Data.NODENO, int.Parse(_itemNo));
                                if (unit == null)
                                {
                                    throw new Exception(string.Format("CAN'T FIND UNIT_NO=[{0}] IN UNITENTITY!", _itemNo));
                                }

                                int.TryParse(_itemNo, out _num);

                                if (_itemType == "UN")
                                {
                                    #region UNXX
                                    if (_lbl.PropertyData.myID.Length == 4)
                                    {
                                        #region unit 的csLabel背景顯示
                                        if (unit.File.UnitAlram == eBitResult.ON)
                                        {
                                            Label _lblTmp = new Label();
                                            EQPStatusColor(_lblTmp, unit.File.Status);
                                            Color _color = _lblTmp.BackColor;

                                            _lbl.BackColor = _lbl.BackColor == Color.Silver ? _color : Color.Silver;
                                        }
                                        else EQPStatusColor(_lbl, unit.File.Status);
                                        Tip.SetToolTip(_lbl, string.Format("U{0}-{1}", unit.Data.UNITNO.ToString().PadLeft(2, '0'), unit.File.Status.ToString()));
                                        #endregion
                                    }
                                    else if (_lbl.PropertyData.myID.Contains(":"))
                                    {
                                        #region 非unit 的csLabel背景與EQP Status相同

                                        string[] _subUnit = _lbl.PropertyData.myID.Split(':');
                                        if (unit.Data.SUBUNIT.Split(':').ToList().Contains(_subUnit[1].Trim()))
                                        {

                                            EQPStatusColor(_lbl, unit.File.Status);

                                            // 學名+俗名+Status(Run，Idle，STOP，Pause)
                                            Tip.SetToolTip(_lbl, unit.Data.UNITNO + "-" + unit.File.Status.ToString());
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else if (_itemType == "MS")
                                {
                                    #region Material Status
                                    //if (_node.MaterialStatus)
                                    //{
                                    //    EQPStatusColor(_lbl, _node.EQPStatus);
                                    //    _lbl.ForeColor = _lbl.ForeColor == Color.Red ? Color.Black : Color.Red;
                                    //    _lbl.Visible = true;
                                    //}
                                    //else _lbl.Visible = false;
                                    #endregion
                                }
                                #endregion

                                break;

                            default: break;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void SetPictureImage(Equipment _node, csPictureBox _pic)
        {
            try
            {
                #region csPictureBox

                string _itemType = _pic.PropertyData.myID.Substring(0, 2);  //UN,RD,TO...
                string _itemNo = _pic.PropertyData.myID.Substring(2, 2);
                string[] _subUnitName = _pic.PropertyData.myID.Split(':');

                KZONE.Entity.Unit unit = ObjectManager.UnitManager.GetUnit(_node.Data.NODENO, int.Parse(_itemNo));
                if (unit == null)
                {
                    throw new Exception(string.Format("CAN'T FIND UNIT_NO=[{0}] IN UNITENTITY!", _itemNo));
                }

                int _portNo = 0;
                int.TryParse(_itemNo, out _portNo);
                string _itemNo_port = _portNo.ToString();

                string _itemKey = string.Empty;

                if (_pic.PropertyData.myID.Trim().Length == 4)
                {
                    switch (_itemType)
                    {

                        case "VR":

                            #region VCR
                            eEnableDisable _vcr = _node.File.DCREnableMode[int.Parse(_itemNo) - 1];

                            if (_vcr != null)
                            {
                                if (_vcr == eEnableDisable.Enable)
                                {
                                    if (_pic.BackgroundImage != Properties.Resources.Layout_VCROn) _pic.BackgroundImage = Properties.Resources.Layout_VCROn;
                                }
                                else
                                {
                                    if (_pic.BackgroundImage != Properties.Resources.Layout_VCROff) _pic.BackgroundImage = Properties.Resources.Layout_VCROff;
                                }
                            }

                            break;

                        #endregion


                        #endregion
                        default:
                            break;
                    }
                }
                else
                {
                    #region subUnit
                    if (_itemType == "UN")
                    {
                        _itemKey = _subUnitName[1];
                        if (unit.Data.SUBUNIT.Split(':').ToList().Contains(_itemKey.Trim()))
                        {

                            EQPStatusColor(_pic, unit.File.Status);

                            Tip.SetToolTip(_pic, string.Format("U{0}-{1}", unit.Data.UNITNO.ToString().PadLeft(2, '0'), unit.File.Status.ToString()));
                        }
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void PortType(Control ctrObject, ePortType PortType)
        {
            try
            {
                switch (PortType)
                {
                    case ePortType.LoadingPort:

                        if (ctrObject != null) ctrObject.Text = "LD";

                        break;

                    case ePortType.UnloadingPort:

                        if (ctrObject != null) ctrObject.Text = "UD";

                        break;

                    case ePortType.CommonPort:

                        if (ctrObject != null) ctrObject.Text = "CO";

                        break;

                    default:

                        if (ctrObject != null) ctrObject.Text = "UN";
                        break;
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void EQPStatusColor(Control ctrObject, eEQPStatus CurrentStatus)
        {
            try
            {
                switch (CurrentStatus)
                {
                    case eEQPStatus.Idle:

                        if (ctrObject.BackColor != Color.Gold) ctrObject.BackColor = Color.Gold;

                        break;

                    case eEQPStatus.Pause:

                        if (ctrObject.BackColor != Color.Cyan) ctrObject.BackColor = Color.Cyan;

                        break;

                    case eEQPStatus.Run:

                        if (ctrObject.BackColor != Color.YellowGreen) ctrObject.BackColor = Color.YellowGreen;

                        break;

                    case eEQPStatus.Initial:

                        if (ctrObject.BackColor != Color.MediumPurple) ctrObject.BackColor = Color.MediumPurple;

                        break;

                    case eEQPStatus.Down:

                        if (ctrObject.BackColor != Color.IndianRed) ctrObject.BackColor = Color.IndianRed;

                        break;

                    default:

                        if (ctrObject.BackColor != Color.Gray) ctrObject.BackColor = Color.Gray;
                        break;
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void dgvJobData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvJobData.CurrentRow != null)
                {
                    if (dgvJobData.Columns[e.ColumnIndex].Name == "colDetail")
                    {
                        #region FormJobDataDetail
                        string _cstSeqNo = dgvJobData.CurrentRow.Cells[colCstSeqNo.Name].Value.ToString();
                        string _jobSeqNo = dgvJobData.CurrentRow.Cells[colCstSlotNo.Name].Value.ToString();
                        string _glassID = dgvJobData.CurrentRow.Cells[colGlassID.Name].Value.ToString();

                        if (_glassID == string.Empty && _cstSeqNo == string.Empty && _jobSeqNo == string.Empty)
                        {
                            ShowMessage(this, this.lblCaption.Text, "", "Cassette Seq No、Cassette Slot No、Glass ID must be Required！", MessageBoxIcon.Warning);
                            return;
                        }

                        FormJobDataDetail _frm = new FormJobDataDetail(_glassID, _cstSeqNo, _jobSeqNo);
                        _frm.TopMost = true;
                        _frm.ShowDialog(this);

                        _frm.Dispose();
                        #endregion
                    }
                    else if (dgvJobData.Columns[e.ColumnIndex].Name == "colRoute")
                    {
                        #region FormRBRouteInfo
                        string _cstSeqNo = dgvJobData.CurrentRow.Cells[colCstSeqNo.Name].Value.ToString();
                        string _jobSeqNo = dgvJobData.CurrentRow.Cells[colCstSlotNo.Name].Value.ToString();

                        if (_cstSeqNo == string.Empty || _jobSeqNo == string.Empty)
                        {
                            ShowMessage(this, this.lblCaption.Text, "", "Cassette Seq No and Cassette Slot No must be Required！", MessageBoxIcon.Warning);
                            return;
                        }

                        //FormRBRouteInfo _frm = new FormRBRouteInfo(_cstSeqNo, _jobSeqNo) { TopMost = true };
                        //_frm.ShowDialog();
                        //if (_frm != null) _frm.Dispose();
                        #endregion
                    }
                    else if (dgvJobData.Columns[e.ColumnIndex].Name == "colStopReason")
                    {
                        #region FormRobotStopReason
                        string _cstSeqNo = dgvJobData.CurrentRow.Cells[colCstSeqNo.Name].Value.ToString();
                        string _jobSeqNo = dgvJobData.CurrentRow.Cells[colCstSlotNo.Name].Value.ToString();

                        if (_cstSeqNo == string.Empty || _jobSeqNo == string.Empty)
                        {
                            ShowMessage(this, this.lblCaption.Text, "", "Cassette Seq No and Cassette Slot No must be Required！", MessageBoxIcon.Warning);
                            return;
                        }

                        //FormRobotStopReason _frm = new FormRobotStopReason(_cstSeqNo, _jobSeqNo) { TopMost = true };
                        //_frm.ShowDialog();
                        //if (_frm != null) _frm.Dispose();
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string CurPortMESMode = string.Empty;

                #region Check Data
                if (CurPort.PortGlassCount > CurPort.MaxCount)
                {
                    ShowMessage(this, lblCaption.Text, "", string.Format("Port Glass Count [{0}] > Port Max Count [{1}]", CurPort.PortGlassCount.ToString(), CurPort.MaxCount.ToString()), MessageBoxIcon.Error);
                    return;
                }

                if (CurPort.LineID == OPIAp.CurLine.LineID)
                {
                    if (OPIAp.CurLine.MesControlMode == null)
                    {
                        ShowMessage(this, lblCaption.Text, "", "MES Control is null", MessageBoxIcon.Error);
                        return;
                    }

                    CurPortMESMode = OPIAp.CurLine.MesControlMode;
                }
                else if (CurPort.LineID == OPIAp.CurLine.LineID2)
                {
                    if (OPIAp.CurLine.MesControlMode2 == null)
                    {
                        ShowMessage(this, lblCaption.Text, "", "MES Control is null", MessageBoxIcon.Error);
                        return;
                    }

                    CurPortMESMode = OPIAp.CurLine.MesControlMode2;
                }
                else
                {
                    CurPortMESMode = OPIAp.CurLine.MesControlMode;
                }
                #endregion

                if (CurPortMESMode == "OFFLINE")
                {
                    //FormMainMDI.FrmCassetteOperation_Offline = new FormCassetteControl_Offline();
                    //FormMainMDI.FrmCassetteOperation_Offline.curPort = CurPort;
                    //FormMainMDI.FrmCassetteOperation_Offline.MesControlMode = CurPortMESMode.ToUpper();
                    //FormMainMDI.FrmCassetteOperation_Offline.ShowDialog();

                }
                else if (CurPortMESMode.ToUpper() == "LOCAL")
                {
                    //FormMainMDI.FrmCassetteOperation_Local = new FormCassetteControl_Local();
                    //FormMainMDI.FrmCassetteOperation_Local.curPort = CurPort;
                    //FormMainMDI.FrmCassetteOperation_Local.MesControlMode = CurPortMESMode.ToUpper();
                    //FormMainMDI.FrmCassetteOperation_Local.ShowDialog();
                }
                else
                {
                    ShowMessage(this, lblCaption.Text, "", string.Format("MES Mode [{0}] is error", CurPortMESMode), MessageBoxIcon.Error);
                    return;
                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnDenseControl_Click(object sender, EventArgs e)
        {
            try
            {
                //if (OPIAp.CurLine.MesControlMode.ToUpper() == "OFFLINE")
                //{
                //    FormMainMDI.FrmDenseControl_Offline = new FormDenseControl_Offline();
                //    FormMainMDI.FrmDenseControl_Offline.CurDense = CurDense;
                //    FormMainMDI.FrmDenseControl_Offline.ShowDialog();
                //}
                //else if (OPIAp.CurLine.MesControlMode.ToUpper() == "LOCAL")
                //{
                //    FormMainMDI.FrmDenseControl_Local = new FormDenseControl_Local();
                //    FormMainMDI.FrmDenseControl_Local.CurDense = CurDense;
                //    FormMainMDI.FrmDenseControl_Local.ShowDialog();
                //}

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }


        private void btnSlot_Click(object sender, EventArgs e)
        {
            //    FormMainMDI.FrmPortStatus.Visible = true;
            //    FormMainMDI.FrmPortStatus.tmrBaseRefresh.Enabled = true;
            //    FormMainMDI.CurForm = FormMainMDI.FrmPortStatus;
            //    FormMainMDI.FrmPortStatus.BringToFront();
        }

        private void btnSlotInformation_Click(object sender, EventArgs e)
        {
            //FormMainMDI.FrmBufferSlotInfoReport.Visible = true;
            //FormMainMDI.FrmBufferSlotInfoReport.tmrBaseRefresh.Enabled = true;
            //FormMainMDI.CurForm = FormMainMDI.FrmBufferSlotInfoReport;
            //FormMainMDI.FrmBufferSlotInfoReport.BringToFront();
        }

        private void btnBufferControl_Click(object sender, EventArgs e)
        {
            //FormMainMDI.FrmBufferControl.Visible = true;
            //FormMainMDI.FrmBufferControl.tmrBaseRefresh.Enabled = true;
            //FormMainMDI.CurForm = FormMainMDI.FrmBufferControl;
            //FormMainMDI.FrmBufferControl.BringToFront();
        }

        private void btnPlan_Click(object sender, EventArgs e)
        {
            // new FormChangerPlanDetail().ShowDialog();
        }

        private void bgwRefresh_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (tlpBase.InvokeRequired)
                    {
                        #region InvokeRequired
                        this.BeginInvoke(new MethodInvoker(
                           delegate
                           {
                               //更新畫面
                               Refresh_LayoutInfo();

                               //更新JobData to DataGrid

                               if (flpEQP.Visible)
                                   DataSetting_EQP();
                               //else if (flpPort.Visible)
                               //    DataSetting_Port();
                               else if (flpUnit.Visible)
                                   DataSetting_Unit();
                               else if (flpVCR.Visible)
                                   DataSetting_VCR();
                               //else if (flpDense.Visible)
                               //    DataSetting_Dense();

                           }));
                        #endregion
                    }
                    else
                    {
                        #region
                        //更新畫面
                        Refresh_LayoutInfo();

                        //更新JobData to DataGrid

                        if (flpEQP.Visible)
                            DataSetting_EQP();
                        //else if (flpPort.Visible)
                        //    DataSetting_Port();
                        else if (flpUnit.Visible)
                            DataSetting_Unit();
                        else if (flpVCR.Visible)
                            DataSetting_VCR();
                        //else if (flpDense.Visible)
                        //    DataSetting_Dense();

                        #endregion
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
                }
            }
        }

        private void GetJobDatatodgvJobData(string localNo, string unitNo)
        {
            try
            {
                IList<Job> jobs = ObjectManager.JobManager.GetJobs();
                List<Job> query = jobs.ToList<Job>();
                if (!string.IsNullOrEmpty(unitNo) && unitNo != "")
                {
                    if (!string.IsNullOrEmpty(localNo) && localNo != "00")
                        query = query.Where(j => j.CurrentEQPNo == localNo).ToList<Job>();
                    if (!string.IsNullOrEmpty(unitNo) && unitNo != "00")
                        query = query.Where(j => j.CurrentUnitNo == int.Parse(unitNo)).ToList<Job>();
                }
                else
                {

                    if (!string.IsNullOrEmpty(localNo) && localNo != "00")
                        query = query.Where(j => j.CurrentEQPNo == localNo).ToList<Job>();
                }

                dgvJobData.Rows.Clear();
                if (query.Count != 0)
                {
                    Entity.Line line = ObjectManager.LineManager.GetLine(eqp.Data.LINEID);
                    foreach (Job jobData in query)
                    {
                        string kzonePPID = string.Empty;

                        if (line.Data.FABTYPE == "ARRAY")
                        {
                            kzonePPID = jobData.PPID.Substring(26, 26);
                        }
                        else if (line.Data.FABTYPE == "CF")
                        {
                            if (line.Data.ATTRIBUTE == "CLN")
                            {
                                if (line.Data.LINEID == "KWF23637R")
                                {
                                    kzonePPID = jobData.PPID.Substring(28, 4);//PH07 28
                                }
                                else
                                {
                                    kzonePPID = jobData.PPID.Substring(32, 4);//PH01~06 28+4
                                }
                            }
                            else if (line.Data.ATTRIBUTE == "DEV")
                            {
                                kzonePPID = jobData.PPID.Substring(56, 4);//52+4
                            }

                        }
                        else
                        {
                            kzonePPID = jobData.PPID;
                        }
                        dgvJobData.Rows.Add("Detail",
                              localNo,
                              jobData.CurrentUnitNo,
                              jobData.PrositonNo,
                              jobData.CassetteSequenceNo,
                              jobData.JobSequenceNo,
                              jobData.Lot_ID,
                              jobData.Product_ID,
                              jobData.GlassID_or_PanelID,
                              jobData.Skip_Flag,
                              jobData.Process_Flag,
                              jobData.Job_Judge,
                              jobData.Job_Grade,
                              jobData.Master_Recipe_ID,
                              kzonePPID

                              );
                        if (jobData.RemoveFlag)
                        {
                            dgvJobData.Rows[dgvJobData.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                    if (dgvJobData.Rows.Count > 0)
                    {
                        dgvJobData.Sort(dgvJobData.Columns[colLocalNo.Name], ListSortDirection.Descending);
                    }

                }

            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }

        }


        private string GetPortProcrssTypeContent(string index)
        {
            int result = 0;
            if (int.TryParse(index, out result) == true)
            {
                foreach (ProcessType_Array processTypeArray in OPIAp.CurLine.Lst_ProcessType_Array)
                {
                    if (processTypeArray.ProcessTypeNo == int.Parse(index))
                    {
                        return processTypeArray.ProcessTypeDesc;
                    }
                }
            }
            return index;
        }


        #region

        //private void tbLayoutSize_ScrollPress(object sender, KeyEventArgs e)
        //{
        //    if (tBlayoutSize.Enabled == true)
        //    {
        //        Double _layoutSzie = Double.Parse(tBlayoutSize.Value.ToString()) / 100;
        //        UniTools.ReLayoutSize(pnlLayout, layoutsize, layoutfontsize, _layoutSzie);
        //    }
        //}

        //private void tbLayoutSize_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (tBlayoutSize.Enabled == true)
        //    {
        //        Double _layoutSzie = Double.Parse(tBlayoutSize.Value.ToString()) / 100;
        //        UniTools.ReLayoutSize(pnlLayout, layoutsize, layoutfontsize, _layoutSzie);
        //    }
        //}

        //private void rdoSize_Click(object sender, EventArgs e)
        //{

        //    if (rdoOtherSzie.Checked == false)
        //    {
        //        tBlayoutSize.Enabled = false;
        //        Double _layoutSzie = Double.Parse(((RadioButton)sender).Tag.ToString()) / 100;
        //        if (_layoutSzie != 0)
        //            UniTools.ReLayoutSize(pnlLayout, layoutsize, layoutfontsize, _layoutSzie);
        //    }
        //    else
        //    {
        //        tBlayoutSize.Enabled = true;
        //    }

        //}
        #endregion

        private void cmsJobDelete_Click(object sender, EventArgs e)
        {
            if (this.cmsJobDelete.Items[0].Text.Trim() == "JobDelete")
            {
                if (MouseButtons.Left == ((MouseEventArgs)e).Button)
                {
                    FrmQues fq = new FrmQues();
                    fq.ShowDialog();
                    if (fq.DialogResult == DialogResult.Yes)
                    {
                        if (dgvJobData.RowCount > 0)
                        {
                            string glassID = dgvJobData.CurrentRow.Cells["colGlassID"].Value.ToString().Trim();
                            Job job = ObjectManager.JobManager.GetJob(glassID);
                            if (job != null)
                            {
                                ObjectManager.JobManager.DeleteJob(job);

                                //删除资料记录
                                ObjectManager.JobManager.SaveJobHistory(job, eJobEvent.Delete.ToString(),
                                  eqp.Data.NODEID,
                                  eqp.Data.NODENO, job.CurrentUnitNo.ToString(), "", "", "");
                                MessageBox.Show($"GlassID=[{glassID}] Delete Success!");
                                GetJobDatatodgvJobData("L3", "");
                            }
                            else
                            {
                                MessageBox.Show($"Delete Error, GlassID=[{glassID}] Not Exist!");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"There's No Job!");
                        }
                    }
                }
            }
        }
    }
}