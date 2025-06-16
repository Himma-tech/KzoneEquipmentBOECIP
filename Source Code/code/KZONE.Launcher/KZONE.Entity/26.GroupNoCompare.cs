using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace KZONE.Entity
{
    [Serializable]
    public class GourpNoConstant : EntityFile
    {
        public List<GroupNo> GroupNoCollection = new List<GroupNo>();
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]  
    public class GroupNo:ICloneable 
    {
        //List<string> _items = new List<string>();//此处暂时不能修改成List 否则无法序列化 20150320 Tom
        List<string> _items = new List<string>();
        int _value = 0;
        DateTime _lastUseTime = DateTime.Now;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public DateTime LastUseTime
        {
            get { return _lastUseTime; }
            set { _lastUseTime = value; }
        }

        //public List<string> Items
        public List<string> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        
        public GroupNo()
        {
            _lastUseTime = DateTime.Now;
        }

        public GroupNo(string[] strs)
        {
            _items.AddRange(strs);
        }

        public GroupNo(List<string> strs)
        {
            _items.AddRange(strs);
        }

        public void SetItemData(List<string> strs)
        {
            _items.AddRange(strs);
        }

        public void SetItemData(string[] strs)
        {
            _items.AddRange(strs);
        }

        public override Boolean Equals(System.Object obj)
        {

            if (obj == null)
                return false;
            bool ok = (obj is GroupNo);
            if(!ok)
                return ok;

            GroupNo other = obj as GroupNo;
          
            if (other.Items.Count != this.Items.Count)
                return false;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].ToString() != other.Items[i].ToString())
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override System.String ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in _items)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }

        //public static bool operator ==(ProductType productType, string[] other)
        //{
        //    ProductType otherProduct = new ProductType(other);
        //    return productType.Equals(otherProduct);
        //}

        //public static bool operator !=(ProductType productType, string[] other)
        //{
        //    ProductType otherProduct = new ProductType(other);
        //    return productType.Equals(otherProduct);
        //}

        //public static bool operator ==(ProductType type, ProductType other)
        //{
        //    return type.Equals(other);
        //}

        //public static bool operator !=(ProductType type, ProductType other)
        //{
        //    return type.Equals(other);
        //}



        public object Clone() {
            GroupNo newInstance = (GroupNo)this.MemberwiseClone();
            newInstance.Items = new List<string>();
            foreach (string v in this.Items) {
                newInstance.Items.Add(v);
            }
            return newInstance;
        }
    }
}
