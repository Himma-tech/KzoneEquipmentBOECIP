using Himma.Common.Communication.DAL;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.DAL.BaseInfo
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：产品数据配置表
    /// 创建人： Henick
    /// 创建时间：2020/4/13 10:35:33
    /// </summary>
    [SugarTable("bas_para_variable")]
    public class BasParaVariableModel
    {
        ///<summary>序号</summary>
        [SugarColumn(ColumnName = "sid", IsPrimaryKey = true, IsIdentity = true)]
        public int Sid { get; set; }

        ///<summary>参数ID</summary>
        [SugarColumn(ColumnName = "para_id")]
        public string ParaId { get; set; }
        ///<summary>参数名称</summary>
        [SugarColumn(ColumnName = "para_name")]
        public string ParaName { get; set; }
        /// <summary>
        /// 参数的处理流程 用来加速处理数据 并不存在于数据库
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int ParaMakeType { get; set; }
        /// <summary>
        /// 参数的处理流程 用来加速处理数据 并不存在于数据库
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int ParaMakeTypeV1 { get; set; }

        ///<summary></summary>
        [SugarColumn(ColumnName = "para_key")]
        public string ParaKey { get; set; }

        ///<summary>参数单位</summary>
        [SugarColumn(ColumnName = "para_unit")]
        public string ParaUnit { get; set; }

        ///<summary>PLC 变量地址</summary>
        [SugarColumn(ColumnName = "station_name")]
        public string StationName { get; set; }

        ///<summary>变量类型，一般只有float：1；string：2;dwNG:3</summary>
        [SugarColumn(ColumnName = "para_type")]
        public int ParaType { get; set; }

        ///<summary>数组下标</summary>
        [SugarColumn(ColumnName = "array_no")]
        public int ArrayNo { get; set; }

        ///<summary>采集工序编码</summary>
        [SugarColumn(ColumnName = "process_no")]
        public string ProcessNo { get; set; }

        ///<summary>创建时间</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }

        ///<summary>是否启用标志</summary>
        [SugarColumn(ColumnName = "used_flag")]
        public Byte UsedFlag { get; set; }

        ///<summary>备注</summary>
        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string value { get; set; }
    }

}
