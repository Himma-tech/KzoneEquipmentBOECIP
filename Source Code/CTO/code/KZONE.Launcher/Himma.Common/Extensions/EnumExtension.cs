using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        public static string GetDescription(this Enum @this)
        {
            return _ConcurrentDictionary.GetOrAdd(@this, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.GetDescriptions() : GetDescription(field);
            });
        }

        /// <summary>
        /// 获取位域枚举的描述，多个按分隔符组合
        /// </summary>
        public static string GetDescriptions(this Enum @this, string separator = ",")
        {
            var names = @this.ToString().Split(',');
            string[] res = new string[names.Length];
            var type = @this.GetType();
            for (int i = 0; i < names.Length; i++)
            {
                var field = type.GetField(names[i].Trim());
                if (field == null) continue;
                res[i] = GetDescription(field);
            }
            return string.Join(separator, res);
        }

        private static string GetDescription(FieldInfo field)
        {
            var att = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }

        /****************** test methods ******************/

        public static string GetDescriptionOriginal(this Enum @this)
        {
            var name = @this.ToString();
            var field = @this.GetType().GetField(name);
            if (field == null) return name;
            var att = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }

        private static Dictionary<Enum, string> _LockDictionary = new Dictionary<Enum, string>();
        public static string GetDescriptionByDictionaryWithLocak(this Enum @this)
        {
            if (_LockDictionary.ContainsKey(@this)) return _LockDictionary[@this];
            Monitor.Enter(_obj);
            if (!_LockDictionary.ContainsKey(@this))
            {
                var value = @this.GetDescriptionOriginal();
                _LockDictionary.Add(@this, value);
            }
            Monitor.Exit(_obj);
            return _LockDictionary[@this];
        }

        private static Dictionary<Enum, string> _ExceptionDictionary = new Dictionary<Enum, string>();
        public static string GetDescriptionByDictionaryWithException(this Enum @this)
        {
            try
            {
                return _ExceptionDictionary[@this];
            }
            catch (KeyNotFoundException)
            {
                Monitor.Enter(_obj);
                if (!_ExceptionDictionary.ContainsKey(@this))
                {
                    var value = @this.GetDescriptionOriginal();
                    _ExceptionDictionary.Add(@this, value);
                }
                Monitor.Exit(_obj);
                return _ExceptionDictionary[@this];
            }
        }

        public static object _obj = new object();
        private static ConcurrentDictionary<Enum, string> _ConcurrentDictionary = new ConcurrentDictionary<Enum, string>();
        public static string GetDescriptionByConcurrentDictionary(this Enum @this)
        {
            return _ConcurrentDictionary.GetOrAdd(@this, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                return field == null ? key.ToString() : GetDescription(field);
            });
        }
    }
}
