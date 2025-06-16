using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;

namespace KZONE.Entity
{
    public class SRCS_METHOD_DEF : EntityData
    {
        private void SetStringProertyToStringEmpty()
        {
            PropertyInfo[] properties = typeof(SRCS_METHOD_DEF).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty, null);
            }
        }

        #region Constructors

        public SRCS_METHOD_DEF()
        {
            SetStringProertyToStringEmpty();
        }

        #endregion

        #region Public Properties

        public virtual long OBJECTKEY { get; set; }

        public virtual string METHODTYPE { get; set; }

        public virtual string METHODNAME { get; set; }

        public virtual string DESCRIPTION { get; set; }

        public virtual string FUNCKEY { get; set; }

        #endregion
    }
}
