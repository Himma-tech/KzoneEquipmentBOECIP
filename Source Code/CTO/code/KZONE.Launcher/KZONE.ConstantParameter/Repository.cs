using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace KZONE.ConstantParameter
{
    public class Repository
    {
        private static ConcurrentDictionary<string, object> _dictionary = new ConcurrentDictionary<string, object>();

        public static void Add(string id, object obj)
        {
            Repository._dictionary.AddOrUpdate(id, obj, (string key, object existingVal) => obj);
        }

        public static object Get(string id)
        {
            object ret = null;
            if (!Repository._dictionary.TryGetValue(id, out ret))
            {
                ret = null;
            }
            return ret;
        }

        public static object Remove(string id)
        {
            object ret = null;
            if (Repository._dictionary.ContainsKey(id))
            {
                Repository._dictionary.TryRemove(id, out ret);
            }
            return ret;
        }

        public static bool ContainsKey(string id)
        {
            return Repository._dictionary.ContainsKey(id);
        }
    }
}
