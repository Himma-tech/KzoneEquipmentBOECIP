using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.UI
{
    public class ItemEntity
    {
        private string _itemValue = string.Empty;
        private string _itemName = string.Empty;

        public ItemEntity(string _value, string _item)
        {
            _itemValue = _value;
            _itemName = _item;
        }

        public string ItemValue
        {
            get { return _itemValue; }
            set { _itemValue = value; }
        }

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public string ItemDesc
        {
            get
            {
                if (_itemName == string.Empty)
                    return _itemValue;
                else
                {
                    if (_itemName == _itemValue)
                        return _itemValue;
                    else 
                        return string.Format("{0}:{1}", _itemValue, _itemName);
                }
            }
        }
    }
}
