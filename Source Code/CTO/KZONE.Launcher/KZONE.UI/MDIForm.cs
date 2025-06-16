using KZONE.ConstantParameter;
using KZONE.Entity;
using KZONE.EntityManager;
using KZONE.Log;
using KZONE.Service;
using KZONE.Work;
using Spring.Context;
using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KZONE.UI
{
    public partial class MDIForm : Form, IApplicationContextAware
    {
        Sunisoft.IrisSkin.SkinEngine SkinEngine = new Sunisoft.IrisSkin.SkinEngine();
        List<string> Skins;

        private static ILogManager ILogMgr = NLogManager.Logger;
        private Dictionary<string, string> _commandButtonList;
        private List<Control> _labelButtons = new List<Control>();
        private string _defaultDisplayFormName = "Layout";
        private IApplicationContext _applicationContext;
        public static FormBase FrmHistory;
        public OPIInfo OPIAp;

        public static CIMMessageManagement FrmCIMMesg;
        public int CimMesgCount = 0;

        int panelWidth;
        bool isCollapsed;
        bool haveMap;
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


        public IApplicationContext ApplicationContext
        {
            get
            {
                return this._applicationContext;
            }
            set
            {
                this._applicationContext = value;
            }
        }
        public Dictionary<string, string> CommandButtonList
        {
            get { return _commandButtonList; }
            set { _commandButtonList = value; }
        }


        public void Init()
        {
            try
            {
                OPIAp = OPIInfo.CreateInstance();

                

                MDIForm.ILogMgr.LogInfoWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Main form Setup Complete.");
            }
            catch (Exception exception)
            {
                MDIForm.ILogMgr.LogErrorWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", "Main form Setup Fail. " + exception.Message);
            }
        }
        public MDIForm()
        {
            InitializeComponent();
            this._commandButtonList = new Dictionary<string, string>();

            panelWidth = _pnlToolBar.Width;
            isCollapsed = false;

            //Skins = Directory.GetFiles(Application.StartupPath + @"\Skins\").ToList();
            //SkinEngine.SkinFile = Skins[3];
            //SkinEngine.Active = true;

        }
        public void Initial(string ServerName, string Version)
        {

        }


        private void MDIForm_Load(object sender, EventArgs e)
        {
            try
            {
                button7.Visible = false;


                OPIConst.TimingChartFolder = ConfigurationManager.AppSettings["TimingChartFolder"];
              
                IObjectFactory obj_factory = this.ApplicationContext;
                for (int i = 0; i < this.CommandButtonList.Count; i++)
                {
                    string frm_name = this.CommandButtonList[(i + 1).ToString()];
                    object obj = obj_factory.GetObject(frm_name);

                    if (i != 5 && obj != null && obj is Form)
                    {
                        Form frm = obj as Form;
                        if (frm.Text == "LogView")
                        {
                            button1.Tag = frm;

                            button1.Click += new EventHandler(this.CommandButton_Click);
                        }
                        if (frm.Text == "Agent")
                        {
                            button2.Tag = frm;

                            button2.Click += new EventHandler(this.CommandButton_Click);
                        }
                        if (frm.Text == "Layout")
                        {
                            button3.Tag = frm;

                            button3.Click += new EventHandler(this.CommandButton_Click);
                        }
                    }
                }
                // label10.AutoSize = true;

                List<Button> labels = new List<Button>();

                labels.Add(button4);
                labels.Add(button5);
                labels.Add(button7);
                labels.Add(button6);
                labels.Add(button8);

                foreach (Button lb in labels)
                {
                    lb.Click += new EventHandler(this.ComboBoxCommandButton_Click);
                    lb.Visible = true;
                    this._labelButtons.Add(lb);
                    this._pnlToolBar.Controls.Add(lb);
                   
                }

                this.ShowForm(this._defaultDisplayFormName);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            Debug.WriteLine("Load: " + base.IsHandleCreated);
        }

        private void ShowForm(string formName)
        {
            try
            {
                for (int i = 0; i < this._pnlToolBar.Controls.Count; i++)
                {
                    if (this._pnlToolBar.Controls[i] is Button)
                    {
                        Button lbl = this._pnlToolBar.Controls[i] as Button;
                        if (lbl.Text.Trim().Contains(formName))
                        {
                            this.CommandButton_Click(lbl, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private void CommandButton_Click(object sender, EventArgs e)
        {
            Control lbl = sender as Control;
            if (lbl.Tag is Form)
            {
                Form frm = lbl.Tag as Form;
                if (frm.MdiParent == null)
                {
                    this.panel1MDI.Controls.Clear();
                    frm.TopLevel = false;
                    frm.Parent = this.panel1MDI;
                    //frm.MdiParent = this;
                    frm.Dock = DockStyle.Fill;
                    frm.Show();
                }
                else
                {
                    frm.Activate();
                }
               
            }
          

            Movepanel5(lbl);
        }


        private void ComboBoxCommandButton_Click(object sender, EventArgs e)
        {
            Button comBox = sender as Button;
            if (FrmHistory != null)
            {
                FrmHistory.Close();
                FrmHistory.Dispose();
            }
            comBox.ForeColor = Color.White;
            if (comBox.Tag != null)
            {
                switch (comBox.Tag.ToString())
                {
                    case "NodeHistory":

                        FrmHistory = new FormHistory(new HisTableParam("SBCS_NODEHISTORY"));
                        FrmHistory.lblCaption.Text = "NODE HISTORY";
                        break;

                    case "UNITHISTORY":

                        FrmHistory = new FormHistory(new HisTableParam("SBCS_UNITHISTORY"));
                        FrmHistory.lblCaption.Text = "UNIT HISTORY";
                        break;

                    case "JobHistory":

                        FrmHistory = new FormHistory(new HisTableParam("SBCS_JOBHISTORY"));
                        FrmHistory.lblCaption.Text = "JOB HISTORY";
                        break;

                    case "AlarmHistory":
                        FrmHistory = new FormHistory(new HisTableParam("SBCS_ALARMHISTORY"));
                        FrmHistory.lblCaption.Text = "ALARM HISTORY";
                        break;
                    //PROCESSDATAHISTORY
                    case "ProcessDataHistory":
                        FrmHistory = new FormProcessDataHistory(new HisTableParam("SBCS_PROCESSDATAHISTORY"));
                        FrmHistory.lblCaption.Text = "PROCESS DATA HISTORY";
                        break;
                    case "LayOut":
                        FrmHistory = new FormLayout(OPIAp);
                        FrmHistory.lblCaption.Text = "LAYOUT";
                        break;
                    case "PlcTrxMoniter":
                        FrmHistory = new FormMonitorPLC();
                        FrmHistory.lblCaption.Text = "PLC TRX MONITER";
                        break;
                    case "MessageHistory":
                        FrmHistory = new FormHistory(new HisTableParam("SBCS_CIMMESSAGEHISTORY"));
                        FrmHistory.lblCaption.Text = "CIM MESSAGE HISTORY";
                        break;
                    case "RecipeTable":
                        FrmHistory = new FormHistory(new HisTableParam("SBRM_RECIPE"));
                        FrmHistory.lblCaption.Text = "RECIPE TABLE";
                        break;
                    case "TankHistory":
                        FrmHistory = new FormHistory(new HisTableParam("SBCS_TANKHISTORY"));
                        FrmHistory.lblCaption.Text = "TANK HISTORY";
                        break;
                    case "RecipeHistory":
                        FrmHistory = new FormHistory(new HisTableParam("SBCS_RECIPEHISTORY"));
                        FrmHistory.lblCaption.Text = "RECIPE HISTORY";
                        break;
                    case "TactTimeHistory":
                        FrmHistory = new FormProcessDataHistory(new HisTableParam("SBCS_TACTDATAHISTORY"));
                        FrmHistory.lblCaption.Text = "TACT TIME HISTORY";
                        break;
                }

                if (FrmHistory.MdiParent == null)
                {
                    this.panel1MDI.Controls.Clear();
                    FrmHistory.TopLevel = false;
                    FrmHistory.Parent = this.panel1MDI;
                    //frm.MdiParent = this;
                    FrmHistory.Dock = DockStyle.Fill;
                    FrmHistory.Show();
                }
                else
                {
                    FrmHistory.Activate();
                }
              
            }

            Movepanel5(comBox);
        }
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                _pnlToolBar.Width = _pnlToolBar.Width + 5;
                if (_pnlToolBar.Width >= panelWidth)
                {
                    timer1.Stop();
                    isCollapsed = false;
                    this.Refresh();
                }
            }
            else
            {
                _pnlToolBar.Width = _pnlToolBar.Width - 5;
                if (_pnlToolBar.Width <= 50)
                {
                    timer1.Stop();
                    isCollapsed = true;
                    this.Refresh();
                }
            }
        }
        private void Movepanel5(Control btn)
        {
            panel5.Top = btn.Top;
            panel5.Height = btn.Height;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            LogOutFrm logout = new LogOutFrm();
            if (logout.ShowDialog(this) == DialogResult.OK)
            {
                Workbench.Instance.Shutdown();

            }
            else
            {
                this.Show();
            }


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {

            e.DrawBackground();
            e.DrawBorder();
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                using (Font f = new Font("宋体", 12))
                {
                    e.Graphics.DrawString(e.ToolTipText, f,
                        SystemBrushes.ActiveCaptionText, e.Bounds, sf);
                }
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (OPIInfo.Q_OPIMessage != null && OPIInfo.Q_OPIMessage.Count > 0)
            {
                FormOPIMessage opiMessage = new FormOPIMessage();
            }
            Equipment eqp = ObjectManager.EquipmentManager.GetEQP("L3");

            if (eqp.File.SetCimMesg.Count > 0 && eqp.File.SetCimMesg.Count != CimMesgCount)
            {
                CimMesgCount = eqp.File.SetCimMesg.Count;
                if (FrmCIMMesg != null)
                {
                    FrmCIMMesg.UpData(eqp);
                }
                else
                {
                    FrmCIMMesg = new CIMMessageManagement(OPIAp);
                    FrmCIMMesg.UpData(eqp);
                }
            }
            else
            {
                if (eqp.File.SetCimMesg.Count == 0 && FrmCIMMesg != null)
                {
                    CimMesgCount = 0;
                    FrmCIMMesg.UpData(eqp);
                    //if (FrmCIMMesg != null)
                    //{
                    //    FrmCIMMesg.Close();
                    //    FrmCIMMesg = null;
                    //}
                }

            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (FrmCIMMesg == null)
            {
                Equipment eqp = ObjectManager.EquipmentManager.GetEQP("L3");
                FrmCIMMesg = new CIMMessageManagement(OPIAp);
                FrmCIMMesg.UpData(eqp);
            }
        }

        private void label3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                button7.Enabled = true;
                button7.Visible = true;
                this.button7.BackgroundImage = global::KZONE.UI.Properties.Resources.icons8_soda_bottle;
                this.button7.Text = "       RecipeCreate";
            }
            //if (e.Button == MouseButtons.Left)
            //{
            //    haveMap = false ;
            //}
        }
    }
}
