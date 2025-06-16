using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;

namespace KZONE.Entity
{
    public class SRCS_ROUTE : EntityData
    {
        private void SetStringProertyToStringEmpty()
        {
            PropertyInfo[] properties = typeof(SRCS_ROUTE).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty, null);
            }
        }

        #region Constructors

        public SRCS_ROUTE()
        {
            SetStringProertyToStringEmpty();
        }

        #endregion

        #region Public Properties

        public virtual long OBJECTKEY { get; set; }

        public virtual string SERVERNAME { get; set; }

        public virtual string ROUTENAME { get; set; }

        public virtual int ROUTEPRIORITY { get; set; }

        public virtual string DESCRIPTION { get; set; }

        public virtual string JUDGE_METHOD { get; set; }

        public virtual string CALC_METHOD { get; set; }

        #endregion
    }
}
