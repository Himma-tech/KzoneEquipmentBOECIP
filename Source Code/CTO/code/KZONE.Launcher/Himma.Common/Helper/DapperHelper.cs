using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System.Transactions;
using Dapper;
using MySqlConnector;
using Himma.Common.Log;

namespace Himma.Common
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    public class DapperHelper
    {
        //private static string _connectionString = ConfigurationManager.AppSettings["ConnectionStr"];
        // private static string _connectionString = string.Empty;
        private static string _logName = "DapperHelper";

      



        //public static Lazy<MysqlOperatorPool> ConnectionPool = new Lazy<MysqlOperatorPool>(() => {
        //    //var mysqlOperatorPoolConnectionInfo = new MysqlOperatorPoolConnectionInfo(ConfigConnStr());
        //    //var mysqlOperatorPoolConnectionInfo = new MysqlOperatorPoolConnectionInfo(@"server=127.0.0.1;database=c100_mib;uid=root;pwd=root;port = 3306;Persist Security Info=True;Charset=utf8;Allow User Variables=True;");
        //    //mysqlOperatorPoolConnectionInfo.MaxPoolSize = GlobalConfig.MaxPoolSize;
        //    //mysqlOperatorPoolConnectionInfo.MinPoolSize = GlobalConfig.MinPoolSize;   //原来Max:100;Min:5;现均改为30；
        //    ////mysqlOperatorPoolConnectionInfo.ConnectTimeout=TimeSpan.FromSeconds(5);
        //    //return new MysqlOperatorPool(mysqlOperatorPoolConnectionInfo);

        //});
        public static string GetPoolInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("===========MysqlPool=============");
            sb.AppendLine("CommunicationPool");
            //sb.AppendLine($"KEY:{ConnectionPool.Value.Key}");
            //sb.AppendLine(ConnectionPool.Value.StatisticsFullily);
            sb.AppendLine("========================");
            return sb.ToString();
        }
        //public static IDbConnection GetContext()
        //{
        //    if (string.IsNullOrEmpty(_connectionString)) throw new Exception("connection string is not allowed null~");
        //    return new MySqlConnection(_connectionString);
        //}

        /// <summary>
        /// 查询返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param
        /// <returns></returns>
        //public static List<T> QueryList<T>(string sql, object parms)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return connection.Value.Query<T>(sql, parms).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        /// <summary>
        /// 异步查询返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        //public static async Task<List<T>> QueryListAsync<T>(string sql, object parms)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return (await connection.Value.QueryAsync<T>(sql, parms)).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        /// <summary>
        /// 查询数据库值，返回dataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        //public static DataTable QueryDatatable(string sql)
        //{
        //    DataTable dataTable = new DataTable();
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            MySqlDataAdapter da = new MySqlDataAdapter(sql, connection.Value);
        //            da.Fill(ds, "table");
        //            return ds.Tables["table"];
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">传参</param>
        /// <returns>查询字段</returns>
        //public static T QueryOne<T>(string sql, object parms)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return connection.Value.QueryFirstOrDefault<T>(sql, parms);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //}

        /// <summary>
        /// 异步查询单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        //public static async Task<T> QueryOneAsync<T>(string sql, object parms)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return await connection.Value.QueryFirstOrDefaultAsync<T>(sql, parms);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //}

        /// <summary>
        /// 查询单个字段
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns>查询字段</returns>
        //public static T QueryOne<T>(string sql)
        //{
        //    return QueryOne<T>(sql, null);
        //}
        //public static Task<T> QueryOneAsync<T>(string sql)
        //{
        //    return QueryOneAsync<T>(sql, null);
        //}

        //public static List<T> QueryList<T>(string sql)
        //{
        //    return QueryList<T>(sql, null);
        //}
        //public static Task<List<T>> QueryListAsync<T>(string sql)
        //{
        //    return QueryListAsync<T>(sql, null);
        //}

        //public static SqlMapper.GridReader QueryMultiple(string sql, object parms = null)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return connection.Value.QueryMultiple(sql, parms);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //}
        //public static List<List<dynamic>> QueryMultipleCount(string sql, object parms = null, int count = 2)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            List<List<dynamic>> dynamics = new List<List<dynamic>>();
        //            var result = connection.Value.QueryMultiple(sql, parms);
        //            for (int i = 0; i < count; i++)
        //            {
        //                dynamics.Add(result.Read().ToList());
        //            }
        //            return dynamics;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //}
        //public static List<IEnumerable<dynamic>> QueryMultipleDynamic(string sql, object parms = null)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            List<IEnumerable<dynamic>> dynamics = new List<IEnumerable<dynamic>>();
        //            var result = connection.Value.QueryMultiple(sql, parms);
        //            while (!result.IsConsumed)
        //            {
        //                var rs = result.Read();
        //                dynamics.Add(rs);
        //            }
        //            return dynamics;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// 查询返回单个值
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="parms"></param>
        ///// <returns></returns>
        //public static object QueryScalar(string sql, object parms)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return connection.Value.ExecuteScalar(sql, parms);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        //public static async Task<object> QueryScalarAsync(string sql, object parms)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return await connection.Value.ExecuteScalarAsync(sql, parms);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, parms);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}


        //public static object QueryScalar(string sql)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return connection.Value.ExecuteScalar(sql, null);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        //public static async Task<object> QueryScalarAsync(string sql)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return await connection.Value.ExecuteScalarAsync(sql, null);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        ///// <summary>
        ///// 执行更新语句
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="obj"></param>
        //public static void Execute(string sql, object obj)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        IDbTransaction transaction = connection.Value.BeginTransaction();
        //        try
        //        {
        //            connection.Value.Execute(sql, obj, transaction: transaction);
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw ex;
        //        }
        //        finally
        //        {

        //        }
        //    }
        //}


        //public static async Task ExecuteAsync(string sql, object obj = null)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        var transaction = await connection.Value.BeginTransactionAsync();
        //        try
        //        {
        //            await connection.Value.ExecuteAsync(sql, obj, transaction: transaction);
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, sql, obj);
        //            transaction.Rollback();
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //}


        ///// <summary>
        ///// 执行存储过程，返回集合
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="funcName"></param>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static List<T> ExecuteFuncToList<T>(string funcName, object obj)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            return connection.Value.Query<T>(funcName, obj, commandType: CommandType.StoredProcedure).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, funcName, obj);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //}

        ///// <summary>
        ///// 执行存储过程
        ///// </summary>
        ///// <param name="funcName"></param>
        ///// <param name="obj"></param>
        //public static void ExecuteFunc(string funcName, object obj)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        try
        //        {
        //            connection.Value.Execute(funcName, obj, null, null, CommandType.StoredProcedure);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, funcName, obj);
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        //public static async Task ExecuteFuncAsync(string funcName, object obj)
        //{
        //    using (var connection = ConnectionPool.Value.Get())
        //    {
        //        IDbTransaction trans = null;
        //        try
        //        {
        //            trans = await connection.Value.BeginTransactionAsync();
        //            await connection.Value.ExecuteAsync(funcName, obj, trans, null, CommandType.StoredProcedure);
        //            trans.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            LogError(ex, funcName, obj);
        //            if (trans != null)
        //            {
        //                trans.Rollback();
        //            }
        //            throw ex;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //}

        //private static void LogError(Exception ex, string sql, object param = null)
        //{
        //    LogHelper.Error($@"SQL脚本执行发生异常，异常原因：{ex}, SQL: {sql} 参数：{JsonConvert.SerializeObject(param)}", _logName);
        //}
    }
}
