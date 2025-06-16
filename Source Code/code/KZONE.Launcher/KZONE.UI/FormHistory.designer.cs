namespace KZONE.UI
{
    partial class FormHistory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHistory));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpBase = new System.Windows.Forms.TableLayoutPanel();
            this.bnavRecord = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tlpHeader = new System.Windows.Forms.TableLayoutPanel();
            this.grbLatest = new System.Windows.Forms.GroupBox();
            this.rdoMonth = new System.Windows.Forms.RadioButton();
            this.rdoWeek = new System.Windows.Forms.RadioButton();
            this.rdoDay = new System.Windows.Forms.RadioButton();
            this.rdoHour = new System.Windows.Forms.RadioButton();
            this.grbDateTime = new System.Windows.Forms.GroupBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.flpnlCondition = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.btnVCRResule = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.gbHistory = new System.Windows.Forms.GroupBox();
            this.dgvData = new KZONE.UI.PagedGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Modify = new System.Windows.Forms.ToolStripMenuItem();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.New = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.spcBase)).BeginInit();
            this.spcBase.Panel2.SuspendLayout();
            this.spcBase.SuspendLayout();
            this.tlpBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bnavRecord)).BeginInit();
            this.bnavRecord.SuspendLayout();
            this.tlpHeader.SuspendLayout();
            this.grbLatest.SuspendLayout();
            this.grbDateTime.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.gbHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCaption.Size = new System.Drawing.Size(2233, 30);
            // 
            // spcBase
            // 
            this.spcBase.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            // 
            // spcBase.Panel2
            // 
            this.spcBase.Panel2.Controls.Add(this.tlpBase);
            this.spcBase.Size = new System.Drawing.Size(2313, 1162);
            // 
            // tlpBase
            // 
            this.tlpBase.ColumnCount = 3;
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tlpBase.Controls.Add(this.bnavRecord, 1, 2);
            this.tlpBase.Controls.Add(this.tlpHeader, 1, 0);
            this.tlpBase.Controls.Add(this.gbHistory, 1, 1);
            this.tlpBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBase.Location = new System.Drawing.Point(0, 0);
            this.tlpBase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tlpBase.Name = "tlpBase";
            this.tlpBase.RowCount = 3;
            this.tlpBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tlpBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpBase.Size = new System.Drawing.Size(2313, 1131);
            this.tlpBase.TabIndex = 4;
            // 
            // bnavRecord
            // 
            this.bnavRecord.AddNewItem = null;
            this.bnavRecord.CountItem = this.bindingNavigatorCountItem;
            this.bnavRecord.DeleteItem = null;
            this.bnavRecord.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bnavRecord.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnavRecord.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bnavRecord.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bnavRecord.Location = new System.Drawing.Point(9, 1375);
            this.bnavRecord.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bnavRecord.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bnavRecord.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bnavRecord.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bnavRecord.Name = "bnavRecord";
            this.bnavRecord.PositionItem = this.bindingNavigatorPositionItem;
            this.bnavRecord.Size = new System.Drawing.Size(2874, 39);
            this.bnavRecord.TabIndex = 2;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(51, 36);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "項目總數";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(29, 36);
            this.bindingNavigatorMoveFirstItem.Text = "移到最前面";
            this.bindingNavigatorMoveFirstItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(29, 36);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一個";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 39);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(65, 27);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "目前的位置";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(29, 36);
            this.bindingNavigatorMoveNextItem.Text = "移到下一個";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(29, 36);
            this.bindingNavigatorMoveLastItem.Text = "移到最後面";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // tlpHeader
            // 
            this.tlpHeader.ColumnCount = 4;
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 208F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 285F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 141F));
            this.tlpHeader.Controls.Add(this.grbLatest, 0, 0);
            this.tlpHeader.Controls.Add(this.grbDateTime, 1, 0);
            this.tlpHeader.Controls.Add(this.flpnlCondition, 2, 0);
            this.tlpHeader.Controls.Add(this.pnlButton, 3, 0);
            this.tlpHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHeader.Location = new System.Drawing.Point(11, 4);
            this.tlpHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tlpHeader.Name = "tlpHeader";
            this.tlpHeader.RowCount = 1;
            this.tlpHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeader.Size = new System.Drawing.Size(2291, 140);
            this.tlpHeader.TabIndex = 10;
            // 
            // grbLatest
            // 
            this.grbLatest.Controls.Add(this.rdoMonth);
            this.grbLatest.Controls.Add(this.rdoWeek);
            this.grbLatest.Controls.Add(this.rdoDay);
            this.grbLatest.Controls.Add(this.rdoHour);
            this.grbLatest.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbLatest.Location = new System.Drawing.Point(4, 4);
            this.grbLatest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbLatest.Name = "grbLatest";
            this.grbLatest.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbLatest.Size = new System.Drawing.Size(200, 125);
            this.grbLatest.TabIndex = 6;
            this.grbLatest.TabStop = false;
            this.grbLatest.Text = "The Latest";
            // 
            // rdoMonth
            // 
            this.rdoMonth.Location = new System.Drawing.Point(100, 75);
            this.rdoMonth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoMonth.Name = "rdoMonth";
            this.rdoMonth.Size = new System.Drawing.Size(107, 31);
            this.rdoMonth.TabIndex = 3;
            this.rdoMonth.TabStop = true;
            this.rdoMonth.Tag = "M";
            this.rdoMonth.Text = "1 Month";
            this.rdoMonth.UseVisualStyleBackColor = true;
            this.rdoMonth.CheckedChanged += new System.EventHandler(this.rdoLatest_CheckedChanged);
            // 
            // rdoWeek
            // 
            this.rdoWeek.Location = new System.Drawing.Point(7, 75);
            this.rdoWeek.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoWeek.Name = "rdoWeek";
            this.rdoWeek.Size = new System.Drawing.Size(96, 31);
            this.rdoWeek.TabIndex = 2;
            this.rdoWeek.TabStop = true;
            this.rdoWeek.Tag = "W";
            this.rdoWeek.Text = "1 Week";
            this.rdoWeek.UseVisualStyleBackColor = true;
            this.rdoWeek.CheckedChanged += new System.EventHandler(this.rdoLatest_CheckedChanged);
            // 
            // rdoDay
            // 
            this.rdoDay.Location = new System.Drawing.Point(100, 30);
            this.rdoDay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoDay.Name = "rdoDay";
            this.rdoDay.Size = new System.Drawing.Size(93, 31);
            this.rdoDay.TabIndex = 1;
            this.rdoDay.TabStop = true;
            this.rdoDay.Tag = "D";
            this.rdoDay.Text = "1 Day";
            this.rdoDay.UseVisualStyleBackColor = true;
            this.rdoDay.CheckedChanged += new System.EventHandler(this.rdoLatest_CheckedChanged);
            // 
            // rdoHour
            // 
            this.rdoHour.Location = new System.Drawing.Point(7, 30);
            this.rdoHour.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoHour.Name = "rdoHour";
            this.rdoHour.Size = new System.Drawing.Size(93, 31);
            this.rdoHour.TabIndex = 0;
            this.rdoHour.TabStop = true;
            this.rdoHour.Tag = "H";
            this.rdoHour.Text = "1 Hour";
            this.rdoHour.UseVisualStyleBackColor = true;
            this.rdoHour.CheckedChanged += new System.EventHandler(this.rdoLatest_CheckedChanged);
            // 
            // grbDateTime
            // 
            this.grbDateTime.Controls.Add(this.lblTo);
            this.grbDateTime.Controls.Add(this.dtpStartDate);
            this.grbDateTime.Controls.Add(this.lblFrom);
            this.grbDateTime.Controls.Add(this.dtpEndDate);
            this.grbDateTime.Font = new System.Drawing.Font("Cambria", 11.25F);
            this.grbDateTime.Location = new System.Drawing.Point(212, 4);
            this.grbDateTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbDateTime.Name = "grbDateTime";
            this.grbDateTime.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbDateTime.Size = new System.Drawing.Size(277, 125);
            this.grbDateTime.TabIndex = 5;
            this.grbDateTime.TabStop = false;
            this.grbDateTime.Text = "Duration";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(9, 80);
            this.lblTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(29, 22);
            this.lblTo.TabIndex = 8;
            this.lblTo.Text = "To";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "yyyy / MM / dd HH";
            this.dtpStartDate.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(60, 26);
            this.dtpStartDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(211, 32);
            this.dtpStartDate.TabIndex = 5;
            this.dtpStartDate.Value = new System.DateTime(2014, 12, 2, 0, 0, 0, 0);
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpDateTime_ValueChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(7, 30);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(54, 22);
            this.lblFrom.TabIndex = 7;
            this.lblFrom.Text = "From";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "yyyy / MM / dd HH";
            this.dtpEndDate.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(60, 75);
            this.dtpEndDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(211, 32);
            this.dtpEndDate.TabIndex = 6;
            this.dtpEndDate.Value = new System.DateTime(2014, 12, 9, 0, 0, 0, 0);
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpDateTime_ValueChanged);
            // 
            // flpnlCondition
            // 
            this.flpnlCondition.AutoScroll = true;
            this.flpnlCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpnlCondition.Location = new System.Drawing.Point(497, 4);
            this.flpnlCondition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpnlCondition.Name = "flpnlCondition";
            this.flpnlCondition.Size = new System.Drawing.Size(1649, 132);
            this.flpnlCondition.TabIndex = 7;
            // 
            // pnlButton
            // 
            this.pnlButton.Controls.Add(this.btnVCRResule);
            this.pnlButton.Controls.Add(this.btnQuery);
            this.pnlButton.Controls.Add(this.btnExport);
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButton.Location = new System.Drawing.Point(2154, 4);
            this.pnlButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(133, 132);
            this.pnlButton.TabIndex = 4;
            // 
            // btnVCRResule
            // 
            this.btnVCRResule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVCRResule.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVCRResule.Location = new System.Drawing.Point(3, 48);
            this.btnVCRResule.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnVCRResule.Name = "btnVCRResule";
            this.btnVCRResule.Size = new System.Drawing.Size(125, 38);
            this.btnVCRResule.TabIndex = 21;
            this.btnVCRResule.Text = "DCR Result";
            this.btnVCRResule.UseVisualStyleBackColor = true;
            this.btnVCRResule.Click += new System.EventHandler(this.btnVCRResule_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnQuery.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuery.Location = new System.Drawing.Point(3, 6);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(125, 38);
            this.btnQuery.TabIndex = 20;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExport.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(4, 91);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(125, 38);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // gbHistory
            // 
            this.gbHistory.Controls.Add(this.dgvData);
            this.gbHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbHistory.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbHistory.Location = new System.Drawing.Point(11, 152);
            this.gbHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbHistory.Name = "gbHistory";
            this.gbHistory.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbHistory.Size = new System.Drawing.Size(2291, 944);
            this.gbHistory.TabIndex = 11;
            this.gbHistory.TabStop = false;
            this.gbHistory.Text = "total Count : 0";
            // 
            // dgvData
            // 
            this.dgvData.AllowDrop = true;
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvData.Location = new System.Drawing.Point(4, 27);
            this.dgvData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.PageSize = 18;
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersWidth = 30;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvData.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvData.Size = new System.Drawing.Size(2283, 913);
            this.dgvData.TabIndex = 0;
            this.dgvData.Tables = ((System.ComponentModel.BindingList<System.Data.DataTable>)(resources.GetObject("dgvData.Tables")));
            this.dgvData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellContentClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Modify,
            this.Save,
            this.Delete,
            this.New});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 100);
            // 
            // Modify
            // 
            this.Modify.Name = "Modify";
            this.Modify.Size = new System.Drawing.Size(112, 24);
            this.Modify.Text = "0";
            this.Modify.Click += new System.EventHandler(this.Modify_Click);
            // 
            // Save
            // 
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(112, 24);
            this.Save.Text = "Save";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(112, 24);
            this.Delete.Text = "0";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // New
            // 
            this.New.Name = "New";
            this.New.Size = new System.Drawing.Size(112, 24);
            this.New.Text = "New";
            this.New.Click += new System.EventHandler(this.New_Click);
            // 
            // FormHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(2313, 1162);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormHistory";
            this.Text = "History";
            this.Load += new System.EventHandler(this.FormHistory_Load);
            this.Controls.SetChildIndex(this.spcBase, 0);
            this.spcBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcBase)).EndInit();
            this.spcBase.ResumeLayout(false);
            this.tlpBase.ResumeLayout(false);
            this.tlpBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bnavRecord)).EndInit();
            this.bnavRecord.ResumeLayout(false);
            this.bnavRecord.PerformLayout();
            this.tlpHeader.ResumeLayout(false);
            this.grbLatest.ResumeLayout(false);
            this.grbDateTime.ResumeLayout(false);
            this.grbDateTime.PerformLayout();
            this.pnlButton.ResumeLayout(false);
            this.gbHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBase;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnExport;
        private PagedGridView dgvData;
        private System.Windows.Forms.GroupBox grbLatest;
        private System.Windows.Forms.RadioButton rdoMonth;
        private System.Windows.Forms.RadioButton rdoWeek;
        private System.Windows.Forms.RadioButton rdoDay;
        private System.Windows.Forms.RadioButton rdoHour;
        private System.Windows.Forms.GroupBox grbDateTime;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.FlowLayoutPanel flpnlCondition;
        private System.Windows.Forms.GroupBox gbHistory;
        private System.Windows.Forms.BindingNavigator bnavRecord;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.Button btnVCRResule;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Modify;
        private System.Windows.Forms.ToolStripMenuItem Save;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripMenuItem New;
    }
}