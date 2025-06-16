using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KZONE.Entity;

namespace KZONE.Entity
{
    public class SRCS_ROBOT : EntityData
    {
        private void SetStringProertyToStringEmpty()
        {
            PropertyInfo[] properties = typeof(SRCS_ROBOT).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty, null);
            }
        }

        #region Constructors

        public SRCS_ROBOT()
        {
            SetStringProertyToStringEmpty();
        }

        #endregion

        #region Public Properties

        public virtual long OBJECTKEY { get; set; }

        public virtual string SERVERNAME { get; set; }

        public virtual string ROBOTNAME { get; set; }

        public virtual string LINEID { get; set; }

        public virtual string NODENO { get; set; }

        public virtual string ROBOTNO { get; set; }

        public virtual string SCAN_METHOD { get; set; }

        public virtual string DISPATCH_METHOD { get; set; }

        #endregion
    }
}
