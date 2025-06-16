using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KZONE.ConstantParameter
{
    public class ParameterData
    {
        private string _name;

        private object _value;

        private string _discription;

        private string _dataType;

        private object _syncObject = new object();

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                lock (this._syncObject)
                {
                    this._value = value;
                }
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

        public string DataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }

        public ParameterData(string name)
        {
            this._name = name;
            this._value = "";
            this._discription = "";
            this._dataType = "string";
        }

        public ParameterData(string name, object val)
        {
            this._name = name;
            this._value = val;
            this._discription = "";
            this._dataType = "string";
        }

        public ParameterData(string name, object value, string disc)
        {
            this._name = name;
            this._value = value;
            this._discription = disc;
            this._dataType = "string";
        }

        public ParameterData(string name, object value, string disc, string type)
        {
            this._name = name;
            this._value = value;
            this._discription = disc;
            this._dataType = type;
        }

        public void WriteTo(XmlWriter writer)
        {
            writer.WriteStartElement("parameter");
            writer.WriteAttributeString("name", this.Name);
            writer.WriteAttributeString("value", this.Value.ToString());
            writer.WriteAttributeString("discription", this.Discription);
            writer.WriteAttributeString("type", this.DataType);
            writer.WriteEndElement();
        }

        public bool GetBoolean()
        {
            return this.DataType == "bool" && this.Value.ToString().ToUpper() == "TRUE";
        }

        public int GetInteger()
        {
            if (this.DataType == "integer")
            {
                try
                {
                    int result = Convert.ToInt32(this.Value);
                    return result;
                }
                catch (Exception)
                {
                    int result = -1;
                    return result;
                }  
            }
            return -1;
        }

        public string GetString()
        {
            if (this.DataType == "string")
            {
                return this.Value.ToString();
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("Name='{0}',Value='{1}',Desc='{2}',Type='{3}'.", new object[]
			{
				this.Name,
				this.Value,
				this.Discription,
				this.DataType
			});
        }
    }
}
