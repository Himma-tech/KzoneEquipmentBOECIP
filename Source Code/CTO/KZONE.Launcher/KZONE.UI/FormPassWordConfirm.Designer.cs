namespace KZONE.UI
{
    partial class FormPassWordConfirm
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
            this.btConfirm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.passWord = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btConfirm
            // 
            this.btConfirm.Location = new System.Drawing.Point(234, 75);
            this.btConfirm.Name = "btConfirm";
            this.btConfirm.Size = new System.Drawing.Size(85, 30);
            this.btConfirm.TabIndex = 0;
            this.btConfirm.Text = "确定";
            this.btConfirm.UseVisualStyleBackColor = true;
            this.btConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(38, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "   请输入密码确认操作";
            // 
            // passWord
            // 
            this.passWord.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.passWord.Location = new System.Drawing.Point(42, 75);
            this.passWord.Multiline = true;
            this.passWord.Name = "passWord";
            this.passWord.PasswordChar = '*';
            this.passWord.Size = new System.Drawing.Size(170, 30);
            this.passWord.TabIndex = 2;
            // 
            // FormPassWordConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(358, 151);
            this.Controls.Add(this.passWord);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btConfirm);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPassWordConfirm";
            this.Text = "FormPassWordConfirm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btConfirm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passWord;
    }
}