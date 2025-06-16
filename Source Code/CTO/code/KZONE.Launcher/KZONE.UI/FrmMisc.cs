using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using KZONE.ConstantParameter;

namespace KZONE.UI
{
	public class FrmMisc : Form
	{
		private IContainer components = null;

		private TabControl _tabMain;

		private TabPage tabPage1;

		private TabPage tabPage2;

		private Panel pnlMain;

		private Panel panel9;

		private Panel panel6;

		private Panel panel7;

		private Panel panel5;

		private Panel panel13;

		private Panel panel1;

		private Panel panel17;

		private ListView _lstConstants;

		private Panel panel2;

		private Panel panel14;

		private Panel panel16;

		private Panel panel15;

		private Panel panel18;

		private DataGridView _dgvConstant;

		private Panel panel19;

		private DataGridView _dgvParameters;

		private Button _btnConstantSave;

		private Button _btnParameterSave;

		private DataSet _dsConstant;

		private DataSet _dsParameter;

		private DataTable _dtConstant;

		private DataColumn constantKey;

		private DataColumn constantValue;

		private Button _btnConstantEdit;

		private Panel panel20;

		private Button _btnParameterEdit;

		private Panel panel21;

		private DataTable _dtParameter;

		private DataColumn parameterName;

		private DataColumn parameterValue;

		private DataColumn parameterDescription;

		private DataColumn parameterType;

		private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;

		private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

		private DataGridViewComboBoxColumn typeDataGridViewComboBoxColumn;

		private DataColumn constantDefault;

        private DataColumn constantDiscription;

		private Button _btnParameterRefresh;

		private Panel panel4;

		private DataTable _dtParamType = null;

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
            this._tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this._lstConstants = new System.Windows.Forms.ListView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this._dgvConstant = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this._btnConstantEdit = new System.Windows.Forms.Button();
            this.panel20 = new System.Windows.Forms.Panel();
            this._btnConstantSave = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel19 = new System.Windows.Forms.Panel();
            this._dgvParameters = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this._dsParameter = new System.Data.DataSet();
            this._dtParameter = new System.Data.DataTable();
            this.parameterName = new System.Data.DataColumn();
            this.parameterValue = new System.Data.DataColumn();
            this.parameterDescription = new System.Data.DataColumn();
            this.parameterType = new System.Data.DataColumn();
            this.panel16 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this._btnParameterRefresh = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this._btnParameterEdit = new System.Windows.Forms.Button();
            this.panel21 = new System.Windows.Forms.Panel();
            this._btnParameterSave = new System.Windows.Forms.Button();
            this.panel17 = new System.Windows.Forms.Panel();
            this._dsConstant = new System.Data.DataSet();
            this._dtConstant = new System.Data.DataTable();
            this.constantKey = new System.Data.DataColumn();
            this.constantValue = new System.Data.DataColumn();
            this.constantDefault = new System.Data.DataColumn();
            this.constantDiscription = new System.Data.DataColumn();
            this._tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvConstant)).BeginInit();
            this.panel14.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvParameters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsParameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtParameter)).BeginInit();
            this.panel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dsConstant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtConstant)).BeginInit();
            this.SuspendLayout();
            // 
            // _tabMain
            // 
            this._tabMain.Controls.Add(this.tabPage1);
            this._tabMain.Controls.Add(this.tabPage2);
            this._tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabMain.Location = new System.Drawing.Point(0, 0);
            this._tabMain.Name = "_tabMain";
            this._tabMain.SelectedIndex = 0;
            this._tabMain.Size = new System.Drawing.Size(1011, 654);
            this._tabMain.TabIndex = 0;
            this._tabMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._tabMain_DrawItem);
            this._tabMain.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this._tabMain_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(216)))), ((int)(((byte)(243)))));
            this.tabPage1.Controls.Add(this.pnlMain);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1003, 628);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Constant Manager";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.panel9);
            this.pnlMain.Controls.Add(this.panel6);
            this.pnlMain.Controls.Add(this.panel7);
            this.pnlMain.Controls.Add(this.panel5);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(3, 3);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(997, 622);
            this.pnlMain.TabIndex = 2;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this._lstConstants);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(0, 25);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(281, 597);
            this.panel9.TabIndex = 16;
            // 
            // _lstConstants
            // 
            this._lstConstants.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstConstants.FullRowSelect = true;
            this._lstConstants.GridLines = true;
            this._lstConstants.HideSelection = false;
            this._lstConstants.Location = new System.Drawing.Point(0, 0);
            this._lstConstants.MultiSelect = false;
            this._lstConstants.Name = "_lstConstants";
            this._lstConstants.Size = new System.Drawing.Size(281, 597);
            this._lstConstants.TabIndex = 0;
            this._lstConstants.UseCompatibleStateImageBehavior = false;
            this._lstConstants.View = System.Windows.Forms.View.Details;
            this._lstConstants.SelectedIndexChanged += new System.EventHandler(this._lstConstants_SelectedIndexChanged);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel6.Location = new System.Drawing.Point(281, 25);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(10, 597);
            this.panel6.TabIndex = 15;
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.Controls.Add(this.panel18);
            this.panel7.Controls.Add(this.panel2);
            this.panel7.Controls.Add(this.panel14);
            this.panel7.Location = new System.Drawing.Point(291, 25);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(706, 597);
            this.panel7.TabIndex = 14;
            // 
            // panel18
            // 
            this.panel18.Controls.Add(this._dgvConstant);
            this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel18.Location = new System.Drawing.Point(0, 0);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(706, 542);
            this.panel18.TabIndex = 13;
            // 
            // _dgvConstant
            // 
            this._dgvConstant.AllowUserToAddRows = false;
            this._dgvConstant.AllowUserToDeleteRows = false;
            this._dgvConstant.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this._dgvConstant.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgvConstant.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvConstant.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._dgvConstant.Location = new System.Drawing.Point(0, 0);
            this._dgvConstant.Name = "_dgvConstant";
            this._dgvConstant.RowTemplate.Height = 27;
            this._dgvConstant.Size = new System.Drawing.Size(706, 542);
            this._dgvConstant.TabIndex = 2;
            this._dgvConstant.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dgvConstant_CellContentClick);
            this._dgvConstant.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._dgvConstant_CellValueChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 542);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(706, 12);
            this.panel2.TabIndex = 12;
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel14.Controls.Add(this._btnConstantEdit);
            this.panel14.Controls.Add(this.panel20);
            this.panel14.Controls.Add(this._btnConstantSave);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel14.Location = new System.Drawing.Point(0, 554);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(706, 43);
            this.panel14.TabIndex = 11;
            // 
            // _btnConstantEdit
            // 
            this._btnConstantEdit.BackColor = System.Drawing.SystemColors.Control;
            this._btnConstantEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnConstantEdit.Enabled = false;
            this._btnConstantEdit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnConstantEdit.Location = new System.Drawing.Point(406, 0);
            this._btnConstantEdit.Name = "_btnConstantEdit";
            this._btnConstantEdit.Size = new System.Drawing.Size(144, 43);
            this._btnConstantEdit.TabIndex = 3;
            this._btnConstantEdit.Text = "Edit";
            this._btnConstantEdit.UseVisualStyleBackColor = false;
            this._btnConstantEdit.Click += new System.EventHandler(this._btnConstantEdit_Click);
            // 
            // panel20
            // 
            this.panel20.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel20.Location = new System.Drawing.Point(550, 0);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(12, 43);
            this.panel20.TabIndex = 2;
            // 
            // _btnConstantSave
            // 
            this._btnConstantSave.BackColor = System.Drawing.SystemColors.Control;
            this._btnConstantSave.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnConstantSave.Enabled = false;
            this._btnConstantSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnConstantSave.Location = new System.Drawing.Point(562, 0);
            this._btnConstantSave.Name = "_btnConstantSave";
            this._btnConstantSave.Size = new System.Drawing.Size(144, 43);
            this._btnConstantSave.TabIndex = 1;
            this._btnConstantSave.Text = "Save";
            this._btnConstantSave.UseVisualStyleBackColor = false;
            this._btnConstantSave.Click += new System.EventHandler(this._btnConstantSave_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(997, 25);
            this.panel5.TabIndex = 9;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(216)))), ((int)(((byte)(243)))));
            this.tabPage2.Controls.Add(this.panel13);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1003, 628);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Parameter Manager";
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.Transparent;
            this.panel13.Controls.Add(this.panel1);
            this.panel13.Controls.Add(this.panel17);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(3, 3);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(997, 622);
            this.panel13.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.panel19);
            this.panel1.Controls.Add(this.panel16);
            this.panel1.Controls.Add(this.panel15);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(997, 597);
            this.panel1.TabIndex = 14;
            // 
            // panel19
            // 
            this.panel19.Controls.Add(this._dgvParameters);
            this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel19.Location = new System.Drawing.Point(0, 0);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(997, 542);
            this.panel19.TabIndex = 14;
            // 
            // _dgvParameters
            // 
            this._dgvParameters.AllowUserToAddRows = false;
            this._dgvParameters.AllowUserToDeleteRows = false;
            this._dgvParameters.AutoGenerateColumns = false;
            this._dgvParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this._dgvParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgvParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.typeDataGridViewComboBoxColumn});
            this._dgvParameters.DataMember = "Table1";
            this._dgvParameters.DataSource = this._dsParameter;
            this._dgvParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvParameters.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._dgvParameters.Location = new System.Drawing.Point(0, 0);
            this._dgvParameters.Name = "_dgvParameters";
            this._dgvParameters.RowTemplate.Height = 27;
            this._dgvParameters.Size = new System.Drawing.Size(997, 542);
            this._dgvParameters.TabIndex = 1;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 54;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.Width = 60;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 96;
            // 
            // typeDataGridViewComboBoxColumn
            // 
            this.typeDataGridViewComboBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewComboBoxColumn.HeaderText = "Type";
            this.typeDataGridViewComboBoxColumn.Name = "typeDataGridViewComboBoxColumn";
            this.typeDataGridViewComboBoxColumn.Width = 35;
            // 
            // _dsParameter
            // 
            this._dsParameter.DataSetName = "NewDataSet";
            this._dsParameter.Tables.AddRange(new System.Data.DataTable[] {
            this._dtParameter});
            // 
            // _dtParameter
            // 
            this._dtParameter.Columns.AddRange(new System.Data.DataColumn[] {
            this.parameterName,
            this.parameterValue,
            this.parameterDescription,
            this.parameterType});
            this._dtParameter.TableName = "Table1";
            // 
            // parameterName
            // 
            this.parameterName.ColumnName = "Name";
            // 
            // parameterValue
            // 
            this.parameterValue.ColumnName = "Value";
            // 
            // parameterDescription
            // 
            this.parameterDescription.ColumnName = "Description";
            // 
            // parameterType
            // 
            this.parameterType.ColumnName = "Type";
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel16.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel16.Location = new System.Drawing.Point(0, 542);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(997, 12);
            this.panel16.TabIndex = 13;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel15.Controls.Add(this._btnParameterRefresh);
            this.panel15.Controls.Add(this.panel4);
            this.panel15.Controls.Add(this._btnParameterEdit);
            this.panel15.Controls.Add(this.panel21);
            this.panel15.Controls.Add(this._btnParameterSave);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel15.Location = new System.Drawing.Point(0, 554);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(997, 43);
            this.panel15.TabIndex = 12;
            // 
            // _btnParameterRefresh
            // 
            this._btnParameterRefresh.BackColor = System.Drawing.SystemColors.Control;
            this._btnParameterRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnParameterRefresh.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this._btnParameterRefresh.Location = new System.Drawing.Point(541, 0);
            this._btnParameterRefresh.Name = "_btnParameterRefresh";
            this._btnParameterRefresh.Size = new System.Drawing.Size(144, 43);
            this._btnParameterRefresh.TabIndex = 7;
            this._btnParameterRefresh.Text = "Refresh";
            this._btnParameterRefresh.UseVisualStyleBackColor = false;
            this._btnParameterRefresh.Click += new System.EventHandler(this._btnParameterRefresh_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(685, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(12, 43);
            this.panel4.TabIndex = 6;
            // 
            // _btnParameterEdit
            // 
            this._btnParameterEdit.BackColor = System.Drawing.SystemColors.Control;
            this._btnParameterEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnParameterEdit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this._btnParameterEdit.Location = new System.Drawing.Point(697, 0);
            this._btnParameterEdit.Name = "_btnParameterEdit";
            this._btnParameterEdit.Size = new System.Drawing.Size(144, 43);
            this._btnParameterEdit.TabIndex = 5;
            this._btnParameterEdit.Text = "Edit";
            this._btnParameterEdit.UseVisualStyleBackColor = false;
            this._btnParameterEdit.Click += new System.EventHandler(this._btnParameterEdit_Click);
            // 
            // panel21
            // 
            this.panel21.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel21.Location = new System.Drawing.Point(841, 0);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(12, 43);
            this.panel21.TabIndex = 4;
            // 
            // _btnParameterSave
            // 
            this._btnParameterSave.BackColor = System.Drawing.SystemColors.Control;
            this._btnParameterSave.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnParameterSave.Enabled = false;
            this._btnParameterSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this._btnParameterSave.Location = new System.Drawing.Point(853, 0);
            this._btnParameterSave.Name = "_btnParameterSave";
            this._btnParameterSave.Size = new System.Drawing.Size(144, 43);
            this._btnParameterSave.TabIndex = 3;
            this._btnParameterSave.Text = "Save";
            this._btnParameterSave.UseVisualStyleBackColor = false;
            this._btnParameterSave.Click += new System.EventHandler(this._btnParameterSave_Click);
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(997, 25);
            this.panel17.TabIndex = 9;
            // 
            // _dsConstant
            // 
            this._dsConstant.DataSetName = "NewDataSet";
            this._dsConstant.Tables.AddRange(new System.Data.DataTable[] {
            this._dtConstant});
            // 
            // _dtConstant
            // 
            this._dtConstant.Columns.AddRange(new System.Data.DataColumn[] {
            this.constantKey,
            this.constantValue,
            this.constantDefault,
            this.constantDiscription});
            this._dtConstant.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "Key"}, false)});
            this._dtConstant.TableName = "Table1";
            // 
            // constantKey
            // 
            this.constantKey.ColumnName = "Key";
            // 
            // constantValue
            // 
            this.constantValue.ColumnName = "Value";
            // 
            // constantDefault
            // 
            this.constantDefault.ColumnName = "Default";
            this.constantDefault.DataType = typeof(bool);
            // 
            // constantDiscription
            // 
            this.constantDiscription.ColumnName = "Description";
            // 
            // FrmMisc
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1011, 654);
            this.Controls.Add(this._tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMisc";
            this.Text = "Parameter";
            this.Load += new System.EventHandler(this.FrmMisc_Load);
            this._tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvConstant)).EndInit();
            this.panel14.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvParameters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dsParameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtParameter)).EndInit();
            this.panel15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dsConstant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dtConstant)).EndInit();
            this.ResumeLayout(false);

		}

		public FrmMisc()
		{
			this.InitializeComponent();
		}

		public void Init()
		{
		}

		private void FrmMisc_Load(object sender, EventArgs e)
		{
			try
			{
				this._lstConstants.Columns.Add("Constant Name", this._lstConstants.ClientSize.Width);
				foreach (string constant_key in this.ConstantManager.ConstantList.Keys)
				{
					ListViewItem lvitem = this._lstConstants.Items.Add(constant_key);
					lvitem.Tag = this.ConstantManager.ConstantList[constant_key];
				}
				this._dgvConstant.EditModeChanged += new EventHandler(this._dgvConstant_EditModeChanged);
				if (this._lstConstants.Items.Count > 0)
				{
					this._lstConstants.Items[0].Selected = true;
				}
				this._dtParamType = new DataTable();
				this._dtParamType.Columns.Add("key");
				this._dtParamType.Columns.Add("value");
				DataRow dr = this._dtParamType.NewRow();
				dr["key"] = "integer";
				dr["value"] = "integer";
				this._dtParamType.Rows.Add(dr);
				dr = this._dtParamType.NewRow();
				dr["key"] = "string";
				dr["value"] = "string";
				this._dtParamType.Rows.Add(dr);
				dr = this._dtParamType.NewRow();
				dr["key"] = "bool";
				dr["value"] = "bool";
				this._dtParamType.Rows.Add(dr);
				this.CreateParameterDataSource();
				foreach (DataGridViewColumn col in this._dgvParameters.Columns)
				{
					if (col is DataGridViewComboBoxColumn)
					{
						DataGridViewComboBoxColumn combobox_col = col as DataGridViewComboBoxColumn;
						combobox_col.DataSource = this._dtParamType;
						combobox_col.DisplayMember = "key";
						combobox_col.ValueMember = "value";
					}
				}
				this._dgvParameters.EditModeChanged += new EventHandler(this._dgvParameters_EditModeChanged);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.Close();
			}
		}

		private void CreateConstantDataSource(ConstantData data)
		{
			this._dgvConstant.DataSource = null;
			this._dtConstant.Rows.Clear();
			if (data != null)
			{
				foreach (string key in data.Values.Keys)
				{
					DataRow dr = this._dtConstant.NewRow();
					dr["Key"] = key;
					dr["Value"] = data.Values[key].Value;
					dr["Description"] = data.Values[key].Discription;
					dr["Default"] = (data.DefaultValue.Value == data.Values[key].Value);
					this._dtConstant.Rows.Add(dr);
				}
			}
			this._dgvConstant.DataSource = this._dtConstant;
		}

		private void CreateParameterDataSource()
		{
			this._dtParameter.Clear();
			foreach (string parameter_key in this.ParameterManager.Keys)
			{
				DataRow dr = this._dtParameter.NewRow();
				dr["Name"] = parameter_key;
				dr["Value"] = this.ParameterManager.Parameters[parameter_key].Value;
				dr["Description"] = this.ParameterManager.Parameters[parameter_key].Discription;
				dr["Type"] = this.ParameterManager.Parameters[parameter_key].DataType;
				this._dtParameter.Rows.Add(dr);
			}
		}

		private void _tabMain_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (this._dgvConstant.EditMode != DataGridViewEditMode.EditProgrammatically || this._dgvParameters.EditMode != DataGridViewEditMode.EditProgrammatically)
			{
				e.Cancel = true;
			}
		}

		private void _lstConstants_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._lstConstants.SelectedItems.Count > 0)
			{
				ConstantData data = this._lstConstants.SelectedItems[0].Tag as ConstantData;
				this.CreateConstantDataSource(data);
				this._btnConstantEdit.Enabled = true;
			}
			else
			{
				this.CreateConstantDataSource(null);
				this._btnConstantEdit.Enabled = false;
			}
		}

		private void _dgvConstant_EditModeChanged(object sender, EventArgs e)
		{
			if (this._dgvConstant.EditMode == DataGridViewEditMode.EditProgrammatically)
			{
				this._lstConstants.Enabled = true;
				this._dgvConstant.AllowUserToAddRows = false;
				this._dgvConstant.AllowUserToDeleteRows = false;
				this._btnConstantSave.Enabled = false;
				this._btnConstantEdit.Text = "Edit";
			}
			else
			{
				this._lstConstants.Enabled = false;
				this._dgvConstant.AllowUserToAddRows = true;
				this._dgvConstant.AllowUserToDeleteRows = true;
				this._btnConstantSave.Enabled = true;
				this._btnConstantEdit.Text = "Cancel";
			}
		}

		private void _btnConstantEdit_Click(object sender, EventArgs e)
		{
			if (this._dgvConstant.EditMode == DataGridViewEditMode.EditProgrammatically)
			{
				this._dgvConstant.EditMode = DataGridViewEditMode.EditOnKeystroke;
			}
			else
			{
				this.CreateConstantDataSource(this._lstConstants.SelectedItems[0].Tag as ConstantData);
				this._dgvConstant.EditMode = DataGridViewEditMode.EditProgrammatically;
			}
		}

		private void _btnConstantSave_Click(object sender, EventArgs e)
		{
			try
			{
				ConstantData old_data = this._lstConstants.SelectedItems[0].Tag as ConstantData;
				ConstantData new_data = new ConstantData(old_data.Name);
				foreach (DataRow dr in this._dtConstant.Rows)
				{
					if (new_data.Values.ContainsKey(dr["Key"].ToString()))
					{
						MessageBox.Show(string.Format("Duplicate Key=[{0}]", dr["Key"].ToString()), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						break;
					}
					ConstantItem item = new ConstantItem(dr["Value"].ToString(), dr["Description"].ToString());
					new_data.Values.Add(dr["Key"].ToString(), item);
					if (dr["Default"].ToString() == "True")
					{
						new_data.DefaultValue = new ConstantItem(dr["Value"].ToString(), dr["Description"].ToString());
					}
				}
				this._lstConstants.SelectedItems[0].Tag = new_data;
				this.ConstantManager.ConstantList[new_data.Name] = new_data;
				this._dgvConstant.EditMode = DataGridViewEditMode.EditProgrammatically;
				this.ConstantManager.WriteToXML();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Save File Failed!!" + ex.ToString());
			}
		}

		private void _dgvParameters_EditModeChanged(object sender, EventArgs e)
		{
			if (this._dgvParameters.EditMode == DataGridViewEditMode.EditProgrammatically)
			{
				this._dgvParameters.AllowUserToAddRows = false;
				this._dgvParameters.AllowUserToDeleteRows = false;
				this._btnParameterSave.Enabled = false;
				this._btnParameterEdit.Text = "Edit";
			}
			else
			{
				this._dgvParameters.AllowUserToAddRows = true;
				this._dgvParameters.AllowUserToDeleteRows = true;
				this._btnParameterSave.Enabled = true;
				this._btnParameterEdit.Text = "Cancel";
			}
		}

		private void _btnParameterEdit_Click(object sender, EventArgs e)
		{
			if (this._dgvParameters.EditMode == DataGridViewEditMode.EditProgrammatically)
			{
				this._dgvParameters.EditMode = DataGridViewEditMode.EditOnKeystroke;
			}
			else
			{
				this.CreateParameterDataSource();
				this._dgvParameters.EditMode = DataGridViewEditMode.EditProgrammatically;
			}
		}

		private void _btnParameterSave_Click(object sender, EventArgs e)
		{
			Dictionary<string, string> tmp = new Dictionary<string, string>();
			bool ok = true;
			foreach (DataRow dr in this._dtParameter.Rows)
			{
				if (tmp.ContainsKey(dr["Name"].ToString()))
				{
					MessageBox.Show(string.Format("Duplicate Name=[{0}]", dr["Name"].ToString()), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					ok = false;
					break;
				}
				if (this.CheckDataRow(dr))
				{
					ok = false;
					break;
				}
				if (dr["Name"].ToString() == "OPIMAXCOUNT")
				{
					int max_count = 0;
					ok = (int.TryParse(dr["Value"].ToString(), out max_count) && max_count >= 6 && max_count <= 20);
					if (!ok)
					{
						MessageBox.Show(string.Format("OPIMAXCOUNT[{0}] must be >= 6 and <= 20", dr["Value"].ToString()), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						break;
					}
				}
				tmp.Add(dr["Name"].ToString(), dr["Name"].ToString());
			}
			if (ok)
			{
				this.ParameterManager.Parameters.Clear();
				foreach (DataRow dr in this._dtParameter.Rows)
				{
					ParameterData data = new ParameterData(dr["Name"].ToString());
					data.Value = dr["Value"].ToString();
					data.Discription = dr["Description"].ToString();
					data.DataType = dr["Type"].ToString();
					this.ParameterManager.Parameters.Add(data.Name, data);
				}
				this.CreateParameterDataSource();
				this._dgvParameters.EditMode = DataGridViewEditMode.EditProgrammatically;
				this.ParameterManager.WriteToXML();
			}
		}

		private bool CheckDataRow(DataRow dataRow)
		{
			string value = dataRow["Value"].ToString();
			string type = dataRow["Type"].ToString();
			bool ret = string.IsNullOrEmpty(dataRow["Name"].ToString());
			if (!ret)
			{
				string text = type;
				if (text != null)
				{
					if (!(text == "integer"))
					{
						if (!(text == "string"))
						{
							if (text == "bool")
							{
								ret = (string.Compare(value, "TRUE", true) != 0 && string.Compare(value, "FALSE", true) != 0);
							}
						}
					}
					else
					{
						int result = 0;
						ret = !int.TryParse(value, out result);
					}
				}
				if (ret)
				{
					MessageBox.Show(string.Format("Name=[{0}], Value=[{1}) is not match to Type=[{2}]", dataRow["Name"].ToString(), dataRow["Value"].ToString(), dataRow["Type"].ToString()), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			return ret;
		}

		private void _tabMain_DrawItem(object sender, DrawItemEventArgs e)
		{
			try
			{
				Brush brText = new SolidBrush(Color.White);
				Font ftText = new Font("Microsoft Sans Serif", 10f);
				Rectangle rcItem = this._tabMain.ClientRectangle;
				Brush brBack = new SolidBrush(SystemColors.AppWorkspace);
				e.Graphics.FillRectangle(brBack, rcItem);
				e.Graphics.DrawString(this._tabMain.TabPages[e.Index].Text, ftText, brText, rcItem.Location);
				brBack.Dispose();
				brText.Dispose();
				ftText.Dispose();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}

		private void _dgvConstant_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			this._dgvConstant.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void _dgvConstant_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (this._dgvConstant.Columns[e.ColumnIndex].Name == "Default")
			{
				if (this._dgvConstant.Rows[e.RowIndex].Cells["Default"].Value.ToString() == true.ToString())
				{
					foreach (DataGridViewRow row in ((IEnumerable)this._dgvConstant.Rows))
					{
						if (row.Index != e.RowIndex)
						{
							row.Cells["Default"].Value = false;
						}
					}
				}
			}
		}

		private void _btnParameterRefresh_Click(object sender, EventArgs e)
		{
			this.CreateParameterDataSource();
		}
	}
}
