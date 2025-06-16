namespace KZONE.UI
{
    partial class FormInterface
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnTimingChart = new System.Windows.Forms.Button();
            this.tlpBase = new System.Windows.Forms.TableLayoutPanel();
            this.flpDown = new System.Windows.Forms.FlowLayoutPanel();
            this.flpUp = new System.Windows.Forms.FlowLayoutPanel();
            this.pblJogData = new System.Windows.Forms.Panel();
            this.dgvJob = new System.Windows.Forms.DataGridView();
            this.colDetail = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colStream = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCassetteSeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJobSeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJobID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGroupNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlassType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlassJudge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessSkipFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastGlassFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCIMModeCreate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSamplingFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReserved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInspectionJudgeResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInspectionReservationSignal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessReservationSignal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrackingDataHistory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEquipmentSpecialFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlassID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSorterGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlassGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFromPortNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTargetPortNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTargetSlotNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTargetCassetteID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReserve = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.DebugMode = new System.Windows.Forms.Button();
            this.BitAllON = new System.Windows.Forms.Button();
            this.BitAllOff = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrRefreshPLC = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.spcBase)).BeginInit();
            this.spcBase.Panel2.SuspendLayout();
            this.spcBase.SuspendLayout();
            this.pnlTopBack2.SuspendLayout();
            this.tlpBase.SuspendLayout();
            this.pblJogData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJob)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Size = new System.Drawing.Size(896, 30);
            // 
            // spcBase
            // 
            // 
            // spcBase.Panel2
            // 
            this.spcBase.Panel2.Controls.Add(this.tlpBase);
            this.spcBase.Size = new System.Drawing.Size(956, 809);
            // 
            // pnlTopBack2
            // 
            this.pnlTopBack2.Controls.Add(this.btnTimingChart);
            // 
            // btnTimingChart
            // 
            this.btnTimingChart.BackgroundImage = global::KZONE.UI.Properties.Resources.BtnTimingChart;
            this.btnTimingChart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTimingChart.FlatAppearance.BorderSize = 0;
            this.btnTimingChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimingChart.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimingChart.ForeColor = System.Drawing.Color.Yellow;
            this.btnTimingChart.Location = new System.Drawing.Point(3, 3);
            this.btnTimingChart.Name = "btnTimingChart";
            this.btnTimingChart.Size = new System.Drawing.Size(25, 25);
            this.btnTimingChart.TabIndex = 21;
            this.btnTimingChart.UseVisualStyleBackColor = true;
            this.btnTimingChart.Click += new System.EventHandler(this.btnTimingChart_Click);
            // 
            // tlpBase
            // 
            this.tlpBase.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tlpBase.ColumnCount = 3;
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpBase.Controls.Add(this.flpDown, 1, 0);
            this.tlpBase.Controls.Add(this.flpUp, 0, 0);
            this.tlpBase.Controls.Add(this.pblJogData, 0, 1);
            this.tlpBase.Controls.Add(this.flowLayoutPanel1, 2, 0);
            this.tlpBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBase.Location = new System.Drawing.Point(0, 0);
            this.tlpBase.Name = "tlpBase";
            this.tlpBase.RowCount = 2;
            this.tlpBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.64516F));
            this.tlpBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.35484F));
            this.tlpBase.Size = new System.Drawing.Size(956, 778);
            this.tlpBase.TabIndex = 2;
            // 
            // flpDown
            // 
            this.flpDown.AutoScroll = true;
            this.flpDown.AutoSize = true;
            this.flpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpDown.Location = new System.Drawing.Point(323, 6);
            this.flpDown.Name = "flpDown";
            this.flpDown.Size = new System.Drawing.Size(308, 552);
            this.flpDown.TabIndex = 4;
            // 
            // flpUp
            // 
            this.flpUp.AutoScroll = true;
            this.flpUp.AutoSize = true;
            this.flpUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpUp.Location = new System.Drawing.Point(6, 6);
            this.flpUp.Name = "flpUp";
            this.flpUp.Size = new System.Drawing.Size(308, 552);
            this.flpUp.TabIndex = 3;
            // 
            // pblJogData
            // 
            this.pblJogData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tlpBase.SetColumnSpan(this.pblJogData, 4);
            this.pblJogData.Controls.Add(this.dgvJob);
            this.pblJogData.Location = new System.Drawing.Point(6, 567);
            this.pblJogData.Name = "pblJogData";
            this.pblJogData.Size = new System.Drawing.Size(944, 205);
            this.pblJogData.TabIndex = 2;
            // 
            // dgvJob
            // 
            this.dgvJob.AllowUserToAddRows = false;
            this.dgvJob.AllowUserToDeleteRows = false;
            this.dgvJob.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvJob.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvJob.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvJob.BackgroundColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvJob.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvJob.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJob.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetail,
            this.colStream,
            this.colAddress,
            this.colCassetteSeqNo,
            this.colJobSeqNo,
            this.colJobID,
            this.colGroupNumber,
            this.colGlassType,
            this.colGlassJudge,
            this.colProcessSkipFlag,
            this.colLastGlassFlag,
            this.colCIMModeCreate,
            this.colSamplingFlag,
            this.colReserved,
            this.colInspectionJudgeResult,
            this.colInspectionReservationSignal,
            this.colProcessReservationSignal,
            this.colTrackingDataHistory,
            this.colEquipmentSpecialFlag,
            this.colGlassID,
            this.colSorterGrade,
            this.colGlassGrade,
            this.colFromPortNo,
            this.colTargetPortNo,
            this.colTargetSlotNo,
            this.colTargetCassetteID,
            this.colReserve,
            this.colPPID});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvJob.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvJob.Location = new System.Drawing.Point(-5, 0);
            this.dgvJob.MultiSelect = false;
            this.dgvJob.Name = "dgvJob";
            this.dgvJob.ReadOnly = true;
            this.dgvJob.RowHeadersVisible = false;
            this.dgvJob.RowTemplate.Height = 24;
            this.dgvJob.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvJob.Size = new System.Drawing.Size(953, 207);
            this.dgvJob.TabIndex = 8;
            this.dgvJob.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJob_CellClick);
            // 
            // colDetail
            // 
            this.colDetail.HeaderText = "Detail";
            this.colDetail.Name = "colDetail";
            this.colDetail.ReadOnly = true;
            this.colDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colDetail.Width = 75;
            // 
            // colStream
            // 
            this.colStream.HeaderText = "Stream";
            this.colStream.Name = "colStream";
            this.colStream.ReadOnly = true;
            this.colStream.Width = 85;
            // 
            // colAddress
            // 
            this.colAddress.HeaderText = "Address";
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            this.colAddress.Width = 91;
            // 
            // colCassetteSeqNo
            // 
            this.colCassetteSeqNo.HeaderText = "Cassette Seq No";
            this.colCassetteSeqNo.Name = "colCassetteSeqNo";
            this.colCassetteSeqNo.ReadOnly = true;
            this.colCassetteSeqNo.Width = 115;
            // 
            // colJobSeqNo
            // 
            this.colJobSeqNo.HeaderText = "Job Seq No";
            this.colJobSeqNo.Name = "colJobSeqNo";
            this.colJobSeqNo.ReadOnly = true;
            this.colJobSeqNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colJobSeqNo.Width = 65;
            // 
            // colJobID
            // 
            this.colJobID.HeaderText = "Job ID";
            this.colJobID.Name = "colJobID";
            this.colJobID.ReadOnly = true;
            this.colJobID.Width = 58;
            // 
            // colGroupNumber
            // 
            this.colGroupNumber.HeaderText = "Group Number";
            this.colGroupNumber.Name = "colGroupNumber";
            this.colGroupNumber.ReadOnly = true;
            this.colGroupNumber.Width = 128;
            // 
            // colGlassType
            // 
            this.colGlassType.HeaderText = "Glass Type";
            this.colGlassType.Name = "colGlassType";
            this.colGlassType.ReadOnly = true;
            this.colGlassType.Width = 99;
            // 
            // colGlassJudge
            // 
            this.colGlassJudge.HeaderText = "Glass Judge";
            this.colGlassJudge.Name = "colGlassJudge";
            this.colGlassJudge.ReadOnly = true;
            this.colGlassJudge.Width = 105;
            // 
            // colProcessSkipFlag
            // 
            this.colProcessSkipFlag.HeaderText = "Process Skip Flag";
            this.colProcessSkipFlag.Name = "colProcessSkipFlag";
            this.colProcessSkipFlag.ReadOnly = true;
            this.colProcessSkipFlag.Width = 114;
            // 
            // colLastGlassFlag
            // 
            this.colLastGlassFlag.HeaderText = "Last Glass Flag";
            this.colLastGlassFlag.Name = "colLastGlassFlag";
            this.colLastGlassFlag.ReadOnly = true;
            this.colLastGlassFlag.Width = 124;
            // 
            // colCIMModeCreate
            // 
            this.colCIMModeCreate.HeaderText = "CIM Mode Create";
            this.colCIMModeCreate.Name = "colCIMModeCreate";
            this.colCIMModeCreate.ReadOnly = true;
            this.colCIMModeCreate.Width = 143;
            // 
            // colSamplingFlag
            // 
            this.colSamplingFlag.HeaderText = "Sampling Flag";
            this.colSamplingFlag.Name = "colSamplingFlag";
            this.colSamplingFlag.ReadOnly = true;
            this.colSamplingFlag.Width = 120;
            // 
            // colReserved
            // 
            this.colReserved.HeaderText = "Reserved";
            this.colReserved.Name = "colReserved";
            this.colReserved.ReadOnly = true;
            this.colReserved.Width = 98;
            // 
            // colInspectionJudgeResult
            // 
            this.colInspectionJudgeResult.HeaderText = "Inspection Judge Result";
            this.colInspectionJudgeResult.Name = "colInspectionJudgeResult";
            this.colInspectionJudgeResult.ReadOnly = true;
            this.colInspectionJudgeResult.Width = 140;
            // 
            // colInspectionReservationSignal
            // 
            this.colInspectionReservationSignal.HeaderText = "Inspection Reservation Signal";
            this.colInspectionReservationSignal.Name = "colInspectionReservationSignal";
            this.colInspectionReservationSignal.ReadOnly = true;
            this.colInspectionReservationSignal.Width = 177;
            // 
            // colProcessReservationSignal
            // 
            this.colProcessReservationSignal.HeaderText = "Process Reservation Signal";
            this.colProcessReservationSignal.Name = "colProcessReservationSignal";
            this.colProcessReservationSignal.ReadOnly = true;
            this.colProcessReservationSignal.Width = 161;
            // 
            // colTrackingDataHistory
            // 
            this.colTrackingDataHistory.HeaderText = "Tracking Dat aHistory";
            this.colTrackingDataHistory.Name = "colTrackingDataHistory";
            this.colTrackingDataHistory.ReadOnly = true;
            this.colTrackingDataHistory.Width = 167;
            // 
            // colEquipmentSpecialFlag
            // 
            this.colEquipmentSpecialFlag.HeaderText = "Equipment Special Flag";
            this.colEquipmentSpecialFlag.Name = "colEquipmentSpecialFlag";
            this.colEquipmentSpecialFlag.ReadOnly = true;
            this.colEquipmentSpecialFlag.Width = 152;
            // 
            // colGlassID
            // 
            this.colGlassID.HeaderText = "Glass ID";
            this.colGlassID.Name = "colGlassID";
            this.colGlassID.ReadOnly = true;
            this.colGlassID.Width = 83;
            // 
            // colSorterGrade
            // 
            this.colSorterGrade.HeaderText = "Sorter Grade";
            this.colSorterGrade.Name = "colSorterGrade";
            this.colSorterGrade.ReadOnly = true;
            this.colSorterGrade.Width = 114;
            // 
            // colGlassGrade
            // 
            this.colGlassGrade.HeaderText = "Glass Grade";
            this.colGlassGrade.Name = "colGlassGrade";
            this.colGlassGrade.ReadOnly = true;
            this.colGlassGrade.Width = 108;
            // 
            // colFromPortNo
            // 
            this.colFromPortNo.HeaderText = "From Port No";
            this.colFromPortNo.Name = "colFromPortNo";
            this.colFromPortNo.ReadOnly = true;
            this.colFromPortNo.Width = 101;
            // 
            // colTargetPortNo
            // 
            this.colTargetPortNo.HeaderText = "Target Port No";
            this.colTargetPortNo.Name = "colTargetPortNo";
            this.colTargetPortNo.ReadOnly = true;
            this.colTargetPortNo.Width = 106;
            // 
            // colTargetSlotNo
            // 
            this.colTargetSlotNo.HeaderText = "Target Slot No";
            this.colTargetSlotNo.Name = "colTargetSlotNo";
            this.colTargetSlotNo.ReadOnly = true;
            this.colTargetSlotNo.Width = 104;
            // 
            // colTargetCassetteID
            // 
            this.colTargetCassetteID.HeaderText = "Target Cassette ID";
            this.colTargetCassetteID.Name = "colTargetCassetteID";
            this.colTargetCassetteID.ReadOnly = true;
            this.colTargetCassetteID.Width = 132;
            // 
            // colReserve
            // 
            this.colReserve.HeaderText = "Reserve";
            this.colReserve.Name = "colReserve";
            this.colReserve.ReadOnly = true;
            this.colReserve.Width = 89;
            // 
            // colPPID
            // 
            this.colPPID.HeaderText = "PPID";
            this.colPPID.Name = "colPPID";
            this.colPPID.ReadOnly = true;
            this.colPPID.Width = 67;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.DebugMode);
            this.flowLayoutPanel1.Controls.Add(this.BitAllON);
            this.flowLayoutPanel1.Controls.Add(this.BitAllOff);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(640, 6);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(310, 552);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // DebugMode
            // 
            this.DebugMode.Location = new System.Drawing.Point(3, 3);
            this.DebugMode.Name = "DebugMode";
            this.DebugMode.Size = new System.Drawing.Size(89, 37);
            this.DebugMode.TabIndex = 0;
            this.DebugMode.Text = "Debug Mode";
            this.DebugMode.UseVisualStyleBackColor = true;
            this.DebugMode.Click += new System.EventHandler(this.DebugMode_Click);
            // 
            // BitAllON
            // 
            this.BitAllON.Location = new System.Drawing.Point(98, 3);
            this.BitAllON.Name = "BitAllON";
            this.BitAllON.Size = new System.Drawing.Size(77, 37);
            this.BitAllON.TabIndex = 1;
            this.BitAllON.Text = "Bit All ON";
            this.BitAllON.UseVisualStyleBackColor = true;
            this.BitAllON.Click += new System.EventHandler(this.BitAllON_Click);
            // 
            // BitAllOff
            // 
            this.BitAllOff.Location = new System.Drawing.Point(181, 3);
            this.BitAllOff.Name = "BitAllOff";
            this.BitAllOff.Size = new System.Drawing.Size(89, 37);
            this.BitAllOff.TabIndex = 2;
            this.BitAllOff.Text = "Bit All Off";
            this.BitAllOff.UseVisualStyleBackColor = true;
            this.BitAllOff.Click += new System.EventHandler(this.BitAllOff_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 506);
            this.panel1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(0, 232);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(310, 43);
            this.button2.TabIndex = 0;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(3, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 28);
            this.label5.TabIndex = 5;
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(0, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(310, 43);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(3, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(300, 28);
            this.label4.TabIndex = 3;
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(3, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(300, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "Equipment Status";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(0, 304);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(313, 202);
            this.textBox1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(300, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Equipment Lock ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inline Mode ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tmrRefreshPLC
            // 
            this.tmrRefreshPLC.Interval = 50;
            this.tmrRefreshPLC.Tick += new System.EventHandler(this.tmrRefreshPLC_Tick);
            // 
            // FormInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(956, 809);
            this.ControlBox = true;
            this.Name = "FormInterface";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "   ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormInterface_FormClosed);
            this.Load += new System.EventHandler(this.FormInterface_Load);
            this.spcBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcBase)).EndInit();
            this.spcBase.ResumeLayout(false);
            this.pnlTopBack2.ResumeLayout(false);
            this.tlpBase.ResumeLayout(false);
            this.tlpBase.PerformLayout();
            this.pblJogData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvJob)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTimingChart;
        private System.Windows.Forms.TableLayoutPanel tlpBase;
        private System.Windows.Forms.Panel pblJogData;
        private System.Windows.Forms.DataGridView dgvJob;
        private System.Windows.Forms.FlowLayoutPanel flpUp;
        private System.Windows.Forms.FlowLayoutPanel flpDown;
        private System.Windows.Forms.Timer tmrRefreshPLC;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button DebugMode;
        private System.Windows.Forms.Button BitAllON;
        private System.Windows.Forms.Button BitAllOff;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewButtonColumn colDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStream;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCassetteSeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJobSeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJobID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroupNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlassType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlassJudge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessSkipFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastGlassFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCIMModeCreate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSamplingFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReserved;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInspectionJudgeResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInspectionReservationSignal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessReservationSignal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrackingDataHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEquipmentSpecialFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlassID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSorterGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlassGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFromPortNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTargetPortNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTargetSlotNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTargetCassetteID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReserve;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPPID;
    }
}