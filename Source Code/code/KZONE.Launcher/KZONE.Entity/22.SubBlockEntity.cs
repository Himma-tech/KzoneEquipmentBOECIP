using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    public class SubBlock : Entity
    {
        public SubBlockEntityData Data { get; set; }

        public SubBlock(SubBlockEntityData data)
        {
            Data = data;
        }

        //即時計算變數 不須存入檔案
        //20170630 add for 紀錄Sub Block Stop bit Off Count
        private int _checkStopBitOffCount = 0;

        /// <summary> Check Stopbit Off Count
        /// 
        /// </summary>
        public int CheckStopBitOffCount
        {
            get { return _checkStopBitOffCount; }
            set { _checkStopBitOffCount = value; }
        }


    }
}
