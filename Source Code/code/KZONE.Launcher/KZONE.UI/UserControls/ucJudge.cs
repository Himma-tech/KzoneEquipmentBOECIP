using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KZONE.UI
{
    public partial class ucJudge : UserControl
    {
        List<ItemEntity> lst_Define;

        public delegate void CheckBoxChange_delegate(string ucName, string data);

        public event CheckBoxChange_delegate CheckBoxValueChange;

        private string _defaultData = string.Empty;

        public string DefaultData
        {
            get { return _defaultData; }
            set { _defaultData = value; }
        }

        bool IsRefresh = false; 

        //public string DefaultData
        //{
        //    get
        //    {
        //        StringBuilder _sb = new StringBuilder();

        //        foreach (CheckBox _chk in flpJudge.Controls.OfType<CheckBox>().Where(r => r.Checked))
        //        {
        //            _sb.Append(_chk.Name);
        //        }

        //        return _sb.ToString();
        //    }
        //    set
        //    {               
        //        foreach (CheckBox _chk in flpJudge.Controls.OfType<CheckBox>())
        //        {

        //            if (value.Contains(_chk.Name)) _chk.Checked = true;
        //            else _chk.Checked = false;

        //        }               
        //    }
        //}

        public string RealData
        {
            get 
            {
                StringBuilder _sb = new StringBuilder();

                foreach (CheckBox _chk in flpJudge.Controls.OfType<CheckBox>().Where(r=>r.Checked))
                {
                    _sb.Append(_chk.Name);
                }

                return _sb.ToString(); 
            }
            set 
            {
                IsRefresh = true;

                foreach (CheckBox _chk in flpJudge.Controls.OfType<CheckBox>())
                {

                    if (value.Contains(_chk.Name)) _chk.Checked = true;
                    else _chk.Checked = false;

                }

                IsRefresh = false; 
            }
        }

        public ucJudge()
        {
            InitializeComponent();
        }

        public ucJudge(string caption, string define)
        {
            InitializeComponent();

            grbData.Text = caption;

            lst_Define = UniTools.CreateItemEntity(define);

            InitialObject(lst_Define);
        }

        private void InitialObject(List<ItemEntity> lstDefine)
        {
            try
            {
                flpJudge.Controls.Clear();

                foreach (ItemEntity _item in lstDefine)
                {
                    CheckBox _chk = new CheckBox();
                    _chk.Name = _item.ItemValue;
                    _chk.Text  = _item.ItemName;
                    _chk.Checked = false;
                   // _chk.Image = Properties.Resources.Bit_Red_64;
                    _chk.AutoSize = false;
                    _chk.Size = new Size(95, 87);
                    _chk.Font = new System.Drawing.Font("Calibri", 12F);
                    _chk.Appearance = Appearance.Button;
                    _chk.TextImageRelation = TextImageRelation.ImageAboveText;
                    _chk.TextAlign = ContentAlignment.MiddleCenter;
                    _chk.CheckedChanged += new EventHandler(chk_CheckedChanged);
                    flpJudge.Controls.Add(_chk);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox _chk = (CheckBox)sender;

                //if (_chk.Checked) _chk.Image = Properties.Resources.Bit_Green_64;
                //else _chk.Image = Properties.Resources.Bit_Red_64;

                if (IsRefresh) return;

                if (CheckBoxValueChange != null)
                {
                    CheckBoxValueChange(this.Name, RealData);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void EnableObject(bool enable)
        {
            foreach (CheckBox _chk in flpJudge.Controls.OfType<CheckBox>())
            {
                _chk.AutoCheck = enable;

                if (enable) _chk.BackColor = Color.Gainsboro;
                else _chk.BackColor = Color.DimGray;
            }
        }


    }
}
