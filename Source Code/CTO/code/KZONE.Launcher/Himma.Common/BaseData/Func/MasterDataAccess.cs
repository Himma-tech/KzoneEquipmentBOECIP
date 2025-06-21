using Himma.Common.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using static NPOI.HSSF.Util.HSSFColor;
using NPOI.SS.Formula.Functions;
using System.IO;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Himma.Common.Models;
using Himma.Common.Models.Base;
using Himma.Common.Log;

namespace Himma.Common.BaseData.Func
{
    //public class MasterDataAccess
    //{
    //    #region Query related functions

    //    /// <summary>
    //    /// 获取用户信息
    //    /// </summary>
    //    /// <param name="userId"></param>
    //    /// <param name="userPsd"></param>
    //    /// <returns></returns>
    //    public static SysUserAccount GetSysUserAccount(string userId, string userPsd)
    //    {
    //        try
    //        {
    //            var sql = $"select user_id as UserId,user_name as UserName,user_psd as UserPsd,level as UserLevel " +
    //                      $"from Sys_User_Account where User_id='{userId}' and User_psd='{userPsd}';";
    //            //SysUserAccount sysUserAccount = DapperHelper.QueryOne<SysUserAccount>(sql);
    //            return sysUserAccount;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取用户信息异常！异常提示【{ex}】", "sql");
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取所有用户信息
    //    /// </summary>
    //    /// <returns></returns>
    //    public static List<string> GetUserIds()
    //    {
    //        try
    //        {
    //            var sql = $"select user_id from Sys_User_Account;";
    //            return DapperHelper.QueryList<string>(sql);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取所有用户信息异常！异常提示【{ex}】", "sql");
    //            return null;
    //        }
    //    }

      
    //    /// <summary>
    //    /// 系统参数查询
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<SysParamInfo> GetSysParams()
    //    {
    //        var sql = @"SELECT PARAM_NO Id, PARAM_NAME ParamName, PARAM_VALUE_TYPE ParamType, PARAM_VALUE ParamValue
    //                    FROM sys_param";
    //        return DapperHelper.QueryList<SysParamInfo>(sql);
    //    }

    //    /// <summary>
    //    /// 查询PLC配置信息
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="pLCType"></param>
    //    /// <returns></returns>
    //    public List<T> GetPlcModels<T>(PLCType pLCType) where T : BasePLC
    //    {
    //        try
    //        {
    //            var sql = @"SELECT PLC_NO PLCNo, PLC_IP IPAddress, PLC_NAME PLCName,
    //                      LOCALAMSNETID localAmsNetId,TARGETAMSNETID targetAmsNetId,
    //                    PLC_PORT IpPort, PLC_TYPE PlcType,USED_FLAG UsedFlag,USED_FLAG as IsEnabled,USED_POOL UsedPool,
				//		CONNC_NUM ConnectionNum,RETRAY_NUM RetryNum,RETRAY_INTTIME RetryInteralTime
    //                    FROM `bas_plc` WHERE used_flag != 3 and PLC_TYPE = @type order by used_flag desc;";

    //            return DapperHelper.QueryList<T>(sql, new { type = pLCType.ToString() });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】查询PLC配置信息异常！异常提示【{ex}】", "sql");
    //            return new List<T>();
    //        }
    //    }

    //    public List<T> GetArmPlcModels<T>(PLCType pLCType) where T : BasePLC
    //    {
    //        try
    //        {
    //            var sql = @"SELECT PLC_NO PLCNo, PLC_IP IPAddress, PLC_NAME PLCName,
    //                    PLC_PORT IpPort, PLC_TYPE PlcType,USED_FLAG UsedFlag,USED_FLAG as IsEnabled
    //                    FROM `bas_plc` WHERE used_flag !=3 and PLC_TYPE = @type and PLC_NO = 'T04' order by used_flag desc;";

    //            return DapperHelper.QueryList<T>(sql, new { type = pLCType.ToString() });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】查询PLC配置信息异常！异常提示【{ex}】", "sql");
    //            return new List<T>();
    //        }
    //    }

    //    public BasePLC GetTransArmPlcModels()
    //    {
    //        try
    //        {
    //            var sql = @"SELECT PLC_NO PLCNo, PLC_IP IPAddress, PLC_NAME PLCName,
    //                    PLC_PORT IpPort, PLC_TYPE PlcType,USED_FLAG UsedFlag,USED_FLAG as IsEnabled
    //                    FROM `bas_plc` WHERE used_flag !=3 and PLC_NO = @type order by used_flag desc;";

    //            List<BasePLC> basePLCs = DapperHelper.QueryList<BasePLC>(sql, new { type = "T04" });
    //            if (basePLCs != null && basePLCs.Count > 0)
    //            {
    //                return basePLCs[0];
    //            }
    //            return null;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】查询PLC配置信息异常！异常提示【{ex}】", "sql");
    //            return null;
    //        }
    //    }

       

    //    public List<dynamic> GetDrivesInfo()
    //    {
    //        try
    //        {
    //            var sql = "SELECT T.PARAM_NO,T.PARAM_NAME,T.PARAM_VALUE FROM SYS_PARAM T WHERE PARAM_NO >= 3001 AND PARAM_NO <=3008";
    //            return DapperHelper.QueryList<dynamic>(sql, null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取托杯清洗数据异常！异常提示【{ex}】", "sql");
    //            return null;
    //        }
    //    }

      
    //    /// <summary>
    //    /// 生成虚拟号码
    //    /// </summary>
    //    /// <param name="businessKey"></param>
    //    /// <returns></returns>
    //    public int GetSequence(string businessKey)
    //    {
    //        var sql = "select FUNC_NEXTVAL(@seqType); commit;";
    //        return Convert.ToInt32(DapperHelper.QueryScalar(sql, new { seqType = businessKey }));
    //    }

    //    /// <summary>
    //    /// 获取已入库托盘数量
    //    /// </summary>
    //    /// <returns></returns>
    //    public int GetTrayCount()
    //    {
    //        try
    //        {
    //            var sql = "SELECT COUNT(1) FROM BIZ_TRAY";
    //            return Convert.ToInt32(DapperHelper.QueryScalar(sql));
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取已入库托盘数量异常！异常提示【{ex}】", "sql");
    //            return 0;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取工位夹具号
    //    /// </summary>
    //    /// <param name="locNo"></param>
    //    /// <returns></returns>
    //    public string GetTrayNo(string locNo)
    //    {
    //        try
    //        {
    //            var sql = $"select tray_no from wst_loc where loc_no = '{locNo}';";
    //            return DapperHelper.QueryOne<string>(sql);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取工位【{locNo}】上的夹具号异常！异常提示【{ex}】", "sql");
    //            return "";
    //        }
    //    }


    //    /// <summary>
    //    /// 获取调度任务
    //    /// </summary>
    //    /// <returns></returns>
    //    public int SelectTransTask()
    //    {
    //        try
    //        {
    //            string sql = $"select count(*) from biz_task_trans";
    //            return DapperHelper.QueryOne<int>(sql, null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取调度任务数据异常！异常提示【{ex}】", "sql");
    //            return 0;
    //        }
    //    }

       

    //    /// <summary>
    //    /// 获取指定炉腔待出水含量的检测任务
    //    /// </summary>
    //    /// <param name="binNo"></param>
    //    /// <returns></returns>
    //    public int SearchWateToOutWaterTask(string binNo)
    //    {
    //        try
    //        {
    //            string sql = $"select count(*) from biz_task_check where task_step = 0 and bin_no = '{binNo}';";
    //            return Convert.ToInt32(DapperHelper.QueryScalar(sql));
    //        }
    //        catch (Exception)
    //        {
    //            return 0;
    //        }
    //    }

    //    public int TestRGV()
    //    {
    //        try
    //        {
    //            string sql = $"select PARAM_VALUE from sys_param where PARAM_NO = 1003;";
    //            return Convert.ToInt32(DapperHelper.QueryScalar(sql));
    //        }
    //        catch (Exception)
    //        {
    //            return 0;
    //        }
    //    }

    //    /// <summary>
    //    /// 更新检测任务的优先级
    //    /// </summary>
    //    /// <param name="binNo"></param>
    //    public bool UpdateCheckTaskPLevel(string binNo)
    //    {
    //        try
    //        {
    //            string sql = $"update biz_task_check set PRIORITY_LEVEL = '20' where task_step = 0 and bin_no = '{binNo}';";
    //            DapperHelper.Execute(sql, null);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新检测任务优先级异常！异常原因【{ex}】", "Check");
    //            return false;
    //        }
    //    }

    //    #endregion Query related functions

    //    #region insert or update operations

    //    public void SaveBatteryCache(object objBattery, bool isNG)
    //    {
    //        var sqlNormal = @"INSERT INTO `biz_battery_cache`(`BATTERY_NO`, `BATTERY_ID`, `BATTERY_TYPE`, `QUALITY_STATUS`, `SCAN_DATE`)
    //                    VALUES (@BatteryNo, @BatteryId, @BatteryType, @Status, @ScanDate);commit;";
    //        var sqlAbnormal = @"INSERT INTO `biz_battery_abnormal`(`AREA_NO`,`LOC_NO`,`BATTERY_NO`, `VIRTUAL_ID`, `BATTERY_TYPE`, `QUALITY_STATUS`, `SCAN_DATE`)
    //                    VALUES (@AreaNo,@LocNo,@BatteryNo, @BatteryId, @BatteryType, @Status, @ScanDate);commit;";
    //        DapperHelper.Execute(isNG ? sqlAbnormal : sqlNormal, objBattery);
    //    }



    //    /// <summary>
    //    /// 更新炉子检测水电芯标志
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public bool UpdateWaterCheckFlag(string locNo, int flag)
    //    {
    //        try
    //        {
    //            var sql = @"update wcs_loc set WATER_CHECK_FLAG=@co where Loc_no = @ln ;commit;";
    //            DapperHelper.Execute(sql, new { ln = locNo, co = flag });
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新WCS_LOC的WATER_CHECK_FLAG异常！异常提示【{ex}】", "System");
    //            return false;
    //        }
    //    }
    //    /// <summary>
    //    /// 更新设备状态
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public void UpdateDevStatus(bool station, string locNo)
    //    {
    //        try
    //        {
    //            int FreeFlag = station ? 1 : 2;
    //            var sql = @"update wcs_loc set PLC_STATUS=@ff where Loc_no = @ln ;commit;";
    //            DapperHelper.Execute(sql, new { ff = FreeFlag, ln = locNo });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新设备状态异常！异常提示【{ex}】", "System");
    //        }
    //    }
    //    /// <summary>
    //    /// 更新机械手有载无载状态状态
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public void UpdateRgvLoadedStatus(int RgvId, int flag)
    //    {
    //        try
    //        {
    //            var sql = @"update wcs_rgv_loc set FREE_FLAG=@flag where RGV_ID = @RgvId ;commit;";
    //            DapperHelper.Execute(sql, new { flag = flag, RgvId = RgvId });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新设备状态异常！异常提示【{ex}】", "System");
    //        }
    //    }

    //    /// <summary>
    //    /// 更新机械手有载无载状态状态yibu
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public async Task UpdateRgvLoadedStatusAsync(int RgvId, int flag)
    //    {
    //        try
    //        {
    //            var sql = @"update wcs_rgv_loc set FREE_FLAG=@flag where RGV_ID = @RgvId ;commit;";
    //            await DapperHelper.ExecuteAsync(sql, new { flag = flag, RgvId = RgvId });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新设备状态异常！异常提示【{ex}】", "System");
    //        }
    //    }

    //    /// <summary>
    //    /// 更新机械手实际位置
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public void UpdateRgvPosition(int RgvId, int value)
    //    {
    //        try
    //        {
    //            var sql = @"update wcs_rgv set CUR_COORDINATES_X=@value where RGV_ID = @RgvId ;commit;";
    //            DapperHelper.Execute(sql, new { value = value, RgvId = RgvId });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新设备状态异常！异常提示【{ex}】", "System");
    //        }
    //    }

    //    /// <summary>
    //    /// 更新机械手实际位置yibu
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public async Task UpdateRgvPositionAsync(int RgvId, int value)
    //    {
    //        try
    //        {
    //            var sql = @"update wcs_rgv set CUR_COORDINATES_X=@value where RGV_ID = @RgvId ;commit;";
    //            await DapperHelper.ExecuteAsync(sql, new { value = value, RgvId = RgvId });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新设备状态异常！异常提示【{ex}】", "System");
    //        }
    //    }

    //    /// <summary>
    //    /// 更新机械手当前状态
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public void UpdateRgvCurStatus(int RgvId, int value)
    //    {
    //        try
    //        {
    //            var sql = @"update wcs_rgv set RUN_STATUS=@value where RGV_ID = @RgvId ;commit;";
    //            DapperHelper.Execute(sql, new { value = value, RgvId = RgvId });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新设备手自动状态异常！异常提示【{ex}】", "System");
    //        }
    //    }

    //    /// <summary>
    //    /// 更新机械手当前状态
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public async Task UpdateRgvCurStatusAsync(int RgvId, int value)
    //    {
    //        try
    //        {
    //            var sql = @"update wcs_rgv set RUN_STATUS=@value where RGV_ID = @RgvId ;commit;";
    //            await DapperHelper.ExecuteAsync(sql, new { value = value, RgvId = RgvId });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新设备手自动状态异常！异常提示【{ex}】", "System");
    //        }
    //    }

    //    /// <summary>
    //    /// 更新标签打印纸消耗数量
    //    /// </summary>
    //    /// <param name="station"></param>
    //    public void UpdatePrtUesdCout(int value, string key)
    //    {
    //        try
    //        {
    //            var sql = @"UPDATE sys_param SET T.PARAM_VALUE = @value WHERE T.PARAM_NO = @key ;commit;";
    //            DapperHelper.Execute(sql, new { value, key });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新标签打印纸消耗数量异常！异常提示【{ex}】", "System");
    //        }
    //    }

    //    /// <summary>
    //    /// 获取当前炉腔的“参与生产”状态
    //    /// </summary>
    //    /// <param name="binNo"></param>
    //    public int GetProductionFlag(string binNo)
    //    {
    //        try
    //        {
    //            var sql = $"select production_flag from bas_bin where bin_no = '{binNo}';";
    //            return (int)DapperHelper.QueryScalar(sql);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取炉腔参与生产状态异常！异常提示【{ex}】", "System");
    //            return 0;
    //        }
    //    }

    //    /// <summary>
    //    /// 更新炉腔的“参与生产”标记
    //    /// </summary>
    //    /// <param name="binNo">库位编码</param>
    //    /// <param name="productionFlag">1.参与生产 2.脱离生产</param>
    //    public bool UpdateProductionFlag(string binNo, int productionFlag)
    //    {
    //        try
    //        {
    //            var sql = $"update bas_bin set production_flag = '{productionFlag}' where bin_no = '{binNo}';commit;";
    //            DapperHelper.Execute(sql, null);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"更新炉腔参与生产状态异常！异常提示【{ex}】", "System");
    //            return false;
    //        }
    //    }

    //    #endregion insert or update operations
    //    protected object _objectLock = new object();

    //    #region call func or sp

        

    //    public void CallJobSP()
    //    {
    //        try
    //        {
    //            if (GlobalConfig.IsEnabledPLC)
    //                DapperHelper.ExecuteFunc("PROC_JOB", null);
    //            else
    //                DapperHelper.ExecuteFunc("PROC_JOB_SIM", null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"执行PROC_JOB失败！错误提示【{ex}】", "proc");
    //        }
    //    }

    //    public async Task CallJobSPAsync()
    //    {
    //        try
    //        {
    //            if (GlobalConfig.IsEnabledPLC)
    //                await DapperHelper.ExecuteFuncAsync("PROC_JOB", null);
    //            else
    //                await DapperHelper.ExecuteFuncAsync("PROC_JOB_SIM", null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"执行PROC_JOB失败！错误提示【{ex}】", "proc");
    //        }
    //    }

    //    /// <summary>
    //    /// 下料位有多余空盘执行
    //    /// </summary>
    //    public void CallUnPackPushing()
    //    {
    //        try
    //        {
    //            DapperHelper.ExecuteFunc("PROC_UNPACK_PUSHING", null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"执行PROC_UNPACK_PUSHING失败！错误提示【{ex}】", "proc");
    //        }
    //    }


    //    #endregion call func or sp

    //    #region GetPlc


       
    //    #endregion GetPlc

    //    #region GetStation

       

    //    #endregion GetStation

    //    #region GetTask

      
    //    /// <summary>
    //    /// 删除上料任务
    //    /// </summary>
    //    /// <param name="locNo"></param>
    //    /// <param name="isUnpack"></param>
    //    /// <returns></returns>
    //    public void DeletePackTask(string locNo)
    //    {
    //        try
    //        {
    //            var sql = @"Delete from biz_task_pack t WHERE t.LOC_NO = @locNo";
    //            DapperHelper.Execute(sql, new { locNo });
    //        }
    //        catch (Exception e)
    //        {
    //            LogHelper.Error($"删除biz_task_pack异常，原因：{e}");
    //        }
    //    }

    //    /// <summary>
    //    /// 删除上料任务yibu
    //    /// </summary>
    //    /// <param name="locNo"></param>
    //    /// <param name="isUnpack"></param>
    //    /// <returns></returns>
    //    public async Task DeletePackTaskAsync(string locNo)
    //    {
    //        try
    //        {
    //            var sql = @"Delete from biz_task_pack t WHERE t.LOC_NO = @locNo";
    //            await DapperHelper.ExecuteAsync(sql, new { locNo });
    //        }
    //        catch (Exception e)
    //        {
    //            LogHelper.Error($"删除biz_task_pack异常，原因：{e}");
    //        }
    //    }

    //    public int GetTaskType(string trayno)
    //    {
    //        try
    //        {
    //            var sql = @"select count(1) FROM biz_tray_temp where TEMPERATURE < 95 AND TRAY_NO = @trayno;";
    //            var sql2 = @"select count(1) FROM biz_tray_temp where TRAY_NO = @trayno;";
    //            return DapperHelper.QueryOne<int>(sql, new { trayno }) == DapperHelper.QueryOne<int>(sql, new { trayno }) ? 7 : 5;
    //        }
    //        catch (Exception)
    //        {
    //            return 5;
    //        }
    //    }

    //    public dynamic GetPackTaskNew(string locNo)
    //    {
    //        return DapperHelper.QueryOne<dynamic>(
    //          @"select p.* ,b.LOC_NAME
    //            from biz_task_pack p
    //            join bas_loc b on b.LOC_NO = p.LOC_NO
    //            where p.loc_no = @locNo
    //            ;", new { locNo }
    //        );
    //    }

    //    /// <summary>
    //    /// 判断工位手/自动状态
    //    /// </summary>
    //    /// <param name="loc"></param>
    //    /// <returns></returns>
    //    public bool IsStatusLoc(string loc)
    //    {
    //        var sql = @"SELECT T.PLC_STATUS FROM wst_loc T WHERE T.LOC_NO = @loc;";
    //        var result = DapperHelper.QueryOne<int>(sql, new { loc });
    //        if (result == 1)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    #endregion GetTask

    //    /// <summary>
    //    /// 获取两线柜体数
    //    /// </summary>
    //    /// <param name="colCountI"></param>
    //    /// <param name="colCountII"></param>
    //    public void GetLineColCount(out int colCountI, out int colCountII)
    //    {
    //        colCountI = 5;
    //        colCountII = 5;
    //        try
    //        {
    //            string sql = @"select row_id rowId,count(t.COL_ID)*2 colCount from
    //                       (select row_id, COL_ID from bas_bin where CABINET_NO != '' group by row_id, COL_ID)t
    //                       group by t.row_id order by t.row_id asc; ";
    //            List<ColCountInfo> colCountInfos = DapperHelper.QueryList<ColCountInfo>(sql);
    //            if (colCountInfos != null && colCountInfos.Count > 0)
    //            {
    //                foreach (var item in colCountInfos)
    //                {
    //                    if (item.rowId == "1")
    //                        colCountI = item.colCount;
    //                    if (item.rowId == "2")
    //                        colCountII = item.colCount;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取两线柜体数异常！异常提示【{ex}】", "sql");
    //            return;
    //        }
    //    }

    //    /// <summary>
    //    /// 查找没有出站的电芯条码
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public List<string> QueryLoadToMesQueryBattery(int taskno)
    //    {
    //        string sql = $"select T1.BATTERY_NO from biz_tray_dtl T1 JOIN biz_task_pack T2 ON T1.TRAY_NO = T2.TRAY_NO where T2.TASK_NO={taskno}";
    //        return DapperHelper.QueryList<string>(sql);
    //    }

    //    /// <summary>
    //    /// 查找禁用的层号
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public dynamic SelectLayerNo(int usedflag)
    //    {
    //        try
    //        {
    //            string sql = $"select T1.LOC_NAME ,T.SUB_OFFSET FROM bas_loc_dtl T JOIN BAS_LOC T1 ON T.LOC_NO=T1.LOC_NO WHERE T.USED_FLAG={usedflag}";
    //            return DapperHelper.QueryList<dynamic>(sql);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】查询禁用层号失败！异常提示【{ex}】", "sql");
    //            return new List<string>();
    //        }
    //    }

    //    /// <summary>
    //    /// 查找没有出站的电芯条码
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public List<string> QueryLoadToMesQueryBattery(string trayNo)
    //    {
    //        try
    //        {
    //            string sql = $"select T1.BATTERY_NO from biz_tray_dtl T1 where T1.TRAY_NO = {trayNo}";
    //            return DapperHelper.QueryList<string>(sql);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】未下发Mes的电芯条码失败！异常提示【{ex}】", "sql");
    //            return new List<string>();
    //        }
    //    }


    //    public void UpdateTaskRecord(int taskNo)
    //    {
    //        try
    //        {
    //            DapperHelper.Execute($"UPDATE biz_task_record T SET T.CONFIRM_TIME = NOW(3) WHERE T.TASK_NO ={taskNo}", null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】修改biz_task_record任务更新时间发生错误！异常提示【{ex}】", "sql");
    //        }
    //    }

    //    public async Task UpdateTaskRecordAsync(int taskNo)
    //    {
    //        try
    //        {
    //            await DapperHelper.ExecuteAsync($"UPDATE biz_task_record T SET T.CONFIRM_TIME = NOW(3) WHERE T.TASK_NO ={taskNo}", null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】修改biz_task_record任务更新时间发生错误！异常提示【{ex}】", "sql");
    //        }
    //    }

    //    public void UpdateUsedFlag(string trayNo, int usedFlag)
    //    {
    //        try
    //        {
    //            DapperHelper.Execute($"update bas_loc_dtl T SET T.USED_FLAG={usedFlag} " +
    //                $"WHERE T.LOC_NO=(SELECT T1.LOC_NO FROM biz_tray_temp T1 WHERE T1.TRAY_NO='{trayNo}' ORDER BY T1.CREATE_TIME DESC LIMIT 1) " +
    //                $"AND T.SUB_OFFSET=(SELECT T1.LOC_OFFSET FROM biz_tray_temp T1 WHERE T1.TRAY_NO='{trayNo}' ORDER BY T1.CREATE_TIME DESC LIMIT 1) ", null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】修改bas_loc_dtl中的used_flag失败！异常提示【{ex}】", "sql");
    //        }
    //    }

    //    //获取炉腔非低温夹具最小温度夹具码
    //    public string GetMinTempTray(string locno)
    //    {
    //        var sql = @"SELECT DISTINCT(a.TRAY_NO) from biz_tray_temp a where a.LOC_NO = @locno and a.TEMPERATURE >= 95 ORDER BY a.TEMPERATURE  LIMIT 1;";
    //        return DapperHelper.QueryOne<string>(sql, new { locno });
    //    }
    //    public dynamic GetWaterFlagTrayInfo(string trayNo)
    //    {
    //        var sql = @"SELECT T.TRAY_NO,T.LOC_OFFSET,T.TRAY_STATUS,T.LOADED_QTY,T.LOADED_TYPE,T.UPLOAD_FLAG,T.OUTTIME_FLAG,T.WATERNG_FLAG,T.BIZ_TYPE_NO,T.BAKING_QTY,T.WBATTERY_NO,T1.LOC_NO,T2.CHANNEL_ID FROM BIZ_TRAY T LEFT JOIN BIZ_TRAY_TEMP T1 ON T.TRAY_NO=T1.TRAY_NO LEFT JOIN BIZ_TRAY_DTL T2 ON T.TRAY_NO=T2.TRAY_NO WHERE T.TRAY_NO=@trayNo AND T.WATER_FLAG=1 AND T2.BATTERY_TYPE=2 LIMIT 1;";
    //        return DapperHelper.QueryOne<dynamic>(sql, new { trayNo });
    //    }

    //    public dynamic GetTrayInfo(string trayno)
    //    {
    //        var sql = @"SELECT T.TRAY_STATUS,T.LOADED_QTY,T.LOADED_TYPE,T.UPLOAD_FLAG,T.OUTTIME_FLAG,T.WATERNG_FLAG,T.BIZ_TYPE_NO,T.BAKING_QTY,T.WATER_FLAG FROM biz_tray T WHERE T.TRAY_NO = @trayno LIMIT 1;";
    //        return DapperHelper.QueryOne<dynamic>(sql, new { trayno });
    //    }

    //    public async Task<dynamic> GetTrayInfoAsync(string trayno)
    //    {
    //        var sql = @"SELECT T.TRAY_STATUS,T.LOADED_QTY,T.LOADED_TYPE,T.UPLOAD_FLAG,T.OUTTIME_FLAG,T.WATERNG_FLAG,T.BIZ_TYPE_NO,T.BAKING_QTY,T.WATER_FLAG FROM biz_tray T WHERE T.TRAY_NO = @trayno LIMIT 1;";
    //        return await DapperHelper.QueryOneAsync<dynamic>(sql, new { trayno });
    //    }
    //    /// <summary>
    //    /// 是否存在未调出站的电芯
    //    /// </summary>
    //    /// <param name="trayno"></param>
    //    /// <returns></returns>
    //    public bool ExistsNotMesOut(string trayNo)
    //    {
    //        try
    //        {
    //            //var sql = @"SELECT T.MES_RESULT FROM biz_tray_dtl T WHERE T.TRAY_NO = @trayno and T.BATTERY_TYPE = 1 order by T.MES_RESULT LIMIT 1;";
    //            var sql = @"SELECT COUNT(1) FROM biz_tray_dtl WHERE TRAY_NO = @trayNo AND (IFNULL(MES_RESULT,'') = '' or MES_RESULT = 0);";//是否存在未调出站的电芯

    //            return DapperHelper.QueryOne<int>(sql, new { trayNo }) > 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】biz_tray_dtl中的MES_RESULT失败！异常提示【{ex}】", "sql");
    //            return true;
    //        }
    //    }

    //    public string ReGetTraydtlInfo(string trayno)
    //    {
    //        try
    //        {
    //            var sql = @"SELECT T.MES_RESULT FROM biz_tray_dtl T WHERE T.TRAY_NO = @trayno and T.BATTERY_TYPE = 1 order by T.MES_RESULT LIMIT 1;";
    //            //var sql = @"SELECT T.MES_RESULT FROM biz_tray_dtl T WHERE T.TRAY_NO = @trayno order by T.MES_RESULT LIMIT 1;";//不区分水含量电芯

    //            return DapperHelper.QueryOne<string>(sql, new { trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】biz_tray_dtl中的MES_RESULT失败！异常提示【{ex}】", "sql");
    //            return "999";
    //        }
    //    }



    //    public async Task<string> GetTraydtlInfoAsync(string trayno)
    //    {
    //        try
    //        {
    //            var sql = @"SELECT T.MES_RESULT FROM biz_tray_dtl T WHERE T.TRAY_NO = @trayno and T.BATTERY_TYPE = 1 order by T.MES_RESULT LIMIT 1;";
    //            return await DapperHelper.QueryOneAsync<string>(sql, new { trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】biz_tray_dtl中的MES_RESULT失败！异常提示【{ex}】", "sql");
    //            return "999";
    //        }
    //    }

    //    public string GetTraynoFromTaskNo(int taskno)
    //    {
    //        var sql = $@"SELECT TRAY_NO from biz_task_pack where TASK_NO = '{taskno}' LIMIT 1;";
    //        return DapperHelper.QueryOne<string>(sql, null);
    //    }

    //    public async Task<string> GetTraynoFromTaskNoAsync(int taskno)
    //    {
    //        var sql = $@"SELECT TRAY_NO from biz_task_pack where TASK_NO = '{taskno}' LIMIT 1;";
    //        return await DapperHelper.QueryOneAsync<string>(sql, null);
    //    }

    //    public string GetTaskNoFromTaskNo(int taskno)
    //    {
    //        var sql = $@"SELECT TASK_NO from biz_task_pack where TASK_NO = '{taskno}' LIMIT 1;";
    //        return DapperHelper.QueryOne<string>(sql, null);
    //    }

       

    //    /// <summary>
    //    /// 获取调度机器人当前综合状态
    //    /// </summary>
    //    /// <returns></returns>
    //    public string GetRGVTaskStatus()
    //    {
    //        try
    //        {
    //            string sql = $"select `DESC` FROM WST_RGV ";
    //            return DapperHelper.QueryList<string>(sql)[0];
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取RGV任务状态失败！异常提示【{ex}】", "sql");
    //            return "";
    //        }
    //    }

    //    /// <summary>
    //    /// 获取烘烤温度是否低温
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public bool GetTemp(string trayNo)
    //    {
    //        try
    //        {
    //            string sql = $@"select COUNT(1) FROM biz_tray_temp T join biz_tray T1 ON T1.TRAY_NO=T.TRAY_NO where T.TRAY_NO= '{trayNo}' and T.TEMPERATURE<95 and T1.BAKING_QTY<3;";
    //            return DapperHelper.QueryOne<int>(sql, new { trayNo }) >= 1;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取biz_tray_temp温度失败！异常提示【{ex}】", "sql");
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取水含量预测值上传MES是否满足要求
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public int GetCalculateData(string trayNo)
    //    {
    //        string sql = $@"select COUNT(1) FROM biz_tray_dtl T where T.TRAY_NO= '{trayNo}' and T.MES_RESULT = 5 and T.BATTERY_TYPE = 1;";
    //        return DapperHelper.QueryOne<int>(sql, new { trayNo });
    //    }

    //    /// <summary>
    //    /// 获取水含量预测值上传MES是否满足要求yibu
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public async Task<int> GetCalculateDataAsync(string trayNo)
    //    {
    //        try
    //        {
    //            string sql = $@"select COUNT(1) FROM biz_tray_dtl T where T.TRAY_NO= '{trayNo}' and T.MES_RESULT = 5 and T.BATTERY_TYPE = 1;";
    //            return await DapperHelper.QueryOneAsync<int>(sql, new { trayNo });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取biz_tray_dtl失败！异常提示【{ex}】", "sql");
    //            return 0;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取夹具烘烤次数
    //    /// </summary>
    //    /// <param name="trayno"></param>
    //    /// <returns></returns>
    //    public int GetTrayBakingCount(string trayno)
    //    {
    //        try
    //        {
    //            string sql = $@"select T.BAKING_QTY FROM biz_tray T where T.TRAY_NO= '{trayno}';";
    //            return DapperHelper.QueryOne<int>(sql, new { trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取biz_tray失败！异常提示【{ex}】", "sql");
    //            return 0;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取托盘装载数量
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public int GetBatteryCount(string trayNo)
    //    {
    //        string sql = $@"select LOADED_QTY FROM biz_tray where TRAY_NO= '{trayNo}';";
    //        return DapperHelper.QueryOne<int>(sql, new { trayNo });
    //    }

    //    /// <summary>
    //    /// 更改托盘状态为等待烘烤
    //    /// </summary>
    //    /// <param name="trayNo"></param>
    //    /// <returns></returns>
    //    public bool TrayStatusChange(string trayNo)
    //    {
    //        try
    //        {
    //            string sql = $@"UPDATE biz_tray set TRAY_STATUS='2' where TRAY_NO='{trayNo}'; ";
    //            DapperHelper.Execute(sql, null);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】更新biz_tray托盘状态失败！异常提示【{ex}】", "sql");
    //            return false;
    //        }
    //    }

      


    //    /// <summary>
    //    /// 获取wcs_unpack_dtl表中虚拟码和电芯码对应关系
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<(string, int)> GetVirtuallist(int[] Virtuallist)
    //    {
    //        try
    //        {
    //            var list = new List<(string, int)>();
    //            string sql = $"SELECT BATTERY_NO,MES_RESULT FROM wcs_unpack_dtl  WHERE VIRTUAL_NO = @Virtuallist;";
    //            foreach (var s in Virtuallist)
    //            {
    //                list.Add(DapperHelper.QueryOne<(string, int)>(sql, new { Virtuallist = s }));
    //            }
    //            return list;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】拆盘后扫码，获取wcs_unpack_dtl表中虚拟码和电芯码对应关系异常！异常提示【{ex}】", "sql");
    //            return null;
    //        }
    //    }

    //    public List<dynamic> GetVirtuallist1(int[] Virtuallist)
    //    {
    //        string sql = $"SELECT * FROM wcs_unpack_dtl  WHERE VIRTUAL_NO in @Virtuallist;";
    //        if (Virtuallist.Length > 0)
    //            return DapperHelper.QueryList<dynamic>(sql, new { Virtuallist });
    //        else
    //            return new List<dynamic>();
    //    }
    //    public List<dynamic> GetVirtuallistByTrayNo(string trayno)
    //    {
    //        string sql = $"SELECT * FROM wcs_unpack_dtl  WHERE TRAY_NO=@trayno;";
    //        return DapperHelper.QueryList<dynamic>(sql, new { trayno });
    //    }
    //    public async Task<List<dynamic>> GetVirtuallist1Async(int[] Virtuallist)
    //    {
    //        string sql = $"SELECT * FROM wcs_unpack_dtl  WHERE VIRTUAL_NO in @Virtuallist;";
    //        if (Virtuallist.Length > 0)
    //            return await DapperHelper.QueryListAsync<dynamic>(sql, new { Virtuallist });
    //        else
    //            return new List<dynamic>();
    //    }

      




    //    public int GetLocUseFlag(string LocNo)
    //    {
    //        string sql = $"SELECT T.USED_FLAG FROM bas_loc T WHERE T.LOC_NO = @LocNo LIMIT 1;";
    //        return DapperHelper.QueryOne<int>(sql, new { LocNo });
    //    }

    //    public async Task<int> GetLocUseFlagAsync(string LocNo)
    //    {
    //        string sql = $"SELECT T.USED_FLAG FROM bas_loc T WHERE T.LOC_NO = @LocNo LIMIT 1;";
    //        return await DapperHelper.QueryOneAsync<int>(sql, new { LocNo });
    //    }

    //    public string GetWaterBatteryNo(string taryno)
    //    {
    //        string sql = $"SELECT T.BATTERY_NO FROM biz_task_check T WHERE T.TRAY_NO = @taryno ORDER BY T.CREATE_DATE LIMIT 1 ;";
    //        return DapperHelper.QueryOne<string>(sql, new { taryno });
    //    }


    //    /// <summary>
    //    // 获取回流通道配置信息
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<dynamic> GetRollerConfig()
    //    {
    //        try
    //        {
    //            var sql = $"select * from bas_roller;";
    //            return DapperHelper.QueryList<dynamic>(sql);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】获取回流通道配置信息异常！异常提示【{ex}】", "sql");
    //            return null;
    //        }
    //    }

    //    public List<BasUiConfig> GetStoveUi()
    //    {
    //        try
    //        {
    //            string sql = @"select t.BIN_NO bin_No,t.COL_ID Y,t.ROW_ID X,t.BIN_LOCAL Local from bas_ui_config t;";
    //            return DapperHelper.QueryList<BasUiConfig>(sql, null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取炉腔前台界面配置信息错误！异常提示【{ex}】", "System");
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// Redi数据操作
    //    /// </summary>
    //    /// <param name="pairs"></param>
    //    /// <param name="battertType">拆解电池类型：1:遗留  2：剩余   3：正常拆解</param>
    //    public void OperateByRedis(string sql)
    //    {
    //        try
    //        {
    //            DapperHelper.Execute(sql, null);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】Redis操作biz_batteryinfo中数据异常！异常提示【{ex}】", "sql");
    //        }
    //    }

    //    /// <summary>
    //    /// 将电芯REMAIN_FLAG标志更新到biz_tray_dtl表
    //    /// </summary>
    //    /// <param name="pairs"></param>
    //    /// <param name="battertType">拆解电池类型：1:遗留  2：剩余   3：正常拆解</param>
    //    public void UpdateUnpackDtl(List<dynamic> batterys)
    //    {
    //        DapperHelper.Execute(@"
    //           UPDATE biz_tray_dtl SET REMAIN_FLAG=@RemainFlag where TRAY_NO = @trayNo AND CHANNEL_ID = @channelId;"
    //            , batterys);
    //    }

    //    /// <summary>
    //    /// 删除对应的电芯
    //    /// </summary>
    //    /// <param name="startindex"></param>
    //    /// <param name="endindex"></param>
    //    public int DeleteBattery(int startindex, int endindex, string trayno)
    //    {
    //        try
    //        {
    //            string sql = $"DELETE FROM BIZ_TRAY_DTL WHERE CHANNEL_ID>=@startindex AND CHANNEL_ID<=@endindex AND TRAY_NO = @trayNO; SELECT count(*) from BIZ_TRAY_DTL where TRAY_NO = @trayNO;";
    //            return DapperHelper.QueryOne<int>(sql, new { startindex, endindex, trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】删除biz_tray_dtl中数据异常！异常提示【{ex}】", "sql");
    //            return -1;
    //        }
    //    }

    //    public void UpdateTrayBatteryQty(int qty, string trayno)
    //    {
    //        try
    //        {
    //            string sql = $"Update biz_tray set loaded_qty=@qty where tray_no=@trayno;";
    //            DapperHelper.Execute(sql, new { qty, trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】更新biz_tray中电芯数量失败！异常提示【{ex}】", "sql");
    //        }
    //    }

    //    /// <summary>
    //    /// 删除biz_tray_temp中的电芯
    //    /// </summary>
    //    /// <param name="pairs"></param>
    //    /// <param name="battertType">拆解电池类型：1:遗留  2：剩余   3：正常拆解</param>
    //    public void DeleteTempBattery(string trayNO)
    //    {
    //        try
    //        {
    //            string sql = "INSERT INTO c100_mib_zbk.zbk_biz_tray_temp(OBJID, LOC_NO, CREATE_TIME,BAKING_START_DATE, ORDER_NO, TRAY_NO, BATTERY_NO, QUALITY_TYPE, TEMPERATURE, CHANNEL_ID, MES_NO, BAKING_TIME, TEMP_VAL_AVG,TEMP_VAL_MAX, TEMP_VAL_MIN, TEMP_AVG_MAIN, TEMP_AVG_SUB, STOVETEMP_VAL_AVG, STOVETEMP_VAL_MAX, STOVETEMP_VAL_MIN,LOC_OFFSET)" +
    //                        $"SELECT 1, LOC_NO, CREATE_TIME,BAKING_START_DATE, ORDER_NO, TRAY_NO, BATTERY_NO, QUALITY_TYPE, TEMPERATURE, CHANNEL_ID, MES_NO, BAKING_TIME, TEMP_VAL_AVG," +
    //                        $"TEMP_VAL_MAX, TEMP_VAL_MIN, TEMP_AVG_MAIN, TEMP_AVG_SUB, STOVETEMP_VAL_AVG, STOVETEMP_VAL_MAX, STOVETEMP_VAL_MIN,LOC_OFFSET  FROM c100_mib.biz_tray_temp T WHERE T.TRAY_NO = @trayNO;" +
    //                        $"DELETE FROM biz_tray_temp WHERE TRAY_NO = @trayNO; commit;";
    //            DapperHelper.Execute(sql, new { trayNO });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】删除biz_tray_temp中数据异常！异常提示【{ex}】", "sql");
    //        }
    //    }

    //    public int GetBatterytype(string trayno, int channelid)
    //    {
    //        try
    //        {
    //            string sql = @"select battery_type from biz_tray_dtl where tray_no=@trayno and channel_id=@channelid ;";
    //            return DapperHelper.QueryOne<int>(sql, new { trayno, channelid });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取电芯数据失败！异常提示【{ex}】", "System");
    //            return 0;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取是否已上传的水电芯
    //    /// </summary>
    //    public bool GetIsBatteryWater(string trayno, int channelid)
    //    {
    //        try
    //        {
    //            string sql = @"select count(*) from biz_tray_dtl where tray_no = @trayno and channel_id=@channelid and battery_type = 2 and mes_result is not null;";
    //            int count = DapperHelper.QueryOne<int>(sql, new { trayno, channelid });
    //            return count > 0 ? true : false;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取水含量电芯数据失败！异常提示【{ex}】", "System");
    //            return false;
    //        }
    //    }

    //    public int Getlocoffset(string trayno)
    //    {
    //        try
    //        {
    //            string sql = @"select LOC_OFFSET from biz_tray where tray_no=@trayno;";
    //            return DapperHelper.QueryOne<int>(sql, new { trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取位置偏移量失败【{ex}】", "System");
    //            return 0;
    //        }
    //    }

    //    public bool ChangeBatterytype(string trayno, int channelid, int batterytype)
    //    {
    //        try
    //        {
    //            string sql = @"Update biz_tray_dtl set battery_type=@batterytype where tray_no=@trayno and channel_id=@channelid;";
    //            DapperHelper.Execute(sql, new { trayno, channelid, batterytype });
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】更新biz_tray_dtl中数据异常！异常提示【{ex}】", "sql");
    //            return false;
    //        }
    //    }

    //    public bool DeteleBattery(string trayno, int channelid, int loadedqty)
    //    {
    //        try
    //        {
    //            string sql = "INSERT INTO C100_mib_zbk.zbk_biz_tray_dtl(BK_DATE,TRAY_NO,CHANNEL_ID,BATTERY_NO,BATTERY_TYPE,BAKING_qty,OUT_TIME,AlarmTemp,QUALITY)" +
    //                         $"SELECT SYSDATE(3),TRAY_NO,CHANNEL_ID,BATTERY_NO,BATTERY_TYPE,BAKING_qty,OUT_TIME,AlarmTemp,QUALITY FROM C100_mib.biz_tray_dtl T " +
    //                         $"where tray_no=@trayno and channel_id=@channelid;";

    //            sql += @"DELETE FROM biz_tray_dtl where tray_no=@trayno and channel_id=@channelid;";
    //            if (loadedqty > 1)
    //            {
    //                sql += @"update biz_tray set loaded_qty=@loadedqty-1,loaded_type=2 where tray_no=@trayno;commit;";
    //            }
    //            else
    //            {
    //                sql += @"update biz_tray set loaded_qty=0,loaded_type=1 where tray_no=@trayno;commit;";
    //            }
    //            sql += @"";
    //            DapperHelper.Execute(sql, new { trayno, channelid, loadedqty });
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"【异常】更新biz_tray_dtl中数据异常！异常提示【{ex}】", "sql");
    //            return false;
    //        }
    //    }

    //    public int GetTrayLoadedQty(string trayno)
    //    {
    //        try
    //        {
    //            string sql = @"select LOADED_QTY from biz_tray where tray_no=@trayno;";
    //            return DapperHelper.QueryOne<int>(sql, new { trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取电芯数据失败！异常提示【{ex}】", "System");
    //            return 0;
    //        }
    //    }
    //    /// <summary>
    //    /// 获取炉子烘烤次数（到6次需要标记水电芯）
    //    /// </summary>
    //    /// <param name="locNo"></param>
    //    /// <returns></returns>
    //    public int GetWaterCheckCount(string locNo)
    //    {
    //        try
    //        {
    //            string sql = @"SELECT W.WATER_COUNT_FLAG FROM wst_loc W WHERE W.LOC_NO = @locNo";
    //            return DapperHelper.QueryOne<int>(sql, new { locNo });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取WST_LOC数据失败！异常提示【{ex}】", "System");
    //            return -1;
    //        }
    //    }
    //    /// <summary>
    //    /// 获取订单类型
    //    /// </summary>
    //    /// <param name="trayno"></param>
    //    /// <returns></returns>
    //    public string GetOrderTypeNo(string trayno)
    //    {
    //        try
    //        {
    //            string sql = @"SELECT a.ORDER_TYPE_NO FROM biz_order a LEFT JOIN biz_tray b on a.LOC_NO = b.LOC_NO where b.TRAY_NO = @trayno;";
    //            return DapperHelper.QueryOne<string>(sql, new { trayno });
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error($"获取订单数据失败！异常提示【{ex}】", "System");
    //            return "9999";
    //        }
    //    }

    //    /// <summary>
    //    /// 保存产能文件
    //    /// </summary>
    //    /// <param name="filePath">文件路径</param>
    //    /// <param name="data">数据</param>
    //    /// <param name="header">文件头</param>
    //    /// <param name="fileName">文件名</param>
    //    public void SaveProductionCSVFile(string filePath, string data, string header, string fileName)
    //    {
    //        DirectoryInfo fileinpath = new DirectoryInfo(ConfigurationManager.AppSettings["ScanDataPath"] + filePath);
    //        if (!fileinpath.Exists)
    //        {
    //            fileinpath.Create();
    //        }
    //        if (!System.IO.File.Exists(fileinpath + "/" + fileName + ".lead"))
    //        {
    //            FileStream fs1 = new FileStream(fileinpath + "/" + fileName + ".lead", FileMode.Create, FileAccess.Write);
    //            StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
    //            sw.WriteLine(header);
    //            sw.WriteLine(data);
    //            sw.Flush();
    //            sw.Close();
    //            fs1.Close();
    //        }
    //        else
    //        {
    //            FileStream fs = new FileStream(fileinpath + "/" + fileName + ".lead" + "", FileMode.Append, FileAccess.Write);
    //            StreamWriter sr = new StreamWriter(fs, Encoding.UTF8);
    //            sr.WriteLine(data);
    //            sr.Flush();
    //            sr.Close();
    //            fs.Close();
    //        }

    //        string fileOutPath = ConfigurationManager.AppSettings["ScanOutDataPath"];
    //        if (string.IsNullOrEmpty(fileOutPath))
    //        {
    //            return;
    //        }
    //        if (!System.IO.Directory.Exists(fileOutPath))
    //        {
    //            System.IO.Directory.CreateDirectory(fileOutPath);
    //        }
    //        fileOutPath = fileOutPath + filePath + "/";
    //        //var msg = FileExtension.RW.FileHelper.CopyFileDelete(fileOutPath, fileinpath + "/" + fileName + ".lead", ".csv");
    //    }

    //    /// <summary>
    //    /// 保存水含量文件
    //    /// </summary>
    //    /// <param name="filePath">文件路径</param>
    //    /// <param name="data">数据</param>
    //    /// <param name="header">文件头</param>
    //    /// <param name="fileName">文件名</param>
    //    public void SaveWaterBatteryCSVFile(string filePath, string data, string header, string fileName)
    //    {
    //        DirectoryInfo fileinpath = new DirectoryInfo(ConfigurationManager.AppSettings["ScanDataPath"] + filePath);
    //        if (!fileinpath.Exists)
    //        {
    //            fileinpath.Create();
    //        }
    //        if (!System.IO.File.Exists(fileinpath + "/" + fileName + ".lead"))
    //        {
    //            FileStream fs1 = new FileStream(fileinpath + "/" + fileName + ".lead", FileMode.Create, FileAccess.Write);
    //            StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
    //            sw.WriteLine(header);
    //            sw.WriteLine(data);
    //            sw.Flush();
    //            sw.Close();
    //            fs1.Close();
    //        }
    //        else
    //        {
    //            FileStream fs = new FileStream(fileinpath + "/" + fileName + ".lead" + "", FileMode.Append, FileAccess.Write);
    //            StreamWriter sr = new StreamWriter(fs, Encoding.UTF8);
    //            sr.WriteLine(data);
    //            sr.Flush();
    //            sr.Close();
    //            fs.Close();
    //        }

    //        string fileOutPath = ConfigurationManager.AppSettings["ScanOutDataPath"];
    //        if (string.IsNullOrEmpty(fileOutPath))
    //        {
    //            return;
    //        }
    //        if (!System.IO.Directory.Exists(fileOutPath))
    //        {
    //            System.IO.Directory.CreateDirectory(fileOutPath);
    //        }
    //        fileOutPath = fileOutPath + filePath + "/";
    //        //var msg = FileExtension.RW.FileHelper.CopyFileDelete(fileOutPath, fileinpath + "/" + fileName + ".lead", ".csv");
    //    }
    //}

    /// <summary>
    /// 左右线柜体数量信息
    /// </summary>
    public class ColCountInfo
    {
        /// <summary>
        /// 线号
        /// </summary>
        public string rowId { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int colCount { get; set; }
    }

    public class BasUiConfig
    {
        public string bin_No { get; set; }

        /// <summary>
        /// 前台界面X轴
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 前台界面Y轴
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 前台位置
        /// </summary>
        public int Local { get; set; }
    }
}