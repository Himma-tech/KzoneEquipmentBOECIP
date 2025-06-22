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
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    [SugarTable("bas_signal_info")]
    public class BasSignalInfoModel
    {
        ///<summary></summary>
        [SugarColumn(ColumnName = "sid")]
        public int Sid { get; set; }

        ///<summary>变量名</summary>
        [SugarColumn(ColumnName = "variable_name")]
        public string VariableName { get; set; }

        ///<summary>信号描述</summary>
        [SugarColumn(ColumnName = "signal_desciption")]
        public string SignalDesciption { get; set; }

        ///<summary>信号触发下标</summary>
        [SugarColumn(ColumnName = "signal_array_idx")]
        public int SignalArrayIndex { get; set; }

        ///<summary>信号反馈下标</summary>
        [SugarColumn(ColumnName = "signal_down_idx")]

        public int SignalDownIndex { get; set; }

        ///<summary>工序号</summary>
        [SugarColumn(ColumnName = "process_no")]
        public string ProcessNo { get; set; }

        ///<summary></summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }
        ///<summary></summary>
        [SugarColumn(ColumnName = "biz_type")]
        public string BizType { get; set; }
        ///<summary></summary>
        [SugarColumn(ColumnName = "component_sid")]
        public int ComponentSid { get; set; }
    }

}

