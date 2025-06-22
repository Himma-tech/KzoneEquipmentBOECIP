using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    /// <summary>
    /// 计量方法帮助类
    /// </summary>
    public class CommonMiniProfiler : IDisposable
    {
        //public static Lazy<MiniProfiler> CaacBakingMiniProfiler = new Lazy<MiniProfiler>(() => MiniProfiler.StartNew("CaacBaking"));
        //static MiniProfiler _miniProfiler;
        ConcurrentDictionary<string, CommonMiniProfilerItem> allDictionary = new ConcurrentDictionary<string, CommonMiniProfilerItem>();
        ConcurrentDictionary<string, double> alldata = new ConcurrentDictionary<string, double>();
        public CommonMiniProfiler(string name)
        {
            StartNew(name);
        }

        private CommonMiniProfilerItem StartNew(string name)
        {
            var cache = allDictionary.GetOrAdd(name, (key) => Creat(key));
            return cache;
        }

        private CommonMiniProfilerItem Creat(string _name)
        {
            return new CommonMiniProfilerItem(Stopwatch.StartNew(), _name, (time, name) =>
            {
                alldata.TryAdd(name, time);
            });
        }

        /// <summary>
        /// 获取一个计量单位
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CommonMiniProfilerItem Step(string name)
        {
            return StartNew(name);
        }
        /// <summary>
        /// 获取耗时信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> GetInfoData()
        {
            foreach (var item in allDictionary)
            {
                if (item.Value != null)
                {
                    item.Value.Dispose();
                }
            }
            var dir = alldata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return dir;
        }
        /// <summary>
        /// 获取耗时信息
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {

            List<string> datas = new List<string>();
            foreach (var item in GetInfoData())
            {
                datas.Add($"{item.Key}>>>>{item.Value}ms");
            }
            return "\n" + string.Join("\n", datas.ToArray());
        }
        public void Dispose()
        {
            foreach (var item in allDictionary)
            {
                if (item.Value != null)
                {
                    item.Value.Dispose();
                }
            }
            allDictionary.Clear();
            alldata.Clear();
            //_miniProfiler.Stop();
            //var profiler = MiniProfiler.StartNew("CaacBaking");
            //using (profiler.Step("Main Work"))
            //{
            //    // Do some work...
            //}
            //profiler.Stop();
            //Console.WriteLine(profiler.RenderPlainText());

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 保存信息到CSV
        /// </summary>
        /// <param name="infodata">耗时信息</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        [Obsolete]
        public static async Task SaveCSVAsync(Dictionary<string, double> infodata, string filepath, string filename)
        {
            try
            {
                var header = "Time," + string.Join(",", infodata.Keys.ToList());
                var datastring = infodata.Values.Select(T => T.ToString("f3")).ToList();
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ',' + string.Join(",", datastring);
                Task.Run(async () =>
                {
                    await CommonCSVHelper.SaveCSVFileAsync(filepath, new List<string>() { data }, header, filename, true);//改为不等待写入}); 
                });
            }
            catch (Exception)
            {
                ;//SaveCSVFileAsync内部有日志
            }

        }
        /// <summary>
        /// 保存信息到CSV
        /// </summary>
        /// <param name="infodata">耗时信息</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static void SaveCSV(Dictionary<string, double> infodata, string filepath, string filename)
        {
            try
            {
                var header = "Time," + string.Join(",", infodata.Keys.ToList());
                var datastring = infodata.Values.Select(T => T.ToString("f3")).ToList();
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ',' + string.Join(",", datastring);
                Task.Run(async () =>
                {
                    await CommonCSVHelper.SaveCSVFileAsync(filepath, new List<string>() { data }, header, filename, true);//改为不等待写入}); 
                });
            }
            catch (Exception)
            {
                ;//SaveCSVFileAsync内部有日志
            }

        }
    }

    public class CommonMiniProfilerItem : IDisposable
    {
        private Stopwatch _miniProfiler;
        private Action<double, string> _dispose;
        private string _name;

        public CommonMiniProfilerItem(Stopwatch cMiniProfiler, string name, Action<double, string> dispose)
        {
            _miniProfiler = cMiniProfiler;
            _dispose = dispose;
            _name = name;
        }

        public void Dispose()
        {
            _miniProfiler.Stop();
            _dispose.Invoke(_miniProfiler.Elapsed.TotalMilliseconds, _name);
        }
    }
}
