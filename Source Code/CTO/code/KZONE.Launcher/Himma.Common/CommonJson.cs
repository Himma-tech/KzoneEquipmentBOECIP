using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common
{
    public class CommonJson
    {
        #region 统一序列化与反序列化
        /// <summary>
        /// 将一个Model对象转为JSON字符
        /// </summary>
        /// <param name="Model">数据源</param>
        /// <returns></returns>
        public static string ConverModelToJson(object Model)
        {
            string FormatTime = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //settings.NullValueHandling=  NullValueHandling.Ignore;
            settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = FormatTime });
            return JsonConvert.SerializeObject(Model, settings);
        }

        /// <summary>
        /// 将一个Model对象转为JSON字符
        /// </summary>
        /// <param name="Model">数据源</param>
        /// <param name="FormatTime">自定义时间格式</param>
        /// <returns></returns>
        public static string ConverModelToJson(object Model, string FormatTime)
        {
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = FormatTime };
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = FormatTime });
            return JsonConvert.SerializeObject(Model, settings);
        }
        public static T ConverJsonToModel<T>(string str) where T : class
        {
            var obj = default(T);
            try
            {
                obj = JsonConvert.DeserializeObject<T>(str);
                return obj;
            }
            catch (System.Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// JSON反序列化方法 JSON转Object
        /// </summary>
        public class AnotherJsonDeserializer
        {
            // 实现此接口的JSON反序列化方法
            public bool Deserialize<T>(string str, out T obj) where T : new()
            {
                obj = default(T);
                bool ok = true;
                try
                {
                    obj = JsonConvert.DeserializeObject<T>(str);
                }
                catch (System.Exception ex)
                {
                    ok = false;
                }
                return ok;
            }
        }
        #endregion
    }
}
