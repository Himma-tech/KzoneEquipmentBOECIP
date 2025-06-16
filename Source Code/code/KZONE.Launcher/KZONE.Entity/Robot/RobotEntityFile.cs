using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KZONE.Entity
{
    [Serializable]
    public class RobotEntityFile : EntityFile
    {
        public enum OPI_CONTROL_MODE
        {
            NONE = 0,
            SEMI = 1,
            AUTO = 2
        }

        [Serializable]
        public class HistoryCmd
        {
            /// <summary>
            /// 'UP' or 'LOW'
            /// </summary>
            public string Arm { get; set; }
            /// <summary>
            /// 'GET' or 'PUT' or 'EXCHANGE'
            /// </summary>
            public string Action { get; set; }
            /// <summary>
            /// '1:Port01' or '9:HDC'
            /// </summary>
            public string Stage { get; set; }
            /// <summary>
            /// '1' or '2' or '3'
            /// </summary>
            public string Slot { get; set; }
            /// <summary>
            /// 'ON' or 'OFF'
            /// </summary>
            public string FirstGlass { get; set; }
            /// <summary>
            /// 'ON' or 'OFF'
            /// </summary>
            public string LastGlass { get; set; }
            /// <summary>
            /// 'ON' or 'OFF'
            /// </summary>
            public string ReturnToCassette { get; set; }
            /// <summary>
            /// 'ON' or 'OFF'
            /// </summary>
            public string LotEndIndicate { get; set; }
            /// <summary>
            /// UpArmJob('{0}' FCO'{1}' PF'{2}' PM'{3}') or 'null'
            /// </summary>
            public string UpArmJob { get; set; }
            /// <summary>
            /// LowArmJob('{0}' FCO'{1}' PF'{2}' PM'{3}') or 'null'
            /// </summary>
            public string LowArmJob { get; set; }

            public HistoryCmd()
            {
                Arm = string.Empty;
                Action = string.Empty;
                Stage = string.Empty;
                Slot = string.Empty;
                FirstGlass = string.Empty;
                LastGlass = string.Empty;
                ReturnToCassette = string.Empty;
                LotEndIndicate = string.Empty;
                UpArmJob = string.Empty;
                LowArmJob = string.Empty;
            }

            public HistoryCmd(string Arm, string Action, string Stage, string Slot, bool FirstGlass, bool LastGlass, bool ReturnToCassette, bool LotEndIndicate, string UpArmJob, string LowArmJob)
            {
                this.Arm = Arm;
                this.Action = Action;
                this.Stage = Stage;
                this.Slot = Slot;
                this.FirstGlass = FirstGlass ? "ON" : "OFF";
                this.LastGlass = LastGlass ? "ON" : "OFF";
                this.ReturnToCassette = ReturnToCassette ? "ON" : "OFF";
                this.LotEndIndicate = LotEndIndicate ? "ON" : "OFF";
                this.UpArmJob = UpArmJob;
                this.LowArmJob = LowArmJob;
            }
        }

        [Serializable]
        public class HistoryRecord_Text
        {
            /// <summary>
            /// yyyy-MM-dd HH:mm:ss.fff
            /// </summary>
            public string DateTime { get; set; }

            public string Type { get; set; }

            public string Text { get; set; }

            public int Argb { get; set; }

            public HistoryRecord_Text()
            {
                DateTime = "0001-01-01 00:00:00.000";
                Type = string.Empty;
                Text = string.Empty;
                Argb = 0;
            }

            public HistoryRecord_Text(string DateTime, string Type, string Text, int Argb)
            {
                this.DateTime = DateTime;
                this.Type = Type;
                this.Text = Text;
                this.Argb = Argb;
            }
        }

        [Serializable]
        public class HistoryRecord_Grid
        {
            /// <summary>
            /// yyyy-MM-dd HH:mm:ss.fff
            /// </summary>
            public string DateTime { get; set; }
            /// <summary>
            /// 'AUTO' or 'SEMI'
            /// </summary>
            public string CmdType { get; set; }

            /// <summary>
            /// yyyy-MM-dd HH:mm:ss.fff
            /// </summary>
            public string ResultDateTime { get; set; }

            public HistoryCmd FirstCmd { get; set; }

            public HistoryCmd SecondCmd { get; set; }

            public string Text { get; set; }

            public int Argb { get; set; }

            public HistoryRecord_Grid()
            {
                this.DateTime = "0001-01-01 00:00:00.000";
                this.ResultDateTime = null;
                this.CmdType = null;
                this.FirstCmd = null;
                this.SecondCmd = null;
                this.Text = null;
                this.Argb = 0;
            }

            public HistoryRecord_Grid(string DateTime, string Text, int Argb)
            {
                this.DateTime = DateTime;
                this.ResultDateTime = null;
                this.CmdType = null;
                this.FirstCmd = null;
                this.SecondCmd = null;
                this.Text = Text;
                this.Argb = Argb;
            }

            public HistoryRecord_Grid(string DateTime, string CmdType, HistoryCmd FirstCmd, HistoryCmd SecondCmd, int Argb)
            {
                this.DateTime = DateTime;
                this.ResultDateTime = null;
                this.CmdType = CmdType;
                this.FirstCmd = FirstCmd;
                this.SecondCmd = SecondCmd;
                this.Text = null;
                this.Argb = Argb;
            }
        }

        private OPI_CONTROL_MODE _OpiCtrlMode = OPI_CONTROL_MODE.NONE;// BC 與 OPI 之間的 Robot Control Mode
        private bool _CheckEquipmentGroupNumber = false;// BC RCS 是否需要判斷 Equipment Group Number

        /// <summary>
        /// OPI 與 BC 之間的 Robot Control Mode
        /// </summary>
        public OPI_CONTROL_MODE OpiCtrlMode { get { return _OpiCtrlMode; } set { _OpiCtrlMode = value; } }

        public List<HistoryRecord_Text> HistoryRecords_Text { get; set; }

        public List<HistoryRecord_Grid> HistoryRecords_Grid { get; set; }

        /// <summary>
        /// BC RCS 是否需要判斷 Equipment Group Number
        /// </summary>
        public bool CheckEquipmentGroupNumber { get { return _CheckEquipmentGroupNumber; } set { _CheckEquipmentGroupNumber = value; } }

        #region NonSerialized

        /// <summary>
        /// true 表示堵塞. 當 Stage 有出片訊號而 RCS 卻無法下令, 需要 BC 發 Line Down. 即時值, 不存檔.
        /// Robot Traffic Jam, please check Robot Arm and EQ Interface.
        /// </summary>
        [XmlIgnore]
        public bool TrafficJam { get; set; }

        #endregion

        public RobotEntityFile()
        {
            HistoryRecords_Text = new List<HistoryRecord_Text>();
            HistoryRecords_Grid = new List<HistoryRecord_Grid>();
            TrafficJam = false;
        }
    }
}
