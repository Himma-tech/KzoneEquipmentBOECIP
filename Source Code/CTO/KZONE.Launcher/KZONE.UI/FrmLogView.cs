using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using KZONE.Log;

namespace KZONE.UI
{
	public class FrmLogView : Form
	{
		public delegate void AddLogHandler(string aMsg);

		private IContainer components = null;

		private Panel pnlMain;

        private Panel panel6;

		private Panel panel5;

		private Button btnClear;

		private Button btnStop;

		private Button btnStart;

		private ComboBox cbbDisplayLine;

		private Label lblDisplayLine;

		private ComboBox cbbLogLevel;

		private Label lblLogLevel;

		private ComboBox cbbDisplayType;

		private Label lblManagerName;

		private Panel panel7;

		private ListBox lstLogView_Part;

		private ListBox lstLogView;

		private static ILogManager ILogMgr = NLogManager.Logger;

		private static LogEventHandler _logHandler;

		private static Queue<string> _logQueue;

		private bool _logStartFlag = false;

		private static Thread _logThread;

		private int currLogLevel;

		private int currDisplayLine = 200;

		private string currManagerName;

		public bool LogStartFlag
		{
			get
			{
				return this._logStartFlag;
			}
			set
			{
				this._logStartFlag = true;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lstLogView_Part = new System.Windows.Forms.ListBox();
            this.lstLogView = new System.Windows.Forms.ListBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.cbbDisplayLine = new System.Windows.Forms.ComboBox();
            this.lblDisplayLine = new System.Windows.Forms.Label();
            this.cbbLogLevel = new System.Windows.Forms.ComboBox();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.cbbDisplayType = new System.Windows.Forms.ComboBox();
            this.lblManagerName = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.panel6);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1735, 930);
            this.pnlMain.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1735, 930);
            this.panel6.TabIndex = 13;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lstLogView_Part);
            this.panel7.Controls.Add(this.lstLogView);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 89);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1735, 841);
            this.panel7.TabIndex = 14;
            // 
            // lstLogView_Part
            // 
            this.lstLogView_Part.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLogView_Part.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogView_Part.FormattingEnabled = true;
            this.lstLogView_Part.HorizontalExtent = 3000;
            this.lstLogView_Part.HorizontalScrollbar = true;
            this.lstLogView_Part.ItemHeight = 15;
            this.lstLogView_Part.Location = new System.Drawing.Point(0, 0);
            this.lstLogView_Part.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lstLogView_Part.Name = "lstLogView_Part";
            this.lstLogView_Part.ScrollAlwaysVisible = true;
            this.lstLogView_Part.Size = new System.Drawing.Size(1735, 841);
            this.lstLogView_Part.TabIndex = 15;
            this.lstLogView_Part.DoubleClick += new System.EventHandler(this.lstLogView_DoubleClick_1);
            // 
            // lstLogView
            // 
            this.lstLogView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLogView.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogView.FormattingEnabled = true;
            this.lstLogView.HorizontalExtent = 3000;
            this.lstLogView.HorizontalScrollbar = true;
            this.lstLogView.ItemHeight = 15;
            this.lstLogView.Location = new System.Drawing.Point(0, 0);
            this.lstLogView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lstLogView.Name = "lstLogView";
            this.lstLogView.ScrollAlwaysVisible = true;
            this.lstLogView.Size = new System.Drawing.Size(1735, 841);
            this.lstLogView.TabIndex = 13;
            this.lstLogView.DoubleClick += new System.EventHandler(this.lstLogView_DoubleClick);
            // 
            // panel5
            // 
            this.panel5.AutoSize = true;
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.btnClear);
            this.panel5.Controls.Add(this.btnStop);
            this.panel5.Controls.Add(this.btnStart);
            this.panel5.Controls.Add(this.cbbDisplayLine);
            this.panel5.Controls.Add(this.lblDisplayLine);
            this.panel5.Controls.Add(this.cbbLogLevel);
            this.panel5.Controls.Add(this.lblLogLevel);
            this.panel5.Controls.Add(this.cbbDisplayType);
            this.panel5.Controls.Add(this.lblManagerName);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1735, 89);
            this.panel5.TabIndex = 13;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(796, 48);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(108, 38);
            this.btnClear.TabIndex = 27;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(681, 48);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(108, 38);
            this.btnStop.TabIndex = 26;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(565, 48);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(108, 38);
            this.btnStart.TabIndex = 25;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cbbDisplayLine
            // 
            this.cbbDisplayLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDisplayLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbDisplayLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbDisplayLine.FormattingEnabled = true;
            this.cbbDisplayLine.Items.AddRange(new object[] {
            "200",
            "500",
            "1000"});
            this.cbbDisplayLine.Location = new System.Drawing.Point(370, 48);
            this.cbbDisplayLine.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbbDisplayLine.Name = "cbbDisplayLine";
            this.cbbDisplayLine.Size = new System.Drawing.Size(123, 25);
            this.cbbDisplayLine.TabIndex = 24;
            this.cbbDisplayLine.SelectedIndexChanged += new System.EventHandler(this.cbbDisplayLine_SelectedIndexChanged);
            // 
            // lblDisplayLine
            // 
            this.lblDisplayLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblDisplayLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDisplayLine.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.lblDisplayLine.ForeColor = System.Drawing.Color.Black;
            this.lblDisplayLine.Location = new System.Drawing.Point(261, 46);
            this.lblDisplayLine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDisplayLine.Name = "lblDisplayLine";
            this.lblDisplayLine.Size = new System.Drawing.Size(101, 27);
            this.lblDisplayLine.TabIndex = 23;
            this.lblDisplayLine.Text = "Max Line";
            this.lblDisplayLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbbLogLevel
            // 
            this.cbbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbLogLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbLogLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbLogLevel.FormattingEnabled = true;
            this.cbbLogLevel.Items.AddRange(new object[] {
            "0_ALL",
            "1_Debug",
            "2_Info",
            "3_Warn",
            "4_Error"});
            this.cbbLogLevel.Location = new System.Drawing.Point(127, 46);
            this.cbbLogLevel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbbLogLevel.Name = "cbbLogLevel";
            this.cbbLogLevel.Size = new System.Drawing.Size(126, 25);
            this.cbbLogLevel.TabIndex = 22;
            this.cbbLogLevel.SelectedIndexChanged += new System.EventHandler(this.cbbLogLevel_SelectedIndexChanged);
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblLogLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLogLevel.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.lblLogLevel.ForeColor = System.Drawing.Color.Black;
            this.lblLogLevel.Location = new System.Drawing.Point(24, 46);
            this.lblLogLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(101, 27);
            this.lblLogLevel.TabIndex = 21;
            this.lblLogLevel.Text = "Log Level";
            this.lblLogLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbbDisplayType
            // 
            this.cbbDisplayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDisplayType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbDisplayType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbDisplayType.FormattingEnabled = true;
            this.cbbDisplayType.Items.AddRange(new object[] {
            "0_All"});
            this.cbbDisplayType.Location = new System.Drawing.Point(183, 5);
            this.cbbDisplayType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbbDisplayType.Name = "cbbDisplayType";
            this.cbbDisplayType.Size = new System.Drawing.Size(310, 25);
            this.cbbDisplayType.TabIndex = 20;
            this.cbbDisplayType.SelectedIndexChanged += new System.EventHandler(this.cbbDisplayType_SelectedIndexChanged);
            // 
            // lblManagerName
            // 
            this.lblManagerName.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblManagerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblManagerName.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.lblManagerName.ForeColor = System.Drawing.Color.Black;
            this.lblManagerName.Location = new System.Drawing.Point(24, 5);
            this.lblManagerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblManagerName.Name = "lblManagerName";
            this.lblManagerName.Size = new System.Drawing.Size(157, 27);
            this.lblManagerName.TabIndex = 19;
            this.lblManagerName.Text = "Logger Name";
            this.lblManagerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmLogView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1735, 930);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FrmLogView";
            this.Text = "LogView";
            this.Load += new System.EventHandler(this.FrmLogview_Load);
            this.pnlMain.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		public FrmLogView()
		{
			this.InitializeComponent();
		}

		public void Init()
		{
			try
			{
				FrmLogView._logHandler = new LogEventHandler(this.OnLogEvent);
				FrmLogView._logQueue = new Queue<string>(100);
				FrmLogView._logThread = new Thread(new ThreadStart(this.BeginLogEvent));
				FrmLogView._logThread.IsBackground = true;
				FrmLogView._logThread.Start();
				this.cbbLogLevel.SelectedIndex = this.currLogLevel + 2;
				this.cbbDisplayLine.Text = this.currDisplayLine.ToString();
				this.cbbDisplayType.SelectedIndex = 0;
				if (FrmLogView.ILogMgr != null)
				{
					if (this._logStartFlag)
					{
						this.btnStart.Enabled = false;
						this.btnStop.Enabled = true;
						FrmLogView.ILogMgr.AddEventSink(FrmLogView._logHandler);
					}
					else
					{
						this.btnStart.Enabled = true;
						this.btnStop.Enabled = false;
					}
				}
			}
			catch (Exception exception)
			{
				FrmLogView.ILogMgr.LogErrorWrite("", "FrmLogView", "Init", "LogViewer Setup Fail", exception);
			}
		}

		private void BeginLogEvent()
		{
			try
			{
				while (true)
				{
					try
					{
						int count = 0;
						while (base.IsHandleCreated && FrmLogView._logQueue.Count > 0 && count < 20)
						{
							string item = null;
							lock (FrmLogView._logQueue)
							{
								item = FrmLogView._logQueue.Dequeue();
							}
							this.lstLogView_Part.BeginInvoke(new FrmLogView.AddLogHandler(this.AddLogData), new object[]
							{
								item
							});
							this.lstLogView.BeginInvoke(new FrmLogView.AddLogHandler(this.AddLogData2), new object[]
							{
								item
							});
							count++;
						}
						Thread.Sleep(3);
					}
					catch
					{
					}
				}
			}
			catch (Exception exception)
			{
				Debug.WriteLine("[ FrmLogView:" + MethodBase.GetCurrentMethod().Name + "] " + exception.Message);
			}
		}

		private void AddLogData(string logMessage)
		{
			if (this.lstLogView_Part.Items.Count > this.currDisplayLine)
			{
				this.lstLogView_Part.Items.Clear();
			}
			if (this.currManagerName != null && logMessage.IndexOf(this.currManagerName) > 0)
			{
				this.lstLogView_Part.Items.Insert(0, logMessage);
			}
		}

		private void AddLogData2(string logMessage)
		{
			if (this.lstLogView.Items.Count > this.currDisplayLine)
			{
				this.lstLogView.Items.Clear();
			}
			this.lstLogView.Items.Insert(0, logMessage);
		}

		private void OnLogEvent(LogInfo logInfo)
		{
			if (!this.btnStart.Enabled && logInfo.Level >= this.currLogLevel)
			{
				string str = string.Format("{0} {1} {2} {3} {4}", new object[]
				{
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff", DateTimeFormatInfo.InvariantInfo),
					((LogLevel)logInfo.Level).ToString().PadRight(5, ' '),
					logInfo.ClassName,
					logInfo.MethodName,
					logInfo.Message
				});
				lock (FrmLogView._logQueue)
				{
					if (FrmLogView._logQueue.Count >= 1000)
					{
						FrmLogView._logQueue.Dequeue();
					}
					FrmLogView._logQueue.Enqueue(str);
				}
				this.cbbDisplayType.BeginInvoke(new FrmLogView.AddLogHandler(this.AddLogHeader), new object[]
				{
					logInfo.ClassName
				});
			}
		}

		public void AddLogHeader(string className)
		{
			try
			{
				if (!this.cbbDisplayType.Items.Contains(className))
				{
					this.cbbDisplayType.Items.Add(className);
				}
			}
			catch (Exception)
			{
			}
		}

		private void FrmLogview_Load(object sender, EventArgs e)
		{
			try
			{
				this.cbbDisplayLine.SelectedIndex = 2;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
			}
		}

		private void cbbDisplayLine_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.currDisplayLine = int.Parse(this.cbbDisplayLine.Text);
			}
			catch
			{
			}
		}

		private void cbbDisplayType_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.cbbDisplayType.SelectedIndex == 0)
				{
					this.currManagerName = this.cbbDisplayType.Text;
					this.lstLogView.Visible = true;
					this.lstLogView_Part.Visible = false;
				}
				else if (this.currManagerName != this.cbbDisplayType.Text)
				{
					this.currManagerName = this.cbbDisplayType.Text;
					this.GetLogByManagerName(this.currManagerName);
				}
			}
			catch (Exception exception)
			{
				Debug.WriteLine("[FrmLogView:cbbDisplayType_SelectedIndexChanged] " + exception.Message);
			}
		}

		private void cbbLogLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cbbLogLevel.SelectedIndex >= 0)
			{
				this.currLogLevel = this.cbbLogLevel.SelectedIndex;
			}
		}

		private void GetLogByManagerName(string sManagerName)
		{
			try
			{
				this.lstLogView_Part.Items.Clear();
				for (int i = this.lstLogView.Items.Count - 1; i >= 0; i--)
				{
					string item = this.lstLogView.Items[i] as string;
					if (item.IndexOf(sManagerName) > 0)
					{
						this.lstLogView_Part.Items.Insert(0, item);
					}
				}
				this.lstLogView.Visible = false;
				this.lstLogView_Part.Visible = true;
			}
			catch (Exception exception)
			{
				Debug.WriteLine("[FrmLogView:GetLogByManagerName] " + exception.Message);
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			try
			{
				FrmLogView.ILogMgr.AddEventSink(FrmLogView._logHandler);
				this.btnStart.Enabled = false;
				this.btnStop.Enabled = true;
			}
			catch (Exception exception)
			{
				Debug.WriteLine("FrmLogView:btnStart_Click] " + exception.Message);
			}
			finally
			{
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			try
			{
				FrmLogView.ILogMgr.RemoveEventSink(FrmLogView._logHandler);
				this.btnStop.Enabled = false;
				this.btnStart.Enabled = true;
			}
			catch (Exception exception)
			{
				Debug.WriteLine("FrmLogView:btnStop_Click] " + exception.Message);
			}
			finally
			{
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			this.lstLogView.Items.Clear();
			this.lstLogView_Part.Items.Clear();
		}

		private void lstLogView_DoubleClick(object sender, EventArgs e)
		{
			if (this.lstLogView.SelectedItems.Count > 0)
			{
				string log = this.lstLogView.SelectedItems[0] as string;
				new FrmLogDetail
				{
					LogDetail = log
				}.Show();
			}
		}

		private void lstLogView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
		}

		private void lstLogView_DoubleClick_1(object sender, EventArgs e)
		{
			if (this.lstLogView_Part.SelectedItems.Count > 0)
			{
				string log = this.lstLogView_Part.SelectedItems[0] as string;
				new FrmLogDetail
				{
					LogDetail = log
				}.Show();
			}
		}
	}
}
