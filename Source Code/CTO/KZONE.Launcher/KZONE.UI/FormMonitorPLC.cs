using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using KZONE.Log;
using UNILAYOUT;
using KZONE.ConstantParameter;
using KZONE.Service;
using KZONE.EntityManager;
using KZONE.Entity;
using KZONE.MessageManager;
using System.Xml;
using KZONE.Work;
using KZONE.PLCAgent.PLC;

namespace KZONE.UI
{
    public partial class FormMonitorPLC : FormBase
    {
        string CurNodeNo = string.Empty;

        List<PLCTrxNameReply.PLCTRXc> PlcTrx;
        AbstractMethod ab = new AbstractMethod();

        public FormMonitorPLC()
        {
            InitializeComponent();
        }
        public void Init()
        { }

        private void FormMonitorPLC_Load(object sender, EventArgs e)
        {

            chkDetail.Checked = false;

            PlcTrx = new List<PLCTrxNameReply.PLCTRXc>();

            ColumsVisible(false);
            
            UniTools.SetComboBox_Node(cboLocalNode);

            cboLocalNode.SelectedValueChanged += new EventHandler(cboLocalNode_SelectedValueChanged);

            if (cboLocalNode.Items.Count > 0 ) cboLocalNode.SelectedIndex = 0;
            
        }

        private void ColumsVisible(bool IsVisible)
        {
            try
            {
                dgvTrxData.Columns["colGroupName"].Visible = IsVisible;
                dgvTrxData.Columns["colDir"].Visible = IsVisible;
                dgvTrxData.Columns["colEventName"].Visible = IsVisible;
                dgvTrxData.Columns["colDevcode"].Visible = IsVisible;
                dgvTrxData.Columns["colAddress"].Visible = IsVisible;
                dgvTrxData.Columns["colRAddress"].Visible = IsVisible; //add by sy.wu
                dgvTrxData.Columns["colPoints"].Visible = IsVisible;
                dgvTrxData.Columns["colSkipDecode"].Visible = IsVisible;

                dgvTrxData.Columns["colWOffset"].Visible = IsVisible;
                dgvTrxData.Columns["colWPoints"].Visible = IsVisible;
                dgvTrxData.Columns["colBOffset"].Visible = IsVisible;
                dgvTrxData.Columns["colBPoints"].Visible = IsVisible;
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
               
                PLCTrxNameReply reply = new PLCTrxNameReply();

               

                if (PlcTrx.Count == 0)
                {
                    #region Send PLCTrxNameRequest
                      //取得PLCAgent PLCFormat.xml
                    ab = AbstractMethod.CreateInstance();
                    IServerAgent plcAgent = ab.GetServerAgent(eAgentName.PLCAgent);
                    XmlDocument trxXml = new XmlDocument();
                    trxXml.Load(plcAgent.FormatFileName);
                    XmlNodeList trxNodes = trxXml.SelectNodes("//plcdriver/transaction/receive/trx");
                    foreach (XmlNode node in trxNodes)
                    {
                        PLCTrxNameReply.PLCTRXc trx = new PLCTrxNameReply.PLCTRXc();
                        XmlElement element = (XmlElement)node;
                        trx.PLCTRXNAME = element.GetAttribute("name");

                        reply.BODY.PLCTRXLIST.Add(trx);
                    }
                    //增加 BCS Send的Trx 
                    trxNodes = trxXml.SelectNodes("//plcdriver/transaction/send/trx");
                    foreach (XmlNode node in trxNodes)
                    {
                        PLCTrxNameReply.PLCTRXc trx = new PLCTrxNameReply.PLCTRXc();
                        XmlElement element = (XmlElement)node;
                        trx.PLCTRXNAME = element.GetAttribute("name");

                        reply.BODY.PLCTRXLIST.Add(trx);
                    }
                    #endregion

                  
                    #region Update Data
                    PlcTrx = new List<PLCTrxNameReply.PLCTRXc>(reply.BODY.PLCTRXLIST.ToArray());
                    #endregion

                }

                GetPLCTrxNameList();
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);                
            }
        }

         private void GetPLCTrxNameList()
        {
            try
            {
                if (CurNodeNo == string.Empty) return;

                string _key = string.Format("{0}_", CurNodeNo);

                if (dgvTrxName.Rows.Count > 0)
                {
                    if (dgvTrxName.Rows[0].Cells[colPLCTrxName.Name].Value.ToString().StartsWith(_key))
                        return;
                }

                foreach (PLCTrxNameReply.PLCTRXc _plcTrx in PlcTrx)
                {
                    if (_plcTrx.PLCTRXNAME.Contains("CassetteCommand_Port")) continue;
                    if (_plcTrx.PLCTRXNAME.StartsWith(_key))

                        dgvTrxName.Rows.Add(_plcTrx.PLCTRXNAME);
                }
                if (dgvTrxName.Rows.Count > 0) dgvTrxName.ClearSelection();                
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

   
        private void dgvTrxName_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1) return;
                if (dgvTrxName.CurrentCell == null) return;
                PLCTrxDataReply reply = new PLCTrxDataReply();

                DataGridViewRow selectedRow = dgvTrxName.SelectedRows[0];

                #region PLCTrxDataRequest
              
                string trxName = selectedRow.Cells[colPLCTrxName.Name].Value.ToString();

                 Trx trx = ab.Invoke(eAgentName.PLCAgent, "GetTransactionFormat", new object[] { trxName }) as Trx;
                  
                    if (trx != null) trx = ab.Invoke(eAgentName.PLCAgent, "SyncReadTrx", new object[] { trxName, false }) as Trx;
                    if (trx == null)
                    { }
                    else
                    {
                     
                        foreach (EventGroup group in trx.EventGroups.AllValues)
                        {
                            PLCTrxDataReply.EVENTGROUPc _group = new PLCTrxDataReply.EVENTGROUPc();
                            _group.NAME = group.Metadata.Name;
                            _group.DIR = group.Metadata.Dir.ToString();

                            foreach (Event evt in group.Events.AllValues)
                            {
                                PLCTrxDataReply.EVENTc _evt = new PLCTrxDataReply.EVENTc();
                                _evt.NAME = evt.Metadata.Name;
                                _evt.DEVCODE = evt.Metadata.DeviceCode;
                                _evt.ADDR = evt.Metadata.Address;
                                _evt.POINTS = evt.Metadata.Points.ToString();
                                _evt.SKIPDECODE = evt.Metadata.SkipDecode.ToString();

                                foreach (KZONE.PLCAgent.PLC.Item item in evt.Items.AllValues)
                                {
                                    PLCTrxDataReply.ITEMc _item = new PLCTrxDataReply.ITEMc();
                                    _item.NAME = item.Name;
                                    _item.VAL = item.Value;
                                    _item.WOFFSET = item.Metadata.WordOffset.ToString();
                                    _item.WPOINTS = item.Metadata.WordPoints.ToString();
                                    _item.BOFFSET = item.Metadata.BitOffset.ToString();
                                    _item.BPOINTS = item.Metadata.BitPoints.ToString();
                                    _item.EXPERESSION = item.Metadata.Expression.ToString();

                                    _evt.ITEMLIST.Add(_item);
                                }
                                _group.EVENTLIST.Add(_evt);
                            }
                            reply.BODY.EVENTGROUPLIST.Add(_group);
                        }
                    }

                #endregion

                #region PLCTrxDataReply

                
                #region Update Data
                string[] columns = new string[] { "EVENT_GROUP_NAME", "DIR", "EVENT_NAME", "DEVCODE", "ADDR", "POINTS", "SKIPDECODE", "ITEM_NAME", "VAL", "WOFFSET", "WPOINTS", "BOFFSET", "BPOINTS", "EXPRESSION", "REALADDR" };

                DataTable dt = UniTools.InitDt(columns);

                DataRow drNew = null;
                foreach (PLCTrxDataReply.EVENTGROUPc grp in reply.BODY.EVENTGROUPLIST)
                {
                    foreach (PLCTrxDataReply.EVENTc evt in grp.EVENTLIST)
                    {
                        foreach (PLCTrxDataReply.ITEMc itm in evt.ITEMLIST)
                        {
                            #region 運算Real Address   add by sy.wu
                            string strA = Convert.ToInt32(evt.ADDR, 16).ToString();
                            string strB = itm.WOFFSET.ToString();
                            string sum = (int.Parse(strA) + int.Parse(strB)).ToString();
                            string strAddr = Convert.ToString(int.Parse(sum), 16);
                            string AddrNew = strAddr.PadLeft(7, '0').ToUpper();
                            #endregion

                            drNew = dt.NewRow();
                            drNew["EVENT_GROUP_NAME"] = grp.NAME;
                            drNew["DIR"] = grp.DIR;
                            drNew["EVENT_NAME"] = evt.NAME;
                            drNew["DEVCODE"] = evt.DEVCODE;
                            drNew["ADDR"] = evt.ADDR;
                            drNew["REALADDR"] = AddrNew;
                            drNew["POINTS"] = evt.POINTS;
                            drNew["SKIPDECODE"] = evt.SKIPDECODE;
                            drNew["ITEM_NAME"] = itm.NAME;
                            drNew["VAL"] = itm.VAL;
                            drNew["WOFFSET"] = itm.WOFFSET;
                            drNew["WPOINTS"] = itm.WPOINTS;
                            drNew["BOFFSET"] = itm.BOFFSET;
                            drNew["BPOINTS"] = itm.BPOINTS;
                            drNew["EXPRESSION"] = itm.EXPERESSION;

                            dt.Rows.Add(drNew);
                        }
                    }
                }

                dgvTrxData.DataSource = dt;

                if (dgvTrxData.Rows.Count > 0) dgvTrxData.ClearSelection();
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void chkDetail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool IsVisible = ((CheckBox)sender).Checked;

                ColumsVisible(IsVisible);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
            }
        }

        private void cboLocalNode_SelectedValueChanged(object sender, EventArgs e)
        {
             try
            {
                if (cboLocalNode.SelectedValue != null)
                {
                    CurNodeNo = cboLocalNode.SelectedValue.ToString();
                }
                else CurNodeNo = string.Empty;

                #region Reset Datagridview
                dgvTrxName.Rows.Clear();

                string[] columns = new string[] { "EVENT_GROUP_NAME", "DIR", "EVENT_NAME", "DEVCODE", "ADDR", "POINTS", "SKIPDECODE", "ITEM_NAME", "VAL", "WOFFSET", "WPOINTS", "BOFFSET", "BPOINTS", "EXPRESSION", "REALADDR" };

                DataTable dt = UniTools.InitDt(columns);
                
                dgvTrxData.DataSource = dt;
                #endregion
            }
             catch (Exception ex)
             {
                 NLogManager.Logger.LogErrorWrite("",this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                 ShowMessage(this, MethodBase.GetCurrentMethod().Name, ex, MessageBoxIcon.Error);
             }
        }
    }

    public class PLCTrxNameReply 
    {
        public class PLCTRXc
        {
            public string PLCTRXNAME { get; set; }

            public PLCTRXc()
            {
                PLCTRXNAME = string.Empty;
            }
        }

        public class TrxBody 
        {
            public string LINENAME { get; set; }

           
            public List<PLCTRXc> PLCTRXLIST { get; set; }

            public TrxBody()
            {
                LINENAME = string.Empty;
                PLCTRXLIST = new List<PLCTRXc>();
            }
        }

        public TrxBody BODY { get; set; }

        public PLCTrxNameReply()
        {
            this.BODY = new TrxBody();
        }
    }


    public class PLCTrxDataReply 
    {
        public class EVENTGROUPc
        {
            public string NAME { get; set; }

            public string DIR { get; set; }

          
            public List<EVENTc> EVENTLIST { get; set; }

            public EVENTGROUPc()
            {
                NAME = string.Empty;
                DIR = string.Empty;
                EVENTLIST = new List<EVENTc>();
            }
        }

        public class EVENTc
        {
            public string NAME { get; set; }

            public string DEVCODE { get; set; }

            public string ADDR { get; set; }

            public string POINTS { get; set; }

            public string SKIPDECODE { get; set; }

         
            public List<ITEMc> ITEMLIST { get; set; }

            public EVENTc()
            {
                NAME = string.Empty;
                DEVCODE = string.Empty;
                ADDR = string.Empty;
                POINTS = string.Empty;
                SKIPDECODE = string.Empty;
                ITEMLIST = new List<ITEMc>();
            }
        }

        public class ITEMc
        {
            public string NAME { get; set; }

            public string VAL { get; set; }

            public string WOFFSET { get; set; }

            public string WPOINTS { get; set; }

            public string BOFFSET { get; set; }

            public string BPOINTS { get; set; }

            public string EXPERESSION { get; set; }

            public ITEMc()
            {
                NAME = string.Empty;
                VAL = string.Empty;
                WOFFSET = string.Empty;
                WPOINTS = string.Empty;
                BOFFSET = string.Empty;
                BPOINTS = string.Empty;
                EXPERESSION = string.Empty;
            }
        }

        public class TrxBody {
            public string LINENAME { get; set; }

            public string PLCTRXNAME { get; set; }

         
            public List<EVENTGROUPc> EVENTGROUPLIST { get; set; }

            public TrxBody()
            {
                LINENAME = string.Empty;
                PLCTRXNAME = string.Empty;
                EVENTGROUPLIST = new List<EVENTGROUPc>();
            }
        }

        public TrxBody BODY { get; set; }

        public PLCTrxDataReply()
        {
           
            this.BODY = new TrxBody();
        }
    }

    
}
