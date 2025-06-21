using Himma.Common.Log;
using Himma.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Himma.Common.BaseData.Func
{
    //public class SysParamDataAccess
    //{
    //    /// <summary>
    //    /// 根据参数编码获取参数值
    //    /// </summary>
    //    /// <param name="paramNo"></param>
    //    /// <returns></returns>
    //    public string ParamInfo(string paramNo)
    //    {
    //        var cfg = CacheManager.Current[Const.CACHED_SYSPARAM] as List<SysParamInfo>;
    //        var find = cfg.FirstOrDefault(x => x.Id == paramNo);
    //        return find == null ? string.Empty : find.ParamValue;
    //    }

    //    /// <summary>
    //    /// 获取系统参数
    //    /// </summary>
    //    /// <param name="groupId"></param>
    //    /// <param name="flag"></param>
    //    /// <returns></returns>
    //    public List<SysParamInfo> QueryAll(int groupId = -1, int flag = 0)
    //    {
    //        try
    //        {
    //            var sql = @"SELECT PARAM_NO AS Id, PARAM_NAME ParamName, PARAM_VALUE_TYPE ParamType, PARAM_VALUE ParamValue
    //                    FROM sys_param;";
    //            return DapperHelper.QueryList<SysParamInfo>(sql, new { groupId, flag });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取系统参数信息异常！异常提示【{ex}】", "System");
    //            return new List<SysParamInfo>();
    //        }
    //    }

    //    /// <summary>
    //    /// 获取系统参数
    //    /// </summary>
    //    /// <param name="groupId"></param>
    //    /// <param name="flag"></param>
    //    /// <returns></returns>
    //    public async Task<List<SysParamInfo>> QueryAllAsync(int groupId = -1, int flag = 0)
    //    {
    //        try
    //        {
    //            var sql = @"SELECT PARAM_NO AS Id, PARAM_NAME ParamName, PARAM_VALUE_TYPE ParamType, PARAM_VALUE ParamValue
    //                    FROM sys_param;";
    //            return await DapperHelper.QueryListAsync<SysParamInfo>(sql, new { groupId, flag });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取系统参数信息异常！异常提示【{ex}】", "System");
    //            return new List<SysParamInfo>();
    //        }
    //    }

    //    /// <summary>
    //    /// 更新系统参数
    //    /// </summary>
    //    /// <param name="sysParamInfos"></param>
    //    public bool UpdateSysParams(List<SysParamInfo> sysParamInfos)
    //    {
    //        try
    //        {
    //            var sql = @"update sys_param set PARAM_VALUE=@ParamValue where PARAM_NO = @Id ;";
    //            DapperHelper.Execute(sql, sysParamInfos);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新系统参数信息【{sysParamInfos[0].Id}】异常！异常提示【{ex}】", "System");
    //            return false;
    //        }
    //    }

    //    public bool UpdateSingleSysParams(SysParamInfo sysParamInfo)
    //    {
    //        try
    //        {
    //            var sql = @"update sys_param set PARAM_VALUE=@ParamValue where PARAM_NO = @Id ;";
    //            DapperHelper.Execute(sql, new { Id = sysParamInfo.Id, ParamValue = sysParamInfo.ParamValue });
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新系统参数信息【{sysParamInfo.Id}】异常！异常提示【{ex}】", "System");
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// 更新系统参数
    //    /// </summary>
    //    /// <param name="sysParamInfos"></param>
    //    /// <returns></returns>
    //    public SysParamInfo QuerySysPamValue(string parm_no)
    //    {
    //        try
    //        {
    //            var sql = @"SELECT PARAM_NO AS Id, PARAM_NAME ParamName,PARAM_VALUE ParamValue
    //                    FROM sys_param where PARAM_NO = @Id ;";
    //            return DapperHelper.QueryList<SysParamInfo>(sql, new { Id = parm_no }).FirstOrDefault();

    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新系统参数信息【{parm_no}】异常！异常提示【{ex}】", "System");
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取参数类别
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<ParamGroup> GetParamGroup()
    //    {
    //        var sql = @"select t.PARAM_GROUP ParamGroupID,t.PARAM_NAME ParamGroupName from  bas_param_group t;";
    //        return DapperHelper.QueryList<ParamGroup>(sql);
    //    }
    //    public void SaveValue(SysParamInfo info)
    //    {

    //        var sql = @"update sys_param set PARAM_VALUE=@ParamValue where PARAM_NO=@Id;COMMIT;";
    //        DapperHelper.Execute(sql, info);
    //    }

    //    public static string GetUserDefineVariable(string strVariableName)
    //    {
    //        var sql = $"select value from user_define_variable where variableName = '{strVariableName}';";
    //        return DapperHelper.QueryScalar(sql).ToString();
    //    }
    //}
}
