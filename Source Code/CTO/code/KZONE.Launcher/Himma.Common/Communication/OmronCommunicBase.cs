using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Himma.Common.Communication.Contract;
using Himma.Common.Log;

namespace Himma.Common.Communication
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public class OmronCommunicBase : IPLCOperatorBase
    {
        public bool IsConnect = false;
        //public OmronCipClient communic;
        public bool ConnectResult = false;
        //public int port = 44818;
        public OmronCommunicBase()
        {
            ClientID = NextId.Get();
            //communic = new OmronCipClient();
        }
        public event EventHandler<PLCSignalModel> SignalChanged;
        public bool Connected => true;
        public string PlcNo { get; set; }
        public long ClientID { get; set; }
        /// <summary>
        /// 是否使用LCC
        /// </summary>
        private bool UseLcc = true;
        //protected MachineAutomationControllerCompolet InstanceWrite;
       // HslCommunication.Profinet.Omron.OmronCipNet InstanceWrite;
        public void Active(string localAmsNetId, string targetAmsNetId, string ip, int interval = 100, int port = 44818)
        {
            UseLcc = false;
            try
            {
                if (port == 0) port = 44818;
                //InstanceWrite = new NJCompolet
                //{
                //    PeerAddress = ip,
                //    //LocalPort = port,
                //    ReceiveTimeLimit = interval
                //};
                //InstanceWrite.Active = true;
                //if (!InstanceWrite.IsConnected)
                //{
                //    InstanceWrite.Active = false;
                //}
                //InstanceWrite = new HslCommunication.Profinet.Omron.OmronCipNet();
                //InstanceWrite.IpAddress = ip;
                //InstanceWrite.Port = port;
                //InstanceWrite.ConnectTimeOut = interval;     // 连接超时，单位毫秒
                //InstanceWrite.ReceiveTimeOut = interval;     // 接收超时，单位毫秒
                //InstanceWrite.Slot = 0;
                //var res = InstanceWrite.ConnectServer();
                //if (res.IsSuccess)
                //{
                //    ConnectResult = true;
                //    IsConnect = true;
                //}
                else
                {
                    LogHelper.Error($"IP:{ip},Active失败！", "plc");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"IP:{ip},Active失败！错误提示【{ex}】", "plc");
                //throw ex;
            }

        }

        public void ActiveNew(string localAmsNetId, string targetAmsNetId, string ip, int interval = 100, int port = 44818)
        {
            try
            {
                UseLcc = true;
                if (port == 0) port = 44818;
                //连接前，先断开释放资源，再连接
                //this.communic.Disconnect();
                //var result = this.communic.Connect(ip, port, interval);
                //if (result.IsSuccess)
                //{
                //    ConnectResult = true;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error($"IP:{ip},Active失败！错误提示【{ex}】", "plc");
                //throw ex;
            }
        }

        public void DisActive()
        {
            //if (UseLcc)
            //{

            //    this.communic.Disconnect();
            //}
            //else
            //{
            //    //this.InstanceWrite.Active= false;
            //    //this.InstanceWrite.Dispose();
            //    this.InstanceWrite.ConnectClose();
            //    this.InstanceWrite.Dispose();
            //}
            ConnectResult = false;
        }

        public bool IsActived()
        {
            //if (UseLcc)
            //{
            //    return communic.IsConnected;
            //}
            
                return IsConnect;
           
        }



        /// <summary>
        /// 变量读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ReadVariable"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Read<T>(string ReadVariable)
        {
            if (UseLcc)
            {
                try
                {
                    //var rResult = communic.Read<T>(ReadVariable);
                    //if (rResult.IsSuccess)
                    //{
                    //    //SaveSignal(ReadVariable, rResult.Content, 0);
                    //    return rResult.Content;
                    //}
                    //else
                    //{
                    //    throw new Exception("PLC读取失败" + rResult.Message);
                    //}
                    return default(T);
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC读取异常" + ex);
                }
            }
            else
            {
                try
                {
                    object obj = null;
                    var msg = "";
                    //obj = HSLRead<T>(ReadVariable, ref msg);
                    if (obj == null)
                    {
                        throw new Exception(msg);
                    }

                    if (typeof(T).IsArray)
                    {
                        return (T)obj;
                    }
                    else
                    {
                        var arrayt = obj as T[];

                        return arrayt[0];
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("PLC读取异常" + ex);
                }

            }

        }

        /// <summary>
        /// 变量读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ReadVariable"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Read<T>(string ReadVariable, int length = 0)
        {
            if (UseLcc)
            {
                try
                {
                    //var rResult = communic.Read<T>(ReadVariable, (ushort)length);
                    //if (rResult.IsSuccess)
                    //{
                    //    return rResult.Content;
                    //}
                    //else
                    //{
                    //    throw new Exception("PLC读取失败" + rResult.Message);
                    //}
                    return default(T);
                }
                catch (Exception ex)
                {
                    throw new Exception("PLC读取异常" + ex);
                }
            }
            else
            {
                try
                {
                    object obj = null;
                    var msg = "";
                   // obj = HSLRead<T>(ReadVariable, ref msg, Convert.ToUInt16(length));
                    if (obj == null)
                    {
                        throw new Exception(msg);
                    }

                    if (typeof(T).IsArray)
                    {
                        return (T)obj;
                    }
                    else
                    {
                        var arrayt = obj as T[];

                        return arrayt[0];
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("PLC读取异常" + ex);
                }
            }

        }

        public async Task<T> ReadAsync<T>(string ReadVariable)
        {
            if (!UseLcc)
            {
                throw new Exception("方法没有实现");
            }
            //return ReadAsync<T>(ReadVariable.Trim(), 1);
            try
            {
                //var rResult = await communic.ReadAsync<T>(ReadVariable);
                //if (rResult.IsSuccess)
                //{
                //    return rResult.Content;
                //}
                //else
                //{
                //    throw new Exception($"PLC读取失败 ReadVariable:{ReadVariable}" + rResult.Message);
                //}
                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception($"PLC读取异常 ReadVariable:{ReadVariable}" + ex);
            }
        }

        public async Task<T> ReadAsync<T>(string ReadVariable, int length = 1)
        {
            if (!UseLcc)
            {
                throw new Exception("方法没有实现");
            }
            try
            {
                //var rResult = await communic.ReadAsync<T>(ReadVariable, (ushort)length);
                //if (rResult.IsSuccess)
                //{
                //    return rResult.Content;
                //}
                //else
                //{
                //    throw new Exception("PLC读取失败" + rResult.Message);
                //}
                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception("PLC读取异常" + ex);
            }
        }

        /// <summary>
        /// 读取单个字符变量
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string ReadString(string variable)
        {
            if (!UseLcc)
            {
                throw new Exception("方法没有实现");
            }
            try
            {
                //var rResult = communic.Read(variable);
                //if (rResult.IsSuccess)
                //{
                //    //SaveSignal(variable,rResult.Content.ToString(),0);
                //    return rResult.Content.ToString();
                //}
                //else
                //{
                //    throw new Exception("PLC读取失败" + rResult.Message);
                //}
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("PLC读取异常" + ex);
            }
        }

        /// <summary>
        /// 读取字符数组
        /// </summary>
        /// <param name="variable">标签名 </param>
        /// <param name="startIdx">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] ReadString(string variable, int startIdx, int length)
        {
            if (!UseLcc)
            {
                throw new Exception("方法没有实现");
            }
            try
            {
                //var rResult = communic.Read<string[]>(variable + $"[{startIdx}]", (ushort)length);
                //if (rResult.IsSuccess)
                //{
                //    //SaveSignal(variable + $"[{startIdx}]",rResult.Content,0);
                //    return rResult.Content;
                //}
                //else
                //{
                //    throw new Exception("PLC读取失败" + rResult.Message);
                //}
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("PLC读取异常" + ex);
            }
        }

        /// <summary>
        /// 批量读取字符数组
        /// </summary>
        /// <param name="variable">标签名 </param>
        /// <param name="startIdx">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public object[] ReadArrayString(string[] variable)
        {
            if (!UseLcc)
            {
                throw new Exception("方法没有实现");
            }
            try
            {
                throw new Exception();
                //var rResult = communic.Read(variable);
                //if (rResult.IsSuccess)
                //{
                //    return rResult.Content;
                //}
                //else
                //{
                //    throw new Exception("PLC读取失败" + rResult.Message);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("PLC读取异常" + ex);
            }
        }

        /// <summary>
        /// 异步批量读取字符数组
        /// </summary>
        /// <param name="variable">标签名 </param>
        /// <param name="startIdx">起始位置</param>
        /// <param name="length">读取长度</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<object[]> ReadArrayStringAsync(string[] variable)
        {
            if (!UseLcc)
            {
                throw new Exception("方法没有实现");
            }
            try
            {
                throw new Exception();
                //var rResult = await communic.ReadAsync(variable);
                //if (rResult.IsSuccess)
                //{
                //    return rResult.Content;
                //}
                //else
                //{
                //    throw new Exception("PLC读取失败" + rResult.Message);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("PLC读取异常" + ex);
            }
        }

        /// <summary>
        /// 写入变量
        /// </summary>
        /// <param name="variableName">标签名</param>
        /// <param name="writeData">写入内容</param>
        /// <exception cref="Exception"></exception>
        //public void Write<T>(string variableName, T writeData)
        //{
        //    if (UseLcc)
        //    {
        //        try
        //        {
        //            var rResult = communic.Write(variableName, writeData);
        //            if (!rResult.IsSuccess)
        //            {
        //                throw new Exception($"PLC写入{variableName}变量失败;{writeData} ,原因：" + rResult.Message);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"PLC写入{variableName}变量异常;{ex.ToString()}");
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var msg = "";
        //            var res = HSLWrite(variableName, writeData, ref msg);
        //            if (!res)
        //            {
        //                throw new Exception($"PLC写入{variableName}变量异常;{msg}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw new Exception($"PLC写入{variableName}变量异常;{ex.ToString()}");
        //        }
        //    }
        //}

        /// <summary>
        /// 异步写入变量
        /// </summary>
        /// <param name="variableName">标签名</param>
        /// <param name="writeData">写入内容</param>
        /// <exception cref="Exception"></exception>
        //public async Task WriteAsync<T>(string variableName, T writeData)
        //{
        //    if (UseLcc)
        //    {
        //        var rResult = await communic.WriteAsync(variableName, writeData);
        //        if (!rResult.IsSuccess)
        //        {
        //            throw new Exception($"[OML]PLC写入{variableName}变量失败;{writeData} ,原因：" + rResult.Message);
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var msg = "";
        //            var res = HSLWrite(variableName, writeData, ref msg);
        //            if (!res)
        //            {
        //                throw new Exception($"PLC写入{variableName}变量异常;{msg}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw new Exception($"PLC写入{variableName}变量异常;{ex.ToString()}");
        //        }
        //    }

        //    // SaveSignal(variableName, writeData, 1);

        //}

        /// <summary>
        /// 信号交互页面保存信号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SaveSignal(string name, dynamic data, int type)
        {
            return;
            try
            {
                PLCSignalModel signal = new PLCSignalModel()
                {
                    name = name,
                    data = data.ToString(),
                    type = type
                };
                SignalChanged?.Invoke(this, signal);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"信号交互页面保存plc数据失败：【{ex.ToString()}】", "SaveSignal");
            }

        }

        //public void Dispose()
        //{
        //    communic.Disconnect();
        //}

        public bool CheckAvailable(string variableName)
        {
            try
            {
                if (UseLcc)
                {
                    Read<short>(variableName);
                }
                else
                {
                    Read<short>(variableName);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"CheckAvailable 检查可用性失败 {ex.Message}变量异常;");
            }

        }

        public void Write<T>(string variableName, T writeData)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync<T>(string variableName, T writeData)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        //public object HSLRead<T>(string address, ref string Msg, ushort length = 1)
        //{
        //    try
        //    {
        //        Type typeFromHandle = typeof(T);
        //        switch (typeFromHandle.ToString())
        //        {
        //            case "System.Byte":
        //                {
        //                    OperateResult<byte[]> operateResult9 = InstanceWrite.Read(address, 1);
        //                    if (operateResult9.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult9.Content;
        //                    }
        //                    Msg = operateResult9.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Int16":
        //                {
        //                    OperateResult<short[]> operateResult3 = InstanceWrite.ReadInt16(address, 1);
        //                    if (operateResult3.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult3.Content;
        //                    }
        //                    Msg = operateResult3.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.UInt16":
        //                {
        //                    OperateResult<ushort[]> operateResult14 = InstanceWrite.ReadUInt16(address, 1);
        //                    if (operateResult14.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult14.Content;
        //                    }
        //                    Msg = operateResult14.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Int32":
        //                {
        //                    OperateResult<int[]> operateResult15 = InstanceWrite.ReadInt32(address, 1);
        //                    if (operateResult15.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult15.Content;
        //                    }
        //                    Msg = operateResult15.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.UInt32":
        //                {
        //                    OperateResult<uint[]> operateResult6 = InstanceWrite.ReadUInt32(address, 1);
        //                    if (operateResult6.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult6.Content;
        //                    }
        //                    Msg = operateResult6.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Int64":
        //                {
        //                    OperateResult<long[]> operateResult18 = InstanceWrite.ReadInt64(address, 1);
        //                    if (operateResult18.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult18.Content;
        //                    }
        //                    Msg = operateResult18.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Boolean":
        //                {
        //                    OperateResult<bool[]> operateResult8;
        //                    operateResult8 = InstanceWrite.ReadBoolArray(address);
        //                    if (operateResult8.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult8.Content;
        //                    }
        //                    Msg = operateResult8.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Single":
        //                {
        //                    OperateResult<float[]> operateResult2 = InstanceWrite.ReadFloat(address, 1);
        //                    if (operateResult2.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult2.Content;
        //                    }
        //                    Msg = operateResult2.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Double":
        //                {
        //                    OperateResult<double[]> operateResult11 = InstanceWrite.ReadDouble(address, 1);
        //                    if (operateResult11.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult11.Content;
        //                    }
        //                    Msg = operateResult11.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Int16[]":
        //                {
        //                    OperateResult<short[]> operateResult5 = InstanceWrite.ReadInt16(address, length);
        //                    if (operateResult5.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult5.Content;
        //                    }
        //                    Msg = operateResult5.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.UInt16[]":
        //                {
        //                    OperateResult<ushort[]> operateResult4 = InstanceWrite.ReadUInt16(address, length);
        //                    if (operateResult4.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult4.Content;
        //                    }
        //                    Msg = operateResult4.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Int32[]":
        //                {
        //                    OperateResult<int[]> operateResult17 = InstanceWrite.ReadInt32(address, length);
        //                    if (operateResult17.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult17.Content;
        //                    }
        //                    Msg = operateResult17.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.UInt32[]":
        //                {
        //                    OperateResult<uint[]> operateResult12 = InstanceWrite.ReadUInt32(address, length);
        //                    if (operateResult12.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult12.Content;
        //                    }
        //                    Msg = operateResult12.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Boolean[]":
        //                {
        //                    OperateResult<bool[]> operateResult16;
        //                    operateResult16 = InstanceWrite.ReadBoolArray(address);
        //                    if (operateResult16.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult16.Content;
        //                    }
        //                    Msg = operateResult16.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Single[]":
        //                {
        //                    OperateResult<float[]> operateResult13 = InstanceWrite.ReadFloat(address, length);
        //                    if (operateResult13.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult13.Content;
        //                    }
        //                    Msg = operateResult13.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Double[]":
        //                {
        //                    OperateResult<double[]> operateResult10 = InstanceWrite.ReadDouble(address, length);
        //                    if (operateResult10.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult10.Content;
        //                    }
        //                    Msg = operateResult10.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.Byte[]":
        //                {
        //                    OperateResult<byte[]> operateResult7 = InstanceWrite.Read(address, length);
        //                    if (operateResult7.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult7.Content;
        //                    }
        //                    Msg = operateResult7.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            case "System.String":
        //                {
        //                    OperateResult<string> operateResult = InstanceWrite.ReadString(address, length, Encoding.ASCII);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return operateResult.Content.Split(default(char))[0].Trim();
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return null;
        //                }
        //            default:
        //                Msg = typeFromHandle.ToString() + " error!";
        //                return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Msg = ex + Environment.NewLine + ex.StackTrace;
        //        return null;
        //    }
        //}

        //public bool HSLWrite<T>(string address, T value, ref string Msg)
        //{
        //    try
        //    {
        //        Type type = value.GetType();
        //        switch (type.ToString())
        //        {
        //            case "System.Byte":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (byte)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Int16":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (short)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.UInt16":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (ushort)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Int32":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (int)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.UInt32":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (uint)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Int64":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (long)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Boolean":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (bool)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Single":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (float)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Double":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (double)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.String":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (string)(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Int16[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (short[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.UInt16[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (ushort[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Int32[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (int[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.UInt32[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (uint[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Int64[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (long[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Boolean[]":
        //                {
        //                    OperateResult operateResult;
        //                    operateResult = InstanceWrite.Write(address, (bool[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Single[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (float[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Double[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (double[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            case "System.Byte[]":
        //                {
        //                    OperateResult operateResult = InstanceWrite.Write(address, (byte[])(object)value);
        //                    if (operateResult.IsSuccess)
        //                    {
        //                        this.IsConnect = true;
        //                        return true;
        //                    }
        //                    Msg = operateResult.ToMessageShowString();
        //                    this.IsConnect = false;
        //                    return false;
        //                }
        //            default:
        //                Msg = type.ToString() + " error!!";
        //                return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Msg = ex + Environment.NewLine + ex.StackTrace;
        //        return false;
        //    }
        //}
    }
}