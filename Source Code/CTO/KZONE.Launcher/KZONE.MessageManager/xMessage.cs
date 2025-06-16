using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.MessageManager
{
    public class xMessage
    {
        private string _fromAgent;

        private string _ToAgent;

        private DateTime _createTime;

        private string _name;

        private object _data;

        private string _trxid;

        private Dictionary<string, object> _useField;

        public string FromAgent
        {
            get
            {
                return this._fromAgent;
            }
            set
            {
                this._fromAgent = value;
            }
        }

        public string ToAgent
        {
            get
            {
                return this._ToAgent;
            }
            set
            {
                this._ToAgent = value;
            }
        }

        public DateTime CreateTime
        {
            get
            {
                return this._createTime;
            }
            set
            {
                this._createTime = value;
            }
        }

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

        public object Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public Dictionary<string, object> UseField
        {
            get
            {
                return this._useField;
            }
            set
            {
                this._useField = value;
            }
        }

        public string TransactionID
        {
            get
            {
                return this._trxid;
            }
            set
            {
                this._trxid = value;
            }
        }

        public xMessage()
        {
            this._createTime = DateTime.Now;
            this._useField = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return string.Format("Message name={0},createTime={1},from={2},to={3}.", new object[]
			{
				this.Name,
				this.CreateTime,
				this.FromAgent,
				this.ToAgent
			});
        }
    }
}
