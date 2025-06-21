using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.DAL.BizModel
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：电芯结果表
    /// 创建人： Henick
    /// 创建时间：2020/4/13 11:17:12
    /// </summary>
    [SugarTable("biz_battery_result")]
    public partial class BizBatteryResultModel
    {
        ///<summary></summary>

        [SugarColumn(ColumnName = "sid", IsPrimaryKey = true, IsIdentity = true)]
        public int Sid { get; set; }

        ///<summary>电池RFID</summary>
        [SugarColumn(ColumnName = "rfid")]
        public string Rfid { get; set; }

        ///<summary>电池GUID</summary>
        [SugarColumn(ColumnName = "guid")]
        public string Guid { get; set; }

        ///<summary>OK/NG</summary>
        [SugarColumn(ColumnName = "result")]
        public string Result { get; set; }

        ///<summary>ng代码，如有多个ng以英文分号隔开</summary>
        [SugarColumn(ColumnName = "ngcode")]
        public string Ngcode { get; set; }

        ///<summary>电池型号</summary>
        [SugarColumn(ColumnName = "model")]
        public string Model { get; set; }

        ///<summary>品质数据</summary>
        [SugarColumn(ColumnName = "detail")]
        public string Detail { get; set; }


        ///<summary>是否上传</summary>
        [SugarColumn(ColumnName = "IsUploaded")]
        public Byte Isuploaded { get; set; }

        ///<summary>生产时间</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }

        ///<summary>下料工位</summary>
        [SugarColumn(ColumnName = "station")]
        public string Station { get; set; }
        ///<summary>station</summary>
        [SugarColumn(ColumnName = "stationindex")]
        public string StationIndex { get; set; }


    }

}
