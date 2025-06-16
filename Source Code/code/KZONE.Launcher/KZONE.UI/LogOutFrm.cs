using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace KZONE.UI
{
    public partial class LogOutFrm : Form
    {
        public LogOutFrm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text;
            string password = this.txtPassword.Text;
            string apUser = "";
            string apPassword = "";
            try
            {
                apUser = ConfigurationManager.AppSettings["UserName"];
                apPassword = ConfigurationManager.AppSettings["Password"];
            }
            catch
            {
            }
            base.DialogResult = DialogResult.Cancel;
            if (string.IsNullOrEmpty(apUser) && string.IsNullOrEmpty(apPassword))
            {
                if (userName == "Unicom" && password == "Unicom")
                {
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("User name or Password is error.", "Exit Server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else if (userName.Equals(apUser) && password.Equals(apPassword))
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("User name or Password is error.", "Exit Server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            base.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.btnOK_Click(sender, e);
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            this.txtUserName.Text = "CIM";
            this.txtPassword.Text = "123456";
        }

    }
}
