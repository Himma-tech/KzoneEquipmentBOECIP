using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using KZONE.Log;
namespace KZONE.ConstantParameter
{

    internal class FileFormatItem : ICloneable
    {
        private const string Pattern = "\\{\\w*}";

        private string _key;

        private string _name;

        private string _property;

        private int _length;

        private string _format;

        private string _value;

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

        public string Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        public string Property
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
            }
        }

        public int Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
            }
        }

        public string Format
        {
            get
            {
                return this._format;
            }
            set
            {
                this._format = value;
            }
        }

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

        public FileFormatItem(string key, string name, string property, int length, string format)
        {
            this._key = key;
            this._name = name;
            this._property = property;
            this._length = length;
            this._format = format;
        }

        public FileFormatItem(string key, string name)
        {
            this._key = key;
            this._name = name;
        }

        public string ToFormatString(object obj)
        {
            string temp = this._format;
            Regex regex = new Regex("\\{\\w*}");
            MatchCollection matches = regex.Matches(this.Format);
            for (int ctr = 0; ctr <= matches.Count - 1; ctr++)
            {
                string value;
                if ((value = matches[ctr].Value) != null)
                {
                    if (!(value == "{name}"))
                    {
                        if (!(value == "{property}"))
                        {
                            if (value == "{key}")
                            {
                                temp = temp.Replace("{key}", this.Key);
                            }
                        }
                        else
                        {
                            this.Value = this.GetPropertyValue(obj);
                            temp = temp.Replace("{property}", this.Value);
                        }
                    }
                    else
                    {
                        temp = temp.Replace("{name}", this.Name);
                    }
                }
            }
            return temp;
        }

        private string GetPropertyValue(object obj)
        {
            if (string.IsNullOrEmpty(this.Property))
            {
                return new string(' ', this.Length);
            }
            object value = obj;
            string result;
            try
            {
                string[] propertys = this.Property.Split(new char[]
				{
					'.'
				});
                for (int i = 0; i < propertys.Length; i++)
                {
                    value = this.GetProperty(value, propertys[i]);
                }
                result = value.ToString().PadRight(this.Length, ' ').Substring(0, this.Length);
            }
            catch (Exception ex)
            {
                NLogManager.Logger.LogDebugWrite("", base.GetType().Name, MethodBase.GetCurrentMethod().Name + "()", string.Format("get property ({0}) from {1} error.{2} ", this.Property, obj.GetType().Name, ex.ToString()));
                result = new string(' ', this.Length);
            }
            return result;
        }

        private object GetProperty(object obj, string property)
        {
            int start = property.IndexOf('[');
            int end = property.IndexOf(']');
            int position = 0;
            if (start != -1)
            {
                position = int.Parse(property.Substring(start + 1, end - start - 1));
                property = property.Substring(0, start);
            }
            PropertyInfo info = obj.GetType().GetProperty(property);
            object value;
            if (start != -1)
            {
                IList list = info.GetValue(obj, null) as IList;
                value = list[position];
            }
            else
            {
                value = info.GetValue(obj, null);
            }
            return value;
        }

        public object Clone()
        {
            FileFormatItem item = (FileFormatItem)base.MemberwiseClone();
            item.Name = this.Name;
            item.Property = this.Property;
            item.Key = this.Key;
            item.Length = this.Length;
            item.Value = this.Value;
            item.Format = this.Format;
            return item;
        }
    }
}
