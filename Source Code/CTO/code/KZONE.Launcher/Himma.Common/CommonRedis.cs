using FreeRedis;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedisClient = FreeRedis.RedisClient;
//using System.Windows.Documents;

namespace Himma.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonRedis
    {
        /// <summary>
        /// Redis实例
        /// </summary>
        public static RedisClient redisConnection { get; set; }
        /// <summary>
        /// 初始化Redis
        /// </summary>
        /// <param name="connStr">redis地址</param>
        /// <returns></returns>
        public static async Task InitialRedisAsync()
        {
            try
            {
                //初始化redis
                //CommonRedis.InitialRedisAsync();
                //protocol=RESP3;
                //其他模式
                var laststr = $"{GlobalConfig.RedisIP},defaultDatabase=0";
                redisConnection = new RedisClient(laststr);
                var info = redisConnection.Info();
                Console.WriteLine($"RedisInfo: {info}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化Redis失败: {ex} {ex.StackTrace}");
            }
        }
        /// <summary>
        /// 获取list长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> ListLengthAsync(string key)
        {
            return await redisConnection.LLenAsync(key);
        }

        /// <summary>
        /// 获取list长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long ListLength(string key)
        {
            return redisConnection.LLen(key);
        }


        /// <summary>
        /// 添加至Redis队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long ListAddToLeftString(string key, string datastring)
        {
            return redisConnection.LPush(key, datastring);
        }

        /// <summary>
        /// 添加至Redis队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> ListAddToLeftStringAsync(string key, string datastring)
        {
            return await redisConnection.LPushAsync(key, datastring);
        }
        /// <summary>
        /// 从reids数据库队列获取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ListGetFromRightString(string key)
        {
            return redisConnection.RPop(key);
        }
        /// <summary>
        /// 从reids数据库队列获取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> ListGetFromRightStringAsync(string key)
        {
            return await redisConnection.RPopAsync(key);
        }




        /// <summary>
        /// 获取Redis中下一个ID
        /// </summary>
        /// <param name="sequenceKey">键值对名称</param>
        /// <returns></returns>
        public static int GetNextVirtualID(string sequenceKey = "BizExchangeBatteryModelKey")
        {

            const long maxValue = 20000;

            // 在 Redis 事务中执行自增和翻转操作
            var _sequence = redisConnection.Eval(
                "local seq = tonumber(redis.call('get', KEYS[1])) " +
                "if not seq then " +
                "   seq = 0 " +
                "end " +
                "if seq < tonumber(ARGV[1]) then " +
                "   seq = redis.call('incr', KEYS[1]) " +
                "   return seq " +
                "else " +
                "   redis.call('decrby', KEYS[1], ARGV[1]) " +
                "   return 0 " +
                "end",
                new[] { sequenceKey },
                new[] { maxValue.ToString() });

            // 返回自增后的值 redis返回的是long
            var sequence = Convert.ToInt32(_sequence) + 10000;
            if (sequenceKey == "BizExchangeBatteryModelKey")//特殊逻辑 组盘虚拟码需要排除仍在使用的虚拟码
            {
                var rediskey = "PuckExchange:VirtualNo:" + sequence.ToString();
                if (redisConnection.Exists(rediskey))
                {
                    return GetNextVirtualID(sequenceKey); //递归调用 生成下一个虚拟码
                }
            }
            return sequence;
        }
        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return redisConnection.Get(key);
        }
        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string key)
        {
            return await redisConnection.GetAsync(key);
        }
        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public static async Task SetStringAsync(string key, string value, int timeoutSeconds = 0)
        {
            await redisConnection.SetAsync(key, value, timeoutSeconds: timeoutSeconds);
        }
        /// <summary>
        /// 获取Redis中下一个ID
        /// </summary>
        /// <param name="sequenceKey">键值对名称</param>
        /// <returns></returns>
        public static async Task<int> GetNextVirtualIDAsync(string sequenceKey = "BizExchangeBatteryModelKey")
        {


            const long maxValue = 20000;

            // 在 Redis 事务中执行自增和翻转操作
            var _sequence = await redisConnection.EvalAsync(
                "local seq = tonumber(redis.call('get', KEYS[1])) " +
                "if not seq then " +
                "   seq = 0 " +
                "end " +
                "if seq < tonumber(ARGV[1]) then " +
                "   seq = redis.call('incr', KEYS[1]) " +
                "   return seq " +
                "else " +
                "   redis.call('decrby', KEYS[1], ARGV[1]) " +
                "   return 0 " +
                "end",
                new[] { sequenceKey },
                new[] { maxValue.ToString() });

            // 返回自增后的值 redis返回的是long
            var sequence = Convert.ToInt32(_sequence) + 10000;
            if (sequenceKey == "BizExchangeBatteryModelKey")//特殊逻辑 组盘虚拟码需要排除仍在使用的虚拟码
            {
                var rediskey = "PuckExchange:VirtualNo:" + sequence.ToString();
                if (await redisConnection.ExistsAsync(rediskey))
                {
                    return await GetNextVirtualIDAsync(sequenceKey); //递归调用 生成下一个虚拟码
                }
            }
            return sequence;
        }
    }
}

