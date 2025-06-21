using Himma.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Communication.Interface
{ /// <summary>
  /// PLC通用接口
  /// </summary>
    public interface IHimmaPLC
    {
        /// <summary>
        /// 给PLC返回的int值变量
        /// </summary>
        //PLCObservableCollection<int> UpperToPlc { get; set; }
        /// <summary>
        /// 给PLC返回的float变量
        /// </summary>
        //PLCObservableCollection<float> UpperToPlcFloatData { get; set; }
        /// <summary>
        /// 下料参数
        /// </summary>
       // List<BasParaVariableModel> _paraList { get; set; }
        /// <summary>
        /// 实时参数
        /// </summary>
        //List<BasParaVariableModel> RealTimeVariableList { get; set; }
        /// <summary>
        /// ReadAsync
        /// </summary>
        //List<BasParaVariableModel> ProductionVariableList { get; set; }
        /// <summary>
        /// ReadAsync
        /// </summary>
        //List<BasParaVariableModel> WindFlawCoordinateList { get; set; }
        /// <summary>
        /// 实时参数
        /// </summary>
        //List<BasParaVariableModel> _monitorVariableList { get; set; }
        /// <summary>
        /// PLC读取值
        /// </summary>
        ObservableCollection<int> PLCVariables { get; set; }
        /// <summary>
        /// PLC名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 工序号
        /// </summary>
        string ProcessNo { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        string StationCode { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        string ProcessCode { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        string EqptNo { get; set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        bool IsConnected { get; set; }
        /// <summary>
        /// 订阅事件
        /// </summary>
        event Action<object, object> PLCNotifyEventArgs;
        event EventHandler<PLCSignalModel> SignalChanged;
        /// <summary>
        /// PLC连接
        /// </summary>
        /// <param name="ipdress">地址</param>
        /// <param name="port">端口,欧姆龙可以置0，CIP无影响</param>
        /// <param name="cycleTime">循环时间</param>
        void PLCInit(string ipdress, int port, int cycleTime, string name);
        /// <summary>
        /// 读取PLC内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ValueName">变量名</param>
        /// <param name="ValueName">线程控制量</param>
        /// <returns></returns>
        T Read<T>(string ValueName);

        /// <summary>
        /// 异步读取PLC内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ValueName">变量名</param>
        /// <returns></returns>
        Task<T> ReadAsync<T>(string ValueName);


        /// <summary>
        /// 读取PLC内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo">变量下标</param>
        /// <returns></returns>
        T Read<T>(int handleNo);


        /// <summary>
        /// 异步读取PLC内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo">变量下标</param>
        /// <returns></returns>
        Task<T> ReadAsync<T>(int handleNo);

        /// <summary>
        /// 读取string类型数据(OMRON)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo"></param>
        /// <param name="arrayno"></param>
        /// <returns></returns>
        string Read(int handleNo, int arrayno);

        /// <summary>
        /// 异步读取string类型数据(OMRON)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo"></param>
        /// <param name="arrayno"></param>
        /// <returns></returns>
        Task<string> ReadAsync(int handleNo, int arrayno);

        /// <summary>
        ///  读取PLC内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo">变量下标</param>
        /// <param name="arrayno"></param>
        T Read<T>(int handleNo, int arrayno);


        /// <summary>
        ///  异步读取PLC内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo">变量下标</param>
        /// <param name="arrayno"></param>
        Task<T> ReadAsync<T>(int handleNo, int arrayno);

        /// <summary>
        /// 变量读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo">变量句柄</param>
        /// <param name="startindex">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        T Read<T>(int handleNo, int startindex, int length = 0);

        /// <summary>
        /// 异步变量读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleNo">变量句柄</param>
        /// <param name="startindex">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<T> ReadAsync<T>(int handleNo, int startindex, int length = 0);

        /// <summary>
        /// 写入PLC内容
        /// </summary>
        /// <param name="handleNo">变量下标</param>
        /// /// <param name="value">值</param>
        void Write(int handleNo, object value);

        /// <summary>
        /// 异步写入PLC内容
        /// </summary>
        /// <param name="handleNo">变量下标</param>
        /// /// <param name="value">值</param>
        Task WriteAsync(int handleNo, object value);

        /// <summary>
        /// 写入PLC内容
        /// </summary>
        /// <param name="variableName">变量名</param>
        /// /// <param name="value">值</param>
        void Write(string variableName, object value);

        /// <summary>
        /// 异步写入PLC内容
        /// </summary>
        /// <param name="variableName">变量名</param>
        /// /// <param name="value">值</param>
        Task WriteAsync(string variableName, object value);

        /// <summary>
        /// 写入PLC String类型
        /// </summary>
        /// <param name="handleNo">变量下标</param>
        /// <param name="value">值</param>
        void Write(int handleNo, object value, int arrayNo);

        /// <summary>
        /// 异步写入PLC String类型
        /// </summary>
        /// <param name="handleNo">变量下标</param>
        /// <param name="value">值</param>
        Task WriteAsync(int handleNo, object value, int arrayNo);

        void SaveSignal(string name, dynamic data, int type);



    }
}
