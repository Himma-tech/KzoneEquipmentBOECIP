using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    #region SBCS_RECIPEVALIDATIONRESULTHISTORY

    /// <summary>
    /// RecipeValidationResultHistory object for NHibernate mapped table 'SBCS_RECIPEVALIDATIONRESULTHISTORY'.
    /// </summary>
    [Serializable]
    public class RecipeValidationResultHistory : EntityData
    {
        #region Member Variables

        protected long _no;
        protected DateTime _rECEIVETIME = DateTime.Now;
        protected string _mASTERRECEPEID;
        protected string _lOCALRECIPEID;
        protected string _rMSRESULT;
        protected string _rMSRESULTTEXT;

        #endregion

        #region Constructors

        public RecipeValidationResultHistory() { }

        public RecipeValidationResultHistory(DateTime rECEIVETIME, string mASTERRECEPEID, string lOCALRECIPEID, string rMSRESULT, string rMSRESULTTEXT)
        {
            //this._no = no;
            this._rECEIVETIME = rECEIVETIME;
            this._mASTERRECEPEID = mASTERRECEPEID;
            this._lOCALRECIPEID = lOCALRECIPEID;
            this._rMSRESULT = rMSRESULT;
            this._rMSRESULTTEXT = rMSRESULTTEXT;
        }

        #endregion

        #region Public Properties

        public virtual long NO
        {
            get { return _no; }
            set { _no = value; }
        }

        public virtual DateTime RECEIVETIME
        {
            get { return _rECEIVETIME; }
            set { _rECEIVETIME = value; }
        }

        public virtual string MASTERRECIPEID
        {
            get { return _mASTERRECEPEID; }
            set
            {
                _mASTERRECEPEID = value;
            }
        }

        public virtual string LOCALRECIPEID
        {
            get { return _lOCALRECIPEID; }
            set
            {
                _lOCALRECIPEID = value;
            }
        }

        public virtual string RMS_RESULT
        {
            get { return _rMSRESULT; }
            set
            {
                _rMSRESULT = value;
            }
        }

        public virtual string RMS_RESULTTEXT
        {
            get { return _rMSRESULTTEXT; }
            set
            {
                _rMSRESULTTEXT = value;
            }
        }

        #endregion
    }
    #endregion

}
