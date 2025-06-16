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
    public partial class FormPassWordConfirm : Form
    {
        public FormPassWordConfirm()
        {
            InitializeComponent();
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
           string  apPassword = ConfigurationManager.AppSettings["Password"];

            if (passWord.Text.Trim() == apPassword)
            {

                base.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Password is error.", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
