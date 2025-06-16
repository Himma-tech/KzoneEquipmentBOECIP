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
    public partial class ucRTCInfo : UserControl
    {
        public string SLotNo = string.Empty;

        //public ucRTCInfo(string slotNo, RTCControlInfo info, Node node)
        //{
        //    InitializeComponent();

        //    SLotNo = slotNo;
        //    gbGlass.Text = "Glass" + slotNo;

        //    CreateRTCInfo("RTC_Target", string.Format("{0}_{1}", node.NodeNo, "RTCTarget"), "chkTargetEQ", info == null ? null : info.TargetEQ);

        //}

        private void CreateRTCInfo(string key, string subKey, string name, List<string> value)
        {
            try
            {
                List<ItemEntity> Lst_rtc = UniTools.CreateItemEntity(UniTools.GetOPIParameterByKey(key, subKey, string.Empty));

                foreach (var item in Lst_rtc)
                {
                    CheckBox chk = new CheckBox();
                    chk.Name = name + item.ItemValue;
                    chk.Text = item.ItemName;
                    chk.Tag = item.ItemValue;
                    chk.Width = 110;
                    chk.Height = 20;
                    if (name.Contains("Target"))
                        flpnlTarget.Controls.Add(chk);

                    if (value != null && value.Count >= Convert.ToInt32(item.ItemValue))
                    {
                        if (value.ToArray()[Convert.ToInt32(item.ItemValue) - 1] != string.Empty)
                            chk.Checked = true;
                        else
                            chk.Checked = false;
                    }
                    else
                    {
                        chk.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void SetChecked(FlowLayoutPanel flp, string chkName, List<string> value)
        {
            try
            {
                var controls = from Control c in flp.Controls
                               where c.Name.Contains(chkName)
                               select c;

                foreach (CheckBox chk in controls)
                {
                    if (value == null)
                        chk.Checked = false;
                    else
                    {
                        chk.Checked = false;
                        string _menber = string.Empty;
                        string[] _NodeNo = chk.Text.Split('_');
                        if (_NodeNo.Length < 2) continue;
                        if (!chk.Text.Contains("#"))
                        {
                            _menber = _NodeNo[0];
                        }
                        else
                        {
                            string[] _no = _NodeNo[1].Split('#');
                            if (_no.Length < 2) continue;
                            if (Convert.ToInt32(_no[1].Substring(_no[1].Length - 1, 1)) == 1)
                            {
                                _menber = _NodeNo[0];
                            }
                            else
                                _menber = _NodeNo[0] + "0" + (Convert.ToInt32(_no[1].Substring(_no[1].Length - 1, 1))-1).ToString();
                        }
                        foreach (string _value in value)
                        {
                            if (_menber == _value)
                            {
                                chk.Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private List<string> GetChecked(FlowLayoutPanel flp, string chkName)
        {
            List<string> value = new List<string>();

            try
            {
                var controls = from Control c in flp.Controls
                               where c.Name.Contains(chkName)
                               select c;

                foreach (CheckBox chk in controls)
                {
                    if (chk.Checked)
                    {
                        string[] nodeNoList = chk.Text.Split('_');
                        if (nodeNoList[1].Substring(nodeNoList[1].Length - 1, 1) == "2")
                            value.Add(nodeNoList[0] + "01");
                        else
                            value.Add(nodeNoList[0]);
                    }
                    else
                        continue;
                }
            }
            catch (Exception ex)
            {
                value = null;
                throw (ex);
            }

            return value;
        }

        public void SetAllUnChecked()
        {
            foreach (Control ctl in flpnlTarget.Controls)
            {
                if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
            }

        }

        //public void IniRTCInfo(RTCControlInfo info)
        //{
        //    try
        //    {
        //        if (info == null)
        //            return;

        //        SetChecked(flpnlTarget, "chkTargetEQ", info.TargetEQ);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        public bool CheckIsSelected()
        {
            bool blnSelected = false;

            try
            {
                foreach (Control ctl in flpnlTarget.Controls)
                {
                    if (ctl is CheckBox)
                    {
                        if (((CheckBox)ctl).Checked)
                        {
                            blnSelected = true;
                            break;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                blnSelected = false;
                throw (ex);
            }

            return blnSelected;
        }

        //public RTCControlInfo GetAllChecked()
        //{
        //    try
        //    {
        //        RTCControlInfo info = new RTCControlInfo();

        //        info.TargetEQ = GetChecked(flpnlTarget, "chkTargetEQ");
        //        info.SlotNo = SLotNo;

        //        return info;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        throw (ex);
        //    }
        //}

    }

}
