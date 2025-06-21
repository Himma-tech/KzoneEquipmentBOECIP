using Himma.Common.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Himma.Common
{
    public class CommonFunc
    {

        /// <summary>
        /// 初始化数组
        /// </summary>
        /// <param name="iTotalWidth">数组长度</param>
        /// <returns></returns>
        public static int[] BuildArray(int iTotalWidth, char dftChar = '0')
        {
            var s1 = string.Join(",", string.Empty.PadLeft(iTotalWidth, dftChar).ToCharArray()).Split(',');
            return Array.ConvertAll(s1, s => int.Parse(s));
        }

        public static Int16[] BuildArrayInt16(int iTotalWidth, char dftChar = '0')
        {
            var s1 = string.Join(",", string.Empty.PadLeft(iTotalWidth, dftChar).ToCharArray()).Split(',');
            return Array.ConvertAll(s1, s => Int16.Parse(s));
        }

        /// <summary>
        /// 初始化数组
        /// </summary>
        /// <param name="iTotalWidth">数组长度</param>
        /// <returns></returns>
        public static float[] BuildArrayReal(int iTotalWidth, char dftChar = '0')
        {
            var s1 = string.Join(",", string.Empty.PadLeft(iTotalWidth, dftChar).ToCharArray()).Split(',');
            return Array.ConvertAll(s1, s => float.Parse(s));
        }
        /// <summary>
        /// 初始化数组
        /// </summary>
        /// <param name="iTotalWidth"></param>
        /// <param name="dfChar"></param>
        /// <returns></returns>
        public static string[] BuildArrayString(int iTotalWidth, char dfChar = '0')
        {
            var s1 = string.Join(",", string.Empty.PadLeft(iTotalWidth, dfChar).ToCharArray()).Split(',');
            return Array.ConvertAll(s1, s => s.ToString());
        }

        public static float[] BuildArrayFloat(int iTotalWidth, char dftChar = '0')
        {
            var s1 = string.Join(",", string.Empty.PadLeft(iTotalWidth, dftChar).ToCharArray()).Split(',');
            return Array.ConvertAll(s1, s => float.Parse(s));
        }

        /// <summary>
        /// 数组初始化
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int[] InitArray(int[] array)
        {
            return BuildArray(array.Length);
        }

        /// <summary>
        /// 初始化一个数组列表
        /// </summary>
        /// <param name="objs"></param>
        public static void InitArrayList(List<int[]> objs)
        {
            objs.ForEach(m => m = BuildArray(m.Length));
        }

        private static string ExecuteAction(Action func, int tryCountMax, int currentTry = 0)
        {
            try
            {
                func();
                return null;
            }
            catch (Exception ex)
            {
                currentTry++;
                if (currentTry <= tryCountMax)
                {
                    Thread.Sleep(GlobalConfig.ExceptionTryInterval);
                    return ExecuteAction(func, tryCountMax, currentTry);
                }
                else return ex.ToString();
            }
        }
        private static ResultModel MapResult(ProcessingMessage res)
        {
            var model = new ResultModel();
            if (res == null) return model;

            model.IsSuccess = res.Success == "0";
            model.Message = res.ErrorMessage;
            model.StatusCode = res.ErrorCode != "200" ? 500 : 200;
            model.Data = res.ParamterObj;
            model.ErrorCode = res.ErrorCode;

            return model;
        }
        private static ResultModel ExecuteAction(Func<ProcessingMessage> func, int tryCountMax, int currentTry = 0)
        {
            try
            {
                var procRes = func();
                return MapResult(procRes);
            }
            catch (Exception ex)
            {
                currentTry++;
                if (currentTry <= tryCountMax)
                {
                    Thread.Sleep(GlobalConfig.ExceptionTryInterval);
                    return ExecuteAction(func, tryCountMax, currentTry);
                }
                else return new ResultModel
                {
                    IsSuccess = false,
                    Message = ex.ToString(),
                    StatusCode = -100 // func执行异常，赋值：-100
                };
            }
        }
        public static string ExecuteFunc(Action func, int maxTry = 3)
        {
            var msg = ExecuteAction(func, maxTry);
            var prevMsg = maxTry > 0 ? $" 发生异常，已重试：{maxTry}次，" : "发生异常：";
            if (!string.IsNullOrEmpty(msg)) msg = prevMsg + msg;
            return msg;
        }

        /// <summary>
        /// 捕捉异常
        /// </summary>
        /// <param name="func"></param>
        /// <param name="onError"></param>
        /// <param name="maxErrorCount"></param>
        /// <returns></returns>
        public static Task<bool> ExecuteFuncAsync(Action func, Action<string> onError, int maxErrorCount = 3)
        {
            maxErrorCount = GlobalConfig.ErrorRetryMaxCount;

            return Task.Run(() =>
            {
                var msg = ExecuteAction(func, maxErrorCount);
                var prevMsg = maxErrorCount > 0 ? $"发生异常，已重试：{maxErrorCount}次，" : "发生异常：";
                if (msg != null)
                    onError?.Invoke(prevMsg + msg);
                return msg == null;
            });
        }

        public static bool ExecuteFunc(Action func, Action<string> onError, int maxErrorCount = 3)
        {
            maxErrorCount = GlobalConfig.ErrorRetryMaxCount;

            var msg = ExecuteAction(func, maxErrorCount);
            var prevMsg = maxErrorCount > 0 ? $"发生异常，已重试：{maxErrorCount}次，" : "发生异常：";
            if (msg != null)
                onError?.Invoke(prevMsg + msg);
            return msg == null;
        }

        public static Task<ResultModel> ExecuteFuncAsync(Func<ProcessingMessage> func, Action<string> onError, int maxErrorCount = 3)
        {
            maxErrorCount = GlobalConfig.ErrorRetryMaxCount;
            return Task.Run(() =>
            {
                var result = ExecuteAction(func, maxErrorCount);
                var prevMsg = maxErrorCount > 0 ? $"发生异常，已重试：{maxErrorCount}次，" : "发生异常：";
                if (result.StatusCode == -100)
                    onError?.Invoke(prevMsg + result.Message);

                return result;
            });
        }

        public static ResultModel ExecuteFunc(Func<ProcessingMessage> func, Action<string> onError, int maxErrorCount = 3)
        {
            maxErrorCount = GlobalConfig.ErrorRetryMaxCount;
            var result = ExecuteAction(func, maxErrorCount);
            var prevMsg = maxErrorCount > 0 ? $"发生异常，已重试：{maxErrorCount}次，" : "发生异常：";
            if (result.StatusCode == -100)
                onError?.Invoke(prevMsg + result.Message);

            return result;

        }

        /// <summary>
        /// 将秒转换成时间格式
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string SecondToHMS(int second)
        {
            int hrs = 0, mm = 0, ss = 0;
            if (second < 60)
            {
                ss = second;
            }
            else
            {
                ss = second % 60;
                mm = second / 60;
                if (mm > 60)
                {
                    hrs = mm / 60;
                    mm = mm % 60;
                }
            }
            string hms = $"{ReformatNumber(hrs)}:{ReformatNumber(mm)}:{ReformatNumber(ss)}";
            return hms;
        }

        private static string ReformatNumber(int inputNumber)
        {
            if (inputNumber > 9)
            {
                return inputNumber.ToString();
            }
            else
            {
                return $"0{inputNumber.ToString()}";
            }
        }

        public static int ExportToCSV<T>(string Path, List<T> ListSource)
        {
            int result = 0;
            try
            {
                string Title = "";
                StringBuilder strVal = new StringBuilder();
                DataTable dtSource = ListToDataTable<T>(ListSource, ref Title);
                string[] titles = Title.Substring(0, Title.Length - 1).Split(',');
                result = DataTablesToExcel(dtSource, true, Path);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public static int ExportToXlsx<T>(string path, List<T> listData)
        {
            int result = 0;
            StringBuilder strVal = new StringBuilder();
            DataTable dtSource = ListToDataTable<T>(listData);
            result = DataTablesToExcel(dtSource, true, path);
            return result;
        }



        #region list转DataTable
        /// <summary>
        /// list转DataTable
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="entitys">泛型集合</param>
        /// <param name="IsToCHS">是否将字段转换为汉语（true：转换，false:不转换）</param>
        /// <returns>转化后的DataTable</returns>
        public static DataTable ListToDataTable<T>(List<T> entitys, ref string Title)
        {

            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable("dt");
            try
            {
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                    dt.Columns.Add(entityProperties[i].Name);
                    Title = Title + entityProperties[i].Name + ",";
                }

                //将所有entity添加到DataTable中
                foreach (object entity in entitys)
                {
                    //检查所有的的实体都为同一类型
                    if (entity.GetType() != entityType)
                    {
                        throw new Exception("要转换的集合元素类型不一致");
                    }
                    object[] entityValues = new object[entityProperties.Length];
                    for (int i = 0; i < entityProperties.Length; i++)
                    {
                        entityValues[i] = entityProperties[i].GetValue(entity, null);

                    }
                    dt.Rows.Add(entityValues);
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            DataTable dt = new DataTable("dt");
            try
            {
                //将所有entity添加到DataTable中
                foreach (object entity in entitys)
                {
                    dt.Rows.Add(entity);
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }
        #endregion
        public static void WriteToCsv(string path, string fileName, string title, string content)
        {
            try
            {
                DirectoryInfo filepath = new DirectoryInfo(path);
                if (!filepath.Exists)
                {
                    filepath.Create();
                }
                fileName = filepath + fileName;
                var fmode = !System.IO.File.Exists(fileName) ? FileMode.Create : FileMode.Append;
                using (var fs1 = new FileStream(fileName, fmode, FileAccess.Write, FileShare.ReadWrite))
                {
                    var sw = new StreamWriter(fs1, Encoding.UTF8);
                    if (fmode == FileMode.Create)
                    {
                        sw.WriteLine(title);
                    }
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 将DataTables数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public static int DataTablesToExcel(DataTable dataTable, bool isColumnWritten, string fileName)
        {
            int count = 0;
            int count_all = 0;
            ISheet[] sheets = new ISheet[1];
            string sheetName = "sheet1";
            IWorkbook workbook = null;
            DirectoryInfo filepath = new DirectoryInfo(fileName.Substring(0, fileName.LastIndexOf('\\')));
            if (!filepath.Exists)
            {
                filepath.Create();
            }
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //if (fileName.IndexOf(".xlsx") > 0) // 2003版本
                //    workbook = new XSSFWorkbook();
                if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook();

                try
                {
                    count = 0;

                    if (workbook != null)
                    {
                        sheets[0] = workbook.CreateSheet(sheetName);
                    }
                    else
                    {
                        return -1;
                    }

                    if (isColumnWritten) //写入DataTable的列名
                    {
                        IRow row = sheets[0].CreateRow(0);
                        for (int j = 0; j < dataTable.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(dataTable.Columns[j].ColumnName);
                        }
                        count = 1;
                    }
                    else
                    {
                        count = 0;
                    }

                    for (int i = 0; i < dataTable.Rows.Count; ++i)
                    {
                        IRow row = sheets[0].CreateRow(count);
                        for (int j = 0; j < dataTable.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(dataTable.Rows[i][j].ToString());
                        }
                        ++count;
                    }
                    if (isColumnWritten) //写入DataTable的列名
                        count_all = count - 1;
                    else
                        count_all = count;
                    workbook.Write(fs); //写入到excel
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: ", ex);
                    return -1;
                }
            }
            return count_all;
        }

        public static void StartThread(Action act, bool IsBackground = true)
        {
            var thread = new Thread(new ThreadStart(act));
            thread.IsBackground = IsBackground;
            thread.Start();
        }
        public static void StartThread(Action<object> act, object parm, bool IsBackground = true)
        {
            var thread = new Thread(x => act(x));
            thread.IsBackground = IsBackground;
            thread.Start(parm);
        }
    }
}
