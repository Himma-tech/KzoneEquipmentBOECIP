using Himma.Common.Log;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.File;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common
{
    public class CommonCSVHelper
    {
        //public static ConcurrentDictionary<string, Semaphore> _fileLocks = new ConcurrentDictionary<string, Semaphore>();
        /// <summary>
        /// 共享流 流写入会有4096 byte的缓存延迟 减少写入压力
        /// </summary>
        public static ConcurrentDictionary<string, BaseFileSink> _fileSWs = new ConcurrentDictionary<string, BaseFileSink>();
        /// <summary>
        /// 写入CSV文件 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <param name="fileName">文件名</param>
        /// <param name="isdivHour">按小时分开</param>
        /// <returns></returns>
        public async static Task SaveCSVFileAsync(string filePath, List<string> data, string header, string fileName = "", bool isdivHour = false, int retry = 0)
        {
            //Mutex mutex = new Mutex();
            retry++;
            if (retry > 2)
            {
                LogHelper.Error($"{fileName} 写入失败 重试次数{retry}", "SaveCSVFileAsync");
                return;
            }
            fileName = fileName == "" ? DateTime.Now.ToString("yyyy-MM-dd") : fileName;

            var timePath = System.IO.Path.Combine(DateTime.Now.ToString("yyyy-MM"), DateTime.Now.ToString("yyyy-MM-dd"));
            var _filepath_string = System.IO.Path.Combine(filePath, timePath);
            var _filepath = new DirectoryInfo(_filepath_string);
            if (!_filepath.Exists)
            {
                _filepath.Create();
            }

            string fullFileName = "";
            if (isdivHour) //拆分小时
            {
                fullFileName = System.IO.Path.Combine(_filepath.FullName, fileName + $"_{DateTime.Now.Hour}.csv");
            }
            else
            {
                fullFileName = System.IO.Path.Combine(_filepath.FullName, fileName + ".csv");
            }
            var isWriteHeader = !File.Exists(fullFileName);

            try
            {
                //mutex.WaitOne();
                var sw = _fileSWs.GetOrAdd(fullFileName, (key) =>
                {
                    var _output = new BaseFileSink(key, buffered: true);
                    LogHelper.Debug($"{key} 创建流成功", "SaveCSVFileAsync");
                    return _output;
                });
                if (isWriteHeader)
                {
                    sw.EmitLine(header);
                }
                foreach (var item in data)
                {
                    sw.EmitLine(item);
                }
                //mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{fileName} 写入失败 {ex.ToString()}", "SaveCSVFileAsync");
            }





        }


        /// <summary>
        /// 将内容写入或追加到 CSV 文件中，默认 UTF-8 编码。
        /// </summary>
        /// <param name="directory">文件所在目录。</param>
        /// <param name="fileName">文件名。</param>
        /// <param name="header">如果是创建新文件，需要写入的头部信息。</param>
        /// <param name="content">要写入或追加的内容。</param>
        /// <param name="append">是否追加到现有文件，默认为 true。</param>
        /// <param name="encoding">编码</param>
        /// <returns>返回元组类型结果，标识写入成功与最终文件路径</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public static async Task<(bool, string)> SaveCsvFileAsync(string directory, string fileName, string header, string content, bool append = true, Encoding encoding = null)
        {
            // 检查目录参数是否为空
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("目录不能为空或空字符串。", nameof(directory));
            }
            // 检查文件名参数是否为空
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("文件名不能为空或空字符串。", nameof(fileName));
            }
            // 如果没有指定编码，则使用 UTF-8
            encoding = encoding ?? Encoding.UTF8;
            try
            {
                // 确保目录存在
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                // 构建完整的文件路径
                string filePath = Path.Combine(directory, fileName + ".csv");

                if (File.Exists(filePath) && !append)
                {
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    filePath = Path.Combine(directory, $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}.csv");
                }
                // 确定文件模式是创建新文件还是追加到现有文件
                FileMode fileMode = append ? FileMode.Append : FileMode.Create;

                // 使用 FileStream 和 StreamWriter 写入文件
                using (FileStream writer = new FileStream(filePath, fileMode, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(writer, encoding))
                    {
                        // 如果是创建新文件，并且有头部信息，先写入头部
                        if (fileMode == FileMode.Create && !string.IsNullOrEmpty(header))
                        {
                            await sw.WriteLineAsync(header);
                        }
                        // 写入或追加内容
                        await sw.WriteLineAsync(content);
                    }
                }

                return (true, filePath);
            }
            catch (Exception e)
            {
                throw new ApplicationException("写入 CSV 文件失败。", e);
            }
        }

        /// <summary>
        /// 清理并且清空缓冲区写入文件 调度器使用 用来定期调用清理
        /// </summary>
        public static void ClearAndFlush()
        {
            //调用 目前是仅仅用来记录耗时信息的 场景固定 暂不考虑线程安全
            List<string> RemoveKeylist = new List<string>();
            //刷新写入所有文件
            foreach (var item in _fileSWs)
            {
                var writetime = DateTime.Now.Subtract(item.Value.LastWriteTime);
                if (writetime.TotalMinutes > 2)//两分钟以上没有新增内容的 写入磁盘
                {
                    item.Value.FlushToDisk();//写入磁盘
                }
                else
                if (writetime.TotalHours > 1)//一个小时内没有任何写入
                {
                    RemoveKeylist.Add(item.Key);

                }
            }
            foreach (var item in RemoveKeylist)
            {
                var rm = _fileSWs.TryRemove(item, out BaseFileSink _cache);
                if (rm)
                {
                    _cache.FlushToDisk();
                    _cache.Dispose();
                }
            }

            RemoveKeylist.Clear();
        }

    }
    /// <summary>
    /// 一个基础的线程安全的文件流终结点
    /// </summary>
    public class BaseFileSink : IDisposable
    {
        TextWriter _output = null;
        FileStream _underlyingStream = null;

        readonly string _path;

        readonly bool _buffered;
        readonly object _syncRoot = new object();
        Encoding _encoding = null;
        public DateTime LastWriteTime { get; private set; }
        /// <summary>
        /// 一个基础的线程安全的文件流终结点 
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encoding"></param>
        /// <param name="buffered">缓冲写入</param>
        public BaseFileSink(string path, Encoding encoding = null, bool buffered = false)
        {
            _path = path;
            _buffered = buffered;
            _encoding = encoding;

            CreatBaseStream();
        }
        private void CreatBaseStream()
        {
            try
            {
                _underlyingStream?.Dispose();
                _output?.Dispose();
                Stream outputStream = _underlyingStream = System.IO.File.Open(_path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                outputStream.Seek(0, SeekOrigin.End);

                _encoding = _encoding ?? new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

                _output = new StreamWriter(outputStream, _encoding);
                LastWriteTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{_path} 创建流失败 {ex.ToString()}", "SaveCSVFileAsync");
            }

        }
        /// <summary>
        /// 向文件流发射字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Emit(string data)
        {
            lock (_syncRoot)
            {
                if (_output == null)
                    CreatBaseStream();
                _output.Write(data);
                if (!_buffered)
                    _output.Flush();

                LastWriteTime = DateTime.Now;

                return true;
            }
        }
        /// <summary>
        /// 向文件流发射字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool EmitLine(string data)
        {
            lock (_syncRoot)
            {
                if (_output == null)
                    CreatBaseStream();
                _output.WriteLine(data);
                if (!_buffered)
                    _output.Flush();

                LastWriteTime = DateTime.Now;

                return true;
            }
        }
        public void Dispose()
        {
            lock (_syncRoot)
            {
                _output?.Dispose();
                _underlyingStream?.Dispose();
            }
        }
        public void FlushToDisk()
        {
            lock (_syncRoot)
            {
                _output?.Flush();
                _underlyingStream?.Flush(true);
            }
        }
    }
}
