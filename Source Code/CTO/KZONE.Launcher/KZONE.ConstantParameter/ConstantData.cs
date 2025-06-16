using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KZONE.ConstantParameter
{

    [Serializable]
    public class ConstantData
    {
        private IDictionary<string, ConstantItem> _values;

        private IList<string> _index;

        private string _name;

        private ConstantItem _value;

        private ConstantItem _defaultValue;

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

        public ConstantItem DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
            }
        }

        public ConstantItem Value
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

        public IDictionary<string, ConstantItem> Values
        {
            get
            {
                return this._values;
            }
            set
            {
                this._values = value;
            }
        }

        public ConstantItem this[int pos]
        {
            get
            {
                if (pos <= -1 || pos >= this._index.Count)
                {
                    throw new IndexOutOfRangeException("Out of range!");
                }
                return this._values[this._index[pos]];
            }
        }

        public ConstantItem this[string key]
        {
            get
            {
                if (!this._values.ContainsKey(key))
                {
                    return this.DefaultValue;
                }
                return this._values[key];
            }
        }

        public ConstantData()
        {
            this._values = new Dictionary<string, ConstantItem>();
            this._index = new List<string>();
            this._defaultValue = new ConstantItem("", "Default Value");
        }

        public ConstantData(string name)
        {
            this._values = new Dictionary<string, ConstantItem>();
            this._index = new List<string>();
            this._name = name;
            this._defaultValue = new ConstantItem("", "Default Value");
        }

        public void Add(string key, ConstantItem item)
        {
            if (this._values.ContainsKey(key))
            {
                throw new ArgumentException("Already Exist " + key.ToString());
            }
            this._index.Add(key);
            this._values.Add(key, item);
        }

        public void WriteTo(XmlWriter writer)
        {
            writer.WriteStartElement("constant");
            writer.WriteAttributeString("name", this.Name);
            if (this._value != null)
            {
                writer.WriteAttributeString("value", this.Value.Value);
                if (!string.IsNullOrEmpty(this.Value.Discription))
                {
                    writer.WriteAttributeString("discription", this.Value.Discription);
                }
            }
            if (this.DefaultValue != null)
            {
                writer.WriteAttributeString("default", this.DefaultValue.Value);
            }
            if (this._values.Count > 0)
            {
                writer.WriteStartElement("values");
                foreach (string key in this._values.Keys)
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString("key", key);
                    writer.WriteAttributeString("value", this._values[key].Value);
                    if (!string.IsNullOrEmpty(this._values[key].Discription))
                    {
                        writer.WriteAttributeString("discription", this._values[key].Discription);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
