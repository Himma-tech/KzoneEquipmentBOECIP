using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace KZONE.UI
{
    public class OPIInfo
    {
        public string APName { get; set; }
        public string Version { get; set; }
        public string ErrMessage { get; set; }
        public string ServerName { get; set; }
        public string LocalIPAddress { get; set; }
        public string LocalHostName { get; set; }

        //History 分頁用
        public int QueryMaxCount { get; set; }
        public int QueryPageSixeCount { get; set; }

        public DBSETDataContext DBCtx = new DBSETDataContext();
        public DBHISSETDataContext HisDBCtx = new DBHISSETDataContext();

        private DBSETDataContext _DBBRMCtx;

        public DBSETDataContext DBBRMCtx
        {
            get
            {
                if (_DBBRMCtx == null) _DBBRMCtx = new DBSETDataContext(DBCtx.Connection);
                return _DBBRMCtx;
            }
            set
            {
                _DBBRMCtx = value;
            }
        }

        public void RefreshDBBRMCtx()
        {
            DBBRMCtx = new DBSETDataContext(DBCtx.Connection);
        }

        private volatile static OPIInfo _instance = null;
        private static readonly object lockHelper = new object();
        public static OPIInfo CreateInstance()
        {
            if(_instance == null)
            {
                lock(lockHelper)
                {
                    if (_instance == null)
                    {
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(@"..\Config\Startup.xml");
                            string serverName = doc["framework"]["servername"].InnerText;
                            string lineType = doc["framework"]["linetype"].InnerText;
                            string fabType = doc["framework"]["fabtype"].InnerText;
                             

                            _instance = new OPIInfo(fabType, lineType, serverName);
                        }
                        catch (System.Exception ex)
                        {
                            ex.ToString();
                        } 
                    }
                }
            }
            return _instance;
        }    

        public Line  CurLine = new Line();

        public Dictionary<string, Node> Dic_Node { get; set; } // Key: NODENO
        
        public Dictionary<string, Port> Dic_Port { get; set; } // Key: NODENO(3) + PORTNO(1) --port no by node
        public Dictionary<string, Unit> Dic_Unit { get; set; } // Key: NODENO(3) + UNITNO(2)
        public Dictionary<string, Line> Dic_Line { get; set; } // Key: LineID 
        public Dictionary<string, Dense> Dic_Dense { get; set; } // Key: NODENO(3) + PORTNO(2) 
        public Dictionary<string, Interface> Dic_Pipe { get; set; } // Key:pipe key 
        public SortedDictionary<int, string> Dic_RecipSeq { get; set; } //Key: SBRM_NODE -> RecipeSeq  
        public List<LinkSignalType> Lst_LinkSignal_Type { get; set; }
        public Dictionary<string, LinkSignalBitDesc> Dic_LinkSignal_Desc { get; set; }  //key:LinkType
        public static Queue<string> Q_BCMessage { get; set; } //BC要OPI Pop的訊息
        public static Queue<OPIMessage> Q_OPIMessage { get; set; } //OPI要pop的訊息
       
        #region

        private OPIInfo(string strFabType, string strLineType, string strServerName)
        {
            try
            {
                string _err = string.Empty;

                ErrMessage = string.Empty;

                Dic_Pipe = new Dictionary<string, Interface>();

                Q_BCMessage = new Queue<string>();
                Q_OPIMessage = new Queue<OPIMessage>();            
                LocalHostName = Dns.GetHostName(); 
                LocalIPAddress = GetIPAddress(LocalHostName);
                QueryMaxCount = 1000;
                QueryPageSixeCount = 300;        
                ServerName = strLineType; //strServerName;

                #region Load DB Line Data

                var _var = DBCtx.SBRM_LINE.Where(r => r.SERVERNAME == strServerName);

                if (_var == null)
                {
                    ErrMessage = string.Format("SBRM_LINE can't find Server Name[{0}]", strServerName);
                    return;
                }

                foreach (SBRM_LINE sbrm_line in _var)
                {
                    if (CurLine.LineID == null)
                    {
                        CurLine.LineID = sbrm_line.LINEID;
                        CurLine.LineName = sbrm_line.LINENAME;
                        CurLine.LineType = sbrm_line.LINETYPE;
                        CurLine.FabType = sbrm_line.FABTYPE;
                        CurLine.ServerName = sbrm_line.SERVERNAME;
                        CurLine.JobDataLineType = sbrm_line.JOBDATALINETYPE;
                        CurLine.CrossLineRecipeCheck = sbrm_line.CHECKCROSSRECIPE == "Y" ? true : false;
                        CurLine.LineID2 = string.Empty;
                        CurLine.HistoryType = sbrm_line.HISTORYTYPE;
                        CurLine.LineSpecialFun = sbrm_line.OPI_FUNCTION;
                    }
                    else
                    {
                        CurLine.LineID2 = sbrm_line.LINEID;
                    }
                }
                #endregion

                #region 取得相同line type的連線資訊
                var dblines = DBCtx.SBRM_LINE.Where(r => r.FABTYPE == CurLine.FabType);
                if (dblines != null)
                {
                    Dic_Line = new Dictionary<string, Line>();
                    foreach (SBRM_LINE lineData in dblines)
                    {
                        Line _line = new Line();
                        _line.LineID = lineData.LINEID;
                        _line.LineName = lineData.LINENAME;
                        _line.LineType = lineData.LINETYPE;
                        _line.FabType = lineData.FABTYPE;
                        _line.ServerName = lineData.SERVERNAME;
                        _line.JobDataLineType = lineData.JOBDATALINETYPE;
                        //_line.ConnectionData = DBConfigXml.getLineData(_line.ServerName);
                        _line.LineSpecialFun = lineData.OPI_FUNCTION;

                        if (Dic_Line.ContainsKey(_line.ServerName))
                        {
                            _line.LineID2 = lineData.LINEID;
                        }
                        else
                            Dic_Line.Add(_line.ServerName, _line);
                    }
                }
                #endregion

                #region Load DB Node Data
                var rstNODE = DBCtx.SBRM_NODE.Where(r => r.SERVERNAME == strServerName).OrderBy(r => r.RECIPEIDX);
                Dic_Node = new Dictionary<string, Node>();
                Dic_RecipSeq = new SortedDictionary<int, string>();
               // Lst_Equipment = new List<Node>();


                foreach (SBRM_NODE sbrm_node in rstNODE)
                {
                    int _vcrCnt = sbrm_node.DCRCOUNT != null ? int.Parse(sbrm_node.DCRCOUNT.ToString()) : 0;
                    int _dispatchCnt = sbrm_node.DISPATCHCOUNT != null ? int.Parse(sbrm_node.DISPATCHCOUNT.ToString()) : 0;
                    Node _Node = new Node();
                    _Node.LineID = sbrm_node.LINEID;
                    _Node.ServerName = sbrm_node.SERVERNAME;
                    _Node.NodeNo = sbrm_node.NODENO;
                    _Node.NodeID = sbrm_node.NODEID;
                    _Node.ReportMode = sbrm_node.REPORTMODE;
                    _Node.NodeAttribute = sbrm_node.ATTRIBUTE;
                    _Node.RecipeLen = sbrm_node.RECIPELEN;
                    _Node.DefaultRecipeNo = "0".PadLeft(_Node.RecipeLen, '0');
                    _Node.UnitCount = sbrm_node.UNITCOUNT;
                    _Node.NodeName = sbrm_node.NODENAME;
                    _Node.UseEDCReport = sbrm_node.USEEDCREPORT;
                    _Node.UseRunMode = sbrm_node.USERUNMODE;
                    _Node.UseIndexerMode = sbrm_node.USEINDEXERMODE == "Y" ? true : false;
                    _Node.VCRs = GetNodeVCRs(_Node.NodeNo, _vcrCnt);
         
                    _Node.OPISpecialType = sbrm_node.OPITYPE;
                    _Node.RecipeRegisterCheck = sbrm_node.RECIPEREGVALIDATIONENABLED == "Y" ? true : false;
                    _Node.RecipeParameterCheck = sbrm_node.RECIPEPARAVALIDATIONENABLED == "Y" ? true : false;
           
                    if (sbrm_node.RECIPESEQ == null) _Node.RecipeSeq = new List<int> { 0 };
                    else _Node.RecipeSeq = sbrm_node.RECIPESEQ.Split(',').Select(int.Parse).ToList();

                    Dic_Node.Add(sbrm_node.NODENO, _Node);


                    if (_Node.UseIndexerMode) CurLine.IndexerNode = _Node;
               

                    #region 依據Recipe Seq 新增recipe node
                    if (_Node.RecipeSeq.Count == 0) continue;
                    if (_Node.RecipeSeq.Contains(0)) continue;

                    foreach (int _seq in _Node.RecipeSeq)
                    {
                        if (Dic_RecipSeq.ContainsKey(_seq))
                        {
                            ErrMessage = string.Format("SBRM_NODE RECIPESEQ[{0}] is duplicate ", _seq.ToString());
                            return;
                        }
                        Dic_RecipSeq.Add(_seq, _Node.NodeNo);
                    }
                    #endregion
                }

                #endregion

                #region Load DB Unit Data
                Dic_Unit = new Dictionary<string, Unit>();
                var rstUnit = DBCtx.SBRM_UNIT.Where(r => r.SERVERNAME == strServerName);
                foreach (SBRM_UNIT r in rstUnit)
                {
                    Unit _unit = new Unit();
                    _unit.LineID = r.LINEID;
                    _unit.ServerName = r.SERVERNAME;
                    _unit.NodeNo = r.NODENO;
                    _unit.NodeID = r.NODEID;
                    _unit.UnitID = r.UNITID;
                    _unit.UnitNo = r.UNITNO; //t3 UnitNo為string,HKC改為int sy.wu 2016/06/23
                    _unit.UnitType = r.UNITTYPE;
                    _unit.UseRunMode = r.USERUNMODE.ToString().ToUpper();
                    _unit.SubUnit = r.SUBUNIT;

                    Dic_Unit.Add(r.NODENO.PadRight(3, ' ') + r.UNITNO.ToString().PadLeft(2, '0'), _unit); //t3 UnitNo為string,HKC改為int sy.wu 2016/06/23
                }
                #endregion

                #region Load Link Signal Description
                Lst_LinkSignal_Type = new List<LinkSignalType>();
                Dic_LinkSignal_Desc = new Dictionary<string, LinkSignalBitDesc>();

                string _path = string.Format(@"..\Config\OPI\LinkSignal\{0}\{1}\LinkSignal.xml", CurLine.FabType, CurLine.LineID);
                string _descPatch = string.Format(@"..\Config\OPI\LinkSignal\LinkSignalType.xml");
                if (File.Exists(_path) && File.Exists(_descPatch))
                {
                    XDocument _doc = XDocument.Load(_path);

                    if (_doc.Element("LinkSignal") == null) return;
                    if (_doc.Element("LinkSignal").Elements("Bit") == null) return;


                    IEnumerable<XElement> _element = _doc.Element("LinkSignal").Elements("Bit");

                    XDocument _typeDoc = XDocument.Load(_descPatch);

                    foreach (XElement _e in _element)
                    {
                        LinkSignalType _type = new LinkSignalType();

                        if (_e.Attribute("UpStream") == null) continue;
                        if (_e.Attribute("DownStream") == null) continue;
                        if (_e.Attribute("SeqNo") == null) continue;
                        if (_e.Attribute("LinkType") == null) continue;
                        if (_e.Attribute("TimingChart") == null) continue;

                        //Upstream path no + Downstream path no
                        if (_e.Attribute("SeqNo").Value.Length != 4) continue;

                        _type.UpStreamLocalNo = _e.Attribute("UpStream").Value;
                        _type.DownStreamLocalNo = _e.Attribute("DownStream").Value;
                        _type.SeqNo = _e.Attribute("SeqNo").Value;
                        _type.LinkType = _e.Attribute("LinkType").Value;
                        _type.TimingChart = _e.Attribute("TimingChart").Value;


                        if (GetLinkSignalDesc(_type.LinkType, _typeDoc, strFabType))
                        {
                            Lst_LinkSignal_Type.Add(_type);
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                ErrMessage = ex.ToString();
            }
        }

        private bool GetLinkSignalDesc(string LinkType, XDocument XmlDoc, string FabType)
        {
            try
            {
                if (Dic_LinkSignal_Desc.ContainsKey(LinkType)) return true;

                if (XmlDoc.Element("LinkSignal") == null) return false;
                if (XmlDoc.Element("LinkSignal").Elements("Type") == null) return false;

                var _v = from page in XmlDoc.Element("LinkSignal").Elements("Type")
                         where LinkType == page.Attribute("Name").Value && page.Attribute("FABTYPE").Value.Equals(FabType)
                         select page;

                int _seqNo = 0;

                if (_v == null || _v.Count() == 0) return false;

                XElement _e = _v.First();

                if (_e.Element("UpStreamBit") == null) return false;
                if (_e.Element("UpStreamBit").Elements("Bit") == null) return false;

                IEnumerable<XElement> _upElement = _e.Element("UpStreamBit").Elements("Bit");

                LinkSignalBitDesc _bitDesc = new LinkSignalBitDesc();

                foreach (XElement _d in _upElement)
                {
                    if (_d.Attribute("SeqNo") == null) continue;
                    if (_d.Attribute("Description") == null) continue;

                    int.TryParse(_d.Attribute("SeqNo").Value, out _seqNo);

                    if (_bitDesc.UpStreamBit.ContainsKey(_seqNo)) continue;

                    _bitDesc.UpStreamBit.Add(_seqNo, _d.Attribute("Description").Value);
                }

                if (_e.Element("DownStreamBit") == null) return false;
                if (_e.Element("DownStreamBit").Elements("Bit") == null) return false;

                IEnumerable<XElement> _downElement = _e.Element("DownStreamBit").Elements("Bit");

                foreach (XElement _d in _downElement)
                {

                    if (_d.Attribute("SeqNo") == null) continue;
                    if (_d.Attribute("Description") == null) continue;

                    int.TryParse(_d.Attribute("SeqNo").Value, out _seqNo);

                    if (_bitDesc.DownStreamBit.ContainsKey(_seqNo)) continue;

                    _bitDesc.DownStreamBit.Add(_seqNo, _d.Attribute("Description").Value);
                }

                Dic_LinkSignal_Desc.Add(LinkType, _bitDesc);

                return true;
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;

                return false;
            }
        }


        #endregion

        private string GetIPAddress(string LocalHostName)
        {
            try
            {
                //// 取得本機名稱
                //string strHostName = Dns.GetHostName();
                //// 取得本機的IpHostEntry類別實體，用這個會提示已過時

                ////IPHostEntry iphostentry = Dns.GetHostByName(strHostName);


                // 取得本機的IpHostEntry類別實體，MSDN建議新的用法
                IPHostEntry iphostentry = Dns.GetHostEntry(LocalHostName);

                // 取得所有 IP 位址
                foreach (IPAddress ipaddress in iphostentry.AddressList)
                {
                    // 只取得IP V4的Address
                    if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ipaddress.ToString();
                    }
                }
                return string.Empty ;
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
                return string.Empty;
            }
        }

        private List<DCR> GetNodeVCRs(string NodeNo, int VCRCnt)
        {
            List<DCR> lstVCRNode = new List<DCR>();

            for (int i = 1; i <= VCRCnt; i++)
            {
                DCR vcr = new DCR();

                vcr.DCRNO = i.ToString().PadLeft(2, '0');

                lstVCRNode.Add(vcr);
            }

            return lstVCRNode;
        }
    }

    public class OPIMessage
    {

        private string _timeStamp;
        private string _msgCaption;
        private string _msgCode;
        private string _msgData;
        private eOPIMessageType _msgType;

        public OPIMessage(string timeStamp, string msgCaption, string msgData, string msgCode, eOPIMessageType msgTyp = eOPIMessageType.Error)
        {
            _timeStamp =  timeStamp;
            _msgCaption= msgCaption;
            _msgCode = msgCode;
            _msgData = msgData;
            _msgType = msgTyp;
        }

        public OPIMessage(string msgCaption, string msgCode, string msgData, eOPIMessageType msgTyp = eOPIMessageType.Error)
        {
            _timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _msgCaption = msgCaption;
            _msgCode = msgCode;
            _msgData = msgData;
            _msgType = msgTyp;
        }

        public OPIMessage(string msgCaption, Exception msgData)
        {
            _timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _msgCaption = msgCaption;
            _msgCode = string.Empty;
            _msgData = msgData.ToString();
            _msgType = eOPIMessageType.Error;
        }

        public string MsgDateTime
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        public string MsgCaption
        {
            get { return _msgCaption; }
            set { _msgCaption = value; }
        }

        public string MsgCode
        {
            get { return _msgCode; }
            set { _msgCode = value; }
        }

        public string MsgData
        {
            get { return _msgData; }
            set { _msgData = value; }
        }

        public eOPIMessageType MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }
    }

    public class SocketInfo
    {
        private DateTime _timeStamp;
        private string _msgName;
        private string _trxId; 
        private string _xml;
        private string _sessionId;

        public DateTime SocketDateTime
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        public string MsgName
        {
            get { return _msgName; }
            set { _msgName = value; }
        }

        public string TrxId
        {
            get { return _trxId; }
            set { _trxId = value; }
        }

        public string SocketXml
        {
            get { return _xml; }
            set { _xml = value; }
        }

        public string SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }
    }
}
