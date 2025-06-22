using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himma.Common.Event
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.	
    /// 描述：
    /// 创建人： Himma
    /// 创建时间：2020/6/15 22:20:25
    /// </summary>
    /// <summary>
    /// 数组变量控制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassArrayEvent<T>
    {
        /// <summary>
        /// 存储最新值
        /// </summary>
        private T[] myValue;

        /// <summary>
        /// 存储最后变化时间
        /// </summary>
        private DateTime _lastUpdateTime;
        public DateTime LastUpdateTime { get => _lastUpdateTime; }
        /// <summary>
        /// 存储最后变化至现在的时间间隔
        /// </summary>
        public int TimeInterval => GetTimeInterval();

        public delegate void MyValueChanged(object sender, ArrayArgs<T> e);

        public event MyValueChanged OnMyValueChanged;

        public T[] MyValue
        {
            get { return myValue; }
            set
            {
                if (myValue is null)
                {
                    myValue = new T[value.Length];
                    value.CopyTo(myValue, 0);
                }
                var ar = Compare(value, myValue);
                if (ar != null && ar.Count > 0)
                {
                    value.CopyTo(myValue, 0);
                    WhenMyValueChange(ar);
                }

            }
        }
        /// <summary>
        /// 对比数据是否改变
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private List<DiffItem<T>> Compare(T[] x, T[] y)
        {
            var r = x.Equals(y);
            if (r)
                return null;
            List<DiffItem<T>> l = new List<DiffItem<T>>();
            for (int i = 0; i < myValue.Length; i++)
            {
                if (!x[i].Equals(y[i]))
                {
                    l.Add(new DiffItem<T>(i, x[i], y[i]));
                }
            }
            return l;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClassArrayEvent()
        {
            _lastUpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 更改之后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfterMyValueChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 改变时调用
        /// </summary>
        private void WhenMyValueChange(List<DiffItem<T>> ar)
        {
            _lastUpdateTime = DateTime.Now;
            OnMyValueChanged?.Invoke(this, new ArrayArgs<T>(ar));
        }
        /// <summary>
        /// 获取时间间隔
        /// </summary>
        /// <returns></returns>
        private int GetTimeInterval()
        {
            return DateTime.Now.Subtract(_lastUpdateTime).Seconds;
        }
    }
    /// <summary>
    /// 参数类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayArgs<T> : EventArgs
    {
        public List<DiffItem<T>> DiffList = new List<DiffItem<T>>();
        public ArrayArgs(List<DiffItem<T>> diffList)
        {
            DiffList = diffList;
        }
    }
    /// <summary>
    /// 差异项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DiffItem<T>
    {
        public int ID;
        public T SourceValue;
        public T DistinctValue;
        public DiffItem(int id, T sourceValue, T distinctValue)
        {
            ID = id;
            SourceValue = sourceValue;
            DistinctValue = distinctValue;
        }
    }
}
