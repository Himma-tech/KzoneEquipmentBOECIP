using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using KZONE.MessageManager;

namespace KZONE.UI
{
	public class FrmMessage : Form
	{
		private string[] columeName = new string[]
		{
			"Message",
			"Service",
			"Method",
			"State",
			"Count",
			"Minimun",
			"Maximun"
		};

		private DataTable dataTable;

		private IMessageManager _messageManager;

        private IContainer components = null;

        private DataGridView _dgvMessage;

		private TextBox txtMessageID;

		private Label label1;

		private Button button1;

		private Label label2;

		private Label lblMaxConcurrent;

		private Label label4;

		private Label label5;

		private Label lblAgvConcurrent;
        private Panel panel1;

        private Label label7;

		public IMessageManager MessageManager
		{
			get
			{
				return this._messageManager;
			}
			set
			{
				this._messageManager = value;
			}
		}

		public void Init()
		{
		}

		public FrmMessage()
		{
			this.InitializeComponent();
			this.dataTable = new DataTable();
			DataColumn[] columnList = new DataColumn[this.columeName.Length];
			for (int i = 0; i < this.columeName.Length; i++)
			{
				columnList[i] = new DataColumn(this.columeName[i]);
			}
			this.dataTable.Columns.AddRange(columnList);
		}

		private void FrmMessage_Load(object sender, EventArgs e)
		{
            FrmMessage_Activated(sender, e);
		}

		private void FrmMessage_Activated(object sender, EventArgs e)
		{
			this.lblAgvConcurrent.Text = (this._messageManager.AvgConcurrentCount * 1000.0).ToString();
			this.lblMaxConcurrent.Text = (this._messageManager.MaxConcurrentCount * 1000.0).ToString();
			this.dataTable.Clear();
			this._dgvMessage.DataSource = null;
			this._dgvMessage.DataSource = this.MessageManager.GetMessageMap(this.dataTable);
			for (int i = 0; i < this._dgvMessage.Columns.Count; i++)
			{
				this._dgvMessage.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in ((IEnumerable)this._dgvMessage.Rows))
			{
				if (row.Cells["Message"].Value.ToString() == this.txtMessageID.Text.Trim())
				{
					row.Selected = true;
					break;
				}
			}
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._dgvMessage = new System.Windows.Forms.DataGridView();
            this.txtMessageID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMaxConcurrent = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAgvConcurrent = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._dgvMessage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _dgvMessage
            // 
            this._dgvMessage.AllowUserToAddRows = false;
            this._dgvMessage.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgvMessage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dgvMessage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgvMessage.DataMember = "Table1";
            this._dgvMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvMessage.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._dgvMessage.GridColor = System.Drawing.Color.White;
            this._dgvMessage.Location = new System.Drawing.Point(0, 0);
            this._dgvMessage.Name = "_dgvMessage";
            this._dgvMessage.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this._dgvMessage.RowTemplate.Height = 27;
            this._dgvMessage.Size = new System.Drawing.Size(1020, 622);
            this._dgvMessage.TabIndex = 4;
            // 
            // txtMessageID
            // 
            this.txtMessageID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMessageID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageID.Location = new System.Drawing.Point(122, 6);
            this.txtMessageID.Name = "txtMessageID";
            this.txtMessageID.Size = new System.Drawing.Size(209, 22);
            this.txtMessageID.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(24, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Message ID";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(354, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 8;
            this.button1.Text = "Query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(480, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 18);
            this.label2.TabIndex = 9;
            this.label2.Text = "Max Concurrency";
            // 
            // lblMaxConcurrent
            // 
            this.lblMaxConcurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMaxConcurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxConcurrent.ForeColor = System.Drawing.Color.Black;
            this.lblMaxConcurrent.Location = new System.Drawing.Point(599, 10);
            this.lblMaxConcurrent.Name = "lblMaxConcurrent";
            this.lblMaxConcurrent.Size = new System.Drawing.Size(42, 17);
            this.lblMaxConcurrent.TabIndex = 10;
            this.lblMaxConcurrent.Text = "0000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(647, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 18);
            this.label4.TabIndex = 11;
            this.label4.Text = "S";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(850, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 18);
            this.label5.TabIndex = 14;
            this.label5.Text = "S";
            // 
            // lblAgvConcurrent
            // 
            this.lblAgvConcurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAgvConcurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgvConcurrent.ForeColor = System.Drawing.Color.Black;
            this.lblAgvConcurrent.Location = new System.Drawing.Point(802, 11);
            this.lblAgvConcurrent.Name = "lblAgvConcurrent";
            this.lblAgvConcurrent.Size = new System.Drawing.Size(42, 17);
            this.lblAgvConcurrent.TabIndex = 13;
            this.lblAgvConcurrent.Text = "0000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(683, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 18);
            this.label7.TabIndex = 12;
            this.label7.Text = "Agv Concurrency";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._dgvMessage);
            this.panel1.Location = new System.Drawing.Point(2, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1020, 622);
            this.panel1.TabIndex = 15;
            // 
            // FrmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1024, 654);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblAgvConcurrent);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblMaxConcurrent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMessageID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMessage";
            this.Text = "Message";
            this.Activated += new System.EventHandler(this.FrmMessage_Activated);
            this.Load += new System.EventHandler(this.FrmMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this._dgvMessage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
