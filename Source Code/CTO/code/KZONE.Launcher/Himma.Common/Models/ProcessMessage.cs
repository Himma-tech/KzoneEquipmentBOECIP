using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Models
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ProcessingMessage", Namespace = "http://schemas.datacontract.org/2004/07/EdgeUtility")]
    [System.SerializableAttribute()]
    public partial class ProcessingMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorMessageField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SuccessField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorCode
        {
            get
            {
                return this.ErrorCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ErrorCodeField, value) != true))
                {
                    this.ErrorCodeField = value;
                    this.RaisePropertyChanged("ErrorCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage
        {
            get
            {
                return this.ErrorMessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ErrorMessageField, value) != true))
                {
                    this.ErrorMessageField = value;
                    this.RaisePropertyChanged("ErrorMessage");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Success
        {
            get
            {
                return this.SuccessField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SuccessField, value) != true))
                {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public object ParamterObj { get; set; }
    }

}
