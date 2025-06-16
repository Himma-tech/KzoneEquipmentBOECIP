using System;
using System.Collections;

namespace KZONE.Entity
{
    #region SBCSGLASSQTIMEHISTORY

    /// <summary>
    /// SBCSGLASSQTIMEHISTORY object for NHibernate mapped table 'SBCS_GLASSQTIMEHISTORY'.
    /// </summary>
    public class GlassQTimeHistory : EntityData
    {
        #region Member Variables

        protected long _id;
        protected DateTime _uPDATETIME = DateTime.Now;
        protected int _cASSETTESEQNO;
        protected int _cASSETTESLOTNO;
        protected string _glASSID;
        protected string _eVENTNAME;
        protected string _qTIMEID;
        protected int _sETTIMEVALUE;
        protected string _sTARTDATETIME;
        protected string _eNDDATETIME;
        protected int _sPENDQTIMEVALUE;
        protected string _iSOVERQTIME;
        protected string _sTARTNODEID;
        protected string _sTARTNODENO;
        protected string _sTARTNUNITID;
        protected string _sTARTNUNITNO;
        protected string _sTARTEVENTMSG;
        protected string _eNDNODEID;
        protected string _eNDNODENO;
        protected string _eNDNUNITID;
        protected string _eNDNUNITNO;
        protected string _eNDEVENTMSG;
        protected string _sTARTNODERECIPEID;
        protected string _eNABLED;

        #endregion

        #region Constructors

        public GlassQTimeHistory() { }

        public GlassQTimeHistory(DateTime uPDATETIME, int cASSETTESEQNO, int cASSETTESLOTNO, string glASSID, string eVENTNAME, string qTIMEID, int sETTIMEVALUE, string sTARTDATETIME, string eNDDATETIME, int sPENDQTIMEVALUE, string iSOVERQTIME, string sTARTNODEID, string sTARTNODENO, string sTARTNUNITID, string sTARTNUNITNO, string sTARTEVENTMSG, string eNDNODEID, string eNDNODENO, string eNDNUNITID, string eNDNUNITNO, string eNDEVENTMSG, string sTARTNODERECIPEID, string eNABLED)
        {
            this._uPDATETIME = uPDATETIME;
            this._cASSETTESEQNO = cASSETTESEQNO;
            this._cASSETTESLOTNO = cASSETTESLOTNO;
            this._glASSID = glASSID;
            this._eVENTNAME = eVENTNAME;
            this._qTIMEID = qTIMEID;
            this._sETTIMEVALUE = sETTIMEVALUE;
            this._sTARTDATETIME = sTARTDATETIME;
            this._eNDDATETIME = eNDDATETIME;
            this._sPENDQTIMEVALUE = sPENDQTIMEVALUE;
            this._iSOVERQTIME = iSOVERQTIME;
            this._sTARTNODEID = sTARTNODEID;
            this._sTARTNODENO = sTARTNODENO;
            this._sTARTNUNITID = sTARTNUNITID;
            this._sTARTNUNITNO = sTARTNUNITNO;
            this._sTARTEVENTMSG = sTARTEVENTMSG;
            this._eNDNODEID = eNDNODEID;
            this._eNDNODENO = eNDNODENO;
            this._eNDNUNITID = eNDNUNITID;
            this._eNDNUNITNO = eNDNUNITNO;
            this._eNDEVENTMSG = eNDEVENTMSG;
            this._sTARTNODERECIPEID = sTARTNODERECIPEID;
            this._eNABLED = eNABLED;
        }

        #endregion

        #region Public Properties

        public virtual long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual DateTime UPDATETIME
        {
            get { return _uPDATETIME; }
            set { _uPDATETIME = value; }
        }

        public virtual int CASSETTESEQNO
        {
            get { return _cASSETTESEQNO; }
            set { _cASSETTESEQNO = value; }
        }

        public virtual int CASSETTESLOTNO
        {
            get { return _cASSETTESLOTNO; }
            set { _cASSETTESLOTNO = value; }
        }

        public virtual string GlASSID
        {
            get { return _glASSID; }
            set
            {
                _glASSID = value;
            }
        }

        public virtual string EVENTNAME
        {
            get { return _eVENTNAME; }
            set
            {
                _eVENTNAME = value;
            }
        }

        public virtual string QTIMEID
        {
            get { return _qTIMEID; }
            set
            {
                _qTIMEID = value;
            }
        }

        public virtual int SETTIMEVALUE
        {
            get { return _sETTIMEVALUE; }
            set { _sETTIMEVALUE = value; }
        }

        public virtual string STARTDATETIME
        {
            get { return _sTARTDATETIME; }
            set
            {
                _sTARTDATETIME = value;
            }
        }

        public virtual string ENDDATETIME
        {
            get { return _eNDDATETIME; }
            set
            {
                _eNDDATETIME = value;
            }
        }

        public virtual int SPENDQTIMEVALUE
        {
            get { return _sPENDQTIMEVALUE; }
            set { _sPENDQTIMEVALUE = value; }
        }

        public virtual string ISOVERQTIME
        {
            get { return _iSOVERQTIME; }
            set
            {
                _iSOVERQTIME = value;
            }
        }

        public virtual string STARTNODEID
        {
            get { return _sTARTNODEID; }
            set
            {
                _sTARTNODEID = value;
            }
        }

        public virtual string STARTNODENO
        {
            get { return _sTARTNODENO; }
            set
            {
                _sTARTNODENO = value;
            }
        }

        public virtual string STARTNUNITID
        {
            get { return _sTARTNUNITID; }
            set
            {
                _sTARTNUNITID = value;
            }
        }

        public virtual string STARTNUNITNO
        {
            get { return _sTARTNUNITNO; }
            set
            {
                _sTARTNUNITNO = value;
            }
        }

        public virtual string STARTEVENTMSG
        {
            get { return _sTARTEVENTMSG; }
            set
            {
                _sTARTEVENTMSG = value;
            }
        }

        public virtual string ENDNODEID
        {
            get { return _eNDNODEID; }
            set
            {
                _eNDNODEID = value;
            }
        }

        public virtual string ENDNODENO
        {
            get { return _eNDNODENO; }
            set
            {
                _eNDNODENO = value;
            }
        }

        public virtual string ENDNUNITID
        {
            get { return _eNDNUNITID; }
            set
            {
                _eNDNUNITID = value;
            }
        }

        public virtual string ENDNUNITNO
        {
            get { return _eNDNUNITNO; }
            set
            {
                _eNDNUNITNO = value;
            }
        }

        public virtual string ENDEVENTMSG
        {
            get { return _eNDEVENTMSG; }
            set
            {
                _eNDEVENTMSG = value;
            }
        }

        public virtual string STARTNODERECIPEID
        {
            get { return _sTARTNODERECIPEID; }
            set
            {
                _sTARTNODERECIPEID = value;
            }
        }

        public virtual string ENABLED
        {
            get { return _eNABLED; }
            set
            {
                _eNABLED = value;
            }
        }



        #endregion
    }
    #endregion
}