using Himma.Common.Helper.Log;
using Microsoft.IdentityModel.Protocols;
using Serilog;
using SharpCompress.Archives;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using System;
using System.Collections.Concurrent;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;



namespace Himma.Common.Log
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 日志滚动模式
        /// </summary>
        private const RollingInterval _rollingInterval = RollingInterval.Day;

        //private static ILog _log;
        //private static object _obj = new object();
        /// <summary>
        /// 缓存的log变量 希望Ilog没有线程安全问题
        /// </summary>
        private static ConcurrentDictionary<string, Serilog.Core.Logger> _IlogCache = new ConcurrentDictionary<string, Serilog.Core.Logger>();
        /// <summary>
        /// 日志滚动天数
        /// </summary>
        private static int _rollingMaxDay = GlobalConfig.LogBackupsMaxDay;

        // 创建ReaderWriterLockSlim
        private static ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        static string exTitle = $"进站时间，出站时间,结果";

        static string strTitle = $"开始时间,结束时间,耗时(ms)";

        static LogHelper()
        {
        }

        public static event EventHandler InfoNotify;
        /// <summary>
        /// 手动操作日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logName"></param>
        public static void ActionLog(string message, string logName = "ManualAction")
        {
            Debug($"{message}", logName);
            //ATL.Core.PlcDAL.ThisObject.SaveUserAction(ATL.Core.Core.UserName, message);         
        }

        /// <summary>
        /// 清理和压缩旧日志文件
        /// </summary>
        /// <returns></returns>
        public static async Task ClearAndBackupAsync()
        {
            var logpath = ConfigurationManager.AppSettings["LogFilePath"];
            //备份目录
            var zippath = Path.Combine(ConfigurationManager.AppSettings["LogBackupsPath"], "LogBackups");
            Directory.CreateDirectory(zippath);
            var deletetime = DateTime.Now.AddDays(-_rollingMaxDay);
            List<string> cleandirs = new List<string>();
            foreach (var item in Directory.EnumerateDirectories(logpath))
            {
                var dirinfo = new DirectoryInfo(item);
                if (dirinfo.CreationTime < deletetime)
                {
                    cleandirs.Add(item);
                    if (cleandirs.Count > 2)//一次最多清理3个目录
                    {
                        break;
                    }
                }

            }

            foreach (var item in cleandirs)
            {
                var dirinfo = new DirectoryInfo(item);
                var zipfilepath = Path.Combine(zippath, dirinfo.Name + ".zip");
                if (!File.Exists(zipfilepath))//如果存在对应的zip文件则跳过
                {
                    using (var archive = SharpCompress.Archives.Zip.ZipArchive.Create())
                    {
                        archive.AddAllFromDirectory(item);
                        WriterOptions writerOptions = new ZipWriterOptions(SharpCompress.Common.CompressionType.Deflate);
                        //writerOptions.ArchiveEncoding.Default = CompressionLevel.Optimal;
                        archive.SaveTo(zipfilepath, writerOptions);
                    }
                    try
                    {
                        Directory.Delete(item, true);
                    }
                    catch (Exception ex)
                    {
                        //客户权限导致的删除失败需要屏蔽
                    }
                    //ZipFile.CreateFromDirectory(item, zipfilepath,CompressionLevel.Optimal,true);
                }

            }

        }

        /// <summary>
        /// 清理过期的Log对象
        /// </summary>
        public static void ClearLogObj()
        {
            if (_rwLock.TryEnterWriteLock(200))//尝试进入锁
            {
                try
                {
                    var lasttime = DateTime.Now;
                    string datestring = "";
                    string dateformat = "yyyyMMdd";
                    switch (_rollingInterval)
                    {
                        case RollingInterval.Infinite:
                            break;
                        case RollingInterval.Year:
                            lasttime = DateTime.Now.AddYears(-1);
                            dateformat = "yyyy";
                            break;
                        case RollingInterval.Month:
                            lasttime = DateTime.Now.AddMonths(-1);
                            dateformat = "yyyyMM";
                            break;
                        case RollingInterval.Day:
                            lasttime = DateTime.Now.AddDays(-1);
                            dateformat = "yyyyMMdd";
                            break;
                        case RollingInterval.Hour:
                            lasttime = DateTime.Now.AddHours(-1);
                            dateformat = "yyyyMMddHH";
                            break;
                        case RollingInterval.Minute:
                            lasttime = DateTime.Now.AddMinutes(-1);
                            dateformat = "yyyyMMddHHmm";
                            break;
                        default:
                            break;
                    }
                    datestring = lasttime.ToString(dateformat);
                    var oldlog = _IlogCache.Where(T => T.Key.StartsWith(datestring)).ToList();
                    if (oldlog.Count > 0)
                    {
                        foreach (var item in oldlog)
                        {
                            Serilog.Core.Logger _cacheitem;
                            var rm = _IlogCache.TryRemove(item.Key, out _cacheitem);
                            if (rm)
                            {
                                _cacheitem.Dispose();

                                Debug($"Log {item.Key} Dispose", "ClearLog");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    ;
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
        }

        public static void Debug(string message, string logType = "kzone")
        {
            if (GlobalConfig.IsShowDebugLog == 2)
            {
                //return;
            }
            message = DateTime.Now.ToString() + "| " + message;
            _log(logType, "Debug").Debug("LogType:{LogType} Log:" + message, logType);
            //InfoNotify?.Invoke(null, new LogEventArg { Message = message, LogType = logType });

        }

        public static void Dispose()
        {
            Serilog.Log.CloseAndFlush();
            foreach (var item in _IlogCache)
            {
                item.Value.Dispose();

            }
        }

        public static void Error(string message, string logType = "Error")
        {
            message = DateTime.Now.ToString() + "| " + message;
            //_log(logType, "Debug").Debug(message);
            _log(logType, "Error").Error("LogType:{LogType} Log:" + message, logType);
            //InfoNotify?.Invoke(null, new LogEventArg { Message = message, LogType = logType });
            InfoNotify?.BeginInvoke(null, new LogEventArg { Message = message, LogType = logType }, null, null);
        }

        //public static void Fatal(string message, string logType = "PlcOperatorPool")
        //{
        //    message = DateTime.Now.ToString() + "| " + message;
        //    //_log(logType, "Debug").Debug(message);
        //    _log(logType, "Fatal").Fatal("LogType:{LogType} Log:" + message, logType);
        //}
        /// <summary>
        /// 创建日期的Log对象
        /// </summary>
        /// <param name="datastring"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Serilog.Core.Logger GetSerilogLoggerByName(string datastring, string name)
        {
            //$"{ConfigurationManager.AppSettings["LogFilePath"]}\\{DateTime.Now.ToString("yyyyMM")}\\{DateTime.Now.ToString("MMdd")}\\{name.Split('-')[1]}\\{name}.log";
            var logpathInfo = Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], datastring, "Info", $"{name}.txt");
            var logpathDebug = Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], datastring, "Debug", $"{name}.txt");
            var logpathError = Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], datastring, "Error", $"{name}.txt");
            var logpathFatal = Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], datastring, "Fatal", $"{name}.txt");
            var LongSize = 10485760L;  //大小10m 滚动
            var logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
                 //.WriteTo.Seq("http://localhost:5341")//发送到默认的seq
                 //.WriteTo.Async(T => T.File(logpath, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 10485760L, rollOnFileSizeLimit: true))
                 // 调试日志
                 .WriteTo.Logger(x => x
                     .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Information)
                    
                     .WriteTo.File(logpathInfo, fileSizeLimitBytes: LongSize, rollOnFileSizeLimit: true, shared: false, encoding: Encoding.UTF8, buffered: false)
                 )
                 // 调试日志 --后续考虑延时写入 buffered: true, flushToDiskInterval: TimeSpan.FromMinutes(1)
                 .WriteTo.Logger(x => x
                     .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Debug)
                    
                     .WriteTo.File(logpathDebug, fileSizeLimitBytes: LongSize, rollOnFileSizeLimit: true, shared: false, encoding: Encoding.UTF8, buffered: false)//buffere
                 )
                 // 错误日志
                 .WriteTo.Logger(x => x
                     .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Error)
                    
                     .WriteTo.File(logpathError, fileSizeLimitBytes: LongSize, rollOnFileSizeLimit: true, shared: false, encoding: Encoding.UTF8, buffered: false)
                 )
               //.WriteTo.Logger(x => x
               //// 致命错误日志?
               //    .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Fatal)
               //   //.WriteTo.Async(T => T.File(logpathFatal, fileSizeLimitBytes: LongSize, rollOnFileSizeLimit: true, shared: true, encoding: Encoding.UTF8, buffered: false))
               //   .WriteTo.File(logpathFatal, fileSizeLimitBytes: LongSize, rollOnFileSizeLimit: true, shared: false, encoding: Encoding.UTF8, buffered: false)
               //)
               .CreateLogger();
            logger.Debug("创建日志对象");
            return logger;
        }

        public static void Info(string message, string logType = "kzone")
        {
            message = DateTime.Now.ToString() + "| " + message;
            _log(logType, "info").Information("LogType:{LogType} Log:" + message, logType);
            //_log(logType, "Debug").Debug(message);

            //InfoNotify?.Invoke(null, new LogEventArg { Message = message, LogType = logType });
            InfoNotify?.BeginInvoke(null, new LogEventArg { Message = message, LogType = logType }, null, null);
        }

        /// <summary>
        /// MES接口调用日志文件
        /// </summary>
        /// <param name="strBuilder"></param>
        /// <param name="logType"></param>
        /// <param name="noticeShow"></param>
        /// <param name="listBoxShow"></param>
        public async static void InvokeMESLog(string strBuilder, string typeStr)
        {
            strBuilder = $"{DateTime.Now.ToString("HH:mm:ss")}，{strBuilder.ToString()}\r\n";

            DirectoryInfo filepath = new DirectoryInfo(ConfigurationManager.AppSettings["MesLogPath"] + "/" + typeStr);
            if (!filepath.Exists)
            {
                filepath.Create();
            }
            var fileName = filepath + " / " + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            await Task.Run(() =>
            {
                var fmode = !File.Exists(fileName)
                ? FileMode.Create : FileMode.Append;
                // 存入文件
                using (var fs1 = new FileStream(fileName, fmode, FileAccess.Write, FileShare.ReadWrite))
                {
                    var sw = new StreamWriter(fs1, Encoding.UTF8);
                    sw.WriteLine(strBuilder);
                    sw.Flush();
                    sw.Close();
                }
            });
        }

        public static void Test(string message, string logType = "Debug")
        {
            message = DateTime.Now.ToString() + "| " + message;
            //_log(logType, "Debug").Debug(message);
            _log(logType, "Debug").Debug("LogType:{LogType} Log:" + message, logType);
        }

        private static Serilog.Core.Logger _log(string logName, string method)
        {
            
            string datestring = "";
            string dateformat = "yyyyMMdd";
            switch (_rollingInterval)
            {
                case RollingInterval.Infinite:
                    break;
                case RollingInterval.Year:
                    dateformat = "yyyy";
                    break;
                case RollingInterval.Month:
                    dateformat = "yyyyMM";
                    break;
                case RollingInterval.Day:
                    dateformat = "yyyyMMdd";
                    break;
                case RollingInterval.Hour:
                    dateformat = "yyyyMMddHH";
                    break;
                case RollingInterval.Minute:
                    dateformat = "yyyyMMddHHmm";
                    break;
                default:
                    break;
            }
            //string _logname = $"{logName}-{method}";
            datestring = DateTime.Now.ToString(dateformat);
            string ln = $"{datestring}-{logName}";//将天考虑进去
            if (_IlogCache.ContainsKey(ln))
            {
                return _IlogCache[ln];
            }
            else
            {
                if (_rwLock.TryEnterWriteLock(200))//尝试进入锁
                {
                    try
                    {
                        return _IlogCache.GetOrAdd(ln, (key) => GetSerilogLoggerByName(datestring, logName));
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        _rwLock.ExitWriteLock();
                    }
                }
                else
                {
                    Thread.SpinWait(100);
                    return _log(logName, method);
                }

            }
            //lock (_obj)
            //{
            //    return GetLoggerByName(ln);
            //}
        }
    }
}
