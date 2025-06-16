using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KZONE.Entity
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class JobArraySpecial
    {

        private string _lotid=string.Empty;
        private string _cstid=string.Empty;
        private eBitResult _EngraveFlag = eBitResult.OFF;
        //20170103 add A1SOR Indexer取片by Sort Priority
        private int _sortGradePriority = eSortGradePriority.Min_Priority; //數字越小Priority越高

        [Category("Array Special")]
        public string LotId
        {
            get { return _lotid; }
            set { _lotid = value; }
        }
        [Category("Array Special")]
        public string CSTId
        {
            get { return _cstid; }
            set { _cstid = value; }
        }

        //20170103 add A1SOR Indexer取片by Sort Priority
        /// <summary> Indexer Robot Fetch By Sort Grade Priority. Priority 1 為最高
        /// 
        /// </summary>
        [Category("Array Special")]
        public int SortGradePriority
        {
            get { return _sortGradePriority; }
            set { _sortGradePriority = value; }
        }

        [Category("Array Special")]
        public eBitResult EngraveFlag {
            get { return _EngraveFlag; }
            set { _EngraveFlag = value; }
        }


    }
}
