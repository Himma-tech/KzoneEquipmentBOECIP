namespace KZONE.UI
{
    partial class ucRTCInfo
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbGlass = new System.Windows.Forms.GroupBox();
            this.flpnlTarget = new System.Windows.Forms.FlowLayoutPanel();
            this.gbGlass.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGlass
            // 
            this.gbGlass.Controls.Add(this.flpnlTarget);
            this.gbGlass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGlass.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbGlass.ForeColor = System.Drawing.Color.Magenta;
            this.gbGlass.Location = new System.Drawing.Point(0, 0);
            this.gbGlass.Name = "gbGlass";
            this.gbGlass.Size = new System.Drawing.Size(711, 100);
            this.gbGlass.TabIndex = 0;
            this.gbGlass.TabStop = false;
            this.gbGlass.Text = "Glass";
            // 
            // flpnlTarget
            // 
            this.flpnlTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpnlTarget.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpnlTarget.ForeColor = System.Drawing.Color.Black;
            this.flpnlTarget.Location = new System.Drawing.Point(3, 22);
            this.flpnlTarget.Name = "flpnlTarget";
            this.flpnlTarget.Size = new System.Drawing.Size(705, 75);
            this.flpnlTarget.TabIndex = 0;
            // 
            // ucRTCInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbGlass);
            this.Name = "ucRTCInfo";
            this.Size = new System.Drawing.Size(711, 100);
            this.gbGlass.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbGlass;
        private System.Windows.Forms.FlowLayoutPanel flpnlTarget;
    }
}
