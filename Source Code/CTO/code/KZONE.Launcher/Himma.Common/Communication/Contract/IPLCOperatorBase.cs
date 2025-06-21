using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Communication.Contract
{
    public interface IPLCOperatorBase : IDisposable
    {
        /// <summary>
        /// 链接的客户端ID 用以分辨各个链接
        /// </summary>
        long ClientID { get; set; }
        string PlcNo { get; set; }
        void Write<T>(string variableName, T writeData);
        Task WriteAsync<T>(string variableName, T writeData);
        T Read<T>(string ReadVariable, int length = 0);
        Task<T> ReadAsync<T>(string ReadVariable, int length = 0);
        T Read<T>(string ReadVariable);
        Task<T> ReadAsync<T>(string ReadVariable);
        string ReadString(string variable);
        object[] ReadArrayString(string[] variable);
        Task<object[]> ReadArrayStringAsync(string[] variable);
        string[] ReadString(string variable, int startIdx, int length);
        void Active(string localAmsNetId, string targetAmsNetId, string ip, int interval = 100, int port = 0);
        void ActiveNew(string localAmsNetId, string targetAmsNetId, string ip, int interval = 100, int port = 0);
        void DisActive();
        bool IsActived();
        bool Connected { get; }
        /// <summary>
        /// 检查可用性
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        bool CheckAvailable(string variableName);
    }
}
