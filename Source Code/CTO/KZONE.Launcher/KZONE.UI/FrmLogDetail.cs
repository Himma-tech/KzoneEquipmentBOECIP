using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KZONE.UI
{
	public class FrmLogDetail : Form
	{
		private string logDetail;

		private IContainer components = null;

		private RichTextBox richTextBox1;

		public string LogDetail
		{
			get
			{
				return this.logDetail;
			}
			set
			{
				this.logDetail = value;
				this.richTextBox1.Text = "";
				this.richTextBox1.Text = this.LogDetail;
			}
		}

		public FrmLogDetail()
		{
			this.InitializeComponent();
		}

		private void FrmLogDetail_Load(object sender, EventArgs e)
		{
		}

		private void FrmLogDetail_FormClosing(object sender, FormClosingEventArgs e)
		{
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
			this.richTextBox1 = new RichTextBox();
			base.SuspendLayout();
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.richTextBox1.Dock = DockStyle.Fill;
			this.richTextBox1.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.richTextBox1.ForeColor = Color.Black;
			this.richTextBox1.Location = new Point(0, 0);
			this.richTextBox1.Margin = new Padding(4);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new Size(623, 202);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(623, 202);
			base.Controls.Add(this.richTextBox1);
			this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Margin = new Padding(4);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmLogDetail";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "LogDetail";
			base.FormClosing += new FormClosingEventHandler(this.FrmLogDetail_FormClosing);
			base.Load += new EventHandler(this.FrmLogDetail_Load);
			base.ResumeLayout(false);
		}
	}
}
