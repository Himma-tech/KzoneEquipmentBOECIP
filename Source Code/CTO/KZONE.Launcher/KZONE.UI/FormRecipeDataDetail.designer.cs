namespace KZONE.UI
{
    partial class FormRecipeDataDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.dgvtxtNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtxtVALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbxGridData = new System.Windows.Forms.GroupBox();
            this.gbxBody = new System.Windows.Forms.GroupBox();
            this.gpxHeader = new System.Windows.Forms.GroupBox();
            this.txtRecipeID = new System.Windows.Forms.TextBox();
            this.lblJobID = new System.Windows.Forms.Label();
            this.txtRecipeNo = new System.Windows.Forms.TextBox();
            this.txtCreateTime = new System.Windows.Forms.TextBox();
            this.txtRecipeStatus = new System.Windows.Forms.TextBox();
            this.lblNodeNo = new System.Windows.Forms.Label();
            this.lblCstSeqNo = new System.Windows.Forms.Label();
            this.lblJobSeqNo = new System.Windows.Forms.Label();
            this.txtVersionNo = new System.Windows.Forms.TextBox();
            this.VersionNo = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.spcBase)).BeginInit();
            this.spcBase.Panel2.SuspendLayout();
            this.spcBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.gbxGridData.SuspendLayout();
            this.gbxBody.SuspendLayout();
            this.gpxHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Size = new System.Drawing.Size(788, 30);
            // 
            // spcBase
            // 
            // 
            // spcBase.Panel2
            // 
            this.spcBase.Panel2.Controls.Add(this.gbxGridData);
            this.spcBase.Size = new System.Drawing.Size(848, 562);
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.BackgroundColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtxtNAME,
            this.dgvtxtVALUE});
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(3, 21);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvData.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(836, 383);
            this.dgvData.TabIndex = 11;
            // 
            // dgvtxtNAME
            // 
            this.dgvtxtNAME.DataPropertyName = "NAME";
            this.dgvtxtNAME.HeaderText = "Name";
            this.dgvtxtNAME.Name = "dgvtxtNAME";
            this.dgvtxtNAME.ReadOnly = true;
            this.dgvtxtNAME.Width = 450;
            // 
            // dgvtxtVALUE
            // 
            this.dgvtxtVALUE.DataPropertyName = "VALUE";
            this.dgvtxtVALUE.HeaderText = "Value";
            this.dgvtxtVALUE.Name = "dgvtxtVALUE";
            this.dgvtxtVALUE.ReadOnly = true;
            this.dgvtxtVALUE.Width = 483;
            // 
            // gbxGridData
            // 
            this.gbxGridData.Controls.Add(this.gbxBody);
            this.gbxGridData.Controls.Add(this.gpxHeader);
            this.gbxGridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxGridData.Location = new System.Drawing.Point(0, 0);
            this.gbxGridData.Name = "gbxGridData";
            this.gbxGridData.Size = new System.Drawing.Size(848, 531);
            this.gbxGridData.TabIndex = 12;
            this.gbxGridData.TabStop = false;
            // 
            // gbxBody
            // 
            this.gbxBody.Controls.Add(this.dgvData);
            this.gbxBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxBody.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxBody.Location = new System.Drawing.Point(3, 121);
            this.gbxBody.Name = "gbxBody";
            this.gbxBody.Size = new System.Drawing.Size(842, 407);
            this.gbxBody.TabIndex = 13;
            this.gbxBody.TabStop = false;
            this.gbxBody.Text = "Detail";
            // 
            // gpxHeader
            // 
            this.gpxHeader.Controls.Add(this.txtFileName);
            this.gpxHeader.Controls.Add(this.label1);
            this.gpxHeader.Controls.Add(this.txtVersionNo);
            this.gpxHeader.Controls.Add(this.VersionNo);
            this.gpxHeader.Controls.Add(this.txtRecipeID);
            this.gpxHeader.Controls.Add(this.lblJobID);
            this.gpxHeader.Controls.Add(this.txtRecipeNo);
            this.gpxHeader.Controls.Add(this.txtCreateTime);
            this.gpxHeader.Controls.Add(this.txtRecipeStatus);
            this.gpxHeader.Controls.Add(this.lblNodeNo);
            this.gpxHeader.Controls.Add(this.lblCstSeqNo);
            this.gpxHeader.Controls.Add(this.lblJobSeqNo);
            this.gpxHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpxHeader.Location = new System.Drawing.Point(3, 17);
            this.gpxHeader.Name = "gpxHeader";
            this.gpxHeader.Size = new System.Drawing.Size(842, 104);
            this.gpxHeader.TabIndex = 12;
            this.gpxHeader.TabStop = false;
            // 
            // txtRecipeID
            // 
            this.txtRecipeID.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecipeID.Location = new System.Drawing.Point(376, 26);
            this.txtRecipeID.Name = "txtRecipeID";
            this.txtRecipeID.ReadOnly = true;
            this.txtRecipeID.Size = new System.Drawing.Size(144, 26);
            this.txtRecipeID.TabIndex = 10;
            // 
            // lblJobID
            // 
            this.lblJobID.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJobID.Location = new System.Drawing.Point(268, 28);
            this.lblJobID.Name = "lblJobID";
            this.lblJobID.Size = new System.Drawing.Size(100, 23);
            this.lblJobID.TabIndex = 9;
            this.lblJobID.Text = "Recipe ID";
            this.lblJobID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRecipeNo
            // 
            this.txtRecipeNo.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecipeNo.Location = new System.Drawing.Point(118, 24);
            this.txtRecipeNo.Name = "txtRecipeNo";
            this.txtRecipeNo.ReadOnly = true;
            this.txtRecipeNo.Size = new System.Drawing.Size(144, 26);
            this.txtRecipeNo.TabIndex = 8;
            // 
            // txtCreateTime
            // 
            this.txtCreateTime.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreateTime.Location = new System.Drawing.Point(374, 68);
            this.txtCreateTime.Name = "txtCreateTime";
            this.txtCreateTime.ReadOnly = true;
            this.txtCreateTime.Size = new System.Drawing.Size(144, 26);
            this.txtCreateTime.TabIndex = 6;
            // 
            // txtRecipeStatus
            // 
            this.txtRecipeStatus.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecipeStatus.Location = new System.Drawing.Point(116, 68);
            this.txtRecipeStatus.Name = "txtRecipeStatus";
            this.txtRecipeStatus.ReadOnly = true;
            this.txtRecipeStatus.Size = new System.Drawing.Size(144, 26);
            this.txtRecipeStatus.TabIndex = 5;
            // 
            // lblNodeNo
            // 
            this.lblNodeNo.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNodeNo.Location = new System.Drawing.Point(3, 26);
            this.lblNodeNo.Name = "lblNodeNo";
            this.lblNodeNo.Size = new System.Drawing.Size(100, 23);
            this.lblNodeNo.TabIndex = 3;
            this.lblNodeNo.Text = "Recipe No";
            this.lblNodeNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCstSeqNo
            // 
            this.lblCstSeqNo.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCstSeqNo.Location = new System.Drawing.Point(271, 70);
            this.lblCstSeqNo.Name = "lblCstSeqNo";
            this.lblCstSeqNo.Size = new System.Drawing.Size(97, 23);
            this.lblCstSeqNo.TabIndex = 1;
            this.lblCstSeqNo.Text = "Create Time";
            this.lblCstSeqNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJobSeqNo
            // 
            this.lblJobSeqNo.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJobSeqNo.Location = new System.Drawing.Point(1, 70);
            this.lblJobSeqNo.Name = "lblJobSeqNo";
            this.lblJobSeqNo.Size = new System.Drawing.Size(100, 23);
            this.lblJobSeqNo.TabIndex = 0;
            this.lblJobSeqNo.Text = "Recipe Status";
            this.lblJobSeqNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtVersionNo
            // 
            this.txtVersionNo.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVersionNo.Location = new System.Drawing.Point(641, 23);
            this.txtVersionNo.Name = "txtVersionNo";
            this.txtVersionNo.ReadOnly = true;
            this.txtVersionNo.Size = new System.Drawing.Size(184, 26);
            this.txtVersionNo.TabIndex = 12;
            // 
            // VersionNo
            // 
            this.VersionNo.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionNo.Location = new System.Drawing.Point(535, 29);
            this.VersionNo.Name = "VersionNo";
            this.VersionNo.Size = new System.Drawing.Size(100, 23);
            this.VersionNo.TabIndex = 11;
            this.VersionNo.Text = "Version No";
            this.VersionNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileName.Location = new System.Drawing.Point(610, 67);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(229, 26);
            this.txtFileName.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(529, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "File Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormRecipeDataDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(848, 562);
            this.ControlBox = true;
            this.Name = "FormRecipeDataDetail";
            this.Text = "  ";
            this.Load += new System.EventHandler(this.FormProcessDataHistoryDetail_Load);
            this.spcBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcBase)).EndInit();
            this.spcBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.gbxGridData.ResumeLayout(false);
            this.gbxBody.ResumeLayout(false);
            this.gpxHeader.ResumeLayout(false);
            this.gpxHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.GroupBox gbxGridData;
        private System.Windows.Forms.GroupBox gbxBody;
        private System.Windows.Forms.GroupBox gpxHeader;
        private System.Windows.Forms.TextBox txtRecipeNo;
        private System.Windows.Forms.TextBox txtCreateTime;
        private System.Windows.Forms.TextBox txtRecipeStatus;
        private System.Windows.Forms.Label lblNodeNo;
        private System.Windows.Forms.Label lblCstSeqNo;
        private System.Windows.Forms.Label lblJobSeqNo;
        private System.Windows.Forms.TextBox txtRecipeID;
        private System.Windows.Forms.Label lblJobID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtxtVALUE;
        private System.Windows.Forms.TextBox txtVersionNo;
        private System.Windows.Forms.Label VersionNo;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
    }
}