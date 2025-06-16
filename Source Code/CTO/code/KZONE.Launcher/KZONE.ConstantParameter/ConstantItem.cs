using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.ConstantParameter
{
    [Serializable]
    public class ConstantItem
    {
        private string _value;

        private string _discription;

        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public string Discription
        {
            get
            {
                return this._discription;
            }
            set
            {
                this._discription = value;
            }
        }

        public ConstantItem()
        {
            this._value = "";
            this._discription = "";
        }

        public ConstantItem(string v, string d)
        {
            this._value = v;
            this._discription = d;
        }
    }
}
