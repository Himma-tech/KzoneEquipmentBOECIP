using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using KZONE.MessageManager;
using KZONE.Work;

namespace KZONE.UI
{
	public class FrmAgent : Form
	{
		private IContainer components = null;

        private Panel pnlMain;

        private Panel panel7;

		private Panel panel8;

		private Panel panel9;

        private TreeView _treTree;

        private ContextMenuStrip contextMenuStrip1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private GroupBox groupBox1;
        private Label _lblConnectState;
        private Label label10;
        private Label _lblFormatFilePath;
        private Label _lblConfigFilePath;
        private Label _lblDllVer;
        private Label _lblDllName;
        private Label _lblName;
        private Label _lblStatus;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private RichTextBox _txtConfig;
        private Label label2;
        private DataGridView _dgvDataGrid;
        private Label label1;

		private ToolStripMenuItem refreshToolStripMenuItem;

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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this._treTree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._lblConnectState = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._lblFormatFilePath = new System.Windows.Forms.Label();
            this._lblConfigFilePath = new System.Windows.Forms.Label();
            this._lblDllVer = new System.Windows.Forms.Label();
            this._lblDllName = new System.Windows.Forms.Label();
            this._lblName = new System.Windows.Forms.Label();
            this._lblStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._txtConfig = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._dgvDataGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.panel9.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.panel9);
            this.pnlMain.Controls.Add(this.panel8);
            this.pnlMain.Controls.Add(this.panel7);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1024, 654);
            this.pnlMain.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this._treTree);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(0, 10);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(209, 644);
            this.panel9.TabIndex = 16;
            // 
            // _treTree
            // 
            this._treTree.ContextMenuStrip = this.contextMenuStrip1;
            this._treTree.Dock = System.Windows.Forms.DockStyle.Left;
            this._treTree.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._treTree.HideSelection = false;
            this._treTree.Location = new System.Drawing.Point(0, 0);
            this._treTree.Name = "_treTree";
            this._treTree.Size = new System.Drawing.Size(206, 644);
            this._treTree.TabIndex = 0;
            this._treTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treTree_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 26);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.ShowShortcutKeys = false;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.MaximumSize = new System.Drawing.Size(212, 750);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(212, 10);
            this.panel8.TabIndex = 15;
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.Controls.Add(this.splitContainer1);
            this.panel7.Location = new System.Drawing.Point(211, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(813, 654);
            this.panel7.TabIndex = 14;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._dgvDataGrid);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(813, 654);
            this.splitContainer1.SplitterDistance = 560;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this._txtConfig);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(560, 654);
            this.splitContainer2.SplitterDistance = 325;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._lblConnectState);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this._lblFormatFilePath);
            this.groupBox1.Controls.Add(this._lblConfigFilePath);
            this.groupBox1.Controls.Add(this._lblDllVer);
            this.groupBox1.Controls.Add(this._lblDllName);
            this.groupBox1.Controls.Add(this._lblName);
            this.groupBox1.Controls.Add(this._lblStatus);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 325);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Agent Status";
            // 
            // _lblConnectState
            // 
            this._lblConnectState.AutoSize = true;
            this._lblConnectState.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblConnectState.Location = new System.Drawing.Point(179, 37);
            this._lblConnectState.Name = "_lblConnectState";
            this._lblConnectState.Size = new System.Drawing.Size(50, 18);
            this._lblConnectState.TabIndex = 25;
            this._lblConnectState.Text = "label9";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 17);
            this.label10.TabIndex = 24;
            this.label10.Text = "ConnectState:";
            // 
            // _lblFormatFilePath
            // 
            this._lblFormatFilePath.AutoSize = true;
            this._lblFormatFilePath.Location = new System.Drawing.Point(179, 220);
            this._lblFormatFilePath.Name = "_lblFormatFilePath";
            this._lblFormatFilePath.Size = new System.Drawing.Size(54, 17);
            this._lblFormatFilePath.TabIndex = 23;
            this._lblFormatFilePath.Text = "label13";
            // 
            // _lblConfigFilePath
            // 
            this._lblConfigFilePath.AutoSize = true;
            this._lblConfigFilePath.Location = new System.Drawing.Point(179, 189);
            this._lblConfigFilePath.Name = "_lblConfigFilePath";
            this._lblConfigFilePath.Size = new System.Drawing.Size(54, 17);
            this._lblConfigFilePath.TabIndex = 22;
            this._lblConfigFilePath.Text = "label14";
            // 
            // _lblDllVer
            // 
            this._lblDllVer.AutoSize = true;
            this._lblDllVer.Location = new System.Drawing.Point(179, 158);
            this._lblDllVer.Name = "_lblDllVer";
            this._lblDllVer.Size = new System.Drawing.Size(53, 17);
            this._lblDllVer.TabIndex = 21;
            this._lblDllVer.Text = "label11";
            // 
            // _lblDllName
            // 
            this._lblDllName.AutoSize = true;
            this._lblDllName.Location = new System.Drawing.Point(179, 127);
            this._lblDllName.Name = "_lblDllName";
            this._lblDllName.Size = new System.Drawing.Size(54, 17);
            this._lblDllName.TabIndex = 20;
            this._lblDllName.Text = "label12";
            // 
            // _lblName
            // 
            this._lblName.AutoSize = true;
            this._lblName.Location = new System.Drawing.Point(179, 96);
            this._lblName.Name = "_lblName";
            this._lblName.Size = new System.Drawing.Size(54, 17);
            this._lblName.TabIndex = 19;
            this._lblName.Text = "label10";
            // 
            // _lblStatus
            // 
            this._lblStatus.AutoSize = true;
            this._lblStatus.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblStatus.Location = new System.Drawing.Point(179, 65);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(50, 18);
            this._lblStatus.TabIndex = 18;
            this._lblStatus.Text = "label9";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Format File Path:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Config File Path:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "dll Ver:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "dll Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Status:";
            // 
            // _txtConfig
            // 
            this._txtConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtConfig.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtConfig.Location = new System.Drawing.Point(0, 16);
            this._txtConfig.Name = "_txtConfig";
            this._txtConfig.Size = new System.Drawing.Size(560, 309);
            this._txtConfig.TabIndex = 1;
            this._txtConfig.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Config:";
            // 
            // _dgvDataGrid
            // 
            this._dgvDataGrid.AllowUserToAddRows = false;
            this._dgvDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dgvDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgvDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dgvDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dgvDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this._dgvDataGrid.Location = new System.Drawing.Point(0, 17);
            this._dgvDataGrid.Name = "_dgvDataGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgvDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this._dgvDataGrid.RowTemplate.Height = 27;
            this._dgvDataGrid.Size = new System.Drawing.Size(244, 637);
            this._dgvDataGrid.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Run Time Info:";
            // 
            // FrmAgent
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1024, 654);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FrmAgent";
            this.Text = "Agent";
            this.Activated += new System.EventHandler(this.FrmAgent_Activated);
            this.Deactivate += new System.EventHandler(this.FrmAgent_Deactivate);
            this.Load += new System.EventHandler(this.FrmAgent_Load);
            this.pnlMain.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvDataGrid)).EndInit();
            this.ResumeLayout(false);

		}

		public FrmAgent()
		{
			this.InitializeComponent();
		}

		public void Init()
		{
		}

		private void FrmAgent_Load(object sender, EventArgs e)
		{
			try
			{
				foreach (IServerAgent agent in Workbench.Instance.AgentList.Values)
				{
					TreeNode node = this._treTree.Nodes.Add(agent.Name);
					node.Tag = agent;
				}
				if (this._treTree.Nodes.Count > 0)
				{
					this._treTree.SelectedNode = this._treTree.Nodes[0];
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
			}
		}

		private void FrmAgent_Activated(object sender, EventArgs e)
		{
			Debug.WriteLine("FrmAgent_Activated");
		}

		private void FrmAgent_Deactivate(object sender, EventArgs e)
		{
			Debug.WriteLine("FrmAgent_Deactivate");
		}

		private void _treTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			try
			{
				if (this._treTree.SelectedNode != null && this._treTree.SelectedNode.Tag is IServerAgent)
				{
					this.ShowAgentInformation(this._treTree.SelectedNode);
				}
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				for (Exception tmp = ex; tmp != null; tmp = tmp.InnerException)
				{
					sb.AppendLine(tmp.Message);
					sb.AppendLine(tmp.StackTrace);
				}
				MessageBox.Show(sb.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private DataTable GetRunTimeInfo(IServerAgent Agent)
		{
			DataTable result;
			try
			{
				DataTable ret = new DataTable();
				ret.Columns.Add("Field", typeof(string));
				ret.Columns.Add("Value", typeof(string));
				foreach (string key in Agent.RuntimeInfo.Keys)
				{
					DataRow dr = ret.NewRow();
					dr["Field"] = key;
					dr["Value"] = Agent.RuntimeInfo[key];
					ret.Rows.Add(dr);
				}
				result = ret;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (this._treTree.SelectedNode != null && this._treTree.SelectedNode.Tag is IServerAgent)
				{
					this.ShowAgentInformation(this._treTree.SelectedNode);
				}
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				for (Exception tmp = ex; tmp != null; tmp = tmp.InnerException)
				{
					sb.AppendLine(tmp.Message);
					sb.AppendLine(tmp.StackTrace);
				}
				MessageBox.Show(sb.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void ShowAgentInformation(TreeNode selectedNode)
		{
			IServerAgent agent = selectedNode.Tag as IServerAgent;
			if (agent.AgentStatus == eAgentStatus.RUN)
			{
				this._lblStatus.ForeColor = Color.Green;
			}
			else if (agent.AgentStatus == eAgentStatus.ERROR || agent.AgentStatus == eAgentStatus.STOP)
			{
				this._lblStatus.ForeColor = Color.Red;
			}
			this._lblStatus.Text = agent.AgentStatus.ToString();
			if (agent.ConnectedState == "Connected")
			{
				this._lblConnectState.ForeColor = Color.Green;
			}
			else
			{
				this._lblConnectState.ForeColor = Color.Red;
			}
			this._lblConnectState.Text = agent.ConnectedState;
			this._lblName.Text = agent.Name;
			Assembly asm = Assembly.GetAssembly(agent.GetType());
			string[] strs = asm.FullName.Split(new char[]
			{
				','
			});
			this._lblDllName.Text = strs[0];
			this._lblDllVer.Text = strs[1];
			this._lblConfigFilePath.Text = agent.ConfigFileName;
			this._lblFormatFilePath.Text = agent.FormatFileName;
			this._txtConfig.Text = agent.Configuration.ToFormatString();
			DataTable dt = this.GetRunTimeInfo(agent);
			if (this._dgvDataGrid.DataSource is DataTable)
			{
				((DataTable)this._dgvDataGrid.DataSource).Dispose();
				this._dgvDataGrid.DataSource = null;
			}
			this._dgvDataGrid.DataSource = dt;
		}
	}
}
