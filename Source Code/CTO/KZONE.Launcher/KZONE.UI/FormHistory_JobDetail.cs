using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using KZONE.Log;

namespace KZONE.UI
{
    public partial class FormHistory_GlassDetail : FormBase
    {
        DataGridViewRow RowData;
        public FormHistory_GlassDetail(DataGridViewRow row)
        {
            InitializeComponent();
            this.lblCaption.Text = "Glass Detail";
            RowData = row;

            
        }

        private void FormHistory_JobDetail_Load(object sender, EventArgs e)
        {
            try
            {
                dgvData.Rows.Clear();

                txtLocalID.Text = RowData.Cells["NODEID"].Value.ToString();
                txtGlassID.Text = RowData.Cells["GLASSID"].Value.ToString(); //  JOBID->GLASSID 
                txtCSTSlotNo.Text = RowData.Cells["CASSETTESLOTNO"].Value.ToString();// JOBSEQNO -> CASSETTESLOTNO
                txtCstSeqNo.Text = RowData.Cells["CASSETTESEQNO"].Value.ToString();

                string _trackingData = RowData.Cells["TRACKINGDATAHISTORY"].Value.ToString(); //UniTools.ReverseStr(RowData.Cells["TRACKINGDATA"].Value.ToString()); //TRACKINGDATA -> TRACKINGDATAHISTORY
                string _EQPFlag = RowData.Cells["EQUIPMENTSPECIALFLAG"].Value.ToString();// EQPFLAG -> EQUIPMENTSPECIALFLAG
                string _Inspjudgeddata = RowData.Cells["INSPJUDGEDRESULT"].Value.ToString();// INSPJUDGEDDATA -> INSPJUDGEDRESULT
                
                string _NGMark = RowData.Cells["NGMARK"].Value.ToString(); //add by sy.wu
                string _processReservationSignal = RowData.Cells["PROCESSRESERVATIONSIGNAL"].Value.ToString();
                string _inspectionReservationSignal = RowData.Cells["INSPECTIONRESERVATIONSIGNAL"].Value.ToString();
                //string _lineSpecialFlag = RowData.Cells["LINERECIPENAME"].Value.ToString();
                
                SetData("InspectionJudgeResult", _Inspjudgeddata);
                SetData("InspectionReservationSignal", _inspectionReservationSignal);
                SetData("ProcessReservationSignal", _processReservationSignal);
                SetData("TrackingDataHistory", _trackingData);
                SetData("EquipmentSpecialFlag", _EQPFlag);
                SetData("NGMark", _NGMark);
                //SetData("LineSpecialFlag", _lineSpecialFlag);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }
        private void SetData(string _dataName,string _data) //2017/1/30 前荣加
        {
            try
            {
                int _offset = 0;
                int _len = 0;
                int _itemLen = 0;
                int _itemValue = 0;
                int _memoValue = 0;
                string _memoDesc = string.Empty;
                string _itemBinData = string.Empty;
                string _itemDesc = string.Empty;
                string _convertData = string.Empty;
                string _itemName = string.Empty;

                Dictionary<int, string> _dicItemList;
                OPIInfo OPIap = OPIInfo.CreateInstance();
                DBSETDataContext _ctxBRM = OPIap.DBBRMCtx;
                var _selDataE = from job in _ctxBRM.SBRM_SUBJOBDATA
                                where job.LINETYPE == OPIap.ServerName &&
                                          job.ITEMNAME == _dataName
                                          orderby job.ITEMNAME
                                          select job;

                foreach (SBRM_SUBJOBDATA _detail in _selDataE)
                {
                    int.TryParse(_detail.ITEMLENGTH.ToString(), out _itemLen);



                    if (_data.Length < _itemLen) _data = _data.PadRight(_itemLen, '0');

                    //取得item描述
                    _dicItemList = new Dictionary<int, string>();
                    string[] _tmp = _detail.MEMO.ToString().Split(',');
                    foreach (string _value in _tmp)
                    {
                        string[] _tmp2 = _value.Split(':');

                        if (_tmp2.Length < 2) continue;

                        int.TryParse(_tmp2[0], out _memoValue);
                        _dicItemList.Add(_memoValue, _tmp2[1]);
                    }

                    _itemDesc = _detail.SUBITEMDESC.ToString();

                    if (int.TryParse(_detail.SUBITEMLOFFSET.ToString(), out _offset) == false) continue;
                    if (int.TryParse(_detail.SUBITEMLENGTH.ToString(), out _len) == false) continue;

                    _itemBinData = ReverseStr(_data.Substring(_offset, _len));
                    _itemValue = Convert.ToInt32(_itemBinData, 2);
                    _itemName = _detail.ITEMNAME;

                    if (_dicItemList.ContainsKey(_itemValue))
                    {
                        dgvData.Rows.Add(_itemDesc, string.Format("{0}:{1}", _itemValue, _dicItemList[_itemValue]), _itemName);
                    }
                    else
                    {
                        dgvData.Rows.Add(_itemDesc, _itemValue, _itemName);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public static string ReverseStr(string strSource)
        {
            if (!string.IsNullOrEmpty(strSource))
            {
                try
                {
                    char[] charArray = strSource.ToCharArray();

                    Array.Reverse(charArray);

                    return new string(charArray);
                }
                catch (Exception ex)
                {
                    NLogManager.Logger.LogErrorWrite("","", MethodBase.GetCurrentMethod().Name, ex);
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
