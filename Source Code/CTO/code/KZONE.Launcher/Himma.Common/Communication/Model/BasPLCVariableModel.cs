using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Communication.Model
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Henick
    /// 创建时间：2020/4/13 16:00:48
    /// </summary>
    [SugarTable("bas_plc_variable")]
    public partial class BasPLCVariableModel
    {
        ///<summary>序号</summary>
        [SugarColumn(ColumnName = "sid")]
        public long Sid { get; set; }

        ///<summary>PLC变量名</summary>
        [SugarColumn(ColumnName = "variable_id")]
        public string VariableName { get; set; }

        ///<summary>PLC变量描述</summary>
        [SugarColumn(ColumnName = "variable_name")]
        public string VariableDescription { get; set; }

        ///<summary>读/写标志(2：订阅 1:读；0：写入)</summary>
        [SugarColumn(ColumnName = "rw_flag")]
        public short RwFlag { get; set; }

        ///<summary>句柄序列</summary>
        [SugarColumn(ColumnName = "handle_no")]
        public int HandleNo { get; set; }

        ///<summary>变量值类型(1:int；2：byte；3：boolen；4.Float；5.String；6.Uint32 用于ADS代码)</summary>
        [SugarColumn(ColumnName = "typeofvariable")]
        public short Typeofvariable { get; set; }

        ///<summary>数组维度</summary>
        [SugarColumn(ColumnName = "array_flag")]
        public int ArrayFlag { get; set; }

        ///<summary>变量数组长度（0：无  |  2,3 | 30 ）</summary>
        [SugarColumn(ColumnName = "arraydimension")]
        public string Arraydimension { get; set; }

        ///<summary>plc类型</summary>
        [SugarColumn(ColumnName = "plc_type")]
        public string PLCType { get; set; }

        ///<summary>创建日期</summary>
        [SugarColumn(ColumnName = "create_datetime")]
        public DateTime CreateDatetime { get; set; }
    }
}

