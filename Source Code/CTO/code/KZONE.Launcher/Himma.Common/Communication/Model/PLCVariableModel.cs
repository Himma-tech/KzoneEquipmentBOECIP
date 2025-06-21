using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common.Communication.Model
{
    public class PLCVariableModel : ICloneable
    {
        /// <summary>
        /// PLC地址 用来区分到底是哪个plc推送的 这个地方存在缺陷 应该是根据PLC连接来区分
        /// </summary>
        public string PLCIPAddress { get; set; }
        public Guid TasKId { get; set; }
        private string _variableDescription;
        /// <summary>
        /// 变量描述
        /// </summary>
        public string VariableDescrition
        {
            get { return _variableDescription; }
            set { _variableDescription = value; }
        }

        private string _variableName;
        /// <summary>
        /// 变量名
        /// </summary>
        public string VariableName
        {
            get { return _variableName; }
            set { _variableName = value; }
        }

        ///// <summary>
        ///// 变量值
        ///// </summary>
        //public object VariableValue
        //{
        //    get { return _variableValue; }
        //    set
        //    {
        //        if (int.Parse(_variableValue.ToString()) != int.Parse(value.ToString()))
        //        {
        //            _variableValue = value;
        //            RaisePropertyChangedEvent("VariableValue");
        //        }
        //    }
        //}
        private object _lock = new object();
        private short _variableValue = 0;
        /// <summary>
        /// 变量值
        /// </summary>
        public short VariableValue
        {
            get
            {
                return _variableValue;

            }
            set
            {
                lock (_lock)
                {
                    try
                    {
                        // 写入操作
                        if (_variableValue != value)
                        {
                            _variableValue = value;
                            RaisePropertyChangedEvent("VariableValue");
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

            }
        }

        private int _variableHandle;
        /// <summary>
        /// 变量触发句柄
        /// </summary>
        public int VariableHandle
        {
            get { return _variableHandle; }
            set { _variableHandle = value; }
        }

        private int _variabdownleHandle;
        /// <summary>
        /// 变量反馈句柄
        /// </summary>
        public int VariableDownHandle
        {
            get { return _variabdownleHandle; }
            set { _variabdownleHandle = value; }
        }

        private int _arrVariableHandle;
        /// <summary>
        /// 如果定义的是数组类型，反馈数组内的第几位
        /// 非数组类型的变量，一般反馈0
        /// </summary>
        public int ArrVariableHandle
        {
            get { return _arrVariableHandle; }
            set { _arrVariableHandle = value; }
        }
        private string _bizType;
        /// <summary>
        /// 触发信号对应的业务类型
        /// </summary>
        public string BizType
        {
            get { return _bizType; }
            set { _bizType = value; }
        }

        private int _component_sid;
        /// <summary>
        /// 关联的组件sid
        /// </summary>
        public int ComponentSid
        {
            get { return _component_sid; }
            set { _component_sid = value; }
        }

        public event EventHandler<PLCVariableModel> PropertyChanged;
        private void RaisePropertyChangedEvent(string name)
        {
            //Task.Run(() => { PropertyChanged?.BeginInvoke(this, new PropertyChangedEventArgs(name), null, null); });
            //if (this.PropertyChanged != null)
            //    this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //this.PropertyChanged?.BeginInvoke(this, new PropertyChangedEventArgs(name), null, null);
            //this.PropertyChanged?.BeginInvoke(this.Clone(), new PropertyChangedEventArgs(name), null, null);
            this.PropertyChanged?.Invoke(null, this.CloneMe());
            //GlobalMessagePipe.GetAsyncPublisher<PLCVariableModel>().PublishAsync(this.CloneMe());
        }

        public object Clone()
        {
            return CloneMe();
        }
        public PLCVariableModel CloneMe()
        {
            var _cl = Newtonsoft.Json.JsonConvert.DeserializeObject<PLCVariableModel>(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            _cl.TasKId = Guid.NewGuid();
            return _cl;
        }
    }
    /// <summary>
    /// PLC信号触发的业务处理类型
    /// </summary>
    public enum BizTypeEnum
    {
        /// <summary>
        /// 1电芯入站
        /// </summary>
        [Description("1")]
        BatteryForIn,
        /// <summary>
        /// 2辅料入站
        /// </summary>
        [Description("2")]
        FeedingForIn,
        /// <summary>
        /// 3电芯出站
        /// </summary>
        [Description("3")]
        BatteryForOut,
        /// <summary>
        /// 4辅料上料
        /// </summary>
        [Description("4")]
        FeedingInRequest,
        /// <summary>
        /// 5刷卡器
        /// </summary>
        [Description("5")]
        CreditCardRequest,
        /// <summary>
        /// 6设备报修
        /// </summary>
        [Description("6")]
        RepairRequest,
        /// <summary>
        /// 7扫码枪
        /// </summary>
        [Description("7")]
        ScannerRequest,
        /// <summary>
        /// 8托杯交换
        /// </summary>
        [Description("8")]
        CupChangeRequest,
        /// <summary>
        /// 9喷码信息上传
        /// </summary>
        [Description("9")]
        PrintRequest,
        /// <summary>
        /// 10数据实时采集
        /// </summary>
        [Description("10")]
        CollectRealTimeData,
        /// <summary>
        /// 11辅料下料
        /// </summary>
        [Description("11")]
        FeedingOutRequest,
        /// <summary>
        /// 12装盘机绑盘触发
        /// </summary>
        [Description("12")]
        TrayloadBindRequest,
        /// <summary>
        /// 13Hipot实时读取块触发
        /// </summary>
        [Description("13")]
        HipotRealTimeRead,
        /// <summary>
        /// 14Hipot操作读取块触发
        /// </summary>
        [Description("14")]
        HipotHandleRead,
        /// <summary>
        /// 15Hipot操作写入块触发
        /// </summary>
        [Description("15")]
        HipotHandleWrite,
        /// <summary>
        /// 16卷绕NG保存触发
        /// </summary>
        [Description("16")]
        WindNGForOut,
        /// <summary>
        /// 17卷绕蓝标触发
        /// </summary>
        [Description("17")]
        WindFlawCoordinate
    }

}
