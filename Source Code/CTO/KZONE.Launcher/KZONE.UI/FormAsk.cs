using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KZONE.UI
{
    public partial class FormAsk : Form
    {
        public FormAsk()
        {
            InitializeComponent();
        }

        public FormAsk(string str)
        {
            InitializeComponent();
            lblQuestion.Text = "Do you want to " + str.Trim()+" ?";
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
