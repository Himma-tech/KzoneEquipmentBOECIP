using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Himma.Common.Models;

namespace Himma.Common.BaseData.Func
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    //public class SysUserDataAccess
    //{
    //    /// <summary>
    //    /// 保存人员信息
    //    /// </summary>
    //    /// <param name="sysUserAccount">用户对象</param>
    //    public bool SaveUserInfo(SysUserAccount sysUserAccount)
    //    {
    //        try
    //        {
    //            string sql = $"update sys_user_account set " +
    //                         $"user_name='{sysUserAccount.UserName}',user_psd='{sysUserAccount.UserPsd}'," +
    //                         $"level='{sysUserAccount.UserLevel}' where user_id='{sysUserAccount.UserId}';commit;";
    //            DapperHelper.Execute(sql, null);
    //            return true;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// 新增用户
    //    /// </summary>
    //    /// <param name="sysUserAccount"></param>
    //    /// <returns></returns>
    //    public bool AddUserInfo(SysUserAccount sysUserAccount)
    //    {
    //        try
    //        {
    //            string sql = $"insert into sys_user_account(user_id,user_name,user_psd,level) " +
    //                $"values('{sysUserAccount.UserId}','{sysUserAccount.UserName}','{sysUserAccount.UserPsd}','{sysUserAccount.UserLevel}');commit;";
    //            DapperHelper.Execute(sql, null);
    //            return true;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// 查询用户信息
    //    /// </summary>
    //    /// <param name="level"></param>
    //    /// <returns></returns>
    //    public List<SysUserAccount> SearchUserInfo(int level)
    //    {
    //        try
    //        {
    //            string sql = $"select user_id as UserId,user_psd as UserPsd,user_name as UserName,level as UserLevel " +
    //                         $"from sys_user_account where 1=1 and level<={level};";
    //            List<SysUserAccount> sysUserAccounts = DapperHelper.QueryList<SysUserAccount>(sql);
    //            if (sysUserAccounts != null && sysUserAccounts.Count > 0)
    //            {
    //                for (int i = 0; i < sysUserAccounts.Count; i++)
    //                {
    //                    sysUserAccounts[i].Id = (i + 1).ToString();
    //                }
    //            }
    //            return sysUserAccounts;
    //        }
    //        catch (Exception)
    //        {
    //            return null;
    //        }
    //    }
    //}
}
