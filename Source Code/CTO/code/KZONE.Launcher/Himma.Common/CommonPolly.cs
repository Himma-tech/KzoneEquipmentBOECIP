using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Polly.Retry;
using Polly.Timeout;

namespace Himma.Common
{
    /// <summary>
    /// 重试
    /// </summary>
    public class CommonPolly
    {
        /// <summary>
        /// 创建一个通用的重试对象
        /// </summary>
        /// <param name="MaxRetryAttempts">重试次数</param>
        /// <param name="Timeout">总体超时实际</param>
        /// <param name="OnRetry">重试时间</param>
        /// <returns></returns>
        public static ResiliencePipeline Creat(int MaxRetryAttempts, TimeSpan Timeout, Func<OnRetryArguments<object>, bool> OnRetry)
        {
            var optionsComplex = new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),//捕获所有类型错误
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = false,  // 是否添加随机延迟
                MaxRetryAttempts = MaxRetryAttempts,//重试次数
                Delay = TimeSpan.FromMilliseconds(50),//间隔
                //OnRetry = args =>
                //{
                //    Console.WriteLine("OnRetry, Attempt: {0}", args.AttemptNumber);//重试

                //    // Event handlers can be asynchronous; here, we return an empty ValueTask.
                //    return default;
                //}
                OnRetry = args => {
                    OnRetry(args);
                    return default;
                }
            };
            // Create an instance of builder that exposes various extensions for adding resilience strategies
            ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
                .AddTimeout(Timeout)
                .AddRetry(optionsComplex) // 添加重试次数
                .Build(); // Builds the resilience pipeline
                          // Execute the pipeline asynchronously
                          //await pipeline.ExecuteAsync(static async token => { /* Your custom logic goes here */ });
            return pipeline;
        }
    }
}
