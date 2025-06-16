namespace KZONE.UI
{
    partial class FrmMaterialDataDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtMaterialID = new System.Windows.Forms.TextBox();
            this.lblMaterialID = new System.Windows.Forms.Label();
            this.txtLineID = new System.Windows.Forms.TextBox();
            this.lblLineID = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.dgvtxtNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtVALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Controls.Add(this.lblFileName);
            this.panel1.Controls.Add(this.txtMaterialID);
            this.panel1.Controls.Add(this.lblMaterialID);
            this.panel1.Controls.Add(this.txtLineID);
            this.panel1.Controls.Add(this.lblLineID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(804, 70);
            this.panel1.TabIndex = 0;
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileName.Location = new System.Drawing.Point(594, 22);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(200, 26);
            this.txtFileName.TabIndex = 14;
            // 
            // lblFileName
            // 
            this.lblFileName.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.Location = new System.Drawing.Point(499, 22);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(80, 23);
            this.lblFileName.TabIndex = 13;
            this.lblFileName.Text = "File Name";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaterialID
            // 
            this.txtMaterialID.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaterialID.Location = new System.Drawing.Point(341, 22);
            this.txtMaterialID.Name = "txtMaterialID";
            this.txtMaterialID.ReadOnly = true;
            this.txtMaterialID.Size = new System.Drawing.Size(144, 26);
            this.txtMaterialID.TabIndex = 12;
            // 
            // lblMaterialID
            // 
            this.lblMaterialID.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaterialID.Location = new System.Drawing.Point(246, 22);
            this.lblMaterialID.Name = "lblMaterialID";
            this.lblMaterialID.Size = new System.Drawing.Size(80, 23);
            this.lblMaterialID.TabIndex = 11;
            this.lblMaterialID.Text = "Material ID";
            this.lblMaterialID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLineID
            // 
            this.txtLineID.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLineID.Location = new System.Drawing.Point(83, 22);
            this.txtLineID.Name = "txtLineID";
            this.txtLineID.ReadOnly = true;
            this.txtLineID.Size = new System.Drawing.Size(144, 26);
            this.txtLineID.TabIndex = 10;
            // 
            // lblLineID
            // 
            this.lblLineID.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineID.Location = new System.Drawing.Point(8, 22);
            this.lblLineID.Name = "lblLineID";
            this.lblLineID.Size = new System.Drawing.Size(60, 23);
            this.lblLineID.TabIndex = 9;
            this.lblLineID.Text = "Line ID";
            this.lblLineID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(804, 531);
            this.panel2.TabIndex = 1;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvData.BackgroundColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtxtNAME,
            this.dgvtxtVALUE});
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvData.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(804, 531);
            this.dgvData.TabIndex = 12;
            // 
            // dgvtxtNAME
            // 
            this.dgvtxtNAME.DataPropertyName = "NAME";
            this.dgvtxtNAME.HeaderText = "Name";
            this.dgvtxtNAME.Name = "dgvtxtNAME";
            this.dgvtxtNAME.ReadOnly = true;
            this.dgvtxtNAME.Width = 400;
            // 
            // dgvtxtVALUE
            // 
            this.dgvtxtVALUE.DataPropertyName = "VALUE";
            this.dgvtxtVALUE.HeaderText = "Value";
            this.dgvtxtVALUE.Name = "dgvtxtVALUE";
            this.dgvtxtVALUE.ReadOnly = true;
            this.dgvtxtVALUE.Width = 400;
            // 
            // FrmMaterialDataDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 601);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMaterialDataDetail";
            this.Text = "FrmMaterialDataDetail";
            this.Load += new System.EventHandler(this.FrmMaterialDataDetail_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtLineID;
        private System.Windows.Forms.Label lblLineID;
        private System.Windows.Forms.TextBox txtMaterialID;
        private System.Windows.Forms.Label lblMaterialID;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtVALUE;
    }
}