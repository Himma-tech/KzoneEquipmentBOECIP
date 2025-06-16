using System;
using System.Collections;

namespace KZONE.Entity
{
    #region SBRMALARM

    /// <summary>
    /// SBRMALARM object for NHibernate mapped table 'SBRM_ALARM'.
    /// </summary>
    [Serializable]
    public class AlarmEntityData : EntityData
    {
        #region Member Variables

        protected long _id;
        protected string _lINEID;
        protected string _nODENO;
        protected string _uNITNO;
        protected string _aLARMLEVEL;
        protected string _aLARMID;
        protected string _aLARMCODE;
        protected string _aLARMTEXT;
        protected string _sERVERNAME;
        protected int _bCALARMID;
        protected int _aLARMTYPECODE;
        #endregion

        #region Constructors

        public AlarmEntityData() { }

        public AlarmEntityData(string lINEID, string nODENO, string uNITNO, string aLARMLEVEL, string aLARMID, int bCALARMID, string aLARMCODE, string aLARMTEXT, string sERVERNAME, int aLARMTYPECODE)
        {
            this._lINEID = lINEID;
            this._nODENO = nODENO;
            this._uNITNO = uNITNO;
            this._aLARMLEVEL = aLARMLEVEL;
            this._aLARMID = aLARMID;
            this._aLARMCODE = aLARMCODE;
            this._aLARMTEXT = aLARMTEXT;
            this._sERVERNAME = sERVERNAME;
            this._bCALARMID = bCALARMID;
            this._aLARMTYPECODE = aLARMTYPECODE;
        }

        #endregion

        #region Public Properties

        public virtual long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string LINEID
        {
            get { return _lINEID; }
            set
            {
                _lINEID = value;
            }
        }

        public virtual string NODENO
        {
            get { return _nODENO; }
            set
            {
                _nODENO = value;
            }
        }

        public virtual string UNITNO
        {
            get { return _uNITNO; }
            set
            {
                _uNITNO = value;
            }
        }

        public virtual string ALARMLEVEL
        {
            get { return _aLARMLEVEL; }
            set
            {
                _aLARMLEVEL = value;
            }
        }

        public virtual string ALARMID
        {
            get { return _aLARMID; }
            set
            {
                _aLARMID = value;
            }
        }

        public virtual int BCALARMID
        {
            get { return _bCALARMID; }
            set
            {
                _bCALARMID = value;
            }
        }


        public virtual string ALARMCODE
        {
            get { return _aLARMCODE; }
            set
            {
                _aLARMCODE = value;
            }
        }

        public virtual string ALARMTEXT
        {
            get { return _aLARMTEXT; }
            set
            {
                _aLARMTEXT = value;
            }
        }

        public virtual string SERVERNAME
        {
            get { return _sERVERNAME; }
            set
            {
                _sERVERNAME = value;
            }
        }

        public virtual int ALARMTYPECODE
        {
            get { return _aLARMTYPECODE; }
            set { _aLARMTYPECODE = value; }
        }


        #endregion
    }
    #endregion
}