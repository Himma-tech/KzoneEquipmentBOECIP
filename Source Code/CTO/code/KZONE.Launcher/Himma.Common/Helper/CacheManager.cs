using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 缓存管理类
    /// </summary>
    public class CacheManager
    {
        private static ConcurrentDictionary<string, object> _cacheDatas;
        private static object _objLock = new object();

        static CacheManager()
        {
            lock (_objLock)
            {
                if (_cacheDatas == null)
                    _cacheDatas = new ConcurrentDictionary<string, object>();
            }
        }

        /// <summary>
        /// 当前缓存对象
        /// </summary>
        public static ConcurrentDictionary<string, object> Current => _cacheDatas;

        /// <summary>
        /// 重置缓存
        /// </summary>
        public void ResetCaches()
        {
            _cacheDatas.Clear();
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Save(string key, object value)
        {
            if (!_cacheDatas.ContainsKey(key))
                _cacheDatas.TryAdd(key, value);
            else
                _cacheDatas[key] = value;
        }

        /// <summary>
        /// 根据key值查询缓存内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            object value;
            _cacheDatas.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// 根据key删除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key)
        {
            object value;
            _cacheDatas.TryRemove(key, out value);
        }



    }
}
