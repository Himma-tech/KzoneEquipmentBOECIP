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
            this.colCassette_Sequence_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJob_Sequence_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLot_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduct_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperation_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlassID_or_PanelID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCST_Operation_Mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubstrate_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduct_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJob_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDummy_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSkip_Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcess_Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcess_Reason_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLOT_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlass_Thickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGlass_Degree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInspection_Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJob_Judge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJob_Grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJob_Recovery_Flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStep_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVCR_Read_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaster_Recipe_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colCassette_Sequence_No,
            this.colJob_Sequence_No,
            this.colLot_ID,
            this.colProduct_ID,
            this.colOperation_ID,
            this.colGlassID_or_PanelID,
            this.colCST_Operation_Mode,
            this.colSubstrate_Type,
            this.colProduct_Type,
            this.colJob_Type,
            this.colDummy_Type,
            this.colSkip_Flag,
            this.colProcess_Flag,
            this.colProcess_Reason_Code,
            this.colLOT_Code,
            this.colGlass_Thickness,
            this.colGlass_Degree,
            this.colInspection_Flag,
            this.colJob_Judge,
            this.colJob_Grade,
            this.colJob_Recovery_Flag,
            this.colMode,
            this.colStep_ID,
            this.colVCR_Read_ID,
            this.colMaster_Recipe_ID,
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
            this.dgvJob.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJob_CellContentClick);
            // 
            // colDetail
            // 
            this.colDetail.HeaderText = "Detail";
            this.colDetail.Name = "colDetail";
            this.colDetail.ReadOnly = true;
            this.colDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDetail.Width = 56;
            // 
            // colStream
            // 
            this.colStream.HeaderText = "Stream";
            this.colStream.Name = "colStream";
            this.colStream.ReadOnly = true;
            this.colStream.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStream.Width = 66;
            // 
            // colAddress
            // 
            this.colAddress.HeaderText = "Address";
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            this.colAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAddress.Width = 72;
            // 
            // colCassette_Sequence_No
            // 
            this.colCassette_Sequence_No.HeaderText = "Cassette_Sequence_No";
            this.colCassette_Sequence_No.Name = "colCassette_Sequence_No";
            this.colCassette_Sequence_No.ReadOnly = true;
            this.colCassette_Sequence_No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCassette_Sequence_No.Width = 176;
            // 
            // colJob_Sequence_No
            // 
            this.colJob_Sequence_No.HeaderText = "Job_Sequence_No";
            this.colJob_Sequence_No.Name = "colJob_Sequence_No";
            this.colJob_Sequence_No.ReadOnly = true;
            this.colJob_Sequence_No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colJob_Sequence_No.Width = 141;
            // 
            // colLot_ID
            // 
            this.colLot_ID.HeaderText = "Lot_ID";
            this.colLot_ID.Name = "colLot_ID";
            this.colLot_ID.ReadOnly = true;
            this.colLot_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLot_ID.Width = 60;
            // 
            // colProduct_ID
            // 
            this.colProduct_ID.HeaderText = "Product_ID";
            this.colProduct_ID.Name = "colProduct_ID";
            this.colProduct_ID.ReadOnly = true;
            this.colProduct_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colProduct_ID.Width = 93;
            // 
            // colOperation_ID
            // 
            this.colOperation_ID.HeaderText = "Operation_ID";
            this.colOperation_ID.Name = "colOperation_ID";
            this.colOperation_ID.ReadOnly = true;
            this.colOperation_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOperation_ID.Width = 107;
            // 
            // colGlassID_or_PanelID
            // 
            this.colGlassID_or_PanelID.HeaderText = "GlassID";
            this.colGlassID_or_PanelID.Name = "colGlassID_or_PanelID";
            this.colGlassID_or_PanelID.ReadOnly = true;
            this.colGlassID_or_PanelID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGlassID_or_PanelID.Width = 67;
            // 
            // colCST_Operation_Mode
            // 
            this.colCST_Operation_Mode.HeaderText = "CST_Operation_Mode";
            this.colCST_Operation_Mode.Name = "colCST_Operation_Mode";
            this.colCST_Operation_Mode.ReadOnly = true;
            this.colCST_Operation_Mode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCST_Operation_Mode.Width = 167;
            // 
            // colSubstrate_Type
            // 
            this.colSubstrate_Type.HeaderText = "Substrate_Type";
            this.colSubstrate_Type.Name = "colSubstrate_Type";
            this.colSubstrate_Type.ReadOnly = true;
            this.colSubstrate_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSubstrate_Type.Width = 123;
            // 
            // colProduct_Type
            // 
            this.colProduct_Type.HeaderText = "Product_Type";
            this.colProduct_Type.Name = "colProduct_Type";
            this.colProduct_Type.ReadOnly = true;
            this.colProduct_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colProduct_Type.Width = 111;
            // 
            // colJob_Type
            // 
            this.colJob_Type.HeaderText = "Job_Type";
            this.colJob_Type.Name = "colJob_Type";
            this.colJob_Type.ReadOnly = true;
            this.colJob_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colJob_Type.Width = 79;
            // 
            // colDummy_Type
            // 
            this.colDummy_Type.HeaderText = "Dummy_Type";
            this.colDummy_Type.Name = "colDummy_Type";
            this.colDummy_Type.ReadOnly = true;
            this.colDummy_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDummy_Type.Width = 111;
            // 
            // colSkip_Flag
            // 
            this.colSkip_Flag.HeaderText = "Skip_Flag";
            this.colSkip_Flag.Name = "colSkip_Flag";
            this.colSkip_Flag.ReadOnly = true;
            this.colSkip_Flag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSkip_Flag.Width = 81;
            // 
            // colProcess_Flag
            // 
            this.colProcess_Flag.HeaderText = "Process_Flag";
            this.colProcess_Flag.Name = "colProcess_Flag";
            this.colProcess_Flag.ReadOnly = true;
            this.colProcess_Flag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colProcess_Flag.Width = 105;
            // 
            // colProcess_Reason_Code
            // 
            this.colProcess_Reason_Code.HeaderText = "Process_Reason_Code";
            this.colProcess_Reason_Code.Name = "colProcess_Reason_Code";
            this.colProcess_Reason_Code.ReadOnly = true;
            this.colProcess_Reason_Code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colProcess_Reason_Code.Width = 170;
            // 
            // colLOT_Code
            // 
            this.colLOT_Code.HeaderText = "LOT_Code";
            this.colLOT_Code.Name = "colLOT_Code";
            this.colLOT_Code.ReadOnly = true;
            this.colLOT_Code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLOT_Code.Width = 85;
            // 
            // colGlass_Thickness
            // 
            this.colGlass_Thickness.HeaderText = "Glass_Thickness";
            this.colGlass_Thickness.Name = "colGlass_Thickness";
            this.colGlass_Thickness.ReadOnly = true;
            this.colGlass_Thickness.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGlass_Thickness.Width = 128;
            // 
            // colGlass_Degree
            // 
            this.colGlass_Degree.HeaderText = "Glass_Degree";
            this.colGlass_Degree.Name = "colGlass_Degree";
            this.colGlass_Degree.ReadOnly = true;
            this.colGlass_Degree.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGlass_Degree.Width = 109;
            // 
            // colInspection_Flag
            // 
            this.colInspection_Flag.HeaderText = "Inspection_Flag";
            this.colInspection_Flag.Name = "colInspection_Flag";
            this.colInspection_Flag.ReadOnly = true;
            this.colInspection_Flag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colInspection_Flag.Width = 123;
            // 
            // colJob_Judge
            // 
            this.colJob_Judge.HeaderText = "Job_Judge";
            this.colJob_Judge.Name = "colJob_Judge";
            this.colJob_Judge.ReadOnly = true;
            this.colJob_Judge.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colJob_Judge.Width = 86;
            // 
            // colJob_Grade
            // 
            this.colJob_Grade.HeaderText = "Job_Grade";
            this.colJob_Grade.Name = "colJob_Grade";
            this.colJob_Grade.ReadOnly = true;
            this.colJob_Grade.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colJob_Grade.Width = 89;
            // 
            // colJob_Recovery_Flag
            // 
            this.colJob_Recovery_Flag.HeaderText = "Job_Recovery_Flag";
            this.colJob_Recovery_Flag.Name = "colJob_Recovery_Flag";
            this.colJob_Recovery_Flag.ReadOnly = true;
            this.colJob_Recovery_Flag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colJob_Recovery_Flag.Width = 146;
            // 
            // colMode
            // 
            this.colMode.HeaderText = "Mode";
            this.colMode.Name = "colMode";
            this.colMode.ReadOnly = true;
            this.colMode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMode.Width = 57;
            // 
            // colStep_ID
            // 
            this.colStep_ID.HeaderText = "Step_ID";
            this.colStep_ID.Name = "colStep_ID";
            this.colStep_ID.ReadOnly = true;
            this.colStep_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStep_ID.Width = 69;
            // 
            // colVCR_Read_ID
            // 
            this.colVCR_Read_ID.HeaderText = "VCR_Read_ID";
            this.colVCR_Read_ID.Name = "colVCR_Read_ID";
            this.colVCR_Read_ID.ReadOnly = true;
            this.colVCR_Read_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colVCR_Read_ID.Width = 108;
            // 
            // colMaster_Recipe_ID
            // 
            this.colMaster_Recipe_ID.HeaderText = "Master_Recipe_ID";
            this.colMaster_Recipe_ID.Name = "colMaster_Recipe_ID";
            this.colMaster_Recipe_ID.ReadOnly = true;
            this.colMaster_Recipe_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMaster_Recipe_ID.Width = 141;
            // 
            // colPPID
            // 
            this.colPPID.HeaderText = "PPID";
            this.colPPID.Name = "colPPID";
            this.colPPID.ReadOnly = true;
            this.colPPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPPID.Width = 48;
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
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(3, 232);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(300, 43);
            this.button2.TabIndex = 7;
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
            this.button1.Location = new System.Drawing.Point(3, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(300, 43);
            this.button1.TabIndex = 4;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colCassette_Sequence_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJob_Sequence_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLot_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduct_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperation_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlassID_or_PanelID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCST_Operation_Mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubstrate_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduct_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJob_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDummy_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSkip_Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcess_Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcess_Reason_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLOT_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlass_Thickness;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGlass_Degree;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInspection_Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJob_Judge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJob_Grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJob_Recovery_Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStep_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVCR_Read_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaster_Recipe_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPPID;
    }
}